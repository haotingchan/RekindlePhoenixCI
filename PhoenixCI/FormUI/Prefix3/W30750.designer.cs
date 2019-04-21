namespace PhoenixCI.FormUI.Prefix3
{
    partial class W30750
    {
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
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            this.txtSDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.ExportShow = new System.Windows.Forms.Label();
            this.txtEDate = new BaseGround.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Controls.Add(this.ExportShow);
            this.panParent.Size = new System.Drawing.Size(697, 491);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(697, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(697, 491);
            this.panelControl1.TabIndex = 0;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.txtEDate);
            this.grpxDescription.Controls.Add(this.txtSDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Location = new System.Drawing.Point(21, 21);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(485, 120);
            this.grpxDescription.TabIndex = 13;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // txtSDate
            // 
            this.txtSDate.DateTimeValue = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtSDate.EditValue = "2018/01";
            this.txtSDate.EnterMoveNextControl = true;
            this.txtSDate.Location = new System.Drawing.Point(100, 43);
            this.txtSDate.MenuManager = this.ribbonControl;
            this.txtSDate.Name = "txtSDate";
            this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSDate.Properties.Mask.EditMask = "yyyy/MM";
            this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSDate.Size = new System.Drawing.Size(144, 26);
            this.txtSDate.TabIndex = 15;
            this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(37, 46);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "月份：";
            // 
            // ExportShow
            // 
            this.ExportShow.AutoSize = true;
            this.ExportShow.Location = new System.Drawing.Point(16, 144);
            this.ExportShow.Name = "ExportShow";
            this.ExportShow.Size = new System.Drawing.Size(54, 20);
            this.ExportShow.TabIndex = 14;
            this.ExportShow.Text = "label1";
            // 
            // txtEDate
            // 
            this.txtEDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEDate.EditValue = "2018/12";
            this.txtEDate.EnterMoveNextControl = true;
            this.txtEDate.Location = new System.Drawing.Point(277, 43);
            this.txtEDate.MenuManager = this.ribbonControl;
            this.txtEDate.Name = "txtEDate";
            this.txtEDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEDate.Properties.Mask.EditMask = "yyyy/MM";
            this.txtEDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEDate.Size = new System.Drawing.Size(144, 26);
            this.txtEDate.TabIndex = 16;
            this.txtEDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(250, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "~";
            // 
            // W30750
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 521);
            this.Controls.Add(this.panelControl1);
            this.Name = "W30750";
            this.Text = "30750";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label ExportShow;
        private BaseGround.Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label label1;
        private BaseGround.Widget.TextDateEdit txtEDate;
    }
}