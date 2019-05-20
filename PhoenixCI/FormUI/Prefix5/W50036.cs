using System;
using System.Collections.Generic;
using System.Data;
using BaseGround;
using DataObjects.Dao.Together.SpecificDao;
using System.Data.Common;
using BusinessObjects.Enums;
using Common;
using BusinessObjects;
using DevExpress.Spreadsheet;
using BaseGround.Report;
using System.IO;
using static BaseGround.Report.ReportHelper;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Caching;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors.Repository;
/// <summary>
/// John,20190129
/// </summary>
namespace PhoenixCI.FormUI.Prefix5
{
   /// <summary>
   /// 造市者合格檔數及績效表
   /// </summary>
   public partial class W50036 : FormParent
   {
      private D50036 dao50036;
      private DataTable is_dw_name { get; set; }
      private defReport defReport;
      public W50036(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;

         dao50036 = new D50036();
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
         w500xx.WfGbReportType("M");
         w500xx.WfGbGroup(true, true, "2");
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
         base.Retrieve();
         _ToolBtnExport.Enabled = true;
         if (!BeforeRetrieve()) return ResultStatus.Fail;
         w500xx.WfLinqSyntaxSelect(is_dw_name);
         if (w500xx.EndRetrieve(is_dw_name)) {
            reportView();
         }
         return ResultStatus.Success;
      }

      private void reportView()
      {
         DataTable dt = is_dw_name;
         List<ReportProp> caption = new List<ReportProp>{
            new ReportProp{DataColumn=ExtensionCommon.rowindex,Caption= "筆數",CellWidth=146} ,
            new ReportProp{DataColumn="AMM0_YMD",Caption="日期",CellWidth=224} ,
            new ReportProp{DataColumn="AMM0_BRK_NO",Caption="期貨商代號",CellWidth=270},
            new ReportProp{DataColumn = "BRK_ABBR_NAME", Caption ="期貨商名稱",CellWidth=530},
            new ReportProp{DataColumn = "AMM0_ACC_NO",Caption="投資人帳號",CellWidth=325},
            new ReportProp{DataColumn = "AMM0_PROD_ID",Caption="商品名稱",CellWidth=389},
            new ReportProp{DataColumn = "AMM0_MMK_QNTY",Caption="造市量",CellWidth=407},
            new ReportProp{DataColumn = "AMM0_TOT_QNTY",Caption="造市者總成交量",CellWidth=407},
            new ReportProp{DataColumn = "AMM0_MMK_RATE",Caption="有效報/詢價比例",CellWidth=302},
            new ReportProp{DataColumn = "AMM0_KEEP_TIME",Caption="每日平均維持時間(分)",CellWidth=407},
            new ReportProp{DataColumn = "AMM0_RESULT",Caption="績效分數",CellWidth=576},
            new ReportProp{DataColumn = "AMM0_CONTRACT_CNT",Caption="合格檔數",CellWidth=325}
            };
         dt = ExtensionCommon.AddSeriNumToDataTable(dt);
         defReport = new defReport(dt, caption);
         documentViewer1.DocumentSource = defReport;
         defReport.CreateDocument(true);
      }

      protected bool BeforeRetrieve()
      {
         if (!w500xx.StartRetrieve()) {
            _ToolBtnExport.Enabled = false;
            return false;
         }
         /* 報表內容 */
         if (w500xx.gb_detial.EditValue.Equals("rb_gdate")) {
            is_dw_name = dao50036.ListAMMO(w500xx.Sdate, w500xx.Edate, w500xx.SumType, w500xx.SumSubType);
         }
         //else {

         //   is_dw_name = "d_" + gs_txn_id + "_accu";
         //}
         if (w500xx.gb_market.EditValue.Equals("rb_market_1")) {
            is_dw_name = dao50036.ListAMMOAH(w500xx.Sdate, w500xx.Edate, w500xx.SumType, w500xx.SumSubType);
         }
         return true;
      }

      protected override ResultStatus CheckShield()
      {
         base.CheckShield();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall)
      {
         base.Save(pokeBall);

         return ResultStatus.Success;
      }

      protected override ResultStatus Run(PokeBall args)
      {
         base.Run(args);

         return ResultStatus.Success;
      }

      protected override ResultStatus Import()
      {
         base.Import();

         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         base.Export();
         string ls_rpt_name = "造市者報表";
         string ls_rpt_id = _ProgramID;
         w500xx.StartExport(ls_rpt_id, ls_rpt_name);
         BeforeRetrieve();
         string ls_filename;
         ls_filename = _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss");
         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, ls_filename + ".xlsx");
         /******************
         開啟檔案
         ******************/
         Workbook workbook = new Workbook();

         /******************
         讀取資料
         ******************/
         DataTable ids_1 = is_dw_name;
         if (ids_1.Rows.Count <= 0) {
            w500xx.EndExport();
            return ResultStatus.Success;
         }

         //// Get its XLSX export options. 
         XlsxExportOptions xlsxOptions = defReport.ExportOptions.Xlsx;
         xlsxOptions.SheetName = ls_filename;
         // Export the report to XLSX. 
         defReport.ExportToXlsx(destinationFilePath);

         w500xx.EndExport();
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         CommonReportLandscapeA4 reportLandscapeA4 = new CommonReportLandscapeA4();
         XtraReport xtraReport = reportHelper.CreateCompositeReport(defReport, reportLandscapeA4);

         reportHelper.Create(xtraReport);
         //reportHelper.Preview();
         base.Print(reportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow()
      {
         base.InsertRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow()
      {
         base.DeleteRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         return base.BeforeClose();
      }
   }

}