﻿using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using System.IO;
/// <summary>
/// john,20190320,標的證券為受益憑證之上市證券保證金狀況表
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 標的證券為受益憑證之上市證券保證金狀況表
   /// </summary>
   public partial class W43030 : FormParent
   {
      private B43030 b43030;

      public W43030(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         emDate.Text = DateTime.Today.ToString("yyyy/MM/dd");
#if DEBUG
         emDate.Text = "2014/10/20";
         this.Text += "(開啟測試模式),ocfDate=2014/10/20";
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
      protected void EndExport()
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
      protected void ShowMsg(string msg)
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
         MessageDisplay message = new MessageDisplay();
         string saveFilePath = PbFunc.wf_copy_file(_ProgramID, "43030");
         try {
            b43030 = new B43030(saveFilePath,emDate.Text);
            
            ShowMsg("43030－上市證券保證金概況表 轉檔中...");
            message.OutputShowMessage = b43030.Wf43030();
            if (string.IsNullOrEmpty(message.OutputShowMessage)) {
               ShowMsg("轉檔有錯誤!");
               if (File.Exists(saveFilePath))
                  File.Delete(saveFilePath);
               return ResultStatus.Fail;
            }
         }
         catch (Exception ex) {
            ShowMsg("轉檔有錯誤!");
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