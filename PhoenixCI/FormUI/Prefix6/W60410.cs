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
using System.Linq;

namespace PhoenixCI.FormUI.Prefix6
{
    public partial class W60410 : FormParent
    {
        /// <summary>
        /// 1. 未符合「成份股檔數≧10」
        /// </summary>
        private int _CON1 = 10;

        /// <summary>
        /// 2. 未符合「權重最大之成份股權重≦30%」
        /// </summary>
        private Decimal _CON2 = 0.3M;

        /// <summary>
        /// 3. 未符合「權重前五大成份股合計權重≦60%」
        /// </summary>
        private Decimal _CON3 = 0.6M;

        /// <summary>
        /// 4. 未符合「最低25%權重之成份股，檔數在15檔(含)以上，過去半年每日合計成交值之平均值＞3,000萬美元」
        /// </summary>
        private int _CON4_1 = 15;

        private Decimal _CON4_2 = 3000;

        /// <summary>
        /// 5. 未符合「最低25%權重之成份股，檔數低於15檔，過去半年每日合計成交值之平均值＞5,000萬美元」
        /// </summary>
        private Decimal _CON5 = 5000;

        private DateTime _Date;

        private DateTime _3M;

      private D60410 dao60410;

        public W60410(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

            dao60410 = new D60410();
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
            _Date = txtDate.DateTimeValue;
            _3M = dao60410.GetRelativeDate(txtDate.FormatValue,3, "MONTH");

            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            Worksheet worksheet = workbook.Worksheets[0];

            #region 60410b

            DataTable dtContentB = dao60410.List60410b(_3M, _Date, _CON1, _CON2, _CON3, _CON4_1, _CON4_2 * 10000, _CON5 * 10000);

            if (dtContentB.Rows.Count == 0)
            {
                MessageDisplay.Info(string.Format("{0},{1},{2},無任何資料!", txtDate.Text, "60410b", _ProgramName));
            }
            else
            {
                #region 明細

                worksheet.Cells[1, 10].Value = _Date;
                worksheet.Cells[2, 10].Value = _3M;
                worksheet.Cells[3, 10].Value = new AOCF().GetAOCFDates(_3M.ToString("yyyyMMdd"), _Date.ToString("yyyyMMdd"));

                int seqNo = 0;
                for (int i = 0; i < dtContentB.Rows.Count; i++)
                {
                    seqNo = dtContentB.Rows[i]["RPT_SEQ_NO"].AsInt();
                    if (seqNo == 0)
                    {
                        continue;
                    }
                    for (int j = 2; j < 7; j++)
                    {
                        worksheet.Cells[seqNo - 1, j].SetValue(dtContentB.Rows[i][j]);
                    }
                }
                worksheet.Range["A1"].Select();

                #endregion 明細
            }

            #endregion 60410b

            #region 60410a

            DataTable dtContentA = dao60410.List60410a(_3M, _Date);
            var query = dtContentA.AsEnumerable().Where(x => x.Field<string>("ymd") == _Date.ToString("yyyyMMdd")).ToList();

            if (query.Count() == 0)
            {
                MessageDisplay.Info(string.Format("{0},{1},{2},無任何資料!", txtDate.Text, "60410a", _ProgramName));
            }
            else
            {
                worksheet.Cells[4, 10].Value = query[0]["day_exchange_rate"].AsDouble();
                int seqNo = 0;
                for (int i = 0; i < query.Count(); i++)
                {
                    seqNo = query[i]["RPT_SEQ_NO"].AsInt();
                    if (seqNo == 0)
                    {
                        continue;
                    }
                    for (int j = 2; j < 6; j++)
                    {
                        worksheet.Cells[seqNo - 1, j].Value = query[i][j].AsDouble();
                        if (j == 5)
                        {
                            if (query[i][j].AsDouble() > _CON4_1)
                            {
                                j = 6;
                            }
                            else
                            {
                                j = 7;
                            }
                            worksheet.Cells[seqNo - 1, j].Value = Math.Round(query[i]["avg_amt_mth_usd"].AsDouble() / 10000);
                            worksheet.Cells[seqNo - 1, 8].Value = Math.Round(query[i]["day_amt_mth_usd"].AsDouble() / 10000);
                        }
                    }
                }
            }

            //1. 未符合「成份股檔數≧10」
            worksheet = workbook.Worksheets[1];
            worksheet.Range["A1"].Select();
            int row = 3;
            for (int i = 0; i < dtContentA.Rows.Count; i++)
            {
                if (dtContentA.Rows[i]["tot_cnt"].AsDouble() < _CON1)
                {
                    row++;
                    worksheet.Cells[row, 0].Value = dtContentA.Rows[i]["cod_name"].AsString();
                    worksheet.Cells[row, 1].Value = dtContentA.Rows[i]["ymd"].AsString();
                    worksheet.Cells[row, 2].Value = dtContentA.Rows[i]["tot_cnt"].AsDouble();
                }
            }

            //4. 未符合「最低25%權重之成份股，檔數在15檔(含)以上，過去半年每日合計成交值之平均值＞3,000萬美元」
            worksheet = workbook.Worksheets[4];
            worksheet.Range["A1"].Select();
            row = 4;
            for (int i = 0; i < dtContentA.Rows.Count; i++)
            {
                if (dtContentA.Rows[i]["cnt25"].AsDouble() >= _CON4_1 && Math.Round(dtContentA.Rows[i]["avg_amt_mth_usd"].AsDecimal() / 10000) <= _CON4_2)
                {
                    row++;
                    worksheet.Cells[row, 0].Value = dtContentA.Rows[i]["cod_name"].AsString();
                    worksheet.Cells[row, 1].Value = dtContentA.Rows[i]["ymd"].AsString();
                    worksheet.Cells[row, 2].Value = dtContentA.Rows[i]["cnt25"].AsDouble();
                    worksheet.Cells[row, 3].Value = dtContentA.Rows[i]["avg_amt_cls_usd"].AsDecimal();
                    worksheet.Cells[row, 4].Value = dtContentA.Rows[i]["avg_amt_cls_tw"].AsDecimal();
                    worksheet.Cells[row, 5].Value = dtContentA.Rows[i]["avg_amt_mth_usd"].AsDecimal();
                    worksheet.Cells[row, 6].Value = dtContentA.Rows[i]["avg_amt_mth_tw"].AsDecimal();
                }
            }

            //5. 未符合「最低25%權重之成份股，檔數低於15檔，過去半年每日合計成交值之平均值＞5,000萬美元」
            worksheet = workbook.Worksheets[5];
            worksheet.Range["A1"].Select();
            row = 4;
            for (int i = 0; i < dtContentA.Rows.Count; i++)
            {
                if (dtContentA.Rows[i]["cnt25"].AsDouble() < _CON4_1 && Math.Round(dtContentA.Rows[i]["avg_amt_mth_usd"].AsDecimal() / 10000) <= _CON5)
                {
                    row++;
                    worksheet.Cells[row, 0].Value = dtContentA.Rows[i]["cod_name"].AsString();
                    worksheet.Cells[row, 1].Value = dtContentA.Rows[i]["ymd"].AsString();
                    worksheet.Cells[row, 2].Value = dtContentA.Rows[i]["cnt25"].AsDouble();
                    worksheet.Cells[row, 3].Value = dtContentA.Rows[i]["avg_amt_cls_usd"].AsDecimal();
                    worksheet.Cells[row, 4].Value = dtContentA.Rows[i]["avg_amt_cls_tw"].AsDecimal();
                    worksheet.Cells[row, 5].Value = dtContentA.Rows[i]["avg_amt_mth_usd"].AsDecimal();
                    worksheet.Cells[row, 6].Value = dtContentA.Rows[i]["avg_amt_mth_tw"].AsDecimal();
                }
            }

            #endregion 60410a

            #region 60410_2

            DataTable dtContent2 = dao60410.List60410_2(_3M, _Date, _CON2);
            //2. 未符合「權重最大之成份股權重≦30%」
            worksheet = workbook.Worksheets[2];
            worksheet.Range["A1"].Select();
            row = 3;
            if (dtContent2.Rows.Count > 0)
            {
                for (int i = 0; i < dtContent2.Rows.Count; i++)
                {
                    if (dtContent2.Rows[i]["index_weight"].AsDouble() <= 0.3)
                    {
                        continue;
                    }
                    row++;
                    for (int j = 0; j < 5; j++)
                    {
                        worksheet.Cells[row, j].SetValue(dtContent2.Rows[i][j]);
                    }
                }
            }

            #endregion 60410_2

            #region 60410_3

            DataTable dtContent3 = dao60410.List60410_3(_3M, _Date, _CON3);
            //3. 未符合「權重前五大成份股合計權重≦60%」
            worksheet = workbook.Worksheets[3];
            worksheet.Range["A1"].Select();
            row = 3;
            if (dtContent3.Rows.Count > 0)
            {
                for (int i = 0; i < dtContent3.Rows.Count; i++)
                {
                    row++;
                    for (int j = 0; j < 5; j++)
                    {
                        worksheet.Cells[row, j].SetValue(dtContent3.Rows[i][j]);
                    }
                }
            }

            #endregion 60410_3

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