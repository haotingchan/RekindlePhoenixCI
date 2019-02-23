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
        protected ReportHelper _ReportHelper;
        protected RWD_REF_OMNI RRO;
        protected D50073 dao50073;
        protected DataTable dtForDeleted;

        //ken,原本拉出來要使用在RepositoryItemLookUpEdit.GetNotInListValue中,但是後來沒用
        protected DataTable dtActId;
        protected RepositoryItemLookUpEdit cbxActId;

        protected class LookupItem {
            public string ValueMember { get; set; }
            public string DisplayMember { get; set; }
        }


        public W50073(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;

            RRO = new RWD_REF_OMNI();
            dtForDeleted = new DataTable();

        }

        protected override ResultStatus Open() {
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


            //[系統別]下拉選單
            List<LookupItem> dsProdType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "F", DisplayMember = "期貨"},
                                        new LookupItem() { ValueMember = "O", DisplayMember = "選擇權" }};
            RepositoryItemLookUpEdit cbxProdType = new RepositoryItemLookUpEdit();
            cbxProdType.SetColumnLookUp(dsProdType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");
            gcMain.RepositoryItems.Add(cbxProdType);
            RWD_REF_OMNI_PROD_TYPE.ColumnEdit = cbxProdType;


            //[盤別]下拉選單
            List<LookupItem> dsMarket = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1", DisplayMember = "一般交易時段"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "盤後交易時段" }};
            RepositoryItemLookUpEdit cbxMarket = new RepositoryItemLookUpEdit();
            cbxMarket.SetColumnLookUp(dsMarket, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");
            gcMain.RepositoryItems.Add(cbxMarket);
            RWD_REF_OMNI_MARKET_CLOSE.ColumnEdit = cbxMarket;


            Retrieve();
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
            if (e.DisplayValue == null || string.Empty.Equals(e.DisplayValue))
                return;

            DataRow dr = dtActId.NewRow();
            dr["COD_ID"] = e.DisplayValue;
            dr["COD_DESC"] = e.DisplayValue;
            dtActId.Rows.Add(dr);

            e.Handled = true;
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
            base.Retrieve(gcMain);
            DataTable returnTable = new DataTable();
            returnTable = RRO.GetData();
            if (returnTable.Rows.Count == 0) {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            dtForDeleted = returnTable.Clone();
            gcMain.DataSource = returnTable;

            gcMain.Focus();

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
            base.Save(gcMain);

            DataTable dt = (DataTable)gcMain.DataSource;

            DataTable dtChange = dt.GetChanges();
            DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
            DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

            if (dtChange == null) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }

            string tableName = "REWARD.RWD_REF_OMNI";
            string keysColumnList = "rwd_ref_omni_activity_id, rwd_ref_omni_prod_type, rwd_ref_omni_fcm_no, rwd_ref_omni_acc_no, rwd_ref_omni_market_close";
            string insertColumnList = "rwd_ref_omni_activity_id, rwd_ref_omni_prod_type, rwd_ref_omni_fcm_no, rwd_ref_omni_acc_no, rwd_ref_omni_market_close";
            string updateColumnList = "rwd_ref_omni_activity_id, rwd_ref_omni_prod_type, rwd_ref_omni_fcm_no, rwd_ref_omni_acc_no, rwd_ref_omni_market_close";
            try {
                //update to DB
                ResultData myResultData = serviceCommon.SaveForChanged(dt, tableName, insertColumnList, updateColumnList, keysColumnList, pokeBall);

                //準備要印的資料(新增/刪除/修改)
                ResultData resultData = new ResultData();
                resultData.ChangedDataViewForAdded = dtForAdd == null ? new DataView() : dtForAdd.DefaultView;
                resultData.ChangedDataViewForDeleted = dtForDeleted == null ? new DataView() : dtForDeleted.DefaultView;
                resultData.ChangedDataViewForModified = dtForModified == null ? new DataView() : dtForModified.DefaultView;
                //列印
                PrintOrExportChanged(gcMain, resultData);
                _IsPreventFlowPrint = true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus Export() {
            //reportHelper = _ReportHelper;
            //base.Export(reportHelper);

            dao50073 = new D50073();
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
            ws50073.Cells[0, 1].Value = "系統別";
            ws50073.Cells[0, 1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            ws50073.Cells[0, 2].Value = "期貨商代號";
            ws50073.Cells[0, 2].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            ws50073.Cells[0, 3].Value = "交易人帳號";
            ws50073.Cells[0, 3].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            ws50073.Cells[0, 4].Value = "盤別";
            ws50073.Cells[0, 4].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            int insertRow = 1;
            for (int i = 0; i < exportTable.Rows.Count; i++) {
                DataRow dr = exportTable.Rows[i];
                ws50073.Cells[insertRow, 0].Value = dr["RWD_REF_OMNI_ACTIVITY_ID"].AsString();
                ws50073.Cells[insertRow, 1].Value = dr["RWD_REF_OMNI_PROD_TYPE"].AsString();
                ws50073.Cells[insertRow, 2].Value = dr["RWD_REF_OMNI_FCM_NO"].AsString();
                ws50073.Cells[insertRow, 3].Value = dr["RWD_REF_OMNI_ACC_NO"].AsString();
                ws50073.Cells[insertRow, 4].Value = dr["RWD_REF_OMNI_MARKET_CLOSE"].AsString();
                insertRow = insertRow + 1;
            }
            ws50073.ScrollToRow(0);
            workbook.SaveDocument(excelDestinationPath);
            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            _ReportHelper = reportHelper;
            CommonReportPortraitA4 report = new CommonReportPortraitA4();
            report.printableComponentContainerMain.PrintableComponent = gcMain;
            _ReportHelper.Create(report);
            //_ReportHelper.LeftMemo = "設定權限給(" + cbxUserId.Text.Trim() + ")";

            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            base.InsertRow(gvMain);
            gvMain.Focus();

            gvMain.FocusedColumn = gvMain.Columns[0];
            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            GridView gv = gvMain as GridView;
            DataRowView deleteRowView = (DataRowView)gv.GetFocusedRow();
            dtForDeleted.ImportRow(deleteRowView.Row);

            base.DeleteRow(gvMain);
            return ResultStatus.Success;
        }

        protected override ResultStatus COMPLETE() {
            MessageDisplay.Info(MessageDisplay.MSG_OK);
            Retrieve();
            return ResultStatus.Success;
        }

    }
}