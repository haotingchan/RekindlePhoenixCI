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
/// john,20190221,十年期公債期貨契約價量資料
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 十年期公債期貨契約價量資料
   /// </summary>
   public partial class W30330 : FormParent
   {
      private B30330 b30330;
      public W30330(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         //讀取交易日
#if DEBUG
         emMonth.Text = "2005/12";
#else
         emMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
#endif
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

         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30330");
         //OutputShowMessage只會儲存ok的狀態,如沒有任何一個ok代表全部function都沒有資料
         MessageDisplay message = new MessageDisplay();
         try {
            //呼叫邏輯類別
            b30330 = new B30330(lsFile, emMonth.Text);

            ShowMsg("30330－「十年期公債」期貨契約價量資料 轉檔中...");
            message.OutputShowMessage = b30330.Wf30331();
            ShowMsg("30333－「十年期公債」期貨契約價量資料(買賣方比重) 轉檔中...");
            message.OutputShowMessage = b30330.Wf30333();

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