namespace PhoenixCI.FormUI.Prefix1
{
    partial class W10012:W1xxx
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
            ((System.ComponentModel.ISupportInitialize)(this.txtOcfDate.Properties)).BeginInit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOcfDate
            // 
            this.txtOcfDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtOcfDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtOcfDate.Properties.EditFormat.FormatString = "yyyy/MM/dd";
            this.txtOcfDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtOcfDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtOcfDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtOcfDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // W10012
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 761);
            this.Name = "W10012";
            this.Text = "W10012";
            ((System.ComponentModel.ISupportInitialize)(this.txtOcfDate.Properties)).EndInit();
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public DevExpress.XtraGrid.Columns.GridColumn UPF_USER_ID;
        public DevExpress.XtraGrid.Columns.GridColumn UPF_EMPLOYEE_ID;
        public DevExpress.XtraGrid.Columns.GridColumn UPF_DPT_ID;
    }
}