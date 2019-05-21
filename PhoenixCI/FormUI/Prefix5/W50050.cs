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
/// Winni, 2019/04/26
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
   /// <summary>
   /// 50050 造市者每日價平上下5檔各序列報價
   /// </summary>
   public partial class W50050 : FormParent {

      private D50050 dao50050;

      string brkNo, accNo, dbName, time1, time2, prodKindId, settleDate, pcCode, ls_p_seq_no;
      int li_p_seq_no1, li_p_seq_no2;
      DataTable defaultTable;

      public W50050(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao50050 = new D50050();
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
         DataTable dtFcmAcc = new ABRK().ListFcmAccNo();//第一行空白+ abrk_abrk_name/cp_display
         dwBrkno.SetDataTable(dtFcmAcc , "AMPD_FCM_NO" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , "");

         //商品
         DataTable dtProd = new APDK().ListAll3();//第一行空白+ abrk_abrk_name/cp_display
         dwProd.SetDataTable(dtProd , "PDK_KIND_ID" , "PDK_KIND_ID" , TextEditStyles.DisableTextEditor , "");

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

            //DbName
            if (gbMarket.EditValue.AsString() == "rbMarket0") {
               dbName = "ammd"; //一般
            } else {
               dbName = "ammdah"; //盤後
            }

            time1 = txtStartDate.Text + " " + txtStartTime.Text + ":00";
            time2 = txtEndDate.Text + " " + txtEndTime.Text + ":00";

            //造市者 get brkNo--accNo (abrk_name) from dw_sbrkno 
            brkNo = string.IsNullOrEmpty(dwBrkno.EditValue.AsString()) ? "%" : dwBrkno.EditValue.AsString().Split(new[] { "--" } , StringSplitOptions.None)[0];
            accNo = string.IsNullOrEmpty(dwBrkno.EditValue.AsString()) ? "%" : dwBrkno.EditValue.AsString().Split(new[] { "--" } , StringSplitOptions.None)[1];

            //商品
            prodKindId = string.IsNullOrEmpty(dwProd.EditValue.AsString()) ? "%" : dwProd.EditValue.AsString();

            //買賣權
            pcCode = ddlb_1.Text.Trim();
            if (string.IsNullOrEmpty(pcCode)) {
               pcCode = "%";
            } else if (pcCode == "買權") {
               pcCode = "C";
            } else if (pcCode == "賣權") {
               pcCode = "P";
            }

            //契約月份
            settleDate = string.IsNullOrEmpty(sle_1.Text.Trim()) ? "%" : sle_1.Text.Trim().Replace("/" , "");
            //settleDate = "201811";

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

            DataTable defaultTable = dao50050.ListAll(brkNo , accNo , time1 , time2 , prodKindId , settleDate ,
                                                                  pcCode , li_p_seq_no1 , li_p_seq_no2 , dbName);

            if (defaultTable.Rows.Count <= 0) {
               MessageDisplay.Info("無任何資料!");
               _ToolBtnExport.Enabled = false;
               this.Cursor = Cursors.Arrow;
               return ResultStatus.Fail;
            }

            gvMain.Columns["AMMD_DATE"].Group();
            gvMain.SetGridGroupSummary(gvMain.Columns[12].FieldName , $"交易時間 : {txtStartTime.Text}~{txtEndTime.Text}" , DevExpress.Data.SummaryItemType.Count);

            gcMain.Visible = true;
            gcMain.DataSource = defaultTable;
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
         defaultTable = dao50050.ListAll(brkNo , accNo , time1 , time2 , prodKindId ,
                                 settleDate , pcCode , li_p_seq_no1 , li_p_seq_no2 , dbName , "Y");
         if (defaultTable.Rows.Count <= 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartDate.Text , this.Text));
            return ResultStatus.Fail;
         }
         this.Cursor = Cursors.WaitCursor;

         foreach (DataRow dr in defaultTable.Rows) {
            dr["ammd_date"] += " 00:00:00";
         }

         //存CSV
         string etfFileName = "50050_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
         etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
         ExportOptions csvref = new ExportOptions();
         csvref.HasHeader = true;
         csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
         Common.Helper.ExportHelper.ToCsv(defaultTable , etfFileName , csvref);

         this.Cursor = Cursors.Arrow;
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

      //對價平上下檔數(AMMD_P_SEQ_NO)欄位做值轉換
      private void gvMain_CustomColumnDisplayText(object sender , DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e) {
         if (e.Column.FieldName == "AMMD_P_SEQ_NO") {
            switch (Convert.ToInt32(e.Value)) {
               case 0:
                  e.DisplayText = "價平";
                  break;
               case 1:
                  e.DisplayText = "價外第一檔";
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

      }

      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         e.Cancel = true;
      }

   }
}