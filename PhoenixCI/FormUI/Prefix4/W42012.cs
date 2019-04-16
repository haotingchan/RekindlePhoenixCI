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
using DataObjects.Dao.Together.TableDao;
using Common;
using System.Threading;
using BaseGround.Shared;
using DevExpress.Spreadsheet;

/// <summary>
/// Lukas, 2019/4/15
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

    /// <summary>
    /// 42012 股票期貨風險價格係數分析表
    /// </summary>
    public partial class W42012 : FormParent {

        private D42011 dao42011;//有幾段共用的sql
        private D42012 dao42012;

        public W42012(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao42012 = new D42012();
            dao42011 = new D42011();
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtEDate.DateTimeValue = DateTime.Now;
            txtEDate.Focus();
            txtSDate.DateTimeValue = txtEDate.DateTimeValue;
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
                lblCmRateRef.Text = (ld_cm_rate1 * 100).ToString("#.####");
                lblCmRate.Text = (ld_cm_rate1 * 100).ToString("#.####");

                //級距1
                DataTable dtRate = dao42011.Get3CmRate();
                ld_cm_rate1 = dtRate.Rows[0]["LD_CM_RATE1"].AsDecimal();
                ld_cm_rate1 = ld_cm_rate1 - 0.01m;
                txtRate1Ref.Text = (ld_cm_rate1 * 100).ToString("#.####");
                txtRate1.Text = (ld_cm_rate1 * 100).ToString("#.####");
                //級距2
                ld_cm_rate2 = dtRate.Rows[0]["LD_CM_RATE2"].AsDecimal();
                ld_cm_rate2 = ld_cm_rate2 - 0.01m;
                txtRate2Ref.Text = (ld_cm_rate2 * 100).ToString("#.####");
                txtRate2.Text = (ld_cm_rate2 * 100).ToString("#.####");
                //級距3
                ld_cm_rate3 = dtRate.Rows[0]["LD_CM_RATE3"].AsDecimal();
                ld_cm_rate3 = ld_cm_rate3 - 0.01m;
                txtRate3Ref.Text = (ld_cm_rate3 * 100).ToString("#.####");
                txtRate3.Text = (ld_cm_rate3 * 100).ToString("#.####");

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
                if (txtSID.Text.AsString() == "") {
                    MessageDisplay.Error("請輸入標的證券代號");
                    return ResultStatus.Fail;
                }
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                string rptId = "42012", file, ls_rpt_name = "股票期貨風險價格係數分析表";

                //讀取資料(保證金適用比例級距)
                DataTable dt42012 = dao42012.d_42012_detl(txtSDate.DateTimeValue.ToString("yyyyMMdd"), txtEDate.DateTimeValue.ToString("yyyyMMdd"), txtSID.Text.AsString(),
                                          txtRange.Text.AsDecimal() / 100, txtRate2Ref.Text.AsDecimal() / 100, txtRate3Ref.Text.AsDecimal() / 100, txtRate4Ref.Text.AsDecimal() / 100,
                                          txtRate1.Text.AsDecimal() / 100, txtRate2.Text.AsDecimal() / 100, txtRate3.Text.AsDecimal() / 100, txtRate4.Text.AsDecimal() / 100);
                if (dt42012.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "～" + txtEDate.Text + "," + rptId + '－' + ls_rpt_name + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //切換Sheet
                Worksheet ws = workbook.Worksheets[0];
                ws.Cells[0, 15].Value = txtSDate.Text + "～" + txtEDate.Text;

                //填資料
                //表首
                int f, ii_ole_row, li_start_row = 3 - 1, li_tot_row = 300, li_minus = 0;
                Range range;
                if (cbx1.Checked) {
                    if (txtRange.Text != "10") {
                        //表首
                        ls_rpt_name = ws.Cells[2, 2].Value.AsString();
                        f = ls_rpt_name.IndexOf("10%") + 1;
                        if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRange.Text + "%" + ls_rpt_name.SubStr(f + 2, ls_rpt_name.Length);
                        ws.Cells[li_start_row, 2].Value = ls_rpt_name;
                        ws.Cells[li_start_row, 2].Font.Name = "標楷體";
                        ws.Cells[li_start_row, 2].Font.Name = "Times New Roman";
                    }
                }
                else {
                    range = ws.Range[(li_start_row + 1).AsString()];
                    range.Delete(DeleteMode.EntireRow);
                    li_minus = li_minus + 1;
                    //改編號
                    ws.Cells[li_start_row, 1].Value = "1";
                    ws.Cells[li_start_row + 1, 1].Value = "2";
                    ws.Cells[li_start_row + 2, 1].Value = "3";
                    ws.Cells[li_start_row + 3, 1].Value = "4";
                }
                li_start_row = 4 - 1;
                li_start_row = li_start_row - li_minus;

                if (cbx2.Checked) {
                    if (txtRange.Text != "8.5" || txtRate2.Text != "10.5" || txtRate3.Text != "13.5" || txtRate4.Text != "1.0" || lblCmRate.Text != "15") {
                        //表首
                        ls_rpt_name = ws.Cells[li_start_row, 2].Value.AsString();
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
                        ws.Cells[li_start_row, 2].Value = ls_rpt_name;
                        ws.Cells[li_start_row, 2].Font.Name = "標楷體";
                        ws.Cells[li_start_row, 2].Font.Name = "Times New Roman";
                    }
                }
                else {
                    range = ws.Range[(li_start_row + 1).AsString()];
                    range.Delete(DeleteMode.EntireRow);
                    li_minus = li_minus + 1;
                    //改編號
                    ws.Cells[li_start_row, 1].Value = "2";
                    ws.Cells[li_start_row + 1, 1].Value = "3";
                    ws.Cells[li_start_row + 2, 1].Value = "4";
                }

                #region 表1
                li_start_row = 13 - 1;
                li_start_row = li_start_row - li_minus;
                li_tot_row = 1000;
                ii_ole_row = li_start_row;

                if (cbx1.Checked) {
                    if (txtRange.Text != "10") {
                        //表頭
                        ls_rpt_name = ws.Cells[ii_ole_row - 3, 2].Value.AsString();
                        f = ls_rpt_name.IndexOf("10%") + 1;
                        if (f > 0) ls_rpt_name = ls_rpt_name.SubStr(0, f - 1) + txtRange.Text + "%" + ls_rpt_name.SubStr(f + 2, ls_rpt_name.Length);
                        ws.Cells[ii_ole_row - 3, 2].Value = ls_rpt_name;
                        ws.Cells[ii_ole_row - 3, 2].Font.Name = "標楷體";
                        ws.Cells[ii_ole_row - 3, 2].Font.Name = "Times New Roman";
                    }
                    if (cbxRate.Checked) {
                        DataView dv = dt42012.AsDataView();
                        dv.RowFilter = "(mgr2_level = 'Z' and T_30_RATE >= (MGR3_CUR_CM - " + (txtRate4Ref.Text.AsDecimal() / 100).AsString() + "))  or (mgr2_level = '1' and t_30_rate >= " + (txtRate1Ref.Text.AsDecimal() / 100).AsString() + ") or (mgr2_level = '2' and t_30_rate >= " + (txtRate2Ref.Text.AsDecimal() / 100).AsString() + ")";
                        dt42012 = dv.ToTable();
                    }

                    dt42012.Sort("MGR3_YMD");
                    foreach (DataRow dr in dt42012.Rows) {
                        ii_ole_row++;
                        ws.Cells[ii_ole_row, 1].Value = dr["MGR3_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                        ws.Cells[ii_ole_row, 2].Value = dr["MGR3_KIND_ID"].AsString();
                        ws.Cells[ii_ole_row, 3].Value = dr["APDK_NAME"].AsString();
                        ws.Cells[ii_ole_row, 4].Value = dr["MGR3_SID"].AsString();
                        ws.Cells[ii_ole_row, 5].Value = dr["PID_NAME"].AsString();
                        ws.Cells[ii_ole_row, 6].SetValue(dr["T_30_RATE"]);
                        ws.Cells[ii_ole_row, 7].SetValue(dr["MGR2_DAY_RATE"]);
                        if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                            ws.Cells[ii_ole_row, 8].Value = "從其高(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)";
                        }
                        else {
                            ws.Cells[ii_ole_row, 8].Value = dr["MGR3_CUR_LEVEL"].AsString();
                        }
                        ws.Cells[ii_ole_row, 8].Font.Name = "標楷體";
                        ws.Cells[ii_ole_row, 8].Font.Name = "Times New Roman";
                        if (dr["DAY_CNT"].AsInt() == 0) {
                            ws.Cells[ii_ole_row, 9].Value = "-";
                        }
                        else {
                            ws.Cells[ii_ole_row, 9].Value = dr["DAY_CNT"].AsInt();
                        }
                        ws.Cells[ii_ole_row, 10].SetValue(dr["TFXM1_PRICE"]);
                        ws.Cells[ii_ole_row, 11].SetValue(dr["AI5_PRICE"]);
                        ws.Cells[ii_ole_row, 12].SetValue(dr["TS_UPDOWN"]);
                        ws.Cells[ii_ole_row, 13].SetValue(dr["TI_UPDOWN"]);
                        ws.Cells[ii_ole_row, 14].SetValue(dr["YS_UPDOWN"]);
                        ws.Cells[ii_ole_row, 15].SetValue(dr["YI_UPDOWN"]);
                        ws.Cells[ii_ole_row, 16].SetValue(dr["AI2_OI"]);
                        ws.Cells[ii_ole_row, 17].SetValue(dr["AI2_M_QNTY"]);
                    }//foreach (DataRow dr in dt42012.Rows)

                    //刪除空白列
                    if (dt42012.Rows.Count < li_tot_row) {
                        range = ws.Range[(li_start_row + dt42012.Rows.Count + 1 + 1) + ":" + (li_start_row + li_tot_row + 1)];
                        range.Delete(DeleteMode.EntireRow);
                        li_minus = li_minus + (li_tot_row - dt42012.Rows.Count);
                    }
                }
                else {
                    range = ws.Range[(li_start_row - 3 + 1) + ":" + (li_start_row + li_tot_row + 1)];
                    range.Delete(DeleteMode.EntireRow);
                    li_minus = li_minus + (li_tot_row + 4);
                }
                #endregion

                #region 表2
                li_start_row = 1018 - 1;
                li_start_row = li_start_row - li_minus;
                li_tot_row = 1000;
                ii_ole_row = li_start_row;
                if (cbx2.Checked) {
                    if (txtRange.Text != "8.5" || txtRate2.Text != "10.5" || txtRate3.Text != "13.5" || txtRate4.Text != "1.0" || lblCmRate.Text != "15") {
                        //表頭
                        ls_rpt_name = ws.Cells[ii_ole_row - 3, 2].Value.AsString();
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
                        ws.Cells[ii_ole_row - 3, 2].Value = ls_rpt_name;
                        ws.Cells[ii_ole_row - 3, 2].Font.Name = "標楷體";
                        ws.Cells[ii_ole_row - 3, 2].Font.Name = "Times New Roman";
                        if (!cbx1.Checked) ws.Cells[ii_ole_row - 3, 1].Value = "1";
                    }
                    if (cbxRate.Checked) {
                        DataView dv = dt42012.AsDataView();
                        dv.RowFilter = "(mgr2_level = 'Z' and mgr2_day_rate >= (MGR3_CUR_CM - " + (txtRate4.Text.AsDecimal() / 100).AsString() + "))  or (mgr2_level = '1' and mgr2_day_rate >= " + (txtRate1.Text.AsDecimal() / 100).AsString() + ") or (mgr2_level = '2' and mgr2_day_rate >= " + (txtRate2.Text.AsDecimal() / 100).AsString() + ")";
                        dt42012 = dv.ToTable();
                    }
                    dt42012.Sort("MGR3_YMD");

                }
                #endregion

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

    }
}