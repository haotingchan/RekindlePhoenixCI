using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
using BaseGround.Shared;
/// <summary>
/// john,20190227,動態價格穩定措施基準價查詢
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 動態價格穩定措施基準價查詢
   /// </summary>
   public partial class W30687 : FormParent
   {
      private B30687 b30687;
      public W30687(string programID, string programName) : base(programID, programName)
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
         emStartDate.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

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
         /*******************
         Messagebox
         *******************/
         stMsgTxt.Visible = true;
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
      }

      protected void ShowMsg(string msg)
      {
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }
      /// <summary>
      /// show文字訊息
      /// </summary>
      private string OutputShowMessage {
         set {
            if (value != MessageDisplay.MSG_OK)
               MessageDisplay.Info(value);
         }
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         try {
            string saveFilePath= Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,$"{_ProgramID}_{DateTime.Now.ToString("yyyy.MM.dd")}-{DateTime.Now.ToString("HH.mm.ss")}.csv");
            b30687 = new B30687(saveFilePath, emStartDate.Text, emEndDate.Text, SleProdIDtxt.Text,rgMarket.SelectedIndex,rgTime.SelectedIndex);
            OutputShowMessage=b30687.WF30687RuNew();
            ShowMsg("30687_ru_new－RU筆數統計 轉檔中...");
         }
         catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
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
   }
}