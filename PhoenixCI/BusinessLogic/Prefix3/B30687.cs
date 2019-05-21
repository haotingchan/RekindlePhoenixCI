using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
/// <summary>
/// 20190320,john,動態價格穩定措施基準價查詢
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 動態價格穩定措施基準價查詢
   /// </summary>
   public class B30687
   {
      private D30687 dao30687;
      private readonly string _saveFilePath;
      private readonly int _rgTimeSelIndex;
      private readonly int _rgMarketSelIndex;
      private readonly string _startDateTxt;
      private readonly string _endDateTxt;
      private readonly string _PridIDTxt;

      public B30687(string path, string StartDate, string EndDate, string AsPridID, int rgMarket, int rgTime)
      {
         dao30687 = new D30687();
         _saveFilePath = CreateCsvFile(path);
         _startDateTxt = StartDate;
         _endDateTxt = EndDate;
         _PridIDTxt = AsPridID;
         _rgMarketSelIndex = rgMarket;
         _rgTimeSelIndex = rgTime;
      }
      /// <summary>
      /// WF30687RuNew輸出CSV
      /// </summary>
      /// <param name="StartDate">起始日期</param>
      /// <param name="EndDate">終止日期</param>
      /// <param name="AsPridID">商品</param>
      /// <param name="IsMarketCode">盤別</param>
      /// <param name="IsDataType">時段</param>
      public string WF30687RuNew()
      {
         try {
            //盤別
            string lsMarketCode = string.Empty;
            switch (_rgMarketSelIndex) {
               case 0://日盤
                  lsMarketCode = "0";
                  break;
               case 1://夜盤
                  lsMarketCode = "1";
                  break;
               default://全部
                  lsMarketCode = "%";
                  break;
            }
            //時段
            string lsDataType = string.Empty;
            switch (_rgTimeSelIndex) {
               case 0://盤前
                  lsDataType = "B";
                  break;
               case 1://交易時段
                  lsDataType = " ";
                  break;
               default://全部
                  lsDataType = "%";
                  break;
            }
            //資料來源
            DataTable dt = dao30687.ListRuNewData(_startDateTxt.Replace("/", ""), _endDateTxt.Replace("/", ""), $"%{_PridIDTxt}%", lsMarketCode, lsDataType);

            dt.Columns["FTPRICELOGS_MARKET_CODE"].ColumnName = "盤別:0一般/1夜盤";
            dt.Columns["FTPRICELOGS_YMD"].ColumnName = "日期";
            dt.Columns["FTPRICELOGS_KIND_ID1"].ColumnName = "商品1";
            dt.Columns["FTPRICELOGS_SEQ_NO1"].ColumnName = "月份序1";
            dt.Columns["FTPRICELOGS_KIND_ID2"].ColumnName = "商品2";
            dt.Columns["FTPRICELOGS_SEQ_NO2"].ColumnName = "月份序2";
            dt.Columns["SUM(FTPRICELOGS_CNT)"].ColumnName = "總筆數";
            return SaveExcel(dt);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception(ex.Message + "-exportListM");
#else
            throw ex;
#endif
         }
      }
      /// <summary>
      /// 存檔(csv)
      /// </summary>
      /// <param name="dataTable">要輸出的資料</param>
      private string SaveExcel(DataTable dataTable)
      {
         try {
            Workbook wb = new Workbook();
            wb.Options.Export.Csv.WritePreamble = true;
            wb.Worksheets[0].Import(dataTable, true, 0, 0);
            wb.Worksheets[0].Name = SheetName(_saveFilePath);
            //存檔
            wb.SaveDocument(_saveFilePath, DocumentFormat.Csv);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception(ex.Message + "-saveExcel");
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

      private string CreateCsvFile(string saveFilePath)
      {
         //避免重複寫入
         try {
            if (File.Exists(saveFilePath)) {
               File.Delete(saveFilePath);
            }
            File.Create(saveFilePath).Close();
         }
         catch (Exception ex) {
            throw ex;
         }
         return saveFilePath;
      }

      private string SheetName(string filePath)
      {
         string filename = Path.GetFileNameWithoutExtension(filePath);
         int nameLen = filename.Length > 31 ? 31 : filename.Length;//sheetName不能超過31字
         return filename.Substring(0, nameLen);
      }
   }
}
