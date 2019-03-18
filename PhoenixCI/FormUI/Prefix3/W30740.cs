using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using System.IO;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System.Globalization;
using DevExpress.Spreadsheet.Charts;
using BaseGround.Shared;

namespace PhoenixCI.FormUI.Prefix3 {
    public partial class W30740 : FormParent {
        private D30740 dao30740;

        public W30740(string programID, string programName) : base(programID, programName) {
            dao30740 = new D30740();

            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dtAM1 = new DataTable();
            DataTable dtAM4 = new DataTable();

            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, Filename);
            DateTime date = txtDate.DateTimeValue;
            int startRow = 6, oleRow = startRow, oleTol = startRow + 24;
            string ymd = "";

            try {
                workbook.LoadDocument(destinationFilePath);
                Worksheet worksheet = workbook.Worksheets[0];

                TaiwanCalendar tai = new TaiwanCalendar();
                worksheet.Cells[oleRow, 0].Value = tai.GetYear(date).ToString();

                //市場總成交量雙邊(A)
                dtAM1 = dao30740.GetAM1Data(date);
                if (dtAM1.Rows.Count <= 0) {
                    MessageDisplay.Info(date.ToString("yyyyMM") + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
                }

                foreach (DataRow r in dtAM1.Rows) {
                    if (ymd != r["am1_ymd"].ToString()) {
                        oleRow += 2;
                        DateTime am1Ymd = r["am1_ymd"].AsDateTime("yyyyMM");
                        worksheet.Cells[oleRow, 0].Value = am1Ymd.Month.ToString();
                        ymd = r["am1_ymd"].ToString();
                    }
                    if (r["am1_prod_type"].ToString() == "F") {
                        worksheet.Cells[oleRow, 1].Value = r["am1_m_qnty"].AsInt();
                    }
                    else {
                        worksheet.Cells[oleRow + 1, 1].Value = r["am1_m_qnty"].AsInt();
                    }
                }

                //刪除空白列
                if (oleTol > oleRow) {
                    Range ra = worksheet.Range[(oleRow + 3).ToString() + ":32"];
                    ra.Delete(DeleteMode.EntireRow);
                }

                //網路下單
                dtAM4 = dao30740.GetAM4Data(date);
                if (dtAM4.Rows.Count <= 0) {
                    MessageDisplay.Info(date.ToString("yyyyMM") + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
                    return ResultStatus.Fail;
                }

                ymd = "";
                oleRow = startRow;
                foreach (DataRow r in dtAM4.Rows) {
                    if (ymd != r["am4_ym"].ToString()) {
                        oleRow += 2;
                        ymd = r["am4_ym"].ToString();
                    }
                    worksheet.Cells[oleRow, 3].Value = r["am4_f_qnty"].AsInt();
                    worksheet.Cells[oleRow, 5].Value = r["am4_o_qnty"].AsInt();
                    worksheet.Cells[oleRow, 8].Value = r["am4_trade_count"].AsInt();
                }
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