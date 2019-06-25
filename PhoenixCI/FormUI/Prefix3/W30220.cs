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
using Common;
using DataObjects.Dao.Together.SpecificDao;

/// <summary>
/// Lukas, 2019/3/12
/// 測試資料: 2018/09/30, 2018/10~2018/12, 2018/09/30
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30220 個股類歷史交易量及流通在外股數
    /// </summary>
    public partial class W30220 : FormParent {

        string logTxt;
        private D30220 dao30220;

        public W30220(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();
            DateTime date = DateTime.Now;
            txtDate.EditValue = date.ToString("yyyy/MM/dd");
            txtStkoutDate.EditValue = date.ToString("yyyy/MM/dd");

            date = PbFunc.relativedate(date, date.Day * -1);
            txtEMonth.EditValue = date.ToString("yyyy/MM");
            date = PbFunc.relativedate(date, date.Day * -1);
            date = PbFunc.relativedate(date, date.Day * -1);
            txtSMonth.EditValue = date.ToString("yyyy/MM");
            txtEMonth.Focus();
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

            dao30220 = new D30220();
            string rptId, file;
            rptId = "30220";

            //複製檔案
            file = PbFunc.wf_copy_file(rptId, rptId);
            if (file == "") return ResultStatus.Fail;
            logTxt = file;

            //開啟檔案
            Workbook workbook = new Workbook();
            workbook.LoadDocument(file);

            int rowNum = 1;
            #region wf_30220
            string rptName, stkYmd, lsStr;
            int rowTol;
            rptName = "個股類歷史交易量及流通在外股數";
            rptId = "30220";
            rowTol = 500;
            rowNum = 3;
            //讀取資料
            stkYmd = txtStkoutDate.Text.Replace("/", "");
            DataTable dt30220 = dao30220.d_30220(txtDate.Text.Replace("/", ""), txtSMonth.Text.Replace("/", ""), txtEMonth.Text.Replace("/", ""), stkYmd);
            if (dt30220.Rows.Count == 0) {
                MessageDisplay.Info(txtEMonth.Text.Replace("/", "") + "," + rptId + '－' + rptName + ",無任何資料!");
                //若所有Sheet皆無資料時，刪除檔案
                workbook = null;
                System.IO.File.Delete(file);
                return ResultStatus.Fail;
            }

            //切換sheet
            Worksheet ws30220 = workbook.Worksheets[0];

            //填入資料
            rowNum = 0;
            if (dt30220.Rows.Count > 0) {
                string pls3ymd = "";
                if (dt30220.Rows[0]["PLS3_YM3"] != DBNull.Value) pls3ymd = dt30220.Rows[0]["PLS3_YM3"].AsDateTime("yyyyMM").ToString("yyyy/MM");
                if (dt30220.Rows[0]["PLS3_YM1"] != DBNull.Value) ws30220.Cells[0, 4].Value = dt30220.Rows[0]["PLS3_YM1"].AsDateTime("yyyyMM").ToString("yyyy/MM");
                if (dt30220.Rows[0]["PLS3_YM2"] != DBNull.Value) ws30220.Cells[0, 5].Value = dt30220.Rows[0]["PLS3_YM2"].AsDateTime("yyyyMM").ToString("yyyy/MM");
                if (dt30220.Rows[0]["PLS3_YM3"] != DBNull.Value) ws30220.Cells[0, 6].Value = dt30220.Rows[0]["PLS3_YM3"].AsDateTime("yyyyMM").ToString("yyyy/MM");
                if (dt30220.Rows[0]["PLS3_YM1"] != DBNull.Value) ws30220.Cells[0, 7].Value = dt30220.Rows[0]["PLS3_YM1"].AsDateTime("yyyyMM").ToString("yyyy/MM") +
                                                                                             "－" + pls3ymd + "\r" + "\n" + ws30220.Cells[0, 7].Value.AsString();
                ws30220.Cells[0, 8].Value = txtStkoutDate.Text + "\r" + "\n" + ws30220.Cells[0, 8].Value.AsString();
            }

            foreach (DataRow dr in dt30220.Rows) {
                rowNum = rowNum + 1;
                if (dr["PLS4_KIND_ID2"] != DBNull.Value) ws30220.Cells[rowNum, 1].Value = dr["PLS4_KIND_ID2"].AsString();
                if (dr["APDK_STOCK_ID"] != DBNull.Value) ws30220.Cells[rowNum, 2].Value = dr["APDK_STOCK_ID"].AsString();
                lsStr = dr["APDK_NAME"].AsString();
                if (PbFunc.Pos(lsStr, "期貨") >= 0) lsStr = lsStr.SubStr(0, lsStr.IndexOf("期貨") + 1);
                if (PbFunc.Pos(lsStr, "選擇權") >= 0) lsStr = lsStr.SubStr(0, lsStr.IndexOf("選擇權") + 1);
                ws30220.Cells[rowNum, 3].Value = lsStr;
                if (dr["PLS3_TOT_QNTY1"] != DBNull.Value) ws30220.Cells[rowNum, 4].Value = dr["PLS3_TOT_QNTY1"].AsDecimal();
                if (dr["PLS3_TOT_QNTY2"] != DBNull.Value) ws30220.Cells[rowNum, 5].Value = dr["PLS3_TOT_QNTY2"].AsDecimal();
                if (dr["PLS3_TOT_QNTY3"] != DBNull.Value) ws30220.Cells[rowNum, 6].Value = dr["PLS3_TOT_QNTY3"].AsDecimal();
                ws30220.Cells[rowNum, 7].Value = dr["PLS3_TOT_QNTY1"].AsDecimal() + dr["PLS3_TOT_QNTY2"].AsDecimal() + dr["PLS3_TOT_QNTY3"].AsDecimal();
                if (dr["STKOUT_B"] != DBNull.Value) ws30220.Cells[rowNum, 8].Value = dr["STKOUT_B"].AsDecimal();
            }
            #endregion

            //存檔
            ws30220.ScrollToRow(0);
            workbook.SaveDocument(file);

            return ResultStatus.Success;
        }
    }
}