using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;

namespace PhoenixCI.FormUI.Prefix3 {
    public partial class W30760 : FormParent {
        private D30760 dao30760;

        public W30760(string programID, string programName) : base(programID, programName) {
            dao30760 = new D30760();

            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtSDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();

            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, Filename);
            string sYm = txtSDate.DateTimeValue.ToString("yyyyMM");
            string eYm = txtEDate.DateTimeValue.ToString("yyyyMM");
            string eymd = string.IsNullOrEmpty(dao30760.GetMaxDate(eYm)) ? eYm + "01" : dao30760.GetMaxDate(eYm);//讀取迄年月的最大之交易日
            int oleRow = 5, oleCol = 0;

            try {
                workbook.LoadDocument(destinationFilePath);

                //各產品資料
                Worksheet worksheet = workbook.Worksheets[0];
                DataTable dtProdData = dao30760.GetProdData(sYm, eYm, eymd);
                DataTable dtSum = dao30760.GetSumData(sYm, eYm, eymd);
                if (dtProdData.Rows.Count <= 0) {
                    ExportShow.Text = "各產品無任何資料!";
                }
                ProdData(worksheet, dtProdData, dtSum, oleRow, oleCol);

                //個股選擇權資料
                worksheet = workbook.Worksheets[1];
                DataTable dtTradedData = dao30760.GetTradedData(sYm, eYm, eymd, "O");
                if (dtTradedData.Rows.Count <= 0) {
                    ExportShow.Text = "個股選擇權無任何資料!";
                }
                oleRow = 5;
                TradedData(worksheet, dtTradedData, oleRow, oleCol);

                //個股期貨資料
                worksheet = workbook.Worksheets[2];
                DataTable dtFuturesData = dao30760.GetFuturesData(sYm, eYm, eymd, "F");
                if (dtFuturesData.Rows.Count <= 0) {
                    ExportShow.Text = "個股期貨無任何資料!";
                }
                oleRow = 5;
                FuturesData(worksheet, dtFuturesData, oleRow, oleCol);

                //Save
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

        private void ProdData(Worksheet worksheet, DataTable dtProdData,DataTable dtSum, int oleRow, int oleCol) {
            //各產品資料
            int endRow = oleRow + 50, rowTol = endRow;
            worksheet.Cells[0, 0].Value = txtSDate.DateTimeValue.ToString("yyyy/MM") + "~" +
                txtEDate.DateTimeValue.ToString("yyyy/MM") + worksheet.Cells[0, 0].Value.ToString();

            DataTable dtImport = dtProdData.Copy();
            dtImport.Columns.Remove("AI2_PROD_TYPE");
            worksheet.Import(dtImport, false, oleRow, oleCol);

            //小記 by ai2_prod_type
            worksheet.Import(dtSum, false, endRow, 3);
            
            DeleteRow(worksheet, dtProdData.Rows.Count + oleRow, rowTol);
        }

        private void TradedData(Worksheet worksheet, DataTable dtTradedData, int oleRow, int oleCol) {
            //個股選擇權資料
            worksheet.Cells[0, 0].Value = txtSDate.DateTimeValue.ToString("yyyy/MM") + "~" +
                txtEDate.DateTimeValue.ToString("yyyy/MM") + worksheet.Cells[0, 0].Value.ToString();

            worksheet.Import(dtTradedData, false, oleRow, oleCol);

            DeleteRow(worksheet, dtTradedData.Rows.Count + oleRow, 505);
        }

        private void FuturesData(Worksheet worksheet, DataTable dtTradedData, int oleRow, int oleCol) {
            //個股選擇權資料
            worksheet.Cells[0, 0].Value = txtSDate.DateTimeValue.ToString("yyyy/MM") + "~" +
                txtEDate.DateTimeValue.ToString("yyyy/MM") + worksheet.Cells[0, 0].Value.ToString();

            worksheet.Import(dtTradedData, false, oleRow, oleCol);           

            DeleteRow(worksheet, dtTradedData.Rows.Count + oleRow, 505);

        }

        private void DeleteRow(Worksheet worksheet, int oleRow, int rowTol) {
            //刪除空白列
            if (rowTol > oleRow) {
                Range ra = worksheet.Range[(oleRow + 1).ToString() + ":" + rowTol.ToString()];
                ra.Delete(DeleteMode.EntireRow);
            }
        }
    }
}