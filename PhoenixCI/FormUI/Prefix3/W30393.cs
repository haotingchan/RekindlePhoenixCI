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
using System.Data;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System.Threading.Tasks;
using System.Collections.Generic;
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

      private bool StartExport()
      {
         if (!emMonth.IsDate(emMonth.Text + "/01", "月份輸入錯誤")) {
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
         Workbook workbook = new Workbook();
         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30393");
         //載入Excel
         workbook.LoadDocument(lsFile);
         try {

            b30393 = new B30393(workbook, emMonth.Text);
            //前月倒數2天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI3", "RHF", emMonth.Text, 2);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI3", "RHF", emMonth.Text);

            //RHF
            ShowMsg("30393－「RHF」期貨契約價量資料 轉檔中...");
            OutputShowMessage = b30393.Wf30393(StartDate, EndDate, "RHF", "30393_1(RHF)");
            ShowMsg("30397－「黃金」期貨契約價量資料(買賣方比重) 轉檔中...");
            OutputShowMessage = b30393.Wf30393abc("RHF", "data_30393_1abc");

            //RTF
            ShowMsg("30393－「RTF」期貨契約價量資料 轉檔中...");
            OutputShowMessage = b30393.Wf30393(StartDate, EndDate, "RTF", "30393_2(RTF)");
            ShowMsg("30397－「黃金」期貨契約價量資料(買賣方比重) 轉檔中...");
            OutputShowMessage = b30393.Wf30393abc("RTF", "data_30393_2abc");

            //XEF
            ShowMsg("30393－「XEF」期貨契約價量資料 轉檔中...");
            OutputShowMessage = b30393.Wf30393(StartDate, EndDate, "XEF", "30393_3(XEF)");
            ShowMsg("30397－「黃金」期貨契約價量資料(買賣方比重) 轉檔中...");
            OutputShowMessage = b30393.Wf30393abc("XEF", "data_30393_3abc");

            //XJF
            ShowMsg("30393－「XJF」期貨契約價量資料 轉檔中...");
            OutputShowMessage = b30393.Wf30393(StartDate, EndDate, "XJF", "30393_4(XJF)");
            ShowMsg("30397－「黃金」期貨契約價量資料(買賣方比重) 轉檔中...");
            OutputShowMessage = b30393.Wf30393abc("XJF", "data_30393_4abc");

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