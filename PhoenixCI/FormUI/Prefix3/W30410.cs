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
/// Winni, 2019/3/13
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
         txtStartDate.Text = GlobalInfo.OCF_DATE.AsString("yyyy/MM/01");
         txtEndDate.Text = GlobalInfo.OCF_DATE.AsString("yyyy/MM/dd");

#if DEBUG
         //winni test
         txtStartDate.DateTimeValue = DateTime.ParseExact("2016/01/01" , "yyyy/MM/dd" , null);
         txtEndDate.DateTimeValue = DateTime.ParseExact("2018/11/29" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2016/01/01~2018/11/29";
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

            if (!txtStartDate.IsDate(txtStartDate.Text , CheckDate.Start)
                  || !txtEndDate.IsDate(txtEndDate.Text , CheckDate.End)) {
               return ResultStatus.Fail; ;
            }

            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(GlobalInfo.ErrorText , CheckDate.Datedif);
               return ResultStatus.Fail; ;
            }
            #endregion

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料轉出中........";
            this.Refresh();

            //2. copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);

            //3. open xls
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //4. write data
            int row;
            bool res1 = false, res2 = false , res3 = false;
            row = 3;
            res1 = wf_30410(workbook , SheetNo.tradeSum , row);

            row = 4; //PB這邊帶1，但進去後帶回4
            res2 = wf_30411(workbook , SheetNo.tradeDetail , row);

            row = 4; //PB這邊帶1，但進去後帶回4
            res3 = wf_30412(workbook , SheetNo.oint , row);

            if(!res1 && !res2 && !res3) {
               //關閉檔案
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
            if (String.IsNullOrEmpty(maxDate)) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
               return false;
            }//if (String.IsNullOrEmpty(maxDate))

            //2. 讀取資料
            string eDate = maxDate;
            DataTable dt30410 = new D30410().ListData(StartDate , eDate);
            if (dt30410.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , eDate , _ProgramID , rptName));
            } //if (dt.Rows.Count <= 0 )

            //3. 切換Sheet
            Worksheet ws = workbook.Worksheets[(int)sheetNo];
            ws.Cells[1 , 0].Value = StartDate;
            ws.Cells[1 , 1].Value = EndDate;

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
            return false;
            throw ex;
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
         labMsg.Text = rptId + "－" + rptName + " 轉檔中...";

         try {

            //1. 取得資料最大日期, 抓取OI用 (在wf_30410取得)
            string maxDate = new D30410().GetMaxDate(StartDate , EndDate);
            if (String.IsNullOrEmpty(maxDate)) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
               return false;
            }//if (String.IsNullOrEmpty(maxDate))
            DateTime eDate = DateTime.ParseExact(maxDate , "yyyy/MM/dd" , null); //yyyy/MM/dd

            //2. 讀取資料
            DataTable dt30411 = new D30410().ListData2(txtStartDate.DateTimeValue , eDate);
            if (dt30411.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , txtStartDate.Text , txtEndDate.Text , rptId , rptName));
               return false;
            } //if (dt.Rows.Count <= 0 )

            //3. 切換Sheet
            Worksheet ws = workbook.Worksheets[(int)sheetNo];
            ws.Range["A1"].Select();
            ws.Cells[1 , 0].Value = StartDate;
            ws.Cells[1 , 1].Value = EndDate;

            //3.1 撈資料列總數
            //PB這邊帶入參數為txnId = 30410 , txdId = 30410,兩者撈出皆為500 
            int rowCnt = new RPT().DataByRptId("30410" , "30411").AsInt();
            int rowTotal = row + rowCnt;

            //4.填資料(交割年月)
            int maxSeqNo = dt30411.Compute("Max(seq_no)" , "").AsInt(); //取得seq_no欄位的最大值
            for (int w = 0 ; w < maxSeqNo ; w++) {
               int found = dt30411.Rows.IndexOf(dt30411.Rows.Find(dt30411.Rows[w]["seq_no"])).AsInt();
               DataRow[] dr30411 = dt30411.Select("seq_no =" + w.AsString());
               if (dr30411.Length != 0) { //若為空陣列即不執行
                  ws.Cells[3 , w + 3].Value = dt30411.Rows[found]["amif_settle_date"].AsString("yyyy/MM");
               }//if (dr30411.Length != 0)
            }

            string kindId = "";
            foreach (DataRow dr in dt30411.Rows) {
               row++;
               string amifKindId = dr["amif_kind_id"].AsString();
               string apdkName = dr["apdk_name"].AsString();
               decimal amifMQntyTal = dr["amif_m_qnty_tal"].AsDecimal();
               int found = dr["seq_no"].AsInt();
               int exRow = row - 1;

               if (kindId != amifKindId) {
                  kindId = amifKindId;
                  ws.Cells[exRow , 0].Value = kindId;
                  ws.Cells[exRow , 1].Value = apdkName;
               }

               ws.Cells[exRow , found + 2].Value = amifMQntyTal;
            }

            //4. 刪除空白列
            if (dt30411.Rows.Count < rowTotal) {
               ws.Rows.Remove(row , rowTotal - row);
            }

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
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
         labMsg.Text = rptId + "－" + rptName + " 轉檔中...";

         try {
            //1. 取得資料最大日期, 抓取OI用 (在wf_30410取得)
            string maxDate = new D30410().GetMaxDate(StartDate , EndDate);
            if (String.IsNullOrEmpty(maxDate)) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
               return false;
            }//if (String.IsNullOrEmpty(maxDate))
            DateTime eDate = DateTime.ParseExact(maxDate , "yyyy/MM/dd" , null); //yyyy/MM/dd

            //2. 讀取資料
            DataTable dt30412 = new D30410().ListData2(txtStartDate.DateTimeValue , eDate);
            if (dt30412.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , txtStartDate.Text , txtEndDate.Text , rptId , rptName));
               return false;
            } //if (dt.Rows.Count <= 0 )

            //3. 切換Sheet
            Worksheet ws = workbook.Worksheets[(int)sheetNo];
            ws.Range["A1"].Select();
            ws.Cells[1 , 0].Value = StartDate;
            ws.Cells[1 , 1].Value = EndDate;

            //3.1 撈資料列總數
            //PB這邊帶入參數為txnId = 30410 , txdId = 30410,兩者撈出皆為500 
            int rowCnt = new RPT().DataByRptId("30410" , "30411").AsInt();
            int rowTotal = row + rowCnt;

            //4.填資料(交割年月)
            int maxSeqNo = dt30412.Compute("Max(seq_no)" , "").AsInt(); //取得seq_no欄位的最大值
            for (int w = 0 ; w < maxSeqNo ; w++) {
               int found = dt30412.Rows.IndexOf(dt30412.Rows.Find(dt30412.Rows[w]["seq_no"])).AsInt();
               DataRow[] dr30411 = dt30412.Select("seq_no =" + w.AsString());
               if (dr30411.Length != 0) { //若為空陣列即不執行
                  ws.Cells[3 , w + 3].Value = dt30412.Rows[found]["amif_settle_date"].AsString("yyyy/MM");
               }//if (dr30411.Length != 0)
            }

            string kindId = "";
            foreach (DataRow dr in dt30412.Rows) {
               row++;
               string amifKindId = dr["amif_kind_id"].AsString();
               string apdkName = dr["apdk_name"].AsString();
               decimal amifOpenInt = dr["amif_open_interest"].AsDecimal();
               int found = dr["seq_no"].AsInt();
               int exRow = row - 1;

               if (kindId != amifKindId) {
                  kindId = amifKindId;
                  ws.Cells[exRow , 0].Value = kindId;
                  ws.Cells[exRow , 1].Value = apdkName;
               }

               ws.Cells[exRow , found + 2].Value = amifOpenInt;
            }

            //4. 刪除空白列
            if (dt30412.Rows.Count < rowTotal) {
               ws.Rows.Remove(row , rowTotal - row);
            }

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }
   }
}