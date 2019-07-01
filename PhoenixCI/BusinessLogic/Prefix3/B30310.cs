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
/// 20190218,john,證期局七組月報
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 證期局七組月報
   /// </summary>
   public class B30310
   {
      private AI2 daoAI2;
      private AI3 daoAI3;
      private D30310 dao30310;
      private readonly string _lsFile;
      private readonly string _emMonthText;

      /// <summary>
      /// 證期局七組月報
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30310(string FilePath, string datetime)
      {
         daoAI2 = new AI2();
         daoAI3 = new AI3();
         dao30310 = new D30310();
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
         //期貨指數/現貨指數/期貨交易量
         Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            {"期貨指數", "B5:B"}, {"現貨指數", "F5:F"},{"期貨交易量", "D5:D"}
        };
         foreach (var item in workbook.ChartSheets[chartName].Chart.Series) {
            item.Values = new ChartData {
               RangeValue = worksheet.Range[dic[item.SeriesName.PlainText] + RowIndex.ToString()]
            };
         }

      }

      /// <summary>
      /// 寫入 30311_1 sheet
      /// </summary>
      /// <param name="lsKindID"></param>
      /// <param name="SheetName"></param>
      /// <returns></returns>
      public string Wf30310one(string lsKindID, string SheetName)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];

            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", lsKindID, _emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", lsKindID, _emMonthText);
            //讀取資料
            DataTable dt = dao30310.GetData(lsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               return $"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30310－我國臺股期貨契約價量資料,{lsKindID}無任何資料!";
            }

            DateTime ldtYMD = new DateTime(1900, 1, 1);

            int rowIndex = 1;
            int RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數

            foreach (DataRow row in dt.Rows) {
               //不同的日期就寫入新的一行
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  //日期
                  worksheet.Cells[$"A{rowIndex + 1}"].SetValue(ldtYMD.ToString("MM/dd"));
               }
               //臺股期貨市場
               worksheet.Cells[$"B{rowIndex + 1}"].SetValue(row["AI3_CLOSE_PRICE"]);//臺股期貨指數(TX)
               worksheet.Cells[$"D{rowIndex + 1}"].SetValue(row["AI3_M_QNTY"]);//股價指數類期貨成交量(註①)
               worksheet.Cells[$"E{rowIndex + 1}"].SetValue(row["AI3_OI"]);//股價指數類未平倉量(註①)
               //臺股現貨市場
               worksheet.Cells[$"F{rowIndex + 1}"].SetValue(row["AI3_INDEX"]);//臺股現貨指數(TAIEX)
               if (lsKindID == "TXF") {
                  worksheet.Cells[$"H{rowIndex + 1}"].SetValue(row["AI3_AMOUNT"]);//成交值(億元)
               }
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               //重新選取圖表範圍
               ResetChartData(rowIndex + 1, workbook, worksheet, $"{SheetName}a");//ex:30393_1a
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }
            //表尾
            dt = daoAI2.ListAI2ym(lsKindID, EndDate.ToString("yyyyMM"), StartDate.ToString("yyyyMM"));
            if (dt.Rows.Count <= 0) {
               return "";
            }

            int liDayCnt;
            //上月
            rowIndex = rowIndex + 5;
            liDayCnt = dt.Rows[0]["LAST_M_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Cells[$"E{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["LAST_M_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Cells[$"G{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["LAST_M_OI"].AsDecimal() / liDayCnt, 0);
            }
            //本月
            rowIndex = rowIndex - 1;
            liDayCnt = dt.Rows[0]["CUR_M_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Cells[$"E{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["CUR_M_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Cells[$"G{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["CUR_M_OI"].AsDecimal() / liDayCnt, 0);
            }
            rowIndex = rowIndex + 1;
            //今年迄今
            rowIndex = rowIndex + 2;
            liDayCnt = dt.Rows[0]["Y_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Cells[$"E{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["Y_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Cells[$"G{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["Y_OI"].AsDecimal() / liDayCnt, 0);
            }

         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);//存檔
         }

         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// 寫入30311_2(EXF)&30311_3(FXF) sheet
      /// </summary>
      /// <param name="lsKindID"></param>
      /// <param name="SheetName"></param>
      /// <returns></returns>
      public string Wf30310two(string lsKindID, string SheetName)
      {
         Workbook workbook = new Workbook();
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", lsKindID, _emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", lsKindID, _emMonthText);
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            //讀取資料
            DataTable dt = daoAI3.ListAI3(lsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               return $"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30310－我國臺股期貨契約價量資料,{lsKindID}無任何資料!";
            }

            int rowIndex = 1;
            int RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數

            foreach (DataRow row in dt.Rows) {
               //不同的日期就寫入新的一行
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  worksheet.Cells[$"A{rowIndex + 1}"].SetValue(ldtYMD.ToString("MM/dd"));//日期
               }
               //電子期貨市場
               worksheet.Cells[$"B{rowIndex + 1}"].SetValue(row["AI3_CLOSE_PRICE"]);//電子期貨指數(TX)
               worksheet.Cells[$"D{rowIndex + 1}"].SetValue(row["AI3_M_QNTY"]);//電子期貨成交量(註①)
               worksheet.Cells[$"E{rowIndex + 1}"].SetValue(row["AI3_OI"]);//電子期貨未平倉量(註①)
               //電子現貨市場
               worksheet.Cells[$"F{rowIndex + 1}"].SetValue(row["AI3_INDEX"]);//電子現貨指數(TAIEX)
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               //重新選取圖表範圍
               ResetChartData(rowIndex + 1, workbook, worksheet, SheetName.Replace($"({lsKindID})", "a"));//ex:30393_1a
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }
            //表尾
            dt = daoAI2.ListAI2ym(lsKindID, EndDate.ToString("yyyyMM"), StartDate.ToString("yyyyMM"));
            if (dt.Rows.Count <= 0) {
               return MessageDisplay.MSG_OK;
            }

            int liDayCnt;
            //上月
            rowIndex = rowIndex + 5;
            liDayCnt = dt.Rows[0]["LAST_M_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Cells[$"E{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["LAST_M_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Cells[$"G{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["LAST_M_OI"].AsDecimal() / liDayCnt, 0);
            }
            //今年迄今
            rowIndex = rowIndex + 2;
            liDayCnt = dt.Rows[0]["Y_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Cells[$"E{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["Y_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Cells[$"G{rowIndex + 1}"].Value = Math.Round(dt.Rows[0]["Y_OI"].AsDecimal() / liDayCnt, 0);
            }

         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);//存檔
         }
         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// 寫入 30311_4 sheet
      /// </summary>
      /// <returns></returns>
      public string Wf30310four()
      {
         Workbook workbook = new Workbook();
         try {
            string lsKindID = "MSF";
            string SheetName = "30311_4";
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", lsKindID, _emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", lsKindID, _emMonthText);
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            //讀取資料
            DataTable dt = daoAI3.ListAI3(lsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               return MessageDisplay.MSG_OK;
            }

            int rowIndex = 1;
            int RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數

            foreach (DataRow row in dt.Rows) {
               //不同的日期就寫入新的一行
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  worksheet.Cells[$"A{rowIndex + 1}"].SetValue(ldtYMD.ToString("MM/dd"));
               }
               //金融期貨市場
               worksheet.Cells[$"B{rowIndex + 1}"].SetValue(row["AI3_CLOSE_PRICE"]);//金融期貨指數(TX)
               worksheet.Cells[$"D{rowIndex + 1}"].SetValue(row["AI3_M_QNTY"]);//金融期貨成交量(註①)
               worksheet.Cells[$"E{rowIndex + 1}"].SetValue(row["AI3_OI"]);//金融期貨未平倉量(註①)
               //金融現貨市場
               worksheet.Cells[$"F{rowIndex + 1}"].SetValue(row["AI3_INDEX"]);//金融現貨指數(TAIEX)
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               string[] data = new string[] { "D4:D", "E4:E", $@"B4:B", $@"F4:F" };
               int count = 0;
               //重新抓取圖表範圍
               foreach (var item in data) {
                  workbook.ChartSheets[$"{SheetName}a"].Chart.Series[count++].Values = new ChartData {
                     RangeValue = worksheet.Range[item + rowIndex.ToString()]
                  };
               }
            }

         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);//存檔
         }

         return MessageDisplay.MSG_OK;
      }
   }
}
