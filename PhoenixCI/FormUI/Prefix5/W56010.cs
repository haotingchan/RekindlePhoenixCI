using System;
using System.Data;
using System.Windows.Forms;
using BaseGround;
using DataObjects.Dao.Together;
using Common;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DevExpress.XtraEditors.Controls;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System.IO;
/// <summary>
/// Lukas, 2019/1/3
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 56010 結算服務費收費明細表
    /// 有寫到的功能：Open, Export
    /// </summary>
    public partial class W56010 : FormParent {

        private ABRK daoABRK;
        private D56010 dao56010;

        public W56010(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtFromMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            txtToMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            daoABRK = new ABRK();
            dao56010 = new D56010();
        }

        public override ResultStatus BeforeOpen() {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open() {
            base.Open();

            #region 處理下拉選單

            dwSbrkno.Properties.DataSource = daoABRK.ListAll2();
            dwSbrkno.Properties.ValueMember = "ABRK_NO";
            dwSbrkno.Properties.DisplayMember = "CP_DISPLAY";
            dwSbrkno.Properties.ShowHeader = false;
            dwSbrkno.Properties.ShowFooter = false;
            dwSbrkno.Properties.NullText = "";
            dwSbrkno.Properties.SearchMode = SearchMode.OnlyInPopup;
            dwSbrkno.Properties.TextEditStyle = TextEditStyles.Standard;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol = dwSbrkno.Properties.Columns;
            singleCol.Add(new LookUpColumnInfo("CP_DISPLAY"));
            dwSbrkno.Properties.BestFitMode = BestFitMode.BestFitResizePopup;

            dwEbrkno.Properties.DataSource = daoABRK.ListAll2();
            dwEbrkno.Properties.ValueMember = "ABRK_NO";
            dwEbrkno.Properties.DisplayMember = "CP_DISPLAY";
            dwEbrkno.Properties.ShowHeader = false;
            dwEbrkno.Properties.ShowFooter = false;
            dwEbrkno.Properties.NullText = "";
            dwEbrkno.Properties.SearchMode = SearchMode.OnlyInPopup;
            dwEbrkno.Properties.TextEditStyle = TextEditStyles.Standard;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol2 = dwEbrkno.Properties.Columns;
            singleCol2.Add(new LookUpColumnInfo("CP_DISPLAY"));
            dwEbrkno.Properties.BestFitMode = BestFitMode.BestFitResizePopup;

            dwProdCond.Properties.DataSource = dao56010.dw_prod_cond();
            dwProdCond.Properties.ValueMember = "PARAM_KEY";
            dwProdCond.Properties.DisplayMember = "CP_DISPLAY";
            dwProdCond.Properties.ShowHeader = false;
            dwProdCond.Properties.ShowFooter = false;
            dwProdCond.Properties.NullText = "";
            dwProdCond.Properties.SearchMode = SearchMode.OnlyInPopup;
            dwProdCond.Properties.TextEditStyle = TextEditStyles.Standard;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol3 = dwProdCond.Properties.Columns;
            singleCol3.Add(new LookUpColumnInfo("CP_DISPLAY"));
            dwProdCond.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            dwProdCond.EditValue = "全部";

            #endregion

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Export() {

            #region 檢查
            //期貨商後面號碼不能小於前面號碼
            //PB可以用字串直接比較但打內不行，只好用Index來比大小
            string isSbrkno, isEbrkno;
            isSbrkno = dwSbrkno.ItemIndex.ToString();
            if (isSbrkno == null) {
                isSbrkno = "";
            }
            isEbrkno = dwEbrkno.ItemIndex.ToString();
            if (isEbrkno == null) {
                isEbrkno = "";
            }
            if (int.Parse(isSbrkno) > int.Parse(isEbrkno) && isSbrkno != "") {
                MessageBox.Show("造市者代號起始不可大於迄止!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return ResultStatus.Fail;
            }
            #endregion

            base.Export();
            lblProcessing.Visible = true;
            //依期貨商別或依商品別 輸出不同的Excel
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);
            if (rdoGroup.EditValue.ToString() == "True") {
                //讀取資料
                string asSym = txtFromMonth.Text.Replace("/", "");
                string asEym = txtToMonth.Text.Replace("/", "");
                string startFcmNo = dwSbrkno.Text.Trim();
                string endFcmNo = dwEbrkno.Text.Trim();
                string prodType = dwProdCond.Text.Trim();
                string rptName = "交易經手費收費明細表－依期貨商別";

                DataTable dt56011 = dao56010.D56011(asSym, asEym, prodType, startFcmNo, endFcmNo);
                if (dt56011.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtToMonth.Text.Replace("/", ""), rptName));
                    File.Delete(excelDestinationPath);
                    return ResultStatus.Fail;
                }
                wf_56011(excelDestinationPath, dt56011);
            }
            else {
                //讀取資料
                string asSym = txtFromMonth.Text.Replace("/", "");
                string asEym = txtToMonth.Text.Replace("/", "");
                string startFcmNo = dwSbrkno.Text.Trim();
                string endFcmNo = dwEbrkno.Text.Trim();
                string rptName = "交易經手費收費明細表－依商品別";

                DataTable dt56012 = dao56010.D56012(asSym, asEym, startFcmNo, endFcmNo);
                if (dt56012.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtToMonth.Text.Replace("/", ""), rptName));
                    File.Delete(excelDestinationPath);
                    return ResultStatus.Fail;
                }
                wf_56012(excelDestinationPath, dt56012);
            }
            lblProcessing.Visible = false;
            return ResultStatus.Success;
        }

        private void wf_56011(string excelDestinationPath, DataTable dt56011) {

            try {
                string rptName, rptId, session;
                int i, rowTol, datacount;
                long found;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = Excel的Column位置
                li_ole_row_tol = Excel的Column預留數
                li_datacount = 資料空白列筆數
                *************************************/
                rptName = "交易經手費收費明細表－依期貨商別";
                rptId = "56011";
                session = "0";

                string asSym = txtFromMonth.Text.Replace("/", "");
                string asEym = txtToMonth.Text.Replace("/", "");

                //切換sheet
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet ws56011 = workbook.Worksheets[0];

                //填資料
                int rowNum = 6;
                datacount = int.Parse(ws56011.Cells[0, 0].Value.ToString());
                if (datacount == null || datacount == 0) {
                    datacount = dt56011.Rows.Count;
                }
                rowTol = rowNum + datacount;
                ws56011.Cells[3, 0].Value = ws56011.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                ws56011.Cells[3, 3].Value = ws56011.Cells[3, 3].Value + asSym;
                ws56011.Cells[3, 6].Value = ws56011.Cells[3, 6].Value + asEym;

                for (i = 0; i < dt56011.Rows.Count; i++) {
                    DataRow dr56011 = dt56011.Rows[i];
                    rowNum = rowNum + 1;
                    ws56011.Cells[rowNum, 0].Value = dr56011["feetdcc_fcm_no"].AsString();
                    ws56011.Cells[rowNum, 1].Value = dr56011["brk_abbr_name"].AsString();
                    ws56011.Cells[rowNum, 2].Value = dr56011["feetdcc_acc_no"].AsString();
                    ws56011.Cells[rowNum, 3].Value = dr56011["feetdcc_kind_id"].AsString();
                    ws56011.Cells[rowNum, 4].SetValue(dr56011["feetrd_m_qnty"]);
                    ws56011.Cells[rowNum, 5].SetValue(dr56011["feetdcc_org_ar"]);
                    ws56011.Cells[rowNum, 6].SetValue(dr56011["feetdcc_disc_amt"]);
                }

                //刪除空白列
                if (rowTol > rowNum) {
                    ws56011.Rows.Remove(rowNum + 1, rowTol - rowNum);
                }
                ws56011.ScrollToRow(0);

                //存檔
                workbook.SaveDocument(excelDestinationPath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }

        private void wf_56012(string excelDestinationPath, DataTable dt56012) {

            try {
                string rptName, rptId, session;
                int i, rowTol, datacount;
                long found;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = Excel的Column位置
                li_ole_row_tol = Excel的Column預留數
                li_datacount = 資料空白列筆數
                *************************************/
                rptName = "交易經手費收費明細表－依商品別";
                rptId = "56012";
                session = "0";

                string asSym = txtFromMonth.Text.Replace("/", "");
                string asEym = txtToMonth.Text.Replace("/", "");

                //切換Sheet
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet ws56012 = workbook.Worksheets[1];

                //填資料
                int rowNum = 6;
                datacount = int.Parse(ws56012.Cells[0, 0].Value.ToString());
                if (datacount == null || datacount == 0) {
                    datacount = dt56012.Rows.Count;
                }
                rowTol = rowNum + datacount;
                ws56012.Cells[3, 0].Value = ws56012.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                ws56012.Cells[3, 3].Value = ws56012.Cells[3, 3].Value + asSym;
                ws56012.Cells[3, 6].Value = ws56012.Cells[3, 6].Value + asEym;

                for (i = 0; i < dt56012.Rows.Count; i++) {
                    DataRow dr56012 = dt56012.Rows[i];
                    rowNum = rowNum + 1;
                    ws56012.Cells[rowNum, 0].Value = dr56012["feetdcc_kind_id"].AsString();
                    ws56012.Cells[rowNum, 1].SetValue(dr56012["feetrd_m_qnty"]);
                    ws56012.Cells[rowNum, 2].SetValue(dr56012["feetdcc_org_ar"]);
                    ws56012.Cells[rowNum, 3].SetValue(dr56012["feetdcc_disc_amt"]);
                }

                //刪除空白列
                if (rowTol > rowNum) {
                    ws56012.Rows.Remove(rowNum + 1, rowTol - rowNum);
                }
                ws56012.ScrollToRow(0);

                //存檔
                workbook.SaveDocument(excelDestinationPath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            base.Print(reportHelper);

            return ResultStatus.Success;
        }

        private void rdoGroup_Properties_EditValueChanged(object sender, EventArgs e) {
            if (rdoGroup.EditValue.ToString() == "True") {
                lblCondition.Visible = true;
                dwProdCond.Visible = true;
            }
            else {
                lblCondition.Visible = false;
                dwProdCond.Visible = false;
            }
        }
    }
}