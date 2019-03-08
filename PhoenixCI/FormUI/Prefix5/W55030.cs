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

                string brkNo, accNo, session = "0";
                int i, colNum, datacount, rowTol, rowNum;

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
                rowNum = 5;
                datacount = int.Parse(worksheet.Cells[0, 0].Value.ToString());

                if (datacount == null || datacount == 0) {
                    datacount = dtContent.Rows.Count;
                }
                rowTol = rowNum + datacount;
                worksheet.Cells[3, 0].Value = worksheet.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                worksheet.Cells[3, 7].Value = worksheet.Cells[3, 7].Value + txtMonth.Text.Replace("/", "");

                brkNo = "";
                accNo = "";
                for (i = 0; i < dtContent.Rows.Count; i++) {

                    if (brkNo != dtContent.Rows[i]["feetrd_fcm_no"].ToString() || accNo != dtContent.Rows[i]["feetrd_acc_no"].ToString()) {
                        rowNum = rowNum + 1;
                        brkNo = dtContent.Rows[i]["feetrd_fcm_no"].ToString();
                        accNo = dtContent.Rows[i]["feetrd_acc_no"].ToString();
                        worksheet.Cells[rowNum, 0].Value = brkNo;
                        worksheet.Cells[rowNum, 1].Value = dtContent.Rows[i]["brk_abbr_name"].ToString();
                        worksheet.Cells[rowNum, 2].Value = accNo;
                    }
                    colNum = int.Parse(dtContent.Rows[i]["rpt_seq_no"].ToString());
                    if (rowNum > 0 && colNum > 0) {
                        worksheet.Cells[rowNum, colNum].Value = decimal.Parse(dtContent.Rows[i]["feetrd_rate"].ToString());
                    }
                }

                /*******************
                刪除空白列
                *******************/
                if (rowTol > rowNum) {

                    worksheet.Rows.Remove(rowNum + 1, rowTol - rowNum);
                }

                #endregion

                #region wf_55031
                string kindId;
                session = "0";

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

                rowNum = 5;
                datacount = int.Parse(worksheet2.Cells[0, 0].Value.ToString());
                if (datacount == null || datacount == 0) {
                    datacount = dtContent2.Rows.Count;
                }
                rowTol = rowNum + datacount;
                worksheet2.Cells[3, 0].Value = worksheet2.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                worksheet2.Cells[3, 7].Value = worksheet2.Cells[3, 7].Value + txtMonth.Text.Replace("/", "");
                //契約檔
                for (i = 0; i < dtAPDK.Rows.Count; i++) {
                    kindId = dtAPDK.Rows[i]["apdk_kind_id"].ToString();
                    if (kindId.Trim() == "STO") {
                        kindId = "平均";
                    }
                    worksheet2.Cells[5, i + 3].Value = kindId;
                }

                brkNo = "";
                i = 0;
                for (i = 0; i < dtContent2.Rows.Count; i++) {
                    if (brkNo != dtContent2.Rows[i]["feetrd_fcm_no"].ToString() || accNo != dtContent2.Rows[i]["feetrd_acc_no"].ToString()) {
                        rowNum = rowNum + 1;
                        brkNo = dtContent2.Rows[i]["feetrd_fcm_no"].ToString();
                        accNo = dtContent2.Rows[i]["feetrd_acc_no"].ToString();
                        worksheet2.Cells[rowNum, 0].Value = brkNo;
                        worksheet2.Cells[rowNum, 1].Value = dtContent2.Rows[i]["brk_abbr_name"].ToString();
                        worksheet2.Cells[rowNum, 2].Value = accNo;
                    }
                    //long datastore.Find ( stringexpression, longstart, longend )找該值位於資料表的第幾筆
                    if (dtAPDK.Select("apdk_kind_id='" + dtContent2.Rows[i]["feetrd_kind_id"].ToString().Trim() + "'").Length == 0) {
                        colNum = 0;
                    }
                    else {
                        colNum = dtAPDK.Rows.IndexOf(dtAPDK.Select("apdk_kind_id='" + dtContent2.Rows[i]["feetrd_kind_id"].ToString().Trim() + "'")[0]) + 1;
                    }
                    if (rowNum > 0 && colNum > 0) {
                        worksheet2.Cells[rowNum, colNum + 2].Value = decimal.Parse(dtContent2.Rows[i]["feetrd_rate"].ToString());
                    }
                }

                /*******************
                刪除空白列
                *******************/
                if (rowTol > rowNum) {

                    worksheet2.Rows.Remove(rowNum + 1, rowTol - rowNum);
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