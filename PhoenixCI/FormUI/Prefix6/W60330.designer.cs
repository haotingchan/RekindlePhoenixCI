namespace PhoenixCI.FormUI.Prefix6
{
    partial class W60330
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
            this.txtEndMonth = new BaseGround.Widget.TextDateEdit();
            this.txtStartMonth = new BaseGround.Widget.TextDateEdit();
            this.lbl1 = new System.Windows.Forms.Label();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.ExportShow = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.panelControl);
            this.panParent.Size = new System.Drawing.Size(754, 536);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(754, 32);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.ForeColor = System.Drawing.Color.Black;
            this.lblDate.Location = new System.Drawing.Point(37, 46);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(58, 21);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "年月：";
            // 
            // grpxDescription
            // 
            this.grpxDescription.Controls.Add(this.txtEndMonth);
            this.grpxDescription.Controls.Add(this.txtStartMonth);
            this.grpxDescription.Controls.Add(this.lbl1);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(25, 25);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(360, 100);
            this.grpxDescription.TabIndex = 5;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // txtEndMonth
            // 
            this.txtEndMonth.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtEndMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtEndMonth.EditValue = "0001/1/1 上午 12:00:00";
            this.txtEndMonth.Location = new System.Drawing.Point(233, 43);
            this.txtEndMonth.MenuManager = this.ribbonControl;
            this.txtEndMonth.Name = "txtEndMonth";
            this.txtEndMonth.Properties.EditFormat.FormatString = "yyyyMM";
            this.txtEndMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtEndMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndMonth.Properties.Mask.ShowPlaceHolders = false;
            this.txtEndMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndMonth.Size = new System.Drawing.Size(100, 20);
            this.txtEndMonth.TabIndex = 13;
            this.txtEndMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartMonth
            // 
            this.txtStartMonth.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtStartMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtStartMonth.EditValue = "0001/1/1 上午 12:00:00";
            this.txtStartMonth.Location = new System.Drawing.Point(100, 43);
            this.txtStartMonth.MenuManager = this.ribbonControl;
            this.txtStartMonth.Name = "txtStartMonth";
            this.txtStartMonth.Properties.EditFormat.FormatString = "yyyyMM";
            this.txtStartMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtStartMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartMonth.Properties.Mask.ShowPlaceHolders = false;
            this.txtStartMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartMonth.Size = new System.Drawing.Size(100, 20);
            this.txtStartMonth.TabIndex = 12;
            this.txtStartMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(206, 46);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(22, 21);
            this.lbl1.TabIndex = 11;
            this.lbl1.Text = "~";
            // 
            // panelControl
            // 
            this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl.Appearance.Options.UseBackColor = true;
            this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl.Controls.Add(this.ExportShow);
            this.panelControl.Controls.Add(this.grpxDescription);
            this.panelControl.Location = new System.Drawing.Point(30, 30);
            this.panelControl.Margin = new System.Windows.Forms.Padding(15);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(410, 170);
            this.panelControl.TabIndex = 18;
            // 
            // ExportShow
            // 
            this.ExportShow.AutoSize = true;
            this.ExportShow.ForeColor = System.Drawing.Color.Blue;
            this.ExportShow.Location = new System.Drawing.Point(21, 136);
            this.ExportShow.Name = "ExportShow";
            this.ExportShow.Size = new System.Drawing.Size(38, 14);
            this.ExportShow.TabIndex = 16;
            this.ExportShow.Text = "label1";
            // 
            // W60330
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 568);
            this.Name = "W60330";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.GroupBox grpxDescription;
        private BaseGround.Widget.TextDateEdit txtEndMonth;
        private BaseGround.Widget.TextDateEdit txtStartMonth;
        private System.Windows.Forms.Label lbl1;
        private DevExpress.XtraEditors.PanelControl panelControl;
        private System.Windows.Forms.Label ExportShow;
    }
}