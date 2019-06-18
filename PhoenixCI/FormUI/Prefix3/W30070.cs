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
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;

/// <summary>
/// Lukas, 2019/3/11
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30070 期貨各商品成交值轉檔
    /// </summary>
    public partial class W30070 : FormParent {

        string logTxt;
        private D30070 dao30070; 

        public W30070(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;
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

        protected override ResultStatus Export() {
            dao30070 = new D30070();
            #region ue_export_before
            string lsRtn;

            lsRtn = PbFunc.f_get_jsw_seq(_ProgramID, "E", 0, txtEDate.DateTimeValue, "0");
            if (lsRtn != "") {
                DialogResult liRtn = MessageDisplay.Choose(txtEDate.Text + " 統計資料未轉入完畢,是否要繼續?" + Environment.NewLine + lsRtn);
                if (liRtn == DialogResult.No) {
                    lblProcessing.Visible = false;
                    this.Cursor = Cursors.Arrow;
                    return ResultStatus.Fail;
                }
            }
            #endregion

            int rowNum;
            string rptId, lsFile;
            rptId = "30070";
            //複製檔案
            lsFile = PbFunc.wf_copy_file(rptId, rptId);
            if (lsFile == "") return ResultStatus.Fail;
            logTxt = lsFile;

            //開啟檔案
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);

            rowNum = 0;
            #region wf_30070
            string rptName, lsYmd;
            rptName = "期貨各商品成交值";
            rptId = "30070";

            //切換Sheet
            Worksheet ws30070 = workbook.Worksheets[0];

            //讀取資料 (每日)
            DataTable dt30070 = dao30070.d_30070(txtSDate.Text.Replace("/",""),txtEDate.Text.Replace("/", ""));
            if (dt30070.Rows.Count == 0) {
                //nothing happens
            }

            lsYmd = "";
            foreach (DataRow dr in dt30070.Rows) {
                rowNum += 1;
                ws30070.Cells[rowNum, 0].Value = dr["AA2_YMD"].AsString();
                ws30070.Cells[rowNum, 1].Value = dr["AA2_PARAM_KEY"].AsString();
                ws30070.Cells[rowNum, 2].SetValue(dr["AA2_AMT"]);
            }
            #endregion

            rowNum = 0;
            #region wf_30071
            rptName = "期貨各商品成交值(現貨價格計算)";
            rptId = "30070_stk";

            //切換Sheet
            Worksheet ws30070stk = workbook.Worksheets[1];

            //讀取資料 (每日)
            DataTable dt30070stk = dao30070.d_30070_stk(txtSDate.Text.Replace("/", ""), txtEDate.Text.Replace("/", ""));
            if (dt30070stk.Rows.Count == 0) {
                //nothing happens
            }

            lsYmd = "";
            foreach (DataRow dr in dt30070stk.Rows) {
                rowNum += 1;
                ws30070stk.Cells[rowNum, 0].Value = dr["AA2_YMD"].AsString();
                ws30070stk.Cells[rowNum, 1].Value = dr["AA2_PARAM_KEY"].AsString();
                ws30070stk.Cells[rowNum, 2].SetValue(dr["AA2_AMT"]);
            }
            #endregion

            //存檔
            //ws30070.ScrollToRow(0);
            workbook.SaveDocument(lsFile);
            return ResultStatus.Success;
        }
    }
}