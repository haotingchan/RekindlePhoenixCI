using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace BaseGround.Report
{
    public partial class CommonReportPortraitA4 : DevExpress.XtraReports.UI.XtraReport
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

        public CommonReportPortraitA4()
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
