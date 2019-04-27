using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
using DataObjects.Dao.Together.SpecificDao;
using System.IO;
/// <summary>
/// john,20190305,新加坡交易所(SGX)摩根臺股期貨市場概況表 
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 新加坡交易所(SGX)摩根臺股期貨市場概況表 
   /// </summary>
   public partial class W30380 : FormParent
   {
      private B30380 b30380;
      public W30380(string programID, string programName) : base(programID, programName)
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
         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30380");

         try {
            b30380 = new B30380(lsFile, emMonth.Text);
            //wf_30311()
            ShowMsg($"30310－當年每月日均量統計表 轉檔中...");
            OutputShowMessage = b30380.Wf30311();
            //wf_30381()
            ShowMsg($"30380－新加坡交易所(SGX)摩根臺股期貨市場概況表 轉檔中...");
            OutputShowMessage = b30380.Wf30381();

         }
         catch (Exception ex) {
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