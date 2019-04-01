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

/// <summary>
/// Lukas, 2019/4/1
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30080 各檔股票期貨、選擇權交易量、未平倉統計
    /// </summary>
    public partial class W30080 : FormParent {
        public W30080(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                txtEDate.EditValue = PbFunc.f_ocf_date(0);
                txtSDate.EditValue = txtEDate.Text.SubStr(0, 8) + "01";
#if DEBUG
                //txtSDate.Text = "2014/01/01";
                //txtEDate.Text = "2017/12/31";
#endif

                txtSDate.Focus();
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

            try {
                lblProcessing.Visible = true;
                //dao30090 = new D30090();
                string rptId, file, rptName, kindId, kindIdName,symd,eymd;
                int rowNum, colNum;
                rptId = "30080";
                rptName = "各檔股票期貨、選擇權交易量、未平倉統計";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                //輸入條件
                symd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                eymd = txtEDate.DateTimeValue.ToString("yyyyMMdd");
                file = "(" + "日期-" + symd + "-" + eymd;

                //市場別
                
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }
    }
}