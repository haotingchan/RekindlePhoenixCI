using System.Collections.Generic;
using System.Data;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using Common;
using BaseGround.Report;
using BaseGround;
using System;
using DataObjects.Dao.Together;
using BaseGround.Shared;
using System.IO;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.Data;
using DevExpress.XtraEditors;
using System.Drawing;

namespace PhoenixCI.FormUI.Prefix5 {
   public partial class W50010 : FormParent {
      private D50010 dao50010;
      protected DataTable dtTarget;
      protected APDK daoAPDK;

      #region get UI Value
      public string paramKey {
         get {
            return string.IsNullOrEmpty(Prod_ct.EditValue.AsString()) ? "" : Prod_ct.EditValue.AsString();
         }
      }

      public string kindIdSt {
         get {
            return string.IsNullOrEmpty(Kind_id_st.EditValue.AsString()) ? "" : Kind_id_st.EditValue.AsString();

         }
      }

      public string kindIdO {
         get {
            return string.IsNullOrEmpty(Kind_id_O.EditValue.AsString()) ? "" : Kind_id_O.EditValue.AsString();
         }
      }

      public string prodSort {
         get {
            //return string.IsNullOrEmpty(Txt_prod_sort.EditValue.AsString()) ? "" :
            //   "and AMMD_PROD_ID like '" + Txt_prod_sort.EditValue.AsString() + "%'";
            return string.IsNullOrEmpty(Txt_prod_sort.EditValue.AsString()) ? "" : Txt_prod_sort.EditValue.AsString();

         }
      }

      public string fcmSNo {
         get {
            return string.IsNullOrEmpty(Fcm_SNo.EditValue.AsString()) ? "" : Fcm_SNo.EditValue.AsString();

         }
      }

      public string fcmENo {
         get {
            return string.IsNullOrEmpty(Fcm_ENo.EditValue.AsString()) ? "" : Fcm_ENo.EditValue.AsString();

         }
      }

      public string printSort {
         get {
            return PrintSort.EditValue.AsString();
         }
      }

      public string matketTime {
         get {
            return MarketTime.EditValue.AsString();
         }
      }
      #endregion

      public W50010(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao50010 = new D50010();
         TxtDate.DateTimeValue = GlobalInfo.OCF_DATE;
         daoAPDK = new APDK();

         //設定下拉選單
         Fcm_SNo.SetDataTable(new AMPD().ListByFcmAccNo(), "AMPD_FCM_NO", "CP_DISPLAY", TextEditStyles.DisableTextEditor, "");
         Fcm_SNo.EditValue = "";
         Fcm_ENo.SetDataTable(new AMPD().ListByFcmAccNo(), "AMPD_FCM_NO", "CP_DISPLAY", TextEditStyles.DisableTextEditor, "");
         Fcm_ENo.EditValue = "";
         Prod_ct.SetDataTable(daoAPDK.ListParamKey(), "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.DisableTextEditor, null);
         Prod_ct.EditValue = " ";
         Kind_id_st.SetDataTable(daoAPDK.ListKind2(), "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.DisableTextEditor, null);
         Kind_id_st.EditValue = " ";
         Kind_id_O.SetDataTable(daoAPDK.ListKindId(), "APDK_KIND_ID", "APDK_KIND_ID", TextEditStyles.DisableTextEditor, null);
         Kind_id_O.EditValue = " ";


         //排序方式 下拉選單
         List<LookupItem> markret = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "O", DisplayMember = "一般"},
                                        new LookupItem() { ValueMember = "AH", DisplayMember = "盤後" }};
         Extension.SetDataTable(MarketTime, markret, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         MarketTime.EditValue = "O";
         List<LookupItem> printSort = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "M", DisplayMember = "造市者"},
                                        new LookupItem() { ValueMember = "P", DisplayMember = "商品" }};
         Extension.SetDataTable(PrintSort, printSort, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         PrintSort.EditValue = "M";


         //加入事件
         MarketTime.EditValueChanged += MarketTime_EditValueChanged;
      }

