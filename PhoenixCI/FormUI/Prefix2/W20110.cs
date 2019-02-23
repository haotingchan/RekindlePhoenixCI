using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BaseGround;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Repository;
using System.IO;
using System.Media;
using DevExpress.Spreadsheet;
using BusinessObjects;
using static DataObjects.Dao.DataGate;

/// <summary>
/// Lukas, 2019/1/29
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {

    /// <summary>
    /// 20110 每日各商品現貨指數資料輸入
    /// </summary>
    public partial class W20110 : FormParent {

        private D20110 dao20110;
        private OCFG daoOCFG;
        private RPT daoRPT;
        private AMIF daoAMIF;
        private STWD daoSTWD;
        private COD daoCOD;
        private APDK daoAPDK;
        protected class LookupItem {
            public string ValueMember { get; set; }
            public string DisplayMember { get; set; }
        }
        protected DataTable dtCheck;
        protected DataTable dtProd;

        public W20110(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            dao20110 = new D20110();
            daoAPDK = new APDK();
            txtDate.Text = PbFunc.f_ocf_date(0);
            //在這邊先撈，因為在不同的事件中會重複用到
            dtCheck = dao20110.d_20110(txtDate.Text.AsDateTime());
        }

        protected override ResultStatus Open() {
            base.Open();
            //隱藏一些開發用的資訊和測試按鈕

            #region 處理下拉選單
            //商品下拉選單
            RepositoryItemLookUpEdit ddlProd = new RepositoryItemLookUpEdit();
            dtProd = daoAPDK.ListAllF_20110();

            #region 資料表內的資料不乾淨,比下拉選單項目還多,必須先做資料合併再設定到下拉選單中
            DataView view = new DataView(dtCheck);
            DataTable dtTemp = view.ToTable(true, "AMIF_KIND_ID");
            dtTemp.Columns[0].ColumnName = "PDK_KIND_ID";
            foreach (DataRow dr in dtTemp.Rows) {
                dr[0] = dr[0].AsString() + "    ";
            }
            dtTemp.PrimaryKey = new DataColumn[] { dtTemp.Columns["PDK_KIND_ID"] };
            dtProd.PrimaryKey = new DataColumn[] { dtProd.Columns["PDK_KIND_ID"] };
            dtTemp.Merge(dtProd, false);
            dtProd = dtTemp;
            #endregion

            ddlProd.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.True;
            ddlProd.ProcessNewValue += ddlProd_ProcessNewValue;//開放輸入下拉選單沒有的資訊

            ddlProd.SetColumnLookUp(dtProd, "PDK_KIND_ID", "PDK_KIND_ID", TextEditStyles.DisableTextEditor, "");
            gcMain.RepositoryItems.Add(ddlProd);
            AMIF_KIND_ID.ColumnEdit = ddlProd;
            //盤別下拉選單
            List<LookupItem> ddlb_grp = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1", DisplayMember = "16:15收盤"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "全部收盤" }};
            Extension.SetDataTable(ddlType, ddlb_grp, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");
            #endregion

            return ResultStatus.Success;
        }

        #region LookUpEdit事件

        private void ddlProd_ProcessNewValue(object sender, ProcessNewValueEventArgs e) {
            if (e.DisplayValue == null || string.Empty.Equals(e.DisplayValue))
                return;

            DataRow dr = dtProd.NewRow();
            dr["PDK_KIND_ID"] = e.DisplayValue;
            dtProd.Rows.Add(dr);

            e.Handled = true;
        }
        #endregion

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();
            daoOCFG = new OCFG();
            //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
            //原本資料在這邊撈，移到外面去

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

            //call OCFG的f_get_txn_osw_grp
            if (daoOCFG.f_get_txn_osw_grp(_ProgramID) == "1" ||
                daoOCFG.f_get_txn_osw_grp(_ProgramID) == "5") {
                ddlType.ItemIndex = 0;
            }
            else {
                ddlType.ItemIndex = 1;
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

            DataTable returnTable = dao20110.d_20110(txtDate.DateTimeValue);

            if (returnTable.Rows.Count == 0) {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            returnTable.Columns.Add("Is_NewRow", typeof(string));
            gcMain.DataSource = returnTable;
            gcMain.Focus();

            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            daoRPT = new RPT();
            int i, li_cnt, ll_found = 0;
            string ls_kind_id = "I5F";
            if (gvMain.RowCount == 0) {
                DialogResult result = MessageBox.Show("請問要新增「" + txtDate.Text + "」所有商品資料?", "請選擇", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) {
                    ll_found = 0;
                }
                else {
                    ll_found = 99;
                }
            }
            else {
                //應該要有的個數
                ll_found = daoRPT.RowCount();
                li_cnt = 0;
                for (i = 0; i < gvMain.RowCount; i++) {
                    if (gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_SETTLE_DATE"]).AsString() == "指數") {
                        li_cnt = li_cnt + 1;
                    }
                }
                if (ll_found != li_cnt) {
                    DialogResult result = MessageBox.Show("請問要新增「" + txtDate.Text + "」有缺少的商品資料?", "請選擇", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) {
                        ll_found = 0;
                    }
                    else {
                        ll_found = 99;
                    }
                }
            }

            //自動新增
            if (ll_found == 0) {
                //PB: wf_insert_all()
                DataTable dtAMIFU = dao20110.d_20110_amifu(txtDate.DateTimeValue);
                DataTable dtRPT = daoRPT.ListAllByTXD_ID("20110");
                int li_row, li_seq_no, li_return;
                string ls_sub_type, ls_settle_date, ls_rtn;
                //如果資料表(AMIFU)沒資料
                if (dtAMIFU.Rows.Count == 0) {
                    //PB: wf_insert_all_zero()
                    for (i = 0; i < dtRPT.Rows.Count; i++) {
                        ls_kind_id = dtRPT.Rows[i]["RPT_VALUE"].AsString();
                        ls_settle_date = dtRPT.Rows[i]["RPT_VALUE_2"].AsString();
                        li_seq_no = dtRPT.Rows[i]["RPT_SEQ_NO"].AsInt();
                        if (ls_settle_date == "000000") {
                            ls_settle_date = "指數";
                        }
                        wf_insert_row(ls_kind_id, ls_settle_date, li_seq_no);
                    }
                    gvMain.BeginSort();
                    try {
                        gvMain.ClearSorting();
                        gvMain.Columns["RPT_SEQ_NO"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                        gvMain.Columns["AMIF_KIND_ID"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                        gvMain.Columns["AMIF_SETTLE_DATE"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

                    }
                    finally {
                        gvMain.EndSort();
                    }
                    //要Focus新資料列以外的地方Sort才會生效。
                    gvMain.FocusedRowHandle = 0;
                    wf_close_price();
                    return ResultStatus.Success;
                }
                //如果有資料，就copy到Grid
                dtAMIFU.Columns.Add("Is_NewRow", typeof(string));
                foreach (DataRow dr in dtAMIFU.Rows) {
                    dr["Is_NewRow"] = "1";
                }
                gcMain.DataSource = dtAMIFU;
                gcMain.RefreshDataSource();
                //補沒有轉入商品之空白
                if (ddlType.Text == "16:15收盤") {
                    DataView dv = dtRPT.AsDataView();
                    dv.RowFilter = "RPT_VALUE_4 <> '7'";
                    dtRPT = dv.ToTable();
                }
                for (i = 0; i < dtRPT.Rows.Count; i++) {
                    ls_kind_id = dtRPT.Rows[i]["RPT_VALUE"].AsString();
                    ls_settle_date = dtRPT.Rows[i]["RPT_VALUE_2"].AsString();
                    if (ls_settle_date != "000000") {
                        ls_settle_date = f_get_20110_settle_date(ls_settle_date, txtDate.DateTimeValue);
                    }
                    else {
                        ls_settle_date = "指數";
                    }
                    //應該要用gridview的資料判斷，但在這個階段其資料等於dtAMIFU，故利用後者做判斷是否要新增資料列
                    if (dtAMIFU.Select("AMIF_KIND_ID='" + ls_kind_id + "' and AMIF_SETTLE_DATE='" + ls_settle_date + "'").Length == 0) {
                        wf_insert_row(ls_kind_id, ls_settle_date, dtRPT.Rows[i]["RPT_SEQ_NO"].AsInt());
                    }
                }
                gvMain.BeginSort();
                try {
                    gvMain.ClearSorting();
                    gvMain.Columns["RPT_SEQ_NO"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    gvMain.Columns["AMIF_KIND_ID"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    gvMain.Columns["AMIF_SETTLE_DATE"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

                }
                finally {
                    gvMain.EndSort();
                }
                //要Focus新資料列以外的地方Sort才會生效。
                gvMain.FocusedRowHandle = 0;
                //前日收盤價
                wf_close_price();

                //JPX下載檔案檢核
                DataTable dtJTW = (DataTable)gcMain.DataSource;
                if (dtJTW.Select("AMIF_KIND_ID = 'JTW'").Length > 0) {
                    li_return = dtJTW.Rows.IndexOf(dtJTW.Select("AMIF_KIND_ID = 'JTW'")[0]);
                }
                else {
                    li_return = -1;
                }
                if (li_return >= 0) {
                    if (gvMain.GetRowCellValue(li_return, gvMain.Columns["Is_NewRow"]).AsString() == "1") {
                        DialogResult result = MessageBox.Show("請問是否要下載JPX網頁「http://www.jpx.co.jp/english/markets/derivatives/trading-volume/index.html」資料？",
                                                              "請選擇", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes) {
                            wf_jpx_get_file();
                        }
                    }
                }
                // 檢查收盤價為0
                for (i = 0; i < gvMain.RowCount; i++) {
                    if (gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE"]).AsDecimal() != 0 ||
                        gvMain.GetRowCellValue(i, gvMain.Columns["Is_NewRow"]).AsString() == "") {
                        continue;
                    }
                    DialogResult result = MessageBox.Show(gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_KIND_ID"]).AsString() + " 今日收盤價為0，是否要複製前日收盤價？",
                                                              "請選擇", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) {
                        gvMain.SetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE"], gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE_Y"]).AsDecimal());
                        gvMain.SetRowCellValue(i, gvMain.Columns["AMIF_UP_DOWN_VAL"], 0);
                    }
                }
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            //base.Save(gcMain);

            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();
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
            else {
                DateTime id_last_date;
                #region ue_save_before
                /******************************
                判斷是否讀取資料後直接改日期
                ex.2008/1/29 Retrieve
                   然後將交易日期改成2008/1/30
                    結果會變成1/29資料刪除,新增1/30
                *******************************/
                DateTime ldt_date, ldt_org;
                ldt_org = gvMain.GetRowCellValue(0, "AMIF_DATE").AsDateTime();
                ldt_date = txtDate.DateTimeValue;
                if (ldt_date != ldt_org) {
                    DialogResult result = MessageBox.Show(ldt_org.ToString("yyyy/MM/dd") + "原有資料將全部刪除,是否繼續存檔?",
                                                          "警告訊息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) {
                        return ResultStatus.Fail;
                    }
                }
                Cursor.Current = Cursors.WaitCursor;
                int i, li_seq_no = 0;
                string ls_rtn, ls_kind_id = "", ls_settle_date = "";
                DateTime ldt_upd_time;
                id_last_date = txtDate.DateTimeValue;
                ldt_upd_time = DateTime.Now;
                //重新排序 應該不用排，因為Grid無法任意插入列
                //DataView dv = dt.AsDataView();
                //dv.Sort = "RPT_SEQ_NO ASC, AMIF_KIND_ID ASC, AMIF_SETTLE_DATE ASC";
                //dt = dv.ToTable();
                //賦值
                for (i = 0; i < dt.Rows.Count; i++) {
                    DataRow dr = dt.Rows[i];
                    dr["AMIF_DATE"] = txtDate.DateTimeValue;
                    if (ls_kind_id != dr["AMIF_KIND_ID"].AsString()) {
                        ls_kind_id = dr["AMIF_KIND_ID"].AsString() + "    ";
                        ls_settle_date = dr["AMIF_SETTLE_DATE"].AsString();
                        li_seq_no = 1;
                    }
                    else if (ls_settle_date != dr["AMIF_SETTLE_DATE"].AsString()) {
                        ls_settle_date = dr["AMIF_SETTLE_DATE"].AsString();
                        li_seq_no = li_seq_no + 1;
                    }
                    if (dr.RowState == DataRowState.Unchanged) {
                        continue;
                    }

                    //若是空值則填入0
                    if (dr["AMIF_CLOSE_PRICE"] == null) {
                        dr["AMIF_CLOSE_PRICE"] = 0;
                    }
                    if (dr["AMIF_CLOSE_PRICE"].AsDecimal() == 0) {
                        DialogResult result = MessageBox.Show(ls_kind_id + " 收盤價為0，是否要繼續存檔?",
                                                          "錯誤訊息", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                        if (result == DialogResult.Cancel) {
                            return ResultStatus.Fail;
                        }
                    }
                    dr["AMIF_YEAR"] = txtDate.Text.SubStr(0, 4);

                    //契約基本資料
                    DataTable dtAPDK = daoAPDK.ListAllByKindId(ls_kind_id);
                    dr["AMIF_PROD_TYPE"] = dtAPDK.Rows[0]["APDK_PROD_TYPE"].AsString();
                    dr["AMIF_PROD_SUBTYPE"] = dtAPDK.Rows[0]["APDK_PROD_SUBTYPE"].AsString();
                    dr["AMIF_PARAM_KEY"] = dtAPDK.Rows[0]["APDK_PARAM_KEY"].AsString();
                    dr["AMIF_M_TIME"] = ldt_upd_time;
                    if (ls_settle_date == "指數") {
                        dr["AMIF_MTH_SEQ_NO"] = li_seq_no;
                    }

                    //商品3碼+月2碼
                    if (ls_settle_date != "指數" && ls_settle_date.SubStr(ls_settle_date.Length - 2, 2) != "00") {
                        dr["AMIF_PROD_ID"] = dtAPDK.Rows[0]["APDK_KIND_ID"].AsString() +
                                             ((ls_settle_date.SubStr(ls_settle_date.Length - 2, 2)).AsInt() + 64).AsString() +
                                             ls_settle_date.SubStr(3, 1);
                    }
                    else {
                        dr["AMIF_PROD_ID"] = dtAPDK.Rows[0]["APDK_KIND_ID"].AsString() + "00";
                    }
                    if (dr["AMIF_CLOSE_PRICE"].AsDecimal() != dr["AMIF_CLOSE_PRICE_ORIG"].AsDecimal()) {
                        id_last_date = PbFunc.relativedate(id_last_date, -1);
                    }
                }
                #endregion

                #region ue_save
                int li_return;
                /*******************
                轉完資料後執行SP
                *******************/
                string ls_prod_type = "M";
                ldt_date = txtDate.DateTimeValue;
                if (f_20110_SP(ldt_date, "20110") != "") {
                    return ResultStatus.Fail;
                }
                /*******************
                轉統計資料AI6
                異動歷史資料時,當日收盤價影響隔日的前日收盤價計算,所以重新計算隔日AI6
                *******************/
                if (ldt_date != id_last_date) {
                    id_last_date = dao20110.nextDay(ldt_date).AsDateTime();
                    if (ldt_date != id_last_date) {
                        ldt_date = id_last_date;
                        if (dao20110.sp_H_gen_AI6(ldt_date).Status != ResultStatus.Success) {
                            MessageBox.Show("執行SP(sp_H_gen_AI6)-" + ldt_date.ToString("yyyy/MM/dd") + "錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return ResultStatus.Fail;
                        }
                        else {
                            li_return = 0;
                        }
                    }
                }
                //更新AMIF
                //先把指數改回000000
                for (i = 0; i < dt.Rows.Count; i++) {
                    DataRow dr = dt.Rows[i];
                    if (dr.RowState != DataRowState.Unchanged && dr["AMIF_SETTLE_DATE"].AsString() == "指數") {
                        dr["AMIF_SETTLE_DATE"] = "000000";
                    }
                }
                string tableName = "CI.AMIF";
                string keysColumnList = "AMIF_DATE, AMIF_PROD_ID";
                string insertColumnList = @"AMIF_DATE, 
                                            AMIF_KIND_ID, 
                                            AMIF_SETTLE_DATE, 
                                            AMIF_OPEN_PRICE, 
                                            AMIF_HIGH_PRICE, 
                                            AMIF_LOW_PRICE,   
                                            AMIF_CLOSE_PRICE, 
                                            AMIF_UP_DOWN_VAL,
                                            AMIF_SETTLE_PRICE, 
                                            AMIF_M_QNTY_TAL,    
                                            AMIF_OPEN_INTEREST,   
                                            AMIF_SUM_AMT,       
                                            AMIF_PROD_TYPE,   
                                            AMIF_PROD_SUBTYPE,   
                                            AMIF_DATA_SOURCE,
                                            AMIF_EXCHANGE_RATE,
                                            AMIF_M_TIME,  
                                            AMIF_PARAM_KEY,   
                                            AMIF_STRIKE_PRICE, 
                                            AMIF_PC_CODE,    
                                            AMIF_PROD_ID,       
                                            AMIF_YEAR, 
                                            AMIF_MTH_SEQ_NO,
                                            AMIF_OSW_GRP";
                string updateColumnList = insertColumnList;
                try {
                    ResultData myResultData = serviceCommon.SaveForChanged(dt, tableName, insertColumnList, updateColumnList, keysColumnList, pokeBall);
                    if (myResultData.Status == ResultStatus.Fail) {
                        return ResultStatus.Fail;
                    }
                    else {
                        PbFunc.f_write_logf(_ProgramID, "I", "變更資料");
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }

                #endregion
            }
            //不要自動列印
            _IsPreventFlowPrint = true;
            return ResultStatus.Success;
        }

        #region GridControl事件

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
                if (gv.FocusedColumn.Name == "AMIF_CLOSE_PRICE_Y" ||
                    gv.FocusedColumn.Name == "AMIF_EXCHANGE_RATE" ||
                    gv.FocusedColumn.Name == "CP_ERR") {

                    e.Cancel = true;
                    if (gv.FocusedColumn.Name == "AMIF_EXCHANGE_RATE" &&
                        gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["AMIF_KIND_ID"]).AsString() == "GDF") {
                        e.Cancel = false;
                    }
                }
            }
            //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
            else if (gv.FocusedColumn.Name == "AMIF_KIND_ID" ||
                     gv.FocusedColumn.Name == "AMIF_SETTLE_DATE" ||
                     gv.FocusedColumn.Name == "AMIF_CLOSE_PRICE_Y" ||
                     gv.FocusedColumn.Name == "AMIF_EXCHANGE_RATE" ||
                     gv.FocusedColumn.Name == "CP_ERR") {

                e.Cancel = true;
                if (gv.FocusedColumn.Name == "AMIF_EXCHANGE_RATE" &&
                    gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["AMIF_KIND_ID"]).AsString() == "GDF") {
                    e.Cancel = false;
                }
            }
            else {
                e.Cancel = false;
            }
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            //要用RowHandle不要用FocusedRowHandle
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();
            string amifu_err_text = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIFU_ERR_TEXT"]).AsString();
            decimal amif_open_price = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_OPEN_PRICE"]).AsDecimal();
            decimal amif_high_price = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_HIGH_PRICE"]).AsDecimal();
            decimal amif_m_qnty_tal = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_M_QNTY_TAL"]).AsDecimal();
            decimal amif_low_price = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_LOW_PRICE"]).AsDecimal();
            decimal amif_close_price = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_CLOSE_PRICE"]).AsDecimal();
            decimal amif_up_down_val = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_UP_DOWN_VAL"]).AsDecimal();
            decimal amif_open_interest = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_OPEN_INTEREST"]).AsDecimal();

            //描述每個欄位,在is_newRow時候要顯示的顏色
            //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
            switch (e.Column.FieldName) {
                case ("AMIF_KIND_ID"):
                case ("AMIF_SETTLE_DATE"):
                    e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192, 192, 192);
                    break;
                case ("AMIF_OPEN_PRICE"):
                    if (amifu_err_text == null || amifu_err_text == "") {
                        if (amif_open_price != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_OPEN_PRICE"]).AsDecimal() ||
                            gv.GetRowCellValue(e.RowHandle, gv.Columns["R_OPEN_PRICE"]).AsString() == null) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                    }
                    else {
                        e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                    }
                    break;
                case ("AMIF_HIGH_PRICE"):
                    if (amifu_err_text == null || amifu_err_text == "") {
                        if (amif_high_price != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_HIGH_PRICE"]).AsDecimal() ||
                            gv.GetRowCellValue(e.RowHandle, gv.Columns["R_HIGH_PRICE"]).AsString() == null) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                    }
                    else {
                        e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                    }
                    break;
                case ("AMIF_M_QNTY_TAL"):
                    if (amifu_err_text == null || amifu_err_text == "") {
                        if (amif_m_qnty_tal != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_M_QNTY_TAL"]).AsDecimal() ||
                            gv.GetRowCellValue(e.RowHandle, gv.Columns["R_M_QNTY_TAL"]).AsString() == null) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                    }
                    else {
                        e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                    }
                    break;
                case ("AMIF_LOW_PRICE"):
                    if (amifu_err_text == null || amifu_err_text == "") {
                        if (amif_low_price != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_LOW_PRICE"]).AsDecimal() ||
                            gv.GetRowCellValue(e.RowHandle, gv.Columns["R_LOW_PRICE"]).AsString() == null) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                    }
                    else {
                        e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                    }
                    break;
                case ("AMIF_CLOSE_PRICE"):
                    if (amifu_err_text == null || amifu_err_text == "") {
                        if (amif_close_price != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_CLOSE_PRICE"]).AsDecimal() ||
                            gv.GetRowCellValue(e.RowHandle, gv.Columns["R_CLOSE_PRICE"]).AsString() == null) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                    }
                    else {
                        e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                    }
                    break;
                case ("AMIF_UP_DOWN_VAL"):
                    if (amifu_err_text == null || amifu_err_text == "") {
                        if (amif_up_down_val != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_UP_DOWN_VAL"]).AsDecimal() ||
                            gv.GetRowCellValue(e.RowHandle, gv.Columns["R_UP_DOWN_VAL"]).AsString() == null) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                    }
                    else {
                        e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                    }
                    break;
                case ("AMIF_OPEN_INTEREST"):
                    if (amifu_err_text == null || amifu_err_text == "") {
                        if (amif_open_interest != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_OPEN_INTEREST"]).AsDecimal() ||
                            gv.GetRowCellValue(e.RowHandle, gv.Columns["R_OPEN_INTEREST"]).AsString() == null) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                    }
                    else {
                        e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                    }
                    break;
                case ("AMIF_CLOSE_PRICE_Y"):
                    e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
                    break;
                case ("AMIF_EXCHANGE_RATE"):
                    if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_KIND_ID"]).AsString() == "GDF") {
                        e.Appearance.BackColor = Color.White;
                    }
                    else {
                        e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
                    }
                    break;
                case ("CP_ERR"):
                    e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
                    e.Appearance.ForeColor = Color.Red;
                    break;
                default:
                    break;
            }//switch (e.Column.FieldName) {

        }

        private void gvMain_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            string cp_err_text = "";
            if (e.Column.Name == "AMIF_OPEN_PRICE" ||
                e.Column.Name == "AMIF_HIGH_PRICE" ||
                e.Column.Name == "AMIF_LOW_PRICE" ||
                e.Column.Name == "AMIF_CLOSE_PRICE") {
                //CP_ERR 訊息顯示的欄位(PB的計算欄位)
                if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIFU_ERR_TEXT"]).AsString() == null ||
                    gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIFU_ERR_TEXT"]).AsString() == "") {

                    if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_OPEN_PRICE"]).AsDecimal() != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_OPEN_PRICE"]).AsDecimal()) {
                        cp_err_text += "開盤價與現貨(" + gv.GetRowCellValue(e.RowHandle, gv.Columns["R_OPEN_PRICE"]).AsString() + ")資料不同,";
                    }
                    if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_HIGH_PRICE"]).AsDecimal() != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_HIGH_PRICE"]).AsDecimal()) {
                        cp_err_text += "最高價價與現貨(" + gv.GetRowCellValue(e.RowHandle, gv.Columns["R_HIGH_PRICE"]).AsString() + ")資料不同,";
                    }
                    if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_LOW_PRICE"]).AsDecimal() != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_LOW_PRICE"]).AsDecimal()) {
                        cp_err_text += "最低價與現貨(" + gv.GetRowCellValue(e.RowHandle, gv.Columns["R_LOW_PRICE"]).AsString() + ")資料不同,";
                    }
                    if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_CLOSE_PRICE"]).AsDecimal() != gv.GetRowCellValue(e.RowHandle, gv.Columns["R_CLOSE_PRICE"]).AsDecimal()) {
                        cp_err_text += "收盤價與現貨(" + gv.GetRowCellValue(e.RowHandle, gv.Columns["R_CLOSE_PRICE"]).AsString() + ")資料不同,";
                    }
                    gv.SetRowCellValue(e.RowHandle, gv.Columns["CP_ERR"], cp_err_text);
                }
                else {
                    if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIFU_ERR_TEXT"]).AsString() == "0") {
                        cp_err_text = "無資料轉入來源";
                    }
                    else {
                        cp_err_text = "原始現貨接收檔案筆數" + gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIFU_ERR_TEXT"]).AsString() + "筆(有少)";
                    }
                    gv.SetRowCellValue(e.RowHandle, gv.Columns["CP_ERR"], cp_err_text);
                }
            }
            if (e.Column.FieldName == "AMIF_CLOSE_PRICE") {
                /*****************
                需9700243 97.10.06
                只要STW = 本日收盤價 - 昨日結算價
                若本日收盤 = 0 ,則漲跌=0
                *****************/
                if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_KIND_ID"]).AsString() == "STW"
                    && e.Value.AsDecimal() == 0) {
                    gv.SetRowCellValue(e.RowHandle, gv.Columns["AMIF_UP_DOWN_VAL"], 0);
                }
                else {
                    gv.SetRowCellValue(e.RowHandle, gv.Columns["AMIF_UP_DOWN_VAL"],
                        e.Value.AsDecimal() - gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_CLOSE_PRICE_Y"]).AsDecimal());
                }
            }
            /***************
            台幣黃金收盤價TGF
            ***************/
            if (gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_KIND_ID"]).AsString() == "GDF" &&
                e.Column.FieldName == "AMIF_CLOSE_PRICE" || e.Column.FieldName == "AMIF_EXCHANGE_RATE") {
                int ll_found;
                decimal ld_close_price, ld_rate;
                if (e.Column.FieldName == "AMIF_CLOSE_PRICE") {
                    ld_close_price = e.Value.AsDecimal();
                    ld_rate = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_EXCHANGE_RATE"]).AsDecimal();
                }
                else {
                    ld_close_price = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMIF_CLOSE_PRICE"]).AsDecimal();
                    ld_rate = e.Value.AsDecimal();
                }
                DataTable dtFind = (DataTable)gcMain.DataSource;
                if (dtFind.Select("trim(amif_kind_id)='TGF'").Length != 0) {
                    ll_found = dtFind.Rows.IndexOf(dtFind.Select("trim(amif_kind_id)='TGF'")[0]);
                }
                else {
                    ll_found = -1;
                }
                if (ll_found >= 0) {
                    gv.SetRowCellValue(ll_found, gv.Columns["AMIF_CLOSE_PRICE"], Math.Round(ld_close_price / 31.1035m * 3.75m * 0.9999m / 0.995m * ld_rate, 1));
                    gv.SetRowCellValue(ll_found, gv.Columns["AMIF_UP_DOWN_VAL"], Math.Round(ld_close_price / 31.1035m * 3.75m * 0.9999m / 0.995m * ld_rate, 1) - gv.GetRowCellValue(ll_found, gv.Columns["AMIF_CLOSE_PRICE_Y"]).AsDecimal());
                }
            }
        }

        #endregion

        private string f_get_20110_settle_date(string as_type, DateTime ad_date) {
            daoAMIF = new AMIF();
            string ls_settle_date = "";
            switch (as_type) {
                case ("YYYYMM"):
                    ls_settle_date = ad_date.ToString("yyyyMM");
                    break;
                case ("YYYYMM+1"):
                    int li_year, li_month;
                    li_year = ad_date.Year;
                    li_month = ad_date.Month;
                    li_month = li_month + 1;
                    if (li_month > 12) {
                        li_year = li_year + 1;
                        li_month = 1;
                    }
                    ls_settle_date = li_year.ToString("0000") + li_month.ToString("00");
                    break;
                case ("TX"):
                    ls_settle_date = daoAMIF.MinSettleDate(ad_date);
                    if (ls_settle_date == null) {
                        ls_settle_date = ad_date.ToString("yyyyMM");
                    }
                    break;
                default:
                    ls_settle_date = as_type;
                    break;

            }
            return ls_settle_date;
        }

        private void wf_close_price() {
            //設定AMIF日期區間,以改善效能
            daoSTWD = new STWD();
            DateTime ldt_date = txtDate.DateTimeValue;
            DateTime ldt_bef_date = PbFunc.relativedate(ldt_date, -30);
            ldt_date = daoAMIF.MaxAMIF_DATE(ldt_bef_date, ldt_date);
            DataTable dt20110_y = dao20110.d_20110_y(ldt_date);
            int i, ll_found;
            for (i = 0; i < dt20110_y.Rows.Count; i++) {
                DataRow dr = dt20110_y.Rows[i];
                if (dt20110_y.Select("AMIF_KIND_ID ='" + gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_KIND_ID"]).AsString() +
                    "' and AMIF_SETTLE_DATE ='" + gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_SETTLE_DATE"]).AsString() + "'").Length > 0) {
                    ll_found = dt20110_y.Rows.IndexOf(dt20110_y.Select("AMIF_KIND_ID ='" + gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_KIND_ID"]).AsString() +
                                                                       "' and AMIF_SETTLE_DATE ='" + gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_SETTLE_DATE"]).AsString() + "'")[0]);
                }
                else {
                    ll_found = -1;
                }
                if (ll_found >= 0) {
                    gvMain.SetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE_Y"], dr["AMIF_CLOSE_PRICE"].AsDecimal());
                    gvMain.SetRowCellValue(i, gvMain.Columns["AMIF_UP_DOWN_VAL"],
                                              gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE"]).AsDecimal() -
                                              gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE_Y"]).AsDecimal());
                    gvMain.SetRowCellValue(i, gvMain.Columns["R_UP_DOWN_VAL"],
                                              gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE"]).AsDecimal() -
                                              gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE_Y"]).AsDecimal());
                }
            }
            string ls_date, ls_m, ls_y;
            decimal ld_value;
            ls_date = ldt_date.ToString("yyyyMMdd");
            for (i = 0; i < gvMain.RowCount; i++) {
                if (gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_KIND_ID"]).AsString() != "STW") {
                    continue;
                }
                /*****************
                需9700243 97.10.06
                只要STW = 本日收盤價 - 昨日結算價
                若本日收盤 = 0 ,則漲跌=0
                *****************/
                ls_m = gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_SETTLE_DATE"]).AsString();
                ld_value = daoSTWD.GetSettlePrice(ls_date, ls_m);
                if (ld_value != 0) {
                    gvMain.SetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE_Y"], ld_value);
                    gvMain.SetRowCellValue(i, gvMain.Columns["AMIF_UP_DOWN_VAL"],
                                              gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE"]).AsDecimal() -
                                              gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE_Y"]).AsDecimal());
                    gvMain.SetRowCellValue(i, gvMain.Columns["R_UP_DOWN_VAL"],
                                              gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE"]).AsDecimal() -
                                              gvMain.GetRowCellValue(i, gvMain.Columns["AMIF_CLOSE_PRICE_Y"]).AsDecimal());
                }
            }
        }

        private void wf_insert_row(string ls_kind_id, string ls_settle_date, int li_seq_no) {
            try {
                gvMain.AddNewRow();
                int i = gvMain.RowCount - 1;
                gvMain.Focus();
                gvMain.SelectRow(i);
                //參數
                if (ls_settle_date != "指數") {
                    ls_settle_date = f_get_20110_settle_date(ls_settle_date, txtDate.DateTimeValue);
                }
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_KIND_ID"], (ls_kind_id + "       ").SubStr(0, 7));
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_SETTLE_DATE"], ls_settle_date);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["RPT_SEQ_NO"], li_seq_no);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIFU_ERR_TEXT"], "0");

                //由AP設定值,不能改
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_PROD_TYPE"], "F");
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_STRIKE_PRICE"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_PC_CODE"], " ");
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_DATA_SOURCE"], "U");
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_DATE"], txtDate.DateTimeValue);
                //預設值
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_OPEN_PRICE"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_HIGH_PRICE"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_LOW_PRICE"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_CLOSE_PRICE"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_UP_DOWN_VAL"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_M_QNTY_TAL"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_SETTLE_PRICE"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_OPEN_INTEREST"], 0);
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["AMIF_SUM_AMT"], 0);
                //用來判斷是否為new row
                gvMain.SetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["Is_NewRow"], "1");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void wf_jpx_get_file() {
            daoCOD = new COD();
            //下載
            string ls_ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
            //執行批次檔
            run_bat("20110", "20110_JTX", ls_ymd, Application.StartupPath + "\\");

            string ls_sender, ls_recipient, ls_cc, ls_title, ls_context;
            string ls_file, ls_file_oi, ls_pathname, ls_pathname_oi;
            DataTable dtCOD = daoCOD.W20110_filename();
            ls_file = dtCOD.Rows[0]["ls_file"].AsString();
            ls_file_oi = dtCOD.Rows[0]["ls_file_oi"].AsString();
            ls_file = ls_ymd + ls_file;
            ls_file_oi = ls_ymd + ls_file_oi;
            //這邊WIN Form預設路徑跟PB不同，所以修改了batch檔裡的路徑
            ls_pathname = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + "\\" + ls_file;
            ls_pathname_oi = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + "\\" + ls_file_oi;

            //Check檔案Exist
            if (!File.Exists(ls_pathname)) {
                MessageBox.Show("無法下載 " + ls_file + " 檔案比對價格，請自行上網取得！", "警告訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ls_pathname = "";
            }
            if (!File.Exists(ls_pathname_oi)) {
                MessageBox.Show("無法下載 " + ls_file_oi + " 檔案比對OI，請自行上網取得！", "警告訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ls_pathname_oi = "";
            }

            //開啟Excel
            Workbook wbJTW_M = new Workbook();
            Workbook wbJTW_OI = new Workbook();
            //價格,成交量
            if (ls_pathname != "") {
                bool ll_rtn = wbJTW_M.LoadDocument(ls_pathname);
                if (ll_rtn == false) {
                    MessageBox.Show("下載東證交易所檔案後開啟失敗，請自行上網取得！", ls_pathname, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else {
                    Worksheet wsJTW_M = wbJTW_M.Worksheets[0];
                    #region wf_jtw_m()
                    string ls_rpt_name, ls_rpt_id;
                    int ll_found, i;
                    /*************************************
                    Excel:
                    rpt_level_1:sheet
                    rpt_level_2:column
                    rpt_level_3:row

                    Table:
                    rpt_value   : kind_id
                    rpt_value_2 : Table column
                    rpt_value_3 : prod_type
                    *************************************/
                    ls_rpt_id = "20110JM";
                    ls_rpt_name = "JPX Derivatives Market Report";

                    /******************
                    讀取基本資料
                    ******************/
                    //報表位置
                    DataTable dt20110JM = daoRPT.ListAllByTXD_ID(ls_rpt_id);
                    if (dt20110JM.Rows.Count == 0) {
                        MessageBox.Show(ls_rpt_id + '－' + ls_rpt_name + ",讀取「報表檔」無任何資料!", "處理結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    DataView dv = dt20110JM.AsDataView();
                    dv.Sort = "rpt_level_1 ASC, rpt_level_2 ASC, rpt_level_3 ASC";
                    dt20110JM = dv.ToTable();
                    /******************
                    擷取結果值
                    ******************/
                    int li_sheet, li_sheet_l = 0, li_ole_row, li_ole_col, li_row = 0, li_col;
                    string ls_settle_date, ls_str;
                    decimal ld_value;
                    gvMain.CloseEditor();
                    for (i = 0; i < dt20110JM.Rows.Count; i++) {
                        DataRow dr = dt20110JM.Rows[i];
                        li_sheet = dr["RPT_LEVEL_1"].AsInt();
                        if (li_sheet != li_sheet_l) {
                            li_sheet_l = li_sheet;
                            //切換Sheet
                            wsJTW_M = wbJTW_M.Worksheets[li_sheet_l - 1];
                        }
                        li_ole_col = dr["RPT_LEVEL_2"].AsInt() - 1;
                        li_ole_row = dr["RPT_LEVEL_3"].AsInt() - 1;
                        //GridView Column
                        li_col = dr["RPT_VALUE_2"].AsString().AsInt() - 1;
                        ls_settle_date = wf_conv_settle_date(wsJTW_M.Cells[li_ole_row, 1].Value.AsString());
                        DataTable dtJTW = (DataTable)gcMain.DataSource;
                        if (dtJTW.Select("AMIF_KIND_ID = 'JTW' and amif_settle_date='" + ls_settle_date + "'").Length > 0) {
                            li_row = dtJTW.Rows.IndexOf(dtJTW.Select("AMIF_KIND_ID = 'JTW' and amif_settle_date='" + ls_settle_date + "'")[0]);
                        }
                        if (li_row >= 0) {
                            ld_value = wf_conv_price(wsJTW_M.Cells[li_ole_row, li_ole_col].Value.AsString());
                            if (ld_value > 0) {
                                gvMain.SetRowCellValue(li_row, ((ColName)li_col).AsString(), ld_value);
                            }
                        }
                    }

                    #endregion
                    wbJTW_M.Dispose();
                }
            }
            //OI
            if (ls_pathname_oi != "") {
                bool ll_rtn = wbJTW_OI.LoadDocument(ls_pathname_oi);
                if (ll_rtn == false) {
                    MessageBox.Show("下載東證交易所檔案後開啟失敗，請自行上網取得！", ls_pathname_oi, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else {
                    Worksheet wsJTW_OI = wbJTW_OI.Worksheets[0];
                    #region wf_jtw_oi()
                    string ls_rpt_name, ls_rpt_id;
                    int ll_found, i;
                    /*************************************
                    Excel:
                    rpt_level_1:sheet
                    rpt_level_2:column
                    rpt_level_3:row
                    
                    Table:
                    rpt_value   : kind_id
                    rpt_value_2 : Table column
                    rpt_value_3 : prod_type
                    *************************************/
                    ls_rpt_id = "20110JO";
                    ls_rpt_name = "JPX open_interest";
                    /******************
                    讀取基本資料
                    ******************/
                    //報表位置
                    DataTable dt20110JO = daoRPT.ListAllByTXD_ID(ls_rpt_id);
                    if (dt20110JO.Rows.Count == 0) {
                        MessageBox.Show(ls_rpt_id + '－' + ls_rpt_name + ",讀取「報表檔」無任何資料!", "處理結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    DataView dv = dt20110JO.AsDataView();
                    dv.Sort = "rpt_level_1 ASC, rpt_level_2 ASC, rpt_level_3 ASC";
                    dt20110JO = dv.ToTable();
                    /******************
                    擷取結果值
                    ******************/
                    int li_sheet, li_sheet_l = 0, li_ole_row, li_ole_col, li_row = 0;
                    string ls_settle_date, ls_str;
                    decimal ld_value;
                    gvMain.CloseEditor();
                    for (i = 0; i < dt20110JO.Rows.Count; i++) {
                        DataRow dr = dt20110JO.Rows[i];
                        li_sheet = dr["RPT_LEVEL_1"].AsInt();
                        if (li_sheet != li_sheet_l) {
                            li_sheet_l = li_sheet;
                            //切換Sheet
                            wsJTW_OI = wbJTW_OI.Worksheets[li_sheet_l - 1];
                        }
                        li_ole_col = dr["RPT_LEVEL_2"].AsInt() - 1;
                        li_ole_row = dr["RPT_LEVEL_3"].AsInt() - 1;
                        ls_settle_date = wf_conv_settle_date(wsJTW_OI.Cells[li_ole_row, li_ole_col - 1].Value.AsString());
                        DataTable dtJTW = (DataTable)gcMain.DataSource;
                        if (dtJTW.Select("AMIF_KIND_ID = 'JTW' and amif_settle_date='" + ls_settle_date + "'").Length > 0) {
                            li_row = dtJTW.Rows.IndexOf(dtJTW.Select("AMIF_KIND_ID = 'JTW' and amif_settle_date='" + ls_settle_date + "'")[0]);
                        }
                        if (li_row >= 0) {
                            //Vol
                            ld_value = wf_conv_price(wsJTW_OI.Cells[li_ole_row, li_ole_col].Value.AsString());
                            if (ld_value > 0) {
                                gvMain.SetRowCellValue(li_row, "AMIF_M_QNTY_TAL", ld_value);
                            }
                            //OI
                            ld_value = wf_conv_price(wsJTW_OI.Cells[li_ole_row, li_ole_col + 1].Value.AsString());
                            if (ld_value > 0) {
                                gvMain.SetRowCellValue(li_row, "AMIF_OPEN_INTEREST", ld_value);
                            }
                        }
                    }
                    #endregion
                    wbJTW_OI.Dispose();
                }
            }

            // 檢查收盤價為0
            for (int i = 0; i < gvMain.RowCount; i++) {
                if (gvMain.GetRowCellValue(i, "AMIF_CLOSE_PRICE").AsDecimal() != 0 ||
                    gvMain.GetRowCellValue(i, "AMIF_KIND_ID").AsString() != "JTW") {
                    continue;
                }
                DialogResult result = MessageBox.Show(gvMain.GetRowCellValue(i, "AMIF_KIND_ID").AsString() + " 今日收盤價為0，是否要複製前日收盤價？", "請選擇", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) {
                    gvMain.SetRowCellValue(i, "AMIF_CLOSE_PRICE", gvMain.GetRowCellValue(i, "AMIF_CLOSE_PRICE_Y").AsDecimal());
                    gvMain.SetRowCellValue(i, "AMIF_UP_DOWN_VAL", 0);
                }
            }
        }

        private void run_bat(string as_txn_id, string as_bat_filename, string as_param1, string as_param2) {
            string is_err, is_chk, ls_flag, ls_oper_bat;
            int k;
            ls_oper_bat = GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH + "\\" + as_bat_filename + ".bat";
            ls_flag = GlobalInfo.DEFAULT_BATCH_ErrSP_DIRECTORY_PATH + "\\" + as_bat_filename + "_flag.txt";

            //先刪掉既有的flag
            File.Delete(ls_flag);

            Process process = new Process();
            process.StartInfo.FileName = ls_oper_bat;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = string.Format(@"{0} {1}", as_param1, as_param2);
            process.Start();
            process.WaitForExit();
            k = process.ExitCode;
            if (k != 0) {
                SystemSounds.Beep.Play();
                MessageBox.Show("Run " + as_bat_filename + " 執行失敗!", "處理結果" + " 作業代號：" + as_txn_id, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            while (!File.Exists(ls_flag)) { };
            if (File.Exists(ls_flag)) {
                File.Delete(ls_flag);
            }
            return;
        }

        private string wf_conv_settle_date(string ls_settle_date) {
            ls_settle_date = ls_settle_date.SubStr(0, ls_settle_date.IndexOf(" "));
            DateTime rtnDate;
            if (DateTime.TryParse(ls_settle_date, out rtnDate)) {
                ls_settle_date = rtnDate.ToString("yyyyMM");
            }
            else {
                ls_settle_date = ls_settle_date.SubStr(0, 5);
            }
            return ls_settle_date;
        }

        private decimal wf_conv_price(string ls_str) {
            decimal ld_value = 0;
            if (ls_str == "-") {
                ls_str = "0";
            }
            else {
                ld_value = ls_str.AsDecimal();
            }

            return ld_value;
        }

        private string f_20110_SP(DateTime ldt_date, string as_txn_id) {
            string ls_prod_type = "M";
            int li_return;
            /*******************
            轉統計資料TDT
            *******************/
            if (dao20110.sp_U_gen_H_TDT(ldt_date, ls_prod_type).Status != ResultStatus.Success) {
                MessageBox.Show("執行SP(sp_U_gen_H_TDT(" + ls_prod_type + "))錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "E";
            }
            else {
                li_return = 0;
            }
            PbFunc.f_write_logf(_ProgramID, "E", "執行sp_U_gen_H_TDT(" + ls_prod_type + ")");

            if (as_txn_id == "20110") {
                ls_prod_type = "J";
                if (dao20110.sp_U_gen_H_TDT(ldt_date, ls_prod_type).Status != ResultStatus.Success) {
                    MessageBox.Show("執行SP(sp_U_gen_H_TDT(" + ls_prod_type + "))錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return "E";
                }
                else {
                    li_return = 0;
                }
                PbFunc.f_write_logf(_ProgramID, "E", "執行sp_U_gen_H_TDT(" + ls_prod_type + ")");

                //JTX 日統計AI2
                if (dao20110.sp_U_stt_H_AI2_Day(ldt_date, ls_prod_type).Status != ResultStatus.Success) {
                    MessageBox.Show("執行SP(sp_U_stt_H_AI2_Day)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return "E";
                }
                else {
                    li_return = 0;
                }
                PbFunc.f_write_logf(_ProgramID, "E", "執行sp_U_stt_H_AI2_Day");

                //JTX 月統計AI2
                if (dao20110.sp_U_stt_H_AI2_Month(ldt_date, ls_prod_type).Status != ResultStatus.Success) {
                    MessageBox.Show("執行SP(sp_U_stt_H_AI2_Month)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return "E";
                }
                else {
                    li_return = 0;
                }
                PbFunc.f_write_logf(_ProgramID, "E", "執行sp_U_stt_H_AI2_Month");
            }
            /*******************
            轉統計資料AI3
            *******************/
            if (dao20110.sp_H_stt_AI3(ldt_date).Status != ResultStatus.Success) {
                MessageBox.Show("執行SP(sp_H_stt_AI3)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "E";
            }
            else {
                li_return = 0;
            }
            PbFunc.f_write_logf(_ProgramID, "E", "執行sp_H_stt_AI3");
            /*******************
            更新AI6 (震幅波動度)
            *******************/
            if (dao20110.sp_H_gen_AI6(ldt_date).Status != ResultStatus.Success) {
                MessageBox.Show("執行SP(sp_H_gen_AI6)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "E";
            }
            else {
                li_return = 0;
            }
            PbFunc.f_write_logf(_ProgramID, "E", "執行sp_H_gen_AI6");
            /*******************
            更新AA3
            *******************/
            if (dao20110.sp_H_upd_AA3(ldt_date).Status != ResultStatus.Success) {
                MessageBox.Show("執行SP(sp_H_upd_AA3)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "E";
            }
            else {
                li_return = 0;
            }
            PbFunc.f_write_logf(_ProgramID, "E", "執行sp_H_upd_AA3");
            /*******************
            更新AI8
            *******************/
            if (dao20110.sp_H_gen_H_AI8(ldt_date).Status != ResultStatus.Success) {
                MessageBox.Show("執行SP(sp_H_gen_H_AI8)錯誤! ", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "E";
            }
            else {
                li_return = 0;
            }
            PbFunc.f_write_logf(_ProgramID, "E", "執行sp_H_gen_H_AI8");

            return "";
        }

        private enum ColName {
            AMIF_OPEN_PRICE = 3,
            AMIF_HIGH_PRICE,
            AMIF_LOW_PRICE,
            AMIF_CLOSE_PRICE,
            AMIF_UP_DOWN_VAL,
            AMIF_M_QNTY_TAL,
            AMIF_SUM_AMT
        }

        private void cb_jpx_Click(object sender, EventArgs e) {
            wf_jpx_get_file();
        }
    }
}