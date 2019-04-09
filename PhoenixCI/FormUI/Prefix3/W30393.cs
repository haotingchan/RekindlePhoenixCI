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
/// john,20190307,匯率類期貨契約價量資料
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 匯率類期貨契約價量資料
   /// </summary>
   public partial class W30393 : FormParent
   {
      private B30393 b30393;
      public W30393(string programID, string programName) : base(programID, programName)
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
         emMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
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
            string lsFile = PbFunc.wf_copy_file(_ProgramID, "30393");
            b30393 = new B30393(lsFile, emMonth.Text);

            //RHF
            ShowMsg("30393－「RHF」期貨契約價量資料 轉檔中...");
            isChk = b30393.Wf30393("RHF", "30393_1(RHF)");
            ShowMsg("30397－「黃金」期貨契約價量資料(買賣方比重) 轉檔中...");
            isChk = b30393.Wf30393abc("RHF", "data_30393_1abc");

            //RTF
            ShowMsg("30393－「RTF」期貨契約價量資料 轉檔中...");
            isChk = b30393.Wf30393("RTF", "30393_2(RTF)");
            ShowMsg("30397－「黃金」期貨契約價量資料(買賣方比重) 轉檔中...");
            isChk = b30393.Wf30393abc("RTF", "data_30393_2abc");

            //XEF
            ShowMsg("30393－「XEF」期貨契約價量資料 轉檔中...");
            isChk = b30393.Wf30393("XEF", "30393_3(XEF)");
            ShowMsg("30397－「黃金」期貨契約價量資料(買賣方比重) 轉檔中...");
            isChk = b30393.Wf30393abc("XEF", "data_30393_3abc");

            //XJF
            ShowMsg("30393－「XJF」期貨契約價量資料 轉檔中...");
            isChk = b30393.Wf30393("XJF", "30393_4(XJF)");
            ShowMsg("30397－「黃金」期貨契約價量資料(買賣方比重) 轉檔中...");
            isChk = b30393.Wf30393abc("XJF", "data_30393_4abc");

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