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

      private D30400 dao30400;
      private AI2 daoAI2;

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

         dao30400 = new D30400();
         daoAI2 = new AI2();

#if DEBUG
         //winni test
         //txtMon.DateTimeValue = DateTime.ParseExact("2012/07" , "yyyy/MM" , null);
         //this.Text += "(開啟測試模式),Date=2012/07";
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
            int row = 1;
            wf_30401(workbook , SheetNo.sheet1 , row);
            wf_30402(workbook , SheetNo.sheet2 , row);
            wf_30408(workbook , SheetNo.sheet8 , row);
            row = 0;
            wf_30403(workbook , SheetNo.sheet3 , row);
            if (txtKindId.Text != "%") {
               row = 1;
               wf_30404(workbook , SheetNo.sheet4 , row , txtKindId.Text);
            }
            row = 2;
            wf_30405(workbook , SheetNo.sheet5 , row);
            row = 3;
            wf_30406(workbook , SheetNo.sheet6 , row);
            wf_30407(workbook , SheetNo.sheet7 , row);

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
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;

      }

      /// <summary>
      /// wf_30401 (sheet1 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 1 </param>
      /// <returns></returns>
      protected bool wf_30401(Workbook workbook , SheetNo sheetNo , int rowNum) {

         string rptName = "股票期貨成交量及未平倉量變化表"; //報表標題名稱
         ShowMsg("30401－" + rptName + " 轉檔中...");

         try {
            //1. 前月倒數1天交易日(?)
            //DateTime sDate = GlobalInfo.OCF_DATE.AddDays(-GlobalInfo.OCF_DATE.Day + 1); //月份第1天
            //DateTime eDate = GlobalInfo.OCF_DATE.AddMonths(1).AddDays(-GlobalInfo.OCF_DATE.AddMonths(1).Day); //月份最後1天
            DateTime sDate = txtMon.DateTimeValue.AddDays(-txtMon.DateTimeValue.Day + 1); //月份第1天
            DateTime eDate = txtMon.DateTimeValue.AddMonths(1).AddDays(-txtMon.DateTimeValue.AddMonths(1).Day); //月份最後1天

            string strSDate = sDate.ToString("yyyyMMdd");
            string strEDate = eDate.ToString("yyyyMMdd");
            string lastTradeDate = daoAI2.GetLastTradeDate("D" , "O" , "S" , sDate , eDate);//抓當月最後交易日

            //2. 讀取資料
            DataTable dt30401 = dao30400.Get30401Data(strSDate , strEDate , "F");
            if (dt30401.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},30401 - {2},無任何資料!" , sDate , eDate , rptName));
            } //if (dt30401.Rows.Count <= 0)

            //3. 切換Sheet
            Worksheet ws1 = workbook.Worksheets[(int)sheetNo];

            //4. 處理資料
            int rowTotal = rowNum + 34;
            string ymd = "";

            foreach (DataRow dr in dt30401.Rows) {
               string ai2Ymd = dr["ai2_ymd"].AsString();
               decimal mQnty = dr["ai2_m_qnty"].AsDecimal();
               decimal sumMmkQnty = dr["cp_sum_ai2_mmk_qnty"].AsDecimal();
               decimal sumOi = dr["cp_sum_ai2_oi"].AsDecimal();

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
            if (rowTotal > dt30401.Rows.Count) {
               Range ra = ws1.Range[(dt30401.Rows.Count + 3).ToString() + ":" + rowTotal.ToString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws1.Range["A1"].Select();
            ws1.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }

      /// <summary>
      /// wf_30402 (sheet2 & sheet8 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 1 </param>
      /// <returns></returns>
      protected bool wf_30402(Workbook workbook , SheetNo sheetNo , int rowNum) {
         string rptName = "股票期貨交易概況表";
         ShowMsg("30402－" + rptName + " 轉檔中...");

         try {
            string strMon = txtMon.Text.Replace("/" , "");

            //1. 讀取資料
            DataTable dt30402 = dao30400.Get30402Data(strMon , "F");
            if (dt30402.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0},30402 - {1},無任何資料!" , strMon , rptName));
            } //if (dt30401.Rows.Count <= 0)

            //2. 切換Sheet
            Worksheet ws2 = workbook.Worksheets[(int)sheetNo];

            //3. 處理資料
            int rowTotal = 1001;
            foreach (DataRow dr in dt30402.Rows) {
               string pdkName = dr["pdk_name"].AsString();
               string kindId2 = dr["kind_id_2"].AsString();
               decimal mQnty = dr["m_qnty"].AsDecimal();

               ws2.Cells[rowNum , 0].Value = string.Format("{0}({1})" , pdkName , kindId2);
               ws2.Cells[rowNum , 1].Value = mQnty;
               ws2.Cells[rowNum , 3].Value = pdkName;
               rowNum++;
            }//foreach (DataRow dr in dt30402.Rows)

            rowNum--;

            //4. 刪除空白列
            if (rowTotal > dt30402.Rows.Count) {
               Range ra = ws2.Range[(dt30402.Rows.Count + 2).ToString() + ":" + rowTotal.ToString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws2.Range["A1"].Select();
            ws2.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30403 (sheet3 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 0 </param>
      /// <returns></returns>
      protected bool wf_30403(Workbook workbook , SheetNo sheetNo , int rowNum) {
         string rptName = "股票期貨交易概況表";
         ShowMsg("30403－" + rptName + " 轉檔中...");

         try {
            //1. 抓當月最後交易日
            string strMon = txtMon.Text.Replace("/" , "");
            string lastTradeDate = dao30400.GetThisMonLastTradeData(strMon); //當月最後交易日(yyyyMMdd)           
            string sDate = strMon + "01"; //當月第一天
            DateTime startDate = DateTime.ParseExact(sDate , "yyyyMMdd" , null);
            DateTime lastTrade = DateTime.ParseExact(lastTradeDate , "yyyyMMdd" , null);

            //2. 讀取資料
            DataTable dt30403 = dao30400.Get30403Data(sDate , lastTradeDate);
            if (dt30403.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},30403 - {2},無任何資料!" , startDate , lastTrade , rptName));
            } //if (dt30401.Rows.Count <= 0)

            //3. 切換Sheet
            Worksheet ws3 = workbook.Worksheets[(int)sheetNo];

            //4. 處理資料
            string kindId = "";
            foreach (DataRow dr in dt30403.Rows) {
               string kindId2 = dr["ai2_kind_id_2"].AsString();
               string pdkName = dr["pdk_name"].AsString();

               if (kindId != kindId2) {
                  kindId = kindId2;
                  rowNum++;
                  ws3.Cells[rowNum , 0].Value = kindId;
                  ws3.Cells[rowNum , 1].Value = pdkName;
               }

               int colNum = dr["seq_no"].AsInt();
               if (colNum > 0) {
                  string ymd = dr["ai2_ymd"].AsString();
                  string tmpYmd = DateTime.ParseExact(ymd , "yyyyMMdd" , null).ToString("yyyy/MM/dd");
                  decimal mQnty = dr["ai2_m_qnty"].AsDecimal();
                  ws3.Cells[0 , colNum + 1].Value = tmpYmd;
                  ws3.Cells[rowNum , colNum + 1].Value = mQnty;
               }

            }//foreach (DataRow dr in dt30403.Rows)

            ws3.Range["A1"].Select();
            ws3.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30404 (sheet4 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 1 </param>
      /// <returns></returns>
      protected bool wf_30404(Workbook workbook , SheetNo sheetNo , int rowNum , string strKindId) {
         string rptName = "個別股票期貨成交量及未平倉量變化表";
         ShowMsg("30404－" + rptName + " 轉檔中...");

         try {
            //1. 取日期
            string sDate = PbFunc.f_get_last_day("AI3" , strKindId , txtMon.Text , 2).ToString("yyyy/MM/dd");//前月倒數2天交易日
            string eDate = PbFunc.f_get_end_day("AI3" , strKindId , txtMon.Text).ToString("yyyy/MM/dd"); //抓當月最後交易日

            //2. 讀取資料
            if (strKindId != "%") {
               strKindId += "%";
            }
            DataTable dt30404 = dao30400.Get30404Data(strKindId , sDate , eDate);
            if (dt30404.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},30404 - {2},無任何資料!" , sDate , eDate , rptName));
            } //if (dt30404.Rows.Count <= 0)

            //3. 切換Sheet
            Worksheet ws4 = workbook.Worksheets[(int)sheetNo];

            //4. 處理資料
            int rowTotal = 35;
            DataTable dtAPDK = dao30400.GetAdpkData(strKindId); //好像只會有一筆資料
            if (dtAPDK.Rows.Count <= 0) {
               return false;
            } else if (dtAPDK.Rows.Count > 0) {
               ws4.Cells[0 , 0].Value = dtAPDK.Rows[0]["apdk_name"].AsString();
               ws4.Cells[0 , 1].Value = txtKindId.Text;
               ws4.Cells[0 , 8].Value = dtAPDK.Rows[0]["apdk_stock_id"].AsString();
            }

            DateTime ymd = DateTime.ParseExact("1900/01/01" , "yyyy/MM/dd" , null);
            foreach (DataRow dr in dt30404.Rows) {
               DateTime ai3Date = dr["ai3_date"].AsDateTime();
               decimal closePrice = dr["ai3_close_price"].AsDecimal();
               decimal mQnty = dr["ai3_m_qnty"].AsDecimal();
               decimal ai3Oi = dr["ai3_oi"].AsDecimal();
               decimal ai3Index = dr["ai3_index"].AsDecimal();
               decimal ai3Amount = dr["ai3_amount"].AsDecimal();
               if (ymd != ai3Date) {
                  ymd = ai3Date;
                  rowNum++;
                  ws4.Cells[rowNum , 0].Value = ymd.ToString("MM/dd");
               }

               ws4.Cells[rowNum , 1].Value = closePrice;
               ws4.Cells[rowNum , 3].Value = mQnty;
               ws4.Cells[rowNum , 4].Value = ai3Oi;
               ws4.Cells[rowNum , 5].Value = ai3Index;
               ws4.Cells[rowNum , 7].Value = ai3Amount;

            }//foreach (DataRow dr in dt30404.Rows)

            //5. 刪除空白列
            if (rowTotal > dt30404.Rows.Count) {
               Range ra = ws4.Range[(dt30404.Rows.Count + 3).ToString() + ":" + rowTotal.ToString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws4.Range["A1"].Select();
            ws4.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30405 (sheet5 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 1 </param>
      /// <returns></returns>
      protected bool wf_30405(Workbook workbook , SheetNo sheetNo , int rowNum) {
         string rptName = "股票期貨交易概況統計表";
         ShowMsg("30405－" + rptName + " 轉檔中...");

         try {
            //1. 取日期 (取查詢年月月份的第1天跟最後1天(string))
            string sDate = txtMon.Text.Replace("/" , "") + "01";
            string eDate = txtMon.DateTimeValue.AddMonths(1).AddDays(-txtMon.DateTimeValue.AddMonths(1).Day).ToString("yyyyMMdd");

            //2. 讀取資料
            DataTable dt30405 = dao30400.Get30405Data(sDate , eDate);
            if (dt30405.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},30405 - {2},無任何資料!" , sDate , eDate , rptName));
            }

            //3. 切換Sheet
            Worksheet ws5 = workbook.Worksheets[(int)sheetNo];

            //4. 處理資料
            ws5.Cells[1 , 0].Value = txtMon.Text;
            int rowTotal = 34;
            string ymd = "19000101";
            foreach (DataRow dr in dt30405.Rows) {
               string ai2Ymd = dr["ai2_ymd"].AsString();
               decimal mQnty = dr["ai2_m_qnty"].AsDecimal();
               decimal ai2Oi = dr["ai2_oi"].AsDecimal();
               decimal am10Cnt = dr["am10_cnt"].AsDecimal();
               decimal cnt = dr["am9_acc_cnt"].AsDecimal();
               decimal idCnt = dr["ab4_id_cnt"].AsDecimal();
               if (ymd != ai2Ymd) {
                  ymd = ai2Ymd;
                  rowNum++;

                  string dTimeYmd = DateTime.ParseExact(ymd , "yyyyMMdd" , null).ToString("yyyy/MM/dd");
                  ws5.Cells[rowNum , 0].Value = dTimeYmd;
               }

               ws5.Cells[rowNum , 1].Value = mQnty;
               ws5.Cells[rowNum , 2].Value = ai2Oi;
               ws5.Cells[rowNum , 3].Value = am10Cnt;
               ws5.Cells[rowNum , 5].Value = cnt;
               ws5.Cells[rowNum , 7].Value = idCnt;

            }//foreach (DataRow dr in dt30405.Rows)

            //5. 刪除空白列
            if (rowTotal > dt30405.Rows.Count) {
               Range ra = ws5.Range[(dt30405.Rows.Count + 4).ToString() + ":" + rowTotal.ToString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws5.Range["A1"].Select();
            ws5.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30406 (sheet6 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 3 </param>
      /// <returns></returns>
      protected bool wf_30406(Workbook workbook , SheetNo sheetNo , int rowNum) {
         string rptName = "股票期貨交易量分佈明細統計表";
         ShowMsg("30406－" + rptName + " 轉檔中...");

         try {
            //1. 取日期 (取查詢年月月份的第1天跟最後1天(string))
            string sDate = txtMon.Text + "/01";
            string eDate = txtMon.DateTimeValue.AddMonths(1).AddDays(-txtMon.DateTimeValue.AddMonths(1).Day).ToString("yyyy/MM/dd");

            //2. 讀取資料
            DataTable dt30406 = dao30400.Get30406Data(sDate , eDate);
            if (dt30406.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},30406 - {2},無任何資料!" , sDate , eDate , rptName));
            }

            //3. 切換Sheet
            Worksheet ws6 = workbook.Worksheets[(int)sheetNo];

            //4. 處理資料
            ws6.Cells[1 , 0].Value = txtMon.Text;
            int rowTotal = 35;

            //交割年月
            int maxSeqNo = dt30406.Compute("MAX(seq_no)" , "").AsInt();
            int found;
            for (int w = 1 ; w <= maxSeqNo ; w++) {
               //string settleDate = dt30406.Rows[found +１]["amif_settle_date"].AsString();
               //dt30406.PrimaryKey = new DataColumn[] { dt30406.Columns["AMIF_DATE"] , dt30406.Columns["SEQ_NO"] };
               found = dt30406.Rows.IndexOf(dt30406.Select(string.Format("seq_no ={0}" , w.AsString())).FirstOrDefault());
               if (found < 0) {
                  continue;
               } else {
                  string settleDate = dt30406.Rows[found]["amif_settle_date"].AsString();
                  ws6.Cells[3 , w + 1].Value = DateTime.ParseExact(settleDate , "yyyyMM" , null).ToString("yyyy/MM");
               }
            }//for (int w = 0 ; w < maxSeqNo ; w++)

            DateTime baseTime = DateTime.ParseExact("1900/01/01" , "yyyy/MM/dd" , null);
            foreach (DataRow dr in dt30406.Rows) {
               DateTime amifDate = dr["amif_date"].AsDateTime();
               int seqNo = dr["seq_no"].AsInt();
               decimal mQntyTal = dr["amif_m_qnty_tal"].AsDecimal();
               if (baseTime != amifDate) {
                  baseTime = amifDate;
                  rowNum++;
                  ws6.Cells[rowNum , 0].Value = baseTime.ToString("yyyy/MM/dd");
               }

               found = seqNo;
               ws6.Cells[rowNum , found + 1].Value = mQntyTal;

            }//foreach (DataRow dr in dt30406.Rows)

            //5. 刪除空白列
            if (rowTotal > rowNum - 3) {
               Range ra = ws6.Range[(rowNum + 2).ToString() + ":" + rowTotal.ToString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws6.Range["A1"].Select();
            ws6.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30407 (sheet7 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 3 </param>
      /// <returns></returns>
      protected bool wf_30407(Workbook workbook , SheetNo sheetNo , int rowNum) {

         string rptName = "股票期貨交易量分佈明細統計表";

         try {
            //1. 切換Sheet
            Worksheet ws7 = workbook.Worksheets[(int)sheetNo];
            ws7.Cells[1 , 0].Value = txtMon.Text;

            int rowTotal = 35;

            //2. 取日期 (取查詢年月月份的第1天跟最後1天(string))
            string sDate = txtMon.Text + "/01";
            string eDate = txtMon.DateTimeValue.AddMonths(1).AddDays(-txtMon.DateTimeValue.AddMonths(1).Day).ToString("yyyy/MM/dd");

            //3. 讀取資料
            DataTable dt30407 = dao30400.Get30406Data(sDate , eDate);
            if (dt30407.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},30406 - {2},無任何資料!" , sDate , eDate , rptName));
            }

            //4. 處理資料
            //交割年月
            int maxSeqNo = dt30407.Compute("MAX(seq_no)" , "").AsInt();
            for (int w = 1 ; w <= maxSeqNo ; w++) {
               //dt30407.PrimaryKey = new DataColumn[] { dt30407.Columns["AMIF_DATE"] , dt30407.Columns["SEQ_NO"] };
               int found = dt30407.Rows.IndexOf(dt30407.Select(string.Format("seq_no ={0}" , w.AsString())).FirstOrDefault());
               if (found < 0) {
                  continue;
               } else {
                  string settleDate = dt30407.Rows[found]["amif_settle_date"].AsString();
                  ws7.Cells[3 , w + 1].Value = DateTime.ParseExact(settleDate , "yyyyMM" , null).ToString("yyyy/MM");
               }

            }//for (int w = 0 ; w < maxSeqNo ; w++)

            DateTime baseTime = DateTime.ParseExact("1900/01/01" , "yyyy/MM/dd" , null);
            foreach (DataRow dr in dt30407.Rows) {
               DateTime amifDate = dr["amif_date"].AsDateTime();
               decimal openInterest = dr["amif_open_interest"].AsDecimal();
               if (baseTime != amifDate) {
                  baseTime = amifDate;

                  rowNum++;

                  ws7.Cells[rowNum , 0].Value = baseTime.ToString("yyyy/MM/dd");
               }
               int found = dr["seq_no"].AsInt();
               ws7.Cells[rowNum , found + 1].Value = openInterest;

            }//foreach (DataRow dr in dt30407.Rows)

            //5. 刪除空白列
            if (rowTotal > rowNum - 3) {
               Range ra = ws7.Range[(rowNum + 2).ToString() + ":" + rowTotal.ToString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws7.Range["A1"].Select();
            ws7.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30408 (sheet8 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="row"> 1 </param>
      /// <returns></returns>
      protected bool wf_30408(Workbook workbook , SheetNo sheetNo , int rowNum) {
         string rptName = "Top 30 股票期貨成交量";

         try {
            string strMon = txtMon.Text.Replace("/" , "");

            //1. 讀取資料
            DataTable dt30402 = dao30400.Get30402Data(strMon , "F");
            if (dt30402.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0},30402 - {1},無任何資料!" , strMon , rptName));
            } //if (dt30401.Rows.Count <= 0)

            //1.1 sort dt
            dt30402.DefaultView.Sort = "m_qnty desc , kind_id_2 asc";
            dt30402 = dt30402.DefaultView.ToTable();

            //2. 切換Sheet
            Worksheet ws8 = workbook.Worksheets[(int)sheetNo];

            //3. 處理資料
            int cnt = Math.Min(30 , dt30402.Rows.Count);

            for (int w = 0 ; w < cnt ; w++) {
               rowNum++;
               DataRow dr = dt30402.Rows[w];
               decimal totalQnty = dt30402.Compute("SUM(m_qnty)" , "").AsDecimal();
               string kindId2 = dr["kind_id_2"].AsString();
               string pdkName = dr["pdk_name"].AsString();
               decimal mQnty = dr["m_qnty"].AsDecimal();

               ws8.Cells[rowNum , 1].Value = kindId2;
               ws8.Cells[rowNum , 2].Value = pdkName;
               ws8.Cells[rowNum , 3].Value = mQnty;
               ws8.Cells[rowNum , 4].Value = mQnty / totalQnty;
            }//for (int w = 0 ; w < cnt ; w++)

            ws8.Range["A1"].Select();
            ws8.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }
   }
}