using BaseGround;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using System.IO;

/// <summary>
/// Winni, 2019/3/19
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 臺股期貨歷史及瞬時波動率查詢
   /// </summary>
   public partial class W30682 : FormParent {

      #region 一般交易查詢條件縮寫
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string StartDate {
         get {
            return txtStartDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string EndDate {
         get {
            return txtEndDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }
      #endregion

      public W30682(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtStartDate.Focus();

#if DEBUG
         txtStartDate.DateTimeValue = DateTime.ParseExact("2018/10/01" , "yyyy/MM/dd" , null);
         txtEndDate.DateTimeValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2018/10/01~2018/10/11";
#endif

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         try {

            #region 輸入&日期檢核
            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

            string rptName, type, chk = "N";

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Refresh();

            //2. 判斷 統計or明細
            if (gbReport.EditValue.AsString() == "rbStatistics") {
               wf_30681();
            } else {
               wf_30682();
            }

            #region wf_30681
            void wf_30681() {
               try {
                  if (gbType.EditValue.AsString() == "rbHistory") {
                     rptName = "歷史";
                     type = "H";
                  } else {
                     rptName = "瞬時";
                     type = "I";
                  }

                  rptName += "波動率統計表";
                  labMsg.Text = _ProgramID + "-" + rptName + " 轉檔中...";

                  //讀取資料
                  DataTable dtS = new D30682().ListStatisticsData(StartDate , EndDate , type , "Y");
                  if (dtS.Rows.Count == 0) {
                     MessageDisplay.Info(string.Format("{0}-{1},{2}-{3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName) , GlobalInfo.ResultText);
                     WriteLog(string.Format("{0}-{1},{2}-{3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
                     return;
                  }//if (dtS.Rows.Count == 0)

                  //存CSV (ps:輸出csv 都用ascii)
                  string etfFileName = string.Format("{0}_{1}-{2}_w{3}.csv" , rptName , StartDate , EndDate , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
                  etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
                  ExportOptions csvref = new ExportOptions();
                  csvref.HasHeader = true;
                  csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
                  Common.Helper.ExportHelper.ToCsv(dtS , etfFileName , csvref);
                  chk = "Y";
               } catch (Exception ex) {
                  WriteLog(ex);
               }
            }
            #endregion

            #region wf_30682
            void wf_30682() {
               try {
                  if (gbType.EditValue.AsString() == "rbHistory") {
                     rptName = "歷史";
                     type = "H";
                  } else {
                     rptName = "瞬時";
                     type = "I";
                  }

                  rptName += "波動率明細表";
                  labMsg.Text = _ProgramID + "-" + rptName + " 轉檔中...";

                  //讀取資料
                  DataTable dtS = new D30682().ListDetailData(StartDate , EndDate , type , "Y");
                  if (dtS.Rows.Count == 0) {
                     MessageDisplay.Info(string.Format("{0}-{1},{2}-{3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName) , GlobalInfo.ResultText);
                     WriteLog(string.Format("{0}-{1},{2}-{3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
                     return;
                  }//if (dtS.Rows.Count == 0)

                  //處理資料型態(轉換時間格式)
                  DataTable dt = dtS.Clone(); //轉型別用的datatable
                  dt.Columns["VOLD_DATA_TIME"].DataType = typeof(string); //將原DataType(datetime)轉為string
                  foreach (DataRow row in dtS.Rows) {
                     dt.ImportRow(row);
                  }

                  for (int i = 0 ; i < dt.Rows.Count ; i++) {
                     dt.Rows[i]["VOLD_DATA_TIME"] = Convert.ToDateTime(dtS.Rows[i]["VOLD_DATA_TIME"]).ToString("yyyy/MM/dd HH:mm:ss");
                  }

                  //存CSV (ps:輸出csv 都用ascii)
                  string etfFileName = string.Format("{0}_{1}-{2}_w{3}.csv" , rptName , StartDate , EndDate , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
                  etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
                  ExportOptions csvref = new ExportOptions();
                  csvref.HasHeader = true;
                  csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
                  Common.Helper.ExportHelper.ToCsv(dt , etfFileName , csvref);
                  chk = "Y";
               } catch (Exception ex) {
                  WriteLog(ex);
               }
            }
            #endregion
            if (chk == "Y") {
               return ResultStatus.Success;
            } else {
               return ResultStatus.Fail;
            }
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
         return ResultStatus.Fail;
      }
   }
}