using ActionService;
using ActionService.Extensions;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DevExpress.XtraEditors.Repository;
using System;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix1
{
    public partial class W10012 : FormParent
    {
        public W10012(string programID, string programName) : base(programID, programName)
        {
	//Hello
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain);
            GridHelper.SetCommonGrid(gvSpLog);

            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;

            RepositoryItemCheckEdit repCheck = new RepositoryItemCheckEdit();
            repCheck.AllowGrayed = false;
            repCheck.ValueChecked = "1";
            repCheck.ValueUnchecked = "0";
            repCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;

            gcMain.RepositoryItems.Add(repCheck);
            gcol_gcMain_TXF_DEFAULT.ColumnEdit = repCheck;
        }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            txtPrevOcfDate.DateTimeValue = GlobalInfo.OCF_PREV_DATE;
            txtOcfDate.DateTimeValue = GlobalInfo.OCF_DATE;

            gcMain.DataSource   = servicePrefix1.ListTxfByTxn(_ProgramID).Trim();
            gcLogsp.DataSource  = servicePrefix1.ListLogsp(txtOcfDate.DateTimeValue, _ProgramID).Trim();

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

            _ToolBtnRun.Enabled = true;

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

            xtraTabControl.SelectedTabPage = xtraTabPageQuery;

            gcLogsp.DataSource = servicePrefix1.ListLogsp(txtOcfDate.DateTimeValue, _ProgramID).Trim();

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall)
        {
            base.Save(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus RunBefore(PokeBall args)
        {
            args.GridControlMain = gcMain;
            args.OcfDate = Convert.ToDateTime(txtOcfDate.Text);

            return base.RunBefore(args);
        }

        protected override ResultStatus Run(PokeBall args)
        {
            this.BeginInvoke(new MethodInvoker(() => {
                args.GridControlMain = gcMain;
                args.GridControlSecond = gcLogsp;
                args.OcfDate = Convert.ToDateTime(txtOcfDate.Text);
            }));

            ResultStatus result = base.RunAsync(args);

            return result;
        }

        protected override ResultStatus RunAfterEveryItem(PokeBall args)
        {
            base.RunAfterEveryItem(args);

            return ResultStatus.Success;
        }

        protected override ResultStatus RunAfter(PokeBall args)
        {
            base.RunAfter(args);

            Retrieve();

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

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvMain.RowCount; i++)
            {
                gvMain.SetRowCellValue(i, "TXF_DEFAULT", "1");
            }
        }

        private void btnUnselectedAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvMain.RowCount; i++)
            {
                gvMain.SetRowCellValue(i, "TXF_DEFAULT", "0");
            }
        }

        private void xtraTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if(xtraTabControl.SelectedTabPage == xtraTabPageMain)
            {
                PrintableComponent = gcMain;
            }
            else if(xtraTabControl.SelectedTabPage == xtraTabPageQuery)
            {
                Retrieve();
                PrintableComponent = gcLogsp;
            }
        }
    }
}