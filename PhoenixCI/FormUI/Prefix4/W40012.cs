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
/// john,20190415,保證金狀況表 (Group2) 
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group2) 
   /// </summary>
   public partial class W40012 : FormParent
   {
      private I4001x b4001xTemp;
      /// <summary>
      /// 存檔路徑
      /// </summary>
      private string _saveFilePath;

      public W40012(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();
#if DEBUG
         emDate.Text = "2018/10/12";
#else
         emDate.DateTimeValue = DateTime.Now;
#endif
         emDate.Focus();
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
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤")) {
            //is_chk = "Y";
            return false;
         }

         _saveFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
         object[] args = { _ProgramID, _saveFilePath, emDate.Text };
         b4001xTemp = new B4001xTemplate().ConcreteClass(_ProgramID, args);

         stMsgTxt.Visible = true;

         //判斷FMIF資料已轉入
         string chkFMIF = b4001xTemp.CheckFMIF();
         if (chkFMIF != MessageDisplay.MSG_OK) {
            if (!OutputChooseMessage(chkFMIF))
               return false;
         }

         //130批次作業做完
         string strRtn = b4001xTemp.Check130Wf();
         if (strRtn != MessageDisplay.MSG_OK) {
            if (!OutputChooseMessage(strRtn))
               return false;
         }

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
      protected void ShowMsg(string msg)
      {
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      private bool OutputChooseMessage(string str)
      {
         DialogResult ChooseResult = MessageDisplay.Choose(str);
         if (ChooseResult == DialogResult.No) {
            EndExport();
            return false;
         }
         return true;
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            File.Delete(_saveFilePath);
            return ResultStatus.Fail;
         }
         try {
            MessageDisplay message = new MessageDisplay();
            //Sheet : rpt_future
            ShowMsg($"{_ProgramID}_1－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b4001xTemp.WfFutureSheet();
            //Sheet : rpt_option
            ShowMsg($"{_ProgramID}_2－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b4001xTemp.WfOptionSheet();
            //Sheet : fut_3index
            ShowMsg($"{_ProgramID}_stat－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b4001xTemp.WfStat("F", "fut_3index");
            //Sheet : opt_3index
            ShowMsg($"{_ProgramID}_stat－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b4001xTemp.WfStat("O", "opt_3index");
         }
         catch (Exception ex) {
            if (File.Exists(_saveFilePath))
               File.Delete(_saveFilePath);
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