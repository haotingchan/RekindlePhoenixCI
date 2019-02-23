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
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Columns;
using System.ComponentModel;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using System.Windows.Forms;
using BusinessObjects;
using static DataObjects.Dao.DataGate;
using DataObjects.Dao;
using System.Linq;

/// <summary>
/// david,2019/1/17
/// </summary>
namespace PhoenixCI.FormUI.PrefixS
{
    /// <summary>
    /// SPAN參數檔案調整模組
    /// </summary>
    public partial class WS0072 : FormParent
    {
        protected DS0072 daoS0072;
        protected COD daoCod;
        protected string is_fm_ymd;
        protected string is_to_ymd;
        protected DateTime is_max_ymd;
        protected string[] modules = { "PSR", "VSR", "INTERMONTH", "SOM" };


        public WS0072(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtStartDate.EnterMoveNextControl = true;
            txtEndDate.EnterMoveNextControl = true;
            GridHelper.SetCommonGrid(gv_ZISP);
            for (int i = 0; i < modules.Length; i++) {
                Control[] gridControls = this.Controls.Find("gc_" + modules[i], true);
                GridControl gc = (GridControl)gridControls[0];

                GridHelper.SetCommonGrid(gc.MainView as GridView);
            }
            daoS0072 = new DS0072();
            daoCod = new COD();

            //1.設定初始年月yyyy/MM
            DataTable dtSPN = daoS0072.d_s0070_1("SPN", GlobalInfo.USER_ID);
            if (dtSPN.Rows.Count <= 0) {
                is_fm_ymd = DateTime.Now.AddDays(-60).ToString("yyyyMMdd");
                is_to_ymd = DateTime.Now.ToString("yyyyMMdd");
                is_max_ymd = new AOCF().GetMaxDate(is_fm_ymd, is_to_ymd);

                txtStartDate.DateTimeValue = DateTime.ParseExact(is_fm_ymd, "yyyyMMdd", null);
                txtEndDate.DateTimeValue = DateTime.ParseExact(is_to_ymd, "yyyyMMdd", null);
            }
            else {
                txtStartDate.DateTimeValue = DateTime.ParseExact(dtSPN.Rows[0]["SPAN_PERIOD_START_DATE"].AsString(), "yyyyMMdd", null);
                txtEndDate.DateTimeValue = DateTime.ParseExact(dtSPN.Rows[0]["SPAN_PERIOD_END_DATE"].AsString(), "yyyyMMdd", null);
            }

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
            for (int i = 0; i < modules.Length; i++) {
                Control[] gridControls = this.Controls.Find("gc_" + modules[i], true);
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

        /// <summary>
        /// 按下[讀取/預覽]按鈕時,去資料庫撈資料
        /// </summary>
        /// <returns></returns>
        protected override ResultStatus Retrieve() {
            if (txtEndDate.DateTimeValue.Subtract(txtStartDate.DateTimeValue).Days > 31) {
                MessageDisplay.Info("請輸入正確的日期區間，勿超過31天");
            }
            else {
                base.Retrieve();
                DataTable dtZISP = daoS0072.zisp(GlobalInfo.USER_ID);
                gc_ZISP.DataSource = dtZISP;

                for (int i = 0; i < modules.Length; i++) {
                    DataTable dt = daoS0072.ListSpanContentByModule(modules[i], GlobalInfo.USER_ID);
                    Control[] gridControls = this.Controls.Find("gc_" + modules[i], true);
                    GridControl gc = (GridControl)gridControls[0];

                    gc.DataSource = dt;
                }
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
            _IsPreventFlowPrint = true;
            ResultStatus resultStatus = ResultStatus.Fail;
            //儲存VSR PSR InterMonth SOM 
            if (checkComplete()) {
                if (checkChanged()) {
                    for (int i = 0; i < modules.Length; i++) {
                        Control[] gridControls = this.Controls.Find("gc_" + modules[i], true);
                        GridControl gc = (GridControl)gridControls[0];
                        GridView gv = gc.MainView as GridView;

                        gv.CloseEditor();
                        gv.UpdateCurrentRow();

                        DataTable dt = (DataTable)gc.DataSource;
                        for (int j = 0; j < dt.Rows.Count; j++) {
                            if (dt.Rows[j].RowState != DataRowState.Deleted) {
                                dt.Rows[j][6] = DateTime.Now; //更新寫入時間
                            }
                        }
                        resultStatus = base.Save_Override(dt, "SPAN_CONTENT", DBName.CFO);
                    }

                    gv_ZISP.CloseEditor();
                    gv_ZISP.UpdateCurrentRow();

                    DataTable dtZISP = (DataTable)gc_ZISP.DataSource;
                    for (int i = 0; i < dtZISP.Rows.Count; i++) {
                        if (dtZISP.Rows[i].RowState != DataRowState.Deleted) {
                            dtZISP.Rows[i][7] = GlobalInfo.USER_ID;
                            dtZISP.Rows[i][8] = DateTime.Now; //更新寫入時間
                        }
                    }
                    string tableName = "CFO.SPAN_ZISP";
                    string keysColumnList = "SPAN_ZISP_PROD_ID,SPAN_ZISP_COM_PROD1,SPAN_ZISP_COM_PROD2,SPAN_ZISP_USER_ID";
                    string insertColumnList = "SPAN_ZISP_PROD_ID,SPAN_ZISP_PRIORITY,SPAN_ZISP_COM_PROD1,SPAN_ZISP_COM_PROD2," +
                        "SPAN_ZISP_CREDIT,SPAN_ZISP_DPSR1,  SPAN_ZISP_DPSR2, SPAN_ZISP_USER_ID,SPAN_ZISP_W_TIME";
                    string updateColumnList = insertColumnList;

                    resultStatus = serviceCommon.SaveForChanged(dtZISP, tableName, insertColumnList, updateColumnList, keysColumnList, args).Status;
                }
                else {
                    MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    resultStatus = ResultStatus.FailButNext;
                }
            }
            else {
                resultStatus = ResultStatus.FailButNext;
            }
            return resultStatus;
        }

        private void btnLoad_Click(object sender, EventArgs e) {
            wf_get_hzisp();
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            BaseButton btn = (BaseButton)sender;
            string module = btn.Name.Split('_')[0];
            Control[] gridControls = this.Controls.Find("gc_" + module, true);
            GridControl gc = (GridControl)gridControls[0];

            base.InsertRow(gc.MainView as GridView);
        }

        private void btnDel_Click(object sender, EventArgs e) {
            BaseButton btn = (BaseButton)sender;
            string module = btn.Name.Split('_')[0];
            Control[] gridControls = this.Controls.Find("gc_" + module, true);
            GridControl gc = (GridControl)gridControls[0];

            base.DeleteRow(gc.MainView as GridView);
        }

        private void btnClear_Click(object sender, EventArgs e) {
            BaseButton btn = (BaseButton)sender;
            string module = btn.Name.Split('_')[0];
            Control[] gridControls = this.Controls.Find("gc_" + module, true);
            GridControl gc = (GridControl)gridControls[0];
            GridView gv = gc.MainView as GridView;

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
                gv.SetFocusedRowCellValue("SPAN_ZISP_COM_PROD1", null);
                gv.SetFocusedRowCellValue("SPAN_ZISP_COM_PROD2", null);
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

                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_PROD_ID", dtHZISP.Rows[i]["SPAN_ZISP_PROD_ID"]);
                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_COM_PROD1", dtHZISP.Rows[i]["SPAN_ZISP_COM_PROD1"]);
                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_COM_PROD2", dtHZISP.Rows[i]["SPAN_ZISP_COM_PROD2"]);
                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_CREDIT", dtHZISP.Rows[i]["SPAN_ZISP_CREDIT"]);
                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_DPSR1", dtHZISP.Rows[i]["SPAN_ZISP_DPSR1"]);
                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_DPSR2", dtHZISP.Rows[i]["SPAN_ZISP_DPSR2"]);
                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_PRIORITY", dtHZISP.Rows[i]["SPAN_ZISP_PRIORITY"]);
                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_USER_ID", dtHZISP.Rows[i]["SPAN_ZISP_USER_ID"]);
                    gv_ZISP.SetRowCellValue(i, "SPAN_ZISP_W_TIME", dtHZISP.Rows[i]["SPAN_ZISP_W_TIME"]);
                    gv_ZISP.SetRowCellValue(i, "IS_NEWROW", dtHZISP.Rows[i]["IS_NEWROW"]);
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

                Control[] gridControls = this.Controls.Find("gc_" + module, true);
                GridControl gc = (GridControl)gridControls[0];
                GridView gv = gc.MainView as GridView;
                GridColumn column = gv.Columns[3]; //設定方式欄位
                gc.RepositoryItems.Add(cbxContentType);
                column.ColumnEdit = cbxContentType;
            }
        }

        private bool checkChanged() {
            bool hasChange = false;
            int changeCount = 0;
            for (int i = 0; i < modules.Length; i++) {
                Control[] gridControls = this.Controls.Find("gc_" + modules[i], true);
                GridControl gc = (GridControl)gridControls[0];
                GridView gv = gc.MainView as GridView;

                gv.CloseEditor();
                gv.UpdateCurrentRow();

                DataTable dt = (DataTable)gc.DataSource;
                DataTable dtChange = dt.GetChanges();

                if (dtChange != null) {
                    if (dtChange.Rows.Count != 0) {
                        changeCount++;
                    }
                }
            }

            gv_ZISP.CloseEditor();
            gv_ZISP.UpdateCurrentRow();

            DataTable dtZISP = (DataTable)gc_ZISP.DataSource;
            DataTable dtZISPchange = dtZISP.GetChanges();

            if (dtZISPchange != null) {
                if (dtZISPchange.Rows.Count != 0) {
                    changeCount++;
                }
            }

            if (changeCount != 0) {
                hasChange = true;
            }

            return hasChange;
        }

        private bool checkComplete() {
            bool completed = true;

            for (int i = 0; i < modules.Length; i++) {
                Control[] gridControls = this.Controls.Find("gc_" + modules[i], true);
                GridControl gc = (GridControl)gridControls[0];
                GridView gv = gc.MainView as GridView;

                gv.CloseEditor();
                gv.UpdateCurrentRow();
                DataTable dt = (DataTable)gc.DataSource;

                foreach (DataColumn column in dt.Columns) {
                    if (dt.Rows.OfType<DataRow>().Where(r => r.RowState != DataRowState.Deleted).Any(r => r.IsNull(column))) {
                        MessageDisplay.Error(modules[i] + "尚未填寫完成");
                        return false;
                    }
                }
            }

            gv_ZISP.CloseEditor();
            gv_ZISP.UpdateCurrentRow();
            DataTable dtZISP = (DataTable)gc_ZISP.DataSource;

            foreach (DataColumn column in dtZISP.Columns) {
                if (dtZISP.Rows.OfType<DataRow>().Where(r => r.RowState != DataRowState.Deleted).Any(r => r.IsNull(column))) {
                    MessageDisplay.Error("跨商品價差設定尚未填寫完成");
                    return false;
                }
            }

            return completed;
        }
    }
}
