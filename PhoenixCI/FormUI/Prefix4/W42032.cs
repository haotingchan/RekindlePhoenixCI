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
using System.IO;

/// <summary>
/// Lukas, 2019/3/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 42032 上市證券原始資料查詢
    /// </summary>
    public partial class W42032 : FormParent {

        private D42032 dao42032;

        public W42032(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            try {
                base.Open();
                txtSDate.DateTimeValue = DateTime.Now;
#if DEBUG
                txtSDate.Text = "2018/10/31";
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
                lblProcessing.Text = "開始轉檔...";
                lblProcessing.Visible = true;
                dao42032 = new D42032();
                #region ue_export_before
                if (rdgCondition.EditValue.AsString() == "SID" && txtSID.Text.Trim() == "") {
                    MessageDisplay.Error("請輸入單一證券代號!");
                    return ResultStatus.Fail;
                }

                string rptId, rptName, fmYmd, toYmd, stockId, file;
                fmYmd = txtSDate.Text.Replace("/", "");
                rptId = "42032";
                //讀取資料
                DataTable dt42032Scrn = dao42032.d_42032_scrn(fmYmd);

                /******************
                條件篩選
                (1)	fut_kind_id<> ' '
                (2)	opt_kind_id<> ' '
                (3)	fut_kind_id<> ' ' OR opt_kind_id<> ' '
                (4)	change_flag = 'Y'
                ******************/
                DataView dv = dt42032Scrn.AsDataView();
                switch (rdgCondition.EditValue.AsString()) {
                    case "F":
                        dv.RowFilter = "fut_kind_id <> ' '";
                        break;
                    case "O":
                        dv.RowFilter = "opt_kind_id<> ' '";
                        break;
                    case "ALL":
                        dv.RowFilter = "fut_kind_id<> ' ' or opt_kind_id<> ' '";
                        break;
                    case "ADJ":
                        dv.RowFilter = "change_flag = 'Y'";
                        break;
                    case "SID":
                        dv.RowFilter = "stock_id=' '";
                        break;
                }
                DataTable dtFiltered = dv.ToTable();
                #endregion
                rptName = "上市證券原始資料查詢";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                //逐每一商品/股票轉出資料
                if (dt42032Scrn.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                foreach (DataRow dr in dtFiltered.Rows) {
                    fmYmd = dr["YMD_FM"].AsString();
                    toYmd = dr["YMD_TO"].AsString();
                    if (rdgCondition.EditValue.AsString() == "SID") {
                        stockId = txtSID.Text.Trim();
                    }
                    else {
                        stockId = dr["STOCK_ID"].AsString();
                    }
                    file = stockId + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";

                    /********************
                     參數：(1)模型S (SMA)、E (EWMA)、M (MaxVol)
                     　　　(2)起始日期YYYYMMDD 
                     　　　(3)迄止日期YYYYMMDD 
                     　　　(4)商品別：F(期貨) / O(選擇權) / 空白(單一證券)
                     　　　(5)商品代號：kind_id / 空白(單一證券)
                     　　　(6)證券代號stock_id
                     *********************/
                    DataTable dt42032 = dao42032.d_42032_detl(fmYmd, toYmd, stockId);
                    if (dt42032.Rows.Count <=4) continue;

                    //存CSV (ps:輸出csv 都用ascii)
                    string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, file);
                    ExportOptions csvref = new ExportOptions();
                    csvref.HasHeader = false;
                    csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
                    Common.Helper.ExportHelper.ToCsv(dt42032, filePath, csvref);
                }
                    lblProcessing.Text = "轉檔成功";
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }
    }
}