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
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/3/28
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30400 股票期貨價量統計表
   /// </summary>
   public partial class W30400 : FormParent {

      protected enum SheetNo {
         sheet1 = 0,
         sheet2 = 1,
         sheet3 = 2,
         sheet4 = 3,
         sheet5 = 4,
         sheet6 = 5,
         sheet7 = 6,
         sheet8 = 7
      }

      public W30400(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtMon.DateTimeValue = GlobalInfo.OCF_DATE;
         txtMon.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");

#if DEBUG
         //winni test
         //txtMon.DateTimeValue = DateTime.ParseExact("2018/11" , "yyyy/MM" , null);
         //this.Text += "(開啟測試模式),Date=2018/11";
#endif

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected void ExportAfter() {
         labMsg.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         labMsg.Visible = false;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {
         try {

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);

            //3. open xls
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //4. write data
            //bool res1 = false, res2 = false, res3 = false;
            int row = 2;
            wf_30401(workbook , SheetNo.sheet1 , row);
            //wf_30402();
            //row = 1;
            //wf_30403();
            //if (textKindId.Text != "%") {
            //   row = 2;
            //   wf_30404();
            //}
            //row = 3;
            //wf_30405();
            //wf_30406();

            //if(!res1 && !res2 && !res3) {
            //   //關閉檔案
            //}

            //5. save 
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

            //測試時直接開檔
            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

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

      /// <summary>
      /// wf_30401 (sheet1 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 2 </param>
      /// <returns></returns>
      protected bool wf_30401(Workbook workbook , SheetNo sheetNo , int row) {

         string rptName = "股票期貨成交量及未平倉量變化表"; //報表標題名稱
         labMsg.Text = "30401－" + rptName + " 轉檔中...";

         try {

            //1.1 前月倒數1天交易日(?)
            DateTime sDate = GlobalInfo.OCF_DATE.AddDays(- GlobalInfo.OCF_DATE.Day + 1); //月份第1天
            DateTime eDate = GlobalInfo.OCF_DATE.AddMonths(1).AddDays(-GlobalInfo.OCF_DATE.AddMonths(1).Day); //月份最後1天
            //DateTime aa = DateTime.ParseExact(txtMon.DateTimeValue.ToString("yyyy/MM/01") , "yyyy/MM/dd" , null); //看起來比較像是選取月份第1天
            //DateTime bb = DateTime.ParseExact(txtMon.DateTimeValue.ToString("yyyy/MM/31") , "yyyy/MM/dd" , null);
            string strSDate = sDate.ToString("yyyyMMdd");
            string strEDate = eDate.ToString("yyyyMMdd");

            //1.2 抓當月最後交易日
            string lastTradeDate = new AI2().GetLastTradeDate("D" , "O" , "S" , sDate , eDate);

            //2. 讀取資料
            DataTable dt30401 = new D30400().Get30401Data(strSDate , strEDate , "F");
            if (dt30401.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},30401 - {2},無任何資料!" , sDate , eDate , rptName));
            } //if (dt30401.Rows.Count <= 0)

            //3. 切換Sheet
            Worksheet ws1 = workbook.Worksheets[(int)sheetNo];

            //4. 處理資料
            int rowNum = 1; //外面帶入2 (C# - 1)
            int rowTotal = rowNum + 33;
            string ymd = "";

            foreach (DataRow dr in dt30401.Rows) {
               string ai2Ymd = dr["ai2_ymd"].AsString();
               decimal mQnty = dr["ai2_m_qnty"].AsDecimal();
               decimal sumMmkQnty = dr["ai2_mmk_qnty"].AsDecimal();
               decimal sumOi = dr["ai2_oi"].AsDecimal();

               if (ymd != ai2Ymd) {
                  ymd = ai2Ymd;
                  rowNum++;
                  ws1.Cells[rowNum , 0].Value = string.Format("{0}/{1}" , ymd.SubStr(4 , 2) , ymd.SubStr(6 , 2));
               }
               ws1.Cells[rowNum , 1].Value = mQnty;
               ws1.Cells[rowNum , 3].Value = sumMmkQnty;
               ws1.Cells[rowNum , 5].Value = sumOi;
            }//foreach (DataRow dr in dt30401.Rows)

            //5. 刪除空白列
            //if (dt30401.Rows.Count < rowTotal) {
            //   ws1.Rows.Remove(row , rowTotal - row);
            //}

            ws1.Range["A1"].Select();
            ws1.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

   }
}