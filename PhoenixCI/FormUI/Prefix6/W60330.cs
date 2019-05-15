using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;

namespace PhoenixCI.FormUI.Prefix6
{
    public partial class W60330 : FormParent
    {
      private D60330 dao60330;

      public W60330(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            txtStartMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEndMonth.DateTimeValue = GlobalInfo.OCF_DATE;

            dao60330 = new D60330();
            ExportShow.Hide();
        }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen()
        {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield();

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall)
        {
            base.Save(pokeBall);

            return ResultStatus.Success;
        }

        protected override ResultStatus Run(PokeBall args)
        {
            base.Run(args);

            return ResultStatus.Success;
        }

        protected override ResultStatus Import()
        {
            base.Import();

            return ResultStatus.Success;
        }

        protected override ResultStatus Export()
        {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            try
            {
                base.Export();

                string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);

                ManipulateExcel(excelDestinationPath);
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

        private void ManipulateExcel(string excelDestinationPath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            Worksheet worksheet = workbook.Worksheets[0];

            DataTable dtContent = dao60330.ListData(txtStartMonth.FormatValue, txtEndMonth.FormatValue);

            var cpDayCnt = dtContent.AsEnumerable().GroupBy(t => new { })
                .Select(g => new
                {
                    CP_DAY_CNT = g.Max(s => s.Field<decimal>("AI2_DAY_COUNT"))
                }).FirstOrDefault();

            if (dtContent.Rows.Count == 0)
            {
                MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtStartMonth.Text, this.Text));
            }
            else
            {
                #region 明細

                worksheet.Cells[2, 0].Value = txtStartMonth.Text + "～" + txtEndMonth.Text + worksheet.Cells[2, 0].Value;
                worksheet.Cells[2, 8].Value = cpDayCnt.CP_DAY_CNT.AsString();

                int rowIndex = 4;
                foreach (DataRow row in dtContent.Rows)
                {
                    worksheet.Cells[rowIndex, 0].Value = row["AI2_PARAM_KEY"].AsString();
                    worksheet.Cells[rowIndex, 1].Value = row["PARAM_NAME"].AsString();
                    worksheet.Cells[rowIndex, 2].SetValue( row["AI2_M_QNTY"]);
                    worksheet.Cells[rowIndex, 3].SetValue(row["AM2_QNTY1"]);
                    worksheet.Cells[rowIndex, 4].SetValue(row["AM2_QNTY2"]);
                    worksheet.Cells[rowIndex, 5].SetValue(row["AM2_QNTY3"]);
                    worksheet.Cells[rowIndex, 6].SetValue(row["AM2_QNTY4"]);
                    worksheet.Cells[rowIndex, 7].SetValue(row["AM2_QNTY5"]);
                    worksheet.Cells[rowIndex, 8].SetValue(row["TAX"]);

                    rowIndex++;
                }

                if (rowIndex < 104)
                {
                    worksheet.Rows.Remove(rowIndex, 103 - rowIndex + 1);
                }
                worksheet.Range["A1"].Select();

                #endregion 明細
            }
            workbook.SaveDocument(excelDestinationPath);
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            base.Print(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow()
        {
            base.InsertRow();

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow();

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }
    }
}