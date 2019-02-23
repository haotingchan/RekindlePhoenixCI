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
/// 20190218,john,證期局七組月報
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 證期局七組月報
   /// </summary>
   public class B30310
   {
      private AI2 daoAI2;
      private AI3 daoAI3;
      private D30310 dao30310;
      private string ls_file;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30310(string FilePath,string datetime)
      {
         daoAI2 = new AI2();
         daoAI3 = new AI3();
         dao30310 = new D30310();
         ls_file = FilePath;
         emMonthText = datetime;
      }
      /// <summary>
      /// 寫入 30311_1 sheet
      /// </summary>
      /// <param name="ls_kind_id"></param>
      /// <param name="SheetName"></param>
      /// <returns></returns>
      public bool wf_30310_1(string ls_kind_id, string SheetName)
      {
         /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號
            li_ole_col = Excel的Column位置
            li_ole_row_tol = Excel的Column預留數
            ldt_ymd = 日期
         *************************************/
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", ls_kind_id, emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", ls_kind_id, emMonthText);
            //讀取資料
            DataTable ids_1 = dao30310.GetData(ls_kind_id, StartDate, EndDate);
            if (ids_1.Rows.Count <= 0) {
               MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30310－我國臺股期貨契約價量資料,{ls_kind_id}無任何資料!");
               return false;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(ls_file);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldt_ymd = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int li_ole_row_tol = rowIndex + 1 + 32;
            foreach (DataRow row in ids_1.Rows) {
               if (ldt_ymd != row["AI3_DATE"].AsDateTime()) {
                  ldt_ymd = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  worksheet.Rows[rowIndex][1 - 1].Value = ldt_ymd.ToString("MM/dd");
               }
               worksheet.Rows[rowIndex][2-1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[rowIndex][4-1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[rowIndex][5-1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[rowIndex][6-1].Value = row["AI3_INDEX"].AsDecimal();
               if (ls_kind_id == "TXF") {
                  worksheet.Rows[rowIndex][8-1].Value = row["AI3_AMOUNT"].AsDecimal();
               }
            }
            //刪除空白列
            if (li_ole_row_tol > rowIndex) {
               worksheet[$"{rowIndex + 2}:{li_ole_row_tol}"].Select();//選取刪除範圍,先多留一行不然公式會出錯
               worksheet.Selection.Delete();
               worksheet.Rows.Remove(rowIndex + 1);//移除多留的那一行
               worksheet.Range["A1"].Select();
            }
            //表尾
            ids_1 = daoAI2.ListAI2ym(ls_kind_id, EndDate.ToString("yyyyMM"), StartDate.ToString("yyyyMM"));
            if (ids_1.Rows.Count <= 0) {
               return false;
            }

            int li_day_cnt;
            //上月
            rowIndex = rowIndex + 5;
            li_day_cnt = ids_1.Rows[0]["LAST_M_DAY_CNT"].AsInt();
            if (li_day_cnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(ids_1.Rows[0]["LAST_M_QNTY"].AsDecimal() / li_day_cnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(ids_1.Rows[0]["LAST_M_OI"].AsDecimal() / li_day_cnt, 0);
            }
            //本月
            rowIndex = rowIndex - 1;
            li_day_cnt = ids_1.Rows[0]["CUR_M_DAY_CNT"].AsInt();
            if (li_day_cnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(ids_1.Rows[0]["CUR_M_QNTY"].AsDecimal() / li_day_cnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(ids_1.Rows[0]["CUR_M_OI"].AsDecimal() / li_day_cnt, 0);
            }
            rowIndex = rowIndex + 1;
            //今年迄今
            rowIndex = rowIndex + 2;
            li_day_cnt = ids_1.Rows[0]["Y_DAY_CNT"].AsInt();
            if (li_day_cnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(ids_1.Rows[0]["Y_QNTY"].AsDecimal() / li_day_cnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(ids_1.Rows[0]["Y_OI"].AsDecimal() / li_day_cnt, 0);
            }
            workbook.SaveDocument(ls_file);
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "30311_1");
            return false;
         }
         return true;
      }
      /// <summary>
      /// 寫入30311_2(EXF)&30311_3(FXF) sheet
      /// </summary>
      /// <param name="ls_kind_id"></param>
      /// <param name="SheetName"></param>
      /// <returns></returns>
      public bool wf_30310_2(string ls_kind_id, string SheetName)
      {
         /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號
            li_ole_col = Excel的Column位置
            li_ole_row_tol = Excel的Column預留數
            ldt_ymd = 日期
         *************************************/
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", ls_kind_id, emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", ls_kind_id, emMonthText);
            //讀取資料
            DataTable ids_1 = daoAI3.ListAI3(ls_kind_id, StartDate, EndDate);
            if (ids_1.Rows.Count <= 0) {
               MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30310－我國臺股期貨契約價量資料,{ls_kind_id}無任何資料!");
               return false;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(ls_file);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldt_ymd = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int li_ole_row_tol = rowIndex + 1 + 32;
            foreach (DataRow row in ids_1.Rows) {
               if (ldt_ymd != row["AI3_DATE"].AsDateTime()) {
                  ldt_ymd = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  worksheet.Rows[rowIndex][1 - 1].Value = ldt_ymd.ToString("MM/dd");
               }
               worksheet.Rows[rowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[rowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[rowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[rowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
            }
            //刪除空白列
            if (li_ole_row_tol > rowIndex) {
               worksheet[$"{rowIndex + 2}:{li_ole_row_tol}"].Select();//選取刪除範圍,先多留一行不然公式會出錯
               worksheet.Selection.Delete();
               worksheet.Rows.Remove(rowIndex + 1);//移除多留的那一行
               worksheet.Range["A1"].Select();
            }
            //表尾
            ids_1 = daoAI2.ListAI2ym(ls_kind_id, EndDate.ToString("yyyyMM"), StartDate.ToString("yyyyMM"));
            if (ids_1.Rows.Count <= 0) {
               return false;
            }

            int li_day_cnt;
            //上月
            rowIndex = rowIndex + 5;
            li_day_cnt = ids_1.Rows[0]["LAST_M_DAY_CNT"].AsInt();
            if (li_day_cnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(ids_1.Rows[0]["LAST_M_QNTY"].AsDecimal() / li_day_cnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(ids_1.Rows[0]["LAST_M_OI"].AsDecimal() / li_day_cnt, 0);
            }
            //今年迄今
            rowIndex = rowIndex + 2;
            li_day_cnt = ids_1.Rows[0]["Y_DAY_CNT"].AsInt();
            if (li_day_cnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(ids_1.Rows[0]["Y_QNTY"].AsDecimal() / li_day_cnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(ids_1.Rows[0]["Y_OI"].AsDecimal() / li_day_cnt, 0);
            }
            workbook.SaveDocument(ls_file);
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "30311_2");
            return false;
         }
         return true;
      }
      /// <summary>
      /// 寫入 30311_4 sheet
      /// </summary>
      /// <returns></returns>
      public bool wf_30310_4()
      {
         /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號
            li_ole_col = Excel的Column位置
            li_ole_row_tol = Excel的Column預留數
            ldt_ymd = 日期
         *************************************/
         try {
            string ls_kind_id = "MSF";
            string SheetName = "30311_4";
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", ls_kind_id, emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", ls_kind_id, emMonthText);
            //讀取資料
            DataTable ids_1 = daoAI3.ListAI3(ls_kind_id, StartDate, EndDate);
            if (ids_1.Rows.Count <= 0) {
               return false;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(ls_file);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldt_ymd = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int li_ole_row_tol = rowIndex + 1 + 32;
            foreach (DataRow row in ids_1.Rows) {
               if (ldt_ymd != row["AI3_DATE"].AsDateTime()) {
                  ldt_ymd = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  worksheet.Rows[rowIndex][1 - 1].Value = ldt_ymd.ToString("MM/dd");
               }
               worksheet.Rows[rowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[rowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[rowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[rowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
            }
            //刪除空白列
            if (li_ole_row_tol > rowIndex) {
               worksheet[$"{rowIndex + 2}:{li_ole_row_tol}"].Select();//選取刪除範圍,先多留一行不然公式會出錯
               worksheet.Selection.Delete();
               worksheet.Rows.Remove(rowIndex + 1);//移除多留的那一行
               //worksheet.Range["A1"].Select();
            }
            workbook.SaveDocument(ls_file);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "30311_4");
            return false;
         }
      }
   }
}
