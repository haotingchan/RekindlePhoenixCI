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
using BusinessObjects;
using BaseGround.Report;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.IO;

/// <summary>
/// Lukas, 2019/2/20
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {

    /// <summary>
    /// 20420 交易量各類別身份碼設定
    /// </summary>
    public partial class W20420 : FormParent {

        private D20420 dao20420;
        protected ReportHelper _ReportHelper;
        private RepositoryItemLookUpEdit _RepLookUpEdit;

        public W20420(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;
            dao20420 = new D20420();
        }

        protected override ResultStatus Open() {
            base.Open();


            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            //設定下拉選單
            //報表別
            DataTable dtTxnId = dao20420.dddw_idfg_txn_id();
            Extension.SetDataTable(dw_txn_id, dtTxnId, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, "");
            //身分碼歸屬
            DataTable dtType = dao20420.dddw_idfg_type();
            _RepLookUpEdit = new RepositoryItemLookUpEdit();
            Extension.SetColumnLookUp(_RepLookUpEdit, dtType, "COD_ID", "CP_DISPLAY", TextEditStyles.DisableTextEditor, "");
            gcMain.RepositoryItems.Add(_RepLookUpEdit);
            IDFG_TYPE.ColumnEdit = _RepLookUpEdit;

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
            _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = true;//列印

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {

            //讀取下拉選單所選擇的資料
            DataTable returnTable = dao20420.ListAllByTxnId(dw_txn_id.EditValue.AsString());

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

        protected override ResultStatus InsertRow() {

            if (gvMain.RowCount == 0) {
                DataTable dtEmpty = dao20420.ListAllByTxnId("");
                dtEmpty.Columns.Add("Is_NewRow", typeof(string));
                gcMain.DataSource = dtEmpty;
            }

            base.InsertRow(gvMain);
            gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            base.DeleteRow(gvMain);

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

            //取得idfg_table_id
            string idfgTableId = dt.Rows[0]["IDFG_TABLE_ID"].AsString();

            //賦值
            foreach (DataRow dr in dt.Rows) {
                if (dr.RowState == DataRowState.Added) {
                    dr["IDFG_W_TIME"] = DateTime.Now;
                    dr["IDFG_W_USER_ID"] = GlobalInfo.USER_ID;
                    dr["IDFG_TABLE_ID"] = idfgTableId;
                }
            }

            //寫入DB
            try {
                Save_Override(dt.GetChanges(), "IDFG");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            //重新Retrieve
            gcMain.RefreshDataSource();

            //不要自動列印
            _IsPreventFlowPrint = true;
            return ResultStatus.Success;
        }

        protected override ResultStatus Export() {

            if (gvMain.RowCount == 0) {
                MessageDisplay.Error("畫面中無資料可轉出！");
            }
            /*******************
            點選儲存檔案之目錄
            *******************/
            string rpt;
            rpt = dw_txn_id.EditValue.AsString();
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "*.csv (*.csv)|*.csv";
            save.Title = "請點選儲存檔案之目錄";
            save.InitialDirectory = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH;
            save.FileName = _ProgramID + "_" + rpt + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
            DialogResult saveResult = save.ShowDialog();
            if (saveResult == DialogResult.Cancel) {
                return ResultStatus.Fail;
            }
            this.Cursor = Cursors.WaitCursor;
            //開一張空的table來轉成CSV
            DataTable dtCSV = new DataTable();
            DataColumn dcType = new DataColumn("ACC_GRP", typeof(string));
            DataColumn dcCode = new DataColumn("ACC_CODE", typeof(string));
            dtCSV.Columns.Add(dcType);
            dtCSV.Columns.Add(dcCode);
            dtCSV.Rows.Add("身份碼歸屬", "身份碼");
            for (int i = 0; i < gvMain.RowCount; i++) {
                dtCSV.Rows.Add();
                dtCSV.Rows[i + 1]["ACC_GRP"] = IDFG_TYPE.ColumnEdit.GetDisplayText(gvMain.GetRowCellValue(i, gvMain.Columns["IDFG_TYPE"])).AsString();
                dtCSV.Rows[i + 1]["ACC_CODE"] = gvMain.GetRowCellValue(i, gvMain.Columns["IDFG_ACC_CODE"]).AsString();
            }
            //存CSV (ps:輸出csv 都用ascii)
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = false;
            csvref.Encoding = Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dtCSV, save.FileName, csvref);
#if DEBUG
            //存完直接開檔檢視
            System.Diagnostics.Process.Start(save.FileName);
#endif
            this.Cursor = Cursors.Arrow;
            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            _ReportHelper = reportHelper;
            CommonReportPortraitA4 report = new CommonReportPortraitA4();
            report.printableComponentContainerMain.PrintableComponent = gcMain;
            _ReportHelper.Create(report);
            //_ReportHelper.LeftMemo = "設定權限給(" + cbxUserId.Text.Trim() + ")";

            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        /// <summary>
        /// 變更選項時，gridview會清空，同時insert row的idfg_type欄位會隨著選定的報表別而變動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dw_txn_id_EditValueChanged(object sender, EventArgs e) {
            gcMain.DataSource = null;
            gcMain.RefreshDataSource();

            DataTable dtType = dao20420.dddw_idfg_type(dw_txn_id.EditValue.AsString());
            Extension.SetColumnLookUp(_RepLookUpEdit, dtType, "COD_ID", "CP_DISPLAY", TextEditStyles.DisableTextEditor, "");
            gcMain.RepositoryItems.Add(_RepLookUpEdit);
            IDFG_TYPE.ColumnEdit = _RepLookUpEdit;
        }

        #region GridControl事件

        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
        }

        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();
            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                e.Cancel = false;
            }
            else {
                e.Cancel = true;
            }
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();
            //描述每個欄位,在is_newRow時候要顯示的顏色
            //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
            switch (e.Column.FieldName) {
                case ("IDFG_TYPE"):
                    if (Is_NewRow == "1") {
                        e.Appearance.BackColor = Color.White;
                    }
                    else {
                        e.Appearance.BackColor = e.CellValue.AsString() == null ? Color.FromArgb(192, 220, 192) : Color.FromArgb(192, 192, 192);
                    }
                    break;
                case ("IDFG_ACC_CODE"):
                    e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192, 192, 192);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 這段暫不實作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void gvMain_CellValueChanged(object sender, CellValueChangedEventArgs e) {
        //    GridView gv = sender as GridView;
        //    gv.CloseEditor();
        //    string lastType = null;
        //    string nextType = null;
        //    if (e.Column.Name == "IDFG_TYPE" && e.RowHandle > 0) {
        //        for (int i = e.RowHandle - 1; i >= 0; i--) {
        //            if (gv.GetRowCellValue(i, gv.Columns["IDFG_TYPE"]).AsString() != null) {
        //                lastType = gv.GetRowCellValue(i, gv.Columns["IDFG_TYPE"]).AsString();
        //                break;
        //            }
        //        }
        //        if (e.Value.AsString() == lastType) {
        //            gv.SetRowCellValue(e.RowHandle, gv.Columns["IDFG_TYPE"], null);
        //        }
        //        else {
        //            gv.SetRowCellValue(e.RowHandle, gv.Columns["IDFG_TYPE"], " ");
        //        }
        //        for (int i = e.RowHandle + 1; i < gv.RowCount; i++) {
        //            if (gv.GetRowCellValue(i, gv.Columns["IDFG_TYPE"]).AsString() != null) {
        //                nextType = gv.GetRowCellValue(i, gv.Columns["IDFG_TYPE"]).AsString();
        //                break;
        //            }
        //        }
        //        if (e.Value.AsString() == nextType) {
        //            gv.SetRowCellValue(e.RowHandle, gv.Columns["IDFG_TYPE"], null);
        //        }
        //        else {
        //            gv.SetRowCellValue(e.RowHandle, gv.Columns["IDFG_TYPE"], " ");
        //        }
        //    }
        //}

        #endregion
    }
}