using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
/// <summary>
/// john,20190312,指數類期貨及現貨資料下載
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 指數類期貨及現貨資料下載
   /// </summary>
   public partial class W40200 : FormParent
   {
      private B40200 b40200;

      public W40200(string programID, string programName) : base(programID, programName)
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
         emStartDate.Text = DateTime.Today.ToString("yyyy/MM/dd");
         emEndDate.Text = DateTime.Today.ToString("yyyy/MM/dd");
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
         return ResultStatus.Success;
      }

      private bool StartExport()
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

      protected void EndExport()
      {
         stMsgTxt.Text = "轉檔完成!";
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
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         try {
            //資料來源
            string saveFilePath = PbFunc.wf_copy_file(_ProgramID, "40200");
            b40200 = new B40200(saveFilePath,emStartDate.Text,emEndDate.Text);
            bool isChk = false;//判斷是否執行成功
            
            ShowMsg("40200－指數類期貨價格及現貨資料下載 轉檔中...");
            isChk=b40200.Wf40200();
            EndExport();
            if (!isChk) return ResultStatus.Fail;//Exception
         }
         catch (Exception ex) {
            EndExport();
            WriteLog(ex);
            return ResultStatus.Fail;
         }

         return ResultStatus.Success;
      }

   }
}