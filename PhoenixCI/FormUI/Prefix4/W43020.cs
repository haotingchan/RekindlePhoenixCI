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
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Controls;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using System.Threading;

/// <summary>
/// Lukas, 2019/3/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 43020 標的證券為受益憑證之股票選擇權保證金狀況表
    /// </summary>
    public partial class W43020 : FormParent {

        private D43020 dao43020;
        private OCFG daoOCFG;

        public W43020(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            try {
                base.Open();
                txtSDate.DateTimeValue = DateTime.Now;
                txtSDate.Focus();
#if DEBUG
                txtSDate.Text = "2019/05/22";
#endif
                //設定商品交易時段下拉選單
                List<LookupItem> ddlOswGrp = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1", DisplayMember = "Group1 (13:45)"},
                                        new LookupItem() { ValueMember = "5", DisplayMember = "Group2 (16:15)" },
                                        new LookupItem() { ValueMember = "%", DisplayMember = "ALL" }};
                Extension.SetDataTable(dwOswGrp, ddlOswGrp, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");

                //從共用方法取得商品交易時段
                daoOCFG = new OCFG();
                dwOswGrp.EditValue = daoOCFG.f_gen_osw_grp();
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

        protected void ShowMsg(string msg) {
            lblProcessing.Text = msg;
            this.Refresh();
            Thread.Sleep(5);
        }

        protected override ResultStatus Export() {

            try {
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                dao43020 = new D43020();
                #region ue_export_before
                //130批次作業做完
                string rtnStr, oswGrp;
                oswGrp = dwOswGrp.EditValue + "%";
                rtnStr = PbFunc.f_chk_130_wf(_ProgramID, txtSDate.DateTimeValue, oswGrp);
                if (rtnStr != "") {
                    DialogResult result = MessageDisplay.Choose(txtSDate.Text + "-" + rtnStr + "，是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }
                #endregion

                #region sheet 1
                string rptName, rptId, file;
                int rowStart;
                rptName = "股票選擇權保證金狀況表－標的證券為受益憑證";
                rptId = "43020";
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");

                //1. 讀取檔案
                DataTable dt43020 = dao43020.d_43020(txtSDate.DateTimeValue.ToString("yyyyMMdd"), oswGrp,"S");
                if (dt43020.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    ShowMsg("");
                    //return ResultStatus.Fail;
                }

                //2. 複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") {
                    return ResultStatus.Fail;
                }

                //3. 開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //4. 切換Sheet
                Worksheet ws43020 = workbook.Worksheets[0];
                ws43020.Cells[4, 11].Value = "資料日期：" + txtSDate.DateTimeValue.Year + "年" + txtSDate.DateTimeValue.Month + "月" + txtSDate.DateTimeValue.Day + "日";

                //5. 填入資料
                int cnt = 0;
                int f = 0;
                int rowCount = dt43020.Rows.Count;
                foreach (DataRow dr in dt43020.Rows) {
                    //5.1 一、現行收取保證金金額
                    rowStart = 8;
                    if (dr["MG1_AB_TYPE"].AsString() == "A") {
                        cnt = cnt + 1;
                        ws43020.Cells[rowStart + f, 1].Value = cnt.AsString();
                        ws43020.Cells[rowStart + f, 2].Value = dr["MG1_KIND_ID"].AsString();
                        ws43020.Cells[rowStart + f, 3].Value = dr["APDK_NAME"].AsString();
                        ws43020.Cells[rowStart + f, 4].Value = dr["APDK_STOCK_ID"].AsString();
                        ws43020.Cells[rowStart + f, 5].Value = dr["PID_NAME"].AsString();
                    }
                    ws43020.Cells[rowStart + f, 7].Value = dr["MG1_CUR_CM"].AsDecimal();
                    ws43020.Cells[rowStart + f, 9].Value = dr["MG1_CUR_MM"].AsDecimal();
                    ws43020.Cells[rowStart + f, 11].Value = dr["MG1_CUR_IM"].AsDecimal();

                    //5.2 二、本日結算保證金計算
                    //SMA 從B79開始填資料
                    rowStart = 78;
                    if (dr["MG1_AB_TYPE"].AsString() == "A") {
                        //cnt = cnt + 1;
                        ws43020.Cells[rowStart + f, 1].Value = cnt.AsString();
                        ws43020.Cells[rowStart + f, 2].Value = dr["MG1_KIND_ID"].AsString();
                        ws43020.Cells[rowStart + f, 3].Value = dr["APDK_NAME"].AsString();
                        ws43020.Cells[rowStart + f, 4].Value = dr["APDK_STOCK_ID"].AsString();
                        ws43020.Cells[rowStart + f, 5].Value = dr["PID_NAME"].AsString();
                    }
                    ws43020.Cells[rowStart + f, 7].SetValue(dr["MGT7_AB_XXX"]);
                    ws43020.Cells[rowStart + f, 8].SetValue(dr["MG1_PRICE"]);
                    ws43020.Cells[rowStart + f, 9].SetValue(dr["MG1_XXX"]);
                    ws43020.Cells[rowStart + f, 10].SetValue(dr["MG1_RISK"]);
                    ws43020.Cells[rowStart + f, 11].SetValue(dr["MG1_CP_RISK"]);
                    ws43020.Cells[rowStart + f, 12].SetValue(dr["MG1_MIN_RISK"]);
                    ws43020.Cells[rowStart + f, 13].SetValue(dr["MG1_CP_CM"]);
                    ws43020.Cells[rowStart + f, 14].SetValue(dr["MG1_CUR_CM"]);
                    ws43020.Cells[rowStart + f, 15].SetValue(dr["MG1_CHANGE_RANGE"]);
                    ws43020.Cells[rowStart + f, 16].SetValue(dr["MG1_CHANGE_FLAG"]);
                    f++;
                }//foreach (DataRow dr in dt43020.Rows)
                dt43020 = dao43020.d_43020(txtSDate.DateTimeValue.ToString("yyyyMMdd"), oswGrp, "E");
                cnt = 0;
                f = 0;
                foreach (DataRow dr in dt43020.Rows) {
                    //EWMA 從B147開始填資料
                    rowStart = 146;
                    if (dr["MG1_AB_TYPE"].AsString() == "A") {
                        cnt = cnt + 1;
                        ws43020.Cells[rowStart + f, 1].Value = cnt.AsString();
                        ws43020.Cells[rowStart + f, 2].Value = dr["MG1_KIND_ID"].AsString();
                        ws43020.Cells[rowStart + f, 3].Value = dr["APDK_NAME"].AsString();
                        ws43020.Cells[rowStart + f, 4].Value = dr["APDK_STOCK_ID"].AsString();
                        ws43020.Cells[rowStart + f, 5].Value = dr["PID_NAME"].AsString();
                    }
                    ws43020.Cells[rowStart + f, 7].SetValue(dr["MGT7_AB_XXX"]);
                    ws43020.Cells[rowStart + f, 8].SetValue(dr["MG1_PRICE"]);
                    ws43020.Cells[rowStart + f, 9].SetValue(dr["MG1_XXX"]);
                    ws43020.Cells[rowStart + f, 10].SetValue(dr["MG1_RISK"]);
                    ws43020.Cells[rowStart + f, 11].SetValue(dr["MG1_CP_RISK"]);
                    ws43020.Cells[rowStart + f, 12].SetValue(dr["MG1_MIN_RISK"]);
                    ws43020.Cells[rowStart + f, 13].SetValue(dr["MG1_CP_CM"]);
                    ws43020.Cells[rowStart + f, 14].SetValue(dr["MG1_CUR_CM"]);
                    ws43020.Cells[rowStart + f, 15].SetValue(dr["MG1_CHANGE_RANGE"]);
                    //ws43020.Cells[rowStart + f, 16].SetValue(dr["MG1_CHANGE_FLAG"]);
                    f++;
                }//foreach (DataRow dr in dt43020.Rows)
                dt43020 = dao43020.d_43020(txtSDate.DateTimeValue.ToString("yyyyMMdd"), oswGrp, "M");
                cnt = 0;
                f = 0;
                foreach (DataRow dr in dt43020.Rows) {
                    //MAX 從B215開始填資料
                    rowStart = 214;
                    if (dr["MG1_AB_TYPE"].AsString() == "A") {
                        cnt = cnt + 1;
                        ws43020.Cells[rowStart + f, 1].Value = cnt.AsString();
                        ws43020.Cells[rowStart + f, 2].Value = dr["MG1_KIND_ID"].AsString();
                        ws43020.Cells[rowStart + f, 3].Value = dr["APDK_NAME"].AsString();
                        ws43020.Cells[rowStart + f, 4].Value = dr["APDK_STOCK_ID"].AsString();
                        ws43020.Cells[rowStart + f, 5].Value = dr["PID_NAME"].AsString();
                    }
                    ws43020.Cells[rowStart + f, 7].SetValue(dr["MGT7_AB_XXX"]);
                    ws43020.Cells[rowStart + f, 8].SetValue(dr["MG1_PRICE"]);
                    ws43020.Cells[rowStart + f, 9].SetValue(dr["MG1_XXX"]);
                    ws43020.Cells[rowStart + f, 10].SetValue(dr["MG1_RISK"]);
                    ws43020.Cells[rowStart + f, 11].SetValue(dr["MG1_CP_RISK"]);
                    ws43020.Cells[rowStart + f, 12].SetValue(dr["MG1_MIN_RISK"]);
                    ws43020.Cells[rowStart + f, 13].SetValue(dr["MG1_CP_CM"]);
                    ws43020.Cells[rowStart + f, 14].SetValue(dr["MG1_CUR_CM"]);
                    ws43020.Cells[rowStart + f, 15].SetValue(dr["MG1_CHANGE_RANGE"]);
                    ws43020.Cells[rowStart + f, 16].SetValue(dr["MG1_CHANGE_FLAG"]);

                    f++;
                }//foreach (DataRow dr in dt43020.Rows)
                //6. 刪除空白列(用Rows.remove或Range.delete都會影響到template，只好用Rows.Hide)
                int delRowCnt = 60 - rowCount;
                Range ra;
                if (rowCount < 60) {
                    rowStart = 213;
                    //ra = ws43020.Range[(rowIndex + rowStart + 1).ToString() + ":" + (rowStart + 60).ToString()];
                    //ra.Delete(DeleteMode.EntireRow);
                    ws43020.Rows.Hide(rowCount + rowStart + 1, rowStart + 60);
                    rowStart = 145;
                    //ra = ws43020.Range[(rowIndex + rowStart + 1).ToString() + ":" + (rowStart + 60).ToString()];
                    //ra.Delete(DeleteMode.EntireRow);
                    ws43020.Rows.Hide(rowCount + rowStart + 1, rowStart + 60);
                    rowStart = 77;
                    //ra = ws43020.Range[(rowIndex + rowStart + 1).ToString() + ":" + (rowStart + 60).ToString()];
                    //ra.Delete(DeleteMode.EntireRow);
                    ws43020.Rows.Hide(rowCount + rowStart + 1, rowStart + 60);
                    rowStart = 7;
                    ws43020.Rows.Hide(rowCount + rowStart + 1, rowStart + 60);
                }
                #endregion

                #region sheet 2 
                rptName = "保證金狀況表";
                rptId = "40011_stat";
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");

                //1. 讀取檔案
                DataTable dt40011stat = dao43020.d_40011_stat(txtSDate.DateTimeValue.ToString("yyyyMMdd"));
                dt40011stat = dt40011stat.Sort("seq_no, kind_id");
                dt40011stat = dt40011stat.Filter("prod_type ='O' and param_key = 'ETC'");
                if (dt40011stat.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    ShowMsg("");
                    //return ResultStatus.Fail;
                }


                //2. 切換Sheet
                ws43020 = workbook.Worksheets["opt_3index"];

                //3. 填入資料
                ws43020.Cells[0, 0].Value = "資料日期：" + Environment.NewLine + txtSDate.DateTimeValue.Year + "年" + txtSDate.DateTimeValue.Month + "月" + txtSDate.DateTimeValue.Day + "日";
                int rowNum = 3 - 1;
                foreach (DataRow dr in dt40011stat.Rows) {
                    for (f = 0; f < 33; f++) {
                        ws43020.Cells[rowNum, f].SetValue(dr[f]);
                    }
                    rowNum++;
                }

                #endregion

                //存檔
                ws43020.ScrollToRow(0);
                if (rowCount == 0 && dt40011stat.Rows.Count == 0) {
                    workbook = null;
                    System.IO.File.Delete(file);
                    return ResultStatus.Fail;
                }
                workbook.SaveDocument(file);
                ShowMsg("轉檔成功");
            }
            catch (Exception ex) {
                //WriteLog(ex, "", false); 如果不用throw會繼續往下執行(?
                ShowMsg("轉檔錯誤");
                throw ex;
            }
            finally {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
            }
            return ResultStatus.Success;
        }
    }
}