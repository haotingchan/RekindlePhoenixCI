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
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Controls;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System.Threading;

/// <summary>
/// Lukas, 2019/3/27
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30100 Kill Switch使用紀錄查詢
    /// </summary>
    public partial class W30100 : FormParent {

        private ABRK daoABRK;
        private D30100 dao30100;

        public W30100(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;
                txtSDate.EditValue = txtEDate.Text.SubStr(0, 8) + "01";
#if DEBUG
                txtSDate.Text = "2017/11/17";
                txtEDate.Text = "2017/11/17";
#endif
                txtSDate.Focus();

                //ＫＳ期貨商下拉選單
                //使用期貨商下拉選單
                daoABRK = new ABRK();
                DataTable dtABRK = daoABRK.ListAll2();
                Extension.SetDataTable(dwFcmKs, dtABRK, "ABRK_NO", "CP_DISPLAY", TextEditStyles.DisableTextEditor, "");
                Extension.SetDataTable(dwFcmIn, dtABRK, "ABRK_NO", "CP_DISPLAY", TextEditStyles.DisableTextEditor, "");
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
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                dao30100 = new D30100();

                string rptId, file, rptName;
                DateTime sYmd = txtSDate.DateTimeValue;
                DateTime eYmd = txtEDate.DateTimeValue;
                int rowNum;
                rptId = "30100";
                rptName = "Kill Switch使用紀錄查詢";
                ShowMsg(rptId + "－" + rptName + " 轉檔中...");

                //讀取資料
                DataTable dt30100 = dao30100.d_30100(sYmd, eYmd,
                                                     dwFcmKs.EditValue.AsString() + "%", dwFcmIn.EditValue.AsString() + "%",
                                                     rdgMarketCode.EditValue.AsString());
                if (dt30100.Rows.Count == 0) {
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
                Worksheet ws30100 = workbook.Worksheets[0];
                //身份碼總列數隱藏於A2
                rowNum = 1;
                ws30100.Import(dt30100, false, rowNum, 0);

                //存檔
                ws30100.ScrollToRow(0);
                workbook.SaveDocument(file);
                lblProcessing.Text = "轉檔成功";
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