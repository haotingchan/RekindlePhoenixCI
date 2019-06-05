﻿namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30223 {
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
            this.lblProcessing = new System.Windows.Forms.Label();
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSDate = new BaseGround.Widget.TextDateEdit();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.panelControl);
            this.panParent.Size = new System.Drawing.Size(924, 579);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(924, 43);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(10, 210);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(127, 30);
            this.lblProcessing.TabIndex = 22;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.grpxDescription.Controls.Add(this.label2);
            this.grpxDescription.Controls.Add(this.txtEDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.txtSDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(15, 15);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(419, 191);
            this.grpxDescription.TabIndex = 21;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label2.Location = new System.Drawing.Point(41, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(276, 31);
            this.label2.TabIndex = 9;
            this.label2.Text = "已完成30222確認之資料";
            // 
            // txtEDate
            // 
            this.txtEDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEDate.EditValue = "2018/12/01";
            this.txtEDate.EnterMoveNextControl = true;
            this.txtEDate.Location = new System.Drawing.Point(274, 107);
            this.txtEDate.MenuManager = this.ribbonControl;
            this.txtEDate.Name = "txtEDate";
            this.txtEDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtEDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtEDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtEDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEDate.Size = new System.Drawing.Size(100, 38);
            this.txtEDate.TabIndex = 8;
            this.txtEDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.lblDate.Location = new System.Drawing.Point(41, 110);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(134, 31);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "計算日期：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label1.Location = new System.Drawing.Point(243, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 31);
            this.label1.TabIndex = 6;
            this.label1.Text = "～";
            // 
            // txtSDate
            // 
            this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtSDate.EditValue = "2018/12/01";
            this.txtSDate.EnterMoveNextControl = true;
            this.txtSDate.Location = new System.Drawing.Point(137, 107);
            this.txtSDate.MenuManager = this.ribbonControl;
            this.txtSDate.Name = "txtSDate";
            this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtSDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtSDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSDate.Size = new System.Drawing.Size(100, 38);
            this.txtSDate.TabIndex = 7;
            this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // panelControl
            // 
            this.panelControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl.Appearance.Options.UseBackColor = true;
            this.panelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl.Controls.Add(this.grpxDescription);
            this.panelControl.Controls.Add(this.lblProcessing);
            this.panelControl.Location = new System.Drawing.Point(30, 30);
            this.panelControl.Margin = new System.Windows.Forms.Padding(15);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(451, 242);
            this.panelControl.TabIndex = 20;
            // 
            // W30223
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 622);
            this.Name = "W30223";
            this.Text = "W30223";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label label2;
        private BaseGround.Widget.TextDateEdit txtEDate;
        private BaseGround.Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraEditors.PanelControl panelControl;
    }
}