using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPrinting;
using System;
using System.Data;
using System.IO;

namespace PhoenixCI.FormUI.Prefix3
{
    /// <summary>
    /// 成交價偏離幅度統計表
    /// </summary>
    public partial class W30681 : FormParent
    {
        private D30681 dao30681;
        private const string ReportName = "成交價偏離幅度統計表";
        private const string OriTabName = "xtraTabPageOri"; //非匯率期貨類
        private const string NewTabName = "xtraTabPageNew"; //匯率期貨類

        #region 抓取畫面值(主要用在縮寫與分辨不同頁籤的控制項)
        /// <summary>
        /// Old=新 , New=舊
        /// </summary>
        public string Source
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return rdoSource.EditValue.AsString();
                }
                else
                {
                    return "New";   //匯率期貨類一律用新的
                }
            }
        }

        /// <summary>
        /// Summary=統計 , Detail=明細
        /// </summary>
        public string ReportType
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return rdoReportType.EditValue.AsString();
                }
                else
                {
                    return rdoReportType_new.EditValue.AsString();
                }
            }
        }

        /// <summary>
        /// 商品代號：第1支腳
        /// </summary>
        public string Kind1
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return txtKind1.Text;
                }
                else
                {
                    return txtKind1_new.Text;
                }
            }
        }

        /// <summary>
        /// 商品代號：第2支腳
        /// </summary>
        public string Kind2
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return txtKind2.Text;
                }
                else
                {
                    return txtKind2_new.Text;
                }
            }
        }

        /// <summary>
        /// 到期月序：第1支腳
        /// </summary>
        public int Mth1
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return txtMth1.EditValue.AsInt(99);
                }
                else
                {
                    return txtMth1_new.EditValue.AsInt(99);
                }
            }
        }

        /// <summary>
        /// 到期月序：第2支腳
        /// </summary>
        public int Mth2
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return txtMth2.EditValue.AsInt(99);
                }
                else
                {
                    return txtMth2_new.EditValue.AsInt(99);
                }
            }
        }

        /// <summary>
        /// get chkLevel 有勾選的值,用逗號串接
        /// </summary>
        public string DetailLevelList
        {
            get
            {
                int max;
                DevExpress.XtraEditors.CheckedListBoxControl chkBox;
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    chkBox = chkLevel;
                    max = 7;
                }
                else
                {
                    chkBox = chkLevel_new;
                    max = 10;
                }

                string temp = "";
                for (int k = 0; k < max; k++)
                {
                    if (chkBox.Items[k].CheckState == System.Windows.Forms.CheckState.Checked)
                    {
                        temp += ",'" + chkBox.Items[k].Value + "'";
                    }
                }
                temp = string.IsNullOrEmpty(temp) ? "" : temp.Substring(1);
                return temp;
            }
        }

        /// <summary>
        /// 是否有勾選空白,Y=是,N=否
        /// </summary>
        public string IsLevelNull
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName && chkLevel.Items[7].CheckState == System.Windows.Forms.CheckState.Checked)
                {
                    return "Y";
                }
                else
                {
                    return "N";
                }
            }
        }

        /// <summary>
        /// 資料日期起日
        /// </summary>
        public BaseGround.Widget.TextDateEdit StartDate
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return txtStartDate;
                }
                else
                {
                    return txtStartDate_new;
                }
            }
        }

        /// <summary>
        /// 資料日期迄日
        /// </summary>
        public BaseGround.Widget.TextDateEdit EndDate
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return txtEndDate;
                }
                else
                {
                    return txtEndDate_new;
                }
            }
        }

        /// <summary>
        /// 單 / 複式
        /// </summary>
        public string DDLScCode
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return ddlScCode.EditValue.AsString();
                }
                else
                {
                    return ddlScCode_new.EditValue.AsString();
                }
            }
        }

        /// <summary>
        /// 委託單方式
        /// </summary>
        public string DDLOsfOrderType
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return ddlOsfOrderType.EditValue.AsString();
                }
                else
                {
                    return ddlOsfOrderType_new.EditValue.AsString();
                }
            }
        }

        /// <summary>
        /// 委託單條件
        /// </summary>
        public string DDLOsfOrderCond
        {
            get
            {
                if (xtraTabControl.SelectedTabPage.Name == OriTabName)
                {
                    return ddlOsfOrderCond.EditValue.AsString();
                }
                else
                {
                    return ddlOsfOrderCond_new.EditValue.AsString();
                }
            }
        }
        #endregion


        public W30681(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;

            dao30681 = new D30681();

            GridHelper.SetCommonGrid(gvExport);
            GridHelper.SetCommonGrid(gvExport_new);

            gvExport.OptionsBehavior.Editable =
            gvExport_new.OptionsBehavior.Editable = false;

            gvExport.OptionsBehavior.AutoPopulateColumns =
            gvExport_new.OptionsBehavior.AutoPopulateColumns = true;

            gvExport.OptionsView.RowAutoHeight =
            gvExport_new.OptionsView.RowAutoHeight = true;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            //設定 商品單複式 下拉選單
            DataTable lstScCode = new CODW().ListLookUpEdit("30681", "DDLB_SC");
            Extension.SetDataTable(ddlScCode, lstScCode, "CODW_ID", "CODW_DESC", TextEditStyles.DisableTextEditor);
            Extension.SetDataTable(ddlScCode_new, lstScCode, "CODW_ID", "CODW_DESC", TextEditStyles.DisableTextEditor);
            ddlScCode.ItemIndex =
            ddlScCode_new.ItemIndex = 0;

            //設定 委託單方式 下拉選單
            DataTable dtOsfOrderType = new CODW().ListLookUpEdit("30681", "OSF_ORDER_TYPE");
            Extension.SetDataTable(ddlOsfOrderType, dtOsfOrderType, "CODW_ID", "CODW_DESC", TextEditStyles.DisableTextEditor);
            Extension.SetDataTable(ddlOsfOrderType_new, dtOsfOrderType, "CODW_ID", "CODW_DESC", TextEditStyles.DisableTextEditor);
            ddlOsfOrderType.ItemIndex =
            ddlOsfOrderType_new.ItemIndex = 0;

            //設定 委託單條件 下拉選單
            DataTable dtOsfOrderCond = new CODW().ListLookUpEdit("30681", "OSF_ORDER_COND");
            Extension.SetDataTable(ddlOsfOrderCond, dtOsfOrderCond, "CODW_ID", "CODW_DESC", TextEditStyles.DisableTextEditor);
            Extension.SetDataTable(ddlOsfOrderCond_new, dtOsfOrderCond, "CODW_ID", "CODW_DESC", TextEditStyles.DisableTextEditor);
            ddlOsfOrderCond.ItemIndex =
            ddlOsfOrderCond_new.ItemIndex = 0;

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen()
        {
            base.AfterOpen();

            txtEndDate.DateTimeValue =
            txtEndDate_new.DateTimeValue = GlobalInfo.OCF_DATE;

            txtStartDate.DateTimeValue = txtEndDate.DateTimeValue;
            txtStartDate_new.DateTimeValue = txtEndDate_new.DateTimeValue;
#if DEBUG
            txtStartDate.DateTimeValue =
            txtStartDate_new.DateTimeValue = DateTime.ParseExact("2017/10/11", "yyyy/MM/dd", null);

            txtEndDate.DateTimeValue =
            txtEndDate_new.DateTimeValue = DateTime.ParseExact("2018/10/11", "yyyy/MM/dd", null);
            this.Text += "(開啟測試模式)";
#endif

            r_frame2.Visible =
            r_frame2_new.Visible = false;

            panDetail.Visible =
            panDetail_new.Visible = false;

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm()
        {
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

        protected override ResultStatus Export()
        {
            try
            {
                #region 輸入&日期檢核
                if (string.Compare(StartDate.Text, EndDate.Text) > 0)
                {
                    MessageDisplay.Error(CheckDate.Datedif, GlobalInfo.ErrorText);
                    return ResultStatus.Fail;
                }
                #endregion

                //1.開始轉出資料
                panFilter.Enabled = false;
                labMsg.Visible = true;
                labMsg.Text = "開始轉檔...";
                this.Refresh();
                ResultStatus res = ResultStatus.Fail;

                if (ReportType == "Summary")
                {
                    //統計,會輸出一個csv檔案
                    if (Source == "Old")    //only 非匯率期貨類
                    {
                        res = wf_30681_s();
                    }
                    else
                    {
                        res = wf_30681_s_new();
                    }

                }
                else
                {
                    //明細,會輸出兩個csv檔案
                    if (Source == "Old")    //only 非匯率期貨類
                    {
                        res = wf_30681_d();
                        res = wf_30681_d_mtf();
                    }
                    else
                    {
                        res = wf_30681_d_new();
                        res = wf_30681_d_mtf_new();
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            finally
            {
                panFilter.Enabled = true;
                labMsg.Text = "";
                labMsg.Visible = false;
            }
            return ResultStatus.Fail;
        }


        protected ResultStatus wf_30681_s()
        {
            try
            {
                string reportId = "30681_s";
                string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
                                                          string.Format("{0}_{1}.csv", reportId, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

                //1.get dataTable
                DataTable dtTarget = dao30681.d_30681_s(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, ddlScCode.EditValue.AsString(),
                                                            txtKind1.Text, txtKind2.Text, Mth1, Mth2);
                if (dtTarget.Rows.Count <= 0)
                {
                    MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!", txtStartDate.DateTimeValue.ToString("yyyyMM"), reportId, ReportName), GlobalInfo.ResultText);
                    return ResultStatus.Fail;
                }

                //2.Export Csv
                ExportCsv(dtTarget, excelDestinationPath);

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return ResultStatus.Fail;
            }
        }

        protected ResultStatus wf_30681_s_new()
        {
            try
            {
                string reportId = "30681_s_new";
                string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
                                                          string.Format("{0}_{1}.csv", reportId, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

                //1.get dataTable
                DataTable dtTarget = dao30681.d_30681_s_new(StartDate.DateTimeValue, EndDate.DateTimeValue, DDLScCode, Kind1, Kind2, Mth1, Mth2);
                if (dtTarget.Rows.Count <= 0)
                {
                    MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!", StartDate.DateTimeValue.ToString("yyyyMM"), reportId, ReportName), GlobalInfo.ResultText);
                    return ResultStatus.Fail;
                }

                //2.Export Csv
                ExportCsv(dtTarget, excelDestinationPath);

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return ResultStatus.Fail;
            }
        }

        protected ResultStatus wf_30681_d()
        {
            try
            {
                string reportId = "30681_d";
                string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
                                                          string.Format("{0}_{1}.csv", reportId, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

                //1.get dataTable
                DataTable dtTarget = dao30681.d_30681_d(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, ddlScCode.EditValue.AsString(),
                                                            txtKind1.Text, txtKind2.Text, Mth1, Mth2,
                                                            ddlOsfOrderType.EditValue.AsString(), ddlOsfOrderCond.EditValue.AsString(), DetailLevelList, IsLevelNull);
                if (dtTarget.Rows.Count <= 0)
                {
                    MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!", txtStartDate.DateTimeValue.ToString("yyyyMM"), reportId, ReportName), GlobalInfo.ResultText);
                    return ResultStatus.Fail;
                }

                //2.Export Csv
                ExportCsv(dtTarget, excelDestinationPath);

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return ResultStatus.Fail;
            }
        }

        protected ResultStatus wf_30681_d_new()
        {
            try
            {
                string reportId = "30681_d_new";
                string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
                                                          string.Format("{0}_{1}.csv", reportId, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

                //1.get dataTable
                DataTable dtTarget = dao30681.d_30681_d_new(StartDate.DateTimeValue, EndDate.DateTimeValue, DDLScCode, Kind1, Kind2, Mth1, Mth2,
                                                            DDLOsfOrderType, DDLOsfOrderCond, DetailLevelList, IsLevelNull);
                if (dtTarget.Rows.Count <= 0)
                {
                    MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!", StartDate.DateTimeValue.ToString("yyyyMM"), reportId, ReportName), GlobalInfo.ResultText);
                    return ResultStatus.Fail;
                }

                //2.Export Csv
                ExportCsv(dtTarget, excelDestinationPath);

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return ResultStatus.Fail;
            }
        }

        protected ResultStatus wf_30681_d_mtf()
        {
            try
            {
                string reportId = "30681_d_mtf";
                string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
                                                          string.Format("{0}_{1}_成交委託明細.csv", reportId, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

                //1.get dataTable
                DataTable dtTarget = dao30681.d_30681_d_mtf(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, ddlScCode.EditValue.AsString(),
                                                            txtKind1.Text, txtKind2.Text, Mth1, Mth2,
                                                            ddlOsfOrderType.EditValue.AsString(), ddlOsfOrderCond.EditValue.AsString(), DetailLevelList, IsLevelNull);
                if (dtTarget.Rows.Count <= 0)
                {
                    //MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!", txtStartDate.DateTimeValue.ToString("yyyyMM"), reportId, ReportName),GlobalInfo.ResultText);
                    return ResultStatus.Fail;
                }

                //2.Export Csv
                ExportCsv(dtTarget, excelDestinationPath);

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return ResultStatus.Fail;
            }
        }

        protected ResultStatus wf_30681_d_mtf_new()
        {
            try
            {
                string reportId = "30681_d_mtf_new";
                string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
                                                          string.Format("{0}_{1}_成交委託明細.csv", reportId, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));

                //1.get dataTable
                DataTable dtTarget = dao30681.d_30681_d_mtf_new(StartDate.DateTimeValue, EndDate.DateTimeValue, DDLScCode, Kind1, Kind2, Mth1, Mth2,
                                                               DDLOsfOrderType, DDLOsfOrderCond, DetailLevelList, IsLevelNull);
                if (dtTarget.Rows.Count <= 0)
                {
                    //MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!", txtStartDate.DateTimeValue.ToString("yyyyMM"), reportId, ReportName),GlobalInfo.ResultText);
                    return ResultStatus.Fail;
                }

                //2.Export Csv
                ExportCsv(dtTarget, excelDestinationPath);

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return ResultStatus.Fail;
            }
        }


        /// <summary>
        /// 利用畫面上隱藏的grid,直接匯出CSV
        /// </summary>
        /// <param name="dtTarget"></param>
        protected void ExportCsv(DataTable dtTarget, string filename)
        {
            //2.設定gvExport
            if (xtraTabControl.SelectedTabPage.Name == OriTabName)
            {
                gvExport.Columns.Clear();
                gvExport.OptionsBehavior.AutoPopulateColumns = true;
                gcExport.DataSource = dtTarget;
                gvExport.BestFitColumns();
            }
            else
            {
                gvExport_new.Columns.Clear();
                gvExport_new.OptionsBehavior.AutoPopulateColumns = true;
                gcExport_new.DataSource = dtTarget;
                gvExport_new.BestFitColumns();
            }

            //3.export to csv (use GridControl)
            CsvExportOptionsEx options = new CsvExportOptionsEx();//ken,如果有需要細部設定csv屬性可以用這個
                                                                  //ken,數字格式不管的話,輸出時會自動加上千分位逗號,TextExportMode=Text會直接輸出
            options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
            options.TextExportMode = TextExportMode.Text;

            //ken,pb的時間輸出格式為 mm/dd/yy hh24:mi:ss ,直接在sql語法調整比較快

            if (xtraTabControl.SelectedTabPage.Name == OriTabName)
            {
                gcExport.ExportToCsv(filename, options);
            }
            else
            {
                gcExport_new.ExportToCsv(filename, options);
            }

#if DEBUG
            if (FlagAdmin)
                System.Diagnostics.Process.Start(filename);
#endif
        }
        private void rdoReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsEnable = (sender as DevExpress.XtraEditors.RadioGroup).SelectedIndex == 0 ? false : true;

            if (xtraTabControl.SelectedTabPage.Name == OriTabName)
            {
                ddlOsfOrderType.Enabled =
                ddlOsfOrderCond.Enabled =
                chkLevel.Enabled = IsEnable;

                r_frame2.Visible = IsEnable;
                panDetail.Visible = IsEnable;

                //ken,第一個check始終不能勾
                chkLevel.Items[0].Enabled = false;
            }
            else
            {
                ddlOsfOrderType_new.Enabled =
                ddlOsfOrderCond_new.Enabled =
                chkLevel_new.Enabled = IsEnable;

                r_frame2_new.Visible = IsEnable;
                panDetail_new.Visible = IsEnable;
            }
        }

    }
}