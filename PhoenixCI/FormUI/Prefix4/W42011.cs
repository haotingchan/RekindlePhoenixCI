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
using System.Threading;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Spreadsheet;

/// <summary>
/// Lukas, 2019/4/11
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 42011 股票期貨風險價格係數檢核表
    /// </summary>
    public partial class W42011 : FormParent {

        private D42011 dao42011;
        private MGR3 daoMGR3;

        public W42011(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao42011 = new D42011();
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.DateTimeValue = DateTime.Now;
            txtSDate.Focus();
            txtRate1Ref.Visible = false;
            txtRate2Ref.Visible = false;
            txtRate3Ref.Visible = false;
            txtRate4Ref.Visible = false;
            lblCmRateRef.Visible = false;

#if DEBUG
            //txtSDate.EditValue = "2018/12/28";
#endif

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            try {
                if (!FlagAdmin) {
                    cbxRate.Visible = false;
                }
                decimal ld_cm_rate1, ld_cm_rate2, ld_cm_rate3;
                //最高%
                ld_cm_rate1 = dao42011.GetCmRate();
                lblCmRate.Text = (ld_cm_rate1 * 100).ToString("#.####");

                //級距1
                DataTable dtRate = dao42011.Get3CmRate();
                ld_cm_rate1 = dtRate.Rows[0]["LD_CM_RATE1"].AsDecimal();
                ld_cm_rate1 = ld_cm_rate1 - 0.01m;
                txtRate1.Text = (ld_cm_rate1 * 100).ToString("#.####");
                //級距2
                ld_cm_rate2 = dtRate.Rows[0]["LD_CM_RATE2"].AsDecimal();
                ld_cm_rate2 = ld_cm_rate2 - 0.01m;
                txtRate2.Text = (ld_cm_rate2 * 100).ToString("#.####");
                //級距3
                ld_cm_rate3 = dtRate.Rows[0]["LD_CM_RATE3"].AsDecimal();
                ld_cm_rate3 = ld_cm_rate3 - 0.01m;
                txtRate3.Text = (ld_cm_rate3 * 100).ToString("#.####");
                //表一達標準判斷
                ld_cm_rate1 = dao42011.GetRange();
                txtRange.Text = (ld_cm_rate1 * 100).ToString("#.####");

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
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                string rptId = "42011", file, rptName = "股票期貨風險價格係數機動評估指標";

                #region ue_export_before
                //判斷資料已轉入
                daoMGR3 = new MGR3();
                int rtn;
                string ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                rtn = daoMGR3.mgr3Count(ymd);
                if (rtn == 0) {
                    DialogResult result = MessageDisplay.Choose(" 當日保證金適用比例資料未轉入完畢,是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }
                //130批次作業做完
                string rtnStr;
                rtnStr = PbFunc.f_chk_130_wf(_ProgramID, txtSDate.DateTimeValue, "5");
                if (rtnStr != "") {
                    DialogResult result = MessageDisplay.Choose(txtSDate.Text + "-" + rtnStr + "，是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }

                //是否勾選任一報表
                if (!cbx1.Checked && !cbx2.Checked && !cbx3.Checked) {
                    MessageDisplay.Error("未勾選任何報表!");
                    return ResultStatus.Fail;
                }
                #endregion

                //取前一交易日
                DateTime ld_last_date = dao42011.GetLastDate(txtSDate.DateTimeValue);
                if (ld_last_date == DateTime.MinValue) {
                    MessageDisplay.Warning(txtSDate.Text + ",讀取前一交易日失敗!");
                    return ResultStatus.Fail;
                }

                //讀取資料(保證金適用比例級距)
                DataTable dt42011 = dao42011.d_42011_detl(txtSDate.DateTimeValue, ld_last_date, txtRange.Text.AsDecimal() / 100,
                                                          txtRate2Ref.Text.AsDecimal() / 100, txtRate3Ref.Text.AsDecimal() / 100, txtRate4Ref.Text.AsDecimal() / 100,
                                                          txtRate1.Text.AsDecimal() / 100, txtRate2.Text.AsDecimal() / 100, txtRate3.Text.AsDecimal() / 100, txtRate4.Text.AsDecimal() / 100);
                if (dt42011.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }
                dt42011.Sort("APDK_KIND_GRP2, APDK_KIND_LEVEL, MGR3_KIND_ID");

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //切換Sheet
                Worksheet ws = workbook.Worksheets[0];
                ws.Cells[0, 15].Value = txtSDate.Text;

                //表1
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                int li_minus;
                li_minus = wf_42011_1(ws, dt42011);
                li_minus = wf_42011_2(li_minus, ws, dt42011);
                li_minus = wf_42011_3(li_minus, ws, dt42011);

                //存檔
                ws.ScrollToRow(0);
                workbook.SaveDocument(file);
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

        protected int wf_42011_1(Worksheet ws, DataTable dt) {
            int ii_ole_row = 2 - 1, li_start_row, li_tot_row, f, li_minus = 0;
            string ls_rpt_name;
            Range range;

            //表1
            li_start_row = 15 - 1;
            li_tot_row = 300;
            ii_ole_row = li_start_row;
            if (!cbx1.Checked) {
                //刪明細
                range = ws.Range[(li_start_row - 4 + 1) + ":" + (li_start_row + li_tot_row + 2 + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + (li_tot_row + 5) + 2;  //5表首,2表尾
                //刪表頭
                li_start_row = 3 - 1;
                range = ws.Range[(li_start_row + 1).AsString()];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + 1;
                //改編號
                f = 1;
                ws.Cells[li_start_row, 1].Value = f.AsString();
                ws.Cells[li_start_row + 1, 1].Value = f.AsString();
                ws.Cells[li_start_row + 2, 1].Value = f.AsString();
                ws.Cells[li_start_row + 3, 1].Value = f.AsString();
                ws.Cells[li_start_row + 4, 1].Value = f.AsString();
                return li_minus;
            }

            if (txtRange.Text != "10") {
                //表首
                ls_rpt_name = ws.Cells[2, 2].Value.AsString();
                f = ls_rpt_name.IndexOf("10%") + 1;
                if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRange.Text + "%" + ls_rpt_name.SubStr(f + 2, ls_rpt_name.Length);
                ws.Cells[2, 2].Value = ls_rpt_name;
                ws.Cells[2, 2].Font.Name = "標楷體";
                ws.Cells[2, 2].Font.Name = "Times New Roman";
                //表頭
                ls_rpt_name = ws.Cells[ii_ole_row - 4, 2].Value.AsString();
                f = ls_rpt_name.IndexOf("10%") + 1;
                if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRange.Text + "%" + ls_rpt_name.SubStr(f + 2, ls_rpt_name.Length);
                ws.Cells[ii_ole_row - 4, 2].Value = ls_rpt_name;
                ws.Cells[ii_ole_row - 4, 2].Font.Name = "標楷體";
                ws.Cells[ii_ole_row - 4, 2].Font.Name = "Times New Roman";
            }

            if (cbxRate.Checked) {
                DataView dv = dt.AsDataView();
                dv.RowFilter = "rpt1_flag='Y'";
                dt = dv.ToTable();
            }
            dt = dt.Sort("T_30_RATE DESC, APDK_KIND_GRP2, APDK_KIND_LEVEL DESC, MGR3_KIND_ID");
            f = 0;
            foreach (DataRow dr in dt.Rows) {
                ii_ole_row++;
                f++;
                ws.Cells[ii_ole_row, 1].Value = f.AsString();
                ws.Cells[ii_ole_row, 2].Value = dr["MGR3_KIND_ID"].AsString();
                ws.Cells[ii_ole_row, 3].Value = dr["APDK_NAME"].AsString();
                ws.Cells[ii_ole_row, 4].Value = dr["MGR3_SID"].AsString();
                ws.Cells[ii_ole_row, 5].Value = dr["PID_NAME"].AsString();
                ws.Cells[ii_ole_row, 6].SetValue(dr["T_30_RATE"]);
                ws.Cells[ii_ole_row, 7].SetValue(dr["MGR2_DAY_RATE"]);
                ws.Cells[ii_ole_row, 8].SetValue(dr["MGR2_DAY_RATE_AVG_1Y"]);
                if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                    ws.Cells[ii_ole_row, 9].Value = "從其高(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)";
                }
                else {
                    ws.Cells[ii_ole_row, 9].Value = dr["MGR3_CUR_LEVEL"].AsString();
                }
                if (dr["DAY_CNT"].AsInt() == 0) {
                    ws.Cells[ii_ole_row, 10].Value = "-";
                }
                else {
                    ws.Cells[ii_ole_row, 10].Value = dr["DAY_CNT"].AsInt();
                }
                ws.Cells[ii_ole_row, 10].Font.Name = "標楷體";
                ws.Cells[ii_ole_row, 10].Font.Name = "Times New Roman";

                ws.Cells[ii_ole_row, 11].SetValue(dr["TFXM1_PRICE"]);
                ws.Cells[ii_ole_row, 12].SetValue(dr["AI5_PRICE"]);
                ws.Cells[ii_ole_row, 13].SetValue(dr["TS_UPDOWN"]);
                ws.Cells[ii_ole_row, 14].SetValue(dr["TI_UPDOWN"]);
                ws.Cells[ii_ole_row, 15].SetValue(dr["YS_UPDOWN"]);
                ws.Cells[ii_ole_row, 16].SetValue(dr["YI_UPDOWN"]);
                ws.Cells[ii_ole_row, 17].SetValue(dr["AI2_OI"]);
                ws.Cells[ii_ole_row, 18].SetValue(dr["AI2_M_QNTY"]);
            }

            //刪除空白列
            //刪"本日無"
            if (dt.Rows.Count > 0) {
                range = ws.Range[(li_start_row - 3 + 1) + ":" + (li_start_row - 3 + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_start_row = li_start_row - 1;
                li_minus = li_minus + 1;
            }
            //刪多餘空白列
            if (dt.Rows.Count < li_tot_row) {
                //沒資料連表頭都刪
                if (dt.Rows.Count == 0) {
                    li_start_row = li_start_row - 3;
                    li_tot_row = li_tot_row + 4;
                    //表頭3列+表尾註1列
                }
                range = ws.Range[(li_start_row + dt.Rows.Count + 1 + 1) + ":" + (li_start_row + li_tot_row + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + (li_tot_row - dt.Rows.Count);
            }
            return li_minus;
        }

        protected int wf_42011_2(int li_minus, Worksheet ws, DataTable dt) {
            int ii_ole_row, li_start_row, li_tot_row, f, li_head_row;
            string ls_rpt_name;
            Range range;

            li_start_row = 322 - 1;
            li_tot_row = 300;
            li_start_row = li_start_row - li_minus;
            ii_ole_row = li_start_row;
            li_head_row = 4 - 1;

            if (!cbx1.Checked) li_head_row = li_head_row - 1;
            if (!cbx2.Checked) {
                //刪明細
                li_start_row = li_start_row - li_minus;
                range = ws.Range[(li_start_row - 4 + 1) + ":" + (li_start_row + li_tot_row + 2 + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + (li_tot_row + 5) + 2;  //5表首,2表尾
                //刪表頭
                range = ws.Range[li_head_row.AsString()];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + 1;
                //改編號
                if (cbx1.Checked) {
                    f = 2;
                }
                else {
                    f = 1;
                }
                ws.Cells[li_head_row, 1].Value = f.AsString();
                ws.Cells[li_head_row + 1, 1].Value = (f + 1).AsString();
                ws.Cells[li_head_row + 2, 1].Value = (f + 2).AsString();
                ws.Cells[li_head_row + 3, 1].Value = (f + 3).AsString();
                return li_minus;
            }

            if (!cbx1.Checked) ws.Cells[ii_ole_row - 4, 1].Value = "1";
            if (txtRange.Text != "8.5" || txtRate2.Text != "10.5" || txtRate3.Text != "13.5" || txtRate4.Text != "1.0" || lblCmRate.Text != "15") {
                //表首
                ls_rpt_name = ws.Cells[li_head_row, 2].Value.AsString();
                if (txtRange.Text != "8.5") {
                    f = ls_rpt_name.IndexOf("8.5 %") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRate1.Text + "%" + ls_rpt_name.SubStr(f + 4, ls_rpt_name.Length);
                }
                if (txtRate2.Text != "10.5") {
                    f = ls_rpt_name.IndexOf("10.5 %") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRate2.Text + "%" + ls_rpt_name.SubStr(f + 5, ls_rpt_name.Length);
                }
                if (txtRate3.Text != "13.5") {
                    f = ls_rpt_name.IndexOf("13.5 %") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRate3.Text + "%" + ls_rpt_name.SubStr(f + 5, ls_rpt_name.Length);
                }
                if (txtRate4.Text != "1.0") {
                    f = ls_rpt_name.IndexOf("1.0 %") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRate4.Text + "%" + ls_rpt_name.SubStr(f + 4, ls_rpt_name.Length);
                }
                if (lblCmRate.Text != "15") {
                    f = ls_rpt_name.IndexOf("15%") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + lblCmRate.Text + "%" + ls_rpt_name.SubStr(f + 4, ls_rpt_name.Length);
                }
                ws.Cells[li_head_row, 2].Value = ls_rpt_name;
                ws.Cells[li_head_row, 2].Font.Name = "標楷體";
                ws.Cells[li_head_row, 2].Font.Name = "Times New Roman";
                //表頭
                ls_rpt_name = ws.Cells[ii_ole_row - 4, 2].Value.AsString();
                if (txtRange.Text != "8.5") {
                    f = ls_rpt_name.IndexOf("8.5 %") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRate1.Text + "%" + ls_rpt_name.SubStr(f + 4, ls_rpt_name.Length);
                }
                if (txtRate2.Text != "10.5") {
                    f = ls_rpt_name.IndexOf("10.5 %") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRate2.Text + "%" + ls_rpt_name.SubStr(f + 5, ls_rpt_name.Length);
                }
                if (txtRate3.Text != "13.5") {
                    f = ls_rpt_name.IndexOf("13.5 %") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRate3.Text + "%" + ls_rpt_name.SubStr(f + 5, ls_rpt_name.Length);
                }
                if (txtRate4.Text != "1.0") {
                    f = ls_rpt_name.IndexOf("1.0 %") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRate4.Text + "%" + ls_rpt_name.SubStr(f + 4, ls_rpt_name.Length);
                }
                if (lblCmRate.Text != "15") {
                    f = ls_rpt_name.IndexOf("15%") + 1;
                    if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + lblCmRate.Text + "%" + ls_rpt_name.SubStr(f + 4, ls_rpt_name.Length);
                }
                ws.Cells[ii_ole_row - 4, 2].Value = ls_rpt_name;
                ws.Cells[ii_ole_row - 4, 2].Font.Name = "標楷體";
                ws.Cells[ii_ole_row - 4, 2].Font.Name = "Times New Roman";
            }
            if (cbxRate.Checked) {
                DataView dv = dt.AsDataView();
                dv.RowFilter = "rpt3_flag='Y'";
                dt = dv.ToTable();
            }
            dt = dt.Sort("MGR2_DAY_RATE DESC, APDK_KIND_GRP2, APDK_KIND_LEVEL DESC, MGR3_KIND_ID");

            f = 0;
            foreach (DataRow dr in dt.Rows) {
                ii_ole_row++;
                f++;
                ws.Cells[ii_ole_row, 1].Value = f.AsString();
                ws.Cells[ii_ole_row, 2].Value = dr["MGR3_KIND_ID"].AsString();
                ws.Cells[ii_ole_row, 3].Value = dr["APDK_NAME"].AsString();
                ws.Cells[ii_ole_row, 4].Value = dr["MGR3_SID"].AsString();
                ws.Cells[ii_ole_row, 5].Value = dr["PID_NAME"].AsString();
                ws.Cells[ii_ole_row, 6].SetValue(dr["MGR2_DAY_RATE"]);
                ws.Cells[ii_ole_row, 7].SetValue(dr["T_30_RATE"]);
                ws.Cells[ii_ole_row, 8].SetValue(dr["MGR2_DAY_RATE_AVG_1Y"]);
                if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                    ws.Cells[ii_ole_row, 9].Value = "從其高(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)";
                }
                else {
                    ws.Cells[ii_ole_row, 9].Value = dr["MGR3_CUR_LEVEL"].AsString();
                }
                if (dr["DAY_CNT_3"].AsInt() == 0) {
                    ws.Cells[ii_ole_row, 10].Value = "-";
                }
                else {
                    ws.Cells[ii_ole_row, 10].Value = dr["DAY_CNT_3"].AsInt();
                }
                ws.Cells[ii_ole_row, 10].Font.Name = "標楷體";
                ws.Cells[ii_ole_row, 10].Font.Name = "Times New Roman";

                ws.Cells[ii_ole_row, 11].SetValue(dr["TFXM1_PRICE"]);
                ws.Cells[ii_ole_row, 12].SetValue(dr["AI5_PRICE"]);
                ws.Cells[ii_ole_row, 13].SetValue(dr["TS_UPDOWN"]);
                ws.Cells[ii_ole_row, 14].SetValue(dr["TI_UPDOWN"]);
                ws.Cells[ii_ole_row, 15].SetValue(dr["YS_UPDOWN"]);
                ws.Cells[ii_ole_row, 16].SetValue(dr["YI_UPDOWN"]);
                ws.Cells[ii_ole_row, 17].SetValue(dr["AI2_OI"]);
                ws.Cells[ii_ole_row, 18].SetValue(dr["AI2_M_QNTY"]);
            }

            //刪除空白列
            //刪"本日無"
            if (dt.Rows.Count > 0) {
                range = ws.Range[(li_start_row - 3 + 1) + ":" + (li_start_row - 3 + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_start_row = li_start_row - 1;
                li_minus = li_minus + 1;
            }
            //刪多餘空白列
            if (dt.Rows.Count < li_tot_row) {
                //沒資料連表頭都刪
                if (dt.Rows.Count == 0) {
                    li_start_row = li_start_row - 3;
                    li_tot_row = li_tot_row + 4;
                    //表頭3列+表尾註1列
                }
                range = ws.Range[(li_start_row + dt.Rows.Count + 1 + 1) + ":" + (li_start_row + li_tot_row + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + (li_tot_row - dt.Rows.Count);
            }

            return li_minus;
        }

        protected int wf_42011_3(int li_minus, Worksheet ws, DataTable dt) {
            int ii_ole_row, li_start_row, li_tot_row, f, li_head_row;
            string ls_rpt_name;
            Range range;

            //3.現貨或期貨連續二日漲跌幅度≧12%之股票期貨(以現貨連續二日漲跌幅度之絕對值由大至小排序)
            li_start_row = 629 - 1;
            li_tot_row = 300;
            li_start_row = li_start_row - li_minus;
            ii_ole_row = li_start_row;
            li_head_row = 5 - 1;
            f = dt.Rows.Count;

            if (!cbx1.Checked) {
                li_head_row--;
                f--;
            }
            if (!cbx2.Checked) {
                li_head_row--;
                f--;
            }
            if (!cbx3.Checked) {
                //刪明細
                range = ws.Range[(li_start_row - 4 + 1) + ":" + (li_start_row + li_tot_row + 2 + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + (li_tot_row + 5);
                //刪表頭
                f = 3;
                if (!cbx1.Checked) f--;
                if (!cbx2.Checked) f--;
                range = ws.Range[li_head_row.AsString()];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + 1;
                //改編號
                ws.Cells[li_head_row, 1].Value = f.AsString();
                ws.Cells[li_head_row + 1, 1].Value = (f + 1).AsString();
                ws.Cells[li_head_row + 2, 1].Value = (f + 2).AsString();
                return li_minus;
            }

            if (!cbx1.Checked) ws.Cells[ii_ole_row - 4, 1].Value = ws.Cells[ii_ole_row - 4, 1].Value.AsInt() - 1;
            if (!cbx2.Checked) ws.Cells[ii_ole_row - 4, 1].Value = ws.Cells[ii_ole_row - 4, 1].Value.AsInt() - 1;

            if (txtUpDown.Text != "12") {
                //表首
                ls_rpt_name = ws.Cells[li_head_row, 2].Value.AsString();
                f = ls_rpt_name.IndexOf("12%") + 1;
                if (f > 0) {
                    ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtUpDown.Text + "%" + ls_rpt_name.SubStr(f + 2, ls_rpt_name.Length);
                    ws.Cells[li_head_row, 2].Value = ls_rpt_name;
                    ws.Cells[li_head_row, 2].Font.Name = "標楷體";
                    ws.Cells[li_head_row, 2].Font.Name = "Times New Roman";
                }
                //表頭
                ls_rpt_name = ws.Cells[ii_ole_row - 4, 2].Value.AsString();
                f = ls_rpt_name.IndexOf("12%") + 1;
                if (f > 0) {
                    ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtUpDown.Text + "%" + ls_rpt_name.SubStr(f + 2, ls_rpt_name.Length);
                    ws.Cells[ii_ole_row - 4, 2].Value = ls_rpt_name;
                    ws.Cells[ii_ole_row - 4, 2].Font.Name = "標楷體";
                    ws.Cells[ii_ole_row - 4, 2].Font.Name = "Times New Roman";
                }
            }
            DataView dv = dt.AsDataView();
            dv.RowFilter = "ABS(YS_UPDOWN * 100) >= "+txtUpDown.Text+" or ABS(YI_UPDOWN*100) >= "+txtUpDown.Text;
            //dv.Sort = "ABS(YS_UPDOWN) DESC, APDK_KIND_GRP2, APDK_KIND_LEVEL DESC, MGR3_KIND_ID";
            dt = dv.ToTable();
            dt.AsEnumerable().OrderByDescending(x => Math.Abs(x.Field<decimal>("YS_UPDOWN")))
                             .ThenBy(x => x.Field<string>("APDK_KIND_GRP2"))
                             .ThenByDescending(x => x.Field<int>("APDK_KIND_LEVEL"))
                             .ThenBy(x => x.Field<string>("MGR3_KIND_ID"));

            f = 0;
            foreach (DataRow dr in dt.Rows) {
                ii_ole_row++;
                f++;
                ws.Cells[ii_ole_row, 1].Value = f.AsString();
                ws.Cells[ii_ole_row, 2].Value = dr["MGR3_KIND_ID"].AsString();
                ws.Cells[ii_ole_row, 3].Value = dr["APDK_NAME"].AsString();
                ws.Cells[ii_ole_row, 4].Value = dr["MGR3_SID"].AsString();
                ws.Cells[ii_ole_row, 5].Value = dr["PID_NAME"].AsString();
                ws.Cells[ii_ole_row, 6].SetValue(dr["TFXM1_PRICE"]);
                ws.Cells[ii_ole_row, 7].SetValue(dr["AI5_PRICE"]);
                ws.Cells[ii_ole_row, 8].SetValue(dr["TS_UPDOWN"]);
                ws.Cells[ii_ole_row, 9].SetValue(dr["TI_UPDOWN"]);
                ws.Cells[ii_ole_row, 10].SetValue(dr["YS_UPDOWN"]);
                ws.Cells[ii_ole_row, 11].SetValue(dr["YI_UPDOWN"]);
                ws.Cells[ii_ole_row, 12].SetValue(dr["T_30_RATE"]);
                ws.Cells[ii_ole_row, 13].SetValue(dr["MGR2_DAY_RATE"]);
                if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                    ws.Cells[ii_ole_row, 14].Value = "從其高(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)";
                }
                else {
                    ws.Cells[ii_ole_row, 14].Value = dr["MGR3_CUR_LEVEL"].AsString();
                }
                ws.Cells[ii_ole_row, 14].Font.Name = "標楷體";
                ws.Cells[ii_ole_row, 14].Font.Name = "Times New Roman";

                ws.Cells[ii_ole_row, 15].SetValue(dr["AI2_OI"]);
                ws.Cells[ii_ole_row, 16].SetValue(dr["AI2_M_QNTY"]);
            }

            //刪除空白列
            //刪"本日無"
            if (dt.Rows.Count > 0) {
                range = ws.Range[(li_start_row - 3 + 1) + ":" + (li_start_row - 3 + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_start_row = li_start_row - 1;
                li_minus = li_minus + 1;
            }
            //刪多餘空白列
            if (dt.Rows.Count < li_tot_row) {
                //沒資料連表頭都刪
                if (dt.Rows.Count == 0) {
                    li_start_row = li_start_row - 3;
                    li_tot_row = li_tot_row + 3;
                }
                range = ws.Range[(li_start_row + dt.Rows.Count + 1 + 1) + ":" + (li_start_row + li_tot_row + 1)];
                range.Delete(DeleteMode.EntireRow);
                li_minus = li_minus + (li_tot_row - dt.Rows.Count);
            }
            return li_minus;
        }
    }
}