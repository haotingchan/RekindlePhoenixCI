﻿using BaseGround.Shared;
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
      private void SaveExcel(string filePath, DataTable dataTable, bool addHeader = false, int firstRowIndex = 0, int firstColIndex = 0)
      {
         try {
            Workbook wb = new Workbook();
            wb.LoadDocument(filePath);
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
      /// 產生檔案並刪除同名檔案，避免重複寫入
      /// </summary>
      /// <param name="SaveFilePath">檔案路徑</param>
      private void CreateFile(string SaveFilePath)
      {
         if (File.Exists(SaveFilePath)) {
            File.Delete(SaveFilePath);
         }
         File.Create(SaveFilePath).Close();
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

         //新增csv檔案
         CreateFile(SaveFilePath);

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

            DateTime ldYMD, ldYMDn;
            //日期:週
            try {
               DataRow toInsert = dtYMDd.NewRow();
               toInsert["am0_ymd"] = "20051231";
               dtYMDd.Rows.InsertAt(toInsert, 0);
               int dtymdCount = dtYMDd.Rows.Count - 1;
               for (int k = 1; k <= dtymdCount; k++) {
                  string am0YMD = dtYMDd.Rows[k]["am0_ymd"].AsString();
                  ldYMD = am0YMD.AsDateTime("yyyyMMdd");
                  ldYMDn = dtYMDd.Rows[k - 1]["am0_ymd"].AsString().AsDateTime("yyyyMMdd");
                  /* 符合下列條件才寫Excel
                  1.最後一筆
                  2.換週 (判斷星期x是否變小)
                  3.與下一日期相差7天以上 (for日期99999999) 
                  */
                  if (k == 1 || ldYMD.DayOfWeek < ldYMDn.DayOfWeek || Math.Abs(PbFunc.DaysAfter(ldYMD, ldYMDn)) > 6) {
                     toInsert = dtYMD.NewRow();
                     toInsert["am0_ymd"] = am0YMD;
                     dtYMD.Rows.Add(toInsert);
                  }
                  dtYMD.Rows[dtYMD.Rows.Count - 1]["ymd_end"] = am0YMD;
               }//for (int k = 2; k <= dtymdCount; k++)
               toInsert = dtYMD.NewRow();
               toInsert["am0_ymd"] = "99999999";
               toInsert["ymd_end"] = "99999999";
               dtYMD.Rows.Add(toInsert);
               dtYMD = dtYMD.Sort("am0_ymd");
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

            DataTable newdtPK = dtPK.Filter($@"rpt_value_3 Is Null or TRIM(rpt_value_3) = '' or rpt_value_3 >= '{ lsStartYMD }'");
            /******************
            表頭
            ******************/
            int ParamKeyCount = 0;
            int arrayLen = 2 + dtYMD.Rows.Count * (newdtPK.Rows.Count + 1) + 1;//期貨商代號+名稱+商品+小計+市佔率
            if (selectEng)
               arrayLen = arrayLen - 1;//-名稱=期貨商代號+商品+小計+市佔率

            try {
               object[] headerRow = new object[arrayLen];
               object[] subtitleRow = new object[arrayLen];
               if (!selectEng) {
                  workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                  headerRow[ParamKeyCount] = "期貨商代號";
                  subtitleRow[ParamKeyCount] = "Date";
                  ParamKeyCount++;
                  workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                  headerRow[ParamKeyCount] = "名稱";
                  subtitleRow[ParamKeyCount] = "";
                  ParamKeyCount++;
               }
               else {
                  workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                  headerRow[ParamKeyCount] = " Sequential No. ";
                  subtitleRow[ParamKeyCount] = "Date";
                  ParamKeyCount++;
               }

               string sumStr = "";
               foreach (DataRow ymdRow in dtYMD.Rows) {
                  string am0YMD = ymdRow["am0_ymd"].AsString();

                  if (am0YMD == "99999999") {
                     sumStr = !selectEng ? "總計" : "Year-To-Date Volume of ";
                  }

                  string pkYMD = "";
                  foreach (DataRow pkRow in newdtPK.Rows) {
                     string lsParamKey = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     lsParamKey = pkRow["rpt_value_2"].AsString();
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     headerRow[ParamKeyCount] = sumStr + lsParamKey;
                     if (pkYMD == am0YMD) {
                        subtitleRow[ParamKeyCount] = "";
                     }
                     else {
                        pkYMD = am0YMD;
                        subtitleRow[ParamKeyCount] = am0YMD + " - " + ymdRow["ymd_end"].AsString();
                     }

                     ParamKeyCount++;
                  }//foreach(DataRow pkRow in newdtPK.Rows)

                  if (am0YMD == "99999999") {
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     headerRow[ParamKeyCount] = !selectEng ? sumStr : "Year-To-Date Market Volume";
                     ParamKeyCount++;
                  }
                  else {
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     headerRow[ParamKeyCount] = !selectEng ? "小計" : "Subtotal";
                     ParamKeyCount++;
                  }
               }//foreach (DataRow ymdRow in dtYMD.Rows)
               workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
               headerRow[ParamKeyCount] = !selectEng ? "市佔率" : "YTD Market shares(%)";
               //新增兩列表頭
               workTable.Rows.Add(headerRow);
               workTable.Rows.Add(subtitleRow);
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

            object[] contentRow = new object[arrayLen];
            /*******************
            OpenData
            *******************/
            //期貨商代號&名稱
            ABRK daoABRK = new ABRK();

            try {
               decimal ldSum = 0;
               foreach (DataRow brkRow in dtBRK.Rows) {
                  string lsBrkNo4 = brkRow["am0_brk_no4"].AsString();
                  string lsBrkType = brkRow["am0_brk_type"].AsString();

                  string lsBrkNo;
                  if (lsBrkType.Trim() == "9") {
                     lsBrkNo = lsBrkNo4.Trim() + "999";
                  }
                  else {
                     lsBrkNo = lsBrkNo4.Trim() + "000";
                  }
                  string lsBrkName = daoABRK.GetNameByNo(lsBrkNo);// f_get_abrk_name(lsBrkNo,'0')	
                  if (!selectEng) {
                     contentRow[0] = lsBrkNo;
                     contentRow[1] = lsBrkName;
                  }
                  else {
                     contentRow[0] = lsBrkNo;
                  }
                  //日期
                  int colIndex = !selectEng ? 2 : 1;
                  foreach (DataRow ymdRow in dtYMD.Rows) {
                     string am0YMD = ymdRow["am0_ymd"].AsString();
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

                     //商品
                     foreach (DataRow pkRow in newdtPK.Rows) {
                        contentRow[colIndex++] = pkRow["qnty"].AsString();
                        if (am0YMD.SubStr(0, 8) != "99999999" && !selectEng) {
                           opendataTable.Rows.Add(new object[] { lsBrkNo, lsBrkName, ymdRow["am0_ymd"].AsString() + " - " + ymdRow["ymd_end"].AsString(), pkRow["am0_param_key"].AsString(), pkRow["qnty"].AsString() });
                        }
                        pkRow["qnty"] = 0;
                     }//foreach(DataRow pkRow in newdtPK.Rows)
                     contentRow[colIndex++] = ldSum.AsString();
                  }//foreach (DataRow ymdRow in dtYMD.Rows)
                  contentRow[colIndex++] = brkRow["cp_rate"].AsDecimal().ToString("n");

                  workTable.Rows.Add(contentRow);
               }//foreach (DataRow brkRow in dtBRK.Rows)

               //存檔
               SaveExcel(SaveFilePath, workTable);

               if (!selectEng) {
                  string openData = PbFunc.f_chg_filename(SaveFilePath, "_OpenData");
                  CreateFile(openData);
                  //WriteFile(openData, "期貨商代號,期貨商名稱,日期,商品,交易量");
                  SaveExcel(openData, opendataTable, true);
               }
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
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)英文版 aka F70010WeekByMarketCode
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
      /// 呼叫來源:70050、70040 (由業務單位手動產生)10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="SaveFilePath">檔名</param>
      /// <param name="lsStartYMD">起始日期</param>
      /// <param name="lsEndYMD">終止日期</param>
      /// <param name="lsSumType">統計別:D日,M月,Y年</param>
      /// <param name="lsKindId2">期別:一週到期契約,一般天到期契約,所有天到期契約</param>
      /// <param name="lsParamKey">商品</param>
      /// <param name="lsProdType">F期貨,O選擇權</param>
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

         //新增csv檔案
         CreateFile(SaveFilePath);

         try {

            /* 期貨商 */
            DataTable dtBRK;
            dtBRK = dao70050.List70050brk(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsKindId2, lsParamKey);

            /* 日期 */
            DataTable dtYMDd = dao70010.ListYMD(lsStartYMD, lsEndYMD, lsSumType, lsProdType);
            //DataTable dtYMD = dao70010.ListYmdEnd(lsStartYMD, lsEndYMD, lsSumType, lsProdType);
            DataTable dtYMD = dao70010.ListYmdEnd(null, null, null, null).Clone();//PB還沒有在這retrieve
            /* 商品 */
            DataTable dtPK;
            if (lsParamKey == "TXO") {
               dtPK = dao70010.ListParamKey("70040");
            }//if (isParamKey == "TXO")
            else {
               dtPK = dao70010.ListParamKey("70050");
            }
            DataTable newdtPK = dtPK.Filter($@"am0_param_key like '{ lsKindId2 }'");

            DateTime ldYMD, ldYMDn;
            //日期:週
            try {
               DataRow toInsert = dtYMDd.NewRow();
               toInsert["am0_ymd"] = "20051231";
               dtYMDd.Rows.InsertAt(toInsert, 0);
               int dtymdCount = dtYMDd.Rows.Count - 1;
               for (int k = 1; k <= dtymdCount; k++) {
                  string am0YMD = dtYMDd.Rows[k]["am0_ymd"].AsString();
                  ldYMD = DateTime.ParseExact(am0YMD, "yyyyMMdd", CultureInfo.CurrentCulture);
                  ldYMDn = DateTime.ParseExact(dtYMDd.Rows[k - 1]["am0_ymd"].AsString(), "yyyyMMdd", CultureInfo.CurrentCulture);
                  /* 符合下列條件才寫Excel
                  1.最後一筆
                  2.換週 (判斷星期x是否變小)
                  3.與下一日期相差7天以上 (for日期99999999) 
                  */
                  if (k == 1 || ldYMD.DayOfWeek < ldYMDn.DayOfWeek || Math.Abs(PbFunc.DaysAfter(ldYMD, ldYMDn)) > 6) {
                     toInsert = dtYMD.NewRow();
                     toInsert["am0_ymd"] = am0YMD;
                     dtYMD.Rows.Add(toInsert);
                  }
                  dtYMD.Rows[dtYMD.Rows.Count - 1]["ymd_end"] = am0YMD;
               }//for (int k = 2; k <= dtymdCount; k++)
               toInsert = dtYMD.NewRow();
               toInsert["am0_ymd"] = "99999999";
               toInsert["ymd_end"] = "99999999";
               dtYMD.Rows.Add(toInsert);
               dtYMD = dtYMD.Sort("am0_ymd");
            }
            catch (Exception ex) {
               throw new Exception("日期:週-" + ex.Message);
            }

            //主要的資料
            DataTable workTable = new DataTable();

            /******************
            表頭
            ******************/
            DataTable newDsYMD = dtYMD;

            int ParamKeyCount = 0;
            int prodCount = lsKindId2 == "%" ? (newdtPK.Rows.Count + 1) :1;//商品群
            int arrayLen = 2 + (dtYMD.Rows.Count * prodCount) + 1;//期貨商代號+名稱+商品群+小計+市佔率
            object[] headerRow = new object[arrayLen];
            object[] subtitleRow = new object[arrayLen];

            workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
            headerRow[ParamKeyCount] = "期貨商代號";
            subtitleRow[ParamKeyCount] = "Date";
            ParamKeyCount++;
            workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
            headerRow[ParamKeyCount] = "名稱";
            subtitleRow[ParamKeyCount] = "";
            ParamKeyCount++;

            try {
               string sumStr = "";
               foreach (DataRow ymdRow in dtYMD.Rows) {
                  string am0YMD = ymdRow["am0_ymd"].AsString();

                  if (am0YMD == "99999999") {
                     sumStr = "總計";
                  }
                  string pkYMD = "";
                  foreach (DataRow pkRow in newdtPK.Rows) {
                     lsParamKey = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     lsParamKey = pkRow["rpt_value_2"].AsString();
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     headerRow[ParamKeyCount] = sumStr + lsParamKey;

                     if (pkYMD == am0YMD && lsKindId2 == "%") {
                        subtitleRow[ParamKeyCount] = "";
                     }
                     else {
                        pkYMD = am0YMD;
                        subtitleRow[ParamKeyCount] = am0YMD + " - " + ymdRow["ymd_end"].AsString();
                     }

                     ParamKeyCount++;
                  }//foreach(DataRow pkRow in newdtPK.Rows)

                  if (lsKindId2 == "%") {
                     if (am0YMD == "99999999") {
                        workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                        headerRow[ParamKeyCount] = sumStr;
                        ParamKeyCount++;
                     }
                     else {
                        workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                        headerRow[ParamKeyCount] = "小計";
                        ParamKeyCount++;
                     }
                  }
               }//foreach (DataRow ymdRow in dtYMD.Rows)
               workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
               headerRow[ParamKeyCount] = "市佔率";

               //新增兩列表頭
               workTable.Rows.Add(headerRow);
               workTable.Rows.Add(subtitleRow);
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
            object[] contentRow = new object[arrayLen];
            decimal ldSum = 0;
            try {
               foreach (DataRow brkRow in dtBRK.Rows) {
                  string lsBrkNo4 = brkRow["am0_brk_no4"].AsString();
                  string lsBrkType = brkRow["am0_brk_type"].AsString();

                  string lsBrkNo;
                  if (lsBrkType.Trim() == "9") {
                     lsBrkNo = lsBrkNo4.Trim() + "999";
                  }
                  else {
                     lsBrkNo = lsBrkNo4.Trim() + "000";
                  }

                  string lsBrkName = new ABRK().GetNameByNo(lsBrkNo);// f_get_abrk_name(lsBrkNo,'0')	
                  contentRow[0] = lsBrkNo;
                  contentRow[1] = lsBrkName;

                  //日期
                  int colIndex = 2;
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
                        contentRow[colIndex++] = ldSum.AsString();
                     }

                  }//foreach (DataRow ymdRow in dtYMD.Rows)
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
         //新增csv檔案
         CreateFile(SaveFilePath);

         try {
            ///******************
            //讀取資料
            //******************/
            DataTable dt = dao70010.ListRowdata(lsStartYMD, lsEndYMD, lsSumType, lsProdType, lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return $@"轉70010-交易量資料轉檔作業({lsSumType})({lsStartYMD}-{lsEndYMD})(期貨/選擇權:{ lsProdType })筆數為０!";
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

            /***************************
            因Excel column數限制,
            若"期貨"選擇日期迄超過15,則分成2區
            ***************************/
            int liArea;
            if (PbFunc.Right(lsEndYMD, 2).AsInt() > 15 && lsSumType == "D" && lsProdType == "F") {
               liArea = 2;
            }
            else {
               liArea = 1;
            }
            /******************
            表頭
            ******************/

            //主要的資料
            DataTable workTable = new DataTable();
            //opendata DataTable
            DataTable opendataTable = new DataTable();
            opendataTable.Columns.Add("期貨商代號", typeof(string));
            opendataTable.Columns.Add("期貨商名稱", typeof(string));
            opendataTable.Columns.Add("日期", typeof(string));
            opendataTable.Columns.Add("商品", typeof(string));
            opendataTable.Columns.Add("交易量", typeof(string));


            DataTable newDsYMD = dtYMD;
            DataTable newDt = dt;
            int arrayLen = 0;
            int useCpuCount = Environment.ProcessorCount > 3 ? 2 : 1;//限制cpu使用數目
            for (int liAreaCnt = 1; liAreaCnt <= liArea; liAreaCnt++) {
               try {
                  if (liArea == 2) {
                     if (liAreaCnt == 1) {
                        newDsYMD = dtYMD.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount)
                           .Where(r => r.Field<object>("am0_ymd").AsString().SubStr(6, 2).AsInt() <= 15).CopyToDataTable();

                        newDt = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount)
                           .Where(r => r.Field<object>("am0_ymd").AsString().SubStr(6, 2).AsInt() <= 15).CopyToDataTable();

                        workTable = new DataTable();

                        arrayLen = 2 + newDsYMD.Rows.Count * (newdtPK.Rows.Count + 1);//期貨商代號+名稱+商品群+小計
                        if (selectEng)
                           arrayLen = arrayLen - 1;//-名稱
                     }
                     else {
                        newDsYMD = dtYMD.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount)
                           .Where(r => r.Field<object>("am0_ymd").AsString().SubStr(6, 2).AsInt() > 15).CopyToDataTable();

                        newDt = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(useCpuCount)
                           .Where(r => r.Field<object>("am0_ymd").AsString().SubStr(6, 2).AsInt() > 15).CopyToDataTable();

                        workTable = new DataTable();
                        arrayLen = 2 + newDsYMD.Rows.Count * (newdtPK.Rows.Count + 1) + 1;//期貨商代號+名稱+商品群+小計+市佔率
                        if (selectEng)
                           arrayLen = arrayLen - 1;//-名稱=期貨商代號+商品群+小計+市佔率
                     }
                     //Parallel PLinq搜尋出來的資料不會按照順序 所以最後要重新排序
                     if (newDsYMD.Rows.Count > 0 && newDt.Rows.Count > 0) {
                        newDsYMD = newDsYMD.Sort("am0_ymd");
                        newDt = newDt.Sort("am0_ymd");
                     }

                  }//if (liArea == 2)
                  else {
                     if (lsSumType != "Y")
                        arrayLen = 2 + dtYMD.Rows.Count * (newdtPK.Rows.Count + 1) + 1;//期貨商代號+名稱+商品群+小計+市佔率
                     else
                        arrayLen = 2 + dtYMD.Rows.Count * (newdtPK.Rows.Count + 1);//期貨商代號+名稱+商品群+小計

                     if (selectEng)
                        arrayLen = arrayLen - 1;//-名稱=期貨商代號+商品群+小計
                  }

                  int ParamKeyCount = 0;
                  object[] headerRow = new object[arrayLen];
                  object[] subtitleRow = new object[arrayLen];
                  if (!selectEng) {
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     headerRow[ParamKeyCount] = "期貨商代號";
                     ParamKeyCount++;
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     headerRow[ParamKeyCount] = "名稱";
                     ParamKeyCount++;

                     subtitleRow[0] = "Date";
                     subtitleRow[1] = "";
                  }
                  else {
                     headerRow[ParamKeyCount] = "Sequential No";
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     ParamKeyCount++;
                     subtitleRow[0] = "Date";
                  }

                  liAreaCnt = 1;

                  string sumStr = "";
                  foreach (DataRow ymdRow in newDsYMD.Rows) {
                     string am0YMD = ymdRow["am0_ymd"].AsString();
                     if (liArea == 2 && PbFunc.Right(am0YMD, 2).AsInt() > 15 && liAreaCnt == 1) {
                        liAreaCnt = 2;
                     }

                     if (am0YMD == "99999999") {
                        sumStr = !selectEng ? "總計" : "Year-To-Date Volume of ";
                     }

                     string pkYMD = "";
                     foreach (DataRow pkRow in newdtPK.Rows) {
                        string lsParamKey = pkRow["am0_param_key"].AsString();
                        /*******************
                        換商品代號
                        *******************/
                        lsParamKey = pkRow["rpt_value_2"].AsString();

                        workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                        headerRow[ParamKeyCount] = sumStr + lsParamKey;

                        if (pkYMD == am0YMD) {
                           subtitleRow[ParamKeyCount] = "";
                        }
                        else {
                           pkYMD = am0YMD;
                           subtitleRow[ParamKeyCount] = am0YMD;
                        }

                        ParamKeyCount++;
                     }//foreach(DataRow pkRow in newdtPK.Rows)

                     if (am0YMD == "99999999") {
                        workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                        headerRow[ParamKeyCount] = !selectEng ? sumStr : "Year-To-Date Market Volume";
                        ParamKeyCount++;
                     }
                     else {
                        workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                        headerRow[ParamKeyCount] = !selectEng ? "小計" : "Subtotal";
                        ParamKeyCount++;
                     }
                  }//foreach (DataRow ymdRow in dtYMD.Rows)

                  if (liAreaCnt == liArea && lsSumType != "Y") {
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     headerRow[ParamKeyCount] = !selectEng ? "市佔率" : "YTD Market shares(%)";
                     ParamKeyCount++;
                  }
                  //新增兩列表頭
                  workTable.Rows.Add(headerRow);
                  workTable.Rows.Add(subtitleRow);

               }
               catch (Exception ex) {
                  throw new Exception("表頭:" + ex.Message);
               }
               //內容
               /*******************
               OpenData
               *******************/
               //期貨商代號&名稱
               object[] contentRow = new object[arrayLen];
               ABRK daoABRK = new ABRK();

               try {
                  foreach (DataRow brkRow in dtBRK.Rows) {
                     string lsBrkNo4 = brkRow["AM0_BRK_NO4"].AsString();
                     string lsBrkType = brkRow["AM0_BRK_TYPE"].AsString();

                     string lsBrkNo;
                     if (lsBrkType.Trim() == "9") {
                        lsBrkNo = lsBrkNo4.Trim() + "999";
                     }
                     else {
                        lsBrkNo = lsBrkNo4.Trim() + "000";
                     }
                     string lsBrkName = daoABRK.GetNameByNo(lsBrkNo);// f_get_abrk_name(lsBrkNo,'0')	

                     if (!selectEng) {
                        contentRow[0] = lsBrkNo;
                        contentRow[1] = lsBrkName;
                     }
                     else {
                        contentRow[0] = lsBrkNo;
                     }

                     //日期
                     int colIndex = !selectEng ? 2 : 1;
                     foreach (DataRow ymdRow in newDsYMD.Rows) {
                        string lsYMD = ymdRow["AM0_YMD"].AsString();
                        decimal ldSum = 0;
                        //商品
                        foreach (DataRow pkRow in newdtPK.Rows) {
                           string lsParamKey = pkRow["AM0_PARAM_KEY"].AsString();
                           int foundIndex = newDt.Rows.IndexOf(newDt.Select($@"AM0_BRK_NO4='{ lsBrkNo4 }' and AM0_BRK_TYPE='{lsBrkType}' and AM0_YMD='{ lsYMD }' and AM0_PARAM_KEY='{ lsParamKey}'").FirstOrDefault());
                           /* 沒有填0 */
                           decimal ldVal;
                           if (foundIndex > -1) {
                              ldVal = newDt.Rows[foundIndex]["QNTY"].AsDecimal();
                              //ldSum = ids_1.getitemdecimal(foundIndex,"cp_sum_qnty")
                              ldSum = newDt.Rows[foundIndex]["CP_SUM_QNTY"].AsDecimal();
                           }
                           else {
                              ldVal = 0;
                           }

                           contentRow[colIndex++] = ldVal.AsString();
                           if (lsYMD != "99999999" && !selectEng) {
                              opendataTable.Rows.Add(new object[] { lsBrkNo, lsBrkName, lsYMD, lsParamKey, ldVal.AsString() });
                           }
                        }//foreach(DataRow pkRow in newdtPK.Rows)
                        contentRow[colIndex++] = ldSum.AsString();

                     }//foreach (DataRow ymdRow in dtYMD.Rows)
                     if (liAreaCnt == liArea && lsSumType != "Y") {
                        contentRow[colIndex++] = brkRow["CP_RATE"].AsDecimal().ToString("n");
                     }
                     //新增一行
                     workTable.Rows.Add(contentRow);
                  }//foreach (DataRow brkRow in dtBRK.Rows)

                  //存檔
                  if (liAreaCnt == 1) {
                     SaveExcel(SaveFilePath, workTable);
                  }
                  else if (liAreaCnt == 2) {
                     SaveExcel(SaveFilePath, workTable, false, dtBRK.Rows.Count + 2 + 8, 0);
                  }

               }
               catch (Exception ex) {
                  throw new Exception("內容:" + ex.Message);
               }
            }//for (int liAreaCnt = 1; liAreaCnt <= liArea; liAreaCnt++)

            if (!selectEng) {
               string openData = PbFunc.f_chg_filename(SaveFilePath, "_OpenData");
               CreateFile(openData);
               //WriteFile(openData, "期貨商代號,期貨商名稱,日期,商品,交易量");
               SaveExcel(openData, opendataTable, true);
            }
            /*******************
             W_OpenData
             *******************/
            if (lsProdType == "O" && lsSumType == "D" && !selectEng) {
               dt = dao70010.ListRowdataOpendata(lsEndYMD, lsEndYMD, lsMarketCode);
               ExportOptions csvref = new ExportOptions();
               csvref.HasHeader = false;
               csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
               ExportHelper.ToCsv(dt, PbFunc.f_chg_filename(SaveFilePath, "_W_opendata"), csvref);
            }
         }
         catch (Exception ex) {
            throw ex;
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)英文版 aka F70010YmdByMarketCode
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
      /// 呼叫來源: 70050、70040(由業務單位手動產生)10012,10022(由OP操作批次時自動產生)
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

         //新增csv檔案
         CreateFile(SaveFilePath);

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


            //主要的資料
            DataTable workTable = new DataTable();

            /******************
            表頭
            ******************/

            DataTable newDsYMD = dtYMD;
            DataTable newDt = dt;

            int ParamKeyCount = 0;
            int arrayLen = 2 + dtYMD.Rows.Count * (newdtPK.Rows.Count + 1) + 1;//期貨商代號+名稱+商品群+小計+市佔率
            object[] headerRow = new object[arrayLen];
            object[] subtitleRow = new object[arrayLen];
            workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
            headerRow[ParamKeyCount] = "期貨商代號";
            subtitleRow[ParamKeyCount] = "Date";
            ParamKeyCount++;
            workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
            headerRow[ParamKeyCount] = "名稱";
            subtitleRow[ParamKeyCount] = "";
            ParamKeyCount++;

            string sumStr = "";
            try {
               foreach (DataRow ymdRow in dtYMD.Rows) {
                  string am0YMD = ymdRow["am0_ymd"].AsString();
                  workTable.Columns.Add(am0YMD, typeof(string));

                  if (am0YMD == "99999999") {
                     sumStr = "總計";
                  }

                  string pkYMD = "";
                  foreach (DataRow pkRow in newdtPK.Rows) {
                     isParamKey = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     isParamKey = pkRow["rpt_value_2"].AsString();
                     workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                     headerRow[ParamKeyCount] = sumStr + isParamKey;
                     if (pkYMD == am0YMD && isKindId2 == "%") {
                        subtitleRow[ParamKeyCount] = "";
                     }
                     else {
                        pkYMD = am0YMD;
                        subtitleRow[ParamKeyCount] = am0YMD;
                     }

                     ParamKeyCount++;
                  }//foreach(DataRow pkRow in newdtPK.Rows)
                  if (isKindId2 == "%") {
                     if (am0YMD == "99999999") {
                        workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                        headerRow[ParamKeyCount] = sumStr;
                        ParamKeyCount++;
                     }
                     else {
                        workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
                        headerRow[ParamKeyCount] = "小計";
                        ParamKeyCount++;
                     }
                  }
               }//foreach (DataRow ymdRow in dtYMD.Rows)
               workTable.Columns.Add(ParamKeyCount.AsString(), typeof(string));
               headerRow[ParamKeyCount] = "市佔率";

               //新增兩列表頭
               workTable.Rows.Add(headerRow);
               workTable.Rows.Add(subtitleRow);
            }
            catch (Exception ex) {
               throw new Exception("表頭:" + ex.Message);
            }
            //內容
            //期貨商代號&名稱
            object[] contentRow = new object[arrayLen];
            try {
               foreach (DataRow brkRow in dtBRK.Rows) {
                  string lsBrkNo4 = brkRow["am0_brk_no4"].AsString();
                  string lsBrkType = brkRow["am0_brk_type"].AsString();

                  string lsBrkNo;
                  if (lsBrkType.Trim() == "9") {
                     lsBrkNo = lsBrkNo4.Trim() + "999";
                  }
                  else {
                     lsBrkNo = lsBrkNo4.Trim() + "000";
                  }

                  string lsBrkName = new ABRK().GetNameByNo(lsBrkNo);// f_get_abrk_name(lsBrkNo,'0')	
                  contentRow[0] = lsBrkNo;
                  contentRow[1] = lsBrkName;

                  //日期
                  int colIndex = 2;
                  foreach (DataRow ymdRow in dtYMD.Rows) {
                     string am0YMD = ymdRow["am0_ymd"].AsString();
                     decimal ldSum = 0;
                     //商品
                     foreach (DataRow pkRow in newdtPK.Rows) {
                        isParamKey = pkRow["am0_param_key"].AsString();
                        int foundIndex = newDt.Rows.IndexOf(newDt.Select($@"am0_brk_no4='{ lsBrkNo4 }' and am0_brk_type='{lsBrkType}' and am0_ymd='{ am0YMD }' and am0_param_key='{ isParamKey}'").FirstOrDefault());
                        /* 沒有填0 */
                        if (foundIndex > -1) {
                           contentRow[colIndex++] = newDt.Rows[foundIndex]["qnty"].AsDecimal().AsString();
                           ldSum = dt.Compute("sum(qnty)", $@"am0_brk_no4='{lsBrkNo4}' and am0_brk_type='{lsBrkType}' and am0_ymd='{am0YMD }'").AsDecimal();
                        }
                        else {
                           contentRow[colIndex++] = "0";
                        }
                     }//foreach(DataRow pkRow in newdtPK.Rows)
                     if (isKindId2 == "%") {
                        contentRow[colIndex++] = ldSum.AsString();
                     }
                  }//foreach (DataRow ymdRow in dtYMD.Rows)
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
            throw new Exception("f_70010_ymd_w:" + ex.Message);
         }
      }
   }
}
