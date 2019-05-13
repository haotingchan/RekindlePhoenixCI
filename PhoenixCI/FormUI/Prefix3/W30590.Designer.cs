namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30590 {
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
         this.label7 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.gbContract = new DevExpress.XtraEditors.RadioGroup();
         this.label4 = new System.Windows.Forms.Label();
         this.gbProd = new DevExpress.XtraEditors.RadioGroup();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.labMarket = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.labAftDate = new System.Windows.Forms.Label();
         this.chkGroup = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.lblProcessing = new System.Windows.Forms.Label();
         this.txtStartYMD = new BaseGround.Widget.TextDateEdit();
         this.txtEndYMD = new BaseGround.Widget.TextDateEdit();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbContract.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbProd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.lblProcessing);
         this.panParent.Controls.Add(this.grpxDescription);
         this.panParent.Size = new System.Drawing.Size(836, 544);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(836, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.Controls.Add(this.txtEndYMD);
         this.grpxDescription.Controls.Add(this.txtStartYMD);
         this.grpxDescription.Controls.Add(this.label7);
         this.grpxDescription.Controls.Add(this.label6);
         this.grpxDescription.Controls.Add(this.label5);
         this.grpxDescription.Controls.Add(this.gbMarket);
         this.grpxDescription.Controls.Add(this.gbContract);
         this.grpxDescription.Controls.Add(this.label4);
         this.grpxDescription.Controls.Add(this.gbProd);
         this.grpxDescription.Controls.Add(this.label2);
         this.grpxDescription.Controls.Add(this.label3);
         this.grpxDescription.Controls.Add(this.labMarket);
         this.grpxDescription.Controls.Add(this.label1);
         this.grpxDescription.Controls.Add(this.labAftDate);
         this.grpxDescription.Controls.Add(this.chkGroup);
         this.grpxDescription.Location = new System.Drawing.Point(15, 15);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(792, 399);
         this.grpxDescription.TabIndex = 7;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.ForeColor = System.Drawing.Color.MidnightBlue;
         this.label7.Location = new System.Drawing.Point(177, 339);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(99, 20);
         this.label7.TabIndex = 74;
         this.label7.Text = "(只能選期別)";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.ForeColor = System.Drawing.Color.MidnightBlue;
         this.label6.Location = new System.Drawing.Point(136, 308);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(99, 20);
         this.label6.TabIndex = 73;
         this.label6.Text = "(只能選期別)";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.ForeColor = System.Drawing.Color.MidnightBlue;
         this.label5.Location = new System.Drawing.Point(417, 279);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(99, 20);
         this.label5.TabIndex = 72;
         this.label5.Text = "(只能選期別)";
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rbMarketAll";
         this.gbMarket.Location = new System.Drawing.Point(460, 32);
         this.gbMarket.Name = "gbMarket";
         this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
         this.gbMarket.Properties.Columns = 3;
         this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarketAll", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarket0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarket1", "盤後")});
         this.gbMarket.Size = new System.Drawing.Size(248, 35);
         this.gbMarket.TabIndex = 2;
         // 
         // gbContract
         // 
         this.gbContract.EditValue = "rbContractTxw";
         this.gbContract.Location = new System.Drawing.Point(104, 72);
         this.gbContract.Name = "gbContract";
         this.gbContract.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbContract.Properties.Appearance.Options.UseBackColor = true;
         this.gbContract.Properties.Columns = 4;
         this.gbContract.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbContractTxw", "一週到期契約"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbContractTxo", "一般天期契約"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbContractAll", "所有天期契約"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbContractAllTot", "所有天期契約合併")});
         this.gbContract.Size = new System.Drawing.Size(604, 35);
         this.gbContract.TabIndex = 3;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
         this.label4.Location = new System.Drawing.Point(355, 249);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(99, 20);
         this.label4.TabIndex = 71;
         this.label4.Text = "(只能選期別)";
         // 
         // gbProd
         // 
         this.gbProd.EditValue = "rbProdAll";
         this.gbProd.Location = new System.Drawing.Point(104, 112);
         this.gbProd.Name = "gbProd";
         this.gbProd.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbProd.Properties.Appearance.Options.UseBackColor = true;
         this.gbProd.Properties.Columns = 3;
         this.gbProd.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbProdPC", "區分買賣權"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbProdAll", "買賣權合計")});
         this.gbProd.Size = new System.Drawing.Size(307, 35);
         this.gbProd.TabIndex = 4;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(41, 119);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(57, 20);
         this.label2.TabIndex = 65;
         this.label2.Text = "序列：";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(41, 81);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(57, 20);
         this.label3.TabIndex = 63;
         this.label3.Text = "期別：";
         // 
         // labMarket
         // 
         this.labMarket.AutoSize = true;
         this.labMarket.Location = new System.Drawing.Point(365, 38);
         this.labMarket.Name = "labMarket";
         this.labMarket.Size = new System.Drawing.Size(89, 20);
         this.labMarket.TabIndex = 14;
         this.labMarket.Text = "交易時段：";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(210, 36);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(25, 20);
         this.label1.TabIndex = 6;
         this.label1.Text = "～";
         // 
         // labAftDate
         // 
         this.labAftDate.AutoSize = true;
         this.labAftDate.Location = new System.Drawing.Point(41, 39);
         this.labAftDate.Name = "labAftDate";
         this.labAftDate.Size = new System.Drawing.Size(57, 20);
         this.labAftDate.TabIndex = 2;
         this.labAftDate.Text = "日期：";
         // 
         // chkGroup
         // 
         this.chkGroup.AppearanceSelected.BackColor = System.Drawing.Color.White;
         this.chkGroup.AppearanceSelected.BackColor2 = System.Drawing.Color.White;
         this.chkGroup.AppearanceSelected.Options.UseBackColor = true;
         this.chkGroup.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.chkGroup.ItemPadding = new System.Windows.Forms.Padding(5);
         this.chkGroup.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkMonQnty", "交易量"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkOI", "未平昌量"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkMonCnt", "成交筆數"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkAmt", "成交金額(權利金 * 契約乘數 * 成交口數)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkAmtStk", "名目契約價值(現貨收盤價 * 契約乘數 * 成交口數)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkAcc", "交易戶數"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkId", "交易人數(ID數)")});
         this.chkGroup.Location = new System.Drawing.Point(45, 153);
         this.chkGroup.Name = "chkGroup";
         this.chkGroup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.chkGroup.Size = new System.Drawing.Size(713, 218);
         this.chkGroup.TabIndex = 5;
         this.chkGroup.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.chkGroup_ItemCheck);
         // 
         // lblProcessing
         // 
         this.lblProcessing.AutoSize = true;
         this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
         this.lblProcessing.Location = new System.Drawing.Point(15, 420);
         this.lblProcessing.Name = "lblProcessing";
         this.lblProcessing.Size = new System.Drawing.Size(85, 20);
         this.lblProcessing.TabIndex = 10;
         this.lblProcessing.Text = "開始轉檔...";
         this.lblProcessing.Visible = false;
         // 
         // txtStartYMD
         // 
         this.txtStartYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartYMD.EditValue = "2018/12";
         this.txtStartYMD.EnterMoveNextControl = true;
         this.txtStartYMD.Location = new System.Drawing.Point(104, 35);
         this.txtStartYMD.MenuManager = this.ribbonControl;
         this.txtStartYMD.Name = "txtStartYMD";
         this.txtStartYMD.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartYMD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartYMD.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStartYMD.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartYMD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartYMD.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartYMD.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartYMD.Size = new System.Drawing.Size(100, 26);
         this.txtStartYMD.TabIndex = 11;
         this.txtStartYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtEndYMD
         // 
         this.txtEndYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndYMD.EditValue = "2018/12";
         this.txtEndYMD.EnterMoveNextControl = true;
         this.txtEndYMD.Location = new System.Drawing.Point(241, 35);
         this.txtEndYMD.MenuManager = this.ribbonControl;
         this.txtEndYMD.Name = "txtEndYMD";
         this.txtEndYMD.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndYMD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndYMD.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndYMD.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndYMD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndYMD.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndYMD.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndYMD.Size = new System.Drawing.Size(100, 26);
         this.txtEndYMD.TabIndex = 75;
         this.txtEndYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // W30590
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Name = "W30590";
         this.Text = "W30590";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbContract.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbProd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox grpxDescription;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label labAftDate;
      private System.Windows.Forms.Label lblProcessing;
      private System.Windows.Forms.Label labMarket;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      protected DevExpress.XtraEditors.RadioGroup gbProd;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label4;
      protected DevExpress.XtraEditors.RadioGroup gbContract;
      private DevExpress.XtraEditors.CheckedListBoxControl chkGroup;
      private BaseGround.Widget.TextDateEdit txtEndYMD;
      private BaseGround.Widget.TextDateEdit txtStartYMD;
   }
}