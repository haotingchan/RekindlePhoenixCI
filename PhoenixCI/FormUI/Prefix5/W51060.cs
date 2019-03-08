using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseGround;
using BaseGround.Report;
using DataObjects.Dao.Together;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;

namespace PhoenixCI.FormUI.Prefix5 {
    public partial class W51060 : FormParent {
        private string allowCol = "MMIQ_INVALID_QNTY";
        private string disableCol = "MMIQ_YM";
        private ReportHelper _ReportHelper;
        private D51060 dao51060;

        public W51060(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            dao51060 = new D51060();
            this.Text = _ProgramID + "─" + _ProgramName;

            txtYM.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
        }

        protected override ResultStatus Retrieve() {
            bool check = CheckDate();
            if (check) {
                base.Retrieve(gcMain);
                DataTable returnTable = new DataTable();
                returnTable = dao51060.GetData(txtYM.Text.Replace("/", ""));
                if (returnTable.Rows.Count == 0) {
                    MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                returnTable.Columns.Add("Is_NewRow", typeof(string));
                gcMain.DataSource = returnTable;

                gcMain.Focus();
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall poke) {
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dt = (DataTable)gcMain.DataSource;
            ResultStatus resultStatus = ResultStatus.Fail;
            DataTable dtChange = dt.GetChanges();
            DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
            DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

            ResultData resultData = new ResultData();
            resultData.ChangedDataViewForAdded = dtForAdd == null ? new DataView() : dtForAdd.DefaultView;
            resultData.ChangedDataViewForModified = dtForModified == null ? new DataView() : dtForModified.DefaultView;

            try {
                if (dtChange.Rows.Count == 0) {
                    MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else {
                    string result = dao51060.ExecuteStoredProcedure(txtYM.Text.Replace("/", ""));
                    if (result == "0") {
                        foreach (DataRow r in dt.Rows) {
                            if (r.RowState != DataRowState.Deleted) {
                                r["MMIQ_W_TIME"] = DateTime.Now;
                                r["MMIQ_W_USER_ID"] = GlobalInfo.USER_ID;
                            }
                            if (Equals(0, r["MMIQ_INVALID_QNTY"])) {
                                r.Delete();
                            }
                        }
                        resultStatus = dao51060.updateData(dt).Status;//base.Save_Override(dt, "MMIQ");
                    }
                }
                PrintOrExportChanged(gcMain, resultData);
                _IsPreventFlowPrint = true;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }

            return resultStatus;
        }

        protected override ResultStatus InsertRow() {
            base.InsertRow(gvMain);
            gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];

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

        protected override ResultStatus DeleteRow() {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = true;
            _ToolBtnSave.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        public override ResultStatus BeforeOpen() {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open() {
            base.Open();

            //直接讀取資料
            Retrieve();
            //Header上色
            //CustomDrawColumnHeader(gcMain,gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose() {
            return base.BeforeClose();
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
            else if (gv.FocusedColumn.FieldName != allowCol) {
                e.Cancel = true;
            }
        }

        private void gvMain_NewRowAllowEdit(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMIQ_YM"], txtYM.Text.Replace("/", ""));
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMIQ_W_TIME"], GlobalInfo.OCF_DATE);
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMIQ_W_USER_ID"], GlobalInfo.USER_ID);
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (e.Column.FieldName != allowCol) {
                e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
            }

            if (e.Column.FieldName == disableCol) {
                e.Appearance.BackColor = Color.Silver;
            }
        }

        private bool CheckDate() {
            try {
                DateTime YM = DateTime.Parse(txtYM.Text);
            }
            catch (Exception ex) {
                MessageBox.Show("請確認輸入的日期");
                return false;
            }
            return true;
        }
    }
}