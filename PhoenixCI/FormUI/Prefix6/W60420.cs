using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

/// <summary>
/// Lukas, 2019/1/15
/// </summary>
namespace PhoenixCI.FormUI.Prefix6 {
    /// <summary>
    /// 60420 指數成份股每日檢核結果查詢
    /// </summary>
    public partial class W60420 : FormParent {

        private D60420 dao60420;
        private COD daoCOD;

        public W60420(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            base.Open();


            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            daoCOD = new COD();
            base.AfterOpen();

            txtStartDate.EditValue = GlobalInfo.OCF_DATE.Year + "/01/01";
            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
            //設定 dw_index
            DataTable dwSource = daoCOD.ListByCol(_ProgramID, "PID-IDSTK", "全部", "%");
            Extension.SetDataTable(dw_index,dwSource, "COD_SEQ_NO", "COD_DESC",TextEditStyles.DisableTextEditor,"");
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

            #region 檢查
            //要先call f_get_jsw
            string rtn = "";
            rtn = PbFunc.f_get_jsw("60420","E", GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd"));
            //ls_rtn = f_get_jsw(is_txn_id,'E',em_date.text)
            if (rtn != "Y") {
                DialogResult result = MessageBox.Show(txtEndDate.Text + " 統計資料未轉入完畢,是否要繼續?",
                                                      "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) {
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }
            }
            #endregion
            dao60420 = new D60420();
            base.Export();
            lblProcessing.Visible = true;
            this.Refresh();
            Thread.Sleep(5);
            //複製檔案
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);
            //開啟檔案
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            #region wf_60410a (sheet 1/4/5)
            string rptName, rptId;
            int i, j, rowNum;
            decimal value;
            /*************************************
            ls_rpt_name = 報表名稱
            ls_rpt_id = 報表代號
            *************************************/
            rptName = "檢查表";
            rptId = "60410_1a";
            lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";
            this.Refresh();
            Thread.Sleep(5);
            //讀取資料
            DataTable dt60410_1a = dao60420.d_60410_1a(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue);
            if (dt60410_1a.Rows.Count == 0) {
                MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtEndDate.EditValue, rptId + "－" + rptName));
            }

            //切換Sheet
            //1. 未符合「成份股檔數≧10」
            Worksheet sheet1 = workbook.Worksheets[0];

            //填資料
            if (cbx1.Checked == false) {
                sheet1.Cells[2, 0].Value = "標準1. 全部「成份股檔數」";
            }
            rowNum = 3;
            for (i = 0; i < dt60410_1a.Rows.Count; i++) {
                DataRow dr60410_1a = dt60410_1a.Rows[i];
                if (cbx1.Checked == false || dr60410_1a["tot_cnt"].AsInt() < sleCond1.Text.AsInt()) {
                    rowNum = rowNum + 1;
                    sheet1.Cells[rowNum, 0].Value = dr60410_1a["cod_name"].AsString();
                    sheet1.Cells[rowNum, 1].Value = (DateTime.ParseExact(dr60410_1a["ymd"].AsString(), "yyyyMMdd", CultureInfo.InvariantCulture)).ToString("yyyy/M/d").AsDateTime();
                    sheet1.Cells[rowNum, 2].Value = dr60410_1a["tot_cnt"].AsInt();
                }
            }
            sheet1.ScrollTo(0, 0);

