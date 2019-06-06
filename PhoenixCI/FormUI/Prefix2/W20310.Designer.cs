namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20310 {
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
            this.components = new System.ComponentModel.Container();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblNote = new System.Windows.Forms.Label();
            this.txtStartDate = new BaseGround.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEndDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.AA1_YM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AA1_TAIFEX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AA1_TSE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AA1_OTC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AA1_DAY_COUNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AA1_US_RATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AA1_SGX_DT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AA1_TAIFEXEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AA1_TAIFEXEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Location = new System.Drawing.Point(0, 114);
            this.panParent.Size = new System.Drawing.Size(877, 513);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(877, 43);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.lblNote);
            this.panelControl1.Controls.Add(this.txtStartDate);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.txtEndDate);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 43);
            this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(877, 71);
            this.panelControl1.TabIndex = 0;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.ForeColor = System.Drawing.Color.Maroon;
            this.lblNote.Location = new System.Drawing.Point(403, 25);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(583, 30);
            this.lblNote.TabIndex = 18;
            this.lblNote.Text = "註：Taifex期貨總成交值，並不含國外指數期貨商品。";
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtStartDate.EditValue = "2018/12";
            this.txtStartDate.EnterMoveNextControl = true;
            this.txtStartDate.Location = new System.Drawing.Point(113, 22);
            this.txtStartDate.MenuManager = this.ribbonControl;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartDate.Properties.EditFormat.FormatString = "yyyyMM";
            this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(118, 40);
            this.txtStartDate.TabIndex = 14;
            this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(237, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 30);
            this.label1.TabIndex = 17;
            this.label1.Text = "～";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtEndDate.EditValue = "2018/12";
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(268, 22);
            this.txtEndDate.MenuManager = this.ribbonControl;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndDate.Properties.EditFormat.FormatString = "yyyyMM";
            this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(118, 40);
            this.txtEndDate.TabIndex = 15;
            this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(27, 25);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(133, 30);
            this.lblDate.TabIndex = 16;
            this.lblDate.Text = "交易日期：";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 114);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(877, 513);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(18, 18);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.AA1_TAIFEXEdit});
            this.gcMain.Size = new System.Drawing.Size(841, 477);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvMain.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvMain.Appearance.HeaderPanel.TextOptions.Trimming = DevExpress.Utils.Trimming.None;
            this.gvMain.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gvMain.Appearance.ViewCaption.Options.UseForeColor = true;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AA1_YM,
            this.AA1_TAIFEX,
            this.AA1_TSE,
            this.AA1_OTC,
            this.AA1_DAY_COUNT,
            this.AA1_US_RATE,
            this.AA1_SGX_DT,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // AA1_YM
            // 
            this.AA1_YM.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.AA1_YM.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.AA1_YM.AppearanceHeader.Options.UseBackColor = true;
            this.AA1_YM.AppearanceHeader.Options.UseBorderColor = true;
            this.AA1_YM.AppearanceHeader.Options.UseForeColor = true;
            this.AA1_YM.AppearanceHeader.Options.UseTextOptions = true;
            this.AA1_YM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AA1_YM.Caption = "年月";
            this.AA1_YM.FieldName = "AA1_YM";
            this.AA1_YM.Name = "AA1_YM";
            this.AA1_YM.OptionsColumn.FixedWidth = true;
            this.AA1_YM.Visible = true;
            this.AA1_YM.VisibleIndex = 0;
            // 
            // AA1_TAIFEX
            // 
            this.AA1_TAIFEX.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AA1_TAIFEX.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.AA1_TAIFEX.AppearanceHeader.Options.UseBackColor = true;
            this.AA1_TAIFEX.AppearanceHeader.Options.UseBorderColor = true;
            this.AA1_TAIFEX.AppearanceHeader.Options.UseTextOptions = true;
            this.AA1_TAIFEX.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AA1_TAIFEX.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.AA1_TAIFEX.Caption = "Taifex";
            this.AA1_TAIFEX.DisplayFormat.FormatString = "0,00";
            this.AA1_TAIFEX.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.AA1_TAIFEX.FieldName = "AA1_TAIFEX";
            this.AA1_TAIFEX.Name = "AA1_TAIFEX";
            this.AA1_TAIFEX.OptionsColumn.FixedWidth = true;
            this.AA1_TAIFEX.Visible = true;
            this.AA1_TAIFEX.VisibleIndex = 1;
            this.AA1_TAIFEX.Width = 160;
            // 
            // AA1_TSE
            // 
            this.AA1_TSE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AA1_TSE.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.AA1_TSE.AppearanceHeader.Options.UseBackColor = true;
            this.AA1_TSE.AppearanceHeader.Options.UseBorderColor = true;
            this.AA1_TSE.AppearanceHeader.Options.UseTextOptions = true;
            this.AA1_TSE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AA1_TSE.Caption = "TWSE";
            this.AA1_TSE.DisplayFormat.FormatString = "0,00";
            this.AA1_TSE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.AA1_TSE.FieldName = "AA1_TSE";
            this.AA1_TSE.Name = "AA1_TSE";
            this.AA1_TSE.OptionsColumn.FixedWidth = true;
            this.AA1_TSE.Visible = true;
            this.AA1_TSE.VisibleIndex = 2;
            this.AA1_TSE.Width = 160;
            // 
            // AA1_OTC
            // 
            this.AA1_OTC.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AA1_OTC.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.AA1_OTC.AppearanceHeader.Options.UseBackColor = true;
            this.AA1_OTC.AppearanceHeader.Options.UseBorderColor = true;
            this.AA1_OTC.AppearanceHeader.Options.UseTextOptions = true;
            this.AA1_OTC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AA1_OTC.Caption = "OTC";
            this.AA1_OTC.DisplayFormat.FormatString = "0,00";
            this.AA1_OTC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.AA1_OTC.FieldName = "AA1_OTC";
            this.AA1_OTC.Name = "AA1_OTC";
            this.AA1_OTC.OptionsColumn.FixedWidth = true;
            this.AA1_OTC.Visible = true;
            this.AA1_OTC.VisibleIndex = 3;
            this.AA1_OTC.Width = 160;
            // 
            // AA1_DAY_COUNT
            // 
            this.AA1_DAY_COUNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AA1_DAY_COUNT.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.AA1_DAY_COUNT.AppearanceHeader.Options.UseBackColor = true;
            this.AA1_DAY_COUNT.AppearanceHeader.Options.UseBorderColor = true;
            this.AA1_DAY_COUNT.AppearanceHeader.Options.UseTextOptions = true;
            this.AA1_DAY_COUNT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AA1_DAY_COUNT.Caption = "月交易天數";
            this.AA1_DAY_COUNT.FieldName = "AA1_DAY_COUNT";
            this.AA1_DAY_COUNT.Name = "AA1_DAY_COUNT";
            this.AA1_DAY_COUNT.OptionsColumn.FixedWidth = true;
            this.AA1_DAY_COUNT.Visible = true;
            this.AA1_DAY_COUNT.VisibleIndex = 4;
            this.AA1_DAY_COUNT.Width = 116;
            // 
            // AA1_US_RATE
            // 
            this.AA1_US_RATE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AA1_US_RATE.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.AA1_US_RATE.AppearanceHeader.Options.UseBackColor = true;
            this.AA1_US_RATE.AppearanceHeader.Options.UseBorderColor = true;
            this.AA1_US_RATE.AppearanceHeader.Options.UseTextOptions = true;
            this.AA1_US_RATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AA1_US_RATE.Caption = "美元匯率";
            this.AA1_US_RATE.FieldName = "AA1_US_RATE";
            this.AA1_US_RATE.Name = "AA1_US_RATE";
            this.AA1_US_RATE.OptionsColumn.FixedWidth = true;
            this.AA1_US_RATE.Visible = true;
            this.AA1_US_RATE.VisibleIndex = 5;
            this.AA1_US_RATE.Width = 123;
            // 
            // AA1_SGX_DT
            // 
            this.AA1_SGX_DT.Caption = "SGX-DT(2005.10 起不用)";
            this.AA1_SGX_DT.FieldName = "AA1_SGX_DT";
            this.AA1_SGX_DT.Name = "AA1_SGX_DT";
            this.AA1_SGX_DT.OptionsColumn.FixedWidth = true;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            this.Is_NewRow.OptionsColumn.FixedWidth = true;
            // 
            // AA1_TAIFEXEdit
            // 
            this.AA1_TAIFEXEdit.AutoHeight = false;
            this.AA1_TAIFEXEdit.Name = "AA1_TAIFEXEdit";
            // 
            // W20310
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 627);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20310";
            this.Text = "W20310";
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
            ((System.ComponentModel.ISupportInitialize)(this.AA1_TAIFEXEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private BaseGround.Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label label1;
        private BaseGround.Widget.TextDateEdit txtEndDate;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraGrid.Columns.GridColumn AA1_YM;
        private DevExpress.XtraGrid.Columns.GridColumn AA1_TAIFEX;
        private DevExpress.XtraGrid.Columns.GridColumn AA1_TSE;
        private DevExpress.XtraGrid.Columns.GridColumn AA1_OTC;
        private DevExpress.XtraGrid.Columns.GridColumn AA1_DAY_COUNT;
        private DevExpress.XtraGrid.Columns.GridColumn AA1_US_RATE;
        private DevExpress.XtraGrid.Columns.GridColumn AA1_SGX_DT;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
        private System.Windows.Forms.Label lblNote;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit AA1_TAIFEXEdit;
    }
}