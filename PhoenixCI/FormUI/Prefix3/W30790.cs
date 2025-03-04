﻿using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;
/// <summary>
/// 20190327,john,盤後交易時段分時交易量分布
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 盤後交易時段分時交易量分布
   /// </summary>
   public partial class W30790 : FormParent
   {
      private B30790 b30790;

      public W30790(string programID, string programName) : base(programID, programName)
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
         emStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         emEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         string lsYMD = new D30790().MaxDate();
         DateTime date;
         if (!string.IsNullOrEmpty(lsYMD)) {
            if (lsYMD.Length == 8)
               date = lsYMD.AsDateTime("yyyyMMdd");
            else if (lsYMD.Length == 6)
               date = lsYMD.AsDateTime("yyyyMM");
            else
               date = DateTime.MinValue;

            emEndDate.DateTimeValue = date;
         }
         else {
            return ResultStatus.Fail;
         }

         if (date != DateTime.MinValue)
            emStartDate.DateTimeValue = PbFunc.relativedate(date, -6);

         emStartDate.Focus();
         emTxEndDate.DateTimeValue = date;
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

      private bool StartExport()
      {
         if (!emStartDate.IsDate(emStartDate.Text, CheckDate.Start)) {
            //is_chk = "Y";
            return false;
         }
         if (!emEndDate.IsDate(emEndDate.Text, CheckDate.End)) {
            //is_chk = "Y";
            return false;
         }
         if (!emTxStartDate.IsDate(emTxStartDate.Text, CheckDate.Start)) {
            //is_chk = "Y";
            return false;
         }
         if (!emTxEndDate.IsDate(emTxEndDate.Text, CheckDate.End)) {
            //is_chk = "Y";
            return false;
         }
         /*******************
         Messagebox
         *******************/
         stMsgTxt.Visible = true;

         gb.Enabled = false;
         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void EndExport()
      {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
         gb.Enabled = true;
      }

      protected void ShowMsg(string msg)
      {
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30790");
         try {

            b30790 = new B30790(lsFile, emStartDate.Text, emEndDate.Text, emTxStartDate.Text, emTxEndDate.Text);
            MessageDisplay message = new MessageDisplay();
            if (chkAvg.Checked) {
               ShowMsg("30790－盤後交易時段分時交易量分布 轉檔中...");
               message.OutputShowMessage = b30790.Wf30790();
            }
            //TX每日日盤及夜盤之振幅及收盤價
            if (chkTx.Checked) {
               ShowMsg("30790_4－盤後交易時段分時交易量分布 轉檔中...");
               message.OutputShowMessage = b30790.Wf30790four();
            }
            if (string.IsNullOrEmpty(message.OutputShowMessage)) {
               if (File.Exists(lsFile))
                  File.Delete(lsFile);
               return ResultStatus.Fail;
            }
         }
         catch (Exception ex) {
            WriteLog(ex);
            if (File.Exists(lsFile))
               File.Delete(lsFile);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

   }
}