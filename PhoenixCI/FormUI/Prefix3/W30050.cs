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
using DataObjects.Dao.Together;

/// <summary>
/// Lukas, 2019/3/6
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30050 期貨市場商品(日)紀錄彙整
    /// </summary>
    public partial class W30050 : FormParent {

        private D30050 dao30050;
        private RPT daoRPT;

        public W30050(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            try {
                base.Open();
                txtSDate.EditValue = PbFunc.f_ocf_date(0);
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
                dao30050 = new D30050();
                daoRPT = new RPT();
                string rptId = "30050", file;
                string asYmd = txtSDate.Text.Replace("/", "");

                // 1. 複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") {
                    return ResultStatus.Fail;
                }

                // 2. 開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                // 3. 匯出資料
                int rowNum = 1;
                string rptName, dataType, ymd;
                int g, rowNumRPT, found, colNum;

                #region wf_30051
                rptName = "最大交易量及未平倉數";
                rptId = "30051";

                // 切換Sheet
                Worksheet ws30050 = workbook.Worksheets[0];

                // 讀取並填入資料
                for (g = 1; g < 3; g++) {
                    if (g == 1) {
                        dataType = "M";
                        colNum = 1;
                    }
                    else {
                        dataType = "OI";
                        colNum = 3;
                    }
                    DataTable dt30051 = dao30050.d_30051(asYmd, dataType);
                    if (dt30051.Rows.Count == 0) {
                        MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                        return ResultStatus.Fail;
                    }
                    dt30051.Filter("RPT_SEQ_NO > 0");

                    foreach (DataRow dr in dt30051.Rows) {
                        rowNumRPT = dr["RPT_SEQ_NO"].AsInt() - 1;
                        ws30050.Cells[rowNumRPT, colNum].Value = dr["AI4_QNTY"].AsDecimal();
                        ymd = dr["AI4_MAX_YMD"].AsString();
                        ymd = ymd.SubStr(0, 4) + "/" + ymd.SubStr(4, 2) + "/" + ymd.SubStr(6, 2);
                        ws30050.Cells[rowNumRPT, colNum + 1].Value = ymd;
                    }
                }
                #endregion

                rowNum = rowNum + 3;

                #region wf_30052
                string sumType = "", sumSubtype = "", prodType = "", paramKey = "";

                rptName = "交易量及未平倉數排序";
                rptId = "30052";

                /*******************
                RPT
                *******************/
                daoRPT = new RPT();
                DataTable dtRPT = daoRPT.ListAllByTXD_ID(rptId);
                if (dtRPT.Rows.Count == 0) {
                    MessageDisplay.Error(rptId + '－' + "RPT無任何資料!");
                    return ResultStatus.Fail;
                }

                for (g = 1; g < 9; g++) {
                    if (g == 2 || g == 4 || g == 6) {
                        dataType = "OI";
                        colNum = 3;
                    }
                    else {
                        dataType = "M";
                        colNum = 1;
                    }
                    switch (g) {
                        case 1:
                        case 2:
                            sumType = "D";
                            sumSubtype = "1";
                            prodType = "F%";
                            paramKey = "%";
                            break;
                        case 3:
                        case 4:
                            sumType = "D";
                            sumSubtype = "3";
                            prodType = "O%";
                            paramKey = "TXO%";
                            break;
                        case 5:
                        case 6:
                            sumType = "D";
                            sumSubtype = "0";
                            prodType = "%";
                            paramKey = "%";
                            break;
                        case 7:
                            sumType = "Y";
                            sumSubtype = "0";
                            prodType = "%";
                            paramKey = "%";
                            break;
                        case 8:
                            sumType = "M";
                            sumSubtype = "0";
                            prodType = "%";
                            paramKey = "%";
                            colNum = 3;
                            break;
                    }

                    DataTable dt30052 = dao30050.d_30052(asYmd, sumType, sumSubtype, prodType, paramKey, dataType);
                    if (dt30052.Rows.Count == 0) {
                        return ResultStatus.Fail;
                    }
                    DataRow[] find = dtRPT.Select("rpt_seq_no=" + g.AsString());
                    if (find.Length != 0) {
                        found = dtRPT.Rows.IndexOf(find[0]);
                    }
                    else {
                        continue;
                    }
                    rowNum = dtRPT.Rows[found]["RPT_VALUE"].AsString().AsInt() - 1;
                    rowNum = rowNum - 1;
                    foreach (DataRow dr in dt30052.Rows) {
                        rowNum = rowNum + 1;
                        ws30050.Cells[rowNum, colNum].Value = dr["AI4_QNTY"].AsDecimal();
                        ymd = dr["AI4_MAX_YMD"].AsString();
                        if (g == 7) {
                            ymd = ymd;
                        }
                        else if (g == 8) {
                            ymd = ymd.SubStr(0, 4) + "/" + ymd.SubStr(4, 2);
                        }
                        else {
                            ymd = ymd.SubStr(0, 4) + "/" + ymd.SubStr(4, 2) + "/" + ymd.SubStr(6, 2);
                        }
                        ws30050.Cells[rowNum, colNum + 1].Value = ymd;
                    }
                }

                // g = 9
                DataRow[] findG9 = dtRPT.Select("rpt_seq_no=" + g.AsString());
                if (findG9.Length != 0) {
                    found = dtRPT.Rows.IndexOf(findG9[0]);
                    rowNum = dtRPT.Rows[found]["RPT_VALUE"].AsString().AsInt() - 1;
                    ws30050.Cells[rowNum, 1].Value = txtSDate.Text;
                }

                #endregion

                // 4. 存檔
                ws30050.ScrollToRow(0);
                workbook.SaveDocument(file);
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }
    }
}