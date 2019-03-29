using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using BaseGround.Shared;
using Common;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix3;
/// <summary>
/// john,20190328,交易量資料轉檔作業
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 交易量資料轉檔作業
   /// </summary>
   public partial class W30720 : FormParent
   {
      private B30720 b30720;
      public W30720(string programID, string programName) : base(programID, programName)
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
         emMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         sleYear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;
         //年月報選取事件
         this.rgDate.SelectedIndexChanged += new System.EventHandler(this.rgDate_SelectedIndexChanged);
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      private bool StartExport()
      {
         if (!emMonth.IsDate(emMonth.Text, "日期輸入錯誤!")) {
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
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }
      /// <summary>
      /// show文字訊息
      /// </summary>
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
         try {
            string lsFile = CopyExcelTemplateFile(_ProgramID, FileType.XLS);
            b30720 = new B30720(lsFile, emMonth.Text, sleYear.Text, rgDate.EditValue.ToString(), rgTime.EditValue.ToString());

            ShowMsg("30720－月份交易量彙總表 轉檔中...");
            OutputShowMessage = b30720.WF30720();

         }
         catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
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
      private void rgDate_SelectedIndexChanged(object sender, EventArgs e)
      {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;
         switch (rb.EditValue.ToString()) {
            case "rb_month"://月報
               emMonth.Enabled = true;
               sleYear.Enabled = false;
               break;
            case "rb_year"://年報
               emMonth.Enabled = false;
               sleYear.Enabled = true;
               break;
            default:
               break;
         }
      }
   }
}