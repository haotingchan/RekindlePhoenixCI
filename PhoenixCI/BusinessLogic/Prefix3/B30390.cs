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
/// 20190305,john,台灣五十期貨契約價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 台灣五十期貨契約價量資料
   /// </summary>
   public class B30390
   {
      private AI3 daoAI3;
      private string lsFile;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="DatetimeVal">em_month.Text</param>
      public B30390(string FilePath,string DatetimeVal)
      {
         lsFile = FilePath;
         emMonthText = DatetimeVal;
         daoAI3 = new AI3();
      }
      /// <summary>
      /// wf_30391()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public bool Wf30391(int RowIndex = 1, int RowTotal = 32, string IsKindID = "T5F", string SheetName = "30391", string RptName = "「台灣五十」期貨契約價量資料")
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
            DateTime StartDate = PbFunc.f_get_last_day("AI3", IsKindID, emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", IsKindID, emMonthText);
            //讀取資料
            DataTable dt = daoAI3.ListAI3(IsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info($"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30391－{RptName},{IsKindID}無任何資料!");
               return true;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();

            RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in dt.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");
               }
               worksheet.Rows[RowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[RowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[RowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[RowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
            }

            workbook.SaveDocument(lsFile);
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "30391");
            return false;
         }
         return true;
      }

   }
}
