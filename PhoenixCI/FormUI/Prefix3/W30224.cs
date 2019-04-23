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
using DevExpress.Spreadsheet;
using Common;
using System.Threading;

/// <summary>
/// Lukas, 2019/3/12
/// 測試資料: 2012/01/01~2018/12/31
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30224 個股類歷次部位限制查詢
    /// </summary>
    public partial class W30224 : FormParent {

        string logTxt;
        private D30224 dao30224;

        public W30224(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtEDate.EditValue = PbFunc.f_ocf_date(0);
            txtSDate.EditValue = txtEDate.Text.SubStr(0, 8) + "01";
            txtSDate.Focus();
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
                dao30224 = new D30224();
                string rptId, file, rptName;
                int rowNum;
                rptName = "個股類歷次部位限制查詢";
                rptId = "30224";

                //讀取資料
                DataTable dt30224 = dao30224.d_30224(txtSDate.Text.Replace("/", ""), txtEDate.Text.Replace("/", ""));
                if (dt30224.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "～" + txtEDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    return ResultStatus.Fail;
                }

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;
                logTxt = file;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                #region wf_30224
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                //切換sheet
                Worksheet ws30224 = workbook.Worksheets[0];
                ws30224.Cells[1, 0].Value = ws30224.Cells[1, 0].Value.ToString() + txtSDate.Text + "～" + txtEDate.Text; //填寫公告日期

                //填入資料
                rowNum = 2;
                foreach (DataRow dr in dt30224.Rows) {
                    rowNum = rowNum + 1;
                    if (dr["PLS2_YMD"] != DBNull.Value) ws30224.Cells[rowNum, 0].Value = dr["PLS2_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                    if (dr["PLS2_EFFECTIVE_YMD"] != DBNull.Value) ws30224.Cells[rowNum, 1].Value = dr["PLS2_EFFECTIVE_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                    if (dr["PLS2_RAISE_CNT"] != DBNull.Value) ws30224.Cells[rowNum, 2].Value = dr["PLS2_RAISE_CNT"].AsInt();
                    if (dr["PLS2_LOWER_CNT"] != DBNull.Value) ws30224.Cells[rowNum, 3].Value = dr["PLS2_LOWER_CNT"].AsInt();
                    if (dr["PLS2_NEW_CNT"] != DBNull.Value) ws30224.Cells[rowNum, 4].Value = dr["PLS2_NEW_CNT"].AsInt();
                    if (dr["PLS2_FUT_CNT"] != DBNull.Value) ws30224.Cells[rowNum, 5].Value = dr["PLS2_FUT_CNT"].AsInt();
                    if (dr["PLS2_OPT_CNT"] != DBNull.Value) ws30224.Cells[rowNum, 6].Value = dr["PLS2_OPT_CNT"].AsInt();
                }
                #endregion

                //存檔
                ws30224.ScrollToRow(0);
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