using System.Data;
using System.Windows.Forms;
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

namespace PhoenixCI.FormUI.Prefix5 {
    public partial class W51020 : FormParent {
        private string disableCol = "MMFT_MARKET_CODE";
        private string disableCol2 = "MMFT_ID";

        private ReportHelper _ReportHelper;
        private COD daoCOD;
        private D51020 dao51020;
        private RepositoryItemLookUpEdit _RepLookUpEdit;
        private RepositoryItemLookUpEdit _RepLookUpEdit2;
        private DataTable dtForDeleted;

        public W51020(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            _IsPreventFlowPrint = false;
            dao51020 = new D51020();
            dtForDeleted = new DataTable();
            daoCOD = new COD();

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
            returnTable.Columns.Add("Is_NewRow", typeof(string));
            dtForDeleted = returnTable.Clone();
            gcMain.DataSource = returnTable;


            gcMain.Focus();

            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            base.InsertRow(gvMain);
            //gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall poke) {
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dt = (DataTable)gcMain.DataSource;

            DataTable dtChange = dt.GetChanges();
            DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
            DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

            ResultData resultData = new ResultData();
            resultData.ChangedDataViewForAdded = dtForAdd == null ? new DataView() : dtForAdd.DefaultView;
            resultData.ChangedDataViewForDeleted = dtForDeleted == null ? new DataView() : dtForDeleted.DefaultView;
            resultData.ChangedDataViewForModified = dtForModified == null ? new DataView() : dtForModified.DefaultView;

            if (dtChange.Rows.Count == 0) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else {
                ResultStatus status = dao51020.updateData(dt).Status;//base.Save_Override(dt, "MMFT");

                if (status == ResultStatus.Fail) {
                    return ResultStatus.Fail;
                }
            }

            PrintOrExportChanged(gcMain, resultData);
            _IsPreventFlowPrint = true;
            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            _ReportHelper = reportHelper;
            CommonReportPortraitA4 report = new CommonReportPortraitA4();
            report.printableComponentContainerMain.PrintableComponent = gcMain;
            _ReportHelper.Create(report);

            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield() {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            GridView gv = gvMain as GridView;
            DataRowView deleteRowView = (DataRowView)gv.GetFocusedRow();
            dtForDeleted.ImportRow(deleteRowView.Row);

            base.DeleteRow(gvMain);

            return ResultStatus.Success;
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

        protected override ResultStatus Open() {
            base.Open();

            //直接讀取資料
            Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus COMPLETE() {
            MessageDisplay.Info(MessageDisplay.MSG_OK);
            Retrieve();
            return ResultStatus.Success;
        }

        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                e.Cancel = false;
                gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
            }
            else if (gv.FocusedColumn.FieldName == disableCol ||
               gv.FocusedColumn.FieldName == disableCol2) {
                e.Cancel = true;
            }
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMFT_END_S"], 0);
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMFT_END_E"], 0);
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (e.Column.FieldName == disableCol ||
                e.Column.FieldName == disableCol2) {
                e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
            }
        }

        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
        }
    }
}