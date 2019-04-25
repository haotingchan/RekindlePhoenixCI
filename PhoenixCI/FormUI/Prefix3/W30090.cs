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
/// 測試資料: 2014/01/01 ～ 2017/12/31
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30090 Position-Transfer to TAIFEX by Investor Group
    /// </summary>
    public partial class W30090 : FormParent {

        string logTxt;
        private D30090 dao30090;

        public W30090(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;
                txtSDate.EditValue = txtEDate.Text.SubStr(0, 8) + "01";
#if DEBUG
                txtSDate.Text = "2014/01/01";
                txtEDate.Text = "2017/12/31";
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
                lblProcessing.Visible = true;
                dao30090 = new D30090();
                string rptId, file, rptName, kindId, kindIdName;
                int rowNum, colNum;
                rptId = "30090";
                rptName = "Position-Transfer to TAIFEX by Investor Group";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                //讀取資料
                DataTable dt30090 = dao30090.d_30090(txtSDate.Text.Replace("/", ""), txtEDate.Text.Replace("/", ""));
                if (dt30090.Rows.Count == 0) {
                    MessageDisplay.Info(GlobalInfo.OCF_DATE.ToString("yyyyMM") + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;
                logTxt = file;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                #region wf_30090
                
                //切換sheet
                Worksheet ws30090 = workbook.Worksheets[0];
                ws30090.Cells[1, 1].Value = "Date:" + txtSDate.Text + "～" + txtEDate.Text; //填寫搜尋日期

                //填入資料
                kindId = "";
                rowNum = 2;
                foreach (DataRow dr in dt30090.Rows) {
                    if (kindId != dr["AE1_PARAM_KEY"].AsString()) {
                        kindId = dr["AE1_PARAM_KEY"].AsString();
                        rowNum = rowNum + 1;
                        switch (kindId) {
                            case "TXF":
                                kindIdName = "FTX";
                                break;
                            case "TXO":
                                kindIdName = "OTX";
                                break;
                            case "ZZZ":
                                kindIdName = "合計";
                                break;
                            default:
                                kindIdName = kindId;
                                break;
                        }
                        ws30090.Cells[rowNum, 1].Value = kindIdName;
                    }
                    colNum = 0;
                    switch (dr["AE1_IDFG_TYPE"].AsString()) {
                        case "1"://證券自營
                            colNum = 3;
                            break;
                        case "2"://證券投信
                            colNum = 4;
                            break;
                        case "3"://外資
                            colNum = 5;
                            break;
                        case "4"://期貨經理事業
                            colNum = 6;
                            break;
                        case "5"://一般法人
                            colNum = 7;
                            break;
                        case "6"://期貨自營商
                            colNum = 9;
                            break;
                        case "7"://自然人
                            colNum = 2;
                            break;
                        case "8"://一般法人
                            colNum = 8;
                            break;
                        default:
                            continue;
                    }
                    if (rowNum > 0 && colNum > 0) {
                        if (dr["AE1_ACCEPTED_OI"] != DBNull.Value) ws30090.Cells[rowNum, colNum].Value = dr["AE1_ACCEPTED_OI"].AsDecimal();
                    }
                }
                #endregion

                //存檔
                ws30090.ScrollToRow(0);
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