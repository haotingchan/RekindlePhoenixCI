using ActionService;
using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ0020 : FormParent
    {
      private TXN daoTXN;
      private UTP daoUTP;
      private LOGUTP daoLOGUTP;

      public WZ0020(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain);

            gvMain.AppearancePrint.HeaderPanel.Font = new Font("標楷體", 10);
            gvMain.AppearancePrint.Row.Font = new Font("標楷體", 10);
            gvMain.ViewCaptionHeight = 30;

            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;

            RepositoryItemCheckEdit repCheck = new RepositoryItemCheckEdit();
            repCheck.AllowGrayed = false;
            repCheck.ValueChecked = "Y";
            repCheck.ValueUnchecked = "N";
            repCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;

            gcMain.RepositoryItems.Add(repCheck);
            TXN_DEFAULT.ColumnEdit = repCheck;
            TXN_EXTEND.ColumnEdit = repCheck;

            DataTable dtType = new COD().ListByTxn("TXN");

            RepositoryItemLookUpEdit cbxType = new RepositoryItemLookUpEdit();
            cbxType.SetColumnLookUp(dtType, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, "");
            TXN_TYPE.ColumnEdit = cbxType;
            TXN_TYPE.ShowButtonMode = ShowButtonModeEnum.ShowAlways;

            daoTXN = new TXN();
             daoUTP = new UTP();
             daoLOGUTP = new LOGUTP();
      }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen()
        {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = true;
            _ToolBtnSave.Enabled = true;
            _ToolBtnDel.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);

            gcMain.DataSource = daoTXN.ListData();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }

            DataTable dtMain = (DataTable)gcMain.DataSource;

            foreach (DataRow row in dtMain.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Unchanged)
                {

                        row["TXN_W_USER_ID"] = GlobalInfo.USER_ID;
                        row["TXN_W_TIME"] = DateTime.Now;
                    
                }
            }

            string[] allowNullColumnList = { "TXN_RMARK", "TXN_AUDIT","OP_TYPE"};
            if (!GridHelper.CheckRequired(gcMain, allowNullColumnList))
            {
                return ResultStatus.FailButNext;
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall)
        {
            try
            {
                ResultStatus myCheckResult = CheckShield();
                if (myCheckResult != ResultStatus.Success) return myCheckResult;
                if (myCheckResult == ResultStatus.Success)
                {
                    base.Save(gcMain);

                    DataTable dt = (DataTable)gcMain.DataSource;
                    gvMain.CloseEditor();
                    gvMain.UpdateCurrentRow();

                    DataTable dtAdd = dt.GetChanges(DataRowState.Added);
                    DataTable dtDelete = dt.GetChanges(DataRowState.Deleted);
                    DataTable dtChange = dt.Clone();
                    var change = dt.GetChanges(DataRowState.Modified).AsEnumerable().Where(row => row.Field<string>("OP_TYPE") == "U");
                    if (change.Any())
                    {
                        dtChange = change.CopyToDataTable<DataRow>();
                    }

                    List<string> txnList = new List<string>();
                    #region 刪除作業代號
                    //刪除代號一併刪除相關權限
                    if (dtDelete != null)
                    {
                        foreach (DataRow row in dtDelete.Rows)
                        {
                            string txnId = row["TXN_ID", DataRowVersion.Original].AsString();
                            string txnType = row["TXN_TYPE", DataRowVersion.Original].AsString();

                            if (txnType == "F")
                            {
                                txnList.Add(txnId);
                                bool result = daoLOGUTP.InsertByUTPAndUPF(txnId, GlobalInfo.USER_DPT_ID, GlobalInfo.USER_ID, GlobalInfo.USER_NAME, "D");
                            }
                        }
                    }
                    #endregion 刪除作業代號

                    #region 變更作業代號
                    //變更代號一併更改作業權限
                    if (dtChange != null)
                    {
                        foreach (DataRow row in dtChange.Rows)
                        {
                            string txnId = row["TXN_ID"].AsString();
                            string txnIdOrg = row["TXN_ID_ORG"].AsString();
                            string txnType = row["TXN_TYPE"].AsString();

                            if (txnId != txnIdOrg && txnType == "F")
                            {
                                txnList.Add(txnIdOrg);
                                bool result = daoLOGUTP.InsertByUTPAndUPF(txnIdOrg, GlobalInfo.USER_DPT_ID, GlobalInfo.USER_ID, GlobalInfo.USER_NAME, "D");
                            }
                        }
                    }

                    #endregion 變更作業代號

                    if (txnList.Count >0)
                    {
                        DataTable dtDeleteUtp = new DZ0020().ListUTPByTxn(txnList);
                        if (dtDeleteUtp.Rows.Count >0)
                        {
                            GridControl gcUtp = new GridControl();
                            gcUtp.DataSource = dtDeleteUtp;
                            gcUtp.MainView = new GridView(gcUtp);
                            DeleteUtpPrint(gcUtp);
                            bool result = daoUTP.DeleteUTPByTxnId(txnList);
                        }
                    }
                    new TXN().UpdateData(dt);

                    AfterSaveForPrint(gcMain, dtAdd, dtDelete, dtChange);
                    _IsPreventFlowPrint = true;
                    _IsPreventFlowExport = true;
                }
            }
            catch (Exception ex)
            {
                MessageDisplay.Error(ex.Message);
                throw;
            }
            
            return ResultStatus.Success;
        }

        protected override ResultStatus Run(PokeBall args)
        {
            base.Run(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Import()
        {
            base.Import(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Export()
        {
            base.Export(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            try
            {
                ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
                

                reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
                _ReportHelper.IsHandlePersonVisible = false;
                _ReportHelper.IsManagerVisible = false;
                _ReportHelper.Create(reportLandscape);
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

            int focusIndex = gvMain.GetFocusedDataSourceRowIndex();
            gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

            //新增一行並做初始值設定
            DataTable dt = (DataTable)gcMain.DataSource;
            DataRow drNew = dt.NewRow();

            drNew["TXN_W_TIME"] = DateTime.Now;
            drNew["TXN_EXTEND"] = "N";
            drNew["TXN_DEFAULT"] = "N";
            drNew["OP_TYPE"] = "I";

            dt.Rows.InsertAt(drNew, focusIndex);
            gcMain.DataSource = dt;
            gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
            gvMain.FocusedColumn = gvMain.Columns[0];
            SetOrder();

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow(gvMain);
            SetOrder();

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }

        protected override ResultStatus COMPLETE()
        {
            MessageDisplay.Info(MessageDisplay.MSG_OK);
            Retrieve();
            return ResultStatus.Success;
        }

        protected void SetOrder() {
            DataTable dtCurrent = (DataTable)gcMain.DataSource;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            int pos = 0;
            foreach (DataRow dr in dtCurrent.Rows)
            {
                pos++;
                int seq = 0;
                if (dr.RowState == DataRowState.Deleted)
                {
                    pos--;
                    continue;
                }

                seq = dr["TXN_SEQ_NO"].AsInt();
                if (seq != pos)
                {
                    gvMain.SetRowCellValue(pos - 1, gvMain.Columns["TXN_SEQ_NO"], pos);
                }

            }
            gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];
        }
        /// <summary>
        /// 將新增、刪除、變更的紀錄分別都列印或匯出出來(橫式A4)
        /// </summary>
        /// <param name="gridControl"></param>
        /// <param name="ChangedForAdded"></param>
        /// <param name="ChangedForDeleted"></param>
        /// <param name="ChangedForModified"></param>
        protected void AfterSaveForPrint(GridControl gridControl, DataTable ChangedForAdded,
            DataTable ChangedForDeleted, DataTable ChangedForModified)
        {
            GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);
            
            string _ReportTitle = _ProgramID + "─" + _ProgramName + GlobalInfo.REPORT_TITLE_MEMO;
            ReportHelper reportHelper = new ReportHelper(gridControl, _ProgramID, _ReportTitle);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4(); //橫向A4
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportHelper.IsHandlePersonVisible = true;
            reportHelper.IsManagerVisible = true;
            reportHelper.Create(reportLandscape);


            if (ChangedForAdded != null)
                if (ChangedForAdded.Rows.Count != 0)
                {
                    gridControlPrint.DataSource = ChangedForAdded;
                    reportHelper.PrintableComponent = gridControlPrint;
                    reportHelper.ReportTitle = _ReportTitle + "─" + "新增";
                    reportHelper.Print();
                    reportHelper.Export(FileType.PDF, reportHelper.FilePath);
                }

            if (ChangedForDeleted != null)
                if (ChangedForDeleted.Rows.Count != 0)
                {
                    DataTable dtTemp = ChangedForDeleted.Clone();

                    int rowIndex = 0;
                    foreach (DataRow dr in ChangedForDeleted.Rows)
                    {
                        DataRow drNewDelete = dtTemp.NewRow();
                        for (int colIndex = 0; colIndex < ChangedForDeleted.Columns.Count; colIndex++)
                        {
                            drNewDelete[colIndex] = dr[colIndex, DataRowVersion.Original];
                        }
                        dtTemp.Rows.Add(drNewDelete);
                        rowIndex++;
                    }

                    gridControlPrint.DataSource = dtTemp.AsDataView();
                    reportHelper.PrintableComponent = gridControlPrint;
                    reportHelper.ReportTitle = _ReportTitle + "─" + "刪除";
                    reportHelper.Print();
                    reportHelper.Export(FileType.PDF, reportHelper.FilePath);
                }

            if (ChangedForModified != null)
                if (ChangedForModified.Rows.Count != 0)
                {
                    gridControlPrint.DataSource = ChangedForModified;
                    reportHelper.PrintableComponent = gridControlPrint;
                    reportHelper.ReportTitle = _ReportTitle + "─" + "變更";
                    reportHelper.Print();
                    reportHelper.Export(FileType.PDF, reportHelper.FilePath);
                }
        }

        /// <summary>
        /// 列印刪除的作業相關權限
        /// </summary>
        protected void DeleteUtpPrint(GridControl gridControl) {
            try
            {
                ((GridView)gridControl.MainView).AppearancePrint.HeaderPanel.Font = new Font("標楷體", 10);
                ((GridView)gridControl.MainView).AppearancePrint.Row.Font = new Font("標楷體", 10);
                ((GridView)gridControl.MainView).OptionsView.AllowCellMerge = true;
                ((GridView)gridControl.MainView).OptionsView.BestFitMode = GridBestFitMode.Full;
                ((GridView)gridControl.MainView).BestFitColumns();
                ReportHelper _ReportHelper = new ReportHelper(gridControl, _ProgramID + "_1", this.Text+"_權限刪除");
                base.Print(_ReportHelper);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private void gvMain_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column != TXN_SEQ_NO && string.IsNullOrEmpty(gvMain.GetRowCellValue(e.RowHandle,OP_TYPE).AsString()))
            {
                gvMain.SetRowCellValue(e.RowHandle, OP_TYPE, "U");
            }
        }
    }
}