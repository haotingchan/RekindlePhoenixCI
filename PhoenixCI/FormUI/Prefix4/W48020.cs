using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;

/// <summary>
/// ken, 2019/3/20
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 最小風險價格係數歷次調整資料查詢
   /// </summary>
   public partial class W48020 : FormParent {
      protected D48020 dao48020;
      protected static string ChooseSingleKind = "選單一契約";
      protected DataTable dtTarget;
      private RepositoryItemLookUpEdit lupSubType;

      public W48020(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         GridHelper.SetCommonGrid(gvMain);
         gvMain.OptionsBehavior.Editable = false;
         gvMain.OptionsBehavior.AutoPopulateColumns = true;

         GridHelper.SetCommonGrid(gvExport);
         gvExport.OptionsBehavior.Editable = false;
         gvExport.OptionsBehavior.AutoPopulateColumns = true;

         dao48020 = new D48020();

         labKind.Visible = false;
         ddlKind.Visible = false;
      }

      protected override ResultStatus Open() {
         base.Open();
         txtSDate.EditValue = GlobalInfo.OCF_DATE;
         lupSubType = new RepositoryItemLookUpEdit();

#if DEBUG
         //txtSDate.DateTimeValue = DateTime.ParseExact("2018/06/15", "yyyy/MM/dd", null);
         //this.Text += "(開啟測試模式),ocfDate=2018/06/15";
#endif

         //1.契約類別 下拉選單(ken,用48010帶入才可以...)
         //DataTable dtSubType = new COD().ListByCol("48010" , "PDK_SUBTYPE         " , "全選" , "%"); //第一行全選 + COD_ID / COD_DESC / COD_SEQ_NO
         //ken,特殊,再往上新增一筆 
         //DataRow drTemp = dtSubType.NewRow();
         //drTemp["COD_ID"] = " ";
         //drTemp["COD_DESC"] = ChooseSingleKind;
         //drTemp["COD_SEQ_NO"] = -1;
         //dtSubType.Rows.InsertAt(drTemp , 0);
         //Extension.SetDataTable(ddlSubType , dtSubType , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");

         DataTable dtSubType = new CODW().ListLookUpEdit("HCPR" , "CPR_PROD_SUBTYPE");
         Extension.SetDataTable(ddlSubType , dtSubType , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");

         lupSubType.SetColumnLookUp(dtSubType , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , null);
         gcMain.RepositoryItems.Add(lupSubType);

         //ken,設定選單事件
         this.ddlSubType.EditValueChanged += new System.EventHandler(this.ddlSubType_EditValueChanged);

         //2.契約代號 下拉選單
         DataTable dtKind = dao48020.ListKind(); //MGT2_SEQ_NO/MGT2_KIND_ID/MGT2_PROD_SUBTYPE
         Extension.SetDataTable(ddlKind , dtKind , "MGT2_KIND_ID" , "MGT2_KIND_ID" , TextEditStyles.DisableTextEditor , "");

         //3.資料內容 下拉選單
         //List<LookupItem> lstData = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "KeyInfo", DisplayMember = "1.重點資料"},
         //                               new LookupItem() { ValueMember = "Detail", DisplayMember = "2.明細資料" }};

         DataTable dtData = new CODW().ListLookUpEdit("HCPR" , "DATA_TYPE");
         Extension.SetDataTable(ddlData , dtData , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");

         //ken,設定選單事件
         this.ddlData.EditValueChanged += new System.EventHandler(this.ddlData_EditValueChanged);

         //4.排序方式 下拉選單
         //List<LookupItem> lstSort = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "DATE", DisplayMember = "1.依系統生效日期排序"},
         //                               new LookupItem() { ValueMember = "KIND", DisplayMember = "2.依契約類別排序" }};

         DataTable dtSort = new CODW().ListLookUpEdit("HCPR" , "SORT_TYPE");
         Extension.SetDataTable(ddlSort , dtSort , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         //設定日期預設值
         txtSDate.Text = "1998";
         txtEDate.Text = DateTime.Now.ToString("yyyy");

         //設定下拉選單預設值
         ddlSubType.ItemIndex = 1;//全選
         ddlKind.ItemIndex = 0;
         ddlData.ItemIndex = 0;//1.重點資料
         ddlSort.ItemIndex = 0;//2.依契約類別排序

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
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {
         try {
            labMsg.Visible = false;

            //1.設定準備傳入的參數
            string subType = ddlSubType.EditValue.AsString();
            string kindId = "%";
            if (ddlSubType.Text == ChooseSingleKind) {
               subType = "%";
               kindId = ddlKind.EditValue.AsString() + "%";
            }//if (ddlSubType.Text == ChooseSingleKind) {

            //2.改成彈性工廠寫法(KeyInfo撈9顯示6個欄位,Detail撈13顯示10個欄位)
            I48020GridData gridData = dao48020.CreateGridData(dao48020.GetType() , ddlData.EditValue.AsString());
            DateTime startDate = DateTime.ParseExact(txtSDate.Text + "/01/01" , "yyyy/MM/dd" , null);
            DateTime endDate = DateTime.ParseExact(txtEDate.Text + "/12/31" , "yyyy/MM/dd" , null);
            Q48020 q48020 = new Q48020(startDate , endDate , subType , kindId , ddlSort.EditValue.AsString());
            dtTarget = gridData.ListAll(q48020);

            DataTable dtExport = dao48020.ListAll2(startDate , endDate , subType , kindId , ddlSort.EditValue.AsString() , "N");

            //3.1開始設定Grid
            gcMain.Visible = true;
            gvMain.Columns.Clear();
            gcMain.DataSource = dtExport;

            gvMain.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            gvMain.AppearancePrint.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;
            gvMain.AppearancePrint.HeaderPanel.Font = new Font("Microsoft YaHei" , 11);

            gvMain.AppearancePrint.Row.Font = new Font("Microsoft YaHei" , 11);
            gvMain.OptionsPrint.AllowMultilineHeaders = true;
            gvMain.AppearancePrint.GroupRow.Font = new Font("Microsoft YaHei" , 11);
            gvMain.AppearancePrint.Row.TextOptions.HAlignment = HorzAlignment.Center;

            //3.2設定每個欄位的caption
            gvMain.SetColumnCaption("CPR_DATA_NUM" , "次數");
            gvMain.SetColumnCaption("CPR_PROD_SUBTYPE" , "契約類別");
            gvMain.Columns["CPR_PROD_SUBTYPE"].ColumnEdit = lupSubType;
            gvMain.SetColumnCaption("CPR_KIND_ID" , "契約代號");
            gvMain.SetColumnCaption("CPR_EFFECTIVE_DATE" , "系統生效日");
            gvMain.SetColumnCaption("CPR_PRICE_RISK_RATE" , "最小風險價格係數");

            RepositoryItemTextEdit priceRiskRate = new RepositoryItemTextEdit();
            gcMain.RepositoryItems.Add(priceRiskRate);
            gvMain.Columns["CPR_PRICE_RISK_RATE"].ColumnEdit = priceRiskRate;
            priceRiskRate.DisplayFormat.FormatType = FormatType.Numeric;
            priceRiskRate.DisplayFormat.FormatString = "P";

            gvMain.SetColumnCaption("CPR_APPROVAL_DATE" , "核定日期");

            gvMain.SetColumnCaption("CPR_APPROVAL_NUMBER" , "核定文號及日期");
            gvMain.SetColumnCaption("CPR_REMARK" , "備註");
            gvMain.SetColumnCaption("CPR_W_TIME" , "異動時間");
            gvMain.SetColumnCaption("CPR_W_USER_ID" , "異動人員");

            //3.3設定隱藏欄位
            //gvMain.Columns["CPR_PROD_SUBTYPE"].Visible = false;
            gvMain.Columns["MGT2_SEQ_NO"].Visible = false;//ken,從頭到尾都沒用到...
            gvMain.Columns["PROD_TYPE"].Visible = false;

            if (ddlData.EditValue.AsString() == "KeyInfo") {
               gvMain.Columns["CPR_APPROVAL_NUMBER"].Visible = false;
               gvMain.Columns["CPR_REMARK"].Visible = false;
               gvMain.Columns["CPR_W_TIME"].Visible = false;
               gvMain.Columns["CPR_W_USER_ID"].Visible = false;
            }

            //3.4統一設定欄位靠左靠右把一些欄位靠左
            gvMain.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;//設定全部欄位先置中
            gvMain.SetColumnHAlignment("CPR_APPROVAL_NUMBER" , DevExpress.Utils.HorzAlignment.Default);
            gvMain.SetColumnHAlignment("CPR_REMARK" , DevExpress.Utils.HorzAlignment.Default);

            //3.5設定每個column header是否自動折行
            //gvMain.SetColumnHeaderWrap("CPR_DATA_NUM" , 60);
            //gvMain.SetColumnHeaderWrap("COD_DESC" , 90);
            //gvMain.SetColumnHeaderWrap("CPR_KIND_ID" , 100);
            //gvMain.SetColumnHeaderWrap("CPR_PRICE_RISK_RATE" , 200);
            //gvMain.SetColumnHeaderWrap("CPR_APPROVAL_DATE" , 120);

            //3.6設定每個column是否自動折行
            gvMain.SetColumnWrap("CPR_APPROVAL_NUMBER" , 350);
            gvMain.SetColumnWrap("CPR_REMARK" , 160);

            //3.7調整gird欄位順序(這兩個欄位retrieve跟export的順序顛倒)
            gvMain.Columns["CPR_EFFECTIVE_DATE"].VisibleIndex = 3;
            gvMain.Columns["CPR_PRICE_RISK_RATE"].VisibleIndex = 4;

            //3.7設定每個column自動擴展
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Export() {
         //0.check
         if (dtTarget == null) {
            labMsg.Visible = true;
            labMsg.Text = "請先查詢,才可轉出資料";
            return ResultStatus.Fail;
         }
         if (dtTarget.Rows.Count <= 0) {
            labMsg.Visible = true;
            labMsg.Text = "無任何資料";
            return ResultStatus.Fail;
         }

         try {
            //1.開始轉出資料
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料轉出中........";
            this.Refresh();

            string subType = ddlSubType.EditValue.AsString();
            string kindId = "%";
            if (ddlSubType.Text == ChooseSingleKind) {
               subType = "%";
               kindId = ddlKind.EditValue.AsString() + "%";
            }//if (ddlSubType.Text == ChooseSingleKind)

            DateTime startDate = DateTime.ParseExact(txtSDate.Text + "/01/01" , "yyyy/MM/dd" , null);
            DateTime endDate = DateTime.ParseExact(txtEDate.Text + "/12/31" , "yyyy/MM/dd" , null);

            DataTable dtExport = dao48020.ListAll2(startDate , endDate , subType , kindId , ddlSort.EditValue.AsString() , "Y");

            if (ddlData.EditValue.AsString() == "KeyInfo") {
               dtExport.Columns.Remove("CPR_APPROVAL_NUMBER");
               dtExport.Columns.Remove("CPR_REMARK");
               dtExport.Columns.Remove("CPR_W_TIME");
               dtExport.Columns.Remove("CPR_W_USER_ID");
               dtExport.Columns.Remove("MGT2_SEQ_NO");
               dtExport.AcceptChanges();
            } else {
               dtExport.Columns.Remove("MGT2_SEQ_NO");
               dtExport.AcceptChanges();
            }

            //2.1 設定gvExport
            gvExport.Columns.Clear();
            gvExport.OptionsBehavior.AutoPopulateColumns = true;
            gcExport.DataSource = dtExport;
            gvExport.BestFitColumns();

            //2.2設定caption
            if (ddlData.EditValue.AsString() == "KeyInfo") {
               gvExport.SetColumnCaption("CPR_DATA_NUM" , "次數");
               gvExport.SetColumnCaption("CPR_PROD_SUBTYPE" , "契約類別");
               gvExport.SetColumnCaption("CPR_KIND_ID" , "契約代號");
               gvExport.SetColumnCaption("CPR_PRICE_RISK_RATE" , "最小風險價格係數");
               gvExport.Columns["CPR_PRICE_RISK_RATE"].DisplayFormat.FormatString = "##0.######";
               gvExport.SetColumnCaption("CPR_EFFECTIVE_DATE" , "系統生效日");
               gvExport.SetColumnCaption("CPR_APPROVAL_DATE" , "主管機關核准日期");
               gvExport.SetColumnCaption("PROD_TYPE" , "prod_type");
            } else {
               gvExport.SetColumnCaption("CPR_DATA_NUM" , "次數");
               gvExport.SetColumnCaption("CPR_PROD_SUBTYPE" , "契約類別(I指數C黃金R利率S股票)");
               gvExport.SetColumnCaption("CPR_KIND_ID" , "契約代號");
               gvExport.SetColumnCaption("CPR_PRICE_RISK_RATE" , "最小風險價格係數(已下市契約之最小風險價格係數顯示空白；有效契約之最小風險價格係數不可為空白)");
               gvExport.Columns["CPR_PRICE_RISK_RATE"].DisplayFormat.FormatString = "##0.######";
               gvExport.SetColumnCaption("CPR_EFFECTIVE_DATE" , "系統生效日");
               gvExport.SetColumnCaption("CPR_APPROVAL_DATE" , "主管機關核准日期");

               //只有detail才有以下這些欄位
               gvExport.SetColumnCaption("CPR_APPROVAL_NUMBER" , "主管機關核准文號");
               gvExport.SetColumnCaption("CPR_REMARK" , "備註");
               gvExport.SetColumnCaption("CPR_W_TIME" , "異動時間");
               gvExport.SetColumnCaption("CPR_W_USER_ID" , "異動人員");

               gvExport.SetColumnCaption("PROD_TYPE" , "prod_type");
            }

            //2.3統一設定欄位靠左靠右把一些欄位靠左(沒影響到excel)
            //gvExport.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;//設定全部欄位先置中
            //gvExport.SetColumnHAlignment("CPR_APPROVAL_NUMBER" , DevExpress.Utils.HorzAlignment.Default);
            //gvExport.SetColumnHAlignment("CPR_REMARK" , DevExpress.Utils.HorzAlignment.Default);

            //3.gird export to excel
            string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                                                        string.Format("{0}_{1}.xlsx" , _ProgramID , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));
            XlsxExportOptions options = new XlsxExportOptions();
            options.SheetName = "";
            options.ShowGridLines = true;
            gvExport.ExportToXlsx(excelDestinationPath , options);

            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            //ReportHelper reportHelper = new ReportHelper(PrintableComponent, _ReportID, _ReportTitle);
            //reportHelper.FilePath = _DefaultFileNamePath;
            //reportHelper.FileType = FileType.PDF;
            //reportHelper.IsPrintedFromPrintButton = true;

            ReportHelper _ReportHelper = reportHelper;

            if (ddlData.EditValue.AsString() == "KeyInfo") {
               //重點資料
               CommonReportPortraitA4 reportPortrait = new CommonReportPortraitA4();//設定為直向列印
               reportPortrait.printableComponentContainerMain.PrintableComponent = gcMain;
               reportPortrait.IsHandlePersonVisible = false;
               reportPortrait.IsManagerVisible = false;
               _ReportHelper.Create(reportPortrait);
            } else {
               //明細資料,欄位比較多
               CommonReportLandscapeA3 reportLandscape = new CommonReportLandscapeA3();//設定為橫向列印
               reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
               reportLandscape.IsHandlePersonVisible = false;
               reportLandscape.IsManagerVisible = false;
               _ReportHelper.Create(reportLandscape);
            }

            //寫一行標題的註解,通常是查詢條件
            string leftMemo = labDate.Text + txtSDate.Text.Trim() + labDateBetween.Text.Trim() + txtEDate.Text.Trim() + labDateUnit.Text + " , ";
            leftMemo += labSubType.Text + ddlSubType.Text + " , ";
            leftMemo += labData.Text + ddlData.Text + " , ";
            leftMemo += labSort.Text + ddlSort.Text;
            _ReportHelper.LeftMemo = leftMemo;

            _ReportHelper.Print();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      private void ddlSubType_EditValueChanged(object sender , EventArgs e) {
         DevExpress.XtraEditors.LookUpEdit ddl = (sender as DevExpress.XtraEditors.LookUpEdit);

         ddlKind.Visible = labKind.Visible = (ddl.Text == ChooseSingleKind ? true : false);
      }

      private void ddlData_EditValueChanged(object sender , EventArgs e) {
         //清除grid data
         dtTarget = null;
         gcMain.DataSource = dtTarget;
         gcMain.Visible = false;
      }
   }
}