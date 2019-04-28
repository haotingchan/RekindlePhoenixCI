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
/// john,20190305,股票選擇權交易概況表
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 股票選擇權交易概況表
   /// </summary>
   public partial class W30360 : FormParent
   {
      private B30360 b30360;
      public W30360(string programID, string programName) : base(programID, programName)
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
         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30360");
         try {
            b30360 = new B30360(lsFile, emMonth.Text);
            //wf_30361()
            ShowMsg($"30361－股票選擇權交易概況表 轉檔中...");
            OutputShowMessage = b30360.Wf30361();
            //wf_30362()
            ShowMsg($"30362－股票選擇權交易概況表 轉檔中...");
            OutputShowMessage = b30360.Wf30362();
            //wf_30363()
            ShowMsg($"30363－股票選擇權交易概況表 轉檔中...");
            OutputShowMessage = b30360.Wf30363();

            int stcCount = new D30360().ApdkSTCcount();
            if (stcCount > 0) {
               //wf_30366()
               ShowMsg($"30366－股票選擇權交易概況表 轉檔中...");
               OutputShowMessage = b30360.Wf30366();
               //wf_30367()
               ShowMsg($"30367－股票選擇權交易概況表 轉檔中...");
               OutputShowMessage = b30360.Wf30367();
               //wf_30368()
               ShowMsg($"30368－股票選擇權交易概況表 轉檔中...");
               OutputShowMessage = b30360.Wf30368(); 
            }
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