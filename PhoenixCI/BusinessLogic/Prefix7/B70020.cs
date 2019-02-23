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

namespace PhoenixCI.BusinessLogic.Prefix7
{
   public class B70020
   {
      private D70020 dao70020;
      private string is_save_file;

      public B70020()
      {
      }

      public B70020(string path)
      {
         dao70020 = new D70020();
         is_save_file = path;
      }
      /// <summary>
      /// ListM輸出excel
      /// </summary>
      /// <param name="startDate">起始日期</param>
      /// <param name="endDate">終止日期</param>
      /// <param name="ls_market_code">交易時段</param>
      public bool exportListM(string startDate, string endDate, string ls_market_code)
      {
         try {
            DataTable dt = dao70020.ListM(startDate, endDate, "M", ls_market_code);
            dt.Columns["RAMM1_BRK_TYPE"].ColumnName = "自營商(9)/一般法人(0)";
            dt.Columns["KIND_ID"].ColumnName = "商品";
            dt.Columns["BO"].ColumnName = "買一般委託";
            dt.Columns["BQ"].ColumnName = "買報價委託";
            dt.Columns["SO"].ColumnName = "賣一般委託";
            dt.Columns["SQ"].ColumnName = "賣報價委託";
            dt.Columns["MARKET_CODE"].ColumnName = "交易時段";
            dt.Columns["IBQ"].ColumnName = "買－報價範圍外成交";
            dt.Columns["ISQ"].ColumnName = "賣－報價範圍外成交";
            saveExcel(dt);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-exportListM");
            return false;
         }
      }
      /// <summary>
      /// ListO輸出excel
      /// </summary>
      /// <param name="startDate">起始日期</param>
      /// <param name="endDate">終止日期</param>
      /// <param name="ls_market_code">交易時段</param>
      public bool exportListO(string startDate, string endDate, string ls_market_code)
      {
         try {
            DataTable dt = dao70020.ListO(startDate, endDate, "O", ls_market_code);
            dt.Columns["RAMM1_BRK_TYPE"].ColumnName = "自營商(9)/一般法人(0)";
            dt.Columns["KIND_ID"].ColumnName = "商品";
            dt.Columns["BO"].ColumnName = "買一般委託";
            dt.Columns["BQ"].ColumnName = "買報價委託";
            dt.Columns["SO"].ColumnName = "賣一般委託";
            dt.Columns["SQ"].ColumnName = "賣報價委託";
            dt.Columns["MARKET_CODE"].ColumnName = "交易時段";
            saveExcel(dt);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-exportListO");
            return false;
         }
      }
      /// <summary>
      /// AM8輸出excel
      /// </summary>
      /// <param name="startDate">起始日期</param>
      /// <param name="endDate">終止日期</param>
      /// <param name="ls_market_code">交易時段</param>
      public bool exportAM8(string startDate, string endDate, string ls_market_code)
      {
         try {
            DataTable dtAM8 = dao70020.ListAM8(startDate, endDate, ls_market_code);
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
            saveExcel(dtAM8);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-exportAM8");
            return false;
         }
      }
      /// <summary>
      /// 判斷資料量選擇要存檔的格式(xls|txt)
      /// </summary>
      /// <param name="dataTable">要輸出的資料</param>
      private void saveExcel(DataTable dataTable)
      {
         try {
            if (dataTable.Rows.Count <= 0) {
               MessageDisplay.Error("轉出筆數為０!", GlobalInfo.gs_t_err);
            }

            Workbook wb = new Workbook();
            wb.Worksheets[0].Import(dataTable, true, 0, 0);
            wb.Worksheets[0].Name = sheetName(is_save_file);
            //存檔
            if (dataTable.Rows.Count > 0) {
               if (dataTable.Rows.Count <= 65536) {
                  wb.SaveDocument(is_save_file, DocumentFormat.Xls);
               }
               else {
                  wb.SaveDocument(is_save_file, DocumentFormat.Text);
               }
            }
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-saveExcel");
            return;
         }
      }

      private string sheetName(string is_save_file)
      {
         string filename = Path.GetFileNameWithoutExtension(is_save_file);
         int nameLen = filename.Length > 31 ? 31 : filename.Length;//sheetName不能超過31字
         return filename.Substring(0, nameLen);
      }
   }
}
