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
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.TableDao;

/// <summary>
/// Lukas, 2019/1/23
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {

    /// <summary>
    /// 20233 標的證券在外流通股數(B)查詢 
    /// </summary>
    public partial class W20233 : FormParent {

        private ReportHelper _ReportHelper;
        private STKOUT daoSTKOUT;
        //private HPDK daoHPDK; PB有開datawindow，但似乎沒用到

        public W20233(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;

            txtDate.EditValue = PbFunc.f_ocf_date(0);
        }

        protected override ResultStatus Open() {
            base.Open();

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

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

            daoSTKOUT = new STKOUT();
            DataTable returnTable = daoSTKOUT.ListAllByDate(txtDate.Text.Replace("/", ""));

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
            //if (cbxUserId.SelectedItem == null) {
            //    MessageDisplay.Warning("使用者代號不可為空白!");
            //    return ResultStatus.Fail;
            //}

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            base.Save(gcMain);

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
            //寫入資料庫
            else {
                foreach (DataRow dr in dt.Rows) {
                    if (dr.RowState == DataRowState.Added) {
                        dr["STKOUT_TIME"] = DateTime.Now.ToString("hhmm");
                    }
                }
                string tableName = "CI.STKOUT";
                string keysColumnList = "STKOUT_ID, STKOUT_DATE";
                string insertColumnList = "STKOUT_ID, STKOUT_NAME, STKOUT_B, STKOUT_DATE, STKOUT_TIME";
                string updateColumnList = insertColumnList;
                try {
                    ResultData myResultData = serviceCommon.SaveForChanged(dt, tableName, insertColumnList, updateColumnList, keysColumnList, pokeBall);
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
            _IsPreventFlowPrint = true;
            return ResultStatus.Success;
        }


        protected override ResultStatus Print(ReportHelper reportHelper) {
            _ReportHelper = reportHelper;
            CommonReportLandscapeA4 report = new CommonReportLandscapeA4();
            report.printableComponentContainerMain.PrintableComponent = gcMain;
            _ReportHelper.Create(report);

            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            int focusIndex = gvMain.GetFocusedDataSourceRowIndex();

            gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

            //新增一行並做初始值設定
            DataTable dt = (DataTable)gcMain.DataSource;
            DataRow drNew = dt.NewRow();

            drNew["Is_NewRow"] = 1;
            drNew["STKOUT_DATE"] = txtDate.Text.Replace("/", "");

            dt.Rows.InsertAt(drNew, focusIndex);
            gcMain.DataSource = dt;//重新設定給grid,雖然會更新但是速度太快,畫面不會閃爍
            gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        #region GridControl事件

        /// <summary>
        /// Insert New Row(給Is_NewRow欄位賦值供辨認用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            //GridView gv = sender as GridView;
            //gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
            //gv.SetRowCellValue(e.RowHandle, gv.Columns["STKOUT_DATE"], txtDate.Text.Replace("/", ""));
        }

        /// <summary>
        /// 當user click一筆時會觸發此事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            try {
                GridView gv = sender as GridView;
                string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                                   gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();
                //決定哪些欄位無法編輯的事件
                //如果是新的一行,則開放編輯
                if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                    e.Cancel = false;
                }
                //舊的筆數,則判斷每個欄位是否能編輯,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
                else if (gv.FocusedColumn.FieldName == "STKOUT_ID" ||
                         gv.FocusedColumn.FieldName == "STKOUT_NAME" ||
                         gv.FocusedColumn.FieldName == "STKOUT_B") {
                    e.Cancel = true;
                }
                else {
                    e.Cancel = true;
                }
            }
            catch (Exception ex) {
                PbFunc.f_write_logf(_ProgramID, "error", "gvMain_ShowingEditor error,des=" + ex.ToString());
            }
        }

        /// <summary>
        /// 會跑NxN次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            try {
                //描述每個欄位,在is_newRow時候要顯示的顏色
                //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
                switch (e.Column.FieldName) {
                    case ("STKOUT_DATE"):
                        e.Appearance.BackColor = Color.Transparent;
                        break;
                    case ("STKOUT_ID"):
                    case ("STKOUT_NAME"):
                    case ("STKOUT_B"):
                        GridView gv = sender as GridView;
                        string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                                           gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();
                        e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192, 192, 192);
                        break;
                }//switch (e.Column.FieldName) {
            }
            catch (Exception ex) {
                PbFunc.f_write_logf(_ProgramID, "error", "gvMain_RowCellStyle error,des=" + ex.ToString());
            }
        }


        #endregion


    }
}