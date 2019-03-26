using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BaseGround;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.TableDao;
using BaseGround.Shared;
using Common;

/// <summary>
/// Lukas, 2019/1/22
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {
    /// <summary>
    /// 20310 每月成交值輸入
    /// </summary>
    public partial class W20310 : FormParent {

        private ReportHelper _ReportHelper;
        private AA1 daoAA1;

        public W20310(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            daoAA1 = new AA1();
            txtStartDate.EditValue = PbFunc.f_ocf_date(0).SubStr(0, 4) + "/01";
            txtEndDate.EditValue = PbFunc.f_ocf_date(0).SubStr(0, 7);
        }

        protected override ResultStatus Open() {
            base.Open();
            AA1_YM.Caption = Environment.NewLine + "年月";
            AA1_TAIFEX.Caption = "Taifex" + Environment.NewLine + "指數期貨總成交值";
            AA1_TSE.Caption = "TWSE" + Environment.NewLine + "股票合計成交值";
            AA1_OTC.Caption = "OTC" + Environment.NewLine + "股票合計成交值";
            AA1_DAY_COUNT.Caption = Environment.NewLine + "月交易天數";
            AA1_US_RATE.Caption = Environment.NewLine + "美元匯率";

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
            string startDate = txtStartDate.Text.Replace("/", "");
            string endDate = txtEndDate.Text.Replace("/", "");
            DataTable dtCheck = daoAA1.ListAllByDate(startDate, endDate);

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
            try {
                string startDate = txtStartDate.Text.Replace("/", "");
                string endDate = txtEndDate.Text.Replace("/", "");
                DataTable returnTable = daoAA1.ListAllByDate(startDate, endDate);

                if (returnTable.Rows.Count == 0) {
                    MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return ResultStatus.Fail;
                }

                returnTable.Columns.Add("Is_NewRow", typeof(string));
                gcMain.DataSource = returnTable;
                gcMain.Focus();
            }
            catch (Exception ex) {
                throw ex;
            }
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
            try {
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
                //更新主要Table
                else {
                    ResultData myResultData = daoAA1.updateAA1(dt);
                }
                //不要自動列印
                _IsPreventFlowPrint = true;
            }
            catch (Exception ex) {
                MessageDisplay.Error("儲存錯誤");
                throw ex;
            }
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
            gv.SetRowCellValue(e.RowHandle, gv.Columns["AA1_YM"], txtEndDate.Text.Replace("/", ""));
            gv.SetRowCellValue(e.RowHandle, gv.Columns["AA1_SGX_DT"], 0);

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
            else if (gv.FocusedColumn.Name == "AA1_YM") {
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
                case ("AA1_YM"):
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