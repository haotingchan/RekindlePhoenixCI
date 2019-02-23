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
      private DbDataAdapter is_dw_name { get; set; }
      public W50036(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;

         dao50036 = new D50036();
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();
         w500xx.BeforeOpen();
         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         w500xx.wf_gb_detial(false, false, "1");
         w500xx.wf_gb_print_sort(false, false, "1");
         w500xx.wf_gb_report_type("M");
         w500xx.wf_gb_group(true, true, "2");
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
         w500xx.wf_select_sqlcode(is_dw_name);
         if (w500xx.RetrieveAfter(is_dw_name)) {
            reportView();
         }
         return ResultStatus.Success;
      }

      private void reportView()
      {
         DataTable dt = new DataTable();
         is_dw_name.Fill(dt);
         List<ReportProp> caption = new List<ReportProp>{
            new ReportProp{dataColumn=ExtensionCommon.rowindex,caption= "筆數",cellWidth=146} ,
            new ReportProp{dataColumn="AMM0_YMD",caption="日期",cellWidth=224} ,
            new ReportProp{dataColumn="AMM0_BRK_NO",caption="期貨商代號",cellWidth=270},
            new ReportProp{dataColumn = "BRK_ABBR_NAME", caption ="期貨商名稱",cellWidth=530},
            new ReportProp{dataColumn = "AMM0_ACC_NO",caption="投資人帳號",cellWidth=325},
            new ReportProp{dataColumn = "AMM0_PROD_ID",caption="商品名稱",cellWidth=389},
            new ReportProp{dataColumn = "AMM0_MMK_QNTY",caption="造市量",cellWidth=407},
            new ReportProp{dataColumn = "AMM0_TOT_QNTY",caption="造市者總成交量",cellWidth=407},
            new ReportProp{dataColumn = "AMM0_MMK_RATE",caption="有效報/詢價比例",cellWidth=302},
            new ReportProp{dataColumn = "AMM0_KEEP_TIME",caption="每日平均維持時間(分)",cellWidth=407},
            new ReportProp{dataColumn = "AMM0_RESULT",caption="績效分數",cellWidth=576},
            new ReportProp{dataColumn = "AMM0_CONTRACT_CNT",caption="合格檔數",cellWidth=325}
            };
         dt = ExtensionCommon.AddSeriNumToDataTable(dt);
         defReport = new defReport(dt, caption);
         //documentViewer1.DocumentSource = defReport;
         //defReport.CreateDocument(true);
         var storage = new MemoryDocumentStorage();
         var report = defReport;
         cachedReportSource1 = new CachedReportSource(report, storage);

         documentViewer1.DocumentSource = cachedReportSource1;
         cachedReportSource1.CreateDocumentAsync();
      }

      protected bool BeforeRetrieve()
      {
         if (!w500xx.Retrieve()) return false;
         /* 報表內容 */
         if (w500xx.gb_detial.EditValue.Equals("rb_gdate")) {
            is_dw_name = dao50036.ListAMMO(w500xx.is_sdate, w500xx.is_edate, w500xx.is_sum_type, w500xx.is_sum_subtype);
         }
         //else {

         //   is_dw_name = "d_" + gs_txn_id + "_accu";
         //}
         if (w500xx.gb_market.EditValue.Equals("rb_market_1")) {
            is_dw_name = dao50036.ListAMMOAH(w500xx.is_sdate, w500xx.is_edate, w500xx.is_sum_type, w500xx.is_sum_subtype);
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
         w500xx.BeforeExport(ls_rpt_id, ls_rpt_name);
         BeforeRetrieve();
         string ls_filename;
         ls_filename = _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + ".xls";
         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, ls_filename);
         /******************
         開啟檔案
         ******************/
         Workbook workbook = new Workbook();
         //判斷檔案是否存在,不存在就開一個新檔案
         if (!File.Exists(destinationFilePath)) {
            File.Create(destinationFilePath).Close();
         }
         //reportHelper.Export(FileType.XLS, destinationFilePath);
         workbook.LoadDocument(destinationFilePath);
         /******************
         讀取資料
         ******************/
         DataTable ids_1 = new DataTable();
         is_dw_name.Fill(ids_1);
         if (ids_1.Rows.Count <= 0) {
            w500xx.AfterExport();
            return ResultStatus.Success;
         }
         defReport.ExportToXls(destinationFilePath);
         /******************
         切換Sheet
         
         Worksheet worksheet = workbook.Worksheets[0];
         
         int rowNum = ids_1.Rows.Count;
         int columnNum = ids_1.Columns.Count;
         int rowIndex = 0;
         int columnIndex = 0;
         int k = 0;
         worksheet.Rows[0][k++].Value = "日期";
         worksheet.Rows[0][k++].Value = "期貨商代號";
         worksheet.Rows[0][k++].Value = "期貨商名稱";
         worksheet.Rows[0][k++].Value = "投資人帳號";
         worksheet.Rows[0][k++].Value = "amm0_prod_type";
         worksheet.Rows[0][k++].Value = "商品名稱";
         worksheet.Rows[0][k++].Value = "造市量";
         worksheet.Rows[0][k++].Value = "造市者總成交量";
         worksheet.Rows[0][k++].Value = "有效報/詢價比例";
         worksheet.Rows[0][k++].Value = "每日平均維持時間(分)";
         worksheet.Rows[0][k++].Value = "績效分數";
         worksheet.Rows[0][k++].Value = "合格檔數";
         k = 0;
         defReport.ExportToXls(destinationFilePath);
         //將DataTable中的數據導入Excel中
         for (int i = 0; i < rowNum; i++) {
            rowIndex++;
            columnIndex = 0;
            for (int j = 0; j < columnNum; j++) {
               worksheet.Cells[rowIndex, columnIndex].Value = ids_1.Rows[i][j].ToString();
               columnIndex++;
            }
         }
         workbook.SaveDocument(destinationFilePath);
         ******************/
         w500xx.AfterExport();
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