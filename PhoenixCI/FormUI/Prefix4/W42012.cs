﻿using System;
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
                string rptId = "42012", file, rptName = "股票期貨風險價格係數分析表";

                //讀取資料(保證金適用比例級距)
                DataTable dt42012 = dao42012.d_42012_detl(txtSDate.DateTimeValue.ToString("yyyyMMdd"), txtEDate.DateTimeValue.ToString("yyyyMMdd"), txtSID.Text.AsString(),
                                          txtRange.Text.AsDecimal() / 100, txtRate2Ref.Text.AsDecimal() / 100, txtRate3Ref.Text.AsDecimal() / 100, txtRate4Ref.Text.AsDecimal() / 100,
                                          txtRate1.Text.AsDecimal() / 100, txtRate2.Text.AsDecimal() / 100, txtRate3.Text.AsDecimal() / 100, txtRate4.Text.AsDecimal() / 100);
                if (dt42012.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "～" + txtEDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
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
                int f, rowIndex, startRow = 3 - 1, totalRow = 300, minusRow = 0;
                Range range;
                if (cbx1.Checked) {
                    if (txtRange.Text != "10") {
                        //表首
                        rptName = ws.Cells[2, 2].Value.AsString();
                        f = rptName.IndexOf("10%") + 1;
                        if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRange.Text + "%" + rptName.SubStr(f + 2, rptName.Length);
                        ws.Cells[startRow, 2].Value = rptName;
                        ws.Cells[startRow, 2].Font.Name = "標楷體";
                        ws.Cells[startRow, 2].Font.Name = "Times New Roman";
                    }
                }
                else {
                    range = ws.Range[(startRow + 1).AsString()];
                    range.Delete(DeleteMode.EntireRow);
                    minusRow = minusRow + 1;
                    //改編號
                    ws.Cells[startRow, 1].Value = "1";
                    ws.Cells[startRow + 1, 1].Value = "2";
                    ws.Cells[startRow + 2, 1].Value = "3";
                    ws.Cells[startRow + 3, 1].Value = "4";
                }
                startRow = 4 - 1;
                startRow = startRow - minusRow;

                if (cbx2.Checked) {
                    if (txtRange.Text != "8.5" || txtRate2.Text != "10.5" || txtRate3.Text != "13.5" || txtRate4.Text != "1.0" || lblCmRate.Text != "15") {
                        //表首
                        rptName = ws.Cells[startRow, 2].Value.AsString();
                        if (txtRange.Text != "8.5") {
                            f = rptName.IndexOf("8.5 %") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRate1.Text + "%" + rptName.SubStr(f + 4, rptName.Length);
                        }
                        if (txtRate2.Text != "10.5") {
                            f = rptName.IndexOf("10.5 %") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRate2.Text + "%" + rptName.SubStr(f + 5, rptName.Length);
                        }
                        if (txtRate3.Text != "13.5") {
                            f = rptName.IndexOf("13.5 %") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRate3.Text + "%" + rptName.SubStr(f + 5, rptName.Length);
                        }
                        if (txtRate4.Text != "1.0") {
                            f = rptName.IndexOf("1.0 %") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRate4.Text + "%" + rptName.SubStr(f + 4, rptName.Length);
                        }
                        if (lblCmRate.Text != "15") {
                            f = rptName.IndexOf("15%") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + lblCmRate.Text + "%" + rptName.SubStr(f + 4, rptName.Length);
                        }
                        ws.Cells[startRow, 2].Value = rptName;
                        ws.Cells[startRow, 2].Font.Name = "標楷體";
                        ws.Cells[startRow, 2].Font.Name = "Times New Roman";
                    }
                }
                else {
                    range = ws.Range[(startRow + 1).AsString()];
                    range.Delete(DeleteMode.EntireRow);
                    minusRow = minusRow + 1;
                    //改編號
                    ws.Cells[startRow, 1].Value = "2";
                    ws.Cells[startRow + 1, 1].Value = "3";
                    ws.Cells[startRow + 2, 1].Value = "4";
                }

                #region 表1
                startRow = 13 - 1;
                startRow = startRow - minusRow;
                totalRow = 1000;
                rowIndex = startRow;

                if (cbx1.Checked) {
                    if (txtRange.Text != "10") {
                        //表頭
                        rptName = ws.Cells[rowIndex - 3, 2].Value.AsString();
                        f = rptName.IndexOf("10%") + 1;
                        if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRange.Text + "%" + rptName.SubStr(f + 2, rptName.Length);
                        ws.Cells[rowIndex - 3, 2].Value = rptName;
                        ws.Cells[rowIndex - 3, 2].Font.Name = "標楷體";
                        ws.Cells[rowIndex - 3, 2].Font.Name = "Times New Roman";
                    }
                    if (cbxRate.Checked) {
                        DataView dv = dt42012.AsDataView();
                        dv.RowFilter = "(mgr2_level = 'Z' and T_30_RATE >= (MGR3_CUR_CM - " + (txtRate4Ref.Text.AsDecimal() / 100).AsString() + "))  or (mgr2_level = '1' and t_30_rate >= " + (txtRate1Ref.Text.AsDecimal() / 100).AsString() + ") or (mgr2_level = '2' and t_30_rate >= " + (txtRate2Ref.Text.AsDecimal() / 100).AsString() + ")";
                        dt42012 = dv.ToTable();
                    }

                    dt42012.Sort("MGR3_YMD");
                    foreach (DataRow dr in dt42012.Rows) {
                        rowIndex++;
                        ws.Cells[rowIndex, 1].Value = dr["MGR3_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                        ws.Cells[rowIndex, 2].Value = dr["MGR3_KIND_ID"].AsString();
                        ws.Cells[rowIndex, 3].Value = dr["APDK_NAME"].AsString();
                        ws.Cells[rowIndex, 4].Value = dr["MGR3_SID"].AsString();
                        ws.Cells[rowIndex, 5].Value = dr["PID_NAME"].AsString();
                        ws.Cells[rowIndex, 6].SetValue(dr["T_30_RATE"]);
                        ws.Cells[rowIndex, 7].SetValue(dr["MGR2_DAY_RATE"]);
                        if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                            ws.Cells[rowIndex, 8].Value = "從其高(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)";
                        }
                        else {
                            ws.Cells[rowIndex, 8].Value = dr["MGR3_CUR_LEVEL"].AsString();
                        }
                        ws.Cells[rowIndex, 8].Font.Name = "標楷體";
                        ws.Cells[rowIndex, 8].Font.Name = "Times New Roman";
                        if (dr["DAY_CNT"].AsInt() == 0) {
                            ws.Cells[rowIndex, 9].Value = "-";
                        }
                        else {
                            ws.Cells[rowIndex, 9].Value = dr["DAY_CNT"].AsInt();
                        }
                        ws.Cells[rowIndex, 10].SetValue(dr["TFXM1_PRICE"]);
                        ws.Cells[rowIndex, 11].SetValue(dr["AI5_PRICE"]);
                        ws.Cells[rowIndex, 12].SetValue(dr["TS_UPDOWN"]);
                        ws.Cells[rowIndex, 13].SetValue(dr["TI_UPDOWN"]);
                        ws.Cells[rowIndex, 14].SetValue(dr["YS_UPDOWN"]);
                        ws.Cells[rowIndex, 15].SetValue(dr["YI_UPDOWN"]);
                        ws.Cells[rowIndex, 16].SetValue(dr["AI2_OI"]);
                        ws.Cells[rowIndex, 17].SetValue(dr["AI2_M_QNTY"]);
                    }//foreach (DataRow dr in dt42012.Rows)

                    //刪除空白列
                    if (dt42012.Rows.Count < totalRow) {
                        range = ws.Range[(startRow + dt42012.Rows.Count + 1 + 1) + ":" + (startRow + totalRow + 1)];
                        range.Delete(DeleteMode.EntireRow);
                        minusRow = minusRow + (totalRow - dt42012.Rows.Count);
                    }
                }
                else {
                    range = ws.Range[(startRow - 3 + 1) + ":" + (startRow + totalRow + 1)];
                    range.Delete(DeleteMode.EntireRow);
                    minusRow = minusRow + (totalRow + 4);
                }
                #endregion

                #region 表2
                startRow = 1018 - 1;
                startRow = startRow - minusRow;
                totalRow = 1000;
                rowIndex = startRow;
                if (cbx2.Checked) {
                    if (txtRange.Text != "8.5" || txtRate2.Text != "10.5" || txtRate3.Text != "13.5" || txtRate4.Text != "1.0" || lblCmRate.Text != "15") {
                        //表頭
                        rptName = ws.Cells[rowIndex - 3, 2].Value.AsString();
                        if (txtRange.Text != "8.5") {
                            f = rptName.IndexOf("8.5 %") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRate1.Text + "%" + rptName.SubStr(f + 4, rptName.Length);
                        }
                        if (txtRate2.Text != "10.5") {
                            f = rptName.IndexOf("10.5 %") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRate2.Text + "%" + rptName.SubStr(f + 5, rptName.Length);
                        }
                        if (txtRate3.Text != "13.5") {
                            f = rptName.IndexOf("13.5 %") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRate3.Text + "%" + rptName.SubStr(f + 5, rptName.Length);
                        }
                        if (txtRate4.Text != "1.0") {
                            f = rptName.IndexOf("1.0 %") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + txtRate4.Text + "%" + rptName.SubStr(f + 4, rptName.Length);
                        }
                        if (lblCmRate.Text != "15") {
                            f = rptName.IndexOf("15%") + 1;
                            if (f > 0) rptName = rptName.SubStr(0, f - 1) + lblCmRate.Text + "%" + rptName.SubStr(f + 4, rptName.Length);
                        }
                        ws.Cells[rowIndex - 3, 2].Value = rptName;
                        ws.Cells[rowIndex - 3, 2].Font.Name = "標楷體";
                        ws.Cells[rowIndex - 3, 2].Font.Name = "Times New Roman";
                        if (!cbx1.Checked) ws.Cells[rowIndex - 3, 1].Value = "1";
                    }
                    if (cbxRate.Checked) {
                        DataView dv = dt42012.AsDataView();
                        dv.RowFilter = "(mgr2_level = 'Z' and mgr2_day_rate >= (MGR3_CUR_CM - " + (txtRate4.Text.AsDecimal() / 100).AsString() + "))  or (mgr2_level = '1' and mgr2_day_rate >= " + (txtRate1.Text.AsDecimal() / 100).AsString() + ") or (mgr2_level = '2' and mgr2_day_rate >= " + (txtRate2.Text.AsDecimal() / 100).AsString() + ")";
                        dt42012 = dv.ToTable();
                    }
                    dt42012.Sort("MGR3_YMD");
                    foreach (DataRow dr in dt42012.Rows) {
                        rowIndex++;
                        ws.Cells[rowIndex, 1].Value = dr["MGR3_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                        ws.Cells[rowIndex, 2].Value = dr["MGR3_KIND_ID"].AsString();
                        ws.Cells[rowIndex, 3].Value = dr["APDK_NAME"].AsString();
                        ws.Cells[rowIndex, 4].Value = dr["MGR3_SID"].AsString();
                        ws.Cells[rowIndex, 5].Value = dr["PID_NAME"].AsString();
                        ws.Cells[rowIndex, 6].SetValue(dr["MGR2_DAY_RATE"]);
                        ws.Cells[rowIndex, 7].SetValue(dr["T_30_RATE"]);
                        if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                            ws.Cells[rowIndex, 8].Value = "從其高(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)";
                        }
                        else {
                            ws.Cells[rowIndex, 8].Value = dr["MGR3_CUR_LEVEL"].AsString();
                        }
                        ws.Cells[rowIndex, 8].Font.Name = "標楷體";
                        ws.Cells[rowIndex, 8].Font.Name = "Times New Roman";
                        if (dr["DAY_CNT_3"].AsInt() == 0) {
                            ws.Cells[rowIndex, 9].Value = "-";
                        }
                        else {
                            ws.Cells[rowIndex, 9].Value = dr["DAY_CNT_3"].AsInt();
                        }
                        ws.Cells[rowIndex, 10].SetValue(dr["TFXM1_PRICE"]);
                        ws.Cells[rowIndex, 11].SetValue(dr["AI5_PRICE"]);
                        ws.Cells[rowIndex, 12].SetValue(dr["TS_UPDOWN"]);
                        ws.Cells[rowIndex, 13].SetValue(dr["TI_UPDOWN"]);
                        ws.Cells[rowIndex, 14].SetValue(dr["YS_UPDOWN"]);
                        ws.Cells[rowIndex, 15].SetValue(dr["YI_UPDOWN"]);
                        ws.Cells[rowIndex, 16].SetValue(dr["AI2_OI"]);
                        ws.Cells[rowIndex, 17].SetValue(dr["AI2_M_QNTY"]);
                    }//foreach (DataRow dr in dt42012.Rows)

                    //刪除空白列
                    if (dt42012.Rows.Count < totalRow) {
                        range = ws.Range[(startRow + dt42012.Rows.Count + 1 + 1) + ":" + (startRow + totalRow + 1)];
                        range.Delete(DeleteMode.EntireRow);
                        minusRow = minusRow + (totalRow - dt42012.Rows.Count);
                    }
                }
                else {
                    range = ws.Range[(startRow - 3 + 1) + ":" + (startRow + totalRow + 1)];
                    range.Delete(DeleteMode.EntireRow);
                    minusRow = minusRow + (totalRow + 4);
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