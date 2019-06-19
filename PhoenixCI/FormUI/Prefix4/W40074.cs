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
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;
using Common;
using BusinessObjects.Enums;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using DataObjects.Dao.Together;
using BusinessObjects;
using BaseGround.Report;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;

/// <summary>
/// Lukas, 2019/5/13
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    public partial class W40074 : FormParent {

        #region 全域變數
        /// <summary>
        /// 調整類型 0一般 1長假 2處置股票 3股票
        /// </summary>
        protected string is_adj_type { get; set; }
        /// <summary>
        /// 前一營業日
        /// </summary>
        protected string is_pre_ymd { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        protected string ymd { get; set; }
        private D40071 dao40071;
        private D40072 dao40072;
        private D40074 dao40074;
        private MGD2 daoMGD2;
        private MGD2L daoMGD2L;
        private MGRT1 daoMGRT1;
        private MOCF daoMOCF;
        private COD daoCOD;
        private OCFG daoOCFG;
        private RepositoryItemLookUpEdit statusLookUpEdit;
        private RepositoryItemLookUpEdit typeLookUpEdit;
        private RepositoryItemLookUpEdit currencyTypeLookUpEdit;//幣別
        /// <summary>
        /// 期貨級距
        /// </summary>
        private RepositoryItemLookUpEdit fLevelLookUpEdit;
        /// <summary>
        /// 選擇權級距
        /// </summary>
        private RepositoryItemLookUpEdit oLevelLookUpEdit;
        /// <summary>
        /// 盤別
        /// </summary>
        private RepositoryItemLookUpEdit oswGrpLookUpEdit;
        /// <summary>
        /// 商品
        /// </summary>
        private RepositoryItemLookUpEdit kindIDLookUpEdit;
        /// <summary>
        /// 商品類
        /// </summary>
        private RepositoryItemLookUpEdit prodTypeLookUpEdit;
        private DataTable dtFLevel;//期貨級距
        private DataTable dtOLevel;//選擇權級距
        private DataTable dtDel;//存放刪除的資料
        private DataTable dtProdType;//商品類
        #endregion

        public W40074(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao40071 = new D40071();
            dao40072 = new D40072();
            dao40074 = new D40074();
            daoMGD2 = new MGD2();
            daoMGD2L = new MGD2L();
            daoMGRT1 = new MGRT1();
            daoMOCF = new MOCF();
            daoCOD = new COD();
            daoOCFG = new OCFG();
            dtFLevel = daoMGRT1.dddw_mgrt1("F");//先讀，後面在不同的地方會用到
            dtOLevel = daoMGRT1.dddw_mgrt1("O");//先讀，後面在不同的地方會用到
            dtProdType = dao40071.d_40071_prod_type_ddl();//先讀，後面在不同的地方會用到
            dtDel = new DataTable();
            kindIDLookUpEdit = new RepositoryItemLookUpEdit();
            GridHelper.SetCommonGrid(gvMain);
            gvMain.AppearancePrint.BandPanel.Font = new Font("Microsoft YaHei", 10);
            gvMain.AppearancePrint.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        }

        protected override ResultStatus Open() {
            base.Open();
            //取得table的schema，因為程式開啟會預設插入一筆空資料列
            DataTable dtMGD2 = dao40074.d_40074();
            gcMain.DataSource = dtMGD2;

            #region 下拉選單設定
            //調整狀態下拉選單
            List<LookupItem> statusList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "Y", DisplayMember = "上市"},
                                        new LookupItem() { ValueMember = "D", DisplayMember = "下市"}};
            statusLookUpEdit = new RepositoryItemLookUpEdit();
            statusLookUpEdit.SetColumnLookUp(statusList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(statusLookUpEdit);
            ADJ_CODE.ColumnEdit = statusLookUpEdit;
            //保證金型態下拉選單
            List<LookupItem> typeList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "F", DisplayMember = "金額"},
                                        new LookupItem() { ValueMember = "P", DisplayMember = "百分比"}};
            typeLookUpEdit = new RepositoryItemLookUpEdit();
            typeLookUpEdit.SetColumnLookUp(typeList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(typeLookUpEdit);
            AMT_TYPE.ColumnEdit = typeLookUpEdit;

            //商品類下拉選單
            prodTypeLookUpEdit = new RepositoryItemLookUpEdit();
            prodTypeLookUpEdit.SetColumnLookUp(dtProdType, "PROD_SEQ_NO", "SUBTYPE_NAME", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(prodTypeLookUpEdit);
            PROD_SEQ_NO.ColumnEdit = prodTypeLookUpEdit;

            //幣別下拉選單
            DataTable dtCurrency = daoCOD.ListByCurrency();
            currencyTypeLookUpEdit = new RepositoryItemLookUpEdit();
            currencyTypeLookUpEdit.SetColumnLookUp(dtCurrency, "CURRENCY_TYPE", "CURRENCY_NAME", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(currencyTypeLookUpEdit);
            CURRENCY_TYPE.ColumnEdit = currencyTypeLookUpEdit;

            //級距下拉選單
            //期貨
            fLevelLookUpEdit = new RepositoryItemLookUpEdit();
            fLevelLookUpEdit.SetColumnLookUp(dtFLevel, "MGRT1_LEVEL", "MGRT1_LEVEL_NAME", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(fLevelLookUpEdit);
            M_LEVEL.ColumnEdit = fLevelLookUpEdit;//開啟時預設為期貨
            //選擇權
            oLevelLookUpEdit = new RepositoryItemLookUpEdit();
            oLevelLookUpEdit.SetColumnLookUp(dtOLevel, "MGRT1_LEVEL", "MGRT1_LEVEL_NAME", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(oLevelLookUpEdit);

            //盤別下拉選單
            DataTable dtOswGrp = daoOCFG.ListAll();
            oswGrpLookUpEdit = new RepositoryItemLookUpEdit();
            oswGrpLookUpEdit.SetColumnLookUp(dtOswGrp, "OSW_GRP", "OSW_GRP_NAME", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(oswGrpLookUpEdit);
            OSW_GRP.ColumnEdit = oswGrpLookUpEdit;
            #endregion

            //預設新增一筆設定資料
            InsertRow();

            //設定日期和全域變數
            txtSDate.DateTimeValue = DateTime.Now;
#if DEBUG
            txtSDate.EditValue = "2019/03/20";
#endif
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            is_adj_type = "4";

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
            try {
                //清空Grid
                gcMain.DataSource = null;

                //日期檢核
                if (txtSDate.Text == "1901/01/01") {
                    MessageDisplay.Error("請輸入交易日期");
                    return ResultStatus.Fail;
                }

                int ii_curr_row = 0, li_prod_seq = 0;
                string ls_kind_id = "", ls_ab_type, ls_stock_id, ls_prod_subtype;
                //讀取資料
                ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                DataTable dtMGD2 = dao40071.d_40071(ymd, is_adj_type);
                if (dtMGD2.Rows.Count == 0) {
                    MessageDisplay.Error("無任何資料！");
                    gcMain.DataSource = dao40074.d_40074();
                    //若無資料，預設新增一筆設定資料
                    InsertRow();
                    return ResultStatus.Fail;
                }
                //準備空的table給grid
                DataTable dtGrid = dao40074.d_40074();

                txtSDate.Text = dtMGD2.Rows[0]["MGD2_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                foreach (DataRow dr in dtMGD2.Rows) {
                    if (ls_kind_id != dr["MGD2_KIND_ID"].AsString()) {
                        ls_kind_id = dr["MGD2_KIND_ID"].AsString();
                        ls_prod_subtype = dr["MGD2_PROD_SUBTYPE"].AsString();
                        ii_curr_row = dtGrid.Rows.Count;
                        dtGrid.Rows.Add();
                        dtGrid.Rows[ii_curr_row]["PROD_TYPE"] = dr["MGD2_PROD_TYPE"];
                        dtGrid.Rows[ii_curr_row]["KIND_ID"] = ls_kind_id;
                        dtGrid.Rows[ii_curr_row]["STOCK_ID"] = dr["MGD2_STOCK_ID"];
                        dtGrid.Rows[ii_curr_row]["PROD_SUBTYPE"] = ls_prod_subtype;
                        dtGrid.Rows[ii_curr_row]["PARAM_KEY"] = dr["MGD2_PARAM_KEY"];

                        dtGrid.Rows[ii_curr_row]["M_LEVEL"] = dr["MGD2_LEVEL"]; ;
                        dtGrid.Rows[ii_curr_row]["CURRENCY_TYPE"] = dr["MGD2_CURRENCY_TYPE"];
                        dtGrid.Rows[ii_curr_row]["SEQ_NO"] = dr["MGD2_SEQ_NO"];
                        dtGrid.Rows[ii_curr_row]["OSW_GRP"] = dr["MGD2_OSW_GRP"].AsString();
                        dtGrid.Rows[ii_curr_row]["AMT_TYPE"] = dr["MGD2_AMT_TYPE"];

                        dtGrid.Rows[ii_curr_row]["ADJ_CODE"] = dr["MGD2_ADJ_CODE"];
                        dtGrid.Rows[ii_curr_row]["PUB_YMD"] = dr["MGD2_PUB_YMD"];
                        dtGrid.Rows[ii_curr_row]["OP_TYPE"] = " "; //預設為空格

                        li_prod_seq = dao40074.getProd(ls_kind_id, ls_prod_subtype);
                    }
                    dtGrid.Rows[ii_curr_row]["PROD_SEQ_NO"] = li_prod_seq;
                    if (dr["MGD2_AB_TYPE"].AsString() == "B") {
                        dtGrid.Rows[ii_curr_row]["CM_B"] = dr["MGD2_CM"];
                        dtGrid.Rows[ii_curr_row]["MM_B"] = dr["MGD2_MM"];
                        dtGrid.Rows[ii_curr_row]["IM_B"] = dr["MGD2_IM"];
                    }
                    else {
                        dtGrid.Rows[ii_curr_row]["CM_A"] = dr["MGD2_CM"];
                        dtGrid.Rows[ii_curr_row]["MM_A"] = dr["MGD2_MM"];
                        dtGrid.Rows[ii_curr_row]["IM_A"] = dr["MGD2_IM"];
                    }
                }//foreach (DataRow dr in dtMGD2.Rows)

                //排序
                if (dtGrid.Rows.Count != 0) {
                    dtGrid = dtGrid.AsEnumerable().OrderBy(x => x.Field<Int32>("SEQ_NO"))
                            .ThenBy(x => x.Field<string>("KIND_ID"))
                            .CopyToDataTable();
                }

                //資料繫結
                gcMain.DataSource = dtGrid;

                //若無資料，預設新增一筆設定資料
                if (gvMain.RowCount == 0) {
                    InsertRow();
                }
            }
            catch (Exception ex) {
                MessageDisplay.Error("讀取錯誤");
                throw ex;
            }

            return ResultStatus.Success;

        }

        protected override ResultStatus InsertRow() {
            base.InsertRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            gvMain.UpdateCurrentRow();
            gvMain.CloseEditor();

            if (!ConfirmToDelete(gvMain.FocusedRowHandle + 1)) { return ResultStatus.Fail; }
            DataTable dtBackUp = (DataTable)gcMain.DataSource;
            dtDel = dtBackUp.Clone();
            dtDel.ImportRow(dtBackUp.Rows[gvMain.FocusedRowHandle]);
            gvMain.DeleteSelectedRows();

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            try {
                if (gvMain.RowCount == 0) {
                    MessageDisplay.Info("沒有變更資料,不需要存檔!");
                    return ResultStatus.Fail;
                }
                #region ue_save_before
                gvMain.CloseEditor();
                gvMain.UpdateCurrentRow();

                string ls_stock_id, ls_ymd, ls_kind_id, ls_adj_type_name, ls_op_type, ls_dbname, ls_stock_id_ck;
                string ls_issue_begin_ymd, ls_trade_ymd, ls_mocf_ymd, ls_next_ymd, ls_level, ls_currency_type;
                int ii_curr_row, ll_found, li_count, li_row, li_col, li_prod_seq;
                decimal ldbl_rate;
                DateTime ldt_w_TIME = DateTime.Now;

                DataTable dtGrid = (DataTable)gcMain.DataSource;
                ll_found = dtGrid.Rows.IndexOf(dtGrid.Select("OP_TYPE <> ' '").FirstOrDefault());
                if (ll_found + dtDel.Rows.Count == -1) {
                    MessageDisplay.Warning("沒有變更資料,不需要存檔!");
                    return ResultStatus.Fail;
                }

                DataTable dtMGD2; //ids_mgd2
                DataTable dtMGD2Log = dao40071.d_40071_log(); //ids_old 
                dtMGD2Log.Clear(); //只取schema

                ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                ls_issue_begin_ymd = ymd;
                dtMGD2 = dao40071.d_40071(ymd, is_adj_type);

                foreach (DataRow dr in dtGrid.Rows) {
                    ls_op_type = dr["OP_TYPE"].ToString();
                    ls_stock_id = dr["STOCK_ID"].AsString();
                    ls_kind_id = dr["KIND_ID"].AsString();
                    ls_level = dr["M_LEVEL"].AsString();

                    //檢查調整後級距為從其高且商品類別為選擇權時，是否有輸入保證金B值
                    if (ls_level == "Z" && dr["PROD_TYPE"].AsString() == "O") {
                        if (dr["CM_B"] == DBNull.Value || dr["MM_B"] == DBNull.Value || dr["IM_B"] == DBNull.Value) {
                            MessageDisplay.Error(ls_stock_id + "," + ls_kind_id + "的保證金B值未輸入完成");
                            return ResultStatus.FailButNext;
                        }
                    }

                    //檢查有異動的資料
                    if (ls_op_type != " ") {
                        //資料修改，將修改前舊資料寫入log
                        if (ls_op_type == "U") {
                            dtMGD2.Filter("mgd2_kind_id = '" + ls_kind_id + "'");
                            foreach (DataRow drU in dtMGD2.Rows) {
                                ii_curr_row = dtMGD2Log.Rows.Count;
                                dtMGD2Log.Rows.Add();
                                for (li_col = 0; li_col < dtMGD2.Columns.Count; li_col++) {
                                    //先取欄位名稱，因為兩張table欄位順序不一致
                                    ls_dbname = dtMGD2.Columns[li_col].ColumnName;
                                    if (ls_dbname == "CPSORT") continue; //這個欄位是拿來排序用的，故無需複製
                                    dtMGD2Log.Rows[ii_curr_row][ls_dbname] = drU[li_col];
                                }
                                dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TYPE"] = "U";
                                dtMGD2Log.Rows[ii_curr_row]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                                dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TIME"] = ldt_w_TIME;
                            }
                        }
                        /*******************************************
                           檢查商品與類別是否符合
                        *******************************************/
                        li_prod_seq = dao40074.getProd(ls_kind_id, dr["PROD_SUBTYPE"].AsString());
                        if (li_prod_seq != dr["PROD_SEQ_NO"].AsInt()) {
                            MessageDisplay.Error(ls_kind_id + "與商品類別不符，請確認");
                            return ResultStatus.FailButNext;
                        }
                        /*****************************************
                           檢查商品代號是否存在及相關資料是否正確
                        ******************************************/
                        DataTable dtCheck = dao40074.checkProd(ls_kind_id);
                        if (dtCheck.Rows.Count == 0) {
                            MessageDisplay.Error(ls_kind_id + "不存在，請重新設定商品代號");
                            return ResultStatus.FailButNext;
                        }
                        li_count = dtCheck.Rows[0]["LI_COUNT"].AsInt();
                        ls_currency_type = dtCheck.Rows[0]["LS_CURRENCY_TYPE"].AsString();
                        ls_stock_id_ck = dtCheck.Rows[0]["LS_STOCK_ID_CK"].AsString();

                        if (li_count == 0) {
                            MessageDisplay.Error(ls_kind_id + "不存在，請重新設定商品代號");
                            return ResultStatus.FailButNext;
                        }
                        else {
                            if (dr["CURRENCY_TYPE"].AsString() != ls_currency_type) {
                                MessageDisplay.Error(ls_kind_id + "的幣別設定錯誤，請重新設定填寫");
                                return ResultStatus.FailButNext;
                            }
                            if (dr["prod_subtype"].AsString() == "S" && ls_stock_id != ls_stock_id_ck) {
                                MessageDisplay.Error(ls_kind_id + "的股票代號設定錯誤，請重新設定填寫");
                                return ResultStatus.FailButNext;
                            }
                        }

                        /******************************************
                           確認商品是否在同一交易日不同情境下設定過
                        ******************************************/
                        DataTable dtSet = dao40071.IsSetOnSameDay(ls_kind_id, ymd, is_adj_type);
                        if (dtSet.Rows.Count == 0) {
                            MessageDisplay.Error("MGD2 " + ls_kind_id + " 無任何資料！");
                            return ResultStatus.FailButNext;
                        }
                        li_count = dtSet.Rows[0]["LI_COUNT"].AsInt();
                        ls_adj_type_name = dtSet.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        if (li_count > 0) {
                            MessageDisplay.Error(ls_kind_id + ",交易日(" + ymd + ")在" + ls_adj_type_name + "已有資料");
                            return ResultStatus.FailButNext;
                        }
                        /*********************************
                        確認商品是否在同一生效日區間設定過
                        生效起日若與生效迄日相同，不重疊
                        ex: 10/11的至10/31一般交易時段結束止，10/30的從10/31一般交易時段結束後始>>應不重疊
                        *************************************/
                        dtSet = dao40071.IsSetInSameSession(ls_kind_id, ymd, ls_issue_begin_ymd);
                        li_count = dtSet.Rows[0]["LI_COUNT"].AsInt();
                        ls_adj_type_name = dtSet.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        ls_trade_ymd = dtSet.Rows[0]["LS_TRADE_YMD"].AsString();
                        if (li_count > 0) {
                            MessageDisplay.Error(ls_kind_id + "," + ls_adj_type_name + ",交易日(" + ls_trade_ymd + ")在同一生效日區間內已有資料");
                            return ResultStatus.FailButNext;
                        }

                    }//if (ls_op_type != " ")
                }//foreach (DataRow dr in dtGrid.Rows)

                //把刪除的資料寫進log
                foreach (DataRow drDel in dtDel.Rows) {
                    ls_kind_id = drDel["KIND_ID"].AsString();
                    dtMGD2.Filter("mgd2_kind_id = '" + ls_kind_id + "'");
                    foreach (DataRow drD in dtMGD2.Rows) {
                        ii_curr_row = dtMGD2Log.Rows.Count;
                        dtMGD2Log.Rows.Add();
                        for (li_col = 0; li_col < dtMGD2.Columns.Count; li_col++) {
                            //先取欄位名稱，因為兩張table欄位順序不一致
                            ls_dbname = dtMGD2.Columns[li_col].ColumnName;
                            if (ls_dbname == "CPSORT") continue; //這個欄位是拿來排序用的，故無需複製
                            dtMGD2Log.Rows[ii_curr_row][ls_dbname] = drD[li_col];
                        }
                        dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TYPE"] = "D";
                        dtMGD2Log.Rows[ii_curr_row]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                        dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TIME"] = ldt_w_TIME;
                    }
                }
                #endregion

                string ls_prod_type;
                DataTable dtTemp = dao40072.d_40072(); //ids_tmp

                foreach (DataRow dr in dtGrid.Rows) {
                    ls_op_type = dr["OP_TYPE"].ToString();
                    //只更新有異動的資料
                    if (ls_op_type != " ") {
                        ls_kind_id = dr["KIND_ID"].AsString();
                        ls_stock_id = dr["KIND_ID"].AsString();

                        //刪除已存在資料
                        if (daoMGD2.DeleteMGD2(ymd, is_adj_type, ls_stock_id, ls_kind_id) < 0) {
                            MessageDisplay.Error("MGD2資料刪除失敗");
                            return ResultStatus.FailButNext;
                        }

                        ii_curr_row = dtTemp.Rows.Count;
                        ls_prod_type = dr["PROD_TYPE"].AsString();
                        dtTemp.Rows.Add();
                        dtTemp.Rows[ii_curr_row]["MGD2_YMD"] = ymd;
                        dtTemp.Rows[ii_curr_row]["MGD2_PROD_TYPE"] = ls_prod_type;
                        dtTemp.Rows[ii_curr_row]["MGD2_KIND_ID"] = ls_kind_id;
                        dtTemp.Rows[ii_curr_row]["MGD2_STOCK_ID"] = ls_stock_id;
                        dtTemp.Rows[ii_curr_row]["MGD2_ADJ_TYPE"] = is_adj_type;

                        dtTemp.Rows[ii_curr_row]["MGD2_ADJ_CODE"] = dr["ADJ_CODE"];
                        dtTemp.Rows[ii_curr_row]["MGD2_PUB_YMD"] = dr["PUB_YMD"];
                        dtTemp.Rows[ii_curr_row]["MGD2_ISSUE_BEGIN_YMD"] = ls_issue_begin_ymd;
                        dtTemp.Rows[ii_curr_row]["MGD2_PROD_SUBTYPE"] = dr["PROD_SUBTYPE"].AsString();
                        dtTemp.Rows[ii_curr_row]["MGD2_PARAM_KEY"] = dr["PARAM_KEY"];

                        dtTemp.Rows[ii_curr_row]["MGD2_CUR_CM"] = 0;
                        dtTemp.Rows[ii_curr_row]["MGD2_CUR_MM"] = 0;
                        dtTemp.Rows[ii_curr_row]["MGD2_CUR_IM"] = 0;
                        dtTemp.Rows[ii_curr_row]["MGD2_CUR_LEVEL"] = 0;
                        dtTemp.Rows[ii_curr_row]["MGD2_CM"] = dr["CM_A"];

                        dtTemp.Rows[ii_curr_row]["MGD2_MM"] = dr["MM_A"];
                        dtTemp.Rows[ii_curr_row]["MGD2_IM"] = dr["IM_A"];
                        dtTemp.Rows[ii_curr_row]["MGD2_LEVEL"] = dr["M_LEVEL"];
                        dtTemp.Rows[ii_curr_row]["MGD2_CURRENCY_TYPE"] = dr["CURRENCY_TYPE"];
                        dtTemp.Rows[ii_curr_row]["MGD2_SEQ_NO"] = dr["SEQ_NO"];

                        dtTemp.Rows[ii_curr_row]["MGD2_OSW_GRP"] = dr["OSW_GRP"];
                        dtTemp.Rows[ii_curr_row]["MGD2_AMT_TYPE"] = dr["AMT_TYPE"];
                        dtTemp.Rows[ii_curr_row]["MGD2_W_TIME"] = ldt_w_TIME;
                        dtTemp.Rows[ii_curr_row]["MGD2_W_USER_ID"] = GlobalInfo.USER_ID;

                        /******************************
                              AB TYTPE：	-期貨
                                          A選擇權A值
                                          B選擇權B值
                        *******************************/
                        if (ls_prod_type == "F") {
                            dtTemp.Rows[ii_curr_row]["MGD2_AB_TYPE"] = "-";
                        }
                        else {
                            dtTemp.Rows[ii_curr_row]["MGD2_AB_TYPE"] = "A";
                            //複製一筆一樣的，AB Type分開存
                            dtTemp.ImportRow(dtTemp.Rows[ii_curr_row]);
                            //dtTemp.Rows.Add(dtTemp.Rows[ii_curr_row]);//會跳錯
                            ii_curr_row = dtTemp.Rows.Count - 1;
                            dtTemp.Rows[ii_curr_row]["MGD2_AB_TYPE"] = "B";
                            dtTemp.Rows[ii_curr_row]["MGD2_CM"] = dr["CM_B"];
                            dtTemp.Rows[ii_curr_row]["MGD2_MM"] = dr["MM_B"];
                            dtTemp.Rows[ii_curr_row]["MGD2_IM"] = dr["IM_B"];
                        }
                    }//if (ls_op_type != " ")
                }//foreach (DataRow dr in dtGrid.Rows)

                //刪除資料
                foreach (DataRow drDel in dtDel.Rows) {
                    ls_kind_id = drDel["KIND_ID"].AsString();
                    ls_stock_id = drDel["STOCK_ID"].AsString();
                    if (daoMGD2.DeleteMGD2(ymd, is_adj_type, ls_stock_id, ls_kind_id) < 0) {
                        MessageDisplay.Error("MGD2資料刪除失敗");
                        return ResultStatus.FailButNext;
                    }
                }

                //Update DB
                //ids_tmp.update()
                ResultData myResultData = daoMGD2.UpdateMGD2(dtTemp);
                if (myResultData.Status == ResultStatus.Fail) {
                    MessageDisplay.Error("更新資料庫MGD2錯誤! ");
                    return ResultStatus.FailButNext;
                }

                //ids_old.update()
                if (dtMGD2Log.Rows.Count > 0) {
                    myResultData = daoMGD2L.UpdateMGD2L(dtMGD2Log);
                    if (myResultData.Status == ResultStatus.Fail) {
                        MessageDisplay.Error("更新資料庫MGD2L錯誤! ");
                        return ResultStatus.FailButNext;
                    }
                }
                //Write LOGF
                WriteLog("變更資料 ", "Info", "I");
                //報表儲存pdf
                ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                CommonReportLandscapeA3 reportLandscape = new CommonReportLandscapeA3();//設定為橫向列印
                reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
                reportLandscape.IsHandlePersonVisible = false;
                reportLandscape.IsManagerVisible = false;
                _ReportHelper.Create(reportLandscape);
                _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);
                MessageDisplay.Info("報表儲存完成!");
            }
            catch (Exception ex) {
                MessageDisplay.Error("儲存錯誤");
                throw ex;
            }
            return ResultStatus.Success;

        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            try {
                ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                CommonReportLandscapeA3 reportLandscape = new CommonReportLandscapeA3();//設定為橫向列印
                reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
                _ReportHelper.LeftMemo = "生效日期：" + txtSDate.Text;
                reportLandscape.IsHandlePersonVisible = false;
                reportLandscape.IsManagerVisible = false;
                _ReportHelper.Create(reportLandscape);

                _ReportHelper.Print();//如果有夜盤會特別標註
                _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);
                MessageDisplay.Info("報表儲存完成!");
                return ResultStatus.Success;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }

        #region GridView Events
        private void gvMain_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
            GridView gv = sender as GridView;
            gv.CloseEditor();
            gv.UpdateCurrentRow();
            string ls_prod_type, ls_prod_subtype, ls_param_key, ls_abroad;
            DataTable dtKind = new DataTable();
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            if (e.Column.FieldName == "KIND_ID") {
                ls_prod_subtype = gv.GetRowCellValue(e.RowHandle, "PROD_SUBTYPE").AsString();
                ls_param_key = gv.GetRowCellValue(e.RowHandle, "CND_PARAM_KEY").AsString();
                ls_abroad = gv.GetRowCellValue(e.RowHandle, "ABROAD") == null ? null :
                            gv.GetRowCellValue(e.RowHandle, "ABROAD").ToString();
                if (ls_prod_subtype == "S") {
                    if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "D") {
                        dtKind = dao40071.dddw_pdk_kind_id_40071(ymd, ls_param_key);
                        kindIDLookUpEdit.SetColumnLookUp(dtKind, "KIND_ID", "KIND_ID", TextEditStyles.Standard, null);
                        gcMain.RepositoryItems.Add(kindIDLookUpEdit);
                        e.RepositoryItem = kindIDLookUpEdit;
                    }
                    if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "Y") {
                        dtKind = dao40074.dddw_pdk_kind_id_40074(ls_param_key);
                        kindIDLookUpEdit.SetColumnLookUp(dtKind, "KIND_ID", "KIND_ID", TextEditStyles.Standard, null);
                        gcMain.RepositoryItems.Add(kindIDLookUpEdit);
                        e.RepositoryItem = kindIDLookUpEdit;
                    }
                }
                else {
                    dtKind = dao40071.dddw_mgt2_kind(ls_prod_subtype + "%", ls_abroad);
                    kindIDLookUpEdit.SetColumnLookUp(dtKind, "KIND_ID", "KIND_ID", TextEditStyles.Standard, null);
                    gcMain.RepositoryItems.Add(kindIDLookUpEdit);
                    e.RepositoryItem = kindIDLookUpEdit;
                }
                if (dtKind.Rows.Count > 0) {
                    DataRow drKind = dtKind.Select("KIND_ID = '" + e.CellValue.ToString() + "'").FirstOrDefault();
                    if (drKind != null) gv.SetRowCellValue(e.RowHandle, "PROD_TYPE", drKind["PROD_TYPE"]);
                }
            }
            if (e.Column.FieldName == "M_LEVEL") {
                ls_prod_type = gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString();
                if (ls_prod_type == "F") e.RepositoryItem = fLevelLookUpEdit;
                if (ls_prod_type == "O") e.RepositoryItem = oLevelLookUpEdit;
            }
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            GridView gv = sender as GridView;
            string amt_type = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMT_TYPE"]).AsString();
            string op_type = gv.GetRowCellValue(e.RowHandle, gv.Columns["OP_TYPE"]) == null ? "I" :
                             gv.GetRowCellValue(e.RowHandle, gv.Columns["OP_TYPE"]).ToString();
            switch (e.Column.FieldName) {
                case "KIND_ID":
                case "STOCK_ID":
                case "M_LEVEL":
                case "PROD_SEQ_NO":
                    e.Appearance.BackColor = op_type == "I" ? Color.White : Color.FromArgb(224, 224, 224);
                    e.Column.OptionsColumn.AllowEdit = op_type == "I" ? true : false;
                    break;
                case "CM_A":
                case "CM_B":
                case "MM_A":
                case "MM_B":
                case "IM_A":
                case "IM_B":
                    e.Column.DisplayFormat.FormatString = amt_type == "P" ? "{0:0.####%}" : "#,###";
                    break;
            }
        }

        /// <summary>
        /// 期貨的保證金B值不能key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string prod_type = gv.GetRowCellValue(gv.FocusedRowHandle, "PROD_TYPE").ToString();
            string prod_subtype = gv.GetRowCellValue(gv.FocusedRowHandle, "PROD_SUBTYPE").ToString();
            string cnd_param_key = gv.GetRowCellValue(gv.FocusedRowHandle, "CND_PARAM_KEY").ToString();
            if (gv.FocusedColumn.Name == "CM_B" ||
                gv.FocusedColumn.Name == "MM_B" ||
                gv.FocusedColumn.Name == "IM_B") {
                e.Cancel = prod_type == "F" ? true : false;
            }
            if (gv.FocusedColumn.Name == "STOCK_ID" ||
                gv.FocusedColumn.Name == "M_LEVEL") {
                e.Cancel = prod_subtype != "S" ? true : false;
                if (cnd_param_key.IndexOf("ST%") >= 0) e.Cancel = false;
            }
        }

        private void gvMain_CellValueChanging(object sender, CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            string ls_prod_subtype, ls_prod_type, ls_param_key, ls_abroad, ls_kind_id;
            int ll_found = -1;
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            ls_param_key = gv.GetRowCellValue(e.RowHandle, "CND_PARAM_KEY").AsString();
            if (e.Column.Name != "OP_TYPE") {
                //如果OP_TYPE是I則固定不變
                if (gv.GetRowCellValue(e.RowHandle, "OP_TYPE").ToString() == " ") gv.SetRowCellValue(e.RowHandle, "OP_TYPE", "U");
            }
            if (e.Column.Name == "M_LEVEL") {
                //如果改變級距
                string level = e.Value.AsString();
                if (gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString() == "F") {
                    DataRow dr = dtFLevel.Select("mgrt1_level = '" + level + "'")[0];
                    gv.SetRowCellValue(e.RowHandle, "CM_A", dr["MGRT1_CM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "MM_A", dr["MGRT1_MM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "IM_A", dr["MGRT1_IM_RATE"]);
                }
                if (gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString() == "O") {
                    DataRow dr = dtOLevel.Select("mgrt1_level = '" + level + "'")[0];
                    gv.SetRowCellValue(e.RowHandle, "CM_A", dr["MGRT1_CM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "MM_A", dr["MGRT1_MM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "IM_A", dr["MGRT1_IM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "CM_B", dr["MGRT1_CM_RATE_B"]);
                    gv.SetRowCellValue(e.RowHandle, "MM_B", dr["MGRT1_MM_RATE_B"]);
                    gv.SetRowCellValue(e.RowHandle, "IM_B", dr["MGRT1_IM_RATE_B"]);
                }
            }
            if (e.Column.Name == "PROD_SEQ_NO") {
                //如果改變商品類
                DataRow dr = dtProdType.Select("prod_seq_no = '" + e.Value.AsString() + "'")[0];
                gv.SetRowCellValue(e.RowHandle, "KIND_ID", "");
                gv.SetRowCellValue(e.RowHandle, "STOCK_ID", " ");
                gv.SetRowCellValue(e.RowHandle, "M_LEVEL", "");
                gv.SetRowCellValue(e.RowHandle, "PROD_SUBTYPE", dr["CND_PROD_SUBTYPE"]);
                gv.SetRowCellValue(e.RowHandle, "CND_PARAM_KEY", dr["CND_PARAM_KEY"]);
                gv.SetRowCellValue(e.RowHandle, "ABROAD", dr["CND_ABROAD"]);
            }
            if (e.Column.Name == "KIND_ID") {
                //商品那欄除了下拉選單已外也可手動key入，key入後會檢查是否正確
                //若kind_id值為空(即預設值)，則視為使用者尚未填寫，不在此進行檢核，否則會進入無限迴圈
                //若使用者未輸入kind_id逕行存檔，存檔時仍會再判斷一次
                ls_prod_subtype = gv.GetRowCellValue(e.RowHandle, "PROD_SUBTYPE").AsString();
                ls_abroad = gv.GetRowCellValue(e.RowHandle, "ABROAD").ToString();
                ls_kind_id = gv.GetRowCellValue(e.RowHandle, "KIND_ID").AsString();
                if (ls_kind_id != "") {
                    DataTable dtKindCheck = new DataTable();
                    if (ls_prod_subtype == "S") {
                        if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "D") {
                            dtKindCheck = dao40071.dddw_pdk_kind_id_40071(ymd, ls_param_key);
                            ll_found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + ls_kind_id + "'").FirstOrDefault());

                        }
                        if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "Y") {
                            dtKindCheck = dao40074.dddw_pdk_kind_id_40074(ls_param_key);
                            ll_found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + ls_kind_id + "'").FirstOrDefault());
                        }
                    }
                    else {
                        dtKindCheck = dao40071.dddw_mgt2_kind(ls_prod_subtype + "%", ls_abroad);
                        ll_found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + ls_kind_id + "'").FirstOrDefault());
                    }
                    if (ll_found == -1) {
                        MessageDisplay.Error("商品代號輸入錯誤");
                        gv.SetRowCellValue(e.RowHandle, "KIND_ID", "");
                    }
                    else {
                        gv.SetRowCellValue(e.RowHandle, "PROD_TYPE", dtKindCheck.Rows[ll_found]["PROD_TYPE"]);
                        gv.SetRowCellValue(e.RowHandle, "PARAM_KEY", dtKindCheck.Rows[ll_found]["PARAM_KEY"].AsString());
                        gv.SetRowCellValue(e.RowHandle, "SEQ_NO", dtKindCheck.Rows[ll_found]["SEQ_NO"]);
                    }
                }
            }
            if (e.Column.Name == "ADJ_CODE") {
                //改變調整狀態(上市/下市)時，//同步公布日期
                if (e.Value.AsString() == "Y") gv.SetRowCellValue(e.RowHandle, "PUB_YMD", is_pre_ymd);
                if (e.Value.AsString() == "D") {
                    gv.SetRowCellValue(e.RowHandle, "PUB_YMD", ymd);
                    gv.SetRowCellValue(e.RowHandle, "CM_A", 0);
                    gv.SetRowCellValue(e.RowHandle, "MM_A", 0);
                    gv.SetRowCellValue(e.RowHandle, "IM_A", 0);
                    gv.SetRowCellValue(e.RowHandle, "CM_B", 0);
                    gv.SetRowCellValue(e.RowHandle, "MM_B", 0);
                    gv.SetRowCellValue(e.RowHandle, "IM_B", 0);
                }
            }
        }

        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["OP_TYPE"], "I");
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["CND_PARAM_KEY"], "%");
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["ABROAD"], "          ");
        }

        private void gvMain_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            //GridView gv = sender as GridView;
            //string ls_prod_subtype, ls_prod_type, ls_param_key, ls_abroad, ls_kind_id;
            //int ll_found = -1;
            //ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            //ls_param_key = gv.GetRowCellValue(e.RowHandle, "CND_PARAM_KEY").AsString();
            //if (e.Column.Name != "OP_TYPE") {
            //    //如果OP_TYPE是I則固定不變
            //    if (gv.GetRowCellValue(e.RowHandle, "OP_TYPE").ToString() == " ") gv.SetRowCellValue(e.RowHandle, "OP_TYPE", "U");
            //}
            //if (e.Column.Name == "M_LEVEL") {
            //    //如果改變級距
            //    string level = e.Value.AsString();
            //    if (gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString() == "F") {
            //        DataRow dr = dtFLevel.Select("mgrt1_level = '" + level + "'")[0];
            //        gv.SetRowCellValue(e.RowHandle, "CM_A", dr["MGRT1_CM_RATE"]);
            //        gv.SetRowCellValue(e.RowHandle, "MM_A", dr["MGRT1_MM_RATE"]);
            //        gv.SetRowCellValue(e.RowHandle, "IM_A", dr["MGRT1_IM_RATE"]);
            //    }
            //    if (gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString() == "O") {
            //        DataRow dr = dtOLevel.Select("mgrt1_level = '" + level + "'")[0];
            //        gv.SetRowCellValue(e.RowHandle, "CM_A", dr["MGRT1_CM_RATE"]);
            //        gv.SetRowCellValue(e.RowHandle, "MM_A", dr["MGRT1_MM_RATE"]);
            //        gv.SetRowCellValue(e.RowHandle, "IM_A", dr["MGRT1_IM_RATE"]);
            //        gv.SetRowCellValue(e.RowHandle, "CM_B", dr["MGRT1_CM_RATE_B"]);
            //        gv.SetRowCellValue(e.RowHandle, "MM_B", dr["MGRT1_MM_RATE_B"]);
            //        gv.SetRowCellValue(e.RowHandle, "IM_B", dr["MGRT1_IM_RATE_B"]);
            //    }
            //}
            //if (e.Column.Name == "PROD_SEQ_NO") {
            //    //如果改變商品類
            //    DataRow dr = dtProdType.Select("prod_seq_no = '" + e.Value.AsString() + "'")[0];
            //    gv.SetRowCellValue(e.RowHandle, "KIND_ID", "");
            //    gv.SetRowCellValue(e.RowHandle, "STOCK_ID", " ");
            //    gv.SetRowCellValue(e.RowHandle, "M_LEVEL", "");
            //    gv.SetRowCellValue(e.RowHandle, "PROD_SUBTYPE", dr["CND_PROD_SUBTYPE"]);
            //    gv.SetRowCellValue(e.RowHandle, "CND_PARAM_KEY", dr["CND_PARAM_KEY"]);
            //    gv.SetRowCellValue(e.RowHandle, "ABROAD", dr["CND_ABROAD"]);
            //}
            //if (e.Column.Name == "KIND_ID") {
            //    //商品那欄除了下拉選單已外也可手動key入，key入後會檢查是否正確
            //    //若kind_id值為空(即預設值)，則視為使用者尚未填寫，不在此進行檢核，否則會進入無限迴圈
            //    //若使用者未輸入kind_id逕行存檔，存檔時仍會再判斷一次
            //    ls_prod_subtype = gv.GetRowCellValue(e.RowHandle, "PROD_SUBTYPE").AsString();
            //    ls_abroad = gv.GetRowCellValue(e.RowHandle, "ABROAD").ToString();
            //    ls_kind_id = gv.GetRowCellValue(e.RowHandle, "KIND_ID").AsString();
            //    if (ls_kind_id != "") {
            //        DataTable dtKindCheck = new DataTable();
            //        if (ls_prod_subtype == "S") {
            //            if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "D") {
            //                dtKindCheck = dao40071.dddw_pdk_kind_id_40071(ymd, ls_param_key);
            //                ll_found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + ls_kind_id + "'").FirstOrDefault());

            //            }
            //            if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "Y") {
            //                dtKindCheck = dao40074.dddw_pdk_kind_id_40074(ls_param_key);
            //                ll_found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + ls_kind_id + "'").FirstOrDefault());
            //            }
            //        }
            //        else {
            //            dtKindCheck = dao40071.dddw_mgt2_kind(ls_prod_subtype + "%", ls_abroad);
            //            ll_found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + ls_kind_id + "'").FirstOrDefault());
            //        }
            //        if (ll_found == -1) {
            //            MessageDisplay.Error("商品代號輸入錯誤");
            //            gv.SetRowCellValue(e.RowHandle, "KIND_ID", "");
            //        }
            //        else {
            //            gv.SetRowCellValue(e.RowHandle, "PROD_TYPE", dtKindCheck.Rows[ll_found]["PROD_TYPE"]);
            //            gv.SetRowCellValue(e.RowHandle, "PARAM_KEY", dtKindCheck.Rows[ll_found]["PARAM_KEY"].AsString());
            //            gv.SetRowCellValue(e.RowHandle, "SEQ_NO", dtKindCheck.Rows[ll_found]["SEQ_NO"]);
            //        }
            //    }
            //}
            //if (e.Column.Name == "ADJ_CODE") {
            //    //改變調整狀態(上市/下市)時，//同步公布日期
            //    if (e.Value.AsString() == "Y") gv.SetRowCellValue(e.RowHandle, "PUB_YMD", is_pre_ymd);
            //    if (e.Value.AsString() == "D") {
            //        gv.SetRowCellValue(e.RowHandle, "PUB_YMD", ymd);
            //        gv.SetRowCellValue(e.RowHandle, "CM_A", 0);
            //        gv.SetRowCellValue(e.RowHandle, "MM_A", 0);
            //        gv.SetRowCellValue(e.RowHandle, "IM_A", 0);
            //        gv.SetRowCellValue(e.RowHandle, "CM_B", 0);
            //        gv.SetRowCellValue(e.RowHandle, "MM_B", 0);
            //        gv.SetRowCellValue(e.RowHandle, "IM_B", 0);
            //    }
            //}
        }
        #endregion

        /// <summary>
        /// 改變生效日期時，同步公布日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSDate_EditValueChanged(object sender, EventArgs e) {
            if (txtSDate.Text.Length < 10) return;//防止還沒輸入完就觸發事件
            string ls_mocf_ymd;
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");

            //交易日+1個月
            ls_mocf_ymd = PbFunc.relativedate(txtSDate.DateTimeValue, -30).ToString("yyyyMMdd");
            /*前一營業日*/
            is_pre_ymd = daoMOCF.GetPrevTradeDay(ymd, ls_mocf_ymd);
            //同步公布日期	 

            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();
            DataTable dtGrid = (DataTable)gcMain.DataSource;
            foreach (DataRow dr in dtGrid.Rows) {
                if (dr["ADJ_CODE"].AsString() == "Y") dr["PUB_YMD"] = is_pre_ymd;
                if (dr["ADJ_CODE"].AsString() == "D") dr["PUB_YMD"] = ymd;
            }
        }
    }
}