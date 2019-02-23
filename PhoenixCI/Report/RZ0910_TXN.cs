namespace BaseGround.Report
{
    public partial class RZ0910_TXN : DevExpress.XtraReports.UI.XtraReport
    {
        public RZ0910_TXN(string defaultCheck, string txnId, string txnName)
        {
            InitializeComponent();
            cellDefault.DataBindings.Add("Text", null, defaultCheck);
            cellTxnId.DataBindings.Add("Text", null, txnId);
            cellTxnName.DataBindings.Add("Text", null, txnName);
        }

        int i = 0;
        private void PageHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            i++;
            if (i != 1)
            {
                lblUser.Visible = false;
                lblOperateDateDescription.Visible = false;
            }
            else {
                lblUser.Visible = true;
                lblOperateDateDescription.Visible = true;
            }
           
        }
    }
}