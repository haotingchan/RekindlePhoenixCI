using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190319,john,櫃買期貨契約價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 櫃買期貨契約價量資料
   /// </summary>
   public class B30398
   {
      private readonly Workbook _workbook;
      private string _emMonthText;
      private readonly AI3 daoAI3;
      private readonly AM2 daoAM2;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30398(Workbook Workbook, string datetime)
      {
         _workbook = Workbook;
         _emMonthText = datetime;
         daoAI3 = new AI3();
         daoAM2 = new AM2();

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
         //櫃買期貨總成交量/櫃買期貨總未平倉量/櫃買價格
         string[] data = new string[] { "D4:D","E4:E", $@"B4:B"};
         int count = 0;
         foreach (var item in data) {
            workbook.ChartSheets[chartName].Chart.Series[count++].Values = new ChartData {
               RangeValue = worksheet.Range[item + RowIndex.ToString()]
            };
         }
      }

      /// <summary>
      /// wf_30331
      /// 30398,30399相同
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30331(string IsKindID= "GTF", string SheetName= "30398", int RowIndex=1, int RowTotal=33)
      {
         
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", IsKindID, _emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", IsKindID, _emMonthText);
            
            Worksheet worksheet = _workbook.Worksheets[SheetName];
            /*add some infor 原本template標題就已經設定 這段看不出意義在哪 所以不翻
            iole_1.application.activecell(1, 1).value = "櫃買期貨"
            iole_1.application.activecell(2, 2).value = "櫃買價格"
            iole_1.application.activecell(2, 4).value = "櫃買期貨總成交量"
            iole_1.application.activecell(2, 5).value = "櫃買期貨總未平倉量"
            end add*/
            worksheet.Range["A1"].Select();
            int addRowCount = 0;//總計寫入的行數
            //讀取資料
            DataTable dtAI3 = daoAI3.ListAI3(IsKindID, StartDate, EndDate);
            //寫入資料
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
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               //重新選取圖表範圍
               ResetChartData(RowIndex+1, _workbook, worksheet, $"{SheetName}a");//ex:30398a
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }
            
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf30331:" + ex.Message);
#else
            throw ex;
#endif
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30333()
      /// 30398,30399,30393相同
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30333(string IsKindID= "GTF", string SheetName= "data_30398abc", int RowIndex = 3, int RowTotal = 12)
      {
         try {
            //切換Sheet
            Worksheet worksheet = _workbook.Worksheets[SheetName];
            //總列數
            int sumRowIndex = RowTotal + RowIndex + 1;//小計行數
            int addRowCount = 0;//總計寫入的行數
            worksheet.Rows[sumRowIndex][1 - 1].Value = $"{PbFunc.Left(_emMonthText, 4).AsInt() - 1911}小計";
            string lsYMD = "";
            //讀取資料
            DataTable dt = daoAM2.ListAM2(IsKindID, $"{PbFunc.Left(_emMonthText, 4)}01", _emMonthText.Replace("/", ""));
            //寫入資料
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
            if (RowTotal > addRowCount) {
               //worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }

         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf30333:" + ex.Message);
#else
            throw ex;
#endif
         }

         return MessageDisplay.MSG_OK;
      }

      
   }
}
