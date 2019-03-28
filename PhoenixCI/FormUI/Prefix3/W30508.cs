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
/// john,20190313,最佳1檔加權平均委託買賣數量統計表(週) 
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 最佳1檔加權平均委託買賣數量統計表(週) 
   /// </summary>
   public partial class W30508 : FormParent
   {
      private B30508 b30508;

      public W30508(string programID, string programName) : base(programID, programName)
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
         emStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         emEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
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
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            //資料來源
            string saveFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,$@"30508(買)_{DateTime.Now.ToString("yyyy.MM.dd")}-{DateTime.Now.ToString("HH.mm.ss")}.csv");
            b30508 = new B30508(saveFilePath, emStartDate.Text, emEndDate.Text);
            bool isChk = false;//判斷是否執行成功

            ShowMsg("30508－股票期貨最近月份契約最佳1檔加權平均委託買進數量週資料統計表 轉檔中...");
            isChk = b30508.Wf30508();
            ExportAfter();
            if (!isChk) return ResultStatus.Fail;//Exception
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