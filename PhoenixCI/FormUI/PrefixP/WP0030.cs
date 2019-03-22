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

/// <summary>
/// David, 2019/03/21 
/// </summary>
namespace PhoenixCI.FormUI.PrefixP {
    public partial class WP0030 : FormParent {

        private DP0030 daoP0030;

        protected class LookupItem {
            public string ValueMember { get; set; }
            public string DisplayMember { get; set; }
        }

        public WP0030(string programID, string programName) : base(programID, programName) {
            try {
                InitializeComponent();
                daoP0030 = new DP0030();

                this.Text = _ProgramID + "─" + _ProgramName;
                gvMain.OptionsBehavior.Editable = false;

                txtStartDate.Text = "%";
                txtEndDate.Text = "%";

                //下拉選單(系統別)
                List<LookupItem> ddlbSystem = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "W", DisplayMember = "W：網際網路"},
                                        new LookupItem() { ValueMember = "V", DisplayMember = "V：語音查詢" }};
                Extension.SetDataTable(ddlbType, ddlbSystem, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
                ddlbType.EditValue = "W";

                //下拉選單(類別)
                List<LookupItem> ddlbCatagroy = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "I", DisplayMember = "I：依交易人查明細"},
                                        new LookupItem() { ValueMember = "F", DisplayMember = "F：依期貨商合計" }};
                Extension.SetDataTable(ddlbCate, ddlbCatagroy, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
                ddlbCate.EditValue = "I";

            }
            catch (Exception ex) {
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
            string type = ddlbType.EditValue.AsString();
            string cate = ddlbCate.EditValue.AsString();
            string searchType = ddlbCate.Text.Substring(0, 1);
            string posconn = PbFunc.f_get_exec_oth("POS");//更換DB

            dtContentI = daoP0030.ExecuteStoredProcedure(txtStartDate.Text, txtEndDate.Text, type, "I", posconn);
            dtContentF = daoP0030.ExecuteStoredProcedure(txtStartDate.Text, txtEndDate.Text, type, "F", posconn);
            gcMain.DataSource = null;
            gvMain.GroupSummary.Clear();
            gvMain.Columns.Clear();//清除grid
            dtContentF.Columns.Remove(dtContentF.Columns["0"]);

            foreach (DataRow drI in dtContentI.Rows) {
                DataRow drF = dtContentF.Select("FCM_NAME =" + "'"+drI["FCM_NAME"].ToString()+"'")[0];
                if (drF["FCM_NAME"].AsString() == drI["FCM_NAME"].ToString()) {
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

            //依交易人查詢
            if (searchType == "I") {
                gvMain.Columns[0].Group();
                gvMain.Columns.Last().Visible = false;
                gvMain.OptionsView.AllowCellMerge = true;
                //設定群組 小記
                GridGroupSummaryItem timesSummary = new GridGroupSummaryItem();
                timesSummary.FieldName = gvMain.Columns[4].FieldName;
                timesSummary.ShowInGroupColumnFooter = gvMain.Columns[4];
                timesSummary.SummaryType = SummaryItemType.Sum;
                timesSummary.DisplayFormat = "合計{0}次";
                gvMain.GroupSummary.Add(timesSummary);               

                GridGroupSummaryItem accountSummary = new GridGroupSummaryItem();
                accountSummary.FieldName = gvMain.Columns[5].FieldName;
                accountSummary.ShowInGroupColumnFooter = gvMain.Columns[2];
                accountSummary.SummaryType = SummaryItemType.Max;
                accountSummary.DisplayFormat = "合計{0}戶";
                gvMain.GroupSummary.Add(accountSummary);

                //客製化總記欄位
                //int tolsum = 0;
                //List<string> tolAccountNo = new List<string>();
                //gvMain.CustomSummaryCalculate += (sender, e) => {
                //    GridView gv = sender as GridView;
                //    if (e.IsTotalSummary && (e.Item as GridColumnSummaryItem).FieldName == gv.Columns[2].FieldName) {
                //        GridColumnSummaryItem item = e.Item as GridColumnSummaryItem;
                //        if (item.FieldName == gv.Columns[2].FieldName) {
                //            switch (e.SummaryProcess) {
                //                case CustomSummaryProcess.Start:
                //                    tolsum = 0;
                //                    tolAccountNo = new List<string>();
                //                    break;
                //                case CustomSummaryProcess.Calculate:
                //                    if (tolAccountNo.Where(a => a == e.GetValue(gv.Columns[2].FieldName).AsString()).Count() == 0) {
                //                        tolAccountNo.Add(e.GetValue(gv.Columns[2].FieldName).AsString());
                //                        tolsum += 1;
                //                    }
                //                    break;
                //                case CustomSummaryProcess.Finalize:
                //                    e.TotalValue = tolsum;
                //                    break;
                //            }
                //        }
                //    }
                //};

                GridColumnSummaryItem tolAccountSummary = new GridColumnSummaryItem();
                tolAccountSummary.FieldName = gvMain.Columns[5].FieldName;
                tolAccountSummary.SummaryType = SummaryItemType.Sum;
                tolAccountSummary.DisplayFormat = "總計{0}戶";
                gvMain.Columns[2].Summary.Add(tolAccountSummary);

                //總計
                GridColumnSummaryItem columnSummary = new GridColumnSummaryItem();
                columnSummary.FieldName = gvMain.Columns[4].FieldName;
                columnSummary.SummaryType = SummaryItemType.Sum;
                columnSummary.DisplayFormat = "總計{0}次";
                gvMain.Columns[4].Summary.Add(columnSummary);
            }//if searchType="I"
            else {//searchType="F"

                GridGroupSummaryItem accountSummary = new GridGroupSummaryItem();
                accountSummary.FieldName = gvMain.Columns[2].FieldName;
                accountSummary.ShowInGroupColumnFooter = gvMain.Columns[2];
                accountSummary.SummaryType = SummaryItemType.Sum;
                accountSummary.DisplayFormat = "合計{0}戶";
                gvMain.GroupSummary.Add(accountSummary);

                GridColumnSummaryItem tolAccountSummary = new GridColumnSummaryItem();
                tolAccountSummary.FieldName = gvMain.Columns[2].FieldName;
                tolAccountSummary.SummaryType = SummaryItemType.Sum;
                tolAccountSummary.DisplayFormat = "總計{0}戶";
                gvMain.Columns[2].Summary.Add(tolAccountSummary);

                //總計
                GridColumnSummaryItem columnSummary = new GridColumnSummaryItem();
                columnSummary.FieldName = gvMain.Columns[3].FieldName;
                columnSummary.SummaryType = SummaryItemType.Sum;
                columnSummary.DisplayFormat = "總計{0}次";
                gvMain.Columns[3].Summary.Add(columnSummary);
            }

            //GridHelper.SetCommonGrid(gvMain);
            gcMain.Visible = true;
            gvMain.OptionsView.ShowFooter = true;
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
                _ReportHelper.LeftMemo = "查詢日期 : " + txtStartDate.Text + "~" + txtEndDate.Text + Environment.NewLine +
                    "系統別 : " + ddlbType.Text + Environment.NewLine + Environment.NewLine + "查詢類別 : " + ddlbCate.Text;

                _ReportHelper.Print();//如果有夜盤會特別標註

                return ResultStatus.Success;
            }
            catch (Exception ex) {
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

            if (txtDate == "%" || txtDate.AsDateTime() != default(DateTime)) {
                return true;
            }
            return false;
        }

    }
}