using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
/// <summary>
/// 20190417,john,指數類期貨及現貨資料下載
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 指數類期貨及現貨資料下載
   /// </summary>
   public class B40040
   {
      /// <summary>
      /// 檔案輸出路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 交易日期
      /// </summary>
      private readonly string _emDateText;
      /// <summary>
      /// 交易時段
      /// </summary>
      private readonly string _oswGrpVal;
      /// <summary>
      /// DataLayer
      /// </summary>
      private readonly D40040 dao40040;

      public B40040(string FilePath, string emDate, string oswGrpLookItemVal)
      {
         _lsFile = FilePath;
         _oswGrpVal = oswGrpLookItemVal;
         _emDateText = emDate;
         dao40040 = new D40040();
      }

      public enum SheetName : int
      {
         /// <summary>
         /// 保證金調整檢核表
         /// </summary>
         SheetOne = 0,
         /// <summary>
         /// 保證金調整檢核表(ETF_ETC類)
         /// </summary>
         SheetTwo = 1,
         /// <summary>
         /// SPAN
         /// </summary>
         SheetSPAN = 2
      }

      private string OX(DataRow dr)
      {
         if (Math.Abs(dr["MG1_CHANGE_RANGE_LAST"].AsDecimal()) < 0.1m
            && Math.Abs(dr["MG1_CHANGE_RANGE"].AsDecimal()) > 0.15m) {
            return "O";
         }
         return "X";
      }

      private string FormatStr(decimal ldVal, string prodSubtype)
      {
         string isFormat = "";
         if (Math.Abs(ldVal % 1) > 0) {
            isFormat = prodSubtype == "E" ? "#0.0###" : "#0.0##";
         }
         else {
            isFormat = "#,##0";
         }
         return isFormat;
      }

      private string FlagStr(decimal ldVal)
      {
         string isFlag = "";
         if (ldVal == 0) {
            isFlag = "0";
         }
         else if (ldVal > 0) {
            isFlag = "+";
         }
         else {
            isFlag = "";
         }
         return isFlag;
      }

      /// <summary>
      /// 寫入 現貨、期貨漲跌幅度
      /// </summary>
      /// <returns></returns>
      private string WriteUpDownPercent(decimal ldVal, decimal mulVal, string prodSubtype, string isflag)
      {
         StringBuilder txt = new StringBuilder();

         #region _up_down
         string isformat = FormatStr(ldVal, prodSubtype);

         txt.Append(isflag != "0" ? isflag : "");
         txt.Append(ldVal.ToString(isformat));//ls_txt = ls_txt + string(ld_value,ls_format) 
         #endregion

         #region _return_rate
         decimal rateVal = mulVal;//ld_value = lds_idxf.getitemdecimal(ll_found,ls_col+"_return_rate")*100
         if (isflag == "" && mulVal > 0) {
            rateVal = mulVal * (-1);
         }
         string rateFormat = FormatStr(rateVal, prodSubtype);

         txt.Append("(");
         if (isflag != "0")
            txt.Append(isflag);

         txt.Append($"{rateVal.ToString(rateFormat)}%)");
         #endregion

         return txt.ToString();
      }

      /// <summary>
      /// 寫入 期貨漲跌與保證金調整方向
      /// </summary>
      /// <param name="flag"></param>
      /// <param name="dr"></param>
      /// <returns></returns>
      private string WriteFlag(string flag, DataRow dr)
      {
         string isflag = "";
         decimal cm = dr["MG1_CM"].AsDecimal();
         decimal cmlast = dr["MG1_CM_LAST"].AsDecimal();

         if (flag == "0") {
            isflag = "-";
         }
         else if (cm - cmlast < 0 && flag == "") {
            isflag = "O";
         }
         else if (cm - cmlast > 0 && flag == "+") {
            isflag = "O";
         }
         else {
            isflag = "X";
         }

         return isflag;
      }

      private string Foreign(string kindID, string valueType, DataTable dtMg8, int dtMg8Index)
      {
         string col = $"CP_{valueType}_RATE";
         decimal isValue = dtMg8.Rows[dtMg8Index][col].AsDecimal();

         string fKindId = "";
         switch (kindID) {
            case "TGF":
            case "GDF":
               fKindId = "GDF";
               break;
            case "TXF":
               fKindId = "TXF";
               break;
            case "RTF":
            case "RHF":
               fKindId = "RHF";
               break;
            case "TJF":
               fKindId = "TJF";
               break;
            default:
               fKindId = kindID;
               break;
         }

         StringBuilder sb = new StringBuilder("");

         //大於cp_aft_rate
         DataTable dtHigher = dtMg8.Filter("cp_aft_rate <" + isValue + " and data_type='2' and mg1_kind_id='" + fKindId + "'");

         int higherCount = dtHigher.Rows.Count;
         if (higherCount > 0) {
            sb.Append("較");
            sb = ForeignStrMerge(sb, dtHigher, higherCount);
            sb.Append("高");
         }

         //小於cp_aft_rate
         DataTable dtLower = dtMg8.Filter("cp_aft_rate >" + isValue + " and data_type='2' and mg1_kind_id='" + fKindId + "'");
         int lowerCount = dtLower.Rows.Count;
         if (lowerCount > 0) {
            if (sb.ToString() != "") {
               sb.Append("，");
            }
            sb.Append("較");
            sb = ForeignStrMerge(sb, dtLower, lowerCount);
            sb.Append("低");
         }

         return sb.ToString();
      }

      private StringBuilder ForeignStrMerge(StringBuilder sb, DataTable dt, int count)
      {
         for (int k = 0; k < count; k++) {
            DataRow dr = dt.Rows[k];
            sb.Append(dr["F_NAME"].AsString());
            sb.Append(FlagMerge(count, k));
         }
         return sb;
      }

      private string FlagMerge(int count, int k)
      {
         string txt = "";
         if (k == count - 2) {
            txt = "及";
         }
         else if (k != count - 1) {
            txt = "、";
         }
         return txt;
      }

      /// <summary>
      /// 保證金調整檢核表
      /// </summary>
      /// <returns></returns>
      public string WfSheetOne()
      {
         Workbook workbook = new Workbook();
         try {
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[(int)SheetName.SheetOne];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["N1"].Value = "資料日期：" + emdate.ToLongDateString();

            //讀取資料
            DataTable dtNotETF = dao40040.List40040SP(emdate, _oswGrpVal);
            //dt.Filter("ISNULL(PDK_PARAM_KEY,'') not in ('ETF','ETC')").Sort("RPT_SEQ_NO, DATA_KIND_ID");
            //DataTable dt = dtNotETF.AsEnumerable().Where(r => r.Field<object>("PDK_PARAM_KEY").AsString()!="ETF" && r.Field<object>("PDK_PARAM_KEY").AsString() != "ETC").CopyToDataTable().Sort("RPT_SEQ_NO, DATA_KIND_ID");
            DataTable dt = dtNotETF.Filter("ISNULL(PDK_PARAM_KEY,'') not in ('ETF','ETC')").Sort("RPT_SEQ_NO, DATA_KIND_ID");
            if (dt.Rows.Count <= 0) {
               return MessageDisplay.MSG_NO_DATA;
            }

            //本日檢核結果表
            int rowIndex = 7;
            foreach (DataRow row in dt.Rows) {
               //判斷3個模組是否皆為達標
               bool changeFlag = row["SMA_CHANGE_FLAG"].AsString() == "Y" ||
                  row["EWMA_CHANGE_FLAG"].AsString() == "Y" ||
                  row["MAXV_CHANGE_FLAG"].AsString() == "Y";
               //若碰上3個模組的flag皆不為Y，2、3、4項下的內容文字為白色
               if (!changeFlag) {
                  worksheet.Range[$"I{rowIndex}:S{rowIndex}"].Font.Color = Color.White;
               }

               //商品
               worksheet.Cells[$"B{rowIndex}"].SetValue(row["DATA_KIND_ID"]);

               #region 1.各項指標計算結算保證金變動幅度
               if (changeFlag) {
                  //1.簡單移動平均法(SMA)
                  worksheet.Cells[$"C{rowIndex}"].SetValue(row["SMA_CHANGE_RANGE"]);
                  worksheet.Cells[$"D{rowIndex}"].SetValue(row["SMA_DAY_CNT"]);
                  //2.加權指數移動平均法(EWMA)
                  worksheet.Cells[$"E{rowIndex}"].SetValue(row["EWMA_CHANGE_RANGE"]);
                  worksheet.Cells[$"F{rowIndex}"].SetValue(row["EWMA_DAY_CNT"]);
                  //3.簡單移動平均法
                  worksheet.Cells[$"G{rowIndex}"].SetValue(row["MAXV_CHANGE_RANGE"]);
                  worksheet.Cells[$"H{rowIndex}"].SetValue(row["MAXV_DAY_CNT"]);
               }
               #endregion

               #region 2.未沖銷部位數
               //當日未沖銷部位數及比例
               worksheet.Cells[$"I{rowIndex}"].SetValue(row["OI"]);
               worksheet.Cells[$"J{rowIndex}"].SetValue(row["OI_RATE"]);
               //屆到期日前7個交易日
               worksheet.Cells[$"K{rowIndex}"].SetValue(row["DELIVERY_FLAG"]);
               #endregion

               #region 3.現貨、期貨漲跌
               //現貨 漲跌(比例 %) (註1)
               var updown = row["SPOT_UP_DOWN"];
               decimal rateMUL100 = row["SPOT_RATE"].AsDecimal() * 100;
               string flag = FlagStr(updown.AsDecimal());
               worksheet.Cells[$"L{rowIndex}"].SetValue(updown == DBNull.Value ? "-" : WriteUpDownPercent(updown.AsDecimal(), rateMUL100, "E", flag));
               //期貨 漲跌(比例 %)
               var fupdown = row["FUT_UP_DOWN"];
               string flag2 = FlagStr(fupdown.AsDecimal());
               decimal frateMUL100 = row["FUT_RATE"].AsDecimal() * 100;
               worksheet.Cells[$"M{rowIndex}"].SetValue(WriteUpDownPercent(fupdown.AsDecimal(), frateMUL100, "E", flag2));
               #endregion

               #region 4.與國外水準相較
               //原始保證金占本日契約價值比率
               worksheet.Cells[$"N{rowIndex}"].SetValue(row["CUR_IM_RATE"]);
               if (changeFlag) {
                  worksheet.Cells[$"O{rowIndex}"].SetValue(row["SMA_ADJ_IM_RATE"]);//調整後原始保證金占比SMA
                  worksheet.Cells[$"P{rowIndex}"].SetValue(row["EWMA_ADJ_IM_RATE"]);//調整後原始保證金占比EWMA
                  worksheet.Cells[$"Q{rowIndex}"].SetValue(row["MAXV_ADJ_IM_RATE"]);//調整後原始保證金占比MAXV
               }
               worksheet.Cells[$"R{rowIndex}"].SetValue(row["F_RATE"]);
               worksheet.Cells[$"S{rowIndex}"].SetValue(row["F_EXCHANGE"]);
               #endregion

               rowIndex++;//換下一行
            }
            //save
            worksheet.ScrollTo(0, 0);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WfSheetOne:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 保證金調整檢核表(ETF_ETC類)
      /// </summary>
      /// <returns></returns>
      public string WfSheetTwo()
      {
         Workbook workbook = new Workbook();
         try {
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[(int)SheetName.SheetTwo];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["P1"].Value = "資料日期：" + emdate.ToLongDateString();

            //讀取資料
            DataTable dtETF = dao40040.List40040SP(emdate, _oswGrpVal);
            //dt.Filter("PDK_PARAM_KEY in ('ETF','ETC')").Sort("RPT_SEQ_NO, DATA_KIND_ID");
            DataTable dt = dtETF.Filter("PDK_PARAM_KEY in ('ETF','ETC')").Sort("RPT_SEQ_NO, DATA_KIND_ID");
            if (dt.Rows.Count <= 0) {
               return MessageDisplay.MSG_NO_DATA;
            }

            //本日檢核結果表
            int rowIndex = 7;
            foreach (DataRow row in dt.Rows) {
               //序號
               worksheet.Cells[$"B{rowIndex}"].SetValue(rowIndex - 6);
               //股票期貨英文代碼
               worksheet.Cells[$"C{rowIndex}"].SetValue(row["DATA_KIND_ID"]);
               //股票期貨中文簡稱
               worksheet.Cells[$"D{rowIndex}"].SetValue(row["PDK_NAME"]);
               //股票期貨標的證券代號
               worksheet.Cells[$"E{rowIndex}"].SetValue(row["PDK_STOCK_ID"]);
               //上市/上櫃類別
               worksheet.Cells[$"F{rowIndex}"].SetValue(row["PID_NAME"]);

               #region 1.各項指標計算結算保證金變動幅度
               if (row["SMA_CHANGE_FLAG"].AsString() == "Y" ||
                  row["EWMA_CHANGE_FLAG"].AsString() == "Y" ||
                  row["MAXV_CHANGE_FLAG"].AsString() == "Y") {
                  //1.簡單移動平均法(SMA)
                  worksheet.Cells[$"G{rowIndex}"].SetValue(row["SMA_CHANGE_RANGE"]);
                  worksheet.Cells[$"H{rowIndex}"].SetValue(row["SMA_DAY_CNT"]);
                  //2.加權指數移動平均法(EWMA)
                  worksheet.Cells[$"I{rowIndex}"].SetValue(row["EWMA_CHANGE_RANGE"]);
                  worksheet.Cells[$"J{rowIndex}"].SetValue(row["EWMA_DAY_CNT"]);
                  //3.簡單移動平均法
                  worksheet.Cells[$"K{rowIndex}"].SetValue(row["MAXV_CHANGE_RANGE"]);
                  worksheet.Cells[$"L{rowIndex}"].SetValue(row["MAXV_DAY_CNT"]);
               }
               #endregion

               //若碰上3個模組的flag不為Y，2、3項下的內容文字為白色
               if (row["SMA_CHANGE_FLAG"].AsString() != "Y" &&
                  row["EWMA_CHANGE_FLAG"].AsString() != "Y" &&
                  row["MAXV_CHANGE_FLAG"].AsString() != "Y") {
                  worksheet.Range[$"M{rowIndex}:Q{rowIndex}"].Font.Color = Color.White;
               }

               #region 2.未沖銷部位數
               //當日未沖銷部位數及比例
               worksheet.Cells[$"M{rowIndex}"].SetValue(row["OI"]);
               worksheet.Cells[$"N{rowIndex}"].SetValue(row["OI_RATE"]);
               //屆到期日前7個交易日
               worksheet.Cells[$"O{rowIndex}"].SetValue(row["DELIVERY_FLAG"]);
               #endregion

               #region 3.現貨、期貨漲跌
               //現貨 漲跌(比例 %) (註1)
               var updown = row["SPOT_UP_DOWN"];
               decimal rateMUL100 = row["SPOT_RATE"].AsDecimal() * 100;
               string flag = FlagStr(updown.AsDecimal());
               worksheet.Cells[$"P{rowIndex}"].SetValue(updown == DBNull.Value ? "-" : WriteUpDownPercent(updown.AsDecimal(), rateMUL100, "E", flag));
               //期貨 漲跌(比例 %)
               var fupdown = row["FUT_UP_DOWN"];
               string flag2 = FlagStr(fupdown.AsDecimal());
               decimal frateMUL100 = row["FUT_RATE"].AsDecimal() * 100;
               worksheet.Cells[$"Q{rowIndex}"].SetValue(WriteUpDownPercent(fupdown.AsDecimal(), frateMUL100, "E", flag2));
               #endregion

               rowIndex++;
            }
            //save
            worksheet.ScrollTo(0, 0);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WfSheetOne:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// sheet 3
      /// </summary>
      /// <returns></returns>
      public string Wf40040SPAN()
      {
         Workbook workbook = new Workbook();
         try {
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[(int)SheetName.SheetSPAN];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["M2"].Value = emdate.ToLongDateString();
            DateTime startDate = worksheet.Cells["K2"].Value.AsDateTime();

            //1.本日波動度偵測全距(VSR)變動狀況
            //讀取資料
            DataTable dtSV = dao40040.ListSpanSvData(emdate, startDate, $"{_oswGrpVal}%");
            worksheet.Import(dtSV, false, 5, 2);

            //2.本日契約價值耗用比率(Delta Per Spread Ratio )變動狀況
            //讀取資料
            DataTable dtSD = dao40040.ListSpanSdData(emdate, startDate, $"{_oswGrpVal}%");
            foreach (DataRow row in dtSD.Rows) {
               int rowIndex = row["RPT_ROW"].AsInt() - 1;
               int colIndex = row["RPT_COL"].AsInt() - 1;
               if (row["SP1_CHANGE_FLAG"].AsString() == "Y") {
                  worksheet.Rows[rowIndex][colIndex].SetValue(row["SP1_CHANGE_RANGE"]);
                  worksheet.Rows[rowIndex][colIndex + 1].SetValue(row["DAY_CNT"]);
               }
            }

            worksheet.ScrollTo(0, 0);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40040SPAN:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);//save
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 7/2上線前的版本 可供新需求的邏輯參考
      /// </summary>
      /// <returns></returns>
      public string Wf40040()
      {
         Workbook workbook = new Workbook();
         try {
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[(int)SheetName.SheetOne];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["O1"].Value = "資料日期：" + emdate.ToLongDateString();

            //前一交易日
            DateTime dateLast = dao40040.GetDateLast(emdate, (int)SheetName.SheetOne);

            //讀取資料
            DataTable dt = dao40040.ListData(emdate, $"{_oswGrpVal}%");
            if (dt.Rows.Count <= 0) {
               return MessageDisplay.MSG_NO_DATA;
            }

            //讀取資料(現貨資料)
            DataTable dtMg6 = dao40040.ListMg6Data(emdate, dateLast, "%");

            //讀取Mg8資料
            DataTable dtMg8 = dao40040.ListMg8Data(emdate, "%");

            //保證金變動幅度達10%，分別為第n天
            DataTable dtDay = dao40040.ListDayData(emdate);

            foreach (DataRow row in dt.Rows) {
               int rowIndex = row["RPT_SEQ_NO"].AsInt();
               if (rowIndex <= 0)
                  continue;

               #region 第B欄
               worksheet.Cells[$"B{rowIndex}"].Value = row["MGT2_KIND_ID_OUT"].AsString();
               #endregion

               if (row["MG1_CHANGE_FLAG"].AsString() != "Y")
                  continue;

               //1.保證金變動幅度之趨勢/
               //15%≧X≧10%
               decimal mg1ChangeRange = row["MG1_CHANGE_RANGE"].AsDecimal();
               decimal Percent10 = row["MG1_PROD_SUBTYPE"].AsString() == "E" ? 0.05m : 0.1m;
               decimal Percent15 = row["MG1_PROD_SUBTYPE"].AsString() == "E" ? 0.1m : 0.15m;
               #region 第C欄
               if (Math.Abs(mg1ChangeRange) >= Percent10 && Math.Abs(mg1ChangeRange) < Percent15) {
                  worksheet.Cells[$"C{rowIndex}"].SetValue(mg1ChangeRange);
               }
               #endregion
               //X≧15%
               #region 第D欄
               if (Math.Abs(mg1ChangeRange) >= Percent15) {
                  worksheet.Cells[$"D{rowIndex}"].SetValue(mg1ChangeRange);
               }
               #endregion

               //前一交易日保證金變動幅度
               var ldValue = row["MG1_CHANGE_RANGE_LAST"];
               #region 第E欄
               worksheet.Cells[$"E{rowIndex}"].SetValue(ldValue == DBNull.Value ? "▲" : ldValue);
               #endregion
               #region 第F欄
               worksheet.Cells[$"F{rowIndex}"].SetValue(ldValue == DBNull.Value ? "▲" : OX(row));
               #endregion

               //達得調整標準天數
               string kindID = row["MG1_KIND_ID"].AsString();
               #region 第G欄
               int dtDayIndex = dtDay.Rows.IndexOf(dtDay.Select($@"mg1_kind_id ='{kindID}'").FirstOrDefault());
               worksheet.Cells[$"G{rowIndex}"].SetValue(dtDayIndex > -1 ? (long)dtDay.Rows[dtDayIndex]["DAY_CNT"].AsDecimal() : 1);
               #endregion

               //2.未沖銷部位數/
               var ai2OI = row["AI2_OI"];
               if (ai2OI != DBNull.Value) {
                  #region 第H欄
                  worksheet.Cells[$"H{rowIndex}"].SetValue(ai2OI);
                  #endregion
                  #region 第I欄
                  var oiRate = row["OI_RATE"];
                  worksheet.Cells[$"I{rowIndex}"].SetValue(oiRate.AsDecimal() < 0.0001m && ai2OI.AsDecimal() > 0 ? "小於0.01%" : oiRate);
                  #endregion
                  #region 第J欄
                  decimal TotOIiRound = Math.Round(dt.Rows[0]["TOT_OI"].AsDecimal() * 0.005m, 0, MidpointRounding.AwayFromZero);
                  worksheet.Cells[$"J{rowIndex}"].SetValue(ai2OI.AsDecimal() >= TotOIiRound ? "O" : "X");
                  #endregion
                  //屆到期日前7個交易日
                  #region 第K欄
                  worksheet.Cells[$"K{rowIndex}"].SetValue(
                                 row["APROD_7DATE"].AsDateTime() <= emdate && row["APROD_DELIVERY_DATE"].AsDateTime() > emdate ? "O" : "X"
                                 );
                  #endregion
               }

               //3.現貨、期貨漲跌/
               int dtMg6Index = dtMg6.Rows.IndexOf(dtMg6.Select($@"F_KIND_ID ='{kindID}' or O_KIND_ID='{kindID}'").FirstOrDefault());
               if (dtMg6Index > -1) {
                  string colTxt = dtMg6.Rows[dtMg6Index]["O_KIND_ID"] == DBNull.Value ? "PDK" : "O";
                  string prodSubtype = dtMg6.Rows[dtMg6Index]["APDK_PROD_SUBTYPE"].AsString();
                  //現貨
                  #region 第L欄
                  var updown = dtMg6.Rows[dtMg6Index][colTxt + "_UP_DOWN"];
                  decimal rateMUL100 = dtMg6.Rows[dtMg6Index][colTxt + "_RETURN_RATE"].AsDecimal() * 100;
                  string flag = FlagStr(updown.AsDecimal());
                  worksheet.Cells[$"L{rowIndex}"].SetValue(updown == DBNull.Value ? "-" : WriteUpDownPercent(updown.AsDecimal(), rateMUL100, prodSubtype, flag));
                  #endregion
                  //現貨漲跌與保證金調整方向相同
                  #region 第N欄
                  switch (kindID) {
                     case "GDF":
                     case "TGF":
                     case "TGO":
                     case "GBF":
                     case "CPF":
                        flag = "-";
                        break;
                     default:
                        break;
                  }

                  if (flag == "-") {
                     //ls_flag = '-'  then 後面沒有要做什麼 只是做個條件區分
                  }
                  else if (row["MG1_CM_LAST"] == DBNull.Value) {
                     flag = "";
                  }
                  else {
                     flag = WriteFlag(flag, row);
                  }

                  worksheet.Cells[$"N{rowIndex}"].SetValue(flag);
                  #endregion

                  //期貨 
                  #region 第M欄
                  var fupdown = dtMg6.Rows[dtMg6Index]["F_UP_DOWN"];
                  string flag2 = FlagStr(fupdown.AsDecimal());
                  decimal frateMUL100 = dtMg6.Rows[dtMg6Index]["F_RETURN_RATE"].AsDecimal() * 100;
                  worksheet.Cells[$"M{rowIndex}"].SetValue(WriteUpDownPercent(fupdown.AsDecimal(), frateMUL100, prodSubtype, flag2));
                  #endregion
                  //期貨漲跌與保證金調整方向相同
                  #region 第O欄
                  var cmlast = row["MG1_CM_LAST"];
                  worksheet.Cells[$"O{rowIndex}"].SetValue(cmlast == DBNull.Value ? "" : WriteFlag(flag2, row));
                  #endregion

               }// if (dtMg6Index > -1)

               //4.與國外水準相較/
               int dtMg8Index = dtMg8.Rows.IndexOf(dtMg8.Select($"mg1_kind_id ='{kindID}' and com ='TAIFEX'").FirstOrDefault());
               if (dtMg8Index > -1) {
                  #region 第P欄
                  worksheet.Cells[$"P{rowIndex}"].SetValue(Foreign(kindID, "BEF", dtMg8, dtMg8Index));
                  #endregion
                  #region 第Q欄
                  worksheet.Cells[$"Q{rowIndex}"].SetValue(Foreign(kindID, "AFT", dtMg8, dtMg8Index));
                  #endregion
               }
            }//foreach (DataRow row in dt.Rows)

            //重大事件
            StringBuilder sb = new StringBuilder("");
            DataTable dtMgt3 = dao40040.ListMgt3Data(emdate);
            int mgt3Count = dtMgt3.Rows.Count;
            for (int k = 0; k < mgt3Count; k++) {
               DataRow dr = dtMgt3.Rows[k];
               string memo = PbFunc.f_conv_date(dr["MGT3_DATE_TO"].AsDateTime(), 3) + dr["MGT3_MEMO"].AsString();
               sb.Append(memo);
               sb.Append(FlagMerge(mgt3Count, k));
            }

            if (sb.ToString() != "") {
               int mg1flagYcount = dt.Select("mg1_change_flag = 'Y'").Length;
               if (mg1flagYcount > 0)
                  worksheet.Cells[$"R16"].SetValue(sb.ToString());
            }
            //save
            worksheet.ScrollTo(0, 0);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40040:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// 7/2上線前的版本 可供新需求的邏輯參考
      /// </summary>
      /// <returns></returns>
      public string Wf40040ETF()
      {
         Workbook workbook = new Workbook();
         try {
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[(int)SheetName.SheetTwo];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["P1"].Value = "資料日期：" + emdate.ToLongDateString();

            //前一交易日
            DateTime dateLast = dao40040.GetDateLast(emdate, (int)SheetName.SheetTwo);

            //讀取資料
            DataTable dtETF = dao40040.ListEtfData(emdate, dateLast, $"{_oswGrpVal}%");
            if (dtETF.Rows.Count <= 0) {
               return MessageDisplay.MSG_NO_DATA;
            }

            //讀取資料(現貨資料)
            DataTable dtEtfMg6 = dao40040.ListEtfMg6Data(emdate);

            //保證金變動幅度達10%，分別為第n天
            DataTable dtDay = dao40040.ListDayData(emdate);

            foreach (DataRow row in dtETF.Rows) {
               int rowIndex = row["SN"].AsInt() + 10;

               #region 第B、C、D、E、F欄
               worksheet.Cells[$"B{rowIndex}"].Value = row["SN"].AsInt();
               worksheet.Cells[$"C{rowIndex}"].Value = row["MGT2_KIND_ID_OUT"].AsString();
               worksheet.Cells[$"D{rowIndex}"].Value = row["APDK_NAME"].AsString();
               worksheet.Cells[$"E{rowIndex}"].Value = row["APDK_STOCK_ID"].AsString();
               worksheet.Cells[$"F{rowIndex}"].Value = row["PID_NAME"].AsString();
               #endregion

               if (row["MG1_CHANGE_FLAG"].AsString() != "Y")
                  continue;

               //1.保證金變動幅度之趨勢/
               //15%≧X≧10%
               decimal mg1ChangeRange = row["MG1_CHANGE_RANGE"].AsDecimal();
               #region 第G欄
               if (Math.Abs(mg1ChangeRange) >= 0.1m && Math.Abs(mg1ChangeRange) <= 0.15m) {
                  worksheet.Cells[$"G{rowIndex}"].SetValue(mg1ChangeRange);
               }
               #endregion
               //X>15%
               #region 第H欄
               if (Math.Abs(mg1ChangeRange) > 0.15m) {
                  worksheet.Cells[$"H{rowIndex}"].SetValue(mg1ChangeRange);
               }
               #endregion

               //前一交易日保證金變動幅度
               var ldValue = row["MG1_CHANGE_RANGE_LAST"];
               #region 第I欄
               worksheet.Cells[$"I{rowIndex}"].SetValue(ldValue == DBNull.Value ? " " : ldValue);
               #endregion
               #region 第J欄
               worksheet.Cells[$"J{rowIndex}"].SetValue(ldValue == DBNull.Value ? " " : OX(row));
               #endregion

               //達得調整標準天數
               string kindID = row["MG1_KIND_ID"].AsString();
               #region 第K欄
               int dtDayIndex = dtDay.Rows.IndexOf(dtDay.Select($@"mg1_kind_id ='{kindID}'").FirstOrDefault());
               worksheet.Cells[$"K{rowIndex}"].SetValue(dtDayIndex > -1 ? (long)dtDay.Rows[dtDayIndex]["DAY_CNT"].AsDecimal() : 1);
               #endregion

               //2.未沖銷部位數/
               var ai2OI = row["AI2_OI"];
               if (ai2OI != DBNull.Value) {
                  #region 第L欄
                  worksheet.Cells[$"L{rowIndex}"].SetValue(ai2OI);
                  #endregion
                  #region 第M欄
                  var oiRate = row["OI_RATE"];
                  worksheet.Cells[$"M{rowIndex}"].SetValue(oiRate.AsDecimal() < 0.0001m && ai2OI.AsDecimal() > 0 ? "小於0.01%" : oiRate);
                  #endregion
                  #region 第N欄
                  decimal TotOIiRound = Math.Round(dtETF.Rows[0]["TOT_OI"].AsDecimal() * 0.005m, 0, MidpointRounding.AwayFromZero);
                  worksheet.Cells[$"N{rowIndex}"].SetValue(ai2OI.AsDecimal() >= TotOIiRound ? "O" : "X");
                  #endregion
                  //屆到期日前7個交易日
                  #region 第O欄
                  worksheet.Cells[$"O{rowIndex}"].SetValue(
                                 row["APROD_7DATE"].AsDateTime() <= emdate && row["APROD_DELIVERY_DATE"].AsDateTime() > emdate ? "O" : "X"
                                 );
                  #endregion
               }

               //3.現貨、期貨漲跌/
               int dtMg6EtfIndex = dtEtfMg6.Rows.IndexOf(dtEtfMg6.Select($@"trim(mg6_kind_id) ='{kindID}'").FirstOrDefault());
               if (dtMg6EtfIndex > -1) {
                  string prodSubtype = "";
                  //現貨漲跌幅度
                  #region 第P欄
                  var updown = dtEtfMg6.Rows[dtMg6EtfIndex]["MG6_UP_DOWN"];
                  decimal rateMUL100 = dtEtfMg6.Rows[dtMg6EtfIndex]["MG6_RETURN_RATE"].AsDecimal() * 100;
                  string flag = FlagStr(updown.AsDecimal());
                  worksheet.Cells[$"P{rowIndex}"].SetValue(updown == DBNull.Value ? updown : WriteUpDownPercent(updown.AsDecimal(), rateMUL100, prodSubtype, flag));
                  #endregion
                  //現貨漲跌與保證金調整方向相同
                  var cmlast = row["MG1_CM_LAST"];
                  #region 第R欄
                  worksheet.Cells[$"R{rowIndex}"].SetValue(cmlast == DBNull.Value ? "" : WriteFlag(flag, row));
                  #endregion

                  //期貨漲跌幅度
                  if (row["APDK_PROD_TYPE"].AsString() == "O") {
                     dtMg6EtfIndex = dtEtfMg6.Rows.IndexOf(dtEtfMg6.Select($@"apdk_stock_id = '{row["APDK_STOCK_ID"].AsString()}' and mg6_prod_type ='F' and kind_seq_no = 1").FirstOrDefault());
                     if (dtMg6EtfIndex <= -1)
                        continue;
                  }
                  #region 第Q欄
                  var fupdown = dtEtfMg6.Rows[dtMg6EtfIndex]["AI5_UP_DOWN"];
                  string flag2 = FlagStr(fupdown.AsDecimal());
                  decimal frateMUL100 = dtEtfMg6.Rows[dtMg6EtfIndex]["AI5_RETURN_RATE"].AsDecimal() * 100;
                  worksheet.Cells[$"Q{rowIndex}"].SetValue(WriteUpDownPercent(fupdown.AsDecimal(), frateMUL100, prodSubtype, flag2));
                  #endregion
                  //期貨漲跌與保證金調整方向相同
                  #region 第S欄
                  worksheet.Cells[$"S{rowIndex}"].SetValue(cmlast == DBNull.Value ? "" : WriteFlag(flag2, row));
                  #endregion

               }// if (dtMg6EtfIndex > -1)

            }//foreach (DataRow row in dt.Rows)

            //刪除空白列
            int rowCount = dtETF.Rows.Count;
            if (50 > rowCount) {
               worksheet.Rows.Hide(rowCount + 10, 60 - 1);
            }
            //save
            worksheet.ScrollTo(0, 0);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40040ETF:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }




   }
}
