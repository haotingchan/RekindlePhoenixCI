using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
/// <summary>
/// john,20190220,指數期貨價量資料
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 指數期貨價量資料
   /// </summary>
   public partial class W30320 : FormParent
   {
      private B30320 b30320;
      public W30320(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         //讀取交易日
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

      /// <summary>
      /// 轉檔前檢查日期格式
      /// </summary>
      /// <returns></returns>
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

      /// <summary>
      /// 轉檔後清除文字訊息
      /// </summary>
      private void EndExport()
      {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      /// <summary>
      /// show出訊息在label
      /// </summary>
      /// <param name="msg"></param>
      private void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }

         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30320");
         //OutputShowMessage只會儲存ok的狀態,如沒有任何一個ok代表全部function都沒有資料
         MessageDisplay message = new MessageDisplay();
         try {
            //呼叫邏輯類別
            b30320 = new B30320(lsFile, emMonth.Text);

            ShowMsg("30321－指數期貨價量資料 轉檔中...");
            message.OutputShowMessage = b30320.Wf30321();
            ShowMsg("30322－指數期貨價量資料 轉檔中...");
            message.OutputShowMessage = b30320.Wf30322();

            //連續跳2次無資料刪除檔案
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