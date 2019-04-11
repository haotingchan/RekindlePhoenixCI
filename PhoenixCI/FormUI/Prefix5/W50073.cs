using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao.REWARD;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
/// <summary>
/// Lukas, 2018/12/14
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 50073 法人機構報價獎勵活動名單維護
    /// 有寫到的功能：Open, Retrieve, Save, Print, InsertRow
    /// </summary>
    public partial class W50073 : FormParent {

        protected RWD_REF_OMNI RRO;
        protected D50073 dao50073;

        //ken,原本拉出來要使用在RepositoryItemLookUpEdit.GetNotInListValue中,但是後來沒用
        protected DataTable dtActId;
        protected RepositoryItemLookUpEdit cbxActId;

        public W50073(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;
            dao50073 = new D50073();
            RRO = new RWD_REF_OMNI();

        }

        protected override ResultStatus Open() {
            try {
                base.Open();

                //[活動名稱]下拉選單
                dtActId = new COD().ListByTxn("50073");

                #region //ken,這邊很特別,資料表內的資料不乾淨,比下拉選單項目還多,必須先做資料合併再設定到下拉選單中
                DataView view = new DataView(RRO.GetData());
                DataTable dtTemp = view.ToTable(true, "RWD_REF_OMNI_ACTIVITY_ID");
                dtTemp.Columns[0].ColumnName = "COD_ID";
                DataColumn dataColumn = new DataColumn("COD_DESC");
                dtTemp.Columns.Add(dataColumn);
                foreach (DataRow dr in dtTemp.Rows) {
                    dr[0] = dr[0].AsString();
                    dr[1] = dr[0];
                }
                dtTemp.PrimaryKey = new DataColumn[] { dtTemp.Columns["COD_ID"] };
                dtActId.PrimaryKey = new DataColumn[] { dtActId.Columns["COD_ID"] };
                dtTemp.Merge(dtActId, false);
                dtActId = dtTemp;
                #endregion
                
                cbxActId = new RepositoryItemLookUpEdit();
                //cbxActId.GetNotInListValue += CbxActId_GetNotInListValue;//假的,DevExpress沒實作
                cbxActId.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.True;
                cbxActId.ProcessNewValue += CbxActId_ProcessNewValue;//開放輸入下拉選單沒有的資訊
                cbxActId.SetColumnLookUp(dtActId, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, "");
                gcMain.RepositoryItems.Add(cbxActId);
                RWD_REF_OMNI_ACTIVITY_ID.ColumnEdit = cbxActId;

                Retrieve();
                //自動調整欄寬
                gvMain.BestFitColumns();
            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        #region LookUpEdit事件
        /// <summary>
        /// 假的,DevExpress沒實作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxActId_GetNotInListValue(object sender, GetNotInListValueEventArgs e) {

            if (e.Value == null || string.Empty.Equals(e.Value))
                return;

            DataRow dr = dtActId.NewRow();
            dr["COD_ID"] = e.Value;
            dr["COD_DESC"] = e.Value;
            dtActId.Rows.Add(dr);

            RepositoryItemLookUpEdit edit = sender as RepositoryItemLookUpEdit;
            gcMain.RepositoryItems.Remove(edit);
            edit.SetColumnLookUp(dtActId, "COD_ID", "COD_DESC");
            gcMain.RepositoryItems.Add(edit);
            RWD_REF_OMNI_ACTIVITY_ID.ColumnEdit = edit;
        }

        private void CbxActId_ProcessNewValue(object sender, ProcessNewValueEventArgs e) {
            try {
                if (e.DisplayValue == null || string.Empty.Equals(e.DisplayValue))
                    return;

                DataRow dr = dtActId.NewRow();
                dr["COD_ID"] = e.DisplayValue;
                dr["COD_DESC"] = e.DisplayValue;
                dtActId.Rows.Add(dr);

                e.Handled = true;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        #endregion

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = true;
            _ToolBtnSave.Enabled = true;
            _ToolBtnDel.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnExport.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {
            try {
                base.Retrieve(gcMain);
                DataTable returnTable = new DataTable();
                returnTable = RRO.GetData();
                if (returnTable.Rows.Count == 0) {
                    MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                gcMain.DataSource = returnTable;

                gcMain.Focus();
            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield() {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }
            //if (cbxUserId.SelectedItem == null) {
            //    MessageDisplay.Warning("使用者代號不可為空白!");
            //    return ResultStatus.Fail;
            //}

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            try {
                base.Save(gcMain);

                DataTable dt = (DataTable)gcMain.DataSource;

                DataTable dtChange = dt.GetChanges();
                DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
                DataTable dtForModified = dt.GetChanges(DataRowState.Modified);
                DataTable dtForDeleted = dt.GetChanges(DataRowState.Deleted);

                if (dtChange == null) {
                    MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return ResultStatus.Fail;
                }
                if (dtChange.Rows.Count == 0) {
                    MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return ResultStatus.Fail;
                }


                //update to DB
                ResultData myResultData = dao50073.update(dt);

                //列印
                PrintOrExportChangedByKen(gcMain, dtForAdd, dtForDeleted, dtForModified);
                _IsPreventFlowPrint = true;

            }
            catch (Exception ex) {
                MessageDisplay.Error("存檔錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Export() {
            //reportHelper = _ReportHelper;
            //base.Export(reportHelper);
            try {
                DataTable exportTable = new DataTable();
                exportTable = dao50073.ListAll();
                if (exportTable.Rows.Count == 0) {
                    MessageBox.Show("轉出筆數為０!", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                string excelDestinationPath = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + "\\" + _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss") + ".xls";
                Workbook workbook = new Workbook();
                Worksheet ws50073 = workbook.Worksheets[0];
                ws50073.Cells[0, 0].Value = "活動名稱";
                ws50073.Cells[0, 0].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                ws50073.Cells[0, 1].Value = "期貨商代號";
                ws50073.Cells[0, 1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                ws50073.Cells[0, 2].Value = "交易人帳號";
                ws50073.Cells[0, 2].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                ws50073.Cells[0, 3].Value = "法人機構名稱";
                ws50073.Cells[0, 3].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                int insertRow = 1;
                for (int i = 0; i < exportTable.Rows.Count; i++) {
                    DataRow dr = exportTable.Rows[i];
                    ws50073.Cells[insertRow, 0].Value = dr["RWD_REF_OMNI_ACTIVITY_ID"].AsString();
                    ws50073.Cells[insertRow, 1].Value = dr["RWD_REF_OMNI_FCM_NO"].AsString();
                    ws50073.Cells[insertRow, 2].Value = dr["RWD_REF_OMNI_ACC_NO"].AsString();
                    ws50073.Cells[insertRow, 3].Value = dr["RWD_REF_OMNI_NAME"].AsString();
                    insertRow = insertRow + 1;
                }
                ws50073.ScrollToRow(0);
                workbook.SaveDocument(excelDestinationPath);
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper ReportHelper) {
            try {
                ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                CommonReportPortraitA4 reportPortrait = new CommonReportPortraitA4();
                reportPortrait.printableComponentContainerMain.PrintableComponent = gcMain;
                reportPortrait.IsHandlePersonVisible = false;
                reportPortrait.IsManagerVisible = false;
                _ReportHelper.Create(reportPortrait);

                _ReportHelper.Print();

            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            try {
                base.InsertRow(gvMain);
                gvMain.Focus();

                gvMain.FocusedColumn = gvMain.Columns[0];
            }
            catch (Exception ex) {
                MessageDisplay.Error("新增資料列錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            try {
                GridView gv = gvMain as GridView;

                base.DeleteRow(gvMain);
            }
            catch (Exception ex) {
                MessageDisplay.Error("刪除資料列錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus COMPLETE() {
            MessageDisplay.Info(MessageDisplay.MSG_OK);
            Retrieve();
            return ResultStatus.Success;
        }

    }
}