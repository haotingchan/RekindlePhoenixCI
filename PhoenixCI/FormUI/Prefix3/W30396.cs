using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
using DevExpress.Spreadsheet;
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

      private void EndExport()
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

         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30396");

         Workbook workbook = new Workbook();
         //載入Excel
         workbook.LoadDocument(lsFile);
         try {
            b30396 = new B30396(workbook, emMonth.Text);

            ShowMsg("30396－「BRF」期貨契約價量資料 轉檔中...");
            OutputShowMessage = b30396.Wf30396();
            ShowMsg("30398abc－「BRF」期貨契約價量資料(買賣方比重) 轉檔中...");
            OutputShowMessage = b30396.Wf30396abc();

         }
         catch (Exception ex) {
            File.Delete(lsFile);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            //存檔
            workbook.SaveDocument(lsFile);
            EndExport();
         }
         return ResultStatus.Success;
      }

   }
}