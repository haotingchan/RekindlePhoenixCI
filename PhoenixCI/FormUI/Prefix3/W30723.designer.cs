namespace PhoenixCI.FormUI.Prefix3
{
    partial class W30723
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
            this.txtDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.ExportShow = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Controls.Add(this.ExportShow);
            this.panParent.Size = new System.Drawing.Size(697, 489);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(697, 32);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 32);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(697, 489);
            this.panelControl1.TabIndex = 0;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.Controls.Add(this.txtDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Location = new System.Drawing.Point(21, 21);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(404, 120);
            this.grpxDescription.TabIndex = 13;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtDate.EditValue = "2018/12";
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(128, 42);
            this.txtDate.MenuManager = this.ribbonControl;
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtDate.Properties.Mask.EditMask = "yyyy/MM";
            this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDate.Size = new System.Drawing.Size(144, 26);
            this.txtDate.TabIndex = 15;
            this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
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
            // W30723
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 521);
            this.Controls.Add(this.panelControl1);
            this.Name = "W30723";
            this.Text = "30723";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label ExportShow;
        private BaseGround.Widget.TextDateEdit txtDate;
    }
}