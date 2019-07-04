using BaseGround;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.TableDao;
using System;
using System.Data;
using System.IO;

/// <summary>
/// Winni, 2019/02/20
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30683 臺股期貨除息影響點數查詢
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30683 : FormParent {

      private TPPINTD daoTPPINTD;

      public W30683(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         daoTPPINTD = new TPPINTD();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtStartDate.Focus();

#if DEBUG
         //winni test
         //txtStartDate.DateTimeValue = DateTime.ParseExact("2017/06/01" , "yyyy/MM/dd" , null);
         //txtEndDate.DateTimeValue = DateTime.ParseExact("2017/06/30" , "yyyy/MM/dd" , null);
         //this.Text += "(開啟測試模式),Date=2017/06/01~2017/06/30";
#endif

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         try {

            #region 日期檢核
            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

            int li_mth_seq1, li_mth_seq2 = 0;

            //ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Refresh();

            //期貨日盤
            //string ls_prod_type = "F";
            if (txtFirstMon.Text.Trim() == "") {
               li_mth_seq1 = 99;
            } else {
               li_mth_seq1 = txtFirstMon.Text.Trim().AsInt();
            }

            //if (txtSecondMon.Text.Trim() == "") {
            //   li_mth_seq2 = 99;
            //} else {
            //   li_mth_seq2 = txtSecondMon.Text.Trim().AsInt();
            //}

            //讀取資料
            DataTable dtContent = new DataTable();
            dtContent = daoTPPINTD.d30683(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , li_mth_seq1 , li_mth_seq2 , "Y");
            if (dtContent.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtEndDate.Text , this.Text) , GlobalInfo.ResultText);
               labMsg.Visible = false;
               return ResultStatus.Fail;
            }

            //1.1處理資料型態
            DataTable dt = dtContent.Clone(); //轉型別用的datatable
            dt.Columns["TPPINTD_DATE"].DataType = typeof(string); //將原DataType(datetime)轉為string
            foreach (DataRow row in dtContent.Rows) {
               dt.ImportRow(row);
            }

            for (int i = 0 ; i < dt.Rows.Count ; i++) {
               dt.Rows[i]["TPPINTD_DATE"] = Convert.ToDateTime(dtContent.Rows[i]["TPPINTD_DATE"]).ToString("yyyy/MM/dd HH:mm:ss");
            }

            //存CSV (ps:輸出csv 都用ascii)
            string etfFileName = "30683_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
            etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = true;
            csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dt , etfFileName , csvref);

            labMsg.Visible = false;
            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;

         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
      }
   }
}