namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0111
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
            this.LOGUTP_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGUTP_W_USER_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGUTP_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DPT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGUTP_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGUTP_USER_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGUTP_TXN_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGUTP_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cbxUserId = new System.Windows.Forms.ComboBox();
            this.lblUserId = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.txtEndDate = new PhoenixCI.Widget.TextDateEdit();
            this.txtStartDate = new PhoenixCI.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.txtEndDate);
            this.panParent.Controls.Add(this.txtStartDate);
            this.panParent.Controls.Add(this.label1);
            this.panParent.Controls.Add(this.lblNote);
            this.panParent.Controls.Add(this.lblSearch);
            this.panParent.Controls.Add(this.cbxUserId);
            this.panParent.Controls.Add(this.lblUserId);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(1087, 542);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1087, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 49);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1060, 478);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.ColumnPanelRowHeight = 50;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.LOGUTP_W_USER_ID,
            this.LOGUTP_W_USER_NAME,
            this.LOGUTP_W_TIME,
            this.DPT_NAME,
            this.LOGUTP_USER_ID,
            this.LOGUTP_USER_NAME,
            this.LOGUTP_TXN_ID,
            this.TXN_NAME,
            this.LOGUTP_TYPE});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsBehavior.Editable = false;
            // 
            // LOGUTP_W_USER_ID
            // 
            this.LOGUTP_W_USER_ID.AppearanceCell.Options.UseTextOptions = true;
            this.LOGUTP_W_USER_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGUTP_W_USER_ID.Caption = "作業人員\n代號";
            this.LOGUTP_W_USER_ID.FieldName = "LOGUTP_W_USER_ID";
            this.LOGUTP_W_USER_ID.Name = "LOGUTP_W_USER_ID";
            this.LOGUTP_W_USER_ID.Visible = true;
            this.LOGUTP_W_USER_ID.VisibleIndex = 0;
            this.LOGUTP_W_USER_ID.Width = 80;
            // 
            // LOGUTP_W_USER_NAME
            // 
            this.LOGUTP_W_USER_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.LOGUTP_W_USER_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGUTP_W_USER_NAME.Caption = "作業人員\n名稱";
            this.LOGUTP_W_USER_NAME.FieldName = "LOGUTP_W_USER_NAME";
            this.LOGUTP_W_USER_NAME.Name = "LOGUTP_W_USER_NAME";
            this.LOGUTP_W_USER_NAME.Visible = true;
            this.LOGUTP_W_USER_NAME.VisibleIndex = 1;
            this.LOGUTP_W_USER_NAME.Width = 90;
            // 
            // LOGUTP_W_TIME
            // 
            this.LOGUTP_W_TIME.AppearanceCell.Options.UseTextOptions = true;
            this.LOGUTP_W_TIME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGUTP_W_TIME.Caption = "異動時間";
            this.LOGUTP_W_TIME.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.LOGUTP_W_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.LOGUTP_W_TIME.FieldName = "LOGUTP_W_TIME";
            this.LOGUTP_W_TIME.Name = "LOGUTP_W_TIME";
            this.LOGUTP_W_TIME.Visible = true;
            this.LOGUTP_W_TIME.VisibleIndex = 2;
            this.LOGUTP_W_TIME.Width = 170;
            // 
            // DPT_NAME
            // 
            this.DPT_NAME.Caption = "異動對象\n部門";
            this.DPT_NAME.FieldName = "DPT_NAME";
            this.DPT_NAME.Name = "DPT_NAME";
            this.DPT_NAME.Visible = true;
            this.DPT_NAME.VisibleIndex = 3;
            this.DPT_NAME.Width = 102;
            // 
            // LOGUTP_USER_ID
            // 
            this.LOGUTP_USER_ID.AppearanceCell.Options.UseTextOptions = true;
            this.LOGUTP_USER_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGUTP_USER_ID.Caption = "異動對象\n代號";
            this.LOGUTP_USER_ID.FieldName = "LOGUTP_USER_ID";
            this.LOGUTP_USER_ID.Name = "LOGUTP_USER_ID";
            this.LOGUTP_USER_ID.Visible = true;
            this.LOGUTP_USER_ID.VisibleIndex = 4;
            this.LOGUTP_USER_ID.Width = 73;
            // 
            // LOGUTP_USER_NAME
            // 
            this.LOGUTP_USER_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.LOGUTP_USER_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGUTP_USER_NAME.Caption = "異動對象\n名稱";
            this.LOGUTP_USER_NAME.FieldName = "LOGUTP_USER_NAME";
            this.LOGUTP_USER_NAME.Name = "LOGUTP_USER_NAME";
            this.LOGUTP_USER_NAME.Visible = true;
            this.LOGUTP_USER_NAME.VisibleIndex = 5;
            this.LOGUTP_USER_NAME.Width = 83;
            // 
            // LOGUTP_TXN_ID
            // 
            this.LOGUTP_TXN_ID.AppearanceCell.Options.UseTextOptions = true;
            this.LOGUTP_TXN_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGUTP_TXN_ID.Caption = "作業代號";
            this.LOGUTP_TXN_ID.FieldName = "LOGUTP_TXN_ID";
            this.LOGUTP_TXN_ID.Name = "LOGUTP_TXN_ID";
            this.LOGUTP_TXN_ID.Visible = true;
            this.LOGUTP_TXN_ID.VisibleIndex = 6;
            this.LOGUTP_TXN_ID.Width = 80;
            // 
            // TXN_NAME
            // 
            this.TXN_NAME.Caption = "作業名稱";
            this.TXN_NAME.FieldName = "TXN_NAME";
            this.TXN_NAME.Name = "TXN_NAME";
            this.TXN_NAME.Visible = true;
            this.TXN_NAME.VisibleIndex = 7;
            this.TXN_NAME.Width = 310;
            // 
            // LOGUTP_TYPE
            // 
            this.LOGUTP_TYPE.AppearanceCell.Options.UseTextOptions = true;
            this.LOGUTP_TYPE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGUTP_TYPE.Caption = "異動\n種類";
            this.LOGUTP_TYPE.FieldName = "LOGUTP_TYPE_NAME";
            this.LOGUTP_TYPE.Name = "LOGUTP_TYPE";
            this.LOGUTP_TYPE.UnboundExpression = "Iif([LOGUTP_TYPE] = \'I\', \'新增\', Iif([LOGUTP_TYPE] = \'D\', \'刪除\', \'\'))";
            this.LOGUTP_TYPE.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.LOGUTP_TYPE.Visible = true;
            this.LOGUTP_TYPE.VisibleIndex = 8;
            this.LOGUTP_TYPE.Width = 54;
            // 
            // cbxUserId
            // 
            this.cbxUserId.BackColor = System.Drawing.Color.White;
            this.cbxUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUserId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxUserId.ForeColor = System.Drawing.Color.Black;
            this.cbxUserId.FormattingEnabled = true;
            this.cbxUserId.Location = new System.Drawing.Point(540, 13);
            this.cbxUserId.Name = "cbxUserId";
            this.cbxUserId.Size = new System.Drawing.Size(169, 28);
            this.cbxUserId.TabIndex = 9;
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(429, 18);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(105, 20);
            this.lblUserId.TabIndex = 8;
            this.lblUserId.Text = "使用者代號：";
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(10, 18);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(89, 20);
            this.lblSearch.TabIndex = 16;
            this.lblSearch.Text = "查詢區間：";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblNote.Location = new System.Drawing.Point(715, 21);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(94, 17);
            this.lblNote.TabIndex = 18;
            this.lblNote.Text = "(空白代表全部)";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtEndDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "";
            this.txtEndDate.Location = new System.Drawing.Point(265, 15);
            this.txtEndDate.MenuManager = this.ribbonControl;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(127, 26);
            this.txtEndDate.TabIndex = 32;
            this.txtEndDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtStartDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "";
            this.txtStartDate.Location = new System.Drawing.Point(105, 15);
            this.txtStartDate.MenuManager = this.ribbonControl;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(127, 26);
            this.txtStartDate.TabIndex = 31;
            this.txtStartDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(238, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 20);
            this.label1.TabIndex = 30;
            this.label1.Text = "~";
            // 
            // WZ0111
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 572);
            this.Name = "WZ0111";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cbxUserId;
        private System.Windows.Forms.Label lblUserId;
        private DevExpress.XtraGrid.Columns.GridColumn LOGUTP_W_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn LOGUTP_W_USER_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn LOGUTP_W_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn DPT_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn LOGUTP_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn LOGUTP_USER_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn LOGUTP_TXN_ID;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn LOGUTP_TYPE;
        private Widget.TextDateEdit txtEndDate;
        private Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label label1;
    }
}