      protected override ResultStatus Retrieve() {
         string[] showColCaption = {"期貨商", $"期貨商{Environment.NewLine}名稱", "帳號","",$"商品{Environment.NewLine}名稱",
                                    $"報價{Environment.NewLine}時間", $"最接近報價{Environment.NewLine}詢價時間","",
                                    $"尋報價{Environment.NewLine}時間差(秒)", $"報價維持{Environment.NewLine}時間(秒)",
                                    $"報價{Environment.NewLine}有效", $"計入維持{Environment.NewLine}時間(秒)", $"價差{Environment.NewLine}權數",
                                    $"數量{Environment.NewLine}權數", $"標的{Environment.NewLine}權數","績效",$"報價{Environment.NewLine}單別","" };

         IGridData50010 gridData = dao50010.CreateGridData(dao50010.GetType(), matketTime);
         Q50010 q50010 = new Q50010(TxtDate.DateTimeValue, paramKey, kindIdSt, kindIdO, prodSort, fcmSNo, fcmENo);
         dtTarget = gridData.GetData(q50010);
         gcMain.DataSource = dtTarget;
         gcPrint.DataSource = dtTarget;

         if (dtTarget != null) {
            if (dtTarget.Rows.Count == 0) {
               _ToolBtnExport.Enabled = false;
               _ToolBtnPrintAll.Enabled = false;
               gcMain.Visible = false;
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
               return ResultStatus.Fail;
            }
         }

         //設定 column caption
         foreach (DataColumn dc in dtTarget.Columns) {
            gvMain.SetColumnCaption(dc.ColumnName, showColCaption[dtTarget.Columns.IndexOf(dc)]);
            gvMain.Columns[dc.ColumnName].OptionsColumn.AllowEdit = false;
            gvMain.Columns[dc.ColumnName].AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
            gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
            gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.False;

            gvPrint.SetColumnCaption(dc.ColumnName, showColCaption[dtTarget.Columns.IndexOf(dc)]);
            gvPrint.Columns[dc.ColumnName].OptionsColumn.AllowEdit = false;
            gvPrint.Columns[dc.ColumnName].AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
            //gvPrint.Columns[dc.ColumnName].AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
            gvPrint.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.False;

            //前4個欄位合併 (一樣的值不顯示)
            if (dc.Ordinal < 5) {
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.VAlignment = VertAlignment.Top;
               gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.True;

               gvPrint.Columns[dc.ColumnName].AppearanceCell.TextOptions.VAlignment = VertAlignment.Top;
               gvPrint.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.True;
            }
         }

         //隱藏欄位
         gvMain.Columns["AMMD_PROD_TYPE"].Visible = false;
         gvMain.Columns["AMMD_M_TIME"].Visible = false;
         gvMain.Columns["AMMD_Q_NO"].Visible = false;

         gvMain.Columns["AMMD_W_TIME"].DisplayFormat.FormatType = FormatType.DateTime;
         gvMain.Columns["AMMD_W_TIME"].DisplayFormat.FormatString = "HH:mm:ss";
         gvMain.Columns["AMMD_BRK_NO"].SortOrder = ColumnSortOrder.Ascending;

         gvPrint.Columns["AMMD_PROD_TYPE"].Visible = false;
         gvPrint.Columns["AMMD_M_TIME"].Visible = false;
         gvPrint.Columns["AMMD_Q_NO"].Visible = false;

         gvPrint.Columns["AMMD_W_TIME"].DisplayFormat.FormatType = FormatType.DateTime;
         gvPrint.Columns["AMMD_W_TIME"].DisplayFormat.FormatString = "HH:mm:ss";
         gvPrint.Columns["AMMD_PROD_ID"].VisibleIndex = 0;

         gvMain.Columns["AMMD_PROD_ID"].VisibleIndex = printSort == "P" ? 0 : 4;


         gvMain.ColumnPanelRowHeight = 40;
         gvMain.OptionsView.AllowCellMerge = true;
         gvMain.BestFitColumns();

         gvPrint.Columns["AMMD_BRK_NO"].SortOrder = ColumnSortOrder.Ascending;
         gvPrint.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
         gvPrint.AppearancePrint.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;
         gvPrint.ColumnPanelRowHeight = 40;
         gvPrint.AppearancePrint.HeaderPanel.Font = new Font("Microsoft YaHei", gvPrint.Appearance.HeaderPanel.Font.Size);
         gvPrint.AppearancePrint.Row.Font = new Font("Microsoft YaHei", 11);
         gvPrint.OptionsPrint.AllowMultilineHeaders = true;
         gvPrint.OptionsView.AllowCellMerge = true;
         gvPrint.BestFitColumns();

         _ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         GridHelper.SetCommonGrid(gvMain);

         gcMain.Visible = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         string fileName = string.Format("{0}_{1}.csv", _ProgramID, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
         string filepath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, fileName);
         string[] exportColCaption = {"期貨商", "帳號", "期貨商名稱","期貨/選擇權","商品", "報價時間", "最接近報價詢價時間",
            "最小成交回報檔時間", "r_second","m_second", "有效報價", "計入維持時間(秒)", "價差權數", "數量權數", "標的權數","績效","報價單別" ,"單號"};

         try {
            gcExport.DataSource = dtTarget;
            dtTarget.Columns["AMMD_ACC_NO"].SetOrdinal(1);

            foreach (DataColumn dc in dtTarget.Columns) {
               gvExport.SetColumnCaption(dc.ColumnName, exportColCaption[dtTarget.Columns.IndexOf(dc)]);
            }

            gvExport.Columns["AMMD_RESULT"].DisplayFormat.FormatType = FormatType.Numeric;
            gvExport.Columns["AMMD_RESULT"].DisplayFormat.FormatString = "d2";
            gvExport.Columns["AMMD_KEEP_TIME"].DisplayFormat.FormatString = "d2";

            gvExport.Columns["AMMD_W_TIME"].DisplayFormat.FormatType = FormatType.DateTime;
            gvExport.Columns["AMMD_W_TIME"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss:fff";

            gvExport.Columns["AMMD_R_TIME"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";

            gvExport.Columns["AMMD_M_TIME"].DisplayFormat.FormatType = FormatType.DateTime;
            gvExport.Columns["AMMD_M_TIME"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss:fff";

            gvExport.Columns["AMMD_ACC_NO"].VisibleIndex = 1;

            gvExport.ExportToCsv(filepath);
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcPrint, _ProgramID, this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
            _ReportHelper.LeftMemo = GenPrintMemo();
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();//如果有夜盤會特別標註
            _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnRetrieve.Enabled = true;


         return ResultStatus.Success;
      }

      private void MarketTime_EditValueChanged(object sender, EventArgs e) {
         LookUpEdit lookupItem = sender as LookUpEdit;
         string marktCodeFilter = "AND APDK_MARKET_CODE in ('1',' ')";

         if (lookupItem.EditValue.AsString() == "AH") {
            Prod_ct.SetDataTable(daoAPDK.ListParamKey(marktCodeFilter), "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.DisableTextEditor, null);
            Kind_id_st.SetDataTable(daoAPDK.ListKind2(marktCodeFilter), "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.DisableTextEditor, null);
            Kind_id_O.SetDataTable(daoAPDK.ListKindId(marktCodeFilter), "APDK_KIND_ID", "APDK_KIND_ID", TextEditStyles.DisableTextEditor, null);
         } else {
            Prod_ct.SetDataTable(daoAPDK.ListParamKey(), "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.DisableTextEditor, null);
            Kind_id_st.SetDataTable(daoAPDK.ListKind2(), "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.DisableTextEditor, null);
            Kind_id_O.SetDataTable(daoAPDK.ListKindId(), "APDK_KIND_ID", "APDK_KIND_ID", TextEditStyles.DisableTextEditor, null);
         }
         Prod_ct.EditValue = " ";
         Kind_id_st.EditValue = " ";
         Kind_id_O.EditValue = " ";
      }

      private string GenPrintMemo() {
         string fcm = "造市者:" + Fcm_SNo.EditValue.AsString() + "~" + Fcm_ENo.EditValue.AsString() + ",";

         if (Fcm_SNo.EditValue.AsString() == Fcm_ENo.EditValue.AsString()) fcm = Fcm_SNo.EditValue.AsString() + ",";


         if (string.IsNullOrEmpty(Fcm_SNo.EditValue.AsString()) && string.IsNullOrEmpty(Fcm_ENo.EditValue.AsString())) {
            fcm = "";
         }

         fcm = fcm.TrimEnd('~');

         string prodGroup = string.IsNullOrEmpty(Prod_ct.EditValue.AsString()) ? "" : "商品群組:" + Prod_ct.EditValue.AsString() + ",";
         string prodCt = string.IsNullOrEmpty(Kind_id_O.EditValue.AsString()) ? "" : "造市商品:" + Kind_id_O.EditValue.AsString() + ",";
         string kindIdSt = string.IsNullOrEmpty(Kind_id_st.EditValue.AsString()) ? "" : "二碼商品(個股):" + Kind_id_st.EditValue.AsString() + ",";
         string marketTime = string.IsNullOrEmpty(MarketTime.EditValue.AsString()) ? "" : MarketTime.Text + "交易時段,";
         string prodId = string.IsNullOrEmpty(Txt_prod_sort.EditValue.AsString()) ? "" : "商品序列:" + Txt_prod_sort.EditValue.AsString() + ",";

         string printMemo = "報表條件: " + fcm + prodGroup + prodCt + kindIdSt + marketTime + prodId;
         printMemo = printMemo.TrimEnd(',');

         return printMemo;
      }
   }
}