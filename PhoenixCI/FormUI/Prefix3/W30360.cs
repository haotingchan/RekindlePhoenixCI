using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
using DataObjects.Dao.Together.SpecificDao;
using System.IO;
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
         string lsFile = PbFunc.wf_copy_file(_ProgramID, "30360");
         //message.OutputShowMessage只會儲存ok的狀態,如沒有任何一個ok代表全部function都沒有資料
         MessageDisplay message = new MessageDisplay();
         try {
            b30360 = new B30360(lsFile, emMonth.Text);
            //wf_30361()
            ShowMsg($"30361－股票選擇權交易概況表 轉檔中...");
            message.OutputShowMessage = b30360.Wf30361();
            //wf_30362()
            ShowMsg($"30362－股票選擇權交易概況表 轉檔中...");
            message.OutputShowMessage = b30360.Wf30362();
            //wf_30363()
            ShowMsg($"30363－股票選擇權交易概況表 轉檔中...");
            message.OutputShowMessage = b30360.Wf30363();

            int stcCount = new D30360().ApdkSTCcount();
            if (stcCount > 0) {
               //wf_30366()
               ShowMsg($"30366－股票選擇權交易概況表 轉檔中...");
               message.OutputShowMessage = b30360.Wf30366();
               //wf_30367()
               ShowMsg($"30367－股票選擇權交易概況表 轉檔中...");
               message.OutputShowMessage = b30360.Wf30367();
               //wf_30368()
               ShowMsg($"30368－股票選擇權交易概況表 轉檔中...");
               message.OutputShowMessage = b30360.Wf30368(); 
            }

            //連續跳3 or 6次無資料刪除檔案
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