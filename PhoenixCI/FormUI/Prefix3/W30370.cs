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
/// john,20190305,年度期間法人機構期貨交易量統計表 
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 年度期間法人機構期貨交易量統計表 
   /// </summary>
   public partial class W30370 : FormParent
   {
      private B30370 b30370;
      public W30370(string programID, string programName) : base(programID, programName)
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

      private string OutputShowMessage {
         set {
            if (value != MessageDisplay.MSG_OK && value != MessageDisplay.MSG_NO_DATA)
               MessageDisplay.Info(value);
         }
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
         stMsgtxt.Visible = true;
         stMsgtxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void EndExport()
      {
         stMsgtxt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgtxt.Visible = false;
      }

      protected void ShowMsg(string msg)
      {
         stMsgtxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30370");
         try {
            b30370 = new B30370(lsFile, emMonth.Text);
            //wf_30371()
            ShowMsg($"30371－年度期間法人機構期貨交易量統計表 轉檔中...");
            OutputShowMessage = b30370.Wf30371();
            //wf_30375()
            ShowMsg($"30375－年度期間法人機構期貨交易量統計表(維持率) 轉檔中...");
            OutputShowMessage = b30370.Wf30375();
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