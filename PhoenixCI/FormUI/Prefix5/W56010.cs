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
using DataObjects.Dao.Together;
using Common;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DevExpress.XtraEditors.Controls;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
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

            dw_sbrkno.Properties.DataSource = daoABRK.ListAll2();
            dw_sbrkno.Properties.ValueMember = "ABRK_NO";
            dw_sbrkno.Properties.DisplayMember = "CP_DISPLAY";
            dw_sbrkno.Properties.ShowHeader = false;
            dw_sbrkno.Properties.ShowFooter = false;
            dw_sbrkno.Properties.NullText = "";
            dw_sbrkno.Properties.SearchMode = SearchMode.OnlyInPopup;
            dw_sbrkno.Properties.TextEditStyle = TextEditStyles.Standard;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol = dw_sbrkno.Properties.Columns;
            singleCol.Add(new LookUpColumnInfo("CP_DISPLAY"));
            dw_sbrkno.Properties.BestFitMode = BestFitMode.BestFitResizePopup;

            dw_ebrkno.Properties.DataSource = daoABRK.ListAll2();
            dw_ebrkno.Properties.ValueMember = "ABRK_NO";
            dw_ebrkno.Properties.DisplayMember = "CP_DISPLAY";
            dw_ebrkno.Properties.ShowHeader = false;
            dw_ebrkno.Properties.ShowFooter = false;
            dw_ebrkno.Properties.NullText = "";
            dw_ebrkno.Properties.SearchMode = SearchMode.OnlyInPopup;
            dw_ebrkno.Properties.TextEditStyle = TextEditStyles.Standard;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol2 = dw_ebrkno.Properties.Columns;
            singleCol2.Add(new LookUpColumnInfo("CP_DISPLAY"));
            dw_ebrkno.Properties.BestFitMode = BestFitMode.BestFitResizePopup;

            dw_prod_cond.Properties.DataSource = dao56010.dw_prod_cond();
            dw_prod_cond.Properties.ValueMember = "PARAM_KEY";
            dw_prod_cond.Properties.DisplayMember = "CP_DISPLAY";
            dw_prod_cond.Properties.ShowHeader = false;
            dw_prod_cond.Properties.ShowFooter = false;
            dw_prod_cond.Properties.NullText = "";
            dw_prod_cond.Properties.SearchMode = SearchMode.OnlyInPopup;
            dw_prod_cond.Properties.TextEditStyle = TextEditStyles.Standard;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol3 = dw_prod_cond.Properties.Columns;
            singleCol3.Add(new LookUpColumnInfo("CP_DISPLAY"));
            dw_prod_cond.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            dw_prod_cond.EditValue = "全部";

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

        protected override ResultStatus Retrieve() {
            base.Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield() {
            base.CheckShield();

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            base.Save(pokeBall);

            return ResultStatus.Success;
        }

        protected override ResultStatus Run(PokeBall args) {
            base.Run(args);

            return ResultStatus.Success;
        }

        protected override ResultStatus Import() {
            base.Import();

            return ResultStatus.Success;
        }

        protected override ResultStatus Export() {

            #region 檢查
            //期貨商後面號碼不能小於前面號碼
            //PB可以用字串直接比較但打內不行，只好用Index來比大小
            string is_sbrkno, is_ebrkno;
            is_sbrkno = dw_sbrkno.ItemIndex.ToString();
            if (is_sbrkno == null) {
                is_sbrkno = "";
            }
            is_ebrkno = dw_ebrkno.ItemIndex.ToString();
            if (is_ebrkno == null) {
                is_ebrkno = "";
            }
            if (int.Parse(is_sbrkno) > int.Parse(is_ebrkno) && is_sbrkno != "") {
                MessageBox.Show("造市者代號起始不可大於迄止!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return ResultStatus.Fail;
            }
            #endregion

            base.Export();
            lblProcessing.Visible = true;
            //依期貨商別或依商品別 輸出不同的Excel
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);
            if (rdoGroup.EditValue.ToString() == "True") {
                wf_56011(excelDestinationPath);
            }
            else {
                wf_56012(excelDestinationPath);
            }
            lblProcessing.Visible = false;
            return ResultStatus.Success;
        }

        private void wf_56011(string excelDestinationPath) {

            try {
                string ls_rpt_name, ls_rpt_id, ls_session;
                int i, li_ole_row_tol, li_datacount;
                long ll_found;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = Excel的Column位置
                li_ole_row_tol = Excel的Column預留數
                li_datacount = 資料空白列筆數
                *************************************/
                ls_rpt_name = "交易經手費收費明細表－依期貨商別";
                ls_rpt_id = "56011";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';
                ls_session = "0";

                //讀取資料
                string as_sym = txtFromMonth.Text.Replace("/", "");
                string as_eym = txtToMonth.Text.Replace("/", "");
                string startFcmNo = dw_sbrkno.EditValue.ToString().Trim();
                string endFcmNo = dw_ebrkno.EditValue.ToString().Trim();
                string prodType = dw_prod_cond.EditValue.ToString().Trim();

                DataTable dt56011 = dao56010.D56011(as_sym, as_eym, prodType, startFcmNo, endFcmNo);
                if (dt56011.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtToMonth.Text.Replace("/", ""), ls_rpt_name));
                }

                //切換sheet
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet ws56011 = workbook.Worksheets[0];

                //填資料
                int ii_ole_row = 6;
                li_datacount = int.Parse(ws56011.Cells[0, 0].Value.ToString());
                if (li_datacount == null || li_datacount == 0) {
                    li_datacount = dt56011.Rows.Count;
                }
                li_ole_row_tol = ii_ole_row + li_datacount;
                ws56011.Cells[3, 0].Value = ws56011.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                ws56011.Cells[3, 3].Value = ws56011.Cells[3, 3].Value + as_sym;
                ws56011.Cells[3, 6].Value = ws56011.Cells[3, 6].Value + as_eym;

                for (i = 0; i < dt56011.Rows.Count; i++) {
                    DataRow dr56011 = dt56011.Rows[i];
                    ii_ole_row = ii_ole_row + 1;
                    ws56011.Cells[ii_ole_row, 0].Value = dr56011["feetdcc_fcm_no"].AsString();
                    ws56011.Cells[ii_ole_row, 1].Value = dr56011["brk_abbr_name"].AsString();
                    ws56011.Cells[ii_ole_row, 2].Value = dr56011["feetdcc_acc_no"].AsString();
                    ws56011.Cells[ii_ole_row, 3].Value = dr56011["feetdcc_kind_id"].AsString();
                    ws56011.Cells[ii_ole_row, 4].Value = dr56011["feetrd_m_qnty"].AsDecimal();
                    ws56011.Cells[ii_ole_row, 5].Value = dr56011["feetdcc_org_ar"].AsDecimal();
                    ws56011.Cells[ii_ole_row, 6].Value = dr56011["feetdcc_disc_amt"].AsDecimal();
                }

                //刪除空白列
                if (li_ole_row_tol > ii_ole_row) {
                    ws56011.Rows.Remove(ii_ole_row + 1, li_ole_row_tol - ii_ole_row);
                }
                ws56011.ScrollToRow(0);

                //存檔
                workbook.SaveDocument(excelDestinationPath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }

        private void wf_56012(string excelDestinationPath) {

            try {
                string ls_rpt_name, ls_rpt_id, ls_session;
                int i, li_ole_row_tol, li_datacount;
                long ll_found;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                li_ole_col = Excel的Column位置
                li_ole_row_tol = Excel的Column預留數
                li_datacount = 資料空白列筆數
                *************************************/
                ls_rpt_name = "交易經手費收費明細表－依商品別";
                ls_rpt_id = "56012";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';
                ls_session = "0";

                //讀取資料
                string as_sym = txtFromMonth.Text.Replace("/", "");
                string as_eym = txtToMonth.Text.Replace("/", "");
                string startFcmNo = dw_sbrkno.EditValue.ToString().Trim();
                string endFcmNo = dw_ebrkno.EditValue.ToString().Trim();

                DataTable dt56012 = dao56010.D56012(as_sym, as_eym, startFcmNo, endFcmNo);
                if (dt56012.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtToMonth.Text.Replace("/", ""), ls_rpt_name));
                }

                //切換Sheet
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet ws56012 = workbook.Worksheets[1];

                //填資料
                int ii_ole_row = 6;
                li_datacount = int.Parse(ws56012.Cells[0, 0].Value.ToString());
                if (li_datacount == null || li_datacount == 0) {
                    li_datacount = dt56012.Rows.Count;
                }
                li_ole_row_tol = ii_ole_row + li_datacount;
                ws56012.Cells[3, 0].Value = ws56012.Cells[3, 0].Value + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                ws56012.Cells[3, 3].Value = ws56012.Cells[3, 3].Value + as_sym;
                ws56012.Cells[3, 6].Value = ws56012.Cells[3, 6].Value + as_eym;

                for (i = 0; i < dt56012.Rows.Count; i++) {
                    DataRow dr56012 = dt56012.Rows[i];
                    ii_ole_row = ii_ole_row + 1;
                    ws56012.Cells[ii_ole_row, 0].Value = dr56012["feetdcc_kind_id"].AsString();
                    ws56012.Cells[ii_ole_row, 1].Value = dr56012["feetrd_m_qnty"].AsDecimal();
                    ws56012.Cells[ii_ole_row, 2].Value = dr56012["feetdcc_org_ar"].AsDecimal();
                    ws56012.Cells[ii_ole_row, 3].Value = dr56012["feetdcc_disc_amt"].AsDecimal();
                }

                //刪除空白列
                if (li_ole_row_tol > ii_ole_row) {
                    ws56012.Rows.Remove(ii_ole_row + 1, li_ole_row_tol - ii_ole_row);
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

        protected override ResultStatus InsertRow() {
            base.InsertRow();

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow();

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose() {
            return base.BeforeClose();
        }

        private void rdoGroup_Properties_EditValueChanged(object sender, EventArgs e) {
            if (rdoGroup.EditValue.ToString() == "True") {
                lblCondition.Visible = true;
                dw_prod_cond.Visible = true;
            }
            else {
                lblCondition.Visible = false;
                dw_prod_cond.Visible = false;
            }
        }
    }
}