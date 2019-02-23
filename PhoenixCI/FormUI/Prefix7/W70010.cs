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
using BaseGround.Shared;
using Common;
using PhoenixCI.Shared;
using DataObjects.Dao.Together.SpecificDao;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix7;
/// <summary>
/// john,20190128,交易量資料轉檔作業
/// </summary>
namespace PhoenixCI.FormUI.Prefix7
{
   /// <summary>
   /// 交易量資料轉檔作業
   /// </summary>
   public partial class W70010 : FormParent
   {
      private B70010 b70010;
      private D70010 dao70010;
      string is_log_txt, is_save_file, is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code;
      public W70010(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         b70010 = new B70010();
         dao70010 = new D70010();
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

         //wf_input(true, false, false, false);覺得不用這段
         return ResultStatus.Success;
      }
      ///// <summary>
      ///// 隱藏輸入欄位
      ///// </summary>
      ///// <param name="ab_date">日期</param>
      ///// <param name="ab_week">週期</param>
      ///// <param name="ab_month">月份</param>
      ///// <param name="ab_year">年度</param>
      //////private void wf_input(bool ab_date, bool ab_week, bool ab_month, bool ab_year)
      //////{
      //////   em_sdate1.Visible = ab_week;
      //////   em_edate1.Visible = ab_week;
      //////   st_week.Visible = ab_week;

      //////   em_smth.Visible = ab_month;
      //////   em_emth.Visible = ab_month;
      //////   st_month.Visible = ab_month;

      //////   em_syear.Visible = ab_year;
      //////   em_eyear.Visible = ab_year;
      //////   st_year.Visible = ab_year;

      //////   if (ab_date) {
      //////      em_sdate.Focus();
      //////   }
      //////   else if (ab_week) {
      //////      em_sdate1.Focus();
      //////   }
      //////   else if (ab_month) {
      //////      em_smth.Focus();
      //////   }
      //////   else if (ab_year) {
      //////      em_syear.Focus();
      //////   }
      //////}

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         //cbx_eng.Visible = true;
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
               ls_type = "Daily";
               is_sum_type = "D";

               is_log_txt = ld_s.ToString("yyyy.MM.dd") + "至" + ld_e.ToString("yyyy.MM.dd") + " 交易量";
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
               ls_type = "Weekly";
               is_sum_type = "D";

               is_log_txt = ld_s.ToString("yyyy.MM.dd") + "至" + ld_e.ToString("yyyy.MM.dd") + " 交易量";
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
               ls_type = "Monthly";
               is_sum_type = "M";

               is_log_txt = is_symd + "至" + is_eymd + " 交易量";
               break;
            case "rb_year":
               //年
               is_symd = em_syear.Text;
               is_eymd = em_eyear.Text;
               ls_type = "Yearly";
               is_sum_type = "Y";
               is_log_txt = is_symd + "至" + is_eymd + " 交易量";
               break;
            default:
               break;
         }

         //商品別
         if (rbTMU.EditValue.Equals("rb_options")) {
            ls_type = ls_type + "_OPT";
            is_prod_type = "O";
         }
         else {
            ls_type = ls_type + "_FUT";
            is_prod_type = "F";
         }
         //ids_1.dataobject = "d_"+gs_txn_id
         if (cbx_eng.Checked) {
            ls_type = ls_type + "_eng";
         }

         //交易時段
         switch (rgTime.EditValue) {
            case "rb_market0":
               is_market_code = "0";
               ls_type = ls_type + "_day";
               break;
            case "rb_market1":
               is_market_code = "1";
               ls_type = ls_type + "_night";
               break;
            default:
               is_market_code = "%";
               break;
         }

         /*點選儲存檔案之目錄*/
         is_save_file = ReportExportFunc.wf_GetFileSaveName(ls_type + "(" + is_symd + "-" + is_eymd + ").csv");
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
            b70010.f70010ByMarketCodeExport(rgDate.EditValue.AsString(), is_save_file, is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code, cbx_eng.Checked);
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