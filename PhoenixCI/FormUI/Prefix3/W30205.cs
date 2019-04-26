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
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using Common;

/// <summary>
/// Lukas, 2019/3/12
/// 測試資料: 2012/01/01~2018/12/31
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30205 指數類歷次部位限制查詢
    /// </summary>
    public partial class W30205 : FormParent {

        string logTxt;
        private D30205 dao30205;

        public W30205(string programID, string programName) : base(programID, programName) {
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
            dao30205 = new D30205();
            string rptId, file;
            rptId = "30205";

            //複製檔案
            file = PbFunc.wf_copy_file(rptId, rptId);
            if (file == "") return ResultStatus.Fail;
            logTxt = file;

            //開啟檔案
            Workbook workbook = new Workbook();
            workbook.LoadDocument(file);

            #region wf_30205
            string rptName, raiseStr, lowerStr;
            int f, g, rowNum;
            rptName = "指數類歷次部位限制查詢";
            rptId = "30205";

            //讀取資料
            DataTable dt30205 = dao30205.d_30205(txtSDate.Text.Replace("/", ""), txtEDate.Text.Replace("/", ""));
            if (dt30205.Rows.Count == 0) {
                MessageDisplay.Info(txtSDate.Text + "～" + txtEDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                return ResultStatus.Fail;
            }

            //切換sheet
            Worksheet ws30205 = workbook.Worksheets[0];
            ws30205.Cells[1, 0].Value = ws30205.Cells[1, 0].Value.ToString() + txtSDate.Text + "～" + txtEDate.Text; //填寫公告日期

            //填入資料
            rowNum = 2;
            for (f = 0; f < dt30205.Rows.Count; f++) {
                DataRow dr = dt30205.Rows[f];
                rowNum = rowNum + 1;
                raiseStr = "";
                lowerStr = "";
                string pl2Ymd, pl2EffectiveYmd;
                pl2Ymd = dr["PL2_YMD"].AsString();
                pl2EffectiveYmd = dr["PL2_EFFECTIVE_YMD"].AsString();
                //運算欄位cp_grp_cnt("count(  pl2_kind_id  for group 1 )")
                //group 1 ("pl2_ymd" , "pl2_effective_ymd)
                int cpGrpCnt = dt30205.Compute("count( pl2_kind_id)", $@"PL2_YMD='{pl2Ymd}' and PL2_EFFECTIVE_YMD='{pl2EffectiveYmd}'").AsInt();
                for (g = 1; g <= cpGrpCnt; g++) {
                    if (dt30205.Rows[f + g - 1]["PL2_NATURE_ADJ"].AsString() == "+") {
                        raiseStr = raiseStr + dt30205.Rows[f + g - 1]["PL2_KIND_ID"].AsString() + "/";
                    }
                    else {
                        lowerStr = lowerStr + dt30205.Rows[f + g - 1]["PL2_KIND_ID"].AsString() + "/";
                    }
                }
                f = f + cpGrpCnt - 1;
                if (dr["PL2_YMD"] != DBNull.Value) ws30205.Cells[rowNum, 0].Value = dr["PL2_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                if (dr["PL2_EFFECTIVE_YMD"] != DBNull.Value) ws30205.Cells[rowNum, 1].Value = dr["PL2_EFFECTIVE_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
                ws30205.Cells[rowNum, 2].Value = raiseStr.SubStr(0, raiseStr.Length - 1);
                ws30205.Cells[rowNum, 3].Value = lowerStr.SubStr(0, lowerStr.Length - 1);
            }
            #endregion

            //存檔
            ws30205.ScrollToRow(0);
            workbook.SaveDocument(file);

            return ResultStatus.Success;
        }
    }
}