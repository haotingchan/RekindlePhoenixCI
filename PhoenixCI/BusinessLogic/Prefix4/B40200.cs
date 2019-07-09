using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190312,john,指數類期貨及現貨資料下載
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 指數類期貨及現貨資料下載
   /// </summary>
   public class B40200
   {
      /// <summary>
      /// 檔案輸出路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 日期 起始日期
      /// </summary>
      private readonly string _startDateText;
      /// <summary>
      /// 日期 迄止日期
      /// </summary>
      private readonly string _endDateText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      public B40200(string FilePath,string StartDate,string EndDate)
      {
         _lsFile = FilePath;
         _startDateText = StartDate;
         _endDateText = EndDate;
      }
      /// <summary>
      /// wf_40200()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf40200(int RowIndex =0, string SheetName = "40200", string RptName = "指數類期貨價格及現貨資料下載")
      {
         Workbook workbook = new Workbook();
         try {
            DateTime startDate = _startDateText.AsDateTime();
            DateTime endDate = _endDateText.AsDateTime();
            
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];

            //讀取資料
            DataTable dt = new D40200().GetData(startDate, endDate);
            if (dt.Rows.Count <= 0) {
               return $"{DateTime.Now.ToShortDateString()},40200－{RptName},讀取「指數類期貨價格及現貨資料下載」無任何資料!";
            }

            worksheet.Range["A1"].Select();
            worksheet.Cells["A1"].Value = $"{startDate.ToShortDateString()}至{endDate.ToShortDateString()}"+ worksheet.Cells["A1"].Value.AsString();
            int colStart = -1;
            int kindCount = 0;
            string kindID = "";
            foreach (DataRow row in dt.Rows) {
               if (kindID != row["AI5_KIND_ID"].AsString()) {
                  kindID = row["AI5_KIND_ID"].AsString();
                  kindCount = kindCount + 1;
                  //每3個column為一組, 若超過256限制則結束
                  colStart = colStart + 3;
                  if (colStart > 254) {
                     return MessageDisplay.MSG_OK;
                  }
                  //Head
                  //1.股票期貨英文代碼
                  worksheet.Rows[2-1][colStart+1 - 1].Value= row["AI5_KIND_ID"].AsString();
                  RowIndex = 2;
               }
               //Detial
               //1.期貨結算價
               //2.期貨開盤參考價
               //3.標的指數收盤價
               //第1個商品才輸日期
               RowIndex = RowIndex + 1;
               if (kindCount == 1) {
                  worksheet.Rows[RowIndex][0].Value = row["AI5_DATE"].AsDateTime().ToString("yyyy/MM/dd");
               }
               if (row["AI5_SETTLE_PRICE"] != DBNull.Value) worksheet.Rows[RowIndex][colStart - 1].Value = row["AI5_SETTLE_PRICE"].AsDecimal();
               if (row["AI5_OPEN_REF"] != DBNull.Value) worksheet.Rows[RowIndex][colStart + 1 - 1].Value = row["AI5_OPEN_REF"].AsDecimal();
               if (row["SCP_CLOSE_PRICE"] != DBNull.Value) worksheet.Rows[RowIndex][colStart + 2 - 1].Value = row["SCP_CLOSE_PRICE"].AsDecimal();
            }
            //刪除空白列
            colStart = colStart + 3;
            for (int k = 0; k < (16-kindCount); k++) {
               worksheet.Range["A1"].Select();
               worksheet.Columns.Remove(colStart + 2 - 1);
               worksheet.Range["A1"].Select();
               worksheet.Columns.Remove(colStart + 1 - 1);
               worksheet.Range["A1"].Select();
               worksheet.Columns.Remove(colStart - 1);
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
