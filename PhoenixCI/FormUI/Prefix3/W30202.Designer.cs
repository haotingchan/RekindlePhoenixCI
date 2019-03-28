namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30202 {
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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMultiLegal = new System.Windows.Forms.TextBox();
            this.txtMultiNature = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCurEMonth = new PhoenixCI.Widget.TextDateEdit();
            this.txtCurSMonth = new PhoenixCI.Widget.TextDateEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDate = new PhoenixCI.Widget.TextDateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEMonth = new PhoenixCI.Widget.TextDateEdit();
            this.txtSMonth = new PhoenixCI.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.txtPrevEymd = new PhoenixCI.Widget.TextDateEdit();
            this.txtCurEymd = new PhoenixCI.Widget.TextDateEdit();
            this.cbxDB = new System.Windows.Forms.CheckBox();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurEMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurSMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurEymd.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.cbxDB);
            this.panParent.Controls.Add(this.txtCurEymd);
            this.panParent.Controls.Add(this.txtPrevEymd);
            this.panParent.Controls.Add(this.lblProcessing);
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Size = new System.Drawing.Size(909, 650);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(909, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(61, 338);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 28;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.Controls.Add(this.label8);
            this.grpxDescription.Controls.Add(this.label7);
            this.grpxDescription.Controls.Add(this.txtMultiLegal);
            this.grpxDescription.Controls.Add(this.txtMultiNature);
            this.grpxDescription.Controls.Add(this.label6);
            this.grpxDescription.Controls.Add(this.label5);
            this.grpxDescription.Controls.Add(this.txtCurEMonth);
            this.grpxDescription.Controls.Add(this.txtCurSMonth);
            this.grpxDescription.Controls.Add(this.label4);
            this.grpxDescription.Controls.Add(this.label2);
            this.grpxDescription.Controls.Add(this.txtDate);
            this.grpxDescription.Controls.Add(this.label3);
            this.grpxDescription.Controls.Add(this.txtEMonth);
            this.grpxDescription.Controls.Add(this.txtSMonth);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Location = new System.Drawing.Point(65, 62);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(430, 277);
            this.grpxDescription.TabIndex = 27;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(194, 223);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 20);
            this.label8.TabIndex = 26;
            this.label8.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(194, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 20);
            this.label7.TabIndex = 25;
            this.label7.Text = "%";
            // 
            // txtMultiLegal
            // 
            this.txtMultiLegal.Location = new System.Drawing.Point(150, 220);
            this.txtMultiLegal.MaxLength = 3;
            this.txtMultiLegal.Name = "txtMultiLegal";
            this.txtMultiLegal.Size = new System.Drawing.Size(38, 29);
            this.txtMultiLegal.TabIndex = 6;
            this.txtMultiLegal.Text = "10";
            // 
            // txtMultiNature
            // 
            this.txtMultiNature.Location = new System.Drawing.Point(150, 186);
            this.txtMultiNature.MaxLength = 3;
            this.txtMultiNature.Name = "txtMultiNature";
            this.txtMultiNature.Size = new System.Drawing.Size(38, 29);
            this.txtMultiNature.TabIndex = 5;
            this.txtMultiNature.Text = "5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 20);
            this.label6.TabIndex = 22;
            this.label6.Text = "法  人  標準：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 20);
            this.label5.TabIndex = 21;
            this.label5.Text = "自然人標準：";
            // 
            // txtCurEMonth
            // 
            this.txtCurEMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtCurEMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtCurEMonth.EditValue = "2018/12";
            this.txtCurEMonth.EnterMoveNextControl = true;
            this.txtCurEMonth.Location = new System.Drawing.Point(265, 139);
            this.txtCurEMonth.MenuManager = this.ribbonControl;
            this.txtCurEMonth.Name = "txtCurEMonth";
            this.txtCurEMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCurEMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCurEMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtCurEMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtCurEMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCurEMonth.Size = new System.Drawing.Size(78, 26);
            this.txtCurEMonth.TabIndex = 4;
            this.txtCurEMonth.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtCurSMonth
            // 
            this.txtCurSMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtCurSMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtCurSMonth.EditValue = "2018/12";
            this.txtCurSMonth.EnterMoveNextControl = true;
            this.txtCurSMonth.Location = new System.Drawing.Point(150, 139);
            this.txtCurSMonth.MenuManager = this.ribbonControl;
            this.txtCurSMonth.Name = "txtCurSMonth";
            this.txtCurSMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCurSMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCurSMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtCurSMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtCurSMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCurSMonth.Size = new System.Drawing.Size(78, 26);
            this.txtCurSMonth.TabIndex = 3;
            this.txtCurSMonth.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(234, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "～";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(22, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 21);
            this.label2.TabIndex = 17;
            this.label2.Text = "本次檢視月份：";
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtDate.EditValue = "2018/12";
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(150, 49);
            this.txtDate.MenuManager = this.ribbonControl;
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDate.Size = new System.Drawing.Size(100, 26);
            this.txtDate.TabIndex = 0;
            this.txtDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(22, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 21);
            this.label3.TabIndex = 15;
            this.label3.Text = "前次檢視月份：";
            // 
            // txtEMonth
            // 
            this.txtEMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtEMonth.EditValue = "2018/12";
            this.txtEMonth.EnterMoveNextControl = true;
            this.txtEMonth.Location = new System.Drawing.Point(265, 96);
            this.txtEMonth.MenuManager = this.ribbonControl;
            this.txtEMonth.Name = "txtEMonth";
            this.txtEMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtEMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEMonth.Size = new System.Drawing.Size(78, 26);
            this.txtEMonth.TabIndex = 2;
            this.txtEMonth.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtSMonth
            // 
            this.txtSMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtSMonth.EditValue = "2018/12";
            this.txtSMonth.EnterMoveNextControl = true;
            this.txtSMonth.Location = new System.Drawing.Point(150, 96);
            this.txtSMonth.MenuManager = this.ribbonControl;
            this.txtSMonth.Name = "txtSMonth";
            this.txtSMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtSMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtSMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSMonth.Size = new System.Drawing.Size(78, 26);
            this.txtSMonth.TabIndex = 1;
            this.txtSMonth.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(234, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "～";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDate.Location = new System.Drawing.Point(54, 52);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(90, 21);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "計算日期：";
            // 
            // txtPrevEymd
            // 
            this.txtPrevEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtPrevEymd.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtPrevEymd.EditValue = "2018/12";
            this.txtPrevEymd.EnterMoveNextControl = true;
            this.txtPrevEymd.Location = new System.Drawing.Point(501, 158);
            this.txtPrevEymd.MenuManager = this.ribbonControl;
            this.txtPrevEymd.Name = "txtPrevEymd";
            this.txtPrevEymd.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPrevEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtPrevEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtPrevEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtPrevEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPrevEymd.Size = new System.Drawing.Size(100, 26);
            this.txtPrevEymd.TabIndex = 27;
            this.txtPrevEymd.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            this.txtPrevEymd.Visible = false;
            // 
            // txtCurEymd
            // 
            this.txtCurEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtCurEymd.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtCurEymd.EditValue = "2018/12";
            this.txtCurEymd.EnterMoveNextControl = true;
            this.txtCurEymd.Location = new System.Drawing.Point(501, 201);
            this.txtCurEymd.MenuManager = this.ribbonControl;
            this.txtCurEymd.Name = "txtCurEymd";
            this.txtCurEymd.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCurEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCurEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtCurEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtCurEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCurEymd.Size = new System.Drawing.Size(100, 26);
            this.txtCurEymd.TabIndex = 29;
            this.txtCurEymd.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            this.txtCurEymd.Visible = false;
            // 
            // cbxDB
            // 
            this.cbxDB.AutoSize = true;
            this.cbxDB.Checked = true;
            this.cbxDB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxDB.Location = new System.Drawing.Point(501, 113);
            this.cbxDB.Name = "cbxDB";
            this.cbxDB.Size = new System.Drawing.Size(114, 24);
            this.cbxDB.TabIndex = 30;
            this.cbxDB.Text = "選擇寫入DB";
            this.cbxDB.UseVisualStyleBackColor = true;
            this.cbxDB.Visible = false;
            // 
            // W30202
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 680);
            this.Name = "W30202";
            this.Text = "W30202";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurEMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurSMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurEymd.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.TextBox txtMultiLegal;
        private System.Windows.Forms.TextBox txtMultiNature;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private Widget.TextDateEdit txtCurEMonth;
        private Widget.TextDateEdit txtCurSMonth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private Widget.TextDateEdit txtDate;
        private System.Windows.Forms.Label label3;
        private Widget.TextDateEdit txtEMonth;
        private Widget.TextDateEdit txtSMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private Widget.TextDateEdit txtCurEymd;
        private Widget.TextDateEdit txtPrevEymd;
        private System.Windows.Forms.CheckBox cbxDB;
    }
}