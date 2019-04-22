namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ9999
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
            this.LOGF_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGF_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_USER_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGF_TXN_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOGF_KEY_DATA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAudit = new DevExpress.XtraEditors.CheckEdit();
            this.chkTime = new DevExpress.XtraEditors.CheckEdit();
            this.txtStartTime = new BaseGround.Widget.TextDateEdit();
            this.txtEndTime = new BaseGround.Widget.TextDateEdit();
            this.txtEndDate = new BaseGround.Widget.TextDateEdit();
            this.txtStartDate = new BaseGround.Widget.TextDateEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAudit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.txtEndDate);
            this.panParent.Controls.Add(this.txtStartDate);
            this.panParent.Controls.Add(this.txtEndTime);
            this.panParent.Controls.Add(this.txtStartTime);
            this.panParent.Controls.Add(this.chkTime);
            this.panParent.Controls.Add(this.chkAudit);
            this.panParent.Controls.Add(this.label2);
            this.panParent.Controls.Add(this.label1);
            this.panParent.Controls.Add(this.lblDate);
            this.panParent.Controls.Add(this.lblNote);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(1283, 538);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1283, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 93);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1259, 431);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.ColumnPanelRowHeight = 50;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.LOGF_TIME,
            this.LOGF_USER_ID,
            this.UPF_USER_NAME,
            this.LOGF_TXN_ID,
            this.TXN_NAME,
            this.LOGF_KEY_DATA});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsBehavior.Editable = false;
            // 
            // LOGF_TIME
            // 
            this.LOGF_TIME.AppearanceCell.Options.UseTextOptions = true;
            this.LOGF_TIME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGF_TIME.Caption = "作業時間";
            this.LOGF_TIME.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.LOGF_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.LOGF_TIME.FieldName = "LOGF_TIME";
            this.LOGF_TIME.Name = "LOGF_TIME";
            this.LOGF_TIME.Visible = true;
            this.LOGF_TIME.VisibleIndex = 0;
            this.LOGF_TIME.Width = 196;
            // 
            // LOGF_USER_ID
            // 
            this.LOGF_USER_ID.AppearanceCell.Options.UseTextOptions = true;
            this.LOGF_USER_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGF_USER_ID.Caption = "作業人員";
            this.LOGF_USER_ID.FieldName = "LOGF_USER_ID";
            this.LOGF_USER_ID.Name = "LOGF_USER_ID";
            this.LOGF_USER_ID.Visible = true;
            this.LOGF_USER_ID.VisibleIndex = 1;
            this.LOGF_USER_ID.Width = 92;
            // 
            // UPF_USER_NAME
            // 
            this.UPF_USER_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_USER_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_USER_NAME.Caption = "人員名稱";
            this.UPF_USER_NAME.FieldName = "UPF_USER_NAME";
            this.UPF_USER_NAME.Name = "UPF_USER_NAME";
            this.UPF_USER_NAME.Visible = true;
            this.UPF_USER_NAME.VisibleIndex = 2;
            this.UPF_USER_NAME.Width = 92;
            // 
            // LOGF_TXN_ID
            // 
            this.LOGF_TXN_ID.AppearanceCell.Options.UseTextOptions = true;
            this.LOGF_TXN_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOGF_TXN_ID.Caption = "作業代號";
            this.LOGF_TXN_ID.FieldName = "LOGF_TXN_ID";
            this.LOGF_TXN_ID.Name = "LOGF_TXN_ID";
            this.LOGF_TXN_ID.Visible = true;
            this.LOGF_TXN_ID.VisibleIndex = 3;
            this.LOGF_TXN_ID.Width = 92;
            // 
            // TXN_NAME
            // 
            this.TXN_NAME.Caption = "作業名稱";
            this.TXN_NAME.FieldName = "TXN_NAME";
            this.TXN_NAME.Name = "TXN_NAME";
            this.TXN_NAME.Visible = true;
            this.TXN_NAME.VisibleIndex = 4;
            this.TXN_NAME.Width = 381;
            // 
            // LOGF_KEY_DATA
            // 
            this.LOGF_KEY_DATA.Caption = "操作內容";
            this.LOGF_KEY_DATA.FieldName = "LOGF_KEY_DATA";
            this.LOGF_KEY_DATA.Name = "LOGF_KEY_DATA";
            this.LOGF_KEY_DATA.Visible = true;
            this.LOGF_KEY_DATA.VisibleIndex = 5;
            this.LOGF_KEY_DATA.Width = 400;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(28, 20);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(89, 20);
            this.lblDate.TabIndex = 10;
            this.lblDate.Text = "交易日期：";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(412, 53);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(243, 20);
            this.lblNote.TabIndex = 11;
            this.lblNote.Text = "(查詢條件為合理時間以外之資料)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "~";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "~";
            // 
            // chkAudit
            // 
            this.chkAudit.EditValue = "Y";
            this.chkAudit.Location = new System.Drawing.Point(416, 19);
            this.chkAudit.Name = "chkAudit";
            this.chkAudit.Properties.Caption = "範圍：權限設定作業";
            this.chkAudit.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.chkAudit.Properties.ValueChecked = "Y";
            this.chkAudit.Properties.ValueUnchecked = "";
            this.chkAudit.Size = new System.Drawing.Size(241, 24);
            this.chkAudit.TabIndex = 24;
            // 
            // chkTime
            // 
            this.chkTime.EditValue = "Y";
            this.chkTime.Location = new System.Drawing.Point(14, 51);
            this.chkTime.Name = "chkTime";
            this.chkTime.Properties.Caption = "合理時間：";
            this.chkTime.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.chkTime.Properties.ValueChecked = "Y";
            this.chkTime.Properties.ValueUnchecked = "";
            this.chkTime.Size = new System.Drawing.Size(95, 24);
            this.chkTime.TabIndex = 25;
            // 
            // txtStartTime
            // 
            this.txtStartTime.DateTimeValue = new System.DateTime(2018, 4, 12, 8, 0, 0, 0);
            this.txtStartTime.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Time;
            this.txtStartTime.EditValue = "08:00:00";
            this.txtStartTime.Location = new System.Drawing.Point(113, 50);
            this.txtStartTime.MenuManager = this.ribbonControl;
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartTime.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartTime.Properties.Mask.EditMask = "HH:mm:ss";
            this.txtStartTime.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartTime.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartTime.Size = new System.Drawing.Size(127, 26);
            this.txtStartTime.TabIndex = 26;
            this.txtStartTime.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // txtEndTime
            // 
            this.txtEndTime.DateTimeValue = new System.DateTime(2018, 4, 12, 17, 30, 0, 0);
            this.txtEndTime.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Time;
            this.txtEndTime.EditValue = "17:30:00";
            this.txtEndTime.Location = new System.Drawing.Point(273, 50);
            this.txtEndTime.MenuManager = this.ribbonControl;
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndTime.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndTime.Properties.Mask.EditMask = "HH:mm:ss";
            this.txtEndTime.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndTime.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndTime.Size = new System.Drawing.Size(127, 26);
            this.txtEndTime.TabIndex = 27;
            this.txtEndTime.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtEndDate.EditValue = "";
            this.txtEndDate.Location = new System.Drawing.Point(273, 17);
            this.txtEndDate.MenuManager = this.ribbonControl;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Size = new System.Drawing.Size(127, 26);
            this.txtEndDate.TabIndex = 29;
            this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtStartDate.EditValue = "";
            this.txtStartDate.Location = new System.Drawing.Point(113, 17);
            this.txtStartDate.MenuManager = this.ribbonControl;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Size = new System.Drawing.Size(127, 26);
            this.txtStartDate.TabIndex = 28;
            this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // WZ9999
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 568);
            this.Name = "WZ9999";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAudit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblNote;
        private DevExpress.XtraGrid.Columns.GridColumn LOGF_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn LOGF_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_USER_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn LOGF_TXN_ID;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn LOGF_KEY_DATA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.CheckEdit chkAudit;
        private DevExpress.XtraEditors.CheckEdit chkTime;
        private BaseGround.Widget.TextDateEdit txtStartTime;
        private BaseGround.Widget.TextDateEdit txtEndTime;
        private BaseGround.Widget.TextDateEdit txtEndDate;
        private BaseGround.Widget.TextDateEdit txtStartDate;
    }
}