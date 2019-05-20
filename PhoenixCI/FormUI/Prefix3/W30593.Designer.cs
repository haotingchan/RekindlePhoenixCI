namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30593 {
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
         this.ddlProd = new DevExpress.XtraEditors.LookUpEdit();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.chkGroup = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtEndYMD = new BaseGround.Widget.TextDateEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.txtStartYMD = new BaseGround.Widget.TextDateEdit();
         this.label8 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.label10 = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlProd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(824, 432);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(824, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // ddlProd
         // 
         this.ddlProd.Location = new System.Drawing.Point(99, 83);
         this.ddlProd.Name = "ddlProd";
         this.ddlProd.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.ddlProd.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.ddlProd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlProd.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name21", "Name21"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name22", "Name22"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name23", "Name23")});
         this.ddlProd.Properties.NullText = "";
         this.ddlProd.Properties.PopupSizeable = false;
         this.ddlProd.Size = new System.Drawing.Size(131, 26);
         this.ddlProd.TabIndex = 31;
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rbMarketAll";
         this.gbMarket.Location = new System.Drawing.Point(446, 36);
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
         // chkGroup
         // 
         this.chkGroup.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.chkGroup.Appearance.Options.UseBackColor = true;
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
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkAcc", "交易戶數"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkId", "交易人數")});
         this.chkGroup.Location = new System.Drawing.Point(40, 122);
         this.chkGroup.Name = "chkGroup";
         this.chkGroup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.chkGroup.Size = new System.Drawing.Size(396, 181);
         this.chkGroup.TabIndex = 5;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(21, 360);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Location = new System.Drawing.Point(15, 15);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(780, 388);
         this.r_frame.TabIndex = 80;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.txtEndYMD);
         this.panFilter.Controls.Add(this.label2);
         this.panFilter.Controls.Add(this.ddlProd);
         this.panFilter.Controls.Add(this.txtStartYMD);
         this.panFilter.Controls.Add(this.label8);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Controls.Add(this.gbMarket);
         this.panFilter.Controls.Add(this.label10);
         this.panFilter.Controls.Add(this.chkGroup);
         this.panFilter.Location = new System.Drawing.Point(25, 20);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(730, 333);
         this.panFilter.TabIndex = 80;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndYMD
         // 
         this.txtEndYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndYMD.EditValue = "2018/12";
         this.txtEndYMD.EnterMoveNextControl = true;
         this.txtEndYMD.Location = new System.Drawing.Point(236, 40);
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
         this.txtEndYMD.TabIndex = 76;
         this.txtEndYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(36, 44);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(57, 20);
         this.label2.TabIndex = 2;
         this.label2.Text = "日期：";
         // 
         // txtStartYMD
         // 
         this.txtStartYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartYMD.EditValue = "2018/12";
         this.txtStartYMD.EnterMoveNextControl = true;
         this.txtStartYMD.Location = new System.Drawing.Point(99, 41);
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
         this.txtStartYMD.TabIndex = 75;
         this.txtStartYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(205, 41);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(25, 20);
         this.label8.TabIndex = 6;
         this.label8.Text = "～";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(360, 43);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(89, 20);
         this.label9.TabIndex = 14;
         this.label9.Text = "交易時段：";
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(36, 86);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(57, 20);
         this.label10.TabIndex = 63;
         this.label10.Text = "商品：";
         // 
         // W30593
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(824, 462);
         this.Name = "W30593";
         this.Text = "W30593";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ddlProd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Label labMsg;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      private DevExpress.XtraEditors.CheckedListBoxControl chkGroup;
      private DevExpress.XtraEditors.LookUpEdit ddlProd;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.GroupBox panFilter;
      private BaseGround.Widget.TextDateEdit txtEndYMD;
      private System.Windows.Forms.Label label2;
      private BaseGround.Widget.TextDateEdit txtStartYMD;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label10;
   }
}