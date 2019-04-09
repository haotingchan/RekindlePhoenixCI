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
/// john,20190218,證期局七組月報
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 證期局七組月報
   /// </summary>
   public partial class W30310 : FormParent
   {
      private B30310 b30310;
      public W30310(string programID, string programName) : base(programID, programName)
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

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      private bool ExportBefore()
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

      protected void ExportAfter()
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
         try {
            if (!ExportBefore()) {
               return ResultStatus.Fail;
            }

            string lsFile = PbFunc.wf_copy_file(_ProgramID, "30310");
            b30310 = new B30310(lsFile, emMonth.Text);
            bool isChk = false;//判斷是否執行成功
            ShowMsg("30310－我國臺股期貨契約價量資料30311_1 轉檔中...");
            isChk = b30310.Wf30310one("TXF", "30311_1");
            ShowMsg("30310－我國臺股期貨契約價量資料30311_2(EXF) 轉檔中...");
            isChk = b30310.Wf30310two("EXF", "30311_2(EXF)");
            ShowMsg("30310－我國臺股期貨契約價量資料30311_3(FXF) 轉檔中...");
            isChk = b30310.Wf30310two("FXF", "30311_3(FXF)");
            ShowMsg("30313－當年每月日均量統計表 轉檔中...");
            isChk = b30310.Wf30310four();
            ExportAfter();
            if (!isChk) return ResultStatus.Fail;//if Exception
         }
         catch (Exception ex) {
            ExportAfter();
            WriteLog(ex);
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