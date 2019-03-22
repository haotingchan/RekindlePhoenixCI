namespace PhoenixCI.FormUI.Prefix3
{
    partial class W30770
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
            this.exportType = new DevExpress.XtraEditors.LookUpEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEymd = new PhoenixCI.Widget.TextDateEdit();
            this.txtEym = new PhoenixCI.Widget.TextDateEdit();
            this.txtSym = new PhoenixCI.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSymd = new PhoenixCI.Widget.TextDateEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ExportShow = new System.Windows.Forms.Label();
            this.MonthReport = new DevExpress.XtraEditors.CheckEdit();
            this.DateReport = new DevExpress.XtraEditors.CheckEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportType.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEymd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEym.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSym.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthReport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateReport.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Controls.Add(this.ExportShow);
            this.panParent.Size = new System.Drawing.Size(580, 442);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(580, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(580, 442);
            this.panelControl1.TabIndex = 0;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.Controls.Add(this.exportType);
            this.grpxDescription.Controls.Add(this.groupBox3);
            this.grpxDescription.Controls.Add(this.label2);
            this.grpxDescription.Location = new System.Drawing.Point(21, 21);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(495, 279);
            this.grpxDescription.TabIndex = 13;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // exportType
            // 
            this.exportType.Location = new System.Drawing.Point(149, 224);
            this.exportType.Name = "exportType";
            this.exportType.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.exportType.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.exportType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.exportType.Properties.NullText = "";
            this.exportType.Properties.PopupSizeable = false;
            this.exportType.Size = new System.Drawing.Size(144, 26);
            this.exportType.TabIndex = 71;
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.Controls.Add(this.DateReport);
            this.groupBox3.Controls.Add(this.MonthReport);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtEymd);
            this.groupBox3.Controls.Add(this.txtEym);
            this.groupBox3.Controls.Add(this.txtSym);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtSymd);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(28, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(451, 167);
            this.groupBox3.TabIndex = 70;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "本公司上市標的";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 20);
            this.label4.TabIndex = 73;
            this.label4.Text = "~";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(249, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 20);
            this.label3.TabIndex = 72;
            this.label3.Text = "~";
            // 
            // txtEymd
            // 
            this.txtEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEymd.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEymd.EditValue = "2018/12";
            this.txtEymd.EnterMoveNextControl = true;
            this.txtEymd.Location = new System.Drawing.Point(276, 113);
            this.txtEymd.MenuManager = this.ribbonControl;
            this.txtEymd.Name = "txtEymd";
            this.txtEymd.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEymd.Size = new System.Drawing.Size(144, 26);
            this.txtEymd.TabIndex = 21;
            this.txtEymd.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtEym
            // 
            this.txtEym.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEym.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEym.EditValue = "2018/12";
            this.txtEym.EnterMoveNextControl = true;
            this.txtEym.Location = new System.Drawing.Point(276, 62);
            this.txtEym.MenuManager = this.ribbonControl;
            this.txtEym.Name = "txtEym";
            this.txtEym.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEym.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEym.Properties.Mask.EditMask = "yyyy/MM";
            this.txtEym.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEym.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEym.Size = new System.Drawing.Size(144, 26);
            this.txtEym.TabIndex = 20;
            this.txtEym.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtSym
            // 
            this.txtSym.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSym.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtSym.EditValue = "2018/12";
            this.txtSym.EnterMoveNextControl = true;
            this.txtSym.Location = new System.Drawing.Point(95, 62);
            this.txtSym.MenuManager = this.ribbonControl;
            this.txtSym.Name = "txtSym";
            this.txtSym.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSym.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSym.Properties.Mask.EditMask = "yyyy/MM";
            this.txtSym.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtSym.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSym.Size = new System.Drawing.Size(144, 26);
            this.txtSym.TabIndex = 17;
            this.txtSym.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "月份：";
            // 
            // txtSymd
            // 
            this.txtSymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSymd.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtSymd.EditValue = "2018/12";
            this.txtSymd.EnterMoveNextControl = true;
            this.txtSymd.Location = new System.Drawing.Point(95, 113);
            this.txtSymd.MenuManager = this.ribbonControl;
            this.txtSymd.Name = "txtSymd";
            this.txtSymd.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSymd.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtSymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtSymd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSymd.Size = new System.Drawing.Size(144, 26);
            this.txtSymd.TabIndex = 15;
            this.txtSymd.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "日期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 69;
            this.label2.Text = "報表類別 ：";
            // 
            // ExportShow
            // 
            this.ExportShow.AutoSize = true;
            this.ExportShow.Location = new System.Drawing.Point(17, 314);
            this.ExportShow.Name = "ExportShow";
            this.ExportShow.Size = new System.Drawing.Size(54, 20);
            this.ExportShow.TabIndex = 14;
            this.ExportShow.Text = "label1";
            // 
            // MonthReport
            // 
            this.MonthReport.Location = new System.Drawing.Point(6, 65);
            this.MonthReport.MenuManager = this.ribbonControl;
            this.MonthReport.Name = "MonthReport";
            this.MonthReport.Properties.Caption = "";
            this.MonthReport.Properties.ValueChecked = "M";
            this.MonthReport.Size = new System.Drawing.Size(20, 19);
            this.MonthReport.TabIndex = 15;
            // 
            // DateReport
            // 
            this.DateReport.Location = new System.Drawing.Point(6, 116);
            this.DateReport.MenuManager = this.ribbonControl;
            this.DateReport.Name = "DateReport";
            this.DateReport.Properties.Caption = "";
            this.DateReport.Properties.ValueChecked = "D";
            this.DateReport.Size = new System.Drawing.Size(20, 19);
            this.DateReport.TabIndex = 74;
            // 
            // W30770
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 472);
            this.Controls.Add(this.panelControl1);
            this.Name = "W30770";
            this.Text = "30770";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportType.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEymd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEym.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSym.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthReport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateReport.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label ExportShow;
        private DevExpress.XtraEditors.LookUpEdit exportType;
        private System.Windows.Forms.GroupBox groupBox3;
        private Widget.TextDateEdit txtSym;
        private System.Windows.Forms.Label label1;
        private Widget.TextDateEdit txtSymd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private Widget.TextDateEdit txtEymd;
        private Widget.TextDateEdit txtEym;
        private DevExpress.XtraEditors.CheckEdit DateReport;
        private DevExpress.XtraEditors.CheckEdit MonthReport;
    }
}