using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using System.IO;
using Common;
using DataObjects.Dao.Together.SpecificDao;
/// <summary>
/// john,20190410,保證金狀況表 (Group1) 
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group1) 
   /// </summary>
   public partial class W40011 : FormParent
   {
      private B40011 b40011;

      public W40011(string programID, string programName) : base(programID, programName)
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
#if DEBUG
         emDate.Text = "2018/10/12";
#else
            emDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
#endif
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         emDate.Focus();
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
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤"))
         {
            //is_chk = "Y";
            return false;
         }
         //判斷FMIF資料已轉入
         DateTime inputDT = emDate.Text.AsDateTime();
         int cnt = new D40011().CheckFMIF(inputDT);
         if (cnt == 0)
         {
            DialogResult ChooseResult = MessageDisplay.Choose(emDate.Text + "期貨結算價資料未轉入完畢,是否要繼續?");
            if (ChooseResult == DialogResult.No)
            {
               return false;
            }
         }
         //130批次作業做完
         string strRtn = PbFunc.f_chk_130_wf(_ProgramID, inputDT, "1");
         if (!string.IsNullOrEmpty(strRtn))
         {
            DialogResult ChooseResult = MessageDisplay.Choose($"{emDate.Text}-{strRtn}，是否要繼續?");
            if (ChooseResult == DialogResult.No)
            {
               return false;
            }
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

      private string OutputShowMessage {
         set {
            if (value != MessageDisplay.MSG_OK)
               MessageDisplay.Info(value);
         }
      }

      protected override ResultStatus Export()
      {
         if (!StartExport())
         {
            return ResultStatus.Fail;
         }
         string saveFilePath = PbFunc.wf_copy_file(_ProgramID, "40011");
         try
         {
            b40011 = new B40011(saveFilePath, emDate.Text);

            //Sheet : rpt_future
            ShowMsg("40011_1－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.Wf40011FutureSheet();
            //Sheet : rpt_option
            ShowMsg("40011_2－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.Wf40011OptionSheet();
         }
         catch (Exception ex)
         {
            File.Delete(saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally
         {
            EndExport();
         }

         return ResultStatus.Success;
      }

   }
}