            //4. 未符合「最低25%權重之成份股，檔數在15檔(含)以上，過去半年每日合計成交值之平均值＞3,000萬美元」
            Worksheet sheet4 = workbook.Worksheets[3];
            //填資料
            if (cbx4.Checked == false) {
                sheet4.Cells[2, 0].Value = "標準4. 全部「最低25%權重之成份股，檔數在15檔(含)以上，過去半年每日合計成交值」";
            }
            rowNum = 4;
            for (i = 0; i < dt60410_1a.Rows.Count; i++) {
                DataRow dr60410_1a = dt60410_1a.Rows[i];
                if (dr60410_1a["cnt25"].AsInt() >= sleCond41.Text.AsInt() &&
                    Math.Round(dr60410_1a["avg_amt_mth_usd"].AsDecimal() / 10000, 0) <= sleCond42.Text.AsDecimal() ||
                    cbx4.Checked == false) {
                    rowNum = rowNum + 1;
                    sheet4.Cells[rowNum, 0].Value = dr60410_1a["cod_name"].AsString();
                    sheet4.Cells[rowNum, 1].Value = (DateTime.ParseExact(dr60410_1a["ymd"].AsString(), "yyyyMMdd", CultureInfo.InvariantCulture)).ToString("yyyy/M/d").AsDateTime();
                    sheet4.Cells[rowNum, 2].Value = dr60410_1a["cnt25"].AsInt();
                    sheet4.Cells[rowNum, 3].Value = dr60410_1a["weight25"].AsDecimal();
                    sheet4.Cells[rowNum, 4].Value = dr60410_1a["avg_amt_cls_usd"].AsDecimal();
                    sheet4.Cells[rowNum, 5].Value = dr60410_1a["avg_amt_cls_tw"].AsDecimal();
                    sheet4.Cells[rowNum, 6].Value = dr60410_1a["avg_amt_mth_usd"].AsDecimal();
                    sheet4.Cells[rowNum, 7].Value = dr60410_1a["avg_amt_mth_tw"].AsDecimal();
                    sheet4.Cells[rowNum, 8].Value = dr60410_1a["day_amt_cls_usd"].AsDecimal();
                    sheet4.Cells[rowNum, 9].Value = dr60410_1a["day_amt_cls_tw"].AsDecimal();
                    sheet4.Cells[rowNum, 10].Value = dr60410_1a["day_amt_mth_usd"].AsDecimal();
                    sheet4.Cells[rowNum, 11].Value = dr60410_1a["day_amt_mth_tw"].AsDecimal();
                }
            }
            sheet4.ScrollTo(0, 0);

            //5. 未符合「最低25%權重之成份股，檔數低於15檔，過去半年每日合計成交值之平均值＞5,000萬美元」
            Worksheet sheet5 = workbook.Worksheets[4];
            //填資料
            if (cbx5.Checked == false) {
                sheet5.Cells[2, 0].Value = "標準5. 全部「最低25%權重之成份股，檔數低於15檔，過去半年每日合計成交值」";
            }
            rowNum = 4;
            for (i = 0; i < dt60410_1a.Rows.Count; i++) {
                DataRow dr60410_1a = dt60410_1a.Rows[i];
                if (dr60410_1a["cnt25"].AsInt() < sleCond41.Text.AsInt() &&
                    (Math.Round(dr60410_1a["avg_amt_mth_usd"].AsDecimal() / 10000, 0) <= sleCond52.Text.AsDecimal() ||
                    cbx5.Checked == false)) {
                    rowNum = rowNum + 1;
                    sheet5.Cells[rowNum, 0].Value = dr60410_1a["cod_name"].AsString();
                    sheet5.Cells[rowNum, 1].Value = (DateTime.ParseExact(dr60410_1a["ymd"].AsString(), "yyyyMMdd", CultureInfo.InvariantCulture)).ToString("yyyy/M/d").AsDateTime();
                    sheet5.Cells[rowNum, 2].Value = dr60410_1a["cnt25"].AsInt();
                    sheet5.Cells[rowNum, 3].Value = dr60410_1a["weight25"].AsDecimal();
                    sheet5.Cells[rowNum, 4].Value = dr60410_1a["avg_amt_cls_usd"].AsDecimal();
                    sheet5.Cells[rowNum, 5].Value = dr60410_1a["avg_amt_cls_tw"].AsDecimal();
                    sheet5.Cells[rowNum, 6].Value = dr60410_1a["avg_amt_mth_usd"].AsDecimal();
                    sheet5.Cells[rowNum, 7].Value = dr60410_1a["avg_amt_mth_tw"].AsDecimal();
                    sheet5.Cells[rowNum, 8].Value = dr60410_1a["day_amt_cls_usd"].AsDecimal();
                    sheet5.Cells[rowNum, 9].Value = dr60410_1a["day_amt_cls_tw"].AsDecimal();
                    sheet5.Cells[rowNum, 10].Value = dr60410_1a["day_amt_mth_usd"].AsDecimal();
                    sheet5.Cells[rowNum, 11].Value = dr60410_1a["day_amt_mth_tw"].AsDecimal();
                }
            }
            sheet5.ScrollTo(0, 0);
            #endregion

