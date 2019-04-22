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
using BaseGround.Shared;
using System.IO;
using System;
using System.Text;
/// <summary>
/// John,20190129
/// </summary>
namespace PhoenixCI.FormUI.Prefix5
{
   /// <summary>
   /// 造市者到期月份成交概況
   /// </summary>
   public partial class W50031 : FormParent
   {
      private D50031 dao50031;
      private DbDataAdapter is_dw_name { get; set; }
      int ii_market_code;
      string is_key, is_kind_id2, is_prod_subtype, is_detail_type, is_sum_subtype;
      public string ls_date_type;
      public W50031(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao50031 = new D50031();
      }
      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();
         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         w500xx.wf_gb_detial(true, true, "1");
         w500xx.wf_gb_print_sort(false, false, "1");
         w500xx.wf_gb_report_type("D");
         w500xx.wf_gb_group(true, true, "4");
         //鎖定和移除舊程式沒有的畫面
         w500xx.gb_group.Properties.Items.Remove(w500xx.gb_group.Properties.Items.GetItemByValue("rb_gall"));
         w500xx.gb_group.Properties.Items.Remove(w500xx.gb_group.Properties.Items.GetItemByValue("rb_gkind"));
         w500xx.gb_group.Properties.Items.Remove(w500xx.gb_group.Properties.Items.GetItemByValue("rb_gprod"));
         w500xx.dw_prod_kd.Enabled = false;
         w500xx.Open();
         //統計依照radioBtn Even
         w500xx.gb_group.EditValueChanged+= new EventHandler(_gb_group_EditValueChanged);
         return ResultStatus.Success;
      }
      private void _gb_group_EditValueChanged(object sender, EventArgs e)
      {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;
         switch (rb.EditValue.ToString()) {
            case "rb_gparam":
               w500xx.dw_prod_ct.Enabled = true;
               w500xx.st_prod_ct.Enabled = true;

               w500xx.dw_prod_kd_sto.Enabled = false;
               w500xx.st_prod_kd_sto.Enabled = false;
               break;
            case "rb_s":
               w500xx.dw_prod_ct.Enabled = true;
               w500xx.st_prod_ct.Enabled = true;

               w500xx.dw_prod_kd_sto.Enabled = false;
               w500xx.st_prod_kd_sto.Enabled = false;
               break;
            case "rb_gkind2":
               w500xx.dw_prod_ct.Enabled = true;
               w500xx.st_prod_ct.Enabled = true;

               w500xx.dw_prod_kd_sto.Enabled = true;
               w500xx.st_prod_kd_sto.Enabled = true;
               break;
            default:
               break;
         }
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         base.Retrieve();
         if (!BeforeRetrieve()) return ResultStatus.Fail;
         return ResultStatus.Success;
      }

      protected bool BeforeRetrieve()
      {
         if (!w500xx.Retrieve("0000000", "Z999999")) return false;
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
         ls_filename = _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + ".csv";
         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, ls_filename);
         w500xx.is_log_txt = ls_filename;
         /******************
         開啟檔案
         ******************/
         Workbook workbook = new Workbook();
         //判斷檔案是否存在,不存在就開一個新檔案
         if (!File.Exists(destinationFilePath)) {
            File.Create(destinationFilePath).Close();
         }
        
         workbook.LoadDocument(destinationFilePath);
         /* 商品群組 */
         is_key = w500xx.dw_prod_ct.SelectedText.Trim() + "%";
         if (string.IsNullOrEmpty(is_key) || w500xx.dw_prod_ct.Enabled == false) {
            is_key = "%";
         }
         /* 商品 */
         is_kind_id2 = w500xx.dw_prod_kd_sto.SelectedText.Trim() + "%";
         if (string.IsNullOrEmpty(is_kind_id2) || w500xx.dw_prod_kd_sto.Enabled == false) {
            is_kind_id2 = "%";
         }
         /*******************
         統計子類別
         *******************/
         is_prod_subtype = "%";
         if (w500xx.gb_group.EditValue.Equals("rb_gparam")) {
            is_sum_subtype = "3";
         }
         else if (w500xx.gb_group.EditValue.Equals("rb_s")) {
            is_sum_subtype = "S";

            is_prod_subtype = "S";
         }
         else if (w500xx.gb_group.EditValue.Equals("rb_gkind2")) {
            is_sum_subtype = "4";
         }
         /*******************
         交易時段
         *******************/
         if (w500xx.gb_market.EditValue.Equals("rb_market_1")) {
            ii_market_code = 1;
         }

         if (w500xx.gb_market.EditValue.Equals("rb_market_0")) {
            ii_market_code = 0;
         }
         /*******************
         報表內容
         *******************/
         if (w500xx.gb_detial.EditValue.Equals("rb_gdate")) {
            is_detail_type = "D";
         }

         if (w500xx.gb_detial.EditValue.Equals("rb_gnodate")) {
            is_detail_type = "N";
         }
         /*******************
         顯示條件
         *******************/
         string ls_text = "";
         if (w500xx.gb_market.EditValue.Equals("rb_market_1")) {
            ls_text = "盤後交易時段";
         }
         else {
            ls_text = "一般交易時段";
         }
         if (!string.IsNullOrEmpty(w500xx.is_sdate)) {
            ls_text = ls_text + "(" + w500xx.is_sdate;
         }
         if (!string.IsNullOrEmpty(w500xx.is_edate)) {
            ls_text = ls_text + "~~" + w500xx.is_edate + ")";
         }
         if (w500xx.is_sbrkno != "%" || w500xx.is_ebrkno != "%") {
            ls_text = ls_text + ",造市者:";
            if (w500xx.is_sbrkno == w500xx.is_ebrkno) {
               ls_text = ls_text + w500xx.is_sbrkno + " ";
            }
            else {
               ls_text = ls_text + w500xx.is_sbrkno + "~~" + w500xx.is_ebrkno + " ";
            }
         }

         if (is_key != "%") {
            ls_text = ls_text + ",商品群組:" + is_key;
}
         if (is_kind_id2 != "%") {
            ls_text = ls_text + ",2碼商品(個股):" + is_kind_id2;
         }

         if (PbFunc.Left(ls_text, 1) == ",") {
            ls_text = PbFunc.Mid(ls_text, 1, 50);
         }
         if (ls_text != "") {
            ls_text = "報表條件：" + ls_text;
         }
         /******************
         讀取資料
         ******************/
         /* 報表內容 */
         is_dw_name = dao50031.ListD50031
            (ii_market_code, is_sum_subtype, is_detail_type, w500xx.is_sdate, w500xx.is_edate, 
            w500xx.is_sbrkno, w500xx.is_ebrkno, is_key, is_prod_subtype, is_kind_id2, ls_text);
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
         workbook.Options.Export.Csv.WritePreamble = true;
         int rowNum = ids_1.Rows.Count;
         int columnNum = ids_1.Columns.Count;
         int rowIndex = 0;
         int columnIndex = 0;

         //將DataTable中的數據導入Excel中
         for (int i = 0; i < rowNum; i++) {
            rowIndex++;
            columnIndex = 0;
            for (int j = 0; j < columnNum; j++) {
               worksheet.Cells[rowIndex, columnIndex].Value = ids_1.Rows[i][j].ToString();
               columnIndex++;
            }
         }
         workbook.SaveDocument(destinationFilePath, DocumentFormat.Csv);
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

   }
}