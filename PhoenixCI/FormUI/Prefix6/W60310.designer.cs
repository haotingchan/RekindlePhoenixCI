namespace PhoenixCI.FormUI.Prefix6
{
    partial class W60310
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
            this.txtEndDate = new BaseGround.Widget.TextDateEdit();
            this.txtStartDate = new BaseGround.Widget.TextDateEdit();
            this.lbl1 = new System.Windows.Forms.Label();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.ExportShow = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
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
            this.lblDate.Text = "日期：";
            // 
            // grpxDescription
            // 
            this.grpxDescription.Controls.Add(this.txtEndDate);
            this.grpxDescription.Controls.Add(this.txtStartDate);
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
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtEndDate.Location = new System.Drawing.Point(233, 43);
            this.txtEndDate.MenuManager = this.ribbonControl;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(100, 20);
            this.txtEndDate.TabIndex = 10;
            this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtStartDate.Location = new System.Drawing.Point(100, 43);
            this.txtStartDate.MenuManager = this.ribbonControl;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(100, 20);
            this.txtStartDate.TabIndex = 9;
            this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(206, 46);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(22, 21);
            this.lbl1.TabIndex = 6;
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
            // W60310
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 568);
            this.Name = "W60310";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label lbl1;
        private BaseGround.Widget.TextDateEdit txtEndDate;
        private BaseGround.Widget.TextDateEdit txtStartDate;
        private DevExpress.XtraEditors.PanelControl panelControl;
        private System.Windows.Forms.Label ExportShow;
    }
}