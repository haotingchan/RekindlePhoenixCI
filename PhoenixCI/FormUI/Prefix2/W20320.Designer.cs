namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20320 {
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
         this.txtStartDate = new BaseGround.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.txtEndDate = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.CMC_YM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CMC_COUNT_G = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CMC_COUNT_P = new DevExpress.XtraGrid.Columns.GridColumn();
         this.CMC_COUNT_B = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AA1_TAIFEXEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager();
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
         this.panParent.Location = new System.Drawing.Point(0, 101);
         this.panParent.Size = new System.Drawing.Size(877, 526);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(877, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.txtStartDate);
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Controls.Add(this.txtEndDate);
         this.panelControl1.Controls.Add(this.lblDate);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(877, 71);
         this.panelControl1.TabIndex = 0;
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
         this.txtStartDate.Size = new System.Drawing.Size(118, 26);
         this.txtStartDate.TabIndex = 14;
         this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.BackColor = System.Drawing.Color.Transparent;
         this.label1.Location = new System.Drawing.Point(237, 25);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(25, 20);
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
         this.txtEndDate.Size = new System.Drawing.Size(118, 26);
         this.txtEndDate.TabIndex = 15;
         this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.BackColor = System.Drawing.Color.Transparent;
         this.lblDate.Location = new System.Drawing.Point(27, 25);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(89, 20);
         this.lblDate.TabIndex = 16;
         this.lblDate.Text = "交易年月：";
         // 
         // panelControl2
         // 
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 101);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(877, 526);
         this.panelControl2.TabIndex = 1;
         // 
         // gcMain
         // 
         this.gcMain.Location = new System.Drawing.Point(12, 12);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.AA1_TAIFEXEdit});
         this.gcMain.Size = new System.Drawing.Size(853, 502);
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
            this.CMC_YM,
            this.CMC_COUNT_G,
            this.CMC_COUNT_P,
            this.CMC_COUNT_B});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
         // 
         // CMC_YM
         // 
         this.CMC_YM.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.CMC_YM.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
         this.CMC_YM.AppearanceHeader.Options.UseBackColor = true;
         this.CMC_YM.AppearanceHeader.Options.UseBorderColor = true;
         this.CMC_YM.AppearanceHeader.Options.UseForeColor = true;
         this.CMC_YM.AppearanceHeader.Options.UseTextOptions = true;
         this.CMC_YM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.CMC_YM.Caption = "年月";
         this.CMC_YM.FieldName = "CMC_YM";
         this.CMC_YM.Name = "CMC_YM";
         this.CMC_YM.OptionsColumn.FixedWidth = true;
         this.CMC_YM.Visible = true;
         this.CMC_YM.VisibleIndex = 0;
         // 
         // CMC_COUNT_G
         // 
         this.CMC_COUNT_G.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
         this.CMC_COUNT_G.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
         this.CMC_COUNT_G.AppearanceHeader.Options.UseBackColor = true;
         this.CMC_COUNT_G.AppearanceHeader.Options.UseBorderColor = true;
         this.CMC_COUNT_G.AppearanceHeader.Options.UseTextOptions = true;
         this.CMC_COUNT_G.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.CMC_COUNT_G.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.CMC_COUNT_G.Caption = "一般家數";
         this.CMC_COUNT_G.DisplayFormat.FormatString = "0,00";
         this.CMC_COUNT_G.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
         this.CMC_COUNT_G.FieldName = "CMC_COUNT_G";
         this.CMC_COUNT_G.Name = "CMC_COUNT_G";
         this.CMC_COUNT_G.OptionsColumn.FixedWidth = true;
         this.CMC_COUNT_G.Visible = true;
         this.CMC_COUNT_G.VisibleIndex = 1;
         this.CMC_COUNT_G.Width = 160;
         // 
         // CMC_COUNT_P
         // 
         this.CMC_COUNT_P.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
         this.CMC_COUNT_P.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
         this.CMC_COUNT_P.AppearanceHeader.Options.UseBackColor = true;
         this.CMC_COUNT_P.AppearanceHeader.Options.UseBorderColor = true;
         this.CMC_COUNT_P.AppearanceHeader.Options.UseTextOptions = true;
         this.CMC_COUNT_P.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.CMC_COUNT_P.Caption = "個別家數";
         this.CMC_COUNT_P.DisplayFormat.FormatString = "0,00";
         this.CMC_COUNT_P.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
         this.CMC_COUNT_P.FieldName = "CMC_COUNT_P";
         this.CMC_COUNT_P.Name = "CMC_COUNT_P";
         this.CMC_COUNT_P.OptionsColumn.FixedWidth = true;
         this.CMC_COUNT_P.Visible = true;
         this.CMC_COUNT_P.VisibleIndex = 2;
         this.CMC_COUNT_P.Width = 160;
         // 
         // CMC_COUNT_B
         // 
         this.CMC_COUNT_B.AppearanceHeader.BackColor = System.Drawing.Color.Aqua;
         this.CMC_COUNT_B.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
         this.CMC_COUNT_B.AppearanceHeader.Options.UseBackColor = true;
         this.CMC_COUNT_B.AppearanceHeader.Options.UseBorderColor = true;
         this.CMC_COUNT_B.AppearanceHeader.Options.UseTextOptions = true;
         this.CMC_COUNT_B.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.CMC_COUNT_B.Caption = "銀行家數";
         this.CMC_COUNT_B.DisplayFormat.FormatString = "0,00";
         this.CMC_COUNT_B.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
         this.CMC_COUNT_B.FieldName = "CMC_COUNT_B";
         this.CMC_COUNT_B.Name = "CMC_COUNT_B";
         this.CMC_COUNT_B.OptionsColumn.FixedWidth = true;
         this.CMC_COUNT_B.Visible = true;
         this.CMC_COUNT_B.VisibleIndex = 3;
         this.CMC_COUNT_B.Width = 160;
         // 
         // AA1_TAIFEXEdit
         // 
         this.AA1_TAIFEXEdit.AutoHeight = false;
         this.AA1_TAIFEXEdit.Name = "AA1_TAIFEXEdit";
         // 
         // W20320
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(877, 627);
         this.Controls.Add(this.panelControl2);
         this.Controls.Add(this.panelControl1);
         this.Name = "W20320";
         this.Text = "W20320";
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
        private DevExpress.XtraGrid.Columns.GridColumn CMC_YM;
        private DevExpress.XtraGrid.Columns.GridColumn CMC_COUNT_G;
        private DevExpress.XtraGrid.Columns.GridColumn CMC_COUNT_P;
        private DevExpress.XtraGrid.Columns.GridColumn CMC_COUNT_B;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit AA1_TAIFEXEdit;
    }
}