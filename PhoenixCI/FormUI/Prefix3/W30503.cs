using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
/// <summary>
/// 20190402,john,最佳1檔加權平均委託買賣價差統計表(月) 
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 最佳1檔加權平均委託買賣價差統計表(月)
   /// </summary>
   public partial class W30503 : FormParent
   {
      private B30503 b30503;
      public W30503(string programID, string programName) : base(programID, programName)
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
         emStartMth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         emEndMth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
#if DEBUG
         emStartMth.Text = "2018/08";
         emEndMth.Text = "2018/10";
#endif
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         emStartMth.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      private bool StartExport()
      {
         if (!emStartMth.IsDate(emStartMth.Text + "/01", CheckDate.Start)) {
            //is_chk = "Y";
            return false;
         }
         if (!emEndMth.IsDate(emEndMth.Text + "/01", CheckDate.End)) {
            //is_chk = "Y";
            return false;
         }
         if(emStartMth.Text.SubStr(0,4)!= emEndMth.Text.SubStr(0, 4)) {
            MessageDisplay.Error("不可跨年度查詢!");
            emStartMth.Focus();
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

      private void EndExport()
      {
         stMsgtxt.Text = "";
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

      private string OutputShowMessage {
         set {
            if (value != MessageDisplay.MSG_OK)
               MessageDisplay.Info(value);
         }
      }

      protected override ResultStatus Export()
      {
         string saveFilePath = PbFunc.wf_copy_file(_ProgramID, "30503");
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         string msg;
         try {
            //轉報表
            b30503 = new B30503(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, saveFilePath, emStartMth.Text, emEndMth.Text);
            ShowMsg("30503－股票期貨最近月份契約買賣價差月資料統計表 轉檔中...");
            msg = b30503.WF30503();
            OutputShowMessage = msg;
            ShowMsg("30504－股票期貨最近月份契約買賣價差日資料 轉檔中...");
            OutputShowMessage = b30503.WF30504();
         }
         catch (Exception ex) {
            File.Delete(saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }

         if (msg != MessageDisplay.MSG_OK) {
            File.Delete(saveFilePath);
            return ResultStatus.Fail;
         }
         return ResultStatus.Success;
      }
   }
}