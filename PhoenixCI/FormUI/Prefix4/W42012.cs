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
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.TableDao;
using Common;
using System.Threading;
using BaseGround.Shared;
using DevExpress.Spreadsheet;

/// <summary>
/// Lukas, 2019/4/15
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

    /// <summary>
    /// 42012 股票期貨風險價格係數分析表
    /// </summary>
    public partial class W42012 : FormParent {

        private D42011 dao42011;//有幾段共用的sql
        private D42012 dao42012;

        public W42012(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao42012 = new D42012();
            dao42011 = new D42011();
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtEDate.DateTimeValue = DateTime.Now;
            txtEDate.Focus();
            txtSDate.DateTimeValue = txtEDate.DateTimeValue;
            txtRate1Ref.Visible = false;
            txtRate2Ref.Visible = false;
            txtRate3Ref.Visible = false;
            txtRate4Ref.Visible = false;
            lblCmRateRef.Visible = false;

#if DEBUG
            //txtSDate.EditValue = "2018/12/28";
#endif

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            try {
                if (!FlagAdmin) {
                    cbxRate.Visible = false;
                }
                decimal ld_cm_rate1, ld_cm_rate2, ld_cm_rate3;
                //最高%
                ld_cm_rate1 = dao42011.GetCmRate();
                lblCmRateRef.Text = (ld_cm_rate1 * 100).ToString("#.####");
                lblCmRate.Text = (ld_cm_rate1 * 100).ToString("#.####");

                //級距1
                DataTable dtRate = dao42011.Get3CmRate();
                ld_cm_rate1 = dtRate.Rows[0]["LD_CM_RATE1"].AsDecimal();
                ld_cm_rate1 = ld_cm_rate1 - 0.01m;
                txtRate1Ref.Text = (ld_cm_rate1 * 100).ToString("#.####");
                txtRate1.Text = (ld_cm_rate1 * 100).ToString("#.####");
                //級距2
                ld_cm_rate2 = dtRate.Rows[0]["LD_CM_RATE2"].AsDecimal();
                ld_cm_rate2 = ld_cm_rate2 - 0.01m;
                txtRate2Ref.Text = (ld_cm_rate2 * 100).ToString("#.####");
                txtRate2.Text = (ld_cm_rate2 * 100).ToString("#.####");
                //級距3
                ld_cm_rate3 = dtRate.Rows[0]["LD_CM_RATE3"].AsDecimal();
                ld_cm_rate3 = ld_cm_rate3 - 0.01m;
                txtRate3Ref.Text = (ld_cm_rate3 * 100).ToString("#.####");
                txtRate3.Text = (ld_cm_rate3 * 100).ToString("#.####");

            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = false;//列印

            return ResultStatus.Success;
        }

        protected void ShowMsg(string msg) {
            lblProcessing.Text = msg;
            this.Refresh();
            Thread.Sleep(5);
        }

        protected override ResultStatus Export() {
            try {
                if (txtSID.Text.AsString() == "") {
                    MessageDisplay.Error("請輸入標的證券代號");
                    return ResultStatus.Fail;
                }
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                string rptId = "42012", file, rptName = "股票期貨風險價格係數分析表";

                //讀取資料(保證金適用比例級距)
                DataTable dt42012 = dao42012.d_42012_detl(txtSDate.DateTimeValue.ToString("yyyyMMdd"), txtEDate.DateTimeValue.ToString("yyyyMMdd"),txtSID.Text.AsString(), 
                                          txtRange.Text.AsDecimal() / 100, txtRate2Ref.Text.AsDecimal() / 100, txtRate3Ref.Text.AsDecimal() / 100, txtRate4Ref.Text.AsDecimal() / 100,
                                          txtRate1.Text.AsDecimal() / 100, txtRate2.Text.AsDecimal() / 100, txtRate3.Text.AsDecimal() / 100, txtRate4.Text.AsDecimal() / 100);
                if (dt42012.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "～" + txtEDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //切換Sheet
                Worksheet ws = workbook.Worksheets[0];
                ws.Cells[0, 15].Value = txtSDate.Text + "～" + txtEDate.Text;

                //存檔
                ws.ScrollToRow(0);
                workbook.SaveDocument(file);
                ShowMsg("轉檔成功");
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            finally {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
            }
            return ResultStatus.Success;
        }

    }
}