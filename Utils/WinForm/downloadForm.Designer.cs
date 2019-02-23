namespace Utils.WinForm
{
    partial class downloadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.strTitle = new System.Windows.Forms.Label();
            this.strFileName = new System.Windows.Forms.Label();
            this.downloadBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // strTitle
            // 
            this.strTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(74)))), ((int)(((byte)(131)))));
            this.strTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.strTitle.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.strTitle.ForeColor = System.Drawing.Color.White;
            this.strTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.strTitle.Location = new System.Drawing.Point(0, 0);
            this.strTitle.Margin = new System.Windows.Forms.Padding(3);
            this.strTitle.Name = "strTitle";
            this.strTitle.Padding = new System.Windows.Forms.Padding(3);
            this.strTitle.Size = new System.Drawing.Size(555, 72);
            this.strTitle.TabIndex = 0;
            this.strTitle.Text = "系統版本檢查中....";
            this.strTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // strFileName
            // 
            this.strFileName.BackColor = System.Drawing.Color.Transparent;
            this.strFileName.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strFileName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.strFileName.Location = new System.Drawing.Point(12, 108);
            this.strFileName.Margin = new System.Windows.Forms.Padding(3);
            this.strFileName.Name = "strFileName";
            this.strFileName.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.strFileName.Size = new System.Drawing.Size(531, 31);
            this.strFileName.TabIndex = 1;
            this.strFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // downloadBar
            // 
            this.downloadBar.Location = new System.Drawing.Point(12, 145);
            this.downloadBar.MarqueeAnimationSpeed = 5;
            this.downloadBar.Name = "downloadBar";
            this.downloadBar.Size = new System.Drawing.Size(531, 23);
            this.downloadBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.downloadBar.TabIndex = 2;
            // 
            // downloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(229)))), ((int)(((byte)(243)))));
            this.ClientSize = new System.Drawing.Size(555, 200);
            this.ControlBox = false;
            this.Controls.Add(this.downloadBar);
            this.Controls.Add(this.strFileName);
            this.Controls.Add(this.strTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "downloadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.downloadForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label strTitle;
        private System.Windows.Forms.Label strFileName;
        private System.Windows.Forms.ProgressBar downloadBar;
    }
}