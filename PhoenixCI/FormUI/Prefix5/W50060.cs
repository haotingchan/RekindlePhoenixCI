using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/05/20
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
   public partial class W50060 : FormParent {

      private D50060 dao50060;

      private string ls_time1, ls_time2, ls_brk_no, ls_prod_kind_id, ls_settle_date, ls_pc_code, ls_acc_no;
      private decimal ld_strike_price1, ld_strike_price2, li_p_seq_no1, li_p_seq_no2;
      private string dbName;

      public W50060(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao50060 = new D50060();
      }

      protected override ResultStatus Open() {
         base.Open();
         //1. 設定時間初始值
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtStartTime.EditValue = "08:45";
         txtEndTime.EditValue = "13:45";

         //2. 設定下拉選單
         //造市者
         DataTable dtFcmAcc = new ABRK().ListFcmAccNo();//第一行空白+ampd_fcm_no/abrk_name/cp_display
         dw_sbrkno.SetDataTable(dtFcmAcc , "AMPD_FCM_NO" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , " ");

         //商品
         DataTable dtProd = new APDK().ListAll3();//第一行空白+apdk_prod_type/pdk_kind_id/market_code
         dw_prod_kd.SetDataTable(dtProd , "PDK_KIND_ID" , "PDK_KIND_ID" , TextEditStyles.DisableTextEditor , " ");

         //買賣權
         DataTable dtCP = new CODW().ListLookUpEdit("50060" , "DDLB_1");
         ddlb_1.SetDataTable(dtCP , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {

         _ToolBtnExport.Enabled = true;
         gvMain.GroupSummary.Clear();
         this.Cursor = Cursors.WaitCursor;

         try {

            #region 輸入&日期檢核
            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

            //報表內容
            if (gbMarket.EditValue.AsString() == "rbMarket0") {
               dbName = "ammd"; //一般
            } else {
               dbName = "ammdAH"; //盤後
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

            DataTable defaultTable = dao50060.ListData(ls_brk_no , ls_acc_no , ls_time1 , ls_time2 , ls_prod_kind_id , ls_settle_date , ls_pc_code , ld_strike_price1 , ld_strike_price2 , dbName);
            if (defaultTable.Rows.Count <= 0) {
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA , GlobalInfo.ResultText);
               gcMain.DataSource = null;
               gcMain.Visible = false;
               this.Cursor = Cursors.Arrow;
               _ToolBtnExport.Enabled = false;
               return ResultStatus.Fail;
            } else {

               //1.1處理資料型態
               DataTable dt = defaultTable.Clone(); //轉型別用的datatable
               dt.Columns["AMMD_W_TIME"].DataType = typeof(string); //將原DataType(datetime)轉為string
               foreach (DataRow row in defaultTable.Rows) {
                  dt.ImportRow(row);
               }

               for (int i = 0 ; i < dt.Rows.Count ; i++) {
                  dt.Rows[i]["AMMD_W_TIME"] = Convert.ToDateTime(defaultTable.Rows[i]["AMMD_W_TIME"]).ToString("yyyy/MM/dd HH:mm:ss:fff");
               }

               gvMain.Columns["AMMD_DATE"].Group();
               gvMain.SetGridGroupSummary(gvMain.Columns[12].FieldName , $"交易時間 : {txtStartTime.Text}~{txtEndTime.Text}" , DevExpress.Data.SummaryItemType.Count);

               gcMain.Visible = true;
               gcMain.DataSource = dt;
               gvMain.OptionsBehavior.AllowFixedGroups = DefaultBoolean.True;
               gvMain.ExpandAllGroups();

               gvMain.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
               gvMain.AppearancePrint.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.ColumnPanelRowHeight = 30;
               gvMain.AppearancePrint.HeaderPanel.Font = new Font("Microsoft YaHei" , gvMain.Appearance.HeaderPanel.Font.Size);
               gvMain.AppearancePrint.Row.Font = new Font("Microsoft YaHei" , 11);
               gvMain.OptionsPrint.AllowMultilineHeaders = true;
               gvMain.AppearancePrint.GroupRow.Font = new Font("Microsoft YaHei" , 11);

               gvMain.BestFitColumns();
               GridHelper.SetCommonGrid(gvMain);
               gcMain.Focus();

               return ResultStatus.Success;
            }

         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Export() {

         //讀取資料
         DataTable rep = dao50060.ListData(ls_brk_no , ls_acc_no , ls_time1 , ls_time2 , ls_prod_kind_id , ls_settle_date , ls_pc_code , ld_strike_price1 , ld_strike_price2 , dbName , "Y");
         if (rep.Rows.Count <= 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartDate.Text , this.Text) , GlobalInfo.ResultText);
            return ResultStatus.Fail;
         }

         //1.1處理資料型態
         DataTable dt = rep.Clone(); //轉型別用的datatable
         dt.Columns["AMMD_W_TIME"].DataType = typeof(string); //將原DataType(datetime)轉為string
         foreach (DataRow row in rep.Rows) {
            dt.ImportRow(row);
         }

         for (int i = 0 ; i < dt.Rows.Count ; i++) {
            dt.Rows[i]["AMMD_W_TIME"] = Convert.ToDateTime(rep.Rows[i]["AMMD_W_TIME"]).ToString("yyyy/MM/dd HH:mm:ss:fff");
         }

         //存CSV
         string etfFileName = "50060_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
         etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
         ExportOptions csvref = new ExportOptions();
         csvref.HasHeader = true;
         csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
         Common.Helper.ExportHelper.ToCsv(dt , etfFileName , csvref);

         return ResultStatus.Success;
      }

      protected override ResultStatus ExportAfter(string startTime) {
         MessageDisplay.Info("轉檔完成!" , GlobalInfo.ResultText);

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {

         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();
            //_ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      private void gvMain_CustomColumnDisplayText(object sender , DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e) {
         //時間格式呈現微調
         if (e.Column.FieldName == "AMMD_W_TIME") {
            e.DisplayText = String.Format("{0:yyyy/MM/dd HH:mm:ss.fff}" , e.Value);
         }
      }

      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         e.Cancel = true;
      }
   }
}