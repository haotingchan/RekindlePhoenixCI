namespace PhoenixCI.FormUI.Prefix5 {
    partial class W55020 {
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
            this.panFilter = new System.Windows.Forms.GroupBox();
            this.rgpType = new DevExpress.XtraEditors.RadioGroup();
            this.labProdType = new System.Windows.Forms.Label();
            this.labFcmNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEndMonth = new BaseGround.Widget.TextDateEdit();
            this.txtStartMonth = new BaseGround.Widget.TextDateEdit();
            this.lbl1 = new System.Windows.Forms.Label();
            this.labDate = new System.Windows.Forms.Label();
            this.cbxFcmStartNo = new DevExpress.XtraEditors.LookUpEdit();
            this.cbxFcmEndNo = new DevExpress.XtraEditors.LookUpEdit();
            this.cbxProdType = new DevExpress.XtraEditors.LookUpEdit();
            this.labMsg = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.panFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgpType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxFcmStartNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxFcmEndNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxProdType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.labMsg);
            this.panParent.Controls.Add(this.panFilter);
            this.panParent.Size = new System.Drawing.Size(1034, 581);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1034, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panFilter
            // 
            this.panFilter.Controls.Add(this.rgpType);
            this.panFilter.Controls.Add(this.labProdType);
            this.panFilter.Controls.Add(this.labFcmNo);
            this.panFilter.Controls.Add(this.label1);
            this.panFilter.Controls.Add(this.txtEndMonth);
            this.panFilter.Controls.Add(this.txtStartMonth);
            this.panFilter.Controls.Add(this.lbl1);
            this.panFilter.Controls.Add(this.labDate);
            this.panFilter.Controls.Add(this.cbxFcmStartNo);
            this.panFilter.Controls.Add(this.cbxFcmEndNo);
            this.panFilter.Controls.Add(this.cbxProdType);
            this.panFilter.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panFilter.Location = new System.Drawing.Point(36, 33);
            this.panFilter.Name = "panFilter";
            this.panFilter.Size = new System.Drawing.Size(880, 216);
            this.panFilter.TabIndex = 6;
            this.panFilter.TabStop = false;
            this.panFilter.Text = "請輸入交易日期";
            // 
            // rgpType
            // 
            this.rgpType.EditValue = "fcm";
            this.rgpType.Location = new System.Drawing.Point(137, 122);
            this.rgpType.Name = "rgpType";
            this.rgpType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("fcm", "依期貨商別"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("product", "依商品別")});
            this.rgpType.Size = new System.Drawing.Size(283, 40);
            this.rgpType.TabIndex = 4;
            this.rgpType.EditValueChanged += new System.EventHandler(this.rgpType_EditValueChanged);
            // 
            // labProdType
            // 
            this.labProdType.AutoSize = true;
            this.labProdType.Location = new System.Drawing.Point(49, 184);
            this.labProdType.Name = "labProdType";
            this.labProdType.Size = new System.Drawing.Size(82, 15);
            this.labProdType.TabIndex = 17;
            this.labProdType.Text = "商品條件：";
            // 
            // labFcmNo
            // 
            this.labFcmNo.AutoSize = true;
            this.labFcmNo.Location = new System.Drawing.Point(64, 88);
            this.labFcmNo.Name = "labFcmNo";
            this.labFcmNo.Size = new System.Drawing.Size(67, 15);
            this.labFcmNo.TabIndex = 16;
            this.labFcmNo.Text = "期貨商：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(399, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "~";
            // 
            // txtEndMonth
            // 
            this.txtEndMonth.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtEndMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtEndMonth.EditValue = "0001/1/1 上午 12:00:00";
            this.txtEndMonth.Location = new System.Drawing.Point(264, 40);
            this.txtEndMonth.Name = "txtEndMonth";
            this.txtEndMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtEndMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndMonth.Size = new System.Drawing.Size(100, 26);
            this.txtEndMonth.TabIndex = 1;
            this.txtEndMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartMonth
            // 
            this.txtStartMonth.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtStartMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtStartMonth.EditValue = "0001/1/1 上午 12:00:00";
            this.txtStartMonth.Location = new System.Drawing.Point(137, 40);
            this.txtStartMonth.Name = "txtStartMonth";
            this.txtStartMonth.Properties.Mask.EditMask = "yyyy/MM";
            this.txtStartMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartMonth.Size = new System.Drawing.Size(100, 26);
            this.txtStartMonth.TabIndex = 0;
            this.txtStartMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(243, 43);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(15, 15);
            this.lbl1.TabIndex = 11;
            this.lbl1.Text = "~";
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.Location = new System.Drawing.Point(79, 47);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(52, 15);
            this.labDate.TabIndex = 2;
            this.labDate.Text = "月份：";
            // 
            // cbxFcmStartNo
            // 
            this.cbxFcmStartNo.Location = new System.Drawing.Point(137, 81);
            this.cbxFcmStartNo.Name = "cbxFcmStartNo";
            this.cbxFcmStartNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxFcmStartNo.Size = new System.Drawing.Size(250, 26);
            this.cbxFcmStartNo.TabIndex = 2;
            // 
            // cbxFcmEndNo
            // 
            this.cbxFcmEndNo.Location = new System.Drawing.Point(424, 81);
            this.cbxFcmEndNo.Name = "cbxFcmEndNo";
            this.cbxFcmEndNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxFcmEndNo.Size = new System.Drawing.Size(250, 26);
            this.cbxFcmEndNo.TabIndex = 3;
            // 
            // cbxProdType
            // 
            this.cbxProdType.Location = new System.Drawing.Point(137, 177);
            this.cbxProdType.Name = "cbxProdType";
            this.cbxProdType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxProdType.Size = new System.Drawing.Size(121, 26);
            this.cbxProdType.TabIndex = 5;
            // 
            // labMsg
            // 
            this.labMsg.AutoSize = true;
            this.labMsg.ForeColor = System.Drawing.Color.Blue;
            this.labMsg.Location = new System.Drawing.Point(32, 262);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new System.Drawing.Size(169, 20);
            this.labMsg.TabIndex = 18;
            this.labMsg.Text = "訊息：資料轉出中........";
            this.labMsg.Visible = false;
            // 
            // W55020
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 611);
            this.Name = "W55020";
            this.Text = "W55020";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.panFilter.ResumeLayout(false);
            this.panFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgpType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxFcmStartNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxFcmEndNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxProdType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox panFilter;
        private BaseGround.Widget.TextDateEdit txtEndMonth;
        private BaseGround.Widget.TextDateEdit txtStartMonth;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label labDate;
        private DevExpress.XtraEditors.RadioGroup rgpType;
        private System.Windows.Forms.Label labProdType;
        private System.Windows.Forms.Label labFcmNo;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.LookUpEdit cbxFcmStartNo;
        private DevExpress.XtraEditors.LookUpEdit cbxFcmEndNo;
        private DevExpress.XtraEditors.LookUpEdit cbxProdType;
        private System.Windows.Forms.Label labMsg;
    }
}