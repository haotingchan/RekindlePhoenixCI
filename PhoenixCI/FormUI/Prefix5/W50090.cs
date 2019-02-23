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
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.SpecificDao;
using System.IO;
using BaseGround.Shared;
/// <summary>
/// Lukas, 2018/12/19
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    /// <summary>
    /// 50090 當月造市詢報價比例下載
    /// 有寫到的功能：Export
    /// </summary>
    public partial class W50090 : FormParent {

        private D50090 dao50090;

        public W50090(string programID, string programName) : base(programID, programName) {
            dao50090 = new D50090();
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;
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
            base.Export();
            lblProcessing.Visible = true;
            /*******************
            點選儲存檔案之目錄
            *******************/
            //檔名	= 報表型態(起-迄).xls
            string is_save_file = txtMonth.Text.Replace("/", "") + "_MMONTH.txt";
            if (is_save_file == "") {
                return ResultStatus.Fail;
            }
            bool ib_title = false;
            /******************************
            (01).char(6)年月 
            (02).char(7)期貨商代號 
            (03).char(4) 商品  
                    2011.05.03 當股票期貨/股票選擇權時,第3碼補上F/O
            (04).char(8) 詢報價筆 000.0000
            (05).char(7)帳號 
            (06).char(1)期貨符合維持時間(Y/N)
            (07).char(1)判斷符合最低口數(Y/N)
            (08).char(4)維持分鐘數
            (09).char(8)造市績效
                   2011.05.03 新增股票期貨的造市績效
            (10).char(8)不合理交易量 
                   2013.07.30 新增交易部認定不合理交易量欄位
            (11).char(1)交易時段:0日盤/1夜盤
            ******************************/
            int i, li_day_count, li_avg_time;
            string ls_text, ls_prod_type, ls_kind_id;
            decimal ll_time;
            DataTable dtContent = dao50090.GetData(txtMonth.Text.Replace("/", ""));
            //PB的D50090有兩個運算欄位(computed fields)
            //運算條件：if(amm0_day_count > 0 , truncate(IF( amm0_qnty1 >  amm0_qnty2  , amm0_qnty2 , amm0_qnty1 )/  amm0_day_count,0) ,0)
            dtContent.Columns.Add("cp_amm0_qnty", typeof(int));
            //運算條件：if(cp_amm0_qnty >=  mmf_qnty_low , 'Y' ,'N')
            dtContent.Columns.Add("cp_qnty_flag", typeof(string));
            /*******************
            DataWindow Error
            *******************/
            if (dtContent.Rows.Count == 0) {
                MessageBox.Show("轉出筆數為０!", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return ResultStatus.Fail;
            }
            //開檔
            try {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "*.txt (*.txt)|*.txt";
                save.Title = "請點選儲存檔案之目錄";
                save.FileName = is_save_file;
                save.ShowDialog();
                Stream fs = (FileStream)save.OpenFile();
                StreamWriter wr = new StreamWriter(fs);
                string txtData = "";
                int rowNum = 0;

                for (i = 0; i < dtContent.Rows.Count; i++) {
                    DataRow dr = dtContent.Rows[i];
                    //先將兩個運算欄位賦值
                    if (int.Parse(dr["amm0_day_count"].AsString()) > 0) {
                        if (dr["amm0_qnty1"].AsDecimal() > dr["amm0_qnty2"].AsDecimal()) {
                            dr["cp_amm0_qnty"] = Math.Truncate(dr["amm0_qnty2"].AsDecimal()) /
                                dr["amm0_day_count"].AsInt();
                        }
                        else {
                            dr["cp_amm0_qnty"] = Math.Truncate(dr["amm0_qnty1"].AsDecimal()) /
                                dr["amm0_day_count"].AsInt();
                        }
                    }
                    else {
                        dr["cp_amm0_qnty"] = 0;
                    }
                    if (dr["cp_amm0_qnty"].AsInt() >= dr["mmf_qnty_low"].AsInt()) {
                        dr["cp_qnty_flag"] = "Y";
                    }
                    else {
                        dr["cp_qnty_flag"] = "N";
                    }
                    ls_prod_type = dr["amm0_prod_type"].AsString();
                    ls_text = dr["amm0_ymd"].AsString().Replace("/", "").SubStr(0, 6);
                    ls_text = ls_text + dr["amm0_brk_no"].AsString().SubStr(0, 7);
                    //商品2碼時,第3碼加上F/O
                    ls_kind_id = dr["amm0_kind_id2"].AsString().Trim();
                    if (ls_kind_id.Length == 2) {
                        ls_kind_id = ls_kind_id + ls_prod_type;
                    }
                    ls_text = ls_text + ls_kind_id.PadRight(4, ' ');

                    //期貨不判斷有效詢報價比例
                    if (dr["amm0_prod_type"].AsString() == "O") {
                        ls_text = ls_text + string.Format("{0:000.0000}", dr["amm0_rate"]);
                    }
                    else {
                        ls_text = ls_text + "000.0000";
                    }
                    ls_text = ls_text + dr["amm0_acc_no"].AsString().SubStr(0, 7);
                    //符合維持時間
                    //		ll_time = lds_1.getitemdecimal(i,"amm0_keep_time")
                    //		li_day_count = lds_1.getitemdecimal(i,"amm0_day_count")
                    //		li_avg_time = lds_1.getitemdecimal(i,"mmf_avg_time")
                    //		if		li_day_count > 0 then 
                    //				ll_time = round(ll_time / 60 / li_day_count,0)
                    //		else
                    //				ll_time = 0
                    //		end	if
                    //		if		ll_time <> 0 and ll_time > li_avg_time then
                    //				ls_text = ls_text + 'Y'
                    //		else
                    //				ls_text = ls_text + 'N'
                    //		end	if

                    //所有的選擇權都是Y
                    //		if		lds_1.getitemstring(i,"amm0_keep_flag") = 'Y' or lds_1.getitemstring(i,"amm0_prod_type") = 'O' then
                    //				ls_text = ls_text + 'Y'
                    //		else
                    //				ls_text = ls_text + 'N'
                    //		end	if	

                    //20091225 modify by Jack
                    if (dr["amm0_keep_flag"].AsString() == "Y") {
                        ls_text = ls_text + "Y";
                    }
                    else {
                        ls_text = ls_text + "N";
                    }
                    // end of Jack modify

                    //ls_text = ls_text + string(lds_1.getitemstring(i,"amm0_keep_flag"),"@")
                    //判斷最低口數
                    ls_text = ls_text + dr["cp_qnty_flag"];

                    //2009.12.17 add		
                    //2011.05.03 add AMM0_RESULT
                    //2011.07.01 改成AMM0_VALID_RESULT
                    li_day_count = dr["amm0_day_count"].AsInt();
                    if (li_day_count > 0) {
                        ls_text = ls_text + string.Format("{0:0000}", Math.Ceiling(dr["amm0_keep_time"].AsDecimal() / 60 / li_day_count));
                        //			if		isnull(lds_1.getitemdecimal(i,"amm0_result"))  then
                        //					ls_text = ls_text + "00000000"
                        //			else
                        //					ls_text = ls_text + string( ceiling(lds_1.getitemdecimal(i,"amm0_result") / li_day_count), "00000000")
                        //			end	if		
                        if (dr["amm0_valid_result"] == null) {
                            ls_text = ls_text + "00000000";
                        }
                        else {
                            ls_text = ls_text + string.Format("{0:00000000}", Math.Ceiling(dr["amm0_valid_result"].AsDecimal() / li_day_count));
                        }
                    }
                    else {
                        ls_text = ls_text + "0000";
                        ls_text = ls_text + "00000000";
                    }
                    //不合理交易量
                    if (dr["amm0_trd_invalid_qnty"] == null) {
                        ls_text = ls_text + "00000000";
                    }
                    else {
                        ls_text = ls_text + string.Format("{0:00000000}", dr["amm0_trd_invalid_qnty"]);
                    }
                    //交易時段:0日盤/1夜盤
                    ls_text = ls_text + dr["market_code"];

                    //寫檔
                    wr.WriteLine(ls_text);

                }

                /*******************
                存檔
                *******************/
                wr.Dispose();
                wr.Close();
                lblProcessing.Visible = false;
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }

            /*******************
            Write LOGF
            *******************/

            return ResultStatus.Success;
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