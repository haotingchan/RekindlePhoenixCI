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
using BaseGround.Shared;

namespace PhoenixCI.FormUI.Prefix3 {
    public partial class W30723 : FormParent {
        private RPT daoRPT;
        private AM2 daoAm2;
        private AI2 daoAI2;
        private RAMM1 daoRamm1;

        public W30723(string programID, string programName) : base(programID, programName) {
            daoRPT = new RPT();
            daoAm2 = new AM2();
            daoAI2 = new AI2();
            daoRamm1 = new RAMM1();

            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dtAM2 = new DataTable();
            DataTable dtRPT = new DataTable();
            DataTable dtAi2 = new DataTable();
            DataTable dtRamm1 = new DataTable();

            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, Filename);
            DateTime date = txtDate.DateTimeValue;
            string asParamKey="MXF";
            int oleRow = 1;

            try {
                workbook.LoadDocument(destinationFilePath);

                #region Get AM2 Data
                dtAM2 = daoAm2.ListAm2DataByYmd(date.ToString("yyyyMM"), asParamKey);

                if (dtAM2.Rows.Count <= 0) {
                    ExportShow.Hide();
                    MessageDisplay.Info(date + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
                    return ResultStatus.Fail;
                }
                #endregion

                Worksheet worksheet = workbook.Worksheets["30720"];

                #region Get RPT
                dtRPT = daoRPT.ListData("%" + _ProgramID + "%");

                if (dtRPT.Rows.Count <= 0) {
                    MessageDisplay.Info(_ProgramID + '－' + "RPT無任何資料!");
                }

                //填寫日期
                TaiwanCalendar tai = new TaiwanCalendar();
                worksheet.Cells[0, 6].Value = tai.GetYear(date).ToString() + "年" + tai.GetMonth(date) + "月" + worksheet.Cells[0, 6].Value;
                worksheet.Cells[1, 4].Value = date.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US")) + "." + date.Year.ToString() + worksheet.Cells[1, 4].Value;

                string paramKey = "";
                foreach (DataRow r in dtAM2.Rows) {
                    if (paramKey != (r["am2_param_key"].ToString().Trim())) {
                        paramKey = r["am2_param_key"].ToString().Trim();
                        oleRow = dtRPT.Rows.IndexOf(dtRPT.Select("trim(rpt_value) = '" + paramKey + "'")[0]);
                        if (oleRow >= 0) {
                            oleRow = dtRPT.Rows[oleRow]["rpt_seq_no"].AsInt();
                        }
                    }

                    int oleCol = GetCol(r["am2_idfg_type"].AsInt(), r["am2_bs_code"].AsString());
                    if (oleRow > 0 && oleCol > 0) {
                        worksheet.Cells[oleRow - 1, oleCol - 1].Value = r["am2_m_qnty"].AsInt();
                    }
                }
                #endregion

                #region Get AI2
                dtAi2 = daoAI2.ListAI2ByYmd(date.ToString("yyyyMM"), date.ToString("yyyyMM"), asParamKey);

                if (dtAi2.Rows.Count <= 0) {
                    MessageDisplay.Info(_ProgramID + '－' + "AI2無任何資料!");
                }

                //切換sheet
                paramKey = "";
                foreach (DataRow r in dtAi2.Rows) {
                    if (paramKey != (r["ai2_param_key"].ToString().Trim())) {
                        paramKey = r["ai2_param_key"].ToString().Trim();
                        oleRow = dtRPT.Rows.IndexOf(dtRPT.Select("trim(rpt_value) = '" + paramKey + "'")[0]);
                        if (oleRow >= 0) {
                            oleRow = dtRPT.Rows[oleRow]["rpt_seq_no"].AsInt();
                        }
                    }
                    if (oleRow > 0) {
                        worksheet.Cells[oleRow - 1, 2].Value = r["ai2_m_qnty"].AsInt();
                        worksheet.Cells[oleRow - 1, 3].Value = r["ai2_oi"].AsInt();
                    }
                }
                #endregion

                #region Ger Ramm1
                dtRamm1 = daoRamm1.ListRamm1Ymd(date.ToString("yyyyMM") + "01", date.ToString("yyyyMM") + "31", asParamKey);

                if (dtRamm1.Rows.Count <= 0) {
                    MessageDisplay.Info(_ProgramID + '－' + "Ramm1無任何資料!");
                }

                paramKey = "";
                foreach (DataRow r in dtRamm1.Rows) {
                    if (paramKey != (r["param_key"].ToString().Trim())) {
                        paramKey = r["param_key"].ToString().Trim();
                        oleRow = dtRPT.Rows.IndexOf(dtRPT.Select("trim(rpt_value) = '" + paramKey + "'")[0]);
                        if (oleRow >= 0) {
                            oleRow = dtRPT.Rows[oleRow]["rpt_seq_no"].AsInt();
                        }
                    }
                    if (oleRow > 0) {
                        worksheet.Cells[oleRow - 1, 16].Value = r["bo"].AsInt();
                        worksheet.Cells[oleRow - 1, 17].Value = r["bq"].AsInt();
                        worksheet.Cells[oleRow - 1, 18].Value = r["so"].AsInt();
                        worksheet.Cells[oleRow - 1, 19].Value = r["sq"].AsInt();
                    }
                }
                #endregion

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

        private int GetCol(int idfgType, string BsCode) {
            int oleCol = 0;

            switch (idfgType) {
                case (int)FuturesBroker.SecuritiesDealer: {
                    if (BsCode == "B") oleCol = 7;
                    else oleCol = 8;
                    break;
                }
                case (int)FuturesBroker.Investment: {
                    if (BsCode == "B") oleCol = 9;
                    else oleCol = 10;
                    break;
                }
                case (int)FuturesBroker.ForeignInvestor: {
                    if (BsCode == "B") oleCol = 11;
                    else oleCol = 12;
                    break;
                }
                case (int)FuturesBroker.FuturesManager: {
                    if (BsCode == "B") oleCol = 13;
                    else oleCol = 14;
                    break;
                }
                case (int)FuturesBroker.RetailInvestors: {
                    if (BsCode == "B") oleCol = 15;
                    else oleCol = 16;
                    break;
                }
                case (int)FuturesBroker.Dealer: {
                    if (BsCode == "B") oleCol = 21;
                    else oleCol = 22;
                    break;
                }
                case (int)FuturesBroker.NaturalPerson: {
                    if (BsCode == "B") oleCol = 5;
                    else oleCol = 6;
                    break;
                }
            }
            return oleCol;
        }

        private enum FuturesBroker {
            //證券自營
            SecuritiesDealer = 1,
            //證券投信
            Investment,
            //外資
            ForeignInvestor,
            //期貨經理
            FuturesManager,
            //其他機構
            RetailInvestors,
            //自營
            Dealer,
            //自然人
            NaturalPerson
        }
    }
}