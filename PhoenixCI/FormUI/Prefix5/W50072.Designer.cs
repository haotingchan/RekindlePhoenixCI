﻿namespace PhoenixCI.FormUI.Prefix5 {

    partial class W50072 {
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
            this.wizardPage1 = new DevExpress.XtraWizard.WizardPage();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            this.txtToDate = new BaseGround.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFromDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.panelControl);
            this.panParent.Size = new System.Drawing.Size(836, 544);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(836, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // wizardPage1
            // 
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(366, 0);
            this.wizardPage1.Text = "";
            // 
            // panelControl
            // 
            this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl.Appearance.Options.UseBackColor = true;
            this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl.Controls.Add(this.lblProcessing);
            this.panelControl.Controls.Add(this.grpxDescription);
            this.panelControl.Location = new System.Drawing.Point(30, 30);
            this.panelControl.Margin = new System.Windows.Forms.Padding(15);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(434, 164);
            this.panelControl.TabIndex = 20;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(11, 137);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 16;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.grpxDescription.Controls.Add(this.txtToDate);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.txtFromDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(15, 15);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(404, 119);
            this.grpxDescription.TabIndex = 15;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // txtToDate
            // 
            this.txtToDate.DateTimeValue = new System.DateTime(2018, 12, 13, 0, 0, 0, 0);
            this.txtToDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtToDate.EditValue = "2018/12/13";
            this.txtToDate.Location = new System.Drawing.Point(237, 43);
            this.txtToDate.MenuManager = this.ribbonControl;
            this.txtToDate.Name = "txtToDate";
            this.txtToDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtToDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtToDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtToDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtToDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtToDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtToDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToDate.Size = new System.Drawing.Size(100, 26);
            this.txtToDate.TabIndex = 7;
            this.txtToDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label1.Location = new System.Drawing.Point(206, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "～";
            // 
            // txtFromDate
            // 
            this.txtFromDate.DateTimeValue = new System.DateTime(2018, 12, 12, 0, 0, 0, 0);
            this.txtFromDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtFromDate.EditValue = "2018/12/12";
            this.txtFromDate.EnterMoveNextControl = true;
            this.txtFromDate.Location = new System.Drawing.Point(100, 43);
            this.txtFromDate.MenuManager = this.ribbonControl;
            this.txtFromDate.Name = "txtFromDate";
            this.txtFromDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFromDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFromDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtFromDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtFromDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtFromDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtFromDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromDate.Size = new System.Drawing.Size(100, 26);
            this.txtFromDate.TabIndex = 5;
            this.txtFromDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.lblDate.Location = new System.Drawing.Point(37, 46);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(58, 21);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "日期：";
            // 
            // W50072
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 574);
            this.Name = "W50072";
            this.Text = "W50072";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtToDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraWizard.WizardPage wizardPage1;
        private DevExpress.XtraEditors.PanelControl panelControl;
        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private BaseGround.Widget.TextDateEdit txtToDate;
        private System.Windows.Forms.Label label1;
        private BaseGround.Widget.TextDateEdit txtFromDate;
        private System.Windows.Forms.Label lblDate;
    }
}