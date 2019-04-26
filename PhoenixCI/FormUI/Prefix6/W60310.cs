using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;

namespace PhoenixCI.FormUI.Prefix6
{
    public partial class W60310 : FormParent
    {
      private D60310 dao60310;
      private RPTF daoRPTF;

      public W60310(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            int weekNow = Convert.ToInt32(GlobalInfo.OCF_DATE.DayOfWeek);
            weekNow = (weekNow == 0 ? 7 - 1 : weekNow - 1)*-1;
            //本周第一天(星期一)
            txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE.AddDays(weekNow);
            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
            dao60310 = new D60310();
            daoRPTF = new RPTF();
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
            int year = txtStartDate.DateTimeValue.Year;
            string rptYear = (year - 1911).ToString();

            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            Worksheet worksheet = workbook.Worksheets[0];

            #region 表頭

            DataTable dtAM7 = dao60310.ListAM7(year.ToString());

            double futAvgQnty = 0;
            double optAvgQnty = 0;
            double tax = 0;
            double totDayCount = 0;

            if (dtAM7.Rows.Count > 0)
            {
                futAvgQnty = dtAM7.Rows[0]["AM7_FUT_AVG_QNTY"] == null ? futAvgQnty : dtAM7.Rows[0]["AM7_FUT_AVG_QNTY"].AsDouble();
                optAvgQnty = dtAM7.Rows[0]["AM7_OPT_AVG_QNTY"] == null ? optAvgQnty : dtAM7.Rows[0]["AM7_OPT_AVG_QNTY"].AsDouble();
                tax = dtAM7.Rows[0]["AM7_FC_TAX"] == null ? tax : dtAM7.Rows[0]["AM7_FC_TAX"].AsDouble();
                totDayCount = dtAM7.Rows[0]["AM7_DAY_COUNT"] == null ? totDayCount : dtAM7.Rows[0]["AM7_DAY_COUNT"].AsDouble();
            }

            //102年達成目標交易量之所需日均量與成長率以及稅收達成率
            worksheet.Cells[0, 0].Value = rptYear + worksheet.Cells[0, 0].Value;
            //102年期貨(含非股價類期貨)目標總量: 53,944,800口
            worksheet.Cells[1, 1].Value = rptYear + worksheet.Cells[1, 1].Value + (futAvgQnty * totDayCount).ToString("#0,000") + "口";
            //目標日均量:218,400口(總交易天數:247天)
            worksheet.Cells[1, 6].Value = worksheet.Cells[1, 6].Value + futAvgQnty.ToString("#0,000") + "口(總交易天數：" + totDayCount.ToString() + "天)";
            //目標總稅收: 30.14億元
            worksheet.Cells[1, 10].Value = worksheet.Cells[1, 10].Value + tax.ToString() + "億元";
            //102年選擇權目標總量: 104,456,300口
            worksheet.Cells[33, 1].Value = rptYear + worksheet.Cells[33, 1].Value + (optAvgQnty * totDayCount).ToString("#0,000") + "口";
            //目標日均量:422,900口
            worksheet.Cells[33, 6].Value = worksheet.Cells[33, 6].Value + optAvgQnty.ToString("#0,000") + "口";

            #endregion 表頭

            #region 明細

            DataTable dtContent = dao60310.ListData(txtStartDate.FormatValue, txtEndDate.FormatValue);
            if (dtContent.Rows.Count == 0)
            {
                MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtStartDate.Text, this.Text));
            }
            else
            {
                int futStartRow = 2;
                int futEndRow = 2;
                int optStartRow = 34;
                int optEndRow = 34;
                int rowIndex = 0;
                double dayAvgQnty = 0;
                double dayCount = 0;
                double totTax = 0;
                for (int i = 0; i < dtContent.Rows.Count; i++)
                {
                    string prodType = dtContent.Rows[i]["PROD_TYPE"].AsString();
                    if (prodType == "F")
                    {
                        futEndRow++;
                        rowIndex = futEndRow;
                        dayAvgQnty = futAvgQnty;
                    }
                    else
                    {
                        optEndRow++;
                        rowIndex = optEndRow;
                        dayAvgQnty = optAvgQnty;
                    }
                    worksheet.Cells[rowIndex, 1].Value = dtContent.Rows[i]["YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                    dayCount = dtContent.Rows[i]["DAY_COUNT"].AsDouble();
                    worksheet.Cells[rowIndex, 2].Value = dayCount;
                    worksheet.Cells[rowIndex, 3].Value = dtContent.Rows[i]["DAY_QNTY"].AsDouble();
                    double yearQnty = dtContent.Rows[i]["YEAR_QNTY"].AsDouble();
                    worksheet.Cells[rowIndex, 4].Value = yearQnty;
                    double yearAvgQnty = yearQnty / dayCount;
                    worksheet.Cells[rowIndex, 5].Value = yearAvgQnty;
                    worksheet.Cells[rowIndex, 6].Value = totDayCount - dayCount;
                    if (totDayCount - dayCount > 0)
                    {
                        worksheet.Cells[rowIndex, 7].Value = ((dayAvgQnty * totDayCount) - yearQnty) / (totDayCount - dayCount);
                        worksheet.Cells[rowIndex, 8].Value = Math.Pow(((dayAvgQnty * totDayCount) / (yearAvgQnty * totDayCount)), (totDayCount / (totDayCount - dayCount))) - 1;
                        worksheet.Cells[rowIndex, 9].Value = Math.Pow(((dayAvgQnty * totDayCount) / (yearAvgQnty * totDayCount)), (1 / (totDayCount - dayCount))) - 1;
                    }
                    worksheet.Cells[rowIndex, 10].Value = dtContent.Rows[i]["DAY_TAX"].AsString();
                    worksheet.Cells[rowIndex, 11].Value = dtContent.Rows[i]["YEAR_TAX"].AsString();
                }
                worksheet.Cells[futStartRow + ((futEndRow - futStartRow) / 2), 0].Value = "期貨";
                worksheet.Cells[optStartRow + ((optEndRow - optStartRow) / 2), 0].Value = "選擇權";

                #endregion 明細

                #region 表尾

                totTax = worksheet.Cells[futEndRow, 11].Value.AsDouble() + worksheet.Cells[optEndRow, 11].Value.AsDouble();
                rowIndex = 65;
                worksheet.Cells[rowIndex, 11].Value = totTax;
                rowIndex++;
                if (totTax > 0)
                {
                    if (tax > 0)
                    {
                        worksheet.Cells[rowIndex, 11].Value = totTax / 100000000 / tax;
                    }
                }

                DataTable dtRPTF = daoRPTF.ListData(_ProgramID, _ProgramID, year.ToString());

                foreach (DataRow row in dtRPTF.Rows)
                {
                    rowIndex++;
                    worksheet.Cells[rowIndex, 0].Value = row["RPTF_TEXT"].ToString();
                }

                #endregion 表尾

                if (optEndRow < optStartRow + 30)
                {
                    optEndRow++;
                    worksheet.Rows.Remove(optEndRow, (64 - optEndRow) + 1);
                }
                if (futEndRow < futStartRow + 30)
                {
                    futEndRow++;
                    worksheet.Rows.Remove(futEndRow, (32 - futEndRow) + 1);
                }
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