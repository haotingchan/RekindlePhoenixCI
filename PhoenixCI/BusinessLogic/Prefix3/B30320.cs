using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// john,20190220,指數期貨價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 指數期貨價量資料
   /// </summary>
   public class B30320
   {
      private D30320 dao30320;
      private string lsFile;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30320(string FilePath,string datetime)
      {
         dao30320 = new D30320();
         lsFile = FilePath;
         emMonthText = datetime;
      }
      /// <summary>
      /// 重新選取圖表資料範圍
      /// </summary>
      /// <param name="RowIndex">選取到第幾列</param>
      /// <param name="chartName">圖表sheet名稱</param>
      private static void ResetChartData(int RowIndex, Workbook workbook, Worksheet worksheet, string chartName)
      {
         //TX台指期貨/TE電子期貨/TF金融期貨/MTX小型台指期貨/T5F台灣50期貨/MSF摩台期貨/XIF非金電期貨/GTF櫃買指數期貨
         string[] data = new string[] { "K3:K", "C3:C", "E3:E", "G3:G", "I3:I", "M3:M", "O3:O", "Q3:Q" };
         int count = 0;
         foreach (var item in data) {
            workbook.ChartSheets[chartName].Chart.Series[count++].Values = new ChartData {
               RangeValue = worksheet.Range[item + RowIndex.ToString()]
            };
         }
      }
      /// <summary>
      /// 寫入 30320 sheet
      /// </summary>
      /// <returns></returns>
      public bool Wf30321()
      {
         /*************************************
         ls_rpt_name = 報表名稱
         ls_rpt_id = 報表代號
         rowIndex = Excel的Row位置
         columnIndex = Excel的Column位置
         RowTotal = Excel的Column預留數
         lsYMD = 日期
         *************************************/
         try {
            //前月倒數1天交易日
            DateTime LastTradeDate = dao30320.GetLastTradeDate(emMonthText.AsDateTime().ToString("yyyyMM01"));
            //讀取資料
            DataTable dt = dao30320.Get30321Data(LastTradeDate.ToString("yyyyMMdd"), emMonthText.AsDateTime().ToString("yyyyMM31"));
            dt = dt.Filter("RPT_SEQ_NO > 0");
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info($"{LastTradeDate.ToShortDateString()}～{emMonthText.AsDateTime().ToString("yyyy/MM/31")},30321－指數期貨價量資料,無任何資料!");
               return true;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            string lsYMD = "";
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AI2_YMD"].AsString()) {
                  lsYMD = row["AI2_YMD"].AsString();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[rowIndex][1 - 1].Value = lsYMD.AsDateTime("yyyyMMdd").ToString("MM/dd");
               }
               int columnIndex = row["RPT_SEQ_NO"].AsInt();
               worksheet.Rows[rowIndex][columnIndex - 1].Value = row["AI2_M_QNTY"].AsDecimal();
               worksheet.Rows[rowIndex][columnIndex + 1 - 1].Value = row["AI2_OI"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
            }
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "wf_30321");
            return false;
         }
      }
      /// <summary>
      /// 寫入 Data_30322ab sheet
      /// </summary>
      /// <returns></returns>
      public bool Wf30322()
      {
         /*************************************
         ls_rpt_name = 報表名稱
         ls_rpt_id = 報表代號
         rowIndex = Excel的Row位置
         columnIndex = Excel的Column位置
         RowTotal = Excel的Column預留數
         lsYMD = 日期
         *************************************/
         string SheetName = "Data_30322ab";
         try {
            DateTime emMonthDate = emMonthText.AsDateTime();//輸入日期轉時間格式
            //前月倒數1天交易日
            DateTime ldtSdate = dao30320.GetLastTradeDate(emMonthDate.ToString("yyyyMM01"));
            //月底
            string lsYMD = string.Empty;
            if (emMonthDate.Month == 12) {
               lsYMD = $"{emMonthDate.AddYears(1)}01";
            }
            else {
               lsYMD = emMonthDate.AddMonths(1).ToString("yyyyMM");
            }
            DateTime ldtEdate = PbFunc.relativedate(lsYMD.AsDateTime("yyyyMM"), -1);
            //讀取資料
            DataTable dt = dao30320.Get30322Data(ldtSdate, ldtEdate);
            dt = dt.Filter("RPT_SEQ_NO > 0");
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info($"{ldtSdate.ToShortDateString()}～{emMonthText.AsDateTime().ToString("yyyy/MM/31")},30322－指數期貨價量資料,無任何資料!");
               return true;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int RowTotal = 32;//Excel的Column預留數32
            int addRowCount = 0;//總計寫入的行數
            lsYMD = "";
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AI3_DATE"].AsString()) {
                  lsYMD = row["AI3_DATE"].AsString();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[rowIndex][1 - 1].Value = row["AI3_DATE"].AsDateTime().ToString("MM/dd");
               }
               int columnIndex = 0;
               columnIndex = row["RPT_SEQ_NO"].AsInt();
               worksheet.Rows[rowIndex][columnIndex - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               //重新選取圖表範圍
               ResetChartData(rowIndex + 1, workbook, worksheet, "30320a");//ex:30320a
            }
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message,"wf_30322");
            return false;
         }
      }
   }
}
