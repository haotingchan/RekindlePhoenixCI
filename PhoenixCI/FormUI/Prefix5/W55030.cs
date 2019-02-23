using System;
using System.Data;
using BaseGround;
using Common;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together;
using System.Windows.Forms;
/// <summary>
/// Lukas, 2018/12/26
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 造市者各商品交易經手費比率月計表
    /// 有寫到的功能：Export
    /// </summary>
    public partial class W55030 : FormParent {

        private D55030 dao55030;
        private D55031 dao55031;
        private APDK daoAPDK;

        public W55030(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            dao55030 = new D55030();
            dao55031 = new D55031();
            daoAPDK = new APDK();
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
                #region wf_55030 造市者各商品交易經手費折減比率月計表

                string ls_rpt_name, ls_rpt_id, ls_brk_no, ls_acc_no, ls_session;
                int i, li_ole_col, li_datacount, li_ole_row_tol, ii_ole_row;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = 欄位位置
                ls_param_key = 契約
                *************************************/
                ls_rpt_name = "造市者各商品交易經手費折減比率月計表";
                ls_rpt_id = "55030";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';
                ls_session = "0";

                //讀取資料
                DataTable dtContent = dao55030.ListByDate(txtMonth.Text.Replace("/", ""));
                if (dtContent.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtMonth.Text, this.Text));
                }

                //切換Sheet
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet worksheet = workbook.Worksheets[0];

                //填資料
                ii_ole_row = 5;
                li_datacount = int.Parse(worksheet.Cells[0, 0].Value.ToString());

                if (li_datacount == null || li_datacount == 0) {
                    li_datacount = dtContent.Rows.Count;
                }
                li_ole_row_tol = ii_ole_row + li_datacount;
                worksheet.Cells[3, 0].Value = worksheet.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                worksheet.Cells[3, 7].Value = worksheet.Cells[3, 7].Value + txtMonth.Text.Replace("/", "");

                ls_brk_no = "";
                ls_acc_no = "";
                for (i = 0; i < dtContent.Rows.Count; i++) {

                    if (ls_brk_no != dtContent.Rows[i]["feetrd_fcm_no"].ToString() || ls_acc_no != dtContent.Rows[i]["feetrd_acc_no"].ToString()) {
                        ii_ole_row = ii_ole_row + 1;
                        ls_brk_no = dtContent.Rows[i]["feetrd_fcm_no"].ToString();
                        ls_acc_no = dtContent.Rows[i]["feetrd_acc_no"].ToString();
                        worksheet.Cells[ii_ole_row, 0].Value = ls_brk_no;
                        worksheet.Cells[ii_ole_row, 1].Value = dtContent.Rows[i]["brk_abbr_name"].ToString();
                        worksheet.Cells[ii_ole_row, 2].Value = ls_acc_no;
                    }
                    li_ole_col = int.Parse(dtContent.Rows[i]["rpt_seq_no"].ToString());
                    if (ii_ole_row > 0 && li_ole_col > 0) {
                        worksheet.Cells[ii_ole_row, li_ole_col].Value = decimal.Parse(dtContent.Rows[i]["feetrd_rate"].ToString());
                    }
                }

                /*******************
                刪除空白列
                *******************/
                if (li_ole_row_tol > ii_ole_row) {

                    worksheet.Rows.Remove(ii_ole_row + 1, li_ole_row_tol - ii_ole_row);
                }

                #endregion

                #region wf_55031
                string ls_kind_id;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = 欄位位置
                ls_param_key = 契約
                *************************************/
                ls_rpt_name = "造市者各商品交易經手費折減比率月計表";
                ls_rpt_id = "55031";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';
                ls_session = "0";

                /******************
                讀取資料
                ******************/
                DataTable dtContent2 = dao55031.ListByDate(txtMonth.Text.Replace("/", ""));
                if (dtContent.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtMonth.Text, this.Text));
                }
                //契約檔
                DataTable dtAPDK = daoAPDK.ListAll_55031();

                /******************
                切換Sheet
                ******************/
                Worksheet worksheet2 = workbook.Worksheets[1];

                ii_ole_row = 5;
                li_datacount = int.Parse(worksheet2.Cells[0, 0].Value.ToString());
                if (li_datacount == null || li_datacount == 0) {
                    li_datacount = dtContent2.Rows.Count;
                }
                li_ole_row_tol = ii_ole_row + li_datacount;
                worksheet2.Cells[3, 0].Value = worksheet2.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                worksheet2.Cells[3, 7].Value = worksheet2.Cells[3, 7].Value + txtMonth.Text.Replace("/", "");
                //契約檔
                for (i = 0; i < dtAPDK.Rows.Count; i++) {
                    ls_kind_id = dtAPDK.Rows[i]["apdk_kind_id"].ToString();
                    if (ls_kind_id.Trim() == "STO") {
                        ls_kind_id = "平均";
                    }
                    worksheet2.Cells[5, i + 3].Value = ls_kind_id;
                }

                ls_brk_no = "";
                i = 0;
                for (i = 0; i < dtContent2.Rows.Count; i++) {
                    if (ls_brk_no != dtContent2.Rows[i]["feetrd_fcm_no"].ToString() || ls_acc_no != dtContent2.Rows[i]["feetrd_acc_no"].ToString()) {
                        ii_ole_row = ii_ole_row + 1;
                        ls_brk_no = dtContent2.Rows[i]["feetrd_fcm_no"].ToString();
                        ls_acc_no = dtContent2.Rows[i]["feetrd_acc_no"].ToString();
                        worksheet2.Cells[ii_ole_row, 0].Value = ls_brk_no;
                        worksheet2.Cells[ii_ole_row, 1].Value = dtContent2.Rows[i]["brk_abbr_name"].ToString();
                        worksheet2.Cells[ii_ole_row, 2].Value = ls_acc_no;
                    }
                    //long datastore.Find ( stringexpression, longstart, longend )找該值位於資料表的第幾筆
                    if (dtAPDK.Select("apdk_kind_id='" + dtContent2.Rows[i]["feetrd_kind_id"].ToString().Trim() + "'").Length == 0) {
                        li_ole_col = 0;
                    }
                    else {
                        li_ole_col = dtAPDK.Rows.IndexOf(dtAPDK.Select("apdk_kind_id='" + dtContent2.Rows[i]["feetrd_kind_id"].ToString().Trim() + "'")[0]) + 1;
                    }
                    if (ii_ole_row > 0 && li_ole_col > 0) {
                        worksheet2.Cells[ii_ole_row, li_ole_col + 2].Value = decimal.Parse(dtContent2.Rows[i]["feetrd_rate"].ToString());
                    }
                }

                /*******************
                刪除空白列
                *******************/
                if (li_ole_row_tol > ii_ole_row) {

                    worksheet2.Rows.Remove(ii_ole_row + 1, li_ole_row_tol - ii_ole_row);
                }

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