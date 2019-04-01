using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using BaseGround.Shared;
using Common;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix3;
using DataObjects.Dao.Together.SpecificDao;
/// <summary>
/// john,20190329,每月報局交易量報表(國內期貨暨選擇權)
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 每月報局交易量報表(國內期貨暨選擇權)
   /// </summary>
   public partial class W30780 : FormParent
   {
      private B30780 b30780;
      public W30780(string programID, string programName) : base(programID, programName)
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
         emMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
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
         if (!emMonth.IsDate(emMonth.Text, "日期輸入錯誤!")) {
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
            string lsFile = PbFunc.wf_copy_file(_ProgramID, "30780");
            //交易時段
            string lsMarketCode = string.Empty;
            switch (rgTime.EditValue.ToString()) {
               case "rb_market0":
                  lsMarketCode = "0";
                  break;
               case "rb_market1":
                  lsMarketCode = "1";
                  break;
               default:
                  lsMarketCode = "%";
                  break;
            }
            //em_end_date.text = string(relativedate(date(em_month.text + "/01"), 31), 'yyyy/mm') + "/10";
            DateTime endDate=PbFunc.relativedate(emMonth.Text.AsDateTime("yyyy/MM"),31);
            //下個月10日
            endDate = new DateTime(endDate.Year, endDate.Month, 10);
            
            b30780 = new B30780(lsFile, emMonth.Text, lsMarketCode,new D30780().MaxDate(endDate));
            
            ShowMsg("30780_1－附表1_期貨暨選擇權最近2個月市場成交量變動比較表 轉檔中...");
            OutputShowMessage = b30780.WF30780one();
            ShowMsg("30780_2－附表2_期貨暨選擇權最近6個月市場成交量彙總表 轉檔中...");
            OutputShowMessage = b30780.WF30780two();
            ShowMsg("30780_4－附表4_國內期貨市場主要期貨商月市占率概況表(依成交量排序) 轉檔中...");
            OutputShowMessage = b30780.WF30780four();
            ShowMsg("30780_5－附表5_國內期貨市場期貨商月成交量成長暨衰退概況表 轉檔中...");
            OutputShowMessage = b30780.WF30780five();
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