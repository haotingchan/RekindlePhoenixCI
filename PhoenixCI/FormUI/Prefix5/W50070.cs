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
using System.Threading;
using BaseGround.Shared;
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

            if (!ManipulateExcel()) return ResultStatus.Fail;
            lblProcessing.Visible = false;
            return ResultStatus.Success;
        }

        private bool ManipulateExcel() {

            try {
                //檢查查詢日期格式是否正確
                if (txtMonth.Text.SubStr(5, 2) == "00") {
                    MessageDisplay.Error("月份輸入錯誤!");
                    return false;
                }
                txtMonth.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");

                string rptName, rptId, file;
                int f;
                rptName = "STF報價每月獎勵活動成績得獎名單月報表";
                rptId = "50070";
                ShowMsg(rptId + "－" + rptName + " 轉檔中...");

                //讀取資料
                daoRMM = new R_MARKET_MONTHLY();
                string asYM = txtMonth.Text.Replace("/", "");
                DataTable dt50070 = daoRMM.ListAllByDate(asYM);
                if (dt50070.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", asYM, rptName));
                    return false;
                }

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return false;

                //切換Sheet
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);
                Worksheet ws50070 = workbook.Worksheets[0];

                //填資料 Sheet1
                ws50070.Import(dt50070, false, 2, 0);
                //int rowNum = 5;
                //for (f = 0; f < dt50070.Rows.Count; f++) {
                //    DataRow dr50070 = dt50070.Rows[f];
                //    rowNum = f + 2;
                //    ws50070.Cells[rowNum, 0].Value = dr50070["MC_MONTH"].AsString();
                //    ws50070.Cells[rowNum, 1].Value = dr50070["FUT_ID"].AsString();
                //    ws50070.Cells[rowNum, 2].Value = dr50070["FUT_NAME"].AsString();
                //    ws50070.Cells[rowNum, 3].Value = dr50070["REWARD_TYPE"].AsDecimal();
                //    ws50070.Cells[rowNum, 4].Value = dr50070["REWARD"].AsDecimal();
                //    ws50070.Cells[rowNum, 5].Value = dr50070["DETAIL"].AsString();
                //    ws50070.Cells[rowNum, 6].Value = dr50070["ACCTNO"].AsString();
                //    ws50070.Cells[rowNum, 7].Value = dr50070["PROD_TYPE"].AsString();
                //}

                //讀取資料
                dt50070 = daoRMM.ListAll2ByDate(asYM);
                if (dt50070.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", asYM, rptName));
                    return false;
                }

                //切換Sheet
                ws50070 = workbook.Worksheets[1];

                //填資料 Sheet2
                ws50070.Import(dt50070,false, 0, 0);

                //存檔
                workbook.SaveDocument(file);
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
                txtMonth.Enabled = true;
            }
        }
    }
}