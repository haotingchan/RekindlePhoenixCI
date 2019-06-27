using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using System.IO;
using DataObjects.Dao.Together;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraReports.UI;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using DevExpress.XtraPrinting;
using static BaseGround.Report.ReportHelper;
/// <summary>
/// John,20190503,市場詢價資料
/// </summary>
namespace PhoenixCI.FormUI.Prefix5
{
   /// <summary>
   /// 市場詢價資料
   /// </summary>
   public partial class W50020 : FormParent
   {
      private defReport _defReport;
      public D500xx _D500Xx { get; set; }
      private DataTable _Data { get; set; }
      private ABRK daoABRK;
      private APDK daoAPDK;
      private D50020 dao50020;
      /// <summary>
      /// 商品群組 dw_prod_ct setfilter("market_code in ('1',' ')")
      /// </summary>
      private DataTable ProdCtCodeFilter;
      /// <summary>
      /// 商品群組 dw_prod_ct
      /// </summary>
      private DataTable ProdCtData;
      /// <summary>
      /// 造市商品 dw_prod_kd
      /// </summary>
      private DataTable ProdKdData;
      /// <summary>
      /// 造市商品 dw_prod_kd setfilter("market_code in ('1',' ')")
      /// </summary>
      private DataTable ProdKdDataFilter;
      /// <summary>
      /// 2碼商品 dw_prod_kd_sto
      /// </summary>
      private DataTable ProdKdStoData;
      /// <summary>
      /// 2碼商品 dw_prod_kd_sto setfilter("market_code in ('1',' ')")  
      /// </summary>
      private DataTable ProdKdStoDataFilter;

      public W50020(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         daoABRK = new ABRK();
         daoAPDK = new APDK();
         dao50020 = new D50020();
         _D500Xx = new D500xx();
      }

