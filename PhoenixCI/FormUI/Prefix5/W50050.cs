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
   // winni, 2019/01/07 造市者每日價平上下5檔各序列報價     
   public partial class W50050 : FormParent {
      private D50050 dao50050;
      string ls_time1, ls_time2, ls_brk_no, ls_prod_kind_id, ls_settle_date, ls_pc_code, ls_p_seq_no, ls_acc_no;
      decimal ld_p_seq_no, ld_avg_spread, ll_found_row, ll_rows;
    
      int i, li_p_seq_no1, li_p_seq_no2;
      DateTime ldt_date;
      string dbName = "";

      public W50050(string programID , string programName) : base(programID , programName) {
         try {
            InitializeComponent();
            //GridHelper.SetCommonGrid(gvMain);
            //PrintableComponent = gcMain;
            dao50050 = new D50050();

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
         DataTable rep = dao50050.ListAll(ls_brk_no , ls_acc_no , ls_time1 , ls_time2 , ls_prod_kind_id , ls_settle_date , ls_pc_code , li_p_seq_no1 , li_p_seq_no2 , dbName , "Y");
         if (rep.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartDate.Text , this.Text));
            return ResultStatus.Fail;
         }
         //存CSV
         string etfFileName = "50050_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
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
            //ls_settle_date = "201811";

            //價內外檔數
            ls_p_seq_no = ddlb_2.Text.Trim();
            switch (ls_p_seq_no) {
               case "價內第5檔":
                  li_p_seq_no1 = -5;
                  li_p_seq_no2 = -5;
                  break;
               case "價內第4檔":
                  li_p_seq_no1 = -4;
                  li_p_seq_no2 = -4;
                  break;
               case "價內第3檔":
                  li_p_seq_no1 = -3;
                  li_p_seq_no2 = -3;
                  break;
               case "價內第2檔":
                  li_p_seq_no1 = -2;
                  li_p_seq_no2 = -2;
                  break;
               case "價內第1檔":
                  li_p_seq_no1 = -1;
                  li_p_seq_no2 = -1;
                  break;
               case "價平":
                  li_p_seq_no1 = 0;
                  li_p_seq_no2 = 0;
                  break;
               case "價外第1檔":
                  li_p_seq_no1 = 1;
                  li_p_seq_no2 = 1;
                  break;
               case "價外第2檔":
                  li_p_seq_no1 = 2;
                  li_p_seq_no2 = 2;
                  break;
               case "價外第3檔":
                  li_p_seq_no1 = 3;
                  li_p_seq_no2 = 3;
                  break;
               case "價外第4檔":
                  li_p_seq_no1 = 4;
                  li_p_seq_no2 = 4;
                  break;
               case "價外第5檔":
                  li_p_seq_no1 = 5;
                  li_p_seq_no2 = 5;
                  break;
               default:
                  li_p_seq_no1 = -5;
                  li_p_seq_no2 = 5;
                  break;
            }

            DataTable defaultTable = new DataTable();
            defaultTable = dao50050.ListAll(ls_brk_no , ls_acc_no , ls_time1 , ls_time2 , ls_prod_kind_id , ls_settle_date , ls_pc_code , li_p_seq_no1 , li_p_seq_no2 , dbName);

            if (defaultTable.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("無任何資料!"));
               _ToolBtnExport.Enabled = false;
               return ResultStatus.Fail;
            }

            DataRow drFirst = defaultTable.Rows[0];
            ammd_date.Text = "日期：" + drFirst["ammd_date"].AsString().Substring(0 , 10); //列出第一筆[ammd_date]的日期
            trade_time.Text = "交易時間：" + txtStartTime.Text + "~" + txtEndTime.Text;

            ammd_date.Visible = true;
            trade_time.Visible = true;

            gcMain.DataSource = defaultTable;
            gcMain.Visible = true;
            gcMain.Focus();

            return ResultStatus.Success;

         } catch (Exception ex) {
            MessageDisplay.Error(ex.ToString());
            return ResultStatus.Fail;
         }

      }

      //對價平上下檔數(AMMD_P_SEQ_NO)欄位做值轉換
      private void gvMain_CustomColumnDisplayText(object sender , DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e) {
         if (e.Column.FieldName == "AMMD_P_SEQ_NO") {
            switch (Convert.ToInt16(e.Value)) {
               case 0:
                  e.DisplayText = "價平";
                  break;
               case 1:
                  e.DisplayText =  "價外第一檔";
                  break;
               case 2:
                  e.DisplayText = "價外第二檔";
                  break;
               case 3:
                  e.DisplayText = "價外第三檔";
                  break;
               case 4:
                  e.DisplayText = "價外第四檔";
                  break;
               case 5:
                  e.DisplayText = "價外第五檔";
                  break;
               case -1:
                  e.DisplayText = "價內第一檔";
                  break;
               case -2:
                  e.DisplayText = "價內第二檔";
                  break;
               case -3:
                  e.DisplayText = "價內第三檔";
                  break;
               case -4:
                  e.DisplayText = "價內第四檔";
                  break;
               case -5:
                  e.DisplayText = "價內第五檔";
                  break;             
            }  

         }
         //時間格式呈現微調
         if (e.Column.FieldName == "AMMD_W_TIME") {
            e.DisplayText = String.Format("{0:yyyy/MM/dd HH:mm:ss.fff}" , e.Value);
         }
      }

   }
}