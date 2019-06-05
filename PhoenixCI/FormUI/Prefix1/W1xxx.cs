using ActionService.Extensions;
using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DevExpress.XtraEditors.Repository;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix1
{
    public partial class W1xxx : FormParent
    {
        public W1xxx() {
            InitializeComponent();
        }
        
        public W1xxx(string programID, string programName) : base(programID, programName)
        {
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

            _IsProcessRunAsync = true;
        }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            if (DesignMode) return ResultStatus.Success;
            base.Open();

            //txtPrevOcfDate.DateTimeValue = GlobalInfo.OCF_PREV_DATE;
            txtOcfDate.DateTimeValue = PbFunc.f_ocf_date(0, _DB_TYPE).AsDateTime(); //GlobalInfo.OCF_DATE;

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

            _ToolBtnInsert.Enabled = false;
            _ToolBtnSave.Enabled = false;
            _ToolBtnDel.Enabled = false;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);

            DataTable dtLogSP = servicePrefix1.ListLogsp(txtOcfDate.DateTimeValue, _ProgramID).Trim();
            gcLogsp.DataSource = dtLogSP;
            if (dtLogSP.Rows.Count == 0)
            {
                MessageDisplay.Info(GlobalInfo.MsgNoData);
                xtraTabControl.SelectedTabPage = xtraTabPageMain;
            }
            else
            {
                xtraTabControl.SelectedTabPage = xtraTabPageQuery;
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);

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