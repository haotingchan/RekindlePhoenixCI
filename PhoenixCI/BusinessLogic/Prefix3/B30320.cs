using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190220,指數期貨價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 指數期貨價量資料
   /// </summary>
   public class B30320
   {
      private D30320 dao30320;
      private string ls_file;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30320(string FilePath,string datetime)
      {
         dao30320 = new D30320();
         ls_file = FilePath;
         emMonthText = datetime;
      }

      public bool wf_30321()
      {
         /*************************************
         ls_rpt_name = 報表名稱
         ls_rpt_id = 報表代號
         li_ole_col = Excel的Column位置
         li_ole_row_tol = Excel的Column預留數
         ls_ymd = 日期
         *************************************/
         //前月倒數1天交易日
         DateTime LastTradeDate = dao30320.GetLastTradeDate(emMonthText.AsDateTime().ToString("yyyyMM01"));
         //讀取資料
         DataTable ids_1 = new D30321().GetData(LastTradeDate.ToString("yyyyMMdd"), emMonthText.AsDateTime().ToString("yyyyMM31"));
         ids_1 = ids_1.Filter("RPT_SEQ_NO > 0");
         if (ids_1.Rows.Count <= 0) {
            MessageDisplay.Info($"{LastTradeDate.ToShortDateString()}～{emMonthText.AsDateTime().ToString("yyyy/MM/31")},30321－指數期貨價量資料,無任何資料!");
            return false;
         }
         //切換Sheet
         Workbook workbook = new Workbook();
         workbook.LoadDocument(ls_file);
         Worksheet worksheet = workbook.Worksheets["30320"];
         string ls_ymd = "";
         worksheet.Range["A1"].Select();

         int rowIndex = 1;
         int li_ole_row_tol = rowIndex + 1 + 32;
         foreach (DataRow row in ids_1.Rows) {
            if (ls_ymd != row["AI2_YMD"].AsString()) {
               ls_ymd = row["AI2_YMD"].AsString();
               rowIndex = rowIndex + 1;
               worksheet.Rows[rowIndex][1 - 1].Value = ls_ymd.AsDateTime("yyyyMMdd").ToString("MM/dd");
            }
            int li_ole_col = row["RPT_SEQ_NO"].AsInt();
            worksheet.Rows[rowIndex][li_ole_col-1].Value = row["AI2_M_QNTY"].AsDecimal();
            worksheet.Rows[rowIndex][li_ole_col+1-1].Value = row["AI2_OI"].AsDecimal();
         }
         //刪除空白列
         if (li_ole_row_tol > rowIndex) {
            worksheet[$"{rowIndex + 2}:{li_ole_row_tol}"].Select();//選取刪除範圍,先多留一行不然公式會出錯
            worksheet.Selection.Delete();
            worksheet.Rows.Remove(rowIndex + 1);//移除多留的那一行
         }
         workbook.SaveDocument(ls_file);
         return true;
      }

      public bool wf_30322()
      {
         /*************************************
         ls_rpt_name = 報表名稱
         ls_rpt_id = 報表代號
         li_ole_col = Excel的Column位置
         li_ole_row_tol = Excel的Column預留數
         ls_ymd = 日期
         *************************************/
         //前月倒數1天交易日
         DateTime ldt_sdate = dao30320.GetLastTradeDate(emMonthText.AsDateTime().ToString("yyyyMM01"));
         //月底
         string ls_ymd = string.Empty;
         if (PbFunc.Right(emMonthText, 2) == "12") {
            ls_ymd = $"{PbFunc.Left(emMonthText,4).AsInt()+1}01";
         }
         else {
            ls_ymd = $"{PbFunc.Left(emMonthText, 4)}{PbFunc.Right($"0{PbFunc.Right(emMonthText,2).AsInt()+1}",2)}";
         }
         DateTime.TryParseExact(ls_ymd, "yyyyMM", null, DateTimeStyles.AllowWhiteSpaces, out DateTime DT);
         DateTime ldt_edate = PbFunc.relativedate(new DateTime(DT.Year,DT.Month,01), -1);
         //讀取資料
         DataTable ids_1 = new D30322().GetData(ldt_sdate, ldt_edate);
         ids_1 = ids_1.Filter("RPT_SEQ_NO > 0");
         if (ids_1.Rows.Count <= 0) {
            MessageDisplay.Info($"{ldt_sdate.ToShortDateString()}～{emMonthText.AsDateTime().ToString("yyyy/MM/31")},30322－指數期貨價量資料,無任何資料!");
            return false;
         }
         //切換Sheet
         Workbook workbook = new Workbook();
         workbook.LoadDocument(ls_file);
         Worksheet worksheet = workbook.Worksheets["Data_30322ab"];
         worksheet.Range["A1"].Select();

         int rowIndex = 1;
         int li_ole_row_tol = rowIndex + 1 + 32;
         ls_ymd = "";
         foreach (DataRow row in ids_1.Rows) {
            if (ls_ymd != row["AI3_DATE"].AsString()) {
               ls_ymd = row["AI3_DATE"].AsString();
               rowIndex = rowIndex + 1;
               worksheet.Rows[rowIndex][1 - 1].Value = row["AI3_DATE"].AsDateTime().ToString("MM/dd");
            }
            int li_ole_col = 0;
            li_ole_col = row["RPT_SEQ_NO"].AsInt();
            worksheet.Rows[rowIndex][li_ole_col-1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
         }
         //刪除空白列
         if (li_ole_row_tol > rowIndex) {
            worksheet[$"{rowIndex + 2}:{li_ole_row_tol - 1}"].Select();//選取刪除範圍,先多留一行不然公式會出錯
            worksheet.Selection.Delete();
            worksheet.Rows.Remove(rowIndex + 1);//移除多留的那一行
         }
         workbook.SaveDocument(ls_file);
         return true;
      }
   }
}
