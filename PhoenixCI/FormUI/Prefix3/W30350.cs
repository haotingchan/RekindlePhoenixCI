using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
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
         //讀取交易日
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

      /// <summary>
      /// 轉檔前檢查日期格式
      /// </summary>
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

      /// <summary>
      /// 轉檔後清除文字訊息
      /// </summary>
      private void EndExport()
      {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      /// <summary>
      /// show出訊息在label
      /// </summary>
      /// <param name="msg"></param>
      private void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }

         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30350");
         //message.OutputShowMessage只會儲存ok的狀態,如沒有任何一個ok代表全部function都沒有資料
         MessageDisplay message = new MessageDisplay();
         Workbook workbook = new Workbook();
         //載入Excel
         workbook.LoadDocument(lsFile);
         try {

            string Txt = string.Empty;
            //輸入交易日期
            string emMonthTxt = emMonth.Text;
            //前月倒數1天交易日
            DateTime StartDate = PbFunc.f_get_last_day("AI2", "TXO", emMonthTxt, 1);
            //抓當月最後交易日
            DateTime EndDate = PbFunc.f_get_end_day("AI2", "TXO", emMonthTxt);

            b30350 = new B30350(workbook, emMonthTxt, StartDate, EndDate);
            //30350_01
            Txt = "臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 33, "TXO", "30350_01", Txt);
            //30350_02
            Txt = "金融選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 33, "TFO", "30350_02", Txt);
            //30350_03
            Txt = "電子選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 33, "TEO", "30350_03", Txt);
            //30350_04
            Txt = "摩臺選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 32, "MSO", "30350_04", Txt, B30350.Condition30350.sheet30350four);
            //30350_05
            Txt = "非金電選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 32, "XIO", "30350_05", Txt, B30350.Condition30350.RowIndexAddOne);
            //30350_06
            Txt = "櫃買選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 32, "GTO", "30350_06", Txt, B30350.Condition30350.RowIndexAddOne);
            //30350_07
            Txt = "黃金選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 32, "TGO", "30350_07", Txt, B30350.Condition30350.RowIndexAddOne);
            //30350_08
            Txt = "週臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30358(1, 33, "TXW", "30350_08", Txt, B30350.Condition30350.NoLastDay);
            //30350_09
            Txt = "月臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30358(1, 33, "TXO", "30350_09", Txt, B30350.Condition30350.NoLastDay);
            //30350_10
            Txt = "美元兌人民幣選擇權 成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 33, "RHO", "30350_10", Txt, B30350.Condition30350.NoLastMonth);
            //30350_11
            Txt = "小型美元兌人民幣選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{Txt} 轉檔中...");
            message.OutputShowMessage = b30350.DataFrom30351(1, 33, "RTO", "30350_11", Txt, B30350.Condition30350.NoLastMonth);
            //存檔
            workbook.SaveDocument(lsFile);
            //連續跳11次無資料刪除檔案
            if (string.IsNullOrEmpty(message.OutputShowMessage)) {
               if (File.Exists(lsFile))
                  File.Delete(lsFile);
               return ResultStatus.Fail;
            }
         }
         catch (Exception ex) {
            if (File.Exists(lsFile))
               File.Delete(lsFile);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }
         return ResultStatus.Success;
      }

   }
}