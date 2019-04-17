using System;
using System.Data;
using BaseGround;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using BaseGround.Report;
using DevExpress.Spreadsheet;
using BusinessObjects;
using System.Windows.Forms;
using System.Threading;
using BaseGround.Shared;
/// <summary>
/// Lukas, 2018/12/26
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 55040 造市者TXO交易經手費折減比率結構表
    /// 有寫到的功能：Export
    /// </summary>
    public partial class W55040 : FormParent {

        private D55040 dao55040;

        public W55040(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            dao55040 = new D55040();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;
        }

        protected override ResultStatus Open() {
            base.Open();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected void ShowMsg(string msg) {
            lblProcessing.Text = msg;
            this.Refresh();
            Thread.Sleep(5);
        }

        protected override ResultStatus Export() {
            base.Export();

            if (!ManipulateExcel()) return ResultStatus.Fail;
            lblProcessing.Visible = false;
            return ResultStatus.Success;
        }

        private bool ManipulateExcel() {

            try {
                #region wf_55040 造市者TXO交易經手費折減比率結構表
                txtMonth.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                string rptName, rptId, brkNo, accNo, file;
                int f, colNum, datacount, rowTol, found;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = 欄位位置
                ls_param_key = 契約
                *************************************/
                rptName = "造市者TXO交易經手費折減比率結構表";
                rptId = "55040";
                ShowMsg(rptId + "－" + rptName + " 轉檔中...");
                /******************
                讀取資料
                ******************/
                DataTable dtContent = dao55040.ListByDate(txtMonth.Text.Replace("/", ""));
                if (dtContent.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtMonth.Text, rptName));
                    return false;
                }
                //TXO詢報價比
                DataTable dtAMM0 = dao55040.ListByDate_AMM0(txtMonth.Text.Replace("/", ""));

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return false;

                /******************
                切換Sheet
                ******************/
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);
                Worksheet worksheet = workbook.Worksheets[0];

                //填資料
                int rowIndex = 5;
                datacount = int.Parse(worksheet.Cells[0, 0].Value.ToString());
                if (datacount == null || datacount == 0) {
                    datacount = dtContent.Rows.Count;
                }
                rowTol = rowIndex + datacount;
                worksheet.Cells[3, 0].Value = worksheet.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                worksheet.Cells[3, 4].Value = worksheet.Cells[3, 4].Value + txtMonth.Text.Replace("/", "");

                brkNo = "";
                accNo = "";
                //TODO: dataobject.ToString 如果為null會報錯，在跑for迴圈之前要拉出來處理。
                for (f = 0; f < dtContent.Rows.Count; f++) {
                    DataRow drContent = dtContent.Rows[f];

                    if (brkNo != drContent["feetxo_fcm_no"].ToString() || accNo != drContent["feetxo_acc_no"].ToString()) {
                        rowIndex = rowIndex + 1;
                        brkNo = drContent["feetxo_fcm_no"].ToString();
                        accNo = drContent["feetxo_acc_no"].ToString();
                        worksheet.Cells[rowIndex, 0].Value = brkNo;
                        worksheet.Cells[rowIndex, 1].Value = drContent["brk_abbr_name"].ToString();
                        worksheet.Cells[rowIndex, 2].Value = accNo;
                        //ll_found = lds_mm.find("amm0_brk_no='" + ls_brk_no + "' and amm0_acc_no='" + ls_acc_no + "'", 1, lds_mm.rowcount());
                        DataRow[] dtAMM0_find = dtAMM0.Select("amm0_brk_no='" + brkNo + "' and amm0_acc_no='" + accNo + "'");
                        if (dtAMM0_find.Length == 0) {
                            found = 0;
                        }
                        else {
                            found = dtAMM0.Rows.IndexOf(dtAMM0_find[0]) + 1;
                        }

                        if (found > 0) {
                            worksheet.Cells[rowIndex, 20].Value = decimal.Parse(dtAMM0.Rows[found]["mmk_rate"].ToString());
                        }
                    }
                    colNum = int.Parse(drContent["rpt_seq_no"].ToString());
                    if (rowIndex > 0 && colNum > 0) {
                        worksheet.Cells[rowIndex, colNum].Value = decimal.Parse(drContent["feetxo_rate"].ToString());
                    }
                }

                /*******************
                刪除空白列
                *******************/
                if (rowTol > rowIndex) {
                    worksheet.Rows.Remove(rowIndex + 1, rowTol - rowIndex);
                }
                worksheet.ScrollToRow(0);
                //存檔
                workbook.SaveDocument(file);
                #endregion
                ShowMsg("轉檔成功");
                return true;
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            finally {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
                txtMonth.Enabled = true;
            }
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            base.Print(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            base.InsertRow();

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow();

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose() {
            return base.BeforeClose();
        }

    }
}