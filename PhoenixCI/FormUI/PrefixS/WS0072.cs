using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraGrid.Columns;
using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using System.Windows.Forms;
using BusinessObjects;
using System.Linq;

/// <summary>
/// david,2019/1/17
/// </summary>
namespace PhoenixCI.FormUI.PrefixS {
    /// <summary>
    /// SPAN參數檔案調整模組
    /// </summary>
    public partial class WS0072 : FormParent {
        protected DS0072 daoS0072;
        protected COD daoCod;
        protected string is_fm_ymd;
        protected string is_to_ymd;
        protected DateTime is_max_ymd;
        protected string[] modules1 = { "PSR", "VSR", "INTERMONTH", "SOM" };
        protected string[] module2 = { "ZISP" };

        public WS0072(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtStartDate.EnterMoveNextControl = true;
            txtEndDate.EnterMoveNextControl = true;
            GridHelper.SetCommonGrid(gv_ZISP);
            for (int i = 0; i < modules1.Length; i++) {
                GridView gv = GetGridView(modules1[i]);

                GridHelper.SetCommonGrid(gv);
            }
            daoS0072 = new DS0072();
            daoCod = new COD();

            //設定初始年月
            setDatePeriod();

            ////2.設定下拉選單
            DataTable dtProdType = daoS0072.dddw_zparm_comb_prod(txtEndDate.DateTimeValue.ToString("yyyyMMdd"));
            RepositoryItemLookUpEdit cbxProdType = new RepositoryItemLookUpEdit();
            cbxProdType.SetColumnLookUp(dtProdType, "PROD_GROUP", "PROD_GROUP", TextEditStyles.DisableTextEditor);
            gc_ZISP.RepositoryItems.Add(cbxProdType);
            SPAN_ZISP_PROD_ID.ColumnEdit = cbxProdType;

            DataTable dtProd = daoS0072.dddw_zparm_comb_prod_by_group(txtEndDate.DateTimeValue.ToString("yyyyMMdd"), "%%", "%%");
            RepositoryItemLookUpEdit cbxProd = new RepositoryItemLookUpEdit();
            cbxProd.SetColumnLookUp(dtProd, "COMB_PROD_VALUE", "COMB_PROD", TextEditStyles.DisableTextEditor);
            gc_ZISP.RepositoryItems.Add(cbxProd);
            SPAN_ZISP_COM_PROD1.ColumnEdit = cbxProd;
            SPAN_ZISP_COM_PROD2.ColumnEdit = cbxProd;

            DataTable dtContentType = daoCod.ListByCol2("S0072", "PSR_METHOD");
            RepositoryItemLookUpEdit cbxContentType = new RepositoryItemLookUpEdit();
            cbxContentType.SetColumnLookUp(dtContentType, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor);
            gc_PSR.RepositoryItems.Add(cbxContentType);
            PSR_SPAN_CONTENT_TYPE.ColumnEdit = cbxContentType;

            //grid 細部設定
            foreach (GridColumn col in gv_ZISP.Columns) {
                col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            Retrieve();

            //宣告事件
            //gv_ZISP.CustomDrawColumnHeader += gvZISP_CustomDrawColumnHeader;

            //製作連動下拉選單(觸發事件)
            for (int i = 0; i < modules1.Length; i++) {
                Control[] gridControls = this.Controls.Find("gc_" + modules1[i], true);
                GridControl gc = (GridControl)gridControls[0];
                GridView gv = gc.MainView as GridView;

                gc.RepositoryItems.Add(cbxProdType);
                gc.RepositoryItems.Add(cbxProd);
                gv.Columns["SPAN_CONTENT_CLASS"].ColumnEdit = cbxProdType;
                gv.Columns["SPAN_CONTENT_CC"].ColumnEdit = cbxProd;
                gv.ShownEditor += GridView_ShownEditor;
            }
            gv_ZISP.ShownEditor += ZISP_gridView_ShownEditor;
            cbxProdType.EditValueChanged += cbxProdType_EditValueChanged;
        }

        protected override ResultStatus Retrieve() {
            base.Retrieve();

            setDatePeriod();

            for (int i = 0; i < modules1.Length; i++) {
                DataTable dt = daoS0072.ListSpanContentByModule(modules1[i], GlobalInfo.USER_ID);
                GridView gv = GetGridView(modules1[i]);

                gv.GridControl.DataSource = dt;
            }

            for (int i = 0; i < module2.Length; i++) {
                DataTable dt = daoS0072.zisp(GlobalInfo.USER_ID, "CFO.SPAN_ZISP");
                GridView gv = GetGridView(module2[i]);

                gv.GridControl.DataSource = dt;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = true;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = false;//列印

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall args) {
            ResultStatus resultStatus = ResultStatus.Fail;

            if (!checkChanged()) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.FailButNext;
            }

            //儲存日期區間
            if (savePeriod() != ResultStatus.Success) return resultStatus;
                       
            //儲存VSR PSR InterMonth SOM 
            if (!checkComplete()) return ResultStatus.FailButNext;

            for (int i = 0; i < modules1.Length; i++) {
                GridView gv = GetGridView(modules1[i]);

                gv.CloseEditor();
                gv.UpdateCurrentRow();

                DataTable dt = (DataTable)gv.GridControl.DataSource;
                resultStatus = daoS0072.udpateSpanContentData(dt).Status;
            }

            for (int i = 0; i < module2.Length; i++) {
                GridView gv = GetGridView(module2[i]);

                gv.CloseEditor();
                gv.UpdateCurrentRow();

                DataTable dtZISP = (DataTable)gv.GridControl.DataSource;

                resultStatus = daoS0072.udpateZIPData(dtZISP).Status;//base.Save_Override(dtZISP, "SPAN_ZISP", DBName.CFO);
            }

            return resultStatus;
        }

        protected override ResultStatus RunBefore(PokeBall args) {
            ResultStatus resultStatus = ResultStatus.Fail;

            if (checkChanged()) {
                MessageDisplay.Info("資料有變更, 請先存檔!");
                resultStatus = ResultStatus.FailButNext;
            }
            else {
                Run(args);
            }
            return resultStatus;
        }

        protected override ResultStatus Run(PokeBall args) {
            if (!checkChanged()) {
                PbFunc.f_bat_span("S0072", "SPN", GlobalInfo.USER_ID);
            }
            return base.Run(args);
        }

        private ResultStatus savePeriod() {
            DataTable periodTable = daoS0072.d_s0070_1("SPN", GlobalInfo.USER_ID);
            periodTable.Rows[0].SetField("span_period_start_date", txtStartDate.DateTimeValue.ToString("yyyyMMdd"));
            periodTable.Rows[0].SetField("span_period_end_date", txtEndDate.DateTimeValue.ToString("yyyyMMdd"));
            periodTable.Rows[0].SetField("span_period_w_time", DateTime.Now);

            if (checkPeriod()) {
                return daoS0072.updatePeriodData(periodTable).Status;//base.Save_Override(periodTable, "SPAN_PERIOD", DBName.CFO);
            }
            else {
                return ResultStatus.FailButNext;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e) {
            wf_get_hzisp();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            BaseButton btn = (BaseButton)sender;
            string module = btn.Name.Split('_')[0];
            GridView gv = GetGridView(module);

            base.InsertRow(gv);
        }

        private void btnDel_Click(object sender, EventArgs e) {
            BaseButton btn = (BaseButton)sender;
            string module = btn.Name.Split('_')[0];
            GridView gv = GetGridView(module);

            base.DeleteRow(gv);
        }

        private void btnClear_Click(object sender, EventArgs e) {
            BaseButton btn = (BaseButton)sender;
            string module = btn.Name.Split('_')[0];
            GridView gv = GetGridView(module);

            if (MessageDisplay.Choose("確定刪除" + module + "設定所有資料?").AsBool()) {
                while (gv.DataRowCount != 0)
                    gv.DeleteRow(0);
            }
        }

        #region Grid 跨欄位合併標題
        //必須先宣告gvZISP.CustomDrawColumnHeader += gvZISP_CustomDrawColumnHeader;
        Rectangle boundsPrevColumn;

        protected void gvZISP_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e) {
            //if (e.Column == null)
            //    return;
            //if (e.Column.FieldName == "SPAN_ZISP_DPSR1") {
            //    // e.Info.Caption = String.Empty;
            //    boundsPrevColumn = e.Info.Bounds;//前面那個準備被覆蓋的欄位
            //    e.Handled = true;
            //}

            ////後面那個欄位往前覆蓋
            //if (e.Column.FieldName == "SPAN_ZISP_DPSR2") {
            //    Rectangle bounds = (e.Painter as HeaderObjectPainter).CalcObjectBounds(e.Info);
            //    e.Info.Bounds = new Rectangle(boundsPrevColumn.X, bounds.Y, bounds.Width + boundsPrevColumn.Width, bounds.Height);
            //}
        }

        #endregion

        private void ZISP_btnClear_Click(object sender, EventArgs e) {
            if (MessageDisplay.Choose("確定刪除跨商品價差設定所有資料?").AsBool()) {
                while (gv_ZISP.DataRowCount != 0)
                    gv_ZISP.DeleteRow(0);
            }
        }

        private void gvZISP_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["SPAN_ZISP_USER_ID"], GlobalInfo.USER_ID);
        }

