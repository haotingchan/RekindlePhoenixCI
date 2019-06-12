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
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using System.Threading;

/// <summary>
/// Lukas, 2019/3/5
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30040 國內外期貨市場比較表
    /// </summary>
    public partial class W30040 : FormParent {

        private D30040 dao30040;

        public W30040(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtEDate.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
            txtSDate.EditValue = txtEDate.EditValue;
            txtSDate.Focus();
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
                // 1. ue_export_before
                if (txtEDate.DateTimeValue.Year != txtSDate.DateTimeValue.Year) {
                    MessageDisplay.Error("不可跨年度查詢!");
                    ShowMsg("");
                    txtSDate.Focus();
                    return ResultStatus.Fail;
                }

                // 2. ue_export
                string rptId = "30040", file;

                // 2.1 複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") {
                    ShowMsg("無法複製template");
                    return ResultStatus.Fail;
                }

                // 2.2 開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                /******************
                SGX-DT(SIMEX)
                ******************/
                int rowNum = 1;

                #region wf_30040_1
                string rptName, ymd;
                int f, rowTol, rowStart;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                ls_ymd = 日期
                ls_col_name = 欄位名稱
                li_ole_row_tol = Excel的Column預留數
                li_ole_row_start = Excel的Start Row
                li_ole_row_end = Excel的End Row
                *************************************/
                rptName = "當月每日總量表";
                rptId = "30040_1";
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                // 切換sheet
                Worksheet ws30040SGX = workbook.Worksheets[0];
                rowStart = rowNum;
                rowTol = rowNum + 300;

                // 讀取並填入資料
                /******************
                (1) 臺指期貨
                ******************/
                dao30040 = new D30040();
                string symd = txtSDate.Text.Replace("/", "");
                string eymd = txtEDate.Text.Replace("/", "");
                DataTable dt300401 = dao30040.d_30040_1(symd + "01", eymd + "31");
                if (dt300401.Rows.Count == 0) {
                    MessageDisplay.Info(symd + "01", eymd + "31" + "," + rptId + '－' + rptName + ",無任何資料!");
                    ShowMsg("");
                    return ResultStatus.Fail;
                }
                ws30040SGX.Cells[rowTol + 1, 0].Value = "合計(" + (txtEDate.DateTimeValue.Year - 1911).AsString() + '/' + txtEDate.DateTimeValue.Month.AsString() + ')';
                ymd = "";
                foreach (DataRow dr in dt300401.Rows) {
                    ymd = dr["AI2_YMD"].AsString();
                    rowNum = rowNum + 1;
                    ws30040SGX.Cells[rowNum, 0].Value = ymd.SubStr(4, 2) + "/" + ymd.SubStr(6, 2);
                    if (dr["AI2_M_QNTY"] != DBNull.Value) ws30040SGX.Cells[rowNum, 1].Value = dr["AI2_M_QNTY"].AsDecimal();
                    if (dr["AI2_OI"] != DBNull.Value) ws30040SGX.Cells[rowNum, 2].Value = dr["AI2_OI"].AsDecimal();
                    if (dr["A_QNTY"] != DBNull.Value) ws30040SGX.Cells[rowNum, 5].Value = dr["A_QNTY"].AsDecimal();
                    if (dr["N_QNTY"] != DBNull.Value) ws30040SGX.Cells[rowNum, 4].Value = dr["N_QNTY"].AsDecimal();
                    if (dr["A_OI"] != DBNull.Value) ws30040SGX.Cells[rowNum, 7].Value = dr["A_OI"].AsDecimal();
                }

                // 刪除空白列
                if (rowTol > rowNum) {
                    ws30040SGX.Rows.Remove(rowNum + 1, rowTol - rowNum);
                }
                #endregion

                rowNum = rowNum + 6;

                #region wf_30040_2
                rptName = "當年每月日均量表";
                rptId = "30040_2";
                rowTol = rowNum + 12;
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                // 讀取並填入資料
                /****************************
                (1) 臺指期貨
                ****************************/
                /******************
                每月
                ******************/
                DataTable dt300402 = dao30040.d_30040_2(symd.SubStr(0, 4) + "01", eymd);
                if (dt300402.Rows.Count == 0) {
                    MessageDisplay.Info(symd.SubStr(0, 4) + "01", eymd + "," + rptId + '－' + rptName + ",無任何資料!");
                    ShowMsg("");
                    return ResultStatus.Fail;
                }
                /******************
                (2) 摩根台指
                ******************/
                // PB宣告了d_30040_stw，但沒有用到

                foreach (DataRow dr in dt300402.Rows) {
                    rowNum = rowNum + 1;
                    ws30040SGX.Cells[rowNum, 0].Value = (dr["AI2_YMD"].AsString().SubStr(0, 4).AsInt() - 1911).AsString() + "/" + dr["AI2_YMD"].AsString().SubStr(4, 2);
                    if (dr["AVG_QNTY"] != DBNull.Value) ws30040SGX.Cells[rowNum, 1].Value = dr["AVG_QNTY"].AsDecimal();
                    if (dr["AVG_OI"] != DBNull.Value) ws30040SGX.Cells[rowNum, 2].Value = dr["AVG_OI"].AsDecimal();
                    if (dr["AVG_N_QNTY"] != DBNull.Value) ws30040SGX.Cells[rowNum, 4].Value = dr["AVG_N_QNTY"].AsDecimal();
                    if (dr["AVG_A_QNTY"] != DBNull.Value) ws30040SGX.Cells[rowNum, 5].Value = dr["AVG_A_QNTY"].AsDecimal();
                    if (dr["AVG_STW_OI"] != DBNull.Value) ws30040SGX.Cells[rowNum, 7].Value = dr["AVG_STW_OI"].AsDecimal();
                }

                // 本年度日均量
                // cp_tot_avg_qnty
                decimal sumAi2MQnty = dt300402.Compute("sum(ai2_m_qnty)", "").AsDecimal();
                decimal sumAi2DayCount = dt300402.Compute("sum(ai2_day_count)", "").AsDecimal();
                decimal cpTotAvgQnty = Math.Round(sumAi2MQnty / sumAi2DayCount, MidpointRounding.AwayFromZero);
                ws30040SGX.Cells[rowTol + 1, 1].Value = cpTotAvgQnty;
                // cp_tot_avg_oi
                decimal sumAi2OI = dt300402.Compute("sum(ai2_oi)", "").AsDecimal();
                decimal cpTotAvgOI = Math.Round(sumAi2OI / sumAi2DayCount, MidpointRounding.AwayFromZero);
                ws30040SGX.Cells[rowTol + 1, 2].Value = cpTotAvgOI;
                // cp_tot_avg_n_qnty
                decimal sumNQnty = dt300402.Compute("sum(n_qnty)", "").AsDecimal();
                decimal cpTotStwDayCount = dt300402.Compute("sum(a_day_count)", "").AsDecimal();
                decimal cpTotAvgNQnty = Math.Round(sumNQnty / cpTotStwDayCount, MidpointRounding.AwayFromZero);
                ws30040SGX.Cells[rowTol + 1, 4].Value = cpTotAvgNQnty;
                // cp_tot_avg_a_qnty
                decimal sumAQnty = dt300402.Compute("sum(a_qnty)", "").AsDecimal();
                decimal cpTotAvgAQnty = Math.Round(sumAQnty / cpTotStwDayCount, MidpointRounding.AwayFromZero);
                ws30040SGX.Cells[rowTol + 1, 5].Value = cpTotAvgAQnty;
                // cp_tot_avg_stw_oi
                decimal sumStwOI = dt300402.Compute("sum(stw_oi)", "").AsDecimal();
                decimal cpTotAvgStwOI = Math.Round(sumStwOI / cpTotStwDayCount, MidpointRounding.AwayFromZero);
                ws30040SGX.Cells[rowTol + 1, 7].Value = cpTotAvgStwOI;

                // 刪除空白列
                if (rowTol > rowNum) {
                    ws30040SGX.Rows.Remove(rowNum + 1, rowTol - rowNum);
                }
                #endregion

                /******************
                JPX vs TX
                ******************/
                rowNum = 1;

                #region wf_30040_3
                rptName = "JPX當月每日總量表";
                rptId = "30040_3";
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                // 切換Sheet
                Worksheet ws30040JPX = workbook.Worksheets[1];
                rowStart = rowNum;
                rowTol = rowNum + 300;

                // 讀取並填入資料
                /******************
                (1) 臺指期貨
                ******************/
                DataTable dt300403 = dao30040.d_30040_3(symd + "01", eymd + "31");
                if (dt300403.Rows.Count == 0) {
                    MessageDisplay.Info(symd + "01", eymd + "31" + "," + rptId + '－' + rptName + ",無任何資料!");
                    ShowMsg("");
                    return ResultStatus.Fail;
                }
                ws30040JPX.Cells[rowTol + 1, 0].Value = "合計(" + (txtEDate.DateTimeValue.Year - 1911).AsString() + '/' + txtEDate.DateTimeValue.Month.AsString() + ')';
                foreach (DataRow dr in dt300403.Rows) {
                    ymd = dr["AI2_YMD"].AsString();
                    rowNum = rowNum + 1;
                    ws30040JPX.Cells[rowNum, 0].Value = ymd.SubStr(4, 2) + "/" + ymd.SubStr(6, 2);
                    if (dr["TX_QNTY"] != DBNull.Value) ws30040JPX.Cells[rowNum, 1].Value = dr["TX_QNTY"].AsDecimal();
                    if (dr["TX_OI"] != DBNull.Value) ws30040JPX.Cells[rowNum, 2].Value = dr["TX_OI"].AsDecimal();
                    if (dr["J_QNTY"] != DBNull.Value) ws30040JPX.Cells[rowNum, 4].Value = dr["J_QNTY"].AsDecimal();
                    if (dr["J_OI"] != DBNull.Value) ws30040JPX.Cells[rowNum, 5].Value = dr["J_OI"].AsDecimal();
                }

                // 刪除空白列
                if (rowTol > rowNum) {
                    ws30040JPX.Rows.Remove(rowNum + 1, rowTol - rowNum);
                }
                #endregion

                rowNum = rowNum + 6;

                #region wf_30040_4
                rptName = "JPX當年每月日均量表";
                rptId = "30040_4";
                rowTol = rowNum + 12;
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                // 讀取並填入資料
                /******************
                (1) 臺指期貨
                ******************/
                DataTable dt300404 = dao30040.d_30040_4(symd.SubStr(0, 4) + "01", eymd);
                if (dt300404.Rows.Count == 0) {
                    MessageDisplay.Info(symd.SubStr(0, 4) + "01", eymd + "," + rptId + '－' + rptName + ",無任何資料!");
                    ShowMsg("");
                    return ResultStatus.Fail;
                }
                foreach (DataRow dr in dt300404.Rows) {
                    rowNum = rowNum + 1;
                    ws30040JPX.Cells[rowNum, 0].Value = (dr["AI2_YMD"].AsString().SubStr(0, 4).AsInt() - 1911).AsString() + "/" + dr["AI2_YMD"].AsString().SubStr(4, 2);
                    if (dr["AVG_QNTY"] != DBNull.Value) ws30040JPX.Cells[rowNum, 1].Value = dr["AVG_QNTY"].AsDecimal();
                    if (dr["AVG_OI"] != DBNull.Value) ws30040JPX.Cells[rowNum, 2].Value = dr["AVG_OI"].AsDecimal();
                    if (dr["AVG_J_QNTY"] != DBNull.Value) ws30040JPX.Cells[rowNum, 4].Value = dr["AVG_J_QNTY"].AsDecimal();
                    if (dr["AVG_J_OI"] != DBNull.Value) ws30040JPX.Cells[rowNum, 5].Value = dr["AVG_J_OI"].AsDecimal();
                }

                // 本年度日均量
                // cp_tot_avg_qnty
                decimal sumTxQnty = dt300404.Compute("sum(tx_qnty)", "").AsDecimal();
                decimal sumAi2DayCountJPX = dt300404.Compute("sum(ai2_day_count)", "").AsDecimal();
                decimal cpTotAvgQntyJPX = Math.Round(sumTxQnty / sumAi2DayCountJPX, MidpointRounding.AwayFromZero);
                ws30040JPX.Cells[rowTol + 1, 1].Value = cpTotAvgQntyJPX;
                // cp_tot_avg_oi
                decimal sumTxOI = dt300404.Compute("sum(tx_oi)", "").AsDecimal();
                decimal cpTotAvgOIJPX = Math.Round(sumTxOI / sumAi2DayCountJPX, MidpointRounding.AwayFromZero);
                ws30040JPX.Cells[rowTol + 1, 2].Value = cpTotAvgOIJPX;
                // cp_tot_avg_j_qnty
                decimal sumJQnty = dt300404.Compute("sum(j_qnty)", "").AsDecimal();
                decimal sumJDayCount = dt300404.Compute("sum(j_day_count)", "").AsDecimal();
                decimal cpTotAvgJQnty = Math.Round(sumJQnty / sumJDayCount, MidpointRounding.AwayFromZero);
                ws30040JPX.Cells[rowTol + 1, 4].Value = cpTotAvgJQnty;
                // cp_tot_avg_j_oi
                decimal sumJOI = dt300404.Compute("sum(j_oi)", "").AsDecimal();
                decimal cpTotAvgJOI = Math.Round(sumJOI / sumJDayCount, MidpointRounding.AwayFromZero);
                ws30040JPX.Cells[rowTol + 1, 5].Value = cpTotAvgJOI;

                // 刪除空白列
                if (rowTol > rowNum) {
                    ws30040JPX.Rows.Remove(rowNum + 1, rowTol - rowNum);
                }
                #endregion

                // 存檔
                ws30040JPX.ScrollToRow(0);
                ws30040SGX.ScrollToRow(0);
                workbook.SaveDocument(file);
                ShowMsg("轉檔成功");
            }
            catch (Exception ex) {
                ShowMsg("轉檔錯誤");
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
    }
}