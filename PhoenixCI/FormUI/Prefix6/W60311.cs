using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraGrid.Columns;
using System;
using System.Data;
using System.Drawing;

namespace PhoenixCI.FormUI.Prefix6
{
    public partial class W60311 : FormParent
    {
        private string _RptfTxnId = "60310";
        private string _RptfTxdId = "60310";

      private D60310 dao60310;
      private RPTF daoRPTF;

        public W60311(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain, true,new GridColumn[] { RPTF_KEY});
            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;

            txtYear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");

            dao60310 = new D60310();
            daoRPTF = new RPTF();
        }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            DataTable dt = daoRPTF.ListData(_RptfTxnId, _RptfTxdId, txtYear.Text);
            gcMain.DataSource = dt;

            GridHelper.AddOpType(gcMain, new GridColumn[] { RPTF_KEY });

            if (gvMain.RowCount == 0)
            {
                InsertRow();
            }

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

            DataTable dt = daoRPTF.ListData(_RptfTxnId, _RptfTxdId, txtYear.Text);
            gcMain.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MessageDisplay.Info(GlobalInfo.MsgNoData);
            }

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
                    row["RPTF_TXN_ID"] = _RptfTxnId;
                    row["RPTF_TXD_ID"] = _RptfTxdId;
                }
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

                    ResultData result = new RPTF().UpdateData(dt.GetChanges());

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
            base.Print(reportHelper);

            return ResultStatus.Success;
        }

        public override void ProcessPrintAll(ReportHelper reportHelper)
        {
            CommonReportPortraitA4 report = new CommonReportPortraitA4();
            report.IsHandlePersonVisible = false;
            report.IsManagerVisible = false;
            reportHelper.Create(report);

            base.Print(reportHelper);
            MessageDisplay.Info(MessageDisplay.MSG_PRINT);
        }

        protected override ResultStatus InsertRow()
        {
            base.InsertRow(gvMain);
            gvMain.GetFocusedDataRow()["OP_TYPE"] = "I";
            gvMain.GetFocusedDataRow()["RPTF_KEY"] = txtYear.Text;
            gvMain.Focus();

            gvMain.FocusedColumn = gvMain.Columns["RPTF_SEQ_NO"];
            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus COMPLETE()
        {
            MessageDisplay.Info(MessageDisplay.MSG_OK);

            Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }
    }
}