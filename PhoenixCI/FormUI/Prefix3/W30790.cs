using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;
/// <summary>
/// 20190327,john,盤後交易時段分時交易量分布
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 盤後交易時段分時交易量分布
   /// </summary>
   public partial class W30790 : FormParent
   {
      private B30790 b30790;
      public W30790(string programID, string programName) : base(programID, programName)
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
         emStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         emEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         string lsYMD = new D30790().MaxDate();
         if (!string.IsNullOrEmpty(lsYMD)) {
            emEndDate.Text = lsYMD;
         }
         emStartDate.Text = PbFunc.relativedate(lsYMD.AsDateTime(),-6).ToString("yyyy/MM/dd");
         emStartDate.Focus();
         emTxEndDate.Text= lsYMD;
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
         if (!emStartDate.IsDate(emStartDate.Text, CheckDate.Start))
         {
            //is_chk = "Y";
            return false;
         }
         if (!emEndDate.IsDate(emEndDate.Text, CheckDate.End))
         {
            //is_chk = "Y";
            return false;
         }
         if (!emTxStartDate.IsDate(emTxStartDate.Text, CheckDate.Start))
         {
            //is_chk = "Y";
            return false;
         }
         if (!emTxEndDate.IsDate(emTxEndDate.Text, CheckDate.End))
         {
            //is_chk = "Y";
            return false;
         }
         /*******************
         Messagebox
         *******************/
         stMsgTxt.Visible = true;

         gb.Enabled = false;
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
         gb.Enabled = true;
      }

      protected void ShowMsg(string msg)
      {
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }
      /// <summary>
      /// show文字訊息
      /// </summary>
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
         try {
            string lsFile = PbFunc.wf_copy_file(_ProgramID, "30790");
            b30790 = new B30790(lsFile, emStartDate.Text, emEndDate.Text,emTxStartDate.Text , emTxEndDate.Text);

            if (chkAvg.Checked) {
               ShowMsg("30790－盤後交易時段分時交易量分布 轉檔中...");
               OutputShowMessage = b30790.Wf30790();
            }
            //TX每日日盤及夜盤之振幅及收盤價
            if (chkTx.Checked) {
               ShowMsg("30790_4－盤後交易時段分時交易量分布 轉檔中...");
               OutputShowMessage = b30790.Wf30790four();
            }
            
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