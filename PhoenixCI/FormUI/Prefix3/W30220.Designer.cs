namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30220 {
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
         this.txtStkoutDate = new BaseGround.Widget.TextDateEdit();
         this.label3 = new System.Windows.Forms.Label();
         this.txtDate = new BaseGround.Widget.TextDateEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.txtEMonth = new BaseGround.Widget.TextDateEdit();
         this.txtSMonth = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.lblDate = new System.Windows.Forms.Label();
         this.panelControl = new DevExpress.XtraEditors.PanelControl();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtStkoutDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
         this.panelControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.panelControl);
         this.panParent.Size = new System.Drawing.Size(590, 415);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(590, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // lblProcessing
         // 
         this.lblProcessing.AutoSize = true;
         this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
         this.lblProcessing.Location = new System.Drawing.Point(15, 245);
         this.lblProcessing.MaximumSize = new System.Drawing.Size(470, 120);
         this.lblProcessing.Name = "lblProcessing";
         this.lblProcessing.Size = new System.Drawing.Size(85, 20);
         this.lblProcessing.TabIndex = 22;
         this.lblProcessing.Text = "開始轉檔...";
         this.lblProcessing.Visible = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.grpxDescription.Controls.Add(this.txtStkoutDate);
         this.grpxDescription.Controls.Add(this.label3);
         this.grpxDescription.Controls.Add(this.txtDate);
         this.grpxDescription.Controls.Add(this.label2);
         this.grpxDescription.Controls.Add(this.txtEMonth);
         this.grpxDescription.Controls.Add(this.txtSMonth);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
         this.grpxDescription.Location = new System.Drawing.Point(20, 15);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(430, 220);
         this.grpxDescription.TabIndex = 21;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // txtStkoutDate
         // 
         this.txtStkoutDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStkoutDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStkoutDate.EditValue = "2018/12/01";
         this.txtStkoutDate.EnterMoveNextControl = true;
         this.txtStkoutDate.Location = new System.Drawing.Point(160, 147);
         this.txtStkoutDate.MenuManager = this.ribbonControl;
         this.txtStkoutDate.Name = "txtStkoutDate";
         this.txtStkoutDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStkoutDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStkoutDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStkoutDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtStkoutDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtStkoutDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtStkoutDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStkoutDate.Size = new System.Drawing.Size(100, 26);
         this.txtStkoutDate.TabIndex = 4;
         this.txtStkoutDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label3.Location = new System.Drawing.Point(37, 150);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(122, 21);
         this.label3.TabIndex = 11;
         this.label3.Text = "流通在外股票：";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate.EditValue = "2018/12/01";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(160, 57);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(100, 26);
         this.txtDate.TabIndex = 1;
         this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label2.Location = new System.Drawing.Point(37, 60);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(90, 21);
         this.label2.TabIndex = 9;
         this.label2.Text = "計算日期：";
         // 
         // txtEMonth
         // 
         this.txtEMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEMonth.EditValue = "2018/12";
         this.txtEMonth.EnterMoveNextControl = true;
         this.txtEMonth.Location = new System.Drawing.Point(289, 102);
         this.txtEMonth.MenuManager = this.ribbonControl;
         this.txtEMonth.Name = "txtEMonth";
         this.txtEMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEMonth.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtEMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtEMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEMonth.Properties.Mask.ShowPlaceHolders = false;
         this.txtEMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEMonth.Size = new System.Drawing.Size(100, 26);
         this.txtEMonth.TabIndex = 3;
         this.txtEMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtSMonth
         // 
         this.txtSMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtSMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtSMonth.EditValue = "2018/12";
         this.txtSMonth.EnterMoveNextControl = true;
         this.txtSMonth.Location = new System.Drawing.Point(160, 102);
         this.txtSMonth.MenuManager = this.ribbonControl;
         this.txtSMonth.Name = "txtSMonth";
         this.txtSMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtSMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtSMonth.Properties.EditFormat.FormatString = "yyyyMM";
         this.txtSMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
         this.txtSMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtSMonth.Properties.Mask.ShowPlaceHolders = false;
         this.txtSMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtSMonth.Size = new System.Drawing.Size(100, 26);
         this.txtSMonth.TabIndex = 2;
         this.txtSMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label1.Location = new System.Drawing.Point(263, 105);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(26, 21);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.lblDate.Location = new System.Drawing.Point(37, 105);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(122, 21);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "總交易量月份：";
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
         this.panelControl.Size = new System.Drawing.Size(470, 295);
         this.panelControl.TabIndex = 23;
         // 
         // W30220
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(590, 445);
         this.Name = "W30220";
         this.Text = "W30220";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtStkoutDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
         this.panelControl.ResumeLayout(false);
         this.panelControl.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private BaseGround.Widget.TextDateEdit txtStkoutDate;
        private System.Windows.Forms.Label label3;
        private BaseGround.Widget.TextDateEdit txtDate;
        private System.Windows.Forms.Label label2;
        private BaseGround.Widget.TextDateEdit txtEMonth;
        private BaseGround.Widget.TextDateEdit txtSMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraEditors.PanelControl panelControl;
    }
}