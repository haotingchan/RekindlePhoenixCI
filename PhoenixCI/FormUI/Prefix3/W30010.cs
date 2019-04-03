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
using BaseGround.Shared;
using BusinessObjects.Enums;
using DevExpress.XtraEditors.Controls;
using Common;
using DataObjects.Dao.Together;
using System.Threading;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System.IO;

/// <summary>
/// Lukas, 2019/4/2
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30010 市場動態報導
    /// </summary>
    public partial class W30010 : FormParent {

        protected class LookupItem {
            public string ValueMember { get; set; }
            public string DisplayMember { get; set; }
        }
        private OCFG daoOCFG;
        private RPT daoRPT;
        private D30010 dao30010;

        public W30010(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao30010 = new D30010();
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.EditValue = PbFunc.f_ocf_date(0);

            //盤別下拉選單
            List<LookupItem> ddlb_grp = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1", DisplayMember = "16:15收盤"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "全部收盤" }};
            Extension.SetDataTable(ddlType, ddlb_grp, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            try {
                //決定盤別下拉選單
                daoOCFG = new OCFG();
                if (daoOCFG.f_get_txn_osw_grp(_ProgramID) == "5") {
                    ddlType.EditValue = "1";
                }
                else {
                    ddlType.EditValue = "2";
                }

            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = false;//列印

            return ResultStatus.Success;
        }

        protected void ShowMsg(string msg) {
            lblProcessing.Text = msg;
            this.Refresh();
            Thread.Sleep(5);
        }

        protected override ResultStatus Export() {
            try {
                lblProcessing.Visible = true;
                string rptId, file, rptName = "",
                         cpYmd = txtSDate.DateTimeValue.ToString("yyyyMMdd");


                #region ue_export_before
                //判斷盤別
                int rtnInt, seq;
                string rtnStr, grp;
                if (ddlType.SelectedText == "16:15收盤") {
                    grp = "1";
                    DialogResult result = MessageDisplay.Choose("盤別為「16:15收盤」，請問是否繼續轉出報表？");
                    if (result == DialogResult.No) {
                        ShowMsg("已取消轉檔...");
                        return ResultStatus.Fail;
                    }
                }
                else {
                    grp = "2";
                }

                //判斷統計資料轉檔已完成
                for (int f = 1; f <= 2; f++) {
                    if (grp == "1") {
                        if (f == 1) {
                            seq = 13;
                        }
                        else {
                            seq = 23;
                        }
                    }
                    else {
                        seq = 17;
                        f = 2;
                    }
                    //check JSW
                    rtnStr = PbFunc.f_get_jsw_seq(_ProgramID, "E", seq, txtSDate.DateTimeValue, "0");
                    if (rtnStr != "") {
                        DialogResult result = MessageDisplay.Choose(" 統計資料未轉入完畢,是否要繼續?");
                        if (result == DialogResult.No) {
                            lblProcessing.Visible = false;
                            return ResultStatus.Fail;
                        }
                    }
                }

                //判斷20110作業已完成
                rtnInt = dao30010.check20110(txtSDate.Text);
                if (rtnInt == 0) {
                    DialogResult result = MessageDisplay.Choose(" 現貨資料 (資料來自20110作業)，" + Environment.NewLine + "請問是否繼續轉出報表？");
                    if (result == DialogResult.No) {
                        ShowMsg("已取消轉檔...");
                        return ResultStatus.Fail;
                    }
                }
                #endregion

                rptId = "30010_";

                //複製檔案
                file = wfCopy30010(rptId + grp, grp);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //切換Sheet
                Worksheet ws30011 = workbook.Worksheets["30011"];

                /* Sheet:30011 */
                wf_30012(rptId, rptName, ws30011);
                wf_30011(rptId, rptName, ws30011);

                /* Sheet:30013 */
                //切換Sheet
                Worksheet ws30013 = workbook.Worksheets["30013"];

                DataTable dtRowCount = dao30010.getRowIndexandCount();
                if (dtRowCount.Rows.Count == 0) {
                    MessageDisplay.Error("無法取得30013總列數");
                    return ResultStatus.Fail;
                }
                int li_tot_rowcount = dtRowCount.Rows[0]["LI_TOT_ROWCOUNT"].AsInt();

                //上市股票
                int ii_ole_row = dtRowCount.Rows[0]["II_OLE_ROW"].AsInt() - 1;
                if (ii_ole_row > 0) {
                    wf_30013("STF", "F", li_tot_rowcount, "1", ii_ole_row, ws30013);
                }
                //上櫃股票
                ii_ole_row = ii_ole_row + 2;
                if (ii_ole_row > 0) {
                    wf_30013("STF", "F", li_tot_rowcount, "2", ii_ole_row, ws30013);
                }
                //ETF股票
                ii_ole_row = ii_ole_row + 2;
                if (ii_ole_row > 0) {
                    wf_30013("ETF", "F", li_tot_rowcount, "%", ii_ole_row, ws30013);
                }
                //上市選擇權
                ii_ole_row = ii_ole_row + 2;
                if (ii_ole_row > 0) {
                    wf_30013("STC", "O", li_tot_rowcount, "1", ii_ole_row, ws30013);
                }
                //上櫃選擇權
                ii_ole_row = ii_ole_row + 2;
                if (ii_ole_row > 0) {
                    wf_30013("STC", "O", li_tot_rowcount, "2", ii_ole_row, ws30013);
                }
                //ETF選擇權
                ii_ole_row = ii_ole_row + 2;
                if (ii_ole_row > 0) {
                    wf_30013("ETC", "O", li_tot_rowcount, "%", ii_ole_row, ws30013);
                }

                /* Sheet:30014 */
                Worksheet ws30014 = workbook.Worksheets["30014"];

                wf_30014(ws30014);

                wf_30015(ws30014);

                //Eurex
                wf_30016(ws30014);

                //存檔
                ws30014.ScrollToRow(0);
                ws30013.ScrollToRow(0);
                ws30011.ScrollToRow(0);
                workbook.SaveDocument(file);
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }

        /// <summary>
        /// 期貨市場動態報導－選擇權
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="rptName"></param>
        /// <param name="ws30011"></param>
        private void wf_30012(string rptId, string rptName, Worksheet ws30011) {
            try {
                string ls_kind_id = "", ls_settle_date = "", ls_kind_id2 = "";
                int? rowIndex = null;
                int li_txw_cnt = 0, li_txw_row = 0;
                Range delRange;
                rptName = "期貨市場動態報導－選擇權";
                rptId = "30012";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                //讀取資料
                DataTable dt30012 = dao30010.d_30012(txtSDate.DateTimeValue);
                if (dt30012.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return;
                }

                //填資料
                foreach (DataRow dr in dt30012.Rows) {
                    if (ls_kind_id != dr["AMIF_PARAM_KEY"].AsString()) {
                        ls_kind_id = dr["AMIF_PARAM_KEY"].AsString();
                        ls_settle_date = dr["AMIF_SETTLE_DATE"].AsString();
                        if (dr["RPT_SEQ_NO"] != DBNull.Value) rowIndex = dr["RPT_SEQ_NO"].AsInt() - 1;
                        rowIndex = rowIndex;
                        if (ls_kind_id == "TXW") {
                            ls_kind_id2 = dr["AMIF_KIND_ID"].AsString();
                            if (rowIndex != null) li_txw_row = rowIndex.AsInt();
                            li_txw_cnt = li_txw_cnt + 1;
                        }
                    }
                    else {
                        if (ls_kind_id2 != dr["AMIF_KIND_ID"].AsString()) {
                            ls_kind_id2 = dr["AMIF_KIND_ID"].AsString();
                            if (ls_kind_id == "TXW") {
                                li_txw_cnt = li_txw_cnt + 1;
                                if (ls_settle_date == dr["AMIF_SETTLE_DATE"].AsString()) {
                                    rowIndex = rowIndex + 1;
                                }
                            }
                        }
                    }
                    if (rowIndex == null) continue;
                    int row = rowIndex.AsInt();
                    if (ls_settle_date != dr["AMIF_SETTLE_DATE"].AsString()) {
                        ls_settle_date = dr["AMIF_SETTLE_DATE"].AsString();
                        rowIndex = rowIndex + 1;
                    }
                    if (dr["AMIF_EXPIRY_TYPE"].AsString() == "W") {
                        ws30011.Cells[row, 1].Value = ls_settle_date.SubStr(0, 2) + "W" + dr["AMIF_KIND_ID"].AsString().SubStr(2, 1);
                    }
                    else {
                        ws30011.Cells[row, 1].Value = ls_settle_date.SubStr(0, 2);
                    }
                    if (dr["AMIF_PC_CODE"].AsString() == "C") {
                        if (dr["M_QNTY"] != DBNull.Value) ws30011.Cells[row, 2].Value = dr["M_QNTY"].AsDecimal();
                        if (dr["OPEN_INTEREST"] != DBNull.Value) ws30011.Cells[row, 4].Value = dr["OPEN_INTEREST"].AsDecimal();
                    }
                    else {
                        if (dr["M_QNTY"] != DBNull.Value) ws30011.Cells[row, 6].Value = dr["M_QNTY"].AsDecimal();
                        if (dr["OPEN_INTEREST"] != DBNull.Value) ws30011.Cells[row, 8].Value = dr["OPEN_INTEREST"].AsDecimal();
                    }
                }
                if (li_txw_cnt == 0) {
                    li_txw_row = dao30010.getTxwRow() - 1;
                    delRange = ws30011.Range[(li_txw_row).ToString() + ":" + (li_txw_row + 1).ToString()];
                    delRange.Delete(DeleteMode.EntireRow);
                }
                else if (li_txw_cnt < 2) {
                    delRange = ws30011.Rows[(li_txw_row + 1).ToString()];
                    delRange.Delete(DeleteMode.EntireRow);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 期貨市場動態報導－期貨
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="rptName"></param>
        /// <param name="ws30011"></param>
        private void wf_30011(string rptId, string rptName, Worksheet ws30011) {
            try {
                rptName = "期貨市場動態報導－期貨";
                rptId = "30011";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                string ls_kind_id = "", ls_settle_date = "", ls_index = "", ls_index_str = "", ls_kind_id2 = "";
                int ii_ole_row = 0, li_mxw_cnt = 0, li_mxw_row = 0, li_row_cnt = 0;
                decimal ld_value, ld_m_qnty, ld_value2;
                Range delRange;
                ls_index_str = "000000";

                ws30011.Cells[0, 10].Value = "民國" + (txtSDate.DateTimeValue.Year - 1911)
                      + "年" + txtSDate.DateTimeValue.Month + "月" + txtSDate.DateTimeValue.Day + "日";

                //讀取資料
                DataTable dt30011 = dao30010.d_30011(txtSDate.DateTimeValue);
                if (dt30011.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return;
                }

                //RPT
                daoRPT = new RPT();
                DataTable dtRPT = daoRPT.ListAllByTXD_ID(rptId);
                if (dtRPT.Rows.Count == 0) {
                    MessageDisplay.Info(rptId + '－' + "RPT無任何資料!");
                    lblProcessing.Visible = false;
                    return;
                }

                //填資料
                li_mxw_cnt = 0;
                foreach (DataRow dr in dt30011.Rows) {
                    ld_m_qnty = dr["AMIF_M_QNTY_TAL"].AsDecimal();
                    if (ls_kind_id2 != dr["AMIF_KIND_ID2"].AsString()) {
                        ls_kind_id2 = dr["AMIF_KIND_ID2"].AsString();
                        ii_ole_row = dr["RPT_SEQ_NO"].AsInt() - 1 - 1;
                        li_row_cnt = 0;
                        ls_index = dr["RPT_VALUE_3"].AsString();
                    }
                    ls_kind_id = dr["AMIF_KIND_ID"].AsString();
                    ii_ole_row = ii_ole_row + 1;
                    if (ls_kind_id2 == "MXW") {
                        li_mxw_row = ii_ole_row;
                        li_mxw_cnt = li_mxw_cnt + 1;
                    }
                    ls_settle_date = dr["AMIF_SETTLE_DATE"].AsString();
                    if (ls_settle_date == ls_index_str) {
                        ls_settle_date = "指數";
                    }
                    else {
                        if (ls_kind_id == "STW" && li_row_cnt >= 2) {
                            li_row_cnt = li_row_cnt + 1;
                            continue;
                        }
                        /* 第一筆不是指數,則跳一列 */
                        if (li_row_cnt == 0 && ls_index == ls_index_str) {
                            ii_ole_row = ii_ole_row + 1;
                            li_row_cnt = li_row_cnt + 1;
                        }
                        if (dr["AMIF_EXPIRY_TYPE"].AsString() == "W") {
                            ws30011.Cells[ii_ole_row, 1].Value = ls_settle_date.SubStr(0, 2) + "W" + ls_kind_id.SubStr(2, 1);
                        }
                        else {
                            ws30011.Cells[ii_ole_row, 1].Value = ls_settle_date.SubStr(0, 2);
                        }
                    }
                    ld_value = dr["AMIF_OPEN_PRICE"].AsDecimal();
                    if ((ld_value != 0 && ld_m_qnty > 0) || (ls_settle_date == "指數" && ld_value != 0)) {
                        ws30011.Cells[ii_ole_row, 2].Value = ld_value;
                    }
                    ld_value = dr["AMIF_HIGH_PRICE"].AsDecimal();
                    if (ld_value != 0) {
                        ws30011.Cells[ii_ole_row, 3].Value = ld_value;
                    }
                    ld_value = dr["AMIF_LOW_PRICE"].AsDecimal();
                    if (ld_value != 0 || (ls_settle_date == "指數" && ld_value != 0)) {
                        ws30011.Cells[ii_ole_row, 4].Value = ld_value;
                    }
                    ld_value = dr["AMIF_CLOSE_PRICE"].AsDecimal();
                    if ((ld_value != 0 && ld_m_qnty > 0) || (ls_settle_date == "指數" && ld_value != 0)) {
                        ws30011.Cells[ii_ole_row, 5].Value = ld_value;
                    }
                    ld_value2 = dr["AMIF_UP_DOWN_VAL"].AsDecimal();
                    if ((ld_value != 0 && ld_m_qnty > 0) || (ls_settle_date == "指數" && ld_value != 0)) {
                        ws30011.Cells[ii_ole_row, 6].Value = ld_value2.AsString();
                        if ((dr["AMIF_CLOSE_PRICE"].AsDecimal() - dr["AMIF_UP_DOWN_VAL"].AsDecimal()) == 0) {
                            MessageDisplay.Error(ls_kind_id + " 收盤價-漲跌幅=0 ,計算漲跌點%造成除數為0");
                            return;
                        }
                        ld_value2 = Math.Round((dr["AMIF_UP_DOWN_VAL"].AsDecimal() / (dr["AMIF_CLOSE_PRICE"].AsDecimal() - dr["AMIF_UP_DOWN_VAL"].AsDecimal())) * 100,
                                                3, MidpointRounding.AwayFromZero);
                        ws30011.Cells[ii_ole_row, 7].Value = ld_value2.AsString();
                    }
                    ws30011.Cells[ii_ole_row, 8].Value = dr["AMIF_M_QNTY_TAL"].AsDecimal();
                    if (ls_settle_date != "指數") {
                        if (ls_kind_id.SubStr(0, 3) != "STW") {
                            ws30011.Cells[ii_ole_row, 9].Value = dr["AMIF_SETTLE_PRICE"].AsDecimal();
                        }
                        ws30011.Cells[ii_ole_row, 10].Value = dr["AMIF_OPEN_INTEREST"].AsDecimal();
                    }
                    li_row_cnt = li_row_cnt + 1;
                }//foreach (DataRow dr in dt30011.Rows)
                if (li_mxw_cnt == 0) {
                    li_mxw_row = dao30010.getMxwRow() - 1;
                    delRange = ws30011.Range[(li_mxw_row).ToString() + ":" + (li_mxw_row + 1).ToString()];
                    delRange.Delete(DeleteMode.EntireRow);
                }
                else if (li_mxw_cnt < 2) {
                    delRange = ws30011.Rows[(li_mxw_row + 1).ToString()];
                    delRange.Delete(DeleteMode.EntireRow);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 期貨市場動態報導－股票選擇權
        /// </summary>
        /// <param name="ai_pdk_param_key"></param>
        /// <param name="as_prod_type"></param>
        /// <param name="ai_tot_rowcount"></param>
        /// <param name="as_pdk_underlying_market"></param>
        /// <param name="ii_ole_row"></param>
        /// <param name="ws30013"></param>
        private void wf_30013(string ai_pdk_param_key, string as_prod_type, int ai_tot_rowcount, string as_pdk_underlying_market,
                              int ii_ole_row, Worksheet ws30013) {
            try {
                string rptName, rptId;
                string ls_kind_id, ls_settle_date, ls_pdk_name;
                int li_col_add, li_pos, li_row_start, li_tot_rowcount;
                Range delRange;

                rptName = "期貨市場動態報導－股票選擇權";
                rptId = "30013";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                //讀取資料
                DataTable dt30013 = dao30010.d_30013(txtSDate.DateTimeValue, ai_pdk_param_key, as_prod_type, as_pdk_underlying_market);
                li_col_add = 0;
                li_row_start = ii_ole_row;
                li_tot_rowcount = li_row_start + Math.Ceiling(ai_tot_rowcount.AsDecimal() / 2).AsInt() - 1;
                if (dt30013.Rows.Count == 0) {
                    //全部刪除
                    ii_ole_row = ii_ole_row - 2;
                    delRange = ws30013.Range[(ii_ole_row + 1).ToString() + ":" + (li_tot_rowcount + 1).ToString()];
                    delRange.Delete(DeleteMode.EntireRow);
                }

                /* 列數在B1 */
                for (int f = 0; f > dt30013.Rows.Count; f++) {
                    DataRow dr = dt30013.Rows[f];
                    if (Math.IEEERemainder(f, 2) == 0) {
                        li_col_add = 0;
                    }
                    else {
                        li_col_add = 4;
                        ii_ole_row = ii_ole_row - 1;
                    }
                    ii_ole_row = ii_ole_row + 1;
                    /* 標的名稱:不要"選擇權" */
                    ls_pdk_name = dr["PDK_NAME"].AsString();
                    ws30013.Cells[ii_ole_row, 1 + li_col_add].Value = dr["PDK_STOCK_ID"].AsString() + "(" + dr["AMIF_KIND_ID"].AsString() + ")";
                    ws30013.Cells[ii_ole_row, 2 + li_col_add].Value = ls_pdk_name;

                    if (dr["AMIF_DATA_SOURCE"].AsString() == "P") {
                        ws30013.Cells[ii_ole_row, 3 + li_col_add].Value = "停止交易";
                    }
                    else {
                        ws30013.Cells[ii_ole_row, 3 + li_col_add].Value = dr["M_QNTY"].AsDecimal();
                    }
                    ws30013.Cells[ii_ole_row, 4 + li_col_add].Value = dr["OPEN_INTEREST"].AsDecimal();
                }
                li_row_start = ii_ole_row;

                //刪除空白列
                if (li_tot_rowcount > li_row_start) {
                    delRange = ws30013.Range[(li_row_start + 1).ToString() + ":" + (li_tot_rowcount).ToString()];
                    delRange.Delete(DeleteMode.EntireRow);
                }
                ii_ole_row = li_row_start + 1;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 期貨市場動態報導－交易量表
        /// </summary>
        /// <param name="ws30014"></param>
        private void wf_30014(Worksheet ws30014) {
            string rptName, rptId, ls_date;
            int li_row_cnt, ii_ole_row;
            decimal ld_value;
            rptName = "期貨市場動態報導－交易量表";
            rptId = "30014";
            lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

            //讀取資料
            DataTable dt30014 = dao30010.d_30014(txtSDate.DateTimeValue);
            if (dt30014.Rows.Count == 0) {
                MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                lblProcessing.Visible = false;
                return;
            }

            //填資料
            foreach (DataRow dr in dt30014.Rows) {
                ii_ole_row = dr["RPT_SEQ_NO"].AsInt() - 1;
                if (ii_ole_row < 0) continue;
                ws30014.Cells[ii_ole_row, 3].Value = dr["AI1_M_QNTY"].AsDecimal();
                ld_value = dr["M_INCREASE"].AsDecimal();
                ws30014.Cells[ii_ole_row, 4].Value = ld_value;
                ws30014.Cells[ii_ole_row, 5].Value = dr["AI1_OI"].AsDecimal();
                ld_value = dr["OI_INCREASE"].AsDecimal();
                ws30014.Cells[ii_ole_row, 6].Value = ld_value;
                ws30014.Cells[ii_ole_row, 7].Value = dr["AI1_AVG_MONTH"].AsDecimal();
                ws30014.Cells[ii_ole_row, 8].Value = dr["AI1_AVG_YEAR"].AsDecimal();
                ws30014.Cells[ii_ole_row, 9].Value = dr["AI1_HIGH_QNTY"].AsDecimal();
                ls_date = dr["AI1_HIGH_DATE"].AsDateTime().ToString("yyyy.MM.dd");
                ls_date = (ls_date.SubStr(0, 4).AsInt() - 1911) + ls_date.SubStr(4, 6);
                ws30014.Cells[ii_ole_row, 10].Value = ls_date;
            }
        }

        /// <summary>
        /// 期貨市場動態報導－開戶數
        /// </summary>
        /// <param name="ws30014"></param>
        private void wf_30015(Worksheet ws30014) {
            string rptName, rptId, ls_date;
            int li_row_cnt, ii_ole_row;
            decimal ld_value;
            DateTime ldt_date, ld_date_fm;
            rptName = "期貨市場動態報導－開戶數";
            rptId = "30015";
            lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";
            ldt_date = txtSDate.DateTimeValue;
            ld_date_fm = PbFunc.relativedate(txtSDate.DateTimeValue, -365);
            ldt_date = dao30010.checkPreviousData(ldt_date, ld_date_fm);

            //讀取資料
            DataTable dt30015 = dao30010.d_30015(ldt_date);
            if (dt30015.Rows.Count == 0) {
                MessageDisplay.Info(ldt_date.ToString("yyyy/MM/dd") + "(前營業日)開戶資料未轉入(功能28610)，或請轉入後重新執行此功能!");
                lblProcessing.Visible = false;
                return;
            }

            //填資料
            /* 只會有1筆 */
            DataRow dr = dt30015.Rows[0];
            ii_ole_row = dao30010.get30015Row() - 1;
            ws30014.Cells[ii_ole_row, 2].Value = dr["AB3_COUNT"].AsDecimal();
            ld_value = dr["AB3_INCREASE"].AsDecimal();
            ws30014.Cells[ii_ole_row, 4].Value = ld_value.AsString();
            ws30014.Cells[ii_ole_row, 6].Value = dr["AB3_COUNT1"].AsDecimal();
            ws30014.Cells[ii_ole_row, 8].Value = dr["AB3_COUNT2"].AsDecimal();
            ws30014.Cells[ii_ole_row, 9].Value = dr["AB3_DATE"].AsDateTime().ToString("MM月dd日");
            ws30014.Cells[ii_ole_row, 10].Value = dr["AB3_TRADE_COUNT"].AsDecimal();

            //成交值
            ii_ole_row = ii_ole_row + 4;
            ldt_date = txtSDate.DateTimeValue;
            decimal ld_amt;
            ls_date = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            ld_amt = dao30010.get30015Amt_1(ls_date);
            ld_amt = Math.Round(ld_amt / 100000000 / 2, 2, MidpointRounding.AwayFromZero);
            ws30014.Cells[ii_ole_row, 0].Value = ld_amt;

            ld_amt = dao30010.get30015Amt_2(ldt_date);
            ws30014.Cells[ii_ole_row, 3].Value = ld_amt;
        }

        /// <summary>
        /// Eurex/TAIFEX 合作商品概況
        /// </summary>
        /// <param name="ws30014"></param>
        private void wf_30016(Worksheet ws30014) {
            string rptName, rptId, ls_date;
            int li_row_cnt, ii_ole_row;
            decimal ld_value;
            rptName = "Eurex/TAIFEX 合作商品概況";
            rptId = "30016";
            lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

            //讀取前一交易日
            ls_date = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            ls_date = dao30010.checkPreviousDay(ls_date).ToString("yyyy/MM/dd");
            if (dao30010.checkPreviousDay(ls_date) == DateTime.MinValue) {
                ls_date = txtSDate.Text;
            }

            //讀取資料
            DataTable dt30016 = dao30010.d_30016(txtSDate.DateTimeValue);
            if (dt30016.Rows.Count == 0) {
                lblProcessing.Visible = false;
                return;
            }

            //填資料
            int li_add = 55 - 1;
            ws30014.Cells[li_add + 1, 9].Value = "民國" + (ls_date.SubStr(0, 4).AsInt() - 1911) + "年" + ls_date.SubStr(5, 2) + "月" + ls_date.SubStr(8, 2) + "日";
            foreach (DataRow dr in dt30016.Rows) {
                ii_ole_row = dr["RPT_SEQ_NO"].AsInt() + li_add;
                if (ii_ole_row == 0) continue;
                ws30014.Cells[ii_ole_row, 3].Value = dr["AE3_M_QNTY"].AsDecimal();
                ld_value = dr["M_INCREASE"].AsDecimal();
                ws30014.Cells[ii_ole_row, 4].Value = ld_value;
                ws30014.Cells[ii_ole_row, 5].Value = dr["AE3_OI"].AsDecimal();
                ld_value = dr["OI_INCREASE"].AsDecimal();
                ws30014.Cells[ii_ole_row, 6].Value = ld_value;
                ws30014.Cells[ii_ole_row, 7].Value = dr["AE3_AVG_MONTH"].AsDecimal();
                ws30014.Cells[ii_ole_row, 8].Value = dr["AE3_AVG_YEAR"].AsDecimal();
                ws30014.Cells[ii_ole_row, 9].Value = dr["AE3_HIGH_QNTY"].AsDecimal();
                ls_date = dr["AE3_HIGH_DATE"].AsDateTime().ToString("yyyy.MM.dd");
                ls_date = (ls_date.SubStr(0, 4).AsInt() - 1911) + ls_date.SubStr(4, 6);
                ws30014.Cells[ii_ole_row, 10].Value = ls_date;
            }
        }

        /// <summary>
        /// 這支功能PB覆寫公用的wf_copyfile
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="grp"></param>
        /// <returns></returns>
        private string wfCopy30010(string fileName, string grp) {

            string template = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH, fileName + ".xls");
            if (!File.Exists(template)) {
                MessageDisplay.Error("無此檔案「" + template + "」!");
                return "";
            }
            string lsFilename;
            lsFilename = "動態報導" + (txtSDate.DateTimeValue.Year - 1911) + "." +
                         txtSDate.DateTimeValue.Month + "." + txtSDate.DateTimeValue.Day + ".xls";
            if (grp == "1") {
                lsFilename = lsFilename + "(16時15分收盤)" + ".xls";
            }
            else {
                lsFilename = lsFilename + "(全部收盤)" + ".xls";
            }
            bool lbChk;
            string file = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, lsFilename);
            lbChk = File.Exists(file);
            if (lbChk) {
                File.Move(file, file + "_bak_" + DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss") + ".xls");
            }
            try {
                File.Copy(template, file, false);
            }
            catch {
                MessageDisplay.Error("複製「" + template + "」到「" + file + "」檔案錯誤!");
                return "";
            }
            lsFilename = file;
            return lsFilename;
        }
    }
}