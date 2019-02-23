using System;
using System.Windows.Forms;

namespace Utils.WinForm
{
    public partial class downloadForm : Form
    {
        public downloadForm()
        {
            InitializeComponent();
        }

        public downloadForm(int count)
        {
            InitializeComponent();
            downloadBar.Maximum = count;
        }

        private void downloadForm_Load(object sender, EventArgs e)
        {
        }

        public void setDownloadBar(int value)
        {
            downloadBar.Value = value;
        }

        public void downloadFile(string fileName)
        {
            strFileName.Text = fileName;
        }

        public void setTitle(string title)
        {
            strTitle.Text = title;
        }

        public void setDownloadBarStyle(ProgressBarStyle style)
        {
            downloadBar.Style = style;
        }

        public void PerformStep() {
            downloadBar.PerformStep();
        }
    }
}