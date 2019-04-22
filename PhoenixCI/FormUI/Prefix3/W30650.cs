using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/01/24
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30650 專、兼營期貨商當沖交易量統計
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30650 : FormParent {

      private D30650 dao30650;

      public W30650(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30650 = new D30650();
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         //起始月份皆設為當年1月
         txtStartMonth.DateTimeValue = DateTime.ParseExact(GlobalInfo.OCF_DATE.ToString("yyyy/01") , "yyyy/MM" , null);
         txtEndMonth.DateTimeValue = DateTime.ParseExact(GlobalInfo.OCF_DATE.ToString("yyyy/MM") , "yyyy/MM" , null);

#if DEBUG
         txtStartMonth.DateTimeValue = DateTime.ParseExact("2012/07" , "yyyy/MM" , null);
         txtEndMonth.DateTimeValue = DateTime.ParseExact("2012/10" , "yyyy/MM" , null);
         this.Text += "(開啟測試模式)";
#endif
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
            //0. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //1. copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            Worksheet worksheet = workbook.Worksheets[0];
            Range range = worksheet.Range["A7:Q7"];
            range.Alignment.WrapText = true;

            //2.填資料
            bool result = false;
            result = wf_Export(workbook , worksheet , txtStartMonth.Text.Replace("/" , "") , txtEndMonth.Text.Replace("/" , ""));

            if (!result) {
               try {
                  workbook = null;
                  System.IO.File.Delete(excelDestinationPath);
               } catch (Exception) {
                  //
               }
               return ResultStatus.Fail;
            }

            //存檔
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }

         return ResultStatus.Fail;
      }

      private bool wf_Export(Workbook workbook , Worksheet worksheet , string as_symd , string as_eymd) {

         try {
            string rptName = "專、兼營期貨商當沖交易量統計";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , _ProgramID , rptName));


            string startDate = txtStartMonth.DateTimeValue.ToString("yyyyMM01"); //取起始月第一天
            string endDate = DateTime.ParseExact(txtEndMonth.DateTimeValue.ToString("yyyy/MM/01") , "yyyy/MM/dd" , null).AddMonths(1).AddDays(-1).ToString("yyyyMMdd"); //取結束月最後一天

            DataTable dtContent = dao30650.GetData(startDate , endDate);
            if (dtContent.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}～{1},{2}－{3},無任何資料!" , txtStartMonth.Text , txtEndMonth.Text , _ProgramID , rptName));
               return false;
            }

            DataTable dtTmp = dao30650.GetTmpData(startDate , endDate);
            if (dtTmp.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}～{1},{2}－{3}(合計),無任何資料!" , txtStartMonth.Text , txtEndMonth.Text , _ProgramID , rptName));
               return false;
            }

            int header = 7;
            worksheet.Cells[header - 2 , 0].Value = "查詢條件：" + txtStartMonth.DateTimeValue.ToString("yyyyMM") + "–" +
                                                                              txtEndMonth.DateTimeValue.ToString("yyyyMM");
            worksheet.Cells[header - 1 , 0].Value = "期貨商" + (char)13 + (char)10 + "代號";
            worksheet.Cells[header - 1 , 1].Value = "期貨商名稱";

            int colNum = 0, found = 0;
            string ymd = "";

            //先填上個月份值
            for (int i = 0 ; i < dtContent.Rows.Count ; i++) {
               DataRow dr = dtContent.Rows[i];

               if (ymd != dr["AM10_YM"].AsString()) {
                  ymd = dr["AM10_YM"].AsString(); //yyyyMM
                  colNum += 3;
                  worksheet.Cells[header - 1 , colNum - 1].Value = ymd.SubStr(0 , 4) + "/" + ymd.SubStr(4 , 2) + Environment.NewLine
                                                                        + "合計" + (char)13 + (char)10 + "交易量";
                  worksheet.Cells[header - 1 , colNum].Value = "當沖" + (char)13 + (char)10 + "交易量";
                  worksheet.Cells[header - 1 , colNum + 1].Value = "當沖" + (char)13 + (char)10 + "比率" + (char)13 + (char)10 + "%";

                  decimal cpTotQnty = dr["cp_tot_qnty"].AsDecimal();
                  decimal cpTotDtQnty = dr["cp_tot_dt_qnty"].AsDecimal();
                  decimal cpRate = dr["cp_rate"].AsDecimal();

                  worksheet.Cells[(dtTmp.Rows.Count + 1 + header) - 2 , colNum - 1].Value = cpTotQnty;
                  worksheet.Cells[(dtTmp.Rows.Count + 1 + header) - 2 , colNum].Value = cpTotDtQnty;
                  worksheet.Cells[(dtTmp.Rows.Count + 1 + header) - 2 , colNum + 1].Value = cpRate;
               }//if (ymd != dr["AM10_YM"].AsString())

               if (dtTmp.Select("abrk_no='" + dtContent.Rows[i]["ABRK_NO"] + "'").Length != 0) {
                  found = dtTmp.Rows.IndexOf(dtTmp.Select("abrk_no='" + dtContent.Rows[i]["ABRK_NO"] + "'")[0]) + 1;
               }

               decimal qnty = dr["am10_qnty"].AsDecimal();
               decimal dtQnty = dr["am10_dt_qnty"].AsDecimal();
               decimal rate = dr["am10_rate"].AsDecimal();
               worksheet.Cells[(found + header - 1) , colNum - 1].Value = qnty;
               worksheet.Cells[(found + header - 1) , colNum].Value = dtQnty;
               worksheet.Cells[(found + header - 1) , colNum + 1].Value = rate;
            }

            //填上各期貨商名稱&合計
            colNum += 3;
            worksheet.Cells[header - 1 , colNum - 1].Value = txtStartMonth.Text + '-' + txtEndMonth.Text + Environment.NewLine
                                                                                  + "合計" + (char)13 + (char)10 + "交易量";
            worksheet.Cells[header - 1 , colNum].Value = "當沖" + (char)13 + (char)10 + "交易量";
            worksheet.Cells[header - 1 , colNum + 1].Value = "當沖" + (char)13 + (char)10 + "比率" + (char)13 + (char)10 + "%";
            for (int i = 0 ; i < dtTmp.Rows.Count - 1 ; i++) {
               string abrkNo = dtTmp.Rows[i]["abrk_no"].AsString();
               string abrkAbbrName = dtTmp.Rows[i]["abrk_abbr_name"].AsString();
               decimal qnty = dtTmp.Rows[i]["am10_qnty"].AsDecimal();
               decimal dtQnty = dtTmp.Rows[i]["am10_dt_qnty"].AsDecimal();
               decimal rate = dtTmp.Rows[i]["am10_rate"].AsDecimal();

               worksheet.Cells[i + header , 0].Value = abrkNo;
               worksheet.Cells[i + header , 1].Value = abrkAbbrName;
               worksheet.Cells[i + header , colNum - 1].Value = qnty;
               worksheet.Cells[i + header , colNum].Value = dtQnty;
               worksheet.Cells[i + header , colNum + 1].Value = rate;
            }

            decimal totQnty = dtTmp.Rows[dtTmp.Rows.Count - 1]["cp_tot_qnty"].AsDecimal();
            decimal totDtQnty = dtTmp.Rows[dtTmp.Rows.Count - 1]["cp_tot_dt_qnty"].AsDecimal();
            decimal rate2 = dtTmp.Rows[dtTmp.Rows.Count - 1]["cp_rate"].AsDecimal();

            worksheet.Cells[dtTmp.Rows.Count + header - 1 , 1].Value = "合計";
            worksheet.Cells[dtTmp.Rows.Count + header - 1 , colNum - 1].Value = totQnty;
            worksheet.Cells[dtTmp.Rows.Count + header - 1 , colNum].Value = totDtQnty;
            worksheet.Cells[dtTmp.Rows.Count + header - 1 , colNum + 1].Value = rate2;

            worksheet.Range["A1"].Select();
            worksheet.ScrollTo(0 , 0);

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }

      }


   }
}