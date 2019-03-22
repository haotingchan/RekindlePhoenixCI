namespace PhoenixCI.FormUI.Prefix4 {
    partial class W41020 {
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
            this.txtEDate = new PhoenixCI.Widget.TextDateEdit();
            this.txtSDate = new PhoenixCI.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdgProdType = new DevExpress.XtraEditors.RadioGroup();
            this.label3 = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgProdType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.lblProcessing);
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Size = new System.Drawing.Size(930, 640);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(930, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(45, 274);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 22;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.Controls.Add(this.label3);
            this.grpxDescription.Controls.Add(this.rdgProdType);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.txtProd);
            this.grpxDescription.Controls.Add(this.label2);
            this.grpxDescription.Controls.Add(this.txtEDate);
            this.grpxDescription.Controls.Add(this.txtSDate);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Location = new System.Drawing.Point(49, 56);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(404, 215);
            this.grpxDescription.TabIndex = 21;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "交易量資料轉出Excel 檔";
            // 
            // txtEDate
            // 
            this.txtEDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtEDate.EditValue = "2018/12";
            this.txtEDate.EnterMoveNextControl = true;
            this.txtEDate.Location = new System.Drawing.Point(237, 94);
            this.txtEDate.MenuManager = this.ribbonControl;
            this.txtEDate.Name = "txtEDate";
            this.txtEDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEDate.Size = new System.Drawing.Size(100, 26);
            this.txtEDate.TabIndex = 3;
            this.txtEDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtSDate
            // 
            this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtSDate.EditValue = "2018/12";
            this.txtSDate.EnterMoveNextControl = true;
            this.txtSDate.Location = new System.Drawing.Point(100, 94);
            this.txtSDate.MenuManager = this.ribbonControl;
            this.txtSDate.Name = "txtSDate";
            this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSDate.Size = new System.Drawing.Size(100, 26);
            this.txtSDate.TabIndex = 2;
            this.txtSDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(37, 97);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "日期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "商品：";
            // 
            // txtProd
            // 
            this.txtProd.Location = new System.Drawing.Point(100, 131);
            this.txtProd.Name = "txtProd";
            this.txtProd.Size = new System.Drawing.Size(100, 29);
            this.txtProd.TabIndex = 4;
            this.txtProd.Text = "%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "（%代表查全部）";
            // 
            // rdgProdType
            // 
            this.rdgProdType.EditValue = "F";
            this.rdgProdType.Location = new System.Drawing.Point(100, 39);
            this.rdgProdType.MenuManager = this.ribbonControl;
            this.rdgProdType.Name = "rdgProdType";
            this.rdgProdType.Properties.Columns = 2;
            this.rdgProdType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("F", "期貨"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("O", "選擇權")});
            this.rdgProdType.Size = new System.Drawing.Size(237, 43);
            this.rdgProdType.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "商品別：";
            // 
            // W41020
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 670);
            this.Name = "W41020";
            this.Text = "W41020";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgProdType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProd;
        private System.Windows.Forms.Label label2;
        private Widget.TextDateEdit txtEDate;
        private Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraEditors.RadioGroup rdgProdType;
        private System.Windows.Forms.Label label3;
    }
}