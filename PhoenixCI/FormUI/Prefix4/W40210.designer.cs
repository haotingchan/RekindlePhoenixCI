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
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.rdoReportType = new DevExpress.XtraEditors.RadioGroup();
         this.txtDays = new DevExpress.XtraEditors.TextEdit();
         this.panKind = new System.Windows.Forms.GroupBox();
         this.label2 = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.gcExport = new DevExpress.XtraGrid.GridControl();
         this.gvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.cbxProd = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.txtEndDate2 = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.panProd = new System.Windows.Forms.GroupBox();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFilter.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rdoReportType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDays.Properties)).BeginInit();
         this.panKind.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbxProd)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate2.Properties)).BeginInit();
         this.panProd.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panParent.Controls.Add(this.gcExport);
         this.panParent.Controls.Add(this.labMsg);
         this.panParent.Controls.Add(this.panFilter);
         this.panParent.Size = new System.Drawing.Size(488, 448);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(488, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFilter
         // 
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.panProd);
         this.panFilter.Controls.Add(this.panKind);
         this.panFilter.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(15, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(442, 387);
         this.panFilter.TabIndex = 7;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入交易日期";
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate.EditValue = "2018/12";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(245, 36);
         this.txtEndDate.MenuManager = this.ribbonControl;
         this.txtEndDate.Name = "txtEndDate";
         this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate.TabIndex = 2;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtStartDate.EditValue = "2018/12";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(108, 36);
         this.txtStartDate.MenuManager = this.ribbonControl;
         this.txtStartDate.Name = "txtStartDate";
         this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate.TabIndex = 1;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // rdoReportType
         // 
         this.rdoReportType.EditValue = "Summary";
         this.rdoReportType.EnterMoveNextControl = true;
         this.rdoReportType.Location = new System.Drawing.Point(6, 26);
         this.rdoReportType.Name = "rdoReportType";
         this.rdoReportType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.rdoReportType.Properties.Appearance.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.rdoReportType.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.rdoReportType.Properties.Appearance.Options.UseBackColor = true;
         this.rdoReportType.Properties.Appearance.Options.UseFont = true;
         this.rdoReportType.Properties.Appearance.Options.UseForeColor = true;
         this.rdoReportType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.rdoReportType.Properties.ColumnIndent = 1;
         this.rdoReportType.Properties.Columns = 1;
         this.rdoReportType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_1", "日期起迄："),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("rb_2", "迄止日期：")});
         this.rdoReportType.Properties.ItemsLayout = DevExpress.XtraEditors.RadioGroupItemsLayout.Column;
         this.rdoReportType.Size = new System.Drawing.Size(108, 85);
         this.rdoReportType.TabIndex = 0;
         // 
         // txtDays
         // 
         this.txtDays.Location = new System.Drawing.Point(262, 74);
         this.txtDays.MenuManager = this.ribbonControl;
         this.txtDays.Name = "txtDays";
         this.txtDays.Properties.Mask.EditMask = "n0";
         this.txtDays.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.txtDays.Size = new System.Drawing.Size(59, 26);
         this.txtDays.TabIndex = 4;
         // 
         // panKind
         // 
         this.panKind.Controls.Add(this.label3);
         this.panKind.Controls.Add(this.label1);
         this.panKind.Controls.Add(this.txtEndDate2);
         this.panKind.Controls.Add(this.txtEndDate);
         this.panKind.Controls.Add(this.txtStartDate);
         this.panKind.Controls.Add(this.label2);
         this.panKind.Controls.Add(this.txtDays);
         this.panKind.Controls.Add(this.rdoReportType);
         this.panKind.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panKind.ForeColor = System.Drawing.Color.Black;
         this.panKind.Location = new System.Drawing.Point(22, 26);
         this.panKind.Name = "panKind";
         this.panKind.Size = new System.Drawing.Size(396, 129);
         this.panKind.TabIndex = 79;
         this.panKind.TabStop = false;
         this.panKind.Text = "日期";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(214, 42);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(25, 16);
         this.label2.TabIndex = 76;
         this.label2.Text = "～";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(12, 405);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(168, 16);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "訊息：資料轉出中........";
         this.labMsg.Visible = false;
         // 
         // gcExport
         // 
         this.gcExport.Location = new System.Drawing.Point(463, 28);
         this.gcExport.MainView = this.gvExport;
         this.gcExport.MenuManager = this.ribbonControl;
         this.gcExport.Name = "gcExport";
         this.gcExport.Size = new System.Drawing.Size(350, 333);
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
         this.cbxProd.Location = new System.Drawing.Point(6, 19);
         this.cbxProd.Name = "cbxProd";
         this.cbxProd.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.cbxProd.Size = new System.Drawing.Size(375, 171);
         this.cbxProd.TabIndex = 5;
         // 
         // txtEndDate2
         // 
         this.txtEndDate2.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate2.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate2.EditValue = "2018/12";
         this.txtEndDate2.EnterMoveNextControl = true;
         this.txtEndDate2.Location = new System.Drawing.Point(108, 74);
         this.txtEndDate2.MenuManager = this.ribbonControl;
         this.txtEndDate2.Name = "txtEndDate2";
         this.txtEndDate2.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate2.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtEndDate2.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndDate2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndDate2.Properties.Mask.ShowPlaceHolders = false;
         this.txtEndDate2.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate2.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate2.TabIndex = 3;
         this.txtEndDate2.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Location = new System.Drawing.Point(214, 80);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(40, 16);
         this.label1.TabIndex = 93;
         this.label1.Text = "，近";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label3.ForeColor = System.Drawing.Color.Black;
         this.label3.Location = new System.Drawing.Point(327, 80);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(56, 16);
         this.label3.TabIndex = 94;
         this.label3.Text = "天資料";
         // 
         // panProd
         // 
         this.panProd.Controls.Add(this.cbxProd);
         this.panProd.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panProd.ForeColor = System.Drawing.Color.Black;
         this.panProd.Location = new System.Drawing.Point(22, 172);
         this.panProd.Name = "panProd";
         this.panProd.Size = new System.Drawing.Size(396, 199);
         this.panProd.TabIndex = 93;
         this.panProd.TabStop = false;
         this.panProd.Text = "商品";
         // 
         // W40210
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(488, 478);
         this.Name = "W40210";
         this.Text = "W40210";
         this.panParent.ResumeLayout(false);
         this.panParent.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFilter.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rdoReportType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDays.Properties)).EndInit();
         this.panKind.ResumeLayout(false);
         this.panKind.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvExport)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbxProd)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate2.Properties)).EndInit();
         this.panProd.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.GroupBox panKind;
      private System.Windows.Forms.Label label2;
      private DevExpress.XtraEditors.TextEdit txtDays;
      protected DevExpress.XtraEditors.RadioGroup rdoReportType;
      private DevExpress.XtraGrid.GridControl gcExport;
      private DevExpress.XtraGrid.Views.Grid.GridView gvExport;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
      private System.Windows.Forms.GroupBox panProd;
      private DevExpress.XtraEditors.CheckedListBoxControl cbxProd;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label1;
      private BaseGround.Widget.TextDateEdit txtEndDate2;
   }
}