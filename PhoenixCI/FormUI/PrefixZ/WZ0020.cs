using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;
using System;
using System.Data;

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

            string[] allowNullColumnList = { "TXN_RMARK", "TXN_AUDIT" };
            if (!GridHelper.CheckRequired(gcMain, allowNullColumnList))
            {
                return ResultStatus.Fail;
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
                    DataTable dtChange = dt.GetChanges(DataRowState.Modified);

                    new TXN().UpdateData(dt);

                    //string tableName = "ci.TXN";
                    //string keysColumnList = "TXN_ID";
                    //string insertColumnList = "TXN_ID, TXN_INS, TXN_DEL, TXN_QUERY, TXN_IMPORT, TXN_EXPORT, TXN_PRINT, TXN_UPDATE, TXN_DEFAULT, TXN_RMARK, TXN_W_TIME, TXN_W_USER_ID, TXN_NAME, TXN_AUDIT";
                    //string updateColumnList = "TXN_ID, TXN_INS, TXN_DEL, TXN_QUERY, TXN_IMPORT, TXN_EXPORT, TXN_PRINT, TXN_UPDATE, TXN_DEFAULT, TXN_RMARK, TXN_W_TIME, TXN_W_USER_ID, TXN_NAME, TXN_AUDIT";

                    //ResultData myResultData = serviceCommon.SaveForChanged(dt, tableName, insertColumnList, updateColumnList, keysColumnList, pokeBall);


                    #region 刪除作業代號
                    //刪除代號一併刪除相關權限
                    if (dtDelete != null)
                    {
                        foreach (DataRow row in dtDelete.Rows)
                        {
                            string txnId = row["TXN_ID", DataRowVersion.Original].AsString();
                            bool result = daoLOGUTP.InsertByUTPAndUPF(txnId, GlobalInfo.USER_DPT_ID, GlobalInfo.USER_ID, GlobalInfo.USER_NAME, "D");
                            result = daoUTP.DeleteUTPByTxnId(txnId);
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

                            if (txnId != txnIdOrg)
                            {
                                bool result = daoLOGUTP.InsertByUTPAndUPF(txnIdOrg, GlobalInfo.USER_DPT_ID, GlobalInfo.USER_ID, GlobalInfo.USER_NAME, "D");
                                result = daoUTP.DeleteUTPByTxnId(txnIdOrg);
                            }
                        }
                    }


                    #endregion 變更作業代號

                    PrintOrExportChangedByKen(gcMain, dtAdd, dtDelete, dtChange,true,true);
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
            CommonReportLandscapeA4 report = new CommonReportLandscapeA4();
            reportHelper.Create(report);

            base.Print(reportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow()
        {
            base.InsertRow(gvMain);

            gvMain.GetFocusedDataRow()["TXN_W_TIME"] = DateTime.Now;
            gvMain.GetFocusedDataRow()["TXN_EXTEND"] = "N";
            gvMain.GetFocusedDataRow()["TXN_DEFAULT"] = "N";
            gvMain.GetFocusedDataRow()["TXN_INS"] = "N";
            gvMain.GetFocusedDataRow()["TXN_DEL"] = "N";
            gvMain.GetFocusedDataRow()["TXN_QUERY"] = "N";
            gvMain.GetFocusedDataRow()["TXN_IMPORT"] = "N";
            gvMain.GetFocusedDataRow()["TXN_EXPORT"] = "N";
            gvMain.GetFocusedDataRow()["TXN_PRINT"] = "N";
            gvMain.GetFocusedDataRow()["TXN_UPDATE"] = "N";
            gvMain.GetFocusedDataRow()["TXN_AUDIT"] = "N";

            gvMain.Focus();

            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow(gvMain);

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
    }
}