using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// ken,2019/4/29
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// SPAN參數計算原始資料下載
   /// </summary>
   public partial class W40210 : FormParent {
      private D40210 dao40210;

      #region 抓取畫面值(主要用在縮寫)
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string StartDate {
         get {
            if (rdoReportType.SelectedIndex == 0) {
               return txtStartDate.DateTimeValue.ToString("yyyyMMdd");
            } else {
               //ken,往前抓N天,但是用奇怪算法去推算,明明系統就有工作日
               int li_year = txtEndDate2.DateTimeValue.Year - (int)Math.Truncate((decimal)TotalDayCount / 240);
               int li_month = txtEndDate2.DateTimeValue.Month - (int)Math.Ceiling((decimal)(TotalDayCount % 240) / 20);
               string temp = string.Format("{0}/{1}/{2}" , li_year , li_month , txtEndDate2.DateTimeValue.Day);

               DateTime.TryParseExact(temp , "yyyy/MM/dd" , null , System.Globalization.DateTimeStyles.AllowWhiteSpaces , out DateTime symd);
               return symd.ToString("yyyyMMdd");
            }
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string EndDate {
         get {
            if (rdoReportType.SelectedIndex == 0) {
               return txtEndDate.DateTimeValue.ToString("yyyyMMdd");
            } else {
               return txtEndDate2.DateTimeValue.ToString("yyyyMMdd");
            }
         }
      }

      /// <summary>
      /// TotalDayCount
      /// </summary>
      public int TotalDayCount {
         get {
            if (rdoReportType.SelectedIndex == 0) {
               return 999999;
            } else {
               int.TryParse(txtDays.Text , out int tempDays);
               return tempDays;
            }
         }
      }

      #endregion

      public W40210(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao40210 = new D40210();

         GridHelper.SetCommonGrid(gvExport);
         gvExport.OptionsBehavior.Editable = false;
         gvExport.OptionsBehavior.AutoPopulateColumns = true;
         gvExport.OptionsView.RowAutoHeight = true;

      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         txtEndDate2.DateTimeValue = txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtStartDate.DateTimeValue = DateTime.ParseExact(txtEndDate.DateTimeValue.ToString("yyyy/MM/01") , "yyyy/MM/dd" , null);
         txtDays.Text = "180";
         rdoReportType.SelectedIndex = 0;

#if DEBUG
         txtEndDate.Text = "2018/11/30";
         txtStartDate.Text = "2018/10/15";
         txtDays.Text = "180";
         rdoReportType.SelectedIndex = 0;
         this.Text += "(開啟測試模式)";
#endif


         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

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

            //0.檢查日期跟商品是否有選擇正確
            //todo check
            if (cbxProd.CheckedItemsCount < 1) {
               MessageDisplay.Warning("請勾選商品!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            }
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);
            labMsg.Visible = true;
            ShowMsg("訊息：資料轉出中........");
            //1.設定一些變數,把邏輯直接寫在該變數屬性內
            List<string> listCode = new List<string>();//多筆,用逗號分隔            
            if (cbxProd.Items[0].CheckState == CheckState.Checked) {
               listCode.Add("'1'");
            }
            if (cbxProd.Items[1].CheckState == CheckState.Checked) {
               listCode.Add("'5'");
            }

            DataTable dtTemp = new SPNT1().ListData();
            decimal chi_150 = dtTemp.Rows[0][0].AsDecimal();
            decimal chi_180 = dtTemp.Rows[0][1].AsDecimal();
            decimal v365 = dtTemp.Rows[0][2].AsDecimal();


            //2.開始轉出資料
            panFilter.Enabled = false;
            panProd.Enabled = false;
            //labMsg.Visible = true;
            //labMsg.Text = "訊息：資料轉出中........";
            //this.Refresh();

            //2.1 open xls
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID , "");
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //3.write sheet data
            if (cbxProd.Items[0].CheckState == CheckState.Checked
               || cbxProd.Items[1].CheckState == CheckState.Checked) {
               //3.1 現貨data
               wf_40210_3_old(workbook , StartDate , EndDate , listCode , TotalDayCount);

               //3.2 指數選擇權VSR
               wf_40210_4(workbook , StartDate , EndDate , listCode , chi_150 , chi_180 , v365 , TotalDayCount);

               //3.3 期貨data
               wf_40210_5(workbook , StartDate , EndDate , listCode , TotalDayCount);

               //3.4 期貨契約PSR
               wf_40210_6(workbook , EndDate , listCode);

               //3.5 Delta折耗比率
               wf_40210_7(workbook , StartDate , EndDate , listCode , TotalDayCount);

               //3.6 跨商品折抵比率
               wf_40210_8(workbook , EndDate , listCode);
            }//if (cbxProd.Items[0].CheckState || cbxProd.Items[1].CheckState)

            //3.7 STC VSR計算
            if (cbxProd.Items[2].CheckState == CheckState.Checked)
               wf_40210_1(workbook , "STC" , StartDate , EndDate , chi_150 , chi_180 , v365 , TotalDayCount);

            //3.8 ETC VSR計算
            if (cbxProd.Items[3].CheckState == CheckState.Checked)
               wf_40210_1(workbook , "ETC" , StartDate , EndDate , chi_150 , chi_180 , v365 , TotalDayCount);



            //存檔
            workbook.SaveDocument(excelDestinationPath);
            ShowMsg("轉檔完成");
            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);


            return ResultStatus.Success;
         } catch (Exception ex) {
            ShowMsg("轉檔錯誤");
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            panProd.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
            this.Refresh();
            Thread.Sleep(5);
         }
         return ResultStatus.Fail;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      /// <summary>
      /// 3.7 STC VSR計算
      /// 3.8 ETC VSR計算
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="kind">STC/ETC</param>
      /// <param name="is_symd">yyyyMMdd</param>
      /// <param name="is_eymd">yyyyMMdd</param>
      /// <param name="chi_150"></param>
      /// <param name="chi_180"></param>
      /// <param name="v365"></param>
      /// <param name="TotalDayCount"></param>
      /// <returns></returns>
      protected ResultStatus wf_40210_1(Workbook workbook , string kind , string is_symd , string is_eymd , decimal chi_150 , decimal chi_180 , decimal v365 , int TotalDayCount) {
         try {
            string reportId = "40210_1";
            string reportName = string.Format("{0} VSR計算" , kind);//ken,跟sheet name有關,不能亂改
            ShowMsg(reportId + '－' + reportName + " 轉檔中...");
            //1.get master dataTable
            DataTable dtTarget = dao40210.d_40210_1(is_eymd , kind);

            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取無任何資料!" , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //1.1 write caption
            Worksheet worksheet = workbook.Worksheets[reportName];

            worksheet.Cells[2 , 1].Value = chi_150;
            worksheet.Cells[3 , 1].Value = chi_180;
            worksheet.Cells[4 , 1].Value = v365;

            //1.2 write data
            foreach (DataRow dr in dtTarget.Rows) {
               int colIndexTemp = dr["rpt_col_num"].AsInt();
               worksheet.Cells[5 , colIndexTemp].SetValue(dr["SPNV1_150_STD"]);
               worksheet.Cells[6 , colIndexTemp].SetValue(dr["SPNV1_180_STD"]);
               worksheet.Cells[7 , colIndexTemp].SetValue(dr["SPNV2_150_RATE"]);
               worksheet.Cells[8 , colIndexTemp].SetValue(dr["SPNV2_180_RATE"]);
               worksheet.Cells[9 , colIndexTemp].SetValue(dr["SPNV2_CP_DAY_RATE"]);
            }//foreach(DataRow dr in dtTarget.Rows){


            //2.get detail dataTable
            DataTable dtDetail = dao40210.d_40210_1_detail(is_symd , is_eymd , kind);

            if (dtDetail.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}-{1},{2}－{3}明細,讀取無任何資料!" , is_symd , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.1 write data
            int rowBegin = (kind == "STC" ? 26 - 1 : 27 - 1);
            int rowIndex = 0, colIndex = 0;
            foreach (DataRow dr in dtDetail.Rows) {
               int rpt_row_num = dr["rpt_row_num"].AsInt();
               int rpt_col_num = dr["rpt_col_num"].AsInt();
               DateTime data_ymd = dr["data_ymd"].AsDateTime("yyyyMMdd");

               if (rpt_row_num > TotalDayCount) {
                  continue;
               }

               if (rowIndex != rpt_row_num) {
                  rowIndex = rpt_row_num;
                  worksheet.Cells[rowBegin + rowIndex , 0].Value = data_ymd.ToString("yyyy/MM/dd");
               }//if (rowIndex != rpt_row_num) {

               if (colIndex != rpt_col_num) {
                  colIndex = rpt_col_num;
                  if (rowIndex == 1) {
                     worksheet.Cells[rowBegin - 4 , colIndex].Value = colIndex;
                     worksheet.Cells[rowBegin - 3 , colIndex].SetValue(dr["data_kind_id"]);
                     worksheet.Cells[rowBegin - 2 , colIndex].SetValue(dr["data_sid"]);
                     worksheet.Cells[rowBegin - 1 , colIndex].SetValue(dr["data_pdk_name"]);
                     worksheet.Cells[rowBegin - 0 , colIndex].SetValue(dr["data_pid_name"]);
                  }
               }//if (colIndex != rpt_col_num) {

               worksheet.Cells[rowBegin + rowIndex , colIndex].SetValue(dr["return_rate"]);
            }//foreach(DataRow dr in dtDetail.Rows){


            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }


      /// <summary>
      /// 3.1 現貨data
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="is_symd">yyyyMMdd</param>
      /// <param name="is_eymd">yyyyMMdd</param>
      /// <param name="is_code"></param>
      /// <param name="TotalDayCount"></param>
      /// <returns></returns>
      protected ResultStatus wf_40210_3(Workbook workbook , string is_symd , string is_eymd , List<string> is_code , int TotalDayCount) {
         try {
            string reportId = "40210_3";
            string reportName = "現貨data";
            ShowMsg(reportId + '－' + reportName + " 轉檔中...");
            //1.get dataTable
            DataTable dtTarget = dao40210.d_40210_3(is_symd , is_eymd , is_code);

            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}-{1},{2}－{3},讀取無任何資料!" , is_symd , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            Worksheet worksheet = workbook.Worksheets[reportName];

            //1.1 write data
            worksheet.Import(dtTarget , false , 1 , 0);

            worksheet.ScrollTo(0 , 0);
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }


      /// <summary>
      /// 3.1 現貨data(廢棄)
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="is_symd">yyyyMMdd</param>
      /// <param name="is_eymd">yyyyMMdd</param>
      /// <param name="is_code"></param>
      /// <param name="TotalDayCount"></param>
      /// <returns></returns>
      protected ResultStatus wf_40210_3_old(Workbook workbook , string is_symd , string is_eymd , List<string> is_code , int TotalDayCount) {
         try {
            string reportId = "40210_3";
            string reportName = "現貨data";
            ShowMsg(reportId + '－' + reportName + " 轉檔中...");
            //1.get dataTable
            DataTable dtTarget = dao40210.d_40210_3_old(is_symd , is_eymd , is_code);

            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}-{1},{2}－{3},讀取無任何資料!" , is_symd , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            Worksheet worksheet = workbook.Worksheets[reportName];

            //1.1 write data
            int rowBegin = 0;
            int rowIndex = 0;
            foreach (DataRow dr in dtTarget.Rows) {
               int ocf_rownum = dr["ocf_rownum"].AsInt();
               int col_num = dr["col_num"].AsInt();
               DateTime ocf_ymd = dr["ocf_ymd"].AsDateTime("yyyyMMdd");

               if (ocf_rownum > TotalDayCount) {
                  continue;
               }

               if (rowIndex != ocf_rownum) {
                  rowIndex = ocf_rownum;
                  worksheet.Cells[rowBegin + rowIndex , 0].Value = ocf_ymd.ToString("yyyy/MM/dd");
               }//if (rowIndex != rpt_row_num) {

               worksheet.Cells[rowBegin + rowIndex , col_num - 1].SetValue(dr["mgr1_close_price"]);
            }//foreach(DataRow dr in dtTarget.Rows){

            worksheet.ScrollTo(0 , 0);
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }


      /// <summary>
      /// 3.2 指數選擇權VSR
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="is_symd">yyyyMMdd</param>
      /// <param name="is_eymd">yyyyMMdd</param>
      /// <param name="is_code"></param>
      /// <param name="chi_150"></param>
      /// <param name="chi_180"></param>
      /// <param name="v365"></param>
      /// <param name="TotalDayCount"></param>
      /// <returns></returns>
      protected ResultStatus wf_40210_4(Workbook workbook , string is_symd , string is_eymd , List<string> is_code , decimal chi_150 , decimal chi_180 , decimal v365 , int TotalDayCount) {
         try {
            string reportId = "40210_4";
            string reportName = "指數選擇權VSR";
            ShowMsg(reportId + '－' + reportName + " 轉檔中...");
            //1.get master dataTable
            DataTable dtTarget = dao40210.d_40210_4(is_eymd , is_code);

            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取無任何資料!" , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //1.1 write caption
            Worksheet worksheet = workbook.Worksheets[reportName];

            worksheet.Cells[1 , 1].Value = chi_150;
            worksheet.Cells[2 , 1].Value = chi_180;
            worksheet.Cells[3 , 1].Value = v365;

            //1.2 write data
            foreach (DataRow dr in dtTarget.Rows) {
               int colIndexTemp = dr["rpt_col_num"].AsInt() - 1;
               worksheet.Cells[4 , colIndexTemp].SetValue(dr["SPNV1_150_STD"]);
               worksheet.Cells[5 , colIndexTemp].SetValue(dr["SPNV1_180_STD"]);
               worksheet.Cells[6 , colIndexTemp].SetValue(dr["SPNV2_150_RATE"]);
               worksheet.Cells[7 , colIndexTemp].SetValue(dr["SPNV2_180_RATE"]);
               worksheet.Cells[8 , colIndexTemp].SetValue(dr["SPNV2_CP_DAY_RATE"]);
            }//foreach(DataRow dr in dtTarget.Rows){


            //2.get detail dataTable
            DataTable dtDetail = dao40210.d_40210_4_detail(is_symd , is_eymd , is_code);

            if (dtDetail.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}-{1},{2}－{3}明細,讀取無任何資料!" , is_symd , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.1 write data
            int rowBegin = 18 - 1;
            int rowIndex = 0, colIndex = 0;
            foreach (DataRow dr in dtDetail.Rows) {
               int rpt_row_num = dr["rpt_row_num"].AsInt();
               int rpt_col_num = dr["rpt_col_num"].AsInt();
               DateTime data_ymd = dr["data_ymd"].AsDateTime("yyyyMMdd");

               if (rpt_row_num > TotalDayCount) {
                  continue;
               }

               if (rowIndex != rpt_row_num) {
                  rowIndex = rpt_row_num;
                  worksheet.Cells[rowBegin + rowIndex , 0].Value = data_ymd.ToString("yyyy/MM/dd");
               }//if (rowIndex != rpt_row_num) {

               colIndex = dr["RPT_COL_NUM"].AsInt() - 1;

               worksheet.Cells[rowBegin + rowIndex , colIndex].SetValue(dr["return_rate"]);
            }//foreach(DataRow dr in dtDetail.Rows){

            worksheet.ScrollTo(0 , 0);
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      /// <summary>
      /// 3.3 期貨data
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="is_symd">yyyyMMdd</param>
      /// <param name="is_eymd">yyyyMMdd</param>
      /// <param name="is_code"></param>
      /// <param name="TotalDayCount"></param>
      /// <returns></returns>
      protected ResultStatus wf_40210_5(Workbook workbook , string is_symd , string is_eymd , List<string> is_code , int TotalDayCount) {
         try {
            string reportId = "40210_5";
            string reportName = "期貨data";
            ShowMsg(reportId + '－' + reportName + " 轉檔中...");
            //1.get dataTable
            DataTable dtTarget = dao40210.d_40210_5(is_symd , is_eymd , is_code);

            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}-{1},{2}－{3},讀取無任何資料!" , is_symd , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            Worksheet worksheet = workbook.Worksheets[reportName];

            //1.1 write data
            int rowBegin = 0;
            int rowIndex = 0;
            foreach (DataRow dr in dtTarget.Rows) {
               int ocf_rownum = dr["ocf_rownum"].AsInt();
               int col_1 = dr["col_1"].AsInt();
               int col_2 = dr["col_2"].AsInt();
               int col_3 = dr["col_3"].AsInt();
               DateTime ocf_ymd = dr["ocf_ymd"].AsDateTime("yyyyMMdd");

               if (ocf_rownum > TotalDayCount) {
                  continue;
               }

               if (rowIndex != ocf_rownum) {
                  rowIndex = ocf_rownum;
                  worksheet.Cells[rowBegin + rowIndex , 0].Value = ocf_ymd.ToString("yyyy/MM/dd");
               }//if (rowIndex != rpt_row_num) {

               worksheet.Cells[rowBegin + rowIndex , col_1 - 1].SetValue(dr["mgr1_close_price"]);
               worksheet.Cells[rowBegin + rowIndex , col_2 - 1].SetValue(dr["mgr1_open_ref"]);
               worksheet.Cells[rowBegin + rowIndex , col_3 - 1].SetValue(dr["up_down"]);
            }//foreach(DataRow dr in dtTarget.Rows){

            worksheet.ScrollTo(0 , 0);
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      /// <summary>
      /// 3.4 期貨契約PSR
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="is_eymd">yyyyMMdd</param>
      /// <param name="is_code"></param>
      /// <returns></returns>
      protected ResultStatus wf_40210_6(Workbook workbook , string is_eymd , List<string> is_code) {
         try {
            string reportId = "40210_6";
            string reportName = "期貨契約PSR";
            ShowMsg(reportId + '－' + reportName + " 轉檔中...");
            //1.get dataTable
            string as_osw_grp = string.Join("," , is_code.ToArray()).Replace("\"" , "");
            WriteLog(as_osw_grp , "info" , " " , false , true);
            DataTable dtTarget = dao40210.d_40210_6(is_eymd , is_code);

            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取無任何資料!" , is_eymd , reportId , reportName , GlobalInfo.ResultText));
               return ResultStatus.Fail;
            }

            Worksheet worksheet = workbook.Worksheets[reportName];

            //1.1 write data
            int rowIndex = 2;
            foreach (DataRow dr in dtTarget.Rows) {
               int rpt_col_num = dr["rpt_col_num"].AsInt();

               worksheet.Cells[rowIndex , rpt_col_num].SetValue(dr["mgr4_cm"]);
            }//foreach(DataRow dr in dtTarget.Rows){

            worksheet.ScrollTo(0 , 0);
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      /// <summary>
      /// 3.5 Delta折耗比率
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="is_symd">yyyyMMdd</param>
      /// <param name="is_eymd">yyyyMMdd</param>
      /// <param name="is_code"></param>
      /// <param name="TotalDayCount"></param>
      /// <returns></returns>
      protected ResultStatus wf_40210_7(Workbook workbook , string is_symd , string is_eymd , List<string> is_code , int TotalDayCount) {
         try {
            string reportId = "40210_7";
            string reportName = "Delta折耗比率";
            ShowMsg(reportId + '－' + reportName + " 轉檔中...");
            //1.get master dataTable
            DataTable dtTarget = dao40210.d_40210_7(is_eymd , is_code);

            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取無任何資料!" , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //1.1 write caption
            Worksheet worksheet = workbook.Worksheets[reportName];

            //1.2 write data
            foreach (DataRow dr in dtTarget.Rows) {
               int rpt_row_num = dr["rpt_row_num"].AsInt();
               int colIndex = dr["rpt_col_num"].AsInt();
               worksheet.Cells[rpt_row_num - 1 , colIndex - 1].SetValue(dr["sp1_rate"]);
            }//foreach(DataRow dr in dtTarget.Rows){


            //2.get detail dataTable
            DataTable dtDetail = dao40210.d_40210_7_detail(is_symd , is_eymd , is_code);

            if (dtDetail.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}-{1},{2}－{3}明細,讀取無任何資料!" , is_symd , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.1 write data
            int rowIndex = 2;
            DateTime ls_ymd = DateTime.MinValue;
            foreach (DataRow dr in dtDetail.Rows) {
               int colIndex = dr["rpt_col_num"].AsInt();
               DateTime spnd_ymd = dr["spnd_ymd"].AsDateTime("yyyyMMdd");

               if (ls_ymd != spnd_ymd && rowIndex + 1 <= TotalDayCount + 2) {
                  ls_ymd = spnd_ymd;
                  rowIndex++;
                  worksheet.Cells[rowIndex - 1 , 0].Value = spnd_ymd.ToString("yyyy/MM/dd");
               }

               worksheet.Cells[rowIndex - 1 , colIndex - 1].SetValue(dr["spnd_t_val"]);
            }//foreach(DataRow dr in dtDetail.Rows){

            worksheet.ScrollTo(0 , 0);
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      /// <summary>
      /// 3.6 跨商品折抵比率
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="is_eymd">yyyyMMdd</param>
      /// <param name="is_code"></param>
      /// <returns></returns>
      protected ResultStatus wf_40210_8(Workbook workbook , string is_eymd , List<string> is_code) {
         try {
            string reportId = "40210_8";
            string reportName = "跨商品折抵比率";
            ShowMsg(reportId + '－' + reportName + " 轉檔中...");
            //1.get master dataTable
            DataTable dtTarget = dao40210.d_40210_8(is_eymd , is_code);

            if (dtTarget.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取無任何資料!" , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //1.1 write caption
            Worksheet worksheet = workbook.Worksheets[reportName];

            //欄位(B1, B5) value=T150 T180
            DataTable dtTemp = new SPNT1().ListData2();
            worksheet.Cells[0 , 1].SetValue(dtTemp.Rows[0]["tinv_150"]);
            worksheet.Cells[4 , 1].SetValue(dtTemp.Rows[0]["tinv_180"]);


            //1.2 write data
            foreach (DataRow dr in dtTarget.Rows) {
               int rpt_col_num = dr["rpt_col_num"].AsInt() - 1;
               worksheet.Cells[1 , rpt_col_num].SetValue(dr["spns1_150_avg_val"]);
               worksheet.Cells[2 , rpt_col_num].SetValue(dr["spns1_150_std"]);
               worksheet.Cells[3 , rpt_col_num].SetValue(dr["spns2_150_rate"]);

               worksheet.Cells[5 , rpt_col_num].SetValue(dr["spns1_180_avg_val"]);
               worksheet.Cells[6 , rpt_col_num].SetValue(dr["spns1_180_std"]);
               worksheet.Cells[7 , rpt_col_num].SetValue(dr["spns2_180_rate"]);

               worksheet.Cells[8 , rpt_col_num].SetValue(dr["spns2_max_rate"]);


               int col = dr["col"].AsInt();
               int row_1 = dr["row_1"].AsInt();
               decimal spns2_cp_day_rate = Math.Truncate(dr["spns2_cp_day_rate"].AsDecimal() * 100) / 100;//ken,要把小數點第二位直接去掉,不能用Round
               worksheet.Cells[row_1 - 1 , col - 1].Value = spns2_cp_day_rate;

               int row_2 = dr["row_2"].AsInt();
               decimal spns2_day_rate = Math.Truncate(dr["spns2_day_rate"].AsDecimal() * 100) / 100;//ken,要把小數點第二位直接去掉,不能用Round
               worksheet.Cells[row_2 - 1 , col - 1].Value = spns2_day_rate;

            }//foreach(DataRow dr in dtTarget.Rows){


            //2.get detail dataTable
            DataTable dtDetail = dao40210.d_40210_8_detail(is_eymd , is_code);

            if (dtDetail.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2}明細,讀取無任何資料!" , is_eymd , reportId , reportName) , GlobalInfo.ResultText);
               return ResultStatus.Fail;
            }

            //2.1 write data
            int rowIndex = 20 - 1;
            DateTime ls_ymd = DateTime.MinValue;
            foreach (DataRow dr in dtDetail.Rows) {
               int colIndex = dr["rpt_col_num"].AsInt();
               DateTime spns1d_detial_ymd = dr["spns1d_detial_ymd"].AsDateTime("yyyyMMdd");

               if (ls_ymd != spns1d_detial_ymd) {
                  ls_ymd = spns1d_detial_ymd;
                  rowIndex++;
                  worksheet.Cells[rowIndex , 0].SetValue(spns1d_detial_ymd.ToString("yyyy/MM/dd"));
               }

               worksheet.Cells[rowIndex , colIndex - 1].SetValue(dr["spns1d_t_val"]);
            }//foreach(DataRow dr in dtDetail.Rows){

            worksheet.ScrollTo(0 , 0);
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }
   }
}