using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

/// <summary>
/// ken,2019/1/24
/// 1.有改PB原本的範本檔[三]的sheet
/// TODO 每月日均量d_30690_mth_qnty_day_night SQL要請期交所優化才行
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 本週交易量分析
    /// </summary>
    public partial class W30690 : FormParent {
        private D30690 dao30690;
        protected bool flagTest = true;
        protected const string NoData = "無任何資料!";
        protected const string FinishPrefix = "完成,共";
        protected const string FinishSuffix = "筆";
        protected DataTable dtMsg;

        #region 一般交易查詢條件縮寫
        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string LastStart {
            get {
                return txtLastStartDate.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string LastEnd {
            get {
                return txtLastEndDate.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string ThisStart {
            get {
                return txtThisStartDate.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string ThisEnd {
            get {
                return txtThisEndDate.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string RateStart {
            get {
                return txtRateStartDate.DateTimeValue.ToString("yyyyMMdd");
            }
        }
        #endregion

        #region 盤後交易查詢條件縮寫
        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string LastStartN {
            get {
                return txtLastStartDateN.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string LastEndN {
            get {
                return txtLastEndDateN.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string ThisStartN {
            get {
                return txtThisStartDateN.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string ThisEndN {
            get {
                return txtThisEndDateN.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public string RateStartN {
            get {
                return txtRateStartDateN.DateTimeValue.ToString("yyyyMMdd");
            }
        }
        #endregion

        #region 平均OI 查詢條件縮寫
        /// <summary>
        /// 日盤平均OI_StartDate yyyyMMdd
        /// </summary>
        public string DiffStart {
            get {
                return txtDiffStartDate.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// 日盤平均OI_EndDate yyyyMMdd
        /// </summary>
        public string DiffEnd {
            get {
                return txtDiffEndDate.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// 夜盤平均OI_StartDate yyyyMMdd
        /// </summary>
        public string DiffStartN {
            get {
                return txtDiffStartDateN.DateTimeValue.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// 夜盤平均OI_EndDate yyyyMMdd
        /// </summary>
        public string DiffEndN {
            get {
                return txtDiffEndDateN.DateTimeValue.ToString("yyyyMMdd");
            }
        }
        #endregion

        

        public W30690(string programID, string programName) : base(programID, programName) {
            try {
                InitializeComponent();

                this.Text = _ProgramID + "─" + _ProgramName;
                txtLastStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
                txtLastEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
                GridHelper.SetCommonGrid(gvMain);

                GridHelper.SetCommonGrid(gvMsg);
                gcMsg.Visible = false;
                gvMsg.OptionsBehavior.Editable = false;
                gvMsg.OptionsBehavior.AutoPopulateColumns = true;
                gvMsg.OptionsView.ColumnAutoWidth = true;

                dtMsg = new DataTable("ProcessMessage");
                dtMsg.Columns.Add(new DataColumn("SheetName"));
                dtMsg.Columns.Add(new DataColumn("SheetSubTitle"));
                dtMsg.Columns.Add(new DataColumn("SubMsg"));
                dtMsg.Columns.Add(new DataColumn("Msg"));


                dao30690 = new D30690();
            } catch (Exception ex) {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// 視窗啟動時,設定一些UI元件初始值
        /// </summary>
        /// <returns></returns>
        protected override ResultStatus Open() {
            try {
                base.Open();

                //1.設定初始年月yyyy/MM
                //1.1取得資料庫內最大日期
                DateTime ocfDate = GlobalInfo.OCF_DATE;

                DateTime ls_ymd = DateTime.Now.AddDays(-30);
                DateTime aocfMaxDate = new AOCF().GetMaxDate(ls_ymd.ToString("yyyyMMdd"), "");
                if (aocfMaxDate != null) {
                    ls_ymd = aocfMaxDate.AddDays(-1);
                    ocfDate = ls_ymd;
                }


#if DEBUG
                //ken test
                ocfDate = DateTime.ParseExact("2018/11/16", "yyyy/MM/dd", null);
                this.Text += "(開啟測試模式),ocfDate=2018/11/16";
#endif


                //1.2一般交易
                txtThisEndDate.Text = ocfDate.ToString("yyyy/MM/dd");//本週
                txtThisStartDate.Text = ocfDate.AddDays(-6).ToString("yyyy/MM/dd");//本週
                txtLastEndDate.Text = ocfDate.AddDays(-7).ToString("yyyy/MM/dd");//上週
                txtLastStartDate.Text = ocfDate.AddDays(-13).ToString("yyyy/MM/dd");//上週
                txtRateStartDate.Text = ocfDate.ToString("yyyy/01/01"); //夜盤占日盤交易量比重起日

                //1.3盤後交易
                txtLastEndDateN.Text = txtLastEndDate.Text;
                txtLastStartDateN.Text = txtLastStartDate.Text;
                txtThisEndDateN.Text = txtThisEndDate.Text;
                txtThisStartDateN.Text = txtThisStartDate.Text;
                txtRateStartDateN.Text = txtRateStartDate.Text;

                //1.4夜盤與日盤平均OI差異
                //日盤起日
                DateTime aocfMinDate = new AOCF().GetMinDate(ocfDate.ToString("yyyy0101"), ">=");
                txtDiffStartDate.Text = aocfMinDate.ToString("yyyy/MM/dd");
                //夜盤起日 = 日盤起日 + 1
                aocfMinDate = new AOCF().GetMinDate(aocfMinDate.ToString("yyyyMMdd"), ">");
                txtDiffStartDateN.Text = aocfMinDate.ToString("yyyy/MM/dd");
                //夜盤迄日
                txtDiffEndDateN.Text = txtThisEndDate.Text;
                //日盤迄日 = 夜盤迄日 - 1
                aocfMaxDate = new AOCF().GetMaxDate("", ocfDate.ToString("yyyyMMdd"));
                txtDiffEndDate.Text = aocfMaxDate.ToString("yyyy/MM/dd");



                //2.設定Grid Data
                DataTable dt = new APDK().ListAll4();//CPR_SELECT / P_KIND_ID2 / P_NAME
                gcMain.DataSource = dt;

#if DEBUG
                //ken test
                gvMain.SetRowCellValue(0, "CPR_SELECT", "Y");
                gvMain.SetRowCellValue(2, "CPR_SELECT", "Y");
                gvMain.SetRowCellValue(4, "CPR_SELECT", "Y");
#endif

                //3.setup test button
                if (FlagAdmin) {
                    btnTest.Visible = true;
                }

                return ResultStatus.Success;
            } catch (Exception ex) {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }

        /// <summary>
        /// 設定此功能哪些按鈕可以按
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 按下[匯出]按鈕時
        /// </summary>
        /// <returns></returns>
        protected override ResultStatus Export() {

            #region //1.檢查
            //ken,如果Mark設定為yyyy/MM/dd,則會強制使用者只能輸入正確的日期格式,這邊不需要多做該欄位檢核
            //txtLastStartDate.DoValidate();
            //txtLastEndDate.DoValidate();
            //txtThisStartDate.DoValidate();
            //txtThisEndDate.DoValidate();
            //txtRateStartDate.DoValidate();

            if (txtLastStartDate.DateTimeValue > txtLastEndDate.DateTimeValue) {
                MessageDisplay.Error(string.Format("{0}起日期({1})不可大於迄日期({2})", labLastWeek.Text, LastStart, LastEnd));
                return ResultStatus.Fail;
            }

            if (txtThisStartDate.DateTimeValue > txtThisEndDate.DateTimeValue) {
                MessageDisplay.Error(string.Format("{0}起日期({1})不可大於迄日期({2})", labThisWeek.Text, ThisStart, ThisEnd));
                return ResultStatus.Fail;
            }

            if (txtLastStartDateN.DateTimeValue > txtLastEndDateN.DateTimeValue) {
                MessageDisplay.Error(string.Format("{0}起日期({1})不可大於迄日期({2})", labLastWeekN.Text, LastStartN, LastEndN));
                return ResultStatus.Fail;
            }

            if (txtThisStartDateN.DateTimeValue > txtThisEndDateN.DateTimeValue) {
                MessageDisplay.Error(string.Format("{0}起日期({1})不可大於迄日期({2})", labThisWeekN.Text, ThisStartN, ThisEndN));
                return ResultStatus.Fail;
            }
            #endregion

            string flowStepDesc = "2.開始轉出資料";
            try {
                //2.開始轉出資料
                panFilter.Enabled = panProd.Enabled = panDiff.Enabled = false;
                gcMsg.DataSource = null;
                gcMsg.Refresh();
                gcMsg.Visible = true;
                labMsg.Visible = true;
                labMsg.Text = "訊息：資料轉出中........";
                this.Refresh();

                //2.1 copy template xls to target path
                string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);//CopyExcelTemplateFile(_ProgramID, FileType.XLS);

                //2.2 open xls
                Workbook workbook = new Workbook();
                workbook.LoadDocument(excelDestinationPath);
                Worksheet worksheet = workbook.Worksheets[0];

                //2.3寫入檔頭
                worksheet.Cells[0, 2].Value = txtLastStartDate.Text;
                worksheet.Cells[0, 3].Value = txtLastEndDate.Text;
                worksheet.Cells[1, 2].Value = txtThisStartDate.Text;
                worksheet.Cells[1, 3].Value = txtThisEndDate.Text;

                //3.寫入資料 
                ////3.1一般成交量
                //flowStepDesc = "3.1一般成交量";
                //wf_30690_day_avg_qnty1(workbook, LastStart, LastEnd, ThisStart, ThisEnd, RateStart, 0);
                //wf_30690_day_avg_qnty2(workbook, RateStart, ThisEnd, 0);

                ////3.2盤後成交量
                //flowStepDesc = "3.2盤後成交量";
                //wf_30690_night_avg_qnty1(workbook, LastStartN, LastEndN, ThisStartN, ThisEndN, RateStartN, 0);
                //wf_30690_night_avg_qnty2(workbook, RateStartN, ThisEndN, 0);
                //wf_30690_ah_oi(workbook, DiffStart, DiffEnd, DiffStartN, DiffEndN, 0);

                ////3.3ETF成交量
                //flowStepDesc = "3.3ETF成交量";
                //wf_30690_etf(workbook, LastStart, LastEnd, ThisStart, ThisEnd, RateStart, 0);

                //3.4每月日均量
                flowStepDesc = "3.4每月日均量";
                wf_30690_mth_qnty(workbook, txtLastStartDate.DateTimeValue.ToString("yyyy0101"), txtThisEndDate.DateTimeValue.AddYears(-1).ToString("yyyy0101"), ThisStart, ThisEnd);

                //3.5每月波動及振幅(現貨)
                flowStepDesc = "3.5每月波動及振幅(現貨)";
                wf_30690_high_low(workbook, txtThisEndDate.DateTimeValue.AddYears(-1).ToString("yyyy0101"), ThisEnd, "T");

                //3.6每月波動及振幅(期貨)
                flowStepDesc = "3.6每月波動及振幅(期貨)";
                wf_30690_high_low(workbook, txtThisEndDate.DateTimeValue.AddYears(-1).ToString("yyyy0101"), ThisEnd, "F");

                //3.7交易人結構
                flowStepDesc = "3.7交易人結構";
                wf_30690_day_am21(workbook, LastStart, LastEnd, ThisStart, ThisEnd, txtThisEndDate.DateTimeValue);

                //3.8盤後交易人結構
                flowStepDesc = "3.8盤後交易人結構";
                wf_30690_night_am21(workbook, LastStartN, LastEndN, ThisStartN, ThisEndN, txtThisEndDateN.DateTimeValue);

                //3.9TX波動及振福
                flowStepDesc = "3.9TX波動及振福";
                wf_30690_tx_high_low(workbook, LastStart, LastEnd, ThisStart, ThisEnd);

                //3.10其它--加掛小型股票期貨交易概況(日均量)
                flowStepDesc = "3.10其它--加掛小型股票期貨交易概況(日均量)";
                wf_30690_stf_m(workbook, LastStart, LastEnd, ThisStart, ThisEnd);

                //3.11其它--新上市個股選擇權交易概況(日均量)
                flowStepDesc = "3.11其它--新上市個股選擇權交易概況(日均量)";
                List<string> prodList = new List<string>();
                for (int k = 0;k < gvMain.RowCount;k++) {
                    //gvMain column name = CPR_SELECT / P_KIND_ID2 / P_NAME
                    if (gvMain.GetRowCellValue(k, "CPR_SELECT").AsString() == "Y") {
                        prodList.Add(gvMain.GetRowCellValue(k, "P_KIND_ID2").AsString());
                    }
                }
                if (prodList.Count > 0) {
                    wf_30690_new_stc(workbook, LastStart, LastEnd, ThisStart, ThisEnd, prodList);
                }

                //3.12二--歐臺期及歐臺選日均量
                flowStepDesc = "3.12二--歐臺期及歐臺選日均量";
                wf_30690_sheet2(workbook, LastStart, LastEnd, ThisStart, ThisEnd);

                //3.13三--人民幣匯率期貨及選擇權交易量
                flowStepDesc = "3.13三--人民幣匯率期貨及選擇權交易量";
                wf_30690_third(workbook, ThisEnd);

                Worksheet ws = workbook.Worksheets[0];
                ws.Range["A1"].Select();
                ws.ScrollToRow(0);

                //存檔
                workbook.SaveDocument(excelDestinationPath);

                if (FlagAdmin)
                    System.Diagnostics.Process.Start(excelDestinationPath);

                return ResultStatus.Success;
            } catch (Exception ex) {
                WriteLog(ex, flowStepDesc);
            } finally {
                panFilter.Enabled = panProd.Enabled = panDiff.Enabled = true;
                labMsg.Text = "";
                labMsg.Visible = false;
            }
            return ResultStatus.Fail;
        }



        /// <summary>
        /// 3.1.1一般成交量(一般)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        /// <param name="em_rate_ymd">yyyyMMdd</param>
        /// <param name="emptyRowCount">excel範例保留的資料總行數,當其值大於dt.Rows.Count則會把多餘的行數刪除,當其值為0=不刪除空白行</param>
        protected void wf_30690_day_avg_qnty1(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd, string em_rate_ymd, int emptyRowCount = 0) {

            string sheetName = "一般成交量";
            string sheetSubTitle = "一般";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            ws.Cells[1, 2].Value = em_aft_fm_ymd;
            ws.Cells[1, 3].Value = em_aft_to_ymd;
            ws.Cells[2, 2].Value = em_prev_fm_ymd;
            ws.Cells[2, 3].Value = em_prev_to_ymd;
            ws.Cells[3, 2].Value = em_rate_ymd;
            ws.Cells[3, 3].Value = em_aft_to_ymd;

            DataTable dt = dao30690.d_30690_day_avg_qnty1(em_prev_fm_ymd, em_prev_to_ymd, em_aft_fm_ymd, em_aft_to_ymd);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, NoData);
                return;
            }

            //寫入明細
            int rowIndex = ws.Cells[1, 5].Value.AsInt() == 0 ? 19 : ws.Cells[1, 5].Value.AsInt();//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int pos = 0;//tmp

            foreach (DataRow dr in dt.Rows) {
                if (pos % 20 == 0) {
                    labMsg.Text = string.Format("{0} 資料轉出中......{1} / {2}", sheetName, pos, dt.Rows.Count);
                    this.Refresh();
                }

                ws.Cells[rowIndex, 0].Value = dr["prod_type"].AsString();
                ws.Cells[rowIndex, 1].Value = dr["param_key"].AsString();
                ws.Cells[rowIndex, 2].Value = dr["kind_id2"].AsString();

                ws.Cells[rowIndex, 3].Value = dr["week_qnty"].AsDecimal();
                //這邊拿到的值如果為DBNull.Value,則輸出也要為null,不能直接轉成0 (不能用AsDecimal)
                if (dr["week_ah_qnty"] != DBNull.Value)
                    ws.Cells[rowIndex, 4].Value = dr["week_ah_qnty"].AsDecimal();
                ws.Cells[rowIndex, 5].Value = dr["week_oi"].AsDecimal();
                ws.Cells[rowIndex, 6].Value = dr["week_days"].AsDecimal();

                ws.Cells[rowIndex, 11].Value = dr["last_qnty"].AsDecimal();
                //這邊拿到的值如果為DBNull.Value,則輸出也要為null,不能直接轉成0 (不能用AsDecimal)
                if (dr["last_ah_qnty"] != DBNull.Value)
                    ws.Cells[rowIndex, 12].Value = dr["last_ah_qnty"].AsDecimal();
                ws.Cells[rowIndex, 13].Value = dr["last_oi"].AsDecimal();
                ws.Cells[rowIndex, 14].Value = dr["last_days"].AsDecimal();

                ws.Cells[rowIndex, 19].Value = dr["year_qnty"].AsDecimal();
                //這邊拿到的值如果為DBNull.Value,則輸出也要為null,不能直接轉成0 (不能用AsDecimal)
                if (dr["year_ah_qnty"] != DBNull.Value)
                    ws.Cells[rowIndex, 20].Value = dr["year_ah_qnty"].AsDecimal();
                ws.Cells[rowIndex, 21].Value = dr["year_oi"].AsDecimal();
                ws.Cells[rowIndex, 22].Value = dr["year_days"].AsDecimal();

                rowIndex++; pos++;
            }//foreach (DataRow row in dt.Rows) {

            //刪除空白列
            if (dt.Rows.Count < emptyRowCount) {
                ws.Rows.Remove(rowIndex, emptyRowCount - dt.Rows.Count);
            }

            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.1.2一般成交量(夜盤上線迄今)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_rate_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        /// <param name="emptyRowCount">excel範例保留的資料總行數,當其值大於dt.Rows.Count則會把多餘的行數刪除,當其值為0=不刪除空白行</param>
        protected void wf_30690_day_avg_qnty2(Workbook workbook, string em_rate_ymd, string em_aft_to_ymd, int emptyRowCount = 0) {

            string sheetName = "一般成交量";
            string sheetSubTitle = "夜盤上線迄今";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            DataTable dt = dao30690.d_30690_day_avg_qnty2(em_rate_ymd, em_aft_to_ymd);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, em_rate_ymd + "~" + em_aft_to_ymd, NoData);
                return;
            }

            //寫入明細
            int rowIndex = ws.Cells[3, 5].Value.AsInt() == 0 ? 135 : ws.Cells[3, 5].Value.AsInt();//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int colIndex = 0;
            bool showHeader = false;

            //DevExpress.Spreadsheet有特別做擴充方法,一次把整個DataTable都輸出,效率很高
            //但是使用此方法請注意
            //1.sql欄位和順序必須一致
            //2.每個欄位的資料型態只能從excel template做設定,輸出之後要再做檢查
            ws.Import(dt, showHeader, rowIndex, colIndex);//dataTable, isAddHeader, RowFirstIndex, ColFirstIndex

            //刪除空白列
            if (dt.Rows.Count < emptyRowCount) {
                ws.Rows.Remove(rowIndex, emptyRowCount - dt.Rows.Count);
            }

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.2.1盤後成交量(一般)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        /// <param name="em_rate_ymd">yyyyMMdd</param>
        /// <param name="emptyRowCount">excel範例保留的資料總行數,當其值大於dt.Rows.Count則會把多餘的行數刪除,當其值為0=不刪除空白行</param>
        protected void wf_30690_night_avg_qnty1(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd, string em_rate_ymd, int emptyRowCount = 0) {

            string sheetName = "盤後成交量";
            string sheetSubTitle = "一般";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            ws.Cells[1, 2].Value = em_aft_fm_ymd;
            ws.Cells[1, 3].Value = em_aft_to_ymd;
            ws.Cells[2, 2].Value = em_prev_fm_ymd;
            ws.Cells[2, 3].Value = em_prev_to_ymd;
            ws.Cells[3, 2].Value = em_rate_ymd;
            ws.Cells[3, 3].Value = em_aft_to_ymd;

            DataTable dt = dao30690.d_30690_night_avg_qnty1(em_prev_fm_ymd, em_prev_to_ymd, em_aft_fm_ymd, em_aft_to_ymd);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, NoData);
                return;
            }

            //寫入明細
            int rowIndex = ws.Cells[1, 5].Value.AsInt() == 0 ? 19 : ws.Cells[1, 5].Value.AsInt();//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int pos = 0;//tmp

            foreach (DataRow dr in dt.Rows) {
                if (pos % 20 == 0) {
                    labMsg.Text = string.Format("{0} 資料轉出中......{1} / {2}", sheetName, pos, dt.Rows.Count);
                    this.Refresh();
                }

                ws.Cells[rowIndex, 0].Value = dr["prod_type"].AsString();
                ws.Cells[rowIndex, 1].Value = dr["param_key"].AsString();
                ws.Cells[rowIndex, 2].Value = dr["kind_id2"].AsString();

                ws.Cells[rowIndex, 3].Value = dr["week_qnty"].AsDecimal();
                ws.Cells[rowIndex, 4].Value = dr["week_oi"].AsDecimal();
                ws.Cells[rowIndex, 5].Value = dr["week_days"].AsDecimal();

                ws.Cells[rowIndex, 10].Value = dr["last_qnty"].AsDecimal();
                ws.Cells[rowIndex, 11].Value = dr["last_oi"].AsDecimal();
                ws.Cells[rowIndex, 12].Value = dr["last_days"].AsDecimal();

                ws.Cells[rowIndex, 17].Value = dr["year_qnty"].AsDecimal();
                ws.Cells[rowIndex, 18].Value = dr["year_oi"].AsDecimal();
                ws.Cells[rowIndex, 19].Value = dr["year_days"].AsDecimal();

                rowIndex++; pos++;
            }//foreach (DataRow row in dt.Rows) {

            //刪除空白列
            if (dt.Rows.Count < emptyRowCount) {
                ws.Rows.Remove(rowIndex, emptyRowCount - dt.Rows.Count);
            }

            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.2.2盤後成交量(夜盤上線迄今)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_rate_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        /// <param name="emptyRowCount">excel範例保留的資料總行數,當其值大於dt.Rows.Count則會把多餘的行數刪除,當其值為0=不刪除空白行</param>
        protected void wf_30690_night_avg_qnty2(Workbook workbook, string em_rate_ymd, string em_aft_to_ymd, int emptyRowCount = 0) {

            string sheetName = "盤後成交量";
            string sheetSubTitle = "夜盤上線迄今";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            DataTable dt = dao30690.d_30690_night_avg_qnty2(em_rate_ymd, em_aft_to_ymd);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, em_rate_ymd + "~" + em_aft_to_ymd, NoData);
                return;
            }

            //寫入明細
            int rowIndex = ws.Cells[3, 5].Value.AsInt() == 0 ? 135 : ws.Cells[3, 5].Value.AsInt();//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int pos = 0;//tmp

            foreach (DataRow dr in dt.Rows) {
                if (pos % 20 == 0) {
                    labMsg.Text = string.Format("{0} 資料轉出中......{1} / {2}", sheetName, pos, dt.Rows.Count);
                    this.Refresh();
                }

                ws.Cells[rowIndex, 0].Value = dr["prod_type"].AsString();
                ws.Cells[rowIndex, 1].Value = dr["param_key"].AsString();
                ws.Cells[rowIndex, 2].Value = dr["kind_id2"].AsString();

                ws.Cells[rowIndex, 3].Value = dr["week_qnty"].AsDecimal();
                ws.Cells[rowIndex, 4].Value = dr["week_oi"].AsDecimal();
                ws.Cells[rowIndex, 5].Value = dr["week_days"].AsDecimal();

                ws.Cells[rowIndex, 10].Value = dr["week_dt_qnty"].AsDecimal();

                rowIndex++; pos++;
            }//foreach (DataRow row in dt.Rows) {

            //刪除空白列
            if (dt.Rows.Count < emptyRowCount) {
                ws.Rows.Remove(rowIndex, emptyRowCount - dt.Rows.Count);
            }

            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.2.3盤後成交量(平均OI)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="as_d_symd">日盤平均OI_StartDate yyyyMMdd</param>
        /// <param name="as_d_eymd">日盤平均OI_EndDate yyyyMMdd</param>
        /// <param name="as_ah_symd">夜盤平均OI_StartDate yyyyMMdd</param>
        /// <param name="as_ah_eymd">夜盤平均OI_EndDate yyyyMMdd</param>
        /// <param name="emptyRowCount">excel範例保留的資料總行數,當其值大於dt.Rows.Count則會把多餘的行數刪除,當其值為0=不刪除空白行</param>
        protected void wf_30690_ah_oi(Workbook workbook, string as_d_symd, string as_d_eymd, string as_ah_symd, string as_ah_eymd, int emptyRowCount = 0) {

            string sheetName = "盤後成交量";
            string sheetSubTitle = "平均OI";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            ws.Cells[238, 4].Value = as_d_symd + " - " + as_d_eymd;//日盤
            ws.Cells[237, 4].Value = as_ah_symd + " - " + as_ah_eymd;//夜盤

            DataTable dt = dao30690.d_30690_ah_oi(as_d_symd, as_d_eymd, as_ah_symd, as_ah_eymd);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, NoData);
                return;
            }

            //寫入明細
            int rowIndex = 252;//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int pos = 0;//tmp

            foreach (DataRow dr in dt.Rows) {
                if (pos % 20 == 0) {
                    labMsg.Text = string.Format("{0} 資料轉出中......{1} / {2}", sheetName, pos, dt.Rows.Count);
                    this.Refresh();
                }

                ws.Cells[rowIndex, 0].Value = dr["prod_type"].AsString();
                ws.Cells[rowIndex, 1].Value = dr["param_key"].AsString();
                ws.Cells[rowIndex, 2].Value = dr["param_key"].AsString();

                ws.Cells[rowIndex, 3].Value = dr["ah_oi"].AsDecimal();
                ws.Cells[rowIndex, 4].Value = dr["ah_days"].AsDecimal();

                ws.Cells[rowIndex, 6].Value = dr["d_oi"].AsDecimal();
                ws.Cells[rowIndex, 7].Value = dr["d_days"].AsDecimal();

                rowIndex++; pos++;
            }//foreach (DataRow row in dt.Rows) {

            //刪除空白列
            if (dt.Rows.Count < emptyRowCount) {
                ws.Rows.Remove(rowIndex, emptyRowCount - dt.Rows.Count);
            }

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.3ETF成交量
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        /// <param name="em_rate_ymd">yyyyMMdd</param>
        /// <param name="emptyRowCount">excel範例保留的資料總行數,當其值大於dt.Rows.Count則會把多餘的行數刪除,當其值為0=不刪除空白行</param>
        protected void wf_30690_etf(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd, string em_rate_ymd, int emptyRowCount = 0) {

            string sheetName = "ETF成交量";
            //string sheetSubTitle = "";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0} 資料轉出中......", sheetName);
            this.Refresh();

            ws.Cells[1, 2].Value = em_aft_fm_ymd;
            ws.Cells[1, 3].Value = em_aft_to_ymd;
            ws.Cells[2, 2].Value = em_prev_fm_ymd;
            ws.Cells[2, 3].Value = em_prev_to_ymd;

            DataTable dt = dao30690.d_30690_etf(em_prev_fm_ymd, em_prev_to_ymd, em_aft_fm_ymd, em_aft_to_ymd);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, "", NoData);
                return;
            }

            //寫入明細
            int rowIndex = ws.Cells[1, 5].Value.AsInt() == 0 ? 11 : ws.Cells[1, 5].Value.AsInt();//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int pos = 0;//tmp

            foreach (DataRow dr in dt.Rows) {
                if (pos % 20 == 0) {
                    labMsg.Text = string.Format("{0} 資料轉出中......{1} / {2}", sheetName, pos, dt.Rows.Count);
                    this.Refresh();
                }

                ws.Cells[rowIndex, 2].Value = dr["apdk_name"].AsString();
                ws.Cells[rowIndex, 3].Value = dr["market_close"].AsString();
                ws.Cells[rowIndex, 4].Value = dr["kind_id2"].AsString();

                //本週期貨
                ws.Cells[rowIndex, 5].Value = dr["f_week_qnty"].AsDecimal();
                ws.Cells[rowIndex, 6].Value = dr["f_week_oi"].AsDecimal();
                ws.Cells[rowIndex, 7].Value = dr["f_week_days"].AsDecimal();
                //這邊拿到的值如果為DBNull.Value,則輸出也要為null,不能直接轉成0 (不能用AsDecimal)
                if (dr["am11_etf_week_qnty"] != DBNull.Value)
                    ws.Cells[rowIndex, 10].Value = dr["am11_etf_week_qnty"].AsDecimal();

                //本週選擇權
                ws.Cells[rowIndex, 12].Value = dr["o_week_qnty"].AsDecimal();
                ws.Cells[rowIndex, 13].Value = dr["o_week_oi"].AsDecimal();
                ws.Cells[rowIndex, 14].Value = dr["o_week_days"].AsDecimal();
                //這邊拿到的值如果為DBNull.Value,則輸出也要為null,不能直接轉成0 (不能用AsDecimal)
                if (dr["am11_etc_week_qnty"] != DBNull.Value)
                    ws.Cells[rowIndex, 17].Value = dr["am11_etc_week_qnty"].AsDecimal();

                //上週期貨
                ws.Cells[rowIndex, 19].Value = dr["f_last_qnty"].AsDecimal();
                ws.Cells[rowIndex, 20].Value = dr["f_last_oi"].AsDecimal();
                ws.Cells[rowIndex, 21].Value = dr["f_last_days"].AsDecimal();
                //這邊拿到的值如果為DBNull.Value,則輸出也要為null,不能直接轉成0 (不能用AsDecimal)
                if (dr["am11_etf_last_qnty"] != DBNull.Value)
                    ws.Cells[rowIndex, 24].Value = dr["am11_etf_last_qnty"].AsDecimal();

                //上週選擇權
                ws.Cells[rowIndex, 26].Value = dr["o_last_qnty"].AsDecimal();
                ws.Cells[rowIndex, 27].Value = dr["o_last_oi"].AsDecimal();
                ws.Cells[rowIndex, 28].Value = dr["o_last_days"].AsDecimal();
                //這邊拿到的值如果為DBNull.Value,則輸出也要為null,不能直接轉成0 (不能用AsDecimal)
                if (dr["am11_etc_last_qnty"] != DBNull.Value)
                    ws.Cells[rowIndex, 31].Value = dr["am11_etc_last_qnty"].AsDecimal();

                rowIndex++; pos++;
            }//foreach (DataRow row in dt.Rows) {

            //刪除空白列
            if (dt.Rows.Count < emptyRowCount) {
                ws.Rows.Remove(rowIndex, emptyRowCount - dt.Rows.Count);
            }

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, "", FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.4每月日均量
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="showDate">單純只是顯示,yyyyMMdd</param>
        /// <param name="ls_start_ymd">實際傳入SQL語法內,yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        protected void wf_30690_mth_qnty(Workbook workbook, string showDate, string ls_start_ymd, string em_aft_fm_ymd, string em_aft_to_ymd) {

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            int totalDataCount = 0;
            string sheetName = "每月日均量";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0} 資料轉出中......", sheetName);
            this.Refresh();

            //1.在表頭寫上日期
            ws.Cells[1, 2].Value = showDate;
            ws.Cells[1, 3].Value = em_aft_to_ymd;
            ws.Cells[2, 2].Value = showDate;
            ws.Cells[2, 3].Value = em_aft_to_ymd;
            ws.Cells[3, 2].Value = showDate;
            ws.Cells[3, 3].Value = em_aft_to_ymd;

            string ls_market_type = "A";
            int rowIndex = 0;

            #region //2.跑三次
            for (int k = 1;k <= 3;k++) {
                switch (k) {
                    case 1://ALL
                        ls_market_type = "A";
                        rowIndex = ws.Cells[1, 5].Value.AsInt() == 0 ? 13 : ws.Cells[1, 5].Value.AsInt();//起始row index position
                        break;
                    case 2://DAY
                        ls_market_type = "D";
                        rowIndex = ws.Cells[2, 5].Value.AsInt() == 0 ? 123 : ws.Cells[2, 5].Value.AsInt();//起始row index position
                        break;
                    case 3://NIGHT
                        ls_market_type = "N";
                        rowIndex = ws.Cells[3, 5].Value.AsInt() == 0 ? 233 : ws.Cells[3, 5].Value.AsInt();//起始row index position
                        break;
                }//switch (k) {

                //2.1 read data
                DataTable dt = dao30690.d_30690_mth_qnty_day_night(ls_start_ymd, em_aft_to_ymd, ls_market_type);
                if (dt.Rows.Count <= 0) {
                    showMsg(sheetName, "", ls_start_ymd + "~" + em_aft_to_ymd, NoData);
                    return;
                }

                totalDataCount += dt.Rows.Count;

                //2.2填入月份表頭(第一次跑就好)
                if (k == 1) {
                    DataView dv = dt.AsDataView();
                    dv.RowFilter = " param_key = 'TXO' ";
                    dv.Sort = "prod_type , param_key , rpt_col";
                    DataTable dtTemp = dv.ToTable();//ken,抓TXO只是為了填表頭,後面寫入資料時要用dt,不要搞混
                    foreach (DataRow dr in dtTemp.Rows) {
                        int y = dr["rpt_col"].AsInt();
                        string pDate = dr["data_ym"].AsString();
                        string title = "";

                        if (pDate.Length > 4) {
                            DateTime pTemp = DateTime.ParseExact(pDate + "01", "yyyyMMdd", null);
                            title = taiwanCalendar.GetYear(pTemp) + "/" + pTemp.ToString("MM");
                        } else {
                            DateTime pTemp = DateTime.ParseExact(pDate + "0101", "yyyyMMdd", null);
                            title = taiwanCalendar.GetYear(pTemp) + "年";
                        }

                        ws.Cells[6, 14 + y * 2].Value = title;//row=rowIndex-7,col=16,18,20...
                        ws.Cells[116, 14 + y * 2].Value = title;
                        ws.Cells[226, 14 + y * 2].Value = title;
                    }//foreach (DataRow dr in dtTemp.Rows) {
                }//if (k == 1) {

                //2.3寫入資料
                string temp_param_key = "";
                rowIndex--;
                rowIndex--;
                foreach (DataRow dr in dt.Rows) {
                    string param_key = dr["param_key"].AsString();
                    if (param_key != temp_param_key) {
                        temp_param_key = param_key;
                        rowIndex++;
                        ws.Cells[rowIndex, 0].Value = dr["prod_type"].AsString();
                        ws.Cells[rowIndex, 1].Value = temp_param_key;
                    }

                    int y = dr["rpt_col"].AsInt();
                    if (y > 0) {
                        ws.Cells[rowIndex, 14 + y * 2].Value = dr["data_qnty"].AsDouble();
                        ws.Cells[rowIndex, 15 + y * 2].Value = dr["data_day_cnt"].AsDouble();
                    }
                }//foreach (DataRow dr in dtTemp.Rows) {

            }//for (int k = 1;k <= 3;k++) {
            #endregion


            //3.平均預估量
            DataTable dtSecond = dao30690.d_30690_mth_qnty_am7t(em_aft_fm_ymd, em_aft_to_ymd);
            if (dtSecond.Rows.Count > 0) {
                rowIndex = ws.Cells[1, 5].Value.AsInt() == 0 ? 13 : ws.Cells[1, 5].Value.AsInt();//起始row index position
                rowIndex--;
                foreach (DataRow dr in dtSecond.Rows) {
                    ws.Cells[rowIndex++, 15].Value = dr["avg_qnty"].AsDecimal();
                }//foreach (DataRow dr in dtSecond.Rows) {
            }


            //4.每月平均預估量
            DataTable dtThird = dao30690.d_30690_mth_qnty_am7m(ls_start_ymd, em_aft_to_ymd);
            if (dtThird.Rows.Count > 0) {
                rowIndex = ws.Cells[1, 4].Value.AsInt() == 0 ? 9 : ws.Cells[1, 4].Value.AsInt();//起始row index position
                rowIndex = rowIndex - 6;
                string ls_param_key = "";
                foreach (DataRow dr in dtThird.Rows) {
                    string param_key = dr["prod_type"].AsString();
                    if (param_key != ls_param_key) {
                        ls_param_key = param_key;
                        rowIndex++;
                        ws.Cells[rowIndex, 1].Value = ls_param_key;
                    }

                    int y = dr["rpt_col"].AsInt();
                    if (y > 0) {
                        ws.Cells[rowIndex, 1 + y].Value = dr["avg_qnty"].AsDouble();
                    }
                }//foreach (DataRow dr in dtSecond.Rows) {
            }


            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, "", FinishPrefix + totalDataCount.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.5+3.6每月波動及振幅(現貨/期貨)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="from_ymd"></param>
        /// <param name="to_ymd"></param>
        /// <param name="prodType">T=現貨,F=期貨</param>
        private void wf_30690_high_low(Workbook workbook, string from_ymd, string to_ymd, string prodType) {

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            string sheetName = (prodType == "T" ? "每月波動及振幅(現貨)" : "每月波動及振幅(期貨)");
            string sheetSubTitle = "";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0} 資料轉出中......", sheetName);
            this.Refresh();

            ws.Cells[1, 2].Value = from_ymd;
            ws.Cells[1, 3].Value = to_ymd;

            DataTable dt = dao30690.d_30690_high_low(from_ymd, to_ymd, prodType);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, from_ymd + "~" + to_ymd, NoData);
                return;
            }

            int rowIndex = ws.Cells[1, 5].Value.AsInt() == 0 ? 7 : ws.Cells[1, 5].Value.AsInt();//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int colIndex = 0;

            //填入月份表頭
            DataView dv = dt.AsDataView();
            dv.RowFilter = " param_key = 'TXF' ";
            DataTable dtMonth = dv.ToTable();
            foreach (DataRow dr in dtMonth.Rows) {
                colIndex = dr["rpt_col"].AsInt();
                string data_ym = dr["data_ym"].AsString();
                string colValue = "";

                if (data_ym.Length > 4) {
                    DateTime tmp = DateTime.ParseExact(data_ym, "yyyyMM", null);
                    colValue = taiwanCalendar.GetYear(tmp) + "/" + tmp.ToString("MM");
                } else {
                    DateTime tmp = DateTime.ParseExact(data_ym, "yyyy", null);
                    colValue = taiwanCalendar.GetYear(tmp) + "年";
                }
                ws.Cells[4, 2 + (colIndex - 1) * 2].Value = colValue;
            }//foreach (DataRow row in dtMonth.Rows) {

            //填入資料
            string ls_param_key = "";
            rowIndex--;
            foreach (DataRow dr in dt.Rows) {
                if (ls_param_key != dr["param_key"].AsString()) {
                    ls_param_key = dr["param_key"].AsString();
                    rowIndex++;
                    ws.Cells[rowIndex, 0].Value = dr["prod_type"].AsString();
                    ws.Cells[rowIndex, 1].Value = ls_param_key;
                }

                colIndex = dr["rpt_col"].AsInt();
                if (colIndex > 0) {
                    ws.Cells[rowIndex, 2 + (colIndex - 1) * 2].Value = dr["tfxm_rate"].AsDouble();
                    ws.Cells[rowIndex, 2 + (colIndex - 1) * 2 + 1].Value = dr["avg_tfxm"].AsDouble();
                }
            }//foreach (DataRow dr in dt.Rows) {

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.7交易人結構
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        /// <param name="ocfDate">today</param>
        private void wf_30690_day_am21(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd, DateTime ocfDate) {

            int totalDataCount = 0;
            string sheetName = "交易人結構";
            string sheetSubTitle = "";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0} 資料轉出中......", sheetName);
            this.Refresh();

            //跑五次不同的時間區段,拿到五個dataTable
            string ls_fm = "", ls_to = "";
            for (int li_data = 1;li_data < 6;li_data++) {
                switch (li_data) {
                    case 1://本週
                        sheetSubTitle = "本週";
                        ls_fm = em_aft_fm_ymd;
                        ls_to = em_aft_to_ymd;
                        break;
                    case 2://上週
                        sheetSubTitle = "上週";
                        ls_fm = em_prev_fm_ymd;
                        ls_to = em_prev_to_ymd;
                        break;
                    case 3://本年度迄今
                        sheetSubTitle = "本年度迄今";
                        ls_fm = ocfDate.ToString("yyyy0101");
                        ls_to = em_aft_to_ymd;// = ocfDate.ToString("yyyyMMdd");
                        break;
                    case 4://本年度迄上個月(上個月的該年度第一天到上個月的月底)
                        sheetSubTitle = "本年度迄上個月";
                        ls_fm = ocfDate.AddDays(-ocfDate.Day).ToString("yyyy0101");
                        ls_to = ocfDate.AddDays(-ocfDate.Day).ToString("yyyyMMdd");
                        break;
                    case 5://上年度(去年第一天到去年的最後一天,如果現在是一月,則改為前年第一天到前年的最後一天)
                        sheetSubTitle = "上年度";
                        if (ocfDate.Month == 1) {
                            ls_fm = ocfDate.AddYears(-2).ToString("yyyy0101");
                            ls_to = ocfDate.AddYears(-2).ToString("yyyy1231");
                        } else {
                            ls_fm = ocfDate.AddYears(-1).ToString("yyyy0101");
                            ls_to = ocfDate.AddYears(-1).ToString("yyyy1231");
                        }
                        break;
                }//switch (li_data) {

                ws.Cells[li_data, 2].Value = ls_fm;
                ws.Cells[li_data, 3].Value = ls_to;


                DataTable dt = dao30690.d_30690_day_am21(ls_fm, ls_to);
                if (dt.Rows.Count <= 0) {
                    showMsg(sheetName, sheetSubTitle, ls_fm + "~" + ls_to, NoData);
                    continue;
                }

                totalDataCount += dt.Rows.Count;

                int rowIndex = ws.Cells[li_data, 5].Value.AsInt();//起始row index position
                if (rowIndex == 0) {
                    rowIndex = 16 + ((li_data - 1) * 108);
                }
                rowIndex--;//c#在計算index總是比PB少1

                foreach (DataRow dr in dt.Rows) {
                    ws.Cells[rowIndex, 0].Value = dr["prod_type"].AsString();
                    ws.Cells[rowIndex, 1].Value = dr["param_key"].AsString();

                    ws.Cells[rowIndex, 8].Value = dr["tot"].AsDecimal();
                    ws.Cells[rowIndex, 9].Value = dr["m1"].AsDecimal();
                    ws.Cells[rowIndex, 10].Value = dr["m2"].AsDecimal();
                    ws.Cells[rowIndex, 11].Value = dr["m3"].AsDecimal();
                    ws.Cells[rowIndex, 12].Value = dr["m4"].AsDecimal();

                    rowIndex++;
                }//foreach (DataRow dr in dt.Rows) {
            }//for (int li_data = 1;li_data < 6;li_data++) {

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, "", FinishPrefix + totalDataCount.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.8盤後交易人結構 (sql語法跟一般盤差一些,其它邏輯都一樣)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        /// <param name="ocfDate">today</param>
        private void wf_30690_night_am21(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd, DateTime ocfDate) {

            int totalDataCount = 0;
            string sheetName = "盤後交易人結構";
            string sheetSubTitle = "";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0} 資料轉出中......", sheetName);
            this.Refresh();

            //跑五次不同的時間區段,拿到五個dataTable
            string ls_fm = "", ls_to = "";
            for (int li_data = 1;li_data < 6;li_data++) {
                switch (li_data) {
                    case 1://本週
                        sheetSubTitle = "本週";
                        ls_fm = em_aft_fm_ymd;
                        ls_to = em_aft_to_ymd;
                        break;
                    case 2://上週
                        sheetSubTitle = "上週";
                        ls_fm = em_prev_fm_ymd;
                        ls_to = em_prev_to_ymd;
                        break;
                    case 3://本年度迄今
                        sheetSubTitle = "本年度迄今";
                        ls_fm = ocfDate.ToString("yyyy0101");
                        ls_to = em_aft_to_ymd;// = ocfDate.ToString("yyyyMMdd");
                        break;
                    case 4://本年度迄上個月(上個月的該年度第一天到上個月的月底)
                        sheetSubTitle = "本年度迄上個月";
                        ls_fm = ocfDate.AddDays(-ocfDate.Day).ToString("yyyy0101");
                        ls_to = ocfDate.AddDays(-ocfDate.Day).ToString("yyyyMMdd");
                        break;
                    case 5://上年度(去年第一天到去年的最後一天,如果現在是一月,則改為前年第一天到前年的最後一天)
                        sheetSubTitle = "上年度";
                        if (ocfDate.Month == 1) {
                            ls_fm = ocfDate.AddYears(-2).ToString("yyyy0101");
                            ls_to = ocfDate.AddYears(-2).ToString("yyyy1231");
                        } else {
                            ls_fm = ocfDate.AddYears(-1).ToString("yyyy0101");
                            ls_to = ocfDate.AddYears(-1).ToString("yyyy1231");
                        }
                        break;
                }//switch (li_data) {

                ws.Cells[li_data, 2].Value = ls_fm;
                ws.Cells[li_data, 3].Value = ls_to;


                DataTable dt = dao30690.d_30690_night_am21(ls_fm, ls_to);
                if (dt.Rows.Count <= 0) {
                    showMsg(sheetName, sheetSubTitle, ls_fm + "~" + ls_to, NoData);
                    continue;
                }

                totalDataCount += dt.Rows.Count;

                int rowIndex = ws.Cells[li_data, 5].Value.AsInt();//起始row index position
                if (rowIndex == 0) {
                    rowIndex = 16 + ((li_data - 1) * 108);
                }
                rowIndex--;//c#在計算index總是比PB少1

                foreach (DataRow dr in dt.Rows) {
                    ws.Cells[rowIndex, 0].Value = dr["prod_type"].AsString();
                    ws.Cells[rowIndex, 1].Value = dr["param_key"].AsString();

                    ws.Cells[rowIndex, 8].Value = dr["tot"].AsDecimal();
                    ws.Cells[rowIndex, 9].Value = dr["m1"].AsDecimal();
                    ws.Cells[rowIndex, 10].Value = dr["m2"].AsDecimal();
                    ws.Cells[rowIndex, 11].Value = dr["m3"].AsDecimal();
                    ws.Cells[rowIndex, 12].Value = dr["m4"].AsDecimal();

                    rowIndex++;
                }//foreach (DataRow dr in dt.Rows) {
            }//for (int li_data = 1;li_data < 6;li_data++) {

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, "", FinishPrefix + totalDataCount.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.9TX波動及振福
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        private void wf_30690_tx_high_low(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd) {

            string sheetName = "TX波動及振福";
            string sheetSubTitle = "";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0} 資料轉出中......", sheetName);
            this.Refresh();

            ws.Cells[1, 2].Value = em_aft_fm_ymd;
            ws.Cells[1, 3].Value = em_aft_to_ymd;
            ws.Cells[2, 2].Value = em_prev_fm_ymd;
            ws.Cells[2, 3].Value = em_prev_to_ymd;

            DataTable dt = dao30690.d_30690_first(em_prev_fm_ymd, em_aft_to_ymd);//ken,傳入的時間很特別
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, em_prev_fm_ymd + "~" + em_aft_to_ymd, NoData);
                return;
            }

            //寫入明細
            int rowIndex = ws.Cells[1, 5].Value.AsInt() == 0 ? 10 : ws.Cells[1, 5].Value.AsInt();//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int colIndex = 0;

            string ls_fm = "", ls_to = "";
            for (int li_data = 0;li_data <= 2;li_data++) {
                switch (li_data) {
                    case 0://本週
                        colIndex = 2;
                        ls_fm = em_aft_fm_ymd;
                        ls_to = em_aft_to_ymd;
                        break;
                    case 1://上週
                        colIndex = 6;
                        ls_fm = em_prev_fm_ymd;
                        ls_to = em_prev_to_ymd;
                        break;
                    case 2://本年度迄今
                        colIndex = 10;
                        ls_fm = em_aft_to_ymd.SubStr(0, 4) + "0101";
                        ls_to = em_aft_to_ymd;
                        break;
                }//switch (li_data) {

                DataView dvTemp = dt.AsDataView();
                dvTemp.RowFilter = string.Format("ocf_ymd >= '{0}' and ocf_ymd <='{1}' ", ls_fm, ls_to);
                DataTable dtSub = dvTemp.ToTable();

                ws.Import(dtSub, false, rowIndex, colIndex);

            }//for (int li_data = 0;li_data <= 2;li_data++) {

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.10其它--加掛小型股票期貨交易概況(日均量)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        private void wf_30690_stf_m(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd) {

            string sheetName = "其它";
            string sheetSubTitle = "加掛小型股票期貨交易概況(日均量)";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            ws.Cells[1, 2].Value = em_prev_fm_ymd;
            ws.Cells[1, 3].Value = em_prev_to_ymd;
            ws.Cells[2, 2].Value = em_aft_fm_ymd;
            ws.Cells[2, 3].Value = em_aft_to_ymd;

            DataTable dt = dao30690.d_30690_6(em_prev_fm_ymd, em_prev_to_ymd, em_aft_fm_ymd, em_aft_to_ymd);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, em_prev_fm_ymd + "~" + em_aft_to_ymd, NoData);
                return;
            }

            //寫入明細
            int rowIndex = 7;//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int pos = 0;
            int colIndex = 5;

            //特殊填入資料,從(6,5)開始,由左往右填入,每筆跳四欄
            foreach (DataRow dr in dt.Rows) {
                if (pos % 10 == 0) {
                    labMsg.Text = string.Format("{0} 資料轉出中......{1} / {2}", sheetName, pos, dt.Rows.Count);
                    this.Refresh();
                }

                ws.Cells[rowIndex, colIndex].Value = dr["p_name"].AsString();
                ws.Cells[rowIndex + 1, colIndex].Value = dr["grp_id2"].AsString();
                ws.Cells[rowIndex + 1, colIndex + 2].Value = dr["kind_id2"].AsString();

                ws.Cells[rowIndex + 3, colIndex].Value = dr["avg_qnty_week_2000"].AsDouble();
                ws.Cells[rowIndex + 3, colIndex + 2].Value = dr["avg_qnty_week_100"].AsDouble();
                ws.Cells[rowIndex + 4, colIndex].Value = dr["avg_qnty_last_2000"].AsDouble();
                ws.Cells[rowIndex + 4, colIndex + 2].Value = dr["avg_qnty_last_100"].AsDouble();

                pos++;
                colIndex += 4;
            }//foreach (DataRow row in dt.Rows) {

            //刪除空白column(目前不做刪除,因為下面還會再填入一個dataTable,或許欄位會比這個多)
            if (colIndex < 124) {
                //ws.Columns.Remove(colIndex, XXX);
            }

            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.11其它--新上市個股選擇權交易概況(日均量)
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        /// <param name="prodList">商品kind_id2</param>
        private void wf_30690_new_stc(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd, List<string> prodList) {

            string sheetName = "其它";
            string sheetSubTitle = "新上市個股選擇權交易概況(日均量)";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            ws.Cells[1, 2].Value = em_prev_fm_ymd;
            ws.Cells[1, 3].Value = em_prev_to_ymd;
            ws.Cells[2, 2].Value = em_aft_fm_ymd;
            ws.Cells[2, 3].Value = em_aft_to_ymd;

            DataTable dt = dao30690.d_30690_7(em_prev_fm_ymd, em_prev_to_ymd, em_aft_fm_ymd, em_aft_to_ymd, prodList);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, em_prev_fm_ymd + "~" + em_aft_to_ymd, NoData);
                return;
            }

            //寫入明細
            int rowIndex = 14;//起始row index position
            rowIndex--;//c#在計算index總是比PB少1
            int pos = 0;
            int colIndex = 5;

            //特殊填入資料,從(13,5)開始,由左往右填入,每筆跳3欄
            foreach (DataRow dr in dt.Rows) {
                if (pos % 10 == 0) {
                    labMsg.Text = string.Format("{0} 資料轉出中......{1} / {2}", sheetName, pos, dt.Rows.Count);
                    this.Refresh();
                }

                ws.Cells[rowIndex, colIndex].Value = dr["p_name"].AsString();
                ws.Cells[rowIndex + 1, colIndex].Value = dr["avg_qnty_week"].AsDouble();
                ws.Cells[rowIndex + 2, colIndex].Value = dr["avg_qnty_last"].AsDouble();

                pos++;
                colIndex += 3;
            }//foreach (DataRow row in dt.Rows) {

            //刪除空白column(目前不做刪除)
            if (colIndex < 124) {
                //ws.Columns.Remove(colIndex, XXX);
            }

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.12二--歐臺期及歐臺選日均量
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_prev_fm_ymd">yyyyMMdd</param>
        /// <param name="em_prev_to_ymd">yyyyMMdd</param>
        /// <param name="em_aft_fm_ymd">yyyyMMdd</param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        private void wf_30690_sheet2(Workbook workbook, string em_prev_fm_ymd, string em_prev_to_ymd, string em_aft_fm_ymd, string em_aft_to_ymd) {

            int totalDataCount = 0;
            string sheetName = "二";
            string sheetSubTitle = "歐臺期及歐臺選日均量";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            ws.Cells[1, 2].Value = em_prev_fm_ymd;
            ws.Cells[1, 3].Value = em_prev_to_ymd;
            ws.Cells[2, 2].Value = em_aft_fm_ymd;
            ws.Cells[2, 3].Value = em_aft_to_ymd;

            //跑五次不同的時間區段,拿到五個dataTable
            string ls_min_ymd = "", ls_max_ymd = "";
            DataTable dtOcfDate = new DataTable();
            int rowIndex = 8;//起始row index position

            for (int k = 1;k <= 5;k++) {
                switch (k) {
                    case 1://本週
                        dtOcfDate = new AOCF().ListOcfYmd(em_aft_fm_ymd, em_aft_to_ymd);
                        break;
                    case 2://上週
                        dtOcfDate = new AOCF().ListOcfYmd(em_prev_fm_ymd, em_prev_to_ymd);
                        break;
                    case 3://本年度迄今
                        dtOcfDate = new AOCF().ListOcfYmd("20140515", em_aft_to_ymd);
                        break;
                    case 4://T-2月
                        dtOcfDate = new AOCF().ListOcfYmdByT(em_aft_to_ymd, em_aft_to_ymd, 2);
                        break;
                    case 5://T-1月
                        dtOcfDate = new AOCF().ListOcfYmdByT(em_aft_to_ymd, em_aft_to_ymd, 1);
                        break;
                }//switch (k) {
                ls_min_ymd = dtOcfDate.Rows[0][0].AsString();
                ls_max_ymd = dtOcfDate.Rows[0][1].AsString();


                DataTable dt = dao30690.d_30690_second(ls_min_ymd, ls_max_ymd);
                if (dt.Rows.Count <= 0) {
                    showMsg(sheetName, sheetSubTitle, ls_min_ymd + "~" + ls_max_ymd, NoData);
                    return;
                }

                totalDataCount += dt.Rows.Count;
                ws.Import(dt, false, rowIndex, 1);

                if (k >= 4) {
                    //輸出=yyy年mm月
                    TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
                    DateTime temp = DateTime.ParseExact(ls_max_ymd, "yyyyMMdd", null);
                    ws.Cells[rowIndex, 0].Value = taiwanCalendar.GetYear(temp) + "年" + temp.ToString("MM") + "月";
                }

                rowIndex++;
            }//for (int k = 1;k < 6;k++) {

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, sheetSubTitle, FinishPrefix + totalDataCount.ToString() + FinishSuffix);
        }

        /// <summary>
        /// 3.13三--人民幣匯率期貨及選擇權交易量
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="em_aft_to_ymd">yyyyMMdd</param>
        private void wf_30690_third(Workbook workbook, string em_aft_to_ymd) {

            string sheetName = "三";
            string sheetSubTitle = "人民幣匯率期貨及選擇權交易量";
            Worksheet ws = workbook.Worksheets[sheetName];
            ws.Range["A1"].Select();
            labMsg.Text = string.Format("{0}-{1} 資料轉出中......", sheetName, sheetSubTitle);
            this.Refresh();

            DataTable dt = dao30690.d_30690_third(em_aft_to_ymd);
            if (dt.Rows.Count <= 0) {
                showMsg(sheetName, sheetSubTitle, em_aft_to_ymd, NoData);
                return;
            }

            //寫入明細
            int rowIndex = 4;//起始row index position
            int colIndex = 0;

            ws.Import(dt, false, rowIndex, colIndex);

            //刪除空白列
            int emptyRowCount = 55;
            if (dt.Rows.Count < emptyRowCount) {
                ws.Rows.Remove(rowIndex, emptyRowCount - dt.Rows.Count);
            }

            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            showMsg(sheetName, sheetSubTitle, FinishPrefix + dt.Rows.Count.ToString() + FinishSuffix);
        }


        /// <summary>
        /// 轉波動度及振幅現貨資料 to csv ( for admin test)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e) {


            string sheetName = "每月波動及振幅(明細)";

            DataTable dt = dao30690.d_30690_high_low_detial(ThisEnd);
            if (dt.Rows.Count <= 0) {
                labMsg.Text = sheetName + NoData;
                return;
            }

            //存CSV (ps:輸出csv 都用ascii)
            string etfFileName = _ProgramID + "_high_low_detial_" + DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss") + ".csv";
            etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, etfFileName);
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = true;
            csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dt, etfFileName, csvref);

            System.Diagnostics.Process.Start(etfFileName);
        }

        #region 顯示匯出訊息的相關函數

        /// <summary>
        /// 顯示匯出的進度
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="sheetSubTitle"></param>
        /// <param name="msg"></param>
        protected void showMsg(string sheetName, string sheetSubTitle, string msg) {
            showMsg(sheetName, sheetSubTitle, "", msg);
        }

        /// <summary>
        /// 顯示匯出的進度
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="sheetSubTitle"></param>
        /// <param name="subMsg"></param>
        /// <param name="msg"></param>
        protected void showMsg(string sheetName, string sheetSubTitle, string subMsg, string msg) {

            DataRow drTemp = dtMsg.NewRow();
            drTemp["SheetName"] = sheetName;
            drTemp["SheetSubTitle"] = sheetSubTitle;
            drTemp["SubMsg"] = subMsg;
            drTemp["Msg"] = msg;
            dtMsg.Rows.Add(drTemp);

            gcMsg.DataSource = dtMsg;
            gcMsg.RefreshDataSource();
            gcMsg.Refresh();

            Thread.Sleep(100);
        }

        private void gvMsg_RowCountChanged(object sender, EventArgs e) {
            gvMsg.MoveLast();
        }

        #endregion
    }
}
