namespace BaseGround
{
    partial class FormParentExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParentExport));
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.panParent = new System.Windows.Forms.Panel();
            this.em_date = new System.Windows.Forms.TextBox();
            this.em_month = new System.Windows.Forms.TextBox();
            this.st_msg_txt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.panParent.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem});
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.MaxItemId = 1;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl.ShowToolbarCustomizeItem = false;
            this.ribbonControl.Size = new System.Drawing.Size(836, 32);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panParent
            // 
            this.panParent.BackColor = System.Drawing.Color.White;
            this.panParent.Controls.Add(this.st_msg_txt);
            this.panParent.Controls.Add(this.em_month);
            this.panParent.Controls.Add(this.em_date);
            this.panParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panParent.Location = new System.Drawing.Point(0, 32);
            this.panParent.Name = "panParent";
            this.panParent.Padding = new System.Windows.Forms.Padding(12);
            this.panParent.Size = new System.Drawing.Size(836, 542);
            this.panParent.TabIndex = 1;
            // 
            // em_date
            // 
            this.em_date.Location = new System.Drawing.Point(126, 58);
            this.em_date.Name = "em_date";
            this.em_date.Size = new System.Drawing.Size(100, 29);
            this.em_date.TabIndex = 0;
            // 
            // em_month
            // 
            this.em_month.Location = new System.Drawing.Point(126, 120);
            this.em_month.Name = "em_month";
            this.em_month.Size = new System.Drawing.Size(100, 29);
            this.em_month.TabIndex = 1;
            // 
            // st_msg_txt
            // 
            this.st_msg_txt.AutoSize = true;
            this.st_msg_txt.Location = new System.Drawing.Point(131, 178);
            this.st_msg_txt.Name = "st_msg_txt";
            this.st_msg_txt.Size = new System.Drawing.Size(85, 20);
            this.st_msg_txt.TabIndex = 2;
            this.st_msg_txt.Text = "開始轉檔...";
            // 
            // FormParentExport
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(836, 574);
            this.Controls.Add(this.panParent);
            this.Controls.Add(this.ribbonControl);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormParentExport";
            this.Ribbon = this.ribbonControl;
            this.RibbonVisibility = DevExpress.XtraBars.Ribbon.RibbonVisibility.Hidden;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormParentExport";
            this.Activated += new System.EventHandler(this.FormParentExport_Activated);
            this.Load += new System.EventHandler(this.FormParentExport_Load);
            this.SizeChanged += new System.EventHandler(this.FormParentExport_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected System.Windows.Forms.Panel panParent;
        public DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
        private System.Windows.Forms.TextBox em_month;
        private System.Windows.Forms.TextBox em_date;
        private System.Windows.Forms.Label st_msg_txt;
    }
}