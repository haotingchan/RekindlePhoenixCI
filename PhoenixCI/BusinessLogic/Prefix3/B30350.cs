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
/// 20190223,john,臺指選擇權成交量及未平倉量變化表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 臺指選擇權成交量及未平倉量變化表
   /// </summary>
   public class B30350
   {
      private D30350 dao30350;
      private string lsFile;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="DatetimeVal">em_month.Text</param>
      public B30350(string FilePath,string DatetimeVal)
      {
         lsFile = FilePath;
         emMonthText = DatetimeVal;
         dao30350 = new D30350();
      }
      /// <summary>
      /// 寫入sheet
      /// </summary>
      /// <param name="SheetName"></param>
      /// <param name="Dt"></param>
      /// <param name="RowIndex"></param>
      private void WriteSheet(string SheetName, DataTable Dt, int RowIndex,int RowTotal)
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            string lsYMD = "";
            worksheet.Range["A1"].Select();


            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in Dt.Rows) {
               if (lsYMD != row["AI2_YMD"].AsString()) {
                  lsYMD = row["AI2_YMD"].AsString();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = lsYMD.AsDateTime("yyyyMMdd").ToString("MM/dd");
               }
               if (row["AI2_PC_CODE"].AsString() == "C") {
                  worksheet.Rows[RowIndex][2 - 1].Value = row["AI2_M_QNTY"].AsDecimal();
               }
               else {
                  worksheet.Rows[RowIndex][3 - 1].Value = row["AI2_M_QNTY"].AsDecimal();
               }
               worksheet.Rows[RowIndex][6 - 1].Value = Dt.Compute("sum(AI2_MMK_QNTY)", $@"AI2_YMD='{lsYMD}'").AsDecimal();
               worksheet.Rows[RowIndex][8 - 1].Value = Dt.Compute("sum(AI2_OI)", $@"AI2_YMD='{lsYMD}'").AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
            }
            workbook.SaveDocument(lsFile);
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, $"B30350-WriteSheet");
         }
      }
      /// <summary>
      /// RowIndex判斷
      /// </summary>
      /// <param name="RowIndex"></param>
      /// <param name="StartDate"></param>
      /// <param name="dt"></param>
      /// <param name="condition"></param>
      /// <returns></returns>
      private int ConditionRowIndex(int RowIndex, DateTime StartDate, DataTable dt, Condition30350 condition)
      {
         switch (condition) {
            case Condition30350.sheet30350four:
               //與輸入月份相同則空一列
               if (StartDate.ToString("yyyy/MM") == emMonthText) {
                  RowIndex = RowIndex + 1;
               }
               //沒有前月份,則空一列
               //TODO 這段怪怪的,照PB邏輯這條件都會是true,改寫成直接+1
               RowIndex = RowIndex + 1;
               break;
            case Condition30350.RowIndexAddOne:
               //沒有前月份,則空一列
               //TODO 這段怪怪的,照PB邏輯這條件都會是true,改寫成直接+1
               RowIndex = RowIndex + 1;
               break;
            case Condition30350.NoLastDay:
               //無上月最後1天資料
               if (dt.Rows.Count > 0 && StartDate.ToString("yyyyMMdd") != dt.Rows[0]["AI2_YMD"].AsString()) {
                  RowIndex = RowIndex + 1;
               }
               break;
            case Condition30350.NoLastMonth:
               //沒有前月份,則空一列
               if (dt.Rows.Count > 0 && dt.Rows[0]["AI2_YMD"].AsString().SubStr(0,6) != StartDate.ToString("yyyyMM")) {
                  RowIndex = RowIndex + 1;
               }
               break;
            case Condition30350.NoCondition:
               break;
         }
         return RowIndex;
      }
      /// <summary>
      /// 寫入 30351 datawindow 資料源
      /// </summary>
      /// <param name="RowIndex">起始行位</param>
      /// <param name="RowTotal">預留要使用的行數</param>
      /// <param name="IsKindID">KindID</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">執行作業名稱</param>
      /// <param name="LastMonth">判斷沒有前月份,則空一列</param>
      /// <param name="SameYYYYMM">判斷與輸入月份相同則空一列</param>
      /// <returns></returns>
      public bool DataFrom30351(int RowIndex, int RowTotal ,string IsKindID, string SheetName, string RptName, Condition30350 condition= Condition30350.NoCondition)
      {
         /*************************************
         rowIndex = Excel的Row位置
         columnIndex = Excel的Column位置
         RowTotal = Excel的Column預留數
         lsYMD = 日期
         *************************************/
         try {
            //前月倒數1天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI2", "TXO", emMonthText, 1);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI2", "TXO", emMonthText);
            //讀取資料
            DataTable dt = dao30350.Get30351Data(IsKindID, StartDate.ToString("yyyyMMdd"), EndDate.ToString("yyyyMMdd"));
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},{SheetName}－{RptName},{IsKindID}無任何資料!");
               return true;
            }
            //行數寫入起始條件
            RowIndex = ConditionRowIndex(RowIndex, StartDate, dt, condition);
            //儲存寫入sheet
            WriteSheet(SheetName, dt, RowIndex, RowTotal);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, $"DataFrom30351-{SheetName}");
            return false;
         }
      }
      /// <summary>
      /// 寫入 30358 datawindow 資料源
      /// </summary>
      /// <param name="RowIndex">起始行位</param>
      /// <param name="RowTotal">預留要使用的行數</param>
      /// <param name="IsKindID">KindID</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">執行作業名稱</param>
      /// <returns></returns>
      public bool DataFrom30358(int RowIndex, int RowTotal, string IsKindID, string SheetName, string RptName,Condition30350 Condition= Condition30350.NoCondition)
      {
         /*************************************
         rowIndex = Excel的Row位置
         columnIndex = Excel的Column位置
         RowTotal = Excel的Column預留數
         lsYMD = 日期
         *************************************/
         try {
            //前月倒數1天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI2", "TXO", emMonthText, 1);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI2", "TXO", emMonthText);
            //讀取資料
            DataTable dt = dao30350.Get30358Data(IsKindID, StartDate.ToString("yyyyMMdd"), EndDate.ToString("yyyyMMdd"));
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},{SheetName}－{RptName},{IsKindID}無任何資料!");
               return true;
            }
            //行數寫入起始條件
            RowIndex = ConditionRowIndex(RowIndex, StartDate, dt, Condition);
            //儲存寫入sheet
            WriteSheet(SheetName, dt, RowIndex, RowTotal);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, $"DataFrom30358-{SheetName}");
            return false;
         }
      }
      /// <summary>
      /// 行數寫入起始條件
      /// </summary>
      public enum Condition30350
      {
         /// <summary>
         /// 只有工作表30350_04會用到這條件
         /// </summary>
         sheet30350four,
         /// <summary>
         /// 沒有前月份,則空一列 這段怪怪的,照PB邏輯這條件都會是true,改寫成直接+1
         /// </summary>
         RowIndexAddOne,
         /// <summary>
         /// 無上月最後1天資料
         /// </summary>
         NoLastDay,
         /// <summary>
         /// 沒有前月份,則空一列
         /// </summary>
         NoLastMonth,
         /// <summary>
         /// 沒有條件
         /// </summary>
         NoCondition
      }
   }
}
