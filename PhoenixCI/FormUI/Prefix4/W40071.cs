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
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using Common;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;
using DataObjects.Dao.Together.TableDao;
using BaseGround.Report;
using DevExpress.XtraGrid.Views.Base;

/// <summary>
/// Lukas, 2019/5/3
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    public partial class W40071 : FormParent {

        /// <summary>
        /// 調整類型 0一般 1長假 2處置股票 3股票
        /// </summary>
        protected string is_adj_type { get; set; }
        private D40071 dao40071;
        private MGD2 daoMGD2;
        private MGD2L daoMGD2L;
        private RepositoryItemLookUpEdit prodTypeLookUpEdit;
        /// <summary>
        /// 指數(國內)
        /// </summary>
        private RepositoryItemLookUpEdit paramKeyLookUpEdit1;
        /// <summary>
        /// 指數(國外)
        /// </summary>
        private RepositoryItemLookUpEdit paramKeyLookUpEdit2;
        /// <summary>
        /// 商品
        /// </summary>
        private RepositoryItemLookUpEdit paramKeyLookUpEdit3;
        /// <summary>
        /// 利率
        /// </summary>
        private RepositoryItemLookUpEdit paramKeyLookUpEdit4;
        /// <summary>
        /// 匯率
        /// </summary>
        private RepositoryItemLookUpEdit paramKeyLookUpEdit5;
        /// <summary>
        /// 個股
        /// </summary>
        private RepositoryItemLookUpEdit paramKeyLookUpEdit6;
        /// <summary>
        /// ETF
        /// </summary>
        private RepositoryItemLookUpEdit paramKeyLookUpEdit7;

        public W40071(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao40071 = new D40071();
            daoMGD2 = new MGD2();
            daoMGD2L = new MGD2L();
            GridHelper.SetCommonGrid(gvMain);
            GridHelper.SetCommonGrid(gvDetail);
            gvDetail.AppearancePrint.BandPanel.Font = new Font("Microsoft YaHei", 10);
            gvDetail.AppearancePrint.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.DateTimeValue = DateTime.Now;
            txtEffectiveSDate.Text = "1901/01/01";
            txtEffectiveEDate.Text = "1901/01/01";
            txtImplSDate.Text = "1901/01/01";
            txtImplEDate.Text = "1901/01/01";
            is_adj_type = "1";
            DataTable dtInput = dao40071.d_40071_input();
            dtInput.Columns.Add("Is_NewRow", typeof(string));
            //既有的資料非新資料列，故預設為0
            foreach (DataRow drInput in dtInput.Rows) {
                drInput["Is_NewRow"] = "0";
            }
            #region 下拉選單設定
            //商品類欄位下拉選單
            DataTable dtLookUp = dao40071.d_40071_prod_type_ddl();
            prodTypeLookUpEdit = new RepositoryItemLookUpEdit();
            prodTypeLookUpEdit.SetColumnLookUp(dtLookUp, "PROD_SEQ_NO", "SUBTYPE_NAME", TextEditStyles.DisableTextEditor, "");
            gcMain.RepositoryItems.Add(prodTypeLookUpEdit);
            PROD_SEQ_NO.ColumnEdit = prodTypeLookUpEdit;

            //商品下拉選單
            string ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            //指數(國內)
            paramKeyLookUpEdit1 = new RepositoryItemLookUpEdit();
            DataTable dtParamKey = dao40071.dddw_mgt2_kind("I%", "          ");
            paramKeyLookUpEdit1.SetColumnLookUp(dtParamKey, "KIND_ID", "KIND_ID", TextEditStyles.DisableTextEditor, "All");
            gcMain.RepositoryItems.Add(paramKeyLookUpEdit1);
            //指數(國外)
            paramKeyLookUpEdit2 = new RepositoryItemLookUpEdit();
            dtParamKey = dao40071.dddw_mgt2_kind("I%", "Y");
            paramKeyLookUpEdit2.SetColumnLookUp(dtParamKey, "KIND_ID", "KIND_ID", TextEditStyles.DisableTextEditor, "All");
            gcMain.RepositoryItems.Add(paramKeyLookUpEdit2);
            //商品
            paramKeyLookUpEdit3 = new RepositoryItemLookUpEdit();
            dtParamKey = dao40071.dddw_mgt2_kind("C%", "          ");
            paramKeyLookUpEdit3.SetColumnLookUp(dtParamKey, "KIND_ID", "KIND_ID", TextEditStyles.DisableTextEditor, "All");
            gcMain.RepositoryItems.Add(paramKeyLookUpEdit3);
            //利率
            paramKeyLookUpEdit4 = new RepositoryItemLookUpEdit();
            dtParamKey = dao40071.dddw_mgt2_kind("B%", "          ");
            paramKeyLookUpEdit4.SetColumnLookUp(dtParamKey, "KIND_ID", "KIND_ID", TextEditStyles.DisableTextEditor, "All");
            gcMain.RepositoryItems.Add(paramKeyLookUpEdit4);
            //匯率
            paramKeyLookUpEdit5 = new RepositoryItemLookUpEdit();
            dtParamKey = dao40071.dddw_mgt2_kind("E%", "          ");
            paramKeyLookUpEdit5.SetColumnLookUp(dtParamKey, "KIND_ID", "KIND_ID", TextEditStyles.DisableTextEditor, "All");
            gcMain.RepositoryItems.Add(paramKeyLookUpEdit5);
            //個股
            paramKeyLookUpEdit6 = new RepositoryItemLookUpEdit();
            dtParamKey = dao40071.dddw_pdk_kind_id_40071(ymd, "ST%");
            paramKeyLookUpEdit6.SetColumnLookUp(dtParamKey, "KIND_ID", "KIND_ID", TextEditStyles.DisableTextEditor, "All");
            gcMain.RepositoryItems.Add(paramKeyLookUpEdit6);
            //ETF
            paramKeyLookUpEdit7 = new RepositoryItemLookUpEdit();
            dtParamKey = dao40071.dddw_pdk_kind_id_40071(ymd, "ET%");
            paramKeyLookUpEdit7.SetColumnLookUp(dtParamKey, "KIND_ID", "KIND_ID", TextEditStyles.DisableTextEditor, "All");
            gcMain.RepositoryItems.Add(paramKeyLookUpEdit7);
            #endregion

            gcMain.DataSource = dtInput;
#if DEBUG
            txtSDate.EditValue = "2019/01/29";
            txtImplSDate.EditValue = "2019/01/01";
            txtImplEDate.EditValue = "2019/01/31";
#endif
            txtSDate.Focus();
            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = true;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = true;//列印

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

        protected override ResultStatus Retrieve() {
            try {
                int ii_curr_row = 0;
                string ls_kind_id = "";
                //0. 清空Grid
                gcMain.DataSource = null;
                gcDetail.DataSource = null;
                //1. 讀取資料
                DataTable dtMGD2 = dao40071.d_40071(txtSDate.DateTimeValue.ToString("yyyyMMdd"), is_adj_type);
                if (dtMGD2.Rows.Count == 0) {
                    MessageDisplay.Warning("無任何資料！");
                    return ResultStatus.Fail;
                }
                //2. 重置實施/生效日期與左側的gridview(PB的wf_clear_ymd())
                txtEffectiveSDate.Text = "1901/01/01";
                txtEffectiveEDate.Text = "1901/01/01";
                txtImplSDate.Text = "1901/01/01";
                txtImplEDate.Text = "1901/01/01";
                DataTable dtInput = dao40071.d_40071_input();
                gcMain.DataSource = dtInput;
                dtInput.Columns.Add("Is_NewRow", typeof(string));
                //既有的資料非新資料列，故預設為0
                foreach (DataRow drInput in dtInput.Rows) {
                    drInput["Is_NewRow"] = "0";
                }
                //3. 篩選並填值到右側的gridview裡(而非直接綁定datasourc e)
                DataTable dtDetail = dao40071.d_40071_detail(); //只取schema
                dtDetail.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
                dtDetail.Columns["DATA_YMD"].ColumnName = "YMD";

                foreach (DataRow dr in dtMGD2.Rows) {

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
                        dtDetail.Rows[ii_curr_row]["YMD"] = dr["MGD2_YMD"];
                        dtDetail.Rows[ii_curr_row]["OP_TYPE"] = " ";
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

                //排序 prod_type A if(prod_subtype<>'S',0,1) A prod_subtype A kind_id A 
                dtDetail = dtDetail.AsEnumerable().OrderBy(x => x.Field<string>("PROD_TYPE"))
                    .ThenBy(x => {
                        if (x.Field<string>("PROD_SUBTYPE") != "S")
                            return 1;
                        else
                            return 2;
                    }).ThenBy(x => x.Field<string>("PROD_SUBTYPE"))
                    .ThenBy(x => x.Field<string>("KIND_ID"))
                    .CopyToDataTable();
                gcDetail.DataSource = dtDetail;
            }
            catch (Exception ex) {
                MessageDisplay.Error("讀取錯誤");
                throw ex;
            }

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

                int ll_found, li_col, ii_curr_row, li_count;
                string ls_dbname, ls_adj_type_name, ls_trade_ymd;
                string ls_ymd, ls_issue_begin_ymd, ls_issue_end_ymd, ls_kind_id, ls_impl_begin_ymd, ls_impl_end_ymd, ls_flag, ls_op_type;
                decimal ldbl_rate;
                DateTime ldt_w_TIME = DateTime.Now;
                ls_ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");

                DataTable dtGrid = (DataTable)gcDetail.DataSource;
                ll_found = dtGrid.Rows.IndexOf(dtGrid.Select("OP_TYPE <> ' '").FirstOrDefault());
                if (ll_found == -1) {
                    MessageDisplay.Warning("沒有變更資料,不需要存檔!");
                    return ResultStatus.Fail;
                }

                DataTable dtMGD2 = dao40071.d_40071(ls_ymd, is_adj_type); //ids_mgd2
                if (dtMGD2.Rows.Count == 0) {
                    MessageDisplay.Error("無任何資料！");
                    return ResultStatus.Fail;
                }
                DataTable dtMGD2Log = dao40071.d_40071_log(); //ids_old 
                dtMGD2Log.Clear(); //只取schema

                //資料重新產製，將舊資料全部寫入log
                ll_found = dtGrid.Rows.IndexOf(dtGrid.Select("OP_TYPE ='I'").FirstOrDefault());
                if (ll_found > -1) {
                    foreach (DataRow drMGD2 in dtMGD2.Rows) {
                        ii_curr_row = dtMGD2Log.Rows.Count;
                        dtMGD2Log.Rows.Add();
                        for (li_col = 0; li_col < dtMGD2.Columns.Count; li_col++) {
                            //先取欄位名稱，因為兩張table欄位順序不一致
                            ls_dbname = dtMGD2.Columns[li_col].ColumnName;
                            if (ls_dbname == "CPSORT") continue; //這個欄位是拿來排序用的，故無需複製
                            dtMGD2Log.Rows[ii_curr_row][ls_dbname] = drMGD2[li_col];
                        }
                        dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TYPE"] = "D";
                        dtMGD2Log.Rows[ii_curr_row]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                        dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TIME"] = ldt_w_TIME;
                    }
                }

                foreach (DataRow dr in dtGrid.Rows) {
                    ls_op_type = dr["OP_TYPE"].ToString();
                    //只更新有異動的資料
                    if (ls_op_type != " ") {
                        ls_kind_id = dr["KIND_ID"].AsString();
                        ls_issue_begin_ymd = dr["ISSUE_BEGIN_YMD"].ToString();
                        ls_issue_end_ymd = dr["ISSUE_END_YMD"].ToString();
                        ls_impl_begin_ymd = dr["IMPL_BEGIN_YMD"].ToString();
                        ls_impl_end_ymd = dr["IMPL_END_YMD"].ToString();
                        ls_flag = dr["DATA_FLAG"].AsString();

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

                        if (ls_flag == "Y") {
                            if (ls_issue_begin_ymd == ls_issue_end_ymd) {
                                MessageDisplay.Error(ls_kind_id + "生效起迄日不可為同一天");
                                return ResultStatus.Fail;
                            }
                            if (ls_impl_begin_ymd == ls_impl_end_ymd) {
                                MessageDisplay.Error(ls_kind_id + "實施起迄日不可為同一天");
                                return ResultStatus.Fail;
                            }
                            if (txtImplSDate.Text != "1901/01/01" && ls_impl_begin_ymd != txtImplSDate.DateTimeValue.ToString("yyyyMMdd")) {
                                DialogResult result = MessageDisplay.Choose(ls_kind_id + "的實施起日(" + ls_impl_begin_ymd + ")與設定(" + txtImplSDate.Text + ")不同，請問是否更新");
                                if (result == DialogResult.No) return ResultStatus.Fail;
                            }
                            if (txtImplEDate.Text != "1901/01/01" && ls_impl_end_ymd != txtImplEDate.DateTimeValue.ToString("yyyyMMdd")) {
                                DialogResult result = MessageDisplay.Choose(ls_kind_id + "的實施迄日(" + ls_impl_end_ymd + ")與設定(" + txtImplEDate.Text + ")不同，請問是否更新");
                                if (result == DialogResult.No) return ResultStatus.Fail;
                            }
                            if (txtEffectiveSDate.Text != "1901/01/01" && ls_issue_begin_ymd != txtEffectiveSDate.DateTimeValue.ToString("yyyyMMdd")) {
                                DialogResult result = MessageDisplay.Choose(ls_kind_id + "的生效起日(" + ls_issue_begin_ymd + ")與設定(" + txtEffectiveSDate.Text + ")不同，請問是否更新");
                                if (result == DialogResult.No) return ResultStatus.Fail;
                            }
                            if (txtEffectiveEDate.Text != "1901/01/01" && ls_issue_end_ymd != txtEffectiveEDate.DateTimeValue.ToString("yyyyMMdd")) {
                                DialogResult result = MessageDisplay.Choose(ls_kind_id + "的生效迄日(" + ls_issue_end_ymd + ")與設定(" + txtEffectiveEDate.Text + ")不同，請問是否更新");
                                if (result == DialogResult.No) return ResultStatus.Fail;
                            }
                            if (ls_issue_begin_ymd != ls_impl_begin_ymd || ls_issue_end_ymd != ls_impl_end_ymd) {
                                DialogResult result = MessageDisplay.Choose(ls_kind_id + "的生效起迄與實施起迄不同，請問是否更新");
                                if (result == DialogResult.No) return ResultStatus.Fail;
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
                        }//if (ls_flag == "Y")
                    }//if (ls_op_type != " ")
                }//foreach (DataRow dr in dtGrid.Rows)
                #endregion

                string ls_prod_type;
                //資料重新產置，將舊資料全部寫入log
                ll_found = dtGrid.Rows.IndexOf(dtGrid.Select("OP_TYPE ='I'").FirstOrDefault());
                if (ll_found > -1) {
                    //刪除已存在資料
                    if (dao40071.DeleteMGD2(ls_ymd, is_adj_type) < 0) {
                        MessageDisplay.Error("MGD2資料刪除失敗");
                        return ResultStatus.Fail;
                    }
                }

                DataTable dtTemp = dao40071.d_40071(ls_ymd, is_adj_type); // ids_tmp
                dtTemp.Clear(); // 只取schema

                foreach (DataRow dr in dtGrid.Rows) {
                    ls_op_type = dr["OP_TYPE"].ToString();
                    //只更新有異動的資料
                    if (ls_op_type != " ") {
                        ls_kind_id = dr["KIND_ID"].AsString();
                        ls_issue_begin_ymd = dr["ISSUE_BEGIN_YMD"].ToString();
                        ls_issue_end_ymd = dr["ISSUE_END_YMD"].ToString();
                        ls_impl_begin_ymd = dr["IMPL_BEGIN_YMD"].ToString();
                        ls_impl_end_ymd = dr["IMPL_END_YMD"].ToString();
                        ls_flag = dr["DATA_FLAG"].AsString();
                        ldbl_rate = dr["ADJ_RATE"].AsDecimal();

                        //若有異動資料，刪除舊資料
                        if (ls_op_type == "U") {
                            if (daoMGD2.DeleteMGD2(ls_ymd, is_adj_type, ls_kind_id) < 0) {
                                MessageDisplay.Error("MGD2資料刪除失敗");
                                return ResultStatus.Fail;
                            }
                        }

                        if (ls_flag == "Y") {
                            ii_curr_row = dtTemp.Rows.Count;
                            ls_prod_type = dr["PROD_TYPE"].ToString();
                            dtTemp.Rows.Add();
                            dtTemp.Rows[ii_curr_row]["MGD2_YMD"] = dr["YMD"];
                            dtTemp.Rows[ii_curr_row]["MGD2_PROD_TYPE"] = ls_prod_type;
                            dtTemp.Rows[ii_curr_row]["MGD2_KIND_ID"] = ls_kind_id;
                            dtTemp.Rows[ii_curr_row]["MGD2_STOCK_ID"] = dr["STOCK_ID"];
                            dtTemp.Rows[ii_curr_row]["MGD2_ADJ_TYPE"] = is_adj_type;

                            dtTemp.Rows[ii_curr_row]["MGD2_ADJ_RATE"] = ldbl_rate;
                            dtTemp.Rows[ii_curr_row]["MGD2_ADJ_CODE"] = "Y";
                            dtTemp.Rows[ii_curr_row]["MGD2_ISSUE_BEGIN_YMD"] = ls_issue_begin_ymd;
                            dtTemp.Rows[ii_curr_row]["MGD2_ISSUE_END_YMD"] = ls_issue_end_ymd;
                            dtTemp.Rows[ii_curr_row]["MGD2_IMPL_BEGIN_YMD"] = ls_impl_begin_ymd;

                            dtTemp.Rows[ii_curr_row]["MGD2_IMPL_END_YMD"] = ls_impl_end_ymd;
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
                        }
                    }//if (ls_op_type != " ")
                }// foreach (DataRow dr in dtGrid.Rows)

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

                return ResultStatus.Success;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }

        #region GridView Events
        private void gvDetail_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            GridView gv = sender as GridView;
            string amt_type = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMT_TYPE"]).AsString();

            switch (e.Column.FieldName) {
                case "KIND_ID":
                case "STOCK_ID":
                case "M_CUR_LEVEL":
                case "YMD":
                case "ADJ_RATE":
                    e.Appearance.BackColor = Color.FromArgb(192, 192, 192);
                    break;
                case "CM_CUR_A":
                case "CM_CUR_B":
                case "MM_CUR_A":
                case "MM_CUR_B":
                case "IM_CUR_A":
                case "IM_CUR_B":
                    e.Column.DisplayFormat.FormatString = amt_type == "P" ? "{0:0.###%}" : "#,###";
                    e.Appearance.BackColor = Color.FromArgb(192, 192, 192);
                    break;
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
        /// 設定OP_TYPE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetail_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            if (e.Column.Name != "OP_TYPE") {
                //如果OP_TYPE是I則固定不變
                if (gv.GetRowCellValue(e.RowHandle, "OP_TYPE").ToString() == " ") gv.SetRowCellValue(e.RowHandle, "OP_TYPE", "U");
            }
        }

        /// <summary>
        /// 期貨的調整後保證金B值不能key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetail_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string prod_type = gv.GetRowCellValue(gv.FocusedRowHandle, "PROD_TYPE").ToString();
            string stock_id = gv.GetRowCellValue(gv.FocusedRowHandle, "STOCK_ID").AsString();
            if (gv.FocusedColumn.Name == "CM_B" ||
                gv.FocusedColumn.Name == "MM_B" ||
                gv.FocusedColumn.Name == "IM_B") {
                e.Cancel = prod_type == "F" ? true : false;
                //e.Cancel = stock_id == null ? true : false;
            }
        }

        /// <summary>
        /// 商品類欄位與商品別欄位連動的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
            GridView gv = sender as GridView;
            gv.CloseEditor();
            gv.UpdateCurrentRow();
            //DataTable dtTemp = (DataTable)gcMain.DataSource;
            if (e.Column.FieldName == "PROD_KIND_ID") {
                string value = gv.GetRowCellValue(e.RowHandle, "PROD_SEQ_NO").AsString();
                switch (value) {
                    case "1":
                        e.RepositoryItem = paramKeyLookUpEdit1;
                        break;
                    case "2":
                        e.RepositoryItem = paramKeyLookUpEdit2;
                        break;
                    case "3":
                        e.RepositoryItem = paramKeyLookUpEdit3;
                        break;
                    case "4":
                        e.RepositoryItem = paramKeyLookUpEdit4;
                        break;
                    case "5":
                        e.RepositoryItem = paramKeyLookUpEdit5;
                        break;
                    case "6":
                        e.RepositoryItem = paramKeyLookUpEdit6;
                        break;
                    case "7":
                        e.RepositoryItem = paramKeyLookUpEdit7;
                        break;
                }
                gv.CloseEditor();
                gv.UpdateCurrentRow();
            }
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            //前7比固定資料不能異動(除了調整幅度欄位)
            GridView gv = sender as GridView;
            if (e.RowHandle <= 6) {
                if (e.Column.FieldName == "PROD_SEQ_NO") {
                    e.Appearance.BackColor = Color.FromArgb(192, 192, 192);
                }
                if (e.Column.FieldName == "PROD_KIND_ID") {
                    e.Appearance.BackColor = Color.FromArgb(192, 192, 192);
                }
            }
            else {
                if (e.Column.FieldName == "PROD_SEQ_NO") {
                    e.Appearance.BackColor = Color.White;
                }
                if (e.Column.FieldName == "PROD_KIND_ID") {
                    e.Appearance.BackColor = Color.White;
                }
            }
        }

        private void gvMain_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            if (e.Column.Name == "PROD_SEQ_NO") {
                DataTable dtInsert = dao40071.d_40071_input(e.Value.AsString());
                gv.SetRowCellValue(e.RowHandle, "PROD_SUBTYPE", dtInsert.Rows[0]["PROD_SUBTYPE"]);
                gv.SetRowCellValue(e.RowHandle, "PROD_SUBTYPE_NAME", dtInsert.Rows[0]["PROD_SUBTYPE_NAME"]);
                gv.SetRowCellValue(e.RowHandle, "PARAM_KEY", dtInsert.Rows[0]["PARAM_KEY"]);
                gv.SetRowCellValue(e.RowHandle, "ABROAD", dtInsert.Rows[0]["ABROAD"]);
            }
        }

        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
        }

        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, "Is_NewRow").ToString();
            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                e.Cancel = false;
            }
            else if (gv.FocusedColumn.Name == "PROD_SEQ_NO" ||
                     gv.FocusedColumn.Name == "PROD_KIND_ID") {
                e.Cancel = true;
            }
            else {
                e.Cancel = false;
            }
        }

        /// <summary>
        /// gvMain資料列大於第七筆可以刪除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e) {
            GridView gv = sender as GridView;
            _ToolBtnDel.Enabled = e.FocusedRowHandle > 6 ? true : false;
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

            int ll_found, li_row, li_col;
            string ymd, is_chk, is_kind_list, ls_prod_type, ls_prod_type_name, ls_kind_id, ls_param_key, ls_abroad, ls_dbname;
            decimal ldc_rate;
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            if (txtImplSDate.Text == "1901/01/01" || txtImplEDate.Text == "1901/01/01") {
                MessageDisplay.Warning("請輸入實施期間!");
                return;
            }
            if (txtImplSDate.Text == txtImplEDate.Text) {
                MessageDisplay.Warning("長假實施日期起迄日期相同，請重新輸入!");
                return;
            }

            //生效日起迄 = 實施日起迄
            txtEffectiveSDate.Text = txtImplSDate.Text;
            txtEffectiveEDate.Text = txtImplEDate.Text;

            DataTable dtGrid = dao40071.d_40071_detail(); //空的，拿來重置gcDetail
            dtGrid.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
            dtGrid.Columns["DATA_YMD"].ColumnName = "YMD";
            //dtGrid.Columns["CM_A * NVL(MGT6_REF_XXX,1)"].ColumnName = "CM_A"; 沒成功撈到資料的話欄位名稱不會變?

            DataTable dtMGD2 = dao40071.d_40071(ymd, is_adj_type); //ids_mgd2
            if (dtMGD2.Rows.Count == 0) {
                MessageDisplay.Error(txtSDate.Text + " ,MGD2無任何資料！");
                return;
            }

            //報表設定條件
            is_kind_list = "";
            //重設gridview
            gcDetail.DataSource = dtGrid;
            DialogResult result = MessageDisplay.Choose("資料已存在，是否重新產製資料,若不重產資料，請按「預覽」!");
            if (result == DialogResult.No) return;

            //產生明細檔
            DataTable dtInput = (DataTable)gcMain.DataSource;
            foreach (DataRow drInput in dtInput.Rows) {
                is_chk = "Y";
                //先把商品為All的值設定好(這段跟PB不同，PB可以直接抓到All，但這邊要把DBNull.Value轉成All)
                if (drInput["KIND_ID"] == DBNull.Value || drInput["KIND_ID"].AsString() == "") drInput["KIND_ID"] = "All";
                //判斷是否有空值
                for (li_col = 0; li_col < dtInput.Columns.Count; li_col++) {
                    if (drInput[li_col] == DBNull.Value || drInput[li_col].ToString() == "") {
                        is_chk = "N";
                    }
                }
                if (is_chk == "N") continue;
                ls_prod_type = drInput["PROD_SUBTYPE"].AsString();
                ls_prod_type_name = drInput["PROD_SUBTYPE_NAME"].AsString();
                ls_param_key = drInput["PARAM_KEY"].AsString();
                ls_abroad = drInput["ABROAD"].AsString() + "%";

                if (drInput["PROD_SEQ_NO"].AsInt() == 1) ls_abroad = " %";
                ls_kind_id = drInput["KIND_ID"].AsString();
                if (ls_kind_id == "All") {
                    ls_kind_id = "%";
                    is_kind_list = is_kind_list + ls_prod_type_name;
                }
                else {
                    ls_prod_type = ls_prod_type + "%";
                    is_kind_list = is_kind_list + ls_kind_id;
                    ls_kind_id = ls_kind_id + "%";
                }
                is_kind_list = is_kind_list + drInput["RATE"].AsDecimal() + "%,"; //不知道要做什麼用

                //百分比換算成數值
                ldc_rate = drInput["RATE"].AsDecimal() / 100;

                //這邊才去讀SP
                DataTable dtTemp = dao40071.d_40071_detail(ymd, ls_prod_type, ls_param_key, ls_abroad, ls_kind_id, "%", ldc_rate);
                dtTemp.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
                dtTemp.Columns["DATA_YMD"].ColumnName = "YMD";
                if (dtTemp.Columns["CM_A*NVL(MGT6_REF_XXX,1)"] != null) dtTemp.Columns["CM_A*NVL(MGT6_REF_XXX,1)"].ColumnName = "CM_A"; //沒撈到值的話欄位名稱不會變，若資料為個股類也不會變

                if (ls_kind_id != "%") {
                    ll_found = dtGrid.Rows.IndexOf(dtGrid.Select("kind_id like'" + ls_kind_id + "'").FirstOrDefault());
                    if (ll_found > -1) dtGrid.Rows[ll_found].Delete();
                }
                //TXF同時要排除MXF
                if (ls_kind_id == "TXF%") {
                    ll_found = dtGrid.Rows.IndexOf(dtGrid.Select("kind_id like 'MXF%'").FirstOrDefault());
                    if (ll_found > -1) dtGrid.Rows[ll_found].Delete();
                }

                //將資料複製到明細表
                //dtGrid = dtTemp.Clone();
                foreach (DataRow drTemp in dtTemp.Rows) {
                    dtGrid.ImportRow(drTemp);
                }
                dtGrid.AcceptChanges();
            }//foreach (DataRow drInput in dtInput.Rows)

            foreach (DataRow drGird in dtGrid.Rows) {
                drGird["ISSUE_BEGIN_YMD"] = txtEffectiveSDate.DateTimeValue.ToString("yyyyMMdd");
                drGird["ISSUE_END_YMD"] = txtEffectiveEDate.DateTimeValue.ToString("yyyyMMdd");
                drGird["IMPL_BEGIN_YMD"] = txtImplSDate.DateTimeValue.ToString("yyyyMMdd");
                drGird["IMPL_END_YMD"] = txtImplEDate.DateTimeValue.ToString("yyyyMMdd");
                drGird["YMD"] = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                drGird["OP_TYPE"] = "I";
            }

            //sort(prod_type A if(prod_subtype<>'S',0,1) A prod_subtype A kind_id A )
            if (dtGrid.Rows.Count != 0) {
                dtGrid = dtGrid.AsEnumerable().OrderBy(x => x.Field<string>("PROD_TYPE"))
                        .ThenBy(x => {
                            if (x.Field<string>("PROD_SUBTYPE") != "S")
                                return 1;
                            else
                                return 2;
                        }).ThenBy(x => x.Field<string>("PROD_SUBTYPE"))
                        .ThenBy(x => x.Field<string>("KIND_ID"))
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