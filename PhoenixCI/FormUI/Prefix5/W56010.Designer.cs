namespace PhoenixCI.FormUI.Prefix5 {
    partial class W56010 {
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
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            this.dwProdCond = new DevExpress.XtraEditors.LookUpEdit();
            this.dwEbrkno = new DevExpress.XtraEditors.LookUpEdit();
            this.rdoGroup = new DevExpress.XtraEditors.RadioGroup();
            this.dwSbrkno = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCondition = new System.Windows.Forms.Label();
            this.lblFCM = new System.Windows.Forms.Label();
            this.txtToMonth = new PhoenixCI.Widget.TextDateEdit();
            this.txtFromMonth = new PhoenixCI.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwProdCond.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwEbrkno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwSbrkno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromMonth.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.lblProcessing);
            this.panParent.Controls.Add(this.grpxDescription);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.Controls.Add(this.dwProdCond);
            this.grpxDescription.Controls.Add(this.dwEbrkno);
            this.grpxDescription.Controls.Add(this.rdoGroup);
            this.grpxDescription.Controls.Add(this.dwSbrkno);
            this.grpxDescription.Controls.Add(this.label2);
            this.grpxDescription.Controls.Add(this.lblCondition);
            this.grpxDescription.Controls.Add(this.lblFCM);
            this.grpxDescription.Controls.Add(this.txtToMonth);
            this.grpxDescription.Controls.Add(this.txtFromMonth);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Location = new System.Drawing.Point(82, 65);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(678, 263);
            this.grpxDescription.TabIndex = 9;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // dwProdCond
            // 
            this.dwProdCond.Location = new System.Drawing.Point(135, 193);
            this.dwProdCond.MenuManager = this.ribbonControl;
            this.dwProdCond.Name = "dwProdCond";
            this.dwProdCond.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dwProdCond.Size = new System.Drawing.Size(187, 26);
            this.dwProdCond.TabIndex = 11;
            // 
            // dwEbrkno
            // 
            this.dwEbrkno.Location = new System.Drawing.Point(359, 91);
            this.dwEbrkno.MenuManager = this.ribbonControl;
            this.dwEbrkno.Name = "dwEbrkno";
            this.dwEbrkno.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dwEbrkno.Size = new System.Drawing.Size(182, 26);
            this.dwEbrkno.TabIndex = 10;
            // 
            // rdoGroup
            // 
            this.rdoGroup.EditValue = true;
            this.rdoGroup.Location = new System.Drawing.Point(135, 135);
            this.rdoGroup.MenuManager = this.ribbonControl;
            this.rdoGroup.Name = "rdoGroup";
            this.rdoGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "依期貨商別"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(false, "依商品別")});
            this.rdoGroup.Properties.EditValueChanged += new System.EventHandler(this.rdoGroup_Properties_EditValueChanged);
            this.rdoGroup.Size = new System.Drawing.Size(237, 42);
            this.rdoGroup.TabIndex = 14;
            // 
            // dwSbrkno
            // 
            this.dwSbrkno.Location = new System.Drawing.Point(135, 91);
            this.dwSbrkno.MenuManager = this.ribbonControl;
            this.dwSbrkno.Name = "dwSbrkno";
            this.dwSbrkno.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dwSbrkno.Size = new System.Drawing.Size(187, 26);
            this.dwSbrkno.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(328, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "～";
            // 
            // lblCondition
            // 
            this.lblCondition.AutoSize = true;
            this.lblCondition.Location = new System.Drawing.Point(40, 196);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(89, 20);
            this.lblCondition.TabIndex = 10;
            this.lblCondition.Text = "商品條件：";
            // 
            // lblFCM
            // 
            this.lblFCM.AutoSize = true;
            this.lblFCM.Location = new System.Drawing.Point(56, 94);
            this.lblFCM.Name = "lblFCM";
            this.lblFCM.Size = new System.Drawing.Size(73, 20);
            this.lblFCM.TabIndex = 9;
            this.lblFCM.Text = "期貨商：";
            // 
            // txtToMonth
            // 
            this.txtToMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtToMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtToMonth.EditValue = "2018/12";
            this.txtToMonth.EnterMoveNextControl = true;
            this.txtToMonth.Location = new System.Drawing.Point(272, 43);
            this.txtToMonth.MenuManager = this.ribbonControl;
            this.txtToMonth.Name = "txtToMonth";
            this.txtToMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtToMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtToMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtToMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtToMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtToMonth.Size = new System.Drawing.Size(100, 26);
            this.txtToMonth.TabIndex = 8;
            this.txtToMonth.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtFromMonth
            // 
            this.txtFromMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtFromMonth.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtFromMonth.EditValue = "2018/12";
            this.txtFromMonth.EnterMoveNextControl = true;
            this.txtFromMonth.Location = new System.Drawing.Point(135, 43);
            this.txtFromMonth.MenuManager = this.ribbonControl;
            this.txtFromMonth.Name = "txtFromMonth";
            this.txtFromMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFromMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFromMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtFromMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtFromMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtFromMonth.Size = new System.Drawing.Size(100, 26);
            this.txtFromMonth.TabIndex = 7;
            this.txtFromMonth.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(241, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "～";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(72, 46);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "月份：";
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(78, 331);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 11;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // W56010
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 574);
            this.Name = "W56010";
            this.Text = "W56010";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dwProdCond.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwEbrkno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwSbrkno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtToMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromMonth.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpxDescription;
        private DevExpress.XtraEditors.LookUpEdit dwProdCond;
        private DevExpress.XtraEditors.LookUpEdit dwEbrkno;
        private DevExpress.XtraEditors.RadioGroup rdoGroup;
        private DevExpress.XtraEditors.LookUpEdit dwSbrkno;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCondition;
        private System.Windows.Forms.Label lblFCM;
        private Widget.TextDateEdit txtToMonth;
        private Widget.TextDateEdit txtFromMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblProcessing;
    }
}