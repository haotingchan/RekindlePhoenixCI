using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using System.IO;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;

namespace PhoenixCI.FormUI.PrefixS {
    public partial class WS0020 : FormParent {
        private DS0020 daoS0020;

        public WS0020(string programID, string programName) : base(programID, programName) {
            daoS0020 = new DS0020();
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dtSP4 = new DataTable();
            DataTable dtSP5 = new DataTable();
            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, Filename);

            try {
                workbook.LoadDocument(destinationFilePath);
                dtSP4 = daoS0020.GetSP4Data(txtDate.DateTimeValue.ToString("yyyy/MM/dd"));
                dtSP5 = daoS0020.GetSP5Data(txtDate.DateTimeValue.ToString("yyyy/MM/dd"));

                if (dtSP4.Rows.Count <= 0) {
                    ExportShow.Hide();
                    MessageDisplay.Info("無任何資料");
                    return ResultStatus.Fail;
                }

                //將SP4資料放入Excel
                Worksheet worksheet = workbook.Worksheets[0];
                worksheet.Cells[0, 3].Value = txtDate.DateTimeValue.ToString("yyyy/MM/dd");
                for (int i = 0; i < dtSP4.Rows.Count; i++) {
                    int row = dtSP4.Rows[i]["SP4_TYPE"].AsInt();
                    worksheet.Cells[row + 7, 1].Value = dtSP4.Rows[i]["sp4_span_cnt"].AsInt();
                    worksheet.Cells[row + 7, 2].Value = dtSP4.Rows[i]["sp4_mkt_cnt"].AsInt();
                }

                //將SP5資料放入Excel(◎尚未申報者:)
                if (dtSP5.Rows.Count > 0) {
                    //從Excel 第40列開始塞資料
                    for (int i = 39; i < dtSP5.Rows.Count + 39; i++) {
                        worksheet.Cells[i, 0].Value = dtSP5.Rows[i - 39]["sp5_brk_no"].AsString() + '－' +
                            dtSP5.Rows[i - 39]["sp5_brk_abbr_name"].AsString();
                    }
                }
                workbook.SaveDocument(destinationFilePath);
            }
            catch (Exception ex) {
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