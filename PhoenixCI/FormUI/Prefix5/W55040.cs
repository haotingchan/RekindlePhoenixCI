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

        public override ResultStatus BeforeOpen() {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open() {
            base.Open();

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {
            base.Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield() {
            base.CheckShield();

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            base.Save(pokeBall);

            return ResultStatus.Success;
        }

        protected override ResultStatus Run(PokeBall args) {
            base.Run(args);

            return ResultStatus.Success;
        }

        protected override ResultStatus Import() {
            base.Import();

            return ResultStatus.Success;
        }

        protected override ResultStatus Export() {
            lblProcessing.Visible = true;
            base.Export();

            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);

            ManipulateExcel(excelDestinationPath);
            lblProcessing.Visible = false;
            return ResultStatus.Success;
        }

        private void ManipulateExcel(string excelDestinationPath) {

            try {
                #region wf_55040 造市者TXO交易經手費折減比率結構表

                string ls_rpt_name, ls_rpt_id, ls_brk_no, ls_acc_no;
                int i, li_ole_col, li_datacount, li_ole_row_tol, ll_found;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = 欄位位置
                ls_param_key = 契約
                *************************************/
                ls_rpt_name = "造市者TXO交易經手費折減比率結構表";
                ls_rpt_id = "55040";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';

                /******************
                讀取資料
                ******************/
                DataTable dtContent = dao55040.ListByDate(txtMonth.Text.Replace("/", ""));
                if (dtContent.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtMonth.Text, ls_rpt_name));
                }
                //TXO詢報價比
                DataTable dtAMM0 = dao55040.ListByDate_AMM0(txtMonth.Text.Replace("/", ""));

                /******************
                切換Sheet
                ******************/
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet worksheet = workbook.Worksheets[0];

                //填資料
                int ii_ole_row = 5;
                li_datacount = int.Parse(worksheet.Cells[0, 0].Value.ToString());
                if (li_datacount == null || li_datacount == 0) {
                    li_datacount = dtContent.Rows.Count;
                }
                li_ole_row_tol = ii_ole_row + li_datacount;
                worksheet.Cells[3, 0].Value = worksheet.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                worksheet.Cells[3, 4].Value = worksheet.Cells[3, 4].Value + txtMonth.Text.Replace("/", "");

                ls_brk_no = "";
                ls_acc_no = "";
                //TODO: dataobject.ToString 如果為null會報錯，在跑for迴圈之前要拉出來處理。
                for (i = 0; i < dtContent.Rows.Count; i++) {
                    DataRow drContent = dtContent.Rows[i];

                    if (ls_brk_no != drContent["feetxo_fcm_no"].ToString() || ls_acc_no != drContent["feetxo_acc_no"].ToString()) {
                        ii_ole_row = ii_ole_row + 1;
                        ls_brk_no = drContent["feetxo_fcm_no"].ToString();
                        ls_acc_no = drContent["feetxo_acc_no"].ToString();
                        worksheet.Cells[ii_ole_row, 0].Value = ls_brk_no;
                        worksheet.Cells[ii_ole_row, 1].Value = drContent["brk_abbr_name"].ToString();
                        worksheet.Cells[ii_ole_row, 2].Value = ls_acc_no;
                        //ll_found = lds_mm.find("amm0_brk_no='" + ls_brk_no + "' and amm0_acc_no='" + ls_acc_no + "'", 1, lds_mm.rowcount());
                        DataRow[] dtAMM0_find = dtAMM0.Select("amm0_brk_no='" + ls_brk_no + "' and amm0_acc_no='" + ls_acc_no + "'");
                        if (dtAMM0_find.Length == 0) {
                            ll_found = 0;
                        }
                        else {
                            ll_found = dtAMM0.Rows.IndexOf(dtAMM0_find[0]) + 1;
                        }

                        if (ll_found > 0) {
                            worksheet.Cells[ii_ole_row, 20].Value = decimal.Parse(dtAMM0.Rows[ll_found]["mmk_rate"].ToString());
                        }
                    }
                    li_ole_col = int.Parse(drContent["rpt_seq_no"].ToString());
                    if (ii_ole_row > 0 && li_ole_col > 0) {
                        worksheet.Cells[ii_ole_row, li_ole_col].Value = decimal.Parse(drContent["feetxo_rate"].ToString());
                    }
                }

                /*******************
                刪除空白列
                *******************/
                if (li_ole_row_tol > ii_ole_row) {
                    worksheet.Rows.Remove(ii_ole_row + 1, li_ole_row_tol - ii_ole_row);
                }
                worksheet.ScrollToRow(0);
                //存檔
                workbook.SaveDocument(excelDestinationPath);
                #endregion
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
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