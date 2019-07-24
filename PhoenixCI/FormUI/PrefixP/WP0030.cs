using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using Common;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using DevExpress.Utils;
using BaseGround.Report;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DataObjects.Dao.Together;
using System.Reflection;

/// <summary>
/// David, 2019/03/21 
/// </summary>
namespace PhoenixCI.FormUI.PrefixP {
   public partial class WP0030 : FormParent {

      private DP00xx daoP00xx;

      public WP0030(string programID, string programName) : base(programID, programName) {
         try {
            InitializeComponent();
            daoP00xx = new DP00xx();

            this.Text = _ProgramID + "─" + _ProgramName;
            gvMain.OptionsBehavior.Editable = false;

            txtStartDate.Text = "%";
            txtEndDate.Text = "%";

            //下拉選單(系統別)
            //List<LookupItem> ddlbSystem = new List<LookupItem>(){
             //                           new LookupItem() { ValueMember = "W", DisplayMember = "W：網際網路"},
             //                           new LookupItem() { ValueMember = "V", DisplayMember = "V：語音查詢" }};
            DataTable dtType = new CODW().ListLookUpEdit("P0030", "P0030_DDLB_1");
            Extension.SetDataTable(ddlbType, dtType, "CODW_ID", "CODW_DESC", TextEditStyles.DisableTextEditor, null);
            ddlbType.EditValue = "W";

            //下拉選單(類別)
            List<LookupItem> ddlbCatagroy = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "I", DisplayMember = "I：依交易人查明細"},
                                        new LookupItem() { ValueMember = "F", DisplayMember = "F：依期貨商合計" }};
            dtType = new CODW().ListLookUpEdit("P0030", "P0030_DDLB_3");
            //Extension.SetDataTable(ddlbCate, ddlbCatagroy, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
            Extension.SetDataTable(ddlbCate, dtType, "CODW_ID", "CODW_DESC", TextEditStyles.DisableTextEditor, null);
            ddlbCate.EditValue = "I";

         } catch (Exception ex) {
            WriteLog(ex);
         }
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

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      /// <summary>
      /// 按下[讀取/預覽]按鈕時,去資料庫撈資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         base.Retrieve();

         if (!CheckInputText(txtStartDate.Text) || !CheckInputText(txtEndDate.Text)) {
            MessageDisplay.Info("請輸入正確日期");
            return ResultStatus.Fail;
         }

         DataTable dtContentI = new DataTable();
         DataTable dtContentF = new DataTable();
         DataTable dtContent = new DataTable();
         DataTable dtTXFP = new DataTable();
         string type = ddlbType.EditValue.AsString();
         string cate = ddlbCate.EditValue.AsString();
         string searchType = ddlbCate.Text.Substring(0, 1);
         string groupSummaryAccount = "合計{0}戶";
         string groupSummaryTimes = "合計{0}次";
         string summaryAccount = "總計{0}戶";
         string summaryTimes = "總計{0}次";

         dtTXFP = new TXFP().ListDataByKey("POS");
         //取 F 的欄位來給I加總用
         IGridDataP00xx gridData = daoP00xx.CreateGridData(daoP00xx.GetType(), GetType(), MethodBase.GetCurrentMethod().Name);
         QP00xx qP00xxI = new QP00xx(txtStartDate.Text, txtEndDate.Text, type, null, "I", dtTXFP);
         QP00xx qP00xxF = new QP00xx(txtStartDate.Text, txtEndDate.Text, type, null, "F", dtTXFP);

         dtContentI = gridData.GetData(qP00xxI);
         dtContentF = gridData.GetData(qP00xxF);
         gcMain.DataSource = null;
         gvMain.GroupSummary.Clear();
         gvMain.Columns.Clear();//清除grid
         dtContentF.Columns.Remove(dtContentF.Columns["0"]);

         List<string> FcmName = new List<string>();
         foreach (DataRow drI in dtContentI.Rows) {
            DataRow drF = dtContentF.Select("FCM_NAME =" + "'" + drI["FCM_NAME"].ToString() + "'")[0];
            if (FcmName.Where(f => f == drI["FCM_NAME"].ToString()).Count() == 0) {
               FcmName.Add(drI["FCM_NAME"].ToString());
               drI["0"] = drF[2].AsString();
            }
         }

