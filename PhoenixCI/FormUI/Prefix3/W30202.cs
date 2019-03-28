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
using Common;
using DataObjects.Dao.Together.SpecificDao;

/// <summary>
/// Lukas, 2019/3/28
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30202 股價指數暨黃金類商品部位限制數計算
    /// </summary>
    public partial class W30202 : FormParent {

        private D30202 dao30202;

        public W30202(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                DateTime date = DateTime.Now;
                //本次
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtCurEymd.DateTimeValue = date;
                txtDate.DateTimeValue = date;
                txtCurEMonth.DateTimeValue = date;
                date = PbFunc.relativedate(date, (date.Day * -1));
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtCurSMonth.DateTimeValue = date;
                //前次
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtPrevEymd.DateTimeValue = date;
                txtEMonth.DateTimeValue = date;
                date = PbFunc.relativedate(date, (date.Day * -1));
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtSMonth.DateTimeValue = date;
#if DEBUG
                //txtDate.Text = "2018/03/31";
                //txtSMonth.Text = "2018/03";
                //txtEMonth.Text = "2018/03";
                //txtStkoutYmd.Text = "2018/03/31";
#endif
                txtDate.Focus();
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

        protected override ResultStatus Export() {
            string showMsg = "";
            try {
                lblProcessing.Text = "開始轉檔...";
                lblProcessing.Visible = true;
                dao30202 = new D30202();
                //判斷是否有檔案,決定是否要寫入DB.
                showMsg = "讀取既有計算資料錯誤";
                string cpYmd = txtDate.DateTimeValue.ToString("yyyyMMdd");
                DataTable dtPL1 = dao30202.d_30202_pl1(cpYmd);
                if (dtPL1.Rows.Count > 0) {
                    DialogResult result = MessageDisplay.Choose("已有計算資料,是否要更新資料庫資料?");
                    if (result == DialogResult.No) {
                        cbxDB.Checked = false;
                    }
                }
                txtPrevEymd.DateTimeValue = PbFunc.f_get_end_day("DATE","",txtEMonth.Text);
                txtCurEymd.DateTimeValue = PbFunc.f_get_end_day("DATE", "", txtCurEMonth.Text);

            }
            catch (Exception ex) {
                MessageDisplay.Error(showMsg);
                throw ex;
            }
            return ResultStatus.Success;
        }
        }
}