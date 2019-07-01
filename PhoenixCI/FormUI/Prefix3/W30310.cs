using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
/// <summary>
/// john,20190218,證期局七組月報
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 證期局七組月報
   /// </summary>
   public partial class W30310 : FormParent
   {
      private B30310 b30310;
      public W30310(string programID, string programName) : base(programID, programName)
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
         emMonth.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         emMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         emMonth.Focus();
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
         if (!emMonth.IsDate(emMonth.Text + "/01", "日期輸入錯誤")) {
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
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

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

         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30310");
         MessageDisplay message = new MessageDisplay();

         try {
            b30310 = new B30310(lsFile, emMonth.Text);
            ShowMsg("30310－我國臺股期貨契約價量資料30311_1 轉檔中...");
            message.OutputShowMessage = b30310.Wf30310one("TXF", "30311_1");
            ShowMsg("30310－我國臺股期貨契約價量資料30311_2(EXF) 轉檔中...");
            message.OutputShowMessage = b30310.Wf30310two("EXF", "30311_2(EXF)");
            ShowMsg("30310－我國臺股期貨契約價量資料30311_3(FXF) 轉檔中...");
            message.OutputShowMessage = b30310.Wf30310two("FXF", "30311_3(FXF)");
            ShowMsg("30313－當年每月日均量統計表 轉檔中...");
            message.OutputShowMessage = b30310.Wf30310four();

            //連續跳四次無資料刪除檔案
            if (string.IsNullOrEmpty(message.OutputShowMessage)) {
               if (File.Exists(lsFile))
                  File.Delete(lsFile);
               return ResultStatus.Fail;
            }
               
         }
         catch (Exception ex) {
            if (File.Exists(lsFile))
               File.Delete(lsFile);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }
         return ResultStatus.Success;
      }

   }
}