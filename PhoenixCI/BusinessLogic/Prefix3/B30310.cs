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
      private string lsFile;
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
         lsFile = FilePath;
         emMonthText = datetime;
      }
      /// <summary>
      /// 寫入 30311_1 sheet
      /// </summary>
      /// <param name="lsKindID"></param>
      /// <param name="SheetName"></param>
      /// <returns></returns>
      public bool Wf30310one(string lsKindID, string SheetName)
      {
         /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號
            rowIndex = Excel的Row位置
            li_ole_col = Excel的Column位置
            RowTotal = Excel的Column預留數
            ldtYMD = 日期
         *************************************/
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", lsKindID, emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", lsKindID, emMonthText);
            //讀取資料
            DataTable dt = dao30310.GetData(lsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30310－我國臺股期貨契約價量資料,{lsKindID}無任何資料!");
               return true;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int RowTotal = 32+1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in dt.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[rowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");
               }
               worksheet.Rows[rowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[rowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[rowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[rowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
               if (lsKindID == "TXF") {
                  worksheet.Rows[rowIndex][8 - 1].Value = row["AI3_AMOUNT"].AsDecimal();
               }
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               worksheet.Range["A1"].Select();
            }
            //表尾
            dt = daoAI2.ListAI2ym(lsKindID, EndDate.ToString("yyyyMM"), StartDate.ToString("yyyyMM"));
            if (dt.Rows.Count <= 0) {
               return true;
            }

            int liDayCnt;
            //上月
            rowIndex = rowIndex + 5;
            liDayCnt = dt.Rows[0]["LAST_M_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(dt.Rows[0]["LAST_M_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(dt.Rows[0]["LAST_M_OI"].AsDecimal() / liDayCnt, 0);
            }
            //本月
            rowIndex = rowIndex - 1;
            liDayCnt = dt.Rows[0]["CUR_M_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(dt.Rows[0]["CUR_M_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(dt.Rows[0]["CUR_M_OI"].AsDecimal() / liDayCnt, 0);
            }
            rowIndex = rowIndex + 1;
            //今年迄今
            rowIndex = rowIndex + 2;
            liDayCnt = dt.Rows[0]["Y_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(dt.Rows[0]["Y_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(dt.Rows[0]["Y_OI"].AsDecimal() / liDayCnt, 0);
            }
            
            workbook.SaveDocument(lsFile);
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
      /// <param name="lsKindID"></param>
      /// <param name="SheetName"></param>
      /// <returns></returns>
      public bool Wf30310two(string lsKindID, string SheetName)
      {
         /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號
            rowIndex = Excel的Row位置
            li_ole_col = Excel的Column位置
            RowTotal = Excel的Column預留數
            ldtYMD = 日期
         *************************************/
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", lsKindID, emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", lsKindID, emMonthText);
            //讀取資料
            DataTable dt = daoAI3.ListAI3(lsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30310－我國臺股期貨契約價量資料,{lsKindID}無任何資料!");
               return true;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in dt.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[rowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");
               }
               worksheet.Rows[rowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[rowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[rowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[rowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
               worksheet.Range["A1"].Select();
            }
            //表尾
            dt = daoAI2.ListAI2ym(lsKindID, EndDate.ToString("yyyyMM"), StartDate.ToString("yyyyMM"));
            if (dt.Rows.Count <= 0) {
               return true;
            }

            int liDayCnt;
            //上月
            rowIndex = rowIndex + 5;
            liDayCnt = dt.Rows[0]["LAST_M_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(dt.Rows[0]["LAST_M_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(dt.Rows[0]["LAST_M_OI"].AsDecimal() / liDayCnt, 0);
            }
            //今年迄今
            rowIndex = rowIndex + 2;
            liDayCnt = dt.Rows[0]["Y_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[rowIndex][5 - 1].Value = Math.Round(dt.Rows[0]["Y_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[rowIndex][7 - 1].Value = Math.Round(dt.Rows[0]["Y_OI"].AsDecimal() / liDayCnt, 0);
            }
            workbook.SaveDocument(lsFile);
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
      public bool Wf30310four()
      {
         /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號
            rowIndex = Excel的Row位置
            li_ole_col = Excel的Column位置
            RowTotal = Excel的Column預留數
            ldtYMD = 日期
         *************************************/
         try {
            string lsKindID = "MSF";
            string SheetName = "30311_4";
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", lsKindID, emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", lsKindID, emMonthText);
            //讀取資料
            DataTable dt = daoAI3.ListAI3(lsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               return true;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            int rowIndex = 1;
            int RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in dt.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  rowIndex = rowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[rowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");
               }
               worksheet.Rows[rowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[rowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[rowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[rowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(rowIndex + 1, RowTotal - addRowCount);
            }
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "30311_4");
            return false;
         }
      }
   }
}
