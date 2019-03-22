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
    public partial class W30770 : FormParent {
        private D30770 dao30770;
        private COD daoCod;

        public W30770(string programID, string programName) : base(programID, programName) {
            dao30770 = new D30770();
            daoCod = new COD();
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtSym.DateTimeValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01").AsDateTime();
            txtSymd.DateTimeValue = GlobalInfo.OCF_DATE;

            //報表類別 下拉選單
            DataTable exportTypeSource = daoCod.ListByCol2("30770", "ddlb_rpt");
            exportType.SetDataTable(exportTypeSource, "COD_ID", "COD_DESC", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, null);
            exportType.EditValue = "A";

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dtProd = new DataTable();
            DataTable dtDate = new DataTable();

            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, ls_filename);
            string sym = txtSym.DateTimeValue.ToString("yyyyMM");
            string eym = txtEym.DateTimeValue.ToString("yyyyMM");
            string symd = txtSymd.DateTimeValue.ToString("yyyyMMdd");
            string eymd = txtEymd.DateTimeValue.ToString("yyyyMMdd");
            string reportType = exportType.EditValue.ToString() + "%";
            int rowStart = 4, colStart = 0;

            try {

                workbook.LoadDocument(destinationFilePath);
                Worksheet worksheet = workbook.Worksheets[0];

                if (DateReport.Checked) {
                    GenDateReport(symd, eymd, reportType);
                }


                //worksheet.Import(dt, false, rowStart, colStart);

                ////刪除空白列
                //Range ra = worksheet.Range[(dt.Rows.Count + rowStart + 1).ToString() + ":1004"];
                //ra.Delete(DeleteMode.EntireRow);

                //workbook.SaveDocument(destinationFilePath);
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

        private string GenProd(DataTable dtProd) {

            string strProd = "";

            foreach (DataRow drProd in dtProd.Rows) {
                strProd += "'" + drProd[0].AsString() + "'";

                //不是最後一筆加上逗號
                if (drProd != dtProd.Rows[dtProd.Rows.Count - 1]) {
                    strProd += ",";
                }
            }

            return strProd;
        }

        private void GenDateReport(string symd, string eymd, string reportType) {
            //13:45 - 18:15交易量
            DataTable dtProd = dao30770.GetProd("D", symd, eymd, reportType);
            string strProd = GenProd(dtProd);
            DataTable dtDate = dao30770.ListNightTransactionData("D", symd, eymd, reportType, strProd);
            if (dtDate.Rows.Count <= 0) {
                ExportShow.Text = symd + "~" + eymd + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!";
                return;
            }
        }
    }
}