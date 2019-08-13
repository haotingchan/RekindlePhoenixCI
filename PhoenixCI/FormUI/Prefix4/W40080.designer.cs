namespace PhoenixCI.FormUI.Prefix4 {
   partial class W40080 {
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
         this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
         this.panel1 = new System.Windows.Forms.Panel();
         this.txtDate2 = new BaseGround.Widget.TextDateEdit();
         this.txtDate1 = new BaseGround.Widget.TextDateEdit();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.ddlAdjust = new DevExpress.XtraEditors.LookUpEdit();
         this.labG2 = new System.Windows.Forms.Label();
         this.labG1 = new System.Windows.Forms.Label();
         this.lblDate = new System.Windows.Forms.Label();
         this.txtTradeDate = new BaseGround.Widget.TextDateEdit();
         this.panel2 = new System.Windows.Forms.Panel();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.OSW_GRP = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_OSW_GRP = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.OP_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_KIND_ID1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_KIND_ID2 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_CHANGE_RANGE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP2_ADJ_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP2_VALUE_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_SEQ_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP2_SPAN_CODE_ORG = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP2_ADJ_CODE_ORG = new DevExpress.XtraGrid.Columns.GridColumn();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate2.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate1.Properties)).BeginInit();
         this.groupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ddlAdjust.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtTradeDate.Properties)).BeginInit();
         this.panel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(939, 624);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(939, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // repositoryItemDateEdit1
         // 
         this.repositoryItemDateEdit1.AutoHeight = false;
         this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemDateEdit1.DisplayFormat.FormatString = "yyyy/MM/dd";
         this.repositoryItemDateEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.repositoryItemDateEdit1.EditFormat.FormatString = "yyyy/MM/dd";
         this.repositoryItemDateEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.repositoryItemDateEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panel1.Controls.Add(this.txtDate2);
         this.panel1.Controls.Add(this.txtDate1);
         this.panel1.Controls.Add(this.groupBox2);
         this.panel1.Controls.Add(this.labG2);
         this.panel1.Controls.Add(this.labG1);
         this.panel1.Controls.Add(this.lblDate);
         this.panel1.Controls.Add(this.txtTradeDate);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 30);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(939, 95);
         this.panel1.TabIndex = 0;
         // 
         // txtDate2
         // 
         this.txtDate2.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate2.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate2.EditValue = "2018/12/01";
         this.txtDate2.EnterMoveNextControl = true;
         this.txtDate2.Location = new System.Drawing.Point(402, 62);
         this.txtDate2.MenuManager = this.ribbonControl;
         this.txtDate2.Name = "txtDate2";
         this.txtDate2.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate2.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate2.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate2.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate2.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate2.Size = new System.Drawing.Size(100, 26);
         this.txtDate2.TabIndex = 2;
         this.txtDate2.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtDate1
         // 
         this.txtDate1.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate1.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate1.EditValue = "2018/12/01";
         this.txtDate1.EnterMoveNextControl = true;
         this.txtDate1.Location = new System.Drawing.Point(402, 17);
         this.txtDate1.MenuManager = this.ribbonControl;
         this.txtDate1.Name = "txtDate1";
         this.txtDate1.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate1.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtDate1.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtDate1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtDate1.Properties.Mask.ShowPlaceHolders = false;
         this.txtDate1.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate1.Size = new System.Drawing.Size(100, 26);
         this.txtDate1.TabIndex = 1;
         this.txtDate1.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.ddlAdjust);
         this.groupBox2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
         this.groupBox2.ForeColor = System.Drawing.Color.Navy;
         this.groupBox2.Location = new System.Drawing.Point(570, 15);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(225, 70);
         this.groupBox2.TabIndex = 15;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "調整";
         // 
         // ddlAdjust
         // 
         this.ddlAdjust.Location = new System.Drawing.Point(34, 28);
         this.ddlAdjust.MenuManager = this.ribbonControl;
         this.ddlAdjust.Name = "ddlAdjust";
         this.ddlAdjust.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ddlAdjust.Properties.LookAndFeel.SkinName = "The Bezier";
         this.ddlAdjust.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ddlAdjust.Size = new System.Drawing.Size(159, 26);
         this.ddlAdjust.TabIndex = 3;
         this.ddlAdjust.EditValueChanged += new System.EventHandler(this.ddlAdjust_EditValueChanged);
         // 
         // labG2
         // 
         this.labG2.AutoSize = true;
         this.labG2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labG2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.labG2.Location = new System.Drawing.Point(249, 65);
         this.labG2.Name = "labG2";
         this.labG2.Size = new System.Drawing.Size(146, 20);
         this.labG2.TabIndex = 8;
         this.labG2.Text = "Group2生效日期：";
         // 
         // labG1
         // 
         this.labG1.AutoSize = true;
         this.labG1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.labG1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.labG1.Location = new System.Drawing.Point(249, 20);
         this.labG1.Name = "labG1";
         this.labG1.Size = new System.Drawing.Size(146, 20);
         this.labG1.TabIndex = 6;
         this.labG1.Text = "Group1生效日期：";
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.lblDate.Location = new System.Drawing.Point(15, 20);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(89, 20);
         this.lblDate.TabIndex = 4;
         this.lblDate.Text = "交易日期：";
         // 
         // txtTradeDate
         // 
         this.txtTradeDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtTradeDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtTradeDate.EditValue = "2018/12/01";
         this.txtTradeDate.EnterMoveNextControl = true;
         this.txtTradeDate.Location = new System.Drawing.Point(110, 17);
         this.txtTradeDate.MenuManager = this.ribbonControl;
         this.txtTradeDate.Name = "txtTradeDate";
         this.txtTradeDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtTradeDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtTradeDate.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtTradeDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtTradeDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtTradeDate.Properties.Mask.ShowPlaceHolders = false;
         this.txtTradeDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtTradeDate.Size = new System.Drawing.Size(100, 26);
         this.txtTradeDate.TabIndex = 0;
         this.txtTradeDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.gcMain);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(0, 125);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(939, 529);
         this.panel2.TabIndex = 3;
         // 
         // gcMain
         // 
         this.gcMain.Location = new System.Drawing.Point(0, -1);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(939, 530);
         this.gcMain.TabIndex = 1;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         this.gcMain.Visible = false;
         // 
         // gvMain
         // 
         this.gvMain.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gvMain.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gvMain.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gvMain.AppearancePrint.GroupRow.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
         this.gvMain.AppearancePrint.GroupRow.Options.UseFont = true;
         this.gvMain.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
         this.gvMain.AppearancePrint.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gvMain.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.OSW_GRP,
            this.SP1_OSW_GRP,
            this.SP1_DATE,
            this.SP1_TYPE,
            this.OP_TYPE,
            this.SP1_KIND_ID1,
            this.SP1_KIND_ID2,
            this.SP1_CHANGE_RANGE,
            this.SP2_ADJ_CODE,
            this.SP2_VALUE_DATE,
            this.SP1_SEQ_NO,
            this.SP2_SPAN_CODE_ORG,
            this.SP2_ADJ_CODE_ORG});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.AllowCellMerge = true;
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         this.gvMain.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvMain_CellValueChanging);
         this.gvMain.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvMain_CustomColumnDisplayText);
         // 
         // OSW_GRP
         // 
         this.OSW_GRP.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.OSW_GRP.AppearanceHeader.Options.UseBackColor = true;
         this.OSW_GRP.Caption = "群組";
         this.OSW_GRP.FieldName = "OSW_GRP";
         this.OSW_GRP.Name = "OSW_GRP";
         this.OSW_GRP.Visible = true;
         this.OSW_GRP.VisibleIndex = 7;
         // 
         // SP1_OSW_GRP
         // 
         this.SP1_OSW_GRP.Caption = "SP1_OSW_GRP";
         this.SP1_OSW_GRP.FieldName = "SP1_OSW_GRP";
         this.SP1_OSW_GRP.Name = "SP1_OSW_GRP";
         // 
         // SP1_DATE
         // 
         this.SP1_DATE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.SP1_DATE.AppearanceHeader.Options.UseBackColor = true;
         this.SP1_DATE.Caption = "日期";
         this.SP1_DATE.FieldName = "SP1_DATE";
         this.SP1_DATE.Name = "SP1_DATE";
         this.SP1_DATE.Visible = true;
         this.SP1_DATE.VisibleIndex = 0;
         this.SP1_DATE.Width = 30;
         // 
         // SP1_TYPE
         // 
         this.SP1_TYPE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.SP1_TYPE.AppearanceHeader.Options.UseBackColor = true;
         this.SP1_TYPE.Caption = "資料類別";
         this.SP1_TYPE.FieldName = "SP1_TYPE";
         this.SP1_TYPE.Name = "SP1_TYPE";
         this.SP1_TYPE.Visible = true;
         this.SP1_TYPE.VisibleIndex = 1;
         this.SP1_TYPE.Width = 50;
         // 
         // OP_TYPE
         // 
         this.OP_TYPE.Caption = "OP_TYPE";
         this.OP_TYPE.FieldName = "OP_TYPE";
         this.OP_TYPE.Name = "OP_TYPE";
         // 
         // SP1_KIND_ID1
         // 
         this.SP1_KIND_ID1.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.SP1_KIND_ID1.AppearanceHeader.Options.UseBackColor = true;
         this.SP1_KIND_ID1.Caption = "商品1";
         this.SP1_KIND_ID1.FieldName = "SP1_KIND_ID1";
         this.SP1_KIND_ID1.Name = "SP1_KIND_ID1";
         this.SP1_KIND_ID1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SP1_KIND_ID1.Visible = true;
         this.SP1_KIND_ID1.VisibleIndex = 2;
         this.SP1_KIND_ID1.Width = 30;
         // 
         // SP1_KIND_ID2
         // 
         this.SP1_KIND_ID2.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.SP1_KIND_ID2.AppearanceHeader.Options.UseBackColor = true;
         this.SP1_KIND_ID2.Caption = "商品2";
         this.SP1_KIND_ID2.FieldName = "SP1_KIND_ID2";
         this.SP1_KIND_ID2.Name = "SP1_KIND_ID2";
         this.SP1_KIND_ID2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SP1_KIND_ID2.Visible = true;
         this.SP1_KIND_ID2.VisibleIndex = 3;
         this.SP1_KIND_ID2.Width = 30;
         // 
         // SP1_CHANGE_RANGE
         // 
         this.SP1_CHANGE_RANGE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.SP1_CHANGE_RANGE.AppearanceHeader.Options.UseBackColor = true;
         this.SP1_CHANGE_RANGE.Caption = "變動幅度";
         this.SP1_CHANGE_RANGE.FieldName = "SP1_CHANGE_RANGE";
         this.SP1_CHANGE_RANGE.Name = "SP1_CHANGE_RANGE";
         this.SP1_CHANGE_RANGE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SP1_CHANGE_RANGE.Visible = true;
         this.SP1_CHANGE_RANGE.VisibleIndex = 4;
         // 
         // SP2_ADJ_CODE
         // 
         this.SP2_ADJ_CODE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.SP2_ADJ_CODE.AppearanceHeader.Options.UseBackColor = true;
         this.SP2_ADJ_CODE.Caption = " 觀察／調整";
         this.SP2_ADJ_CODE.FieldName = "SP2_ADJ_CODE";
         this.SP2_ADJ_CODE.Name = "SP2_ADJ_CODE";
         this.SP2_ADJ_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SP2_ADJ_CODE.Visible = true;
         this.SP2_ADJ_CODE.VisibleIndex = 5;
         // 
         // SP2_VALUE_DATE
         // 
         this.SP2_VALUE_DATE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.SP2_VALUE_DATE.AppearanceHeader.Options.UseBackColor = true;
         this.SP2_VALUE_DATE.Caption = "生效日期";
         this.SP2_VALUE_DATE.ColumnEdit = this.repositoryItemDateEdit1;
         this.SP2_VALUE_DATE.FieldName = "SP2_VALUE_DATE";
         this.SP2_VALUE_DATE.Name = "SP2_VALUE_DATE";
         this.SP2_VALUE_DATE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SP2_VALUE_DATE.Visible = true;
         this.SP2_VALUE_DATE.VisibleIndex = 6;
         // 
         // SP1_SEQ_NO
         // 
         this.SP1_SEQ_NO.Caption = "SP1_SEQ_NO";
         this.SP1_SEQ_NO.FieldName = "SP1_SEQ_NO";
         this.SP1_SEQ_NO.Name = "SP1_SEQ_NO";
         // 
         // SP2_SPAN_CODE_ORG
         // 
         this.SP2_SPAN_CODE_ORG.Caption = "SP2_SPAN_CODE_ORG";
         this.SP2_SPAN_CODE_ORG.FieldName = "SP2_SPAN_CODE_ORG";
         this.SP2_SPAN_CODE_ORG.Name = "SP2_SPAN_CODE_ORG";
         // 
         // SP2_ADJ_CODE_ORG
         // 
         this.SP2_ADJ_CODE_ORG.Caption = "SP2_ADJ_CODE_ORG";
         this.SP2_ADJ_CODE_ORG.FieldName = "SP2_ADJ_CODE_ORG";
         this.SP2_ADJ_CODE_ORG.Name = "SP2_ADJ_CODE_ORG";
         // 
         // W40080
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(939, 654);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Name = "W40080";
         this.Text = "W40080";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panel1, 0);
         this.Controls.SetChildIndex(this.panel2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate2.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate1.Properties)).EndInit();
         this.groupBox2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ddlAdjust.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtTradeDate.Properties)).EndInit();
         this.panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Label lblDate;
      private BaseGround.Widget.TextDateEdit txtTradeDate;
      private System.Windows.Forms.Label labG2;
      private BaseGround.Widget.TextDateEdit txtDate2;
      private System.Windows.Forms.Label labG1;
      private BaseGround.Widget.TextDateEdit txtDate1;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_OSW_GRP;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn OP_TYPE;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_KIND_ID1;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_KIND_ID2;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_CHANGE_RANGE;
      private DevExpress.XtraGrid.Columns.GridColumn SP2_ADJ_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_SEQ_NO;
      private DevExpress.XtraGrid.Columns.GridColumn SP2_SPAN_CODE_ORG;
      private DevExpress.XtraGrid.Columns.GridColumn SP2_ADJ_CODE_ORG;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_TYPE;
      private DevExpress.XtraGrid.Columns.GridColumn SP2_VALUE_DATE;
      private System.Windows.Forms.GroupBox groupBox2;
      private DevExpress.XtraEditors.LookUpEdit ddlAdjust;
      private DevExpress.XtraGrid.Columns.GridColumn OSW_GRP;
      private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
   }
}