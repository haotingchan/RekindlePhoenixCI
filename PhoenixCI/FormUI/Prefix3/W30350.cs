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
/// john,20190226,臺指選擇權成交量及未平倉量變化表
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 臺指選擇權成交量及未平倉量變化表
   /// </summary>
   public partial class W30350 : FormParent
   {
      private B30350 b30350;
      public W30350(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
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

         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30350");

         Workbook workbook = new Workbook();
         //載入Excel
         workbook.LoadDocument(lsFile);
         try {

            string msgTxt = string.Empty;

            //前月倒數1天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI2", "TXO", emMonth.Text, 1);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI2", "TXO", emMonth.Text);

            b30350 = new B30350(workbook, StartDate, EndDate);
            //30350_01
            msgTxt = "臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 33, "TXO", "30350_01", msgTxt);
            //30350_02
            msgTxt = "金融選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 33, "TFO", "30350_02", msgTxt);
            //30350_03
            msgTxt = "電子選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 33, "TEO", "30350_03", msgTxt);
            //30350_04
            msgTxt = "摩臺選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 32, "MSO", "30350_04", msgTxt, B30350.Condition30350.sheet30350four);
            //30350_05
            msgTxt = "非金電選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 32, "XIO", "30350_05", msgTxt, B30350.Condition30350.RowIndexAddOne);
            //30350_06
            msgTxt = "櫃買選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 32, "GTO", "30350_06", msgTxt, B30350.Condition30350.RowIndexAddOne);
            //30350_07
            msgTxt = "黃金選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 32, "TGO", "30350_07", msgTxt, B30350.Condition30350.RowIndexAddOne);
            //30350_08
            msgTxt = "週臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30358(1, 33, "TXW", "30350_08", msgTxt, B30350.Condition30350.NoLastDay);
            //30350_09
            msgTxt = "月臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30358(1, 33, "TXO", "30350_09", msgTxt, B30350.Condition30350.NoLastDay);
            //30350_10
            msgTxt = "美元兌人民幣選擇權 成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 33, "RHO", "30350_10", msgTxt, B30350.Condition30350.NoLastMonth);
            //30350_11
            msgTxt = "小型美元兌人民幣選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            OutputShowMessage = b30350.DataFrom30351(1, 33, "RTO", "30350_11", msgTxt, B30350.Condition30350.NoLastMonth);

         }
         catch (Exception ex) {
            File.Delete(lsFile);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            workbook.SaveDocument(lsFile);
            EndExport();
         }
         return ResultStatus.Success;
      }

   }
}