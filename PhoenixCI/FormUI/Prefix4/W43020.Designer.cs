﻿namespace PhoenixCI.FormUI.Prefix4 {
    partial class W43020 {
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
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            this.dwOswGrp = new DevExpress.XtraEditors.LookUpEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwOswGrp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.panelControl);
            this.panParent.Size = new System.Drawing.Size(900, 600);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(900, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
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
            this.panelControl.Size = new System.Drawing.Size(423, 233);
            this.panelControl.TabIndex = 20;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(11, 205);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 20;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.grpxDescription.Controls.Add(this.dwOswGrp);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.txtSDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(15, 15);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(392, 187);
            this.grpxDescription.TabIndex = 19;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // dwOswGrp
            // 
            this.dwOswGrp.Location = new System.Drawing.Point(163, 110);
            this.dwOswGrp.MenuManager = this.ribbonControl;
            this.dwOswGrp.Name = "dwOswGrp";
            this.dwOswGrp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dwOswGrp.Size = new System.Drawing.Size(161, 26);
            this.dwOswGrp.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label1.Location = new System.Drawing.Point(36, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 21);
            this.label1.TabIndex = 8;
            this.label1.Text = "商品交易時段：";
            // 
            // txtSDate
            // 
            this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtSDate.EditValue = "2018/12/01";
            this.txtSDate.EnterMoveNextControl = true;
            this.txtSDate.Location = new System.Drawing.Point(163, 50);
            this.txtSDate.MenuManager = this.ribbonControl;
            this.txtSDate.Name = "txtSDate";
            this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtSDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtSDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSDate.Size = new System.Drawing.Size(100, 26);
            this.txtSDate.TabIndex = 7;
            this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.lblDate.Location = new System.Drawing.Point(100, 53);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(58, 21);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "日期：";
            // 
            // W43020
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 630);
            this.Name = "W43020";
            this.Text = "W43020";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwOswGrp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl;
        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private DevExpress.XtraEditors.LookUpEdit dwOswGrp;
        private System.Windows.Forms.Label label1;
        private BaseGround.Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label lblDate;
    }
}