      /// <summary>
      /// 讀取前條件檢核
      /// </summary>
      /// <param name="sbrkno">造市者代號起始</param>
      /// <param name="ebrkno">造市者代號迄止</param>
      /// <returns></returns>
      private bool StartRetrieve(string sbrkno = "", string ebrkno = "")
      {
         /*******************
         條件值檢核
         *******************/
         _D500Xx.IsCheck = "N";
         /*造市者代號 */
         _D500Xx.Sbrkno = dwSbrkno.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.Sbrkno)) {
            _D500Xx.Sbrkno = sbrkno;
         }
         _D500Xx.Ebrkno = dwEbrkno.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.Ebrkno)) {
            _D500Xx.Ebrkno = ebrkno;
         }
         if ((string.Compare(dwSbrkno.SelectedText, dwEbrkno.SelectedText) > 0) && !string.IsNullOrEmpty(_D500Xx.Ebrkno)) {
            MessageDisplay.Error("造市者代號起始不可大於迄止");

            dwEbrkno.Focus();
            _D500Xx.IsCheck = "Y";
            return false;
         }

         /* 商品群組 */
         _D500Xx.ProdCategory = dwProdCt.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.ProdCategory) || dwProdCt.Enabled == false) {
            _D500Xx.ProdCategory = "";
         }

         /* 商品 */
         _D500Xx.ProdKindId = dwProdKd.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.ProdKindId) || dwProdKd.Enabled == false) {
            _D500Xx.ProdKindId = "";
         }
         _D500Xx.ProdKindIdSto = dwProdKdSto.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.ProdKindIdSto) || dwProdKdSto.Enabled == false) {
            _D500Xx.ProdKindIdSto = "";
         }
         //DateTime dtDate;
         /* 月報表 */
         if (gbReportType.EditValue.Equals("rb_month")) {
            if (emStartYM.Visible == true) {
               if (!emStartYM.IsDate(emStartYM.Text + "/01", CheckDate.Start)) {
                  _D500Xx.IsCheck = "Y";
                  return false;
               }
               _D500Xx.Sdate = emStartYM.Text.Replace("/", "").SubStr(0, 6);

            }
            if (emEndYM.Visible == true) {
               if (!emEndYM.IsDate(emEndYM.Text + "/01", CheckDate.End)) {
                  _D500Xx.IsCheck = "Y";
                  return false;
               }
               _D500Xx.Edate = emEndYM.Text.Replace("/", "").SubStr(0, 6);

            }
         }
         /* 日報表 */
         else {
            if (emStartDate.Visible == true) {
               if (!emStartDate.IsDate(emStartDate.Text, CheckDate.Start)) {
                  _D500Xx.IsCheck = "Y";
                  return false;
               }
            }
            _D500Xx.Sdate = emStartDate.Text.Replace("/", "").SubStr(0, 8);
            if (emEndDate.Visible == true) {
               if (!emEndDate.IsDate(emEndDate.Text, CheckDate.End)) {
                  _D500Xx.IsCheck = "Y";
                  return false;
               }
               _D500Xx.Edate = emEndDate.Text.Replace("/", "").SubStr(0, 8);
            }
         }
         _D500Xx.SumType = ReportSumType(gbReportType.EditValue.AsString());
         _D500Xx.SortType = PrintSortType(gbPrintSort.EditValue.AsString());
         _D500Xx.SumSubType = GrpSubType(gbGroup.EditValue.AsString());
         /*******************
         資料類別
         *******************/
         _D500Xx.DataType = "R";

         /*******************
         條件值檢核OK
         *******************/
         _D500Xx.IsCheck = "Y";


         /*******************
         //Local Window 
         條件值檢核
         if		is_chk <> 'E'	then
               is_chk = 'Y'
         end	if

         資料類別:
         報價:
         is_data_type = 'Q' 
         詢價:
         is_data_type = 'R' 
         *******************/
         return true;
      }

      /// <summary>
      /// 匯出轉檔前的狀態顯示
      /// </summary>
      /// <param name="RptID">程式代號</param>
      /// <param name="RptName">報表名稱</param>
      /// <param name="ls_param_key">契約</param>
      /// <param name="li_ole_col">欄位位置</param>
      private void StartExport(string RptID, string RptName)
      {
         /*************************************
        ls_rpt_name = 報表名稱
        ls_rpt_id = 報表代號
        li_ole_col = 欄位位置
        ls_param_key = 契約
        *************************************/
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         stMsgTxt.Text = RptID + "－" + RptName + " 轉檔中...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
      }

      /// <summary>
      /// 轉檔結束後
      /// </summary>
      private void EndExport(string msg = "轉檔完成!")
      {
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
         this.Cursor = Cursors.Arrow;
      }

      /// <summary>
      /// 顯示選取條件
      /// </summary>
      /// <returns></returns>
      private string ConditionText()
      {
         string lsText;
         /*******************
         顯示條件
         *******************/
         if (gbMarket.EditValue.Equals("rb_market_1")) {
            lsText = "盤後交易時段";
         }
         else {
            lsText = "一般交易時段";
         }
         if (!string.IsNullOrEmpty(_D500Xx.Sbrkno) || !string.IsNullOrEmpty(_D500Xx.Ebrkno)) {
            lsText = lsText + ",造市者:";

            if (_D500Xx.Sbrkno == _D500Xx.Ebrkno) {
               lsText = lsText + _D500Xx.Sbrkno + " ";
            }
            else {

               lsText = lsText + _D500Xx.Sbrkno + "～" + _D500Xx.Ebrkno + " ";
            }
         }

         if (!string.IsNullOrEmpty(_D500Xx.ProdCategory)) {
            lsText = lsText + ",商品群組:" + _D500Xx.ProdCategory;
         }
         if (!string.IsNullOrEmpty(_D500Xx.ProdKindIdSto)) {
            lsText = lsText + ",2碼商品(個股):" + _D500Xx.ProdKindIdSto;
         }
         if (!string.IsNullOrEmpty(_D500Xx.ProdKindId)) {
            lsText = lsText + ",造市商品:" + _D500Xx.ProdKindId;
         }
         if (PbFunc.Left(lsText, 1) == ",") {
            lsText = PbFunc.Mid(lsText, 1, 50);
         }
         if (!string.IsNullOrEmpty(lsText)) {
            lsText = "報表條件：" + lsText;
         }
         return lsText;
      }

      /// <summary>
      /// 顯示日期條件
      /// </summary>
      /// <returns></returns>
      private string DateText()
      {
         /*******************
         顯示條件
         *******************/
         string lsText = "";
         if (!string.IsNullOrEmpty(_D500Xx.Sdate)) {
            lsText = lsText + _D500Xx.Sdate;
         }
         if (!string.IsNullOrEmpty(_D500Xx.Edate)) {
            lsText = lsText + '～' + _D500Xx.Edate;
         }
         return lsText;
      }

      /// <summary>
      /// 統計類別
      /// </summary>
      private string ReportSumType(string rptVal)
      {
         string type = string.Empty;
         if (rptVal.Equals("rb_month")) {
            type = "M";
         }
         else if (rptVal.Equals("rb_date")) {
            type = "D";
         }
         return type;
      }

      /// <summary>
      /// Sort順序
      /// </summary>
      private string PrintSortType(string sortVal)
      {
         string type = string.Empty;
         if (sortVal.Equals("rb_mmk")) {
            type = "F";
         }
         else {
            type = "P";
         }
         return type;
      }

      /// <summary>
      /// 統計子類別
      /// </summary>
      private string GrpSubType(string type)
      {
         string subtype = string.Empty;
         switch (type) {
            case "rb_gall":
               subtype = "1";
               break;
            case "rb_gparam":
               subtype = "3";
               break;
            case "rb_s":
               subtype = "S";
               break;
            case "rb_gkind2":
               subtype = "4";
               break;
            case "rb_gkind":
               subtype = "5";
               break;
            case "rb_gprod":
               subtype = "6";
               break;
            default:
               break;
         }
         return subtype;
      }

      /// <summary>
      /// 轉檔失敗刪除檔案
      /// </summary>
      private void WfRunError()
      {
         /*******************
         Messagebox
         *******************/
         //SetPointer(Arrow!);
         this.Cursor = Cursors.Arrow;
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "轉檔有錯誤!";

         if (File.Exists(_D500Xx.Filename))
            File.Delete(_D500Xx.Filename);
      }

      /// <summary>
      /// 報表資料篩選
      /// </summary>
      /// <returns></returns>
      protected bool GetData()
      {
         /* 報表內容 */
         //報表內容選擇分日期
         if (gbDetial.EditValue.Equals("rb_gdate")) {

            //交易時段選盤後
            if (gbMarket.EditValue.Equals("rb_market_1")) {
               _Data = dao50020.ListAH(_D500Xx);
            }
            else {
               _Data = dao50020.List50020(_D500Xx);
            }
         }
         else {

            //交易時段選盤後
            if (gbMarket.EditValue.Equals("rb_market_1")) {
               _Data = dao50020.ListACCUAH(_D500Xx);
            }
            else {
               _Data = dao50020.ListACCU(_D500Xx);
            }
         }

         if (_Data.Rows.Count <= 0) {
            documentViewer1.DocumentSource = null;
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            return false;
         }
         return true;
      }

      /// <summary>
      /// 顯示報表
      /// </summary>
      /// <param name="report"></param>
      private void ShowReport(XtraReport report)
      {
         documentViewer1.DocumentSource = report;
         report.CreateDocument(true);
      }

      /// <summary>
      /// 顯示Label訊息
      /// </summary>
      /// <param name="msg"></param>
      protected void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      private void gbDetial_EditValueChanged(object sender, EventArgs e)
      {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;

         switch (rb.EditValue.ToString()) {
            case "rb_detail":
               layoutControl1.Enabled = false;
               gbReportType.EditValue = "rb_date";
               gbReportType.Enabled = false;
               break;
            default:
               layoutControl1.Enabled = true;
               gbReportType.Enabled = true;
               break;
         }
      }

      private void gbGroup_EditValueChanged(object sender, EventArgs e)
      {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;
         _D500Xx.GbGroup = rb.EditValue.ToString();
         DataTable dt = gbMarket.EditValue.Equals("rb_market_0") ? ProdCtData : ProdCtCodeFilter;
         dwProdCt.Properties.DataSource = dt;

         switch (_D500Xx.GbGroup) {
            case "rb_gall":
               dwProdCt.Enabled = false;
               stProdCt.Enabled = false;

               dwProdKdSto.Enabled = false;
               stProdKdSto.Enabled = false;

               dwProdKd.Enabled = false;
               stProdKd.Enabled = false;
               break;
            case "rb_gparam":
               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = false;
               stProdKdSto.Enabled = false;

               dwProdKd.Enabled = false;
               stProdKd.Enabled = false;
               break;
            case "rb_s":
               //統計依照"股票各類群組"時僅開放"商品群組"選單，且選單內容僅提供APDK_PROD_SUBTYPE='S'的商品
               dwProdCt.SelectedText = "";
               DataTable dtFilter = dt.Filter("APDK_PROD_SUBTYPE='S'");
               dwProdCt.Properties.DataSource = dtFilter;
               if (dtFilter.Rows.Count <= 0) {
                  dtFilter.Rows.Add();
                  dwProdCt.Properties.DataSource = dtFilter;
               }

               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = false;
               stProdKdSto.Enabled = false;

               dwProdKd.Enabled = false;
               stProdKd.Enabled = false;
               break;
            case "rb_gkind2":
               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = true;
               stProdKdSto.Enabled = true;

               dwProdKd.Enabled = false;
               stProdKd.Enabled = false;
               break;
            case "rb_gkind":
               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = true;
               stProdKdSto.Enabled = true;

               dwProdKd.Enabled = true;
               stProdKd.Enabled = true;
               break;
            case "rb_gprod":
               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = true;
               stProdKdSto.Enabled = true;

               dwProdKd.Enabled = true;
               stProdKd.Enabled = true;
               break;
            default:
               break;
         }

      }

      private void gbMarket_EditValueChanged(object sender, EventArgs e)
      {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;
         switch (rb.EditValue.ToString()) {
            case "rb_market_0":
               //商品群組
               dwProdCt.SelectedText = "";
               dwProdCt.Properties.DataSource = _D500Xx.GbGroup == "rb_s" ? ProdCtData.Filter("APDK_PROD_SUBTYPE='S'") : ProdCtData;//統計依照"股票各類群組"時僅開放"商品群組"選單，且選單內容僅提供APDK_PROD_SUBTYPE='S'的商品
               //2碼商品
               dwProdKdSto.SelectedText = "";
               dwProdKdSto.Properties.DataSource = ProdKdStoData;
               //造市商品
               dwProdKd.SelectedText = "";
               dwProdKd.Properties.DataSource = ProdKdData;
               break;
            case "rb_market_1":
               //商品群組
               dwProdCt.SelectedText = "";
               dwProdCt.Properties.DataSource = _D500Xx.GbGroup == "rb_s" ? ProdCtCodeFilter.Filter("APDK_PROD_SUBTYPE='S'") : ProdCtCodeFilter;//統計依照"股票各類群組"時僅開放"商品群組"選單，且選單內容僅提供APDK_PROD_SUBTYPE='S'的商品
               //2碼商品
               dwProdKdSto.SelectedText = "";
               dwProdKdSto.Properties.DataSource = ProdKdStoDataFilter;
               //造市商品
               dwProdKd.SelectedText = "";
               dwProdKd.Properties.DataSource = ProdKdDataFilter;
               break;
            default:
               break;
         }

         DataTable dt = (DataTable)dwProdCt.Properties.DataSource;
         if (dt.Rows.Count <= 0) {
            dt.Rows.Add();
            dwProdCt.Properties.DataSource = dt;
         }

      }

      protected override ResultStatus Open()
      {
         base.Open();
         /*******************
         Input Condition
         *******************/
         //GlobalInfo.OCF_DATE = serviceCommon.GetOCF().OCF_DATE;
         emEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         emStartDate.DateTimeValue = new DateTime(GlobalInfo.OCF_DATE.Year, GlobalInfo.OCF_DATE.Month, 01);
         emStartYM.DateTimeValue = GlobalInfo.OCF_DATE;
         emEndYM.DateTimeValue = GlobalInfo.OCF_DATE;
         /* 造市者代號 */
         //起始選項
         dwSbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         //目的選項
         dwEbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);

         string marketcodefilter = "MARKET_CODE in ('1',' ')";
         /* 商品群組 */
         ProdCtData = daoAPDK.ListParamKey();
         dwProdCt.SetDataTable(ProdCtData, "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.Standard, null);
         ProdCtCodeFilter = ProdCtData.Filter(marketcodefilter);
         /* 造市商品 */
         ProdKdData = daoAPDK.ListAll3();
         dwProdKd.SetDataTable(ProdKdData, "PDK_KIND_ID", "PDK_KIND_ID", TextEditStyles.Standard, null);
         ProdKdDataFilter = ProdKdData.Filter(marketcodefilter);
         /* 2碼商品 */
         ProdKdStoData = daoAPDK.ListKind2();
         dwProdKdSto.SetDataTable(ProdKdStoData, "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.Standard, null);
         ProdKdStoDataFilter = ProdKdStoData.Filter(marketcodefilter);
         //預設資料表
         _D500Xx.TableName = "AMM0";

         _D500Xx.GbGroup = gbGroup.EditValue.ToString();

         gbGroup.EditValueChanged += gbGroup_EditValueChanged;
         gbDetial.EditValueChanged += gbDetial_EditValueChanged;
         gbMarket.EditValueChanged += gbMarket_EditValueChanged;
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         if (gbDetial.EditValue.Equals("rb_detail")) {
            MessageDisplay.Warning("明細資料僅提供「轉出」CSV檔案，請按轉出功能！");
            return ResultStatus.Fail;
         }
         if (!StartRetrieve()) return ResultStatus.Fail;

         if (!GetData()) return ResultStatus.Fail;

         List<ReportProp> caption = new List<ReportProp>{
            new ReportProp{DataColumn="AMM0_YMD",Caption= "日期" ,CellWidth=gbDetial.EditValue.Equals("rb_gnodate")?120:65,DetailRowFontSize=gbDetial.EditValue.Equals("rb_gnodate")?8:9,HeaderFontSize=11},
            new ReportProp{DataColumn="AMM0_BRK_NO",Caption= "期貨商        代號",CellWidth=80,DetailRowFontSize=10,HeaderFontSize=11},
            new ReportProp{DataColumn="BRK_ABBR_NAME",Caption= "期貨商名稱" ,CellWidth=150,DetailRowFontSize=9.75f,HeaderFontSize=11},
            new ReportProp{DataColumn="AMM0_CNT",Caption= "詢價筆數",CellWidth=gbDetial.EditValue.Equals("rb_gnodate")?90:120,textAlignment=TextAlignment.MiddleRight,DetailRowFontSize=11,HeaderFontSize=11},
            new ReportProp{DataColumn="CP_RATE_VALID_CNT",Caption= "佔全市場            詢價比例(%)",CellWidth=100,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:##0.0#}",DetailRowFontSize=11,HeaderFontSize=11},
            new ReportProp{DataColumn="AMM0_MARKET_R_CNT",Caption= "全市場                詢價筆數",CellWidth=120,textAlignment=TextAlignment.MiddleRight,DetailRowFontSize=11,HeaderFontSize=11 }
            };

         //商品名稱會根據列印順序有所不同
         ReportProp productName = new ReportProp { DataColumn = "AMM0_PROD_ID", Caption = "商品名稱", CellWidth = 120, DetailRowFontSize = 11, HeaderFontSize = 11 };
         if (_D500Xx.SortType == "P") {
            caption.Insert(1, productName);//選擇商品，商品名稱會位於第二欄
         }
         else {
            caption.Insert(3, productName);//選擇造勢者，商品名稱會位於第四欄
         }

         _defReport = new defReport(_Data, caption);
         ShowReport(_defReport);

         //WriteLog("查詢資料 " + _D500Xx.LogText, "query", "R");
         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         string lsRptName = ConditionText().Trim();
         StartExport(_ProgramID, lsRptName);

         //開啟檔案
         Workbook workbook = new Workbook();

         try {
            if (gbDetial.EditValue.Equals("rb_detail")) {
               DataTable dt;
               if (gbMarket.EditValue.Equals("rb_market_1")) {
                  dt = dao50020.List50020dAH(emStartDate.DateTimeValue, emEndDate.DateTimeValue);
               }
               else {
                  dt = dao50020.List50020d(emStartDate.DateTimeValue, emEndDate.DateTimeValue);
               }

               if (dt.Rows.Count <= 0) {
                  MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
                  return ResultStatus.Fail;
               }

               dt.Columns["DATA_DATE"].ColumnName = "交易日期";
               dt.Columns["FCM"].ColumnName = "期貨商";
               dt.Columns["PROD"].ColumnName = "商品";
               dt.Columns["SEND_TIME"].ColumnName = "詢價送出時間";

               string csvFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, $"50020_detail_{DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss")}.csv");
               workbook.CreateNewDocument();
               workbook.SaveDocument(csvFilePath, DocumentFormat.Csv);
               workbook.Options.Export.Csv.WritePreamble = true;//不加這段中文會是亂碼
               workbook.Worksheets[0].Import(dt, true, 0, 0);
               workbook.SaveDocument(csvFilePath);
               return ResultStatus.Success;
            }

            Retrieve();

            //讀取資料
            if (_Data.Rows.Count <= 0) {
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
               return ResultStatus.Fail;
            }

            //複製檔案
            _D500Xx.Filename = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
            if (_D500Xx.Filename == "") {
               return ResultStatus.Fail;
            }
            _D500Xx.LogText = _D500Xx.Filename;

            if (lsRptName == "") {
               lsRptName = "報表條件：" + "(" + DateText() + ")";
            }
            else {
               lsRptName = ConditionText().Trim() + " " + "(" + DateText() + ")";
            }

            workbook.LoadDocument(_D500Xx.Filename);
            //切換Sheet
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Cells["E3"].Value = lsRptName;
            
            worksheet.Import(_Data, false, 4, 0);
            workbook.SaveDocument(_D500Xx.Filename);
         }
         catch (Exception ex) {
            WriteLog(ex);
            WfRunError();
         }
         finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         ShowMsg("列印中...");
         try {
            CommonReportPortraitA4 reportPortraitA4 = new CommonReportPortraitA4();
            if (_defReport == null)
               _defReport = new defReport();

            _defReport.SetMemoInPageFooter("註：★代表收盤前40秒間詢價");
            XtraReport xtraReport = reportHelper.CreateCompositeReport(_defReport, reportPortraitA4);
            string dateCondition = DateText() == "" ? "" : "," + DateText();
            reportHelper.LeftMemo = ConditionText() + dateCondition;
            reportHelper.Create(xtraReport);

            reportHelper.Print(reportHelper.MainReport);
            ShowReport(_defReport);
         }
         catch (Exception ex) {
            WriteLog(ex);
            throw ex;
         }
         finally {
            EndExport("列印完成!");
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         _Data = null;
         documentViewer1.DocumentSource = null;
         return ResultStatus.Success;
      }

   }
}