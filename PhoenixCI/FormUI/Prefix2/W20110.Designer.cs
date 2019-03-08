namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20110 {
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbJpx = new System.Windows.Forms.Button();
            this.ddlType = new DevExpress.XtraEditors.LookUpEdit();
            this.lblType = new System.Windows.Forms.Label();
            this.txtDate = new PhoenixCI.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.AMIF_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_SETTLE_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_OPEN_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_HIGH_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_LOW_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_CLOSE_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_UP_DOWN_VAL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_M_QNTY_TAL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_OPEN_INTEREST = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_SUM_AMT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_CLOSE_PRICE_Y = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIF_EXCHANGE_RATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CP_ERR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPT_SEQ_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.R_OPEN_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.R_HIGH_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.R_M_QNTY_TAL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.R_LOW_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.R_CLOSE_PRICE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.R_UP_DOWN_VAL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.R_OPEN_INTEREST = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AMIFU_ERR_TEXT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddlType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Location = new System.Drawing.Point(0, 100);
            this.panParent.Size = new System.Drawing.Size(1170, 678);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1170, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cbJpx);
            this.panelControl1.Controls.Add(this.ddlType);
            this.panelControl1.Controls.Add(this.lblType);
            this.panelControl1.Controls.Add(this.txtDate);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1170, 70);
            this.panelControl1.TabIndex = 0;
            // 
            // cbJpx
            // 
            this.cbJpx.Location = new System.Drawing.Point(414, 21);
            this.cbJpx.Name = "cbJpx";
            this.cbJpx.Size = new System.Drawing.Size(173, 27);
            this.cbJpx.TabIndex = 12;
            this.cbJpx.Text = "手動下載JPX檔案檢核";
            this.cbJpx.UseVisualStyleBackColor = true;
            this.cbJpx.Click += new System.EventHandler(this.cbJpx_Click);
            // 
            // ddlType
            // 
            this.ddlType.Location = new System.Drawing.Point(298, 22);
            this.ddlType.MenuManager = this.ribbonControl;
            this.ddlType.Name = "ddlType";
            this.ddlType.Properties.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.ddlType.Properties.Appearance.Options.UseBackColor = true;
            this.ddlType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlType.Size = new System.Drawing.Size(100, 26);
            this.ddlType.TabIndex = 11;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(245, 25);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(57, 20);
            this.lblType.TabIndex = 10;
            this.lblType.Text = "盤別：";
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtDate.EditValue = "2018/12";
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(107, 22);
            this.txtDate.MenuManager = this.ribbonControl;
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDate.Size = new System.Drawing.Size(118, 26);
            this.txtDate.TabIndex = 8;
            this.txtDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(21, 25);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(89, 20);
            this.lblDate.TabIndex = 9;
            this.lblDate.Text = "交易日期：";
            // 
            // panelControl2
            // 
            this.panelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panelControl2.Appearance.Options.UseBackColor = true;
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 100);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1170, 678);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(12, 12);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1146, 654);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AMIF_KIND_ID,
            this.AMIF_SETTLE_DATE,
            this.AMIF_OPEN_PRICE,
            this.AMIF_HIGH_PRICE,
            this.AMIF_LOW_PRICE,
            this.AMIF_CLOSE_PRICE,
            this.AMIF_UP_DOWN_VAL,
            this.AMIF_M_QNTY_TAL,
            this.AMIF_OPEN_INTEREST,
            this.AMIF_SUM_AMT,
            this.AMIF_CLOSE_PRICE_Y,
            this.AMIF_EXCHANGE_RATE,
            this.CP_ERR,
            this.Is_NewRow,
            this.RPT_SEQ_NO,
            this.R_OPEN_PRICE,
            this.R_HIGH_PRICE,
            this.R_M_QNTY_TAL,
            this.R_LOW_PRICE,
            this.R_CLOSE_PRICE,
            this.R_UP_DOWN_VAL,
            this.R_OPEN_INTEREST,
            this.AMIFU_ERR_TEXT});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvMain_CellValueChanged);
            // 
            // AMIF_KIND_ID
            // 
            this.AMIF_KIND_ID.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.AMIF_KIND_ID.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_KIND_ID.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_KIND_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_KIND_ID.Caption = "商品";
            this.AMIF_KIND_ID.FieldName = "AMIF_KIND_ID";
            this.AMIF_KIND_ID.Name = "AMIF_KIND_ID";
            this.AMIF_KIND_ID.Visible = true;
            this.AMIF_KIND_ID.VisibleIndex = 0;
            // 
            // AMIF_SETTLE_DATE
            // 
            this.AMIF_SETTLE_DATE.AppearanceCell.Options.UseTextOptions = true;
            this.AMIF_SETTLE_DATE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_SETTLE_DATE.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.AMIF_SETTLE_DATE.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_SETTLE_DATE.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_SETTLE_DATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_SETTLE_DATE.Caption = "年月";
            this.AMIF_SETTLE_DATE.FieldName = "AMIF_SETTLE_DATE";
            this.AMIF_SETTLE_DATE.Name = "AMIF_SETTLE_DATE";
            this.AMIF_SETTLE_DATE.Visible = true;
            this.AMIF_SETTLE_DATE.VisibleIndex = 1;
            // 
            // AMIF_OPEN_PRICE
            // 
            this.AMIF_OPEN_PRICE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AMIF_OPEN_PRICE.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_OPEN_PRICE.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_OPEN_PRICE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_OPEN_PRICE.Caption = "開盤價";
            this.AMIF_OPEN_PRICE.FieldName = "AMIF_OPEN_PRICE";
            this.AMIF_OPEN_PRICE.Name = "AMIF_OPEN_PRICE";
            this.AMIF_OPEN_PRICE.Visible = true;
            this.AMIF_OPEN_PRICE.VisibleIndex = 2;
            this.AMIF_OPEN_PRICE.Width = 100;
            // 
            // AMIF_HIGH_PRICE
            // 
            this.AMIF_HIGH_PRICE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AMIF_HIGH_PRICE.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_HIGH_PRICE.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_HIGH_PRICE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_HIGH_PRICE.Caption = "最高";
            this.AMIF_HIGH_PRICE.FieldName = "AMIF_HIGH_PRICE";
            this.AMIF_HIGH_PRICE.Name = "AMIF_HIGH_PRICE";
            this.AMIF_HIGH_PRICE.Visible = true;
            this.AMIF_HIGH_PRICE.VisibleIndex = 3;
            this.AMIF_HIGH_PRICE.Width = 100;
            // 
            // AMIF_LOW_PRICE
            // 
            this.AMIF_LOW_PRICE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AMIF_LOW_PRICE.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_LOW_PRICE.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_LOW_PRICE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_LOW_PRICE.Caption = "最低";
            this.AMIF_LOW_PRICE.FieldName = "AMIF_LOW_PRICE";
            this.AMIF_LOW_PRICE.Name = "AMIF_LOW_PRICE";
            this.AMIF_LOW_PRICE.Visible = true;
            this.AMIF_LOW_PRICE.VisibleIndex = 4;
            this.AMIF_LOW_PRICE.Width = 100;
            // 
            // AMIF_CLOSE_PRICE
            // 
            this.AMIF_CLOSE_PRICE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AMIF_CLOSE_PRICE.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_CLOSE_PRICE.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_CLOSE_PRICE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_CLOSE_PRICE.Caption = "收盤價";
            this.AMIF_CLOSE_PRICE.FieldName = "AMIF_CLOSE_PRICE";
            this.AMIF_CLOSE_PRICE.Name = "AMIF_CLOSE_PRICE";
            this.AMIF_CLOSE_PRICE.Visible = true;
            this.AMIF_CLOSE_PRICE.VisibleIndex = 5;
            this.AMIF_CLOSE_PRICE.Width = 100;
            // 
            // AMIF_UP_DOWN_VAL
            // 
            this.AMIF_UP_DOWN_VAL.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AMIF_UP_DOWN_VAL.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_UP_DOWN_VAL.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_UP_DOWN_VAL.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_UP_DOWN_VAL.Caption = "漲跌(點)";
            this.AMIF_UP_DOWN_VAL.FieldName = "AMIF_UP_DOWN_VAL";
            this.AMIF_UP_DOWN_VAL.Name = "AMIF_UP_DOWN_VAL";
            this.AMIF_UP_DOWN_VAL.Visible = true;
            this.AMIF_UP_DOWN_VAL.VisibleIndex = 6;
            // 
            // AMIF_M_QNTY_TAL
            // 
            this.AMIF_M_QNTY_TAL.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AMIF_M_QNTY_TAL.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_M_QNTY_TAL.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_M_QNTY_TAL.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_M_QNTY_TAL.Caption = "交易量";
            this.AMIF_M_QNTY_TAL.FieldName = "AMIF_M_QNTY_TAL";
            this.AMIF_M_QNTY_TAL.Name = "AMIF_M_QNTY_TAL";
            this.AMIF_M_QNTY_TAL.Visible = true;
            this.AMIF_M_QNTY_TAL.VisibleIndex = 7;
            this.AMIF_M_QNTY_TAL.Width = 100;
            // 
            // AMIF_OPEN_INTEREST
            // 
            this.AMIF_OPEN_INTEREST.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AMIF_OPEN_INTEREST.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_OPEN_INTEREST.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_OPEN_INTEREST.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_OPEN_INTEREST.Caption = "未平倉量";
            this.AMIF_OPEN_INTEREST.FieldName = "AMIF_OPEN_INTEREST";
            this.AMIF_OPEN_INTEREST.Name = "AMIF_OPEN_INTEREST";
            this.AMIF_OPEN_INTEREST.Visible = true;
            this.AMIF_OPEN_INTEREST.VisibleIndex = 8;
            this.AMIF_OPEN_INTEREST.Width = 100;
            // 
            // AMIF_SUM_AMT
            // 
            this.AMIF_SUM_AMT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AMIF_SUM_AMT.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_SUM_AMT.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_SUM_AMT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_SUM_AMT.Caption = "成交值";
            this.AMIF_SUM_AMT.FieldName = "AMIF_SUM_AMT";
            this.AMIF_SUM_AMT.Name = "AMIF_SUM_AMT";
            this.AMIF_SUM_AMT.Visible = true;
            this.AMIF_SUM_AMT.VisibleIndex = 9;
            this.AMIF_SUM_AMT.Width = 100;
            // 
            // AMIF_CLOSE_PRICE_Y
            // 
            this.AMIF_CLOSE_PRICE_Y.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
            this.AMIF_CLOSE_PRICE_Y.AppearanceCell.Options.UseBackColor = true;
            this.AMIF_CLOSE_PRICE_Y.AppearanceHeader.BackColor = System.Drawing.Color.Transparent;
            this.AMIF_CLOSE_PRICE_Y.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_CLOSE_PRICE_Y.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_CLOSE_PRICE_Y.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_CLOSE_PRICE_Y.Caption = "前日收盤價";
            this.AMIF_CLOSE_PRICE_Y.FieldName = "AMIF_CLOSE_PRICE_Y";
            this.AMIF_CLOSE_PRICE_Y.Name = "AMIF_CLOSE_PRICE_Y";
            this.AMIF_CLOSE_PRICE_Y.Visible = true;
            this.AMIF_CLOSE_PRICE_Y.VisibleIndex = 10;
            this.AMIF_CLOSE_PRICE_Y.Width = 100;
            // 
            // AMIF_EXCHANGE_RATE
            // 
            this.AMIF_EXCHANGE_RATE.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
            this.AMIF_EXCHANGE_RATE.AppearanceCell.Options.UseBackColor = true;
            this.AMIF_EXCHANGE_RATE.AppearanceHeader.BackColor = System.Drawing.Color.Transparent;
            this.AMIF_EXCHANGE_RATE.AppearanceHeader.Options.UseBackColor = true;
            this.AMIF_EXCHANGE_RATE.AppearanceHeader.Options.UseTextOptions = true;
            this.AMIF_EXCHANGE_RATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AMIF_EXCHANGE_RATE.Caption = "匯率";
            this.AMIF_EXCHANGE_RATE.FieldName = "AMIF_EXCHANGE_RATE";
            this.AMIF_EXCHANGE_RATE.Name = "AMIF_EXCHANGE_RATE";
            this.AMIF_EXCHANGE_RATE.Visible = true;
            this.AMIF_EXCHANGE_RATE.VisibleIndex = 11;
            // 
            // CP_ERR
            // 
            this.CP_ERR.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
            this.CP_ERR.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.CP_ERR.AppearanceCell.Options.UseBackColor = true;
            this.CP_ERR.AppearanceCell.Options.UseForeColor = true;
            this.CP_ERR.Caption = "備註";
            this.CP_ERR.FieldName = "CP_ERR";
            this.CP_ERR.Name = "CP_ERR";
            this.CP_ERR.OptionsColumn.ShowCaption = false;
            this.CP_ERR.Visible = true;
            this.CP_ERR.VisibleIndex = 12;
            this.CP_ERR.Width = 1000;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // RPT_SEQ_NO
            // 
            this.RPT_SEQ_NO.Caption = "RPT_SEQ_NO";
            this.RPT_SEQ_NO.FieldName = "RPT_SEQ_NO";
            this.RPT_SEQ_NO.Name = "RPT_SEQ_NO";
            // 
            // R_OPEN_PRICE
            // 
            this.R_OPEN_PRICE.Caption = "R_OPEN_PRICE";
            this.R_OPEN_PRICE.FieldName = "R_OPEN_PRICE";
            this.R_OPEN_PRICE.Name = "R_OPEN_PRICE";
            // 
            // R_HIGH_PRICE
            // 
            this.R_HIGH_PRICE.Caption = "R_HIGH_PRICE";
            this.R_HIGH_PRICE.FieldName = "R_HIGH_PRICE";
            this.R_HIGH_PRICE.Name = "R_HIGH_PRICE";
            // 
            // R_M_QNTY_TAL
            // 
            this.R_M_QNTY_TAL.Caption = "R_M_QNTY_TAL";
            this.R_M_QNTY_TAL.FieldName = "R_M_QNTY_TAL";
            this.R_M_QNTY_TAL.Name = "R_M_QNTY_TAL";
            // 
            // R_LOW_PRICE
            // 
            this.R_LOW_PRICE.Caption = "R_LOW_PRICE";
            this.R_LOW_PRICE.FieldName = "R_LOW_PRICE";
            this.R_LOW_PRICE.Name = "R_LOW_PRICE";
            // 
            // R_CLOSE_PRICE
            // 
            this.R_CLOSE_PRICE.Caption = "R_CLOSE_PRICE";
            this.R_CLOSE_PRICE.FieldName = "R_CLOSE_PRICE";
            this.R_CLOSE_PRICE.Name = "R_CLOSE_PRICE";
            // 
            // R_UP_DOWN_VAL
            // 
            this.R_UP_DOWN_VAL.Caption = "R_UP_DOWN_VAL";
            this.R_UP_DOWN_VAL.FieldName = "R_UP_DOWN_VAL";
            this.R_UP_DOWN_VAL.Name = "R_UP_DOWN_VAL";
            // 
            // R_OPEN_INTEREST
            // 
            this.R_OPEN_INTEREST.Caption = "R_OPEN_INTEREST";
            this.R_OPEN_INTEREST.FieldName = "R_OPEN_INTEREST";
            this.R_OPEN_INTEREST.Name = "R_OPEN_INTEREST";
            // 
            // AMIFU_ERR_TEXT
            // 
            this.AMIFU_ERR_TEXT.Caption = "AMIFU_ERR_TEXT";
            this.AMIFU_ERR_TEXT.FieldName = "AMIFU_ERR_TEXT";
            this.AMIFU_ERR_TEXT.Name = "AMIFU_ERR_TEXT";
            // 
            // W20110
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 778);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20110";
            this.Text = "";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddlType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private Widget.TextDateEdit txtDate;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraEditors.LookUpEdit ddlType;
        private System.Windows.Forms.Label lblType;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_SETTLE_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_OPEN_PRICE;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_HIGH_PRICE;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_LOW_PRICE;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_CLOSE_PRICE;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_UP_DOWN_VAL;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_M_QNTY_TAL;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_OPEN_INTEREST;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_SUM_AMT;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_CLOSE_PRICE_Y;
        private DevExpress.XtraGrid.Columns.GridColumn AMIF_EXCHANGE_RATE;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
        private DevExpress.XtraGrid.Columns.GridColumn RPT_SEQ_NO;
        private DevExpress.XtraGrid.Columns.GridColumn R_OPEN_PRICE;
        private DevExpress.XtraGrid.Columns.GridColumn R_HIGH_PRICE;
        private DevExpress.XtraGrid.Columns.GridColumn R_M_QNTY_TAL;
        private DevExpress.XtraGrid.Columns.GridColumn R_LOW_PRICE;
        private DevExpress.XtraGrid.Columns.GridColumn R_CLOSE_PRICE;
        private DevExpress.XtraGrid.Columns.GridColumn R_UP_DOWN_VAL;
        private DevExpress.XtraGrid.Columns.GridColumn R_OPEN_INTEREST;
        private System.Windows.Forms.Button cbJpx;
        private DevExpress.XtraGrid.Columns.GridColumn CP_ERR;
        private DevExpress.XtraGrid.Columns.GridColumn AMIFU_ERR_TEXT;
    }
}