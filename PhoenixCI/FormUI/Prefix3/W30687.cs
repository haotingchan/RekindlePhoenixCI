using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using PhoenixCI.Shared;
using DataObjects.Dao.Together.SpecificDao;
using Common;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix7;
using PhoenixCI.BusinessLogic.Prefix3;
/// <summary>
/// john,20190227,動態價格穩定措施基準價查詢
/// </summary>
namespace PhoenixCI.FormUI.Prefix7
{
   /// <summary>
   /// 動態價格穩定措施基準價查詢
   /// </summary>
   public partial class W30687 : FormParent
   {
      private B30687 b30687;
      string saveFilePath;
      public W30687(string programID, string programName) : base(programID, programName)
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
         emStartDate.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;
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
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void ExportAfter()
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

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {

            //盤別
            string lsMarketCode = string.Empty;
            switch (rgMarket.SelectedIndex) {
               case 0://日盤
                  lsMarketCode = "0";
                  break;
               case 1://夜盤
                  lsMarketCode = "1";
                  break;
               default://全部
                  lsMarketCode = "%";
                  break;
            }
            //時段
            string lsDataType = string.Empty;
            switch (rgTime.SelectedIndex) {
               case 0://盤前
                  lsDataType = "B";
                  break;
               case 1://交易時段
                  lsDataType = "";
                  break;
               default://全部
                  lsDataType = "A";
                  break;
            }
            //資料來源
            b30687 = new B30687(saveFilePath);

            ShowMsg("30687_ru_new－RU筆數統計 轉檔中...");
            ExportAfter();
         }
         catch (Exception ex) {
            ExportAfter();
            throw ex;
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