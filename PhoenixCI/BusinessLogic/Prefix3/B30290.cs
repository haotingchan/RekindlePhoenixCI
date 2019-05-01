using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using System.IO;
using DevExpress.XtraEditors;
using DataObjects.Dao.Together;
using BusinessObjects;
using System.Linq;
/// <summary>
/// john,20190424,轉出交易系統部位限制檔及公告表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 轉出交易系統部位限制檔及公告表
   /// </summary>
   public class B30290
   {
      private D30290 dao30290;
      private readonly string _txnID;

      public B30290(string ProgramID)
      {
         dao30290 = new D30290();
         _txnID = ProgramID;
      }

      public DataTable GetEffectiveYMD(string emDateText)
      {
         return dao30290.ListEffectiveYmd(emDateText.Replace("/", ""));
      }

      public DataTable ListInsertGridData(string isCalYMD, string isYMD)
      {
         return dao30290.ListInsertData(isCalYMD.Replace("/", ""), isYMD);
      }

      public DataTable List30290GridData(string isYMD)
      {
         return dao30290.List30290Data(isYMD);
      }

      public int DataCount(string YMD)
      {
         return dao30290.PLP13ListCount(YMD);
      }

      public bool DeleteData(string isYMD)
      {
         return dao30290.DeletePLP13(isYMD);
      }

      public ResultData UpdateData(DataTable inputData)
      {
         return dao30290.UpdatePLP13(inputData);
      }

      /// <summary>
      /// //取得前一季月份
      /// </summary>
      /// <returns></returns>
      public string LastQuarter(DateTime ocfYMD)
      {
         DateTime dateTime = ocfYMD;
         //取得前一季月份
         int month = dateTime.Month;
         int num = 0;
         if (month % 3 > 0) {
            num = (month / 3) + 1;
         }
         else if ((month % 3) == 0) {
            num = month / 3;
         }

         DateTime ymd = new DateTime();

         if (num == 1) {
            num = 4;
            month = num * 3;
            ymd = new DateTime(dateTime.Year - 1, month, 1);
         }
         else {
            month = (num - 1) * 3;
            ymd = new DateTime(dateTime.Year, month, 1);
         }

         //取得前一季月底最後交易日
         DateTime maxDate = new AOCF().GetMaxDate(ymd.ToString("yyyyMM01"), ymd.ToString("yyyyMM31"));
         return maxDate.ToString("yyyy/MM/dd");
      }

      /// <summary>
      /// WfExport
      /// </summary>
      /// <returns></returns>
      public string WfExport(string lsFile, string SelDate, string emDateTxt)
      {
         string msg;
         try {
            msg = Wf30290gbf(lsFile, SelDate, emDateTxt);
            if (msg != MessageDisplay.MSG_OK) return msg;
            msg = Wf30290NStock(lsFile, SelDate, emDateTxt);
            if (msg != MessageDisplay.MSG_OK) return msg;
            msg = Wf30290Stock(lsFile, SelDate, emDateTxt);
            if (msg != MessageDisplay.MSG_OK) return msg;

            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("WfExport:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

      public string Wf30290gbf(string lsFile, string SelDate, string emDateTxt)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(lsFile);

            //讀取資料
            DataTable dt = dao30290.ListGBF(SelDate);
            if (dt.Rows.Count <= 0) {
               return $"{emDateTxt},30290－股價指數暨黃金類公告表,無任何資料!";
            }

            foreach (DataRow row in dt.Rows) {
               int rowIndex = row["C_SEQ_NO"].AsInt();

               string legalMth = row["PL2B_NATURE_LEGAL_MTH"].AsString();
               string legalTot = row["PL2B_NATURE_LEGAL_TOT"].AsString();
               string mth999 = row["PL2B_999_MTH"].AsString();
               string nearbyMth = row["PL2B_999_NEARBY_MTH"].AsString();
               string tot999 = row["PL2B_999_TOT"].AsString();
               //中文版
               workbook.Worksheets[0].Cells[$"C{rowIndex}"].Value = string.Format("單一月份{0:N0}，各月份合計{1:N0}", legalMth, legalTot);
               workbook.Worksheets[0].Cells[$"E{rowIndex}"].Value = string.Format("單一月份{0:N0}(最近到期月份{1:N0})，各月份合計{2:N0}", mth999, nearbyMth, tot999);
               //英文版
               workbook.Worksheets[1].Cells[$"B{rowIndex}"].Value = string.Format("{0:N0} contracts for any single month, and {1:N0} contracts for all months combined ", legalMth, legalTot);
               workbook.Worksheets[1].Cells[$"D{rowIndex}"].Value = string.Format("{0:N0} contracts for any single month({1:N0} contracts for nearest month), and {2:N0} contracts for all months combined ", mth999, nearbyMth, tot999);
            }
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      public string Wf30290NStock(string lsFile, string SelDate, string emDateTxt)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(lsFile);

            //讀取資料
            DataTable dt = dao30290.ListNStock(SelDate);
            if (dt.Rows.Count <= 0) {
               return $"{emDateTxt},30290－股價指數暨黃金類公告表,無任何資料!";
            }
            else {
               string date = dt.AsEnumerable().LastOrDefault()["PLP13_ISSUE_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
               workbook.Worksheets[0].Cells["B1"].Value = workbook.Worksheets[0].Cells["B1"].Value + date;
               workbook.Worksheets[1].Cells["A1"].Value = workbook.Worksheets[1].Cells["A1"].Value + date;
            }

            foreach (DataRow row in dt.Rows) {
               int rowIndex = row["C_SEQ_NO"].AsInt();

               var pl2Nature = row["PL2_NATURE"];
               var pl2Legal = row["PL2_LEGAL"];
               var pl2999 = row["PL2_999"];
               //中文版
               workbook.Worksheets[0].Cells[$"C{rowIndex}"].SetValue(pl2Nature);
               workbook.Worksheets[0].Cells[$"D{rowIndex}"].SetValue(pl2Legal);
               workbook.Worksheets[0].Cells[$"E{rowIndex}"].SetValue(pl2999);
               //英文版
               workbook.Worksheets[1].Cells[$"B{rowIndex}"].SetValue(pl2Nature);
               workbook.Worksheets[1].Cells[$"C{rowIndex}"].SetValue(pl2Legal);
               workbook.Worksheets[1].Cells[$"D{rowIndex}"].SetValue(pl2999);
            }
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      public string Wf30290Stock(string lsFile, string SelDate, string emDateTxt)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(lsFile);

            //讀取資料
            DataTable dt = dao30290.ListStock(SelDate);
            if (dt.Rows.Count <= 0) {
               return $"{emDateTxt},30290－個股類部位限制公告表,無任何資料!";
            }

            string date = dt.AsEnumerable().Select(dr => dr.Field<string>("PLP13_ISSUE_YMD")).Max().AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
            ChStock(workbook, dt, date);
            EngStock(workbook, dt, date);
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(lsFile);//save
         }
         return MessageDisplay.MSG_OK;
      }

      private static void ChStock(Workbook workbook, DataTable dt, string date)
      {
         Worksheet worksheet = workbook.Worksheets[2];
         worksheet.Cells["B1"].Value = worksheet.Cells["B1"].Value + date;

         int rowIndex = 4;
         foreach (DataRow row in dt.Rows) {
            rowIndex = rowIndex + 1;
            //提高報表中的降低
            var pls2Level = row["PLS2_LEVEL"];
            var pls2Nature = row["PLS2_NATURE"];
            var pls2Legal = row["PLS2_LEGAL"];
            var pls2999 = row["PLS2_999"];

            worksheet.Cells[$"A{rowIndex}"].SetValue(row["PLS2_KIND_ID2"].AsString());
            worksheet.Cells[$"B{rowIndex}"].SetValue(pls2Level);
            string str = row["APDK_NAME"].AsString();
            str = str.IndexOf("小型") >= 0 ? str.SubStr(str.IndexOf("小型") + 2, 99) : str;
            str = str.IndexOf("期貨") >= 0 ? str.SubStr(0, str.IndexOf("期貨")) : str;
            str = str.IndexOf("選擇權") >= 0 ? str.SubStr(0, str.IndexOf("選擇權")) : str;
            worksheet.Cells[$"C{rowIndex}"].SetValue(str);
            worksheet.Cells[$"D{rowIndex}"].SetValue(row["PLS2_SID"]);
            //大型
            if (row["PLS2_KIND_ID2"].AsString() == row["APDK_KIND_GRP2"].AsString()) {
               if (row["PLS2_FUT"].AsString() == "F") {
                  worksheet.Cells[$"E{rowIndex}"].SetValue("○");
               }
               if (row["PLS2_OPT"].AsString() == "O") {
                  worksheet.Cells[$"F{rowIndex}"].SetValue("○");
               }
               worksheet.Cells[$"G{rowIndex}"].SetValue(pls2Nature);
               worksheet.Cells[$"H{rowIndex}"].SetValue(pls2Legal);
               worksheet.Cells[$"I{rowIndex}"].SetValue(pls2999);
            }
            else {//小型
               if (row["PLS2_FUT"].AsString() == "F") {
                  worksheet.Cells[$"E{rowIndex}"].SetValue("◎");
                  str = string.Format("與{0}期貨合併計算(依20口小型{0}期貨等於1口{0}期貨合併計算)", str);
               }
               if (row["PLS2_OPT"].AsString() == "O") {
                  worksheet.Cells[$"F{rowIndex}"].SetValue("◎");
               }
               worksheet.Cells[$"G{rowIndex}"].SetValue(str);
               worksheet.Cells[$"G{rowIndex}"].Font.Size = 10;
               worksheet.Cells[$"G{rowIndex}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.General;
            }
         }
         //刪除空白列
         if (1004 > rowIndex) {
            worksheet.Rows.Remove(rowIndex, 1004 - rowIndex);
            worksheet.ScrollTo(0, 0);
         }
      }

      private static void EngStock(Workbook workbook, DataTable dt, string date)
      {
         Worksheet worksheet = workbook.Worksheets[3];
         worksheet.Cells["B1"].Value = worksheet.Cells["B1"].Value + date;

         int rowIndex = 2;
         foreach (DataRow row in dt.Rows) {
            rowIndex = rowIndex + 1;
            //提高報表中的降低
            var pls2Level = row["PLS2_LEVEL"];
            var pls2Nature = row["PLS2_NATURE"];
            var pls2Legal = row["PLS2_LEGAL"];
            var pls2999 = row["PLS2_999"];

            worksheet.Cells[$"A{rowIndex}"].SetValue(row["PLS2_KIND_ID2"].AsString());
            worksheet.Cells[$"B{rowIndex}"].SetValue(pls2Level);
            worksheet.Cells[$"C{rowIndex}"].SetValue(row["PLS2_SID"]);
            //大型
            if (row["PLS2_KIND_ID2"].AsString() == row["APDK_KIND_GRP2"].AsString()) {
               if (row["PLS2_FUT"].AsString() == "F") {
                  worksheet.Cells[$"D{rowIndex}"].SetValue("○");
               }
               if (row["PLS2_OPT"].AsString() == "O") {
                  worksheet.Cells[$"E{rowIndex}"].SetValue("○");
               }
               worksheet.Cells[$"F{rowIndex}"].SetValue(pls2Nature);
               worksheet.Cells[$"G{rowIndex}"].SetValue(pls2Legal);
               worksheet.Cells[$"H{rowIndex}"].SetValue(pls2999);
            }
            else {//小型
               if (row["PLS2_FUT"].AsString() == "F") {
                  worksheet.Cells[$"D{rowIndex}"].SetValue("◎");
                  worksheet.Cells[$"F{rowIndex}"].Value = $"Combined with the calculation of {row["APDK_KIND_GRP2"].AsString()}F  position limit (on a pro rata basis of 20:1 contract size)";
               }
               if (row["PLS2_OPT"].AsString() == "O") {
                  worksheet.Cells[$"E{rowIndex}"].SetValue("◎");
               }
               worksheet.Cells[$"F{rowIndex}"].Font.Size = 8;
               worksheet.Cells[$"F{rowIndex}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.General;
            }
         }
         //刪除空白列
         if (1002 > rowIndex) {
            worksheet.Rows.Remove(rowIndex, 1002 - rowIndex);
            worksheet.ScrollTo(0, 0);
         }
      }


   }
}