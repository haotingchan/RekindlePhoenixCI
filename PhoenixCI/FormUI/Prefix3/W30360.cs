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
/// <summary>
/// john,20190305,股票選擇權交易概況表
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 股票選擇權交易概況表
   /// </summary>
   public partial class W30360 : FormParent
   {
      private B30360 b30360;
      public W30360(string programID, string programName) : base(programID, programName)
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

            bool isChk = false;//判斷是否執行成功
            string lsFile = PbFunc.wf_copy_file(_ProgramID, "30360");
            string msgTxt=string.Empty;

            b30360 = new B30360(lsFile, emMonth.Text);
            //wf_30361()
            ShowMsg($"30361－股票選擇權交易概況表 轉檔中...");
            isChk = b30360.Wf30361();
            //wf_30362()
            ShowMsg($"30362－股票選擇權交易概況表 轉檔中...");
            isChk = b30360.Wf30362();
            //wf_30363()
            ShowMsg($"30363－股票選擇權交易概況表 轉檔中...");
            isChk = b30360.Wf30363();

            int stcCount = new D30360().ApdkSTCcount();
            if (stcCount > 0) {
               //wf_30366()
               ShowMsg($"30366－股票選擇權交易概況表 轉檔中...");
               isChk = b30360.Wf30366();
               //wf_30367()
               ShowMsg($"30367－股票選擇權交易概況表 轉檔中...");
               isChk = b30360.Wf30367();
               //wf_30368()
               ShowMsg($"30368－股票選擇權交易概況表 轉檔中...");
               isChk = b30360.Wf30368(); 
            }

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