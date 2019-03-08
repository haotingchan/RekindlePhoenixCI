using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using PhoenixCI.Shared;
using Common;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix7;
/// <summary>
/// john,20190211,臺指選擇權期貨商交易量表
/// </summary>
namespace PhoenixCI.FormUI.Prefix7
{
   /// <summary>
   /// 臺指選擇權期貨商交易量表
   /// </summary>
   public partial class W70040 : FormParent
   {
      private B700xxFunc B700xxFunc;
      string logText, saveFilePath, startYMD, endYMD, sumType, isProdType, isKindId2, isParamKey;
      public W70040(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         B700xxFunc = new B700xxFunc();
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

         emStartDate1.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/") + "01";
         emEndDate1.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");

         emStartMth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/") + "01";
         emEndMth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");

         emStartYear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");
         emEndYear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");
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
         DateTime ldStart, ldEnd;
         string lsType = "";
         switch (rgDate.EditValue) {
            case "rb_day":
               //週
               if (!emStartDate.IsDate(emStartDate.Text, CheckDate.Start)
                  || !emEndDate.IsDate(emEndDate.Text, CheckDate.End)) {
                  return false;
               }
               if (string.Compare(emStartDate.Text, emEndDate.Text) > 0) {
                  MessageDisplay.Error(GlobalInfo.ErrorText, CheckDate.Datedif);
                  return false;
               }
               ldStart = Convert.ToDateTime(emStartDate.Text);
               ldEnd = Convert.ToDateTime(emEndDate.Text);

               startYMD = emStartDate.Text.Replace("/", "").SubStr(0, 8);
               endYMD = emEndDate.Text.Replace("/", "").SubStr(0, 8);
               lsType = "Day";
               sumType = "D";

               logText = ldStart.ToString("yyyy.MM.dd") + "至" + ldEnd.ToString("yyyy.MM.dd") + " 交易量";
               break;
            case "rb_week":
               //週
               if (!emStartDate1.IsDate(emStartDate1.Text, CheckDate.Start)
                  || !emEndDate1.IsDate(emEndDate1.Text, CheckDate.End)) {
                  return false;
               }
               if (string.Compare(emStartDate1.Text, emEndDate1.Text) > 0) {
                  MessageDisplay.Error(GlobalInfo.ErrorText, CheckDate.Datedif);
                  return false;
               }
               ldStart = Convert.ToDateTime(emStartDate1.Text);
               ldEnd = Convert.ToDateTime(emEndDate1.Text);

               startYMD = emStartDate1.Text.Replace("/", "").SubStr(0, 8);
               endYMD = emEndDate1.Text.Replace("/", "").SubStr(0, 8);
               lsType = "Week";
               sumType = "D";

               logText = ldStart.ToString("yyyy.MM.dd") + "至" + ldEnd.ToString("yyyy.MM.dd") + " 交易量";
               break;
            case "rb_month":
               //月
               string emSmth = emStartMth.Text + "/01";
               string emEmth = emEndMth.Text + "/01";
               if (!emStartMth.IsDate(emSmth, CheckDate.Start)
                  || !emEndMth.IsDate(emEmth, CheckDate.End)) {
                  return false;
               }
               ldStart = Convert.ToDateTime(emSmth);
               ldEnd = PbFunc.relativedate(Convert.ToDateTime(emEmth), 31);
               if (ldEnd.Month != PbFunc.Right(emStartMth.Text, 2).AsInt()) {
                  ldEnd = PbFunc.relativedate(ldEnd, -ldEnd.Day);
               }
               startYMD = emStartMth.Text.Replace("/", "").SubStr(0, 6);
               endYMD = emEndMth.Text.Replace("/", "").SubStr(0, 6);
               lsType = "Month";
               sumType = "M";

               logText = startYMD + "至" + endYMD + " 交易量";
               break;
            case "rb_year":
               //年
               startYMD = emStartYear.Text;
               endYMD = emEndYear.Text;
               lsType = "Year";
               sumType = "Y";
               logText = startYMD + "至" + endYMD + " 交易量";
               break;
            default:
               break;
         }

         saveFilePath = _ProgramID + "_" + lsType + "(" + startYMD + "-" + endYMD + ")";
         //商品別
         switch (rgPeriod.EditValue) {
            case "rb_txw":
               isKindId2 = "TXW%";
               saveFilePath = saveFilePath + "W";
               break;
            case "rb_txo":
               isKindId2 = "TXO%";
               saveFilePath = saveFilePath + "S";
               break;
            default:
               isKindId2 = "%";
               break;
         }
         isParamKey = "TXO";
         isProdType = "O";
         /*點選儲存檔案之目錄*/
         saveFilePath = ReportExportFunc.wf_GetFileSaveName(saveFilePath + ".csv");
         if (string.IsNullOrEmpty(saveFilePath)) {
            return false;
         }
         /*******************
         Messagebox
         *******************/
         st_msg_txt.Visible = true;
         st_msg_txt.Text = "開始轉檔...";
         st_msg_txt.Text = logText + " 轉檔中...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void ExportAfter()
      {
         st_msg_txt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         st_msg_txt.Visible = false;
      }

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            if (rgDate.EditValue.Equals("rb_week")) {
               B700xxFunc.F70010WeekW(saveFilePath, startYMD, endYMD, sumType, isKindId2, isParamKey, isProdType);
            }//if (rgDate.EditValue.Equals("rb_week"))
            else {
               B700xxFunc.F70010YmdW(saveFilePath, startYMD, endYMD, sumType, isKindId2, isParamKey, isProdType);
            }
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

      private void rgDate_SelectedIndexChanged(object sender, EventArgs e)
      {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;
         switch (rb.EditValue.ToString()) {
            case "rb_day"://日期
               stDate.Visible = true;//日期
               stDate.FocusHelper.FocusFirst();
               stWeek.Visible = false;//週期
               stMonth.Visible = false;//月份
               stYear.Visible = false;//年度
               break;
            case "rb_week"://週期
               stDate.Visible = false;//日期
               stWeek.Visible = true;//週期
               stWeek.FocusHelper.FocusFirst();
               stMonth.Visible = false;//月份
               stYear.Visible = false;//年度
               break;
            case "rb_month"://月份
               stDate.Visible = false;//日期
               stWeek.Visible = false;//週期
               stMonth.Visible = true;//月份
               stMonth.FocusHelper.FocusFirst();
               stYear.Visible = false;//年度
               break;
            case "rb_year"://年度
               stDate.Visible = false;//日期
               stWeek.Visible = false;//週期
               stMonth.Visible = false;//月份
               stYear.Visible = true;//年度
               stYear.FocusHelper.FocusFirst();
               break;
            default:
               break;
         }
      }
   }
}