         dtContent = searchType == "I" ? dtContentI : dtContentF;
         gcMain.DataSource = dtContent;

         foreach (DataColumn dc in dtContent.Columns) {
            //設定欄位屬性
            gvMain.SetColumnCaption(dc.ColumnName, GetColumnCaption(dc.Ordinal, searchType));
            gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.VAlignment = VertAlignment.Top;
            //設定合併欄位(一樣的值不顯示)
            gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = (dc.Ordinal != 0 && dc.Ordinal != 1) ? DefaultBoolean.False : DefaultBoolean.True;
         }

         gvMain.Columns[1].Group();//依流水號分群

         //依交易人查詢
         if (searchType == "I") {
            gvMain.Columns.Last().Visible = false;//隱藏小記欄位
            gvMain.OptionsView.AllowCellMerge = true;
            //設定群組 小記
            gvMain.SetGridGroupSummary(gvMain.Columns[4].FieldName, groupSummaryTimes, SummaryItemType.Sum, true, gvMain.Columns[4].FieldName);
            gvMain.SetGridGroupSummary(gvMain.Columns[5].FieldName, groupSummaryAccount, SummaryItemType.Sum, true, gvMain.Columns[2].FieldName);

            //總計
            gvMain.SetGridSummary(gvMain.Columns[2].FieldName, gvMain.Columns[5].FieldName, summaryAccount, SummaryItemType.Sum);
            gvMain.SetGridSummary(gvMain.Columns[4].FieldName, gvMain.Columns[4].FieldName, summaryTimes, SummaryItemType.Sum);
         } else {//searchType="F"
            gvMain.OptionsView.AllowCellMerge = false;
            //分群小記 
            gvMain.SetGridGroupSummary(gvMain.Columns[2].FieldName, groupSummaryAccount, SummaryItemType.Sum, true, gvMain.Columns[2].FieldName);
            gvMain.SetGridGroupSummary(gvMain.Columns[3].FieldName, groupSummaryTimes, SummaryItemType.Sum, true, gvMain.Columns[3].FieldName);

            //總計
            gvMain.SetGridSummary(gvMain.Columns[2].FieldName, gvMain.Columns[2].FieldName, summaryAccount, SummaryItemType.Sum);
            gvMain.SetGridSummary(gvMain.Columns[3].FieldName, gvMain.Columns[3].FieldName, summaryTimes, SummaryItemType.Sum);
         }

         GridHelper.SetCommonGrid(gvMain);
         gcMain.Visible = true;
         gvMain.OptionsView.ShowFooter = true;
         gvMain.OptionsBehavior.AllowFixedGroups = DefaultBoolean.True;
         gvMain.ExpandAllGroups();
         //設定每個column自動擴展
         gvMain.BestFitColumns();
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);

            //寫一行標題的註解,通常是查詢條件
            _ReportHelper.LeftMemo = "查詢日期 : " + txtStartDate.Text + "~" + txtEndDate.Text + "," +
                "系統別 : " + ddlbType.Text + ","  + "查詢類別 : " + ddlbCate.Text;

            _ReportHelper.Print();//如果有夜盤會特別標註

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      /// <summary>
      /// Get Column Caption 
      /// </summary>
      /// <param name="colIndex">欄位序</param>
      /// <param name="searchType"></param>
      /// <returns></returns>
      private string GetColumnCaption(int colIndex, string searchType) {
         string caption = "";

         switch (colIndex) {
            case 0: {
               caption = "期貨商";
               break;
            }
            case 1: {
               caption = "期貨商代號";
               break;
            }
            case 2: {
               caption = searchType == "I" ? "流水帳號" : "查詢戶數";
               break;
            }
            case 3: {
               caption = searchType == "I" ? "查詢日期" : "查詢次數";
               break;
            }
            case 4: {
               caption = "查詢次數";
               break;
            }
         }
         return caption;
      }

      private bool CheckInputText(string txtDate) {
         string txt = txtDate.TrimEnd('%');

         if (string.IsNullOrEmpty(txt)) {
            return true;
         }

         if (txt.AsDateTime("yyyyMMdd") != default(DateTime)) {
            return true;
         }

         if (txt.AsDateTime("yyyyMM") != default(DateTime)) {
            return true;
         }

         return false;
      }
   }
}