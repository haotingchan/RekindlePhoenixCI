using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;

namespace PhoenixCI.FormUI.Prefix3
{
    public partial class W30689 : FormParent
    {
        private TPRICES_OPT daoTPRICES_OPT;
        private DataTable _Data = new DataTable();

        public W30689(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            daoTPRICES_OPT = new TPRICES_OPT();

            this.Text = _ProgramID + "─" + _ProgramName;
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

            ExportShow.Hide();
        }

        protected override ResultStatus Export()
        {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dt = new DataTable();

            try
            {
                string ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
                _Data = daoTPRICES_OPT.ListAllByDate(ymd);
                if (_Data.Rows.Count <= 0)
                {
                    MessageDisplay.Info(txtDate.FormatValue + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
                    ExportShow.Text = "轉檔失敗";
                    return ResultStatus.Fail;
                }

                string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);

                workbook.LoadDocument(destinationFilePath);
                Worksheet worksheet = workbook.Worksheets[0];

                //買權
                dt = _Data.Filter("TRIM(TPRICES_PC_CODE) = 'C'");
                ExportData(dt, worksheet);

                worksheet = workbook.Worksheets[1];
                //賣權
                dt = _Data.Filter("TRIM(TPRICES_PC_CODE) = 'P'");
                ExportData(dt, worksheet);

                workbook.SaveDocument(destinationFilePath);
            }
            catch (Exception ex)
            {
                ExportShow.Text = "轉檔失敗";
                WriteLog(ex);
                return ResultStatus.Fail;
            }
            ExportShow.Text = "轉檔成功!";
            return ResultStatus.Success;
        }

        private void ExportData(DataTable dt, Worksheet worksheet)
        {
            int startRow = 1, oleRow = startRow, oleTol;
            foreach (DataRow r in dt.Rows)
            {
                worksheet.Cells[oleRow, 0].Value = r["TPRICES_TRADE_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                worksheet.Cells[oleRow, 1].Value = r["TPRICES_KIND_ID"].AsString();
                worksheet.Cells[oleRow, 2].Value = r["TPRICES_SETTLE_MONTH"].AsString();
                worksheet.Cells[oleRow, 3].Value = r["TPRICES_FUT_PRICE"].AsDecimal();
                worksheet.Cells[oleRow, 4].Value = r["TPRICES_STRIKE_PRICE"].AsDecimal();
                worksheet.Cells[oleRow, 5].Value = r["TPRICES_RISK_FREE_RATE"].AsDecimal();
                worksheet.Cells[oleRow, 6].Value = r["TPRICES_DIST_TIME"].AsDecimal();
                worksheet.Cells[oleRow, 7].Value = r["TPRICES_VOLATILITY"].AsDecimal();
                worksheet.Cells[oleRow, 8].Value = r["TPRICES_PC_CODE_NAME"].AsString();
                worksheet.Cells[oleRow, 11].Value = r["TPRICES_R_POINT"].AsDecimal();
                oleRow++;
            }
            oleTol = worksheet.GetUsedRange().RowCount;
            //刪除空白列
            if (oleTol > oleRow)
            {
                Range ra = worksheet.Range[$"{oleRow + 1}:{oleTol - 1}"];
                ra.Delete(DeleteMode.EntireRow);
            }
            worksheet.ScrollToRow(0);
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();
            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }
    }
}