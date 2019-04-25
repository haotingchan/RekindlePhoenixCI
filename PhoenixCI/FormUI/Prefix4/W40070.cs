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

/// <summary>
/// Lukas, 2019/4/17
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 40070
    /// </summary>
    public partial class W40070 : FormParent {

        private D40070 dao40070;
        private D40071 dao40071;
        private MGD2 daoMGD2;

        public W40070(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.DateTimeValue = DateTime.Now;
            //先隨便給個日期
            txtDateG1.DateTimeValue = DateTime.Now;
            txtDateG5.DateTimeValue = DateTime.Now;
            txtDateG7.DateTimeValue = DateTime.Now;

            //設定調整商品條件下拉選單
            List<LookupItem> modelType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "S", DisplayMember = "SMA達調整標準"},
                                        new LookupItem() { ValueMember = "a", DisplayMember = "任一model達調整標準" },
                                        new LookupItem() { ValueMember = "%", DisplayMember = "全部商品" }};
            Extension.SetDataTable(ddlModel, modelType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");
            ddlModel.EditValue = "S";

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
            txtSDate.EditValue = "2018/10/11";
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
                //讀取資料
                dao40070 = new D40070();
                DataTable dt40070 = dao40070.d_40070_scrn(txtSDate.DateTimeValue.ToString("yyyyMMdd"), ddlModel.EditValue.AsString());
                //排序
                dt40070 = dt40070.Sort("OSW_GRP, SEQ_NO, PROD_TYPE, KIND_ID");
                //過濾
                DataView dv = dt40070.AsDataView();
                dv.RowFilter = " ab_type in ('-','A')";
                DataTable dtFiltered = dv.ToTable();

                gcMain.DataSource = dtFiltered;
                //預設展開群組
                gvMain.ExpandAllGroups();

                //複製
                //dw_1.RowsCopy(1, dw_1.rowcount(), primary!, ids_tmp, 1, primary!)

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
                daoMGD2 = new MGD2();
                #region ue_save_before
                gvMain.CloseEditor();
                string ls_ymd, ls_issue_begin_ymd, ls_kind_id, ls_adj_type_name, ls_trade_ymd, ls_adj_rsn, is_adj_type;
                decimal ldc_cm = 0, ldc_cur_cm;
                int li_count;
                /***************************
		          調整類型:  
						0一般
						1長假
						2處置股票
						3股票
                ****************************/
                is_adj_type = "0";

                ls_ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");

                DataTable dtGrid = (DataTable)gcMain.DataSource;
                DataView dv = dtGrid.AsDataView();
                dv.RowFilter = " ab_type in ('-','A')";
                DataTable dtFiltered = dv.ToTable();

                cbxCodeY.Checked = true;
                cbxCodeN.Checked = true;
                cbxCode.Checked = true;

                foreach (DataRow dr in dtGrid.Rows) {
                    ls_kind_id = dr["KIND_ID"].AsString();
                    ls_issue_begin_ymd = dr["ISSUE_BEGIN_YMD"].AsString();
                    if (dr["ADJ_CODE"].AsString() == "Y") {
                        /******************************************
                           確認商品是否在同一交易日不同情境下設定過
                        ******************************************/
                        DataTable dtCheck = daoMGD2.IsProdSetOnSameDay(ls_kind_id, ls_ymd, is_adj_type);
                        li_count = dtCheck.Rows[0]["LI_COUNT"].AsInt();
                        ls_adj_type_name = dtCheck.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        if (li_count > 0) {
                            MessageDisplay.Error(ls_kind_id + ",交易日(" + ls_ymd + ")在" + ls_adj_type_name + "已有資料");
                            return ResultStatus.Fail;
                        }
                        /*********************************
                        確認商品是否在同一生效日區間設定過
                        生效起日若與生效迄日相同，不重疊
                        ex: 10/11的至10/31一般交易時段結束止，10/30的從10/31一般交易時段結束後始>>應不重疊
                        *************************************/
                        dtCheck = daoMGD2.IsProdSetInSameInterval(ls_kind_id, ls_ymd, ls_issue_begin_ymd);
                        li_count = dtCheck.Rows[0]["LI_COUNT"].AsInt();
                        ls_adj_type_name = dtCheck.Rows[0]["LS_ADJ_TYPE_NAME"].AsString();
                        ls_trade_ymd = dtCheck.Rows[0]["LS_TRADE_YMD"].AsString();
                        if (li_count > 0) {
                            MessageDisplay.Error(ls_kind_id + "," + ls_adj_type_name + ",交易日(" + ls_trade_ymd + ")在同一生效日區間內已有資料");
                            return ResultStatus.Fail;
                        }
                        /**************************************
                        判斷調整前後值不同，相同則警示且無法存檔
                        **************************************/
                        ls_adj_rsn = dr["ADJ_RSN"].AsString();
                        ldc_cur_cm = dr["CUR_CM"].AsDecimal();
                        if (ls_adj_rsn == "S") ldc_cm = dr["SMA_CM"].AsDecimal();
                        if (ls_adj_rsn == "E") ldc_cm = dr["EWMA_CM"].AsDecimal();
                        if (ls_adj_rsn == "M") ldc_cm = dr["MAXV_CM"].AsDecimal();
                        if (ls_adj_rsn == "U") {
                            ldc_cm = dr["USER_CM"].AsDecimal();
                            if (ldc_cm == 0) {
                                MessageDisplay.Error(ls_kind_id + ",請輸入保證金金額");
                                return ResultStatus.Fail;
                            }
                        }
                        if (ldc_cm == 0) {
                            MessageDisplay.Error(ls_kind_id + ",保證金計算值為空，請選擇其他模型");
                            return ResultStatus.Fail;
                        }
                        if (ldc_cm == ldc_cur_cm) {
                            MessageDisplay.Error(ls_kind_id + ",調整前後保證金一致，請重新輸入");
                            return ResultStatus.Fail;
                        }
                    }
                }
                #endregion

                DateTime ldt_w_time, ldt_date;
                ldt_date = txtSDate.DateTimeValue;
                int i, ll_found, li_col, ll_found2, ii_curr_row;
                string ls_rtn, ls_dbname;
                decimal ldc_cur_mm, ldc_cur_im, ldc_mm, ldc_im, ldc_rate;

                ldt_w_time = DateTime.Now;

                DataTable dtMGD2 = dao40071.d_40071(ls_ymd, is_adj_type);
                DataTable dtMGD2Log = dao40071.d_40071_log();
                //再產生一張空的 d_40071 table
                DataTable dt40071Empty = dao40071.d_40071(ls_ymd, is_adj_type);
                dt40071Empty.Clear();

                foreach (DataRow dr in dtGrid.Rows) {
                    ls_kind_id = dr["KIND_ID"].AsString();
                    ls_issue_begin_ymd = dr["ISSUE_BEGIN_YMD"].AsString();
                    ls_adj_rsn = dr["ADJ_RSN"].AsString();

                    dv = dtMGD2.AsDataView();
                    dv.RowFilter = "mgd2_kind_id = '" + ls_kind_id + "'";
                    dtMGD2 = dv.ToTable();

                    if (dtMGD2.Rows.Count > 0) {
                        foreach (DataRow drMGD2 in dtMGD2.Rows) {
                            ii_curr_row = dtMGD2Log.Rows.Count;
                            dtMGD2Log.Rows.Add();
                            for (li_col = 0; li_col < dtMGD2Log.Columns.Count; li_col++) {
                                //先取欄位名稱，因為兩張table欄位順序不一致
                                ls_dbname = dtMGD2.Columns[li_col].ColumnName;
                                dtMGD2Log.Rows[ii_curr_row][ls_dbname] = drMGD2[li_col];
                            }
                            if (dr["ADJ_CODE"].AsString() == "N") {
                                dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TYPE"] = "D";
                            }
                            else {
                                dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TYPE"] = "U";
                            }
                            dtMGD2Log.Rows[ii_curr_row]["MGD2_L_USER_ID"] = GlobalInfo.USER_ID;
                            dtMGD2Log.Rows[ii_curr_row]["MGD2_L_TIME"] = ldt_w_time;
                        }//foreach (DataRow drMGD2 in dtMGD2.Rows)

                        //刪除已存在資料

                    }
                }
            }
            catch (Exception ex) {
                MessageDisplay.Error("儲存錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }
    }
}