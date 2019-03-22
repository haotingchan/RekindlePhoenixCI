namespace PhoenixCI.FormUI.PrefixP {
    partial class WP0050 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDpt = new System.Windows.Forms.Label();
            this.txtStartDate = new DevExpress.XtraEditors.TextEdit();
            this.txtEndDate = new DevExpress.XtraEditors.TextEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Size = new System.Drawing.Size(774, 469);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(774, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.lblDpt);
            this.panelControl1.Controls.Add(this.txtStartDate);
            this.panelControl1.Controls.Add(this.txtEndDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(774, 52);
            this.panelControl1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 20);
            this.label4.TabIndex = 69;
            this.label4.Text = "~";
            // 
            // lblDpt
            // 
            this.lblDpt.AutoSize = true;
            this.lblDpt.Location = new System.Drawing.Point(42, 17);
            this.lblDpt.Name = "lblDpt";
            this.lblDpt.Size = new System.Drawing.Size(73, 20);
            this.lblDpt.TabIndex = 54;
            this.lblDpt.Text = "交易日期";
            // 
            // txtStartDate
            // 
            this.txtStartDate.EditValue = "";
            this.txtStartDate.Location = new System.Drawing.Point(121, 14);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Properties.MaxLength = 8;
            this.txtStartDate.Size = new System.Drawing.Size(122, 26);
            this.txtStartDate.TabIndex = 55;
            // 
            // txtEndDate
            // 
            this.txtEndDate.EditValue = "";
            this.txtEndDate.Location = new System.Drawing.Point(284, 14);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Properties.MaxLength = 8;
            this.txtEndDate.Size = new System.Drawing.Size(122, 26);
            this.txtEndDate.TabIndex = 68;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcMain);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 82);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(774, 417);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(2, 2);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(770, 413);
            this.gcMain.TabIndex = 49;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            this.gcMain.Visible = false;
            // 
            // gvMain
            // 
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            // 
            // WP0050
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 499);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "WP0050";
            this.Text = "WP0020";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.Label lblDpt;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtStartDate;
        private DevExpress.XtraEditors.TextEdit txtEndDate;
    }
}