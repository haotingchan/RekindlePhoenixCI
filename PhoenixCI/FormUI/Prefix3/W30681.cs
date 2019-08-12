using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

/// <summary>
/// ken,2019/4/9
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 成交價偏離幅度統計表
   /// </summary>
   public partial class W30681 : FormParent {

      private D30681 dao30681;
      private const string ReportName = "成交價偏離幅度統計表";

      #region 抓取畫面值(主要用在縮寫)
      /// <summary>
      /// Old=新 , New=舊
      /// </summary>
      public string Source {
         get {
            return rdoSource.EditValue.AsString();
         }
      }

      /// <summary>
      /// Summary=統計 , Detail=明細
      /// </summary>
      public string ReportType {
         get {
            return rdoReportType.EditValue.AsString();
         }
      }

      /// <summary>
      /// 到期月序：第1支腳
      /// </summary>
      public int Mth1 {
         get {
            return txtMth1.EditValue.AsInt(99);
         }
      }

      /// <summary>
      /// 到期月序：第2支腳
      /// </summary>
      public int Mth2 {
         get {
            return txtMth2.EditValue.AsInt(99);
         }
      }

      /// <summary>
      /// get chkLevel 有勾選的值,用逗號串接
      /// </summary>
      public string DetailLevelList {
         get {
            string temp = "";

            //跑前七個
            for (int k = 0 ; k < 7 ; k++) {
               if (chkLevel.Items[k].CheckState == System.Windows.Forms.CheckState.Checked)
                  temp += ",'" + chkLevel.Items[k].Value + "'";
            }

            temp = string.IsNullOrEmpty(temp) ? "" : temp.Substring(1);
            return temp;
         }
      }

      /// <summary>
      /// 是否有勾選空白,Y=是,N=否
      /// </summary>
      public string IsLevelNull {
         get {
            if (chkLevel.Items[7].CheckState == System.Windows.Forms.CheckState.Checked)
               return "Y";
            else
               return "N";
         }
      }
      #endregion


      public W30681(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30681 = new D30681();

         GridHelper.SetCommonGrid(gvExport);
         gvExport.OptionsBehavior.Editable = false;
         gvExport.OptionsBehavior.AutoPopulateColumns = true;
         gvExport.OptionsView.RowAutoHeight = true;

      }

      protected override ResultStatus Open() {
         base.Open();

         //設定 商品單複式 下拉選單
         DataTable lstScCode = new CODW().ListLookUpEdit("30681" , "DDLB_SC");
         Extension.SetDataTable(ddlScCode , lstScCode , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor);
         ddlScCode.ItemIndex = 0;

         //設定 委託單方式 下拉選單
         DataTable dtOsfOrderType = new CODW().ListLookUpEdit("30681" , "OSF_ORDER_TYPE");
         Extension.SetDataTable(ddlOsfOrderType , dtOsfOrderType , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor);
         ddlOsfOrderType.ItemIndex = 0;

         //設定 委託單條件 下拉選單
         DataTable dtOsfOrderCond = new CODW().ListLookUpEdit("30681" , "OSF_ORDER_COND");
         Extension.SetDataTable(ddlOsfOrderCond , dtOsfOrderCond , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor);
         ddlOsfOrderCond.ItemIndex = 0;

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtStartDate.DateTimeValue = txtEndDate.DateTimeValue;

#if DEBUG
         txtStartDate.DateTimeValue = DateTime.ParseExact("2017/10/11" , "yyyy/MM/dd" , null);
         txtEndDate.DateTimeValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式)";
#endif

         r_frame2.Visible = false;
         panDetail.Visible = false;

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         try {

            #region 輸入&日期檢核
            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

            //1.開始轉出資料
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Refresh();
            ResultStatus res = ResultStatus.Fail;

            if (ReportType == "Summary") {
               //統計,會輸出一個csv檔案
               if (Source == "Old") {
                  res = wf_30681_s();
               } else {
                  res = wf_30681_s_new();
               }

            } else {
               //明細,會輸出兩個csv檔案
               if (Source == "Old") {
                  res = wf_30681_d();
                  res = wf_30681_d_mtf();
               } else {
                  res = wf_30681_d_new();
                  res = wf_30681_d_mtf_new();
               }
            }

            return res;
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
         return ResultStatus.Fail;
      }


      protected ResultStatus wf_30681_s() {
         try {

            string reportId = "30681_s";
            string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                                                      string.Format("{0}_{1}.csv" , reportId , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

            //1.get dataTable
            DataTable dtTarget = dao30681.d_30681_s(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , ddlScCode.EditValue.AsString() ,
                                                        txtKind1.Text , txtKind2.Text , Mth1 , Mth2);
            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!" , txtStartDate.DateTimeValue.ToString("yyyyMM") , reportId , ReportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.Export Csv
            ExportCsv(dtTarget , excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected ResultStatus wf_30681_s_new() {
         try {

            string reportId = "30681_s_new";
            string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                                                      string.Format("{0}_{1}.csv" , reportId , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

            //1.get dataTable
            DataTable dtTarget = dao30681.d_30681_s_new(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , ddlScCode.EditValue.AsString() ,
                                                        txtKind1.Text , txtKind2.Text , Mth1 , Mth2);
            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!" , txtStartDate.DateTimeValue.ToString("yyyyMM") , reportId , ReportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.Export Csv
            ExportCsv(dtTarget , excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected ResultStatus wf_30681_d() {
         try {

            string reportId = "30681_d";
            string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                                                      string.Format("{0}_{1}.csv" , reportId , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

            //1.get dataTable
            DataTable dtTarget = dao30681.d_30681_d(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , ddlScCode.EditValue.AsString() ,
                                                        txtKind1.Text , txtKind2.Text , Mth1 , Mth2 ,
                                                        ddlOsfOrderType.EditValue.AsString() , ddlOsfOrderCond.EditValue.AsString() , DetailLevelList , IsLevelNull);
            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!" , txtStartDate.DateTimeValue.ToString("yyyyMM") , reportId , ReportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.Export Csv
            ExportCsv(dtTarget , excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected ResultStatus wf_30681_d_new() {
         try {

            string reportId = "30681_d_new";
            string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                                                      string.Format("{0}_{1}.csv" , reportId , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

            //1.get dataTable
            DataTable dtTarget = dao30681.d_30681_d_new(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , ddlScCode.EditValue.AsString() ,
                                                        txtKind1.Text , txtKind2.Text , Mth1 , Mth2 ,
                                                        ddlOsfOrderType.EditValue.AsString() , ddlOsfOrderCond.EditValue.AsString() , DetailLevelList , IsLevelNull);
            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!" , txtStartDate.DateTimeValue.ToString("yyyyMM") , reportId , ReportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.Export Csv
            ExportCsv(dtTarget , excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected ResultStatus wf_30681_d_mtf() {
         try {

            string reportId = "30681_d_mtf";
            string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                                                      string.Format("{0}_{1}_成交委託明細.csv" , reportId , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

            //1.get dataTable
            DataTable dtTarget = dao30681.d_30681_d_mtf(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , ddlScCode.EditValue.AsString() ,
                                                        txtKind1.Text , txtKind2.Text , Mth1 , Mth2 ,
                                                        ddlOsfOrderType.EditValue.AsString() , ddlOsfOrderCond.EditValue.AsString() , DetailLevelList , IsLevelNull);
            if (dtTarget.Rows.Count <= 0) {
               //MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!", txtStartDate.DateTimeValue.ToString("yyyyMM"), reportId, ReportName),GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.Export Csv
            ExportCsv(dtTarget , excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected ResultStatus wf_30681_d_mtf_new() {
         try {

            string reportId = "30681_d_mtf_new";
            string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                                                      string.Format("{0}_{1}_成交委託明細.csv" , reportId , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

            //1.get dataTable
            DataTable dtTarget = dao30681.d_30681_d_mtf_new(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , ddlScCode.EditValue.AsString() ,
                                                           txtKind1.Text , txtKind2.Text , Mth1 , Mth2 ,
                                                           ddlOsfOrderType.EditValue.AsString() , ddlOsfOrderCond.EditValue.AsString() , DetailLevelList , IsLevelNull);
            if (dtTarget.Rows.Count <= 0) {
               //MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!", txtStartDate.DateTimeValue.ToString("yyyyMM"), reportId, ReportName),GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.Export Csv
            ExportCsv(dtTarget , excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }


      /// <summary>
      /// 利用畫面上隱藏的grid,直接匯出CSV
      /// </summary>
      /// <param name="dtTarget"></param>
      protected void ExportCsv(DataTable dtTarget , string filename) {
         //2.設定gvExport
         gvExport.Columns.Clear();
         gvExport.OptionsBehavior.AutoPopulateColumns = true;
         gcExport.DataSource = dtTarget;
         gvExport.BestFitColumns();

         //3.export to csv (use GridControl)
         CsvExportOptionsEx options = new CsvExportOptionsEx();//ken,如果有需要細部設定csv屬性可以用這個
         //ken,數字格式不管的話,輸出時會自動加上千分位逗號,TextExportMode=Text會直接輸出
         options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
         options.TextExportMode = TextExportMode.Text;

         //ken,pb的時間輸出格式為 mm/dd/yy hh24:mi:ss ,直接在sql語法調整比較快

         gcExport.ExportToCsv(filename , options);

#if DEBUG
         if (FlagAdmin)
            System.Diagnostics.Process.Start(filename);
#endif
      }
      private void rdoReportType_SelectedIndexChanged(object sender , EventArgs e) {
         bool IsEnable = (sender as DevExpress.XtraEditors.RadioGroup).SelectedIndex == 0 ? false : true;

         ddlOsfOrderType.Enabled =
         ddlOsfOrderCond.Enabled =
         chkLevel.Enabled = IsEnable;

         r_frame2.Visible = IsEnable;
         panDetail.Visible = IsEnable;

         //ken,第一個check始終不能勾
         chkLevel.Items[0].Enabled = false;

      }

   }
}