namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30594 {
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
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.labMsg = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.label3 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.gbProd = new DevExpress.XtraEditors.RadioGroup();
         this.label10 = new System.Windows.Forms.Label();
         this.gbMarket = new DevExpress.XtraEditors.RadioGroup();
         this.label11 = new System.Windows.Forms.Label();
         this.label13 = new System.Windows.Forms.Label();
         this.txtEndYMD = new BaseGround.Widget.TextDateEdit();
         this.txtStartYMD = new BaseGround.Widget.TextDateEdit();
         this.label14 = new System.Windows.Forms.Label();
         this.label15 = new System.Windows.Forms.Label();
         this.chkGroup = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbProd.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Size = new System.Drawing.Size(820, 510);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(820, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(770, 450);
         this.r_frame.TabIndex = 82;
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(15, 400);
         this.labMsg.MaximumSize = new System.Drawing.Size(730, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 82;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panFilter
         // 
         this.panFilter.AutoSize = true;
         this.panFilter.Controls.Add(this.label3);
         this.panFilter.Controls.Add(this.label8);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Controls.Add(this.gbProd);
         this.panFilter.Controls.Add(this.label10);
         this.panFilter.Controls.Add(this.gbMarket);
         this.panFilter.Controls.Add(this.label11);
         this.panFilter.Controls.Add(this.label13);
         this.panFilter.Controls.Add(this.txtEndYMD);
         this.panFilter.Controls.Add(this.txtStartYMD);
         this.panFilter.Controls.Add(this.label14);
         this.panFilter.Controls.Add(this.label15);
         this.panFilter.Controls.Add(this.chkGroup);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(730, 375);
         this.panFilter.TabIndex = 6;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F);
         this.label3.ForeColor = System.Drawing.Color.Navy;
         this.label3.Location = new System.Drawing.Point(229, 314);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(115, 20);
         this.label3.TabIndex = 76;
         this.label3.Text = "(不分買賣權別)";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Font = new System.Drawing.Font("微軟正黑體", 12F);
         this.label8.ForeColor = System.Drawing.Color.Navy;
         this.label8.Location = new System.Drawing.Point(188, 284);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(115, 20);
         this.label8.TabIndex = 75;
         this.label8.Text = "(不分買賣權別)";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Font = new System.Drawing.Font("微軟正黑體", 12F);
         this.label9.ForeColor = System.Drawing.Color.Navy;
         this.label9.Location = new System.Drawing.Point(466, 254);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(115, 20);
         this.label9.TabIndex = 74;
         this.label9.Text = "(不分買賣權別)";
         // 
         // gbProd
         // 
         this.gbProd.EditValue = "rbProdAll";
         this.gbProd.Location = new System.Drawing.Point(92, 82);
         this.gbProd.Name = "gbProd";
         this.gbProd.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbProd.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbProd.Properties.Appearance.Options.UseBackColor = true;
         this.gbProd.Properties.Appearance.Options.UseForeColor = true;
         this.gbProd.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.gbProd.Properties.Columns = 3;
         this.gbProd.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbProdPC", "區分買賣權"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbProdAll", "買賣權合計")});
         this.gbProd.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbProd.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbProd.Size = new System.Drawing.Size(307, 35);
         this.gbProd.TabIndex = 3;
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Font = new System.Drawing.Font("微軟正黑體", 12F);
         this.label10.ForeColor = System.Drawing.Color.Navy;
         this.label10.Location = new System.Drawing.Point(404, 224);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(115, 20);
         this.label10.TabIndex = 73;
         this.label10.Text = "(不分買賣權別)";
         // 
         // gbMarket
         // 
         this.gbMarket.EditValue = "rbMarketAll";
         this.gbMarket.Location = new System.Drawing.Point(448, 37);
         this.gbMarket.Name = "gbMarket";
         this.gbMarket.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.gbMarket.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.gbMarket.Properties.Appearance.Options.UseBackColor = true;
         this.gbMarket.Properties.Appearance.Options.UseForeColor = true;
         this.gbMarket.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.gbMarket.Properties.Columns = 3;
         this.gbMarket.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarketAll", "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarket0", "一般"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rbMarket1", "盤後")});
         this.gbMarket.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.gbMarket.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gbMarket.Size = new System.Drawing.Size(248, 35);
         this.gbMarket.TabIndex = 2;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.ForeColor = System.Drawing.Color.Black;
         this.label11.Location = new System.Drawing.Point(37, 90);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(58, 21);
         this.label11.TabIndex = 8;
         this.label11.Text = "序列：";
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.ForeColor = System.Drawing.Color.Black;
         this.label13.Location = new System.Drawing.Point(361, 45);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(90, 21);
         this.label13.TabIndex = 7;
         this.label13.Text = "交易時段：";
         // 
         // txtEndYMD
         // 
         this.txtEndYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndYMD.EditValue = "2018/12/01";
         this.txtEndYMD.EnterMoveNextControl = true;
         this.txtEndYMD.Location = new System.Drawing.Point(220, 42);
         this.txtEndYMD.MenuManager = this.ribbonControl;
         this.txtEndYMD.Name = "txtEndYMD";
         this.txtEndYMD.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndYMD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndYMD.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndYMD.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtEndYMD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEndYMD.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndYMD.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndYMD.Size = new System.Drawing.Size(100, 26);
         this.txtEndYMD.TabIndex = 1;
         this.txtEndYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartYMD
         // 
         this.txtStartYMD.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartYMD.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartYMD.EditValue = "2018/12/01";
         this.txtStartYMD.EnterMoveNextControl = true;
         this.txtStartYMD.Location = new System.Drawing.Point(92, 42);
         this.txtStartYMD.MenuManager = this.ribbonControl;
         this.txtStartYMD.Name = "txtStartYMD";
         this.txtStartYMD.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartYMD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartYMD.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStartYMD.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtStartYMD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtStartYMD.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartYMD.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartYMD.Size = new System.Drawing.Size(100, 26);
         this.txtStartYMD.TabIndex = 0;
         this.txtStartYMD.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(194, 45);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(26, 21);
         this.label14.TabIndex = 6;
         this.label14.Text = "～";
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.ForeColor = System.Drawing.Color.Black;
         this.label15.Location = new System.Drawing.Point(37, 45);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(58, 21);
         this.label15.TabIndex = 2;
         this.label15.Text = "日期：";
         // 
         // chkGroup
         // 
         this.chkGroup.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.chkGroup.Appearance.ForeColor = System.Drawing.Color.Black;
         this.chkGroup.Appearance.Options.UseBackColor = true;
         this.chkGroup.Appearance.Options.UseForeColor = true;
         this.chkGroup.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.chkGroup.ItemPadding = new System.Windows.Forms.Padding(5);
         this.chkGroup.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkMonQnty", "交易量"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkOI", "未平倉量"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkMonCnt", "成交筆數"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkAmt", "成交金額(權利金 * 契約乘數 * 成交口數)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkAmtStk", "名目契約價值(現貨收盤價 * 契約乘數 * 成交口數)"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkAcc", "交易戶數"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkId", "交易人數(ID數)")});
         this.chkGroup.Location = new System.Drawing.Point(96, 129);
         this.chkGroup.LookAndFeel.SkinName = "Office 2013";
         this.chkGroup.LookAndFeel.UseDefaultLookAndFeel = false;
         this.chkGroup.Name = "chkGroup";
         this.chkGroup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.chkGroup.Size = new System.Drawing.Size(515, 218);
         this.chkGroup.TabIndex = 4;
         this.chkGroup.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.chkGroup_ItemCheck);
         this.chkGroup.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.chkGroup_DrawItem);
         // 
         // W30594
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(820, 540);
         this.Name = "W30594";
         this.Text = "W30594";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gbProd.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gbMarket.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYMD.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYMD.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroup)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label9;
      protected DevExpress.XtraEditors.RadioGroup gbProd;
      private System.Windows.Forms.Label label10;
      protected DevExpress.XtraEditors.RadioGroup gbMarket;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label13;
      private BaseGround.Widget.TextDateEdit txtEndYMD;
      private BaseGround.Widget.TextDateEdit txtStartYMD;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.Label label15;
      private DevExpress.XtraEditors.CheckedListBoxControl chkGroup;
   }
}