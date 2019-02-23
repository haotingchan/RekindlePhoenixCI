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
using DevExpress.XtraPrinting.Caching;
/// <summary>
/// John,20190129
/// </summary>
namespace PhoenixCI.FormUI.Prefix5
{
   /// <summary>
   /// 不符造市規定統計表
   /// </summary>
   public partial class W50032 : FormParent
   {
      private D50032 dao50032;
      private DbDataAdapter is_dw_name { get; set; }
      public W50032(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         
         this.Text = _ProgramID + "─" + _ProgramName;

         dao50032 = new D50032();
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
         //w500xx.wf_gb_detial(false, false, "1");
         //w500xx.wf_gb_print_sort(false, false, "1");
         //w500xx.wf_gb_report_type("D");
         //w500xx.wf_gb_group(true, true, "3");
         #region 隱藏控制項
         w500xx.gb.Text = string.Empty;
         w500xx.gb.Width = 329;
         w500xx.gb.Height = 60;
         w500xx.gb1.Visible = false;
         w500xx.gb2.Visible = false;
         w500xx.gb3.Visible = false;
         w500xx.gb4.Visible = false;
         w500xx.gb_report_type.Visible = false;
         #endregion
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
         _ToolBtnExport.Enabled = false;
         if (!BeforeRetrieve()) return ResultStatus.Fail;
         w500xx.wf_select_sqlcode(is_dw_name);
         if (w500xx.RetrieveAfter(is_dw_name)) {
            reportView();
         }
         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }
      private void reportView()
      {
         DataTable dt = new DataTable();
         is_dw_name.Fill(dt);
         List<ReportProp> caption = new List<ReportProp>{
            new ReportProp{ dataColumn=ExtensionCommon.rowindex,caption= "筆數",cellWidth=174} ,
            new ReportProp{dataColumn="AMM0_YMD",caption= "日期" ,cellWidth=224},
            new ReportProp{dataColumn="AMM0_BRK_NO",caption= "期貨商代號" ,cellWidth=270},
            new ReportProp{dataColumn="BRK_ABBR_NAME",caption= "期貨商名稱" ,cellWidth=530},
            new ReportProp{dataColumn="AMM0_ACC_NO",caption= "投資人帳號" ,cellWidth=347},
            new ReportProp{dataColumn="AMM0_PROD_ID",caption= "商品名稱",cellWidth=357},
            new ReportProp{dataColumn="AMM0_O_SUBTRACT_QNTY",caption= "委託自行成交量",cellWidth=379},
            new ReportProp{dataColumn="AMM0_Q_SUBTRACT_QNTY",caption= "報價自行成交量",cellWidth=379},
            new ReportProp{dataColumn="AMM0_IQM_SUBTRACT_QNTY",caption= "不符合－報價自行成交量",cellWidth=379}
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
            is_dw_name = dao50032.ListD50032(w500xx.is_sbrkno, w500xx.is_ebrkno, w500xx.is_prod_category, w500xx.is_prod_kind_id_sto, w500xx.is_sdate, w500xx.is_edate);
         }
         else {
            is_dw_name = dao50032.ListChk(w500xx.is_sdate, w500xx.is_edate);
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
         /******************
         複製檔案
         ******************/
         string ls_file = w500xx.wf_copyfile(ls_rpt_id);
         //ls_file = CopyExcelTemplateFile(ls_rpt_id, FileType.XLS);
         if (ls_file == "") {
            return ResultStatus.Fail;
         }
         w500xx.is_log_txt = ls_file;
         /******************
         開啟檔案
         ******************/
         Workbook workbook = new Workbook();
         workbook.LoadDocument(ls_file);

         /******************
         讀取資料
         ******************/
         DataTable ids_1 = new DataTable();
         is_dw_name.Fill(ids_1);
         if (ids_1.Rows.Count <= 0) {
            w500xx.AfterExport();
            return ResultStatus.Success;
         }
         /******************
         切換Sheet
         ******************/
         Worksheet worksheet = workbook.Worksheets[0];

         ls_rpt_name = w500xx.t_conditionText().Trim();
         if (ls_rpt_name == "") {
            ls_rpt_name = "報表條件：" + "(" + w500xx.t_dateText() + ")";
         }
         else {
            ls_rpt_name = w500xx.t_conditionText().Trim() + " " + "(" + w500xx.t_dateText() + ")";
         }
         worksheet.Cells[2, 4].Value = ls_rpt_name;
         int rowIndex = 4;int k = 1;
         foreach (DataRow row in ids_1.Rows) {
            worksheet.Rows[rowIndex][0].Value = k;
            worksheet.Rows[rowIndex][1].Value = row["amm0_ymd"].AsString();
            worksheet.Rows[rowIndex][2].Value = row["amm0_brk_no"].AsString();
            worksheet.Rows[rowIndex][3].Value = row["brk_abbr_name"].AsString();
            worksheet.Rows[rowIndex][4].Value = row["amm0_acc_no"].AsString();
            worksheet.Rows[rowIndex][5].Value = row["amm0_prod_id"].AsString();
            worksheet.Rows[rowIndex][6].Value = row["amm0_o_subtract_qnty"].AsDecimal();
            worksheet.Rows[rowIndex][7].Value = row["amm0_q_subtract_qnty"].AsDecimal();
            worksheet.Rows[rowIndex][8].Value = row["amm0_iqm_subtract_qnty"].AsDecimal();
            rowIndex = rowIndex + 1;
            k++;
         }
         workbook.SaveDocument(ls_file);
         w500xx.AfterExport();
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         CommonReportLandscapeA4 reportLandscapeA4 = new CommonReportLandscapeA4();
         XtraReport xtraReport=reportHelper.CreateCompositeReport(defReport, reportLandscapeA4);

         reportHelper.Create(xtraReport);
         reportHelper.Preview();
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