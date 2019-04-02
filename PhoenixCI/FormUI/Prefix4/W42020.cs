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
using Common;
using BaseGround.Shared;
using DevExpress.Spreadsheet;
using System.Threading;

/// <summary>
/// Lukas, 2019/3/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

    /// <summary>
    /// 42020 股票選擇權保證金概況表
    /// </summary>
    public partial class W42020 : FormParent {

        private D42020 dao42020;

        public W42020(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtSDate.DateTimeValue = DateTime.Now;
            txtSDate.Focus();
#if DEBUG
            txtSDate.Text = "2018/12/18";
#endif

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
                lblProcessing.Text = "轉檔中...";
                lblProcessing.Visible = true;
                dao42020 = new D42020();
                #region ue_export_before
                //1. 判斷資料已轉入
                int rtn;
                string ymd = txtSDate.Text.Replace("/", "");
                rtn = dao42020.mgr3Count(ymd);
                if (rtn == 0) {
                    DialogResult result = MessageDisplay.Choose(" 當日保證金適用比例資料未轉入完畢,是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }
                //2. 130批次作業做完
                string rtnStr;
                rtnStr = PbFunc.f_chk_130_wf(_ProgramID,txtSDate.DateTimeValue,"1");
                if (rtnStr!="") {
                    DialogResult result = MessageDisplay.Choose(txtSDate.Text + "-" + rtnStr + "，是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }
                #endregion

                string rptName, rptId, prodType, status, file;
                int f, rowStart, chgCnt, spaceRow;
                rptName = "股票選擇權保證金概況表";
                rptId = "42020";
                lblProcessing.Text = rptId + '－' + rptName + " 轉檔中...";
                prodType = "O";

                //1. 讀取資料(保證金適用比例級距)
                DataTable dt42020Mgrt1 = dao42020.d_42020_mgrt1("O");
                if (dt42020Mgrt1.Rows.Count == 0) {
                    ShowMsg("轉檔失敗");
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",讀取「保證金適用比例級距」無任何資料!");
                    return ResultStatus.Fail;
                }

                //2. 讀取資料(當日保證金適用比例)
                DataTable dt42020 = dao42020.d_42020(ymd, prodType);
                if (dt42020.Rows.Count == 0) {
                    ShowMsg("轉檔失敗");
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",讀取「當日保證金適用比例」無任何資料!");
                    return ResultStatus.Fail;
                }

                //3. 複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") {
                    return ResultStatus.Fail;
                }

                //4. 開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //5. 切換Sheet
                Worksheet ws42020 = workbook.Worksheets[0];
                ws42020.Cells[1, 7].Value = "資料日期：" + txtSDate.DateTimeValue.ToString("yyyy年MM月dd日");
                rowStart = 7;

                //6. 從B8 開始放資料
                ws42020.Import(dt42020Mgrt1, false, rowStart, 1);

                rowStart = 21;
                spaceRow = 200;
                chgCnt = 0;

                //7. 從B22 開始放資料
                ws42020.Import(dt42020, false, rowStart, 1);

                //8. 2015.06.01 需10400043
                DataTable dt42020Compute = dao42020.d_42020Compute(ymd, prodType);
                if (dt42020Compute.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",讀取「當日保證金適用比例」無任何資料!");
                    return ResultStatus.Fail;
                }
                for (f = 0; f < dt42020Compute.Rows.Count; f++) {
                    status = "";
                    DataRow dr = dt42020Compute.Rows[f];
                    if (dr["MGR3_CM"].AsDecimal() != dr["MGR3_CUR_CM"].AsDecimal()) {
                        chgCnt = chgCnt + 1;
                        status = "由";
                        if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                            status = status + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%";
                        }
                        else {
                            status = status + dr["MGRT1_LAST_LEVEL_NAME"].AsString();
                        }
                        status = status + "調整為";
                        if (dr["MGR3_LEVEL"].AsString() == "Z") {
                            status = status + (dr["MGR3_CM"].AsDecimal() * 100).AsString() + "%";
                        }
                        else {
                            status = status + dr["MGRT1_LEVEL_NAME"].AsString();
                        }
                    }
                    ws42020.Cells[f + rowStart, 9].Value = status;
                    switch (dr["MGR3_STATUS"].AsString()) {
                        case "P":
                            status = "暫停交易";
                            break;
                        default:
                            status = "";
                            break;
                    }
                    ws42020.Cells[f + rowStart, 10].Value = status;
                }
                if (chgCnt > 0) {
                    ws42020.Cells[rowStart + spaceRow + 1, 1].Value = "本日級距變動檔數：" + chgCnt.ToString("##0");
                }

                //9. 刪除空白列
                int rowIndex = dt42020.Rows.Count;
                if (spaceRow > rowIndex) {
                    ws42020.Rows.Remove(rowIndex + rowStart, spaceRow - rowIndex);
                }

                //10. 存檔
                ws42020.ScrollToRow(0);
                workbook.SaveDocument(file);
            }
            catch (Exception ex) {
                //WriteLog(ex, "", false); 如果不用throw會繼續往下執行(?
                lblProcessing.Text = "轉檔失敗";
                throw ex;
            }
            lblProcessing.Text = "轉檔成功";

            return ResultStatus.Success;
        }
    }
}