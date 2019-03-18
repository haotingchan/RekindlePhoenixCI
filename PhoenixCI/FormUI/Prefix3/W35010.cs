using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;

namespace PhoenixCI.FormUI.Prefix3 {
    public partial class W35010 : FormParent {
        private D35010 dao35010;

        public W35010(string programID, string programName) : base(programID, programName) {
            dao35010 = new D35010();
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dt = new DataTable();

            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, ls_filename);
            DateTime date = txtDate.DateTimeValue;
            int rowStart = 4;

            try {
                workbook.LoadDocument(destinationFilePath);
                dt = dao35010.GetHDPKData(date, date);

                if (dt.Rows.Count <= 0) {
                    ExportShow.Hide();
                    MessageDisplay.Info(date.ToShortDateString() + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
                    return ResultStatus.Fail;
                }

                //切換sheet
                Worksheet worksheet = workbook.Worksheets[0];

                worksheet.Cells[1, 0].Value = "查詢日期:" + date.ToShortDateString();
                //從A5 開始放資料
                worksheet.Import(dt, false, rowStart, 0);

                //刪除空白列
                Range ra = worksheet.Range[(dt.Rows.Count + rowStart + 1).ToString() + ":1004"];
                ra.Delete(DeleteMode.EntireRow);

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