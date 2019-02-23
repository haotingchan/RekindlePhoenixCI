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
using BaseGround.Shared;
using BusinessObjects.Enums;
using DevExpress.XtraGrid.Views.Grid;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.TableDao;
using Common;

/// <summary>
/// Lukas, 2019/1/23
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {

    /// <summary>
    /// 20410 年度交易量預估值維護
    /// </summary>
    public partial class W20410 : FormParent {

        private ReportHelper _ReportHelper;
        private AM7 daoAM7;
        private LOGV daoLOGV;

        public W20410(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;

            daoAM7 = new AM7();
            txtEndDate.EditValue = PbFunc.f_ocf_date(0).SubStr(0, 4);
            txtStartDate.EditValue = txtEndDate.EditValue;
        }

        protected override ResultStatus Open() {
            base.Open();
            #region Column Header換行
            AM7_Y.Caption = Environment.NewLine + "年月";
            AM7_DAY_COUNT.Caption = "總交易" + Environment.NewLine + "天數";
            AM7_FUT_AVG_QNTY.Caption = "預估期貨" + Environment.NewLine + "目標日均量";
            AM7_OPT_AVG_QNTY.Caption = "預估選擇權" + Environment.NewLine + "目標日均量";
            AM7_FC_TAX.Caption = "預估" + Environment.NewLine + "目標稅收";
            AM7_FC_QNTY.Caption = "預估" + Environment.NewLine + "日均量";
            #endregion
            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
            string startDate = txtStartDate.Text;
            string endDate = txtEndDate.Text;
            DataTable dtCheck = daoAM7.ListAllByDate(startDate, endDate);

            //沒有新增資料時,則自動新增內容
            if (dtCheck.Rows.Count == 0) {
                dtCheck.Columns.Add("Is_NewRow", typeof(string));
                gcMain.DataSource = dtCheck;
                InsertRow();
            }
            else {
                dtCheck.Columns.Add("Is_NewRow", typeof(string));
                gcMain.DataSource = dtCheck;
                gcMain.Focus();
            }

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

            string startDate = txtStartDate.Text;
            string endDate = txtEndDate.Text;
            DataTable returnTable = daoAM7.ListAllByDate(startDate, endDate);

            if (returnTable.Rows.Count == 0) {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ResultStatus.Fail;
            }

            returnTable.Columns.Add("Is_NewRow", typeof(string));
            gcMain.DataSource = returnTable;
            gcMain.Focus();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield() {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            base.Save(gcMain);
            daoLOGV = new LOGV();
            DataTable dt = (DataTable)gcMain.DataSource;
            DataTable dtChange = dt.GetChanges();

            if (dtChange == null) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }
            //Update to DB
            else {
                string ls_type, ls_val1, ls_val2, ls_val3, ls_val4, ls_val5;
                //隱藏欄位賦值
                foreach (DataRow dr in dt.Rows) {
                    if (dr.RowState == DataRowState.Added) {
                        dr["AM7_FC_QNTY"] = 0;
                        dr["OP_TYPE"] = "I";
                        //寫LOGV
                        ls_type = dr["OP_TYPE"].AsString();
                        ls_val1 = dr["AM7_Y"].AsString();
                        ls_val2 = dr["AM7_DAY_COUNT"].AsString();
                        ls_val3 = dr["AM7_FUT_AVG_QNTY"].AsString();
                        ls_val4 = dr["AM7_OPT_AVG_QNTY"].AsString();
                        ls_val5 = dr["AM7_FC_TAX"].AsString();
                        daoLOGV.Insert(_ProgramID, GlobalInfo.USER_ID, ls_type, ls_val1, ls_val2, ls_val3, ls_val4, ls_val5);
                    }
                    if (dr.RowState == DataRowState.Modified) {
                        dr["OP_TYPE"] = "U";
                        //寫LOGV
                        ls_type = dr["OP_TYPE"].AsString();
                        ls_val1 = dr["AM7_Y"].AsString();
                        ls_val2 = dr["AM7_DAY_COUNT"].AsString();
                        ls_val3 = dr["AM7_FUT_AVG_QNTY"].AsString();
                        ls_val4 = dr["AM7_OPT_AVG_QNTY"].AsString();
                        ls_val5 = dr["AM7_FC_TAX"].AsString();
                        daoLOGV.Insert(_ProgramID, GlobalInfo.USER_ID, ls_type, ls_val1, ls_val2, ls_val3, ls_val4, ls_val5);
                    }
                }
                ResultStatus status = base.Save_Override(dt, "AM7");
                if (status == ResultStatus.Fail) {
                    return ResultStatus.Fail;
                }
            }
            //不要自動列印
            _IsPreventFlowPrint = true;
            return ResultStatus.Success;
        }


        protected override ResultStatus Print(ReportHelper reportHelper) {
            _ReportHelper = reportHelper;
            CommonReportPortraitA4 report = new CommonReportPortraitA4();
            report.printableComponentContainerMain.PrintableComponent = gcMain;
            _ReportHelper.Create(report);

            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            base.InsertRow(gvMain);
            gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        #region GridControl事件

        /// <summary>
        /// 年月欄自動填上查詢年月(gvMain_InitNewRow事件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);

            //直接設定值給dataTable(have UI)
            gv.SetRowCellValue(e.RowHandle, gv.Columns["AM7_Y"], txtEndDate.Text);

        }

        /// <summary>
        /// 決定哪些欄位無法編輯的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                e.Cancel = false;
                gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
            }
            //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
            else if (gv.FocusedColumn.Name == "AM7_Y") {
                e.Cancel = true;
            }
            else {
                e.Cancel = false;
            }

        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            //要用RowHandle不要用FocusedRowHandle
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();

            //描述每個欄位,在is_newRow時候要顯示的顏色
            //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
            //當該欄位不可編輯時,AllowFocus為false(PB的wf_set_order方法)
            switch (e.Column.FieldName) {
                case ("AM7_Y"):
                    e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
                    e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192, 192, 192);
                    break;
                default:
                    e.Appearance.BackColor = Color.White;
                    break;
            }//switch (e.Column.FieldName) {

        }

        #endregion
    }
}