using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
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
      private readonly string _lsFile;
      private readonly string _emDateText;
      private readonly string _oswGrpVal;
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

      public string Wf40040()
      {
         try {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[(int)SheetName.SheetOne];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["O1"].Value = "資料日期：" + emdate.ToLongDateString();

            //前一交易日
            DateTime dateLast = dao40040.GetDateLast(emdate, (int)SheetName.SheetOne);

            //讀取資料
            string OswGrp = new OCFG().ListAll().AsEnumerable().FirstOrDefault()["OSW_GRP"].AsString();
            DataTable dt = dao40040.ListData(emdate, $"{OswGrp}%");
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

               worksheet.Cells[$"B{rowIndex}"].Value = row["MGT2_KIND_ID_OUT"].AsString();

               if (row["MG1_CHANGE_FLAG"].AsString() != "Y")
                  continue;

               //1.保證金變動幅度之趨勢/
               //15%≧X≧10%
               decimal mg1ChangeRange = row["MG1_CHANGE_RANGE"].AsDecimal();
               decimal Percent10 = row["MG1_PROD_SUBTYPE"].AsString() == "E" ? 0.05m : 0.1m;
               decimal Percent15 = row["MG1_PROD_SUBTYPE"].AsString() == "E" ? 0.1m : 0.15m;
               if (Math.Abs(mg1ChangeRange) >= Percent10 && Math.Abs(mg1ChangeRange) < Percent15) {
                  worksheet.Cells[$"C{rowIndex}"].SetValue(mg1ChangeRange);
               }
               //X≧15%
               if (Math.Abs(mg1ChangeRange) >= Percent15) {
                  worksheet.Cells[$"D{rowIndex}"].SetValue(mg1ChangeRange);
               }

               //前一交易日保證金變動幅度
               var ldValue = row["MG1_CHANGE_RANGE_LAST"];
               worksheet.Cells[$"E{rowIndex}"].SetValue(ldValue == DBNull.Value ? "▲" : ldValue);
               worksheet.Cells[$"F{rowIndex}"].SetValue(ldValue == DBNull.Value ? "▲" : OX(row));

               //達得調整標準天數
               string kindID = row["MG1_KIND_ID"].AsString();
               int dtDayIndex = dtDay.Rows.IndexOf(dtDay.Select($"mg1_kind_id ='{kindID}'").FirstOrDefault());
               worksheet.Cells[$"G{rowIndex}"].SetValue(dtDayIndex > -1 ? (long)dtDay.Rows[dtDayIndex]["DAY_CNT"].AsDecimal() : 1);

               //2.未沖銷部位數/
               var ai2OI = row["AI2_OI"];
               if (ai2OI != DBNull.Value) {
                  worksheet.Cells[$"H{rowIndex}"].SetValue(ai2OI);
                  var oiRate = row["OI_RATE"];
                  worksheet.Cells[$"I{rowIndex}"].SetValue(oiRate.AsDecimal() < 0.0001m && ai2OI.AsDecimal() > 0 ? "小於0.01%" : oiRate);
                  decimal TotOIiRound = Math.Round(dt.Rows[0]["TOT_OI"].AsDecimal() * 0.005m, 0, MidpointRounding.AwayFromZero);
                  worksheet.Cells[$"J{rowIndex}"].SetValue(ai2OI.AsDecimal() >= TotOIiRound ? "O" : "X");
                  //屆到期日前7個交易日
                  worksheet.Cells[$"K{rowIndex}"].SetValue(
                     row["APROD_7DATE"].AsDateTime() <= emdate && row["APROD_DELIVERY_DATE"].AsDateTime() > emdate ? "O" : "X"
                     );
               }

               

               //3.現貨、期貨漲跌/
               int dtMg6Index = dtMg6.Rows.IndexOf(dtMg6.Select($"f_kind_id ='{kindID}' or o_kind_id='{kindID}'").FirstOrDefault());
               if (dtMg6Index > -1) {
                  string colTxt = dtMg6.Rows[dtMg6Index]["O_KIND_ID"] == DBNull.Value ? "PDK" : "O";
                  string prodSubtype = dtMg6.Rows[dtMg6Index]["APDK_PROD_SUBTYPE"].AsString();
                  //現貨
                  var updown = dtMg6.Rows[dtMg6Index][colTxt + "_UP_DOWN"];
                  decimal rateMUL100 = dtMg6.Rows[dtMg6Index][colTxt + "_RETURN_RATE"].AsDecimal() * 100;
                  string flag = FlagStr(updown.AsDecimal());
                  worksheet.Cells[$"L{rowIndex}"].SetValue(updown == DBNull.Value ? "-" : WriteUpDownPercent(updown.AsDecimal(), rateMUL100, prodSubtype, flag));
                  //現貨漲跌與保證金調整方向相同
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

                  if (flag != "-") {
                     if (dtMg6.Rows[dtMg6Index]["MG1_CM_LAST"] == DBNull.Value) {
                        flag = "";
                     }
                     else {
                        flag = WriteFlag(flag, row);
                     }
                  } 
                  
                  worksheet.Cells[$"N{rowIndex}"].SetValue(flag);

                  //期貨 
                  var fupdown = dtMg6.Rows[dtMg6Index]["F_UP_DOWN"];
                  string flag2 = FlagStr(fupdown.AsDecimal());
                  decimal frateMUL100 = dtMg6.Rows[dtMg6Index]["F_RETURN_RATE"].AsDecimal() * 100;
                  worksheet.Cells[$"M{rowIndex}"].SetValue(WriteUpDownPercent(fupdown.AsDecimal(), frateMUL100, prodSubtype, flag2));
                  //期貨漲跌與保證金調整方向相同
                  var cmlast = dtMg6.Rows[dtMg6Index]["MG1_CM_LAST"];
                  worksheet.Cells[$"O{rowIndex}"].SetValue(cmlast == DBNull.Value ? "" : WriteFlag(flag2, row));

               }// if (dtMg6Index > -1)

               //4.與國外水準相較/


            }//foreach (DataRow row in dt.Rows)

            //重大事件
            //save
            //workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40040:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

   }
}
