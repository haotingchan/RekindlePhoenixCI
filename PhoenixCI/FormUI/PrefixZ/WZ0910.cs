using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ0910 : FormParent
    {
        private RepositoryItemCheckEdit _RepCheck;
      private UTP daoUTP;
      private LOGUTP daoLOGUTP;
      private DZ0110 daoDZ0110;

      public WZ0910(string programID, string programName) : base(programID, programName)
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
            TXN_DEFAULT.ColumnEdit = _RepCheck;

         daoUTP = new UTP();
         daoLOGUTP = new LOGUTP();
         daoDZ0110 = new DZ0110();
      }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            lblUserId.Text = string.Format("{0}     {1}", GlobalInfo.USER_ID, GlobalInfo.USER_NAME);
            gcMain.DataSource = daoDZ0110.ListUTPByUser(GlobalInfo.USER_ID);

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

            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);

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

        private void btnPrintEmpty_Click(object sender, System.EventArgs e)
        {
            RZ0910_TXN report = new RZ0910_TXN("TXN_DEFAULT", "TXN_ID", "TXN_NAME");
            report.DataSource = gcMain.DataSource;
            string reportTitle = _ProgramID + "─空白作業權限表列印";
            string reportId = GlobalInfo.SYSTEM_ALIAS + _ProgramID; ;
            ReportHelper reportHelper = new ReportHelper(null, reportId, reportTitle);
            reportHelper.Create(report);
            base.Print(reportHelper);
        }
    }
}