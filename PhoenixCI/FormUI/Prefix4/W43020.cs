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
        protected class LookupItem {
            public string ValueMember { get; set; }
            public string DisplayMember { get; set; }
        }

        public W43020(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            try {
                base.Open();
                txtSDate.EditValue = DateTime.Now;
                txtSDate.Focus();
#if DEBUG
                txtSDate.Text = "2018/11/15";
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

        protected override ResultStatus Export() {

            try {
                lblProcessing.Visible = true;
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

                string rptName, rptId, file;
                int rowStart;
                rptName = "股票選擇權保證金狀況表－標的證券為受益憑證";
                rptId = "43020";
                lblProcessing.Text = rptId + '－' + rptName + " 轉檔中...";

                //1. 讀取檔案
                DataTable dt43020 = dao43020.d_43020(txtSDate.DateTimeValue, oswGrp);
                if (dt43020.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    return ResultStatus.Fail;
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
                foreach (DataRow dr in dt43020.Rows) {
                    //5.1 一、現行收取保證金金額
                    rowStart = 8;
                    if (dr["MG1_TYPE"].AsString() == "A") {
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
                    rowStart = 77;
                    ws43020.Cells[rowStart + f, 7].Value = dr["MGT7_AB_XXX"].AsDecimal();
                    ws43020.Cells[rowStart + f, 8].Value = dr["MG1_PRICE"].AsDecimal();
                    ws43020.Cells[rowStart + f, 9].Value = dr["MG1_XXX"].AsDecimal();
                    ws43020.Cells[rowStart + f, 10].Value = dr["MG1_RISK"].AsDecimal();
                    ws43020.Cells[rowStart + f, 11].Value = dr["MG1_CP_RISK"].AsDecimal();
                    ws43020.Cells[rowStart + f, 12].Value = dr["MG1_CP_CM"].AsDecimal();

                    //5.3 三、本日結算保證金變動幅度
                    rowStart = 142;
                    ws43020.Cells[rowStart + f, 11].Value = dr["MG1_CHANGE_RANGE"].AsDecimal();
                    if (dr["MG1_TYPE"].AsString() == "A") {
                        ws43020.Cells[rowStart + f, 12].Value = dr["MG1_CHANGE_FLAG"].AsString();
                    }

                    f++;
                }
                //6. 刪除空白列(用Rows.remove或Range.delete都會影響到template，只好用Rows.Hide)
                int rowIndex = dt43020.Rows.Count;
                int delRowCnt = 60 - rowIndex;
                Range ra;
                if (rowIndex < 60) {
                    rowStart = 142;
                    //ra = ws43020.Range[(rowIndex + rowStart + 1).ToString() + ":" + (rowStart + 60).ToString()];
                    //ra.Delete(DeleteMode.EntireRow);
                    ws43020.Rows.Hide(rowIndex + rowStart + 1, rowStart + 60);
                    rowStart = 77;
                    //ra = ws43020.Range[(rowIndex + rowStart + 1).ToString() + ":" + (rowStart + 60).ToString()];
                    //ra.Delete(DeleteMode.EntireRow);
                    ws43020.Rows.Hide(rowIndex + rowStart + 1, rowStart + 60);
                    rowStart = 8;
                    //ra = ws43020.Range[(rowIndex + rowStart + 1).ToString() + ":" + (rowStart + 60).ToString()];
                    //ra.Delete(DeleteMode.EntireRow);
                    ws43020.Rows.Hide(rowIndex + rowStart + 1, rowStart + 60);
                }

                //7. 存檔
                ws43020.ScrollToRow(0);
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