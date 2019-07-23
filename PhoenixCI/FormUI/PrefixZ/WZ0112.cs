using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;
using PhoenixCI.Widget;
using System;
using System.Data;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ0112 : FormParent
    {
        private ReportHelper _ReportHelper;
        private RepositoryItemCheckEdit _RepCheck;
        private DZ0112 daoDZ0112;
        private UTP daoUTP;
        private LOGUTP daoLOGUTP;

        public WZ0112(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain);

            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;

            _RepCheck = new RepositoryItemCheckEdit();
            _RepCheck.AllowGrayed = false;
            _RepCheck.ValueChecked = "Y";
            _RepCheck.ValueUnchecked = " ";
            _RepCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;

            gcMain.RepositoryItems.Add(_RepCheck);
            UTP_FLAG.ColumnEdit = _RepCheck;

            daoDZ0112 = new DZ0112();
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
            DropDownList.LookUpItemDptIdAndName(ddlDept);
            

            DataTable dt = (DataTable)ddlDept.Properties.DataSource;
            DataRow row = dt.NewRow();
            row["DPT_ID"] = "%";
            row["DPT_NAME"] = "全部";
            row["DPT_ID_NAME"] = "%：全部";
            dt.Rows.InsertAt(row, 0);
            ddlDept.Properties.DataSource = dt;

            DropDownList.LookUpItemTxnIdAndName(ddlTxnId);
            GridHelper.AddModifyCheckMark(gcMain, _RepCheck, MODIFY_MARK);

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

            _ToolBtnSave.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);

            gcMain.DataSource = daoDZ0112.ListUTPByTxnAndDpt(ddlTxnId.EditValue.AsString(), ddlDept.EditValue.AsString());
            //gvMain.TrimAllCell();
            gcMain.Focus();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }
            if (string.IsNullOrEmpty(ddlTxnId.EditValue.AsString()))
            {
                MessageDisplay.Warning("作業代號不可為空白!");
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

                    DataTable dtChange = dt.GetChanges();

                    foreach (DataRow row in dtChange.Rows)
                    {
                        string flag = row["UTP_FLAG"].AsString();
                        string userId = row["UPF_USER_ID"] == null ? "" : row["UPF_USER_ID"].AsString();
                        string txnId = ddlTxnId.EditValue.AsString();
                        string opType = "";
                        if (flag == "Y")
                        {
                            //INSERT
                            opType = "I";
                            bool result = daoUTP.InsertUTPByTXN(userId, GlobalInfo.USER_ID, txnId);
                            bool logResult = daoLOGUTP.InsertByUTPAndUPF(txnId, GlobalInfo.USER_DPT_ID, GlobalInfo.USER_ID, GlobalInfo.USER_NAME, opType, userId);
                        }
                        else if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(flag))
                        {
                            //DELETE
                            opType = "D";
                            bool logResult = daoLOGUTP.InsertByUTPAndUPF(txnId, GlobalInfo.USER_DPT_ID, GlobalInfo.USER_ID, GlobalInfo.USER_NAME, opType, userId);
                            bool result = daoUTP.DeleteUTPByUserIdAndTxnId(userId, txnId);
                        }
                    }
                    _IsPreventFlowPrint = false;

                    gcMain.DataSource = dt.GetChanges();
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

        protected override ResultStatus Export(ReportHelper reportHelper)
        {
            reportHelper = _ReportHelper;
            base.Export(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            _ReportHelper = reportHelper;
            CommonReportPortraitA4 report = new CommonReportPortraitA4();
            _ReportHelper.IsHandlePersonVisible = true;
            _ReportHelper.IsManagerVisible = true;
            _ReportHelper.Create(report);
            _ReportHelper.AddHeaderBottomInfo("設定作業項目權限：" + ddlTxnId.EditValue.AsString());

            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow()
        {
            base.InsertRow(gvMain);

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

        private void ddlTxnId_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTxnId.EditValue.AsString()))
            {
                Retrieve();
            }
            else
            {
                gcMain.DataSource = null;
            }
        }

        private void ddlDept_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDept.EditValue.AsString()))
            {
                Retrieve();
            }
            else
            {
                gcMain.DataSource = null;
            }
        }
    }
}