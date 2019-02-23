using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using System.IO;
using BusinessObjects;
using DataObjects.Dao.Together;
using DevExpress.XtraLayout.Utils;
using Common.Helper;

namespace PhoenixCI.FormUI.Prefix5
{
   public partial class W500xx : FormParent
   {
      private OCF OCF;
      bool ib_open;
      string is_chk;
      DataSet ids_1;

      string is_sbrkno, is_ebrkno, is_prod_kind_id_sto, is_prod_kind_id, is_prod_category,
               is_sdate, is_edate, is_select, is_dw_name, is_where;
      string is_sum_type, is_sum_subtype, is_data_type, is_sort_type, is_table_name, is_log_txt, is_txn_id, is_time, is_filename;
      object iole_1;
      long ii_ole_row;
      public W500xx(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         /*******************
         Input Condition
         *******************/
         //GlobalInfo.OCF_DATE = serviceCommon.GetOCF().OCF_DATE;
         em_edate.EditValue = GlobalInfo.OCF_DATE;
         em_sdate.EditValue = (em_edate.Text.Substring(0, 5) + "01").AsDateTime();
         em_sym.EditValue = GlobalInfo.OCF_DATE;
         em_eym.EditValue = GlobalInfo.OCF_DATE;
         /* 造市者代號 */
         ////////dw_sbrkno.settransobject(sqlca);
         ////////dw_sbrkno.insertrow(0);
         ////////dw_ebrkno.settransobject(sqlca);
         ////////dw_ebrkno.insertrow(0);
         /////////* 商品群組 */
         ////////dw_prod_ct.settransobject(sqlca);
         ////////dw_prod_ct.insertrow(0);
         /////////* 造市商品 */
         ////////dw_prod_kd.settransobject(sqlca);
         ////////dw_prod_kd.insertrow(0);
         /////////* 個股商品 */
         ////////dw_prod_kd_sto.settransobject(sqlca);
         ////////dw_prod_kd_sto.insertrow(0);

         //is_table_name = 'AMM0';
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

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         base.Retrieve();

         return ResultStatus.Success;
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
         st_msg_txt.Visible = true;
         st_msg_txt.Text = "開始轉檔...";
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

      public void wf_gb_report_type(string as_type)
      {

         switch (as_type) {
            case "M":
               /* 只有月份 */
               gp_date.Visibility = LayoutVisibility.Never;
               gb_report_type.Visible = false;
               break;
            case "m":
               gb_report_type.Visible = false;
               gp_date.Visibility = LayoutVisibility.Never;
               /* 無迄止值 */
               em_eym.Visible = false;
               st_month.Visibility = LayoutVisibility.Never;
               break;
            case "D":
               /* 只有日期 */
               gp_month.Visibility = LayoutVisibility.Never;
               gb_report_type.Visible = false;
               break;
            case "d":
               /* 只有日期 */
               gp_month.Visibility = LayoutVisibility.Never;
               gb_report_type.Visible = false;
               /* 無迄止值 */
               st_date.Visibility = LayoutVisibility.Never;
               em_edate.Visible = false;
               break;
            default:
               break;
         }
      }

      public void wf_select_sqlcode()
      {
         is_where = "";
         /* 日期起迄 */
         if (is_sdate != "") {
            is_where = is_where + "and " + is_table_name + "_YMD >='" + is_sdate + "' ";
         }
         if (is_edate != "") {
            is_where = is_where + "and " + is_table_name + "_YMD <='" + is_edate + "' ";
         }
         /* 期貨商代號起迄 */
         if (is_sbrkno != "") {
            is_where = is_where + "and " + is_table_name + "_BRK_NO >='" + is_sbrkno + "' ";
         }
         if (is_ebrkno != "") {
            is_where = is_where + "and " + is_table_name + "_BRK_NO <='" + is_ebrkno + "' ";
         }
         /*******************
         統計類別
         *******************/
         if (gb_report_type.EditValue.Equals("rb_month")) {
            is_sum_type = "M";
         }
         else if (gb_report_type.EditValue.Equals("rb_date")) {
            is_sum_type = "D";
         }

         /*******************
         統計子類別
         *******************/
         switch (gb_group.EditValue.AsString()) {
            case "rb_gall":
               is_sum_subtype = "1";
               break;
            case "rb_gparam":
               is_sum_subtype = "3";
               break;
            case "rb_s":
               is_sum_subtype = "S";
               break;
            case "rb_gkind2":
               is_sum_subtype = "4";
               break;
            case "rb_gkind":
               is_sum_subtype = "5";
               break;
            case "rb_gprod":
               is_sum_subtype = "6";
               break;
            default:
               break;
         }
         /*******************
         Sort順序
         *******************/
         if (gb_print_sort.EditValue.Equals("rb_mmk")) {
            is_sort_type = "F";
         }
         else {
            is_sort_type = "P";
         }

         /*******************
         Where條件
         *******************/
         /* 商品群組 */
         if (!gb_group.EditValue.Equals("rb_gall")) {
            if (is_prod_category != "") {
               is_where = is_where + "and " + is_table_name + "_PARAM_KEY ='" + is_prod_category + "' ";
            }
            /* 個股商品 */
            if (!gb_group.EditValue.Equals("rb_gparam")) {
               if (is_prod_kind_id_sto != "") {
                  is_where = is_where + "and " + is_table_name + "_KIND_ID2 ='" + is_prod_kind_id_sto + "' ";
               }
               /* 商品 */
               if (!gb_group.EditValue.Equals("rb_gkind2")) {
                  if (is_prod_kind_id != "") {
                     is_where = is_where + "and " + is_table_name + "_KIND_ID ='" + is_prod_kind_id + "' ";
                  }
               }//rb_gkind2.checked = False
            }//rb_gparam.checked = False
         }//rb_gall.checked = False
          /******************************
          在dw_1的SQL Statement中插入where條件(is_select)
          (1)is_select 都以'and ...'開頭
          (2)若有GROUP,則在 FROM 和 GROUP BY 中間插入 WHERE 條件
          ******************************/
         //////string ls_select;
         //////dw_1.dataobject = is_dw_name;
         //////dw_1.settransobject(sqlca);
         //////ls_select = dw_1.describe("datawindow.table.select");
         //////int li_pos;
         ///////* (1) */
         //////li_pos = ls_select.IndexOf("FROM");
         //////li_pos = ls_select.IndexOf("WHERE", li_pos);
         //////if (li_pos == 0) {
         //////   is_where = "WHERE" + PbFunc.Mid(is_where, 4, PbFunc.Len(is_where));
         //////}
         ///////* (2) */
         //////li_pos = ls_select.IndexOf("GROUP BY");
         //////if (li_pos == 0) {
         //////   li_pos = PbFunc.Len(ls_select) + 1;
         //////}
         //////ls_select = PbFunc.Mid(ls_select, 1, li_pos - 1) + is_where + PbFunc.Mid(ls_select, li_pos, PbFunc.Len(ls_select));
         //////is_select = ls_select;
      }


      public void wf_gb_group(bool ab_visible_value, bool ab_enable_value, string as_type)
      {
         gb_group.Visible = ab_visible_value;

         gb_group.Enabled = ab_enable_value;

         switch (as_type) {
            case "1":
               gb_group.EditValue = "rb_gall";
               break;
            case "2":
               gb_group.EditValue = "rb_gparam";
               break;
            case "3":
               gb_group.EditValue = "rb_gkind";
               break;
            case "4":
               gb_group.EditValue = "rb_gkind2";
               break;
            case "5":
               gb_group.EditValue = "rb_gprod";
               break;
            default:
               break;
         }
      }
      public void wf_gb_print_sort(bool ab_visible_value, bool ab_enable_value, string as_type)
      {
         gb_print_sort.Visible = ab_visible_value;

         gb_print_sort.Enabled = ab_enable_value;

         switch (as_type) {
            case "1":
               gb_print_sort.EditValue = "rb_mmk";
               break;

            case "2":
               gb_print_sort.EditValue = "rb_prod";
               break;
            default:
               break;
         }
      }
      public void wf_gb_detial(bool ab_visible_value, bool ab_enable_value, string as_type)
      {
         gb_detial.Visible = ab_visible_value;

         gb_detial.Enabled = ab_enable_value;

         switch (as_type) {
            case "1":
               gb_detial.EditValue = "rb_gdate";
               break;

            case "2":
               gb_detial.EditValue = "rb_gnodate";
               break;
            default:
               break;
         }
      }
      public void wf_retrieve()
      {
         string ls_text;
         //mask 暫加 1
         //dw_1.settransobject(sqlca_FUT)
         //////ls_text = dw_1.modify("Datawindow.table.select=" + '"' + is_select + '"');
         //////if (ls_text == "") {
         //////   dw_1.retrieve(is_sum_type, is_sum_subtype, is_data_type, is_sort_type, is_sdate, is_edate);
         //////}
         //////else {
         //////   PbFunc.messageBox("錯誤訊息"+" DataWindow sqlsyntax Modify Failed ", ls_text, MessageBoxIcon.Warning);
         //////   return;
         //////}
      }
   }
}