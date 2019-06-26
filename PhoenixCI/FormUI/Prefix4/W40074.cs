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
        protected string isAdjType { get; set; }
        /// <summary>
        /// 前一營業日
        /// </summary>
        protected string preYmd { get; set; }
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
            isAdjType = "4";

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

                int currRow = 0, prodSeq = 0;
                string kindID = "", abType, stockID, prodSubtype;
                //讀取資料
                ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                DataTable dtMGD2 = dao40071.d_40071(ymd, isAdjType);
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
                    if (kindID != dr["MGD2_KIND_ID"].AsString()) {
                        kindID = dr["MGD2_KIND_ID"].AsString();
                        prodSubtype = dr["MGD2_PROD_SUBTYPE"].AsString();
                        currRow = dtGrid.Rows.Count;
                        dtGrid.Rows.Add();
                        dtGrid.Rows[currRow]["PROD_TYPE"] = dr["MGD2_PROD_TYPE"];
                        dtGrid.Rows[currRow]["KIND_ID"] = kindID;
                        dtGrid.Rows[currRow]["STOCK_ID"] = dr["MGD2_STOCK_ID"];
                        dtGrid.Rows[currRow]["PROD_SUBTYPE"] = prodSubtype;
                        dtGrid.Rows[currRow]["PARAM_KEY"] = dr["MGD2_PARAM_KEY"];

                        dtGrid.Rows[currRow]["M_LEVEL"] = dr["MGD2_LEVEL"]; ;
                        dtGrid.Rows[currRow]["CURRENCY_TYPE"] = dr["MGD2_CURRENCY_TYPE"];
                        dtGrid.Rows[currRow]["SEQ_NO"] = dr["MGD2_SEQ_NO"];
                        dtGrid.Rows[currRow]["OSW_GRP"] = dr["MGD2_OSW_GRP"].AsString();
                        dtGrid.Rows[currRow]["AMT_TYPE"] = dr["MGD2_AMT_TYPE"];

                        dtGrid.Rows[currRow]["ADJ_CODE"] = dr["MGD2_ADJ_CODE"];
                        dtGrid.Rows[currRow]["PUB_YMD"] = dr["MGD2_PUB_YMD"];
                        dtGrid.Rows[currRow]["OP_TYPE"] = " "; //預設為空格

                        prodSeq = dao40074.getProd(kindID, prodSubtype);
                    }
                    dtGrid.Rows[currRow]["PROD_SEQ_NO"] = prodSeq;
                    if (dr["MGD2_AB_TYPE"].AsString() == "B") {
                        dtGrid.Rows[currRow]["CM_B"] = dr["MGD2_CM"];
                        dtGrid.Rows[currRow]["MM_B"] = dr["MGD2_MM"];
                        dtGrid.Rows[currRow]["IM_B"] = dr["MGD2_IM"];
                    }
                    else {
                        dtGrid.Rows[currRow]["CM_A"] = dr["MGD2_CM"];
                        dtGrid.Rows[currRow]["MM_A"] = dr["MGD2_MM"];
                        dtGrid.Rows[currRow]["IM_A"] = dr["MGD2_IM"];
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

                string stockID, ymd, kindID, adjTypeName, opType, dbname, stockIDCk;
                string issueBeginYmd, tradeYmd, mocfYmd, nextYmd, level, currencyType;
                int currRow, found, count, row, col, prodSeq;
                decimal ldblRate;
                DateTime ldtWTIME = DateTime.Now;

                DataTable dtGrid = (DataTable)gcMain.DataSource;
                found = dtGrid.Rows.IndexOf(dtGrid.Select("OP_TYPE <> ' '").FirstOrDefault());
                if (found + dtDel.Rows.Count == -1) {
                    MessageDisplay.Warning("沒有變更資料,不需要存檔!");
                    return ResultStatus.Fail;
                }

                DataTable dtMGD2; //ids_mgd2
                DataTable dtMGD2Log = dao40071.d_40071_log(); //ids_old 
                dtMGD2Log.Clear(); //只取schema

                this.ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                issueBeginYmd = this.ymd;
                dtMGD2 = dao40071.d_40071(this.ymd, isAdjType);

                foreach (DataRow dr in dtGrid.Rows) {
                    opType = dr["OP_TYPE"].ToString();
                    stockID = dr["STOCK_ID"].AsString();
                    kindID = dr["KIND_ID"].AsString();
                    level = dr["M_LEVEL"].AsString();

                    //檢查調整後級距為從其高且商品類別為選擇權時，是否有輸入保證金B值
                    if (level == "Z" && dr["PROD_TYPE"].AsString() == "O") {
                        if (dr["CM_B"] == DBNull.Value || dr["MM_B"] == DBNull.Value || dr["IM_B"] == DBNull.Value) {
                            MessageDisplay.Error(stockID + "," + kindID + "的保證金B值未輸入完成");
                            return ResultStatus.FailButNext;
                        }
                    }

                    //檢查有異動的資料
                    if (opType != " ") {
                        //資料修改，將修改前舊資料寫入log
                        if (opType == "U") {
                            dtMGD2.Filter("mgd2_kind_id = '" + kindID + "'");
                            foreach (DataRow drU in dtMGD2.Rows) {
                                currRow = dtMGD2Log.Rows.Count;
                                dtMGD2Log.Rows.Add();
                                for (col = 0; col < dtMGD2.Columns.Count; col++) {
                                    //先取欄位名稱，因為兩張table欄位順序不一致
                                    dbname = dtMGD2.Columns[col].ColumnName;
                                    if (dbname == "CPSORT") continue; //這個欄位是拿來排序用的，故無需複製
                                    dtMGD2Log.Rows[currRow][dbname] = drU[col];
                                }
                                dtMGD2Log.Rows[currRow]["MGD2_L_TYPE"] = "U";
                                dtMGD2Log.Rows[currRow]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                                dtMGD2Log.Rows[currRow]["MGD2_L_TIME"] = ldtWTIME;
                            }
                        }
                        /*******************************************
                           檢查商品與類別是否符合
                        *******************************************/
                        prodSeq = dao40074.getProd(kindID, dr["PROD_SUBTYPE"].AsString());
                        if (prodSeq != dr["PROD_SEQ_NO"].AsInt()) {
                            MessageDisplay.Error(kindID + "與商品類別不符，請確認");
                            return ResultStatus.FailButNext;
                        }
                        /*****************************************
                           檢查商品代號是否存在及相關資料是否正確
                        ******************************************/
                        DataTable dtCheck = dao40074.checkProd(kindID);
                        if (dtCheck.Rows.Count == 0) {
                            MessageDisplay.Error(kindID + "不存在，請重新設定商品代號");
                            return ResultStatus.FailButNext;
                        }
                        count = dtCheck.Rows[0]["LI_COUNT"].AsInt();
                        currencyType = dtCheck.Rows[0]["LS_CURRENCY_TYPE"].AsString();
                        stockIDCk = dtCheck.Rows[0]["LS_STOCK_ID_CK"].AsString();

                        if (count == 0) {
                            MessageDisplay.Error(kindID + "不存在，請重新設定商品代號");
                            return ResultStatus.FailButNext;
                        }
                        else {
                            if (dr["CURRENCY_TYPE"].AsString() != currencyType) {
                                MessageDisplay.Error(kindID + "的幣別設定錯誤，請重新設定填寫");
                                return ResultStatus.FailButNext;
                            }
                            if (dr["prod_subtype"].AsString() == "S" && stockID != stockIDCk) {
                                MessageDisplay.Error(kindID + "的股票代號設定錯誤，請重新設定填寫");
                                return ResultStatus.FailButNext;
                            }
                        }

                        /******************************************
                           確認商品是否在同一交易日不同情境下設定過
                        ******************************************/
                        DataTable dtSet = dao40071.IsSetOnSameDay(kindID, this.ymd, isAdjType);
                        if (dtSet.Rows.Count == 0) {
                            MessageDisplay.Error("MGD2 " + kindID + " 無任何資料！");
                            return ResultStatus.FailButNext;
                        }
                        count = dtSet.Rows[0]["LI_COUNT"].AsInt();
                        adjTypeName = dtSet.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        if (count > 0) {
                            MessageDisplay.Error(kindID + ",交易日(" + this.ymd + ")在" + adjTypeName + "已有資料");
                            return ResultStatus.FailButNext;
                        }
                        /*********************************
                        確認商品是否在同一生效日區間設定過
                        生效起日若與生效迄日相同，不重疊
                        ex: 10/11的至10/31一般交易時段結束止，10/30的從10/31一般交易時段結束後始>>應不重疊
                        *************************************/
                        dtSet = dao40071.IsSetInSameSession(kindID, this.ymd, issueBeginYmd);
                        count = dtSet.Rows[0]["LI_COUNT"].AsInt();
                        adjTypeName = dtSet.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        tradeYmd = dtSet.Rows[0]["LS_TRADE_YMD"].AsString();
                        if (count > 0) {
                            MessageDisplay.Error(kindID + "," + adjTypeName + ",交易日(" + tradeYmd + ")在同一生效日區間內已有資料");
                            return ResultStatus.FailButNext;
                        }

                    }//if (ls_op_type != " ")
                }//foreach (DataRow dr in dtGrid.Rows)

                //把刪除的資料寫進log
                foreach (DataRow drDel in dtDel.Rows) {
                    kindID = drDel["KIND_ID"].AsString();
                    dtMGD2.Filter("mgd2_kind_id = '" + kindID + "'");
                    foreach (DataRow drD in dtMGD2.Rows) {
                        currRow = dtMGD2Log.Rows.Count;
                        dtMGD2Log.Rows.Add();
                        for (col = 0; col < dtMGD2.Columns.Count; col++) {
                            //先取欄位名稱，因為兩張table欄位順序不一致
                            dbname = dtMGD2.Columns[col].ColumnName;
                            if (dbname == "CPSORT") continue; //這個欄位是拿來排序用的，故無需複製
                            dtMGD2Log.Rows[currRow][dbname] = drD[col];
                        }
                        dtMGD2Log.Rows[currRow]["MGD2_L_TYPE"] = "D";
                        dtMGD2Log.Rows[currRow]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                        dtMGD2Log.Rows[currRow]["MGD2_L_TIME"] = ldtWTIME;
                    }
                }
                #endregion

                string prodType;
                DataTable dtTemp = dao40072.d_40072(); //ids_tmp

                foreach (DataRow dr in dtGrid.Rows) {
                    opType = dr["OP_TYPE"].ToString();
                    //只更新有異動的資料
                    if (opType != " ") {
                        kindID = dr["KIND_ID"].AsString();
                        stockID = dr["KIND_ID"].AsString();

                        //刪除已存在資料
                        if (daoMGD2.DeleteMGD2(this.ymd, isAdjType, stockID, kindID) < 0) {
                            MessageDisplay.Error("MGD2資料刪除失敗");
                            return ResultStatus.FailButNext;
                        }

                        currRow = dtTemp.Rows.Count;
                        prodType = dr["PROD_TYPE"].AsString();
                        dtTemp.Rows.Add();
                        dtTemp.Rows[currRow]["MGD2_YMD"] = this.ymd;
                        dtTemp.Rows[currRow]["MGD2_PROD_TYPE"] = prodType;
                        dtTemp.Rows[currRow]["MGD2_KIND_ID"] = kindID;
                        dtTemp.Rows[currRow]["MGD2_STOCK_ID"] = stockID;
                        dtTemp.Rows[currRow]["MGD2_ADJ_TYPE"] = isAdjType;

                        dtTemp.Rows[currRow]["MGD2_ADJ_CODE"] = dr["ADJ_CODE"];
                        dtTemp.Rows[currRow]["MGD2_PUB_YMD"] = dr["PUB_YMD"];
                        dtTemp.Rows[currRow]["MGD2_ISSUE_BEGIN_YMD"] = issueBeginYmd;
                        dtTemp.Rows[currRow]["MGD2_PROD_SUBTYPE"] = dr["PROD_SUBTYPE"].AsString();
                        dtTemp.Rows[currRow]["MGD2_PARAM_KEY"] = dr["PARAM_KEY"];

                        dtTemp.Rows[currRow]["MGD2_CUR_CM"] = 0;
                        dtTemp.Rows[currRow]["MGD2_CUR_MM"] = 0;
                        dtTemp.Rows[currRow]["MGD2_CUR_IM"] = 0;
                        dtTemp.Rows[currRow]["MGD2_CUR_LEVEL"] = 0;
                        dtTemp.Rows[currRow]["MGD2_CM"] = dr["CM_A"];

                        dtTemp.Rows[currRow]["MGD2_MM"] = dr["MM_A"];
                        dtTemp.Rows[currRow]["MGD2_IM"] = dr["IM_A"];
                        dtTemp.Rows[currRow]["MGD2_LEVEL"] = dr["M_LEVEL"];
                        dtTemp.Rows[currRow]["MGD2_CURRENCY_TYPE"] = dr["CURRENCY_TYPE"];
                        dtTemp.Rows[currRow]["MGD2_SEQ_NO"] = dr["SEQ_NO"];

                        dtTemp.Rows[currRow]["MGD2_OSW_GRP"] = dr["OSW_GRP"];
                        dtTemp.Rows[currRow]["MGD2_AMT_TYPE"] = dr["AMT_TYPE"];
                        dtTemp.Rows[currRow]["MGD2_W_TIME"] = ldtWTIME;
                        dtTemp.Rows[currRow]["MGD2_W_USER_ID"] = GlobalInfo.USER_ID;

                        /******************************
                              AB TYTPE：	-期貨
                                          A選擇權A值
                                          B選擇權B值
                        *******************************/
                        if (prodType == "F") {
                            dtTemp.Rows[currRow]["MGD2_AB_TYPE"] = "-";
                        }
                        else {
                            dtTemp.Rows[currRow]["MGD2_AB_TYPE"] = "A";
                            //複製一筆一樣的，AB Type分開存
                            dtTemp.ImportRow(dtTemp.Rows[currRow]);
                            //dtTemp.Rows.Add(dtTemp.Rows[ii_curr_row]);//會跳錯
                            currRow = dtTemp.Rows.Count - 1;
                            dtTemp.Rows[currRow]["MGD2_AB_TYPE"] = "B";
                            dtTemp.Rows[currRow]["MGD2_CM"] = dr["CM_B"];
                            dtTemp.Rows[currRow]["MGD2_MM"] = dr["MM_B"];
                            dtTemp.Rows[currRow]["MGD2_IM"] = dr["IM_B"];
                        }
                    }//if (ls_op_type != " ")
                }//foreach (DataRow dr in dtGrid.Rows)

                //刪除資料
                foreach (DataRow drDel in dtDel.Rows) {
                    kindID = drDel["KIND_ID"].AsString();
                    stockID = drDel["STOCK_ID"].AsString();
                    if (daoMGD2.DeleteMGD2(this.ymd, isAdjType, stockID, kindID) < 0) {
                        MessageDisplay.Error("MGD2資料刪除失敗");
                        return ResultStatus.FailButNext;
                    }
                }

                //Update DB
                //ids_tmp.update()
                if (dtTemp.Rows.Count > 0) {
                    ResultData myResultData = daoMGD2.UpdateMGD2(dtTemp);
                    if (myResultData.Status == ResultStatus.Fail) {
                        MessageDisplay.Error("更新資料庫MGD2錯誤! ");
                        return ResultStatus.FailButNext;
                    }
                }
                //ids_old.update()
                if (dtMGD2Log.Rows.Count > 0) {
                    ResultData myResultData = daoMGD2L.UpdateMGD2L(dtMGD2Log);
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
            string prodType, prodSubtype, paramKey, abroad;
            DataTable dtKind = new DataTable();
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            if (e.Column.FieldName == "KIND_ID") {
                prodSubtype = gv.GetRowCellValue(e.RowHandle, "PROD_SUBTYPE").AsString();
                paramKey = gv.GetRowCellValue(e.RowHandle, "CND_PARAM_KEY").AsString();
                abroad = gv.GetRowCellValue(e.RowHandle, "ABROAD") == null ? null :
                            gv.GetRowCellValue(e.RowHandle, "ABROAD").ToString();
                if (prodSubtype == "S") {
                    if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "D") {
                        dtKind = dao40071.dddw_pdk_kind_id_40071(ymd, paramKey);
                        kindIDLookUpEdit.SetColumnLookUp(dtKind, "KIND_ID", "KIND_ID", TextEditStyles.Standard, null);
                        gcMain.RepositoryItems.Add(kindIDLookUpEdit);
                        e.RepositoryItem = kindIDLookUpEdit;
                    }
                    if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "Y") {
                        dtKind = dao40074.dddw_pdk_kind_id_40074(paramKey);
                        kindIDLookUpEdit.SetColumnLookUp(dtKind, "KIND_ID", "KIND_ID", TextEditStyles.Standard, null);
                        gcMain.RepositoryItems.Add(kindIDLookUpEdit);
                        e.RepositoryItem = kindIDLookUpEdit;
                    }
                }
                else {
                    dtKind = dao40071.dddw_mgt2_kind(prodSubtype + "%", abroad);
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
                prodType = gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString();
                if (prodType == "F") e.RepositoryItem = fLevelLookUpEdit;
                if (prodType == "O") e.RepositoryItem = oLevelLookUpEdit;
            }
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            GridView gv = sender as GridView;
            string amtType = gv.GetRowCellValue(e.RowHandle, gv.Columns["AMT_TYPE"]).AsString();
            string opType = gv.GetRowCellValue(e.RowHandle, gv.Columns["OP_TYPE"]) == null ? "I" :
                             gv.GetRowCellValue(e.RowHandle, gv.Columns["OP_TYPE"]).ToString();
            switch (e.Column.FieldName) {
                case "KIND_ID":
                case "STOCK_ID":
                case "M_LEVEL":
                case "PROD_SEQ_NO":
                    e.Appearance.BackColor = opType == "I" ? Color.White : Color.FromArgb(224, 224, 224);
                    e.Column.OptionsColumn.AllowEdit = opType == "I" ? true : false;
                    break;
                case "CM_A":
                case "CM_B":
                case "MM_A":
                case "MM_B":
                case "IM_A":
                case "IM_B":
                    //e.Column.DisplayFormat.FormatString = amt_type == "P" ? "{0:0.####%}" : "#,###";
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
            string prodType = gv.GetRowCellValue(gv.FocusedRowHandle, "PROD_TYPE").ToString();
            string prodSubtype = gv.GetRowCellValue(gv.FocusedRowHandle, "PROD_SUBTYPE").ToString();
            string cndParamKey = gv.GetRowCellValue(gv.FocusedRowHandle, "CND_PARAM_KEY").ToString();
            //string op_type = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["OP_TYPE"]) == null ? "I" :
            //     gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["OP_TYPE"]).ToString();
            if (gv.FocusedColumn.Name == "CM_B" ||
                gv.FocusedColumn.Name == "MM_B" ||
                gv.FocusedColumn.Name == "IM_B") {
                e.Cancel = prodType == "F" ? true : false;
            }
            if (gv.FocusedColumn.Name == "STOCK_ID" ||
                gv.FocusedColumn.Name == "M_LEVEL") {
                e.Cancel = prodSubtype != "S" ? true : false;
                if (cndParamKey.IndexOf("ST%") >= 0) e.Cancel = false;
            }
            //if (gv.FocusedColumn.Name == "PROD_SEQ_NO") e.Cancel = op_type == "I" ? false : true;
        }

        private void gvMain_CellValueChanging(object sender, CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            string prodSubtype, prodType, paramKey, abroad, kindID;
            int found = -1;
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            paramKey = gv.GetRowCellValue(e.RowHandle, "CND_PARAM_KEY").AsString();
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
                prodSubtype = gv.GetRowCellValue(e.RowHandle, "PROD_SUBTYPE").AsString();
                abroad = gv.GetRowCellValue(e.RowHandle, "ABROAD").ToString();
                kindID = gv.GetRowCellValue(e.RowHandle, "KIND_ID").AsString();
                if (kindID != "") {
                    DataTable dtKindCheck = new DataTable();
                    if (prodSubtype == "S") {
                        if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "D") {
                            dtKindCheck = dao40071.dddw_pdk_kind_id_40071(ymd, paramKey);
                            found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + kindID + "'").FirstOrDefault());

                        }
                        if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "Y") {
                            dtKindCheck = dao40074.dddw_pdk_kind_id_40074(paramKey);
                            found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + kindID + "'").FirstOrDefault());
                        }
                    }
                    else {
                        dtKindCheck = dao40071.dddw_mgt2_kind(prodSubtype + "%", abroad);
                        found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + kindID + "'").FirstOrDefault());
                    }
                    if (found == -1) {
                        MessageDisplay.Error("商品代號輸入錯誤");
                        gv.SetRowCellValue(e.RowHandle, "KIND_ID", "");
                    }
                    else {
                        gv.SetRowCellValue(e.RowHandle, "PROD_TYPE", dtKindCheck.Rows[found]["PROD_TYPE"]);
                        gv.SetRowCellValue(e.RowHandle, "PARAM_KEY", dtKindCheck.Rows[found]["PARAM_KEY"].AsString());
                        gv.SetRowCellValue(e.RowHandle, "SEQ_NO", dtKindCheck.Rows[found]["SEQ_NO"]);
                    }
                }
            }
            if (e.Column.Name == "ADJ_CODE") {
                //改變調整狀態(上市/下市)時，//同步公布日期
                if (e.Value.AsString() == "Y") gv.SetRowCellValue(e.RowHandle, "PUB_YMD", preYmd);
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
            GridView gv = sender as GridView;
            string prodSubtype, prodType, paramKey, abroad, kindID;
            int found = -1;
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            paramKey = gv.GetRowCellValue(e.RowHandle, "CND_PARAM_KEY").AsString();
            if (e.Column.Name == "KIND_ID") {
                //商品那欄除了下拉選單已外也可手動key入，key入後會檢查是否正確
                //若kind_id值為空(即預設值)，則視為使用者尚未填寫，不在此進行檢核，否則會進入無限迴圈
                //若使用者未輸入kind_id逕行存檔，存檔時仍會再判斷一次
                prodSubtype = gv.GetRowCellValue(e.RowHandle, "PROD_SUBTYPE").AsString();
                abroad = gv.GetRowCellValue(e.RowHandle, "ABROAD").ToString();
                kindID = gv.GetRowCellValue(e.RowHandle, "KIND_ID").AsString();
                if (kindID != "") {
                    DataTable dtKindCheck = new DataTable();
                    if (prodSubtype == "S") {
                        if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "D") {
                            dtKindCheck = dao40071.dddw_pdk_kind_id_40071(ymd, paramKey);
                            found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + kindID + "'").FirstOrDefault());

                        }
                        if (gv.GetRowCellValue(e.RowHandle, "ADJ_CODE").AsString() == "Y") {
                            dtKindCheck = dao40074.dddw_pdk_kind_id_40074(paramKey);
                            found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + kindID + "'").FirstOrDefault());
                        }
                    }
                    else {
                        dtKindCheck = dao40071.dddw_mgt2_kind(prodSubtype + "%", abroad);
                        found = dtKindCheck.Rows.IndexOf(dtKindCheck.Select("kind_id = '" + kindID + "'").FirstOrDefault());
                    }
                    if (found == -1) {
                        MessageDisplay.Error("商品代號輸入錯誤");
                        gv.SetRowCellValue(e.RowHandle, "KIND_ID", "");
                    }
                    else {
                        gv.SetRowCellValue(e.RowHandle, "PROD_TYPE", dtKindCheck.Rows[found]["PROD_TYPE"]);
                        gv.SetRowCellValue(e.RowHandle, "PARAM_KEY", dtKindCheck.Rows[found]["PARAM_KEY"].AsString());
                        gv.SetRowCellValue(e.RowHandle, "SEQ_NO", dtKindCheck.Rows[found]["SEQ_NO"]);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 改變生效日期時，同步公布日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSDate_EditValueChanged(object sender, EventArgs e) {
            if (txtSDate.Text.Length < 10) return;//防止還沒輸入完就觸發事件
            string mocfYmd;
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");

            //交易日+1個月
            mocfYmd = PbFunc.relativedate(txtSDate.DateTimeValue, -30).ToString("yyyyMMdd");
            /*前一營業日*/
            preYmd = daoMOCF.GetPrevTradeDay(ymd, mocfYmd);
            //同步公布日期	 

            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();
            DataTable dtGrid = (DataTable)gcMain.DataSource;
            foreach (DataRow dr in dtGrid.Rows) {
                if (dr["ADJ_CODE"].AsString() == "Y") dr["PUB_YMD"] = preYmd;
                if (dr["ADJ_CODE"].AsString() == "D") dr["PUB_YMD"] = ymd;
            }
        }

        private void gvMain_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e) {
            ColumnView view = sender as ColumnView;
            if ((e.Column.FieldName == "CM_A" ||
                 e.Column.FieldName == "CM_B" ||
                 e.Column.FieldName == "MM_A" ||
                 e.Column.FieldName == "MM_B" ||
                 e.Column.FieldName == "IM_A" ||
                 e.Column.FieldName == "IM_B") && e.ListSourceRowIndex != DevExpress.XtraGrid.GridControl.InvalidRowHandle) {
                string amtType = view.GetListSourceRowCellValue(e.ListSourceRowIndex, "AMT_TYPE").AsString();
                decimal value = e.Value.AsDecimal();
                switch (amtType) {
                    case "P": e.DisplayText = string.Format("{0:0.###%}", value); break;
                    default: e.DisplayText = string.Format("{0:#,###}", value); break;
                }
            }
        }
    }
}