namespace PhoenixCI.FormUI.Prefix1
{
    partial class W1xxx
    {
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
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcol_gcMain_TXF_DEFAULT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcMain_TXF_SEQ_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcMain_TXF_TID_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcMain_TXF_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcMain_TXF_TID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcMain_ERR_MSG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageMain = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPageQuery = new DevExpress.XtraTab.XtraTabPage();
            this.gcLogsp = new DevExpress.XtraGrid.GridControl();
            this.gvSpLog = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcol_gcLogsp_LOGSP_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcLogsp_LOGSP_SEQ_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcLogsp_LOGSP_TID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcLogsp_LOGSP_TID_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcLogsp_LOGSP_END_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcol_gcLogsp_LOGSP_MSG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblOcfDate = new System.Windows.Forms.Label();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnUnselectedAll = new System.Windows.Forms.Button();
            this.txtOcfDate = new BaseGround.Widget.TextDateEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).BeginInit();
            this.xtraTabControl.SuspendLayout();
            this.xtraTabPageMain.SuspendLayout();
            this.xtraTabPageQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcLogsp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSpLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOcfDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.xtraTabControl);
            this.panParent.Controls.Add(this.txtOcfDate);
            this.panParent.Controls.Add(this.btnUnselectedAll);
            this.panParent.Controls.Add(this.btnSelectAll);
            this.panParent.Controls.Add(this.lblOcfDate);
            this.panParent.Size = new System.Drawing.Size(1104, 731);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1104, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(0, 0);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1071, 598);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcol_gcMain_TXF_DEFAULT,
            this.gcol_gcMain_TXF_SEQ_NO,
            this.gcol_gcMain_TXF_TID_NAME,
            this.gcol_gcMain_TXF_DESC,
            this.gcol_gcMain_TXF_TID,
            this.gcol_gcMain_ERR_MSG});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            // 
            // gcol_gcMain_TXF_DEFAULT
            // 
            this.gcol_gcMain_TXF_DEFAULT.AppearanceCell.Options.UseTextOptions = true;
            this.gcol_gcMain_TXF_DEFAULT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcol_gcMain_TXF_DEFAULT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcol_gcMain_TXF_DEFAULT.AppearanceHeader.Options.UseBackColor = true;
            this.gcol_gcMain_TXF_DEFAULT.Caption = "勾選";
            this.gcol_gcMain_TXF_DEFAULT.FieldName = "TXF_DEFAULT";
            this.gcol_gcMain_TXF_DEFAULT.Name = "gcol_gcMain_TXF_DEFAULT";
            this.gcol_gcMain_TXF_DEFAULT.Visible = true;
            this.gcol_gcMain_TXF_DEFAULT.VisibleIndex = 0;
            this.gcol_gcMain_TXF_DEFAULT.Width = 77;
            // 
            // gcol_gcMain_TXF_SEQ_NO
            // 
            this.gcol_gcMain_TXF_SEQ_NO.AppearanceCell.Options.UseTextOptions = true;
            this.gcol_gcMain_TXF_SEQ_NO.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcol_gcMain_TXF_SEQ_NO.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcol_gcMain_TXF_SEQ_NO.AppearanceHeader.Options.UseBackColor = true;
            this.gcol_gcMain_TXF_SEQ_NO.Caption = "序號";
            this.gcol_gcMain_TXF_SEQ_NO.FieldName = "TXF_SEQ_NO";
            this.gcol_gcMain_TXF_SEQ_NO.Name = "gcol_gcMain_TXF_SEQ_NO";
            this.gcol_gcMain_TXF_SEQ_NO.OptionsColumn.AllowEdit = false;
            this.gcol_gcMain_TXF_SEQ_NO.Visible = true;
            this.gcol_gcMain_TXF_SEQ_NO.VisibleIndex = 1;
            this.gcol_gcMain_TXF_SEQ_NO.Width = 77;
            // 
            // gcol_gcMain_TXF_TID_NAME
            // 
            this.gcol_gcMain_TXF_TID_NAME.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcol_gcMain_TXF_TID_NAME.AppearanceHeader.Options.UseBackColor = true;
            this.gcol_gcMain_TXF_TID_NAME.Caption = "名稱";
            this.gcol_gcMain_TXF_TID_NAME.FieldName = "TXF_TID_NAME";
            this.gcol_gcMain_TXF_TID_NAME.Name = "gcol_gcMain_TXF_TID_NAME";
            this.gcol_gcMain_TXF_TID_NAME.OptionsColumn.AllowEdit = false;
            this.gcol_gcMain_TXF_TID_NAME.Visible = true;
            this.gcol_gcMain_TXF_TID_NAME.VisibleIndex = 2;
            this.gcol_gcMain_TXF_TID_NAME.Width = 459;
            // 
            // gcol_gcMain_TXF_DESC
            // 
            this.gcol_gcMain_TXF_DESC.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcol_gcMain_TXF_DESC.AppearanceHeader.Options.UseBackColor = true;
            this.gcol_gcMain_TXF_DESC.Caption = "代號";
            this.gcol_gcMain_TXF_DESC.FieldName = "TXF_DESC";
            this.gcol_gcMain_TXF_DESC.Name = "gcol_gcMain_TXF_DESC";
            this.gcol_gcMain_TXF_DESC.Visible = true;
            this.gcol_gcMain_TXF_DESC.VisibleIndex = 3;
            this.gcol_gcMain_TXF_DESC.Width = 280;
            // 
            // gcol_gcMain_TXF_TID
            // 
            this.gcol_gcMain_TXF_TID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcol_gcMain_TXF_TID.AppearanceHeader.Options.UseBackColor = true;
            this.gcol_gcMain_TXF_TID.Caption = "代號";
            this.gcol_gcMain_TXF_TID.FieldName = "TXF_TID";
            this.gcol_gcMain_TXF_TID.Name = "gcol_gcMain_TXF_TID";
            this.gcol_gcMain_TXF_TID.OptionsColumn.AllowEdit = false;
            this.gcol_gcMain_TXF_TID.Width = 281;
            // 
            // gcol_gcMain_ERR_MSG
            // 
            this.gcol_gcMain_ERR_MSG.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcol_gcMain_ERR_MSG.AppearanceHeader.Options.UseBackColor = true;
            this.gcol_gcMain_ERR_MSG.Caption = "執行結果訊息";
            this.gcol_gcMain_ERR_MSG.FieldName = "ERR_MSG";
            this.gcol_gcMain_ERR_MSG.Name = "gcol_gcMain_ERR_MSG";
            this.gcol_gcMain_ERR_MSG.OptionsColumn.AllowEdit = false;
            this.gcol_gcMain_ERR_MSG.Visible = true;
            this.gcol_gcMain_ERR_MSG.VisibleIndex = 4;
            this.gcol_gcMain_ERR_MSG.Width = 160;
            // 
            // xtraTabControl
            // 
            this.xtraTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl.Location = new System.Drawing.Point(12, 81);
            this.xtraTabControl.Name = "xtraTabControl";
            this.xtraTabControl.SelectedTabPage = this.xtraTabPageMain;
            this.xtraTabControl.Size = new System.Drawing.Size(1077, 633);
            this.xtraTabControl.TabIndex = 13;
            this.xtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageMain,
            this.xtraTabPageQuery});
            this.xtraTabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl_SelectedPageChanged);
            // 
            // xtraTabPageMain
            // 
            this.xtraTabPageMain.Controls.Add(this.gcMain);
            this.xtraTabPageMain.Name = "xtraTabPageMain";
            this.xtraTabPageMain.Size = new System.Drawing.Size(1071, 598);
            this.xtraTabPageMain.Text = "執行項目";
            // 
            // xtraTabPageQuery
            // 
            this.xtraTabPageQuery.Controls.Add(this.gcLogsp);
            this.xtraTabPageQuery.Name = "xtraTabPageQuery";
            this.xtraTabPageQuery.Size = new System.Drawing.Size(1071, 598);
            this.xtraTabPageQuery.Text = "執行結果";
            // 
            // gcLogsp
            // 
            this.gcLogsp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLogsp.Location = new System.Drawing.Point(0, 0);
            this.gcLogsp.MainView = this.gvSpLog;
            this.gcLogsp.Name = "gcLogsp";
            this.gcLogsp.Size = new System.Drawing.Size(1071, 598);
            this.gcLogsp.TabIndex = 1;
            this.gcLogsp.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvSpLog});
            // 
            // gvSpLog
            // 
            this.gvSpLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcol_gcLogsp_LOGSP_DATE,
            this.gcol_gcLogsp_LOGSP_SEQ_NO,
            this.gcol_gcLogsp_LOGSP_TID,
            this.gcol_gcLogsp_LOGSP_TID_NAME,
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME,
            this.gcol_gcLogsp_LOGSP_END_TIME,
            this.gcol_gcLogsp_LOGSP_MSG});
            this.gvSpLog.GridControl = this.gcLogsp;
            this.gvSpLog.Name = "gvSpLog";
            this.gvSpLog.OptionsBehavior.Editable = false;
            // 
            // gcol_gcLogsp_LOGSP_DATE
            // 
            this.gcol_gcLogsp_LOGSP_DATE.AppearanceCell.Options.UseTextOptions = true;
            this.gcol_gcLogsp_LOGSP_DATE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcol_gcLogsp_LOGSP_DATE.Caption = "日期";
            this.gcol_gcLogsp_LOGSP_DATE.DisplayFormat.FormatString = "yyyy/MM/dd";
            this.gcol_gcLogsp_LOGSP_DATE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcol_gcLogsp_LOGSP_DATE.FieldName = "LOGSP_DATE";
            this.gcol_gcLogsp_LOGSP_DATE.MaxWidth = 100;
            this.gcol_gcLogsp_LOGSP_DATE.Name = "gcol_gcLogsp_LOGSP_DATE";
            this.gcol_gcLogsp_LOGSP_DATE.Visible = true;
            this.gcol_gcLogsp_LOGSP_DATE.VisibleIndex = 0;
            this.gcol_gcLogsp_LOGSP_DATE.Width = 73;
            // 
            // gcol_gcLogsp_LOGSP_SEQ_NO
            // 
            this.gcol_gcLogsp_LOGSP_SEQ_NO.AppearanceCell.Options.UseTextOptions = true;
            this.gcol_gcLogsp_LOGSP_SEQ_NO.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcol_gcLogsp_LOGSP_SEQ_NO.Caption = "序號";
            this.gcol_gcLogsp_LOGSP_SEQ_NO.FieldName = "LOGSP_SEQ_NO";
            this.gcol_gcLogsp_LOGSP_SEQ_NO.MaxWidth = 50;
            this.gcol_gcLogsp_LOGSP_SEQ_NO.Name = "gcol_gcLogsp_LOGSP_SEQ_NO";
            this.gcol_gcLogsp_LOGSP_SEQ_NO.Visible = true;
            this.gcol_gcLogsp_LOGSP_SEQ_NO.VisibleIndex = 1;
            this.gcol_gcLogsp_LOGSP_SEQ_NO.Width = 46;
            // 
            // gcol_gcLogsp_LOGSP_TID
            // 
            this.gcol_gcLogsp_LOGSP_TID.Caption = "批次作業代號";
            this.gcol_gcLogsp_LOGSP_TID.FieldName = "LOGSP_TID";
            this.gcol_gcLogsp_LOGSP_TID.MaxWidth = 300;
            this.gcol_gcLogsp_LOGSP_TID.Name = "gcol_gcLogsp_LOGSP_TID";
            this.gcol_gcLogsp_LOGSP_TID.Visible = true;
            this.gcol_gcLogsp_LOGSP_TID.VisibleIndex = 2;
            this.gcol_gcLogsp_LOGSP_TID.Width = 280;
            // 
            // gcol_gcLogsp_LOGSP_TID_NAME
            // 
            this.gcol_gcLogsp_LOGSP_TID_NAME.Caption = "批次名稱";
            this.gcol_gcLogsp_LOGSP_TID_NAME.FieldName = "LOGSP_TID_NAME";
            this.gcol_gcLogsp_LOGSP_TID_NAME.MinWidth = 300;
            this.gcol_gcLogsp_LOGSP_TID_NAME.Name = "gcol_gcLogsp_LOGSP_TID_NAME";
            this.gcol_gcLogsp_LOGSP_TID_NAME.Visible = true;
            this.gcol_gcLogsp_LOGSP_TID_NAME.VisibleIndex = 3;
            this.gcol_gcLogsp_LOGSP_TID_NAME.Width = 300;
            // 
            // gcol_gcLogsp_LOGSP_BEGIN_TIME
            // 
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.Caption = "開始時間";
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.FieldName = "LOGSP_BEGIN_TIME";
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.MaxWidth = 160;
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.MinWidth = 135;
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.Name = "gcol_gcLogsp_LOGSP_BEGIN_TIME";
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.Visible = true;
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.VisibleIndex = 4;
            this.gcol_gcLogsp_LOGSP_BEGIN_TIME.Width = 135;
            // 
            // gcol_gcLogsp_LOGSP_END_TIME
            // 
            this.gcol_gcLogsp_LOGSP_END_TIME.Caption = "結束時間";
            this.gcol_gcLogsp_LOGSP_END_TIME.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.gcol_gcLogsp_LOGSP_END_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcol_gcLogsp_LOGSP_END_TIME.FieldName = "LOGSP_END_TIME";
            this.gcol_gcLogsp_LOGSP_END_TIME.MaxWidth = 160;
            this.gcol_gcLogsp_LOGSP_END_TIME.MinWidth = 135;
            this.gcol_gcLogsp_LOGSP_END_TIME.Name = "gcol_gcLogsp_LOGSP_END_TIME";
            this.gcol_gcLogsp_LOGSP_END_TIME.Visible = true;
            this.gcol_gcLogsp_LOGSP_END_TIME.VisibleIndex = 5;
            this.gcol_gcLogsp_LOGSP_END_TIME.Width = 135;
            // 
            // gcol_gcLogsp_LOGSP_MSG
            // 
            this.gcol_gcLogsp_LOGSP_MSG.Caption = "執行結果訊息";
            this.gcol_gcLogsp_LOGSP_MSG.FieldName = "LOGSP_MSG";
            this.gcol_gcLogsp_LOGSP_MSG.Name = "gcol_gcLogsp_LOGSP_MSG";
            this.gcol_gcLogsp_LOGSP_MSG.Visible = true;
            this.gcol_gcLogsp_LOGSP_MSG.VisibleIndex = 6;
            this.gcol_gcLogsp_LOGSP_MSG.Width = 137;
            // 
            // lblOcfDate
            // 
            this.lblOcfDate.AutoSize = true;
            this.lblOcfDate.Location = new System.Drawing.Point(31, 34);
            this.lblOcfDate.Name = "lblOcfDate";
            this.lblOcfDate.Size = new System.Drawing.Size(89, 20);
            this.lblOcfDate.TabIndex = 6;
            this.lblOcfDate.Text = "交易日期：";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(330, 28);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(60, 32);
            this.btnSelectAll.TabIndex = 8;
            this.btnSelectAll.Text = "全選";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnUnselectedAll
            // 
            this.btnUnselectedAll.Location = new System.Drawing.Point(396, 28);
            this.btnUnselectedAll.Name = "btnUnselectedAll";
            this.btnUnselectedAll.Size = new System.Drawing.Size(60, 32);
            this.btnUnselectedAll.TabIndex = 9;
            this.btnUnselectedAll.Text = "全清";
            this.btnUnselectedAll.UseVisualStyleBackColor = true;
            this.btnUnselectedAll.Click += new System.EventHandler(this.btnUnselectedAll_Click);
            // 
            // txtOcfDate
            // 
            this.txtOcfDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtOcfDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtOcfDate.EditValue = "0001/1/1 上午 12:00:00";
            this.txtOcfDate.Location = new System.Drawing.Point(126, 31);
            this.txtOcfDate.MenuManager = this.ribbonControl;
            this.txtOcfDate.Name = "txtOcfDate";
            this.txtOcfDate.Properties.EditFormat.FormatString = "yyyy/MM/dd";
            this.txtOcfDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtOcfDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtOcfDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtOcfDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtOcfDate.Size = new System.Drawing.Size(100, 26);
            this.txtOcfDate.TabIndex = 11;
            this.txtOcfDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // W1xxx
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 761);
            this.Name = "W1xxx";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).EndInit();
            this.xtraTabControl.ResumeLayout(false);
            this.xtraTabPageMain.ResumeLayout(false);
            this.xtraTabPageQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcLogsp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSpLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOcfDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        public DevExpress.XtraGrid.Columns.GridColumn UPF_USER_ID;
        public DevExpress.XtraGrid.Columns.GridColumn UPF_EMPLOYEE_ID;
        public DevExpress.XtraGrid.Columns.GridColumn UPF_DPT_ID;
        public DevExpress.XtraGrid.Columns.GridColumn gcol_gcMain_TXF_DEFAULT;
        private DevExpress.XtraGrid.Columns.GridColumn gcol_gcMain_TXF_SEQ_NO;
        public DevExpress.XtraGrid.Columns.GridColumn gcol_gcMain_TXF_TID_NAME;
        public DevExpress.XtraGrid.Columns.GridColumn gcol_gcMain_TXF_TID;
        public DevExpress.XtraGrid.Columns.GridColumn gcol_gcMain_ERR_MSG;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageMain;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageQuery;
        private DevExpress.XtraGrid.GridControl gcLogsp;
        private DevExpress.XtraGrid.Views.Grid.GridView gvSpLog;
        public DevExpress.XtraGrid.Columns.GridColumn gcol_gcLogsp_LOGSP_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn gcol_gcLogsp_LOGSP_SEQ_NO;
        public DevExpress.XtraGrid.Columns.GridColumn gcol_gcLogsp_LOGSP_TID;
        public DevExpress.XtraGrid.Columns.GridColumn gcol_gcLogsp_LOGSP_TID_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gcol_gcLogsp_LOGSP_BEGIN_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn gcol_gcLogsp_LOGSP_END_TIME;
        public DevExpress.XtraGrid.Columns.GridColumn gcol_gcLogsp_LOGSP_MSG;
        private System.Windows.Forms.Button btnUnselectedAll;
        private System.Windows.Forms.Button btnSelectAll;
        protected BaseGround.Widget.TextDateEdit txtOcfDate;
        protected System.Windows.Forms.Label lblOcfDate;
        private DevExpress.XtraGrid.Columns.GridColumn gcol_gcMain_TXF_DESC;
    }
}