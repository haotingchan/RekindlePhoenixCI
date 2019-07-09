using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using System.IO;
/// <summary>
/// john,20190325,股票類(ETF)期貨價格及現貨資料下載
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 股票類(ETF)期貨價格及現貨資料下載
   /// </summary>
   public partial class W43033 : FormParent
   {
      private B43033 b43033;

      public W43033(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         emStartDate.Text = DateTime.Today.ToString("yyyy/MM/dd");
         emEndDate.Text = DateTime.Today.ToString("yyyy/MM/dd");
#if DEBUG
         emStartDate.Text = "2019/02/27";
         emEndDate.Text = "2019/03/15";
         this.Text += "(開啟測試模式),Date=2019/02/27~2019/03/15";
#endif
         emStartDate.Focus();
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

      /// <summary>
      /// 轉檔後清除文字訊息
      /// </summary>
      protected void EndExport()
      {
         stMsgTxt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      /// <summary>
      /// show出訊息在label
      /// </summary>
      /// <param name="msg"></param>
      protected void ShowMsg(string msg)
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
         MessageDisplay message = new MessageDisplay();
         string saveFilePath = PbFunc.wf_copy_file(_ProgramID, "43033");
         try {
            b43033 = new B43033(saveFilePath, emStartDate.Text, emEndDate.Text);
            ShowMsg("43033－股票類(ETF)期貨價格及現貨資料 轉檔中...");
            message.OutputShowMessage = b43033.Wf43033();
            if (string.IsNullOrEmpty(message.OutputShowMessage)) {
               ShowMsg("轉檔有錯誤!");
               if (File.Exists(saveFilePath))
                  File.Delete(saveFilePath);
               return ResultStatus.Fail;
            }
         }
         catch (Exception ex) {
            ShowMsg("轉檔有錯誤!");
            if (File.Exists(saveFilePath))
               File.Delete(saveFilePath);
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