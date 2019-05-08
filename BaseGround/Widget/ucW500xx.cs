using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using Common;
using DevExpress.XtraLayout.Utils;
using DataObjects.Dao.Together;
using BusinessObjects;
using BaseGround.Report;
using BaseGround.Shared;
using System.Data.Common;
using System.IO;
using DevExpress.Spreadsheet;
using DevExpress.XtraBars;
using System.Data;
using DevExpress.XtraEditors.Controls;
using System.Threading.Tasks;
using System.Threading;
using BusinessObjects.Enums;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BaseGround.Widget
{
   public partial class ucW500xx : DevExpress.XtraEditors.XtraUserControl
   {
      #region 控制項存取子供外部使用
      public GroupBox gb { get { return _ReportType; } set { _ReportType = value; } }
      public GroupBox gb1 { get { return _MarketGrp; } set { _MarketGrp = value; } }
      public GroupBox gb2 { get { return _Grp; } set { _Grp = value; } }
      public GroupBox gb3 { get { return _DetialGrp; } set { _DetialGrp = value; } }
      public GroupBox gb4 { get { return _PrintSort; } set { _PrintSort = value; } }
      public LayoutControlItem st_1 { get { return _st_1; } set { _st_1 = value; } }
      public LookUpEdit dw_prod_kd_sto { get { return _dw_prod_kd_sto; } set { _dw_prod_kd_sto = value; } }
      public LookUpEdit dw_prod_ct { get { return _dw_prod_ct; } set { _dw_prod_ct = value; } }
      public LookUpEdit dw_prod_kd { get { return _dw_prod_kd; } set { _dw_prod_kd = value; } }
      public LookUpEdit dw_ebrkno { get { return _dw_ebrkno; } set { _dw_ebrkno = value; } }
      public LookUpEdit dw_sbrkno { get { return _dw_sbrkno; } set { _dw_sbrkno = value; } }
      public TextEdit emEndDate { get { return _em_edate; } set { _em_edate = value; } }
      public LayoutControlItem st_date { get { return _st_date; } set { _st_date = value; } }
      public TextEdit emStartDate { get { return _em_sdate; } set { _em_sdate = value; } }
      public LayoutControlItem st_month { get { return _st_month; } set { _st_month = value; } }
      public TextEdit em_eym { get { return _em_eym; } set { _em_eym = value; } }
      public TextEdit em_sym { get { return _em_sym; } set { _em_sym = value; } }
      public LayoutControlItem st_prod_kd_sto { get { return _st_prod_kd_sto; } set { _st_prod_kd_sto = value; } }
      public LayoutControlItem st_prod_ct { get { return _st_prod_ct; } set { _st_prod_ct = value; } }
      public LayoutControlItem st_prod_kd { get { return _st_prod_kd; } set { _st_prod_kd = value; } }
      public LayoutControlItem st_2 { get { return _st_2; } set { _st_2 = value; } }
      public RadioGroup gb_report_type { get { return _gb_report_type; } set { _gb_report_type = value; } }
      public RadioGroup gb_print_sort { get { return _gb_print_sort; } set { _gb_print_sort = value; } }
      public RadioGroup gb_group { get { return _gb_group; } set { _gb_group = value; } }
      public RadioGroup gb_detial { get { return _gb_detial; } set { _gb_detial = value; } }
      public PanelControl r_input { get { return _r_input; } set { _r_input = value; } }
      public LabelControl stMsgTxt { get { return _st_msg_txt; } set { _st_msg_txt = value; } }
      public RadioGroup gb_market { get { return _gb_market; } set { _gb_market = value; } }
      public LayoutControlGroup gp_month { get { return _gp_month; } set { _gp_month = value; } }
      public LayoutControlGroup gp_date { get { return _gp_date; } set { _gp_date = value; } }
      #endregion

      #region PB變數存取子供外部使用
      public OCF OCF { get; set; }
      public bool IsOpen { get; set; }
      public string IsCheck { get; set; }
      public string Sbrkno { get; set; }
      public string Ebrkno { get; set; }
      public string ProdKindIdSto { get; set; }
      public string ProdKindId { get; set; }
      public string ProdCategory { get; set; }
      public string Sdate { get; set; }
      public string Edate { get; set; }
      public string SumType { get; set; }
      public string SumSubType { get; set; }
      public string DataType { get; set; }
      public string SortType { get; set; }
      public string TableName { get; set; }
      public string LogText { get; set; }
      public string TxnID { get; set; }
      public string TimeNow { get; set; }
      public string Filename { get; set; }
      private ABRK daoABRK;
      private APDK daoAPDK;
      #endregion
      public ucW500xx()
      {
         InitializeComponent();
         Filename = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH;
         daoABRK = new ABRK();
         daoAPDK = new APDK();
      }

      public void Open()
      {
         /*******************
         Input Condition
         *******************/
         //GlobalInfo.OCF_DATE = serviceCommon.GetOCF().OCF_DATE;

         emEndDate.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         //em_sdate.EditValue = (em_edate.Text.Substring(0, 5) + "01").AsDateTime();
         emStartDate.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
#if DEBUG
         //em_sdate.EditValue = new DateTime(2019, 4, 15).ToString("yyyy/MM/01");
#endif
         em_sym.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         em_eym.EditValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         /* 造市者代號 */
         //起始選項
         dw_sbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         //目的選項
         dw_ebrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         /* 商品群組 */
         dw_prod_ct.SetDataTable(daoAPDK.ListParamKey(), "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.Standard, null);
         /* 造市商品 */
         dw_prod_kd.SetDataTable(daoAPDK.ListAll3(), "PDK_KIND_ID", "PDK_KIND_ID", TextEditStyles.Standard, null);
         /* 2碼商品 */
         dw_prod_kd_sto.SetDataTable(daoAPDK.ListKind2(), "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.Standard, null);
         //預設資料表
         TableName = "AMM0";
      }

      public bool StartRetrieve(string sbrkno = "", string ebrkno = "")
      {
         /*******************
         條件值檢核
         *******************/
         IsCheck = "N";
         /*造市者代號 */
         Sbrkno = dw_sbrkno.EditValue.AsString();
         if (string.IsNullOrEmpty(Sbrkno)) {
            Sbrkno = sbrkno;
         }
         Ebrkno = dw_ebrkno.EditValue.AsString();
         if (string.IsNullOrEmpty(Ebrkno)) {
            Ebrkno = ebrkno;
         }
         if ((string.Compare(dw_sbrkno.SelectedText, dw_ebrkno.SelectedText) > 0) && !string.IsNullOrEmpty(Ebrkno)) {
            PbFunc.messageBox(GlobalInfo.ErrorText, "造市者代號起始不可大於迄止", MessageBoxIcon.Stop);

            dw_sbrkno.Focus();
            IsCheck = "Y";
            return false;
         }

         /* 商品群組 */
         ProdCategory = dw_prod_ct.EditValue.AsString();
         if (string.IsNullOrEmpty(ProdCategory) || dw_prod_ct.Enabled == false) {
            ProdCategory = "";
         }

         /* 商品 */
         ProdKindId = dw_prod_kd.EditValue.AsString();
         if (string.IsNullOrEmpty(ProdKindId) || dw_prod_kd.Enabled == false) {
            ProdKindId = "";
         }
         ProdKindIdSto = dw_prod_kd_sto.EditValue.AsString();
         if (string.IsNullOrEmpty(ProdKindIdSto) || dw_prod_kd_sto.Enabled == false) {
            ProdKindIdSto = "";
         }
         //DateTime dtDate;
         /* 月報表 */
         if (gb_report_type.EditValue.Equals("rb_month")) {
            if (em_sym.Visible == true) {
               if (!em_sym.IsDate(em_sym.Text + "/01", CheckDate.Start)) {
                  IsCheck = "Y";
                  return false;
               }
               Sdate = em_sym.Text.Replace("/", "").SubStr(0, 6);

            }
            if (em_eym.Visible == true) {
               if (!em_eym.IsDate(em_eym.Text + "/01", CheckDate.End)) {
                  IsCheck = "Y";
                  return false;
               }
               Edate = em_eym.Text.Replace("/", "").SubStr(0, 6);

            }
         }
         /* 日報表 */
         else {
            if (emStartDate.Visible == true) {
               if (!emStartDate.IsDate(emStartDate.Text, CheckDate.Start)) {
                  IsCheck = "Y";
                  return false;
               }
            }
            Sdate = emStartDate.Text.Replace("/", "").SubStr(0, 8);
            if (emEndDate.Visible == true) {
               if (!emEndDate.IsDate(emEndDate.Text, CheckDate.End)) {
                  IsCheck = "Y";
                  return false;
               }
               Edate = emEndDate.Text.Replace("/", "").SubStr(0, 8);
            }
         }
         SumType = ReportSumType(gb_report_type.EditValue.ToString());
         SortType = PrintSortType(gb_print_sort.EditValue.ToString());
         SumSubType = GrpSubType(gb_group.EditValue.AsString());
         /*******************
         資料類別
         *******************/
         DataType = "Q";

         /*******************
         條件值檢核OK
         *******************/
         IsCheck = "Y";


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

      public bool EndRetrieve(DataTable dt)
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
      public void StartExport(string RptID, string RptName)
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
         TimeNow = DateTime.Now.ToString();
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
      }

      /// <summary>
      /// 轉檔結束後
      /// </summary>
      public void EndExport()
      {
         stMsgTxt.Text = "轉檔完成!";
         this.Refresh();
         Thread.Sleep(5);
         //is_time = is_time + "～" + DateTime.Now;
         //PbFunc.messageBox(GlobalInfo.gs_t_result + " " + is_time, "轉檔完成!", MessageBoxIcon.Information);
         stMsgTxt.Visible = false;
         this.Cursor = Cursors.Arrow;
      }

      public string ConditionText()
      {
         string lsText;
         /*******************
         顯示條件
         *******************/
         if (gb_market.EditValue.Equals("rb_market_1")) {
            lsText = "盤後交易時段";
         }
         else {
            lsText = "一般交易時段";
         }
         if (!string.IsNullOrEmpty(Sbrkno) || !string.IsNullOrEmpty(Ebrkno)) {
            lsText = lsText + ",造市者:";

            if (Sbrkno == Ebrkno) {
               lsText = lsText + Sbrkno + " ";
            }
            else {

               lsText = lsText + Sbrkno + "～" + Ebrkno + " ";
            }
         }

         if (!string.IsNullOrEmpty(ProdCategory)) {
            lsText = lsText + ",商品群組:" + ProdCategory;
         }
         if (!string.IsNullOrEmpty(ProdKindIdSto)) {
            lsText = lsText + ",2碼商品(個股):" + ProdKindIdSto;
         }
         if (!string.IsNullOrEmpty(ProdKindId)) {
            lsText = lsText + ",造市商品:" + ProdKindId;
         }
         if (PbFunc.Left(lsText, 1) == ",") {
            lsText = PbFunc.Mid(lsText, 1, 50);
         }
         if (!string.IsNullOrEmpty(lsText)) {
            lsText = "報表條件：" + lsText;
         }
         return lsText;
      }

      public string DateText()
      {
         /*******************
         顯示條件
         *******************/
         string lsText = "";
         if (!string.IsNullOrEmpty(Sdate)) {
            lsText = lsText + Sdate;
         }
         if (!string.IsNullOrEmpty(Edate)) {
            lsText = lsText + '～' + Edate;
         }
         return lsText;
      }

      public void WfGbReportType(string AsType)
      {

         switch (AsType) {
            case "M":
               /* 只有月份 */
               gb_report_type.EditValue = "rb_month";
               gp_date.Visibility = LayoutVisibility.Never;
               gb_report_type.Visible = false;
               break;
            case "m":
               gb_report_type.EditValue = "rb_month";
               gb_report_type.Visible = false;
               gp_date.Visibility = LayoutVisibility.Never;
               /* 無迄止值 */
               em_eym.Visible = false;
               st_month.Visibility = LayoutVisibility.Never;
               break;
            case "D":
               /* 只有日期 */
               gb_report_type.EditValue = "rb_date";
               gp_month.Visibility = LayoutVisibility.Never;
               gb_report_type.Visible = false;
               break;
            case "d":
               /* 只有日期 */
               gb_report_type.EditValue = "rb_date";
               gp_month.Visibility = LayoutVisibility.Never;
               gb_report_type.Visible = false;
               /* 無迄止值 */
               st_date.Visibility = LayoutVisibility.Never;
               emEndDate.Visible = false;
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Linq動態新增where條件
      /// </summary>
      /// <param name="dw1">DataTable</param>
      /// <returns></returns>
      public DataTable WfLinqSyntaxSelect(DataTable dw1)
      {

         var query = from dt in dw1.AsEnumerable() select dt;
         try {
            /* 日期起迄 */
            if (!string.IsNullOrEmpty(Sdate)) {
               query = query.Where(dt => string.Compare(dt.Field<string>(TableName + "_YMD").AsString(), Sdate) >= 0);
            }
            if (!string.IsNullOrEmpty(Edate)) {
               query = query.Where(dt => string.Compare(dt.Field<string>(TableName + "_YMD").AsString(), Edate) <= 0);
            }
            /* 期貨商代號起迄 */
            if (!string.IsNullOrEmpty(Sbrkno)) {
               query = query.Where(dt => string.Compare(dt.Field<string>(TableName + "_BRK_NO").AsString(), Sbrkno) >= 0);
            }
            if (!string.IsNullOrEmpty(Ebrkno)) {
               query = query.Where(dt => string.Compare(dt.Field<string>(TableName + "_BRK_NO").AsString(), Ebrkno) <= 0);
            }
            SumType = ReportSumType(gb_report_type.EditValue.ToString());
            SortType = PrintSortType(gb_print_sort.EditValue.ToString());
            SumSubType = GrpSubType(gb_group.EditValue.AsString());
            /*******************
            Where條件
            *******************/
            /* 商品群組 */
            if (!gb_group.EditValue.Equals("rb_gall")) {
               string paramkey = TableName + "_PARAM_KEY";
               if (!string.IsNullOrEmpty(ProdCategory)) {
                  if (dw1.Columns.Contains(paramkey))
                     query = query.Where(dt => dt.Field<string>(paramkey).AsString() == ProdCategory);
                  else
                     query = query.Where(dt => dt.Field<string>(TableName + "_PROD_ID").AsString() == ProdCategory);
               }
               /* 個股商品 */
               if (!gb_group.EditValue.Equals("rb_gparam")) {
                  string kindid2 = TableName + "_KIND_ID2";
                  if (!string.IsNullOrEmpty(ProdKindIdSto)) {
                     if (dw1.Columns.Contains(kindid2))
                        query = query.Where(dt => dt.Field<string>(kindid2).AsString() == ProdKindIdSto);
                     else
                        query = query.Where(dt => dt.Field<string>(TableName + "_PROD_ID").AsString() == ProdKindIdSto);
                  }
                  /* 商品 */
                  if (!gb_group.EditValue.Equals("rb_gkind2")) {
                     string kindid = TableName + "_KIND_ID";
                     if (!string.IsNullOrEmpty(ProdKindId)) {
                        if (dw1.Columns.Contains(kindid))
                           query = query.Where(dt => dt.Field<string>(kindid).AsString() == ProdKindId);
                        else
                           query = query.Where(dt => dt.Field<string>(TableName + "_PROD_ID").AsString() == ProdKindId);
                     }
                  }//rb_gkind2.checked = False
               }//rb_gparam.checked = False
            }//rb_gall.checked = False

            if (query.AsEnumerable().Count() <= 0) {
               dw1.Clear();
               return dw1;
            }

            DataTable dataTable = query.CopyToDataTable();
            dataTable = ExtensionCommon.AddSeriNumToDataTable(dataTable);
            return dataTable;
         }
         catch (Exception ex) {
            PbFunc.messageBox(GlobalInfo.ErrorText + " DataWindow linqsyntax Modify Failed ", ex.Message, MessageBoxIcon.Warning);
            return dw1;
            throw;
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

      public void WfGbGroup(bool VisibleValue, bool EnableValue, string AsType)
      {
         gb_group.Visible = VisibleValue;
         _Grp.Visible = VisibleValue;
         gb_group.Enabled = EnableValue;

         switch (AsType) {
            case "1":
               gb_group.EditValue = "rb_gall";
               break;
            case "2":
               gb_group.EditValue = "rb_gparam";
               break;
            case "3":
               gb_group.EditValue = "rb_gkind";
               break;
            case "4":
               gb_group.EditValue = "rb_gkind2";
               break;
            case "5":
               gb_group.EditValue = "rb_gprod";
               break;
            default:
               break;
         }
      }

      public void WfGbPrintSort(bool VisibleValue, bool EnableValue, string AsType)
      {
         gb_print_sort.Visible = VisibleValue;
         _PrintSort.Visible = VisibleValue;
         gb_print_sort.Enabled = EnableValue;

         switch (AsType) {
            case "1":
               gb_print_sort.EditValue = "rb_mmk";
               break;

            case "2":
               gb_print_sort.EditValue = "rb_prod";
               break;
            default:
               break;
         }
      }

      public void WfGrpDetial(bool VisibleValue, bool EnableValue, string AsType)
      {
         gb_detial.Visible = VisibleValue;
         _DetialGrp.Visible = VisibleValue;
         gb_detial.Enabled = EnableValue;

         switch (AsType) {
            case "1":
               gb_detial.EditValue = "rb_gdate";
               break;

            case "2":
               gb_detial.EditValue = "rb_gnodate";
               break;
            default:
               break;
         }
      }

      public void WfRunError()
      {
         /*******************
         Messagebox
         *******************/
         //SetPointer(Arrow!);
         this.Cursor = Cursors.Arrow;
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "轉檔有錯誤!";

         File.Delete(Filename);
      }

      public string wf_copyfile(string as_filename)
      {
         string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH, as_filename + ".xls");
         if (!File.Exists(originalFilePath)) {
            PbFunc.messageBox(GlobalInfo.ErrorText, "無此檔案「" + originalFilePath + "」!", MessageBoxIcon.Stop);
            return "";
         }
         string ls_filename;
         ls_filename = as_filename + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + ".xls";
         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
             ls_filename);
         try {
            File.Copy(originalFilePath, destinationFilePath, true);
         }
         catch (Exception ex) {
            PbFunc.messageBox(ex.InnerException.ToString(), "複製「" + originalFilePath + "」到「" + destinationFilePath + "」檔案錯誤!", MessageBoxIcon.Stop);
            throw;
         }
         Filename = destinationFilePath;
         return Filename;
      }

      private void InitializeComponent()
      {
         this._r_input = new DevExpress.XtraEditors.PanelControl();
         this._ReportType = new System.Windows.Forms.GroupBox();
         this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
         this._em_edate = new DevExpress.XtraEditors.TextEdit();
         this._em_sdate = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
         this._gp_date = new DevExpress.XtraLayout.LayoutControlGroup();
         this._st_date = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this._em_sym = new DevExpress.XtraEditors.TextEdit();
         this._em_eym = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
         this._gp_month = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this._st_month = new DevExpress.XtraLayout.LayoutControlItem();
         this._gb_report_type = new DevExpress.XtraEditors.RadioGroup();
         this._st_msg_txt = new DevExpress.XtraEditors.LabelControl();
         this._PrintSort = new System.Windows.Forms.GroupBox();
         this._gb_print_sort = new DevExpress.XtraEditors.RadioGroup();
         this._DetialGrp = new System.Windows.Forms.GroupBox();
         this._gb_detial = new DevExpress.XtraEditors.RadioGroup();
         this._Grp = new System.Windows.Forms.GroupBox();
         this._gb_group = new DevExpress.XtraEditors.RadioGroup();
         this._MarketGrp = new System.Windows.Forms.GroupBox();
         this._gb_market = new DevExpress.XtraEditors.RadioGroup();
         this.dataLayoutControl2 = new DevExpress.XtraDataLayout.DataLayoutControl();
         this._dw_prod_ct = new DevExpress.XtraEditors.LookUpEdit();
         this._dw_prod_kd_sto = new DevExpress.XtraEditors.LookUpEdit();
         this._dw_prod_kd = new DevExpress.XtraEditors.LookUpEdit();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this._AMM0_grp = new DevExpress.XtraLayout.LayoutControlGroup();
         this._st_prod_kd_sto = new DevExpress.XtraLayout.LayoutControlItem();
         this._st_prod_kd = new DevExpress.XtraLayout.LayoutControlItem();
         this._st_prod_ct = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
         this._dw_sbrkno = new DevExpress.XtraEditors.LookUpEdit();
         this._dw_ebrkno = new DevExpress.XtraEditors.LookUpEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this._ABRK_NO_grp = new DevExpress.XtraLayout.LayoutControlGroup();
         this._st_2 = new DevExpress.XtraLayout.LayoutControlItem();
         this._st_1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._r_input)).BeginInit();
         this._r_input.SuspendLayout();
         this._ReportType.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
         this.layoutControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._em_edate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._em_sdate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._gp_date)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_date)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._em_sym.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._em_eym.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._gp_month)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_month)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._gb_report_type.Properties)).BeginInit();
         this._PrintSort.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._gb_print_sort.Properties)).BeginInit();
         this._DetialGrp.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._gb_detial.Properties)).BeginInit();
         this._Grp.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._gb_group.Properties)).BeginInit();
         this._MarketGrp.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._gb_market.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl2)).BeginInit();
         this.dataLayoutControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._dw_prod_ct.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._dw_prod_kd_sto.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._dw_prod_kd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._AMM0_grp)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_prod_kd_sto)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_prod_kd)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_prod_ct)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
         this.dataLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._dw_sbrkno.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._dw_ebrkno.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._ABRK_NO_grp)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_1)).BeginInit();
         this.SuspendLayout();
         // 
         // _r_input
         // 
         this._r_input.Appearance.BackColor = System.Drawing.Color.Transparent;
         this._r_input.Appearance.Options.UseBackColor = true;
         this._r_input.AutoSize = true;
         this._r_input.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this._r_input.Controls.Add(this._ReportType);
         this._r_input.Controls.Add(this._st_msg_txt);
         this._r_input.Controls.Add(this._PrintSort);
         this._r_input.Controls.Add(this._DetialGrp);
         this._r_input.Controls.Add(this._Grp);
         this._r_input.Controls.Add(this._MarketGrp);
         this._r_input.Controls.Add(this.dataLayoutControl2);
         this._r_input.Controls.Add(this.dataLayoutControl1);
         this._r_input.Dock = System.Windows.Forms.DockStyle.Fill;
         this._r_input.Location = new System.Drawing.Point(0, 0);
         this._r_input.Name = "_r_input";
         this._r_input.Size = new System.Drawing.Size(604, 282);
         this._r_input.TabIndex = 5;
         // 
         // _ReportType
         // 
         this._ReportType.Controls.Add(this.layoutControl2);
         this._ReportType.Controls.Add(this.layoutControl1);
         this._ReportType.Controls.Add(this._gb_report_type);
         this._ReportType.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this._ReportType.Location = new System.Drawing.Point(19, 146);
         this._ReportType.Margin = new System.Windows.Forms.Padding(2);
         this._ReportType.Name = "_ReportType";
         this._ReportType.Padding = new System.Windows.Forms.Padding(2);
         this._ReportType.Size = new System.Drawing.Size(251, 70);
         this._ReportType.TabIndex = 10;
         this._ReportType.TabStop = false;
         this._ReportType.Text = "區間";
         // 
         // layoutControl2
         // 
         this.layoutControl2.Controls.Add(this._em_edate);
         this.layoutControl2.Controls.Add(this._em_sdate);
         this.layoutControl2.Location = new System.Drawing.Point(28, 41);
         this.layoutControl2.Margin = new System.Windows.Forms.Padding(2);
         this.layoutControl2.Name = "layoutControl2";
         this.layoutControl2.Root = this.layoutControlGroup4;
         this.layoutControl2.Size = new System.Drawing.Size(212, 24);
         this.layoutControl2.TabIndex = 9;
         this.layoutControl2.Text = "layoutControl2";
         // 
         // _em_edate
         // 
         this._em_edate.CausesValidation = false;
         this._em_edate.EditValue = new System.DateTime(2018, 12, 22, 10, 33, 17, 591);
         this._em_edate.Location = new System.Drawing.Point(134, 2);
         this._em_edate.Margin = new System.Windows.Forms.Padding(2);
         this._em_edate.Name = "_em_edate";
         this._em_edate.Properties.DisplayFormat.FormatString = "yyyy/MM/dd";
         this._em_edate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this._em_edate.Properties.Mask.EditMask = "\\d\\d\\d\\d/\\d\\d/\\d\\d";
         this._em_edate.Properties.Mask.IgnoreMaskBlank = false;
         this._em_edate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
         this._em_edate.Properties.Mask.PlaceHolder = '0';
         this._em_edate.Size = new System.Drawing.Size(76, 20);
         this._em_edate.StyleController = this.layoutControl2;
         this._em_edate.TabIndex = 7;
         // 
         // _em_sdate
         // 
         this._em_sdate.CausesValidation = false;
         this._em_sdate.EditValue = new System.DateTime(2018, 12, 22, 10, 32, 59, 808);
         this._em_sdate.Location = new System.Drawing.Point(41, 2);
         this._em_sdate.Margin = new System.Windows.Forms.Padding(2);
         this._em_sdate.Name = "_em_sdate";
         this._em_sdate.Properties.DisplayFormat.FormatString = "yyyy/MM/dd";
         this._em_sdate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this._em_sdate.Properties.Mask.EditMask = "\\d\\d\\d\\d/\\d\\d/\\d\\d";
         this._em_sdate.Properties.Mask.IgnoreMaskBlank = false;
         this._em_sdate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
         this._em_sdate.Properties.Mask.PlaceHolder = '0';
         this._em_sdate.Size = new System.Drawing.Size(75, 20);
         this._em_sdate.StyleController = this.layoutControl2;
         this._em_sdate.TabIndex = 6;
         // 
         // layoutControlGroup4
         // 
         this.layoutControlGroup4.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup4.GroupBordersVisible = false;
         this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this._gp_date});
         this.layoutControlGroup4.Name = "layoutControlGroup4";
         this.layoutControlGroup4.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup4.Size = new System.Drawing.Size(212, 24);
         this.layoutControlGroup4.TextVisible = false;
         // 
         // _gp_date
         // 
         this._gp_date.CustomizationFormText = "layoutControlGroup3";
         this._gp_date.GroupBordersVisible = false;
         this._gp_date.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this._st_date,
            this.layoutControlItem3});
         this._gp_date.Location = new System.Drawing.Point(0, 0);
         this._gp_date.Name = "_gp_date";
         this._gp_date.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this._gp_date.Size = new System.Drawing.Size(212, 24);
         this._gp_date.Text = "layoutControlGroup3";
         // 
         // _st_date
         // 
         this._st_date.Control = this._em_edate;
         this._st_date.CustomizationFormText = "layoutControlItem4";
         this._st_date.Location = new System.Drawing.Point(118, 0);
         this._st_date.Name = "_st_date";
         this._st_date.Size = new System.Drawing.Size(94, 24);
         this._st_date.Text = "~";
         this._st_date.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this._st_date.TextLocation = DevExpress.Utils.Locations.Left;
         this._st_date.TextSize = new System.Drawing.Size(9, 14);
         this._st_date.TextToControlDistance = 5;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this._em_sdate;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(118, 24);
         this.layoutControlItem3.Text = "日期：";
         this.layoutControlItem3.TextLocation = DevExpress.Utils.Locations.Left;
         this.layoutControlItem3.TextSize = new System.Drawing.Size(36, 14);
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this._em_sym);
         this.layoutControl1.Controls.Add(this._em_eym);
         this.layoutControl1.Location = new System.Drawing.Point(28, 14);
         this.layoutControl1.Margin = new System.Windows.Forms.Padding(2);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1067, 0, 650, 400);
         this.layoutControl1.Root = this.layoutControlGroup3;
         this.layoutControl1.Size = new System.Drawing.Size(197, 25);
         this.layoutControl1.TabIndex = 2;
         this.layoutControl1.Text = "區間";
         // 
         // _em_sym
         // 
         this._em_sym.CausesValidation = false;
         this._em_sym.EditValue = "2018/12";
         this._em_sym.Location = new System.Drawing.Point(41, 2);
         this._em_sym.Margin = new System.Windows.Forms.Padding(2);
         this._em_sym.Name = "_em_sym";
         this._em_sym.Properties.DisplayFormat.FormatString = "yyyy/MM";
         this._em_sym.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this._em_sym.Properties.Mask.EditMask = "\\d\\d\\d\\d/\\d\\d";
         this._em_sym.Properties.Mask.IgnoreMaskBlank = false;
         this._em_sym.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
         this._em_sym.Properties.Mask.PlaceHolder = '0';
         this._em_sym.Properties.MaxLength = 7;
         this._em_sym.Size = new System.Drawing.Size(65, 20);
         this._em_sym.StyleController = this.layoutControl1;
         this._em_sym.TabIndex = 4;
         // 
         // _em_eym
         // 
         this._em_eym.CausesValidation = false;
         this._em_eym.EditValue = "2018/12";
         this._em_eym.Location = new System.Drawing.Point(124, 2);
         this._em_eym.Margin = new System.Windows.Forms.Padding(2);
         this._em_eym.Name = "_em_eym";
         this._em_eym.Properties.DisplayFormat.FormatString = "yyyy/MM";
         this._em_eym.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this._em_eym.Properties.Mask.EditMask = "\\d\\d\\d\\d/\\d\\d";
         this._em_eym.Properties.Mask.IgnoreMaskBlank = false;
         this._em_eym.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
         this._em_eym.Properties.Mask.PlaceHolder = '0';
         this._em_eym.Properties.MaxLength = 7;
         this._em_eym.Size = new System.Drawing.Size(71, 20);
         this._em_eym.StyleController = this.layoutControl1;
         this._em_eym.TabIndex = 5;
         // 
         // layoutControlGroup3
         // 
         this.layoutControlGroup3.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup3.GroupBordersVisible = false;
         this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this._gp_month});
         this.layoutControlGroup3.Name = "Root";
         this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup3.Size = new System.Drawing.Size(197, 25);
         this.layoutControlGroup3.Text = "gb_report_type";
         // 
         // _gp_month
         // 
         this._gp_month.CustomizationFormText = "layoutControlGroup2";
         this._gp_month.GroupBordersVisible = false;
         this._gp_month.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this._st_month});
         this._gp_month.Location = new System.Drawing.Point(0, 0);
         this._gp_month.Name = "_gp_month";
         this._gp_month.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this._gp_month.Size = new System.Drawing.Size(197, 25);
         this._gp_month.Text = "layoutControlGroup2";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this._em_sym;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(108, 25);
         this.layoutControlItem1.Text = "月份：";
         this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left;
         this.layoutControlItem1.TextSize = new System.Drawing.Size(36, 14);
         // 
         // _st_month
         // 
         this._st_month.Control = this._em_eym;
         this._st_month.CustomizationFormText = "layoutControlItem2";
         this._st_month.Location = new System.Drawing.Point(108, 0);
         this._st_month.MinSize = new System.Drawing.Size(35, 17);
         this._st_month.Name = "_st_month";
         this._st_month.Size = new System.Drawing.Size(89, 25);
         this._st_month.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this._st_month.Text = "~";
         this._st_month.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this._st_month.TextLocation = DevExpress.Utils.Locations.Left;
         this._st_month.TextSize = new System.Drawing.Size(9, 14);
         this._st_month.TextToControlDistance = 5;
         // 
         // _gb_report_type
         // 
         this._gb_report_type.EditValue = "rb_month";
         this._gb_report_type.Location = new System.Drawing.Point(2, 11);
         this._gb_report_type.Margin = new System.Windows.Forms.Padding(0);
         this._gb_report_type.Name = "_gb_report_type";
         this._gb_report_type.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this._gb_report_type.Properties.Appearance.Options.UseBackColor = true;
         this._gb_report_type.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this._gb_report_type.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_month", ""),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_date", "")});
         this._gb_report_type.Size = new System.Drawing.Size(24, 57);
         this._gb_report_type.TabIndex = 8;
         // 
         // _st_msg_txt
         // 
         this._st_msg_txt.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this._st_msg_txt.Appearance.Options.UseForeColor = true;
         this._st_msg_txt.Location = new System.Drawing.Point(34, 221);
         this._st_msg_txt.Name = "_st_msg_txt";
         this._st_msg_txt.Size = new System.Drawing.Size(4, 14);
         this._st_msg_txt.TabIndex = 8;
         this._st_msg_txt.Text = " ";
         this._st_msg_txt.Visible = false;
         // 
         // _PrintSort
         // 
         this._PrintSort.Controls.Add(this._gb_print_sort);
         this._PrintSort.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this._PrintSort.Location = new System.Drawing.Point(508, 146);
         this._PrintSort.Name = "_PrintSort";
         this._PrintSort.Size = new System.Drawing.Size(80, 70);
         this._PrintSort.TabIndex = 7;
         this._PrintSort.TabStop = false;
         this._PrintSort.Text = "列印順序";
         // 
         // _gb_print_sort
         // 
         this._gb_print_sort.EditValue = "rb_mmk";
         this._gb_print_sort.Location = new System.Drawing.Point(6, 16);
         this._gb_print_sort.Name = "_gb_print_sort";
         this._gb_print_sort.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this._gb_print_sort.Properties.Appearance.Options.UseBackColor = true;
         this._gb_print_sort.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this._gb_print_sort.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_mmk", "造市者"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_prod", "商品")});
         this._gb_print_sort.Size = new System.Drawing.Size(62, 53);
         this._gb_print_sort.TabIndex = 0;
         // 
         // _DetialGrp
         // 
         this._DetialGrp.Controls.Add(this._gb_detial);
         this._DetialGrp.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this._DetialGrp.Location = new System.Drawing.Point(508, 45);
         this._DetialGrp.Name = "_DetialGrp";
         this._DetialGrp.Size = new System.Drawing.Size(80, 76);
         this._DetialGrp.TabIndex = 6;
         this._DetialGrp.TabStop = false;
         this._DetialGrp.Text = "報表內容";
         // 
         // _gb_detial
         // 
         this._gb_detial.EditValue = "rb_gdate";
         this._gb_detial.Location = new System.Drawing.Point(6, 16);
         this._gb_detial.Name = "_gb_detial";
         this._gb_detial.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this._gb_detial.Properties.Appearance.Options.UseBackColor = true;
         this._gb_detial.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this._gb_detial.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_gdate", "分日期"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_gnodate", "不分日期")});
         this._gb_detial.Size = new System.Drawing.Size(89, 55);
         this._gb_detial.TabIndex = 0;
         // 
         // _Grp
         // 
         this._Grp.Controls.Add(this._gb_group);
         this._Grp.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this._Grp.Location = new System.Drawing.Point(381, 85);
         this._Grp.Name = "_Grp";
         this._Grp.Size = new System.Drawing.Size(103, 144);
         this._Grp.TabIndex = 5;
         this._Grp.TabStop = false;
         this._Grp.Text = "統計依照";
         // 
         // _gb_group
         // 
         this._gb_group.EditValue = "rb_gkind";
         this._gb_group.Location = new System.Drawing.Point(6, 17);
         this._gb_group.Name = "_gb_group";
         this._gb_group.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this._gb_group.Properties.Appearance.Options.UseBackColor = true;
         this._gb_group.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this._gb_group.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_gall", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_gparam", "商品群組"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_s", "股票各類群組"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_gkind2", "商品(2碼)"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_gkind", "商品(3碼)"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_gprod", "序列")});
         this._gb_group.Size = new System.Drawing.Size(123, 130);
         this._gb_group.TabIndex = 0;
         this._gb_group.EditValueChanged += new System.EventHandler(this._gb_group_EditValueChanged);
         // 
         // _MarketGrp
         // 
         this._MarketGrp.Controls.Add(this._gb_market);
         this._MarketGrp.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this._MarketGrp.Location = new System.Drawing.Point(297, 160);
         this._MarketGrp.Name = "_MarketGrp";
         this._MarketGrp.Size = new System.Drawing.Size(78, 69);
         this._MarketGrp.TabIndex = 4;
         this._MarketGrp.TabStop = false;
         this._MarketGrp.Text = "交易時段";
         // 
         // _gb_market
         // 
         this._gb_market.EditValue = "rb_market_0";
         this._gb_market.Location = new System.Drawing.Point(6, 17);
         this._gb_market.Name = "_gb_market";
         this._gb_market.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this._gb_market.Properties.Appearance.Options.UseBackColor = true;
         this._gb_market.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this._gb_market.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_market_1", "盤後")});
         this._gb_market.Size = new System.Drawing.Size(63, 54);
         this._gb_market.TabIndex = 0;
         // 
         // dataLayoutControl2
         // 
         this.dataLayoutControl2.Controls.Add(this._dw_prod_ct);
         this.dataLayoutControl2.Controls.Add(this._dw_prod_kd_sto);
         this.dataLayoutControl2.Controls.Add(this._dw_prod_kd);
         this.dataLayoutControl2.DataMember = "AMM0";
         this.dataLayoutControl2.Location = new System.Drawing.Point(12, 61);
         this.dataLayoutControl2.Name = "dataLayoutControl2";
         this.dataLayoutControl2.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(664, 0, 650, 400);
         this.dataLayoutControl2.Root = this.layoutControlGroup2;
         this.dataLayoutControl2.Size = new System.Drawing.Size(327, 70);
         this.dataLayoutControl2.TabIndex = 1;
         this.dataLayoutControl2.Text = "dataLayoutControl2";
         // 
         // _dw_prod_ct
         // 
         this._dw_prod_ct.Location = new System.Drawing.Point(65, 12);
         this._dw_prod_ct.Name = "_dw_prod_ct";
         this._dw_prod_ct.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._dw_prod_ct.Properties.ImmediatePopup = true;
         this._dw_prod_ct.Properties.NullText = "";
         this._dw_prod_ct.Properties.PopupSizeable = false;
         this._dw_prod_ct.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
         this._dw_prod_ct.Size = new System.Drawing.Size(102, 20);
         this._dw_prod_ct.StyleController = this.dataLayoutControl2;
         this._dw_prod_ct.TabIndex = 4;
         // 
         // _dw_prod_kd_sto
         // 
         this._dw_prod_kd_sto.Location = new System.Drawing.Point(66, 36);
         this._dw_prod_kd_sto.Name = "_dw_prod_kd_sto";
         this._dw_prod_kd_sto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._dw_prod_kd_sto.Properties.ImmediatePopup = true;
         this._dw_prod_kd_sto.Properties.NullText = "";
         this._dw_prod_kd_sto.Properties.PopupSizeable = false;
         this._dw_prod_kd_sto.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
         this._dw_prod_kd_sto.Size = new System.Drawing.Size(101, 20);
         this._dw_prod_kd_sto.StyleController = this.dataLayoutControl2;
         this._dw_prod_kd_sto.TabIndex = 5;
         // 
         // _dw_prod_kd
         // 
         this._dw_prod_kd.Location = new System.Drawing.Point(224, 36);
         this._dw_prod_kd.Name = "_dw_prod_kd";
         this._dw_prod_kd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._dw_prod_kd.Properties.ImmediatePopup = true;
         this._dw_prod_kd.Properties.NullText = "";
         this._dw_prod_kd.Properties.PopupSizeable = false;
         this._dw_prod_kd.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
         this._dw_prod_kd.Size = new System.Drawing.Size(91, 20);
         this._dw_prod_kd.StyleController = this.dataLayoutControl2;
         this._dw_prod_kd.TabIndex = 6;
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup2.GroupBordersVisible = false;
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this._AMM0_grp});
         this.layoutControlGroup2.Name = "Root";
         this.layoutControlGroup2.Size = new System.Drawing.Size(327, 70);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // _AMM0_grp
         // 
         this._AMM0_grp.AllowDrawBackground = false;
         this._AMM0_grp.GroupBordersVisible = false;
         this._AMM0_grp.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this._st_prod_kd_sto,
            this._st_prod_kd,
            this._st_prod_ct,
            this.emptySpaceItem1});
         this._AMM0_grp.Location = new System.Drawing.Point(0, 0);
         this._AMM0_grp.Name = "_AMM0_grp";
         this._AMM0_grp.Size = new System.Drawing.Size(307, 50);
         // 
         // _st_prod_kd_sto
         // 
         this._st_prod_kd_sto.Control = this._dw_prod_kd_sto;
         this._st_prod_kd_sto.Location = new System.Drawing.Point(0, 24);
         this._st_prod_kd_sto.Name = "_st_prod_kd_sto";
         this._st_prod_kd_sto.Size = new System.Drawing.Size(159, 26);
         this._st_prod_kd_sto.Text = "  2碼商品";
         this._st_prod_kd_sto.TextLocation = DevExpress.Utils.Locations.Left;
         this._st_prod_kd_sto.TextSize = new System.Drawing.Size(51, 14);
         // 
         // _st_prod_kd
         // 
         this._st_prod_kd.Control = this._dw_prod_kd;
         this._st_prod_kd.ControlAlignment = System.Drawing.ContentAlignment.BottomLeft;
         this._st_prod_kd.Location = new System.Drawing.Point(159, 24);
         this._st_prod_kd.Name = "_st_prod_kd";
         this._st_prod_kd.Size = new System.Drawing.Size(148, 26);
         this._st_prod_kd.Text = "造市商品";
         this._st_prod_kd.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this._st_prod_kd.TextSize = new System.Drawing.Size(48, 14);
         this._st_prod_kd.TextToControlDistance = 5;
         // 
         // _st_prod_ct
         // 
         this._st_prod_ct.Control = this._dw_prod_ct;
         this._st_prod_ct.Location = new System.Drawing.Point(0, 0);
         this._st_prod_ct.Name = "_st_prod_ct";
         this._st_prod_ct.Size = new System.Drawing.Size(159, 24);
         this._st_prod_ct.Text = "商品群組";
         this._st_prod_ct.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this._st_prod_ct.TextLocation = DevExpress.Utils.Locations.Left;
         this._st_prod_ct.TextSize = new System.Drawing.Size(48, 14);
         this._st_prod_ct.TextToControlDistance = 5;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(159, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(148, 24);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // dataLayoutControl1
         // 
         this.dataLayoutControl1.Controls.Add(this._dw_sbrkno);
         this.dataLayoutControl1.Controls.Add(this._dw_ebrkno);
         this.dataLayoutControl1.DataMember = "ABRK";
         this.dataLayoutControl1.Location = new System.Drawing.Point(5, 5);
         this.dataLayoutControl1.Name = "dataLayoutControl1";
         this.dataLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(325, 252, 1065, 583);
         this.dataLayoutControl1.Root = this.layoutControlGroup1;
         this.dataLayoutControl1.Size = new System.Drawing.Size(488, 48);
         this.dataLayoutControl1.TabIndex = 0;
         this.dataLayoutControl1.Text = "dataLayoutControl1";
         // 
         // _dw_sbrkno
         // 
         this._dw_sbrkno.Location = new System.Drawing.Point(75, 12);
         this._dw_sbrkno.Name = "_dw_sbrkno";
         this._dw_sbrkno.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._dw_sbrkno.Properties.ImmediatePopup = true;
         this._dw_sbrkno.Properties.NullText = "";
         this._dw_sbrkno.Properties.PopupSizeable = false;
         this._dw_sbrkno.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
         this._dw_sbrkno.Size = new System.Drawing.Size(190, 20);
         this._dw_sbrkno.StyleController = this.dataLayoutControl1;
         this._dw_sbrkno.TabIndex = 4;
         // 
         // _dw_ebrkno
         // 
         this._dw_ebrkno.Location = new System.Drawing.Point(283, 12);
         this._dw_ebrkno.Name = "_dw_ebrkno";
         this._dw_ebrkno.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._dw_ebrkno.Properties.ImmediatePopup = true;
         this._dw_ebrkno.Properties.NullText = "";
         this._dw_ebrkno.Properties.PopupSizeable = false;
         this._dw_ebrkno.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
         this._dw_ebrkno.Size = new System.Drawing.Size(193, 20);
         this._dw_ebrkno.StyleController = this.dataLayoutControl1;
         this._dw_ebrkno.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this._ABRK_NO_grp});
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(488, 48);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // _ABRK_NO_grp
         // 
         this._ABRK_NO_grp.AllowDrawBackground = false;
         this._ABRK_NO_grp.GroupBordersVisible = false;
         this._ABRK_NO_grp.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this._st_2,
            this._st_1});
         this._ABRK_NO_grp.Location = new System.Drawing.Point(0, 0);
         this._ABRK_NO_grp.Name = "_ABRK_NO_grp";
         this._ABRK_NO_grp.Size = new System.Drawing.Size(468, 28);
         // 
         // _st_2
         // 
         this._st_2.Control = this._dw_sbrkno;
         this._st_2.Location = new System.Drawing.Point(0, 0);
         this._st_2.Name = "_st_2";
         this._st_2.Size = new System.Drawing.Size(257, 28);
         this._st_2.Text = "期貨商代號";
         this._st_2.TextSize = new System.Drawing.Size(60, 14);
         // 
         // _st_1
         // 
         this._st_1.Control = this._dw_ebrkno;
         this._st_1.CustomizationFormText = "ABRK_NO";
         this._st_1.Location = new System.Drawing.Point(257, 0);
         this._st_1.Name = "_st_1";
         this._st_1.Size = new System.Drawing.Size(211, 28);
         this._st_1.Text = "~";
         this._st_1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this._st_1.TextSize = new System.Drawing.Size(9, 14);
         this._st_1.TextToControlDistance = 5;
         // 
         // ucW500xx
         // 
         this.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.Appearance.Options.UseBackColor = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this._r_input);
         this.Name = "ucW500xx";
         this.Size = new System.Drawing.Size(604, 282);
         ((System.ComponentModel.ISupportInitialize)(this._r_input)).EndInit();
         this._r_input.ResumeLayout(false);
         this._r_input.PerformLayout();
         this._ReportType.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
         this.layoutControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._em_edate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._em_sdate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._gp_date)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_date)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._em_sym.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._em_eym.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._gp_month)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_month)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._gb_report_type.Properties)).EndInit();
         this._PrintSort.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._gb_print_sort.Properties)).EndInit();
         this._DetialGrp.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._gb_detial.Properties)).EndInit();
         this._Grp.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._gb_group.Properties)).EndInit();
         this._MarketGrp.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._gb_market.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl2)).EndInit();
         this.dataLayoutControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._dw_prod_ct.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._dw_prod_kd_sto.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._dw_prod_kd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._AMM0_grp)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_prod_kd_sto)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_prod_kd)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_prod_ct)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
         this.dataLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._dw_sbrkno.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._dw_ebrkno.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._ABRK_NO_grp)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._st_1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      private void _gb_group_EditValueChanged(object sender, EventArgs e)
      {
         DevExpress.XtraEditors.RadioGroup rb = sender as DevExpress.XtraEditors.RadioGroup;
         if (rb == null) return;
         switch (rb.EditValue.ToString()) {
            case "rb_gall":
               dw_prod_ct.Enabled = false;
               st_prod_ct.Enabled = false;

               dw_prod_kd_sto.Enabled = false;
               st_prod_kd_sto.Enabled = false;

               dw_prod_kd.Enabled = false;
               st_prod_kd.Enabled = false;
               break;
            case "rb_gparam":
               dw_prod_ct.Enabled = true;
               st_prod_ct.Enabled = true;

               dw_prod_kd_sto.Enabled = false;
               st_prod_kd_sto.Enabled = false;

               dw_prod_kd.Enabled = false;
               st_prod_kd.Enabled = false;
               break;
            case "rb_gkind2":
               dw_prod_ct.Enabled = true;
               st_prod_ct.Enabled = true;

               dw_prod_kd_sto.Enabled = true;
               st_prod_kd_sto.Enabled = true;

               dw_prod_kd.Enabled = false;
               st_prod_kd.Enabled = false;
               break;
            case "rb_gkind":
               dw_prod_ct.Enabled = true;
               st_prod_ct.Enabled = true;

               dw_prod_kd_sto.Enabled = true;
               st_prod_kd_sto.Enabled = true;

               dw_prod_kd.Enabled = true;
               st_prod_kd.Enabled = true;
               break;
            case "rb_gprod":
               dw_prod_ct.Enabled = true;
               st_prod_ct.Enabled = true;

               dw_prod_kd_sto.Enabled = true;
               st_prod_kd_sto.Enabled = true;

               dw_prod_kd.Enabled = true;
               st_prod_kd.Enabled = true;
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// 動態新增where條件
      /// </summary>
      /// <param name="dw_1">DataAdapter</param>
      ////public void wf_select_sqlcode(DbDataAdapter dw_1)
      ////{
      ////   string is_where;
      ////   is_where = "";
      ////   try {
      ////      /* 日期起迄 */
      ////      if (!string.IsNullOrEmpty(Sdate)) {
      ////         is_where = is_where + @" and " + TableName + "_YMD >='" + Sdate + "' ";
      ////      }
      ////      if (!string.IsNullOrEmpty(Edate)) {
      ////         is_where = is_where + @" and " + TableName + "_YMD <='" + Edate + "' ";
      ////      }
      ////      /* 期貨商代號起迄 */
      ////      if (!string.IsNullOrEmpty(is_sbrkno)) {
      ////         is_where = is_where + @" and " + TableName + "_BRK_NO >='" + is_sbrkno + "' ";
      ////      }
      ////      if (!string.IsNullOrEmpty(is_ebrkno)) {
      ////         is_where = is_where + @" and " + TableName + "_BRK_NO <='" + is_ebrkno + "' ";
      ////      }
      ////      sum_sortType();

      ////      /*******************
      ////      Where條件
      ////      *******************/
      ////      /* 商品群組 */
      ////      if (!gb_group.EditValue.Equals("rb_gall")) {
      ////         if (!string.IsNullOrEmpty(is_prod_category)) {
      ////            is_where = is_where + @" and " + TableName + "_PARAM_KEY ='" + is_prod_category + "' ";
      ////         }
      ////         /* 個股商品 */
      ////         if (!gb_group.EditValue.Equals("rb_gparam")) {
      ////            if (!string.IsNullOrEmpty(is_prod_kind_id_sto)) {
      ////               is_where = is_where + @" and " + TableName + "_KIND_ID2 ='" + is_prod_kind_id_sto + "' ";
      ////            }
      ////            /* 商品 */
      ////            if (!gb_group.EditValue.Equals("rb_gkind2")) {
      ////               if (!string.IsNullOrEmpty(is_prod_kind_id)) {
      ////                  is_where = is_where + @" and " + TableName + "_KIND_ID ='" + is_prod_kind_id + "' ";
      ////               }
      ////            }//rb_gkind2.checked = False
      ////         }//rb_gparam.checked = False
      ////      }//rb_gall.checked = False
      ////       /******************************
      ////       在dw_1的SQL Statement中插入where條件(is_select)
      ////       (1)is_select 都以'and ...'開頭
      ////       (2)若有GROUP,則在 FROM 和 GROUP BY 中間插入 WHERE 條件
      ////       ******************************/
      ////      string ls_select;

      ////      //dw_1.dataobject = is_dw_name;
      ////      //dw_1.settransobject(sqlca);
      ////      ls_select = dw_1.SelectCommand.CommandText;//describe("datawindow.table.select");

      ////      int li_pos;
      ////      /* (1) */
      ////      li_pos = ls_select.IndexOf("FROM") - 1;
      ////      li_pos = ls_select.IndexOf("WHERE", li_pos) - 1;
      ////      if (li_pos < 0) {
      ////         is_where = "WHERE" + PbFunc.Mid(is_where, 3, PbFunc.Len(is_where));
      ////      }
      ////      /* (2) */
      ////      li_pos = ls_select.IndexOf("GROUP BY");
      ////      int orderby = ls_select.IndexOf("ORDER BY");
      ////      if (li_pos < 0) {
      ////         li_pos = PbFunc.Len(ls_select) - 1;
      ////         if (orderby > 0) {
      ////            li_pos = orderby;
      ////         }
      ////      }
      ////      ls_select = PbFunc.Mid(ls_select, 0, li_pos) + is_where + PbFunc.Mid(ls_select, li_pos, ls_select.Length - 1);

      ////      dw_1.SelectCommand.CommandText = ls_select;
      ////   }
      ////   catch (Exception ex) {
      ////      PbFunc.messageBox(GlobalInfo.ErrorText + " DataWindow sqlsyntax Modify Failed ", ex.Message, MessageBoxIcon.Warning);
      ////      return;
      ////      throw;
      ////   }
      ////}

   }
}
