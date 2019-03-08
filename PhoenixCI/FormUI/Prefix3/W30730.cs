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
    public partial class W30730 : FormParent {
        private D30730 dao30730;

        public W30730(string programID, string programName) : base(programID, programName) {
            dao30730 = new D30730();

            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;

            ExportShow.Hide();
        }

        protected override ResultStatus Export() {
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();

            Workbook workbook = new Workbook();
            DataTable dtAM6 = new DataTable();
            DataTable dtAM0 = new DataTable();

            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, Filename);
            DateTime date = txtDate.DateTimeValue;
            int oleRow = 1;

            try {
                workbook.LoadDocument(destinationFilePath);

                #region Export
                //交易輔助人
                dtAM6 = dao30730.GetAM6Data(date.ToString("yyyyMM"));
                dtAM0 = dao30730.GetAM0Data(date.ToString("yyyyMM"));

                if (dtAM6.Rows.Count <= 0 && dtAM0.Rows.Count <= 0) {
                    ExportShow.Hide();
                    MessageDisplay.Info(date + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
                    return ResultStatus.Fail;
                }

                if (dtAM6.Rows.Count <= 0) {
                    dtAM6.Rows.Add();
                    dtAM6.Rows[0]["am6_ym"] = date.ToString("yyyyMM");
                    dtAM6.Rows[0]["am6_trade_aux"] = 0;
                }

                foreach (DataRow r in dtAM0.Rows) {
                    int v = r["am0_m_qnty"].AsInt();
                    if (r["am0_brk_type"].ToString() == "9") {
                        dtAM6.Rows[0]["am6_f999"] = v;
                    }
                    else {
                        dtAM6.Rows[0]["am6_f000"] = v;
                    }
                }
                dtAM6.AcceptChanges();

                Worksheet worksheet = workbook.Worksheets["30731"];
                //寫入資料
                foreach (DataRow r in dtAM6.Rows) {
                    worksheet.Cells[oleRow, 0].Value = r["am6_trade_aux"].AsDecimal();
                    worksheet.Cells[oleRow, 1].Value = r["am6_f000"].AsDecimal();
                    worksheet.Cells[oleRow, 2].Value = r["am6_f999"].AsDecimal();
                    worksheet.Cells[oleRow, 3].Value = r["am6_000"].AsDecimal();
                    worksheet.Cells[oleRow, 4].Value = r["am6_999"].AsDecimal();
                }

                worksheet = workbook.Worksheets["30730"];
                worksheet.Range["A1"].Select();
                worksheet.Cells[0, 1].Value = "期貨商及交易輔助人" + date.Year.ToString() + "年" + date.Month.ToString() + "月份期貨交易量統計表";
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