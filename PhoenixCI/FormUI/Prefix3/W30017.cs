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
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;
using Common;
using DevExpress.Spreadsheet;
using System.IO;
using System.Threading;

/// <summary>
/// Lukas, 2019/3/6
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30017 本公司及周邊單位市場交易速報表
    /// </summary>
    public partial class W30017 : FormParent {

        private D30017 dao30017;

        public W30017(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtSDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtSDate.Focus();
            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();
            if (FlagAdmin) {
                cbxAfter.Visible = true;
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
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                dao30017 = new D30017();

                #region ue_export_before
                string asYmd, logTxt = "";
                asYmd = txtSDate.Text.Replace("/", "");
                DataTable dtCheck20110 = dao30017.check20110(asYmd);

                if (dtCheck20110.Rows[0]["LI_TFXM_CNT"].AsInt() == 0) {
                    DialogResult result = MessageDisplay.Choose("未完成「20110－每日各商品現貨指數資料輸入」作業，是否要繼續?");
                    if (result == DialogResult.No) {
                        txtSDate.Focus();
                        return ResultStatus.Fail;
                    }
                    else {
                        WriteLog("轉出檔案 " + logTxt.Trim() + "(20110未完成)", "Info", "E");
                    }
                }

                if (dtCheck20110.Rows[0]["LI_FUT_CNT"].AsInt() == 0) {
                    DialogResult result = MessageDisplay.Choose("未完成「期貨批次－AA00121B」作業，是否要繼續?");
                    if (result == DialogResult.No) {
                        txtSDate.Focus();
                        return ResultStatus.Fail;
                    }
                    else {
                        WriteLog("轉出檔案 " + logTxt.Trim() + "(期貨AA00120B未完成)", "Info", "E");
                    }
                }

                if (dtCheck20110.Rows[0]["LI_OPT_CNT"].AsInt() == 0) {
                    DialogResult result = MessageDisplay.Choose("未完成「選擇權批次－AA00121B」作業，是否要繼續?");
                    if (result == DialogResult.No) {
                        txtSDate.Focus();
                        return ResultStatus.Fail;
                    }
                    else {
                        WriteLog("轉出檔案 " + logTxt.Trim() + "(選擇權AA00120B未完成)", "Info", "E");
                    }
                }
                #endregion

                string rptId, lsFile;
                if (cbxAfter.Checked) {
                    rptId = "30017_2";
                }
                else {
                    rptId = "30017";
                }
                // 1. 複製檔案
                lsFile = wfCopyFile(rptId);
                if (lsFile == "") {
                    return ResultStatus.Fail;
                }
                logTxt = lsFile;

                // 2. 開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(lsFile);
                // 切換Sheet
                Worksheet ws30017 = workbook.Worksheets[0];

                // 3. 匯出檔案
                string rptName, lsStr;
                int rowNum;
                decimal? ldVal;

                if (cbxAfter.Checked) {
                    #region wf_30017_all
                    rptName = "本公司及周邊單位市場交易速報表";
                    rptId = "30017_all";

                    // 讀取資料
                    DataTable dt30017all = dao30017.d_30017_all(asYmd);
                    if (dt30017all.Rows.Count == 0) {
                        MessageDisplay.Error(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                        return ResultStatus.Fail;
                    }
                    ShowMsg(rptId + '－' + rptName + " 轉檔中...");

                    // 填入資料
                    foreach (DataRow dr in dt30017all.Rows) {
                        rowNum = dr["RPT_SEQ_NO"].AsInt() - 1;
                        if (rowNum < 0) continue;
                        ldVal = string.IsNullOrEmpty(dr["AI8_CLOSE_PRICE"].AsString()) ? null : (decimal?)dr["AI8_CLOSE_PRICE"].AsDecimal();
                        if (ldVal != null && dr["AI8_QNTY"].AsDecimal() > 0) {
                            ws30017.Cells[rowNum, 6].Value = ldVal;
                        }
                        ldVal = string.IsNullOrEmpty(dr["AI8_UP_DOWN_VAL"].AsString()) ? null : (decimal?)dr["AI8_UP_DOWN_VAL"].AsDecimal();
                        if (ldVal != null) {
                            ws30017.Cells[rowNum, 7].Value = ldVal;
                        }
                        ldVal = string.IsNullOrEmpty(dr["UP_DOWN_RATE"].AsString()) ? null : (decimal?)dr["UP_DOWN_RATE"].AsDecimal();
                        if (ldVal != null) {
                            ws30017.Cells[rowNum, 8].Value = ldVal;
                        }
                        ws30017.Cells[rowNum, 9].SetValue(dr["AI8_QNTY"]);
                        if (dr["AI8_AH_MTH_DAYS"].AsInt() > 0) {
                            ws30017.Cells[rowNum, 11].SetValue(dr["AI8_AH_QNTY"]);
                        }
                        ws30017.Cells[rowNum, 13].SetValue(dr["AVG_YEAR"]);
                        ws30017.Cells[rowNum, 15].SetValue(dr["AVG_ALL_LAST_YEAR"]);
                        decimal? am7tAvgQnty = string.IsNullOrEmpty(dr["AM7T_AVG_QNTY"].AsString()) ? null : (decimal?)dr["AM7T_AVG_QNTY"].AsDecimal();
                        if (am7tAvgQnty == 0) {
                            ws30017.Cells[rowNum, 17].Value = "NA";
                        }
                        else {
                            ws30017.Cells[rowNum, 17].Value = am7tAvgQnty;
                        }

                        // OI
                        ws30017.Cells[rowNum, 19].SetValue(dr["AI8_OI"]);
                        ws30017.Cells[rowNum, 21].SetValue(dr["AI8_OI_COMPARE"]);

                        // 新高
                        lsStr = "";
                        if (dr["YMD_QNTY"].AsString() == asYmd) {
                            lsStr = "交易量";
                        }
                        if (dr["YMD_OI"].AsString() == asYmd) {
                            if (lsStr != "") {
                                lsStr = lsStr + "、";
                            }
                            lsStr = lsStr + "OI";
                        }
                        if (lsStr != "") {
                            lsStr = lsStr + "新高";
                        }
                        ws30017.Cells[rowNum, 22].Value = lsStr;
                    }
                    #endregion
                }
                else {
                    #region wf_30017
                    rptName = "本公司及周邊單位市場交易速報表";
                    rptId = "30017";

                    // 讀取資料
                    DataTable dt30017 = dao30017.d_30017(asYmd);
                    if (dt30017.Rows.Count == 0) {
                        MessageDisplay.Error(asYmd + "," + rptId + '－' + rptName + ",無任何資料!");
                        return ResultStatus.Fail;
                    }
                    ShowMsg(rptId + '－' + rptName + " 轉檔中...");

                    // 填入資料
                    foreach (DataRow dr in dt30017.Rows) {
                        rowNum = dr["RPT_SEQ_NO"].AsInt() - 1;
                        if (rowNum < 0) continue;
                        ldVal = string.IsNullOrEmpty(dr["AI8_CLOSE_PRICE"].AsString()) ? null : (decimal?)dr["AI8_CLOSE_PRICE"].AsDecimal();
                        if (ldVal != null && dr["AI8_QNTY"].AsDecimal() > 0) {
                            ws30017.Cells[rowNum, 6].Value = ldVal;
                        }
                        ldVal = string.IsNullOrEmpty(dr["AI8_UP_DOWN_VAL"].AsString()) ? null : (decimal?)dr["AI8_UP_DOWN_VAL"].AsDecimal();
                        if (ldVal != null) {
                            ws30017.Cells[rowNum, 7].Value = ldVal;
                        }
                        ldVal = string.IsNullOrEmpty(dr["UP_DOWN_RATE"].AsString()) ? null : (decimal?)dr["UP_DOWN_RATE"].AsDecimal();
                        if (ldVal != null) {
                            ws30017.Cells[rowNum, 8].Value = ldVal;
                        }
                        ws30017.Cells[rowNum, 9].SetValue(dr["AI8_QNTY"]);

                        if (dr["AI8_AH_MTH_DAYS"].AsInt() > 0) {
                            ws30017.Cells[rowNum, 11].SetValue(dr["AI8_AH_QNTY"]);
                        }
                        ws30017.Cells[rowNum, 13].SetValue(dr["AVG_YEAR"]);
                        ws30017.Cells[rowNum, 15].SetValue(dr["AVG_ALL_LAST_YEAR"]);
                        decimal? am7tAvgQnty = string.IsNullOrEmpty(dr["AM7T_AVG_QNTY"].AsString()) ? null : (decimal?)dr["AM7T_AVG_QNTY"].AsDecimal();
                        if (am7tAvgQnty == 0) {
                            ws30017.Cells[rowNum, 17].Value = "NA";
                        }
                        else {
                            ws30017.Cells[rowNum, 17].Value = am7tAvgQnty;
                        }
                    }
                    #endregion
                }

                #region wf_30017_up
                rptName = "本公司及周邊單位市場交易速報表";
                rptId = "30017up";

                // 讀取資料
                DataTable dt30017up = dao30017.d_30017_up(asYmd);
                if (dt30017up.Rows.Count == 0) {
                    MessageDisplay.Error(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    return ResultStatus.Fail;
                }
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");

                // 寫入資料
                ws30017.Cells[1, 9].Value = txtSDate.Text.Replace("/", ".") + ws30017.Cells[1, 9].Value.AsString();
                foreach (DataRow dr in dt30017up.Rows) {
                    rowNum = dr["RPT_SEQ_NO"].AsInt() - 1;
                    if (rowNum < 0) continue;
                    ws30017.Cells[rowNum, 9].SetValue(dr["AI8_QNTY"]);
                    ws30017.Cells[rowNum, 10].SetValue(dr["AI8_CLOSE_PRICE"]);
                    ws30017.Cells[rowNum, 11].SetValue(dr["AI8_UP_DOWN_VAL"]);
                    ws30017.Cells[rowNum, 12].SetValue(dr["UP_DOWN_RATE"]);
                    ws30017.Cells[rowNum, 13].SetValue(dr["AVG_MTH"]);
                    ws30017.Cells[rowNum, 14].SetValue(dr["AVG_YEAR"]);
                    ws30017.Cells[rowNum, 16].SetValue(dr["AVG_LAST_ALL_YEAR"]);
                }

                // 去年度日均值
                string lsYmd = (txtSDate.DateTimeValue.Year - 1).AsString();
                ldVal = dao30017.txfAvgQnty(lsYmd);
                ws30017.Cells[5, 16].Value = ldVal;

                ldVal = dao30017.gtfAvgQnty(lsYmd);
                ws30017.Cells[6, 16].Value = ldVal;
                #endregion

                if (cbxAfter.Checked) {
                    #region wf_30017down_all
                    rptName = "本公司及周邊單位市場交易速報表";
                    rptId = "30017down_all";

                    // 讀取資料
                    DataTable dt30017downall = dao30017.d_30017down_all(asYmd);
                    if (dt30017downall.Rows.Count == 0) {
                        MessageDisplay.Error(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                        return ResultStatus.Fail;
                    }
                    ShowMsg(rptId + '－' + rptName + " 轉檔中...");

                    // 填入資料
                    int f, liDays, liFut, liOpt, liAll;
                    decimal ldFutQnty, ldOptQnty, ldQnty, ldRate;
                    liAll = -1;
                    liFut = -1;
                    liOpt = -1;
                    for (f = 0; f < dt30017downall.Rows.Count; f++) {
                        DataRow dr = dt30017downall.Rows[f];
                        switch (dr["AI8_PROD_TYPE"].ToString()) {
                            case " ":
                                liAll = f;
                                break;
                            case "F":
                                liFut = f;
                                break;
                            case "O":
                                liOpt = f;
                                break;

                        }
                    }
                    rowNum = 61;

                    // 預計交易天數
                    lsStr = asYmd.SubStr(0, 4);
                    liDays = dao30017.am7tDayCount(lsStr);
                    ws30017.Cells[rowNum, 20].Value = liDays;
                    ws30017.Cells[rowNum, 16].Value = liDays - dt30017downall.Rows[liAll]["AI8_YEAR_DAYS"].AsInt();
                    if (liFut >= 0) {
                        ws30017.Cells[rowNum, 13].SetValue(dt30017downall.Rows[liFut]["AVG_MTH"]);
                        ws30017.Cells[rowNum, 17].SetValue(dt30017downall.Rows[liFut]["AI8_YEAR_QNTY"]);
                    }
                    if (liOpt >= 0) {
                        ws30017.Cells[rowNum + 1, 13].SetValue(dt30017downall.Rows[liOpt]["AVG_MTH"]);
                        ws30017.Cells[rowNum + 1, 17].SetValue(dt30017downall.Rows[liOpt]["AI8_YEAR_QNTY"]);
                    }
                    if (liAll >= 0) {
                        ws30017.Cells[rowNum + 2, 13].SetValue(dt30017downall.Rows[liAll]["AVG_MTH"]);
                        ws30017.Cells[rowNum + 2, 17].SetValue(dt30017downall.Rows[liAll]["AI8_YEAR_QNTY"]);
                    }

                    //
                    rowNum = 57;
                    lsStr = "註1：";
                    lsStr = lsStr + (txtSDate.DateTimeValue.Year - 1911) + "年度預計有" +
                             liDays.AsString() + "個交易日，已交易" +
                             dt30017downall.Rows[liAll]["AI8_YEAR_DAYS"].AsInt() + "日，";
                    lsStr = lsStr + "共計成交" + dt30017downall.Rows[liAll]["AI8_YEAR_QNTY"].AsDecimal().ToString("#,##0.##") + "口";
                    ldQnty = 0;
                    ldFutQnty = 0;
                    ldOptQnty = 0;
                    if (liAll >= 0) ldQnty = dt30017downall.Rows[liAll]["AVG_YEAR"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017downall.Rows[liFut]["AVG_YEAR"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017downall.Rows[liOpt]["AVG_YEAR"].AsDecimal();
                    lsStr = lsStr + "【日均量" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均量" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均量" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)】。";

                    // 夜盤
                    lsStr = lsStr + "；其中夜盤交易量合計" + dt30017downall.Rows[liAll]["AI8_AH_YEAR_QNTY"].AsDecimal().ToString("#,##0.##") + "口，日平均" +
                             dt30017downall.Rows[liAll]["AVG_YEAR_AH"].AsDecimal().ToString("#,##0.##") + "口。";
                    lsStr = lsStr + "\r" + "\n";

                    // OI
                    if (liAll >= 0) ldQnty = dt30017downall.Rows[liAll]["AVG_YEAR_OI"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017downall.Rows[liFut]["AVG_YEAR_OI"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017downall.Rows[liOpt]["AVG_YEAR_OI"].AsDecimal();
                    lsStr = lsStr + "　　 全市場日均OI:" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均OI" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均OI" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)。";
                    lsStr = lsStr + "\r" + "\n";

                    //
                    lsStr = lsStr + "　　 " + (txtSDate.DateTimeValue.Year - 1911 - 1) + "年度同期(交易天數" +
                             dt30017downall.Rows[liAll]["AI8_LAST_YEAR_DAYS"].AsInt() + "天)";
                    lsStr = lsStr + dt30017downall.Rows[liAll]["AI8_LAST_YEAR_QNTY"].AsDecimal().ToString("#,##0.##") + "口";
                    ldQnty = 0;
                    ldFutQnty = 0;
                    ldOptQnty = 0;
                    if (liAll >= 0) ldQnty = dt30017downall.Rows[liAll]["AVG_LAST_YEAR"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017downall.Rows[liFut]["AVG_LAST_YEAR"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017downall.Rows[liOpt]["AVG_LAST_YEAR"].AsDecimal();
                    lsStr = lsStr + "【日均量" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均量" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均量" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)】，";
                    lsStr = lsStr + "\r" + "\n";

                    //
                    lsStr = lsStr + "　　 另達成" + (txtSDate.DateTimeValue.Year - 1911 - 1) + "年度" +
                             dt30017downall.Rows[liAll]["AI8_LAST_ALL_YEAR_DAYS"].AsInt() + "個交易日，";
                    lsStr = lsStr + "全年成交" + dt30017downall.Rows[liAll]["AI8_LAST_ALL_YEAR_QNTY"].AsDecimal().ToString("#,##0.##") + "口";
                    ldQnty = 0;
                    ldFutQnty = 0;
                    ldOptQnty = 0;
                    if (liAll >= 0) ldQnty = dt30017downall.Rows[liAll]["AVG_LAST_ALL_YEAR"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017downall.Rows[liFut]["AVG_LAST_ALL_YEAR"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017downall.Rows[liOpt]["AVG_LAST_ALL_YEAR"].AsDecimal();
                    lsStr = lsStr + "【日均量" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均量" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均量" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)】";
                    ldRate = dt30017downall.Rows[liAll]["AI8_YEAR_QNTY"].AsInt() / dt30017downall.Rows[liAll]["AI8_LAST_ALL_YEAR_QNTY"].AsInt() * 100;
                    lsStr = lsStr + "的" + ldRate.ToString("#0.0#") + "%。";
                    lsStr = lsStr + "\r" + "\n";

                    //
                    ws30017.Cells[rowNum, 0].Value = lsStr;

                    //
                    rowNum = rowNum + 1;
                    lsStr = "註2：";
                    lsStr = lsStr + (txtSDate.DateTimeValue.Year - 1911) + "年" +
                             (txtSDate.DateTimeValue.Month) + "月份預計有" + PbFunc.f_calc_mth_trade_days(asYmd.SubStr(0, 6)) + "個交易日，";
                    lsStr = lsStr + "已交易" + dt30017downall.Rows[liAll]["AI8_MTH_DAYS"].AsInt() + "日，";
                    lsStr = lsStr + "共計成交" + dt30017downall.Rows[liAll]["AI8_MTH_QNTY"].AsDecimal().ToString("#,##0.##") + "口";
                    ldQnty = 0;
                    ldFutQnty = 0;
                    ldOptQnty = 0;
                    if (liAll >= 0) ldQnty = dt30017downall.Rows[liAll]["AVG_MTH"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017downall.Rows[liFut]["AVG_MTH"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017downall.Rows[liOpt]["AVG_MTH"].AsDecimal();
                    lsStr = lsStr + "【日均量" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均量" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均量" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)】。";

                    //夜盤
                    lsStr = lsStr + "；其中夜盤交易量合計" + dt30017downall.Rows[liAll]["AI8_AH_MTH_QNTY"].AsDecimal().ToString("#,##0.##") + "口，日平均" +
                             dt30017downall.Rows[liAll]["AVG_MTH_AH"].AsDecimal().ToString("#,##0.##") + "口。";
                    lsStr = lsStr + "\r" + "\n";

                    //
                    lsStr = lsStr + "　　 " + (txtSDate.DateTimeValue.Year - 1911 - 1) + "年度" + (txtSDate.DateTimeValue.Month) + "月份";
                    lsStr = lsStr + "共計成交" + dt30017downall.Rows[liAll]["AI8_LAST_ALL_MTH_QNTY"].AsDecimal().ToString("#,##0.##") + "口(";
                    ldFutQnty = 0;
                    if (liFut >= 0) ldFutQnty = dt30017downall.Rows[liAll]["AVG_LAST_ALL_MTH"].AsDecimal();
                    if (ldFutQnty > 0) lsStr = lsStr + "日均量" + ldFutQnty.ToString("#,##0.##") + "口)，日均量比值約為" + (ldQnty / ldFutQnty * 100).ToString("#0.0#") + "%。";

                    //
                    ws30017.Cells[rowNum, 0].Value = lsStr;
                    #endregion
                }
                else {
                    #region wf_30017down
                    rptName = "本公司及周邊單位市場交易速報表";
                    rptId = "30017down";

                    // 讀取資料
                    DataTable dt30017down = dao30017.d_30017down(asYmd);
                    if (dt30017down.Rows.Count == 0) {
                        MessageDisplay.Error(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                        return ResultStatus.Fail;
                    }
                    ShowMsg(rptId + '－' + rptName + " 轉檔中...");

                    // 填入資料
                    int f, liDays, liFut, liOpt, liAll;
                    decimal ldFutQnty, ldOptQnty, ldQnty;
                    liAll = -1;
                    liFut = -1;
                    liOpt = -1;
                    for (f = 0; f < dt30017down.Rows.Count; f++) {
                        DataRow dr = dt30017down.Rows[f];
                        switch (dr["AI8_PROD_TYPE"].ToString()) {
                            case " ":
                                liAll = f;
                                break;
                            case "F":
                                liFut = f;
                                break;
                            case "O":
                                liOpt = f;
                                break;

                        }
                    }
                    rowNum = 61;
                    // 預計交易天數
                    lsStr = asYmd.SubStr(0, 4);
                    liDays = dao30017.am7tDayCount(lsStr);
                    ws30017.Cells[rowNum, 20].Value = liDays;
                    ws30017.Cells[rowNum, 16].Value = liDays - dt30017down.Rows[liAll]["AI8_YEAR_DAYS"].AsInt();
                    if (liFut >= 0) {
                        ws30017.Cells[rowNum, 13].SetValue(dt30017down.Rows[liFut]["AVG_MTH"]);
                        ws30017.Cells[rowNum, 17].SetValue(dt30017down.Rows[liFut]["AI8_YEAR_QNTY"]);
                    }
                    if (liOpt >= 0) {
                        ws30017.Cells[rowNum + 1, 13].SetValue(dt30017down.Rows[liOpt]["AVG_MTH"]);
                        ws30017.Cells[rowNum + 1, 17].SetValue(dt30017down.Rows[liOpt]["AI8_YEAR_QNTY"]);
                    }
                    if (liAll >= 0) {
                        ws30017.Cells[rowNum + 2, 13].SetValue(dt30017down.Rows[liAll]["AVG_MTH"]);
                        ws30017.Cells[rowNum + 2, 17].SetValue(dt30017down.Rows[liAll]["AI8_YEAR_QNTY"]);
                    }

                    //
                    rowNum = 57;
                    lsStr = "註1：";
                    lsStr = lsStr + (txtSDate.DateTimeValue.Year - 1911) + "年度預計有" +
                             liDays.AsString() + "個交易日，已交易" +
                             dt30017down.Rows[liAll]["AI8_YEAR_DAYS"].AsInt() + "日，";
                    lsStr = lsStr + "共計成交" + dt30017down.Rows[liAll]["AI8_YEAR_QNTY"].AsDecimal().ToString("#,##0.##") + "口";
                    ldQnty = 0;
                    ldFutQnty = 0;
                    ldOptQnty = 0;
                    if (liAll >= 0) ldQnty = dt30017down.Rows[liAll]["AVG_YEAR"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017down.Rows[liFut]["AVG_YEAR"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017down.Rows[liOpt]["AVG_YEAR"].AsDecimal();
                    lsStr = lsStr + "【日均量" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均量" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均量" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)】。";
                    lsStr = lsStr + "\r" + "\n";

                    //
                    lsStr = lsStr + "　　 " + (txtSDate.DateTimeValue.Year - 1911 - 1) + "年度同期(交易天數" +
                             dt30017down.Rows[liAll]["AI8_LAST_YEAR_DAYS"].AsInt() + "天)";
                    lsStr = lsStr + dt30017down.Rows[liAll]["AI8_LAST_YEAR_QNTY"].AsDecimal().ToString("#,##0.##") + "口";
                    ldQnty = 0;
                    ldFutQnty = 0;
                    ldOptQnty = 0;
                    if (liAll >= 0) ldQnty = dt30017down.Rows[liAll]["AVG_LAST_YEAR"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017down.Rows[liFut]["AVG_LAST_YEAR"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017down.Rows[liOpt]["AVG_LAST_YEAR"].AsDecimal();
                    lsStr = lsStr + "【日均量" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均量" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均量" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)】，";
                    lsStr = lsStr + "\r" + "\n";

                    //
                    lsStr = lsStr + "　　 另達成" + (txtSDate.DateTimeValue.Year - 1911 - 1) + "年度" +
                             dt30017down.Rows[liAll]["AI8_LAST_ALL_YEAR_DAYS"].AsInt() + "個交易日，";
                    lsStr = lsStr + "全年成交" + dt30017down.Rows[liAll]["AI8_LAST_ALL_YEAR_QNTY"].AsDecimal().ToString("#,##0.##") + "口";
                    ldQnty = 0;
                    ldFutQnty = 0;
                    ldOptQnty = 0;
                    if (liAll >= 0) ldQnty = dt30017down.Rows[liAll]["AVG_LAST_ALL_YEAR"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017down.Rows[liFut]["AVG_LAST_ALL_YEAR"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017down.Rows[liOpt]["AVG_LAST_ALL_YEAR"].AsDecimal();
                    lsStr = lsStr + "【日均量" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均量" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均量" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)】。";
                    lsStr = lsStr + "\r" + "\n";

                    //
                    ws30017.Cells[rowNum, 0].Value = lsStr;

                    //
                    rowNum = rowNum + 1;
                    lsStr = "註2：";
                    lsStr = lsStr + (txtSDate.DateTimeValue.Year - 1911) + "年" +
                             (txtSDate.DateTimeValue.Month) + "月份預計有" + PbFunc.f_calc_mth_trade_days(asYmd.SubStr(0, 6)) + "個交易日，";
                    lsStr = lsStr + "已交易" + dt30017down.Rows[liAll]["AI8_MTH_DAYS"].AsInt() + "日，";
                    lsStr = lsStr + "共計成交" + dt30017down.Rows[liAll]["AI8_MTH_QNTY"].AsDecimal().ToString("#,##0.##") + "口";
                    ldQnty = 0;
                    ldFutQnty = 0;
                    ldOptQnty = 0;
                    if (liAll >= 0) ldQnty = dt30017down.Rows[liAll]["AVG_MTH"].AsDecimal();
                    if (liFut >= 0) ldFutQnty = dt30017down.Rows[liFut]["AVG_MTH"].AsDecimal();
                    if (liOpt >= 0) ldOptQnty = dt30017down.Rows[liOpt]["AVG_MTH"].AsDecimal();
                    lsStr = lsStr + "【日均量" + ldQnty.ToString("#,##0.##") + "口(";
                    lsStr = lsStr + "期貨日均量" + ldFutQnty.ToString("#,##0.##") + "口，占比:" + (ldFutQnty / ldQnty * 100).ToString("#0.0#") + "%、";
                    lsStr = lsStr + "選擇權日均量" + ldOptQnty.ToString("#,##0.##") + "口，占比:" + (100 - (ldFutQnty / ldQnty * 100)).ToString("#0.0#") + "%)】。";
                    lsStr = lsStr + "\r" + "\n";

                    //
                    lsStr = lsStr + "　　 " + (txtSDate.DateTimeValue.Year - 1911 - 1) + "年度" + (txtSDate.DateTimeValue.Month) + "月份";
                    lsStr = lsStr + "共計成交" + dt30017down.Rows[liAll]["AI8_LAST_ALL_MTH_QNTY"].AsDecimal().ToString("#,##0.##") + "口(";
                    ldFutQnty = 0;
                    if (liFut >= 0) ldFutQnty = dt30017down.Rows[liAll]["AVG_LAST_ALL_MTH"].AsDecimal();
                    if (ldFutQnty > 0) lsStr = lsStr + "日均量" + ldFutQnty.ToString("#,##0.##") + "口)，日均量比值約為" + (ldQnty / ldFutQnty * 100).ToString("#0.0#") + "%。";

                    //
                    ws30017.Cells[rowNum, 0].Value = lsStr;
                    #endregion
                }

                // 4. 存檔
                ws30017.ScrollToRow(0);
                workbook.SaveDocument(lsFile);
                ShowMsg("轉檔成功");
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            finally {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
            }
            return ResultStatus.Success;
        }

        /// <summary>
        /// 這支功能PB複寫公用的wf_copyfile
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string wfCopyFile(string fileName) {

            string template = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH, fileName + ".xls");
            if (!File.Exists(template)) {
                MessageDisplay.Error("無此檔案「" + template + "」!");
                return "";
            }
            string lsFilename;
            if (cbxAfter.Checked) {
                lsFilename = txtSDate.Text.Replace("/", "") + "本日交易量彙整" + ".xls";
            }
            else {
                lsFilename = fileName + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".xls";
            }
            bool lbChk;
            string file = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, lsFilename);
            lbChk = File.Exists(file);
            if (lbChk) {
                File.Move(file, file + "_bak_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".xls");
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