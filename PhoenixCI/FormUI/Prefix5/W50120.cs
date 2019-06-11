using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using Common;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.Utils.Drawing;
using BaseGround.Shared;
/// <summary>
/// Lukas, 2018/12/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 50120 造市者異動狀態維護
    /// 有寫到的功能：Open, Retrieve, Save, Print, InsertRow
    /// </summary>
    public partial class W50120 : FormParent {

        private ReportHelper _ReportHelper;
        private D50120 dao50120;
        private ABRK daoABRK;
        private APDK daoAPDK;
        protected DataTable dtActId;
        private COD daoCOD;
        private RepositoryItemLookUpEdit _RepLookUpEdit;
        private RepositoryItemLookUpEdit _RepLookUpEdit2;
        private RepositoryItemLookUpEdit _RepLookUpEdit3;

        public W50120(string programID, string programName) : base(programID, programName) {

            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;
            txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            dao50120 = new D50120();
            _IsPreventFlowPrint = false;

        }

        public override ResultStatus BeforeOpen() {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open() {
            base.Open();

            #region 處理下拉選單

            //[造市者代號]下拉選單
            _RepLookUpEdit = new RepositoryItemLookUpEdit();
            //_RepLookUpEdit.GetNotInListValue += new GetNotInListValueEventHandler(_RepLookUpEdit_GetNotInListValue);
            daoABRK = new ABRK();
            _RepLookUpEdit.DataSource = daoABRK.ListByNo();
            _RepLookUpEdit.ValueMember = "ABRK_NO";
            _RepLookUpEdit.DisplayMember = "CP_DISPLAY";
            _RepLookUpEdit.ShowHeader = false;
            _RepLookUpEdit.ShowFooter = false;
            _RepLookUpEdit.NullText = "";
            _RepLookUpEdit.SearchMode = SearchMode.AutoFilter;
            _RepLookUpEdit.TextEditStyle = TextEditStyles.Standard;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol = _RepLookUpEdit.Columns;
            singleCol.Add(new LookUpColumnInfo("CP_DISPLAY"));
            gcMain.RepositoryItems.Add(_RepLookUpEdit);
            MPDF_FCM_NO.ColumnEdit = _RepLookUpEdit;
            _RepLookUpEdit.BestFitMode = BestFitMode.BestFitResizePopup;


            //[狀態]下拉選單
            _RepLookUpEdit2 = new RepositoryItemLookUpEdit();
            daoCOD = new COD();
            _RepLookUpEdit2.DataSource = daoCOD.ListByTxn("50120");
            _RepLookUpEdit2.ValueMember = "COD_ID";
            _RepLookUpEdit2.DisplayMember = "CP_DISPLAY";
            _RepLookUpEdit2.ShowHeader = false;
            _RepLookUpEdit2.ShowFooter = false;
            _RepLookUpEdit2.NullText = "";
            _RepLookUpEdit2.SearchMode = SearchMode.AutoFilter;
            _RepLookUpEdit2.TextEditStyle = TextEditStyles.Standard;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol2 = _RepLookUpEdit2.Columns;
            singleCol2.Add(new LookUpColumnInfo("CP_DISPLAY"));
            gcMain.RepositoryItems.Add(_RepLookUpEdit2);
            MPDF_STATUS.ColumnEdit = _RepLookUpEdit2;
            _RepLookUpEdit2.BestFitMode = BestFitMode.BestFitResizePopup;

            //[契約]下拉選單
            _RepLookUpEdit3 = new RepositoryItemLookUpEdit();
            daoAPDK = new APDK();
            dtActId = daoAPDK.ListAll2();
            #region //資料表內的資料不乾淨,比下拉選單項目還多,必須先做資料合併在設定到下拉選單中
            DataView view = new DataView(dao50120.GetData(txtMonth.Text.Replace("/", "")));
            DataTable dtTemp = view.ToTable(true, "MPDF_KIND_ID");
            if (dtTemp.Rows.Count != 0) {
                dtTemp.Columns[0].ColumnName = "APDK_KIND_ID";
                DataColumn dataColumn = new DataColumn("CP_DISPLAY");
                dtTemp.Columns.Add(dataColumn);
                foreach (DataRow dr in dtTemp.Rows) {
                    dr[0] = dr[0].AsString();
                    dr[1] = dr[0];
                }
                dtTemp.PrimaryKey = new DataColumn[] { dtTemp.Columns["CP_DISPLAY"] };
                dtActId.PrimaryKey = new DataColumn[] { dtActId.Columns["CP_DISPLAY"] };
                dtTemp.Merge(dtActId, false);
                dtActId = dtTemp;
            }
            _RepLookUpEdit3.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.True;
            _RepLookUpEdit3.ProcessNewValue += _RepLookUpEdit3_ProcessNewValue;//開放輸入下拉選單沒有的資訊
            _RepLookUpEdit3.SetColumnLookUp(dtActId, "APDK_KIND_ID", "CP_DISPLAY", TextEditStyles.DisableTextEditor, "");
            gcMain.RepositoryItems.Add(_RepLookUpEdit3);
            MPDF_KIND_ID.ColumnEdit = _RepLookUpEdit3;
            #endregion

            #endregion

            return ResultStatus.Success;
        }

        private void _RepLookUpEdit3_ProcessNewValue(object sender, ProcessNewValueEventArgs e) {
            if (e.DisplayValue == null || string.Empty.Equals(e.DisplayValue))
                return;

            DataRow dr = dtActId.NewRow();
            dr["COD_ID"] = e.DisplayValue;
            dr["COD_DESC"] = e.DisplayValue;
            dtActId.Rows.Add(dr);

            e.Handled = true;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();
            //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
            DataTable dtCheck = dao50120.GetData(txtMonth.Text.Replace("/", ""));

            //沒有新增資料時,則自動新增內容
            if (dtCheck.Rows.Count == 0) {
                dtCheck.Columns.Add("Is_NewRow", typeof(string));
                gcMain.DataSource = dtCheck;
                InsertRow();
            }
            else {
                dtCheck.Columns.Add("Is_NewRow", typeof(string));
                gcMain.DataSource = dtCheck;
                gcMain.Focus();
            }
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

        protected override ResultStatus Retrieve() {
            //測試資料查詢日期: 2007/04
            base.Retrieve(gcMain);
            DataTable returnTable = new DataTable();
            returnTable = dao50120.GetData(txtMonth.Text.Replace("/", ""));
            if (returnTable.Rows.Count == 0) {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            returnTable.Columns.Add("Is_NewRow", typeof(string));

            //[契約]下拉選單
            _RepLookUpEdit3 = new RepositoryItemLookUpEdit();
            daoAPDK = new APDK();
            dtActId = daoAPDK.ListAll2();
            #region //資料表內的資料不乾淨,比下拉選單項目還多,必須先做資料合併在設定到下拉選單中
            DataView view = new DataView(returnTable);
            DataTable dtTemp = view.ToTable(true, "MPDF_KIND_ID");
            if (dtTemp.Rows.Count != 0) {
                dtTemp.Columns[0].ColumnName = "APDK_KIND_ID";
                DataColumn dataColumn = new DataColumn("CP_DISPLAY");
                dtTemp.Columns.Add(dataColumn);
                foreach (DataRow dr in dtTemp.Rows) {
                    dr[0] = dr[0].AsString();
                    dr[1] = dr[0];
                }
                dtTemp.PrimaryKey = new DataColumn[] { dtTemp.Columns["CP_DISPLAY"] };
                dtActId.PrimaryKey = new DataColumn[] { dtActId.Columns["CP_DISPLAY"] };
                dtTemp.Merge(dtActId, false);
                dtActId = dtTemp;
            }
            _RepLookUpEdit3.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.True;
            _RepLookUpEdit3.ProcessNewValue += _RepLookUpEdit3_ProcessNewValue;//開放輸入下拉選單沒有的資訊
            _RepLookUpEdit3.SetColumnLookUp(dtActId, "APDK_KIND_ID", "CP_DISPLAY", TextEditStyles.DisableTextEditor, "");
            gcMain.RepositoryItems.Add(_RepLookUpEdit3);
            MPDF_KIND_ID.ColumnEdit = _RepLookUpEdit3;
            #endregion

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
            _IsPreventFlowPrint = true;//儲存完不要自動列印
            DataTable dt = (DataTable)gcMain.DataSource;

            DataTable dtChange = dt.GetChanges();

            if (dtChange == null) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }
            //PB存的日期格式月份沒有補0，在維護上處理比較麻煩，故讀取資料時轉成字串，存檔時再轉回datetime
            DataTable dtCloned = dt.Clone();
            dtCloned.Columns["MPDF_EFF_DATE"].DataType = typeof(DateTime);
            foreach (DataRow row in dt.Rows) {
                dtCloned.ImportRow(row);
            }
            //foreach (DataRow dr in dt.Rows) {
            //    if (dr.RowState != DataRowState.Unchanged &&
            //        dr.RowState != DataRowState.Deleted) dr["MPDF_EFF_DATE"] = dr["MPDF_EFF_DATE"].AsDateTime("yyyy/MM/dd");
            //}

            try {
                //update to DB
                ResultData myResultData = dao50120.UpdateMPDF(dtCloned);
                if (myResultData.Status == ResultStatus.Fail) {
                    MessageDisplay.Error("更新資料庫MPDF錯誤! ");
                    return ResultStatus.Fail;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
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

        #region GridControl事件

        /// <summary>
        /// 年月欄自動填上查詢年月(gvMain_InitNewRow事件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
            gv.SetRowCellValue(e.RowHandle, gv.Columns["MPDF_YM"], txtMonth.Text.Replace("/", ""));
        }

        /// <summary>
        /// 決定哪些欄位無法編輯的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                e.Cancel = false;
                gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
            }
            //既有資料除了生效日期之外不能編輯
            else if (gv.FocusedColumn.Name == "MPDF_EFF_DATE") {
                e.Cancel = false;
            }
            else {
                e.Cancel = true;
            }

        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            //要用RowHandle不要用FocusedRowHandle
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();
            e.Column.OptionsColumn.AllowFocus = true;

            if (e.Column.FieldName != "MPDF_EFF_DATE") {
                e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(224, 224, 224);
            }
        }

        private void gvMain_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {

            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
            gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (e.FocusedColumn != null) {
                if (Is_NewRow == "1") {
                    e.FocusedColumn.OptionsColumn.AllowFocus = true;
                }
                else if (e.FocusedColumn.FieldName != "MPDF_EFF_DATE") {
                    e.FocusedColumn.OptionsColumn.AllowFocus = false;
                }
            }
        }

        #endregion

        protected override ResultStatus DeleteRow() {
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