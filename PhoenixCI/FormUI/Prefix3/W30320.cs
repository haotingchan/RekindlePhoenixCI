using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
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
      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         em_month.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         em_month.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         em_month.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      private bool ExportBefore()
      {
         /*******************
         Messagebox
         *******************/
         st_msg_txt.Visible = true;
         st_msg_txt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void ExportAfter()
      {
         st_msg_txt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         st_msg_txt.Visible = false;
      }

      protected void ShowMsg(string msg)
      {
         st_msg_txt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            bool isComply = false;//判斷是否執行成功
            string ls_file = PbFunc.wf_copy_file(_ProgramID, "30320");
            b30320 = new B30320(ls_file, em_month.Text);

            ShowMsg("30321－指數期貨價量資料 轉檔中...");
            isComply = b30320.wf_30321();
            if (!isComply) return ResultStatus.Fail;
            ShowMsg("30322－指數期貨價量資料 轉檔中...");
            isComply = b30320.wf_30322();
            if (!isComply) return ResultStatus.Fail;
            ExportAfter();
         }
         catch (Exception ex) {
            ExportAfter();
            PbFunc.messageBox(GlobalInfo.gs_t_err, ex.Message, MessageBoxIcon.Stop);
            return ResultStatus.Fail;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Export(ReportHelper reportHelper)
      {
         base.Export(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus COMPLETE()
      {
         return ResultStatus.Success;
      }
   }
}