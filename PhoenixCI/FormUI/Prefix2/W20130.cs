using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Lukas, 2018/12/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {
    /// <summary>
    /// 20130 造市者異動狀態維護
    /// </summary>
    public partial class W20130 : FormParent {

        private ReportHelper _ReportHelper;
        private D20130 dao20130;
        private RepositoryItemLookUpEdit _RepLookUpEdit;

        public W20130(string programID, string programName) : base(programID, programName) {

            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao20130 = new D20130();

            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;

            txtStartDate.DateTimeValue = GlobalInfo.OCF_PREV_DATE;
            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

        }

        protected override ResultStatus Open() {
            base.Open();

            _RepLookUpEdit = new RepositoryItemLookUpEdit();
            DataTable lookUpDt = new DataTable();
            lookUpDt.Columns.Add("COD_ID");
            lookUpDt.Columns.Add("COD_DESC");
            //參數設定下拉選單
            string[] spnParms = { "CME", "HKEX", "SGX" };
            for (int i = 0; i < spnParms.Count(); i++) {
                lookUpDt.Rows.Add();
                lookUpDt.Rows[i].SetField("COD_ID", spnParms[i]);
                lookUpDt.Rows[i].SetField("COD_DESC", spnParms[i]);
            }
            Extension.SetColumnLookUp(_RepLookUpEdit, lookUpDt, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor,"");
            gcMain.RepositoryItems.Add(_RepLookUpEdit);
            AM12_F_ID.ColumnEdit = _RepLookUpEdit;

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
            string startDate = txtStartDate.Text.Replace("/", "");
            string endDate = txtEndDate.Text.Replace("/", "");
            DataTable dtCheck = dao20130.ListAll(startDate, endDate);

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

            string startDate = txtStartDate.Text.Replace("/", "");
            string endDate = txtEndDate.Text.Replace("/", "");
            DataTable returnTable = dao20130.ListAll(startDate, endDate);

            if (returnTable.Rows.Count == 0) {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ResultStatus.Fail;
            }

            returnTable.Columns.Add("Is_NewRow", typeof(string));
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

            if (dtChange == null) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }

            foreach (DataRow dr in dt.Rows) {
                if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                    dr["AM12_W_TIME"] = DateTime.Now;
                }
            }
            //更新主要Table
            string tableName = "CI.AM12";
            string keysColumnList = "am12_ymd, am12_f_id, am12_kind_id";
            string insertColumnList = "am12_ymd, am12_f_id, am12_vol, am12_w_time, am12_w_user_id, am12_data_type, am12_kind_id";
            string updateColumnList = "am12_ymd, am12_f_id, am12_vol, am12_w_time, am12_w_user_id, am12_data_type, am12_kind_id";
            try {
                //update to DB
                ResultData ResultData = serviceCommon.SaveForChanged(dt, tableName, insertColumnList, updateColumnList, keysColumnList, pokeBall);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            //把所有新增和修改的Row,INSERT到歷史檔AM12L
            try {
                //update to DB
                foreach (DataRow dr in dtChange.Rows) {
                    if (dr.RowState != DataRowState.Deleted) {
                        string am12Ymd = dr["AM12_YMD"].ToString();
                        string am12FId = dr["AM12_F_ID"].ToString();
                        string am12KindId = dr["AM12_KIND_ID"].ToString();
                        bool rtn = dao20130.InsertAM12L(am12Ymd, am12FId, am12KindId);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            //不要自動列印
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

        protected override ResultStatus InsertRow() {
            base.InsertRow(gvMain);
            gvMain.Focus();
            gvMain.FocusedColumn = gvMain.VisibleColumns[0];

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow(gvMain);

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

            //直接設定值給dataTable(have UI)
            gv.SetRowCellValue(e.RowHandle, gv.Columns["AMI12_VOL"], 0);
            gv.SetRowCellValue(e.RowHandle, gv.Columns["AMI12_YMD"], DateTime.Now.ToString("yyyyMMdd"));

            //直接設定值給dataTable(no UI)
            gv.SetRowCellValue(e.RowHandle, gv.Columns["AM12_KIND_ID"], "RHF");
            gv.SetRowCellValue(e.RowHandle, gv.Columns["AM12_DATA_TYPE"], "U");
            gv.SetRowCellValue(e.RowHandle, gv.Columns["AM12_W_USER_ID"], GlobalInfo.USER_ID);
            
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
            //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
            else if (gv.FocusedColumn.Name == "AM12_VOL") {
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

            //描述每個欄位,在is_newRow時候要顯示的顏色
            //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
            //當該欄位不可編輯時,TabStop為false(PB的wf_set_order方法)
            switch (e.Column.FieldName) {
                case ("AM12_YMD"):
                case ("AM12_F_ID"):
                    e.Column.OptionsColumn.TabStop = Is_NewRow == "1" ? true : false;
                    e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
                    break;
                case ("AM12_VOL"):
                    e.Appearance.BackColor = Color.White;
                    break;
                case ("AM12_STATUS"):
                    //e.Column.OptionsColumn.TabStop = Is_NewRow == "1" ? true : false;
                    e.Appearance.BackColor = Color.FromArgb(224, 224, 224);
                    break;
            }//switch (e.Column.FieldName) {

        }
        #endregion
    }
}