﻿using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix7;
using System.IO;
/// <summary>
/// john,20190128,交易量資料轉檔作業
/// </summary>
namespace PhoenixCI.FormUI.Prefix7 {
   /// <summary>
   /// 交易量資料轉檔作業
   /// </summary>
   public partial class W70010 : FormParent {
      private B70010 b70010;
      private D70010 dao70010;
      string logText, saveFilePath, lsSymd, lsEymd, sumType, lsProdType, lsMarketCode;
      public W70010(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         b70010 = new B70010();
         dao70010 = new D70010();
      }

      protected override ResultStatus Open() {
         base.Open();
         //日期
         emStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
         emEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
#if DEBUG
         emStartDate.Text = "2019/03/01";
         emEndDate.Text = "2019/03/31";
#endif
         //週期
         emStartDate1.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
         emEndDate1.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         //月份
         emStartMth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/01");
         emEndMth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         //年度
         emStartYear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");
         emEndYear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");

         if (!FlagAdmin) {
            cbxEng.Visible = false;
         } else {
            cbxEng.Visible = true;
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();
         //cbx_eng.Visible = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

      private bool StartExport() {
         /* 條件值檢核*/
         DateTime ldStart, ldEnd;
         string lsType = "";
         switch (rgDate.EditValue) {
            case "rb_day":
               //週
               if (!emStartDate.IsDate(emStartDate.Text , CheckDate.Start)
                  || !emEndDate.IsDate(emEndDate.Text , CheckDate.End)) {
                  return false;
               }
               if (string.Compare(emStartDate.Text , emEndDate.Text) > 0) {
                  MessageDisplay.Error(GlobalInfo.ErrorText , CheckDate.Datedif);
                  return false;
               }
               ldStart = Convert.ToDateTime(emStartDate.Text);
               ldEnd = Convert.ToDateTime(emEndDate.Text);

               lsSymd = emStartDate.Text.Replace("/" , "").SubStr(0 , 8);
               lsEymd = emEndDate.Text.Replace("/" , "").SubStr(0 , 8);
               lsType = "Daily";
               sumType = "D";

               logText = ldStart.ToString("yyyy.MM.dd") + "至" + ldEnd.ToString("yyyy.MM.dd") + " 交易量";
               break;
            case "rb_week":
               //週
               if (!emStartDate1.IsDate(emStartDate1.Text , CheckDate.Start)
                  || !emEndDate1.IsDate(emEndDate1.Text , CheckDate.End)) {
                  return false;
               }
               if (string.Compare(emStartDate1.Text , emEndDate1.Text) > 0) {
                  MessageDisplay.Error(GlobalInfo.ErrorText , CheckDate.Datedif);
                  return false;
               }
               ldStart = Convert.ToDateTime(emStartDate1.Text);
               ldEnd = Convert.ToDateTime(emEndDate1.Text);

               lsSymd = emStartDate1.Text.Replace("/" , "").SubStr(0 , 8);
               lsEymd = emEndDate1.Text.Replace("/" , "").SubStr(0 , 8);
               lsType = "Weekly";
               sumType = "D";

               logText = ldStart.ToString("yyyy.MM.dd") + "至" + ldEnd.ToString("yyyy.MM.dd") + " 交易量";
               break;
            case "rb_month":
               //月
               string Smth = this.emStartMth.Text + "/01";
               string Emth = this.emEndMth.Text + "/01";
               if (!this.emStartMth.IsDate(Smth , CheckDate.Start)
                  || !this.emEndMth.IsDate(Emth , CheckDate.End)) {
                  return false;
               }
               ldStart = Convert.ToDateTime(Smth);
               ldEnd = PbFunc.relativedate(Convert.ToDateTime(Emth) , 31);
               if (ldEnd.Month != PbFunc.Right(emStartMth.Text , 2).AsInt()) {
                  ldEnd = PbFunc.relativedate(ldEnd , -ldEnd.Day);
               }
               lsSymd = emStartMth.Text.Replace("/" , "").SubStr(0 , 6);
               lsEymd = emEndMth.Text.Replace("/" , "").SubStr(0 , 6);
               lsType = "Monthly";
               sumType = "M";

               logText = lsSymd + "至" + lsEymd + " 交易量";
               break;
            case "rb_year":
               //年
               lsSymd = emStartYear.Text;
               lsEymd = emEndYear.Text;
               lsType = "Yearly";
               sumType = "Y";
               logText = lsSymd + "至" + lsEymd + " 交易量";
               break;
            default:
               break;
         }

         //商品別
         if (rbTMU.EditValue.Equals("rb_options")) {
            lsType = lsType + "_OPT";
            lsProdType = "O";
         } else {
            lsType = lsType + "_FUT";
            lsProdType = "F";
         }
         //ids_1.dataobject = "d_"+gs_txn_id
         if (cbxEng.Checked) {
            lsType = lsType + "_eng";
         }

         //交易時段
         switch (rgTime.EditValue) {
            case "rb_market0":
               lsMarketCode = "0";
               lsType = lsType + "_day";
               break;
            case "rb_market1":
               lsMarketCode = "1";
               lsType = lsType + "_night";
               break;
            default:
               lsMarketCode = "%";
               break;
         }

         /*點選儲存檔案之目錄*/
         saveFilePath = PbFunc.wf_GetFileSaveName(lsType + "(" + lsSymd + "-" + lsEymd + ").csv");
         if (string.IsNullOrEmpty(saveFilePath)) {
            return false;
         }
         /*******************
         Messagebox
         *******************/
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         stMsgTxt.Text = logText + " 轉檔中...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      private void EndExport() {
         stMsgTxt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      protected override ResultStatus Export() {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }

         try {
            MessageDisplay message = new MessageDisplay();
            message.OutputShowMessage = b70010.F70010ByMarketCodeExport(rgDate.EditValue.AsString() , saveFilePath , lsSymd , lsEymd , sumType , lsProdType , lsMarketCode , cbxEng.Checked);

            if (string.IsNullOrEmpty(message.OutputShowMessage))
               return ResultStatus.Fail;

         } catch (Exception ex) {
            if (File.Exists(saveFilePath))
               File.Delete(saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         } finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

      private void rgDate_SelectedIndexChanged(object sender , EventArgs e) {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;
         switch (rb.EditValue.ToString()) {
            case "rb_day"://日期
               stDate.Visible = true;//日期
               stDate.FocusHelper.FocusFirst();
               stWeek.Visible = false;//週期
               stMonth.Visible = false;//月份
               stYear.Visible = false;//年度
               break;
            case "rb_week"://週期
               stDate.Visible = false;//日期
               stWeek.Visible = true;//週期
               stWeek.FocusHelper.FocusFirst();
               stMonth.Visible = false;//月份
               stYear.Visible = false;//年度
               break;
            case "rb_month"://月份
               stDate.Visible = false;//日期
               stWeek.Visible = false;//週期
               stMonth.Visible = true;//月份
               stMonth.FocusHelper.FocusFirst();
               stYear.Visible = false;//年度
               break;
            case "rb_year"://年度
               stDate.Visible = false;//日期
               stWeek.Visible = false;//週期
               stMonth.Visible = false;//月份
               stYear.Visible = true;//年度
               stYear.FocusHelper.FocusFirst();
               break;
            default:
               break;
         }
      }
   }
}