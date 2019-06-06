using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraLayout.Utils;
using System.IO;
using System;
using DevExpress.XtraReports.UI;
using DevExpress.Spreadsheet;
/// <summary>
/// John,20190602,造市者到期月份成交概況
/// </summary>
namespace PhoenixCI.FormUI.Prefix5
{
   /// <summary>
   /// 造市者到期月份成交概況
   /// </summary>
   public partial class W50031 : FormParent
   {
      public D500xx _D500Xx { get; set; }
      private DataTable _Data { get; set; }
      private ABRK daoABRK;
      private APDK daoAPDK;
      private D50031 dao50031;
      /// <summary>
      /// 商品群組 dw_prod_ct setfilter("market_code in ('1',' ')")
      /// </summary>
      private DataTable ProdCtCodeFilter;
      /// <summary>
      /// 商品群組 dw_prod_ct
      /// </summary>
      private DataTable ProdCtData;
      /// <summary>
      /// 2碼商品 dw_prod_kd_sto
      /// </summary>
      private DataTable ProdKdStoData;
      /// <summary>
      /// 2碼商品 dw_prod_kd_sto setfilter("market_code in ('1',' ')")  
      /// </summary>
      private DataTable ProdKdStoDataFilter;

      public W50031(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         daoABRK = new ABRK();
         daoAPDK = new APDK();
         _D500Xx = new D500xx();
         dao50031 = new D50031();
      }

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

