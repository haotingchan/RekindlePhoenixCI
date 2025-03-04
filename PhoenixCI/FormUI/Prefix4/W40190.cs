﻿using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using System.IO;
using Common;
/// <summary>
/// john,20190408,每月保證金狀況表
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 每月保證金狀況表
   /// </summary>
   public partial class W40190 : FormParent
   {
      private B40190 b40190;

      public W40190(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();
#if DEBUG
         emDate.Text = "2018/10/12";
#else
            emDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
#endif
         emDate.Focus();
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
      /// <returns></returns>
      private bool StartExport()
      {
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤")) {
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
         stMsgTxt.Text = "轉檔完成!";
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
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         string saveFilePath = PbFunc.wf_copy_file(_ProgramID, "40190");
         MessageDisplay message = new MessageDisplay();
         try {
            b40190 = new B40190(saveFilePath,emDate.Text);

            ShowMsg("40190_1－期貨保證金 轉檔中...");
            message.OutputShowMessage = b40190.Wf40191();
            ShowMsg("40192－選擇權保證金 轉檔中...");
            message.OutputShowMessage = b40190.Wf40192();
            ShowMsg("40193－調整狀況 轉檔中...");
            message.OutputShowMessage = b40190.Wf40193();

            //沒有任何資料時刪除檔案
            if (string.IsNullOrEmpty(message.OutputShowMessage)) {
               //要跳3次無任何資料才能刪除
               if (File.Exists(saveFilePath))
                  File.Delete(saveFilePath);
               return ResultStatus.Fail;
            }
         }
         catch (Exception ex) {
            if (File.Exists(saveFilePath))
               File.Delete(saveFilePath);
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