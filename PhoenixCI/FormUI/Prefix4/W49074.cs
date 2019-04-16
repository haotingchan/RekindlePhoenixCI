using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DataObjects.Dao.Together;

/// <summary>
/// Winni, 2019/4/15
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49074 會議記錄/議程出席者維護
   /// </summary>
   public partial class W49074 : FormParent {

      private D49074 dao49074;
      protected const string mainKey = "minutes";
      protected const string subKey = "agenda";

      public W49074(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao49074 = new D49074();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            Retrieve();
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = true;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {
         try {

            //0.check (沒有資料時,則自動新增一筆)
            DataTable dtMain = dao49074.GetDataList(mainKey);
            if (dtMain.Rows.Count <= 0) {
               InsertRow();
            }

            DataTable dtSub = dao49074.GetDataList(subKey);
            if (dtSub.Rows.Count <= 0) {
               InsertRow();
            }

            //1. 設定gvMain
            gcMain.Visible = true;
            gcMain.DataSource = dtMain;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);
            gcMain.Focus();

            //2. 設定gvSub
            gcSub.Visible = true;
            gcSub.DataSource = dtSub;
            gvSub.BestFitColumns();
            GridHelper.SetCommonGrid(gvSub);
            gcSub.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         try {

            DataTable dtMainCur = (DataTable)gcMain.DataSource;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dtMainChange = dtMainCur.GetChanges();
            DataTable dtMainForAdd = dtMainCur.GetChanges(DataRowState.Added);
            DataTable dtMainForModified = dtMainCur.GetChanges(DataRowState.Modified);
            DataTable dtMainForDeleted = dtMainCur.GetChanges(DataRowState.Deleted);

            DataTable dtSubCur = (DataTable)gcSub.DataSource;
            gvSub.CloseEditor();
            gvSub.UpdateCurrentRow();

            DataTable dtSubChange = dtSubCur.GetChanges();
            DataTable dtSubForAdd = dtSubCur.GetChanges(DataRowState.Added);
            DataTable dtSubForModified = dtSubCur.GetChanges(DataRowState.Modified);
            DataTable dtSubForDeleted = dtSubCur.GetChanges(DataRowState.Deleted);

            if (dtMainChange == null && dtSubChange == null) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
            if (dtMainChange.Rows.Count == 0 && dtSubChange.Rows.Count == 0) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }

            foreach (DataRow drMain in dtMainCur.Rows) {
               if (drMain.RowState == DataRowState.Added || drMain.RowState == DataRowState.Modified) {
                  drMain["RPTF_TXN_ID"] = _ProgramID;
                  drMain["RPTF_TXD_ID"] = _ProgramID;
                  drMain["RPTF_KEY"] = mainKey;
               }
            }

            foreach (DataRow drSub in dtSubCur.Rows) {
               if (drSub.RowState == DataRowState.Added || drSub.RowState == DataRowState.Modified) {
                  drSub["RPTF_TXN_ID"] = _ProgramID;
                  drSub["RPTF_TXD_ID"] = _ProgramID;
                  drSub["RPTF_KEY"] = subKey;
               }
            }

            dtMainChange = dtMainCur.GetChanges();
            dtSubChange = dtSubCur.GetChanges();
            dtMainChange.Merge(dtSubChange);
            ResultData result = new RPTF().UpdateData(dtMainChange);
            if (result.Status == ResultStatus.Fail) {
               return ResultStatus.Fail;
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
         _IsPreventFlowPrint = true; //不要自動列印
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         ReportHelper _ReportHelper = reportHelper;
         CommonReportPortraitA4 report = new CommonReportPortraitA4();
         report.printableComponentContainerMain.PrintableComponent = gcMain;
         _ReportHelper.Create(report);

         base.Print(_ReportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         DataTable dt = (DataTable)gcMain.DataSource;
         gvMain.AddNewRow();
         int row = dt.Compute("Max(RPTF_SEQ_NO)" , "").AsInt();

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["RPTF_SEQ_NO"] , row + 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         setRowNumber(gvMain.Columns.View);
         return ResultStatus.Success;
      }

      /// <summary>
      /// 刪除列後index自動重排
      /// </summary>
      /// <param name="View"></param>
      private void setRowNumber(ColumnView View) {
         GridColumn col = View.Columns.ColumnByFieldName("RPTF_SEQ_NO");
         if (col == null) return;
         View.BeginSort();

         try {
            // Obtain the number of data rows.  
            int dataRowCount = View.DataRowCount;
            // Traverse data rows and change the "RPTF_SEQ_NO" field index  
            for (int i = 0 ; i < dataRowCount ; i++) {
               object cellValue = View.GetRowCellValue(i , col);
               View.SetRowCellValue(i , col , i + 1);
            }
         } finally { View.EndSort(); }

      }

      #region GridControl事件
      private void btnSubAdd_Click(object sender , EventArgs e) {
         base.InsertRow(gvSub);
         DataTable dt = (DataTable)gcSub.DataSource;
         int row = dt.Compute("Max(RPTF_SEQ_NO)" , "").AsInt();
         gvSub.SetRowCellValue(GridControl.NewItemRowHandle , gvSub.Columns["RPTF_SEQ_NO"] , row + 1);
         gvSub.FocusedColumn = gvSub.Columns[0];
      }

      private void btnSubDel_Click(object sender , EventArgs e) {
         base.DeleteRow(gvSub);
         setRowNumber(gvSub.Columns.View);
      }
      #endregion
   }
}