         _D500Xx.ProdKindIdSto = dwProdKdSto.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.ProdKindIdSto) || dwProdKdSto.Enabled == false) {
            _D500Xx.ProdKindIdSto = "";
         }
         /* 日報表 */
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
         _D500Xx.SumType = ReportSumType(gbReportType.EditValue.ToString());
         _D500Xx.SumSubType = GrpSubType(gbGroup.EditValue.AsString());
         /*******************
         資料類別
         *******************/
         _D500Xx.DataType = "Q";

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

      private bool EndRetrieve(DataTable dt)
      {
         if (dt.Rows.Count <= 0) {
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            return false;
         }
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
      private void EndExport()
      {
         stMsgTxt.Text = "轉檔完成!";
         this.Refresh();
         Thread.Sleep(5);
         //is_time = is_time + "～" + DateTime.Now;
         //PbFunc.messageBox(GlobalInfo.gs_t_result + " " + is_time, "轉檔完成!", MessageBoxIcon.Information);
         stMsgTxt.Visible = false;
         this.Cursor = Cursors.Arrow;
      }

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

      private void WfGbReportType(string AsType)
      {

         switch (AsType) {
            case "D":
               /* 只有日期 */
               gbReportType.EditValue = "rb_date";
               gbReportType.Visible = false;
               break;
            case "d":
               /* 只有日期 */
               gbReportType.EditValue = "rb_date";
               gbReportType.Visible = false;
               /* 無迄止值 */
               stDate.Visibility = LayoutVisibility.Never;
               emEndDate.Visible = false;
               break;
            default:
               break;
         }
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

      private void WfGbGroup(bool VisibleValue, bool EnableValue, string AsType)
      {
         gbGroup.Visible = VisibleValue;
         gb2.Visible = VisibleValue;
         gbGroup.Enabled = EnableValue;

         switch (AsType) {
            case "1":
               gbGroup.EditValue = "rb_gall";
               break;
            case "2":
               gbGroup.EditValue = "rb_gparam";
               break;
            case "3":
               gbGroup.EditValue = "rb_gkind";
               break;
            case "4":
               gbGroup.EditValue = "rb_gkind2";
               break;
            case "5":
               gbGroup.EditValue = "rb_gprod";
               break;
            default:
               break;
         }
      }

      private void WfGrpDetial(bool VisibleValue, bool EnableValue, string AsType)
      {
         gbDetial.Visible = VisibleValue;
         gb3.Visible = VisibleValue;
         gbDetial.Enabled = EnableValue;

         switch (AsType) {
            case "1":
               gbDetial.EditValue = "rb_gdate";
               break;

            case "2":
               gbDetial.EditValue = "rb_gnodate";
               break;
            default:
               break;
         }
      }

      private void WfRunError()
      {
         /*******************
         Messagebox
         *******************/
         //SetPointer(Arrow!);
         this.Cursor = Cursors.Arrow;
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "轉檔有錯誤!";

         File.Delete(_D500Xx.Filename);
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
               break;
            case "rb_gparam":
               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = false;
               stProdKdSto.Enabled = false;
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
               break;
            case "rb_gkind2":
               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = true;
               stProdKdSto.Enabled = true;
               break;
            case "rb_gkind":
               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = true;
               stProdKdSto.Enabled = true;
               break;
            case "rb_gprod":
               dwProdCt.Enabled = true;
               stProdCt.Enabled = true;

               dwProdKdSto.Enabled = true;
               stProdKdSto.Enabled = true;
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
               break;
            case "rb_market_1":
               //商品群組
               dwProdCt.SelectedText = "";
               dwProdCt.Properties.DataSource = _D500Xx.GbGroup == "rb_s" ? ProdCtCodeFilter.Filter("APDK_PROD_SUBTYPE='S'") : ProdCtCodeFilter;//統計依照"股票各類群組"時僅開放"商品群組"選單，且選單內容僅提供APDK_PROD_SUBTYPE='S'的商品
               //2碼商品
               dwProdKdSto.SelectedText = "";
               dwProdKdSto.Properties.DataSource = ProdKdStoDataFilter;
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

      public override ResultStatus BeforeOpen()
      {
         if (!PbFunc.f_chk_run_timing(_ProgramID))
            MessageDisplay.Info("今日盤後轉檔作業還未完畢!");

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         /*******************
         Input Condition
         *******************/
         //GlobalInfo.OCF_DATE = serviceCommon.GetOCF().OCF_DATE;
         emEndDate.EditValue = GlobalInfo.OCF_DATE;
         emStartDate.EditValue = new DateTime(GlobalInfo.OCF_DATE.Year, GlobalInfo.OCF_DATE.Month, 01);
#if DEBUG
         emEndDate.EditValue = new DateTime(2018, 06, 15);
         emStartDate.EditValue = new DateTime(2018, 06, 01);
#endif
         /* 造市者代號 */
         DataTable FcmAccNo = new AMPD().ListByFcmAccNo();
         //起始選項
         dwSbrkno.SetDataTable(FcmAccNo, "AMPD_FCM_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         //目的選項
         dwEbrkno.SetDataTable(FcmAccNo, "AMPD_FCM_NO", "CP_DISPLAY", TextEditStyles.Standard, null);

         string marketcodefilter = "MARKET_CODE in ('1',' ')";
         /* 商品群組 */
         ProdCtData = daoAPDK.ListParamKey();
         dwProdCt.SetDataTable(ProdCtData, "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.Standard, null);
         ProdCtCodeFilter = ProdCtData.Filter(marketcodefilter);
         /* 2碼商品 */
         ProdKdStoData = daoAPDK.ListKind2();
         dwProdKdSto.SetDataTable(ProdKdStoData, "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.Standard, null);
         ProdKdStoDataFilter = ProdKdStoData.Filter(marketcodefilter);
         //預設資料表
         _D500Xx.TableName = "AMM0";

         _D500Xx.GbGroup = gbGroup.EditValue.ToString();

         gbGroup.EditValueChanged += gbGroup_EditValueChanged;
         gbMarket.EditValueChanged += gbMarket_EditValueChanged;
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         base.Export();
         string ls_rpt_name = "造市者報表";
         string ls_rpt_id = _ProgramID;
         StartExport(ls_rpt_id, ls_rpt_name);

         StartRetrieve("0000000", "Z999999");
         _D500Xx.Filename = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + ".csv");
         try {
            /******************
      開啟檔案
      ******************/
            Workbook workbook = new Workbook();
            //判斷檔案是否存在,不存在就開一個新檔案
            if (!File.Exists(_D500Xx.Filename)) {
               File.Create(_D500Xx.Filename).Close();
            }

            workbook.LoadDocument(_D500Xx.Filename);
            /* 商品群組 */
            string isKey = dwProdCt.SelectedText.Trim() + "%";
            if (string.IsNullOrEmpty(isKey) || dwProdCt.Enabled == false) {
               isKey = "%";
            }
            /* 商品 */
            string kindID2 = dwProdKdSto.SelectedText.Trim() + "%";
            if (string.IsNullOrEmpty(kindID2) || dwProdKdSto.Enabled == false) {
               kindID2 = "%";
            }
            /*******************
            統計子類別
            *******************/
            string prodSubtype = "%";
            string sumSubtype = "";
            if (gbGroup.EditValue.Equals("rb_gparam")) {
               sumSubtype = "3";
            }
            else if (gbGroup.EditValue.Equals("rb_s")) {
               sumSubtype = "S";

               prodSubtype = "S";
            }
            else if (gbGroup.EditValue.Equals("rb_gkind2")) {
               sumSubtype = "4";
            }
            /*******************
            交易時段
            *******************/
            int marketCode = 0;
            if (gbMarket.EditValue.Equals("rb_market_1")) {
               marketCode = 1;
            }

            if (gbMarket.EditValue.Equals("rb_market_0")) {
               marketCode = 0;
            }
            /*******************
            報表內容
            *******************/
            string detailType = "";
            if (gbDetial.EditValue.Equals("rb_gdate")) {
               detailType = "D";
            }

            if (gbDetial.EditValue.Equals("rb_gnodate")) {
               detailType = "N";
            }
            /*******************
            顯示條件
            *******************/
            string lsText = "";
            if (gbMarket.EditValue.Equals("rb_market_1")) {
               lsText = "盤後交易時段";
            }
            else {
               lsText = "一般交易時段";
            }
            if (!string.IsNullOrEmpty(_D500Xx.Sdate)) {
               lsText = lsText + "(" + _D500Xx.Sdate;
            }
            if (!string.IsNullOrEmpty(_D500Xx.Edate)) {
               lsText = lsText + "~" + _D500Xx.Edate + ")";
            }
            if (_D500Xx.Sbrkno != "%" || _D500Xx.Ebrkno != "%") {
               lsText = lsText + ",造市者:";
               if (_D500Xx.Sbrkno == _D500Xx.Ebrkno) {
                  lsText = lsText + _D500Xx.Sbrkno + " ";
               }
               else {
                  lsText = lsText + _D500Xx.Sbrkno + "~" + _D500Xx.Ebrkno + " ";
               }
            }

            if (isKey != "%") {
               lsText = lsText + ",商品群組:" + isKey;
            }
            if (kindID2 != "%") {
               lsText = lsText + ",2碼商品(個股):" + kindID2;
            }

            if (PbFunc.Left(lsText, 1) == ",") {
               lsText = PbFunc.Mid(lsText, 1, 50);
            }
            if (lsText != "") {
               lsText = "報表條件：" + lsText;
            }
            /******************
            讀取資料
            ******************/
            /* 報表內容 */
            _Data = dao50031.ListD50031
               (marketCode, sumSubtype, detailType, _D500Xx.Sdate, _D500Xx.Edate,
               _D500Xx.Sbrkno, _D500Xx.Ebrkno, isKey, prodSubtype, kindID2, lsText);
            if (_Data.Rows.Count <= 0) {
               EndExport();
               return ResultStatus.Success;
            }
            /******************
            切換Sheet
            ******************/
            Worksheet worksheet = workbook.Worksheets[0];
            workbook.Options.Export.Csv.WritePreamble = true;//不加這段中文會是亂碼
                                                             //將DataTable中的數據導入Excel中
            worksheet.Import(_Data, false, 0, 0);
            workbook.SaveDocument(_D500Xx.Filename, DocumentFormat.Csv);
         }
         catch (Exception ex) {
            WfRunError();
            WriteLog(ex);
         }
         finally {
            EndExport();
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         _Data = null;
         return ResultStatus.Success;
      }

   }
}