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
using DevExpress.Spreadsheet;

/// <summary>
/// Lukas, 2019/3/27
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30204 股價指數暨黃金類公告表
    /// </summary>
    public partial class W30204 : FormParent {

        private D30204 dao30204;

        public W30204(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                txtSDate.EditValue = PbFunc.f_ocf_date(0);
                txtEDate.EditValue = txtSDate.EditValue;
#if DEBUG
                txtSDate.Text = "2018/03/31";
                txtEDate.Text = txtSDate.Text;
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
                lblProcessing.Text= "開始轉檔...";
                lblProcessing.Visible = true;
                dao30204 = new D30204();
                string rptId, file, rptName,
                    sYmd = txtSDate.DateTimeValue.ToString("yyyyMMdd"),
                    eYmd = txtEDate.DateTimeValue.ToString("yyyyMMdd");
                int rowNum, rowTol, seq, rowUp, seqUp, rowDown, seqDown, rowMinus;
                rptId = "30204";
                rptName = "股類指數暨黃金類交易人部位限制調整一覽表";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                //讀取資料
                DataTable dt30204 = dao30204.d_30204(sYmd, eYmd);
                if (dt30204.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //寫入資料
                rowNum = 7;
                #region wf_30204_data(9)
                //切換Sheet
                Worksheet ws30204 = workbook.Worksheets[1];

                //從A8開始填資料
                ws30204.Import(dt30204, false, rowNum, 0);
                #endregion

                rowNum = 4;
                #region wf_30204_new
                ws30204 = workbook.Worksheets[0];
                //提高filter("pl2_nature_adj<>'-' and pl2_nature_adj<>' '")
                rowTol = rowNum + 500;
                seq = 0;
                //從A5開始填資料
                dt30204 = dao30204.d_30204_up(sYmd, eYmd);
                ws30204.Import(dt30204, false, rowNum, 0);
                rowUp = rowNum + dt30204.Rows.Count;
                seqUp = dt30204.Rows.Count;
                //降低filter("pl2_nature_adj='-'")
                //從A509開始填資料
                rowNum = 508;
                rowTol = rowNum + 500;
                dt30204 = dao30204.d_30204_down(sYmd, eYmd);
                ws30204.Import(dt30204, false, rowNum, 0);
                rowDown = rowNum + dt30204.Rows.Count;
                seqDown = dt30204.Rows.Count;
                #endregion

                rptName = "公告表－股價指數暨黃金類(公債類)";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";
                dt30204 = dao30204.d_30204_gbf(sYmd, eYmd);
                if (dt30204.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                rowNum = 3;
                #region wf_30204_data(13)
                //從A4開始填資料
                ws30204 = workbook.Worksheets[1];
                ws30204.Import(dt30204, false, rowNum, 0);
                #endregion

                rowNum = 1;
                #region wf_30204_new_gbf
                ws30204 = workbook.Worksheets[0];
                //提高
                dt30204 = dao30204.d_30204_gbf_up(sYmd, eYmd);
                rowNum = rowUp;
                rowTol = 500 + rowNum;
                seq = seqUp;
                ws30204.Import(dt30204, false, rowNum, 0);
                rowNum = rowNum + dt30204.Rows.Count;
                //刪除空白列
                rowMinus = rowTol - rowNum;
                if (rowTol > rowNum) {
                    Range ra;
                    ra = ws30204.Range[(rowNum + 1).ToString() + ":" + rowTol.ToString()];
                    ra.Delete(DeleteMode.EntireRow);
                }

                //降低
                dt30204 = dao30204.d_30204_gbf_down(sYmd, eYmd);
                rowNum = rowDown - rowMinus;
                rowTol = 500 + rowNum;
                seq = seqDown;
                ws30204.Import(dt30204, false, rowNum, 0);
                rowNum = rowNum + dt30204.Rows.Count;
                //刪除空白列
                if (rowTol > rowNum) {
                    Range ra;
                    ra = ws30204.Range[(rowNum + 1).ToString() + ":" + rowTol.ToString()];
                    ra.Delete(DeleteMode.EntireRow);
                }
                #endregion

                //存檔
                ws30204.ScrollToRow(0);
                workbook.SaveDocument(file);
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