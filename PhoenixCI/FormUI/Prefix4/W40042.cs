using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using System.IO;
using Common;
/// <summary>
/// john,20190422,收盤前保證金試算表
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// john,20190422,收盤前保證金試算表 
   /// </summary>
   public partial class W40042 : FormParent
   {
      private B40042 b40042;
      private string _saveFilePath;

      public W40042(string programID, string programName) : base(programID, programName)
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
#if DEBUG
         emDate.Text = "2018/10/12";
#else
            emDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
#endif
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         emDate.Focus();

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
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤"))
         {
            //is_chk = "Y";
            return false;
         }

         _saveFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
         b40042 = new B40042(_ProgramID, _saveFilePath, emDate.Text);


         stMsgTxt.Visible = true;

         //判斷FMIF資料已轉入
         string chkFMIF = b40042.CheckFMIF();
         if (chkFMIF != MessageDisplay.MSG_OK)
         {
            return OutputChooseMessage(chkFMIF);
         }

         //130批次作業做完
         string strRtn = b40042.Check130Wf();
         if (strRtn != MessageDisplay.MSG_OK)
         {
            return OutputChooseMessage(strRtn);
         }

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

      private bool OutputChooseMessage(string str)
      {
         DialogResult ChooseResult = MessageDisplay.Choose(str);
         if (ChooseResult == DialogResult.No)
         {
            EndExport();
            return false;
         }
         return true;
      }

      private string OutputShowMessage {
         set {
            if (value != MessageDisplay.MSG_OK)
               MessageDisplay.Info(value);
         }
      }

      protected override ResultStatus Export()
      {
         if (!StartExport())
         {
            return ResultStatus.Fail;
         }
         try
         {
            //Sheet : rpt_future
            ShowMsg($"{_ProgramID}_1－保證金狀況表 轉檔中...");
            OutputShowMessage = b40042.WfFutureSheet();
            //Sheet : rpt_option
            ShowMsg($"{_ProgramID}_2－保證金狀況表 轉檔中...");
            OutputShowMessage = b40042.WfOptionSheet();

            
            ShowMsg($"{_ProgramID}_mg1－股票期貨保證金狀況表－標的證券為受益憑證 轉檔中...");
            OutputShowMessage = b40042.Wf40042();

         }
         catch (Exception ex)
         {
            File.Delete(_saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally
         {
            EndExport();
         }

         return ResultStatus.Success;
      }

   }
}