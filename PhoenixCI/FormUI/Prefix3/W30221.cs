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
using DevExpress.Spreadsheet;
using BusinessObjects;
using System.IO;

/// <summary>
/// Lukas, 2019/3/28
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30221 個股部位限制計算
    /// </summary>
    public partial class W30221 : FormParent {

        private D30221 dao30221;

        public W30221(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                DateTime date = DateTime.Now;
                txtDate.DateTimeValue = date;
                txtStkoutYmd.DateTimeValue = date;
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtPrevEymd.DateTimeValue = date;
                txtEMonth.DateTimeValue = date;
                date = PbFunc.relativedate(date, (date.Day * -1));
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtSMonth.DateTimeValue = date;
                cbxDB.Checked = true;
#if DEBUG
                txtDate.Text = "2018/03/31";
                txtSMonth.Text = "2018/03";
                txtEMonth.Text = "2018/03";
                txtStkoutYmd.Text = "2018/03/31";
#endif

                txtDate.Focus();
            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            try {
                base.AfterOpen();
                if (FlagAdmin) {
                    cbxWriteTxt.Visible = true;
                }
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
                string showMsg = "";
                lblProcessing.Text = "開始轉檔...";
                lblProcessing.Visible = true;
                dao30221 = new D30221();
                //判斷是否有檔案,決定是否要寫入DB.
                showMsg = "判斷是否有已有計算資料";
                string cpYmd = txtDate.DateTimeValue.ToString("yyyyMMdd");
                DataTable dtPLS1 = dao30221.d_30221_pls1(cpYmd);
                if (dtPLS1.Rows.Count > 0) {
                    DialogResult result = MessageDisplay.Choose("已有計算資料,是否要更新資料庫資料?");
                    if (result == DialogResult.No) {
                        cbxDB.Checked = false;
                    }
                }
                string rptId, rptName = "個股部位限制計算表", file,
                       stkYmd = txtStkoutYmd.DateTimeValue.ToString("yyyyMMdd"),
                       sMonth = txtSMonth.Text.Replace("/", ""),
                       eMonth = txtEMonth.Text.Replace("/", "");
                rptId = "30221";

                //讀取資料
                showMsg = "讀取資料";
                DataTable dt30221 = dao30221.d_30221(cpYmd, sMonth, eMonth, stkYmd);
                if (dt30221.Rows.Count == 0) {
                    MessageDisplay.Info(eMonth + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                //複製檔案
                showMsg = "複製檔案";
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                showMsg = "開啟檔案";
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //切換Sheet
                showMsg = "切換Sheet";
                Worksheet ws30221 = workbook.Worksheets[0];

                //寫入資料
                showMsg = "寫入資料";
                int rowNum = 2, rowTol;
                #region wf_30221

                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";
                rowTol = 500;

                if (cbxDB.Checked) ws30221.Cells[0, 0].Value = ws30221.Cells[0, 0].Value.AsString() + "(試算)";
                //(一)
                ws30221.Cells[2, 4].Value = txtSMonth.Text + "～" + txtEMonth.Text;
                //(二)
                ws30221.Cells[2, 5].Value = txtStkoutYmd.Text;

                rowNum = 4;
                //從A5開始填資料
                ws30221.Import(dt30221, false, rowNum, 0);

                //存檔
                ws30221.ScrollToRow(0);
                workbook.SaveDocument(file);
                lblProcessing.Text = "轉檔成功";
                #endregion

                if (!cbxDB.Checked) {
                    return ResultStatus.Success;
                }
                #region wf_30221_write
                bool dbCommit = false;
                //刪除PL0的資料
                showMsg = "PL0刪除失敗";
                dbCommit = dao30221.DeletePL0ByDate(cpYmd);
                if (!dbCommit) {
                    MessageDisplay.Error(showMsg);
                    return ResultStatus.Fail;
                }
                //新增PL0的資料
                string etfFileName;
                showMsg = "PL0新增失敗";
                dbCommit = dao30221.InsertPL0(cpYmd, sMonth, eMonth, stkYmd, GlobalInfo.USER_ID);
                if (!dbCommit) {
                    MessageDisplay.Error(showMsg);
                    return ResultStatus.Fail;
                }
                //刪除PLS2的資料
                showMsg = "PLS2刪除失敗";
                dbCommit = dao30221.DeletePLS2ByDate(cpYmd);
                if (!dbCommit) {
                    MessageDisplay.Error(showMsg);
                    return ResultStatus.Fail;
                }
                //刪除PLS1資料 dtPLS1已在前面讀取過
                if (dtPLS1.Rows.Count > 0) {
                    while (dtPLS1.Rows.Count > 0) {
                        dtPLS1.Rows.RemoveAt(dtPLS1.Rows.Count - 1);
                    }
                    showMsg = "PLS1刪除失敗";
                    dbCommit = dao30221.DeletePLS1ByDate(cpYmd);
                }
                if (!dbCommit) {
                    MessageDisplay.Error("刪除資料庫中舊資料錯誤! ");
                    return ResultStatus.Fail;
                }
                if (cbxWriteTxt.Checked) {
                    //把刪除結果存成txt
                    etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, "30221_AfterDel.txt");
                    ExportOptions txtref = new ExportOptions();
                    txtref.HasHeader = false;
                    txtref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
                    Common.Helper.ExportHelper.ToText(dtPLS1, etfFileName, txtref);
                }

                dtPLS1.Clear();
                dtPLS1.AcceptChanges();
                //寫入PLS1資料
                //dt30221 資料重撈因為欄位比報表多
                dt30221 = dao30221.d_30221_all(cpYmd, sMonth, eMonth, stkYmd);
                for (int f = 0; f < dt30221.Rows.Count; f++) {
                    dtPLS1.Rows.Add();
                    for (int g = 0; g < 16; g++) {
                        dtPLS1.Rows[dtPLS1.Rows.Count - 1][g] = dt30221.Rows[f][g];
                    }
                    dtPLS1.Rows[dtPLS1.Rows.Count - 1][16] = DateTime.Now;
                    dtPLS1.Rows[dtPLS1.Rows.Count - 1][17] = GlobalInfo.USER_ID;
                }
                if (cbxWriteTxt.Checked) {
                    //把寫入結果存成txt
                    etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, "30221.txt");
                    ExportOptions txtref = new ExportOptions();
                    txtref.HasHeader = false;
                    txtref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
                    Common.Helper.ExportHelper.ToText(dtPLS1, etfFileName, txtref);
                }
                try {
                    ResultData myResultData = dao30221.updatePLS1(dtPLS1);
                }
                catch (Exception ex) {
                    MessageDisplay.Error("計算結果新增至資料庫錯誤! ");
                    WriteLog(ex);
                    return ResultStatus.Fail;
                }
                #endregion
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }
    }
}