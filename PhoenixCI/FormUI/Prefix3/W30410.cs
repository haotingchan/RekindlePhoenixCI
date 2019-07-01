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
/// Winni, 2019/4/24
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 股票期貨各標的統計表
   /// </summary>
   public partial class W30410 : FormParent {

      protected enum SheetNo {
         tradeSum = 0,
         tradeDetail = 1,
         oint = 2
      }

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

      public W30410(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = DateTime.ParseExact(GlobalInfo.OCF_DATE.ToString("yyyy/MM/01") , "yyyy/MM/dd" , null);
         txtEndDate.DateTimeValue = DateTime.ParseExact(GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd") , "yyyy/MM/dd" , null);

#if DEBUG
         txtStartDate.DateTimeValue = DateTime.ParseExact("2019/02/01" , "yyyy/MM/dd" , null);
         txtEndDate.DateTimeValue = DateTime.ParseExact("2019/02/27" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2019/02/01~2019/02/27";
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
            string lsRtn = PbFunc.f_get_jsw(_ProgramID , "E" , txtEndDate.Text);
            DialogResult liRtn;

            if (lsRtn != "Y") {
               liRtn = MessageDisplay.Choose(String.Format("{0} 統計資料未轉入完畢,是否要繼續?" , txtEndDate.Text));
               if (liRtn == DialogResult.No) {
                  labMsg.Visible = false;
                  Cursor.Current = Cursors.Arrow;
                  return ResultStatus.Fail;
               }//if (liRtn == DialogResult.Yes)
            }//if (lsRtn != "Y")

            //if (!txtStartDate.IsDate(txtStartDate.Text , CheckDate.Start)
            //      || !txtEndDate.IsDate(txtEndDate.Text , CheckDate.End)) {
            //   return ResultStatus.Fail; ;
            //}

            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
               return ResultStatus.Fail; ;
            }
            #endregion

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);

            //3. open xls
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //4. write data
            int row;
            bool res1 = false, res2 = false, res3 = false;
            row = 3;
            res1 = wf_30410(workbook , SheetNo.tradeSum , row);

            row = 4; //PB這邊帶1，但進去後帶回4
            res2 = wf_30411(workbook , SheetNo.tradeDetail , row);

            row = 4; //PB這邊帶1，但進去後帶回4
            res3 = wf_30412(workbook , SheetNo.oint , row);

            if (!res1 && !res2 && !res3) {
               File.Delete(excelDestinationPath);
               return ResultStatus.Fail;
            }

            //5. save 
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

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

      /// <summary>
      /// wf_30410 (sheet1 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 3 </param>
      /// <returns></returns>
      protected bool wf_30410(Workbook workbook , SheetNo sheetNo , int row) {

         string rptName = "股票期貨各標的交易概況統計表"; //報表標題名稱
         labMsg.Text = _ProgramID + "－" + rptName + " 轉檔中...";

         try {
            //1. 取得資料最大日期, 抓取OI用
            string maxDate = new D30410().GetMaxDate(StartDate , EndDate);
            if (string.IsNullOrEmpty(maxDate)) {
               MessageDisplay.Info(string.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName) , "處理結果");
               return false;
            }//if (string.IsNullOrEmpty(maxDate))

            //2. 讀取資料
            string eDate = maxDate;
            DataTable dt30410 = new D30410().ListData(StartDate , eDate);
            if (dt30410.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , eDate , _ProgramID , rptName) , "處理結果");
            } //if (dt.Rows.Count <= 0 )

            //3. 切換Sheet
            Worksheet ws = workbook.Worksheets[(int)sheetNo];
            ws.Cells[1 , 0].Value = txtStartDate.Text;
            ws.Cells[1 , 1].Value = txtEndDate.Text;

            int rowTotal = new RPT().DataByRptId("30410" , "30410").AsInt(); //將rowTotal轉為int使用
            rowTotal += row;

            string KindId = "19000101";
            foreach (DataRow dr in dt30410.Rows) {
               row++;
               string ai2KindId = dr["ai2_kind_id"].AsString();
               string apdkName = dr["apdk_name"].AsString();
               int exRow = row - 1;

               if (KindId != ai2KindId) {
                  KindId = ai2KindId;
                  ws.Cells[exRow , 0].Value = KindId;
                  ws.Cells[exRow , 1].Value = apdkName;
               }
               ws.Cells[exRow , 2].Value = dr["ai2_m_qnty"].AsDecimal();
               ws.Cells[exRow , 3].Value = dr["ai2_oi"].AsDecimal();
               ws.Cells[exRow , 4].Value = dr["am10_cnt"].AsDecimal();
               ws.Cells[exRow , 6].Value = dr["am9_acc_cnt"].AsDecimal();
               ws.Cells[exRow , 8].Value = dr["ab4_id_cnt"].AsDecimal();
            }

            //4. 刪除空白列
            if (dt30410.Rows.Count < rowTotal) {
               ws.Rows.Remove(row , rowTotal - row);
            }

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }

      /// <summary>
      /// wf_30411 (sheet2 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeDetail</param>
      /// <param name="row"> 4 </param>
      /// <returns></returns>
      protected bool wf_30411(Workbook workbook , SheetNo sheetNo , int row) {

         string rptName = "股票期貨各標的交易量分佈明細統計表"; //報表標題名稱
         string rptId = "30411";
         ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

         try {

            //1. 取得資料最大日期, 抓取OI用 (在wf_30410取得)
            string maxDate = new D30410().GetMaxDate(StartDate , EndDate);
            if (string.IsNullOrEmpty(maxDate)) {
               MessageDisplay.Info(string.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , rptId , rptName) , "處理結果");
               return false;
            }//if (string.IsNullOrEmpty(maxDate))
            DateTime eDate = DateTime.ParseExact(maxDate , "yyyyMMdd" , null); //yyyy/MM/dd

            //2. 讀取資料
            DataTable dt30411 = new D30410().ListData2(txtStartDate.DateTimeValue , eDate);
            if (dt30411.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2} - {3},無任何資料!" , txtStartDate.Text , txtEndDate.Text , rptId , rptName) , "處理結果");
               return false;
            } //if (dt.Rows.Count <= 0 )

            //3. 切換Sheet
            Worksheet ws30411 = workbook.Worksheets[(int)sheetNo];
            ws30411.Range["A1"].Select();
            ws30411.Cells[1 , 0].Value = txtStartDate.Text;
            ws30411.Cells[1 , 1].Value = txtEndDate.Text;

            //3.1 撈資料列總數
            //PB這邊帶入參數為txnId = 30410 , txdId = 30410,兩者撈出皆為500 
            int rowCnt = new RPT().DataByRptId("30410" , "30410").AsInt();
            int rowTotal = row + rowCnt;

            //4.填資料(交割年月)
            int maxSeqNo = dt30411.Compute("Max(seq_no)" , "").AsInt(); //取得seq_no欄位的最大值
            int found = 0;
            for (int w = 1 ; w <= maxSeqNo ; w++) {
               if (dt30411.Select("seq_no =" + w).Length != 0) {
                  found = dt30411.Rows.IndexOf(dt30411.Select(string.Format("seq_no ={0}" , w))[0]);
               }

               if (found >= 0) {
                  string text = dt30411.Rows[found]["amif_settle_date"].AsString();
                  ws30411.Cells[3 , w + 2].Value = text.SubStr(0 , 4) + "/" + text.SubStr(4 , 2);
               }

            }

            string kindId = "";
            foreach (DataRow dr in dt30411.Rows) {
               string amifKindId = dr["amif_kind_id"].AsString();
               string apdkName = dr["apdk_name"].AsString();
               decimal amifMQntyTal = dr["amif_m_qnty_tal"].AsDecimal();
               found = dr["seq_no"].AsInt();
               //int exRow = row - 1;

               if (kindId != amifKindId) {
                  kindId = amifKindId;
                  row++;
                  ws30411.Cells[row - 1 , 0].Value = kindId;
                  ws30411.Cells[row - 1 , 1].Value = apdkName;
               }

               ws30411.Cells[row - 1 , found + 2].Value = amifMQntyTal;
            }

            //4. 刪除空白列
            Range ra = ws30411.Range[string.Format("{0}:504" , row + 1)];
            ra.Delete(DeleteMode.EntireRow);

            ws30411.Range["A1"].Select();
            ws30411.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }

      /// <summary>
      /// wf_30412 (sheet3 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeDetail</param>
      /// <param name="row"> 4 </param>
      /// <returns></returns>
      protected bool wf_30412(Workbook workbook , SheetNo sheetNo , int row) {

         string rptName = "股票期貨各標的未平倉量分佈明細統計表"; //報表標題名稱
         string rptId = "30412";
         ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

         try {
            //1. 取得資料最大日期, 抓取OI用 (在wf_30410取得)
            string maxDate = new D30410().GetMaxDate(StartDate , EndDate);
            if (string.IsNullOrEmpty(maxDate)) {
               MessageDisplay.Info(string.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , rptId , rptName) , "處理結果");
               return false;
            }//if (string.IsNullOrEmpty(maxDate))
            DateTime eDate = DateTime.ParseExact(maxDate , "yyyyMMdd" , null); //yyyy/MM/dd

            //2. 讀取資料
            DataTable dt30412 = new D30410().ListData2(txtStartDate.DateTimeValue , eDate);
            if (dt30412.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2} - {3},無任何資料!" , txtStartDate.Text , txtEndDate.Text , rptId , rptName) , "處理結果");
               return false;
            } //if (dt.Rows.Count <= 0 )

            //3. 切換Sheet
            Worksheet ws30412 = workbook.Worksheets[(int)sheetNo];
            ws30412.Range["A1"].Select();
            ws30412.Cells[1 , 0].Value = txtStartDate.Text;
            ws30412.Cells[1 , 1].Value = txtEndDate.Text;

            //3.1 撈資料列總數
            //PB這邊帶入參數為txnId = 30410 , txdId = 30410,兩者撈出皆為500 
            int rowCnt = new RPT().DataByRptId("30410" , "30411").AsInt();
            int rowTotal = row + rowCnt;

            //4.填資料(交割年月)
            int found = 0;
            int maxSeqNo = dt30412.Compute("Max(seq_no)" , "").AsInt(); //取得seq_no欄位的最大值
            for (int w = 1 ; w <= maxSeqNo ; w++) {
               if (dt30412.Select("seq_no =" + w).Length != 0) {
                  found = dt30412.Rows.IndexOf(dt30412.Select(string.Format("seq_no ={0}" , w))[0]);
               }

               if (found >= 0) {
                  string text = dt30412.Rows[found]["amif_settle_date"].AsString();
                  ws30412.Cells[3 , w + 2].Value = text.SubStr(0 , 4) + "/" + text.SubStr(4 , 2);
               }

            }

            string kindId = "";
            foreach (DataRow dr in dt30412.Rows) {

               string amifKindId = dr["amif_kind_id"].AsString();
               string apdkName = dr["apdk_name"].AsString();
               decimal amifOpenInt = dr["amif_open_interest"].AsDecimal();
               found = dr["seq_no"].AsInt();

               if (kindId != amifKindId) {
                  kindId = amifKindId;
                  row++;
                  ws30412.Cells[row - 1 , 0].Value = kindId;
                  ws30412.Cells[row - 1 , 1].Value = apdkName;
               }

               ws30412.Cells[row - 1 , found + 2].Value = amifOpenInt;
            }

            //4. 刪除空白列
            Range ra = ws30412.Range[string.Format("{0}:504" , row + 1)];
            ra.Delete(DeleteMode.EntireRow);

            ws30412.Range["A1"].Select();
            ws30412.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }
   }
}