using ActionService;
using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;

namespace PhoenixCI.FormUI.Prefix6
{
    public partial class W60320 : FormParent
    {
      private D60320 dao60320;

        public W60320(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            int weekNow = Convert.ToInt32(GlobalInfo.OCF_DATE.DayOfWeek);
            weekNow = (weekNow == 0 ? 7 - 1 : weekNow - 1) * -1;
            //本周第一天(星期一)
            txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE.AddDays(weekNow);
            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

            dao60320 = new D60320();
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
            if (txtStartDate.DateTimeValue > txtEndDate.DateTimeValue)
            {
                MessageDisplay.Error("起始日期不得大於結束日期!");
                return ResultStatus.Fail;
            }

            ExportShow.Text = "轉檔中...";
            ExportShow.Show();
            try
            {
                base.Export();
                
                return ManipulateExcel();
            }
            catch (Exception ex)
            {
                ExportShow.Text = "轉檔失敗";
                WriteLog(ex);
                return ResultStatus.Fail;
            }
        }

        private ResultStatus ManipulateExcel()
        {
            string excelDestinationPath = "";
            Workbook workbook = new Workbook();

            DataTable dtContent = dao60320.ListData(txtStartDate.FormatValue, txtEndDate.FormatValue);

            if (dtContent.Rows.Count == 0)
            {
                MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtStartDate.Text, this.Text));
                ExportShow.Text = "轉檔失敗";
                return ResultStatus.Fail;
            }
            else
            {
                excelDestinationPath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);

                workbook.LoadDocument(excelDestinationPath);

                Worksheet worksheet = workbook.Worksheets[0];

                #region 明細
                int rowIndex = 7;
                foreach (DataRow row in dtContent.Rows)
                {
                    worksheet.Cells[rowIndex, 0].Value = row["YMD"].AsDouble().ToString("0000/00/00");
                    worksheet.Cells[rowIndex, 1].Value = row["M_QNTY_I"].AsDouble();
                    worksheet.Cells[rowIndex, 2].Value = row["S_QNTY_I"].AsDouble() + row["M_QNTY_I"].AsDouble();
                    worksheet.Cells[rowIndex, 3].SetValue(row["T5_QNTY"]);
                    worksheet.Cells[rowIndex, 4].SetValue(row["T3_QNTY"]);
                    worksheet.Cells[rowIndex, 5].SetValue(row["M_AMT_T_I"]);
                    worksheet.Cells[rowIndex, 6].SetValue(row["M_AMT_I"]);
                    worksheet.Cells[rowIndex, 7].SetValue(row["M_QNTY_S"]);
                    worksheet.Cells[rowIndex, 8].SetValue(row["S_QNTY_S"].AsDouble() + row["M_QNTY_S"].AsDouble());
                    worksheet.Cells[rowIndex, 9].SetValue(row["M_AMT_T_S"]);
                    worksheet.Cells[rowIndex, 10].SetValue(row["M_AMT_S"]);
                    worksheet.Cells[rowIndex, 11].SetValue(row["S_QNTY_I"].AsDouble() + row["M_QNTY_I"].AsDouble() + row["S_QNTY_S"].AsDouble() + row["M_QNTY_S"].AsDouble());
                    worksheet.Cells[rowIndex, 12].SetValue(Math.Round(row["ACCU_QNTY"].AsDouble() / row["DAY_COUNT"].AsDouble(), 0));
                    worksheet.Cells[rowIndex, 13].SetValue(row["M_AMT_I"].AsDouble() + row["M_AMT_S"].AsDouble());
                    worksheet.Cells[rowIndex, 14].SetValue(row["STWD_QNTY"]);
                    worksheet.Cells[rowIndex, 15].SetValue(row["STWD_AMT"]);
                    worksheet.Cells[rowIndex, 16].SetValue(row["AMIF_SUM_AMT"]);

                    rowIndex++;
                }
                if (rowIndex < 307)
                {
                    worksheet.Rows.Remove(rowIndex, 306 - rowIndex + 1);
                }
                worksheet.Range["A1"].Select();
                workbook.SaveDocument(excelDestinationPath);

                ExportShow.Text = "轉檔成功!";

                return ResultStatus.Success;
                #endregion 明細
            }
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