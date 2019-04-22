namespace PhoenixCI.FormUI.Prefix3 {
    partial class W30221 {
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
            this.cbxWriteTxt = new System.Windows.Forms.CheckBox();
            this.txtStkoutYmd = new BaseGround.Widget.TextDateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDate = new BaseGround.Widget.TextDateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEMonth = new BaseGround.Widget.TextDateEdit();
            this.txtSMonth = new BaseGround.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.cbxDB = new System.Windows.Forms.CheckBox();
            this.txtPrevEymd = new BaseGround.Widget.TextDateEdit();
            this.txtCurEymd = new BaseGround.Widget.TextDateEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStkoutYmd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurEymd.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.txtCurEymd);
            this.panParent.Controls.Add(this.txtPrevEymd);
            this.panParent.Controls.Add(this.cbxDB);
            this.panParent.Controls.Add(this.lblProcessing);
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Size = new System.Drawing.Size(941, 637);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(941, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(61, 335);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 26;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.grpxDescription.Controls.Add(this.cbxWriteTxt);
            this.grpxDescription.Controls.Add(this.txtStkoutYmd);
            this.grpxDescription.Controls.Add(this.label2);
            this.grpxDescription.Controls.Add(this.txtDate);
            this.grpxDescription.Controls.Add(this.label3);
            this.grpxDescription.Controls.Add(this.txtEMonth);
            this.grpxDescription.Controls.Add(this.txtSMonth);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(65, 59);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(430, 273);
            this.grpxDescription.TabIndex = 25;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // cbxWriteTxt
            // 
            this.cbxWriteTxt.AutoSize = true;
            this.cbxWriteTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.cbxWriteTxt.Location = new System.Drawing.Point(123, 185);
            this.cbxWriteTxt.Name = "cbxWriteTxt";
            this.cbxWriteTxt.Size = new System.Drawing.Size(221, 25);
            this.cbxWriteTxt.TabIndex = 4;
            this.cbxWriteTxt.Text = "寫入資料庫資料儲存文字檔";
            this.cbxWriteTxt.UseVisualStyleBackColor = true;
            this.cbxWriteTxt.Visible = false;
            // 
            // txtStkoutYmd
            // 
            this.txtStkoutYmd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStkoutYmd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtStkoutYmd.EditValue = "2018/12";
            this.txtStkoutYmd.EnterMoveNextControl = true;
            this.txtStkoutYmd.Location = new System.Drawing.Point(150, 139);
            this.txtStkoutYmd.MenuManager = this.ribbonControl;
            this.txtStkoutYmd.Name = "txtStkoutYmd";
            this.txtStkoutYmd.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStkoutYmd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStkoutYmd.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtStkoutYmd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStkoutYmd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStkoutYmd.Size = new System.Drawing.Size(100, 26);
            this.txtStkoutYmd.TabIndex = 3;
            this.txtStkoutYmd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label2.Location = new System.Drawing.Point(22, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 21);
            this.label2.TabIndex = 17;
            this.label2.Text = "流通在外股票：";
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
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
            this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label3.Location = new System.Drawing.Point(22, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 21);
            this.label3.TabIndex = 15;
            this.label3.Text = "總交易量月份：";
            // 
            // txtEMonth
            // 
            this.txtEMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
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
            this.txtEMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtSMonth
            // 
            this.txtSMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
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
            this.txtSMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(234, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "～";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.lblDate.Location = new System.Drawing.Point(54, 52);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(90, 21);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "計算日期：";
            // 
            // cbxDB
            // 
            this.cbxDB.AutoSize = true;
            this.cbxDB.Checked = true;
            this.cbxDB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxDB.Location = new System.Drawing.Point(540, 111);
            this.cbxDB.Name = "cbxDB";
            this.cbxDB.Size = new System.Drawing.Size(114, 24);
            this.cbxDB.TabIndex = 27;
            this.cbxDB.Text = "選擇寫入DB";
            this.cbxDB.UseVisualStyleBackColor = true;
            this.cbxDB.Visible = false;
            // 
            // txtPrevEymd
            // 
            this.txtPrevEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtPrevEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtPrevEymd.EditValue = "2018/12";
            this.txtPrevEymd.EnterMoveNextControl = true;
            this.txtPrevEymd.Location = new System.Drawing.Point(540, 154);
            this.txtPrevEymd.MenuManager = this.ribbonControl;
            this.txtPrevEymd.Name = "txtPrevEymd";
            this.txtPrevEymd.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPrevEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtPrevEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtPrevEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtPrevEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPrevEymd.Size = new System.Drawing.Size(100, 26);
            this.txtPrevEymd.TabIndex = 20;
            this.txtPrevEymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            this.txtPrevEymd.Visible = false;
            // 
            // txtCurEymd
            // 
            this.txtCurEymd.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtCurEymd.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtCurEymd.EditValue = "2018/12";
            this.txtCurEymd.EnterMoveNextControl = true;
            this.txtCurEymd.Location = new System.Drawing.Point(540, 198);
            this.txtCurEymd.MenuManager = this.ribbonControl;
            this.txtCurEymd.Name = "txtCurEymd";
            this.txtCurEymd.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCurEymd.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCurEymd.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtCurEymd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtCurEymd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCurEymd.Size = new System.Drawing.Size(100, 26);
            this.txtCurEymd.TabIndex = 30;
            this.txtCurEymd.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            this.txtCurEymd.Visible = false;
            // 
            // W30221
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 667);
            this.Name = "W30221";
            this.Text = "W30221";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStkoutYmd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrevEymd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurEymd.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox cbxDB;
        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.CheckBox cbxWriteTxt;
        private BaseGround.Widget.TextDateEdit txtStkoutYmd;
        private System.Windows.Forms.Label label2;
        private BaseGround.Widget.TextDateEdit txtDate;
        private System.Windows.Forms.Label label3;
        private BaseGround.Widget.TextDateEdit txtEMonth;
        private BaseGround.Widget.TextDateEdit txtSMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private BaseGround.Widget.TextDateEdit txtCurEymd;
        private BaseGround.Widget.TextDateEdit txtPrevEymd;
    }
}