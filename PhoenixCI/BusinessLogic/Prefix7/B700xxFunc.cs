using BaseGround;
using BaseGround.Shared;
using Common;
using Common.Helper;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PhoenixCI.BusinessLogic.Prefix7
{
   public class B700xxFunc
   {
      /// <summary>
      /// 存Csv
      /// </summary>
      /// <param name="filePath">存檔路徑</param>
      /// <param name="dataTable">要輸出的資料</param>
      private void SaveExcel(string filePath, DataTable dataTable, bool addHeader = true, int firstRowIndex = 0, int firstColIndex = 0)
      {
         try {
            Workbook wb = new Workbook();
            wb.Options.Export.Csv.WritePreamble = true;
            wb.Worksheets[0].Import(dataTable, addHeader, firstRowIndex, firstColIndex);
            wb.Worksheets[0].Name = SheetName(filePath);
            //存檔
            wb.SaveDocument(filePath, DocumentFormat.Csv);
         }
         catch (Exception ex) {
            throw ex;
         }
      }

      /// <summary>
      /// 用檔案名稱命名sheet 且不超過31個字元
      /// </summary>
      /// <param name="filePath"></param>
      /// <returns></returns>
      private string SheetName(string filePath)
      {
         string filename = Path.GetFileNameWithoutExtension(filePath);
         int nameLen = filename.Length > 31 ? 31 : filename.Length;//sheetName不能超過31字
         return filename.Substring(0, nameLen);
      }

      /// <summary>
      /// 重複寫入文字並換行
      /// </summary>
      /// <param name="openData">檔案路徑</param>
      /// <param name="textToAdd">文字內容</param>
      private void WriteFile(string openData, string textToAdd)
      {
         using (FileStream fs = new FileStream(openData, FileMode.Append)) {
            using (StreamWriter writer = new StreamWriter(fs, Encoding.GetEncoding(950))) {
               writer.WriteLine(textToAdd);
            }
         }
      }

      /// <summary>
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)
      /// |
      /// 呼叫來源: 70010 (由業務單位手動產生) 10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="SaveFilePath">檔名</param>
      /// <param name="lsStartYMD">起始日期</param>
      /// <param name="lsEndYMD">終止日期</param>
      /// <param name="lsSumType">統計別:D日,M月,Y年</param>
      /// <param name="lsProdType">商品別:F期貨,O選擇權</param>
      /// <param name="lsMarketCode"></param>
      /// <param name="selectEng">選取英文版本</param>
      /// <returns>RETURN:false代表error/true代表成功</returns>
      public string F70010WeekByMarketCode
         (string SaveFilePath, string lsStartYMD, string lsEndYMD, string lsSumType, string lsProdType, string lsMarketCode, bool selectEng = false)
      {
         /********************************
         作業:轉70010 週檔 (公司網站\統計資料\週)
         呼叫來源: 70010 (由業務單位手動產生)
                   10012,10022 (由OP操作批次時自動產生)
         參數:(1)檔名
              (2)起始日期
	           (3)終止日期
	           (4)統計別:D日,M月,Y年
	           (5)商品別:F期貨,O選擇權
         RETURN:E代表error
                Y代表成功
         ********************************/
         string openData = "";

         //避免重複寫入
         if (File.Exists(SaveFilePath)) {
            File.Delete(SaveFilePath);
         }
         File.Create(SaveFilePath).Close();
         if (!selectEng) {
            openData = PbFunc.f_chg_filename(SaveFilePath, "_OpenData");
            if (!File.Exists(openData)) {
               File.Delete(openData);
            }
            File.Create(openData).Close();
            //WriteFile(openData, "期貨商代號,期貨商名稱,日期,商品,交易量");
         }

         D70010 dao70010 = new D70010();
         try {
            ///******************
            //讀取資料
            //******************/
            DataTable dt = dao70010.ListRowdata(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return $@"轉70010-交易量資料轉檔作業(週)({lsStartYMD }-{ lsEndYMD })(期貨/選擇權:{lsProdType })筆數為０!";
            }
            /* 期貨商 */
            DataTable dtBRK = dao70010.List70010brkByMarketCode(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode);
            /* 日期 */
            DataTable dtYMDd = dao70010.ListYmdByMarketCode(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode);
            DataTable dtYMD = dao70010.ListYmdEnd(null, null, null, null).Clone();
            //DataTable dtYMD = dao70010.ListYmdEnd(lsStartYMD, lsEndYMD, lsSumType, lsProdType);
            /* 商品 */
            DataTable dtPK;
            if (lsProdType == "F") {
               if (lsMarketCode == "1") {

                  dtPK = dao70010.ListParamKey("70011n");
               }
               else {
                  dtPK = dao70010.ListParamKey("70011");
               }
            }//if (lsProdType == "F")
            else {
               if (lsMarketCode == "1") {
                  dtPK = dao70010.ListParamKey("70012n");
               }
               else {
                  dtPK = dao70010.ListParamKey("70012");
               }
            }//if (lsProdType == "F")else
            DataTable newdtPK = dtPK.Filter($@"rpt_value_3 Is Null or TRIM(rpt_value_3) = '' or rpt_value_3 >= '{ lsStartYMD }'");
            string lsBrkNo, lsBrkName, lsBrkNo4, lsBrkType, lsYMD, lsParamKey, lsStr;
            DateTime ldYMD, ldYMDn;
            //日期:週
            try {
               DataRow toInsert = dtYMDd.NewRow();
               toInsert["am0_ymd"] = "20051231";
               dtYMDd.Rows.InsertAt(toInsert, 0);
               int dtymdCount = dtYMDd.Rows.Count - 1;
               for (int k = 1; k <= dtymdCount; k++) {
                  lsYMD = dtYMDd.Rows[k]["am0_ymd"].AsString();
                  ldYMD = lsYMD.AsDateTime("yyyyMMdd");
                  ldYMDn = dtYMDd.Rows[k - 1]["am0_ymd"].AsString().AsDateTime("yyyyMMdd");
                  /* 符合下列條件才寫Excel
                  1.最後一筆
                  2.換週 (判斷星期x是否變小)
                  3.與下一日期相差7天以上 (for日期99999999) 
                  */
                  if (k == 1 || ldYMD.DayOfWeek < ldYMDn.DayOfWeek || Math.Abs(PbFunc.DaysAfter(ldYMD, ldYMDn)) > 6) {
                     toInsert = dtYMD.NewRow();
                     toInsert["am0_ymd"] = lsYMD;
                     dtYMD.Rows.Add(toInsert);
                  }
                  dtYMD.Rows[dtYMD.Rows.Count - 1]["ymd_end"] = lsYMD;
               }//for (int k = 2; k <= dtymdCount; k++)
               toInsert = dtYMD.NewRow();
               toInsert["am0_ymd"] = "99999999";
               toInsert["ymd_end"] = "99999999";
               dtYMD.Rows.Add(toInsert);
               dtYMD.Sort("am0_ymd");
            }
            catch (Exception ex) {
               throw new Exception("日期:週-" + ex.Message);
            }

            //主要的資料
            DataTable workTable = new DataTable();
            //opendata DataTable
            DataTable opendataTable = new DataTable();
            opendataTable.Columns.Add("期貨商代號", typeof(string));
            opendataTable.Columns.Add("期貨商名稱", typeof(string));
            opendataTable.Columns.Add("日期", typeof(string));
            opendataTable.Columns.Add("商品", typeof(string));
            opendataTable.Columns.Add("交易量", typeof(string));
            /******************
            表頭
            ******************/
            lsStr = "";

            //StringBuilder lsOutput1 = new StringBuilder();
            //StringBuilder lsOutput2 = new StringBuilder();

            decimal ldSum = 0;
            try {
               //lsOutput1.Append(!selectEng ? "期貨商代號" + "," + "名稱" : " Sequential No.");
               DataRow headerRow = workTable.NewRow();
               if (!selectEng) {
                  workTable.Columns.Add("期貨商代號", typeof(string));
                  workTable.Columns.Add("名稱", typeof(string));
                  headerRow[0] = "Date";
                  headerRow[1] = "";
               }
               else {
                  workTable.Columns.Add("Sequential No.", typeof(string));
                  headerRow[0] = "Date";
               }

               int ParamKeyCount = 2;//在商品代號和名稱之後，從第三欄開始
               //lsOutput2.Append(!selectEng ? "Date" + "," : "Date");
               foreach (DataRow ymdRow in dtYMD.Rows) {
                  lsYMD = ymdRow["am0_ymd"].AsString();

                  if (lsYMD == "99999999") {
                     lsStr = !selectEng ? "總計" : "Year-To-Date Volume of";
                  }

                  string pkYMD = "";
                  foreach (DataRow pkRow in newdtPK.Rows) {
                     lsParamKey = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     lsParamKey = pkRow["rpt_value_2"].AsString();
                     //lsOutput1.Append("," + lsStr + lsParamKey);
                     workTable.Columns.Add(lsStr + lsParamKey, typeof(string));
                     if (pkYMD == lsYMD) {
                        headerRow[ParamKeyCount++] = "";
                     }
                     else {
                        pkYMD = lsYMD;
                        headerRow[ParamKeyCount++] = lsYMD + " - " + ymdRow["ymd_end"].AsString();
                     }

                  }//foreach(DataRow pkRow in newdtPK.Rows)

                  //lsOutput2.Append("," + lsYMD + " - " + ymdRow["ymd_end"].AsString() + ",".PadRight(newdtPK.Rows.Count, ','));


                  if (lsYMD == "99999999") {
                     //lsOutput1.Append("," + (!selectEng ? lsStr : "Year-To-Date Market Volume"));
                     workTable.Columns.Add(!selectEng ? lsStr : "Year-To-Date Market Volume", typeof(string));
                  }
                  else {
                     //lsOutput1.Append("," + (!selectEng ? "小計" : "Subtotal"));
                     workTable.Columns.Add(!selectEng ? "小計" : "Subtotal", typeof(string));
                  }
               }//foreach (DataRow ymdRow in dtYMD.Rows)
               //lsOutput1.Append("," + (!selectEng ? "市佔率" : "YTD Market shares(%)"));
               workTable.Columns.Add(!selectEng ? "市佔率" : "YTD Market shares(%)", typeof(string));
               //新增兩列表頭
               workTable.Rows.Add(headerRow);
            }
            catch (Exception ex) {
               throw new Exception("表頭:" + ex.Message);
            }
            /******************
            內容
            ******************/
            DataRow newRow = dt.NewRow();
            newRow["am0_ymd"] = "20060101";
            dt.Rows.Add(newRow);

            DataRow contentRow = workTable.NewRow();
            /*******************
            OpenData
            *******************/
            //期貨商代號&名稱
            ABRK daoABRK = new ABRK();
            try {
               foreach (DataRow brkRow in dtBRK.Rows) {
                  lsBrkNo4 = brkRow["am0_brk_no4"].AsString();
                  lsBrkType = brkRow["am0_brk_type"].AsString();
                  if (lsBrkType.Trim() == "9") {
                     lsBrkNo = lsBrkNo4.Trim() + "999";
                  }
                  else {
                     lsBrkNo = lsBrkNo4.Trim() + "000";
                  }
                  lsBrkName = daoABRK.GetNameByNo(lsBrkNo);// f_get_abrk_name(lsBrkNo,'0')	
                  //lsOutput1 = new StringBuilder(!selectEng ? (lsBrkNo + "," + lsBrkName) : lsBrkNo);//轉英文只秀編號
                  if (!selectEng) {
                     contentRow[0] = lsBrkNo;
                     contentRow[1] = lsBrkName;
                  }
                  else {
                     contentRow[0] = lsBrkNo;
                  }
                  //日期
                  int colIndex = 0;
                  foreach (DataRow ymdRow in dtYMD.Rows) {
                     lsYMD = ymdRow["am0_ymd"].AsString();
                     DataTable newdt = dt.Filter($@"am0_brk_no4='{ lsBrkNo4 }' and am0_brk_type='{ lsBrkType }' and am0_ymd>='{ ymdRow["am0_ymd"].AsString() }' and am0_ymd<='{ ymdRow["ymd_end"].AsString() }'");
                     for (int k = 0; k <= newdt.Rows.Count - 1; k++) {
                        int foundIndex = newdtPK.Rows.IndexOf(newdtPK.Select($@"am0_param_key ='{ newdt.Rows[k]["am0_param_key"] }'").FirstOrDefault());
                        if (foundIndex > -1) {
                           decimal ll_qnty = newdtPK.Rows[foundIndex]["qnty"].AsDecimal() + newdt.Rows[k]["qnty"].AsDecimal();
                           newdtPK.Rows[foundIndex]["qnty"] = ll_qnty;
                        }
                     }

                     if (newdtPK.Rows.Count > 1)
                        ldSum = newdtPK.Compute("sum(qnty)", "").AsDecimal();//ldSum = newdtPK.getitemdecimal(1,"cp_sum_qnty")

                     //string lsOpenDataStr = lsBrkNo + "," + lsBrkName + "," + ymdRow["am0_ymd"].AsString() + " - " + ymdRow["ymd_end"].AsString();
                     //商品
                     foreach (DataRow pkRow in newdtPK.Rows) {
                        //lsOutput1.Append("," + pkRow["qnty"]);
                        contentRow[colIndex++] = pkRow["qnty"].AsString();
                        if (lsYMD.SubStr(0, 8) != "99999999" && !selectEng) {
                           //string writeOpenData = lsOpenDataStr + "," + pkRow["am0_param_key"].AsString() + "," + pkRow["qnty"].AsString();
                           opendataTable.Rows.Add(new object[] { lsBrkNo, lsBrkName, ymdRow["am0_ymd"].AsString() + " - " + ymdRow["ymd_end"].AsString(), pkRow["am0_param_key"].AsString(), pkRow["qnty"].AsString() });
                        }
                        pkRow["qnty"] = 0;
                     }//foreach(DataRow pkRow in newdtPK.Rows)
                     //lsOutput1.Append("," + ldSum.AsString());
                     contentRow[colIndex++] = ldSum.AsString();
                  }//foreach (DataRow ymdRow in dtYMD.Rows)
                  //lsOutput1.Append($",{brkRow["cp_rate"].AsDecimal().ToString("n")}");
                  contentRow[colIndex++] = brkRow["cp_rate"].AsDecimal().ToString("n");

                  workTable.Rows.Add(contentRow);
               }//foreach (DataRow brkRow in dtBRK.Rows)

               //存檔
               SaveExcel(SaveFilePath, workTable);
               SaveExcel(openData, opendataTable);
            }
            catch (Exception ex) {
               throw new Exception("內容:" + ex.Message);
            }
         }
         catch (Exception ex) {
            throw ex;
         }
         return MessageDisplay.MSG_OK;
      }



      /// <summary>
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)英文版
      /// |
      /// 呼叫來源: 70010 (由業務單位手動產生) 10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="SaveFilePath">檔名</param>
      /// <param name="lsStartYMD">起始日期</param>
      /// <param name="lsEndYMD">終止日期</param>
      /// <param name="lsSumType">統計別:D日,M月,Y年</param>
      /// <param name="lsProdType">商品別:F期貨,O選擇權</param>
      /// <param name="lsMarketCode"></param>
      /// <param name="selectEng">選取英文版本</param>
      /// <returns>RETURN:false代表error/true代表成功</returns>
      public string F70010WeekEngByMarketCode(string SaveFilePath, string lsStartYMD, string lsEndYMD, string lsSumType, string lsProdType, string lsMarketCode)
      {
         /********************************
         作業:轉70010 週檔 (公司網站\統計資料\週)
         呼叫來源: 70010 (由業務單位手動產生)
                   10012,10022 (由OP操作批次時自動產生)
         參數:(1)檔名
              (2)起始日期
	           (3)終止日期
	           (4)統計別:D日,M月,Y年
	           (5)商品別:F期貨,O選擇權
         RETURN:E代表error
                Y代表成功
         ********************************/
         return F70010WeekByMarketCode(SaveFilePath, lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode, true);
      }

      /// <summary>
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)
      /// |
      /// 呼叫來源: 11010
      /// </summary>
      /// <param name="SaveFilePath"></param>
      /// <param name="lsStartYMD"></param>
      /// <param name="lsEndYMD"></param>
      /// <param name="lsSumType"></param>
      /// <param name="lsProdType"></param>
      /// <returns></returns>
      //public string F70010WeekHis(string SaveFilePath, string lsStartYMD, string lsEndYMD, string lsSumType, string lsProdType)
      //{
      //   return "";
      //}

      /// <summary>
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)
      /// |
      /// 呼叫來源:70010 (由業務單位手動產生)10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="SaveFilePath">檔名</param>
      /// <param name="lsStartYMD">起始日期</param>
      /// <param name="lsEndYMD">終止日期</param>
      /// <param name="lsSumType">統計別:D日,M月,Y年</param>
      /// <param name="lsKindId2">商品別:F期貨,O選擇權</param>
      /// <param name="lsParamKey">商品</param>
      /// <param name="lsProdType"></param>
      /// <returns></returns>
      public string F70010WeekW(string SaveFilePath, string lsStartYMD, string lsEndYMD, string lsSumType, string lsKindId2, string lsParamKey, string lsProdType)
      {
         /********************************
         作業:轉70010 週檔 (公司網站\統計資料\週)
         呼叫來源: 70010 (由業務單位手動產生)
                   10012,10022 (由OP操作批次時自動產生)
         參數:(1)檔名
              (2)起始日期
	           (3)終止日期
	           (4)統計別:D日,M月,Y年
	           (5)商品別:F期貨,O選擇權
         RETURN:E代表error
                Y代表成功
         ********************************/

         D70050 dao70050 = new D70050();
         D70010 dao70010 = new D70010();
         ///******************
         //讀取資料
         //******************/
         DataTable dt = dao70050.ListAll(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsKindId2, lsParamKey);
         if (dt.Rows.Count <= 0) {
            return $@"轉{lsParamKey}-交易量資料轉檔作業(週)({lsStartYMD}-{lsEndYMD})(期貨/選擇權:{ lsProdType })筆數為０!";
         }

         //避免重複寫入
         if (File.Exists(SaveFilePath)) {
            File.Delete(SaveFilePath);
         }
         File.Create(SaveFilePath).Close();

         try {

            /* 期貨商 */
            DataTable dtBRK;
            dtBRK = dao70050.List70050brk(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsKindId2, lsParamKey);

            /* 日期 */
            DataTable dtYMDd = dao70010.ListYMD(lsStartYMD, lsEndYMD, lsSumType, lsProdType);
            //DataTable dtYMD = dao70010.ListYmdEnd(lsStartYMD, lsEndYMD, lsSumType, lsProdType);
            DataTable dtYMD = dao70010.ListYmdEnd(null, null, null, null);
            dtYMD.Clear();//PB還沒有在這retrieve
            /* 商品 */
            DataTable dtPK;
            if (lsParamKey == "TXO") {
               dtPK = dao70010.ListParamKey("70040");
            }//if (isParamKey == "TXO")
            else {
               dtPK = dao70010.ListParamKey("70050");
            }
            DataTable newdtPK = dtPK.Filter($@"am0_param_key like '{ lsKindId2 }'");
            string lsBrkNo, lsBrkNo4, lsBrkType, lsYMD, lsStr;
            DateTime ldYMD, ldYMDn;
            //日期:週
            try {
               DataRow toInsert = dtYMDd.NewRow();
               toInsert["am0_ymd"] = "20051231";
               dtYMDd.Rows.InsertAt(toInsert, 0);
               int dtymdCount = dtYMDd.Rows.Count - 1;
               for (int k = 1; k <= dtymdCount; k++) {
                  lsYMD = dtYMDd.Rows[k]["am0_ymd"].AsString();
                  ldYMD = DateTime.ParseExact(lsYMD, "yyyyMMdd", CultureInfo.CurrentCulture);
                  ldYMDn = DateTime.ParseExact(dtYMDd.Rows[k - 1]["am0_ymd"].AsString(), "yyyyMMdd", CultureInfo.CurrentCulture);
                  /* 符合下列條件才寫Excel
                  1.最後一筆
                  2.換週 (判斷星期x是否變小)
                  3.與下一日期相差7天以上 (for日期99999999) 
                  */
                  if (k == 1 || ldYMD.DayOfWeek < ldYMDn.DayOfWeek || Math.Abs(PbFunc.DaysAfter(ldYMD, ldYMDn)) > 6) {
                     toInsert = dtYMD.NewRow();
                     toInsert["am0_ymd"] = lsYMD;
                     dtYMD.Rows.Add(toInsert);
                  }
                  dtYMD.Rows[dtYMD.Rows.Count - 1]["ymd_end"] = lsYMD;
               }//for (int k = 2; k <= dtymdCount; k++)
               toInsert = dtYMD.NewRow();
               toInsert["am0_ymd"] = "99999999";
               toInsert["ymd_end"] = "99999999";
               dtYMD.Rows.Add(toInsert);
               DataView dsDv = dtYMD.AsDataView();
               dsDv.Sort = "am0_ymd";
               dtYMD = dsDv.ToTable();
            }
            catch (Exception ex) {
               throw new Exception("日期:週-" + ex.Message);
            }

            //主要的資料
            DataTable workTable = new DataTable();

            /******************
            表頭
            ******************/
            lsStr = "";
            decimal ldSum = 0;
            DataTable newDsYMD = dtYMD;
            DataRow headerRow = workTable.NewRow();
            workTable.Columns.Add("期貨商代號", typeof(string));
            workTable.Columns.Add("名稱", typeof(string));
            headerRow[0] = "Date";
            headerRow[1] = "";

            //StringBuilder lsOutput1 = new StringBuilder("期貨商代號" + "," + "名稱");
            //StringBuilder lsOutput2 = new StringBuilder("Date" + ",");

            try {
               int ParamKeyCount = 1;
               foreach (DataRow ymdRow in dtYMD.Rows) {
                  lsYMD = ymdRow["am0_ymd"].AsString();

                  if (lsYMD == "99999999") {
                     lsStr = "總計";
                  }
                  string pkYMD = "";
                  foreach (DataRow pkRow in newdtPK.Rows) {
                     lsParamKey = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     lsParamKey = pkRow["rpt_value_2"].AsString();
                     workTable.Columns.Add(lsYMD + " - " + ymdRow["ymd_end"].AsString(), typeof(string));
                     //lsOutput1.Append("," + lsStr + lsParamKey);
                     workTable.Columns.Add(lsStr + lsParamKey, typeof(string));
                     if (pkYMD == lsYMD && lsKindId2 == "%") {
                        headerRow[ParamKeyCount++] = "";
                     }
                     else {
                        pkYMD = lsYMD;
                        headerRow[ParamKeyCount++] = lsYMD + " - " + ymdRow["ymd_end"].AsString();
                     }
                  }//foreach(DataRow pkRow in newdtPK.Rows)

                  //lsOutput2.Append("," + lsYMD + " - " + ymdRow["ymd_end"].AsString());
                  //if (lsKindId2 == "%") {
                  //   lsOutput2.Append(",".PadRight(newdtPK.Rows.Count, ','));
                  //}

                  if (lsKindId2 == "%") {
                     if (lsYMD == "99999999") {
                        //lsOutput1.Append("," + lsStr);
                        workTable.Columns.Add(lsStr, typeof(string));
                     }
                     else {
                        //lsOutput1.Append("," + "小計");
                        workTable.Columns.Add("小計", typeof(string));
                     }
                  }
               }//foreach (DataRow ymdRow in dtYMD.Rows)
               //lsOutput1.Append("," + "市佔率");
               headerRow[ParamKeyCount++] = "市佔率";

               //新增兩列表頭
               workTable.Rows.Add(headerRow);
            }
            catch (Exception ex) {
               throw new Exception("表頭:" + ex.Message);
            }
            /******************
                  內容
            ******************/
            DataRow newRow = dt.NewRow();
            newRow["am0_ymd"] = "20060101";
            dt.Rows.Add(newRow);
            //期貨商代號&名稱
            DataRow contentRow = workTable.NewRow();
            try {
               foreach (DataRow brkRow in dtBRK.Rows) {
                  lsBrkNo4 = brkRow["am0_brk_no4"].AsString();
                  lsBrkType = brkRow["am0_brk_type"].AsString();
                  if (lsBrkType.Trim() == "9") {
                     lsBrkNo = lsBrkNo4.Trim() + "999";
                  }
                  else {
                     lsBrkNo = lsBrkNo4.Trim() + "000";
                  }
                  string lsBrkName = new ABRK().GetNameByNo(lsBrkNo);// f_get_abrk_name(lsBrkNo,'0')	
                  //lsOutput1 = new StringBuilder(lsBrkNo + "," + lsBrkName);
                  contentRow[0] = lsBrkNo;
                  contentRow[1] = lsBrkName;

                  //日期
                  int colIndex = 0;
                  foreach (DataRow ymdRow in dtYMD.Rows) {
                     //將週日期區間數量相加
                     DataTable newDt = dt.Filter($@"am0_brk_no4='{ lsBrkNo4 }' and am0_brk_type='{ lsBrkType }' and am0_ymd>='{ ymdRow["am0_ymd"].AsString() }' and am0_ymd<='{ ymdRow["ymd_end"].AsString() }'");
                     for (int k = 0; k <= newDt.Rows.Count - 1; k++) {
                        int foundIndex = newdtPK.Rows.IndexOf(newdtPK.Select($@"am0_param_key ='{ newDt.Rows[k]["am0_param_key"] }'").FirstOrDefault());
                        if (foundIndex > -1) {
                           decimal ll_qnty = newdtPK.Rows[foundIndex]["qnty"].AsDecimal() + newDt.Rows[k]["qnty"].AsDecimal();
                           newdtPK.Rows[foundIndex]["qnty"] = ll_qnty;
                        }
                     }
                     if (newdtPK.Rows.Count > 1)
                        ldSum = newdtPK.Compute("sum(qnty)", "").AsDecimal();//ldSum = newdtPK.getitemdecimal(1,"cp_sum_qnty")

                     //商品
                     foreach (DataRow pkRow in newdtPK.Rows) {
                        contentRow[colIndex++] = pkRow["qnty"].AsString();
                        pkRow["qnty"] = 0;
                     }//foreach(DataRow pkRow in newdtPK.Rows)

                     if (lsKindId2 == "%") {
                        //lsOutput1.Append(lsOutput1 + "," + ldSum.AsString());
                        contentRow[colIndex++] = ldSum.AsString();
                     }

                  }//foreach (DataRow ymdRow in dtYMD.Rows)
                  //lsOutput1.Append($",{brkRow["cp_rate"].AsDecimal().ToString("n")}");
                  contentRow[colIndex++] = brkRow["cp_rate"].AsDecimal().ToString("n");
                  //新增一行
                  workTable.Rows.Add(contentRow);
               }//foreach (DataRow brkRow in dtBRK.Rows)

               //存檔
               SaveExcel(SaveFilePath, workTable);
            }
            catch (Exception ex) {
               throw new Exception("內容:" + ex.Message);
            }
            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
            throw new Exception("f_70010_week_w:" + ex.Message);
         }
      }

      /// <summary>
      /// 作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)
      /// |
      /// 呼叫來源: 70010 (由業務單位手動產生)10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="SaveFilePath">檔名</param>
      /// <param name="lsStartYMD">起始日期</param>
      /// <param name="lsEndYMD">終止日期</param>
      /// <param name="lsSumType">統計別:D日,M月,Y年</param>
      /// <param name="lsProdType">商品別:F期貨,O選擇權</param>
      /// <param name="lsMarketCode"></param>
      /// <param name="selectEng">選取英文版本</param>
      /// <returns>RETURN:false代表error/true代表成功</returns>
      public string F70010YmdByMarketCode(string SaveFilePath, string lsStartYMD, string lsEndYMD, string lsSumType, string lsProdType, string lsMarketCode, bool selectEng = false)
      {
         /********************************
         作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)
         呼叫來源: 70010 (由業務單位手動產生)
                   10012,10022 (由OP操作批次時自動產生)
         參數:(1)檔名
              (2)起始日期
	           (3)終止日期
	           (4)統計別:D日,M月,Y年
	           (5)商品別:F期貨,O選擇權
         RETURN:E代表error
                Y代表成功
         ********************************/

         D70010 dao70010 = new D70010();
         //避免重複寫入
         if (File.Exists(SaveFilePath)) {
            File.Delete(SaveFilePath);
         }
         File.Create(SaveFilePath).Close();

         try {
            ///******************
            //讀取資料
            //******************/
            DataTable dt = dao70010.ListRowdata(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return $@"轉70010-交易量資料轉檔作業({lsSumType})({lsStartYMD}-{lsEndYMD})(期貨/選擇權:{ lsProdType })筆數為０!";
            }

            string openData = "";

            if (!selectEng) {
               openData = PbFunc.f_chg_filename(SaveFilePath, "_OpenData");
               if (!File.Exists(openData)) {
                  File.Delete(openData);
               }
               File.Create(openData).Close();
               //WriteFile(openData, "期貨商代號,期貨商名稱,日期,商品,交易量");
            }

            /* 期貨商 */
            DataTable dtBRK;
            //年度依當年度排序
            if (lsSumType != "Y") {
               dtBRK = dao70010.List70010brkByMarketCode(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode);
            }
            else {
               dtBRK = dao70010.List70010brkYearByMarketCode(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode);
            }

            /* 日期 */
            DataTable dtYMD = dao70010.ListYmdByMarketCode(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode);
            //年度不需要總計
            if (lsSumType != "Y") {
               DataRow newymdRow = dtYMD.NewRow();
               newymdRow["am0_ymd"] = "99999999";
               dtYMD.Rows.Add(newymdRow);
            }
            /* 商品 */
            DataTable dtPK;
            if (lsProdType == "F") {
               if (lsMarketCode == "1") {

                  dtPK = dao70010.ListParamKey("70011n");
               }
               else {
                  dtPK = dao70010.ListParamKey("70011");
               }
            }//if (lsProdType == "F")
            else {
               if (lsMarketCode == "1") {
                  dtPK = dao70010.ListParamKey("70012n");
               }
               else {
                  dtPK = dao70010.ListParamKey("70012");
               }
            }//if (lsProdType == "F")else
            var dtPKquery = from datatable in dtPK.AsEnumerable() select datatable;
            switch (lsSumType) {
               case "D":
                  //newdtPK = dtPK.Filter($@"rpt_value_3 Is Null or Trim( rpt_value_3) = '' or rpt_value_3 >= '{ lsStartYMD }'");
                  dtPKquery = dtPKquery.Where(datatable => string.IsNullOrEmpty(datatable.Field<string>("rpt_value_3").AsString()) || datatable.Field<string>("rpt_value_3") == lsStartYMD);
                  break;
               case "M":
                  //newdtPK = dtPK.Filter($@"rpt_value_3 Is Null or Trim( rpt_value_3) = '' or Substring(rpt_value_3,0,5) >= '{ lsStartYMD }'");
                  dtPKquery = dtPKquery.Where(datatable => string.IsNullOrEmpty(datatable.Field<string>("rpt_value_3").AsString()) || datatable.Field<string>("rpt_value_3").SubStr(0, 5) == lsStartYMD);
                  break;
               case "Y":
                  //newdtPK = dtPK.Filter($@"rpt_value_3 Is Null or Trim( rpt_value_3) = '' or Substring(rpt_value_3,0,3) >= '{ lsStartYMD } '");
                  dtPKquery = dtPKquery.Where(datatable => string.IsNullOrEmpty(datatable.Field<string>("rpt_value_3").AsString()) || datatable.Field<string>("rpt_value_3").SubStr(0, 3) == lsStartYMD);
                  break;
               default:
                  break;
            }
            DataTable newdtPK = dtPKquery.CopyToDataTable();
            int liArea;
            string lsBrkNo, lsBrkNo4, lsBrkType, lsYMD, lsParamKey, lsStr, lsBrkName;
            /***************************
            因Excel column數限制,
            若"期貨"選擇日期迄超過15,則分成2區
            ***************************/
            if (PbFunc.Right(lsEndYMD, 2).AsInt() > 15 && lsSumType == "D" && lsProdType == "F") {
               liArea = 2;
            }
            else {
               liArea = 1;
            }
            /******************
            表頭
            ******************/
            lsStr = "";


            //主要的資料
            DataTable workTable = new DataTable();
            //opendata DataTable
            DataTable opendataTable = new DataTable();
            opendataTable.Columns.Add("期貨商代號", typeof(string));
            opendataTable.Columns.Add("期貨商名稱", typeof(string));
            opendataTable.Columns.Add("日期", typeof(string));
            opendataTable.Columns.Add("商品", typeof(string));
            opendataTable.Columns.Add("交易量", typeof(string));

            //StringBuilder lsOutput1 = new StringBuilder();
            //StringBuilder lsOutput2 = new StringBuilder();

            decimal ldSum, ldVal;
            DataTable newDsYMD = dtYMD;
            DataTable newDt = dt;

            int useCpuCount = Environment.ProcessorCount > 3 ? 2 : 1;//限制cpu使用數目
            for (int liAreaCnt = 1; liAreaCnt <= liArea; liAreaCnt++) {
               try {
                  if (liArea == 2) {
                     if (liAreaCnt == 1) {
                        newDsYMD = dtYMD.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount)
                           .Where(r => r.Field<object>("am0_ymd").AsString().SubStr(6, 2).AsInt() <= 15).CopyToDataTable();

                        newDt = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount)
                           .Where(r => r.Field<object>("am0_ymd").AsString().SubStr(6, 2).AsInt() <= 15).CopyToDataTable();

                     }
                     else {
                        newDsYMD = dtYMD.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount)
                           .Where(r => r.Field<object>("am0_ymd").AsString().SubStr(6, 2).AsInt() > 15).CopyToDataTable();

                        newDt = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount)
                           .Where(r => r.Field<object>("am0_ymd").AsString().SubStr(6, 2).AsInt() > 15).CopyToDataTable();

                        for (int k = 1; k <= 8; k++) {
                           //WriteFile(SaveFilePath, "");
                           workTable.Rows.Add(workTable.NewRow());
                        }
                     }

                     if (newDsYMD.Rows.Count > 0 && newDt.Rows.Count > 0) {
                        newDsYMD.Sort("am0_ymd");
                        newDt.Sort("am0_ymd");
                     }

                  }
                  DataRow headerRow = workTable.NewRow();
                  if (!selectEng) {
                     workTable.Columns.Add("期貨商代號", typeof(string));
                     workTable.Columns.Add("名稱", typeof(string));
                     headerRow[0] = "Date";
                     headerRow[1] = "";
                  }
                  else {
                     workTable.Columns.Add("Sequential No.", typeof(string));
                     headerRow[0] = "Date";
                  }
                  //lsOutput1 = new StringBuilder(!selectEng ? "期貨商代號" + "," + "名稱" : " Sequential No.");
                  //lsOutput2 = new StringBuilder(!selectEng ? "Date" + "," : "Date");
                  liAreaCnt = 1;
                  int ParamKeyCount = 1;
                  foreach (DataRow ymdRow in newDsYMD.Rows) {
                     lsYMD = ymdRow["am0_ymd"].AsString();
                     if (liArea == 2 && PbFunc.Right(lsYMD, 2).AsInt() > 15 && liAreaCnt == 1) {
                        liAreaCnt = 2;
                     }
                     
                     if (lsYMD == "99999999") {
                        lsStr = !selectEng ? "總計" : "Year-To-Date Volume of";
                     }

                     string pkYMD = "";
                     foreach (DataRow pkRow in newdtPK.Rows) {
                        lsParamKey = pkRow["am0_param_key"].AsString();
                        /*******************
                        換商品代號
                        *******************/
                        lsParamKey = pkRow["rpt_value_2"].AsString();
                        //lsOutput1.Append("," + lsStr + lsParamKey);

                        workTable.Columns.Add(lsYMD, typeof(string));
                        //lsOutput1.Append("," + lsStr + lsParamKey);
                        workTable.Columns.Add(lsStr + lsParamKey, typeof(string));
                        if (pkYMD == lsYMD) {
                           headerRow[ParamKeyCount++] = "";
                        }
                        else {
                           pkYMD = lsYMD;
                           headerRow[ParamKeyCount++] = lsYMD + " - " + ymdRow["ymd_end"].AsString();
                        }
                     }//foreach(DataRow pkRow in newdtPK.Rows)

                     //lsOutput2.Append("," + lsYMD + ",".PadRight(newdtPK.Rows.Count, ','));

                     if (lsYMD == "99999999") {
                        //lsOutput1.Append("," + (!selectEng ? lsStr : "Year-To-Date Market Volume"));
                        headerRow[ParamKeyCount++] = !selectEng ? lsStr : "Year-To-Date Market Volume";
                     }
                     else {
                        //lsOutput1.Append("," + (!selectEng ? "小計" : "Subtotal"));
                        headerRow[ParamKeyCount++] = !selectEng ? "小計" : "Subtotal";
                     }
                  }//foreach (DataRow ymdRow in dtYMD.Rows)
                  if (liAreaCnt == liArea && lsSumType != "Y") {
                     headerRow[ParamKeyCount++] = !selectEng ? "市佔率" : "YTD Market shares(%)";
                     //lsOutput1.Append("," + (!selectEng ? "市佔率" : "YTD Market shares(%)"));
                  }
                  //新增兩列表頭
                  workTable.Rows.Add(headerRow);

               }
               catch (Exception ex) {
                  throw new Exception("表頭:" + ex.Message);
               }
               //內容
               /*******************
               OpenData
               *******************/
               //期貨商代號&名稱
               DataRow contentRow = workTable.NewRow();
               ABRK daoABRK = new ABRK();
               try {
                  foreach (DataRow brkRow in dtBRK.Rows) {
                     lsBrkNo4 = brkRow["AM0_BRK_NO4"].AsString();
                     lsBrkType = brkRow["AM0_BRK_TYPE"].AsString();
                     if (lsBrkType.Trim() == "9") {
                        lsBrkNo = lsBrkNo4.Trim() + "999";
                     }
                     else {
                        lsBrkNo = lsBrkNo4.Trim() + "000";
                     }
                     lsBrkName = daoABRK.GetNameByNo(lsBrkNo);// f_get_abrk_name(lsBrkNo,'0')	
                     //lsOutput1 = new StringBuilder(!selectEng ? (lsBrkNo + "," + lsBrkName) : lsBrkNo);//轉英文只秀編號
                     if (!selectEng) {
                        contentRow[0] = lsBrkNo;
                        contentRow[1] = lsBrkName;
                     }
                     else {
                        contentRow[0] = lsBrkNo;
                     }

                     //日期
                     int colIndex = 2;
                     foreach (DataRow ymdRow in newDsYMD.Rows) {
                        lsYMD = ymdRow["AM0_YMD"].AsString();
                        string lsOpenDataStr = lsBrkNo + "," + lsBrkName + "," + lsYMD;
                        ldSum = 0;
                        //商品
                        foreach (DataRow pkRow in newdtPK.Rows) {
                           lsParamKey = pkRow["AM0_PARAM_KEY"].AsString();
                           int foundIndex = newDt.Rows.IndexOf(newDt.Select($@"AM0_BRK_NO4='{ lsBrkNo4 }' and AM0_BRK_TYPE='{lsBrkType}' and AM0_YMD='{ lsYMD }' and AM0_PARAM_KEY='{ lsParamKey}'").FirstOrDefault());
                           /* 沒有填0 */
                           if (foundIndex > -1) {
                              ldVal = newDt.Rows[foundIndex]["QNTY"].AsDecimal();
                              //ldSum = ids_1.getitemdecimal(foundIndex,"cp_sum_qnty")
                              ldSum = newDt.Rows[foundIndex]["CP_SUM_QNTY"].AsDecimal();
                           }
                           else {
                              ldVal = 0;
                           }
                           //lsOutput1.Append("," + ldVal.AsString());
                           contentRow[colIndex++] = ldVal.AsString();
                           if (lsYMD != "99999999" && !selectEng) {
                              string writeOpenData = lsOpenDataStr + "," + lsParamKey + "," + ldVal.AsString();
                              DataRow newrow = opendataTable.NewRow();
                              newrow[0] = writeOpenData;
                              opendataTable.Rows.Add(newrow[0]);
                           }
                        }//foreach(DataRow pkRow in newdtPK.Rows)
                        contentRow[colIndex++] = ldSum.AsString();
                        //lsOutput1.Append("," + ldSum.AsString());
                     }//foreach (DataRow ymdRow in dtYMD.Rows)
                     if (liAreaCnt == liArea && lsSumType != "Y") {
                        //lsOutput1.Append($",{brkRow["CP_RATE"].AsDecimal().ToString("n")}");
                        contentRow[colIndex++] = brkRow["CP_RATE"].AsDecimal().ToString("n");
                     }
                     //新增一行
                     workTable.Rows.Add(contentRow);
                  }//foreach (DataRow brkRow in dtBRK.Rows)

                  //存檔
                  SaveExcel(SaveFilePath, workTable);
                  SaveExcel(openData, opendataTable);
               }
               catch (Exception ex) {
                  throw new Exception("內容:" + ex.Message);
               }
            }//for (int liAreaCnt = 1; liAreaCnt <= liArea; liAreaCnt++)
             /*******************
             W_OpenData
             *******************/
            if (lsProdType == "O" && lsSumType == "D" && !selectEng) {
               dt = dao70010.ListRowdataOpendata(lsEndYMD, lsEndYMD, lsMarketCode);
               SaveExcel(PbFunc.f_chg_filename(SaveFilePath, "_W_opendata"), dt, false);
            }
         }
         catch (Exception ex) {
            throw ex;
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)英文版
      /// |
      /// 呼叫來源: 70010 (由業務單位手動產生)10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="SaveFilePath">檔名</param>
      /// <param name="lsStartYMD">起始日期</param>
      /// <param name="lsEndYMD">終止日期</param>
      /// <param name="lsSumType">統計別:D日,M月,Y年</param>
      /// <param name="lsProdType">商品別:F期貨,O選擇權</param>
      /// <param name="lsMarketCode"></param>
      /// <param name="selectEng">選取英文版本</param>
      /// <returns>RETURN:false代表error/true代表成功</returns>
      public string F70010YmdEngByMarketCode(string SaveFilePath, string lsStartYMD, string lsEndYMD, string lsSumType, string lsProdType, string lsMarketCode)
      {
         return F70010YmdByMarketCode(SaveFilePath, lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode, true);
      }

      /// <summary>
      /// 作業:轉"歷史"70010日,月,年檔 (公司網站\統計資料\日,月,年)
      /// |
      /// 呼叫來源: 11010
      /// </summary>
      /// <param name="SaveFilePath"></param>
      /// <param name="lsStartYMD"></param>
      /// <param name="lsEndYMD"></param>
      /// <param name="lsSumType"></param>
      /// <param name="lsProdType"></param>
      /// <returns></returns>
      //public string F70010YmdHis(string SaveFilePath, string lsStartYMD, string lsEndYMD, string lsSumType, string lsProdType)
      //{
      //   return "";
      //}

      /// <summary>
      /// 作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)
      /// |
      /// 呼叫來源: 11010
      /// </summary>
      /// <param name="SaveFilePath"></param>
      /// <param name="lsStartYMD"></param>
      /// <param name="lsEndYMD"></param>
      /// <param name="lsSumType"></param>
      /// <param name="lsProdType"></param>
      /// <returns></returns>
      //public string F70010YmdOrg(string SaveFilePath, string lsStartYMD, string lsEndYMD, string lsSumType, string lsProdType)
      //{
      //   return "";
      //}

      /// <summary>
      /// 作業: 轉70010 日, 月, 年檔 (公司網站\統計資料\日 , 月 , 年)
      /// |
      /// 呼叫來源: 70010(由業務單位手動產生)10012,10022(由OP操作批次時自動產生)
      /// </summary>
      /// <param name="SaveFilePath">檔名</param>
      /// <param name="lsStartYMD">起始日期</param>
      /// <param name="lsEndYMD">終止日期</param>
      /// <param name="lsSumType">統計別: D日,M月,Y年</param>
      /// <param name="isKindId2">商品別: F期貨,O選擇權</param>
      /// <param name="isParamKey"></param>
      /// <param name="lsProdType"></param>
      public string F70010YmdW(string SaveFilePath, string lsStartYMD, string lsEndYMD,
                                 string lsSumType, string isKindId2,
                                 string isParamKey, string lsProdType)
      {

         //作業: 轉70010 日, 月, 年檔 (公司網站\統計資料\日 , 月 , 年)
         //呼叫來源: 70010(由業務單位手動產生)
         //          10012,10022(由OP操作批次時自動產生)
         //參數: (1)檔名
         //      (2)起始日期
         //      (3)終止日期
         //      (4)統計別: D日,M月,Y年
         //       (5)商品別: F期貨,O選擇權
         //   RETURN:E代表error
         //          Y代表成功


         D70050 dao70050 = new D70050();
         D70010 dao70010 = new D70010();

         ///******************
         //讀取資料
         //******************/
         DataTable dt = dao70050.ListAll(lsStartYMD, lsEndYMD, lsSumType, lsProdType, isKindId2, isParamKey);
         if (dt.Rows.Count <= 0) {
            return $@"轉{isParamKey}-交易量資料轉檔作業({lsSumType})({lsStartYMD}-{lsEndYMD})(期貨/選擇權:{ lsProdType })筆數為０!";
         }

         //避免重複寫入
         if (File.Exists(SaveFilePath)) {
            File.Delete(SaveFilePath);
         }
         File.Create(SaveFilePath).Close();

         try {

            /* 期貨商 */
            DataTable dtBRK;
            dtBRK = dao70050.List70050brk(lsStartYMD, lsEndYMD, lsSumType, lsProdType, isKindId2, isParamKey);

            /* 日期 */
            DataTable dtYMD = dao70010.ListYMD(lsStartYMD, lsEndYMD, lsSumType, lsProdType);
            DataRow newymdRow = dtYMD.NewRow();
            newymdRow["am0_ymd"] = "99999999";
            dtYMD.Rows.Add(newymdRow);
            /* 商品 */
            DataTable dtPK;
            if (isParamKey == "TXO") {
               dtPK = dao70010.ListParamKey("70040");
            }//if (isParamKey == "TXO")
            else {
               dtPK = dao70010.ListParamKey("70050");
            }
            DataTable newdtPK = dtPK.Filter($@"am0_param_key like '{ isKindId2 }'");

            string lsBrkNo, lsBrkNo4, lsBrkType, lsYMD, lsStr;
            //主要的資料
            DataTable workTable = new DataTable();
            workTable.Columns.Add("DATA", typeof(string));
            /******************
            表頭
            ******************/
            lsStr = "";
            StringBuilder lsOutput1, lsOutput2;
            decimal ldSum;
            DataTable newDsYMD = dtYMD;
            DataTable newDt = dt;
            try {
               lsOutput1 = new StringBuilder("期貨商代號" + "," + "名稱");
               lsOutput2 = new StringBuilder("Date" + ",");
               foreach (DataRow ymdRow in dtYMD.Rows) {
                  lsYMD = ymdRow["am0_ymd"].AsString();
                  lsOutput2.Append("," + lsYMD);
                  if (isKindId2 == "%") {
                     lsOutput2.Append(",".PadRight(newdtPK.Rows.Count, ','));
                  }

                  if (lsYMD == "99999999") {
                     lsStr = "總計";
                  }
                  foreach (DataRow pkRow in newdtPK.Rows) {
                     isParamKey = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     isParamKey = pkRow["rpt_value_2"].AsString();
                     lsOutput1.Append("," + lsStr + isParamKey);
                  }//foreach(DataRow pkRow in newdtPK.Rows)
                  if (isKindId2 == "%") {
                     if (lsYMD == "99999999") {
                        lsOutput1.Append("," + lsStr);
                     }
                     else {
                        lsOutput1.Append("," + "小計");
                     }
                  }
               }//foreach (DataRow ymdRow in dtYMD.Rows)
               lsOutput1.Append("," + "市佔率");

               //新增兩列表頭
               //第一行
               DataRow workRow = workTable.NewRow();
               workRow["DATA"] = lsOutput1.ToString();
               workTable.Rows.Add(workRow["DATA"]);
               //第二行
               DataRow workRow2 = workTable.NewRow();
               workRow2["DATA"] = lsOutput2.ToString();
               workTable.Rows.Add(workRow2["DATA"]);
            }
            catch (Exception ex) {
               throw new Exception("表頭:" + ex.Message);
            }
            //內容
            //期貨商代號&名稱
            try {
               foreach (DataRow brkRow in dtBRK.Rows) {
                  lsBrkNo4 = brkRow["am0_brk_no4"].AsString();
                  lsBrkType = brkRow["am0_brk_type"].AsString();
                  if (lsBrkType.Trim() == "9") {
                     lsBrkNo = lsBrkNo4.Trim() + "999";
                  }
                  else {
                     lsBrkNo = lsBrkNo4.Trim() + "000";
                  }
                  string lsBrkName = new ABRK().GetNameByNo(lsBrkNo);// f_get_abrk_name(lsBrkNo,'0')	
                  lsOutput1 = new StringBuilder(lsBrkNo + "," + lsBrkName);

                  //日期
                  foreach (DataRow ymdRow in dtYMD.Rows) {
                     lsYMD = ymdRow["am0_ymd"].AsString();
                     ldSum = 0;
                     //商品
                     foreach (DataRow pkRow in newdtPK.Rows) {
                        isParamKey = pkRow["am0_param_key"].AsString();
                        int foundIndex = newDt.Rows.IndexOf(newDt.Select($@"am0_brk_no4='{ lsBrkNo4 }' and am0_brk_type='{lsBrkType}' and am0_ymd='{ lsYMD }' and am0_param_key='{ isParamKey}'").FirstOrDefault());
                        /* 沒有填0 */
                        if (foundIndex > -1) {
                           lsOutput1.Append("," + newDt.Rows[foundIndex]["qnty"].AsDecimal());
                           //ldSum = dt.getitemdecimal(foundIndex,"cp_sum_qnty")
                           //ldSum = (from dt in dt.AsEnumerable() where dt.Field<string>("am0_brk_no4") == lsBrkNo4 && dt.Field<string>("am0_brk_type") == lsBrkType && dt.Field<string>("am0_ymd") == lsYMD select dt).ToList().Sum(x=>x.Field<decimal>("qnty"));
                           ldSum = dt.Compute("sum(qnty)", $@"am0_brk_no4='{lsBrkNo4}' and am0_brk_type='{lsBrkType}' and am0_ymd='{lsYMD }'").AsDecimal();
                        }
                        else {
                           lsOutput1.Append(",0");
                        }
                     }//foreach(DataRow pkRow in newdtPK.Rows)
                     if (isKindId2 == "%") {
                        lsOutput1.Append("," + ldSum.AsString());
                     }
                  }//foreach (DataRow ymdRow in dtYMD.Rows)
                  lsOutput1.Append($",{brkRow["cp_rate"].AsDecimal().ToString("n")}");
                  //新增一行
                  DataRow workRow = workTable.NewRow();
                  workRow["DATA"] = lsOutput1.ToString();
                  workTable.Rows.Add(workRow["DATA"]);
               }//foreach (DataRow brkRow in dtBRK.Rows)

               //存檔
               SaveExcel(SaveFilePath, workTable);
            }
            catch (Exception ex) {
               throw new Exception("內容:" + ex.Message);
            }
            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
            throw new Exception("f_70010_ymd_w:" + ex.Message);
         }
      }
   }
}
