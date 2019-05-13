using System.Data;
using BaseGround;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Repository;
using BaseGround.Report;
using DevExpress.XtraEditors.Controls;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;
using System.ComponentModel;
using Common;
using System.Drawing;
using BaseGround.Shared;
using System;

namespace PhoenixCI.FormUI.Prefix5 {
   public partial class W51020 : FormParent {
      private string disableCol = "MMFT_MARKET_CODE";
      private string disableCol2 = "MMFT_ID";

      private COD daoCOD;
      private D51020 dao51020;
      private RepositoryItemLookUpEdit _RepLookUpEdit;
      private RepositoryItemLookUpEdit _RepLookUpEdit2;

      public W51020(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         dao51020 = new D51020();
         daoCOD = new COD();
         GridHelper.SetCommonGrid(gvMain);

         this.Text = _ProgramID + "─" + _ProgramName;

         #region Set Drop Down Lsit
         //交易時段 價平月份 兩個欄位要換成LookUpEdit
         _RepLookUpEdit = new RepositoryItemLookUpEdit();
         DataTable cbxCPKindSource = daoCOD.ListByCol2("MMFT", "MMFT_CP_KIND");
         cbxCPKindSource.Rows.Add("", "");
         _RepLookUpEdit.SetColumnLookUp(cbxCPKindSource, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit);
         MMFT_CP_KIND.ColumnEdit = _RepLookUpEdit;

         _RepLookUpEdit2 = new RepositoryItemLookUpEdit();
         DataTable cbxMarketCodeSource = daoCOD.ListByCol2("MMFT", "MMFT_MARKET_CODE");
         _RepLookUpEdit2.SetColumnLookUp(cbxMarketCodeSource, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit2);
         MMFT_MARKET_CODE.ColumnEdit = _RepLookUpEdit2;
         #endregion
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);
         DataTable returnTable = new DataTable();
         returnTable = dao51020.ListAll();
         if (returnTable.Rows.Count == 0) {
            MessageDisplay.Info("無任何資料");
         }
         //returnTable.Columns.Add("Is_NewRow", typeof(string));
         gcMain.DataSource = returnTable;

         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         base.InsertRow(gvMain);
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         try {
            DataTable dt = (DataTable)gcMain.DataSource;

            DataTable dtChange = dt.GetChanges();
            DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
            DataTable dtForModified = dt.GetChanges(DataRowState.Modified);
            DataTable dtForDeleted = dt.GetChanges(DataRowState.Deleted);

            if (dtChange == null) {
               MessageDisplay.Info("沒有變更資料, 不需要存檔!");
               return ResultStatus.FailButNext;
            }
            ResultStatus status = dao51020.updateData(dt).Status;//base.Save_Override(dt, "MMFT");

            if (status == ResultStatus.Fail) {
               MessageDisplay.Error("儲存失敗");
               return ResultStatus.Fail;
            }

            PrintOrExportChangedByKen(gcMain, dtForAdd, dtForDeleted, dtForModified);
         } catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            string footerMemo = "";
            string txtFilePath = System.IO.Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH, _ProgramID + ".txt");
            using (System.IO.TextReader tr = new System.IO.StreamReader(txtFilePath, System.Text.Encoding.Default)) {
               string line = "";
               while ((line = tr.ReadLine()) != null) {
                  footerMemo += line + Environment.NewLine;
               }
            }

            ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.FooterMemo = footerMemo;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();//如果有夜盤會特別標註
            _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus DeleteRow() {
         return base.DeleteRow(gvMain);
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;
         _ToolBtnSave.Enabled = true;
         _ToolBtnDel.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
             gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
         } else if (gv.FocusedColumn.FieldName == disableCol ||
              gv.FocusedColumn.FieldName == disableCol2) {
            e.Cancel = true;
         }
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMFT_END_S"], 0);
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMFT_END_E"], 0);
      }

      private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]).ToString();

         if (e.Column.FieldName == disableCol ||
             e.Column.FieldName == disableCol2) {
            e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
         }
      }

      private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
         GridView gv = sender as GridView;
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
      }
   }
}