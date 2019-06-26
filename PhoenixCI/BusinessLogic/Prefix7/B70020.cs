using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix7
{
   public class B70020
   {
      private D70020 dao70020;
      private readonly string _saveFilePath;

      public B70020()
      {
      }

      public B70020(string path)
      {
         dao70020 = new D70020();
         _saveFilePath = path;
      }
      /// <summary>
      /// ListM輸出excel
      /// </summary>
      /// <param name="startDate">起始日期</param>
      /// <param name="endDate">終止日期</param>
      /// <param name="lsMarketCode">交易時段</param>
      public string ExportListM(string startDate, string endDate, string lsMarketCode)
      {
         try {
            DataTable dt = dao70020.ListM(startDate, endDate, "M", lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return MessageDisplay.MSG_NO_DATA;
            }
            dt.Columns["RAMM1_BRK_TYPE"].ColumnName = "自營商(9)/一般法人(0)";
            dt.Columns["KIND_ID"].ColumnName = "商品";
            dt.Columns["BO"].ColumnName = "買一般委託";
            dt.Columns["BQ"].ColumnName = "買報價委託";
            dt.Columns["SO"].ColumnName = "賣一般委託";
            dt.Columns["SQ"].ColumnName = "賣報價委託";
            dt.Columns["MARKET_CODE"].ColumnName = "交易時段";
            dt.Columns["IBQ"].ColumnName = "買－報價範圍外成交";
            dt.Columns["ISQ"].ColumnName = "賣－報價範圍外成交";
            SaveExcel(dt);
         }
         catch (Exception ex) {
            throw ex;
         }
         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// ListO輸出excel
      /// </summary>
      /// <param name="startDate">起始日期</param>
      /// <param name="endDate">終止日期</param>
      /// <param name="lsMarketCode">交易時段</param>
      public string ExportListO(string startDate, string endDate, string lsMarketCode)
      {
         try {
            DataTable dt = dao70020.ListO(startDate, endDate, "O", lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return MessageDisplay.MSG_NO_DATA;
            }
            dt.Columns["RAMM1_BRK_TYPE"].ColumnName = "自營商(9)/一般法人(0)";
            dt.Columns["KIND_ID"].ColumnName = "商品";
            dt.Columns["BO"].ColumnName = "買一般委託";
            dt.Columns["BQ"].ColumnName = "買報價委託";
            dt.Columns["SO"].ColumnName = "賣一般委託";
            dt.Columns["SQ"].ColumnName = "賣報價委託";
            dt.Columns["MARKET_CODE"].ColumnName = "交易時段";
            
            SaveExcel(dt);
         }
         catch (Exception ex) {
            throw ex;
         }

         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// AM8輸出excel
      /// </summary>
      /// <param name="startDate">起始日期</param>
      /// <param name="endDate">終止日期</param>
      /// <param name="lsMarketCode">交易時段</param>
      public string ExportAM8(string startDate, string endDate, string lsMarketCode)
      {
         try {
            DataTable dtAM8 = dao70020.ListAM8(startDate, endDate, lsMarketCode);
            if (dtAM8.Rows.Count<=0) {
               return MessageDisplay.MSG_NO_DATA;
            }
            DataView dsDv = dtAM8.AsDataView();
            dsDv.Sort = "AM8_YMD,AM8_PROD_TYPE,AM8_FCM_NO,AM8_PARAM_KEY,qnty_8 Desc,qnty_2 Desc";
            dtAM8 = dsDv.ToTable();
            dtAM8.Columns["AM8_YMD"].ColumnName = "日期";
            dtAM8.Columns["AM8_PROD_TYPE"].ColumnName = "商品別";
            dtAM8.Columns["AM8_FCM_NO"].ColumnName = "期貨商代號";
            dtAM8.Columns["AM8_PARAM_KEY"].ColumnName = "商品";
            dtAM8.Columns["qnty_8"].ColumnName = "造市者帳號(8)成交量";
            dtAM8.Columns["qnty_2"].ColumnName = "自營商帳號(2)成交量";
            dtAM8.Columns["MARKET_CODE"].ColumnName = "交易時段";
            SaveExcel(dtAM8);
         }
         catch (Exception ex) {
            throw ex;
         }

         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// 判斷資料量選擇要存檔的格式(xls|txt)
      /// </summary>
      /// <param name="dataTable">要輸出的資料</param>
      private void SaveExcel(DataTable dataTable)
      {
         try {
            Workbook wb = new Workbook();
            wb.Worksheets[0].Import(dataTable, true, 0, 0);
            wb.Worksheets[0].Name = SheetName(_saveFilePath);
            //存檔
            if (dataTable.Rows.Count > 0) {
               if (dataTable.Rows.Count <= 65536) {
                  wb.SaveDocument(_saveFilePath, DocumentFormat.Xls);
               }
               else {
                  wb.SaveDocument(_saveFilePath, DocumentFormat.Text);
               }
            }
         }
         catch (Exception ex) {
            throw new Exception( "saveExcel:"+ ex.Message);
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
