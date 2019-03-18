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
/// 20190307,john,匯率類期貨契約價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 匯率類期貨契約價量資料
   /// </summary>
   public class B30393
   {
      private string lsFile;
      private string emMonthText;
      private Workbook workbook;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30393(string FilePath,string datetime)
      {
         lsFile = FilePath;
         emMonthText = datetime;
         workbook = new Workbook();
      }
      /// <summary>
      /// 判斷要填入買或賣的欄位
      /// </summary>
      /// <param name="row">datarow AM2_BS_CODE</param>
      /// <returns></returns>
      private static int IDFGtype(DataRow row)
      {
         string drBScode = row["AM2_BS_CODE"].AsString();
         string bsCode = "B";
         int columnIndex = 0;
         switch (row["AM2_IDFG_TYPE"].AsString()) {
            case "1":
               columnIndex = (drBScode == bsCode ? 2 : 3) - 1;
               break;
            case "2":
               columnIndex = (drBScode == bsCode ? 4 : 5) - 1;
               break;
            case "3":
               columnIndex = (drBScode == bsCode ? 6 : 7) - 1;
               break;
            case "5":
               columnIndex = (drBScode == bsCode ? 8 : 9) - 1;
               break;
            case "6":
               columnIndex = (drBScode == bsCode ? 10 : 11) - 1;
               break;
            case "8":
               columnIndex = (drBScode == bsCode ? 12 : 13) - 1;
               break;
            case "7":
               columnIndex = (drBScode == bsCode ? 14 : 15) - 1;
               break;
         }

         return columnIndex;
      }
      /// <summary>
      /// 重新選取圖表資料範圍
      /// </summary>
      /// <param name="RowIndex">選取到第幾列</param>
      /// <param name="chartName">圖表sheet名稱</param>
      private static void ResetChartData(int RowIndex, Workbook workbook, Worksheet worksheet, string chartName)
      {
         //期貨總成交量/期貨總未平倉量/期貨價格/現貨價格
         string[] data = new string[] { "D4:D","E4:E", $@"B4:B", $@"F4:F" };
         int count = 0;
         foreach (var item in data) {
            workbook.ChartSheets[chartName].Chart.Series[count++].Values = new ChartData {
               RangeValue = worksheet.Range[item + RowIndex.ToString()]
            };
         }
         ////期貨總成交量
         //workbook.ChartSheets[chartName].Chart.Series[0].Values = new ChartData {
         //   RangeValue = worksheet.Range[data[0] + RowIndex.ToString()]
         //};
         ////期貨總未平倉量
         //workbook.ChartSheets[chartName].Chart.Series[1].Values = new ChartData {
         //   RangeValue = worksheet.Range[$@"E4:E{RowIndex}"]
         //};
         ////期貨價格
         //workbook.ChartSheets[chartName].Chart.Series[2].Values = new ChartData {
         //   RangeValue = worksheet.Range[$@"B4:B{RowIndex}"]
         //};
         ////現貨價格
         //workbook.ChartSheets[chartName].Chart.Series[3].Values = new ChartData {
         //   RangeValue = worksheet.Range[$@"F4:F{RowIndex}"]
         //};
      }

      /// <summary>
      /// wf_30393_1 
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public bool Wf30393(string IsKindID,string SheetName, int RowIndex=1, int RowTotal=33)
      {
         string flowStepDesc = "開始轉出資料";
         try {
            //前月倒數2天交易日
            flowStepDesc= "前月倒數2天交易日";
            DateTime StartDate = PbFunc.f_get_last_day("AI3", "RHF", emMonthText, 2);
            //抓當月最後交易日
            flowStepDesc = "抓當月最後交易日";
            DateTime EndDate = PbFunc.f_get_end_day("AI3", "RHF", emMonthText);

            //切換Sheet
            flowStepDesc = "切換Sheet";
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            
            worksheet.Range["A1"].Select();
            int addRowCount = 0;//總計寫入的行數
            //讀取資料
            flowStepDesc = "讀取資料";
            DataTable dtAI3 = new AI3().ListAI3(IsKindID, StartDate, EndDate);
            //寫入資料
            flowStepDesc = "寫入資料";
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            foreach (DataRow row in dtAI3.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");
               }

               worksheet.Rows[RowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[RowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[RowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[RowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
            }
            //刪除空白列
            flowStepDesc = "刪除空白列";
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               //重新選取圖表範圍
               flowStepDesc = "重新選取圖表範圍";
               ResetChartData(RowIndex+1, workbook, worksheet, SheetName.Replace($"({IsKindID})", "a"));//ex:30393_1a
               //int newRowIndex = RowIndex + (RowTotal - addRowCount);
               //worksheet.Rows.Hide(RowIndex + 1, newRowIndex);
               //RowIndex= newRowIndex;//沒有隱藏的下一行
            }

            //表尾
            flowStepDesc = "表尾資料讀取";
            DataTable dtAI2 = new AI2().ListAI2ym(IsKindID, EndDate.ToString("yyyyMM"), StartDate.ToString("yyyyMM"));
            if (dtAI2.Rows.Count <= 0) {
               return true;
            }

            int liDayCnt;
            //上月
            flowStepDesc = "表尾-上月";
            RowIndex = RowIndex + 5;
            liDayCnt = dtAI2.Rows[0]["LAST_M_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[RowIndex][5 - 1].Value = Math.Round(dtAI2.Rows[0]["LAST_M_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[RowIndex][7 - 1].Value = Math.Round(dtAI2.Rows[0]["LAST_M_OI"].AsDecimal() / liDayCnt, 0);
            }
            //今年迄今
            flowStepDesc = "表尾-今年迄今";
            RowIndex = RowIndex + 2;
            liDayCnt = dtAI2.Rows[0]["Y_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[RowIndex][5 - 1].Value = Math.Round(dtAI2.Rows[0]["Y_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[RowIndex][7 - 1].Value = Math.Round(dtAI2.Rows[0]["Y_OI"].AsDecimal() / liDayCnt, 0);
            }
            //存檔
            flowStepDesc = "存檔";
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf30393-{flowStepDesc}:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

      /// <summary>
      /// wf_30393_1abc
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public bool Wf30393abc(string IsKindID, string SheetName, int RowIndex = 3, int RowTotal = 12)
      {
         string flowStepDesc = "開始轉出資料";
         try {

            //切換Sheet
            flowStepDesc = "切換Sheet";
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            worksheet.Range["A1"].Select();
            //總列數
            int sumRowIndex = RowTotal + RowIndex + 1;//小計行數
            int addRowCount = 0;//總計寫入的行數
            worksheet.Rows[sumRowIndex][1 - 1].Value = $"{PbFunc.Left(emMonthText, 4).AsInt() - 1911}小計";
            string lsYMD = "";
            //讀取資料
            flowStepDesc = "讀取資料";
            DataTable dt = new AM2().ListAM2(IsKindID, $"{PbFunc.Left(emMonthText, 4)}01", emMonthText.Replace("/", ""));
            //寫入資料
            flowStepDesc = "寫入資料";
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AM2_YMD"].AsString()) {
                  RowIndex = RowIndex + 1;
                  lsYMD = row["AM2_YMD"].AsString();
                  //li_month_cnt = li_month_cnt + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = $"{PbFunc.Left(lsYMD, 4).AsInt() - 1911}/{PbFunc.Right(lsYMD, 2)}";
               }
               //判斷欄位
               int columnIndex = IDFGtype(row);

               worksheet.Rows[RowIndex][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();
            }
            //刪除空白列
            flowStepDesc = "刪除空白列";
            if (RowTotal > addRowCount) {
               //worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));
            }

            //存檔
            flowStepDesc = "存檔";
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf30393abc-{flowStepDesc}:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

      
   }
}
