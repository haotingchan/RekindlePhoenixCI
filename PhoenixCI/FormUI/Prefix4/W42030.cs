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
using Common;
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;
using DevExpress.Spreadsheet;

/// <summary>
/// Lukas, 2019/3/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

    /// <summary>
    /// 42030 上市證券保證金所屬級距及適用比例估計
    /// </summary>
    public partial class W42030 : FormParent {

        private D42030 dao42030;

        public W42030(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtSDate.DateTimeValue = DateTime.Now;
            txtSDate.Focus();
#if DEBUG
            txtSDate.Text = "2018/12/28";
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

        protected override ResultStatus Export() {

            try {
                lblProcessing.Visible = true;
                dao42030 = new D42030();
                #region ue_export_before
                //1. 判斷資料已轉入
                int rtn;
                string ymd = txtSDate.Text.Replace("/", "");
                rtn = dao42030.mgr5Count(ymd);
                if (rtn == 0) {
                    DialogResult result = MessageDisplay.Choose(" 當日上市證券保證金適用比例資料未轉入完畢,是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }
                //2. 130批次作業做完
                string rtnStr;
                rtnStr = PbFunc.f_chk_130_wf(_ProgramID, txtSDate.DateTimeValue, "1");
                if (rtnStr != "") {
                    DialogResult result = MessageDisplay.Choose(txtSDate.Text + "-" + rtnStr + "，是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }
                #endregion

                string rptName, rptId, file;
                int rowStart;
                rptName = "上市證券保證金概況表";
                rptId = "42030";
                lblProcessing.Text = rptId + '－' + rptName + " 轉檔中...";

                //1. 複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") {
                    return ResultStatus.Fail;
                }

                //2. 讀取資料(保證金適用比例級距)
                DataTable dt42030 = dao42030.d_42030(ymd);
                if (dt42030.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",讀取「當日上市證券保證金適用比例」無任何資料!");
                    return ResultStatus.Fail;
                }

                //3. 開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //4. 切換Sheet
                Worksheet ws42030 = workbook.Worksheets[0];
                ws42030.Cells[0, 0].Value = txtSDate.DateTimeValue.ToString("yyyy年MM月dd日") + ws42030.Cells[0, 0].Value;
                rowStart = 4;

                //5. 從A5開始放資料
                ws42030.Import(dt42030, false, rowStart, 0);

                //6. 刪除空白列
                int rowIndex = dt42030.Rows.Count;
                if (2000 > rowIndex) {
                    ws42030.Rows.Remove(rowIndex + rowStart, 2000 - rowIndex);
                }

                //7. 存檔
                ws42030.ScrollToRow(0);
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