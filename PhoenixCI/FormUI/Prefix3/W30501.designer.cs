namespace PhoenixCI.FormUI.Prefix3
{
    partial class W30501 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.labTime = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.lblDate = new System.Windows.Forms.Label();
         this.txtEDate = new PhoenixCI.Widget.TextDateEdit();
         this.txtSDate = new PhoenixCI.Widget.TextDateEdit();
         this.ExportShow = new System.Windows.Forms.Label();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
         this.PROD_ID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.APDK_NAME = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.B_WEIGHT_QNTY = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.S_WEIGHT_QNTY = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.B_MAX_QNTY = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.B_MAX_SEC = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.S_MAX_QNTY = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.S_MAX_SEC = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand5 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand9 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand10 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand6 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand11 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand12 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         this.panelControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Size = new System.Drawing.Size(813, 694);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(813, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.labTime);
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Controls.Add(this.lblDate);
         this.panelControl1.Controls.Add(this.txtEDate);
         this.panelControl1.Controls.Add(this.txtSDate);
         this.panelControl1.Controls.Add(this.ExportShow);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(813, 143);
         this.panelControl1.TabIndex = 0;
         // 
         // labTime
         // 
         this.labTime.AutoSize = true;
         this.labTime.Location = new System.Drawing.Point(486, 120);
         this.labTime.Name = "labTime";
         this.labTime.Size = new System.Drawing.Size(54, 20);
         this.labTime.TabIndex = 19;
         this.labTime.Text = "label2";
         this.labTime.Visible = false;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(254, 39);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(21, 20);
         this.label1.TabIndex = 18;
         this.label1.Text = "~";
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Location = new System.Drawing.Point(9, 39);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(89, 20);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "統計期間：";
         // 
         // txtEDate
         // 
         this.txtEDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtEDate.EditValue = "2018/12";
         this.txtEDate.EnterMoveNextControl = true;
         this.txtEDate.Location = new System.Drawing.Point(281, 36);
         this.txtEDate.MenuManager = this.ribbonControl;
         this.txtEDate.Name = "txtEDate";
         this.txtEDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEDate.Size = new System.Drawing.Size(144, 26);
         this.txtEDate.TabIndex = 17;
         this.txtEDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtSDate
         // 
         this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtSDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtSDate.EditValue = "2018/12";
         this.txtSDate.EnterMoveNextControl = true;
         this.txtSDate.Location = new System.Drawing.Point(104, 36);
         this.txtSDate.MenuManager = this.ribbonControl;
         this.txtSDate.Name = "txtSDate";
         this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtSDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtSDate.Size = new System.Drawing.Size(144, 26);
         this.txtSDate.TabIndex = 16;
         this.txtSDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // ExportShow
         // 
         this.ExportShow.AutoSize = true;
         this.ExportShow.Location = new System.Drawing.Point(15, 120);
         this.ExportShow.Name = "ExportShow";
         this.ExportShow.Size = new System.Drawing.Size(54, 20);
         this.ExportShow.TabIndex = 12;
         this.ExportShow.Text = "label1";
         // 
         // panelControl2
         // 
         this.panelControl2.AllowTouchScroll = true;
         this.panelControl2.Controls.Add(this.gcMain);
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 173);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(813, 551);
         this.panelControl2.TabIndex = 1;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(2, 2);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(809, 547);
         this.gcMain.TabIndex = 10;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.AppearancePrint.BandPanel.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gvMain.AppearancePrint.BandPanel.Options.UseFont = true;
         this.gvMain.AppearancePrint.BandPanel.Options.UseTextOptions = true;
         this.gvMain.AppearancePrint.BandPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gvMain.AppearancePrint.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gvMain.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3,
            this.gridBand4,
            this.gridBand5,
            this.gridBand6});
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.PROD_ID,
            this.APDK_NAME,
            this.B_WEIGHT_QNTY,
            this.S_WEIGHT_QNTY,
            this.B_MAX_QNTY,
            this.B_MAX_SEC,
            this.S_MAX_QNTY,
            this.S_MAX_SEC});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsBehavior.Editable = false;
         this.gvMain.OptionsCustomization.AllowSort = false;
         this.gvMain.OptionsPrint.PrintHeader = false;
         this.gvMain.OptionsView.ShowColumnHeaders = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         // 
         // PROD_ID
         // 
         this.PROD_ID.FieldName = "PROD_ID";
         this.PROD_ID.MinWidth = 30;
         this.PROD_ID.Name = "PROD_ID";
         this.PROD_ID.OptionsColumn.AllowEdit = false;
         this.PROD_ID.OptionsColumn.ShowCaption = false;
         this.PROD_ID.Visible = true;
         this.PROD_ID.Width = 125;
         // 
         // APDK_NAME
         // 
         this.APDK_NAME.FieldName = "APDK_NAME";
         this.APDK_NAME.MinWidth = 30;
         this.APDK_NAME.Name = "APDK_NAME";
         this.APDK_NAME.OptionsColumn.AllowEdit = false;
         this.APDK_NAME.OptionsColumn.ShowCaption = false;
         this.APDK_NAME.Visible = true;
         this.APDK_NAME.Width = 108;
         // 
         // B_WEIGHT_QNTY
         // 
         this.B_WEIGHT_QNTY.FieldName = "B_WEIGHT_QNTY";
         this.B_WEIGHT_QNTY.MinWidth = 30;
         this.B_WEIGHT_QNTY.Name = "B_WEIGHT_QNTY";
         this.B_WEIGHT_QNTY.OptionsColumn.AllowEdit = false;
         this.B_WEIGHT_QNTY.OptionsColumn.ShowCaption = false;
         this.B_WEIGHT_QNTY.Visible = true;
         this.B_WEIGHT_QNTY.Width = 113;
         // 
         // S_WEIGHT_QNTY
         // 
         this.S_WEIGHT_QNTY.FieldName = "S_WEIGHT_QNTY";
         this.S_WEIGHT_QNTY.MinWidth = 30;
         this.S_WEIGHT_QNTY.Name = "S_WEIGHT_QNTY";
         this.S_WEIGHT_QNTY.OptionsColumn.AllowEdit = false;
         this.S_WEIGHT_QNTY.OptionsColumn.ShowCaption = false;
         this.S_WEIGHT_QNTY.Visible = true;
         this.S_WEIGHT_QNTY.Width = 106;
         // 
         // B_MAX_QNTY
         // 
         this.B_MAX_QNTY.FieldName = "B_MAX_QNTY";
         this.B_MAX_QNTY.MinWidth = 30;
         this.B_MAX_QNTY.Name = "B_MAX_QNTY";
         this.B_MAX_QNTY.OptionsColumn.AllowEdit = false;
         this.B_MAX_QNTY.OptionsColumn.ShowCaption = false;
         this.B_MAX_QNTY.Visible = true;
         this.B_MAX_QNTY.Width = 116;
         // 
         // B_MAX_SEC
         // 
         this.B_MAX_SEC.FieldName = "B_MAX_SEC";
         this.B_MAX_SEC.MinWidth = 30;
         this.B_MAX_SEC.Name = "B_MAX_SEC";
         this.B_MAX_SEC.OptionsColumn.AllowEdit = false;
         this.B_MAX_SEC.OptionsColumn.ShowCaption = false;
         this.B_MAX_SEC.Visible = true;
         this.B_MAX_SEC.Width = 76;
         // 
         // S_MAX_QNTY
         // 
         this.S_MAX_QNTY.FieldName = "S_MAX_QNTY";
         this.S_MAX_QNTY.MinWidth = 30;
         this.S_MAX_QNTY.Name = "S_MAX_QNTY";
         this.S_MAX_QNTY.OptionsColumn.AllowEdit = false;
         this.S_MAX_QNTY.OptionsColumn.ShowCaption = false;
         this.S_MAX_QNTY.Visible = true;
         this.S_MAX_QNTY.Width = 116;
         // 
         // S_MAX_SEC
         // 
         this.S_MAX_SEC.DisplayFormat.FormatString = "n4";
         this.S_MAX_SEC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.S_MAX_SEC.FieldName = "S_MAX_SEC";
         this.S_MAX_SEC.MinWidth = 30;
         this.S_MAX_SEC.Name = "S_MAX_SEC";
         this.S_MAX_SEC.OptionsColumn.AllowEdit = false;
         this.S_MAX_SEC.OptionsColumn.ShowCaption = false;
         this.S_MAX_SEC.Visible = true;
         this.S_MAX_SEC.Width = 76;
         // 
         // gridBand1
         // 
         this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand1.Caption = "股票期貨代號";
         this.gridBand1.Columns.Add(this.PROD_ID);
         this.gridBand1.Name = "gridBand1";
         this.gridBand1.VisibleIndex = 0;
         this.gridBand1.Width = 125;
         // 
         // gridBand2
         // 
         this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand2.AppearanceHeader.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
         this.gridBand2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand2.Caption = "股票期貨契約名稱";
         this.gridBand2.Columns.Add(this.APDK_NAME);
         this.gridBand2.Name = "gridBand2";
         this.gridBand2.VisibleIndex = 1;
         this.gridBand2.Width = 108;
         // 
         // gridBand3
         // 
         this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand3.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand3.Caption = "最佳1檔揭示委託買進之加權平均數量";
         this.gridBand3.Columns.Add(this.B_WEIGHT_QNTY);
         this.gridBand3.Name = "gridBand3";
         this.gridBand3.VisibleIndex = 2;
         this.gridBand3.Width = 113;
         // 
         // gridBand4
         // 
         this.gridBand4.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand4.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand4.Caption = "最佳1檔揭示委託賣出之加權平均數量";
         this.gridBand4.Columns.Add(this.S_WEIGHT_QNTY);
         this.gridBand4.Name = "gridBand4";
         this.gridBand4.RowCount = 4;
         this.gridBand4.VisibleIndex = 3;
         this.gridBand4.Width = 106;
         // 
         // gridBand5
         // 
         this.gridBand5.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand5.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand5.Caption = "最佳1檔揭示委託買進之最大數量";
         this.gridBand5.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand9,
            this.gridBand10});
         this.gridBand5.Name = "gridBand5";
         this.gridBand5.VisibleIndex = 4;
         this.gridBand5.Width = 192;
         // 
         // gridBand9
         // 
         this.gridBand9.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand9.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand9.Caption = "數量(單位:口)";
         this.gridBand9.Columns.Add(this.B_MAX_QNTY);
         this.gridBand9.Name = "gridBand9";
         this.gridBand9.VisibleIndex = 0;
         this.gridBand9.Width = 116;
         // 
         // gridBand10
         // 
         this.gridBand10.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand10.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand10.Caption = "出現時間累計\n(秒)";
         this.gridBand10.Columns.Add(this.B_MAX_SEC);
         this.gridBand10.Name = "gridBand10";
         this.gridBand10.RowCount = 2;
         this.gridBand10.VisibleIndex = 1;
         this.gridBand10.Width = 76;
         // 
         // gridBand6
         // 
         this.gridBand6.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand6.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand6.Caption = "最佳1檔揭示委託賣出之最大數量";
         this.gridBand6.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand11,
            this.gridBand12});
         this.gridBand6.Name = "gridBand6";
         this.gridBand6.VisibleIndex = 5;
         this.gridBand6.Width = 192;
         // 
         // gridBand11
         // 
         this.gridBand11.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand11.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand11.Caption = "數量(單位:口)";
         this.gridBand11.Columns.Add(this.S_MAX_QNTY);
         this.gridBand11.Name = "gridBand11";
         this.gridBand11.VisibleIndex = 0;
         this.gridBand11.Width = 116;
         // 
         // gridBand12
         // 
         this.gridBand12.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand12.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand12.Caption = "出現時間累計\n(秒)";
         this.gridBand12.Columns.Add(this.S_MAX_SEC);
         this.gridBand12.Name = "gridBand12";
         this.gridBand12.RowCount = 2;
         this.gridBand12.VisibleIndex = 1;
         this.gridBand12.Width = 76;
         // 
         // W30501
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(813, 724);
         this.Controls.Add(this.panelControl2);
         this.Controls.Add(this.panelControl1);
         this.Name = "W30501";
         this.Text = "W30501";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         this.panelControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label ExportShow;
        private Widget.TextDateEdit txtEDate;
        private Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvMain;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PROD_ID;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn APDK_NAME;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn B_WEIGHT_QNTY;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn S_WEIGHT_QNTY;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn B_MAX_QNTY;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn B_MAX_SEC;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn S_MAX_QNTY;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn S_MAX_SEC;
        private System.Windows.Forms.Label labTime;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand5;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand9;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand10;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand6;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand11;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand12;
   }
}