using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
/// <summary>
/// 20190305,john,新加坡交易所(SGX)摩根臺股期貨市場概況表 
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 新加坡交易所(SGX)摩根臺股期貨市場概況表 
   /// </summary>
   public class B30380
   {
      /// <summary>
      /// 檔案輸出路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 交易日期 月份
      /// </summary>
      private readonly string _emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="DatetimeVal">em_month.Text</param>
      public B30380(string FilePath, string DatetimeVal)
      {
         _lsFile = FilePath;
         _emMonthText = DatetimeVal;
      }
      /// <summary>
      /// wf_30311()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30311(int RowIndex = 1, int RowTotal = 32, string IsKindID = "TXF", string SheetName = "30311", string RptName = "當年每月日均量統計表")
      {
         Workbook workbook = new Workbook();
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", IsKindID, _emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", IsKindID, _emMonthText);
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            //讀取資料
            DataTable dt = new D30310().GetData(IsKindID, StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               return $"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30311－{RptName},{IsKindID}無任何資料!";
            }

            DateTime ldtYMD = new DateTime(1900, 1, 1);

            RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in dt.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");//日期
               }
               worksheet.Rows[RowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();//臺股期貨指數(TX)
               worksheet.Rows[RowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();//臺股期貨總成交量(註①)
               worksheet.Rows[RowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();//臺股期貨總未平倉量(註①)
               worksheet.Rows[RowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();//臺股現貨指數(TAIEX)
               worksheet.Rows[RowIndex][8 - 1].Value = row["AI3_AMOUNT"].AsDecimal();//成交值      (億元)
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Range[$"{RowIndex + 2}:{RowTotal + 2}"].Clear();//清空沒有必要的數值
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));//隱藏代替刪除
            }

         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30381()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30381(int RowIndex = 1, int RowTotal = 32, string IsKindID = "STW", string SheetName = "30381", string RptName = "新加坡交易所(SGX)摩根臺股期貨市場概況表")
      {
         Workbook workbook = new Workbook();
         try {
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", IsKindID, _emMonthText, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", IsKindID, _emMonthText);

            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];

            //讀取資料
            DataTable dt = new D30380().GetData(StartDate, EndDate);
            if (dt.Rows.Count <= 0) {
               return $"{StartDate.ToShortDateString()}～{EndDate.ToShortDateString()},30311－{RptName},{IsKindID}無任何資料!";
            }

            DateTime ldtYMD = new DateTime(1900, 1, 1);

            RowTotal = 32 + 1;//Excel的Column預留數 預留顯示32行加上隱藏的1行
            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in dt.Rows) {
               if (ldtYMD != row["AI3_DATE"].AsDateTime()) {
                  ldtYMD = row["AI3_DATE"].AsDateTime();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = ldtYMD.ToString("MM/dd");//日期
                  worksheet.Rows[RowIndex][4 - 1].Value = row["AI3_M_QNTY"].AsDecimal();//總成交量
                  worksheet.Rows[RowIndex][5 - 1].Value = row["AI3_OI"].AsDecimal();//總未平倉量
               }
               worksheet.Rows[RowIndex][2 - 1].Value = row["AI3_CLOSE_PRICE"].AsDecimal();//期貨指數
               worksheet.Rows[RowIndex][6 - 1].Value = row["AI3_INDEX"].AsDecimal();//現貨指數
               worksheet.Rows[RowIndex][10 - 1].Value = row["AI3_M_QNTY_FITX"].AsDecimal();//臺指量
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               worksheet.Range[$"{RowIndex + 2}:{RowTotal + 2}"].Clear();//清空沒有必要的數值
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));//隱藏代替刪除
            }

         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }
   }
}
