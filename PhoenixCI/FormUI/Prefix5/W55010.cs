using BaseGround;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;

namespace PhoenixCI.FormUI.Prefix5 {
   //Winni,2019/01/10 調整邏輯
   public partial class W55010 : FormParent {
      private D55010 dao55010;

      public W55010(string programID , string programName) : base(programID , programName) {

         InitializeComponent();
         dao55010 = new D55010();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtFromDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtToDate.DateTimeValue = GlobalInfo.OCF_DATE;
      }
      /// <summary>
      /// 設定此功能哪些按鈕可以按
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      //TODO : 原PB有按下Save即輸出Txt檔的作用，但輸出的Txt檔都只有Header，需再確認
      protected override ResultStatus Save(PokeBall pokeBall) {
         base.Save(pokeBall);

         return ResultStatus.Success;
      }

      /// <summary>
      /// 按下[匯出]按鈕時
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Export() {
         base.Export();

         #region 輸入&日期檢核
         if (string.Compare(txtFromDate.Text , txtToDate.Text) > 0) {
            MessageDisplay.Error("月份起始年月不可小於迄止年月!" , GlobalInfo.ErrorText);
            return ResultStatus.Fail;
         }
         #endregion

         int li_run_times; //單月報表需抓取資料庫的次數(月份數) 可先設定200806-200808
         DateTime ldt_ym;
         int sYear = txtFromDate.Text.SubStr(0 , 4).AsInt(); //起年
         int eYear = txtToDate.Text.SubStr(0 , 4).AsInt(); //迄年
         int sMonth = txtFromDate.Text.SubStr(5 , 2).AsInt(); //起月
         int eMonth = txtToDate.Text.SubStr(5 , 4).AsInt(); //迄月

         bool flag = false;
         //選擇單月
         if (rb_repo.SelectedIndex == 0) {
            //計算共需幾個月份
            if (txtFromDate.Text != txtToDate.Text) {
               //迄月大於起月
               if (eMonth >= sMonth) {
                  li_run_times = eMonth - sMonth;
               } else {
                  li_run_times = (sMonth - eMonth) * -1;
               }
               //迄年大於起年
               if (eYear > sYear) {
                  li_run_times = li_run_times + ((eYear - sYear) * 12);
               }
               //迄年小於或等於起年
               li_run_times = li_run_times + 1;

            } else {
               li_run_times = 1; // txtFromDate.Text == txtToDate.Text (同年同月算一個月份)
            }
            ldt_ym = Convert.ToDateTime(sYear + "-" + sMonth + "-01").AddDays(-1); //2008-05-31

            for (int i = 0 ; i < li_run_times ; i++) {
               //先將ldt_ym回到1號(2008-05-01)，再加32天讓它跨月(2008-06-02)
               ldt_ym = ldt_ym.AddDays(1 - ldt_ym.Day);
               ldt_ym = ldt_ym.AddDays(32);
               //資料月份
               string dataMonth = ldt_ym.ToString("yyyyMM");
               //em_data_month.text = string(ldt_ym , 'yyyy/mm');

               //跑單月填表
               flag = SingleMonthReport(dataMonth, dataMonth);
               if (!flag)
                  return ResultStatus.Fail;
            }

         } else {
            //跑多月填表
            flag = MultiMonthReport();
            if (!flag)
               return ResultStatus.Fail;
         }

         //ManipulateExcel(excelDestinationPath);

         return ResultStatus.Success;
      }

      private bool SingleMonthReport(string txtFrom , string txtTo) {

         int li_ole_row_tol, li_datacount;
         int ii_ole_row = 5;
         string excelDestinationPath;

         #region Excel

         //讀取資料
         DataTable dtContent = dao55010.ListDataSingleMonth(txtFrom , txtTo);
         if (dtContent.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtFromDate.Text , this.Text) , GlobalInfo.ResultText);
            return false;
         }

         excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID); //單月路徑

         //開啟檔案     
         Workbook workbook = new Workbook();
         workbook.LoadDocument(excelDestinationPath);
         Worksheet worksheet = workbook.Worksheets[0];
         //var t = worksheet.Cells["A1"].DisplayText;
         li_datacount = Int32.Parse((worksheet.Cells[0 , 0].Value).ToString());
         if (li_datacount == 0) {
            li_datacount = dtContent.Rows.Count;
         }
         li_ole_row_tol = ii_ole_row + li_datacount;
         DateTime today = DateTime.Now;
         worksheet.Cells[3 , 0].Value += today.ToString("yyyy/MM/dd hh:mm:ss");
         worksheet.Cells[3 , 4].Value += txtTo;

         for (int x = 0 ; x < dtContent.Rows.Count ; x++) {

            ii_ole_row = ii_ole_row + 1;
            worksheet.Cells[ii_ole_row , 0].Value = dtContent.Rows[x]["feetrd_fcm_no"].ToString();
            worksheet.Cells[ii_ole_row , 1].Value = dtContent.Rows[x]["brk_abbr_name"].ToString();
            worksheet.Cells[ii_ole_row , 2].Value = dtContent.Rows[x]["feetrd_acc_no"].ToString();
            worksheet.Cells[ii_ole_row , 3].Value = dtContent.Rows[x]["feetrd_kind_id"].ToString();
            worksheet.Cells[ii_ole_row , 4].Value = dtContent.Rows[x]["feetrd_disc_qnty"].AsInt();
            worksheet.Cells[ii_ole_row , 5].Value = dtContent.Rows[x]["disc_rate"].AsDouble();
            worksheet.Cells[ii_ole_row , 6].Value = dtContent.Rows[x]["disc_amt"].AsDecimal();
            worksheet.Cells[ii_ole_row , 7].Value = dtContent.Rows[x]["feetrd_disc_qnty_ah"].AsInt();
            worksheet.Cells[ii_ole_row , 8].Value = dtContent.Rows[x]["disc_rate_ah"].AsDouble();
            worksheet.Cells[ii_ole_row , 9].Value = dtContent.Rows[x]["disc_amt_ah"].AsDecimal();
            worksheet.Cells[ii_ole_row , 10].Value = dtContent.Rows[x]["feetrd_disc_qnty_sum"].AsInt();
            worksheet.Cells[ii_ole_row , 11].Value = dtContent.Rows[x]["disc_amt_sum"].AsDecimal();
         }

         //刪除空白列
         if (ii_ole_row < li_ole_row_tol) {
            worksheet.Rows.Remove(ii_ole_row + 1 , li_ole_row_tol - ii_ole_row);
         }
         //worksheet.Range["A1"].Select();

         //儲存及關閉檔案
         workbook.SaveDocument(excelDestinationPath);

         return true;

         #endregion
      }

      private bool MultiMonthReport() {
         int li_ole_row_tol, li_datacount;
         int ii_ole_row = 1;
         string excelDestinationPath;

         #region Excel

         //讀取資料
         DataTable dtContent = dao55010.ListDataMultiMonth(txtFromDate.FormatValue , txtToDate.FormatValue);
         if (dtContent.Rows.Count <= 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtFromDate.Text , this.Text) , GlobalInfo.ResultText);
            return false;
         }

         excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID + "_2");//多月路徑

         //開啟檔案     
         Workbook workbook = new Workbook();
         workbook.LoadDocument(excelDestinationPath);
         Worksheet worksheet = workbook.Worksheets[0];

         worksheet.Name = txtFromDate.FormatValue + "-" + txtToDate.FormatValue;
         li_datacount = Int32.Parse((worksheet.Cells[0 , 0].Value).ToString());

         if (li_datacount == 0) {
            li_datacount = dtContent.Rows.Count;
         }
         li_ole_row_tol = ii_ole_row + li_datacount;

         for (int x = 0 ; x < dtContent.Rows.Count ; x++) {

            ii_ole_row = ii_ole_row + 1;
            worksheet.Cells[ii_ole_row , 0].Value = dtContent.Rows[x]["feetrd_ym"].ToString();
            worksheet.Cells[ii_ole_row , 1].Value = dtContent.Rows[x]["feetrd_fcm_no"].ToString();
            worksheet.Cells[ii_ole_row , 2].Value = dtContent.Rows[x]["feetrd_acc_no"].ToString();
            worksheet.Cells[ii_ole_row , 3].Value = dtContent.Rows[x]["brk_abbr_name"].ToString();
            worksheet.Cells[ii_ole_row , 4].Value = dtContent.Rows[x]["feetrd_kind_id"].ToString();
            worksheet.Cells[ii_ole_row , 5].Value = dtContent.Rows[x]["feetrd_disc_qnty"].AsDecimal();
            worksheet.Cells[ii_ole_row , 6].Value = dtContent.Rows[x]["disc_rate"].AsDecimal();
            worksheet.Cells[ii_ole_row , 7].Value = dtContent.Rows[x]["disc_amt"].AsDecimal();
            worksheet.Cells[ii_ole_row , 8].Value = dtContent.Rows[x]["feetrd_disc_qnty_ah"].AsInt();
            worksheet.Cells[ii_ole_row , 9].Value = dtContent.Rows[x]["disc_rate_ah"].AsDouble();
            worksheet.Cells[ii_ole_row , 10].Value = dtContent.Rows[x]["disc_amt_ah"].AsDecimal();
            worksheet.Cells[ii_ole_row , 11].Value = dtContent.Rows[x]["feetrd_disc_qnty_sum"].AsInt();
            worksheet.Cells[ii_ole_row , 12].Value = dtContent.Rows[x]["disc_amt_sum"].AsDecimal();
         }

         //刪除空白列
         if (ii_ole_row < li_ole_row_tol) {
            worksheet.Rows.Remove(ii_ole_row + 1 , li_ole_row_tol - ii_ole_row);
         }
         //worksheet.Range["A1"].Select();

         //儲存及關閉檔案
         workbook.SaveDocument(excelDestinationPath);

         return true;

         #endregion
      }

   }

}
