using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190319,john,布蘭特原油期貨契約價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 布蘭特原油期貨契約價量資料
   /// </summary>
   public class B30396
   {
      private readonly Workbook _workbook;
      private readonly string _emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30396(Workbook workbook, string datetime)
      {
         _workbook = workbook;
         _emMonthText = datetime;
      }
      /// <summary>
      /// 重新選取圖表資料範圍
      /// </summary>
      /// <param name="RowIndex">選取到第幾列</param>
      /// <param name="chartName">圖表sheet名稱</param>
      private static void ResetChartData(int RowIndex, Workbook workbook, Worksheet worksheet, string chartName)
      {
         //布蘭特原油期貨總成交量/布蘭特原油期貨總未平倉量/布蘭特原油期貨價格
         string[] data = new string[] { "D4:D", "E4:E", $@"B4:B" };
         int count = 0;
         foreach (var item in data) {
            workbook.ChartSheets[chartName].Chart.Series[count++].Values = new ChartData {
               RangeValue = worksheet.Range[item + RowIndex.ToString()]
            };
         }
      }

      /// <summary>
      /// wf_30396
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30396(string IsKindID = "BRF", string SheetName = "30396", int RowIndex=1, int RowTotal=33)
      {
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", IsKindID, _emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", IsKindID, _emMonthText);

            //切換Sheet
            Worksheet worksheet = _workbook.Worksheets[SheetName];
            //無前月資料

            int addRowCount = 0;//總計寫入的行數
            
            DataTable dtAI3 = new AI3().ListAI3(IsKindID, StartDate, EndDate);
            //讀取資料
            string firstDATE = dtAI3.AsEnumerable().FirstOrDefault()["AI3_DATE"].AsDateTime().ToString("yyyy/MM");
            if (firstDATE==_emMonthText) {
               RowIndex = RowIndex + 2;
            }
            //寫入資料
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            foreach (DataRow row in dtAI3.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");
               }
               //if  not isnull(ld_val) then  iole_1.application.activecell(ii_ole_row, 3).value = ids_1.getitemdecimal(i, "ai3_close_price") - ids_1.getitemdecimal(i, "ai3_last_close_price")
               //這段在PB不會執行成功 有寫跟沒寫一樣
               worksheet.Rows[RowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[RowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[RowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               //重新選取圖表範圍
               ResetChartData(RowIndex + 1, _workbook, worksheet, $"{SheetName}a");//ex:30396a
            }
            
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf30396:" + ex.Message);
#else
            throw ex;
#endif
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30396_abc
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30396abc(string IsKindID= "BRF", string SheetName= "data_30396abc", int RowIndex = 3, int RowTotal = 12)
      {
         try {
            return new B30398(_workbook, _emMonthText).Wf30333(IsKindID, SheetName);
         }
         catch (Exception ex) {
            throw ex;
         }
      }

      
   }
}
