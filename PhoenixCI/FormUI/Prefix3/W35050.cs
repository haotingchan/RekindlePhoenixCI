using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;

namespace PhoenixCI.FormUI.Prefix3 {
    public partial class W35050 : FormParent {
        private D35050 dao35050;

        public W35050(string programID, string programName) : base(programID, programName) {
            dao35050 = new D35050();

            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dtTse = new DataTable();
            DataTable dtTW50 = new DataTable();

            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, Filename);
            DateTime date = txtDate.DateTimeValue;
            int oleRow = 2, oleCol = 0;

            try {
                workbook.LoadDocument(destinationFilePath);

                //Sheet TSE_OTC
                Worksheet worksheet = workbook.Worksheets[0];
                worksheet.Cells[0, 0].Value = date.ToShortDateString();
                dtTse = dao35050.GetTseOtcData(date);
                worksheet.Import(dtTse, false, oleRow, oleCol);

                //Sheet TW50
                worksheet = workbook.Worksheets[1];
                worksheet.Cells[0, 0].Value = date.ToShortDateString();
                dtTW50 = dao35050.GetTW50Data(date);
                worksheet.Import(dtTW50, false, oleRow, oleCol);

                workbook.SaveDocument(destinationFilePath);
            }
            catch (Exception ex) {
                ExportShow.Text = "轉檔失敗";
                throw ex;
            }
            ExportShow.Text = "轉檔成功!";
            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();
            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }
    }
}