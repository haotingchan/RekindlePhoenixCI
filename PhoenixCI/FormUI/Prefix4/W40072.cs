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
using Common;
using BusinessObjects.Enums;
using BaseGround.Shared;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using BusinessObjects;
using BaseGround.Report;

/// <summary>
/// Lukas, 2019/5/8
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

    public partial class W40072 : FormParent {

        /// <summary>
        /// 調整類型 0一般 1長假 2處置股票 3股票
        /// </summary>
        protected string is_adj_type { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        protected string ymd { get; set; }
        private D40071 dao40071;
        private D40072 dao40072;
        private MGD2 daoMGD2;
        private MGD2L daoMGD2L;
        private MGRT1 daoMGRT1;
        private MOCF daoMOCF;
        private RepositoryItemLookUpEdit rateLookUpEdit;
        private RepositoryItemLookUpEdit prodTypeLookUpEdit1;//期貨
        private RepositoryItemLookUpEdit prodTypeLookUpEdit2;//選擇權
        private DataTable dtFLevel;//期貨級距
        private DataTable dtOLevel;//選擇權級距

        public W40072(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao40071 = new D40071();
            dao40072 = new D40072();
            daoMGD2 = new MGD2();
            daoMGD2L = new MGD2L();
            daoMGRT1 = new MGRT1();
            daoMOCF = new MOCF();
            dtFLevel = new DataTable();
            dtFLevel = daoMGRT1.dddw_mgrt1("F");//先讀，後面在不同的地方會用到
            dtOLevel = new DataTable();
            dtOLevel = daoMGRT1.dddw_mgrt1("O");//先讀，後面在不同的地方會用到
            GridHelper.SetCommonGrid(gvMain);
            GridHelper.SetCommonGrid(gvDetail);
            gvDetail.AppearancePrint.BandPanel.Font = new Font("Microsoft YaHei", 10);
            gvDetail.AppearancePrint.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        }

        protected override ResultStatus Open() {
            base.Open();
            //設定日期和全域變數
            txtSDate.DateTimeValue = DateTime.Now;
#if DEBUG
            txtSDate.EditValue = "2019/02/27";
#endif
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            is_adj_type = "2";

            //取得table的schema，因為程式開啟會預設插入一筆空資料列
            DataTable dtMGD2 = dao40071.d_40071();
            gcMain.DataSource = dtMGD2;

            #region 下拉選單設定
            //調整倍數下拉選單
            List<LookupItem> rateList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1.5", DisplayMember = "1.5"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "2"},
                                        new LookupItem() { ValueMember = "3", DisplayMember = "3" }};
            rateLookUpEdit = new RepositoryItemLookUpEdit();
            rateLookUpEdit.SetColumnLookUp(rateList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(rateLookUpEdit);
            RATE_INPUT.ColumnEdit = rateLookUpEdit;

            //級距下拉選單
            //期貨
            prodTypeLookUpEdit1 = new RepositoryItemLookUpEdit();
            prodTypeLookUpEdit1.SetColumnLookUp(dtFLevel, "MGRT1_LEVEL", "MGRT1_LEVEL_NAME", TextEditStyles.DisableTextEditor, null);
            gcDetail.RepositoryItems.Add(prodTypeLookUpEdit1);
            //選擇權
            prodTypeLookUpEdit2 = new RepositoryItemLookUpEdit();
            prodTypeLookUpEdit2.SetColumnLookUp(dtOLevel, "MGRT1_LEVEL", "MGRT1_LEVEL_NAME", TextEditStyles.DisableTextEditor, null);
            gcDetail.RepositoryItems.Add(prodTypeLookUpEdit2);
            #endregion

            //預設新增一筆設定資料
            InsertRow();


            //txtSDate.Focus();
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
                gcDetail.DataSource = null;

                //日期檢核
                if (txtSDate.Text == "1901/01/01") {
                    MessageDisplay.Error("請輸入交易日期");
                    return ResultStatus.Fail;
                }
                int ii_curr_row = 0;
                string ls_kind_id = "", ls_ab_type, ls_stock_id = "";
                //讀取資料
                ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                DataTable dtMGD2 = dao40071.d_40071(ymd, is_adj_type);
                if (dtMGD2.Rows.Count == 0) {
                    MessageDisplay.Error("無任何資料！");
                    gcMain.DataSource = dao40071.d_40071();
                    //若無資料，預設新增一筆設定資料
                    InsertRow();
                    return ResultStatus.Fail;
                }
                dtMGD2 = dtMGD2.Sort("mgd2_stock_id,mgd2_kind_id");

                //準備兩個空的table給兩個grid
                DataTable dtInput = dao40071.d_40071();
                DataTable dtDetail = dao40071.d_40071_detail();
                dtDetail.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
                dtDetail.Columns["DATA_YMD"].ColumnName = "YMD";

                //依條件將讀取來的資料分配給兩個grid
                foreach (DataRow dr in dtMGD2.Rows) {
                    if (ls_stock_id != dr["MGD2_STOCK_ID"].AsString()) {
                        ls_stock_id = dr["MGD2_STOCK_ID"].AsString();
                        ii_curr_row = dtInput.Rows.Count;
                        dtInput.Rows.Add();
                        dtInput.Rows[ii_curr_row]["STOCK_ID"] = ls_stock_id;
                        dtInput.Rows[ii_curr_row]["RATE"] = dr["MGD2_ADJ_RATE"].AsDecimal() + 1;
                        dtInput.Rows[ii_curr_row]["PUB_YMD"] = dr["MGD2_PUB_YMD"];
                        dtInput.Rows[ii_curr_row]["IMPL_BEGIN_YMD"] = dr["MGD2_IMPL_BEGIN_YMD"];
                        dtInput.Rows[ii_curr_row]["IMPL_END_YMD"] = dr["MGD2_IMPL_END_YMD"];

                        dtInput.Rows[ii_curr_row]["ISSUE_BEGIN_YMD"] = dr["MGD2_ISSUE_BEGIN_YMD"];
                        dtInput.Rows[ii_curr_row]["ISSUE_END_YMD"] = dr["MGD2_ISSUE_END_YMD"];
                        dtInput.Rows[ii_curr_row]["YMD"] = dr["MGD2_YMD"];
                    }
                    if (ls_kind_id != dr["MGD2_KIND_ID"].AsString()) {
                        ls_kind_id = dr["MGD2_KIND_ID"].AsString();
                        ii_curr_row = dtDetail.Rows.Count;
                        dtDetail.Rows.Add();
                        dtDetail.Rows[ii_curr_row]["PROD_TYPE"] = dr["MGD2_PROD_TYPE"];
                        dtDetail.Rows[ii_curr_row]["KIND_ID"] = dr["MGD2_KIND_ID"];
                        dtDetail.Rows[ii_curr_row]["STOCK_ID"] = dr["MGD2_STOCK_ID"];
                        dtDetail.Rows[ii_curr_row]["ADJ_RATE"] = dr["MGD2_ADJ_RATE"];
                        dtDetail.Rows[ii_curr_row]["DATA_FLAG"] = "Y";

                        dtDetail.Rows[ii_curr_row]["PROD_SUBTYPE"] = dr["MGD2_PROD_SUBTYPE"];
                        dtDetail.Rows[ii_curr_row]["PARAM_KEY"] = dr["MGD2_PARAM_KEY"];
                        dtDetail.Rows[ii_curr_row]["M_CUR_LEVEL"] = dr["MGD2_CUR_LEVEL"];
                        dtDetail.Rows[ii_curr_row]["CURRENCY_TYPE"] = dr["MGD2_CURRENCY_TYPE"];
                        dtDetail.Rows[ii_curr_row]["SEQ_NO"] = dr["MGD2_SEQ_NO"];

                        dtDetail.Rows[ii_curr_row]["OSW_GRP"] = dr["MGD2_OSW_GRP"];
                        dtDetail.Rows[ii_curr_row]["AMT_TYPE"] = dr["MGD2_AMT_TYPE"];
                        dtDetail.Rows[ii_curr_row]["ISSUE_BEGIN_YMD"] = dr["MGD2_ISSUE_BEGIN_YMD"];
                        dtDetail.Rows[ii_curr_row]["ISSUE_END_YMD"] = dr["MGD2_ISSUE_END_YMD"];
                        dtDetail.Rows[ii_curr_row]["IMPL_BEGIN_YMD"] = dr["MGD2_IMPL_BEGIN_YMD"];

                        dtDetail.Rows[ii_curr_row]["IMPL_END_YMD"] = dr["MGD2_IMPL_END_YMD"];
                        dtDetail.Rows[ii_curr_row]["PUB_YMD"] = dr["MGD2_PUB_YMD"];
                        dtDetail.Rows[ii_curr_row]["YMD"] = dr["MGD2_YMD"];
                        dtDetail.Rows[ii_curr_row]["OP_TYPE"] = " "; //預設為空格
                    }
                    if (dr["MGD2_AB_TYPE"].AsString() == "B") {
                        dtDetail.Rows[ii_curr_row]["CM_CUR_B"] = dr["MGD2_CUR_CM"];
                        dtDetail.Rows[ii_curr_row]["MM_CUR_B"] = dr["MGD2_CUR_MM"];
                        dtDetail.Rows[ii_curr_row]["IM_CUR_B"] = dr["MGD2_CUR_IM"];
                        dtDetail.Rows[ii_curr_row]["CM_B"] = dr["MGD2_CM"];
                        dtDetail.Rows[ii_curr_row]["MM_B"] = dr["MGD2_MM"];

                        dtDetail.Rows[ii_curr_row]["IM_B"] = dr["MGD2_IM"];
                    }
                    else {
                        dtDetail.Rows[ii_curr_row]["CM_CUR_A"] = dr["MGD2_CUR_CM"];
                        dtDetail.Rows[ii_curr_row]["MM_CUR_A"] = dr["MGD2_CUR_MM"];
                        dtDetail.Rows[ii_curr_row]["IM_CUR_A"] = dr["MGD2_CUR_IM"];
                        dtDetail.Rows[ii_curr_row]["CM_A"] = dr["MGD2_CM"];
                        dtDetail.Rows[ii_curr_row]["MM_A"] = dr["MGD2_MM"];

                        dtDetail.Rows[ii_curr_row]["IM_A"] = dr["MGD2_IM"];
                    }
                }//foreach (DataRow dr in dtMGD2.Rows)

                //資料繫結
                gcMain.DataSource = dtInput;
                gcDetail.DataSource = dtDetail;

                //若無資料，預設新增一筆設定資料
                if (gvDetail.RowCount == 0) {
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
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            try {
                if (gvDetail.RowCount == 0) {
                    MessageDisplay.Info("沒有變更資料,不需要存檔!");
                    return ResultStatus.Fail;
                }
                #region ue_save_before
                gvMain.CloseEditor();
                gvMain.UpdateCurrentRow();
                gvDetail.CloseEditor();
                gvDetail.UpdateCurrentRow();

                string ls_stock_id, ls_ymd, ls_kind_id, ls_adj_type_name, ls_op_type, ls_dbname, ls_flag;
                string ls_issue_begin_ymd, ls_issue_end_ymd, ls_impl_begin_ymd, ls_impl_end_ymd, ls_pub_ymd, ls_trade_ymd, ls_mocf_ymd, ls_next_ymd;
                int ll_found, li_count, li_row, li_col, ii_curr_row;
                decimal ldbl_rate;
                DateTime ldt_w_TIME = DateTime.Now;

                DataTable dtGrid = (DataTable)gcDetail.DataSource;
                ll_found = dtGrid.Rows.IndexOf(dtGrid.Select("OP_TYPE <> ' '").FirstOrDefault());
                if (ll_found == -1) {
                    MessageDisplay.Warning("沒有變更資料,不需要存檔!");
                    return ResultStatus.Fail;
                }

                if (dtGrid.Rows.Count == 0) {
                    MessageDisplay.Warning("無明細資料，請重新產生明細");
                    return ResultStatus.Fail;
                }

                DataTable dtMGD2; //ids_mgd2
                DataTable dtMGD2Log = dao40071.d_40071_log(); //ids_old 
                dtMGD2Log.Clear(); //只取schema

                for (int f = 0; f < dtGrid.Rows.Count; f++) {
                    DataRow dr = dtGrid.Rows[f];
                    ls_op_type = dr["OP_TYPE"].ToString();
                    ls_flag = dr["DATA_FLAG"].AsString();
                    ls_stock_id = dr["STOCK_ID"].AsString();

                    //檢查同一標的的級距是否一致
                    if ((f + 1) < dtGrid.Rows.Count) {
                        if (ls_stock_id == dtGrid.Rows[f + 1]["STOCK_ID"].AsString() &&
                            dr["M_CUR_LEVEL"].AsString() != dtGrid.Rows[f + 1]["M_CUR_LEVEL"].AsString()) {
                            MessageDisplay.Error(ls_stock_id + "的級距不一致");
                            return ResultStatus.Fail;
                        }
                    }

                    //檢查有異動的資料
                    if (ls_op_type != " ") {
                        ls_kind_id = dr["KIND_ID"].AsString();
                        ls_ymd = dr["YMD"].ToString();
                        ls_issue_begin_ymd = dr["ISSUE_BEGIN_YMD"].ToString();
                        ls_issue_end_ymd = dr["ISSUE_END_YMD"].ToString();
                        ls_impl_begin_ymd = dr["IMPL_BEGIN_YMD"].ToString();
                        ls_impl_end_ymd = dr["IMPL_END_YMD"].ToString();
                        ls_pub_ymd = dr["PUB_YMD"].ToString();

                        if (ls_ymd != ls_impl_begin_ymd) {
                            DialogResult result = MessageDisplay.Choose(ls_stock_id + "," + ls_kind_id + "交易日不等於處置起日，請問是否更新");
                            if (result == DialogResult.No) return ResultStatus.Fail;
                        }
                        if (ls_issue_end_ymd != ls_impl_end_ymd) {
                            DialogResult result = MessageDisplay.Choose(ls_stock_id + "," + ls_kind_id + "生效迄日不等於處置迄日，請問是否更新");
                            if (result == DialogResult.No) return ResultStatus.Fail;
                        }

                        //處置期間首日+1個月
                        ls_mocf_ymd = PbFunc.relativedate(ls_impl_begin_ymd.AsDateTime("yyyyMMdd"), 30).ToString("yyyyMMdd");

                        /*次一營業日*/
                        ls_next_ymd = daoMOCF.GetNextTradeDay(ls_impl_begin_ymd, ls_mocf_ymd);
                        if (ls_issue_begin_ymd != ls_next_ymd) {
                            DialogResult result = MessageDisplay.Choose(ls_stock_id + "," + ls_kind_id + "生效起日不等於處置起日之次一營業日，請問是否更新");
                            if (result == DialogResult.No) return ResultStatus.Fail;
                        }

                        dtMGD2 = dao40072.d_40072(ls_ymd, is_adj_type, ls_stock_id);

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
                                if (ls_flag == "Y") dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TYPE"] = "U";
                                if (ls_flag == "N") dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TYPE"] = "D";
                                dtMGD2Log.Rows[ii_curr_row]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                                dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TIME"] = ldt_w_TIME;
                            }
                        }

                        /******************************************
                           確認商品是否在同一交易日不同情境下設定過
                        ******************************************/
                        DataTable dtSet = dao40071.IsSetOnSameDay(ls_kind_id, ls_ymd, is_adj_type);
                        if (dtSet.Rows.Count == 0) {
                            MessageDisplay.Error("MGD2 " + ls_kind_id + " 無任何資料！");
                            return ResultStatus.Fail;
                        }
                        li_count = dtSet.Rows[0]["LI_COUNT"].AsInt();
                        ls_adj_type_name = dtSet.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        if (li_count > 0) {
                            MessageDisplay.Error(ls_kind_id + ",交易日(" + ls_ymd + ")在" + ls_adj_type_name + "已有資料");
                            return ResultStatus.Fail;
                        }

                        /*********************************
                        確認商品是否在同一生效日區間設定過
                        生效起日若與生效迄日相同，不重疊
                        ex: 10/11的至10/31一般交易時段結束止，10/30的從10/31一般交易時段結束後始>>應不重疊
                        *************************************/
                        dtSet = dao40071.IsSetInSameSession(ls_kind_id, ls_ymd, ls_issue_begin_ymd, ls_issue_end_ymd);
                        li_count = dtSet.Rows[0]["LI_COUNT"].AsInt();
                        ls_adj_type_name = dtSet.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        ls_trade_ymd = dtSet.Rows[0]["LS_TRADE_YMD"].AsString();
                        if (li_count > 0) {
                            MessageDisplay.Error(ls_kind_id + "," + ls_adj_type_name + ",交易日(" + ls_trade_ymd + ")在同一生效日區間內已有資料");
                            return ResultStatus.Fail;
                        }

                        //判斷調整幅度是否為0
                        ldbl_rate = dr["ADJ_RATE"].AsDecimal();
                        if (ldbl_rate == 0) {
                            MessageDisplay.Error("商品調整幅度不可為0");
                            return ResultStatus.Fail;
                        }

                    }//if (ls_op_type != " ")
                }//for (int f = 0; f < dtGrid.Rows.Count; f++)
                #endregion
                string ls_prod_type;

                DataTable dtTemp = dao40072.d_40072(); //ids_tmp
                foreach (DataRow dr in dtGrid.Rows) {
                    ls_op_type = dr["OP_TYPE"].ToString();
                    //只更新有異動的資料
                    if (ls_op_type != " ") {
                        ls_kind_id = dr["KIND_ID"].AsString();
                        ls_stock_id = dr["STOCK_ID"].AsString();
                        ls_issue_begin_ymd = dr["ISSUE_BEGIN_YMD"].ToString();
                        ls_issue_end_ymd = dr["ISSUE_END_YMD"].ToString();
                        ls_impl_begin_ymd = dr["IMPL_BEGIN_YMD"].ToString();
                        ls_impl_end_ymd = dr["IMPL_END_YMD"].ToString();
                        ls_pub_ymd = dr["PUB_YMD"].ToString();
                        ls_ymd = dr["YMD"].ToString();
                        ldbl_rate = dr["ADJ_RATE"].AsDecimal();

                        //刪除已存在資料
                        if (daoMGD2.DeleteMGD2(ls_ymd, is_adj_type, ls_stock_id, ls_kind_id) < 0) {
                            MessageDisplay.Error("MGD2資料刪除失敗");
                            return ResultStatus.Fail;
                        }

                        if (dr["DATA_FLAG"].AsString() == "Y") {
                            ii_curr_row = dtTemp.Rows.Count;
                            ls_prod_type = dr["PROD_TYPE"].AsString();
                            dtTemp.Rows.Add();
                            dtTemp.Rows[ii_curr_row]["MGD2_YMD"] = ls_ymd;
                            dtTemp.Rows[ii_curr_row]["MGD2_PROD_TYPE"] = ls_prod_type;
                            dtTemp.Rows[ii_curr_row]["MGD2_KIND_ID"] = ls_kind_id;
                            dtTemp.Rows[ii_curr_row]["MGD2_STOCK_ID"] = ls_stock_id;
                            dtTemp.Rows[ii_curr_row]["MGD2_ADJ_TYPE"] = is_adj_type;

                            dtTemp.Rows[ii_curr_row]["MGD2_ADJ_RATE"] = ldbl_rate;
                            dtTemp.Rows[ii_curr_row]["MGD2_ADJ_CODE"] = "Y";
                            dtTemp.Rows[ii_curr_row]["MGD2_ISSUE_BEGIN_YMD"] = ls_issue_begin_ymd;
                            dtTemp.Rows[ii_curr_row]["MGD2_ISSUE_END_YMD"] = ls_issue_end_ymd;
                            dtTemp.Rows[ii_curr_row]["MGD2_IMPL_BEGIN_YMD"] = ls_impl_begin_ymd;

                            dtTemp.Rows[ii_curr_row]["MGD2_IMPL_END_YMD"] = ls_impl_end_ymd;
                            dtTemp.Rows[ii_curr_row]["MGD2_PUB_YMD"] = ls_pub_ymd;
                            dtTemp.Rows[ii_curr_row]["MGD2_PROD_SUBTYPE"] = dr["PROD_SUBTYPE"];
                            dtTemp.Rows[ii_curr_row]["MGD2_PARAM_KEY"] = dr["PARAM_KEY"];
                            dtTemp.Rows[ii_curr_row]["MGD2_CUR_CM"] = dr["CM_CUR_A"];

                            dtTemp.Rows[ii_curr_row]["MGD2_CUR_MM"] = dr["MM_CUR_A"];
                            dtTemp.Rows[ii_curr_row]["MGD2_CUR_IM"] = dr["IM_CUR_A"];
                            dtTemp.Rows[ii_curr_row]["MGD2_CUR_LEVEL"] = dr["M_CUR_LEVEL"];
                            dtTemp.Rows[ii_curr_row]["MGD2_CM"] = dr["CM_A"];
                            dtTemp.Rows[ii_curr_row]["MGD2_MM"] = dr["MM_A"];

                            dtTemp.Rows[ii_curr_row]["MGD2_IM"] = dr["IM_A"];
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
                                dtTemp.Rows[ii_curr_row]["MGD2_CUR_CM"] = dr["CM_CUR_B"];
                                dtTemp.Rows[ii_curr_row]["MGD2_CUR_MM"] = dr["MM_CUR_B"];
                                dtTemp.Rows[ii_curr_row]["MGD2_CUR_IM"] = dr["IM_CUR_B"];
                                dtTemp.Rows[ii_curr_row]["MGD2_CM"] = dr["CM_B"];

                                dtTemp.Rows[ii_curr_row]["MGD2_MM"] = dr["MM_B"];
                                dtTemp.Rows[ii_curr_row]["MGD2_IM"] = dr["IM_B"];
                            }
                        }//if (dr["DATA_FLAG"].AsString()=="Y")
                    }//if (ls_op_type != " ")
                }//foreach (DataRow dr in dtGrid.Rows)

                //Update DB
                //ids_tmp.update()
                ResultData myResultData = daoMGD2.UpdateMGD2(dtTemp);
                if (myResultData.Status == ResultStatus.Fail) {
                    MessageDisplay.Error("更新資料庫MGD2錯誤! ");
                    return ResultStatus.Fail;
                }

                //ids_old.update()
                if (dtMGD2Log.Rows.Count > 0) {
                    myResultData = daoMGD2L.UpdateMGD2L(dtMGD2Log);
                    if (myResultData.Status == ResultStatus.Fail) {
                        MessageDisplay.Error("更新資料庫MGD2L錯誤! ");
                        return ResultStatus.Fail;
                    }
                }
                //Write LOGF
                WriteLog("變更資料 ", "Info", "I");
                //列印
                ReportHelper _ReportHelper = new ReportHelper(gcDetail, _ProgramID, this.Text);
                Print(_ReportHelper);
                //for     i = 1 to dw_1.rowcount()
                //      dw_1.setitem(i, "op_type", ' ')
                //next
                //messagebox(gs_t_result, gs_m_ok, Information!)
                //wf_clear_ymd()
            }
            catch (Exception ex) {
                MessageDisplay.Error("儲存錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            try {
                ReportHelper _ReportHelper = new ReportHelper(gcDetail, _ProgramID, this.Text);
                CommonReportLandscapeA3 reportLandscape = new CommonReportLandscapeA3();//設定為橫向列印
                reportLandscape.printableComponentContainerMain.PrintableComponent = gcDetail;
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
        /// <summary>
        /// 級距下拉選單根據商品類別轉換的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetail_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
            GridView gv = sender as GridView;
            gv.CloseEditor();
            gv.UpdateCurrentRow();
            if (e.Column.FieldName == "M_CUR_LEVEL") {
                string ls_prod_type = gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString();
                if (ls_prod_type == "F") e.RepositoryItem = prodTypeLookUpEdit1;
                if (ls_prod_type == "O") e.RepositoryItem = prodTypeLookUpEdit2;
            }
        }

        private void gvDetail_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            if (e.Column.Name != "OP_TYPE") {
                //如果OP_TYPE是I則固定不變
                if (gv.GetRowCellValue(e.RowHandle, "OP_TYPE").ToString() == " ") gv.SetRowCellValue(e.RowHandle, "OP_TYPE", "U");
            }
        }

        private void gvDetail_CellValueChanging(object sender, CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            if (e.Column.Name == "M_CUR_LEVEL") {
                //如果改變級距
                string level = e.Value.AsString();
                if (gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString() == "F") {
                    DataRow dr = dtFLevel.Select("mgrt1_level = '" + level + "'")[0];
                    gv.SetRowCellValue(e.RowHandle, "CM_CUR_A", dr["MGRT1_CM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "MM_CUR_A", dr["MGRT1_MM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "IM_CUR_A", dr["MGRT1_IM_RATE"]);
                    if (gv.GetRowCellValue(e.RowHandle, "CM_CUR_B") != DBNull.Value) {
                        gv.SetRowCellValue(e.RowHandle, "CM_CUR_B", dr["MGRT1_CM_RATE_B"]);
                    }
                    if (gv.GetRowCellValue(e.RowHandle, "MM_CUR_B") != DBNull.Value) {
                        gv.SetRowCellValue(e.RowHandle, "MM_CUR_B", dr["MGRT1_MM_RATE_B"]);
                    }
                    if (gv.GetRowCellValue(e.RowHandle, "IM_CUR_B") != DBNull.Value) {
                        gv.SetRowCellValue(e.RowHandle, "IM_CUR_B", dr["MGRT1_IM_RATE_B"]);
                    }
                }
                if (gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString() == "O") {
                    DataRow dr = dtOLevel.Select("mgrt1_level = '" + level + "'")[0];
                    gv.SetRowCellValue(e.RowHandle, "CM_CUR_A", dr["MGRT1_CM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "MM_CUR_A", dr["MGRT1_MM_RATE"]);
                    gv.SetRowCellValue(e.RowHandle, "IM_CUR_A", dr["MGRT1_IM_RATE"]);
                    if (gv.GetRowCellValue(e.RowHandle, "CM_CUR_B") != DBNull.Value) {
                        gv.SetRowCellValue(e.RowHandle, "CM_CUR_B", dr["MGRT1_CM_RATE_B"]);
                    }
                    if (gv.GetRowCellValue(e.RowHandle, "MM_CUR_B") != DBNull.Value) {
                        gv.SetRowCellValue(e.RowHandle, "MM_CUR_B", dr["MGRT1_MM_RATE_B"]);
                    }
                    if (gv.GetRowCellValue(e.RowHandle, "IM_CUR_B") != DBNull.Value) {
                        gv.SetRowCellValue(e.RowHandle, "IM_CUR_B", dr["MGRT1_IM_RATE_B"]);
                    }
                }
            }//if (e.Column.Name == "M_CUR_LEVEL")
        }

        /// <summary>
        /// 期貨的保證金B值不能key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetail_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string prod_type = gv.GetRowCellValue(gv.FocusedRowHandle, "PROD_TYPE").ToString();
            //string stock_id = gv.GetRowCellValue(gv.FocusedRowHandle, "STOCK_ID").AsString();
            if (gv.FocusedColumn.Name == "CM_CUR_B" ||
                gv.FocusedColumn.Name == "MM_CUR_B" ||
                gv.FocusedColumn.Name == "IM_CUR_B" ||
                gv.FocusedColumn.Name == "CM_B" ||
                gv.FocusedColumn.Name == "MM_B" ||
                gv.FocusedColumn.Name == "IM_B") {
                e.Cancel = prod_type == "F" ? true : false;
                //e.Cancel = stock_id == null ? true : false;
            }
        }

        private void gvDetail_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            GridView gv = sender as GridView;
            string amt_type = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMT_TYPE"]).AsString();

            switch (e.Column.FieldName) {
                case "KIND_ID":
                case "STOCK_ID":
                case "IMPL_BEGIN_YMD":
                case "IMPL_END_YMD":
                case "ISSUE_BEGIN_YMD":
                case "ISSUE_END_YMD":
                case "PUB_YMD":
                case "YMD":
                case "ADJ_RATE":
                    e.Appearance.BackColor = Color.FromArgb(224, 224, 224);
                    break;
                case "CM_CUR_A":
                case "CM_CUR_B":
                case "MM_CUR_A":
                case "MM_CUR_B":
                case "IM_CUR_A":
                case "IM_CUR_B":
                case "CM_A":
                case "CM_B":
                case "MM_A":
                case "MM_B":
                case "IM_A":
                case "IM_B":
                    e.Column.DisplayFormat.FormatString = amt_type == "P" ? "{0:0.###%}" : "#,###";
                    break;
            }
        }

        /// <summary>
        /// 只要gvMain有異動，gvDetail就要清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_CellValueChanging(object sender, CellValueChangedEventArgs e) {

            gcDetail.DataSource = null;
        }
        #endregion

        /// <summary>
        /// 全選
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAll_Click(object sender, EventArgs e) {
            for (int f = 0; f < gvDetail.RowCount; f++) {
                gvDetail.SetRowCellValue(f, "DATA_FLAG", "Y");
                if (gvDetail.GetRowCellValue(f, "OP_TYPE").ToString() != "I") {
                    gvDetail.SetRowCellValue(f, "OP_TYPE", "U");
                }
            }
        }

        /// <summary>
        /// 不全選
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNone_Click(object sender, EventArgs e) {
            for (int f = 0; f < gvDetail.RowCount; f++) {
                gvDetail.SetRowCellValue(f, "DATA_FLAG", "N");
                if (gvDetail.GetRowCellValue(f, "OP_TYPE").ToString() != "I") {
                    gvDetail.SetRowCellValue(f, "OP_TYPE", "U");
                }
            }
        }

        /// <summary>
        /// 顯示明細
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetail_Click(object sender, EventArgs e) {

            //重設gridview
            gcDetail.DataSource = null;

            int li_row, li_col, ll_found;
            string ls_prod_type, ls_prod_type_name, ls_kind_id, ls_stock_id, ls_param_key, ls_abroad, ls_impl_begin_ymd, ls_issue_begin_ymd, ls_mocf_ymd;
            string ls_op_type;
            decimal ldc_rate;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dtGrid = dao40071.d_40071_detail(); //ids_tmp 空的，拿來重置gcDetail 
            dtGrid.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
            dtGrid.Columns["DATA_YMD"].ColumnName = "YMD";
            //dtTemp.Columns["CM_A * NVL(MGT6_REF_XXX,1)"].ColumnName = "CM_A"; 沒成功撈到資料的話欄位名稱不會變?
            DataTable dtMGD2 = dao40072.d_40072(); //ids_mgd2 空的

            txtSDate.Text = "1901/01/01";
            ls_prod_type = "S";
            ls_param_key = "ST%";
            ls_abroad = "%";
            ls_kind_id = "%";

            //產生明細檔
            DataTable dtInput = (DataTable)gcMain.DataSource;
            foreach (DataRow drInput in dtInput.Rows) {
                ls_op_type = "I";
                ls_stock_id = drInput["STOCK_ID"].ToString();
                ls_impl_begin_ymd = drInput["IMPL_BEGIN_YMD"].ToString();

                //交易日為處置期間之首日
                drInput["YMD"] = ls_impl_begin_ymd;

                dtMGD2 = dao40072.d_40072(drInput["YMD"].ToString(), is_adj_type, ls_stock_id);
                if (dtMGD2.Rows.Count > 0) {
                    DialogResult result = MessageDisplay.Choose(ls_stock_id + "資料已存在，是否重新產製資料,若不重產資料，請按「預覽」!");
                    if (result == DialogResult.No) return;
                    ls_op_type = "U";
                }

                //處置期間首日+1個月
                ls_mocf_ymd = PbFunc.relativedate(ls_impl_begin_ymd.AsDateTime("yyyy/MM/dd"), 30).ToString("yyyyMMdd");
                /*次一營業日*/
                ls_impl_begin_ymd=ls_impl_begin_ymd.AsDateTime("yyyy/MM/dd").ToString("yyyyMMdd");
                ls_issue_begin_ymd = daoMOCF.GetNextTradeDay(ls_impl_begin_ymd, ls_mocf_ymd);

                //終止生效日為處置期間迄日
                drInput["ISSUE_END_YMD"] = drInput["IMPL_END_YMD"];
                //開始生效日為處置期間首日之次一個營業日
                drInput["ISSUE_BEGIN_YMD"] = ls_issue_begin_ymd;

                //判斷是否有空值 
                for (li_col = 0; li_col < dtInput.Columns.Count; li_col++) {
                    if (dtInput.Columns[li_col].ColumnName == "CPSORT") continue; //這欄是排序用的毋須判斷
                    if (drInput[li_col] == DBNull.Value || drInput[li_col].ToString() == "") {
                        MessageDisplay.Error("請確認資料是否輸入完成!");
                        return;
                    }
                }
                ls_stock_id = ls_stock_id + "%";

                //調整倍數(計算用1+調整備數)
                ldc_rate = drInput["RATE"].AsDecimal() - 1;

                //這邊才去讀SP
                DataTable dtTemp = dao40071.d_40071_detail(ls_impl_begin_ymd, ls_prod_type, ls_param_key, ls_abroad, ls_kind_id, ls_stock_id, ldc_rate);
                dtTemp.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
                dtTemp.Columns["DATA_YMD"].ColumnName = "YMD";
                if (dtTemp.Columns["CM_A*NVL(MGT6_REF_XXX,1)"] != null) dtTemp.Columns["CM_A*NVL(MGT6_REF_XXX,1)"].ColumnName = "CM_A"; //沒撈到值的話欄位名稱不會變，若資料為個股類也不會變
                foreach (DataRow drTemp in dtTemp.Rows) {
                    drTemp["ISSUE_BEGIN_YMD"] = ls_issue_begin_ymd;
                    drTemp["ISSUE_END_YMD"] = drInput["impl_end_ymd"];
                    drTemp["IMPL_BEGIN_YMD"] = ls_impl_begin_ymd;
                    drTemp["IMPL_END_YMD"] = drInput["impl_end_ymd"];
                    drTemp["PUB_YMD"] = drInput["pub_ymd"];
                    drTemp["YMD"] = ls_impl_begin_ymd;
                    drTemp["OP_TYPE"] = ls_op_type;
                }

                //將資料複製到明細表
                //dtGrid = dtTemp.Clone();
                foreach (DataRow drTemp in dtTemp.Rows) {
                    dtGrid.ImportRow(drTemp);
                }
                dtGrid.AcceptChanges();
            }//foreach (DataRow drInput in dtInput.Rows)

            //sort("stock_id A prod_type A ")
            if (dtGrid.Rows.Count != 0) {
                dtGrid = dtGrid.AsEnumerable().OrderBy(x => x.Field<string>("STOCK_ID"))
                        .ThenBy(x => x.Field<string>("PROD_TYPE"))
                        .CopyToDataTable();
            }
            gcDetail.DataSource = dtGrid;

            if (gvDetail.RowCount == 0) {
                MessageDisplay.Warning("無明細資料，請確認「交易日期」及「商品調整幅度」是否填寫正確!");
                return;
            }
        }
    }
}