using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixCI.BusinessLogic.Prefix3
{
   public class B30687
   {
      private D30687 dao30687;
      private string saveFilePath;

      public B30687(string path)
      {
         dao30687 = new D30687();
         saveFilePath = path;
      }
      /// <summary>
      /// WF30687RuNew輸出CSV
      /// </summary>
      /// <param name="StartDate">起始日期</param>
      /// <param name="EndDate">終止日期</param>
      /// <param name="AsPridID">商品</param>
      /// <param name="IsMarketCode">盤別</param>
      /// <param name="IsDataType">時段</param>
      public bool WF30687RuNew(string StartDate, string EndDate,string AsPridID, string IsMarketCode,string IsDataType)
      {
         try {
            DataTable dt = dao30687.ListRuNewData(StartDate, EndDate, AsPridID, IsMarketCode, IsDataType);
            dt.Columns["FTPRICELOGS_MARKET_CODE"].ColumnName = "盤別:0一般/1夜盤";
            dt.Columns["FTPRICELOGS_YMD"].ColumnName = "日期";
            dt.Columns["FTPRICELOGS_KIND_ID1"].ColumnName = "商品1";
            dt.Columns["FTPRICELOGS_SEQ_NO1"].ColumnName = "月份序1";
            dt.Columns["FTPRICELOGS_KIND_ID2"].ColumnName = "商品2";
            dt.Columns["FTPRICELOGS_SEQ_NO2"].ColumnName = "月份序2";
            dt.Columns["SUM(FTPRICELOGS_CNT)"].ColumnName = "總筆數";
            SaveExcel(dt);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.ErrorText + "-exportListM");
            return false;
         }
      }
      /// <summary>
      /// 存檔(csv)
      /// </summary>
      /// <param name="dataTable">要輸出的資料</param>
      private void SaveExcel(DataTable dataTable)
      {
         try {
            if (dataTable.Rows.Count <= 0) {
               MessageDisplay.Error("轉出筆數為０!", GlobalInfo.ErrorText);
            }

            Workbook wb = new Workbook();
            wb.Options.Export.Csv.WritePreamble = true;
            wb.Worksheets[0].Import(dataTable, true, 0, 0);
            wb.Worksheets[0].Name = SheetName(saveFilePath);
            //存檔
            wb.SaveDocument(saveFilePath, DocumentFormat.Csv);
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.ErrorText + "-saveExcel");
            return;
         }
      }

      private string SheetName(string filePath)
      {
         string filename = Path.GetFileNameWithoutExtension(filePath);
         int nameLen = filename.Length > 31 ? 31 : filename.Length;//sheetName不能超過31字
         return filename.Substring(0, nameLen);
      }
   }
}
