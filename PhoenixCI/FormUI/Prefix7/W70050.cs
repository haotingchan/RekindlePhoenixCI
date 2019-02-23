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
using DataObjects.Dao.Together;
using BusinessObjects.Enums;
using BaseGround.Report;
using PhoenixCI.Shared;
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;
using System.Threading;
using Common;
using PhoenixCI.BusinessLogic.Prefix7;
/// <summary>
/// john,20190211,小型臺股期貨期貨商交易量表
/// </summary>
namespace PhoenixCI.FormUI.Prefix7
{
   /// <summary>
   /// 小型臺股期貨期貨商交易量表
   /// </summary>
   public partial class W70050 : FormParent
   {
      private B700xxFunc b700xxFunc;
      string is_log_txt, is_save_file, is_symd, is_eymd, is_sum_type, is_prod_type, is_kind_id2, is_param_key;
      public W70050(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         b700xxFunc = new B700xxFunc();
      }
      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         em_sdate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/") + "01";
         em_edate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");

         em_sdate1.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/") + "01";
         em_edate1.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");

         em_smth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/") + "01";
         em_emth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");

         em_syear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");
         em_eyear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");
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
         _ToolBtnPrintAll.Enabled = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      private bool ExportBefore()
      {
         /* 條件值檢核*/
         DateTime ld_s, ld_e;
         string ls_type = "";
         switch (rgDate.EditValue) {
            case "rb_day":
               //週
               if (!em_sdate.IsDate(em_sdate.Text, CheckDate.Start)
                  || !em_edate.IsDate(em_edate.Text, CheckDate.End)) {
                  return false;
               }
               if (string.Compare(em_sdate.Text, em_edate.Text) > 0) {
                  MessageDisplay.Error(GlobalInfo.gs_t_err, CheckDate.Datedif);
                  return false;
               }
               ld_s = Convert.ToDateTime(em_sdate.Text);
               ld_e = Convert.ToDateTime(em_edate.Text);

               is_symd = em_sdate.Text.Replace("/", "").SubStr(0, 8);
               is_eymd = em_edate.Text.Replace("/", "").SubStr(0, 8);
               ls_type = "Day";
               is_sum_type = "D";

               is_log_txt = ld_s.ToString("yyyy.mm.dd") + "至" + ld_e.ToString("yyyy.mm.dd") + " 交易量";
               break;
            case "rb_week":
               //週
               if (!em_sdate1.IsDate(em_sdate1.Text, CheckDate.Start)
                  || !em_edate1.IsDate(em_edate1.Text, CheckDate.End)) {
                  return false;
               }
               if (string.Compare(em_sdate1.Text, em_edate1.Text) > 0) {
                  MessageDisplay.Error(GlobalInfo.gs_t_err, CheckDate.Datedif);
                  return false;
               }
               ld_s = Convert.ToDateTime(em_sdate1.Text);
               ld_e = Convert.ToDateTime(em_edate1.Text);

               is_symd = em_sdate1.Text.Replace("/", "").SubStr(0, 8);
               is_eymd = em_edate1.Text.Replace("/", "").SubStr(0, 8);
               ls_type = "Week";
               is_sum_type = "D";

               is_log_txt = ld_s.ToString("yyyy.mm.dd") + "至" + ld_e.ToString("yyyy.mm.dd") + " 交易量";
               break;
            case "rb_month":
               //月
               string emSmth = em_smth.Text + "/01";
               string emEmth = em_emth.Text + "/01";
               if (!em_smth.IsDate(emSmth, CheckDate.Start)
                  || !em_emth.IsDate(emEmth, CheckDate.End)) {
                  return false;
               }
               ld_s = Convert.ToDateTime(emSmth);
               ld_e = PbFunc.relativedate(Convert.ToDateTime(emEmth), 31);
               if (ld_e.Month != PbFunc.Right(em_smth.Text, 2).AsInt()) {
                  ld_e = PbFunc.relativedate(ld_e, -ld_e.Day);
               }
               is_symd = em_smth.Text.Replace("/", "").SubStr(0, 6);
               is_eymd = em_emth.Text.Replace("/", "").SubStr(0, 6);
               ls_type = "Month";
               is_sum_type = "M";

               is_log_txt = is_symd + "至" + is_eymd + " 交易量";
               break;
            case "rb_year":
               //年
               is_symd = em_syear.Text;
               is_eymd = em_eyear.Text;
               ls_type = "Year";
               is_sum_type = "Y";
               is_log_txt = is_symd + "至" + is_eymd + " 交易量";
               break;
            default:
               break;
         }

         is_save_file = _ProgramID + "_" + ls_type + "(" + is_symd + "-" + is_eymd + ")";
         //商品別
         switch (rgPeriod.EditValue) {
            case "rb_txw":
               is_kind_id2 = "MXW%";
               is_save_file = is_save_file + "W";
               break;
            case "rb_txo":
               is_kind_id2 = "MXF%";
               is_save_file = is_save_file + "S";
               break;
            default:
               is_kind_id2 = "%";
               break;
         }
         is_param_key = "MXF";
         is_prod_type = "F";
         /*點選儲存檔案之目錄*/
         is_save_file = ReportExportFunc.wf_GetFileSaveName(is_save_file + ".csv");
         if (string.IsNullOrEmpty(is_save_file)) {
            return false;
         }
         /*******************
         Messagebox
         *******************/
         st_msg_txt.Visible = true;
         st_msg_txt.Text = "開始轉檔...";
         st_msg_txt.Text = is_log_txt + " 轉檔中...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void ExportAfter()
      {
         st_msg_txt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         st_msg_txt.Visible = false;
      }

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            if (rgDate.EditValue.Equals("rb_week")) {
               b700xxFunc.f_70010_week_w(is_save_file, is_symd, is_eymd, is_sum_type, is_kind_id2, is_param_key, is_prod_type);
            }//if (rgDate.EditValue.Equals("rb_week"))
            else {
               b700xxFunc.f_70010_ymd_w(is_save_file, is_symd, is_eymd, is_sum_type, is_kind_id2, is_param_key, is_prod_type);
            }
            ExportAfter();
         }
         catch (Exception ex) {
            PbFunc.messageBox(GlobalInfo.gs_t_err, ex.Message, MessageBoxIcon.Stop);
            return ResultStatus.Fail;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Export(ReportHelper reportHelper)
      {
         base.Export(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus COMPLETE()
      {
         return ResultStatus.Success;
      }
      private void rgDate_SelectedIndexChanged(object sender, EventArgs e)
      {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;
         switch (rb.EditValue.ToString()) {
            case "rb_day"://日期
               st_date.Visible = true;//日期
               st_date.FocusHelper.FocusFirst();
               st_week.Visible = false;//週期
               st_month.Visible = false;//月份
               st_year.Visible = false;//年度
               break;
            case "rb_week"://週期
               st_date.Visible = false;//日期
               st_week.Visible = true;//週期
               st_week.FocusHelper.FocusFirst();
               st_month.Visible = false;//月份
               st_year.Visible = false;//年度
               break;
            case "rb_month"://月份
               st_date.Visible = false;//日期
               st_week.Visible = false;//週期
               st_month.Visible = true;//月份
               st_month.FocusHelper.FocusFirst();
               st_year.Visible = false;//年度
               break;
            case "rb_year"://年度
               st_date.Visible = false;//日期
               st_week.Visible = false;//週期
               st_month.Visible = false;//月份
               st_year.Visible = true;//年度
               st_year.FocusHelper.FocusFirst();
               break;
            default:
               break;
         }
      }
   }
}