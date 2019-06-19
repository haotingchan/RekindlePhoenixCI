using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.TableDao.REWARD;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace PhoenixCI.FormUI.Prefix5
{
    /// <summary>
    /// 50074 造市者股票期貨及選擇權交易標的維護
    /// </summary>
    public partial class W50074 : FormParent
    {
        private R_MARKET_STF_EXCL dao;

        public W50074(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open()
        {
            base.Open();
            try
            {
                dao = new R_MARKET_STF_EXCL();

                Retrieve();
                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                return ResultStatus.Fail;
            }
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = true;
            _ToolBtnSave.Enabled = true;
            _ToolBtnDel.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            try
            {
                DataTable dt = dao.ListData();

                //check (沒有資料時,則自動新增一筆)
                if (dt.Rows.Count <= 0)
                {
                    InsertRow();
                }

                //設定gvExport
                gvMain.Columns.Clear();
                gvMain.OptionsBehavior.AutoPopulateColumns = true;
                gcMain.DataSource = dt;
                gvMain.BestFitColumns();
                GridHelper.SetCommonGrid(gvMain);

                //設定欄位caption       
                gvMain.SetColumnCaption("MC_MONTH", "月份");
                gvMain.SetColumnCaption("GOODS_ID", "標的代號");
                gvMain.SetColumnCaption("IS_NEWROW", "Is_NewRow");
                gvMain.Columns["MC_MONTH"].AppearanceHeader.BackColor = gvMain.Columns["GOODS_ID"].AppearanceHeader.BackColor = Color.FromArgb(255, 255, 128);

                //設定欄位format格式
                RepositoryItemTextEdit month = new RepositoryItemTextEdit();
                gcMain.RepositoryItems.Add(month);
                gvMain.Columns["MC_MONTH"].ColumnEdit = month;
                month.MaxLength = 6;

                RepositoryItemTextEdit id = new RepositoryItemTextEdit();
                gcMain.RepositoryItems.Add(id);
                gvMain.Columns["GOODS_ID"].ColumnEdit = id;
                id.MaxLength = 7;

                //設定隱藏欄位
                gvMain.Columns["IS_NEWROW"].Visible = false;

                gcMain.Focus();

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }

        protected override ResultStatus Save(PokeBall poke)
        {
            DataTable dtCurrent = (DataTable)gcMain.DataSource;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dtChange = dtCurrent.GetChanges();
            DataTable dtForAdd = dtCurrent.GetChanges(DataRowState.Added);
            DataTable dtForModified = dtCurrent.GetChanges(DataRowState.Modified);
            DataTable dtForDeleted = dtCurrent.GetChanges(DataRowState.Deleted);

            if (dtChange == null)
            {
                MessageDisplay.Info("沒有變更資料,不需要存檔!");
                return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0)
            {
                MessageDisplay.Info("沒有變更資料,不需要存檔!");
                return ResultStatus.Fail;
            }

            dtChange = dtCurrent.GetChanges();
            ResultData result = dao.UpdateData(dtChange);
            if (result.Status == ResultStatus.Fail)
            {
                return ResultStatus.Fail;
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            try
            {
                ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                _ReportHelper.Print();
                _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);

                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }

        protected override ResultStatus InsertRow()
        {
            DataTable dt = (DataTable)gcMain.DataSource;
            gvMain.AddNewRow();

            gvMain.SetRowCellValue(GridControl.NewItemRowHandle, gvMain.Columns["MC_MONTH"], "");
            gvMain.SetRowCellValue(GridControl.NewItemRowHandle, gvMain.Columns["GOODS_ID"], "");
            gvMain.SetRowCellValue(GridControl.NewItemRowHandle, gvMain.Columns["IS_NEWROW"], 1);

            gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow(gvMain);
            return ResultStatus.Success;
        }

        #region GridControl事件
        /// <summary>
        /// 決定哪些欄位無法編輯的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]).ToString();

            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1")
            {
                e.Cancel = false;
                gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
            }

            //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
            else if (gv.FocusedColumn.Name == "MC_MONTH" || gv.FocusedColumn.Name == "GOODS_ID")
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //要用RowHandle不要用FocusedRowHandle
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]).ToString();

            //描述每個欄位,在 IS_NEWROW 時候要顯示的顏色
            //當該欄位不可編輯時,設定為灰色,AllowFocus為false(PB的wf_set_order方法)
            switch (e.Column.FieldName)
            {
                case ("MC_MONTH"):
                case ("GOODS_ID"):
                    e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
                    e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
                    break;
                default:
                    e.Appearance.BackColor = Color.White;
                    break;
            }
        }
        #endregion
    }
}