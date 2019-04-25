using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

/// <summary>
/// ken, 2019/3/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 最小風險價格係數現況查詢
    /// </summary>
    public partial class W48010 : FormParent {
        protected D48010 dao48010;
        protected static string ChooseSingleKind = "選單一契約";
        protected DataTable dtTarget;

        public W48010(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;

            GridHelper.SetCommonGrid(gvMain);
            gvMain.OptionsBehavior.Editable = false;
            gvMain.OptionsBehavior.AutoPopulateColumns = true;

            GridHelper.SetCommonGrid(gvExport);
            gvExport.OptionsBehavior.Editable = false;
            gvExport.OptionsBehavior.AutoPopulateColumns = true;
            gvMain.OptionsView.RowAutoHeight = true;

            dao48010 = new D48010();

            labKind.Visible = false;
            ddlKind.Visible = false;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtSDate.EditValue = GlobalInfo.OCF_DATE;

#if DEBUG
            //ken test
            //txtSDate.DateTimeValue = DateTime.ParseExact("2018/06/15", "yyyy/MM/dd", null);
            //this.Text += "(開啟測試模式),ocfDate=2018/06/15";
#endif

            //1.契約類別 下拉選單
            DataTable dtSubType = new COD().ListByCol("48010", "PDK_SUBTYPE         ", "全選", "%"); //第一行全選 + COD_ID / COD_DESC / COD_SEQ_NO
            //ken,特殊,再往上新增一筆 
            DataRow drTemp = dtSubType.NewRow();
            drTemp["COD_ID"] = " ";
            drTemp["COD_DESC"] = ChooseSingleKind;
            drTemp["COD_SEQ_NO"] = -1;
            dtSubType.Rows.InsertAt(drTemp, 0);
            Extension.SetDataTable(ddlSubType, dtSubType, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, "");
            //ken,設定選單事件
            this.ddlSubType.EditValueChanged += new System.EventHandler(this.ddlSubType_EditValueChanged);


            //2.契約代號 下拉選單
            DataTable dtKind = dao48010.ListKind(); //MGT2_SEQ_NO/MGT2_KIND_ID/MGT2_PROD_SUBTYPE
            Extension.SetDataTable(ddlKind, dtKind, "MGT2_KIND_ID", "MGT2_KIND_ID", TextEditStyles.DisableTextEditor, "");

            //3.資料內容 下拉選單
            List<LookupItem> lstData = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "KeyInfo", DisplayMember = "1.重點資料"},
                                        new LookupItem() { ValueMember = "Detail", DisplayMember = "2.明細資料" }};
            Extension.SetDataTable(ddlData, lstData, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");
            //ken,設定選單事件
            this.ddlData.EditValueChanged += new System.EventHandler(this.ddlData_EditValueChanged);

            //4.排序方式 下拉選單
            List<LookupItem> lstSort = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "DATE", DisplayMember = "1.依系統生效日期排序"},
                                        new LookupItem() { ValueMember = "KIND", DisplayMember = "2.依契約類別排序" }};
            Extension.SetDataTable(ddlSort, lstSort, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            //設定下拉選單預設值
            ddlSubType.ItemIndex = 1;//全選
            ddlKind.ItemIndex = 0;
            ddlData.ItemIndex = 0;//1.重點資料
            ddlSort.ItemIndex = 1;//2.依契約類別排序

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = true;//列印

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {
            try {
                labMsg.Visible = false;

                //1.設定準備傳入的參數
                string subType = ddlSubType.EditValue.AsString();
                string kindId = "%";
                if (ddlSubType.Text == ChooseSingleKind) {
                    subType = "%";
                    kindId = ddlKind.EditValue.AsString() + "%";
                }//if (ddlSubType.Text == ChooseSingleKind) {

                //2.改成彈性工廠寫法(KeyInfo撈8顯示5個欄位,Detail撈12顯示9個欄位)

                //原本寫法(廢除)
                //if (ddlData.EditValue.AsString()=="1")
                //    dtTarget = dao48010.d_48010_s(txtSDate.DateTimeValue, subType, kindId, ddlSort.EditValue.AsString());
                //else
                //    dtTarget = dao48010.d_48010_d(txtSDate.DateTimeValue, subType, kindId, ddlSort.EditValue.AsString());

                //ken,第一次改寫(廢除)
                //IGridData gridData = dao48010.CreateGridData(dao48010.GetType(), ddlData.EditValue.AsString());
                //dtTarget = gridData.ListAll(txtSDate.DateTimeValue, subType, kindId, ddlSort.EditValue.AsString());

                //ken,第二次改寫
                IGridData gridData = dao48010.CreateGridData(dao48010.GetType(), ddlData.EditValue.AsString());
                Q48010 q48010 = new Q48010(txtSDate.DateTimeValue, subType, kindId, ddlSort.EditValue.AsString());
                dtTarget = gridData.ListAll(q48010);

                //3.1開始設定Grid
                gcMain.Visible = true;
                gvMain.Columns.Clear();
                gcMain.DataSource = dtTarget;

                //3.2設定每個欄位的caption
                gvMain.SetColumnCaption("COD_DESC", "契約類別");
                gvMain.SetColumnCaption("CPR_KIND_ID", "契約代號");
                gvMain.SetColumnCaption("CPR_EFFECTIVE_DATE", "系統生效日");
                gvMain.SetColumnCaption("CPR_PRICE_RISK_RATE", "最小風險價格係數");
                gvMain.SetColumnCaption("CPR_APPROVAL_DATE", "核定日期");

                gvMain.SetColumnCaption("CPR_APPROVAL_NUMBER", "核定文號及日期");
                gvMain.SetColumnCaption("CPR_REMARK", "備註");
                gvMain.SetColumnCaption("CPR_W_TIME", "異動時間");
                gvMain.SetColumnCaption("CPR_W_USER_ID", "異動人員");

                //3.3設定隱藏欄位
                gvMain.Columns["CPR_PROD_SUBTYPE"].Visible = false;
                gvMain.Columns["SEQ_NO"].Visible = false;
                gvMain.Columns["PROD_TYPE"].Visible = false;

                //3.4統一設定欄位靠左靠右把一些欄位靠左
                gvMain.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;//設定全部欄位先置中
                gvMain.SetColumnHAlignment("CPR_APPROVAL_NUMBER", DevExpress.Utils.HorzAlignment.Default);
                gvMain.SetColumnHAlignment("CPR_REMARK", DevExpress.Utils.HorzAlignment.Default);

                //3.5設定每個column header是否自動折行
                gvMain.SetColumnHeaderWrap("COD_DESC", 70);
                gvMain.SetColumnHeaderWrap("CPR_KIND_ID", 60);
                gvMain.SetColumnHeaderWrap("CPR_PRICE_RISK_RATE", 80);

                //3.6設定每個column是否自動折行
                gvMain.SetColumnWrap("CPR_APPROVAL_NUMBER", 280);
                gvMain.SetColumnWrap("CPR_REMARK", 180);
                gvMain.SetColumnWrap("CPR_W_TIME", 100);

                //3.7設定每個column自動擴展
                gvMain.BestFitColumns();

                return ResultStatus.Success;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }

        protected override ResultStatus Export() {
            //0.check
            if (dtTarget == null) {
                labMsg.Visible = true;
                labMsg.Text = "請先查詢,才可轉出資料";
                return ResultStatus.Fail;
            }
            if (dtTarget.Rows.Count <= 0) {
                labMsg.Visible = true;
                labMsg.Text = "無任何資料";
                return ResultStatus.Fail;
            }

            try {
                //1.開始轉出資料
                panFilter.Enabled = false;
                labMsg.Visible = true;
                labMsg.Text = "訊息：資料轉出中........";
                this.Refresh();

                //2.1 設定gvExport
                gvExport.Columns.Clear();
                gvExport.OptionsBehavior.AutoPopulateColumns = true;
                gcExport.DataSource = dtTarget;
                gvExport.BestFitColumns();

                //2.2重新設定隱藏欄位不輸出(結果全部欄位都要輸出)
                //gvExport.Columns["CPR_PROD_SUBTYPE"].Visible = true;
                //gvExport.Columns["SEQ_NO"].Visible = true;
                //gvExport.Columns["PROD_TYPE"].Visible = true;

                //2.3設定caption
                gvExport.SetColumnCaption("CPR_PROD_SUBTYPE", "契約類別(I指數C黃金R利率S股票)");
                gvExport.SetColumnCaption("CPR_KIND_ID", "契約代號");
                gvExport.SetColumnCaption("CPR_EFFECTIVE_DATE", "系統生效日");
                gvExport.SetColumnCaption("CPR_PRICE_RISK_RATE", "最小風險價格係數(已下市契約之最小風險價格係數顯示空白；有效契約之最小風險價格係數不可為空白)");
                gvExport.SetColumnCaption("CPR_APPROVAL_DATE", "主管機關核准日期");

                //2.3只有detail才有以下這些欄位,有做一個擴充函數,就算沒有這些欄位,設定也不會發生錯誤
                gvExport.SetColumnCaption("CPR_APPROVAL_NUMBER", "主管機關核准文號");
                gvExport.SetColumnCaption("CPR_REMARK", "備註");
                gvExport.SetColumnCaption("CPR_W_TIME", "異動時間");
                gvExport.SetColumnCaption("CPR_W_USER_ID", "異動人員");

                //2.4統一設定欄位靠左靠右把一些欄位靠左(沒影響到excel)
                gvExport.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;//設定全部欄位先置中
                gvExport.SetColumnHAlignment("CPR_APPROVAL_NUMBER", DevExpress.Utils.HorzAlignment.Default);
                gvExport.SetColumnHAlignment("CPR_REMARK", DevExpress.Utils.HorzAlignment.Default);

                //3.gird export to excel
                string excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
                                                            string.Format("{0}_{1}.xlsx", _ProgramID, DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss")));
                XlsxExportOptions options = new XlsxExportOptions();
                options.SheetName = "";//預設為sheet1
                options.ShowGridLines = true;
                gvExport.ExportToXlsx(excelDestinationPath, options);

                if (FlagAdmin)
                    System.Diagnostics.Process.Start(excelDestinationPath);

                return ResultStatus.Success;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            finally {
                panFilter.Enabled = true;
                labMsg.Text = "";
                labMsg.Visible = false;
            }
            return ResultStatus.Fail;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            try {
                //ReportHelper reportHelper = new ReportHelper(PrintableComponent, _ReportID, _ReportTitle);
                //reportHelper.FilePath = _DefaultFileNamePath;
                //reportHelper.FileType = FileType.PDF;
                //reportHelper.IsPrintedFromPrintButton = true;

                ReportHelper _ReportHelper = reportHelper;

                if (ddlData.EditValue.AsString() == "KeyInfo") {
                    //重點資料
                    CommonReportPortraitA4 reportPortrait = new CommonReportPortraitA4();//設定為直向列印
                    reportPortrait.printableComponentContainerMain.PrintableComponent = gcMain;
                    reportPortrait.IsHandlePersonVisible = false;
                    reportPortrait.IsManagerVisible = false;
                    _ReportHelper.Create(reportPortrait);
                }
                else {
                    //明細資料,欄位比較多
                    CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
                    reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
                    reportLandscape.IsHandlePersonVisible = false;
                    reportLandscape.IsManagerVisible = false;
                    _ReportHelper.Create(reportLandscape);
                }

                _ReportHelper.LeftMemo = labDate.Text + txtSDate.Text;//寫一行標題的註解,通常是查詢條件

                _ReportHelper.Print();//如果有夜盤會特別標註

                return ResultStatus.Success;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }



        private void ddlSubType_EditValueChanged(object sender, EventArgs e) {
            DevExpress.XtraEditors.LookUpEdit ddl = (sender as DevExpress.XtraEditors.LookUpEdit);

            ddlKind.Visible = labKind.Visible = (ddl.Text == ChooseSingleKind ? true : false);
        }

        private void ddlData_EditValueChanged(object sender, EventArgs e) {
            //清除grid data
            dtTarget = null;
            gcMain.DataSource = dtTarget;
            gcMain.Visible = false;
        }
    }
}