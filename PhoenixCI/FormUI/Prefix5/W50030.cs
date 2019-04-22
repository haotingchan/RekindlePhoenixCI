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

namespace PhoenixCI.FormUI.Prefix5
{
   public partial class W50030 : FormParent
   {
      private D50030 dao50030;
      private DbDataAdapter is_dw_name { get; set; }
      //private DataTable is_dw_name { get; set; }
      public string ls_date_type;
      public W50030(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao50030 = new D50030();
      }
      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();
         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
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
         ////System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
         ////sw.Reset();//碼表歸零
         ////sw.Start();//碼表開始計時
         base.Retrieve();
         //_ToolBtnExport.Enabled = true;
         BeforeRetrieve();
         w500xx.wf_select_sqlcode(is_dw_name);
         //is_dw_name = w500xx.wf_select_sqlcode_linq(is_dw_name);
         //if (w500xx.RetrieveAfter(is_dw_name)) {
         //   reportView();
         //}
         reportView();
         ////string result1 = sw.Elapsed.TotalMilliseconds.ToString();
         ////MessageBox.Show(result1, "執行時間");
         return ResultStatus.Success;
      }

      private void reportView()
      {
         DataTable dt = new DataTable();
         is_dw_name.Fill(dt);
         //dt = is_dw_name;
         //List<ReportProp> caption = new List<ReportProp>{
         //   new ReportProp{ dataColumn=ExtensionCommon.rowindex,caption= "筆數"} ,
         //   new ReportProp{dataColumn="AMM0_YMD",caption=  "日期"},
         //   new ReportProp{ dataColumn="AMM0_BRK_NO", caption= "期貨商代號"},
         //   new ReportProp{dataColumn="BRK_ABBR_NAME",caption= "期貨商名稱"},
         //   new ReportProp{dataColumn="AMM0_ACC_NO",caption= "投資人帳號"}, 
         //   new ReportProp{dataColumn= "AMM0_PROD_ID", caption= "商品名稱"},
         //   new ReportProp{dataColumn="AMM0_O_SUBTRACT_QNTY",caption= "委託自行成交量"},
         //   new ReportProp {dataColumn = "AMM0_Q_SUBTRACT_QNTY", caption = "報價自行成交量" },
         //   new ReportProp{dataColumn="AMM0_IQM_SUBTRACT_QNTY",caption= "不符合－報價自行成交量"}
         //   };
         //dt = ExtensionCommon.AddSeriNumToDataTable(dt);
         //defReport report = new defReport(dt, caption);
         RW50030 r50030rpt = new RW50030(true);
         r50030rpt.DataSource = dt;
         var storage = new MemoryDocumentStorage();
         var report = r50030rpt;
         cachedReportSource1 = new CachedReportSource(report, storage);

         documentViewer1.DocumentSource = cachedReportSource1;
         cachedReportSource1.CreateDocumentAsync();
         cachedReportSource1.AfterBuildPages += cachedReportSource1_AfterBuildPages;
         //r50030rpt.CreateDocument(false);
      }

