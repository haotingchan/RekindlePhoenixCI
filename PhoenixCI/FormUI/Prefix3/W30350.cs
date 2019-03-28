﻿using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
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
            string lsFile = PbFunc.wf_copy_file(_ProgramID, "30350");
            string msgTxt=string.Empty;

            b30350 = new B30350(lsFile, emMonth.Text);
            //30350_01
            msgTxt = "臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 33, "TXO", "30350_01", msgTxt);
            //30350_02
            msgTxt = "金融選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 33, "TFO", "30350_02", msgTxt);
            //30350_03
            msgTxt = "電子選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 33, "TEO", "30350_03", msgTxt);
            //30350_04
            msgTxt = "摩臺選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 32, "MSO", "30350_04", msgTxt, B30350.Condition30350.sheet30350four);
            //30350_05
            msgTxt = "非金電選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 32, "XIO", "30350_05", msgTxt, B30350.Condition30350.RowIndexAddOne);
            //30350_06
            msgTxt = "櫃買選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 32, "GTO", "30350_06", msgTxt, B30350.Condition30350.RowIndexAddOne);
            //30350_07
            msgTxt = "黃金選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 32, "TGO", "30350_07", msgTxt, B30350.Condition30350.RowIndexAddOne);
            //30350_08
            msgTxt = "週臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30358(1, 33, "TXW", "30350_08", msgTxt, B30350.Condition30350.NoLastDay);
            //30350_09
            msgTxt = "月臺指選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30358(1, 33, "TXO", "30350_09", msgTxt, B30350.Condition30350.NoLastDay);
            //30350_10
            msgTxt = "美元兌人民幣選擇權 成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 33, "RHO", "30350_10", msgTxt, B30350.Condition30350.NoLastMonth);
            //30350_11
            msgTxt = "小型美元兌人民幣選擇權成交量及未平倉量變化表";
            ShowMsg($"30350－{msgTxt} 轉檔中...");
            isChk = b30350.DataFrom30351(1, 33, "RTO", "30350_11", msgTxt, B30350.Condition30350.NoLastMonth);

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