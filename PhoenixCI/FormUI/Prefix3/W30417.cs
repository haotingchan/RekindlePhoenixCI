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
/// Winni, 2019/3/19
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 股票期貨每週交概況統計表
   /// </summary>
   public partial class W30417 : FormParent {

      protected enum SheetNo {
         tradeSum = 0, //每週交易概況
         dailyAvg = 1, //每週日均量
         oint = 2 //每週日均未平倉量
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

      /// <summary>
      /// yyyyMM
      /// </summary>
      public string StartMon {
         get {
            return txtStartDate.DateTimeValue.ToString("yyyyMM");
         }
      }

      /// <summary>
      /// yyyyMM
      /// </summary>
      public string EndMon {
         get {
            return txtEndDate.DateTimeValue.ToString("yyyyMM");
         }
      }
      #endregion

      public W30417(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //設定初始年月yyyy/MM/dd     
            txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtStartDate.EnterMoveNextControl = true;
            txtStartDate.Focus();

            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEndDate.EnterMoveNextControl = true;
            txtEndDate.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
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
               return ResultStatus.Fail;
            }

            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(GlobalInfo.ErrorText , CheckDate.Datedif);
               return ResultStatus.Fail;
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

            //4. 年月表頭
            DataTable dtAi2Ymd = new AI2().ListWeek(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue , "D" , "F");
            if (dtAi2Ymd.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2}-年月,無任何資料!" , StartDate , EndDate , _ProgramID));
               WriteLog(string.Format("{0}~{1},{2}-年月,無任何資料!" , StartDate , EndDate , _ProgramID));
            }//if (dtAi2Ymd.Rows.Count <= 0)

            //4.1處理資料型態
            DataTable dtYmd = dtAi2Ymd.Clone(); //轉型別用的datatable
            dtYmd.Columns["startDate"].DataType = typeof(string); //將原DataType(datetime)轉為string
            dtYmd.Columns["endDate"].DataType = typeof(string);
            foreach (DataRow row in dtAi2Ymd.Rows) {
               dtYmd.ImportRow(row);
            }

            for (int i = 0 ; i < dtYmd.Rows.Count ; i++) {
               dtYmd.Rows[i]["startDate"] = Convert.ToDateTime(dtAi2Ymd.Rows[i]["startDate"]).ToString("yyyy/MM/dd");
               dtYmd.Rows[i]["endDate"] = Convert.ToDateTime(dtAi2Ymd.Rows[i]["endDate"]).ToString("yyyy/MM/dd");
            }


            //5. 填資料
            bool res1 = false, res2 = false, res3 = false;
            int rowNum = 2;
            res1 = wf_30417(workbook , SheetNo.tradeSum , rowNum , dtYmd);
            res2 = wf_30418(workbook , SheetNo.dailyAvg , rowNum , dtYmd);
            res3 = wf_30419(workbook , SheetNo.oint , rowNum , dtYmd);

            //6. save 
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
      /// wf_30417 (sheet1 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="rowNum"> 2 </param>
      /// <returns></returns>
      protected bool wf_30417(Workbook workbook , SheetNo sheetNo , int rowNum , DataTable dtYmd) {

         string rptName = "股票期貨每週交概況統計表"; //報表標題名稱
         labMsg.Text = _ProgramID + "－" + rptName + " 轉檔中...";

         try {
            //1. 切換Sheet
            Worksheet ws30417 = workbook.Worksheets[(int)sheetNo];

            //2. 讀取資料
            DataTable dt = new D30417().ListData(StartDate , EndDate);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
               return false;
            } //if (dt.Rows.Count <= 0 )

            //3. 日期
            int pos = 0;
            foreach (DataRow dr in dtYmd.Rows) {
               pos++;
               //將週日期區間數量相加
               string ai2Ymd = dr["startDate"].AsString().Replace("/" , "");
               string ymdEnd = dr["endDate"].AsString().Replace("/" , "");
               DataTable dtTmp = dt.Filter(string.Format("ai2_ymd>='{0}' and ai2_ymd<='{1}'" , ai2Ymd , ymdEnd));

               if (dtTmp.Rows.Count > 0) {
                  int tmpRow = pos + 1;
                  decimal sumDayCount = dtTmp.Compute("Sum(ai2_day_count)" , "").AsDecimal();
                  decimal sumMQnty = dtTmp.Compute("Sum(ai2_m_qnty)" , "").AsDecimal();
                  decimal sumOi = dtTmp.Compute("Sum(ai2_oi)" , "").AsDecimal();
                  decimal sumCnt = dtTmp.Compute("Sum(am10_cnt)" , "").AsDecimal();
                  decimal sumAccCnt = dtTmp.Compute("Sum(am9_acc_cnt)" , "").AsDecimal();

                  string sDate = dr["startDate"].AsString().SubStr(0 , 10);
                  string eDate = dr["endDate"].AsString().SubStr(0 , 10);

                  ws30417.Cells[tmpRow , 0].Value = sumDayCount;
                  ws30417.Cells[tmpRow , 1].Value = string.Format("{0}~{1}" , sDate , eDate);
                  ws30417.Cells[tmpRow , 2].Value = sumMQnty;
                  ws30417.Cells[tmpRow , 4].Value = sumOi;
                  ws30417.Cells[tmpRow , 6].Value = sumCnt;
                  ws30417.Cells[tmpRow , 9].Value = sumAccCnt;
               }//if (dtTmp.Rows.Count > 0)
            }//foreach (DataRow dr in dtYm.Rows)

            ws30417.Range["A1"].Select();
            ws30417.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30418 (sheet2 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="rowNum"> 2 </param>
      /// <returns></returns>
      protected bool wf_30418(Workbook workbook , SheetNo sheetNo , int rowNum , DataTable dtYmd) {

         string rptName = "股票期貨各標的每週日均量統計表"; //報表標題名稱
         labMsg.Text = "30419－" + rptName + " 轉檔中...";

         try {
            #region 30418
            //1. 表頭
            DataTable dtProd = new D30414().ListProdByMon(StartMon , EndMon);
            if (dtProd.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},30418 - 商品檔,無任何資料!" , StartMon , EndMon));
               return false;
            }//if (dtProd.Rows.Count <= 0)

            //2. 切換Sheet
            Worksheet ws30418 = workbook.Worksheets[(int)sheetNo];

            //3. 讀取資料           
            for (int w = 0 ; w < dtProd.Rows.Count ; w++) {
               rowNum++;
               string apdkName = dtProd.Rows[w]["apdk_name"].AsString();
               string kindId = dtProd.Rows[w]["ai2_kind_id"].AsString();
               int rowTmp = rowNum - 1;

               ws30418.Cells[rowTmp , 0].Value = w + 1;
               ws30418.Cells[rowTmp , 1].Value = string.Format("{0}({1})" , apdkName , kindId);

            }//for (int w = 0 ; w < dtProd.Rows.Count ; w++)

            int colNum = 2;
            foreach (DataRow dr in dtYmd.Rows) {
               colNum++;
               int tmpCol = colNum - 1;
               string sDate = dr["startDate"].AsString().SubStr(0 , 10);
               string eDate = dr["endDate"].AsString().SubStr(0 , 10);

               ws30418.Cells[1 , tmpCol].Value = string.Format("{0}～{1}" , sDate + Environment.NewLine , Environment.NewLine + eDate);
            }//foreach (DataRow dr in dtTmp.Rows)

            DataTable dt = new D30417().ListData2(StartDate , EndDate);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
               return false;
            } //if (dt.Rows.Count <= 0 )

            int pos1 = 0;
            int pos2 = 0;
            foreach (DataRow drYmd in dtYmd.Rows) {
               pos1++;
               pos2 = 0;
               foreach (DataRow drProd in dtProd.Rows) {
                  pos2++;
                  //將週日期區間數量相加
                  string kindId = drProd["ai2_kind_id"].AsString();
                  string ai2Symd = drYmd["startDate"].AsString().Replace("/" , "");
                  string ai2Eymd = drYmd["endDate"].AsString().Replace("/" , "");
                  DataTable dtTmp = dt.Filter(string.Format("ai2_kind_id='{0}' and ai2_ymd>='{1}' and ai2_ymd<='{2}'" , kindId , ai2Symd , ai2Eymd));

                  if (dtTmp.Rows.Count > 0) {
                     decimal sumDayCount = dtTmp.Compute("Sum(ai2_day_count)" , "").AsDecimal();
                     decimal sumMQnty = dtTmp.Compute("Sum(ai2_m_qnty)" , "").AsDecimal();
                     decimal yAvgMQnty;
                     rowNum = pos2 + 1;
                     if (sumDayCount == 0) {
                        yAvgMQnty = 0;
                     } else {
                        yAvgMQnty = Math.Round(sumMQnty / sumDayCount , 0 , MidpointRounding.AwayFromZero);
                     }
                     ws30418.Cells[rowNum , pos1 + 1].Value = yAvgMQnty;
                  }//if (dtTmp.Rows.Count < 0)
               }//foreach (DataRow drProd in dtProd.Rows)
            }//foreach (DataRow drYmd in dtYmd.Rows)

            #endregion

            ws30418.Range["A1"].Select();
            ws30418.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }

      /// <summary>
      /// wf_30419 (sheet3 data)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="sheetNo">SheetNo.tradeSum</param>
      /// <param name="rowNum"> 2 </param>
      /// <returns></returns>
      protected bool wf_30419(Workbook workbook , SheetNo sheetNo , int rowNum , DataTable dtYmd) {

         string rptName = "股票期貨各標的每週日均未平倉量統計表"; //報表標題名稱
         labMsg.Text = "30419－" + rptName + " 轉檔中...";

         try {
            #region 30419
            //1. 表頭
            DataTable dtProd = new D30414().ListProdByMon(StartMon , EndMon);
            if (dtProd.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},30418 - 商品檔,無任何資料!" , StartMon , EndMon));
               return false;
            }//if (dtProd.Rows.Count <= 0)

            //2. 切換Sheet
            Worksheet ws30419 = workbook.Worksheets[(int)sheetNo];

            //3. 讀取資料           
            for (int w = 0 ; w < dtProd.Rows.Count ; w++) {
               rowNum++;
               string apdkName = dtProd.Rows[w]["apdk_name"].AsString();
               string kindId = dtProd.Rows[w]["ai2_kind_id"].AsString();
               int rowTmp = rowNum - 1;

               ws30419.Cells[rowTmp , 0].Value = w + 1;
               ws30419.Cells[rowTmp , 1].Value = string.Format("{0}({1})" , apdkName , kindId);

            }//for (int w = 0 ; w < dtProd.Rows.Count ; w++)

            int colNum = 2;
            foreach (DataRow dr in dtYmd.Rows) {
               colNum++;
               int tmpCol = colNum - 1;
               string sDate = dr["startDate"].AsString().SubStr(0 , 10);
               string eDate = dr["endDate"].AsString().SubStr(0 , 10);

               ws30419.Cells[1 , tmpCol].Value = string.Format("{0}~{1}" , sDate + Environment.NewLine , Environment.NewLine + eDate);
            }//foreach (DataRow dr in dtTmp.Rows)

            DataTable dt = new D30417().ListData2(StartDate , EndDate);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0}~{1},{2} - {3},無任何資料!" , StartDate , EndDate , _ProgramID , rptName));
               return false;
            } //if (dt.Rows.Count <= 0 )

            int pos1 = 0;
            int pos2 = 0;
            foreach (DataRow drYmd in dtYmd.Rows) {
               pos1++;
               pos2 = 0;
               foreach (DataRow drProd in dtProd.Rows) {
                  pos2++;
                  //將週日期區間數量相加
                  string kindId = drProd["ai2_kind_id"].AsString();
                  string ai2Symd = drYmd["startDate"].AsString().Replace("/" , "");
                  string ai2Eymd = drYmd["endDate"].AsString().Replace("/" , "");
                  DataTable dtTmp = dt.Filter(string.Format("ai2_kind_id='{0}' and ai2_ymd>='{1}' and ai2_ymd<='{2}'" , kindId , ai2Symd , ai2Eymd));

                  if (dtTmp.Rows.Count > 0) {
                     decimal sumDayCount = dtTmp.Compute("Sum(ai2_day_count)" , "").AsDecimal();
                     decimal sumOi = dtTmp.Compute("Sum(ai2_oi)" , "").AsDecimal();
                     decimal yAvgOi;
                     rowNum = pos2 + 1;
                     if (sumDayCount == 0) {
                        yAvgOi = 0;
                     } else {
                        yAvgOi = Math.Round(sumOi / sumDayCount , 0);
                     }
                     ws30419.Cells[rowNum , pos1 + 1].Value = yAvgOi;
                  }//if (dtTmp.Rows.Count < 0)
               }//foreach (DataRow drProd in dtProd.Rows)
            }//foreach (DataRow drYmd in dtYmd.Rows)

            #endregion

            ws30419.Range["A1"].Select();
            ws30419.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            return false;
            throw ex;
         }
      }
   }
}