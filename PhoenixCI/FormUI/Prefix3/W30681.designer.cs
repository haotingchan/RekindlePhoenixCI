namespace PhoenixCI.FormUI.Prefix3 {
   partial class W30681 {
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
            this.labDate = new System.Windows.Forms.Label();
            this.labReportType = new System.Windows.Forms.Label();
            this.rdoSource = new DevExpress.XtraEditors.RadioGroup();
            this.labMth2 = new System.Windows.Forms.Label();
            this.labMth1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMth2 = new DevExpress.XtraEditors.TextEdit();
            this.txtMth1 = new DevExpress.XtraEditors.TextEdit();
            this.panKind = new System.Windows.Forms.GroupBox();
            this.labKind2 = new System.Windows.Forms.Label();
            this.labKind1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKind2 = new DevExpress.XtraEditors.TextEdit();
            this.txtKind1 = new DevExpress.XtraEditors.TextEdit();
            this.ddlScCode = new DevExpress.XtraEditors.LookUpEdit();
            this.labScCode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkLevel = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.labMsg = new System.Windows.Forms.Label();
            this.panOsf = new System.Windows.Forms.GroupBox();
            this.ddlOsfOrderCond = new DevExpress.XtraEditors.LookUpEdit();
            this.ddlOsfOrderType = new DevExpress.XtraEditors.LookUpEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panDetail = new System.Windows.Forms.GroupBox();
            this.labMemo3 = new System.Windows.Forms.Label();
            this.labMemo2 = new System.Windows.Forms.Label();
            this.labMemo1 = new System.Windows.Forms.Label();
            this.labLevel = new System.Windows.Forms.Label();
            this.gcExport = new DevExpress.XtraGrid.GridControl();
            this.gvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.panFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoReportType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoSource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMth2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMth1.Properties)).BeginInit();
            this.panKind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtKind2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKind1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlScCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLevel)).BeginInit();
            this.panOsf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddlOsfOrderCond.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlOsfOrderType.Properties)).BeginInit();
            this.panDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExport)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panParent.Controls.Add(this.gcExport);
            this.panParent.Controls.Add(this.panDetail);
            this.panParent.Controls.Add(this.labMsg);
            this.panParent.Controls.Add(this.panFilter);
            this.panParent.Size = new System.Drawing.Size(735, 648);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(735, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panFilter
            // 
            this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panFilter.Controls.Add(this.txtEndDate);
            this.panFilter.Controls.Add(this.txtStartDate);
            this.panFilter.Controls.Add(this.rdoReportType);
            this.panFilter.Controls.Add(this.labDate);
            this.panFilter.Controls.Add(this.labReportType);
            this.panFilter.Controls.Add(this.rdoSource);
            this.panFilter.Controls.Add(this.labMth2);
            this.panFilter.Controls.Add(this.labMth1);
            this.panFilter.Controls.Add(this.label9);
            this.panFilter.Controls.Add(this.txtMth2);
            this.panFilter.Controls.Add(this.txtMth1);
            this.panFilter.Controls.Add(this.panKind);
            this.panFilter.Controls.Add(this.label2);
            this.panFilter.Controls.Add(this.label6);
            this.panFilter.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panFilter.ForeColor = System.Drawing.Color.Navy;
            this.panFilter.Location = new System.Drawing.Point(15, 15);
            this.panFilter.Name = "panFilter";
            this.panFilter.Size = new System.Drawing.Size(596, 333);
            this.panFilter.TabIndex = 7;
            this.panFilter.TabStop = false;
            this.panFilter.Text = "請輸入條件";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "2018/12";
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(250, 96);
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
            this.txtEndDate.TabIndex = 91;
            this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "2018/12";
            this.txtStartDate.EnterMoveNextControl = true;
            this.txtStartDate.Location = new System.Drawing.Point(113, 96);
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
            this.txtStartDate.TabIndex = 90;
            this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // rdoReportType
            // 
            this.rdoReportType.EditValue = "Summary";
            this.rdoReportType.Location = new System.Drawing.Point(113, 61);
            this.rdoReportType.Name = "rdoReportType";
            this.rdoReportType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoReportType.Properties.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.rdoReportType.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.rdoReportType.Properties.Appearance.Options.UseBackColor = true;
            this.rdoReportType.Properties.Appearance.Options.UseFont = true;
            this.rdoReportType.Properties.Appearance.Options.UseForeColor = true;
            this.rdoReportType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.rdoReportType.Properties.Columns = 3;
            this.rdoReportType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Summary", "統計"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Detail", "明細")});
            this.rdoReportType.Size = new System.Drawing.Size(164, 29);
            this.rdoReportType.TabIndex = 1;
            this.rdoReportType.SelectedIndexChanged += new System.EventHandler(this.rdoReportType_SelectedIndexChanged);
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labDate.ForeColor = System.Drawing.Color.Black;
            this.labDate.Location = new System.Drawing.Point(17, 102);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(93, 16);
            this.labDate.TabIndex = 89;
            this.labDate.Text = "資料日期：";
            // 
            // labReportType
            // 
            this.labReportType.AutoSize = true;
            this.labReportType.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labReportType.ForeColor = System.Drawing.Color.Black;
            this.labReportType.Location = new System.Drawing.Point(16, 69);
            this.labReportType.Name = "labReportType";
            this.labReportType.Size = new System.Drawing.Size(93, 16);
            this.labReportType.TabIndex = 88;
            this.labReportType.Text = "報表種類：";
            // 
            // rdoSource
            // 
            this.rdoSource.AutoSizeInLayoutControl = true;
            this.rdoSource.EditValue = "Old";
            this.rdoSource.Location = new System.Drawing.Point(113, 26);
            this.rdoSource.Name = "rdoSource";
            this.rdoSource.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoSource.Properties.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.rdoSource.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.rdoSource.Properties.Appearance.Options.UseBackColor = true;
            this.rdoSource.Properties.Appearance.Options.UseFont = true;
            this.rdoSource.Properties.Appearance.Options.UseForeColor = true;
            this.rdoSource.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.rdoSource.Properties.Columns = 3;
            this.rdoSource.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Old", "舊"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("New", "新")});
            this.rdoSource.Size = new System.Drawing.Size(164, 29);
            this.rdoSource.TabIndex = 0;
            // 
            // labMth2
            // 
            this.labMth2.AutoSize = true;
            this.labMth2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labMth2.ForeColor = System.Drawing.Color.Black;
            this.labMth2.Location = new System.Drawing.Point(145, 301);
            this.labMth2.Name = "labMth2";
            this.labMth2.Size = new System.Drawing.Size(68, 16);
            this.labMth2.TabIndex = 84;
            this.labMth2.Text = "第2支腳";
            // 
            // labMth1
            // 
            this.labMth1.AutoSize = true;
            this.labMth1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labMth1.ForeColor = System.Drawing.Color.Black;
            this.labMth1.Location = new System.Drawing.Point(60, 269);
            this.labMth1.Name = "labMth1";
            this.labMth1.Size = new System.Drawing.Size(153, 16);
            this.labMth1.TabIndex = 83;
            this.labMth1.Text = "到期月序：第1支腳";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.ForeColor = System.Drawing.Color.Navy;
            this.label9.Location = new System.Drawing.Point(324, 269);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 16);
            this.label9.TabIndex = 82;
            this.label9.Text = "（空白代表全部）";
            // 
            // txtMth2
            // 
            this.txtMth2.Location = new System.Drawing.Point(218, 295);
            this.txtMth2.MenuManager = this.ribbonControl;
            this.txtMth2.Name = "txtMth2";
            this.txtMth2.Properties.Mask.EditMask = "[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?";
            this.txtMth2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtMth2.Size = new System.Drawing.Size(100, 26);
            this.txtMth2.TabIndex = 8;
            // 
            // txtMth1
            // 
            this.txtMth1.Location = new System.Drawing.Point(218, 263);
            this.txtMth1.MenuManager = this.ribbonControl;
            this.txtMth1.Name = "txtMth1";
            this.txtMth1.Properties.Mask.EditMask = "[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?";
            this.txtMth1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtMth1.Size = new System.Drawing.Size(100, 26);
            this.txtMth1.TabIndex = 7;
            // 
            // panKind
            // 
            this.panKind.Controls.Add(this.labKind2);
            this.panKind.Controls.Add(this.labKind1);
            this.panKind.Controls.Add(this.label1);
            this.panKind.Controls.Add(this.txtKind2);
            this.panKind.Controls.Add(this.txtKind1);
            this.panKind.Controls.Add(this.ddlScCode);
            this.panKind.Controls.Add(this.labScCode);
            this.panKind.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panKind.ForeColor = System.Drawing.Color.Black;
            this.panKind.Location = new System.Drawing.Point(19, 128);
            this.panKind.Name = "panKind";
            this.panKind.Size = new System.Drawing.Size(464, 129);
            this.panKind.TabIndex = 79;
            this.panKind.TabStop = false;
            this.panKind.Text = "商品";
            // 
            // labKind2
            // 
            this.labKind2.AutoSize = true;
            this.labKind2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labKind2.ForeColor = System.Drawing.Color.Black;
            this.labKind2.Location = new System.Drawing.Point(125, 96);
            this.labKind2.Name = "labKind2";
            this.labKind2.Size = new System.Drawing.Size(68, 16);
            this.labKind2.TabIndex = 13;
            this.labKind2.Text = "第2支腳";
            // 
            // labKind1
            // 
            this.labKind1.AutoSize = true;
            this.labKind1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labKind1.ForeColor = System.Drawing.Color.Black;
            this.labKind1.Location = new System.Drawing.Point(40, 64);
            this.labKind1.Name = "labKind1";
            this.labKind1.Size = new System.Drawing.Size(153, 16);
            this.labKind1.TabIndex = 12;
            this.labKind1.Text = "商品代號：第1支腳";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(304, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "（空白代表全部）";
            // 
            // txtKind2
            // 
            this.txtKind2.Location = new System.Drawing.Point(198, 90);
            this.txtKind2.MenuManager = this.ribbonControl;
            this.txtKind2.Name = "txtKind2";
            this.txtKind2.Size = new System.Drawing.Size(100, 26);
            this.txtKind2.TabIndex = 6;
            // 
            // txtKind1
            // 
            this.txtKind1.Location = new System.Drawing.Point(198, 58);
            this.txtKind1.MenuManager = this.ribbonControl;
            this.txtKind1.Name = "txtKind1";
            this.txtKind1.Size = new System.Drawing.Size(100, 26);
            this.txtKind1.TabIndex = 5;
            // 
            // ddlScCode
            // 
            this.ddlScCode.EditValue = "%";
            this.ddlScCode.Location = new System.Drawing.Point(176, 26);
            this.ddlScCode.Name = "ddlScCode";
            this.ddlScCode.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ddlScCode.Properties.Appearance.Options.UseBackColor = true;
            this.ddlScCode.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ddlScCode.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.ddlScCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlScCode.Properties.DropDownRows = 5;
            this.ddlScCode.Properties.NullText = "";
            this.ddlScCode.Properties.PopupSizeable = false;
            this.ddlScCode.Size = new System.Drawing.Size(122, 26);
            this.ddlScCode.TabIndex = 4;
            // 
            // labScCode
            // 
            this.labScCode.AutoSize = true;
            this.labScCode.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labScCode.ForeColor = System.Drawing.Color.Black;
            this.labScCode.Location = new System.Drawing.Point(91, 32);
            this.labScCode.Name = "labScCode";
            this.labScCode.Size = new System.Drawing.Size(91, 16);
            this.labScCode.TabIndex = 11;
            this.labScCode.Text = "單 / 複式：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 16);
            this.label2.TabIndex = 76;
            this.label2.Text = "～";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label6.Location = new System.Drawing.Point(123, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 16);
            this.label6.TabIndex = 73;
            // 
            // chkLevel
            // 
            this.chkLevel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
            this.chkLevel.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.chkLevel.Appearance.Options.UseBackColor = true;
            this.chkLevel.Appearance.Options.UseFont = true;
            this.chkLevel.AppearanceSelected.BackColor = System.Drawing.Color.White;
            this.chkLevel.AppearanceSelected.BackColor2 = System.Drawing.Color.White;
            this.chkLevel.AppearanceSelected.Options.UseBackColor = true;
            this.chkLevel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.chkLevel.ColumnWidth = 57;
            this.chkLevel.HorzScrollStep = 9;
            this.chkLevel.ItemAutoHeight = true;
            this.chkLevel.ItemPadding = new System.Windows.Forms.Padding(11);
            this.chkLevel.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("1", "1", System.Windows.Forms.CheckState.Unchecked, false),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("2", "2"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("3", "3"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("4", "4"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("5", "5"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("6", "6"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("7", "7"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("NULL", "空白")});
            this.chkLevel.Location = new System.Drawing.Point(113, 116);
            this.chkLevel.MultiColumn = true;
            this.chkLevel.Name = "chkLevel";
            this.chkLevel.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.chkLevel.Size = new System.Drawing.Size(456, 41);
            this.chkLevel.TabIndex = 11;
            // 
            // labMsg
            // 
            this.labMsg.AutoSize = true;
            this.labMsg.ForeColor = System.Drawing.Color.Blue;
            this.labMsg.Location = new System.Drawing.Point(15, 593);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new System.Drawing.Size(169, 20);
            this.labMsg.TabIndex = 10;
            this.labMsg.Text = "訊息：資料轉出中........";
            this.labMsg.Visible = false;
            // 
            // panOsf
            // 
            this.panOsf.Controls.Add(this.ddlOsfOrderCond);
            this.panOsf.Controls.Add(this.ddlOsfOrderType);
            this.panOsf.Controls.Add(this.label10);
            this.panOsf.Controls.Add(this.label13);
            this.panOsf.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panOsf.ForeColor = System.Drawing.Color.Black;
            this.panOsf.Location = new System.Drawing.Point(20, 26);
            this.panOsf.Name = "panOsf";
            this.panOsf.Size = new System.Drawing.Size(463, 93);
            this.panOsf.TabIndex = 85;
            this.panOsf.TabStop = false;
            this.panOsf.Text = "委託單種類";
            // 
            // ddlOsfOrderCond
            // 
            this.ddlOsfOrderCond.EditValue = "%";
            this.ddlOsfOrderCond.Location = new System.Drawing.Point(175, 58);
            this.ddlOsfOrderCond.Name = "ddlOsfOrderCond";
            this.ddlOsfOrderCond.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ddlOsfOrderCond.Properties.Appearance.Options.UseBackColor = true;
            this.ddlOsfOrderCond.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ddlOsfOrderCond.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.ddlOsfOrderCond.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlOsfOrderCond.Properties.DropDownRows = 5;
            this.ddlOsfOrderCond.Properties.NullText = "";
            this.ddlOsfOrderCond.Properties.PopupSizeable = false;
            this.ddlOsfOrderCond.Size = new System.Drawing.Size(122, 26);
            this.ddlOsfOrderCond.TabIndex = 10;
            // 
            // ddlOsfOrderType
            // 
            this.ddlOsfOrderType.EditValue = "%";
            this.ddlOsfOrderType.Location = new System.Drawing.Point(175, 26);
            this.ddlOsfOrderType.Name = "ddlOsfOrderType";
            this.ddlOsfOrderType.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ddlOsfOrderType.Properties.Appearance.Options.UseBackColor = true;
            this.ddlOsfOrderType.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ddlOsfOrderType.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.ddlOsfOrderType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlOsfOrderType.Properties.DropDownRows = 5;
            this.ddlOsfOrderType.Properties.NullText = "";
            this.ddlOsfOrderType.Properties.PopupSizeable = false;
            this.ddlOsfOrderType.Size = new System.Drawing.Size(122, 26);
            this.ddlOsfOrderType.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(122, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 16);
            this.label10.TabIndex = 13;
            this.label10.Text = "條件：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(122, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 16);
            this.label13.TabIndex = 11;
            this.label13.Text = "方式：";
            // 
            // panDetail
            // 
            this.panDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
            this.panDetail.Controls.Add(this.labMemo3);
            this.panDetail.Controls.Add(this.labMemo2);
            this.panDetail.Controls.Add(this.labMemo1);
            this.panDetail.Controls.Add(this.panOsf);
            this.panDetail.Controls.Add(this.chkLevel);
            this.panDetail.Controls.Add(this.labLevel);
            this.panDetail.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panDetail.ForeColor = System.Drawing.Color.Black;
            this.panDetail.Location = new System.Drawing.Point(15, 354);
            this.panDetail.Name = "panDetail";
            this.panDetail.Size = new System.Drawing.Size(596, 236);
            this.panDetail.TabIndex = 86;
            this.panDetail.TabStop = false;
            this.panDetail.Text = "明細條件";
            // 
            // labMemo3
            // 
            this.labMemo3.AutoSize = true;
            this.labMemo3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labMemo3.ForeColor = System.Drawing.Color.Black;
            this.labMemo3.Location = new System.Drawing.Point(52, 211);
            this.labMemo3.Name = "labMemo3";
            this.labMemo3.Size = new System.Drawing.Size(323, 16);
            this.labMemo3.TabIndex = 87;
            this.labMemo3.Text = "複式單是>=0.5%(即級距2至7)才有明細資料。";
            // 
            // labMemo2
            // 
            this.labMemo2.AutoSize = true;
            this.labMemo2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labMemo2.ForeColor = System.Drawing.Color.Black;
            this.labMemo2.Location = new System.Drawing.Point(52, 186);
            this.labMemo2.Name = "labMemo2";
            this.labMemo2.Size = new System.Drawing.Size(508, 16);
            this.labMemo2.TabIndex = 86;
            this.labMemo2.Text = "TX及MTX最近月與次近月契約單式單>=1%(即級距3至7)才有明細資料，";
            // 
            // labMemo1
            // 
            this.labMemo1.AutoSize = true;
            this.labMemo1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labMemo1.ForeColor = System.Drawing.Color.Black;
            this.labMemo1.Location = new System.Drawing.Point(19, 160);
            this.labMemo1.Name = "labMemo1";
            this.labMemo1.Size = new System.Drawing.Size(471, 16);
            this.labMemo1.TabIndex = 13;
            this.labMemo1.Text = "註：級距間隔為0.5%，例如級距1為0<=X<0.5，級距2為0.5<=X<1。";
            // 
            // labLevel
            // 
            this.labLevel.AutoSize = true;
            this.labLevel.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labLevel.ForeColor = System.Drawing.Color.Black;
            this.labLevel.Location = new System.Drawing.Point(19, 131);
            this.labLevel.Name = "labLevel";
            this.labLevel.Size = new System.Drawing.Size(93, 16);
            this.labLevel.TabIndex = 11;
            this.labLevel.Text = "明細級距：";
            // 
            // gcExport
            // 
            this.gcExport.Location = new System.Drawing.Point(617, 380);
            this.gcExport.MainView = this.gvExport;
            this.gcExport.MenuManager = this.ribbonControl;
            this.gcExport.Name = "gcExport";
            this.gcExport.Size = new System.Drawing.Size(329, 253);
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
            // W30681
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 678);
            this.Name = "W30681";
            this.Text = "W30681";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.panFilter.ResumeLayout(false);
            this.panFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoReportType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoSource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMth2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMth1.Properties)).EndInit();
            this.panKind.ResumeLayout(false);
            this.panKind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtKind2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKind1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlScCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLevel)).EndInit();
            this.panOsf.ResumeLayout(false);
            this.panOsf.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddlOsfOrderCond.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlOsfOrderType.Properties)).EndInit();
            this.panDetail.ResumeLayout(false);
            this.panDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox panFilter;
      private System.Windows.Forms.Label labMsg;
      private System.Windows.Forms.Label label6;
      private DevExpress.XtraEditors.CheckedListBoxControl chkLevel;
      private System.Windows.Forms.GroupBox panKind;
      private System.Windows.Forms.Label labKind2;
      private System.Windows.Forms.Label labKind1;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.TextEdit txtKind2;
      private DevExpress.XtraEditors.TextEdit txtKind1;
      private DevExpress.XtraEditors.LookUpEdit ddlScCode;
      private System.Windows.Forms.Label labScCode;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label labMth2;
      private System.Windows.Forms.Label labMth1;
      private System.Windows.Forms.Label label9;
      private DevExpress.XtraEditors.TextEdit txtMth2;
      private DevExpress.XtraEditors.TextEdit txtMth1;
      private System.Windows.Forms.GroupBox panDetail;
      private System.Windows.Forms.Label labMemo3;
      private System.Windows.Forms.Label labMemo2;
      private System.Windows.Forms.Label labMemo1;
      private System.Windows.Forms.GroupBox panOsf;
      private DevExpress.XtraEditors.LookUpEdit ddlOsfOrderCond;
      private System.Windows.Forms.Label label10;
      private DevExpress.XtraEditors.LookUpEdit ddlOsfOrderType;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Label labLevel;
      private System.Windows.Forms.Label labDate;
      private System.Windows.Forms.Label labReportType;
      protected DevExpress.XtraEditors.RadioGroup rdoReportType;
      protected DevExpress.XtraEditors.RadioGroup rdoSource;
      private DevExpress.XtraGrid.GridControl gcExport;
      private DevExpress.XtraGrid.Views.Grid.GridView gvExport;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private BaseGround.Widget.TextDateEdit txtStartDate;
   }
}