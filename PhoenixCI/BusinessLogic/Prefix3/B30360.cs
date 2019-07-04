using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190304,john,股票選擇權交易概況表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 股票選擇權交易概況表
   /// </summary>
   public class B30360
   {
      private D30360 dao30360;
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
      public B30360(string FilePath,string DatetimeVal)
      {
         _lsFile = FilePath;
         _emMonthText = DatetimeVal;
         dao30360 = new D30360();
      }
      /// <summary>
      /// 寫入sheet
      /// </summary>
      /// <param name="SheetName"></param>
      /// <param name="Dt"></param>
      /// <param name="RowIndex"></param>
      private void WriteSheet(string SheetName, DataTable Dt, int RowIndex,int RowTotal)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            string lsYMD = "";

            int addRowCount = 0;//總計寫入的行數
            foreach (DataRow row in Dt.Rows) {
               if (lsYMD != row["AI2_YMD"].AsString()) {
                  lsYMD = row["AI2_YMD"].AsString();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  //日期
                  worksheet.Rows[RowIndex][1 - 1].Value = lsYMD.AsDateTime("yyyyMMdd").ToString("MM/dd");
               }
               if (row["AI2_PC_CODE"].AsString() == "C") {
                  worksheet.Rows[RowIndex][2 - 1].Value = row["AI2_M_QNTY"].AsDecimal();//買權成交量
               }
               else {
                  worksheet.Rows[RowIndex][3 - 1].Value = row["AI2_M_QNTY"].AsDecimal();//賣權成交量
               }
               worksheet.Rows[RowIndex][6 - 1].Value = Dt.Compute("sum(AI2_MMK_QNTY)", $@"AI2_YMD='{lsYMD}'").AsDecimal();//造市者交易量
               worksheet.Rows[RowIndex][8 - 1].Value = Dt.Compute("sum(AI2_OI)", $@"AI2_YMD='{lsYMD}'").AsDecimal();//未平倉量
            }
            //刪除空白列
            if (RowTotal > addRowCount) {
               //worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));
            }
            
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }
      }
      /// <summary>
      /// 寫入30362sheet&30367sheet
      /// </summary>
      /// <param name="RowIndex"></param>
      /// <param name="RowTotal"></param>
      /// <param name="SheetName"></param>
      /// <param name="dt"></param>
      private void WriteSheet2(int RowIndex, int RowTotal, string SheetName, DataTable dt)
      {
         //切換Sheet
         Workbook workbook = new Workbook();
         workbook.LoadDocument(_lsFile);
         Worksheet worksheet = workbook.Worksheets[SheetName];
         worksheet.Range["A1"].Select();


         int addRowCount = 0;//總計寫入的行數
         foreach (DataRow row in dt.Rows) {
            string pdkName = row["PDK_NAME"].AsString();
            worksheet.Rows[RowIndex][1 - 1].Value = pdkName + $"({row["KIND_ID_2"].AsString()})";//股票選擇權名稱(現金交割)
            worksheet.Rows[RowIndex][2 - 1].Value = row["M_QNTY"].AsDecimal();//成交量
            worksheet.Rows[RowIndex][4 - 1].Value = pdkName.SubStr(0, pdkName.IndexOf("選擇"));//抓取選擇權前面的字元
            RowIndex = RowIndex + 1;
            addRowCount++;
         }
         RowIndex = RowIndex - 1;
         //刪除空白列
         if (RowTotal > addRowCount) {
            //worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
            worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));
         }
         workbook.SaveDocument(_lsFile);
      }

      /// <summary>
      /// wf_30361() 需確認sheet
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="IsKindID">商品種類</param>
      /// <param name="SheetName">工作表名稱</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30361(int RowIndex=1, int RowTotal=33 ,string IsKindID= "STO", string SheetName= "30361", string RptName= "股票選擇權交易概況表")
      {
         try {
            //當月第1天交易日
            DateTime StartDate = new DateTime(_emMonthText.AsDateTime().Year, _emMonthText.AsDateTime().Month, 01);
            //抓當月最後交易日
            string EndDate = dao30360.GetMaxLastDay30361(StartDate);
            //讀取資料
            DataTable dt = dao30360.Get30361Data(IsKindID, StartDate.ToString("yyyyMMdd"), EndDate);
            if (dt.Rows.Count <= 0) {
               return $"{StartDate.ToShortDateString()}～{EndDate.AsDateTime("yyyyMMdd").ToShortDateString()},{SheetName}-{RptName},無任何資料!";
            }
            //儲存寫入sheet
            WriteSheet(SheetName, dt, RowIndex, RowTotal);
         }
         catch (Exception ex) {
            throw ex;
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30362()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="IsKindID">商品種類</param>
      /// <param name="SheetName">工作表名稱</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30362(int RowIndex = 1, int RowTotal = 50, string IsKindID = "STO", string SheetName = "30362", string RptName = "股票選擇權交易概況表")
      {
         try {
            //讀取資料
            DataTable dt = dao30360.Get30362Data(_emMonthText.AsDateTime(), IsKindID);
            if (dt.Rows.Count <= 0) {
               return $"{_emMonthText.AsDateTime().ToString("yyyyMM")},{SheetName}-{RptName},無任何資料!";
            }
            //儲存寫入sheet
            WriteSheet2(RowIndex, RowTotal, SheetName, dt);
            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
            throw ex;
         }
      }

      /// <summary>
      /// wf_30363()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="IsKindID">商品種類</param>
      /// <param name="SheetName">工作表名稱</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30363(int RowIndex = 1, int RowTotal = 32, string IsKindID = "STO", string SheetName = "30363", string RptName = "股票選擇權交易概況表")
      {
         Workbook workbook = new Workbook();
         try {
            //當月第1天交易日
            DateTime StartDate = new DateTime(_emMonthText.AsDateTime().Year, _emMonthText.AsDateTime().Month, 01);
            //抓當月最後交易日
            string EndDate = dao30360.GetMaxLastDay30361(StartDate);
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            //讀取資料
            DataTable dt = dao30360.Get30363Data(StartDate.ToString("yyyyMMdd"), EndDate, IsKindID);
            if (dt.Rows.Count <= 0) {
               return $"{_emMonthText.AsDateTime().ToString("yyyyMM")},{SheetName}－{RptName},無任何資料!";
            }
            //商品
            DataTable dtProd = dao30360.Get30363KindID2Data(StartDate.ToString("yyyyMMdd"), EndDate, IsKindID);

            //表頭
            int columnIndex = 1;//Excel的Column位置
            foreach (DataRow row in dtProd.Rows) {
               string kindID = row["PDK_NAME"].AsString();
               worksheet.Rows[0][columnIndex].Value = kindID.SubStr(0, kindID.IndexOf("選擇權")) + $"({row["KIND_ID_2"].AsString()})";
               worksheet.Rows[1][columnIndex].Value = "買權";
               worksheet.Rows[1][columnIndex + 1].Value = "賣權";
               columnIndex = columnIndex + 2;
            }
            //內容
            int addRowCount = 0;//總計寫入的行數
            string lsYMD = "";
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AI2_YMD"].AsString()) {
                  lsYMD = row["AI2_YMD"].AsString();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = lsYMD.AsDateTime("yyyyMMdd").ToString("MM/dd");
               }

               int foundIndex = dtProd.Rows.IndexOf(dtProd.Select($@"KIND_ID_2 ='{ row["AI2_KIND_ID_2"].AsString() }'").FirstOrDefault());
               if (foundIndex > -1) {
                  foundIndex = foundIndex * 2;
                  if (row["AI2_PC_CODE"].AsString() != "C") {
                     foundIndex = foundIndex + 1;
                  }
                  worksheet.Rows[RowIndex][foundIndex+1].Value = row["AI2_M_QNTY"].AsDecimal();//從第二欄開始寫
               }

            }//foreach (DataRow row in dt.Rows)
            //刪除空白列
            if (RowTotal > addRowCount) {
               //worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));
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
      /// wf_30366()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="IsKindID">商品種類</param>
      /// <param name="SheetName">工作表名稱</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30366(int RowIndex = 1, int RowTotal = 33, string SheetName = "30366", string RptName = "股票選擇權交易概況表")
      {
         try {
            //當月第1天交易日
            DateTime StartDate = new DateTime(_emMonthText.AsDateTime().Year, _emMonthText.AsDateTime().Month, 01);
            //抓當月最後交易日
            DateTime EndDate = new DateTime(_emMonthText.AsDateTime().Year, _emMonthText.AsDateTime().Month, 31);
            string lastDate=dao30360.GetMaxLastDay30366(StartDate, EndDate);
            //讀取資料
            DataTable dt = dao30360.Get30366Data("O", StartDate.ToString("yyyyMMdd"), lastDate);
            if (dt.Rows.Count <= 0) {
               return $"{StartDate.ToShortDateString()}～{lastDate.AsDateTime("yyyyMMdd").ToShortDateString()},{SheetName}－{RptName},無任何資料!";
            }
            //儲存寫入sheet
            WriteSheet(SheetName, dt, RowIndex, RowTotal);
         }
         catch (Exception ex) {
            throw ex;
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30367()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="SheetName">工作表名稱</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30367(int RowIndex = 1, int RowTotal = 100, string SheetName = "30367", string RptName = "股票選擇權交易概況表")
      {
         try {
            //讀取資料
            DataTable dt = dao30360.Get30367Data(_emMonthText.AsDateTime());
            if (dt.Rows.Count <= 0) {
               return $"{_emMonthText.AsDateTime().ToString("yyyyMM")},{SheetName}－{RptName},無任何資料!";
            }
            //儲存寫入sheet
            WriteSheet2(RowIndex, RowTotal, SheetName, dt);
         }
         catch (Exception ex) {
            throw ex;
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30368()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <param name="IsKindID">商品種類</param>
      /// <param name="SheetName">工作表名稱</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30368(int RowIndex = 1, int RowTotal = 32, string IsKindID = "STC", string SheetName = "30368", string RptName = "股票選擇權交易概況表")
      {
         Workbook workbook = new Workbook();
         try {
            //當月第1天交易日
            DateTime StartDate = new DateTime(_emMonthText.AsDateTime().Year, _emMonthText.AsDateTime().Month, 01);
            //抓當月最後交易日
            string EndDate = dao30360.GetMaxLastDay30361(StartDate);
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            //讀取資料
            DataTable dt = dao30360.Get30368Data(StartDate.ToString("yyyyMMdd"), EndDate);
            if (dt.Rows.Count <= 0) {
               return $"{_emMonthText.AsDateTime().ToString("yyyyMM")},{SheetName}－{RptName},無任何資料!";
            }
            
            //內容
            int addRowCount = 0;//總計寫入的行數
            string lsYMD = "";
            RowIndex = 2 - 1;
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["DATA_YMD"].AsString()) {
                  lsYMD = row["DATA_YMD"].AsString();
                  RowIndex = RowIndex + 1;
                  addRowCount++;
                  worksheet.Rows[RowIndex][1 - 1].Value = lsYMD.AsDateTime("yyyyMMdd").ToString("MM/dd");
               }
               int columnIndex= row["SEQ_NO"].AsInt() * 2 - 1;//有合併欄位跳一欄

               //首筆
               if (RowIndex == 2) {
                  string kindID = row["PDK_NAME"].AsString();
                  worksheet.Rows[0][columnIndex].Value = kindID.SubStr(0, kindID.IndexOf("選擇權")) + $"({row["DATA_KIND_ID_2"].AsString()})";//抓取選擇權前面的字元+(DATA_KIND_ID_2)
                  worksheet.Rows[1][columnIndex].Value = "買權";
                  worksheet.Rows[1][columnIndex + 1].Value = "賣權";
               }

               //成交量
               if (row["AI2_PC_CODE"].AsString() == "C") {
                  worksheet.Rows[RowIndex][columnIndex].Value = row["AI2_M_QNTY"].AsDecimal();//賣權
               }
               else {
                  worksheet.Rows[RowIndex][columnIndex+1].Value = row["AI2_M_QNTY"].AsDecimal();//買權
               }
            }//foreach (DataRow row in dt.Rows)

            //刪除空白列
            if (RowTotal > addRowCount) {
               //worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + (RowTotal - addRowCount));
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
