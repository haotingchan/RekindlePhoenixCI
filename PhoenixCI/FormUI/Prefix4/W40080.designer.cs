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
         this.panel1 = new System.Windows.Forms.Panel();
         this.label4 = new System.Windows.Forms.Label();
         this.adjustmentRadioGroup = new DevExpress.XtraEditors.RadioGroup();
         this.label2 = new System.Windows.Forms.Label();
         this.txtDate2 = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.txtDate1 = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.txtTradeDate = new BaseGround.Widget.TextDateEdit();
         this.panel2 = new System.Windows.Forms.Panel();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.SP1_OSW_GRP = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CP_TYPE_SORT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_KIND_ID1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_KIND_ID2 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SP1_CHANGE_RANGE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.ADJ_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SPAN_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemCheckedComboBoxEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
         this.SP1_SEQ_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SPAN_CODE_ORG = new DevExpress.XtraGrid.Columns.GridColumn();
         this.ADJ_CODE_ORG = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
         this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.adjustmentRadioGroup.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate2.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate1.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtTradeDate.Properties)).BeginInit();
         this.panel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(939, 704);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(939, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panel1.Controls.Add(this.label4);
         this.panel1.Controls.Add(this.adjustmentRadioGroup);
         this.panel1.Controls.Add(this.label2);
         this.panel1.Controls.Add(this.txtDate2);
         this.panel1.Controls.Add(this.label1);
         this.panel1.Controls.Add(this.txtDate1);
         this.panel1.Controls.Add(this.lblDate);
         this.panel1.Controls.Add(this.txtTradeDate);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 30);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(939, 95);
         this.panel1.TabIndex = 0;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(519, 15);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(41, 20);
         this.label4.TabIndex = 22;
         this.label4.Text = "調整";
         // 
         // adjustmentRadioGroup
         // 
         this.adjustmentRadioGroup.EditValue = "Clear";
         this.adjustmentRadioGroup.Location = new System.Drawing.Point(566, 15);
         this.adjustmentRadioGroup.Name = "adjustmentRadioGroup";
         this.adjustmentRadioGroup.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.adjustmentRadioGroup.Properties.Appearance.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.adjustmentRadioGroup.Properties.Appearance.Options.UseBackColor = true;
         this.adjustmentRadioGroup.Properties.Appearance.Options.UseFont = true;
         this.adjustmentRadioGroup.Properties.Columns = 3;
         this.adjustmentRadioGroup.Properties.ItemHorzAlignment = DevExpress.XtraEditors.RadioItemHorzAlignment.Near;
         this.adjustmentRadioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Clear", "全取消"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("AllSelect", "全選"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "全選Group 1"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Index", "全選指數類"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("StcEtc", "全選STC,ETC"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "全選Group 2")});
         this.adjustmentRadioGroup.Properties.ItemsLayout = DevExpress.XtraEditors.RadioGroupItemsLayout.Flow;
         this.adjustmentRadioGroup.Size = new System.Drawing.Size(349, 65);
         this.adjustmentRadioGroup.TabIndex = 21;
         this.adjustmentRadioGroup.SelectedIndexChanged += new System.EventHandler(this.adjustmentRadioGroup_SelectedIndexChanged_1);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label2.Location = new System.Drawing.Point(212, 44);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(130, 20);
         this.label2.TabIndex = 8;
         this.label2.Text = "Group2生效日期";
         // 
         // txtDate2
         // 
         this.txtDate2.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate2.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate2.EditValue = "2018/12/01";
         this.txtDate2.EnterMoveNextControl = true;
         this.txtDate2.Location = new System.Drawing.Point(348, 41);
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
         this.txtDate2.TabIndex = 7;
         this.txtDate2.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.label1.Location = new System.Drawing.Point(212, 12);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(130, 20);
         this.label1.TabIndex = 6;
         this.label1.Text = "Group1生效日期";
         // 
         // txtDate1
         // 
         this.txtDate1.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate1.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtDate1.EditValue = "2018/12/01";
         this.txtDate1.EnterMoveNextControl = true;
         this.txtDate1.Location = new System.Drawing.Point(348, 9);
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
         this.txtDate1.TabIndex = 5;
         this.txtDate1.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
         this.lblDate.Location = new System.Drawing.Point(15, 12);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(73, 20);
         this.lblDate.TabIndex = 4;
         this.lblDate.Text = "交易日期";
         // 
         // txtTradeDate
         // 
         this.txtTradeDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtTradeDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtTradeDate.EditValue = "2018/12/01";
         this.txtTradeDate.EnterMoveNextControl = true;
         this.txtTradeDate.Location = new System.Drawing.Point(95, 9);
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
         this.txtTradeDate.TabIndex = 3;
         this.txtTradeDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // panel2
         // 
         this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(220)))), ((int)(((byte)(192)))));
         this.panel2.Controls.Add(this.gcMain);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(0, 125);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(939, 609);
         this.panel2.TabIndex = 3;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(0, 0);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemTextEdit1,
            this.repositoryItemCheckedComboBoxEdit1});
         this.gcMain.Size = new System.Drawing.Size(939, 609);
         this.gcMain.TabIndex = 1;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
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
            this.SP1_OSW_GRP,
            this.SP1_DATE,
            this.SP1_TYPE,
            this.CP_TYPE_SORT,
            this.SP1_KIND_ID1,
            this.SP1_KIND_ID2,
            this.SP1_CHANGE_RANGE,
            this.ADJ_CODE,
            this.SPAN_CODE,
            this.SP1_SEQ_NO,
            this.SPAN_CODE_ORG,
            this.ADJ_CODE_ORG});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.AllowCellMerge = true;
         this.gvMain.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvMain_CustomColumnDisplayText);
         // 
         // SP1_OSW_GRP
         // 
         this.SP1_OSW_GRP.Caption = "群組";
         this.SP1_OSW_GRP.FieldName = "SP1_OSW_GRP";
         this.SP1_OSW_GRP.Name = "SP1_OSW_GRP";
         this.SP1_OSW_GRP.Visible = true;
         this.SP1_OSW_GRP.VisibleIndex = 0;
         // 
         // SP1_DATE
         // 
         this.SP1_DATE.Caption = "日期";
         this.SP1_DATE.FieldName = "SP1_DATE";
         this.SP1_DATE.Name = "SP1_DATE";
         this.SP1_DATE.Visible = true;
         this.SP1_DATE.VisibleIndex = 1;
         // 
         // SP1_TYPE
         // 
         this.SP1_TYPE.Caption = "資料類別";
         this.SP1_TYPE.FieldName = "SP1_TYPE";
         this.SP1_TYPE.Name = "SP1_TYPE";
         this.SP1_TYPE.Visible = true;
         this.SP1_TYPE.VisibleIndex = 2;
         // 
         // CP_TYPE_SORT
         // 
         this.CP_TYPE_SORT.Caption = "CP_TYPE_SORT";
         this.CP_TYPE_SORT.FieldName = "CP_TYPE_SORT";
         this.CP_TYPE_SORT.Name = "CP_TYPE_SORT";
         // 
         // SP1_KIND_ID1
         // 
         this.SP1_KIND_ID1.Caption = "商品1";
         this.SP1_KIND_ID1.FieldName = "SP1_KIND_ID1";
         this.SP1_KIND_ID1.Name = "SP1_KIND_ID1";
         this.SP1_KIND_ID1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SP1_KIND_ID1.Visible = true;
         this.SP1_KIND_ID1.VisibleIndex = 3;
         // 
         // SP1_KIND_ID2
         // 
         this.SP1_KIND_ID2.Caption = "商品2";
         this.SP1_KIND_ID2.FieldName = "SP1_KIND_ID2";
         this.SP1_KIND_ID2.Name = "SP1_KIND_ID2";
         this.SP1_KIND_ID2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SP1_KIND_ID2.Visible = true;
         this.SP1_KIND_ID2.VisibleIndex = 4;
         // 
         // SP1_CHANGE_RANGE
         // 
         this.SP1_CHANGE_RANGE.Caption = "變動幅度";
         this.SP1_CHANGE_RANGE.FieldName = "SP1_CHANGE_RANGE";
         this.SP1_CHANGE_RANGE.Name = "SP1_CHANGE_RANGE";
         this.SP1_CHANGE_RANGE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SP1_CHANGE_RANGE.Visible = true;
         this.SP1_CHANGE_RANGE.VisibleIndex = 5;
         // 
         // ADJ_CODE
         // 
         this.ADJ_CODE.Caption = " 觀察  ／ 調整";
         this.ADJ_CODE.FieldName = "ADJ_CODE";
         this.ADJ_CODE.Name = "ADJ_CODE";
         this.ADJ_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.ADJ_CODE.Visible = true;
         this.ADJ_CODE.VisibleIndex = 6;
         // 
         // SPAN_CODE
         // 
         this.SPAN_CODE.Caption = "SPAN調整";
         this.SPAN_CODE.ColumnEdit = this.repositoryItemCheckEdit1;
         this.SPAN_CODE.FieldName = "SPAN_CODE";
         this.SPAN_CODE.Name = "SPAN_CODE";
         this.SPAN_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
         this.SPAN_CODE.Visible = true;
         this.SPAN_CODE.VisibleIndex = 7;
         // 
         // repositoryItemCheckedComboBoxEdit1
         // 
         this.repositoryItemCheckedComboBoxEdit1.AutoHeight = false;
         this.repositoryItemCheckedComboBoxEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemCheckedComboBoxEdit1.Name = "repositoryItemCheckedComboBoxEdit1";
         // 
         // SP1_SEQ_NO
         // 
         this.SP1_SEQ_NO.Caption = "SP1_SEQ_NO";
         this.SP1_SEQ_NO.FieldName = "SP1_SEQ_NO";
         this.SP1_SEQ_NO.Name = "SP1_SEQ_NO";
         // 
         // SPAN_CODE_ORG
         // 
         this.SPAN_CODE_ORG.Caption = "SPAN_CODE_ORG";
         this.SPAN_CODE_ORG.FieldName = "SPAN_CODE_ORG";
         this.SPAN_CODE_ORG.Name = "SPAN_CODE_ORG";
         // 
         // ADJ_CODE_ORG
         // 
         this.ADJ_CODE_ORG.Caption = "ADJ_CODE_ORG";
         this.ADJ_CODE_ORG.FieldName = "ADJ_CODE_ORG";
         this.ADJ_CODE_ORG.Name = "ADJ_CODE_ORG";
         // 
         // repositoryItemCheckEdit1
         // 
         this.repositoryItemCheckEdit1.AutoHeight = false;
         this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
         this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
         this.repositoryItemCheckEdit1.ValueChecked = "Y";
         this.repositoryItemCheckEdit1.ValueUnchecked = " ";
         // 
         // repositoryItemTextEdit1
         // 
         this.repositoryItemTextEdit1.AutoHeight = false;
         this.repositoryItemTextEdit1.Mask.EditMask = "n";
         this.repositoryItemTextEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
         // 
         // W40080
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(939, 734);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Name = "W40080";
         this.Text = "W40080";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panel1, 0);
         this.Controls.SetChildIndex(this.panel2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.adjustmentRadioGroup.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate2.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate1.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtTradeDate.Properties)).EndInit();
         this.panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Label lblDate;
      private BaseGround.Widget.TextDateEdit txtTradeDate;
      private System.Windows.Forms.Label label2;
      private BaseGround.Widget.TextDateEdit txtDate2;
      private System.Windows.Forms.Label label1;
      private BaseGround.Widget.TextDateEdit txtDate1;
      private System.Windows.Forms.Label label4;
      private DevExpress.XtraEditors.RadioGroup adjustmentRadioGroup;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_OSW_GRP;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn CP_TYPE_SORT;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_KIND_ID1;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_KIND_ID2;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_CHANGE_RANGE;
      private DevExpress.XtraGrid.Columns.GridColumn ADJ_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn SPAN_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_SEQ_NO;
      private DevExpress.XtraGrid.Columns.GridColumn SPAN_CODE_ORG;
      private DevExpress.XtraGrid.Columns.GridColumn ADJ_CODE_ORG;
      private DevExpress.XtraGrid.Columns.GridColumn SP1_TYPE;
      private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit1;
   }
}