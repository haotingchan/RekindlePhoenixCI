using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190320,john,標的證券為受益憑證之上市證券保證金狀況表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 標的證券為受益憑證之上市證券保證金狀況表
   /// </summary>
   public class B43030
   {
      private readonly string _lsFile;
      private readonly string _emDateText;
      private D43030 dao43030;
      private Workbook _workbook;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      public B43030(string FilePath, string emDate)
      {
         _lsFile = FilePath;
         _emDateText = emDate;
         dao43030 = new D43030();
         _workbook = new Workbook();
      }
      /// <summary>
      /// wf_43030_f(),wf_43030_o()
      /// </summary>
      /// <returns></returns>
      public string Wf43030()
      {
         string msgTxt = "";
         try {
            //讀取資料(當日保證金適用比例)
            DataTable dtFuture = dao43030.GetListF(_emDateText.Replace("/", ""));
            msgTxt = WriteExcel(dtFuture, 4, "future");
            DataTable dtOption = dao43030.GetListO(_emDateText.Replace("/", ""));
            msgTxt = WriteExcel(dtOption, 5, "option");
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf43030:" + ex.Message);
#else
            throw ex;
#endif
         }
         return msgTxt;
      }

      /// <summary>
      /// 寫入資料
      /// </summary>
      /// <param name="RowStart">Excel的Row位置</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      private string WriteExcel(DataTable dt, int RowStart, string SheetName, string RptName = "上市證券保證金概況表")
      {
         try {
            if (dt.Rows.Count <= 0) {
               return $"{_emDateText},43030－{RptName},讀取「當日上市證券保證金適用比例」無任何資料!";
            }
            //切換Sheet
            _workbook.LoadDocument(_lsFile);
            Worksheet worksheet = _workbook.Worksheets[SheetName];
            worksheet.Cells["A3"].Value = worksheet.Cells["A3"].Value + string.Format("{0:D}", _emDateText.AsDateTime());
            worksheet.Range["A1"].Select();

            int sidCount = 0;
            string lsSid = string.Empty;
            int addRowCount;//總計寫入的行數
            for (addRowCount = 0; addRowCount < dt.Rows.Count; addRowCount++) {
               DataRow row = dt.Rows[addRowCount];

               if (lsSid != row["MGR5E_SID"].AsString()) {
                  lsSid = row["MGR5E_SID"].AsString();
                  sidCount = sidCount + 1;
                  Cell excelVal = worksheet.Rows[addRowCount + RowStart][1 - 1];
                  if ((row["MGR5E_CP_STATUS_CODE"].AsString() != "N")) {
                     excelVal.Value = $"*{sidCount}";
                  }
                  else {
                     excelVal.Value = sidCount;
                  }
               }
               for (int liCol = 2; liCol <= 16; liCol++) {
                  worksheet.Rows[addRowCount + RowStart][liCol - 1].SetValue(dt.Rows[addRowCount][liCol - 1]);
               }
            }
            //刪除空白列
            if (dt.Rows.Count < 300) {
               //iole_1.application.Rows(string(li_row_start+ids_1.rowcount()+1)+":"+string(li_row_start+300)).Select
               worksheet.Range[$"{RowStart + dt.Rows.Count + 1}:{RowStart + 300}"].Delete(DeleteMode.EntireRow);
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }

            _workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WriteExcel:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

   }
}
