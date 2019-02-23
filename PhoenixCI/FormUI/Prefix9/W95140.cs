using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Repository;

/// <summary>
/// Winni, 2019/1/16
/// </summary>
namespace PhoenixCI.FormUI.Prefix9 {
   /// <summary>
   /// 95140 Eurex商品OI轉入帳號檢核錯誤(訊息代碼9514)統計表 
   /// </summary>
   public partial class W95140 : FormParent {

      protected enum ReportType {
         mon, date, detail
      }

      protected enum SheetNo {
         mon = 0,
         date = 1,
         detail = 2
      }

      private D95140 dao95140;

      public W95140(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao95140 = new D95140();

         txtStartMth.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/01");//2018/01
         txtEndMth.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM");//2018/06
         txtStartDate.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
         txtEndDate.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");

         txtStartMth.EnterMoveNextControl = true;
         txtEndMth.EnterMoveNextControl = true;
         txtStartDate.EnterMoveNextControl = true;
         txtEndDate.EnterMoveNextControl = true;
         chkMon.EnterMoveNextControl = true;
         chkFcm.EnterMoveNextControl = true;
         chkAcc.EnterMoveNextControl = true;

         chkMon.Checked = true;

         //winni test
         txtStartMth.EditValue = "2015/04";
         txtEndMth.EditValue = "2015/05";
         txtStartDate.EditValue = "2015/04/20";
         txtEndDate.EditValue = "2015/04/25";
      }


      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         //1.檢查日期欄位是否為正常日期


         lblProcessing.Visible = true;

         //2.複製檔案 & 開啟檔案 (因為三張報表都輸出到同一份excel,所以提出來)
         string excelDestinationPath = CopyExcelTemplateFile(_ProgramID , FileType.XLS);
         Workbook workbook = new Workbook();
         workbook.LoadDocument(excelDestinationPath);

         //3.export data (獨立,最多輸出三個sheet)
         bool result1 = false, result2 = false, result3 = false;
         if (chkMon.Checked) {
            result1 = wf_Export(workbook , SheetNo.mon , ReportType.mon ,
                              "月計總表" , 1000 , 3 , txtStartMth.DateTimeValue , txtEndMth.DateTimeValue);
         }

         if (chkFcm.Checked) {
            result2 = wf_Export(workbook , SheetNo.date , ReportType.date ,
                              "日計明細表_by期貨商" , 1000 , 3 , txtStartDate.DateTimeValue , txtEndDate.DateTimeValue);
         }

         if (chkAcc.Checked) {
            result3 = wf_Export(workbook , SheetNo.detail , ReportType.detail ,
                             "日計明細表_by交易人帳號" , 1000 , 4 , txtStartDate.DateTimeValue , txtEndDate.DateTimeValue);
         }

         if (!result1 && !result2 && !result3) {
            try {
               workbook = null;
               System.IO.File.Delete(excelDestinationPath);
            } catch (Exception) {
               //
            }
            return ResultStatus.Fail;
         }

         //4.存檔並關閉
         workbook.SaveDocument(excelDestinationPath);
         return ResultStatus.Success;

      }


      /// <summary>
      /// 根據報表類型,轉出excel (其實三張報表都輸出到同一份excel)
      /// </summary>
      /// <param name="sheetNo"></param>
      /// <param name="reportType"></param>
      /// <param name="ls_rpt_name">sheet標題中文</param>
      /// <param name="rowTotalCount">空白行的筆數</param>
      /// <param name="startRowIndex">起始插入資料的row index</param>
      /// <param name="startDate">根據reportType傳入[起始年月]或[起始年月日]</param>
      /// <param name="endDate">根據reportType傳入[結束年月]或[結束年月日]</param>
      /// <returns></returns>
      protected bool wf_Export(Workbook workbook , SheetNo sheetNo , ReportType reportType ,
                                 string ls_rpt_name , int rowTotalCount , int startRowIndex ,
                                 DateTime startDate , DateTime endDate) {

         try {
            string tmpStart = (reportType == ReportType.mon ? startDate.ToString("yyyy/MM") : startDate.ToString("yyyy/MM/dd"));
            string tmpEnd = (reportType == ReportType.mon ? endDate.ToString("yyyy/MM") : endDate.ToString("yyyy/MM/dd"));

            //1.讀取資料
            DataTable dtTemp = new DataTable();
            switch (reportType) {
               case ReportType.mon:
                  dtTemp = dao95140.ListMonth(startDate.ToString("yyyyMM") , endDate.ToString("yyyyMM"));
                  break;
               case ReportType.date:
                  dtTemp = dao95140.ListDateFcm(startDate.ToString("yyyyMMdd") , endDate.ToString("yyyyMMdd"));
                  break;
               case ReportType.detail:
                  dtTemp = dao95140.ListDateAcc(startDate.ToString("yyyyMMdd") , endDate.ToString("yyyyMMdd"));
                  break;
            }

            //1.1 check row count
            if (dtTemp.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , tmpStart + "~" + tmpEnd , _ProgramID + "－" + ls_rpt_name));
               return false;
            }



            //2.切換Sheet
            Worksheet sheet1 = workbook.Worksheets[(int)sheetNo];

            //2.1 設定標題
            string titleDateDesc = "";
            if (startDate == endDate) {
               titleDateDesc = tmpStart;
            } else {
               titleDateDesc = tmpStart + "~" + tmpEnd;
            }
            sheet1.Cells[0 , 0].Value = titleDateDesc + sheet1.Cells[0 , 0].Value;

            //3.匯出資料(因為單純輸出dataTable,可直接用函數sheet.Import(DataTable,isNeedTitle,rowIndex,colIndex)
            sheet1.Import(dtTemp , false , startRowIndex , 0);

            //4.刪除空白列
            if (dtTemp.Rows.Count <= rowTotalCount) {
               sheet1.Rows.Remove(startRowIndex + dtTemp.Rows.Count , rowTotalCount - dtTemp.Rows.Count);
            }

            //4.1 把指標移到[A1]
            sheet1.Range["A1"].Select();
            sheet1.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            PbFunc.f_write_logf(_ProgramID , "error" , ex.Message);
            return false;
         }

      }


   }
}