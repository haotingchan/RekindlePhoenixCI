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

namespace PhoenixCI.FormUI.Prefix6
{
    public partial class W60320 : FormParent
    {
      private D60320 dao60320;

      public W60320(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

         dao60320 = new D60320();
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
            base.Export();

            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);

            ManipulateExcel(excelDestinationPath);

            return ResultStatus.Success;
        }

        private void ManipulateExcel(string excelDestinationPath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            Worksheet worksheet = workbook.Worksheets[0];

            DataTable dtContent = dao60320.ListData(txtStartDate.FormatValue, txtEndDate.FormatValue);

            if (dtContent.Rows.Count == 0)
            {
                MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtStartDate.Text, this.Text));
            }
            else
            {
                #region 明細

                int rowIndex = 7;
                foreach (DataRow row in dtContent.Rows)
                {
                    worksheet.Cells[rowIndex, 0].Value = row["YMD"].AsDouble().ToString("0000/00/00");
                    worksheet.Cells[rowIndex, 1].Value = row["M_QNTY_I"].AsDouble();
                    worksheet.Cells[rowIndex, 2].Value = row["S_QNTY_I"].AsDouble() + row["M_QNTY_I"].AsDouble();
                    worksheet.Cells[rowIndex, 3].Value = row["T5_QNTY"].AsDouble();
                    worksheet.Cells[rowIndex, 4].Value = row["T3_QNTY"].AsDouble();
                    worksheet.Cells[rowIndex, 5].Value = row["M_AMT_T_I"].AsDouble();
                    worksheet.Cells[rowIndex, 6].Value = row["M_AMT_I"].AsDouble();
                    worksheet.Cells[rowIndex, 7].Value = row["M_QNTY_S"].AsDouble();
                    worksheet.Cells[rowIndex, 8].Value = row["S_QNTY_S"].AsDouble() + row["M_QNTY_S"].AsDouble();
                    worksheet.Cells[rowIndex, 9].Value = row["M_AMT_T_S"].AsDouble();
                    worksheet.Cells[rowIndex, 10].Value = row["M_AMT_S"].AsDouble();
                    worksheet.Cells[rowIndex, 11].Value = row["S_QNTY_I"].AsDouble() + row["M_QNTY_I"].AsDouble() + row["S_QNTY_S"].AsDouble() + row["M_QNTY_S"].AsDouble();
                    worksheet.Cells[rowIndex, 12].Value = Math.Round(row["ACCU_QNTY"].AsDouble() / row["DAY_COUNT"].AsDouble(), 0);
                    worksheet.Cells[rowIndex, 13].Value = row["M_AMT_I"].AsDouble() + row["M_AMT_S"].AsDouble();
                    worksheet.Cells[rowIndex, 14].Value = row["STWD_QNTY"].AsDouble();
                    worksheet.Cells[rowIndex, 15].Value = row["STWD_AMT"].AsDouble();
                    worksheet.Cells[rowIndex, 16].Value = row["AMIF_SUM_AMT"].AsDouble();

                    rowIndex++;
                }
                if (rowIndex < 307)
                {
                    worksheet.Rows.Remove(rowIndex, 306 - rowIndex + 1);
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