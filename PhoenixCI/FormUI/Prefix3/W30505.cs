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
/// john,20190313,最佳1檔加權平均委託買賣價差統計表(週)
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 最佳1檔加權平均委託買賣價差統計表(週)
   /// </summary>
   public partial class W30505 : FormParent
   {
      private B30505 b30505;

      public W30505(string programID, string programName) : base(programID, programName)
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
         emStartDate.Text = GlobalInfo.OCF_DATE.ToString("2018/10/11");
         emEndDate.Text = GlobalInfo.OCF_DATE.ToString("2018/10/21");
#else
            emStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         emEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
#endif
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         emStartDate.Focus();
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
         if (!emStartDate.IsDate(emStartDate.Text, CheckDate.Start)) {
            //is_chk = "Y";
            return false;
         }
         if (!emEndDate.IsDate(emEndDate.Text, CheckDate.End)) {
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

      protected void ExportAfter()
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

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            //資料來源

            string saveFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,$@"30505_{DateTime.Now.ToString("yyyy.MM.dd")}-{DateTime.Now.ToString("HH.mm.ss")}.csv");
            b30505 = new B30505(saveFilePath, emStartDate.Text, emEndDate.Text);

            ShowMsg("30505－股票期貨最近月份契約買賣價差週資料統計表(單位：tick) 轉檔中...");
            OutputShowMessage = b30505.Wf30505();
         }
         catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            ExportAfter();
         }

         return ResultStatus.Success;
      }

      private string OutputShowMessage {
         set {
            if(value!=MessageDisplay.MSG_OK)
            MessageDisplay.Info(value);
         }
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