namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20130 {
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
            this.txtStartDate = new PhoenixCI.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEndDate = new PhoenixCI.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.AM12_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.AM12_F_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM12_VOL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM12_STATUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM12_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM12_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM12_DATA_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Location = new System.Drawing.Point(0, 100);
            this.panParent.Size = new System.Drawing.Size(1018, 418);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1018, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            this.panelControl1.Appearance.BackColor2 = System.Drawing.Color.SkyBlue;
            this.panelControl1.Appearance.BorderColor = System.Drawing.Color.Gray;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Appearance.Options.UseBorderColor = true;
            this.panelControl1.Controls.Add(this.txtStartDate);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.txtEndDate);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1018, 70);
            this.panelControl1.TabIndex = 0;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStartDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtStartDate.EditValue = "2018/12";
            this.txtStartDate.EnterMoveNextControl = true;
            this.txtStartDate.Location = new System.Drawing.Point(105, 21);
            this.txtStartDate.MenuManager = this.ribbonControl;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(118, 26);
            this.txtStartDate.TabIndex = 1;
            this.txtStartDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(229, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "～";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEndDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtEndDate.EditValue = "2018/12";
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(260, 21);
            this.txtEndDate.MenuManager = this.ribbonControl;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(118, 26);
            this.txtEndDate.TabIndex = 2;
            this.txtEndDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(19, 24);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(89, 20);
            this.lblDate.TabIndex = 7;
            this.lblDate.Text = "交易日期：";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 100);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1018, 418);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(12, 12);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.gcMain.Size = new System.Drawing.Size(994, 394);
            this.gcMain.TabIndex = 3;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AM12_YMD,
            this.AM12_F_ID,
            this.AM12_VOL,
            this.AM12_STATUS,
            this.Is_NewRow,
            this.AM12_W_USER_ID,
            this.AM12_KIND_ID,
            this.AM12_DATA_TYPE});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // AM12_YMD
            // 
            this.AM12_YMD.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.AM12_YMD.AppearanceCell.Options.UseBackColor = true;
            this.AM12_YMD.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.AM12_YMD.AppearanceHeader.Options.UseBackColor = true;
            this.AM12_YMD.Caption = "日期";
            this.AM12_YMD.ColumnEdit = this.repositoryItemTextEdit1;
            this.AM12_YMD.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.AM12_YMD.FieldName = "AM12_YMD";
            this.AM12_YMD.MaxWidth = 108;
            this.AM12_YMD.Name = "AM12_YMD";
            this.AM12_YMD.OptionsColumn.FixedWidth = true;
            this.AM12_YMD.Visible = true;
            this.AM12_YMD.VisibleIndex = 0;
            this.AM12_YMD.Width = 108;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.MaxLength = 8;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // AM12_F_ID
            // 
            this.AM12_F_ID.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.AM12_F_ID.AppearanceCell.Options.UseBackColor = true;
            this.AM12_F_ID.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.AM12_F_ID.AppearanceHeader.Options.UseBackColor = true;
            this.AM12_F_ID.Caption = "交易所";
            this.AM12_F_ID.FieldName = "AM12_F_ID";
            this.AM12_F_ID.MaxWidth = 115;
            this.AM12_F_ID.Name = "AM12_F_ID";
            this.AM12_F_ID.OptionsColumn.FixedWidth = true;
            this.AM12_F_ID.Visible = true;
            this.AM12_F_ID.VisibleIndex = 1;
            this.AM12_F_ID.Width = 115;
            // 
            // AM12_VOL
            // 
            this.AM12_VOL.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.AM12_VOL.AppearanceCell.Options.UseBackColor = true;
            this.AM12_VOL.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
            this.AM12_VOL.AppearanceHeader.Options.UseBackColor = true;
            this.AM12_VOL.Caption = "成交量";
            this.AM12_VOL.DisplayFormat.FormatString = "0,0";
            this.AM12_VOL.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.AM12_VOL.FieldName = "AM12_VOL";
            this.AM12_VOL.MaxWidth = 133;
            this.AM12_VOL.Name = "AM12_VOL";
            this.AM12_VOL.OptionsColumn.FixedWidth = true;
            this.AM12_VOL.Visible = true;
            this.AM12_VOL.VisibleIndex = 2;
            this.AM12_VOL.Width = 133;
            // 
            // AM12_STATUS
            // 
            this.AM12_STATUS.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.AM12_STATUS.AppearanceCell.Options.UseBackColor = true;
            this.AM12_STATUS.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
            this.AM12_STATUS.AppearanceHeader.Options.UseBackColor = true;
            this.AM12_STATUS.Caption = "資料狀態";
            this.AM12_STATUS.FieldName = "AM12_STATUS";
            this.AM12_STATUS.MaxWidth = 360;
            this.AM12_STATUS.Name = "AM12_STATUS";
            this.AM12_STATUS.OptionsColumn.AllowEdit = false;
            this.AM12_STATUS.OptionsColumn.FixedWidth = true;
            this.AM12_STATUS.Visible = true;
            this.AM12_STATUS.VisibleIndex = 3;
            this.AM12_STATUS.Width = 360;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            this.Is_NewRow.OptionsColumn.AllowEdit = false;
            // 
            // AM12_W_USER_ID
            // 
            this.AM12_W_USER_ID.Caption = "AM12_W_USER_ID";
            this.AM12_W_USER_ID.FieldName = "AM12_W_USER_ID";
            this.AM12_W_USER_ID.Name = "AM12_W_USER_ID";
            // 
            // AM12_KIND_ID
            // 
            this.AM12_KIND_ID.Caption = "AM12_KIND_ID";
            this.AM12_KIND_ID.FieldName = "AM12_KIND_ID";
            this.AM12_KIND_ID.Name = "AM12_KIND_ID";
            // 
            // AM12_DATA_TYPE
            // 
            this.AM12_DATA_TYPE.Caption = "AM12_DATA_TYPE";
            this.AM12_DATA_TYPE.FieldName = "AM12_DATA_TYPE";
            this.AM12_DATA_TYPE.Name = "AM12_DATA_TYPE";
            // 
            // W20130
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 518);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20130";
            this.Text = "W20130";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraGrid.Columns.GridColumn AM12_YMD;
        private DevExpress.XtraGrid.Columns.GridColumn AM12_F_ID;
        private DevExpress.XtraGrid.Columns.GridColumn AM12_VOL;
        private DevExpress.XtraGrid.Columns.GridColumn AM12_STATUS;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
        private System.Windows.Forms.Label label1;
        private Widget.TextDateEdit txtEndDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn AM12_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn AM12_DATA_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn AM12_W_USER_ID;
    }
}