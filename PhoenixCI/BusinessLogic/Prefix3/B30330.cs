using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190221,john,十年期公債期貨契約價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 十年期公債期貨契約價量資料
   /// </summary>
   public class B30330
   {
      private AI3 daoAI3;
      private readonly string _lsFile;
      private string _emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30330(string FilePath, string datetime)
      {
         daoAI3 = new AI3();
         _lsFile = FilePath;
         _emMonthText = datetime;
      }
      /// <summary>
      /// 重新選取圖表資料範圍
      /// </summary>
      /// <param name="RowIndex">選取到第幾列</param>
      /// <param name="chartName">圖表sheet名稱</param>
      private static void ResetChartData(int RowIndex, Workbook workbook, Worksheet worksheet, string chartName)
      {
         //公債期貨總成交量/公債期貨總未平倉量/公債期貨價格
         string[] data = new string[] { "D4:D", "E4:E", $@"B4:B" };
         int count = 0;
         foreach (var item in data) {
            workbook.ChartSheets[chartName].Chart.Series[count++].Values = new ChartData {
               RangeValue = worksheet.Range[item + RowIndex.ToString()]
            };
         }
      }
      /// <summary>
      /// 寫入 30331 sheet
      /// </summary>
      /// <returns></returns>
      public string Wf30331()
      {
         Workbook workbook = new Workbook();
         try {
            string lsKindID = "GBF";
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", lsKindID, _emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", lsKindID, _emMonthText);

            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
            //讀取資料
            DataTable dt = daoAI3.ListAI3(lsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               //MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30330－十年期公債期貨契約價量資料,{lsKindID}無任何資料!");
               //return true;
            }
            foreach (DataRow row in dt.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[rowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");
               }
               worksheet.Rows[rowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[rowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[rowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               //重新選取圖表範圍
               ResetChartData(rowIndex + 1, workbook, worksheet, "30332");
            }
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 寫入 Data_30333.30334 sheet
      /// </summary>
      /// <returns></returns>
      public string Wf30333()
      {
         Workbook workbook = new Workbook();
         try {
            string SheetName = "Data_30333.30334";
            string lsKindID = "GBF";

            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);

            //總列數
            int rowIndex = 3; //int li_month_cnt=0;
            int RowTotal = 12;//Excel的Column預留空白行數12
            int sumRowIndex = RowTotal + rowIndex + 1;//小計行數
            int addRowCount = 0;//總計寫入的行數
            worksheet.Rows[sumRowIndex][1 - 1].Value = $"{PbFunc.Left(_emMonthText, 4).AsInt() - 1911}小計";
            string lsYMD = "";
            //讀取資料
            DataTable dt = new AM2().ListAM2(lsKindID, $"{PbFunc.Left(_emMonthText, 4)}01", _emMonthText.Replace("/", ""));
            if (dt.Rows.Count <= 0) {
               //MessageDisplay.Info($"{PbFunc.Left(emMonthText, 4)}01～{emMonthText.Replace("/", "")},30330－十年期公債期貨契約價量資料,{lsKindID}無任何資料!");
               //return true;
            }
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AM2_YMD"].AsString()) {
                  rowIndex = rowIndex + 1;
                  lsYMD = row["AM2_YMD"].AsString();
                  //li_month_cnt = li_month_cnt + 1;
                  addRowCount++;
                  worksheet.Rows[rowIndex][1 - 1].Value = $"{PbFunc.Left(lsYMD, 4).AsInt() - 1911}/{PbFunc.Right(lsYMD, 2)}";
               }
               int columnIndex = 0;
               switch (row["AM2_IDFG_TYPE"].AsString()) {
                  case "1":
                     columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 2 : 3) - 1;
                     break;
                  case "2":
                     columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 4 : 5) - 1;
                     break;
                  case "3":
                     columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 6 : 7) - 1;
                     break;
                  case "5":
                     columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 8 : 9) - 1;
                     break;
                  case "6":
                     columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 10 : 11) - 1;
                     break;
                  case "8":
                     columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 12 : 13) - 1;
                     break;
                  case "7":
                     columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 14 : 15) - 1;
                     break;
               }

               worksheet.Rows[rowIndex][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
            }

         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }
   }
}
