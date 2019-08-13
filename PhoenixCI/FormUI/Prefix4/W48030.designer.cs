namespace PhoenixCI.FormUI.Prefix4 {
    partial class W48030 {
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
         DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
         this.panFirst = new System.Windows.Forms.GroupBox();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.btnFirstFilter = new System.Windows.Forms.Button();
         this.labEndDate = new System.Windows.Forms.Label();
         this.panFilter = new System.Windows.Forms.GroupBox();
         this.labMsg = new System.Windows.Forms.Label();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.chkModel = new DevExpress.XtraEditors.CheckedListBoxControl();
         this.gcDate = new DevExpress.XtraGrid.GridControl();
         this.gvDate = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.AI2_SELECT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MON_DIFF = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SDATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.EDATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.DAY_CNT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.labDateDesc = new System.Windows.Forms.Label();
         this.panSecond = new System.Windows.Forms.GroupBox();
         this.cbxSubType = new DevExpress.XtraEditors.LookUpEdit();
         this.labSubType = new System.Windows.Forms.Label();
         this.btnClearAll = new System.Windows.Forms.Button();
         this.btnChooseAll = new System.Windows.Forms.Button();
         this.gcKind = new DevExpress.XtraGrid.GridControl();
         this.gvKind = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.CPR_SELECT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
         this.CPR_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CPR_PRICE_RISK_RATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.RISK_INTERVAL = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.CPR_EFFECTIVE_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.LAST_RISK_RATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CPR_PROD_SUBTYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.PROD_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CPR_PARAM_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CPR_MODIFY_FLAG = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CPR_PRICE_RISK_RATE_ORG = new DevExpress.XtraGrid.Columns.GridColumn();
         this.r_frame = new DevExpress.XtraEditors.PanelControl();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panFirst.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         this.panFilter.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkModel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcDate)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvDate)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(repositoryItemCheckEdit2)).BeginInit();
         this.panSecond.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbxSubType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcKind)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvKind)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).BeginInit();
         this.r_frame.SuspendLayout();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.r_frame);
         this.panParent.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.panParent.Size = new System.Drawing.Size(945, 753);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.ribbonControl.Size = new System.Drawing.Size(945, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panFirst
         // 
         this.panFirst.Controls.Add(this.txtEndDate);
         this.panFirst.Controls.Add(this.label1);
         this.panFirst.Controls.Add(this.btnFirstFilter);
         this.panFirst.Controls.Add(this.labEndDate);
         this.panFirst.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFirst.ForeColor = System.Drawing.Color.Navy;
         this.panFirst.Location = new System.Drawing.Point(16, 28);
         this.panFirst.Name = "panFirst";
         this.panFirst.Size = new System.Drawing.Size(823, 105);
         this.panFirst.TabIndex = 6;
         this.panFirst.TabStop = false;
         this.panFirst.Text = "1.設定初步條件";
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEndDate.EditValue = "2018/12/01";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(126, 34);
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
         this.txtEndDate.TabIndex = 0;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         this.txtEndDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEndDate_KeyDown);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label1.ForeColor = System.Drawing.Color.Red;
         this.label1.Location = new System.Drawing.Point(40, 69);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(257, 20);
         this.label1.TabIndex = 3;
         this.label1.Text = "日期調整後請按Enter更新畫面資料";
         // 
         // btnFirstFilter
         // 
         this.btnFirstFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.btnFirstFilter.ForeColor = System.Drawing.Color.Black;
         this.btnFirstFilter.Location = new System.Drawing.Point(302, 32);
         this.btnFirstFilter.Margin = new System.Windows.Forms.Padding(0);
         this.btnFirstFilter.Name = "btnFirstFilter";
         this.btnFirstFilter.Size = new System.Drawing.Size(77, 32);
         this.btnFirstFilter.TabIndex = 1;
         this.btnFirstFilter.Text = "查詢";
         this.btnFirstFilter.UseVisualStyleBackColor = true;
         this.btnFirstFilter.Click += new System.EventHandler(this.btnFirstFilter_Click);
         // 
         // labEndDate
         // 
         this.labEndDate.AutoSize = true;
         this.labEndDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labEndDate.ForeColor = System.Drawing.Color.Black;
         this.labEndDate.Location = new System.Drawing.Point(40, 37);
         this.labEndDate.Name = "labEndDate";
         this.labEndDate.Size = new System.Drawing.Size(90, 21);
         this.labEndDate.TabIndex = 2;
         this.labEndDate.Text = "查詢日期：";
         // 
         // panFilter
         // 
         this.panFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panFilter.Controls.Add(this.labMsg);
         this.panFilter.Controls.Add(this.groupBox1);
         this.panFilter.Controls.Add(this.gcDate);
         this.panFilter.Controls.Add(this.labDateDesc);
         this.panFilter.Controls.Add(this.panSecond);
         this.panFilter.Controls.Add(this.panFirst);
         this.panFilter.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panFilter.ForeColor = System.Drawing.Color.Navy;
         this.panFilter.Location = new System.Drawing.Point(20, 15);
         this.panFilter.Name = "panFilter";
         this.panFilter.Size = new System.Drawing.Size(855, 682);
         this.panFilter.TabIndex = 19;
         this.panFilter.TabStop = false;
         this.panFilter.Text = "請輸入查詢條件";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(446, 367);
         this.labMsg.MaximumSize = new System.Drawing.Size(389, 120);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(86, 21);
         this.labMsg.TabIndex = 86;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.chkModel);
         this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.groupBox1.ForeColor = System.Drawing.Color.Navy;
         this.groupBox1.Location = new System.Drawing.Point(16, 600);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(416, 70);
         this.groupBox1.TabIndex = 85;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "3.指標種類";
         // 
         // chkModel
         // 
         this.chkModel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.chkModel.Appearance.Options.UseBackColor = true;
         this.chkModel.Appearance.Options.UseTextOptions = true;
         this.chkModel.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.chkModel.AppearanceSelected.BackColor = System.Drawing.Color.White;
         this.chkModel.AppearanceSelected.BackColor2 = System.Drawing.Color.White;
         this.chkModel.AppearanceSelected.Options.UseBackColor = true;
         this.chkModel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.chkModel.CheckOnClick = true;
         this.chkModel.ColumnWidth = 130;
         this.chkModel.HotTrackSelectMode = DevExpress.XtraEditors.HotTrackSelectMode.SelectItemOnClick;
         this.chkModel.ItemAutoHeight = true;
         this.chkModel.ItemPadding = new System.Windows.Forms.Padding(5);
         this.chkModel.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkSma", "SMA"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkEwma", "EWMA"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("chkMax", "Max")});
         this.chkModel.Location = new System.Drawing.Point(20, 26);
         this.chkModel.LookAndFeel.SkinName = "Office 2013";
         this.chkModel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.chkModel.MultiColumn = true;
         this.chkModel.Name = "chkModel";
         this.chkModel.SelectionMode = System.Windows.Forms.SelectionMode.None;
         this.chkModel.Size = new System.Drawing.Size(393, 34);
         this.chkModel.TabIndex = 5;
         // 
         // gcDate
         // 
         this.gcDate.Location = new System.Drawing.Point(450, 177);
         this.gcDate.MainView = this.gvDate;
         this.gcDate.MenuManager = this.ribbonControl;
         this.gcDate.Name = "gcDate";
         this.gcDate.Size = new System.Drawing.Size(389, 169);
         this.gcDate.TabIndex = 6;
         this.gcDate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDate});
         // 
         // gvDate
         // 
         this.gvDate.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Cyan;
         this.gvDate.Appearance.HeaderPanel.Options.UseBackColor = true;
         this.gvDate.Appearance.Row.BorderColor = System.Drawing.Color.Black;
         this.gvDate.Appearance.Row.Options.UseBorderColor = true;
         this.gvDate.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.gvDate.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AI2_SELECT,
            this.MON_DIFF,
            this.SDATE,
            this.EDATE,
            this.DAY_CNT});
         this.gvDate.GridControl = this.gcDate;
         this.gvDate.Name = "gvDate";
         this.gvDate.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gvDate.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // AI2_SELECT
         // 
         this.AI2_SELECT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.AI2_SELECT.AppearanceHeader.Options.UseBackColor = true;
         this.AI2_SELECT.Caption = "勾選";
         repositoryItemCheckEdit2.AutoWidth = true;
         repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
         repositoryItemCheckEdit2.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
         repositoryItemCheckEdit2.ValueChecked = "Y";
         repositoryItemCheckEdit2.ValueUnchecked = "N";
         this.AI2_SELECT.ColumnEdit = repositoryItemCheckEdit2;
         this.AI2_SELECT.FieldName = "AI2_SELECT";
         this.AI2_SELECT.Name = "AI2_SELECT";
         this.AI2_SELECT.Width = 39;
         // 
         // MON_DIFF
         // 
         this.MON_DIFF.AppearanceCell.Options.UseTextOptions = true;
         this.MON_DIFF.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MON_DIFF.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MON_DIFF.AppearanceHeader.Options.UseBackColor = true;
         this.MON_DIFF.AppearanceHeader.Options.UseTextOptions = true;
         this.MON_DIFF.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MON_DIFF.Caption = "期間";
         this.MON_DIFF.FieldName = "MON_DIFF";
         this.MON_DIFF.Name = "MON_DIFF";
         this.MON_DIFF.OptionsColumn.AllowEdit = false;
         this.MON_DIFF.Visible = true;
         this.MON_DIFF.VisibleIndex = 0;
         this.MON_DIFF.Width = 78;
         // 
         // SDATE
         // 
         this.SDATE.AppearanceCell.Options.UseTextOptions = true;
         this.SDATE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.SDATE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.SDATE.AppearanceHeader.Options.UseBackColor = true;
         this.SDATE.AppearanceHeader.Options.UseTextOptions = true;
         this.SDATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.SDATE.Caption = "起日";
         this.SDATE.FieldName = "SDATE";
         this.SDATE.Name = "SDATE";
         this.SDATE.OptionsColumn.AllowEdit = false;
         this.SDATE.Visible = true;
         this.SDATE.VisibleIndex = 1;
         this.SDATE.Width = 117;
         // 
         // EDATE
         // 
         this.EDATE.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
         this.EDATE.AppearanceCell.Options.UseForeColor = true;
         this.EDATE.AppearanceCell.Options.UseTextOptions = true;
         this.EDATE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.EDATE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.EDATE.AppearanceHeader.Options.UseBackColor = true;
         this.EDATE.AppearanceHeader.Options.UseTextOptions = true;
         this.EDATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.EDATE.Caption = "迄日";
         this.EDATE.FieldName = "EDATE";
         this.EDATE.Name = "EDATE";
         this.EDATE.OptionsColumn.AllowEdit = false;
         this.EDATE.Visible = true;
         this.EDATE.VisibleIndex = 2;
         this.EDATE.Width = 117;
         // 
         // DAY_CNT
         // 
         this.DAY_CNT.AppearanceCell.Options.UseTextOptions = true;
         this.DAY_CNT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.DAY_CNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.DAY_CNT.AppearanceHeader.Options.UseBackColor = true;
         this.DAY_CNT.AppearanceHeader.Options.UseTextOptions = true;
         this.DAY_CNT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.DAY_CNT.Caption = "天數";
         this.DAY_CNT.FieldName = "DAY_CNT";
         this.DAY_CNT.Name = "DAY_CNT";
         this.DAY_CNT.OptionsColumn.AllowEdit = false;
         this.DAY_CNT.Visible = true;
         this.DAY_CNT.VisibleIndex = 3;
         this.DAY_CNT.Width = 60;
         // 
         // labDateDesc
         // 
         this.labDateDesc.AutoSize = true;
         this.labDateDesc.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labDateDesc.ForeColor = System.Drawing.Color.Black;
         this.labDateDesc.Location = new System.Drawing.Point(446, 150);
         this.labDateDesc.Name = "labDateDesc";
         this.labDateDesc.Size = new System.Drawing.Size(122, 21);
         this.labDateDesc.TabIndex = 24;
         this.labDateDesc.Text = "圖表資料區間：";
         // 
         // panSecond
         // 
         this.panSecond.Controls.Add(this.cbxSubType);
         this.panSecond.Controls.Add(this.labSubType);
         this.panSecond.Controls.Add(this.btnClearAll);
         this.panSecond.Controls.Add(this.btnChooseAll);
         this.panSecond.Controls.Add(this.gcKind);
         this.panSecond.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.panSecond.ForeColor = System.Drawing.Color.Navy;
         this.panSecond.Location = new System.Drawing.Point(16, 142);
         this.panSecond.Name = "panSecond";
         this.panSecond.Size = new System.Drawing.Size(416, 450);
         this.panSecond.TabIndex = 21;
         this.panSecond.TabStop = false;
         this.panSecond.Text = "2.選擇契約";
         // 
         // cbxSubType
         // 
         this.cbxSubType.Location = new System.Drawing.Point(127, 34);
         this.cbxSubType.Name = "cbxSubType";
         this.cbxSubType.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.cbxSubType.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.cbxSubType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbxSubType.Properties.LookAndFeel.SkinName = "The Bezier";
         this.cbxSubType.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.cbxSubType.Properties.NullText = "";
         this.cbxSubType.Properties.PopupSizeable = false;
         this.cbxSubType.Size = new System.Drawing.Size(121, 26);
         this.cbxSubType.TabIndex = 2;
         this.cbxSubType.EditValueChanged += new System.EventHandler(this.cbxSubType_EditValueChanged);
         // 
         // labSubType
         // 
         this.labSubType.AutoSize = true;
         this.labSubType.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.labSubType.ForeColor = System.Drawing.Color.Black;
         this.labSubType.Location = new System.Drawing.Point(40, 37);
         this.labSubType.Name = "labSubType";
         this.labSubType.Size = new System.Drawing.Size(90, 21);
         this.labSubType.TabIndex = 26;
         this.labSubType.Text = "契約種類：";
         // 
         // btnClearAll
         // 
         this.btnClearAll.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.btnClearAll.ForeColor = System.Drawing.Color.Black;
         this.btnClearAll.Location = new System.Drawing.Point(140, 75);
         this.btnClearAll.Margin = new System.Windows.Forms.Padding(0);
         this.btnClearAll.Name = "btnClearAll";
         this.btnClearAll.Size = new System.Drawing.Size(77, 32);
         this.btnClearAll.TabIndex = 4;
         this.btnClearAll.Text = "全不選";
         this.btnClearAll.UseVisualStyleBackColor = true;
         this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
         // 
         // btnChooseAll
         // 
         this.btnChooseAll.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.btnChooseAll.ForeColor = System.Drawing.Color.Black;
         this.btnChooseAll.Location = new System.Drawing.Point(45, 75);
         this.btnChooseAll.Margin = new System.Windows.Forms.Padding(0);
         this.btnChooseAll.Name = "btnChooseAll";
         this.btnChooseAll.Size = new System.Drawing.Size(77, 32);
         this.btnChooseAll.TabIndex = 3;
         this.btnChooseAll.Text = "全選";
         this.btnChooseAll.UseVisualStyleBackColor = true;
         this.btnChooseAll.Click += new System.EventHandler(this.btnChooseAll_Click);
         // 
         // gcKind
         // 
         this.gcKind.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.gcKind.Location = new System.Drawing.Point(3, 125);
         this.gcKind.MainView = this.gvKind;
         this.gcKind.MenuManager = this.ribbonControl;
         this.gcKind.Name = "gcKind";
         this.gcKind.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2});
         this.gcKind.Size = new System.Drawing.Size(410, 322);
         this.gcKind.TabIndex = 5;
         this.gcKind.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvKind});
         // 
         // gvKind
         // 
         this.gvKind.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Cyan;
         this.gvKind.Appearance.HeaderPanel.Options.UseBackColor = true;
         this.gvKind.Appearance.Row.BorderColor = System.Drawing.Color.Black;
         this.gvKind.Appearance.Row.Options.UseBorderColor = true;
         this.gvKind.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.gvKind.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.CPR_SELECT,
            this.CPR_KIND_ID,
            this.CPR_PRICE_RISK_RATE,
            this.RISK_INTERVAL,
            this.CPR_EFFECTIVE_DATE,
            this.LAST_RISK_RATE,
            this.CPR_PROD_SUBTYPE,
            this.PROD_TYPE,
            this.CPR_PARAM_KEY,
            this.CPR_MODIFY_FLAG,
            this.CPR_PRICE_RISK_RATE_ORG});
         this.gvKind.GridControl = this.gcKind;
         this.gvKind.Name = "gvKind";
         this.gvKind.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gvKind.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // CPR_SELECT
         // 
         this.CPR_SELECT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.CPR_SELECT.AppearanceHeader.Options.UseBackColor = true;
         this.CPR_SELECT.Caption = "勾選";
         this.CPR_SELECT.ColumnEdit = this.repositoryItemCheckEdit1;
         this.CPR_SELECT.FieldName = "CPR_SELECT";
         this.CPR_SELECT.Name = "CPR_SELECT";
         this.CPR_SELECT.Visible = true;
         this.CPR_SELECT.VisibleIndex = 0;
         this.CPR_SELECT.Width = 59;
         // 
         // repositoryItemCheckEdit1
         // 
         this.repositoryItemCheckEdit1.AutoWidth = true;
         this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
         this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
         this.repositoryItemCheckEdit1.ValueChecked = "Y";
         this.repositoryItemCheckEdit1.ValueUnchecked = "N";
         // 
         // CPR_KIND_ID
         // 
         this.CPR_KIND_ID.AppearanceCell.Options.UseTextOptions = true;
         this.CPR_KIND_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.CPR_KIND_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.CPR_KIND_ID.AppearanceHeader.Options.UseBackColor = true;
         this.CPR_KIND_ID.AppearanceHeader.Options.UseTextOptions = true;
         this.CPR_KIND_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.CPR_KIND_ID.Caption = "契約名稱";
         this.CPR_KIND_ID.FieldName = "CPR_KIND_ID";
         this.CPR_KIND_ID.Name = "CPR_KIND_ID";
         this.CPR_KIND_ID.OptionsColumn.AllowEdit = false;
         this.CPR_KIND_ID.Visible = true;
         this.CPR_KIND_ID.VisibleIndex = 1;
         this.CPR_KIND_ID.Width = 110;
         // 
         // CPR_PRICE_RISK_RATE
         // 
         this.CPR_PRICE_RISK_RATE.AppearanceCell.BackColor = System.Drawing.Color.White;
         this.CPR_PRICE_RISK_RATE.AppearanceCell.Options.UseBackColor = true;
         this.CPR_PRICE_RISK_RATE.AppearanceCell.Options.UseTextOptions = true;
         this.CPR_PRICE_RISK_RATE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.CPR_PRICE_RISK_RATE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.CPR_PRICE_RISK_RATE.AppearanceHeader.Options.UseBackColor = true;
         this.CPR_PRICE_RISK_RATE.AppearanceHeader.Options.UseTextOptions = true;
         this.CPR_PRICE_RISK_RATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.CPR_PRICE_RISK_RATE.Caption = "最小風險價格係數";
         this.CPR_PRICE_RISK_RATE.ColumnEdit = this.repositoryItemTextEdit1;
         this.CPR_PRICE_RISK_RATE.DisplayFormat.FormatString = "P";
         this.CPR_PRICE_RISK_RATE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.CPR_PRICE_RISK_RATE.FieldName = "CPR_PRICE_RISK_RATE";
         this.CPR_PRICE_RISK_RATE.Name = "CPR_PRICE_RISK_RATE";
         this.CPR_PRICE_RISK_RATE.Visible = true;
         this.CPR_PRICE_RISK_RATE.VisibleIndex = 2;
         this.CPR_PRICE_RISK_RATE.Width = 145;
         // 
         // repositoryItemTextEdit1
         // 
         this.repositoryItemTextEdit1.AutoHeight = false;
         this.repositoryItemTextEdit1.DisplayFormat.FormatString = "#0.00";
         this.repositoryItemTextEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
         this.repositoryItemTextEdit1.EditFormat.FormatString = "#0.00";
         this.repositoryItemTextEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
         this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
         // 
         // RISK_INTERVAL
         // 
         this.RISK_INTERVAL.AppearanceCell.BackColor = System.Drawing.Color.White;
         this.RISK_INTERVAL.AppearanceCell.Options.UseBackColor = true;
         this.RISK_INTERVAL.AppearanceCell.Options.UseTextOptions = true;
         this.RISK_INTERVAL.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.RISK_INTERVAL.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.RISK_INTERVAL.AppearanceHeader.Options.UseBackColor = true;
         this.RISK_INTERVAL.AppearanceHeader.Options.UseTextOptions = true;
         this.RISK_INTERVAL.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.RISK_INTERVAL.Caption = "間距";
         this.RISK_INTERVAL.ColumnEdit = this.repositoryItemTextEdit2;
         this.RISK_INTERVAL.DisplayFormat.FormatString = "P";
         this.RISK_INTERVAL.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.RISK_INTERVAL.FieldName = "RISK_INTERVAL";
         this.RISK_INTERVAL.Name = "RISK_INTERVAL";
         this.RISK_INTERVAL.Visible = true;
         this.RISK_INTERVAL.VisibleIndex = 3;
         this.RISK_INTERVAL.Width = 73;
         // 
         // repositoryItemTextEdit2
         // 
         this.repositoryItemTextEdit2.AutoHeight = false;
         this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
         // 
         // CPR_EFFECTIVE_DATE
         // 
         this.CPR_EFFECTIVE_DATE.Caption = "最近一次調整日期";
         this.CPR_EFFECTIVE_DATE.FieldName = "CPR_EFFECTIVE_DATE";
         this.CPR_EFFECTIVE_DATE.Name = "CPR_EFFECTIVE_DATE";
         // 
         // LAST_RISK_RATE
         // 
         this.LAST_RISK_RATE.Caption = "最近一次修改前之最小風險價格係數";
         this.LAST_RISK_RATE.FieldName = "LAST_RISK_RATE";
         this.LAST_RISK_RATE.Name = "LAST_RISK_RATE";
         // 
         // CPR_PROD_SUBTYPE
         // 
         this.CPR_PROD_SUBTYPE.Caption = "CPR_PROD_SUBTYPE";
         this.CPR_PROD_SUBTYPE.FieldName = "CPR_PROD_SUBTYPE";
         this.CPR_PROD_SUBTYPE.Name = "CPR_PROD_SUBTYPE";
         // 
         // PROD_TYPE
         // 
         this.PROD_TYPE.Caption = "PROD_TYPE";
         this.PROD_TYPE.FieldName = "PROD_TYPE";
         this.PROD_TYPE.Name = "PROD_TYPE";
         // 
         // CPR_PARAM_KEY
         // 
         this.CPR_PARAM_KEY.Caption = "CPR_PARAM_KEY";
         this.CPR_PARAM_KEY.FieldName = "CPR_PARAM_KEY";
         this.CPR_PARAM_KEY.Name = "CPR_PARAM_KEY";
         // 
         // CPR_MODIFY_FLAG
         // 
         this.CPR_MODIFY_FLAG.Caption = "CPR_MODIFY_FLAG";
         this.CPR_MODIFY_FLAG.FieldName = "CPR_MODIFY_FLAG";
         this.CPR_MODIFY_FLAG.Name = "CPR_MODIFY_FLAG";
         // 
         // CPR_PRICE_RISK_RATE_ORG
         // 
         this.CPR_PRICE_RISK_RATE_ORG.Caption = "CPR_PRICE_RISK_RATE_ORG";
         this.CPR_PRICE_RISK_RATE_ORG.FieldName = "CPR_PRICE_RISK_RATE_ORG";
         this.CPR_PRICE_RISK_RATE_ORG.Name = "CPR_PRICE_RISK_RATE_ORG";
         // 
         // r_frame
         // 
         this.r_frame.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.r_frame.Appearance.Options.UseBackColor = true;
         this.r_frame.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.r_frame.Controls.Add(this.panFilter);
         this.r_frame.Location = new System.Drawing.Point(30, 30);
         this.r_frame.Name = "r_frame";
         this.r_frame.Size = new System.Drawing.Size(895, 711);
         this.r_frame.TabIndex = 78;
         // 
         // W48030
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(945, 783);
         this.Name = "W48030";
         this.Text = "W48030";
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panFirst.ResumeLayout(false);
         this.panFirst.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         this.panFilter.ResumeLayout(false);
         this.panFilter.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkModel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcDate)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvDate)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(repositoryItemCheckEdit2)).EndInit();
         this.panSecond.ResumeLayout(false);
         this.panSecond.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbxSubType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcKind)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvKind)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.r_frame)).EndInit();
         this.r_frame.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox panFirst;
        private System.Windows.Forms.Label labEndDate;
        private System.Windows.Forms.GroupBox panFilter;
        private System.Windows.Forms.GroupBox panSecond;
        private DevExpress.XtraGrid.GridControl gcKind;
        private DevExpress.XtraGrid.Views.Grid.GridView gvKind;
        private DevExpress.XtraGrid.Columns.GridColumn CPR_SELECT;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn CPR_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn CPR_PRICE_RISK_RATE;
        private System.Windows.Forms.Label labDateDesc;
        private DevExpress.XtraGrid.GridControl gcDate;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDate;
        private DevExpress.XtraGrid.Columns.GridColumn AI2_SELECT;
        private DevExpress.XtraGrid.Columns.GridColumn MON_DIFF;
        private DevExpress.XtraGrid.Columns.GridColumn SDATE;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnChooseAll;
        private System.Windows.Forms.Button btnFirstFilter;
        private System.Windows.Forms.Label labSubType;
        private DevExpress.XtraGrid.Columns.GridColumn EDATE;
        private DevExpress.XtraGrid.Columns.GridColumn DAY_CNT;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn RISK_INTERVAL;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn CPR_EFFECTIVE_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn LAST_RISK_RATE;
        private DevExpress.XtraGrid.Columns.GridColumn CPR_PROD_SUBTYPE;
        private DevExpress.XtraGrid.Columns.GridColumn PROD_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn CPR_PARAM_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn CPR_MODIFY_FLAG;
        private DevExpress.XtraGrid.Columns.GridColumn CPR_PRICE_RISK_RATE_ORG;
        private System.Windows.Forms.Label label1;
      private BaseGround.Widget.TextDateEdit txtEndDate;
      private System.Windows.Forms.GroupBox groupBox1;
      private DevExpress.XtraEditors.CheckedListBoxControl chkModel;
      private DevExpress.XtraEditors.LookUpEdit cbxSubType;
      private System.Windows.Forms.Label labMsg;
      private DevExpress.XtraEditors.PanelControl r_frame;
   }
}