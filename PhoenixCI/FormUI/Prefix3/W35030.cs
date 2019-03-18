using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;

namespace PhoenixCI.FormUI.Prefix3 {
    public partial class W35030 : FormParent {
        private D35030 dao35030;

        public W35030(string programID, string programName) : base(programID, programName) {
            dao35030 = new D35030();
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01").AsDateTime();

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dt = new DataTable();

            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, ls_filename);
            DateTime date = txtDate.DateTimeValue;
            int rowStart = 4, colStart = 0;

            if (txt.EditValue == null) {
                MessageDisplay.Info("請輸入季度資訊 ! ");
                ExportShow.Hide();
                return ResultStatus.FailButNext;
            }

            string yearQ = txt.EditValue.ToString();

            try {

                workbook.LoadDocument(destinationFilePath);
                Worksheet worksheet = workbook.Worksheets[0];

                worksheet.Cells[2, 0].Select();
                worksheet.Cells[2, 0].Value = yearQ;

                dt = dao35030.GetData(yearQ, date, date);
                if (dt.Rows.Count <= 0) {
                    ExportShow.Text = date.ToShortDateString() + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!";
                    return ResultStatus.Fail;
                }
                worksheet.Import(dt, false, rowStart, colStart);
                
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