        private void InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            string module = gv.Name.Split('_')[1];

            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["SPAN_CONTENT_MODULE"], module);
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["SPAN_CONTENT_USER_ID"], GlobalInfo.USER_ID);
        }

        private void gvZISP_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]).ToString();

            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                e.Cancel = false;
                gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
            }
            //既有資料群組類別 Leg1 Leg2 不能編輯
            else if (gv.FocusedColumn.Name == "SPAN_ZISP_PROD_ID" ||
                gv.FocusedColumn.Name == "SPAN_ZISP_COM_PROD1" ||
                 gv.FocusedColumn.Name == "SPAN_ZISP_COM_PROD2") {
                e.Cancel = true;
            }
        }

        private void gvZISP_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            //要用RowHandle不要用FocusedRowHandle
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]).ToString();
            e.Column.OptionsColumn.AllowFocus = true;

            if (e.Column.FieldName == "SPAN_ZISP_PROD_ID" ||
                e.Column.FieldName == "SPAN_ZISP_COM_PROD1" ||
                 e.Column.FieldName == "SPAN_ZISP_COM_PROD2") {
                e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
            }
        }

        private void GridView_ShownEditor(object sender, EventArgs e) {
            GridView view = (GridView)sender;
            if (view.FocusedColumn.FieldName == "SPAN_CONTENT_CC") {
                string prodType = view.GetFocusedRowCellValue(view.Columns["SPAN_CONTENT_CLASS"]).ToString();
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                DataTable dtProd = new DataTable();
                RepositoryItemLookUpEdit cbxProd = new RepositoryItemLookUpEdit();
                //修改商品組合下拉清單(重綁data source)
                switch (prodType) {
                    case "EQT-STF":
                    case "EQT-ETF": {
                            string[] prodEQT = prodType.Split('-');
                            dtProd = daoS0072.dddw_zparm_comb_prod_by_group(txtEndDate.DateTimeValue.ToString("yyyyMMdd"), "%" + prodEQT[0] + "%", "%" + prodEQT[1] + "%");
                            break;
                        }
                    default: {
                            dtProd = daoS0072.dddw_zparm_comb_prod_by_group(txtEndDate.DateTimeValue.ToString("yyyyMMdd"), "%" + prodType + "%", "%%");
                            break;
                        }
                }
                cbxProd.SetColumnLookUp(dtProd, "comb_prod_value", "comb_prod", TextEditStyles.DisableTextEditor);
                edit.Properties.DataSource = cbxProd.DataSource;
                edit.ShowPopup();
            }
        }

        private void ZISP_gridView_ShownEditor(object sender, EventArgs e) {
            GridView view = (GridView)sender;
            if (view.FocusedColumn.FieldName == "SPAN_ZISP_COM_PROD1" ||
                view.FocusedColumn.FieldName == "SPAN_ZISP_COM_PROD2") {
                string prodType = view.GetFocusedRowCellValue(SPAN_ZISP_PROD_ID).ToString();
                LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
                DataTable dtProd = new DataTable();
                RepositoryItemLookUpEdit cbxProd = new RepositoryItemLookUpEdit();
                //修改商品組合下拉清單(重綁data source)
                switch (prodType) {
                    case "EQT-STF":
                    case "EQT-ETF": {
                            string[] prodEQT = prodType.Split('-');
                            dtProd = daoS0072.dddw_zparm_comb_prod_by_group(txtEndDate.DateTimeValue.ToString("yyyyMMdd"), "%" + prodEQT[0] + "%", "%" + prodEQT[1] + "%");
                            break;
                        }
                    default: {
                            dtProd = daoS0072.dddw_zparm_comb_prod_by_group(txtEndDate.DateTimeValue.ToString("yyyyMMdd"), "%" + prodType + "%", "%%");
                            break;
                        }
                }
                cbxProd.SetColumnLookUp(dtProd, "comb_prod_value", "comb_prod", TextEditStyles.DisableTextEditor);
                edit.Properties.DataSource = cbxProd.DataSource;
                edit.ShowPopup();
            }
        }

        private void cbxProdType_EditValueChanged(object sender, EventArgs e) {
            LookUpEdit lookUp = (LookUpEdit)sender;
            GridControl gc = (GridControl)lookUp.Parent;
            GridView gv = (GridView)gc.MainView;
            gv.PostEditor();
            if (gv.Name == "gv_ZISP") {
                gv.SetFocusedRowCellValue(SpanZispColumn.SPAN_ZISP_COM_PROD1.ToString(), null);
                gv.SetFocusedRowCellValue(SpanZispColumn.SPAN_ZISP_COM_PROD2.ToString(), null);
            }
            else {
                gv.SetFocusedRowCellValue("SPAN_CONTENT_CC", null);
            }
        }

        protected void wf_get_hzisp() {
            if (txtEndDate.DateTimeValue.Subtract(txtStartDate.DateTimeValue).Days > 31) {
                MessageDisplay.Info("請輸入正確的日期區間，勿超過31天");
            }
            else {
                while (gv_ZISP.DataRowCount != 0) {
                    gv_ZISP.DeleteRow(0);
                }

                DataTable dtHZISP = daoS0072.hzisp(txtStartDate.DateTimeValue.ToString("yyyyMMdd"), txtEndDate.DateTimeValue.ToString("yyyyMMdd"));

                gv_ZISP.UpdateCurrentRow();

                for (int i = 0; i < dtHZISP.Rows.Count; i++) {
                    gv_ZISP.AddNewRow();
                    gv_ZISP.UpdateCurrentRow();

                    foreach (string column in Enum.GetNames(typeof(SpanZispColumn))) {
                        gv_ZISP.SetRowCellValue(i, column, dtHZISP.Rows[i][column]);
                    }
                }
            }
        }

        private void SpanTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e) {
            if (e.Page.Name != "tab_ZISP") {
                string module = e.Page.Name.Split('_')[1];
                string cod_col_id = module + "_METHOD";

                DataTable dtContentType = daoCod.ListByCol2("S0072", cod_col_id);
                RepositoryItemLookUpEdit cbxContentType = new RepositoryItemLookUpEdit();
                cbxContentType.SetColumnLookUp(dtContentType, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor);

                GridView gv = GetGridView(module);
                GridColumn column = gv.Columns[3]; //設定方式欄位
                gv.GridControl.RepositoryItems.Add(cbxContentType);
                column.ColumnEdit = cbxContentType;
            }
        }

        /// <summary>
        /// Get grid View by module Name 
        /// </summary>
        /// <param name="modeuleName"></param>
        /// <returns></returns>
        private GridView GetGridView(string modeuleName) {
            Control[] gridControls = this.Controls.Find("gc_" + modeuleName, true);
            GridControl gc = (GridControl)gridControls[0];
            return gc.MainView as GridView;
        }

        private void setDatePeriod() {
            DataTable dtSPN = daoS0072.d_s0070_1("SPN", GlobalInfo.USER_ID);
            if (dtSPN.Rows.Count <= 0) {
                is_fm_ymd = DateTime.Now.AddDays(-60).ToString("yyyyMMdd");
                is_to_ymd = DateTime.Now.ToString("yyyyMMdd");
                is_max_ymd = new AOCF().GetMaxDate(is_fm_ymd, is_to_ymd);

                txtStartDate.DateTimeValue = dtSPN.Rows[0]["SPAN_PERIOD_START_DATE"].AsDateTime("yyyyMMdd");
                txtEndDate.DateTimeValue = dtSPN.Rows[0]["SPAN_PERIOD_END_DATE"].AsDateTime("yyyyMMdd");
            }
            else {
                txtStartDate.DateTimeValue = dtSPN.Rows[0]["SPAN_PERIOD_START_DATE"].AsDateTime("yyyyMMdd");
                txtEndDate.DateTimeValue = dtSPN.Rows[0]["SPAN_PERIOD_END_DATE"].AsDateTime("yyyyMMdd");
            }
        }

        private bool checkChanged() {
            for (int i = 0; i < modules1.Length; i++) {
                GridView gv = GetGridView(modules1[i]);

                gv.CloseEditor();
                gv.UpdateCurrentRow();

                DataTable dt = (DataTable)gv.GridControl.DataSource;
                DataTable dtChange = dt.GetChanges();

                if (dtChange != null) {
                    if (dtChange.Rows.Count != 0) {
                        return true;
                    }
                }
            }

            for (int i = 0; i < module2.Length; i++) {
                GridView gv = GetGridView(module2[i]);

                gv.CloseEditor();
                gv.UpdateCurrentRow();

                DataTable dtZISP = (DataTable)gv.GridControl.DataSource;
                DataTable dtZISPchange = dtZISP.GetChanges();

                if (dtZISPchange != null) {
                    if (dtZISPchange.Rows.Count != 0) {
                        return true;
                    }
                }
            }

            DataTable periodTable = daoS0072.d_s0070_1("SPN", GlobalInfo.USER_ID);
            if (periodTable != null) {
                if (periodTable.Rows.Count != 0) {
                    string sartDate = periodTable.Rows[0]["span_period_start_date"].ToString();
                    string endDate = periodTable.Rows[0]["span_period_end_date"].ToString();

                    if (txtStartDate.DateTimeValue.ToString("yyyyMMdd") != sartDate ||
                        txtEndDate.DateTimeValue.ToString("yyyyMMdd") != endDate) {
                        return true;
                    }
                }
            }
            else {
                return true;
            }

            return false;
        }

        private bool checkComplete() {
            bool completed = true;

            for (int i = 0; i < modules1.Length; i++) {
                GridView gv = GetGridView(modules1[i]);

                gv.CloseEditor();
                gv.UpdateCurrentRow();
                DataTable dt = (DataTable)gv.GridControl.DataSource;

                for (int j = 0; j < dt.Rows.Count; j++) {
                    if (dt.Rows[j].RowState != DataRowState.Deleted) {
                        dt.Rows[j][6] = DateTime.Now; //更新寫入時間
                    }
                }

                foreach (DataColumn column in dt.Columns) {
                    if (dt.Rows.OfType<DataRow>().Where(r => r.RowState != DataRowState.Deleted).Any(r => r.IsNull(column))) {
                        MessageDisplay.Error(modules1[i] + "尚未填寫完成");
                        return false;
                    }
                }
            }

            for (int i = 0; i < module2.Length; i++) {
                GridView gv = GetGridView(module2[i]);

                gv.CloseEditor();
                gv.UpdateCurrentRow();

                DataTable dtZISP = (DataTable)gv.GridControl.DataSource;
                for (int j = 0; j < dtZISP.Rows.Count; j++) {
                    if (dtZISP.Rows[j].RowState != DataRowState.Deleted) {
                        dtZISP.Rows[j][7] = GlobalInfo.USER_ID;
                        dtZISP.Rows[j][8] = DateTime.Now; //更新寫入時間
                    }
                }

                foreach (DataColumn column in dtZISP.Columns) {
                    if (dtZISP.Rows.OfType<DataRow>().Where(r => r.RowState != DataRowState.Deleted).Any(r => r.IsNull(column) ||
                    string.IsNullOrEmpty(r["SPAN_ZISP_PRIORITY"].ToString()))) {
                        MessageDisplay.Error("跨商品價差設定尚未填寫完成");
                        return false;
                    }
                }
            }

            return completed;
        }

        private bool checkPeriod() {
            bool check = true;

            if (txtEndDate.DateTimeValue.Subtract(txtStartDate.DateTimeValue).Days > 31) {
                MessageDisplay.Info("日期區間不可超過31天!");
                txtEndDate.Select();
                check = false;
            }
            else if (txtStartDate.DateTimeValue > txtEndDate.DateTimeValue) {
                MessageDisplay.Info("起始值不可大於迄止值!");
                txtStartDate.Select();
                check = false;
            }
            return check;
        }

        private enum SpanZispColumn
        {
            SPAN_ZISP_PROD_ID = 0,
            SPAN_ZISP_COM_PROD1,
            SPAN_ZISP_COM_PROD2,
            SPAN_ZISP_USER_ID,
            SPAN_ZISP_PRIORITY,
            SPAN_ZISP_CREDIT,
            SPAN_ZISP_DPSR1,
            SPAN_ZISP_DPSR2,
            SPAN_ZISP_W_TIME
        }
    }
}
