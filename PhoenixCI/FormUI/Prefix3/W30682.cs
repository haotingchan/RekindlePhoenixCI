using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
         txtStartDate.Text = GlobalInfo.OCF_DATE.AsString("yyyy/MM/dd");
         txtEndDate.Text = GlobalInfo.OCF_DATE.AsString("yyyy/MM/dd");

#if DEBUG
         //winni test
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
            if (!txtStartDate.IsDate(txtStartDate.Text , CheckDate.Start)
                  || !txtEndDate.IsDate(txtEndDate.Text , CheckDate.End)) {
               return ResultStatus.Fail; ;
            }

            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(GlobalInfo.ErrorText , CheckDate.Datedif);
               return ResultStatus.Fail; ;
            }
            #endregion

            string rptName, type;

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料轉出中........";
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
                  if (dtS.Rows.Count < 0) {
                     MessageDisplay.Info(string.Format("{0}-{1},{2}-{3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
                     WriteLog(string.Format("{0}-{1},{2}-{3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
                  }//if (dtS.Rows.Count < 0)

                  //存CSV (ps:輸出csv 都用ascii)
                  string etfFileName = string.Format("{0}_{1}-{2}_w{3}.csv" , rptName , StartDate , EndDate , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
                  //string etfFileName = rptName + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
                  etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
                  ExportOptions csvref = new ExportOptions();
                  csvref.HasHeader = true;
                  csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
                  Common.Helper.ExportHelper.ToCsv(dtS , etfFileName , csvref);
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
                  if (dtS.Rows.Count < 0) {
                     MessageDisplay.Info(string.Format("{0}-{1},{2}-{3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
                     WriteLog(string.Format("{0}-{1},{2}-{3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
                  }//if (dtS.Rows.Count < 0)

                  //存CSV (ps:輸出csv 都用ascii)
                  string etfFileName = string.Format("{0}_{1}-{2}_w{3}.csv" , rptName , StartDate , EndDate , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
                  //string etfFileName = rptName + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
                  etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
                  ExportOptions csvref = new ExportOptions();
                  csvref.HasHeader = true;
                  csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
                  Common.Helper.ExportHelper.ToCsv(dtS , etfFileName , csvref);
               } catch (Exception ex) {
                  WriteLog(ex);
               }
            }
            #endregion

            return ResultStatus.Success;
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