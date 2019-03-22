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

/// <summary>
/// David, 2019/03/21
/// </summary>
namespace PhoenixCI.FormUI.PrefixP {
    /// <summary>
    /// P0020 查詢資料明細
    /// 功能：Retrieve, Print
    /// </summary>
    public partial class WP0050 : FormParent {

        private DP0050 daoP0050;

        protected class LookupItem {
            public string ValueMember { get; set; }
            public string DisplayMember { get; set; }
        }

        public WP0050(string programID, string programName) : base(programID, programName) {
            try {
                InitializeComponent();
                daoP0050 = new DP0050();

                this.Text = _ProgramID + "─" + _ProgramName;
                gvMain.OptionsBehavior.Editable = false;

                txtStartDate.Text = "%";
                txtEndDate.Text = "%";
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

            DataTable dtContent = new DataTable();
            string posconn = PbFunc.f_get_exec_oth("POS");

            dtContent = daoP0050.ExecuteStoredProcedure(txtStartDate.Text, txtEndDate.Text, posconn);
            gcMain.DataSource = null;
            gvMain.GroupSummary.Clear();
            gvMain.Columns.Clear();//清除grid
            gcMain.DataSource = dtContent;

            foreach (DataColumn dc in dtContent.Columns) {
                //設定欄位屬性
                gvMain.SetColumnCaption(dc.ColumnName, GetColumnCaption(dc.Ordinal));
                gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.VAlignment = VertAlignment.Top;
                //設定合併欄位(一樣的值不顯示)
                gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = (dc.Ordinal != 0 && dc.Ordinal != 1) ? DefaultBoolean.False : DefaultBoolean.True;
            }

            //設定群組 小記          
            gvMain.OptionsView.AllowCellMerge = true;
            gvMain.Columns[1].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            gvMain.Columns[2].SortOrder = DevExpress.Data.ColumnSortOrder.None;
            gvMain.Columns[1].Group();
            gvMain.Columns[2].Group();

            gvMain.SetGridGroupSummary(gvMain.Columns[1].FieldName, "總計{0}", DevExpress.Data.SummaryItemType.Count);

            GridHelper.SetCommonGrid(gvMain);
            gcMain.Visible = true;
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
                _ReportHelper.LeftMemo = "查詢日期 : " + txtStartDate.Text + "~" + txtEndDate.Text + Environment.NewLine;

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
        private string GetColumnCaption(int colIndex) {
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
                    caption = "流水帳號";
                    break;
                }
                case 3: {
                    caption = "鎖住日期";
                    break;
                }
                case 4: {
                    caption = "網際網路累積次數";
                    break;
                }
                case 5: {
                    caption = "電話語音累積次數";
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