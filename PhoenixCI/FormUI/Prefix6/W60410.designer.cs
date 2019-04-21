namespace PhoenixCI.FormUI.Prefix6
{
    partial class W60410
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
            this.lblDate = new System.Windows.Forms.Label();
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            this.txtDate = new BaseGround.Widget.TextDateEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Size = new System.Drawing.Size(754, 538);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(754, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(37, 46);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "日期：";
            // 
            // grpxDescription
            // 
            this.grpxDescription.Controls.Add(this.txtDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Location = new System.Drawing.Point(15, 15);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(391, 119);
            this.grpxDescription.TabIndex = 5;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtDate.Location = new System.Drawing.Point(100, 43);
            this.txtDate.MenuManager = this.ribbonControl;
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDate.Size = new System.Drawing.Size(100, 26);
            this.txtDate.TabIndex = 12;
            this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // W60410
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 568);
            this.Name = "W60410";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.GroupBox grpxDescription;
        private BaseGround.Widget.TextDateEdit txtDate;
    }
}