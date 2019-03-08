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
/// <summary>
/// john,20190212,造市者交易量轉檔作業
/// </summary>
namespace PhoenixCI.FormUI.Prefix7
{
   /// <summary>
   /// 造市者交易量轉檔作業
   /// </summary>
   public partial class W70020 : FormParent
   {
      private B70020 b70020;
      private D70020 dao70020;
      string saveFilePath;
      public W70020(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         dao70020 = new D70020();
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         emStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/") + "01";
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
         /* 條件值檢核*/
         string lsRtn;
         DialogResult liRtn;
         if (rgTime.EditValue.Equals("rb_market1")) {
            lsRtn = "1";
         }
         else {
            lsRtn = "0";
         }
         //檢查批次作業是否完成
         lsRtn = PbFunc.f_get_jsw_seq(_ProgramID, "E", 0, emEndDate.Text.AsDateTime(), "0");//f_get_jsw_seq(is_txn_id,'E',0,datetime(date(em_edate.text)),'0')
         if (lsRtn!="") {
            liRtn = MessageBox.Show(emEndDate.Text + " 統計資料未轉入完畢,是否要繼續?\r\n" + lsRtn, GlobalInfo.QuestionText, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(liRtn== DialogResult.No) {
               stMsgTxt.Visible = false;
               this.Cursor = Cursors.Arrow;
               return false;
            }
         }
         //檢查日期是否符合DateTime型別
         if (!emStartDate.IsDate(emStartDate.Text, CheckDate.Start)
                  || !emEndDate.IsDate(emEndDate.Text, CheckDate.End)) {
            return false;
         }
         if (string.Compare(emStartDate.Text, emEndDate.Text) > 0) {
            MessageDisplay.Error(GlobalInfo.ErrorText, CheckDate.Datedif);
            return false;
         }
         //TextBox轉DateTime
         DateTime ldStart = Convert.ToDateTime(emStartDate.Text);
         DateTime ldEnd = Convert.ToDateTime(emEndDate.Text);
         //資料來源選取
         string lsType;
         if (rgData.EditValue.Equals("rb_mmk")) {
            lsType = "_M";
         }
         else {
            lsType = "";
         }

         /*點選儲存檔案之目錄*/
         switch (rgTime.EditValue) {
            case "rb_market0":
               lsRtn = "_ 一般";
               break;
            case "rb_market1":
               lsRtn = "_ 盤後";
               break;
            default:
               lsRtn = "_ 全部";
               break;
         }
         //選取儲存路徑
         saveFilePath = ReportExportFunc.wf_GetFileSaveName($@"MarketMaker{lsType}_{lsRtn}-{ldStart.ToString("yyyyMMdd")}-{ldEnd.ToString("yyyyMMdd")}.xls");
         if (string.IsNullOrEmpty(saveFilePath)) {
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
      
      protected void ExportAfter()
      {
         stMsgTxt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            string lsMarketCode;

            //交易時段
            switch (rgTime.EditValue) {
               case "rb_market0"://一般
                  lsMarketCode = "0%";
                  break;
               case "rb_market1"://盤後
                  lsMarketCode = "1%";
                  break;
               default://全部
                  lsMarketCode = "%";
                  break;
            }

            string startDate = emStartDate.Text.Replace("/", "").SubStr(0, 8);
            string endDate = emEndDate.Text.Replace("/", "").SubStr(0, 8);
            //資料來源
            b70020 = new B70020(saveFilePath);
            switch (rgData.EditValue) {
               case "rb_0"://自營商成交量(身份碼8,2)
                  b70020.ExportAM8(startDate, endDate, lsMarketCode);
                  break;
               case "rb_mtf"://成交資料
                  b70020.ExportListO(startDate, endDate, lsMarketCode);
                  break;
               case "rb_mmk"://造市者資料
                  b70020.ExportListM(startDate, endDate, lsMarketCode);
                  break;
               default:
                  break;
            }
            WriteLog("轉出檔案:" + saveFilePath, "Info", "E");

            ExportAfter();
         }
         catch (Exception ex) {
            PbFunc.messageBox(GlobalInfo.ErrorText, ex.Message, MessageBoxIcon.Stop);
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