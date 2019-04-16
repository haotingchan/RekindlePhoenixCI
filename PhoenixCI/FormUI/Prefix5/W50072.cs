﻿using System;
using System.Data;
using BaseGround;
using Common;
using BusinessObjects.Enums;
using BusinessObjects;
using DevExpress.Spreadsheet;
using BaseGround.Report;
using DataObjects.Dao.Together.SpecificDao;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
/// <summary>
/// Lukas, 2018/12/13
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 50072 獎勵活動報價期貨商明細加總日報表
    /// 有寫到的功能：Export
    /// </summary>
    public partial class W50072 : FormParent {
        private D50072 dao50072;
        public W50072(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            dao50072 = new D50072();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtToDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtFromDate.DateTimeValue = (txtToDate.Text.Substring(0, 8) + "01").AsDateTime();
        }

        protected override ResultStatus Open() {
            base.Open();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield() {
            base.CheckShield();

            return ResultStatus.Success;
        }

        protected void ShowMsg(string msg) {
            lblProcessing.Text = msg;
            this.Refresh();
            Thread.Sleep(5);
        }

        protected override ResultStatus Export() {
            base.Export();
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);

            if(!ManipulateExcel(excelDestinationPath))return ResultStatus.Fail;
            lblProcessing.Visible = false;
            return ResultStatus.Success;
        }

        private bool ManipulateExcel(string excelDestinationPath) {

            //測試資料查詢日期:2017/12/01
            try {
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;

                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                string rptName, rptId;
                int i;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = 欄位位置
                ls_param_key = 契約
                *************************************/
                rptName = "STF報價期貨商明細加總日報表";
                rptId = "50072";
                ShowMsg(rptId + "－" + rptName + " 轉檔中...");

                #region Excel

                //讀取資料
                DataTable dtContent = dao50072.ListData(txtFromDate.FormatValue, txtToDate.FormatValue);
                if (dtContent.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromDate.Text, this.Text));
                }

                //填資料
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet worksheet = workbook.Worksheets[0];

                for (i = 0; i < dtContent.Rows.Count; i++) {

                    int rowNum = i + 2;

                    worksheet.Cells[rowNum, 0].Value = dtContent.Rows[i]["mc_date"].AsString();
                    worksheet.Cells[rowNum, 1].Value = dtContent.Rows[i]["fut_id"].AsString();
                    worksheet.Cells[rowNum, 2].Value = dtContent.Rows[i]["acctno"].AsString();
                    worksheet.Cells[rowNum, 3].Value = dtContent.Rows[i]["param_key"].AsString();
                    worksheet.Cells[rowNum, 4].Value = dtContent.Rows[i]["valid_cnt"].AsDecimal();
                    worksheet.Cells[rowNum, 5].Value = dtContent.Rows[i]["valid_time"].AsDecimal();
                    worksheet.Cells[rowNum, 6].Value = dtContent.Rows[i]["valid_result"].AsDecimal();
                    worksheet.Cells[rowNum, 7].Value = dtContent.Rows[i]["qty"].AsDecimal();
                    worksheet.Cells[rowNum, 8].Value = dtContent.Rows[i]["nqty"].AsDecimal();
                    worksheet.Cells[rowNum, 9].Value = dtContent.Rows[i]["prod_type"].AsString();
                    worksheet.Cells[rowNum, 10].Value = dtContent.Rows[i]["drank"].AsDecimal();

                }
                //workbook.SaveDocument(excelDestinationPath);

                #endregion

                #region CSV
                /******************
                ETF
                ******************/
                //讀取資料
                string asSymEtf = txtFromDate.Text.Replace("/", "").Substring(0, 6);
                string asEymEtf = txtToDate.Text.Replace("/", "").Substring(0, 6);
                DataTable dtContentETF = dao50072.ListData_etf(asSymEtf, asEymEtf);
                if (dtContent.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromDate.Text, this.Text));
                }
                //存CSV
               
                string etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,"50072_ETF.csv");
                File.Create(etfFileName).Close();
                StringBuilder sbETF = new StringBuilder();

                IEnumerable<string> etfColumnNames = dtContentETF.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sbETF.AppendLine(string.Join(",", etfColumnNames));

                foreach (DataRow row in dtContentETF.Rows) {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                      string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sbETF.AppendLine(string.Join(",", fields));
                }

                File.WriteAllText(etfFileName, sbETF.ToString());

                /******************
                TXF
                ******************/
                //讀取資料
                string asSymTxf = txtFromDate.Text.Replace("/", "").Substring(0, 6);
                string asEymTxf = txtToDate.Text.Replace("/", "").Substring(0, 6);
                DataTable dtContentTXF = dao50072.ListData_txf(asSymTxf, asEymTxf);
                if (dtContentTXF.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromDate.Text, this.Text));
                }
                //存CSV
                string txfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, "50072_TXF.csv");
                File.Create(txfFileName).Close();

                StringBuilder sbTXF = new StringBuilder();

                IEnumerable<string> txfColumnNames = dtContentTXF.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sbTXF.AppendLine(string.Join(",", txfColumnNames));

                foreach (DataRow row in dtContentTXF.Rows) {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                      string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sbTXF.AppendLine(string.Join(",", fields));
                }

                File.WriteAllText(txfFileName, sbTXF.ToString());

                #endregion

                #region MTX 已廢除
                //讀取資料
                //DataTable dtContentMTX = dao50072.ListData_mtx(txtFromDate.FormatValue, txtToDate.FormatValue);
                //if (dtContent.Rows.Count == 0) {
                //    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromDate.Text, this.Text));
                //}

                //填資料
                //Workbook workbookMTX = new Workbook();
                //workbook.LoadDocument(excelDestinationPath);
                //Worksheet worksheetMTX = workbook.Worksheets[1];

                //for (i = 0; i < dtContentMTX.Rows.Count; i++) {

                //    int ii_ole_row = i + 3;

                //        worksheetMTX.Cells[ii_ole_row, 0].Value = dtContentMTX.Rows[i]["yymmdd"].AsString();
                //        worksheetMTX.Cells[ii_ole_row, 1].Value = dtContentMTX.Rows[i]["fcm_no"].AsString();
                //        worksheetMTX.Cells[ii_ole_row, 2].Value = dtContentMTX.Rows[i]["acc_no"].AsString();
                //        worksheetMTX.Cells[ii_ole_row, 3].Value = dtContentMTX.Rows[i]["auction_rate"].AsString();
                //        worksheetMTX.Cells[ii_ole_row, 4].Value = dtContentMTX.Rows[i]["buy_keep_time"].AsString();
                //        worksheetMTX.Cells[ii_ole_row, 5].Value = dtContentMTX.Rows[i]["sell_keep_time"].AsString();
                //        worksheetMTX.Cells[ii_ole_row, 6].Value = dtContentMTX.Rows[i]["match_rate"].AsString();

                //}

                #endregion

                //Excel存檔
                workbook.SaveDocument(excelDestinationPath);
                ShowMsg("轉檔成功");
                return true;
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            finally {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
            }
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            base.Print(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            base.InsertRow();

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow();

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose() {
            return base.BeforeClose();
        }
    }
}