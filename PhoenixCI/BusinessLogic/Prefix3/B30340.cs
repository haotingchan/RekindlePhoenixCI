using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 20190221,john,三十天期商業本票利率期貨契約價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 三十天期商業本票利率期貨契約價量資料
   /// </summary>
   public class B30340
   {
      private AI3 daoAI3;
      private string lsFile;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30340(string FilePath,string datetime)
      {
         daoAI3 = new AI3();
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
         //期貨總成交量/期貨總未平倉量/期貨價格/現貨價格
         string[] data = new string[] { "D4:D", "E4:E", $@"B4:B", $@"G4:G" };
         int count = 0;
         foreach (var item in data) {
            workbook.ChartSheets[chartName].Chart.Series[count++].Values = new ChartData {
               RangeValue = worksheet.Range[item + RowIndex.ToString()]
            };
         }
      }

      /// <summary>
      /// 寫入 30341 sheet
      /// </summary>
      /// <returns></returns>
      public bool Wf30341()
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
            string lsKindID = "CPF";
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", lsKindID, emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", lsKindID, emMonthText);
            //讀取資料
            DataTable dt = daoAI3.ListAI3(lsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               //MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30340－三十天期商業本票利率期貨契約價量資料,{lsKindID}無任何資料!");
               //return true;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
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
               worksheet.Rows[rowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               ResetChartData(rowIndex + 1, workbook, worksheet, "30342");//ex:30342
            }
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "wf_30341");
            return false;
         }
      }
      /// <summary>
      /// 寫入 Data_30343.30344 sheet
      /// </summary>
      /// <returns></returns>
      public bool Wf30343()
      {
         /*************************************
         ls_rpt_name = 報表名稱 
         ls_rpt_id = 報表代號
         rowIndex = Excel的Row位置
         columnIndex = Excel的Column位置
         RowTotal = Excel的Column預留數
         ii_ole_y_row_tol = Excel年部份的Column預留數
         li_month_cnt = Excel的月份個數
         lsYMD = 日期
         ls_end_ymd = 最後一筆日期
         *************************************/
         try {
            string SheetName = "Data_30343.30344";
            string lsKindID = "CPF";
            
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();
            //總列數
            int rowIndex = 3; //int li_month_cnt=0;
            int RowTotal = 12;//Excel的Column預留數12行
            int sumRowIndex = RowTotal + rowIndex + 1;//小計行數
            int addRowCount = 0;//總計寫入的行數
            worksheet.Rows[sumRowIndex][1 - 1].Value = $"{PbFunc.Left(emMonthText, 4).AsInt() - 1911}小計";
            string lsYMD = "";
            //讀取資料
            DataTable dt = new AM2().ListAM2(lsKindID, $"{PbFunc.Left(emMonthText, 4)}01", emMonthText.Replace("/", ""));
            if (dt.Rows.Count <= 0) {
               //MessageDisplay.Info($"{PbFunc.Left(emMonthText, 4)}01～{emMonthText.Replace("/", "")},30340－三十天期商業本票利率期貨契約價量資料,{lsKindID}無任何資料!");
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
               int columnIndex=0;
               switch (row["AM2_IDFG_TYPE"].AsString()) {
                  case "1":
                     columnIndex = (row["AM2_BS_CODE"].AsString()=="B"?2:3)-1;
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
               //worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               if (dt.Rows.Count > 0) {
                  worksheet.Rows.Hide(rowIndex + 1, RowTotal + (RowTotal - addRowCount));
               }
               else {
                  worksheet.Rows.Hide(4, 15);
               }
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "wf_30343");
            return false;
         }
      }
   }
}
