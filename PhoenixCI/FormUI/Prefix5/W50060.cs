using System;
using System.Data;
using System.Windows.Forms;
using BaseGround;
using Common;
using BusinessObjects.Enums;
using BaseGround.Report;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Controls;
using System.Globalization;
using BaseGround.Shared;
using System.IO;

namespace PhoenixCI.FormUI.Prefix5 {
   public partial class W50060 : FormParent {
      private D50060 dao50060;
      string ls_time1, ls_time2, ls_brk_no, ls_prod_kind_id, ls_settle_date, ls_pc_code, ls_acc_no;
      decimal ld_strike_price1, ld_strike_price2, li_p_seq_no1, li_p_seq_no2;
      string dbName = "";

      public W50060(string programID , string programName) : base(programID , programName) {
         try {
            InitializeComponent();
            //GridHelper.SetCommonGrid(gvMain);
            //PrintableComponent = gcMain;
            dao50060 = new D50060();

            this.Text = _ProgramID + "─" + _ProgramName;

            txtStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
            txtEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
            txtStartTime.EditValue = "08:45";
            txtEndTime.EditValue = "13:45";

            //造市者下拉選單
            DataTable dtFcmAcc = new ABRK().ListFcmAccNo();//第一行空白+ abrk_abrk_name/cp_display
            dw_sbrkno.SetDataTable(dtFcmAcc , "AMPD_FCM_NO");

            //商品下拉選單
            DataTable dtProd = new APDK().ListAll3();//第一行空白+ abrk_abrk_name/cp_display
            dw_prod_kd.SetDataTable(dtProd , "PDK_KIND_ID" , "PDK_KIND_ID");

         } catch (Exception ex) {
            MessageDisplay.Error(ex.ToString());
         }
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnExport.Enabled = false;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         //讀取資料
         DataTable rep = dao50060.ListData(ls_brk_no , ls_acc_no , ls_time1 , ls_time2 , ls_prod_kind_id , ls_settle_date , ls_pc_code , ld_strike_price1 , ld_strike_price2 , dbName , "Y");
         if (rep.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartDate.Text , this.Text));
            return ResultStatus.Fail;
         }
         //存CSV
         string etfFileName = "50060_" + DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss") + ".csv";
         etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
         ExportOptions csvref = new ExportOptions();
         csvref.HasHeader = true;
         csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
         Common.Helper.ExportHelper.ToCsv(rep , etfFileName , csvref);

         return ResultStatus.Success;
      }

      protected override ResultStatus ExportAfter(string startTime) {
         MessageDisplay.Info("轉檔完成!");

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {

         //gcMain.Print();
         CommonReportLandscapeA4 rep = new CommonReportLandscapeA4();
         rep.printableComponentContainerMain.PrintableComponent = gcMain;

         reportHelper.Create(rep);
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {
         try {
            _ToolBtnExport.Enabled = true;
            //報表內容
            if (gb_market.EditValue.ToString() == "rb_market_0") {
               dbName = "ammd";
            } else {
               dbName = "ammdAH";
            }

            ls_time1 = txtStartDate.Text + " " + txtStartTime.Text + ":00";
            ls_time2 = txtEndDate.Text + " " + txtEndTime.Text + ":00";

            //get brk_no--acc_no from dw_sbrkno 造市者
            ls_brk_no = string.IsNullOrEmpty(dw_sbrkno.EditValue.AsString()) ? "%" : dw_sbrkno.EditValue.AsString().Split(new[] { "--" } , StringSplitOptions.None)[0];
            ls_acc_no = string.IsNullOrEmpty(dw_sbrkno.EditValue.AsString()) ? "%" : dw_sbrkno.EditValue.AsString().Split(new[] { "--" } , StringSplitOptions.None)[1];

            //商品
            ls_prod_kind_id = string.IsNullOrEmpty(dw_prod_kd.EditValue.AsString()) ? "%" : dw_prod_kd.EditValue.AsString();

            //買賣權
            ls_pc_code = ddlb_1.Text.Trim();
            if (string.IsNullOrEmpty(ls_pc_code)) {
               ls_pc_code = "%";
            } else if (ls_pc_code == "買權") {
               ls_pc_code = "C";
            } else if (ls_pc_code == "賣權") {
               ls_pc_code = "P";
            }

            //契約月份
            ls_settle_date = string.IsNullOrEmpty(sle_1.Text.Trim()) ? "%" : sle_1.Text.Trim().Replace("/" , "");

            //履約價格
            string tmp = sle_2.Text.Trim();
            if (tmp == "") {
               ld_strike_price1 = 0;
               ld_strike_price2 = 99999;
            } else {
               ld_strike_price1 = Convert.ToDecimal(tmp);
               ld_strike_price2 = ld_strike_price1;
            }

            DataTable defaultTable = new DataTable();
            defaultTable = dao50060.ListData(ls_brk_no , ls_acc_no , ls_time1 , ls_time2 , ls_prod_kind_id , ls_settle_date , ls_pc_code , ld_strike_price1 , ld_strike_price2 , dbName);
            if (defaultTable.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("無任何資料!"));
            } else {
               DataRow drFirst = defaultTable.Rows[0];
               ammd_date.Text = "日期：" + drFirst["ammd_date"].AsString().Substring(0 , 10); //列出第一筆[ammd_date]的日期
               trade_time.Text = "交易時間：" + txtStartTime.Text + "~" + txtEndTime.Text;
               ammd_date.Visible = true;
               trade_time.Visible = true;

               gcMain.DataSource = defaultTable;
               gcMain.Visible = true;
               gcMain.Focus();
            }

         } catch (Exception ex) {
            MessageDisplay.Error(ex.ToString());
         }

         return ResultStatus.Success;
      }

      private void gvMain_CustomColumnDisplayText(object sender , DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e) {
         //時間格式呈現微調
         if (e.Column.FieldName == "AMMD_W_TIME") {
            e.DisplayText = String.Format("{0:yyyy/MM/dd HH:mm:ss.fff}" , e.Value);
         }
      }

   }
}