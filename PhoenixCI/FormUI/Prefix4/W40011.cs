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
/// john,20190410,保證金狀況表 (Group1) 
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group1) 
   /// </summary>
   public partial class W40011 : FormParent
   {
      private B40011 b40011;
      private string _saveFilePath;

      public W40011(string programID, string programName) : base(programID, programName)
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
         emDate.DateTimeValue = DateTime.Now;
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
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤")) {
            //is_chk = "Y";
            return false;
         }

         _saveFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
         b40011 = new B40011(_ProgramID, _saveFilePath, emDate.Text);

         stMsgTxt.Visible = true;

         //判斷FMIF資料已轉入
         string chkFMIF = b40011.CheckFMIF();
         if (chkFMIF != MessageDisplay.MSG_OK) {
            if (!OutputChooseMessage(chkFMIF))
               return false;
         }

         //130批次作業做完
         string strRtn = b40011.Check130Wf();
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
            File.Delete(_saveFilePath);
            return ResultStatus.Fail;
         }
         try
         {
            //Sheet : rpt_future
            ShowMsg($"{_ProgramID}_1－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.WfFutureSheet();
            //Sheet : rpt_option
            ShowMsg($"{_ProgramID}_2－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.WfOptionSheet();
            //Sheet : fut_3index
            ShowMsg("40011_stat－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.WfStat("F", "fut_3index");
            //Sheet : opt_3index
            ShowMsg("40011_stat－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.WfStat("O", "opt_3index");
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

      private void EWMAbtn_Click(object sender, EventArgs e)
      {
         string filepath = CopyExcelTemplateFile("40010_EWMA",FileType.XLS);
         try {
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);
            ShowMsg("EWMA 計算中...");
            OutputShowMessage = new B40010(_ProgramID, emDate.Text).ComputeEWMA(filepath,"40011_1","F");
            MessageDisplay.Info(MessageDisplay.MSG_IMPORT);
         }
         catch (Exception ex) {
            File.Delete(filepath);
            throw ex;
         }
         finally {
            EndExport();
         }
      }

   }
}