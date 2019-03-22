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
using DevExpress.XtraEditors.Controls;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using Common;
using DevExpress.Spreadsheet;

/// <summary>
/// Lukas, 2019/3/20
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

    /// <summary>
    /// 43010 標的證券為受益憑證之股票期貨保證金狀況表
    /// </summary>
    public partial class W43010 : FormParent {

        private D43010 dao43010;
        private OCFG daoOCFG;
        protected class LookupItem {
            public string ValueMember { get; set; }
            public string DisplayMember { get; set; }
        }

        public W43010(string programID, string programName) : base(programID, programName) {
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
                dao43010 = new D43010();
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
                rptName = "股票期貨保證金狀況表－標的證券為受益憑證";
                rptId = "43010";
                lblProcessing.Text = rptId + '－' + rptName + " 轉檔中...";

                //1. 讀取檔案
                DataTable dt43010 = dao43010.d_43010a(txtSDate.DateTimeValue, oswGrp);
                if (dt43010.Rows.Count == 0) {
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
                Worksheet ws43010 = workbook.Worksheets[0];

                //5. 填入資料
                //5.1 一、現行收取保證金金額
                //從B3開始填資料
                rowStart = 2;
                ws43010.Import(dt43010, false, rowStart, 1);
                //5.2 二、本日結算保證金計算
                //從B37開始填資料
                rowStart = 36;
                dt43010 = dao43010.d_43010b(txtSDate.DateTimeValue, oswGrp);
                ws43010.Import(dt43010, false, rowStart, 6);
                //5.3 三、本日結算保證金變動幅度
                //從B70開始填資料
                rowStart = 69;
                dt43010 = dao43010.d_43010c(txtSDate.DateTimeValue, oswGrp);
                ws43010.Import(dt43010, false, rowStart, 8);

                //6. 刪除空白列
                int rowIndex = dt43010.Rows.Count;
                int delRowCnt = 30 - rowIndex;
                if (rowIndex < 30) {
                    rowStart = 69;
                    ws43010.Rows.Remove(rowIndex + rowStart, delRowCnt);
                    rowStart = 36 ;
                    ws43010.Rows.Remove(rowIndex + rowStart, delRowCnt);
                    rowStart = 2 ;
                    ws43010.Rows.Remove(rowIndex + rowStart, delRowCnt);
                }

                //7. 存檔
                ws43010.ScrollToRow(0);
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