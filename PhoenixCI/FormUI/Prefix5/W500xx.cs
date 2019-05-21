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

namespace PhoenixCI.FormUI.Prefix5
{
   public partial class W500xx : FormParent
   {
      public D500xx _D500Xx { get; set; }
      private DataTable _Data { get; set; }
      private ABRK daoABRK;
      private APDK daoAPDK;

      public W500xx(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         daoABRK = new ABRK();
         daoAPDK = new APDK();
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
            PbFunc.messageBox(GlobalInfo.ErrorText, "造市者代號起始不可大於迄止", MessageBoxIcon.Stop);

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
         _D500Xx.SumType = ReportSumType(gbReportType.EditValue.ToString());
         _D500Xx.SortType = PrintSortType(gbPrintSort.ToString());
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
            PbFunc.messageBox(GlobalInfo.ResultText, "無任何資料!", MessageBoxIcon.Information);
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
            case "M":
               /* 只有月份 */
               gbReportType.EditValue = "rb_month";
               gpDate.Visibility = LayoutVisibility.Never;
               gbReportType.Visible = false;
               break;
            case "m":
               gbReportType.EditValue = "rb_month";
               gbReportType.Visible = false;
               gpDate.Visibility = LayoutVisibility.Never;
               /* 無迄止值 */
               emEndYM.Visible = false;
               stMonth.Visibility = LayoutVisibility.Never;
               break;
            case "D":
               /* 只有日期 */
               gbReportType.EditValue = "rb_date";
               gpMonth.Visibility = LayoutVisibility.Never;
               gbReportType.Visible = false;
               break;
            case "d":
               /* 只有日期 */
               gbReportType.EditValue = "rb_date";
               gpMonth.Visibility = LayoutVisibility.Never;
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

      private void WfGbPrintSort(bool VisibleValue, bool EnableValue, string AsType)
      {
         gbPrintSort.Visible = VisibleValue;
         gb4.Visible = VisibleValue;
         gbPrintSort.Enabled = EnableValue;

         switch (AsType) {
            case "1":
               gbPrintSort.EditValue = "rb_mmk";
               break;

            case "2":
               gbPrintSort.EditValue = "rb_prod";
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


      public override ResultStatus BeforeOpen()
      {
         if(!PbFunc.f_chk_run_timing(_ProgramID))
            MessageDisplay.Info("今日盤後轉檔作業還未完畢!");

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         //Input Condition
         emEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         emStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
         emStartYM.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         emEndYM.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         /* 造市者代號 */
         //起始選項
         dwSbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         //目的選項
         dwEbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         /* 商品群組 */
         dwProdCt.SetDataTable(daoAPDK.ListParamKey(), "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.Standard, null);
         /* 造市商品 */
         dwProdKd.SetDataTable(daoAPDK.ListAll3(), "PDK_KIND_ID", "PDK_KIND_ID", TextEditStyles.Standard, null);
         /* 2碼商品 */
         dwProdKdSto.SetDataTable(daoAPDK.ListKind2(), "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.Standard, null);
         //預設資料表
         _D500Xx.TableName = "AMM0";

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         _ToolBtnRetrieve.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         _Data.Clear();
         documentViewer1.DocumentSource = null;
         return ResultStatus.Success;
      }
   }
}