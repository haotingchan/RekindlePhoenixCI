using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190321,john,股票類(ETF)期貨價格及現貨資料下載
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 股票類(ETF)期貨價格及現貨資料下載
   /// </summary>
   public class B43033
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
      public B43033(string FilePath, string StartDate, string EndDate)
      {
         _lsFile = FilePath;
         _startDateText = StartDate;
         _endDateText = EndDate;
      }
      /// <summary>
      /// wf_43033()
      /// </summary>
      /// <returns></returns>
      public string Wf43033()
      {
         try {
            DateTime startDate = _startDateText.AsDateTime();
            DateTime endDate = _endDateText.AsDateTime();
            //讀取資料
            DataTable dt = new D43033().ExecuteStoredProcedure(startDate, endDate);
            if (dt.Rows.Count <= 0) {
               return $"{DateTime.Now.ToShortDateString()},43033－指數類期貨價格及現貨資料下載,讀取「股票類(ETF)期貨價格及現貨資料」無任何資料!";
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["A1"].Select();
            worksheet.Cells["A1"].Value = $"{startDate.ToString("yyyy/MM/dd")}至{endDate.ToString("yyyy/MM/dd")}" + worksheet.Cells["A1"].Value.AsString();

            int totKindCount = dt.AsEnumerable().Select(q => q.Field<string>("KIND_ID")).Distinct().Count();//li_kind_cnt = ids_1.getitemnumber(1,"cp_tot_kind_cnt")
            //複製column
            for (int k = 2; k <= totKindCount; k++) {
               //可擺放的最大商品數 = 63 = truncate((256 - 1日期欄) / 4column)
               if (totKindCount > 63) {
                  k = totKindCount;
                  continue;
               }
               /*iole_1.application.Range("A1").select	
		         iole_1.application.Range("B:E").select	
		         iole_1.application.Selection.copy		
		
		         iole_1.application.Range("A1").select	
		         iole_1.application.activecell(1 ,(i * 4) -2).Select
		         //Paste or Insert
		         iole_1.application.ActiveSheet.Paste 
		         //iole_1.application.Selection.Insert*/

               worksheet.Range["A1"].Select();
               worksheet.Rows[1 - 1][(k * 4) - 2-1].CopyFrom(worksheet.Range["B:E"]);
               //插入分頁 
               //iole_1.application.worksheets(1).VPageBreaks.add(iole_1.application.activecell(1, (i * 4) - 2))
               //worksheet.VerticalPageBreaks.Add((k * 4) - 2);
            }

            int rowStart = 0;
            int colStart = -2-1;
            int kindCount = 0;
            string kindID = "";
            foreach (DataRow row in dt.Rows) {
               if (kindID != row["KIND_ID"].AsString()) {
                  kindID = row["KIND_ID"].AsString();
                  kindCount = kindCount + 1;
                  //每4個column為一組, 若超過256限制則結束
                  colStart = colStart + 4;
                  if (colStart > 253) {
                     return MessageDisplay.MSG_OK;
                  }
                  //Head
                  //1.股票期貨英文代碼
                  //2.股票期貨中文簡稱
                  //3.股票期貨標的證券代號
                  //4.上市/上櫃類別
                  rowStart = 5 - 1;
                  for (int liCol = 2; liCol <= 5; liCol++) {
                     worksheet.Rows[rowStart][colStart].Value = row["KIND_ID"].AsString();
                     worksheet.Rows[rowStart][colStart + 1].Value = row["APDK_NAME"].AsString();
                     worksheet.Rows[rowStart][colStart + 2].Value = row["APDK_STOCK_ID"].AsString();
                     worksheet.Rows[rowStart][colStart + 3].Value = row["PID_NAME"].AsString();
                  }
                  rowStart = 6 - 1;
               }//if (kindID != row["KIND_ID"].AsString())

               //Detial
               //1.期貨結算價
               //2.期貨開盤參考價
               //3.標的指數收盤價
               //4.標的指數開盤參考價
               //第1個商品才輸日期
               rowStart = rowStart + 1;
               if (kindCount == 1) {
                  worksheet.Rows[rowStart][1-1].Value = row["DATA_DATE"].AsDateTime();
               }
               worksheet.Rows[rowStart][colStart].SetValue(row["F_SETTLE_PRICE"]);
               worksheet.Rows[rowStart][colStart + 1].SetValue(row["F_OPEN_REF"]);
               worksheet.Rows[rowStart][colStart + 2].SetValue(row["T_PRICE"]);
               worksheet.Rows[rowStart][colStart + 3].SetValue(row["T_OPEN_REF"]);
            }//foreach (DataRow row in dt.Rows)

            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf43033:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

   }
}
