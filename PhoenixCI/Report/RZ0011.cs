namespace BaseGround.Report
{
    public partial class RZ0011 : DevExpress.XtraReports.UI.XtraReport
    {
        public bool IsHandlePersonVisible
        {
            set { lblHandlingDescription.Visible = value; }
        }

        public bool IsDoubleConfirmVisible
        {
            set { lblDoubleConfirmDescription.Visible = value; }
        }

        public bool IsManagerVisible
        {
            set { lblManagerDescription.Visible = value; }
        }

        public RZ0011()
        {
            InitializeComponent();
        }

        public void SetMemoTextInFooter(string memo)
        {
            xrTableRowMemo.Visible = true;
            lblMemo.Visible = true;
            lblMemo.Text = memo;
        }
    }
}