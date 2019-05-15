using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/04/26
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
   /// <summary>
   /// 50050 造市者每日價平上下5檔各序列報價
   /// </summary>
   public partial class W50040 : FormParent {

      private D50040 dao50040;
      string ls_sort_type = "", ls_fcm_no = "", ls_kind_id2 = "", dbName = "";
      int li_val = 0;

      DataTable defaultTable;

      public W50040(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao50040 = new D50040();
      }

      protected override ResultStatus Open() {
         base.Open();
         //1. 設定時間初始值
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

         //2. 設定下拉選單
         //造市者
         DataTable dtFcm = new ABRK().ListFcmNo();//第一行空白+ampd_fcm_no/abrk_abrk_name/cp_display
         dw_sbrkno.SetDataTable(dtFcm , "AMPD_FCM_NO" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , " ");

         //商品
         DataTable dtProd = new APDK().ListKind2();//前面空一行+APDK_KIND_ID_STO/MARKET_CODE
         dw_prod_kd.SetDataTable(dtProd , "APDK_KIND_ID_STO" , "APDK_KIND_ID_STO" , TextEditStyles.DisableTextEditor , " ");

         gbItem_SelectedIndexChanged(gbItem , null);

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

      protected override ResultStatus Export() {

         //讀取資料
         DataTable rep = dao50040.ListAll(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , ls_sort_type , li_val , ls_fcm_no , ls_kind_id2 , dbName , "Y");
         if (rep.Rows.Count <= 0) {
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
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();
            _ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Retrieve() {

         _ToolBtnExport.Enabled = true;
         gvMain.GroupSummary.Clear();
         this.Cursor = Cursors.WaitCursor;

         try {
            //報表內容
            if (gbMarket.EditValue.ToString() == "rb_market_0") {
               dbName = "AMM1";
            } else {
               dbName = "AMM1AH";
            }

            //輸入選擇權
            if (gbItem.EditValue.ToString() == "rb_item_0") {
               ls_sort_type = "F";
               ls_fcm_no = dw_sbrkno.EditValue.AsString();
               if (string.IsNullOrEmpty(ls_fcm_no)) {
                  ls_fcm_no = "%";
               }
               ls_kind_id2 = "%";

            } else if (gbItem.EditValue.ToString() == "rb_item_1") {
               ls_sort_type = "P";
               ls_kind_id2 = dw_prod_kd.EditValue.AsString();
               if (string.IsNullOrEmpty(ls_kind_id2)) {
                  ls_kind_id2 = "%";
               } else {
                  ls_kind_id2 += "%";
               }
               ls_fcm_no = "%";
            }

            //輸出選擇
            if (gbPrice.EditValue.ToString() == "rb_price_0") {
               li_val = 1; //最大價差  
            } else if (gbPrice.EditValue.ToString() == "rb_price_1") {
               li_val = 2; //最小價差
            } else {
               li_val = 3; //平均價差
            }

            DataTable defaultTable = dao50040.ListAll(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , ls_sort_type , li_val , ls_fcm_no , ls_kind_id2 , dbName);

            if (defaultTable.Rows.Count <= 0) {
               MessageDisplay.Info("無任何資料!");
               gcMain.DataSource = null;
               gcMain.Visible = false;
               _ToolBtnExport.Enabled = false;
               return ResultStatus.Fail;
            }

            //1. 設定gvMain
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = defaultTable;

            //1.1 設定欄位caption
            gvMain.SetColumnCaption("AMM1_DATE" , "資料日期");
            gvMain.SetColumnCaption("AMM1_FCM_NO" , "造市者代號");          
            gvMain.SetColumnCaption("ABRK_ABBR_NAME" , "造市者名稱");
            gvMain.SetColumnCaption("AMM1_ACC_NO" , "帳號");
            gvMain.SetColumnCaption("AMM1_PROD_TYPE" , "AMM1_PROD_TYPE");

            gvMain.SetColumnCaption("AMM1_KIND_ID2" , "商品");
            gvMain.SetColumnCaption("O_OUT5" , "價外第5檔");
            gvMain.SetColumnCaption("O_OUT4" , "價外第4檔");
            gvMain.SetColumnCaption("O_OUT3" , "價外第3檔");
            gvMain.SetColumnCaption("O_OUT2" , "價外第2檔");

            gvMain.SetColumnCaption("O_OUT1" , "價外第1檔");
            gvMain.SetColumnCaption("O_0" , "價平");
            gvMain.SetColumnCaption("O_IN1" , "價內第1檔");
            gvMain.SetColumnCaption("O_IN2" , "價內第2檔");
            gvMain.SetColumnCaption("O_IN3" , "價內第3檔");

            gvMain.SetColumnCaption("O_IN4" , "價內第4檔");
            gvMain.SetColumnCaption("O_IN5" , "價內第5檔");
            gvMain.SetColumnCaption("F_0" , "期貨");
            gvMain.SetColumnCaption("SORT_TYPE" , "SORT_TYPE");

            foreach (GridColumn item in gvMain.Columns) {
               item.AppearanceHeader.BackColor = Color.FromArgb(128 , 255 , 255);
            }

            if (ls_sort_type == "F") {

               gvMain.SetColumnCaption("AMM1_FCM_NO" , "造市者");

               //設定隱藏欄位
               gvMain.Columns["AMM1_DATE"].Visible = false;
               gvMain.Columns["AMM1_FCM_NO"].Visible = false;
               gvMain.Columns["AMM1_ACC_NO"].Visible = false;
               gvMain.Columns["ABRK_ABBR_NAME"].Visible = false;
               gvMain.Columns["AMM1_PROD_TYPE"].Visible = false;

               gvMain.Columns["SORT_TYPE"].Visible = false;

               //針對每個欄位做值轉換
               //若AMM1_KIND_ID2取的值長度為2，後面加上AMM1_PROD_TYPE的值，若不為2直接取AMM1_KIND_ID2值即可
               foreach (DataRow dr in defaultTable.Rows) {
                  if (dr["AMM1_KIND_ID2"].AsString().Length == 2) {
                     dr["AMM1_KIND_ID2"] = dr["AMM1_KIND_ID2"].AsString() + dr["AMM1_PROD_TYPE"].AsString();
                  } else {
                     dr["AMM1_KIND_ID2"] = dr["AMM1_KIND_ID2"].AsString();
                  }
               }

               //設定 平行group
               gvMain.SortInfo.ClearAndAddRange(new[]
               { new GridMergedColumnSortInfo (new[] { gvMain.Columns["AMM1_DATE"] ,gvMain.Columns["AMM1_FCM_NO"] ,gvMain.Columns["AMM1_ACC_NO"] },
            new [] {ColumnSortOrder.Ascending,ColumnSortOrder.Ascending,ColumnSortOrder.Ascending})} , 2);

               gvMain.SetGridGroupSummary(gvMain.Columns["AMM1_DATE"].FieldName + gvMain.Columns["AMM1_FCM_NO"].FieldName + gvMain.Columns["AMM1_ACC_NO"].FieldName , "" , SummaryItemType.Count);

            } else {

               //設定隱藏欄位
               gvMain.Columns["AMM1_DATE"].Visible = false;
               gvMain.Columns["AMM1_KIND_ID2"].Visible = false;
               gvMain.Columns["AMM1_PROD_TYPE"].Visible = false;
               gvMain.Columns["SORT_TYPE"].Visible = false;

               foreach (DataRow dr in defaultTable.Rows) {
                  dr["AMM1_FCM_NO"] = dr["AMM1_FCM_NO"].AsString();
               }

               //設定 平行group
               gvMain.SortInfo.ClearAndAddRange(new[]
               { new GridMergedColumnSortInfo (new[] { gvMain.Columns["AMM1_DATE"] ,gvMain.Columns["AMM1_KIND_ID2"] },
            new [] {ColumnSortOrder.Ascending,ColumnSortOrder.Ascending,ColumnSortOrder.Ascending})} , 2);

               gvMain.SetGridGroupSummary(gvMain.Columns["AMM1_DATE"].FieldName + gvMain.Columns["AMM1_KIND_ID2"].FieldName , "" , SummaryItemType.Count);

            }

            #region 特殊規則,某些欄位小數點後四碼 或兩碼
            foreach (DataRow dr in defaultTable.Rows) {
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
            }
            #endregion

            gcMain.Visible = true;
            gcMain.DataSource = defaultTable;
            gvMain.OptionsBehavior.AllowFixedGroups = DefaultBoolean.True;

            gvMain.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            gvMain.AppearancePrint.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;
            gvMain.ColumnPanelRowHeight = 30;
            gvMain.AppearancePrint.HeaderPanel.Font = new Font("Microsoft YaHei" , gvMain.AppearancePrint.HeaderPanel.Font.Size);
            gvMain.AppearancePrint.Row.Font = new Font("Microsoft YaHei" , 8);
            gvMain.OptionsPrint.AllowMultilineHeaders = true;
            gvMain.AppearancePrint.GroupRow.Font = new Font("Microsoft YaHei" , 12);

            gvMain.BestFitColumns();
            gvMain.ExpandAllGroups();
            GridHelper.SetCommonGrid(gvMain);
            gcMain.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;
      }

      private void gbItem_SelectedIndexChanged(object sender , EventArgs e) {
         if (gbItem.EditValue.ToString() == "rb_item_0") {
            dw_sbrkno.Visible = true;
            dw_prod_kd.Visible = false;
         } else if (gbItem.EditValue.ToString() == "rb_item_1") {
            dw_sbrkno.Visible = false;
            dw_prod_kd.Visible = true;
         }
      }

      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         e.Cancel = true;
      }

   }
}