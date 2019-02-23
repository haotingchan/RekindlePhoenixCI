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
using Common;
using BusinessObjects.Enums;
using BusinessObjects;
using BaseGround.Report;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
/// <summary>
/// Lukas, 2018/12/27
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 55060 CME美國道瓊及標普500期貨授權費表
    /// 有寫到的功能：Export
    /// </summary>
    public partial class W55060 : FormParent {

        private D55060 dao55060;

        public W55060(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            dao55060 = new D55060();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtFromMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            txtToMonth.DateTimeValue = GlobalInfo.OCF_DATE;
        }


        public override ResultStatus BeforeOpen() {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open() {
            base.Open();

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
            if (txtFromMonth.Text.SubStr(0, 4) != txtToMonth.Text.SubStr(0, 4)) {
                MessageBox.Show("不可跨年度查詢!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return ResultStatus.Fail;
            }
            base.Export();
            lblProcessing.Visible = true;
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);
            string excelDestinationPath_Detail = CopyExcelTemplateFile(_ProgramID + "MM", FileType.XLS);
            ManipulateExcel(excelDestinationPath);
            ManipulateExcel_Detail(excelDestinationPath_Detail);
            /**********************
            轉檔後資訊
            **********************/
            string ls_ym;
            ls_ym = txtToMonth.Text.Replace("/", "");
            DataTable dt_55060_after_export = dao55060.d_55060_after_export(ls_ym);
            if (dt_55060_after_export.Rows[0]["ld_disc_qnty"].AsString() == "0") {
                MessageBox.Show(ls_ym + "「結算手續費」的可折抵口數皆為０，"+ Environment.NewLine +"請確認結算手續費作業是否已完成！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Success;
            }
            lblProcessing.Visible = false;
            return ResultStatus.Success;
        }

        private void ManipulateExcel(string excelDestinationPath) {

           
            try {
                #region wf_55060_1
                string ls_rpt_name, ls_rpt_id;
                int i;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                *************************************/
                ls_rpt_name = "交易量(單邊)";
                ls_rpt_id = "55060_1";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';

                /******************
                讀取資料
                ******************/
                string as_symd = txtFromMonth.Text.Replace("/", "")+"01";
                string as_eymd = txtToMonth.Text.Replace("/", "")+"31";
                DataTable dt55060_1 = dao55060.d_55060_1(as_symd, as_eymd);
                if (dt55060_1.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromMonth.Text + "-" + txtToMonth.Text, ls_rpt_name));
                }

                /******************
                切換Sheet
                ******************/
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet worksheet = workbook.Worksheets[1];

                int ii_ole_row = 4;
                for (i = 0; i < dt55060_1.Rows.Count; i++) {
                    DataRow dr55060_1 = dt55060_1.Rows[i];

                    ii_ole_row = ii_ole_row + 1;
                    worksheet.Cells[ii_ole_row, 0].Value = dr55060_1["data_date"].AsString();
                    worksheet.Cells[ii_ole_row, 1].Value = dr55060_1["udf_qnty"].AsDecimal();
                    worksheet.Cells[ii_ole_row, 2].Value = dr55060_1["spf_qnty"].AsDecimal();
                }

                #endregion

                #region wf_55060_2
                //string ls_rpt_name, ls_rpt_id;
                //int i;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                *************************************/
                ls_rpt_name = "到期結算OI";
                ls_rpt_id = "55060_2";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';

                /******************
                讀取資料
                ******************/
                //計算月底日期
                DateTime ldt_date;
                ldt_date = Convert.ToDateTime(txtToMonth.Text + "/01").AddDays(31);
                ldt_date = ldt_date.AddDays(ldt_date.Day * -1);
                string as_sdate = Convert.ToDateTime(txtFromMonth.Text + "/01").ToString("yyyy/M/d tt hh:mm:ss");
                string as_edate = ldt_date.ToString("yyyy/M/d tt hh:mm:ss");

                DataTable dt55060_2 = dao55060.d_55060_2(as_sdate, as_edate);
                if (dt55060_2.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromMonth.Text + "-" + txtToMonth.Text, ls_rpt_name));
                }

                /******************
                切換Sheet
                ******************/
                Worksheet worksheet2 = workbook.Worksheets[2];

                //填資料
                ii_ole_row = 4;
                for (i = 0; i < dt55060_2.Rows.Count; i++) {
                    DataRow dr55060_2 = dt55060_2.Rows[i];

                    ii_ole_row = ii_ole_row + 1;
                    worksheet2.Cells[ii_ole_row, 0].Value = dr55060_2["data_date"].AsString();
                    worksheet2.Cells[ii_ole_row, 1].Value = dr55060_2["spf_oi"].AsDecimal();
                    worksheet2.Cells[ii_ole_row, 2].Value = dr55060_2["udf_oi"].AsDecimal();
                }

                #endregion

                #region wf_55060_3
                string ls_kind_id;
                int j, li_add_col, li_num;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                *************************************/
                ls_rpt_name = "造市折減";
                ls_rpt_id = "55060_3";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';


                /******************
                讀取資料
                ******************/
                string as_sym = txtFromMonth.Text.Replace("/", "");
                string as_eym = txtToMonth.Text.Replace("/", "");
                DataTable dt55060_3 = dao55060.d_55060_3(as_sym, as_eym);
                if (dt55060_3.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromMonth.Text + "-" + txtToMonth.Text, ls_rpt_name));
                }

                /******************
                切換Sheet
                ******************/
                Worksheet worksheet3 = workbook.Worksheets[3];

                //填資料
                ls_kind_id = "";
                li_num = 0;
                li_add_col = 0;
                for (i = 0; i < dt55060_3.Rows.Count; i++) {
                    DataRow dr55060_3 = dt55060_3.Rows[i];
                    if (ls_kind_id != dr55060_3["kind_id"].AsString().Trim()) {
                        ii_ole_row = 6;
                        ls_kind_id = dr55060_3["kind_id"].AsString().Trim();
                        if (ls_kind_id == "UDF") {
                            li_add_col = 8;
                        }
                        else {
                            li_add_col = 0;
                        }
                        li_num = 0;
                    }

                    ii_ole_row = ii_ole_row + 1;
                    li_num = li_num + 1;
                    worksheet3.Cells[ii_ole_row, 0 + li_add_col].Value = dr55060_3["data_ym"].AsString();
                    worksheet3.Cells[ii_ole_row, 1 + li_add_col].Value = li_num;
                    worksheet3.Cells[ii_ole_row, 2 + li_add_col].Value = ls_kind_id;
                    worksheet3.Cells[ii_ole_row, 3 + li_add_col].Value = dr55060_3["trd_ar_amt"].AsDecimal();
                    worksheet3.Cells[ii_ole_row, 4 + li_add_col].Value = dr55060_3["trd_rec_amt"].AsDecimal();
                    worksheet3.Cells[ii_ole_row, 5 + li_add_col].Value = dr55060_3["cm_ar_amt"].AsDecimal();
                    worksheet3.Cells[ii_ole_row, 6 + li_add_col].Value = dr55060_3["cm_rec_amt"].AsDecimal();
                }

                #endregion

                //存檔
                workbook.SaveDocument(excelDestinationPath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void ManipulateExcel_Detail(string excelDestinationPath) {

            
            try {
                #region wf_55060_3_trd
                string ls_rpt_name, ls_rpt_id, ls_kind_id;
                int i, j, li_add_col;
                //long i;
                /*************************************
                ls_rpt_name = 報表名稱
                ls_rpt_id = 報表代號
                *************************************/
                ls_rpt_name = "造市折減";
                ls_rpt_id = "55060_3MM";
                //st_msg_txt.text = ls_rpt_id + '－' + ls_rpt_name + ' 轉檔中...';

                /******************
                讀取資料
                ******************/
                string as_sym = txtFromMonth.Text.Replace("/", "");
                string as_eym = txtToMonth.Text.Replace("/", "");
                DataTable dt55060_3_trd = dao55060.d_55060_3_trd(as_sym, as_eym);
                if (dt55060_3_trd.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromMonth.Text + "-" + txtToMonth.Text, ls_rpt_name));
                }

                //切換Sheet
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                //造市折減(交易經手費)
                Worksheet worksheet = workbook.Worksheets[1];

                //填資料
                ls_kind_id = "";
                int ii_ole_row = 0;
                li_add_col = 0;
                for (i = 0; i < dt55060_3_trd.Rows.Count; i++) {
                    DataRow dr55060_3_trd = dt55060_3_trd.Rows[i];
                    if (ls_kind_id != dr55060_3_trd["feetrd_kind_id"].AsString()) {
                        ii_ole_row = 6;
                        ls_kind_id = dr55060_3_trd["feetrd_kind_id"].AsString();
                        if (dr55060_3_trd["feetrd_kind_id"].AsString() == "UDF") {
                            li_add_col = 14;
                        }
                        else {
                            li_add_col = 0;
                        }
                    }

                    ii_ole_row = ii_ole_row + 1;
                    worksheet.Cells[ii_ole_row, 0 + li_add_col].Value = dr55060_3_trd["feetrd_ym"].AsString();
                    worksheet.Cells[ii_ole_row, 1 + li_add_col].Value = dr55060_3_trd["feetrd_fcm_no"].AsString();
                    worksheet.Cells[ii_ole_row, 2 + li_add_col].Value = dr55060_3_trd["feetrd_kind_id"].AsString();
                    worksheet.Cells[ii_ole_row, 3 + li_add_col].Value = dr55060_3_trd["feetrd_disc_qnty"].AsDecimal();
                    worksheet.Cells[ii_ole_row, 4 + li_add_col].Value = dr55060_3_trd["disc_rate"].AsInt();
                    worksheet.Cells[ii_ole_row, 5 + li_add_col].Value = dr55060_3_trd["feetrd_ar"].AsDecimal();
                    worksheet.Cells[ii_ole_row, 6 + li_add_col].Value = dr55060_3_trd["disc_amt"].AsDecimal();
                    worksheet.Cells[ii_ole_row, 7 + li_add_col].Value = dr55060_3_trd["feetrd_rec_amt"].AsDecimal();
                    worksheet.Cells[ii_ole_row, 8 + li_add_col].Value = dr55060_3_trd["feetrd_m_qnty"].AsDecimal();
                    worksheet.Cells[ii_ole_row, 9 + li_add_col].Value = dr55060_3_trd["feetrd_fcm_kind"].AsString();
                    worksheet.Cells[ii_ole_row, 10 + li_add_col].Value = dr55060_3_trd["feetrd_param_key"].AsString();
                    worksheet.Cells[ii_ole_row, 11 + li_add_col].Value = dr55060_3_trd["feetrd_acc_no"].AsString();
                    worksheet.Cells[ii_ole_row, 12 + li_add_col].Value = dr55060_3_trd["feetrd_session"].AsString();
                    //PB的寫法，但打內沒辦法自動分辨型別
                    //for (j = 0; j < 13; j++) {
                    //    worksheet.Cells[ii_ole_row, j + li_add_col].Value = dr55060_3_trd[j].ToString();
                    //}
                }

                #endregion

                #region wf_55060_3_cm
                //讀取資料
                DataTable dt55060_3_cm = dao55060.d_55060_3_cm(as_sym, as_eym);
                if (dt55060_3_cm.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromMonth.Text + "-" + txtToMonth.Text, ls_rpt_name));
                }

                //切換Sheet
                //造市折減(結算手續費)
                Worksheet worksheet2 = workbook.Worksheets[2];

                //填資料
                ls_kind_id = "";
                for (i = 0; i < dt55060_3_cm.Rows.Count; i++) {
                    DataRow dr55060_3_cm = dt55060_3_cm.Rows[i];
                    if (ls_kind_id != dr55060_3_cm["feetdcc_kind_id"].AsString()) {
                        ii_ole_row = 6;
                        ls_kind_id = dr55060_3_cm["feetdcc_kind_id"].AsString();
                        if (dr55060_3_cm["feetdcc_kind_id"].AsString() == "UDF") {
                            li_add_col = 11;
                        }
                        else {
                            li_add_col = 0;
                        }
                    }

                    ii_ole_row = ii_ole_row + 1;

                    worksheet2.Cells[ii_ole_row, 0 + li_add_col].Value = dr55060_3_cm["feetdcc_ym"].AsString();
                    worksheet2.Cells[ii_ole_row, 1 + li_add_col].Value = dr55060_3_cm["feetdcc_fcm_no"].AsString();
                    worksheet2.Cells[ii_ole_row, 2 + li_add_col].Value = dr55060_3_cm["feetdcc_kind_id"].AsString();
                    worksheet2.Cells[ii_ole_row, 3 + li_add_col].Value = dr55060_3_cm["feetdcc_disc_qnty"].AsDecimal();
                    worksheet2.Cells[ii_ole_row, 4 + li_add_col].Value = dr55060_3_cm["disc_rate"].AsDecimal();
                    worksheet2.Cells[ii_ole_row, 5 + li_add_col].Value = dr55060_3_cm["feetdcc_org_ar"].AsDecimal();
                    worksheet2.Cells[ii_ole_row, 6 + li_add_col].Value = dr55060_3_cm["feetdcc_disc_amt"].AsDecimal();
                    worksheet2.Cells[ii_ole_row, 7 + li_add_col].Value = dr55060_3_cm["rec_amt"].AsDecimal();
                    worksheet2.Cells[ii_ole_row, 8 + li_add_col].Value = dr55060_3_cm["feetdcc_acc_no"].AsString();
                    worksheet2.Cells[ii_ole_row, 9 + li_add_col].Value = dr55060_3_cm["feetdcc_session"].AsString();
                    //for (j = 0; j < 10; j++) {
                    //    worksheet2.Cells[ii_ole_row, j + li_add_col].Value = dr55060_3_cm[j].ToString();
                    //}
                }

                #endregion

                #region wf_55060_3_all
                //讀取資料
                DataTable dt55060_3_all = dao55060.d_55060_3_all(as_sym, as_eym);
                if (dt55060_3_all.Rows.Count == 0) {
                    MessageDisplay.Info(string.Format("{0},{1},無任何資料!", txtFromMonth.Text + "-" + txtToMonth.Text, ls_rpt_name));
                }

                //切換Sheet
                //造市折減(交易+結算)
                Worksheet worksheet3 = workbook.Worksheets[0];

                //填資料
                ls_kind_id = "";
                for (i = 0; i < dt55060_3_all.Rows.Count; i++) {
                    DataRow dr55060_3_all = dt55060_3_all.Rows[i];
                    if (ls_kind_id != dr55060_3_all["feetrd_kind_id"].AsString()) {
                        ii_ole_row = 6;
                        ls_kind_id = dr55060_3_all["feetrd_kind_id"].AsString();
                        if (dr55060_3_all["feetrd_kind_id"].AsString() == "UDF") {
                            li_add_col = 14;
                        }
                        else {
                            li_add_col = 0;
                        }
                    }

                    ii_ole_row = ii_ole_row + 1;
                    worksheet3.Cells[ii_ole_row, 0 + li_add_col].Value = dr55060_3_all["feetrd_feetrd_ym"].AsString();
                    worksheet3.Cells[ii_ole_row, 1 + li_add_col].Value = dr55060_3_all["feetrd_feetrd_fcm_no"].AsString();
                    worksheet3.Cells[ii_ole_row, 2 + li_add_col].Value = dr55060_3_all["feetrd_kind_id"].AsString();
                    worksheet3.Cells[ii_ole_row, 3 + li_add_col].Value = dr55060_3_all["feetrd_feetrd_disc_qnty"].AsDecimal();
                    worksheet3.Cells[ii_ole_row, 4 + li_add_col].Value = dr55060_3_all["disc_rate"].AsId();
                    worksheet3.Cells[ii_ole_row, 5 + li_add_col].Value = dr55060_3_all["ar"].AsDecimal();
                    worksheet3.Cells[ii_ole_row, 6 + li_add_col].Value = dr55060_3_all["disc_amt"].AsDecimal();
                    worksheet3.Cells[ii_ole_row, 7 + li_add_col].Value = dr55060_3_all["rec_amt"].AsDecimal();
                    worksheet3.Cells[ii_ole_row, 8 + li_add_col].Value = dr55060_3_all["feetrd_feetrd_m_qnty"].AsDecimal();
                    worksheet3.Cells[ii_ole_row, 9 + li_add_col].Value = dr55060_3_all["feetrd_feetrd_fcm_kind"].AsString();
                    worksheet3.Cells[ii_ole_row, 10 + li_add_col].Value = dr55060_3_all["feetrd_feetrd_param_key"].AsString();
                    worksheet3.Cells[ii_ole_row, 11 + li_add_col].Value = dr55060_3_all["feetrd_feetrd_acc_no"].AsString();
                    worksheet3.Cells[ii_ole_row, 12 + li_add_col].Value = dr55060_3_all["feetrd_feetrd_session"].AsString();
                    //for (j = 0; j < 13; j++) {
                    //    worksheet3.Cells[ii_ole_row, j + li_add_col].Value = dr55060_3_all[j].ToString();
                    //}
                }

                #endregion

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
    }
}