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
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using Common;
using BusinessObjects.Enums;
using BaseGround.Shared;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;

/// <summary>
/// Lukas, 2019/5/8
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

    public partial class W40072 : FormParent {

        /// <summary>
        /// 調整類型 0一般 1長假 2處置股票 3股票
        /// </summary>
        protected string is_adj_type { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        protected string ymd { get; set; }
        private D40071 dao40071;
        private MGD2 daoMGD2;
        private MGD2L daoMGD2L;
        private MGRT1 daoMGRT1;
        private RepositoryItemLookUpEdit rateLookUpEdit;
        private RepositoryItemLookUpEdit prodTypeLookUpEdit1;//期貨
        private RepositoryItemLookUpEdit prodTypeLookUpEdit2;//選擇權

        public W40072(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao40071 = new D40071();
            daoMGD2 = new MGD2();
            daoMGD2L = new MGD2L();
            daoMGRT1 = new MGRT1();
            GridHelper.SetCommonGrid(gvMain);
            GridHelper.SetCommonGrid(gvDetail);
            gvDetail.AppearancePrint.BandPanel.Font = new Font("Microsoft YaHei", 10);
            gvDetail.AppearancePrint.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        }

        protected override ResultStatus Open() {
            base.Open();
            //設定日期和全域變數
            txtSDate.DateTimeValue = DateTime.Now;
#if DEBUG
            txtSDate.EditValue = "2018/12/28";
#endif
            ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
            is_adj_type = "2";

            //取得table的schema，因為程式開啟會預設插入一筆空資料列
            DataTable dtMGD2 = dao40071.d_40071();
            gcMain.DataSource = dtMGD2;

            #region 下拉選單設定
            //調整倍數下拉選單
            List<LookupItem> rateList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1.5", DisplayMember = "1.5"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "2"},
                                        new LookupItem() { ValueMember = "3", DisplayMember = "3" }};
            rateLookUpEdit = new RepositoryItemLookUpEdit();
            rateLookUpEdit.SetColumnLookUp(rateList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
            gcMain.RepositoryItems.Add(rateLookUpEdit);
            RATE_INPUT.ColumnEdit = rateLookUpEdit;

            //級距下拉選單
            //期貨
            DataTable dtLevel = daoMGRT1.dddw_mgrt1("F");
            prodTypeLookUpEdit1 = new RepositoryItemLookUpEdit();
            prodTypeLookUpEdit1.SetColumnLookUp(dtLevel, "MGRT1_LEVEL", "MGRT1_LEVEL_NAME", TextEditStyles.DisableTextEditor, null);
            gcDetail.RepositoryItems.Add(prodTypeLookUpEdit1);
            //選擇權
            dtLevel = daoMGRT1.dddw_mgrt1("O");
            prodTypeLookUpEdit2 = new RepositoryItemLookUpEdit();
            prodTypeLookUpEdit2.SetColumnLookUp(dtLevel, "MGRT1_LEVEL", "MGRT1_LEVEL_NAME", TextEditStyles.DisableTextEditor, null);
            gcDetail.RepositoryItems.Add(prodTypeLookUpEdit2);
            #endregion

            //預設新增一筆設定資料
            InsertRow();


            //txtSDate.Focus();
            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = true;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = true;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = true;//列印

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {

            try {
                //清空Grid
                gcMain.DataSource = null;
                gcDetail.DataSource = null;

                //日期檢核
                if (txtSDate.Text == "1901/01/01") {
                    MessageDisplay.Error("請輸入交易日期");
                    return ResultStatus.Fail;
                }
                int ii_curr_row = 0;
                string ls_kind_id = "", ls_ab_type, ls_stock_id = "";
                //讀取資料
                DataTable dtMGD2 = dao40071.d_40071(ymd, is_adj_type);
                if (dtMGD2.Rows.Count == 0) {
                    MessageDisplay.Error("無任何資料！");
                    return ResultStatus.Fail;
                }
                dtMGD2 = dtMGD2.Sort("mgd2_stock_id,mgd2_kind_id");

                //準備兩個空的table給兩個grid
                DataTable dtInput = dao40071.d_40071();
                DataTable dtDetail = dao40071.d_40071_detail();
                dtDetail.Columns["ADJ_TYPE"].ColumnName = "OP_TYPE";
                dtDetail.Columns["DATA_YMD"].ColumnName = "YMD";

                //依條件將讀取來的資料分配給兩個grid
                foreach (DataRow dr in dtMGD2.Rows) {
                    if (ls_stock_id != dr["MGD2_STOCK_ID"].AsString()) {
                        ls_stock_id = dr["MGD2_STOCK_ID"].AsString();
                        ii_curr_row = dtInput.Rows.Count;
                        dtInput.Rows.Add();
                        dtInput.Rows[ii_curr_row]["STOCK_ID"] = ls_stock_id;
                        dtInput.Rows[ii_curr_row]["RATE"] = dr["MGD2_ADJ_RATE"].AsDecimal() + 1;
                        dtInput.Rows[ii_curr_row]["PUB_YMD"] = dr["MGD2_PUB_YMD"];
                        dtInput.Rows[ii_curr_row]["IMPL_BEGIN_YMD"] = dr["MGD2_IMPL_BEGIN_YMD"];
                        dtInput.Rows[ii_curr_row]["IMPL_END_YMD"] = dr["MGD2_IMPL_END_YMD"];

                        dtInput.Rows[ii_curr_row]["ISSUE_BEGIN_YMD"] = dr["MGD2_ISSUE_BEGIN_YMD"];
                        dtInput.Rows[ii_curr_row]["ISSUE_END_YMD"] = dr["MGD2_ISSUE_END_YMD"];
                        dtInput.Rows[ii_curr_row]["YMD"] = dr["MGD2_YMD"];
                    }
                    if (ls_kind_id != dr["MGD2_KIND_ID"].AsString()) {
                        ls_kind_id = dr["MGD2_KIND_ID"].AsString();
                        ii_curr_row = dtDetail.Rows.Count;
                        dtDetail.Rows.Add();
                        dtDetail.Rows[ii_curr_row]["PROD_TYPE"] = dr["MGD2_PROD_TYPE"];
                        dtDetail.Rows[ii_curr_row]["KIND_ID"] = dr["MGD2_KIND_ID"];
                        dtDetail.Rows[ii_curr_row]["STOCK_ID"] = dr["MGD2_STOCK_ID"];
                        dtDetail.Rows[ii_curr_row]["ADJ_RATE"] = dr["MGD2_ADJ_RATE"];
                        dtDetail.Rows[ii_curr_row]["DATA_FLAG"] = "Y";

                        dtDetail.Rows[ii_curr_row]["PROD_SUBTYPE"] = dr["MGD2_PROD_SUBTYPE"];
                        dtDetail.Rows[ii_curr_row]["PARAM_KEY"] = dr["MGD2_PARAM_KEY"];
                        dtDetail.Rows[ii_curr_row]["M_CUR_LEVEL"] = dr["MGD2_CUR_LEVEL"];
                        dtDetail.Rows[ii_curr_row]["CURRENCY_TYPE"] = dr["MGD2_CURRENCY_TYPE"];
                        dtDetail.Rows[ii_curr_row]["SEQ_NO"] = dr["MGD2_SEQ_NO"];

                        dtDetail.Rows[ii_curr_row]["OSW_GRP"] = dr["MGD2_OSW_GRP"];
                        dtDetail.Rows[ii_curr_row]["AMT_TYPE"] = dr["MGD2_AMT_TYPE"];
                        dtDetail.Rows[ii_curr_row]["ISSUE_BEGIN_YMD"] = dr["MGD2_ISSUE_BEGIN_YMD"];
                        dtDetail.Rows[ii_curr_row]["ISSUE_END_YMD"] = dr["MGD2_ISSUE_END_YMD"];
                        dtDetail.Rows[ii_curr_row]["IMPL_BEGIN_YMD"] = dr["MGD2_IMPL_BEGIN_YMD"];

                        dtDetail.Rows[ii_curr_row]["IMPL_END_YMD"] = dr["MGD2_IMPL_END_YMD"];
                        dtDetail.Rows[ii_curr_row]["PUB_YMD"] = dr["MGD2_PUB_YMD"];
                        dtDetail.Rows[ii_curr_row]["YMD"] = dr["MGD2_YMD"];
                    }
                    if (dr["MGD2_AB_TYPE"].AsString() == "B") {
                        dtDetail.Rows[ii_curr_row]["CM_CUR_B"] = dr["MGD2_CUR_CM"];
                        dtDetail.Rows[ii_curr_row]["MM_CUR_B"] = dr["MGD2_CUR_MM"];
                        dtDetail.Rows[ii_curr_row]["IM_CUR_B"] = dr["MGD2_CUR_IM"];
                        dtDetail.Rows[ii_curr_row]["CM_B"] = dr["MGD2_CM"];
                        dtDetail.Rows[ii_curr_row]["MM_B"] = dr["MGD2_MM"];

                        dtDetail.Rows[ii_curr_row]["IM_B"] = dr["MGD2_IM"];
                    }
                    else {
                        dtDetail.Rows[ii_curr_row]["CM_CUR_A"] = dr["MGD2_CUR_CM"];
                        dtDetail.Rows[ii_curr_row]["MM_CUR_A"] = dr["MGD2_CUR_MM"];
                        dtDetail.Rows[ii_curr_row]["IM_CUR_A"] = dr["MGD2_CUR_IM"];
                        dtDetail.Rows[ii_curr_row]["CM_A"] = dr["MGD2_CM"];
                        dtDetail.Rows[ii_curr_row]["MM_A"] = dr["MGD2_MM"];

                        dtDetail.Rows[ii_curr_row]["IM_A"] = dr["MGD2_IM"];
                    }
                }//foreach (DataRow dr in dtMGD2.Rows)

                //資料繫結
                gcMain.DataSource = dtInput;
                gcDetail.DataSource = dtDetail;

                //若無資料，預設新增一筆設定資料
                if (gvDetail.RowCount==0) {
                    InsertRow();
                }

            }
            catch (Exception ex) {
                MessageDisplay.Error("讀取錯誤");
                throw ex;
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            base.InsertRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        #region GridView Events
        /// <summary>
        /// 級距下拉選單根據商品類別轉換的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetail_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e) {
            GridView gv = sender as GridView;
            gv.CloseEditor();
            gv.UpdateCurrentRow();
            if (e.Column.FieldName == "M_CUR_LEVEL") {
                string ls_prod_type = gv.GetRowCellValue(e.RowHandle, "PROD_TYPE").AsString();
                if (ls_prod_type == "F") e.RepositoryItem = prodTypeLookUpEdit1;
                if (ls_prod_type == "O") e.RepositoryItem = prodTypeLookUpEdit2;
            }
        }

        private void gvDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) {
            GridView gv = sender as GridView;
            if (e.Column.Name != "OP_TYPE") {
                //如果OP_TYPE是I則固定不變
                if (gv.GetRowCellValue(e.RowHandle, "OP_TYPE").ToString() == " ") gv.SetRowCellValue(e.RowHandle, "OP_TYPE", "U");
            }

        }
        #endregion


    }
}