            #region wf_60410_2 (sheet 2)
            rptName = "2. 未符合「權重最大之成份股權重≦30%」";
            rptId = "60410_2";
            lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";
            this.Refresh();
            Thread.Sleep(5);
            //讀取資料
            DataTable dt60410_2;
            if (cbx2.Checked == true) {
                dt60410_2 = dao60420.d_60410_2(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, sleCond2.Text.AsDecimal());
            }
            else {
                dt60410_2 = dao60420.d_60410_2(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, -1.AsDecimal());
            }
            if (dt60410_2.Rows.Count == 0) {
                //PB把這段註解掉，先照著寫
                //MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtEndDate.EditValue, ls_rpt_id + "－" + ls_rpt_name));
            }

            //切換Sheet
            //2. 未符合「權重最大之成份股權重≦30%」
            Worksheet sheet2 = workbook.Worksheets[1];
            //填資料
            if (cbx2.Checked == false) {
                sheet2.Cells[2, 0].Value = "標準2. 全部「權重最大之成份股權重%」";
            }
            rowNum = 3;
            for (i = 0; i < dt60410_2.Rows.Count; i++) {
                DataRow dr60410_2 = dt60410_2.Rows[i];
                if (cbx2.Checked == true && dr60410_2["index_weight"].AsDecimal() <= 0.3.AsDecimal()) {
                    continue;
                }
                rowNum = rowNum + 1;
                sheet2.Cells[rowNum, 0].Value = dr60410_2["cod_name"].AsString();
                sheet2.Cells[rowNum, 1].Value = dr60410_2["TSE3_YMD"].AsDateTime().ToString("yyyy/M/d").AsDateTime();
                sheet2.Cells[rowNum, 2].Value = dr60410_2["TSE3_SID"].AsInt();
                sheet2.Cells[rowNum, 3].Value = dr60410_2["TFXMS_SNAME"].AsString();
                sheet2.Cells[rowNum, 4].Value = dr60410_2["INDEX_WEIGHT"].AsDecimal();
            }
            sheet2.ScrollTo(0, 0);
            #endregion

            #region wf_60410_3 (sheet 3)
            rptName = "3. 未符合「權重前五大成份股合計權重≦60%」";
            rptId = "60412_3"; //PB就是這樣寫
            lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";
            this.Refresh();
            Thread.Sleep(5);
            //讀取資料
            DataTable dt60412_3;
            if (cbx3.Checked == true) {
                dt60412_3 = dao60420.d_60412_3(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, sleCond3.Text.AsDecimal());
            }
            else {
                dt60412_3 = dao60420.d_60412_3(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, -1.AsDecimal());
            }
            if (dt60412_3.Rows.Count == 0) {
                //PB把這段註解掉，先照著寫
                //MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtEndDate.EditValue, ls_rpt_id + "－" + ls_rpt_name));
            }

            //切換Sheet
            //3. 未符合「權重前五大成份股合計權重≦60%」
            Worksheet sheet3 = workbook.Worksheets[2];
            //填資料
            rowNum = 3;
            for (i = 0; i < dt60412_3.Rows.Count; i++) {
                DataRow dr60412_3 = dt60412_3.Rows[i];
                rowNum = rowNum + 1;
                sheet3.Cells[rowNum, 0].Value = dr60412_3["cod_name"].AsString();
                sheet3.Cells[rowNum, 1].Value = dr60412_3["TSE3_DATE"].AsDateTime().ToString("yyyy/M/d").AsDateTime();
                sheet3.Cells[rowNum, 2].Value = dr60412_3["TSE5_25_WEIGHT"].AsDecimal();
                sheet3.Cells[rowNum, 3].Value = dr60412_3["TSE3_DESC_SEQ"].AsInt();
                sheet3.Cells[rowNum, 4].Value = dr60412_3["TSE3_SID"].AsInt();
                sheet3.Cells[rowNum, 5].Value = dr60412_3["TFXMS_SNAME"].AsString();
                sheet3.Cells[rowNum, 6].Value = dr60412_3["INDEX_WEIGHT"].AsDecimal();
            }
            sheet3.ScrollTo(0, 0);
            #endregion
            //預設打開第一張sheet
            workbook.Worksheets.ActiveWorksheet = workbook.Worksheets[0];
            workbook.SaveDocument(excelDestinationPath);
            lblProcessing.Visible = false;
            this.Refresh();
            Thread.Sleep(5);
            return ResultStatus.Success;
        }
    }
}