      protected void BeforeRetrieve()
      {
         w500xx.Retrieve();
         /* 報表內容 */
         //報表內容選擇分日期
         if (w500xx.gb_detial.EditValue.Equals("rb_gdate")) {
            is_dw_name = dao50030.ListD50030(w500xx.is_sum_type, w500xx.is_sum_subtype, w500xx.is_data_type);
            //is_dw_name = dao50030.dt50030(w500xx.is_sum_type, w500xx.is_sum_subtype, w500xx.is_data_type);
            ls_date_type = "D";//匯出Excel時需要用到的判斷
            //交易時段選盤後
            if (w500xx.gb_market.EditValue.Equals("rb_market_1")) {
               is_dw_name = dao50030.ListAH(w500xx.is_sum_type, w500xx.is_sum_subtype, w500xx.is_data_type);
               //is_dw_name = dao50030.AH(w500xx.is_sum_type, w500xx.is_sum_subtype, w500xx.is_data_type);
            }
         }
         else {
            is_dw_name = dao50030.ListACCU(w500xx.is_sdate, w500xx.is_edate, w500xx.is_sum_type, w500xx.is_sum_subtype, w500xx.is_data_type);
            //is_dw_name = dao50030.ACCU(w500xx.is_sdate, w500xx.is_edate, w500xx.is_sum_type, w500xx.is_sum_subtype, w500xx.is_data_type);
            ls_date_type = "A";//匯出Excel時需要用到的判斷
            //交易時段選盤後
            if (w500xx.gb_market.EditValue.Equals("rb_market_1")) {
               is_dw_name = dao50030.ListACCUAH(w500xx.is_sdate, w500xx.is_edate, w500xx.is_sum_type, w500xx.is_sum_subtype, w500xx.is_data_type);
               //is_dw_name = dao50030.ACCUAH(w500xx.is_sdate, w500xx.is_edate, w500xx.is_sum_type, w500xx.is_sum_subtype, w500xx.is_data_type);
               ls_date_type = "D";//匯出Excel時需要用到的判斷
            }
         }
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
         string ls_rpt_name = w500xx.t_conditionText().Trim();
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

         //ids_1 = is_dw_name;
         if (ids_1.Rows.Count <= 0) {
            w500xx.AfterExport();
            return ResultStatus.Success;
         }
         /******************
         切換Sheet
         ******************/
         Worksheet worksheet = workbook.Worksheets[0];


         if (ls_rpt_name == "") {
            ls_rpt_name = "報表條件：" + "(" + w500xx.t_dateText() + ")";
         }
         else {
            ls_rpt_name = w500xx.t_conditionText().Trim() + " " + "(" + w500xx.t_dateText() + ")";
         }
         worksheet.Cells[2, 4].Value = ls_rpt_name;
         int rowIndex = 4; int k = 1;
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
         foreach (DataRow row in ids_1.Rows) {
            int index = 0;
            worksheet.Rows[rowIndex][index++].Value = k;
            worksheet.Rows[rowIndex][index++].Value = row["amm0_ymd"].AsString();
            worksheet.Rows[rowIndex][index++].Value = row["amm0_brk_no"].AsString();
            worksheet.Rows[rowIndex][index++].Value = row["brk_abbr_name"].AsString();
            worksheet.Rows[rowIndex][index++].Value = row["amm0_acc_no"].AsString();
            worksheet.Rows[rowIndex][index++].Value = row["amm0_prod_id"].AsString();
            worksheet.Rows[rowIndex][index++].Value = row["amm0_om_qnty"].AsDecimal();
            worksheet.Rows[rowIndex][index++].Value = row["amm0_qm_qnty"].AsDecimal();
            worksheet.Rows[rowIndex][index++].Value = row["cp_m_qnty"].AsDecimal();
            worksheet.Rows[rowIndex][index++].Value = row["cp_rate_m"].AsDecimal();
            worksheet.Rows[rowIndex][index++].Value = row["amm0_valid_cnt"].AsDecimal();
            worksheet.Rows[rowIndex][index++].Value = row["cp_rate_valid_cnt"].AsDecimal();
            worksheet.Rows[rowIndex][index++].Value = row["amm0_market_r_cnt"].AsDecimal();
            worksheet.Rows[rowIndex][index++].Value = row["amm0_market_m_qnty"].AsDecimal();
            worksheet.Rows[rowIndex][index++].Value = row["cp_keep_time"].AsDecimal();
            index = 0;
            if (ls_date_type == "D") {
               worksheet.Rows[rowIndex][9 - 1].Value = row["cp_qnty"].AsDecimal();
               worksheet.Rows[rowIndex][17 - 1].Value = row["amm0_keep_flag"].AsString();
               worksheet.Rows[rowIndex][18 - 1].Value = row["amm0_trd_invalid_qnty"].AsDecimal();
               worksheet.Rows[rowIndex][19 - 1].Value = row["cp_avg_mmk_qnty"].AsDecimal();
            }
            rowIndex = rowIndex + 1;
            k++;
         }

         if (ls_date_type == "A") {

            worksheet.Columns[19 - 1].Delete();
            worksheet.Columns[18 - 1].Delete();
            worksheet.Columns[17 - 1].Delete();
            worksheet.Columns[9 - 1].Delete();
         }
         if (ls_date_type == "D" && !w500xx.gb_group.EditValue.Equals("rb_gparam")) {
            worksheet.Columns[17 - 1].Delete();
         }
         workbook.SaveDocument(ls_file);
         w500xx.AfterExport();
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
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

      private void cachedReportSource1_AfterBuildPages(object sender, EventArgs e)
      {
         _ToolBtnExport.Enabled = true;
      }

   }
}