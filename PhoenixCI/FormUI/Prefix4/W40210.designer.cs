namespace PhoenixCI.FormUI.Prefix4 {
   partial class W40210 {
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
         this.cbxProd = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.txtDays = new DevExpress.XtraEditors.TextEdit();
         this.gcExport = new DevExpress.XtraGrid.GridControl();
         this.gvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.label1 = new System.Windows.Forms.Label();
         this.panKind = new System.Windows.Forms.GroupBox();
         this.txtEndDate2 = new BaseGround.Widget.TextDateEdit();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.label11 = new System.Windows.Forms.Label();
         this.label12 = new System.Windows.Forms.Label();
         this.label13 = new System.Windows.Forms.Label();
         this.rdoReportType = new DevExpress.XtraEditors.RadioGroup();
         this.label8 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.panProd = new System.Windows.Forms.GroupBox();
         this.labMsg = new System.Windows.Forms.Label();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbxProd)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDays.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.panFilter.SuspendLayout();
         this.panKind.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate2.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rdoReportType.Properties)).BeginInit();
         this.panProd.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Controls.Add(this.gcExport);
         this.panParent.Size = new System.Drawing.Size(555, 545);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(555, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // cbxProd
         // 
         this.cbxProd.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.cbxProd.Appearance.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.cbxProd.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
         this.cbxProd.Appearance.Options.UseBackColor = true;
         this.cbxProd.Appearance.Options.UseFont = true;
         this.cbxProd.AppearanceDisabled.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
         this.cbxProd.AppearanceSelected.BackColor = System.Drawing.Color.White;
         this.cbxProd.AppearanceSelected.BackColor2 = System.Drawing.Color.White;
         this.cbxProd.AppearanceSelected.Options.UseBackColor = true;
         this.cbxProd.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.cbxProd.ColumnWidth = 57;
         this.cbxProd.HorzScrollStep = 9;
         this.cbxProd.ItemAutoHeight = true;
         this.cbxProd.ItemPadding = new System.Windows.Forms.Padding(11);
         this.cbxProd.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("1", "第一盤非股票類商品"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("2", "第二盤商品"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("3", "標的證券為股票之股票類商品"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("4", "標的證券為受益憑證之股票類商品")});
         this.cbxProd.Location = new System.Drawing.Point(15, 25);
         this.cbxProd.LookAndFeel.SkinName = "Office 2013";
         this.cbxProd.LookAndFeel.UseDefaultLookAndFeel = false;
         this.cbxProd.Name = "cbxProd";
         this.cbxProd.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.cbxProd.Size = new System.Drawing.Size(375, 171);
         this.cbxProd.TabIndex = 5;
         // 
         // txtDays
         // 
         this.txtDays.Location = new System.Drawing.Point(269, 68);
         this.txtDays.MenuManager = this.ribbonControl;
         this.txtDays.Name = "txtDays";
         this.txtDays.Properties.Mask.EditMask = "n0";
         this.txtDays.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.txtDays.Size = new System.Drawing.Size(66, 26);
         this.txtDays.TabIndex = 4;
         // 
         // gcExport
         // 
         this.gcExport.Location = new System.Drawing.Point(521, 45);
         this.gcExport.MainView = this.gvExport;
         this.gcExport.MenuManager = this.ribbonControl;
         this.gcExport.Name = "gcExport";
         this.gcExport.Size = new System.Drawing.Size(172, 239);
         this.gcExport.TabIndex = 90;
         this.gcExport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExport});
         this.gcExport.Visible = false;
         // 
         // gvExport
         // 
         this.gvExport.GridControl = this.gcExport;
         this.gvExport.Name = "gvExport";
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Controls.Add(this.labMsg);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(505, 485);
         this.r_frame.TabIndex = 91;
         // 
         // panFilter
         // 
         this.panFilter.Controls.Add(this.label1);
         this.panFilter.Controls.Add(this.panKind);
         this.panFilter.Controls.Add(this.label8);
         this.panFilter.Controls.Add(this.label9);
         this.panFilter.Controls.Add(this.panProd);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(465, 410);
         this.panFilter.TabIndex = 76;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label1.ForeColor = System.Drawing.Color.Maroon;
         this.label1.Location = new System.Drawing.Point(21, 377);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(411, 19);
         this.label1.TabIndex = 91;
         this.label1.Text = "RHO與RTO使用同一個VSR(採兩商品分別計算結果之較大值)";
         // 
         // panKind
         // 
         this.panKind.Controls.Add(this.txtEndDate2);
         this.panKind.Controls.Add(this.txtEndDate);
         this.panKind.Controls.Add(this.txtStartDate);
         this.panKind.Controls.Add(this.txtDays);
         this.panKind.Controls.Add(this.label11);
         this.panKind.Controls.Add(this.label12);
         this.panKind.Controls.Add(this.label13);
         this.panKind.Controls.Add(this.rdoReportType);
         this.panKind.Location = new System.Drawing.Point(25, 30);
         this.panKind.Name = "panKind";
         this.panKind.Size = new System.Drawing.Size(415, 115);
         this.panKind.TabIndex = 88;
         this.panKind.TabStop = false;
         this.panKind.Text = "日期";
         // 
         // txtEndDate2
         // 
         this.txtEndDate2.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate2.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate2.EditValue = "2018/12/01";
         this.txtEndDate2.EnterMoveNextControl = true;
         this.txtEndDate2.Location = new System.Drawing.Point(117, 68);
         this.txtEndDate2.MenuManager = this.ribbonControl;
         this.txtEndDate2.Name = "txtEndDate2";
         this.txtEndDate2.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate2.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndDate2.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtEndDate2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEndDate2.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndDate2.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate2.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate2.TabIndex = 85;
         this.txtEndDate2.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate.EditValue = "2018/12/01";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(247, 31);
         this.txtEndDate.MenuManager = this.ribbonControl;
         this.txtEndDate.Name = "txtEndDate";
         this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtEndDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate.TabIndex = 84;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartDate.EditValue = "2018/12/01";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(117, 31);
         this.txtStartDate.MenuManager = this.ribbonControl;
         this.txtStartDate.Name = "txtStartDate";
         this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStartDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtStartDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate.TabIndex = 83;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Font = new System.Drawing.Font("微軟正黑體", 12F);
         this.label11.ForeColor = System.Drawing.Color.Black;
         this.label11.Location = new System.Drawing.Point(342, 71);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(57, 20);
         this.label11.TabIndex = 82;
         this.label11.Text = "天資料";
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(219, 34);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(26, 21);
         this.label12.TabIndex = 6;
         this.label12.Text = "～";
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Font = new System.Drawing.Font("微軟正黑體", 12F);
         this.label13.ForeColor = System.Drawing.Color.Black;
         this.label13.Location = new System.Drawing.Point(223, 71);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(41, 20);
         this.label13.TabIndex = 80;
         this.label13.Text = "，近";
         // 
         // rdoReportType
         // 
         this.rdoReportType.EditValue = "rb_1";
         this.rdoReportType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.rdoReportType.Location = new System.Drawing.Point(12, 20);
         this.rdoReportType.Name = "rdoReportType";
         this.rdoReportType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.rdoReportType.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.rdoReportType.Properties.Appearance.Options.UseBackColor = true;
         this.rdoReportType.Properties.Appearance.Options.UseForeColor = true;
         this.rdoReportType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.rdoReportType.Properties.Columns = 1;
         this.rdoReportType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_1", "日期起訖："),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_2", "迄止日期：")});
         this.rdoReportType.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Light";
         this.rdoReportType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rdoReportType.Size = new System.Drawing.Size(100, 85);
         this.rdoReportType.TabIndex = 77;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label8.ForeColor = System.Drawing.Color.Maroon;
         this.label8.Location = new System.Drawing.Point(21, 358);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(54, 19);
         this.label8.TabIndex = 90;
         this.label8.Text = "備註：";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.ForeColor = System.Drawing.Color.MidnightBlue;
         this.label9.Location = new System.Drawing.Point(137, 313);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(0, 21);
         this.label9.TabIndex = 86;
         // 
         // panProd
         // 
         this.panProd.Controls.Add(this.cbxProd);
         this.panProd.Location = new System.Drawing.Point(25, 151);
         this.panProd.Name = "panProd";
         this.panProd.Size = new System.Drawing.Size(413, 200);
         this.panProd.TabIndex = 87;
         this.panProd.TabStop = false;
         this.panProd.Text = "商品";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(16, 435);
         this.labMsg.MaximumSize = new System.Drawing.Size(465, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // W40210
         // 
         this.Appearance.BackColor = System.Drawing.Color.White;
         this.Appearance.Options.UseBackColor = true;
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(555, 575);
         this.Name = "W40210";
         this.Text = "W40210";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbxProd)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDays.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.r_frame.PerformLayout();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         this.panKind.ResumeLayout(false);
         this.panKind.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate2.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rdoReportType.Properties)).EndInit();
         this.panProd.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private DevExpress.XtraEditors.TextEdit txtDays;
      private DevExpress.XtraGrid.GridControl gcExport;
      private DevExpress.XtraGrid.Views.Grid.GridView gvExport;
      private DevExpress.XtraEditors.CheckedListBoxControl cbxProd;
      private DevExpress.XtraEditors.PanelControl r_frame;
      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.GroupBox panKind;
      private BaseGround.Widget.TextDateEdit txtEndDate2;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label13;
      protected DevExpress.XtraEditors.RadioGroup rdoReportType;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.GroupBox panProd;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label1;
   }
}