using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using Common;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Spreadsheet;
/// <summary>
/// Lukas, 2019/1/3
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 50070 STF報價每月獎勵活動成績得獎名單月報表
    /// 有寫到的功能：Export
    /// </summary>
    public partial class W50070 : FormParent {

        private R_MARKET_MONTHLY daoRMM;

        public W50070(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            //查詢條件賦值
            txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;
        }

        public override ResultStatus BeforeOpen() {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open() {
            base.Open();

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {
            base.Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield() {
            base.CheckShield();

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            base.Save(pokeBall);

            return ResultStatus.Success;
        }

        protected override ResultStatus Run(PokeBall args) {
            base.Run(args);

            return ResultStatus.Success;
        }

        protected override ResultStatus Import() {
            base.Import();

            return ResultStatus.Success;
        }

        protected override ResultStatus Export() {
            base.Export();
            lblProcessing.Visible = true;
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);

            ManipulateExcel(excelDestinationPath);
            lblProcessing.Visible = false;
            return ResultStatus.Success;
        }

        private void ManipulateExcel(string excelDestinationPath) {

            try {
                string ls_rpt_name, ls_rpt_id;
                int i, li_ole_col;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = 欄位位置
                ls_param_key = 契約
                *************************************/
                ls_rpt_name = "STF報價每月獎勵活動成績得獎名單月報表";
                ls_rpt_id = "50070";
                lblProcessing.Text = ls_rpt_id + "－" + ls_rpt_name + " 轉檔中...";

                //讀取資料
                daoRMM = new R_MARKET_MONTHLY();
                string as_ym = txtMonth.Text.Replace("/", "");
                DataTable dt50070 = daoRMM.ListAllByDate(as_ym);
                if (dt50070.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", as_ym, ls_rpt_name));
                }

                //切換Sheet
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet ws50070 = workbook.Worksheets[0];

                //填資料
                int ii_ole_row = 5;
                for (i = 0; i < dt50070.Rows.Count; i++) {
                    DataRow dr50070 = dt50070.Rows[i];
                    ii_ole_row = i + 2;
                    ws50070.Cells[ii_ole_row, 0].Value = dr50070["mc_month"].AsString();
                    ws50070.Cells[ii_ole_row, 1].Value = dr50070["fut_id"].AsString();
                    ws50070.Cells[ii_ole_row, 2].Value = dr50070["fut_name"].AsString();
                    ws50070.Cells[ii_ole_row, 3].Value = dr50070["reward_type"].AsDecimal();
                    ws50070.Cells[ii_ole_row, 4].Value = dr50070["reward"].AsDecimal();
                    ws50070.Cells[ii_ole_row, 5].Value = dr50070["detail"].AsString();
                    ws50070.Cells[ii_ole_row, 6].Value = dr50070["acctno"].AsString();
                    ws50070.Cells[ii_ole_row, 7].Value = dr50070["prod_type"].AsString();
                }

                //存檔
                workbook.SaveDocument(excelDestinationPath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
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