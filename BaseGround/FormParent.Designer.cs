namespace BaseGround
{
    partial class FormParent
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParent));
         this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.panParent = new System.Windows.Forms.Panel();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
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
         this.panParent.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panParent.Location = new System.Drawing.Point(0, 32);
         this.panParent.Name = "panParent";
         this.panParent.Padding = new System.Windows.Forms.Padding(12);
         this.panParent.Size = new System.Drawing.Size(836, 542);
         this.panParent.TabIndex = 1;
         // 
         // FormParent
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Controls.Add(this.panParent);
         this.Controls.Add(this.ribbonControl);
         this.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.KeyPreview = true;
         this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
         this.Name = "FormParent";
         this.Ribbon = this.ribbonControl;
         this.RibbonVisibility = DevExpress.XtraBars.Ribbon.RibbonVisibility.Hidden;
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "FormParent";
         this.Activated += new System.EventHandler(this.FormParent_Activated);
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormParent_FormClosing);
         this.Load += new System.EventHandler(this.FormParent_Load);
         this.Shown += new System.EventHandler(this.FormParent_Shown);
         this.SizeChanged += new System.EventHandler(this.FormParent_SizeChanged);
         this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormParent_KeyDown);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
        protected System.Windows.Forms.Panel panParent;
        public DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
    }
}