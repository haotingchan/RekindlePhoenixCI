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
using DevExpress.XtraReports.UI;
using static BaseGround.Report.ReportHelper;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Caching;
using System;
using System.Linq;
using DevExpress.XtraPrinting;
/// <summary>
/// John,20190129
/// </summary>
namespace PhoenixCI.FormUI.Prefix5
{
   /// <summary>
   /// 造市者自行成交量統計表
   /// </summary>
   public partial class W50034 : FormParent
   {
      private D50034 dao50034;
      private DataTable is_dw_name { get; set; }
      private defReport _defReport;
      public W50034(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;

         dao50034 = new D50034();
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();
         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         w500xx.WfGrpDetial(false, false, "1");
         w500xx.WfGbPrintSort(false, false, "1");
         w500xx.WfGbReportType("D");
         w500xx.WfGbGroup(true, true, "3");
         w500xx.Open();

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnRetrieve.Enabled = true;
         //_ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
         //sw.Reset();//碼表歸零
         //sw.Start();//碼表開始計時
         base.Retrieve();
         _ToolBtnExport.Enabled = true;
         if (!BeforeRetrieve()) return ResultStatus.Fail;
         is_dw_name = w500xx.WfLinqSyntaxSelect(is_dw_name);
         if (w500xx.EndRetrieve(is_dw_name)) {
            reportView();
         }
         reportView();
         //string result1 = sw.Elapsed.TotalMilliseconds.ToString();
         //MessageBox.Show(result1, "執行時間");
         return ResultStatus.Success;
      }
      private void reportView()
      {
         DataTable dt = is_dw_name;
         List<ReportProp> caption = new List<ReportProp>{
            new ReportProp{ DataColumn=ExtensionCommon.rowindex,Caption= "筆數",CellWidth=40} ,
            new ReportProp{DataColumn="AMM0_YMD",Caption= "日期" ,CellWidth=65},
            new ReportProp{DataColumn="AMM0_BRK_NO",Caption= "期貨商        代號",CellWidth=70},
            new ReportProp{DataColumn="BRK_ABBR_NAME",Caption= "期貨商名稱" ,CellWidth=140},
            new ReportProp{DataColumn="AMM0_ACC_NO",Caption= "帳號" ,CellWidth=80},
            new ReportProp{DataColumn="AMM0_PROD_ID",Caption= "商品名稱",CellWidth=80},
            new ReportProp{DataColumn="AMM0_O_SUBTRACT_QNTY",Caption= "委託            自行成交量",CellWidth=80},
            new ReportProp{DataColumn="AMM0_Q_SUBTRACT_QNTY",Caption= "報價            自行成交量",CellWidth=80},
            new ReportProp{DataColumn="AMM0_IQM_SUBTRACT_QNTY",Caption= "不符合－報價自行成交量",CellWidth=80,HeaderFontSize=8 }
            };
         dt = ExtensionCommon.AddSeriNumToDataTable(dt);
         _defReport = new defReport(dt, caption);
         documentViewer1.DocumentSource = _defReport;
         _defReport.CreateDocument(true);
      }

      protected bool BeforeRetrieve()
      {
         if (!w500xx.StartRetrieve()) return false;
         /* 報表內容 */
         if (w500xx.gb_detial.EditValue.Equals("rb_gdate")) {
            is_dw_name = dao50034.ListAMMO(w500xx.Sdate, w500xx.Edate, w500xx.SumType, w500xx.SumSubType);
            //is_dw_name = dao50034.AMMO(w500xx.is_sdate, w500xx.is_edate, w500xx.is_sum_type, w500xx.is_sum_subtype);
         }
         //else {

         //   is_dw_name = "d_" + gs_txn_id + "_accu";
         //}
         if (w500xx.gb_market.EditValue.Equals("rb_market_1")) {
            is_dw_name = dao50034.ListAMMOAH(w500xx.Sdate, w500xx.Edate, w500xx.SumType, w500xx.SumSubType);
            //is_dw_name = dao50034.AMMOAH(w500xx.is_sdate, w500xx.is_edate, w500xx.is_sum_type, w500xx.is_sum_subtype);
         }
         return true;
      }

      protected override ResultStatus Export()
      {
         string ls_rpt_name = w500xx.ConditionText().Trim();
         string ls_rpt_id = _ProgramID;
         w500xx.StartExport(ls_rpt_id, ls_rpt_name);
         BeforeRetrieve();
         /******************
         複製檔案
         ******************/
         string ls_file = w500xx.wf_copyfile(ls_rpt_id);
         //ls_file = CopyExcelTemplateFile(ls_rpt_id, FileType.XLS);
         if (ls_file == "") {
            return ResultStatus.Fail;
         }
         w500xx.LogText = ls_file;


         /******************
         讀取資料
         ******************/
         DataTable ids_1 = ExtensionCommon.AddSeriNumToDataTable(is_dw_name);

         if (ids_1.Rows.Count <= 0) {
            w500xx.EndExport();
            return ResultStatus.Success;
         }

         if (ls_rpt_name == "") {
            ls_rpt_name = "報表條件：" + "(" + w500xx.DateText() + ")";
         }
         else {
            ls_rpt_name = w500xx.ConditionText().Trim() + " " + "(" + w500xx.DateText() + ")";
         }
         // Get its XLSX export options. 
         XlsExportOptions xlsOptions = _defReport.ExportOptions.Xls;
         xlsOptions.SheetName = _ProgramID;

         //Export the report to XLSX. 
         _defReport.ExportToXls(ls_file);

         /******************
         開啟檔案
         ******************/
         Workbook workbook = new Workbook();
         workbook.LoadDocument(ls_file);
         /******************
         切換Sheet
         ******************/
         Worksheet worksheet = workbook.Worksheets[0];
         for (int k = 0; k < 3; k++) {
            worksheet.Rows.Insert(0);
         }

         worksheet.Cells["F1"].Value = $"[{_ProgramID}]{_ProgramName}";
         worksheet.Cells["F3"].Value = ls_rpt_name;

         workbook.SaveDocument(ls_file);
         w500xx.EndExport();
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         CommonReportPortraitA4 reportPortraitA4 = new CommonReportPortraitA4();
         XtraReport xtraReport = reportHelper.CreateCompositeReport(_defReport, reportPortraitA4);
         string dateCondition = w500xx.DateText() == "" ? "" : "," + w500xx.DateText();
         reportHelper.LeftMemo = w500xx.ConditionText() + dateCondition;
         reportHelper.Create(xtraReport);

         //reportHelper.Preview();
         base.Print(reportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         is_dw_name.Clear();
         documentViewer1.DocumentSource = null;
         return ResultStatus.Success;
      }
   }
}