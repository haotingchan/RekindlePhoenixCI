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
using BusinessObjects.Enums;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;
using Common;
using System.Threading;
using DataObjects.Dao.Together.TableDao;

/// <summary>
/// Lukas, 2019/4/11
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 42011 股票期貨風險價格係數檢核表
    /// </summary>
    public partial class W42011 : FormParent {

        private D42011 dao42011;
        private MGR3 daoMGR3;

        public W42011(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao42011 = new D42011();
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.DateTimeValue = DateTime.Now;
            txtSDate.Focus();
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
                lblCmRate.Text = (ld_cm_rate1 * 100).ToString("#.####");

                //級距1
                DataTable dtRate = dao42011.Get3CmRate();
                ld_cm_rate1 = dtRate.Rows[0]["LD_CM_RATE1"].AsDecimal();
                ld_cm_rate1 = ld_cm_rate1 - 0.01m;
                txtRate1.Text = (ld_cm_rate1 * 100).ToString("#.####");
                //級距2
                ld_cm_rate2 = dtRate.Rows[0]["LD_CM_RATE2"].AsDecimal();
                ld_cm_rate2 = ld_cm_rate2 - 0.01m;
                txtRate2.Text = (ld_cm_rate2 * 100).ToString("#.####");
                //級距3
                ld_cm_rate3 = dtRate.Rows[0]["LD_CM_RATE3"].AsDecimal();
                ld_cm_rate3 = ld_cm_rate3 - 0.01m;
                txtRate3.Text = (ld_cm_rate3 * 100).ToString("#.####");
                //表一達標準判斷
                ld_cm_rate1 = dao42011.GetRange();
                txtRange.Text = (ld_cm_rate1 * 100).ToString("#.####");

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
                #region ue_export_before
                //判斷資料已轉入
                daoMGR3 = new MGR3();
                int rtn;
                string ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                rtn = daoMGR3.mgr3Count(ymd);
                if (rtn == 0) {
                    DialogResult result = MessageDisplay.Choose(" 當日保證金適用比例資料未轉入完畢,是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }
                //130批次作業做完
                string rtnStr;
                rtnStr = PbFunc.f_chk_130_wf(_ProgramID, txtSDate.DateTimeValue, "5");
                if (rtnStr != "") {
                    DialogResult result = MessageDisplay.Choose(txtSDate.Text + "-" + rtnStr + "，是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }

                //是否勾選任一報表
                if (!cbx1.Checked && !cbx2.Checked && !cbx3.Checked) {
                    MessageDisplay.Error("未勾選任何報表!");
                    return ResultStatus.Fail;
                }
                #endregion

                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");

                //讀取資料(保證金適用比例級距)
                //DataTable dt42011 = dao42011.d_30223(sDate, eDate);
                //if (dt30223.Rows.Count == 0) {
                //    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                //    lblProcessing.Visible = false;
                //    return ResultStatus.Fail;
                //}

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

        protected void wf_42011() {

        }

        protected void wf_42011_1() {

        }

        protected int wf_42011_2(int li_minus) {

            return li_minus;
        }

        protected int wf_42011_3(int li_minus) {

            return li_minus;
        }
    }
}