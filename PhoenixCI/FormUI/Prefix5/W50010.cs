using System.Collections.Generic;
using System.Data;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using DevExpress.Spreadsheet;
using BusinessObjects;
using Common;
using BaseGround.Report;
using BaseGround;
using System.Data.Common;
using static BaseGround.Report.ReportHelper;
using System.Windows.Forms;
using PhoenixCI.Report;
using DevExpress.XtraPrinting.Caching;
using System;
using DataObjects.Dao.Together;
using BaseGround.Shared;
using System.Linq;
using System.IO;
using DevExpress.XtraEditors.Controls;

namespace PhoenixCI.FormUI.Prefix5
{
   public partial class W50010 : FormParent
   {
      private D50010 dao50010;

      protected class LookupItem {
         public string ValueMember { get; set; }
         public string DisplayMember { get; set; }
      }

      public W50010(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao50010 = new D50010();
         TxtDate.DateTimeValue = GlobalInfo.OCF_DATE;
         APDK daoAPDK = new APDK();

         //設定下拉選單
         Fcm_SNo.SetDataTable(new AMPD().ListByFcmAccNo(), "AMPD_FCM_NO", "cp_display", TextEditStyles.DisableTextEditor, "");
         Fcm_SNo.EditValue = "";
         Fcm_ENo.SetDataTable(new AMPD().ListByFcmAccNo(), "AMPD_FCM_NO", "cp_display", TextEditStyles.DisableTextEditor, "");
         Fcm_ENo.EditValue = "";
         Prod_ct.SetDataTable(daoAPDK.dw_prod_500xx("APDK_PROD_TYPE,APDK_PARAM_KEY", "'%','%'"), "APDK_PARAM_KEY", "APDK_PARAM_KEY",
            TextEditStyles.DisableTextEditor, null);
         Prod_ct.EditValue = "%";
         Kind_id_st.SetDataTable(daoAPDK.dw_prod_500xx("APDK_KIND_ID_STO", "'%'"), "APDK_KIND_ID_STO", "APDK_KIND_ID_STO",
            TextEditStyles.DisableTextEditor, null);
         Kind_id_st.EditValue = "%";
         Kind_id_O.SetDataTable(daoAPDK.dw_prod_500xx("APDK_PROD_TYPE,APDK_KIND_ID", "'%','%'"), "APDK_KIND_ID", "APDK_KIND_ID",
            TextEditStyles.DisableTextEditor, null);
         Kind_id_O.EditValue = "%";


         //排序方式 下拉選單
         List<LookupItem> markret = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "O", DisplayMember = "一般"},
                                        new LookupItem() { ValueMember = "N", DisplayMember = "盤後" }};
         Extension.SetDataTable(MarketTime, markret, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         MarketTime.EditValue = "O";
         List<LookupItem> printSort = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "M", DisplayMember = "造市者"},
                                        new LookupItem() { ValueMember = "P", DisplayMember = "商品" }};
         Extension.SetDataTable(PrintSort, printSort, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         PrintSort.EditValue = "M";

      }

      protected override ResultStatus Retrieve()
      {
         string paramKey = "%" + Prod_ct.EditValue.AsString() + "%";
         string kindIdSt = "%" + Kind_id_st.EditValue.AsString() + "%";
         string kindIdO = "%" + Kind_id_O.EditValue.AsString() + "%";
         string[] showColCaption = { "期貨商", "期貨商名稱", "帳號","商品名稱", "報價時間", "最接近報價詢價時間",
            "尋報價時間差(秒)", "報價維持時間(秒)", "報價有效", "計入維持時間(秒)", "價差權數", "數量權數", "標的權數","績效","報價單別" };

         DataTable dt = new DataTable();
         dt = dao50010.GetData("F", TxtDate.DateTimeValue, paramKey, kindIdSt, kindIdO, "%", "");
         gcMain.DataSource = dt;

         //設定 column caption
         foreach (DataColumn dc in dt.Columns) {
            gvMain.SetColumnCaption(dc.ColumnName, showColCaption[dt.Columns.IndexOf(dc)]);
            gvMain.Columns[dc.ColumnName].OptionsColumn.AllowEdit = false;
         }
         gvMain.BestFitColumns();
         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         string paramKey = "%" + Prod_ct.EditValue.AsString() + "%";
         string kindIdSt = "%" + Kind_id_st.EditValue.AsString() + "%";
         string kindIdO = "%" + Kind_id_O.EditValue.AsString() + "%";
         string fileName = string.Format("{0}_{1}.csv", _ProgramID, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
         string filepath= Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, fileName);

         string[] exportColCaption = { "期貨商", "帳號", "期貨商名稱","期貨/選擇權","商品", "報價時間", "最接近報價詢價時間",
            "最小成交回報檔時間", "r_second","m_second", "有效報價", "計入維持時間(秒)", "價差權數", "數量權數", "標的權數","績效","報價單別" ,"單號"};

         DataTable dtExport = new DataTable();
         dtExport = dao50010.GetExportData("F", TxtDate.DateTimeValue, "%", "%", "%", "%", "");
         gcExport.DataSource = dtExport;

         foreach (DataColumn dc in dtExport.Columns) {
            gvExport.SetColumnCaption(dc.ColumnName, exportColCaption[dtExport.Columns.IndexOf(dc)]);
         }

         gvExport.Columns["AMMD_RESULT"].DisplayFormat.FormatString = "n2";
         gvExport.Columns["AMMD_W_TIME"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
         gvExport.Columns["AMMD_R_TIME"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
         gvExport.Columns["AMMD_M_TIME"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";

         gvExport.ExportToCsv(filepath);

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }
   }
}