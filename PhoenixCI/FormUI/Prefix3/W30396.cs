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
/// john,20190319,布蘭特原油期貨契約價量資料
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 布蘭特原油期貨契約價量資料
   /// </summary>
   public partial class W30396 : FormParent
   {
      private B30396 b30396;
      public W30396(string programID, string programName) : base(programID, programName)
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
#if DEBUG
         emMonth.Text = GlobalInfo.OCF_DATE.ToString("2018/12");
#else
            emMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
#endif
         emMonth.Focus();
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

      private bool StartExport()
      {
         if (!emMonth.IsDate(emMonth.Text + "/01", "月份輸入錯誤")) {
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

      protected override ResultStatus Export()
      {
         try {
            if (!StartExport()) {
               return ResultStatus.Fail;
            }

            bool isChk = false;//判斷是否執行成功
            string lsFile;
            try {
               lsFile = PbFunc.wf_copy_file(_ProgramID, "30396");
            }
            catch (Exception) {
               lsFile = CopyExcelTemplateFile(_ProgramID, FileType.XLS);
            }
            
            b30396 = new B30396(lsFile, emMonth.Text);

            ShowMsg("30396－「BRF」期貨契約價量資料 轉檔中...");
            isChk = b30396.Wf30396();
            ShowMsg("30398abc－「BRF」期貨契約價量資料(買賣方比重) 轉檔中...");
            isChk = b30396.Wf30396abc();

            if (!isChk) return ResultStatus.Fail;//Exception
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
   }
}