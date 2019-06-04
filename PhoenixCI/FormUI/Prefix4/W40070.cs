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
using BusinessObjects.Enums;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using Common;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils;
using BusinessObjects;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using BaseGround.Report;

/// <summary>
/// Lukas, 2019/4/17
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 40070
    /// </summary>
    public partial class W40070 : FormParent {

        protected string nullYmd { get; set; }
        private D40070 dao40070;
        private D40071 dao40071;
        private MGD2 daoMGD2;
        private MGD2L daoMGD2L;
        private DataTable dtTemp; //ids_tmp

        public W40070(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            gvMain.AppearancePrint.BandPanel.Font = new Font("Microsoft YaHei", 10);
            gvMain.AppearancePrint.BandPanel.TextOptions.WordWrap = WordWrap.Wrap;
        }

        protected override ResultStatus Open() {
            base.Open();
            //全域變數
            nullYmd = null;
            //日期
            txtSDate.DateTimeValue = DateTime.Now;
            //先隨便給個日期
            txtDateG1.Text = "1901/01/01";
            txtDateG5.Text = "1901/01/01";
            txtDateG7.Text = "1901/01/01";

            #region DropDownList
            //設定調整商品條件下拉選單
            List<LookupItem> modelType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "S", DisplayMember = "SMA達調整標準"},
                                        new LookupItem() { ValueMember = "a", DisplayMember = "任一model達調整標準" },
                                        new LookupItem() { ValueMember = "%", DisplayMember = "全部商品" }};
            Extension.SetDataTable(ddlModel, modelType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");
            ddlModel.EditValue = "S";

            //設定依條件選擇狀態的下拉選單
            List<LookupItem> adjustType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "none", DisplayMember = "全取消"},
                                        new LookupItem() { ValueMember = "indes", DisplayMember = "全選指數類" },
                                        new LookupItem() { ValueMember = "all", DisplayMember = "全選"},
                                        new LookupItem() { ValueMember = "ETF", DisplayMember = "全選ETF" },
                                        new LookupItem() { ValueMember = "1", DisplayMember = "全選Group1"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "全選Group2" },
                                        new LookupItem() { ValueMember = "3", DisplayMember = "全選Group3" }};
            Extension.SetDataTable(ddlAdjust, adjustType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");
            ddlAdjust.EditValue = "none";
            #endregion

            //設定群組
            GRP_NAME.GroupIndex = 0;
            GRP_NAME.Caption = "群組";

            #region RadioGroup
            //設定結算保證金調整金額項目RadioGroup
            // creating and initializing the radio group items 
            RadioGroupItem item1 = new RadioGroupItem();
            item1.Description = "SMA";
            item1.Value = "S";
            RadioGroupItem item2 = new RadioGroupItem();
            item2.Description = "EWMA";
            item2.Value = "E";
            RadioGroupItem item3 = new RadioGroupItem();
            item3.Description = "MaxVol";
            item3.Value = "M";
            RadioGroupItem item4 = new RadioGroupItem();
            item4.Description = "使用者自訂";
            item4.Value = "U";

            RepositoryItemRadioGroup repositoryItemRadioGroup = new RepositoryItemRadioGroup();
            repositoryItemRadioGroup.Items.Add(item1);
            repositoryItemRadioGroup.Items.Add(item2);
            repositoryItemRadioGroup.Items.Add(item3);
            repositoryItemRadioGroup.Items.Add(item4);
            repositoryItemRadioGroup.Columns = 4;
            ADJ_RSN.ColumnEdit = repositoryItemRadioGroup;
            //ADJ_RSN.ColumnEdit.Appearance.TextOptions.HAlignment = HorzAlignment.Near;

            //不處理/觀察/調整 RadioGroup
            RadioGroupItem item5 = new RadioGroupItem();
            item5.Description = "　　";
            item5.Value = "N";
            RadioGroupItem item6 = new RadioGroupItem();
            item6.Description = "　　";
            item6.Value = " ";
            RadioGroupItem item7 = new RadioGroupItem();
            item7.Description = "　　";
            item7.Value = "Y";

            RepositoryItemRadioGroup repositoryItemRadioGroup2 = new RepositoryItemRadioGroup();
            repositoryItemRadioGroup2.Items.Add(item5);
            repositoryItemRadioGroup2.Items.Add(item6);
            repositoryItemRadioGroup2.Items.Add(item7);
            repositoryItemRadioGroup2.Columns = 3;
            ADJ_CODE.ColumnEdit = repositoryItemRadioGroup2;
            ADJ_CODE.ColumnEdit.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            #endregion

#if DEBUG
            txtSDate.EditValue = "2019/05/03";
#endif

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

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

                //讀取資料
                dao40070 = new D40070();
                DataTable dt40070 = dao40070.d_40070_scrn(txtSDate.DateTimeValue.ToString("yyyyMMdd"), ddlModel.EditValue.AsString());
                if (dt40070.Rows.Count == 0) {
                    MessageDisplay.Warning("無任何資料！");
                    return ResultStatus.Fail;
                }
                //排序
                dt40070 = dt40070.Sort("OSW_GRP, SEQ_NO, PROD_TYPE, KIND_ID");
                //複製
                //dw_1.RowsCopy(1, dw_1.rowcount(), primary!, ids_tmp, 1, primary!)
                dtTemp = dt40070.Copy();
                //過濾
                DataView dv = dt40070.AsDataView();
                dv.RowFilter = " ab_type in ('-','A')";
                DataTable dtFiltered = dv.ToTable();

                gcMain.DataSource = dtFiltered;
                gcMain.Refresh();
                //預設展開群組
                gvMain.ExpandAllGroups();

                //設定三個Group的生效日期
                string validDateG1, validDateG5, validDateG7;
                int found;
                //Group1
                found = dtFiltered.Rows.IndexOf(dtFiltered.Select("osw_grp='1' and issue_begin_ymd is not null ").FirstOrDefault());
                if (found > -1) {
                    txtDateG1.DateTimeValue = dtFiltered.Rows[found]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd");
                }
                else {
                    txtDateG1.DateTimeValue = PbFunc.f_get_ocf_next_n_day(txtSDate.DateTimeValue, 1);
                }
                validDateG1 = txtDateG1.Text;
                //Group2
                found = dtFiltered.Rows.IndexOf(dtFiltered.Select("osw_grp='5' and issue_begin_ymd is not null ").FirstOrDefault());
                if (found > -1) {
                    txtDateG5.DateTimeValue = dtFiltered.Rows[found]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd");
                }
                else {
                    txtDateG5.DateTimeValue = PbFunc.f_get_ocf_next_n_day(txtSDate.DateTimeValue, 2);
                }
                validDateG5 = txtDateG5.Text;
                //Group2
                found = dtFiltered.Rows.IndexOf(dtFiltered.Select("osw_grp='7' and issue_begin_ymd is not null ").FirstOrDefault());
                if (found > -1) {
                    txtDateG7.DateTimeValue = dtFiltered.Rows[found]["ISSUE_BEGIN_YMD"].AsDateTime("yyyyMMdd");
                }
                else {
                    txtDateG7.DateTimeValue = PbFunc.f_get_ocf_next_n_day(txtSDate.DateTimeValue, 2);
                }
                validDateG7 = txtDateG7.Text;
            }
            catch (Exception ex) {
                MessageDisplay.Error("讀取錯誤");
                throw ex;
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            try {
                if (gvMain.RowCount == 0) {
                    MessageDisplay.Info("沒有變更資料,不需要存檔!");
                    return ResultStatus.Fail;
                }
                dao40071 = new D40071();
                daoMGD2 = new MGD2();
                daoMGD2L = new MGD2L();
                #region ue_save_before
                gvMain.CloseEditor();
                gvMain.UpdateCurrentRow();
                string ymd, issueBeginYmd, kindID, adjTypeName, tradeYmd, adjRsn, adjType;
                decimal cm = 0, curCm;
                int count;
                /***************************
		          調整類型:  
						0一般
						1長假
						2處置股票
						3股票
                ****************************/
                adjType = "0";

                ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");

                DataTable dtGrid = (DataTable)gcMain.DataSource;
                DataView dv = dtGrid.AsDataView();
                dv.RowFilter = " ab_type in ('-','A')";
                DataTable dtFiltered = dv.ToTable(); //dw_1

                cbxCodeY.Checked = true;
                cbxCodeN.Checked = true;
                cbxCode.Checked = true;

                foreach (DataRow dr in dtFiltered.Rows) {
                    kindID = dr["KIND_ID"].AsString();
                    issueBeginYmd = dr["ISSUE_BEGIN_YMD"].AsString();
                    if (dr["ADJ_CODE"].AsString() == "Y") {
                        /******************************************
                           確認商品是否在同一交易日不同情境下設定過
                        ******************************************/
                        DataTable dtCheck = daoMGD2.IsProdSetOnSameDay(kindID, ymd, adjType);
                        count = dtCheck.Rows[0]["LI_COUNT"].AsInt();
                        adjTypeName = dtCheck.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        if (count > 0) {
                            MessageDisplay.Error(kindID + ",交易日(" + ymd + ")在" + adjTypeName + "已有資料");
                            return ResultStatus.Fail;
                        }
                        /*********************************
                        確認商品是否在同一生效日區間設定過
                        生效起日若與生效迄日相同，不重疊
                        ex: 10/11的至10/31一般交易時段結束止，10/30的從10/31一般交易時段結束後始>>應不重疊
                        *************************************/
                        dtCheck = daoMGD2.IsProdSetInSameInterval(kindID, ymd, issueBeginYmd);
                        count = dtCheck.Rows[0]["LI_COUNT"].AsInt();
                        adjTypeName = dtCheck.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        tradeYmd = dtCheck.Rows[0]["LS_TRADE_YMD"].AsString();
                        if (count > 0) {
                            MessageDisplay.Error(kindID + "," + adjTypeName + ",交易日(" + tradeYmd + ")在同一生效日區間內已有資料");
                            return ResultStatus.Fail;
                        }
                        /**************************************
                        判斷調整前後值不同，相同則警示且無法存檔
                        **************************************/
                        adjRsn = dr["ADJ_RSN"].AsString();
                        curCm = dr["CUR_CM"].AsDecimal();
                        if (adjRsn == "S") cm = dr["SMA_CM"].AsDecimal();
                        if (adjRsn == "E") cm = dr["EWMA_CM"].AsDecimal();
                        if (adjRsn == "M") cm = dr["MAXV_CM"].AsDecimal();
                        if (adjRsn == "U") {
                            cm = dr["USER_CM"].AsDecimal();
                            if (cm == 0) {
                                MessageDisplay.Error(kindID + ",請輸入保證金金額");
                                return ResultStatus.Fail;
                            }
                        }
                        if (cm == 0) {
                            MessageDisplay.Error(kindID + ",保證金計算值為空，請選擇其他模型");
                            return ResultStatus.Fail;
                        }
                        if (cm == curCm) {
                            MessageDisplay.Error(kindID + ",調整前後保證金一致，請重新輸入");
                            return ResultStatus.Fail;
                        }
                    }
                }
                #endregion

                DateTime wTime, date;
                date = txtSDate.DateTimeValue;
                int found, colIndex, found2, currRow;
                string dbname;
                decimal curMM = 0, curIM = 0, MM = 0, IM = 0, rate = 0;

                wTime = DateTime.Now;

                DataTable dtMGD2 = dao40071.d_40071(ymd, adjType); //ids_mgd2
                DataTable dtMGD2Log = dao40071.d_40071_log(); //ids_old 
                dtMGD2Log.Clear(); //只取schema
                //再產生一張空的 d_40071 table
                DataTable dtEmpty = dao40071.d_40071(ymd, adjType); //dw_3
                dtEmpty.Clear();
                dtEmpty.Columns.Remove(dtEmpty.Columns["CPSORT"]);//刪除排序用的運算欄位

                foreach (DataRow dr in dtFiltered.Rows) {
                    kindID = dr["KIND_ID"].AsString();
                    issueBeginYmd = dr["ISSUE_BEGIN_YMD"].AsString();
                    adjRsn = dr["ADJ_RSN"].AsString();

                    dv = dtMGD2.AsDataView();
                    dv.RowFilter = "mgd2_kind_id = '" + kindID + "'";
                    dtMGD2 = dv.ToTable();

                    if (dtMGD2.Rows.Count > 0) {
                        foreach (DataRow drMGD2 in dtMGD2.Rows) {
                            currRow = dtMGD2Log.Rows.Count;
                            dtMGD2Log.Rows.Add();
                            for (colIndex = 0; colIndex < dtMGD2.Columns.Count; colIndex++) {
                                //先取欄位名稱，因為兩張table欄位順序不一致
                                dbname = dtMGD2.Columns[colIndex].ColumnName;
                                if (dbname == "CPSORT") continue; //這個欄位是拿來排序用的，故無需複製
                                dtMGD2Log.Rows[currRow][dbname] = drMGD2[colIndex];
                            }
                            if (dr["ADJ_CODE"].AsString() == "N") {
                                dtMGD2Log.Rows[currRow]["MGD2_L_TYPE"] = "D";
                            }
                            else {
                                dtMGD2Log.Rows[currRow]["MGD2_L_TYPE"] = "U";
                            }
                            dtMGD2Log.Rows[currRow]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                            dtMGD2Log.Rows[currRow]["MGD2_L_TIME"] = wTime;
                        }//foreach (DataRow drMGD2 in dtMGD2.Rows)

                        //刪除已存在資料
                        if (daoMGD2.DeleteMGD2(ymd, adjType, kindID) < 0) {
                            MessageDisplay.Error("MGD2資料刪除失敗");
                            return ResultStatus.Fail;
                        }
                    }
                    //判斷是否重新塞入新資料
                    count = daoMGD2.IsInsertNeeded(ymd, adjType, kindID);
                    if (count == 0) {
                        dtEmpty.Rows.Add();
                        found = dtEmpty.Rows.Count - 1;
                        dtEmpty.Rows[found]["MGD2_YMD"] = ymd;
                        dtEmpty.Rows[found]["MGD2_PROD_TYPE"] = dr["PROD_TYPE"];
                        dtEmpty.Rows[found]["MGD2_KIND_ID"] = kindID;
                        dtEmpty.Rows[found]["MGD2_ADJ_TYPE"] = adjType;

                        curCm = dr["CUR_CM"].AsDecimal();
                        curMM = dr["CUR_MM"].AsDecimal();
                        curIM = dr["CUR_IM"].AsDecimal();

                        dtEmpty.Rows[found]["MGD2_CUR_CM"] = curCm;
                        dtEmpty.Rows[found]["MGD2_CUR_MM"] = curMM;
                        dtEmpty.Rows[found]["MGD2_CUR_IM"] = curIM;

                        if (adjRsn == "S") {
                            rate = dr["SMA_ADJ_RATE"].AsDecimal();
                            cm = dr["SMA_CM"].AsDecimal();
                            MM = dr["SMA_MM"].AsDecimal();
                            IM = dr["SMA_IM"].AsDecimal();
                        }
                        if (adjRsn == "E") {
                            rate = dr["EWMA_ADJ_RATE"].AsDecimal();
                            cm = dr["EWMA_CM"].AsDecimal();
                            MM = dr["EWMA_MM"].AsDecimal();
                            IM = dr["EWMA_IM"].AsDecimal();
                        }
                        if (adjRsn == "M") {
                            rate = dr["MAXV_ADJ_RATE"].AsDecimal();
                            cm = dr["MAXV_CM"].AsDecimal();
                            MM = dr["MAXV_MM"].AsDecimal();
                            IM = dr["MAXV_IM"].AsDecimal();
                        }
                        if (adjRsn == "M") {
                            if (kindID == "MXF") {
                                found2 = dtFiltered.Rows.IndexOf(dtFiltered.Select("kind_id = 'TXF'").FirstOrDefault());
                                cm = dtFiltered.Rows[found2]["USER_CM"].AsDecimal();
                                MM = dao40070.GetMarginVal("TXF", cm, 0, "MM");
                                MM = dao40070.GetMarginVal(kindID, MM, 0, "MTX_MM");
                                IM = dao40070.GetMarginVal("TXF", cm, 0, "IM");
                                IM = dao40070.GetMarginVal(kindID, IM, 0, "MTX_IM");
                                cm = dr["USER_CM"].AsDecimal();
                            }
                            else {
                                cm = dr["USER_CM"].AsDecimal();
                                MM = dao40070.GetMarginVal(kindID, cm, 0, "MM");
                                IM = dao40070.GetMarginVal(kindID, cm, 0, "IM");
                            }
                            rate = dao40070.GetMarginVal(kindID, cm, curCm, "ADJ");
                        }

                        dtEmpty.Rows[found]["MGD2_ADJ_RATE"] = rate;
                        dtEmpty.Rows[found]["MGD2_CM"] = cm;
                        dtEmpty.Rows[found]["MGD2_MM"] = MM;
                        dtEmpty.Rows[found]["MGD2_IM"] = IM;
                        dtEmpty.Rows[found]["MGD2_ADJ_RSN"] = adjRsn;

                        dtEmpty.Rows[found]["MGD2_ADJ_CODE"] = dr["ADJ_CODE"];
                        dtEmpty.Rows[found]["MGD2_ISSUE_BEGIN_YMD"] = issueBeginYmd;
                        dtEmpty.Rows[found]["MGD2_STOCK_ID"] = " ";
                        dtEmpty.Rows[found]["MGD2_PROD_SUBTYPE"] = dr["PROD_SUBTYPE"];
                        dtEmpty.Rows[found]["MGD2_PARAM_KEY"] = dr["PARAM_KEY"];

                        dtEmpty.Rows[found]["MGD2_AB_TYPE"] = dr["AB_TYPE"];
                        dtEmpty.Rows[found]["MGD2_CURRENCY_TYPE"] = dr["CURRENCY_TYPE"];
                        dtEmpty.Rows[found]["MGD2_SEQ_NO"] = dr["SEQ_NO"];
                        dtEmpty.Rows[found]["MGD2_OSW_GRP"] = dr["OSW_GRP"];
                        dtEmpty.Rows[found]["MGD2_AMT_TYPE"] = dr["AMT_TYPE"];

                        dtEmpty.Rows[found]["MGD2_W_TIME"] = wTime;
                        dtEmpty.Rows[found]["MGD2_W_USER_ID"] = GlobalInfo.USER_ID;

                        //type 有AB值分兩筆存
                        if (dr["AB_TYPE"].AsString() == "A") {
                            //DataRow drA = dtEmpty.Rows[found];
                            //dtEmpty.Rows.InsertAt(drA, found - 1);
                            dtEmpty.ImportRow(dtEmpty.Rows[found]);
                            found2 = dtTemp.Rows.IndexOf(dtTemp.Select("kind_id = '" + kindID + "' and ab_type = 'B'").FirstOrDefault());
                            if (found2 < 0) {
                                MessageDisplay.Error(kindID + "無保證金B值資料!");
                                return ResultStatus.Fail;
                            }

                            found = dtEmpty.Rows.Count - 1;
                            dtEmpty.Rows[found]["MGD2_AB_TYPE"] = "B";
                            dtEmpty.Rows[found]["MGD2_CUR_CM"] = dtTemp.Rows[found2]["CUR_CM"];
                            dtEmpty.Rows[found]["MGD2_CUR_MM"] = dtTemp.Rows[found2]["CUR_MM"];
                            dtEmpty.Rows[found]["MGD2_CUR_IM"] = dtTemp.Rows[found2]["CUR_IM"];

                            if (adjRsn == "S") {
                                dtEmpty.Rows[found]["MGD2_CM"] = dtTemp.Rows[found2]["SMA_CM"];
                                dtEmpty.Rows[found]["MGD2_MM"] = dtTemp.Rows[found2]["SMA_MM"];
                                dtEmpty.Rows[found]["MGD2_IM"] = dtTemp.Rows[found2]["SMA_IM"];
                            }
                            if (adjRsn == "E") {
                                dtEmpty.Rows[found]["MGD2_CM"] = dtTemp.Rows[found2]["EWMA_CM"];
                                dtEmpty.Rows[found]["MGD2_MM"] = dtTemp.Rows[found2]["EWMA_MM"];
                                dtEmpty.Rows[found]["MGD2_IM"] = dtTemp.Rows[found2]["EWMA_IM"];
                            }
                            if (adjRsn == "M") {
                                dtEmpty.Rows[found]["MGD2_CM"] = dtTemp.Rows[found2]["MAXV_CM"];
                                dtEmpty.Rows[found]["MGD2_MM"] = dtTemp.Rows[found2]["MAXV_MM"];
                                dtEmpty.Rows[found]["MGD2_IM"] = dtTemp.Rows[found2]["MAXV_IM"];
                            }
                            if (adjRsn == "U") {
                                cm = dao40070.GetMarginVal(kindID, cm, 0, "CM_B");
                                dtEmpty.Rows[found]["MGD2_CM"] = cm;
                                dtEmpty.Rows[found]["MGD2_MM"] = dao40070.GetMarginVal(kindID, MM, cm, "MM_B");
                                dtEmpty.Rows[found]["MGD2_IM"] = dao40070.GetMarginVal(kindID, IM, cm, "IM_B");
                            }
                        }
                    }
                }//foreach (DataRow dr in dtFiltered.Rows)

                //if    ib_print = True then
                //      ids_1.dataobject = dw_1.dataobject
                //      ids_1.reset()
                //      dw_1.RowsCopy(1, dw_1.rowcount(), primary!, ids_1, 1, primary!)
                //end   if

                //dw_3.update()
                ResultData myResultData = daoMGD2.UpdateMGD2(dtEmpty);
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

                ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);
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
                reportLandscape.IsHandlePersonVisible = false;
                reportLandscape.IsManagerVisible = false;
                _ReportHelper.Create(reportLandscape);

                _ReportHelper.Print();//如果有夜盤會特別標註
                _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);

                //印完filter資料
                gvMain.Columns["AB_TYPE"].FilterInfo = new ColumnFilterInfo("[AB_TYPE] In ('-','A')");
                return ResultStatus.Success;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }

        /// <summary>
        /// 組合篩選條件並執行
        /// </summary>
        private void wf_filter() {

            gvMain.CloseEditor();
            DataTable dt = (DataTable)gcMain.DataSource;
            if (dt == null) {
                MessageDisplay.Error("請先讀取資料!");
                return;
            }

            List<string> adjCodeList = new List<string>();
            string[] is_adj_code;
            string ls_filter;

            if (cbxCodeY.Checked) adjCodeList.Add("'Y'");
            if (cbxCode.Checked) adjCodeList.Add("' '");
            if (cbxCodeN.Checked) adjCodeList.Add("'N'");
            is_adj_code = adjCodeList.ToArray();

            if (!cbxCodeY.Checked && !cbxCode.Checked && !cbxCodeN.Checked) {
                ls_filter = "''";
                adjCodeList.Clear();
            }
            else {
                ls_filter = f_gen_array_txt_ex(is_adj_code, ",", ",");
            }

            gvMain.Columns["ADJ_CODE"].FilterInfo = new ColumnFilterInfo("[ADJ_CODE] In (" + ls_filter + ") and [AB_TYPE] In ('-','A')");
        }

        /// <summary>
        /// 將篩選條件的陣列組成字串
        /// </summary>
        /// <param name="as_arr"></param>
        /// <param name="as_other"></param>
        /// <param name="as_last"></param>
        /// <returns></returns>
        private string f_gen_array_txt_ex(string[] as_arr, string as_other, string as_last) {
            int f, li_count;
            string ls_txt = "";
            li_count = as_arr.Length;
            for (f = 0; f < li_count; f++) {
                ls_txt = ls_txt + as_arr[f];
                if (f < li_count - 1) {
                    if (f == li_count - 1 - 1) {
                        ls_txt = ls_txt + as_last;
                    }
                    else {
                        ls_txt = ls_txt + as_other;
                    }
                }
            }
            return ls_txt;
        }

        #region 篩選 Checkbox Group
        private void cbxCodeN_CheckedChanged(object sender, EventArgs e) {
            wf_filter();
        }

        private void cbxCode_CheckedChanged(object sender, EventArgs e) {
            wf_filter();
        }

        private void cbxCodeY_CheckedChanged(object sender, EventArgs e) {
            wf_filter();
        }
        #endregion

        #region GridView Events
        private void gvMain_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            int ll_found;
            gv.CloseEditor();
            gv.UpdateCurrentRow();
            DataTable dt = (DataTable)gcMain.DataSource;
            string ls_kind_id = gv.GetRowCellValue(e.RowHandle, "KIND_ID").AsString();
            if (e.Column.Name == "ADJ_CODE") {
                if (e.Value.AsString() != "Y") {
                    gv.SetRowCellValue(e.RowHandle, "ISSUE_BEGIN_YMD", nullYmd);
                }
                else {
                    switch (gv.GetRowCellValue(e.RowHandle, "OSW_GRP").AsString()) {
                        case "5":
                            gv.SetRowCellValue(e.RowHandle, "ISSUE_BEGIN_YMD", txtDateG5.DateTimeValue.ToString("yyyyMMdd"));
                            break;
                        case "7":
                            gv.SetRowCellValue(e.RowHandle, "ISSUE_BEGIN_YMD", txtDateG7.DateTimeValue.ToString("yyyyMMdd"));
                            break;
                        default:
                            gv.SetRowCellValue(e.RowHandle, "ISSUE_BEGIN_YMD", txtDateG1.DateTimeValue.ToString("yyyyMMdd"));
                            break;
                    }
                }
                if (ls_kind_id == "TXF") {
                    ll_found = dt.Rows.IndexOf(dt.Select("kind_id ='MXF'").FirstOrDefault());
                    gv.SetRowCellValue(ll_found, "ADJ_CODE", e.Value);
                    gv.SetRowCellValue(ll_found, "ISSUE_BEGIN_YMD", gv.GetRowCellValue(e.RowHandle, "ISSUE_BEGIN_YMD"));
                }
            }
            if (e.Column.Name == "ADJ_RSN") {
                if (ls_kind_id == "TXF") {
                    ll_found = dt.Rows.IndexOf(dt.Select("kind_id ='MXF'").FirstOrDefault());
                    gv.SetRowCellValue(ll_found, "ADJ_RSN", e.Value);
                }
            }
            if (e.Column.Name == "USER_CM") {
                if (ls_kind_id == "TXF") {
                    ll_found = dt.Rows.IndexOf(dt.Select("kind_id ='MXF'").FirstOrDefault());
                    gv.SetRowCellValue(ll_found, "USER_CM", dao40070.GetMarginVal("MXF", e.Value.AsInt(), 0, "MTX_CM"));
                }
            }
        }

        /// <summary>
        /// MXF不能單獨編輯，要和TXF連動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string kindID = gv.GetRowCellValue(gv.FocusedRowHandle, "KIND_ID").AsString();
            if (kindID == "MXF") e.Cancel = true;
        }
        #endregion

        /// <summary>
        /// 若改動商品條件下拉選單，則清空GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlModel_EditValueChanging(object sender, ChangingEventArgs e) {
            gcMain.DataSource = null;
        }

        /// <summary>
        /// 調整下拉選單改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlAdjust_EditValueChanged(object sender, EventArgs e) {
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();
            DataTable dtAdjust = (DataTable)gcMain.DataSource;
            if (dtAdjust == null) return;
            if (dtAdjust.Rows.Count == 0) return;
            switch (ddlAdjust.EditValue.AsString()) {
                case "none":
                    foreach (DataRow dr in dtAdjust.Rows) {
                        dr["ADJ_CODE"] = " ";
                        dr["ISSUE_BEGIN_YMD"] = nullYmd;
                    }
                    break;
                case "indes":
                    for (int f = 0; f < dtAdjust.Rows.Count; f++) {
                        DataRow dr = dtAdjust.Rows[f];
                        if (dr["PROD_SUBTYPE"].AsString() == "I") {
                            dr["ADJ_CODE"] = "Y";
                            if (wf_set_valid_date(f) != "") {
                                dr["ADJ_CODE"] = " ";
                                return;
                            }
                        }
                        else {
                            dr["ADJ_CODE"] = " ";
                            dr["ISSUE_BEGIN_YMD"] = nullYmd;
                        }
                    }
                    break;
                case "all":
                    for (int f = 0; f < dtAdjust.Rows.Count; f++) {
                        DataRow dr = dtAdjust.Rows[f];
                        dr["ADJ_CODE"] = "Y";
                        if (wf_set_valid_date(f) != "") {
                            dr["ADJ_CODE"] = " ";
                            return;
                        }
                    }
                    break;
                case "ETF":
                    for (int f = 0; f < dtAdjust.Rows.Count; f++) {
                        DataRow dr = dtAdjust.Rows[f];
                        if (dr["PROD_SUBTYPE"].AsString() == "S") {
                            dr["ADJ_CODE"] = "Y";
                            if (wf_set_valid_date(f) != "") {
                                dr["ADJ_CODE"] = " ";
                                return;
                            }
                        }
                        else {
                            dr["ADJ_CODE"] = " ";
                            dr["ISSUE_BEGIN_YMD"] = nullYmd;
                        }
                    }
                    break;
                case "1":
                    for (int f = 0; f < dtAdjust.Rows.Count; f++) {
                        DataRow dr = dtAdjust.Rows[f];
                        if (dr["OSW_GRP"].AsString() == "1") {
                            dr["ADJ_CODE"] = "Y";
                            if (wf_set_valid_date(f) != "") {
                                dr["ADJ_CODE"] = " ";
                                return;
                            }
                        }
                        else {
                            dr["ADJ_CODE"] = " ";
                            dr["ISSUE_BEGIN_YMD"] = nullYmd;
                        }
                    }
                    break;
                case "2":
                    for (int f = 0; f < dtAdjust.Rows.Count; f++) {
                        DataRow dr = dtAdjust.Rows[f];
                        if (dr["OSW_GRP"].AsString() == "5") {
                            dr["ADJ_CODE"] = "Y";
                            if (wf_set_valid_date(f) != "") {
                                dr["ADJ_CODE"] = " ";
                                return;
                            }
                        }
                        else {
                            dr["ADJ_CODE"] = " ";
                            dr["ISSUE_BEGIN_YMD"] = nullYmd;
                        }
                    }
                    break;
                case "3":
                    for (int f = 0; f < dtAdjust.Rows.Count; f++) {
                        DataRow dr = dtAdjust.Rows[f];
                        if (dr["OSW_GRP"].AsString() == "7") {
                            dr["ADJ_CODE"] = "Y";
                            if (wf_set_valid_date(f) != "") {
                                dr["ADJ_CODE"] = " ";
                                return;
                            }
                        }
                        else {
                            dr["ADJ_CODE"] = " ";
                            dr["ISSUE_BEGIN_YMD"] = nullYmd;
                        }
                    }
                    break;
            }//switch (ddlAdjust.EditValue.AsString())

            gcMain.DataSource = dtAdjust;
        }

        private string wf_set_valid_date(int ai_row) {
            string osw_grp = gvMain.GetRowCellValue(ai_row, "OSW_GRP").AsString();
            if (gvMain.GetRowCellValue(ai_row, "ADJ_CODE").AsString() == "Y") {
                if (osw_grp == "1") {
                    if (txtDateG1.Text == "1901/01/01") {
                        MessageDisplay.Error("請先輸入" + lblG1.Text);
                        return "N";
                    }
                    gvMain.SetRowCellValue(ai_row, "ISSUE_BEGIN_YMD", txtDateG1.DateTimeValue.ToString("yyyyMMdd"));
                }
                if (osw_grp == "5") {
                    if (txtDateG5.Text == "1901/01/01") {
                        MessageDisplay.Error("請先輸入" + lblG2.Text);
                        return "N";
                    }
                    gvMain.SetRowCellValue(ai_row, "ISSUE_BEGIN_YMD", txtDateG5.DateTimeValue.ToString("yyyyMMdd"));
                }
                if (osw_grp == "7") {
                    if (txtDateG7.Text == "1901/01/01") {
                        MessageDisplay.Error("請先輸入" + lblG3.Text);
                        return "N";
                    }
                    gvMain.SetRowCellValue(ai_row, "ISSUE_BEGIN_YMD", txtDateG7.DateTimeValue.ToString("yyyyMMdd"));
                }
            }
            else {
                if (gvMain.GetRowCellValue(ai_row, "ISSUE_BEGIN_YMD").AsString() != null) {
                    gvMain.SetRowCellValue(ai_row, "ISSUE_BEGIN_YMD", nullYmd);
                }
            }
            return "";
        }
    }
}