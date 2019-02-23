using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix5 {
   /// <summary>
   /// Winni ,2019/1/14 修改 ToCSV的部分
   /// </summary>
   public partial class W50040 : FormParent {
      private D50040 dao50040;
      DateTime ls_time1, ls_time2;
      string ls_sort_type = "", ls_fcm_no = "", ls_kind_id2 = "", dbName = "";
      int li_val = 0;

      public W50040(string programID , string programName) : base(programID , programName) {
         try {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;
            dao50040 = new D50040();

            txtStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
            txtEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");

            //造市者
            DataTable dtFcm = new ABRK().ListFcmNo();//第一行空白+ampd_fcm_no/abrk_abrk_name/cp_display
            dw_sbrkno.SetDataTable(dtFcm , "AMPD_FCM_NO");

            //商品
            DataTable dtProd = new APDK().ListKind2();//前面空一行+APDK_KIND_ID_STO/MARKET_CODE
            dw_prod_kd.SetDataTable(dtProd , "APDK_KIND_ID_STO" , "APDK_KIND_ID_STO");

            gb_item_SelectedIndexChanged(gb_item , null);


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
         DataTable rep = dao50040.ListAll(ls_time1 , ls_time2 , ls_sort_type , li_val , ls_fcm_no , ls_kind_id2 , dbName , "Y");
         if (rep.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartDate.Text , this.Text));
            return ResultStatus.Fail;
         }

         //存CSV (ps:輸出csv 都用ascii)
         string etfFileName = "50040_" + DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss") + ".csv";
         etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
         ExportOptions csvref = new ExportOptions();
         csvref.HasHeader = true;
         csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
         Common.Helper.ExportHelper.ToCsv(rep , etfFileName , csvref);

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
            //string dbName = "";
            if (gb_market.EditValue.ToString() == "rb_market_0") {
               dbName = "AMM1";
            } else {
               dbName = "AMM1AH";
            }

            //輸入選擇權
            //string ls_sort_type = "", ls_fcm_no = "", ls_kind_id2 = "";
            if (gb_item.EditValue.ToString() == "rb_item_0") {
               ls_sort_type = "F";
               ls_fcm_no = dw_sbrkno.EditValue.AsString("%");
               ls_kind_id2 = "%";

            } else if (gb_item.EditValue.ToString() == "rb_item_1") {
               ls_sort_type = "P";
               ls_fcm_no = "%";
               ls_kind_id2 = dw_prod_kd.EditValue.AsString("%") + "%";
            }

            //輸出選擇
            //int li_val;
            //RadioGroup rb_price = new RadioGroup();
            if (gb_price.EditValue.ToString() == "rb_price_0") {
               li_val = 1; //最大價差  
            } else if (gb_price.EditValue.ToString() == "rb_price_1") {
               li_val = 2; //最小價差
            } else {
               li_val = 3; //平均價差
            }

            //DateTime ls_time1, ls_time2;
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            ls_time1 = Convert.ToDateTime(txtStartDate.Text , dtFormat);
            ls_time2 = Convert.ToDateTime(txtEndDate.Text , dtFormat);

            DataTable defaultTable = new DataTable();
            defaultTable = dao50040.ListAll(ls_time1 , ls_time2 , ls_sort_type , li_val , ls_fcm_no , ls_kind_id2 , dbName);

            if (defaultTable.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("無任何資料!"));
               _ToolBtnExport.Enabled = false;
               return ResultStatus.Fail;
            }

            //將資料填入grid上面四個Computer計算域(其中兩個cp_key1、cp_key2在D50040用SQL改寫)

            //SORT_TYPE/abrk_abbr_name/AMM1_ACC_NO+................
            if (ls_sort_type == "F") {
               //當選擇造市者時,第一個欄位=amm1_kind_id2+amm1_prod_type or amm1_kind_id2
               //第一、第二跟第三個欄位都隱藏，只顯示商品(AMM1_KIND_ID2)
               gvMain.Columns["AMM1_FCM_NO"].Visible = false;
               gvMain.Columns["ABRK_ABBR_NAME"].Visible = false;
               gvMain.Columns["AMM1_ACC_NO"].Visible = false;
               gvMain.Columns["AMM1_KIND_ID2"].Visible = true;
               gvMain.Columns["AMM1_KIND_ID2"].VisibleIndex = 0;

               //開始針對每個欄位做值轉換
               //若AMM1_KIND_ID2取的值長度為2，後面加上AMM1_PROD_TYPE的值，若不為2直接取AMM1_KIND_ID2值即可
               foreach (DataRow dr in defaultTable.Rows) {
                  if (dr["AMM1_KIND_ID2"].AsString().Length == 2) {
                     dr["AMM1_KIND_ID2"] = dr["AMM1_KIND_ID2"].AsString() + dr["AMM1_PROD_TYPE"].AsString();
                  } else {
                     dr["AMM1_KIND_ID2"] = dr["AMM1_KIND_ID2"].AsString();
                  }//if (dr["amm1_kind_id2"].AsString().Length == 2) 

                  #region //winni,特殊規則,某些欄位小數點後四碼 或兩碼
                  //if( TRIM(amm1_kind_id2) in ("RHF","RTF","RHO","RTO","XEF"),'#0.0000','#0.00')
                  //O_OUT5,O_OUT4,O_OUT3,O_OUT2,O_OUT1,O_0, O_IN1,O_IN2,O_IN3,O_IN4,O_IN5,F_0都要改為小數點後四碼,否則為兩碼
                  string[] tmp = new string[] { "RHF" , "RTF" , "RHO" , "RTO" , "XEF" };
                  string[] fields = new string[] { "O_OUT5" , "O_OUT4" , "O_OUT3" , "O_OUT2" , "O_OUT1" , "O_0" ,
                                                   "O_IN1" , "O_IN2" , "O_IN3" , "O_IN4" , "O_IN5" , "F_0" };
                  if (tmp.Any(s => dr["AMM1_KIND_ID2"].AsString().Contains(s))) {
                     foreach (string f in fields) {
                        if (dr[f] != DBNull.Value)
                           dr[f] = dr[f].AsDecimal().ToString("0,0.0000");
                     }
                  } else {
                     foreach (string f in fields) {
                        if (dr[f] != DBNull.Value)
                           dr[f] = dr[f].AsDecimal().ToString("0,0.00");
                     }
                  }
                  #endregion

               }//foreach

               //group "if( sort_type ='F' ,amm1_fcm_no+ amm1_acc_no  , amm1_kind_id2 )"
               gvMain.Columns["AMM1_DATE"].Group();
               gvMain.Columns["AMM1_FCM_NO"].Group();
               gvMain.Columns["AMM1_ACC_NO"].Group();

            } else {
               //當選擇商品時,第一個欄位=amm1_fcm_no
               gvMain.Columns["AMM1_FCM_NO"].Visible = true;
               gvMain.Columns["ABRK_ABBR_NAME"].Visible = true;
               gvMain.Columns["AMM1_ACC_NO"].Visible = true;
               gvMain.Columns["AMM1_KIND_ID2"].Visible = false;
               gvMain.Columns["AMM1_FCM_NO"].VisibleIndex = 1;
               gvMain.Columns["ABRK_ABBR_NAME"].VisibleIndex = 2;
               gvMain.Columns["AMM1_ACC_NO"].VisibleIndex = 3;
               //defaultTable.Columns["SORT_TYPE"].Caption = "造市者代號";

               foreach (DataRow dr in defaultTable.Rows) {
                  dr["AMM1_FCM_NO"] = dr["AMM1_FCM_NO"].AsString();


                  #region //winni,特殊規則,某些欄位小數點後四碼 或兩碼
                  //if( TRIM(amm1_kind_id2) in ("RHF","RTF","RHO","RTO","XEF"),'#0.0000','#0.00')
                  //O_OUT5,O_OUT4,O_OUT3,O_OUT2,O_OUT1,O_0, O_IN1,O_IN2,O_IN3,O_IN4,O_IN5,F_0都要改為小數點後四碼,否則為兩碼
                  string[] tmp = new string[] { "RHF" , "RTF" , "RHO" , "RTO" , "XEF" };
                  string[] fields = new string[] { "O_OUT5" , "O_OUT4" , "O_OUT3" , "O_OUT2" , "O_OUT1" , "O_0" ,
                                                   "O_IN1" , "O_IN2" , "O_IN3" , "O_IN4" , "O_IN5" , "F_0" };
                  if (tmp.Any(s => dr["AMM1_KIND_ID2"].AsString().Contains(s))) {
                     foreach (string f in fields) {
                        if (dr[f] != DBNull.Value)
                           dr[f] = dr[f].AsDecimal().ToString("0,0.0000");
                     }
                  } else {
                     foreach (string f in fields) {
                        if (dr[f] != DBNull.Value)
                           dr[f] = dr[f].AsDecimal().ToString("0,0.00");
                     }
                  }
                  #endregion
               }//foreach

               //group "if( sort_type ='F' ,amm1_fcm_no+ amm1_acc_no  , amm1_kind_id2 )"
               gvMain.Columns["AMM1_DATE"].Group();
               gvMain.Columns["AMM1_KIND_ID2"].Group();

            }//if (ls_sort_type == "F") {

            DataRow drFirst = defaultTable.Rows[0];
            labAmmDate1.Text = "交易日期：" + drFirst["amm1_date"].AsString().Substring(0 , 10); //列出第一筆[ammd_date]的日期
            labInfo.Text = (ls_sort_type == "F" ? "造市者：" + drFirst["AMM1_FCM_NO"].AsString() + " ,帳號： " + drFirst["AMM1_ACC_NO"].AsString()
                                             : "商品：" + drFirst["AMM1_KIND_ID2"].AsString());
            labAmmDate1.Visible = true;
            labInfo.Visible = true;

            //gvMain.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            //gvMain.OptionsCustomization.AllowMergedGrouping = DevExpress.Utils.DefaultBoolean.True;

            gcMain.DataSource = defaultTable;
            gcMain.Visible = true;
            //gcMain.MainView.PopulateColumns();這個會根據dataTable整個對應
            gcMain.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            MessageDisplay.Error(ex.ToString());
            _ToolBtnExport.Enabled = false;
            return ResultStatus.Fail;
         }

      }


      private void gb_item_SelectedIndexChanged(object sender , EventArgs e) {
         if (gb_item.EditValue.ToString() == "rb_item_0") {
            dw_sbrkno.Visible = true;
            dw_prod_kd.Visible = false;
         } else if (gb_item.EditValue.ToString() == "rb_item_1") {
            dw_sbrkno.Visible = false;
            dw_prod_kd.Visible = true;
         }
      }



   }
}