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
/// 20190307,john,匯率類期貨契約價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 匯率類期貨契約價量資料
   /// </summary>
   public class B30393
   {
      private string lsFile;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30393(string FilePath,string datetime)
      {
         lsFile = FilePath;
         emMonthText = datetime;
      }

      private static int IDFGtype(DataRow row)
      {
         int columnIndex = 0;
         switch (row["AM2_IDFG_TYPE"].AsString()) {
            case "1":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 2 : 3) - 1;
               break;
            case "2":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 4 : 5) - 1;
               break;
            case "3":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 6 : 7) - 1;
               break;
            case "5":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 8 : 9) - 1;
               break;
            case "6":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 10 : 11) - 1;
               break;
            case "8":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 12 : 13) - 1;
               break;
            case "7":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 14 : 15) - 1;
               break;
         }

         return columnIndex;
      }

      /// <summary>
      /// wf_30393_1 
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public bool Wf30393(string IsKindID,string SheetName, int RowIndex=1, int RowTotal=33)
      {
         /*************************************
         ls_rpt_name = 報表名稱
         ls_rpt_id = 報表代號
         RowIndex = Excel的Row位置
         columnIndex = Excel的Column位置
         RowTotal = Excel的Column預留數
         lsYMD = 日期
         *************************************/
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", "RHF", emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", "RHF", emMonthText);
            
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();
            int addRowCount = 0;//總計寫入的行數
            //讀取資料
            DataTable dtAI3 = new AI3().ListAI3(IsKindID, StartDate, EndDate);

            foreach (DataRow row in dtAI3.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");
               }
               decimal closePrice = row["AI3_CLOSE_PRICE"].AsDecimal();
               worksheet.Rows[RowIndex][2 - 1].Value = closePrice;

               //TODO PB這段有問題，實際上就算執行也不會覆蓋Excel的值，但C#會把公式覆蓋掉
               //decimal lastClosePrice = row["AI3_LAST_CLOSE_PRICE"].AsDecimal();
               //if (!string.IsNullOrEmpty(lastClosePrice.AsString())) {
               //   worksheet.Rows[RowIndex][3 - 1].Value = closePrice - lastClosePrice;
               //}
               worksheet.Rows[RowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();
               worksheet.Rows[RowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();
               worksheet.Rows[RowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               //int newRowIndex = RowIndex + (RowTotal - addRowCount);
               //worksheet.Rows.Hide(RowIndex + 1, newRowIndex);
               //RowIndex= newRowIndex;//沒有隱藏的下一行
            }

            //表尾
            DataTable dtAI2 = new AI2().ListAI2ym(IsKindID, EndDate.ToString("yyyyMM"), StartDate.ToString("yyyyMM"));
            if (dtAI2.Rows.Count <= 0) {
               return true;
            }

            int liDayCnt;
            //上月
            RowIndex = RowIndex + 5;
            liDayCnt = dtAI2.Rows[0]["LAST_M_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[RowIndex][5 - 1].Value = Math.Round(dtAI2.Rows[0]["LAST_M_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[RowIndex][7 - 1].Value = Math.Round(dtAI2.Rows[0]["LAST_M_OI"].AsDecimal() / liDayCnt, 0);
            }
            //今年迄今
            RowIndex = RowIndex + 2;
            liDayCnt = dtAI2.Rows[0]["Y_DAY_CNT"].AsInt();
            if (liDayCnt > 0) {
               worksheet.Rows[RowIndex][5 - 1].Value = Math.Round(dtAI2.Rows[0]["Y_QNTY"].AsDecimal() / liDayCnt, 0);
               worksheet.Rows[RowIndex][7 - 1].Value = Math.Round(dtAI2.Rows[0]["Y_OI"].AsDecimal() / liDayCnt, 0);
            }

            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "Wf30393");
            return false;
         }
      }

      /// <summary>
      /// wf_30393_1abc
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public bool Wf30393abc(string IsKindID, string SheetName, int RowIndex = 3, int RowTotal = 12)
      {
         /*************************************
         ls_rpt_name = 報表名稱 
         ls_rpt_id = 報表代號
         RowIndex = Excel的Row位置
         columnIndex = Excel的Column位置
         RowTotal = Excel的Column預留數
         ii_ole_y_row_tol = Excel年部份的Column預留數
         li_month_cnt = Excel的月份個數
         lsYMD = 日期
         ls_end_ymd = 最後一筆日期
         *************************************/
         try {
            
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime ldtYMD = new DateTime(1900, 1, 1);
            worksheet.Range["A1"].Select();
            //總列數
            int sumRowIndex = RowTotal + RowIndex + 1;//小計行數
            int addRowCount = 0;//總計寫入的行數
            worksheet.Rows[sumRowIndex][1 - 1].Value = $"{PbFunc.Left(emMonthText, 4).AsInt() - 1911}小計";
            string lsYMD = "";
            //讀取資料
            DataTable dt = new AM2().ListAM2(IsKindID, $"{PbFunc.Left(emMonthText, 4)}01", emMonthText.Replace("/", ""));

            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AM2_YMD"].AsString()) {
                  RowIndex = RowIndex + 1;
                  lsYMD = row["AM2_YMD"].AsString();
                  //li_month_cnt = li_month_cnt + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = $"{PbFunc.Left(lsYMD, 4).AsInt() - 1911}/{PbFunc.Right(lsYMD, 2)}";
               }
               //判斷欄位
               int columnIndex = IDFGtype(row);

               worksheet.Rows[RowIndex][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               //worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));
            }
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "Wf30393abc");
            return false;
         }
      }

      
   }
}
