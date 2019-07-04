using BaseGround;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/02/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30684 新加坡摩台期基準價查詢
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30684 : FormParent {

      private VPR daoVPR;
      private ResultStatus exportStatus = ResultStatus.Fail;

      #region 一般查詢縮寫
      /// <summary>
      /// 起日(yyyyMMdd)
      /// </summary>
      public string StartDate {
         get {
            return txtStartDate.Text.Replace("/" , "");
         }
      }

      /// <summary>
      /// 訖日(yyyyMMdd)
      /// </summary>
      public string EndDate {
         get {
            return txtEndDate.Text.Replace("/" , "");
         }
      }
      #endregion

      public W30684(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         daoVPR = new VPR();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
         txtStartDate.DateTimeValue = DateTime.ParseExact("2017/12/18" , "yyyy/MM/dd" , null);
         txtEndDate.DateTimeValue = DateTime.ParseExact("2017/12/19" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2017/12/18~2017/12/19";
#endif
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         labMsg.Visible = true;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {

         try {

            #region 輸入&日期檢核
            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

            //0. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //1.撈資料
            DataTable dtContent = daoVPR.ListByMarket(StartDate , EndDate , 'S' , 'S');
            if (dtContent.Rows.Count <= 0) {
               labMsg.Hide();
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartDate.Text + "-" + txtEndDate.Text , this.Text) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //1.1處理資料型態
            DataTable dt = dtContent.Clone(); //轉型別用的datatable
            dt.Columns["VPR_DATA_TIME"].DataType = typeof(string); //將原DataType(datetime)轉為string
            foreach (DataRow row in dtContent.Rows) {
               dt.ImportRow(row);
            }

            for (int i = 0 ; i < dt.Rows.Count ; i++) {
               dt.Rows[i]["VPR_DATA_TIME"] = Convert.ToDateTime(dtContent.Rows[i]["VPR_DATA_TIME"]).ToString("yyyy/MM/dd HH:mm:ss.fff");
            }

            //2.存Txt
            string etfFileName = "30684_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
            ExportOptions txtref = new ExportOptions();
            txtref.HasHeader = true;
            txtref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToText(dt , etfFileName , txtref);

            labMsg.Text = "轉檔成功!";
            exportStatus = ResultStatus.Success;
            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
            labMsg.Text = "轉檔失敗";

         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus ExportAfter(string startTime) {
         if (exportStatus == ResultStatus.Success) {
            MessageDisplay.Info("轉檔完成!" , GlobalInfo.ResultText);
            return ResultStatus.Success;
         } else {
            MessageDisplay.Error("轉檔失敗" , GlobalInfo.ErrorText);
            return ResultStatus.Fail;
         }
      }

   }
}