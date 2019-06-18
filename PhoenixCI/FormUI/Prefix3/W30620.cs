using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/01/22
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30620 商品每月平均震幅、波動度彙集
    /// </summary>
    public partial class W30620 : FormParent {

        int ii_ole_row;
        private D30620 dao30620;

        public W30620(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;

            dao30620 = new D30620();
            //winni test
            //201207-201210
        }

        protected override ResultStatus Open() {
            base.Open();

            txtStartMonth.DateTimeValue = DateTime.ParseExact(GlobalInfo.OCF_DATE.ToString("yyyy/01/dd"), "yyyy/MM/dd", null);
            txtEndMonth.DateTimeValue = GlobalInfo.OCF_DATE;

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Export() {

            try {

                //0. ready
                panFilter.Enabled = false;
                labMsg.Visible = true;
                labMsg.Text = "開始轉檔...";
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);

                DateTime as_symd = DateTime.ParseExact((txtStartMonth.Text + "/01"), "yyyy/MM/dd", null);
                DateTime as_eymd = PbFunc.f_get_end_day("AI2", "TXF", txtEndMonth.Text); //抓當月最後交易日

                //1.複製檔案 & 開啟檔案 (因為三張報表都輸出到同一份excel,所以提出來)
                string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet worksheet = workbook.Worksheets[0];

                //2.填資料
                ii_ole_row = 3;
                bool sheet1 = wf_Export_30621(workbook, worksheet, as_symd, as_eymd);
                ii_ole_row += 5;
                bool sheet2 = wf_Export_30622(workbook, worksheet, as_symd, as_eymd);

                //存檔
                workbook.SaveDocument(excelDestinationPath);
                if (!sheet1 && !sheet2) {
                    workbook = null;
                    File.Delete(excelDestinationPath);
                }
                return ResultStatus.Success;

            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            finally {
                panFilter.Enabled = true;
                labMsg.Text = "";
                labMsg.Visible = false;
                this.Cursor = Cursors.Arrow;
            }
            return ResultStatus.Fail;
        }

        private bool wf_Export_30621(Workbook workbook, Worksheet worksheet, DateTime as_symd, DateTime as_eymd) {

            int li_ole_row_tol = ii_ole_row + 60;
            string ls_ymd = "";
            int li_col = 0;
            labMsg.Text = _ProgramID + "－" + _ProgramName + " 轉檔中...";

            //先跑各商品每月平均振幅明細表
            DataTable dt30620_1 = dao30620.GetListAavg(as_symd, as_eymd);
            if (dt30620_1.Rows.Count == 0) {
                MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtStartMonth.Text + "-" + txtEndMonth.Text, this.Text));
                return false;
            }

            for (int i = 0; i < dt30620_1.Rows.Count; i++) {
                if (ls_ymd != dt30620_1.Rows[i]["AI6_YM"].AsString()) {
                    ls_ymd = dt30620_1.Rows[i]["AI6_YM"].AsString();
                    ii_ole_row += 1;
                    worksheet.Cells[ii_ole_row - 1, 0].Value = int.Parse(ls_ymd.SubStr(0, 4)) - 1911 + ls_ymd.SubStr(4, 2);
                }

                li_col = dt30620_1.Rows[i]["RPT_LEVEL_1"].AsInt();
                if (li_col > 0) {
                    worksheet.Cells[ii_ole_row - 1, li_col - 1].Value = dt30620_1.Rows[i]["AVG_AI6"].AsDecimal();
                }

                li_col = dt30620_1.Rows[i]["RPT_LEVEL_2"].AsInt();
                if (li_col > 0) {
                    worksheet.Cells[ii_ole_row - 1, li_col - 1].Value = dt30620_1.Rows[i]["AVG_TFXM"].AsDecimal();
                }
            }

            //刪除空白列
            if (ii_ole_row < li_ole_row_tol) {
                worksheet.Rows.Remove(ii_ole_row, li_ole_row_tol - ii_ole_row);
            }
            return true;
        }

        private bool wf_Export_30622(Workbook workbook, Worksheet worksheet, DateTime as_symd, DateTime as_eymd) {
            int li_ole_row_tol = ii_ole_row + 60;
            string ls_ymd = "";
            int li_col = 0;

            DataTable dt30620_2 = dao30620.GetListVol(as_symd, as_eymd);
            if (dt30620_2.Rows.Count == 0) {
                MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtStartMonth.Text + "-" + txtEndMonth.Text, this.Text));
                return false;
            }

            for (int i = 0; i < dt30620_2.Rows.Count; i++) {
                if (ls_ymd != dt30620_2.Rows[i]["AI6_YM"].AsString()) {
                    ls_ymd = dt30620_2.Rows[i]["AI6_YM"].AsString();
                    ii_ole_row += 1;
                    worksheet.Cells[ii_ole_row - 1, 0].Value = int.Parse(ls_ymd.SubStr(0, 4)) - 1911 + ls_ymd.SubStr(4, 2);
                }

                li_col = dt30620_2.Rows[i]["RPT_LEVEL_1"].AsInt();
                if (li_col > 0) {
                    worksheet.Cells[ii_ole_row - 1, li_col - 1].Value = dt30620_2.Rows[i]["AI6_RATE"].AsDecimal() * 100;
                }

                li_col = dt30620_2.Rows[i]["RPT_LEVEL_2"].AsInt();
                if (li_col > 0) {
                    worksheet.Cells[ii_ole_row - 1, li_col - 1].Value = dt30620_2.Rows[i]["TFXM_RATE"].AsDecimal() * 100;
                }
            }

            //刪除空白列
            if (ii_ole_row < li_ole_row_tol) {
                worksheet.Rows.Remove(ii_ole_row, li_ole_row_tol - ii_ole_row);
            }

            //把指標移到[A1]
            worksheet.Range["A1"].Select();
            worksheet.ScrollToRow(0);
            return true;
        }

    }
}