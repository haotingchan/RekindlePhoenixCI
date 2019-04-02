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
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;

/// <summary>
/// Lukas, 2019/2/27
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30030 當月,當年度日均量表
    /// </summary>
    public partial class W30030 : FormParent {

        private RPT daoRPT;
        private D30030 dao30030;

        public W30030(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtEDate.EditValue = PbFunc.f_ocf_date(0).SubStr(0, 7);
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

        protected override ResultStatus Export() {
            // 1. ue_export_before
            if (txtEDate.DateTimeValue.Year != txtSDate.DateTimeValue.Year) {
                MessageDisplay.Error("不可跨年度查詢!");
                txtSDate.Focus();
                return ResultStatus.Fail;
            }

            // 2. ue_export
            string rptId = "30030", file;

            // 2.1 複製檔案
            file = PbFunc.wf_copy_file(rptId, rptId);
            if (file == "") {
                return ResultStatus.Fail;
            }

            // 2.2 開啟檔案
            Workbook workbook = new Workbook();
            workbook.LoadDocument(file);

            // 2.3 匯出Excel
            int rowKeep, rowNum;
            rowNum = 1;

            #region wf_30031
            string rptName, kindId;
            int f, g, colNum, rowTol, found;
            /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號
            li_ole_col = Excel的Column位置
            li_ole_row_tol = Excel的Column預留數
            ls_kind_id = param_key
            *************************************/
            rptName = "當年每月日均量統計表";
            rptId = "30031";

            // RPT
            daoRPT = new RPT();
            DataTable dtRPT = daoRPT.ListAllByTXD_ID(rptId);
            if (dtRPT.Rows.Count == 0) {
                MessageDisplay.Error(rptId + '－' + "RPT無任何資料!");
                return ResultStatus.Fail;
            }
            // 讀取資料
            dao30030 = new D30030();
            string symd = txtSDate.Text.Replace("/", "").Trim();
            string eymd = txtEDate.Text.Replace("/", "").Trim();
            DataTable dt30031 = dao30030.d_30031(symd + "01", eymd + "31");
            if (dt30031.Rows.Count == 0) {
                MessageDisplay.Info(eymd + "," + rptId + '－' + rptName + ",無任何資料!");
                return ResultStatus.Fail;
            }
            dt30031.Filter("RPT_SEQ_NO > 0");

            // 切換Sheet，總共只有一個sheet
            Worksheet ws30030 = workbook.Worksheets[0];

            // 填入資料
            rowTol = rowNum + 302;
            ws30030.Cells[rowTol + 2, 0].Value = txtEDate.Text.SubStr(5, 2) + "月百分比(%)";
            string rptType, ymd = "", sumType;
            rptType = "";
            g = 0;
            for (f = 0; f < dt30031.Rows.Count; f++) {
                DataRow dr = dt30031.Rows[f];
                if (ymd != dr["AI2_YMD"].AsString()) {
                    ymd = dr["AI2_YMD"].AsString();
                    rowNum = rowNum + 1;
                    ws30030.Cells[rowNum, 0].Value = ymd.SubStr(4, 2) + "/" + ymd.SubStr(6, 2);
                }
                kindId = dr["AI2_PARAM_KEY"].AsString();
                colNum = dr["RPT_SEQ_NO"].AsInt() - 1;
                ws30030.Cells[rowNum, colNum].Value = dr["AI2_M_QNTY"].AsDecimal();
                /* 未沖銷量 */
                DataRow[] find = dtRPT.Select("rpt_value like 'OI%'");
                if (find.Length != 0) {
                    found = dtRPT.Rows.IndexOf(find[0]);
                }
                else {
                    found = -1;
                }
                if (found >= 0) {
                    found = dtRPT.Rows[found]["RPT_SEQ_NO"].AsInt();
                    ws30030.Cells[rowNum, found - 1].Value = dt30031.Compute("sum(AI2_OI)", $@"AI2_YMD='{ymd}'").AsDecimal();
                }
            }
            ws30030.Cells[rowTol + 1, 0].Value = "合計(" + (txtEDate.DateTimeValue.Year - 1911).AsString() + '/' + txtEDate.DateTimeValue.Month.AsString() + ')';

            // 刪除空白列
            if (rowTol > rowNum) {
                ws30030.Rows.Remove(rowNum + 1, rowTol - rowNum);
            }
            #endregion

            rowNum = rowNum + 6;

            #region wf_30032
            rptName = "當年每月日均量統計表";
            rptId = "30032";

            // 讀取資料
            DataTable dt30032 = dao30030.d_30032("M", symd.SubStr(0, 4) + "01", eymd);
            if (dt30032.Rows.Count == 0) {
                //PB這邊註解掉
                //MessageDisplay.Info(eymd + "," + rptId + '－' + rptName + ",無任何資料!");
                //return ResultStatus.Fail;
            }
            dt30032.Filter("RPT_SEQ_NO > 0");
            /* OI */
            DataTable dt30032OI = dao30030.d_30032_OI(symd.SubStr(0, 4) + "01", eymd);
            if (dt30032OI.Rows.Count == 0) {
                //PB這邊註解掉
                //MessageDisplay.Info(eymd + "," + rptId + '－' + rptName + ",無任何資料!");
                //return ResultStatus.Fail;
            }
            int colTol = dao30030.colTol() - 1;
            rowTol = rowNum + 12;

            //填入資料
            int row = 0;
            ymd = "";
            g = 0;
            found = 0;
            for (f = 0; f < dt30032.Rows.Count; f++) {
                DataRow dr = dt30032.Rows[f];
                if (ymd != dr["AI2_YMD"].AsString()) {
                    ymd = dr["AI2_YMD"].AsString();
                    if (ymd == "999999") {
                        row = rowTol + 1;
                    }
                    else {
                        rowNum = rowNum + 1;
                        row = rowNum;
                        ws30030.Cells[row, 0].Value = (ymd.SubStr(0, 4).AsInt() - 1911).AsString() + "/" + ymd.SubStr(4, 2);
                    }
                    /* 未沖銷量 */
                    if (ymd == "999999") {
                        colNum = dt30032OI.Rows[found]["RPT_SEQ_NO"].AsInt() - 1;
                        decimal sumAI2OI = dt30032OI.Compute("sum(AI2_OI)", "").AsDecimal();
                        decimal sumAI2DayCount = dt30032OI.Compute("sum(AI2_DAY_COUNT)", "").AsDecimal();
                        if (sumAI2DayCount == 0) {
                            ws30030.Cells[row, colNum].Value = 0;
                        }
                        else {
                            decimal sumAvgOI = Math.Round(sumAI2OI / sumAI2DayCount, MidpointRounding.AwayFromZero);
                            ws30030.Cells[row, colNum].Value = sumAvgOI;
                        }
                    }
                    else {
                        DataRow[] find = dt30032OI.Select("ai2_ymd ='" + ymd + "'");
                        if (find.Length != 0) {
                            found = dt30032OI.Rows.IndexOf(find[0]);
                        }
                        else {
                            found = -1;
                        }
                        if (found >= 0) {
                            colNum = dt30032OI.Rows[found]["RPT_SEQ_NO"].AsInt() - 1;
                            decimal AI2OI = dt30032OI.Rows[found]["AI2_OI"].AsDecimal();
                            decimal AI2DayCount = dt30032OI.Rows[found]["AI2_DAY_COUNT"].AsDecimal();
                            if (AI2DayCount == 0) {
                                ws30030.Cells[row, colNum].Value = 0;
                            }
                            else {
                                decimal avgOI = Math.Round(AI2OI / AI2DayCount,MidpointRounding.AwayFromZero);
                                ws30030.Cells[row, colNum].Value = avgOI;
                            }
                        }
                    }
                    colNum = colTol;
                    if (colNum > 0) {
                        decimal mQntyYM = dt30032.Compute($@"sum(ai2_m_qnty)", $@"AI2_YMD='{ymd}'").AsDecimal() -
                                      dt30032.Compute($@"sum(ai2_m_qnty)", $@"AI2_YMD='{ymd}' and substring(AI2_PARAM_KEY,1,3)='SUM'").AsDecimal();
                        decimal dayCountYM = dt30032.Compute("max(ai2_day_count)", $@"AI2_YMD='{ymd}'").AsDecimal();
                        ws30030.Cells[row, colNum].Value = Math.Round(mQntyYM / dayCountYM, MidpointRounding.AwayFromZero);
                    }
                }
                kindId = dr["AI2_PARAM_KEY"].AsString();
                colNum = dr["RPT_SEQ_NO"].AsInt() - 1;
                if (kindId == "RHF") {
                    colNum = colNum;
                }
                decimal ai2MQnty = dr["AI2_M_QNTY"].AsDecimal();
                decimal ai2DayCount = dr["AI2_DAY_COUNT"].AsDecimal();
                ws30030.Cells[row, colNum].Value = Math.Round(ai2MQnty / ai2DayCount, MidpointRounding.AwayFromZero);
            }
            // 刪除空白列
            if (rowTol > rowNum) {
                ws30030.Rows.Remove(rowNum + 1, rowTol - rowNum);
            }
            #endregion

            rowNum = rowNum + 4;
            rowKeep = rowNum;

            #region wf_30033
            int dayCount;
            decimal taifex, tse, sgxDt;
            rowTol = 0;
            rptName = "當年每月TAIFEX期貨與TSE成交值之比較表";
            rptId = "30033";

            // 讀取資料
            DataTable dt30033 = dao30030.d_30033(symd.SubStr(0, 4) + "01", eymd);
            if (dt30033.Rows.Count == 0) {
                MessageDisplay.Info(symd.SubStr(0, 4) + "01～" + eymd + "," + rptId + '－' + rptName + ",無任何資料!");
                return ResultStatus.Fail;
            }

            // 填入資料
            rowTol = rowNum + 1 + 11;
            /* 明細 */
            for (f = 0; f < dt30033.Rows.Count; f++) {
                DataRow dr = dt30033.Rows[f];
                rowNum = rowNum + 1;
                taifex = dr["AA1_TAIFEX"].AsDecimal();
                tse = dr["AA1_TSE"].AsDecimal();
                sgxDt = dr["AA1_SGX_DT"].AsDecimal();
                dayCount = dr["AA1_DAY_COUNT"].AsInt();
                ws30030.Cells[rowNum, 0].Value = dr["AA1_YM"].AsString();
                ws30030.Cells[rowNum, 1].Value = taifex;
                ws30030.Cells[rowNum, 2].Value = tse;
                ws30030.Cells[rowNum, 4].Value = dayCount;
            }

            // 刪除空白列
            if (rowTol > rowNum) {
                ws30030.Rows.Remove(rowNum + 1, rowTol - rowNum);
            }
            #endregion

            rowTol = rowNum + 1;
            rowNum = rowKeep;

            #region wf_30034
            string ym;
            decimal value;
            rptName = "當年每月TAIFEX期貨與TSE成交值之比較表";
            rptId = "30034";

            // 讀取資料
            // 計算當月最後一日
            if (eymd.SubStr(4, 2) == "12") {
                ym = (eymd.SubStr(4, 2).AsInt() + 1).AsString() + "01";
            }
            else {
                ym = eymd.SubStr(0, 4) + ("0" + (eymd.SubStr(4, 2).AsInt() + 1).AsString()).SubStr(1, 2);
            }
            ym = PbFunc.relativedate((eymd.SubStr(0, 4) + "/" + eymd.SubStr(4, 2) + "/01").AsDateTime("yyyy/MM/dd"), -1).ToString("yyyyMMdd");
            DataTable dt30034 = dao30030.d_30034(txtSDate.Text.SubStr(0, 4) + "0101",
                                                 ym,
                                                 txtSDate.Text.SubStr(0, 4) + "01",
                                                 eymd);
            if (dt30034.Rows.Count == 0) {
                MessageDisplay.Info(eymd.SubStr(0, 4) + "0101～" + ym + "," + rptId + '－' + rptName + ",無任何資料!");
                return ResultStatus.Fail;
            }

            // 每月
            for (f = rowNum + 1; f < rowTol; f++) {
                ym = ws30030.Cells[f, 0].Value.AsString();
                DataRow[] find = dt30034.Select("trim(stw_ymd) ='" + ym + "'");
                if (find.Length != 0) {
                    found = dt30034.Rows.IndexOf(find[0]);
                }
                else {
                    found = -1;
                }
                if (found >= 0) {
                    decimal stwAmt = dt30034.Rows[found]["STW_AMT"].AsDecimal();
                    decimal aa1UsRate = dt30034.Rows[found]["AA1_US_RATE"].AsDecimal();
                    decimal stwDays = dt30034.Rows[found]["STW_DAYS"].AsDecimal();
                    ws30030.Cells[f, 6].Value = stwAmt * aa1UsRate / stwDays / 100000000;
                }
            }

            // 本年度
            decimal sumC = dt30034.Compute("sum(cp_c)", "").AsDecimal();
            decimal sumStwDays = dt30034.Compute("sum(stw_days)", "").AsDecimal();
            value = sumC / sumStwDays / 100000000;
            ws30030.Cells[rowTol, 6].Value = value;
            #endregion

            workbook.SaveDocument(file);
            return ResultStatus.Success;
        }
    }
}