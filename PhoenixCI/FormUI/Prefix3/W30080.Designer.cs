namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30080 {
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
            this.lblDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSDate = new BaseGround.Widget.TextDateEdit();
            this.txtEDate = new BaseGround.Widget.TextDateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtKindID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            this.rgpData = new DevExpress.XtraEditors.RadioGroup();
            this.rgpType = new DevExpress.XtraEditors.RadioGroup();
            this.rgpMarket = new DevExpress.XtraEditors.RadioGroup();
            this.txtRank = new DevExpress.XtraEditors.TextEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).BeginInit();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgpData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgpType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgpMarket.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRank.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.lblProcessing);
            this.panParent.Controls.Add(this.grpxDescription);
            this.panParent.Size = new System.Drawing.Size(974, 700);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(974, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(63, 385);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(85, 20);
            this.lblProcessing.TabIndex = 30;
            this.lblProcessing.Text = "開始轉檔...";
            this.lblProcessing.Visible = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.lblDate.Location = new System.Drawing.Point(36, 52);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(58, 21);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "日期：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label1.Location = new System.Drawing.Point(215, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "～";
            // 
            // txtSDate
            // 
            this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtSDate.EditValue = "2018/12/01";
            this.txtSDate.EnterMoveNextControl = true;
            this.txtSDate.Location = new System.Drawing.Point(100, 49);
            this.txtSDate.MenuManager = this.ribbonControl;
            this.txtSDate.Name = "txtSDate";
            this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtSDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtSDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSDate.Size = new System.Drawing.Size(109, 26);
            this.txtSDate.TabIndex = 1;
            this.txtSDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtEDate
            // 
            this.txtEDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEDate.EditValue = "2018/12/01";
            this.txtEDate.EnterMoveNextControl = true;
            this.txtEDate.Location = new System.Drawing.Point(246, 49);
            this.txtEDate.MenuManager = this.ribbonControl;
            this.txtEDate.Name = "txtEDate";
            this.txtEDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtEDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtEDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtEDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEDate.Size = new System.Drawing.Size(109, 26);
            this.txtEDate.TabIndex = 2;
            this.txtEDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label3.Location = new System.Drawing.Point(36, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 21);
            this.label3.TabIndex = 15;
            this.label3.Text = "市場：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label2.Location = new System.Drawing.Point(36, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 21);
            this.label2.TabIndex = 17;
            this.label2.Text = "類別：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label5.Location = new System.Drawing.Point(36, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 21);
            this.label5.TabIndex = 21;
            this.label5.Text = "資料：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label6.Location = new System.Drawing.Point(36, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 21);
            this.label6.TabIndex = 22;
            this.label6.Text = "統計前";
            // 
            // txtKindID
            // 
            this.txtKindID.Location = new System.Drawing.Point(148, 262);
            this.txtKindID.MaxLength = 3;
            this.txtKindID.Name = "txtKindID";
            this.txtKindID.Size = new System.Drawing.Size(61, 29);
            this.txtKindID.TabIndex = 6;
            this.txtKindID.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label7.Location = new System.Drawing.Point(156, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 20);
            this.label7.TabIndex = 25;
            this.label7.Text = "大(999表查全部)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label8.Location = new System.Drawing.Point(215, 265);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 20);
            this.label8.TabIndex = 26;
            this.label8.Text = "(%表查全部)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.label9.Location = new System.Drawing.Point(36, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 21);
            this.label9.TabIndex = 27;
            this.label9.Text = "股票商品代號";
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.grpxDescription.Controls.Add(this.txtRank);
            this.grpxDescription.Controls.Add(this.rgpData);
            this.grpxDescription.Controls.Add(this.rgpType);
            this.grpxDescription.Controls.Add(this.rgpMarket);
            this.grpxDescription.Controls.Add(this.label9);
            this.grpxDescription.Controls.Add(this.label8);
            this.grpxDescription.Controls.Add(this.label7);
            this.grpxDescription.Controls.Add(this.txtKindID);
            this.grpxDescription.Controls.Add(this.label6);
            this.grpxDescription.Controls.Add(this.label5);
            this.grpxDescription.Controls.Add(this.label2);
            this.grpxDescription.Controls.Add(this.label3);
            this.grpxDescription.Controls.Add(this.txtEDate);
            this.grpxDescription.Controls.Add(this.txtSDate);
            this.grpxDescription.Controls.Add(this.label1);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grpxDescription.ForeColor = System.Drawing.Color.Navy;
            this.grpxDescription.Location = new System.Drawing.Point(67, 60);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(430, 322);
            this.grpxDescription.TabIndex = 29;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // rgpData
            // 
            this.rgpData.EditValue = "M_QNTY";
            this.rgpData.Location = new System.Drawing.Point(100, 178);
            this.rgpData.MenuManager = this.ribbonControl;
            this.rgpData.Name = "rgpData";
            this.rgpData.Properties.Columns = 2;
            this.rgpData.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("M_QNTY", "交易量"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("OI", "未平倉量")});
            this.rgpData.Size = new System.Drawing.Size(182, 32);
            this.rgpData.TabIndex = 30;
            // 
            // rgpType
            // 
            this.rgpType.EditValue = "F";
            this.rgpType.Location = new System.Drawing.Point(100, 135);
            this.rgpType.MenuManager = this.ribbonControl;
            this.rgpType.Name = "rgpType";
            this.rgpType.Properties.Columns = 2;
            this.rgpType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("F", "期貨"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("O", "選擇權")});
            this.rgpType.Size = new System.Drawing.Size(182, 31);
            this.rgpType.TabIndex = 29;
            // 
            // rgpMarket
            // 
            this.rgpMarket.EditValue = "%";
            this.rgpMarket.Location = new System.Drawing.Point(100, 88);
            this.rgpMarket.MenuManager = this.ribbonControl;
            this.rgpMarket.Name = "rgpMarket";
            this.rgpMarket.Properties.Columns = 4;
            this.rgpMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("%", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("_TWSE", "TWSE"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("_OTC", "OTC"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("_ETF", "ETF")});
            this.rgpMarket.Size = new System.Drawing.Size(255, 36);
            this.rgpMarket.TabIndex = 28;
            // 
            // txtRank
            // 
            this.txtRank.EditValue = "999";
            this.txtRank.Location = new System.Drawing.Point(100, 223);
            this.txtRank.MenuManager = this.ribbonControl;
            this.txtRank.Name = "txtRank";
            this.txtRank.Properties.Mask.EditMask = "[0-9]?[0-9]?[0-9]?";
            this.txtRank.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtRank.Size = new System.Drawing.Size(50, 26);
            this.txtRank.TabIndex = 31;
            // 
            // W30080
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 730);
            this.Name = "W30080";
            this.Text = "W30080";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).EndInit();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgpData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgpType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgpMarket.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRank.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label lblProcessing;
      private System.Windows.Forms.GroupBox grpxDescription;
      private DevExpress.XtraEditors.RadioGroup rgpMarket;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox txtKindID;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private BaseGround.Widget.TextDateEdit txtEDate;
      private BaseGround.Widget.TextDateEdit txtSDate;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label lblDate;
      private DevExpress.XtraEditors.RadioGroup rgpData;
      private DevExpress.XtraEditors.RadioGroup rgpType;
        private DevExpress.XtraEditors.TextEdit txtRank;
    }
}