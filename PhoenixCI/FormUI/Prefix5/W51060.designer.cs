namespace PhoenixCI.FormUI.Prefix5
{
    partial class W51060
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtYM = new PhoenixCI.Widget.TextDateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.MMIQ_YM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMIQ_FCM_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.MMIQ_ACC_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMIQ_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMIQ_INVALID_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.IS_NEWROW = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMIQ_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMIQ_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtYM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Size = new System.Drawing.Size(1055, 559);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1055, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtYM);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1055, 147);
            this.panelControl1.TabIndex = 0;
            // 
            // txtYM
            // 
            this.txtYM.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtYM.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtYM.EditValue = "2018/12";
            this.txtYM.EnterMoveNextControl = true;
            this.txtYM.Location = new System.Drawing.Point(59, 10);
            this.txtYM.MenuManager = this.ribbonControl;
            this.txtYM.Name = "txtYM";
            this.txtYM.Properties.Appearance.Options.UseTextOptions = true;
            this.txtYM.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtYM.Properties.Mask.EditMask = "yyyy/MM";
            this.txtYM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtYM.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtYM.Size = new System.Drawing.Size(144, 26);
            this.txtYM.TabIndex = 16;
            this.txtYM.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(243, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(549, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "(2)已儲存過之資料不提供刪除功能，只能將成交量設為0，存檔後會自動刪除";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(243, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(534, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "(3)在執行造市者統計檔批次時不可存檔(僅適用一般交易，不適用盤後交易)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "年月 : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(332, 40);
            this.label2.TabIndex = 12;
            this.label2.Text = "(1)輸入條件「年月」變更後請先按「讀取」，\r\n若與「下方視窗年月」不同則不可新增及存檔";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcMain);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 177);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1055, 412);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(2, 2);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2});
            this.gcMain.Size = new System.Drawing.Size(1051, 408);
            this.gcMain.TabIndex = 6;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.MMIQ_YM,
            this.MMIQ_FCM_NO,
            this.MMIQ_ACC_NO,
            this.MMIQ_KIND_ID,
            this.MMIQ_INVALID_QNTY,
            this.IS_NEWROW,
            this.MMIQ_W_TIME,
            this.MMIQ_W_USER_ID});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsCustomization.AllowSort = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_NewRowAllowEdit);
            // 
            // MMIQ_YM
            // 
            this.MMIQ_YM.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.MMIQ_YM.AppearanceCell.Options.UseBackColor = true;
            this.MMIQ_YM.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMIQ_YM.AppearanceHeader.Options.UseBackColor = true;
            this.MMIQ_YM.Caption = "年月";
            this.MMIQ_YM.DisplayFormat.FormatString = "yyyyMM";
            this.MMIQ_YM.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.MMIQ_YM.FieldName = "MMIQ_YM";
            this.MMIQ_YM.MinWidth = 30;
            this.MMIQ_YM.Name = "MMIQ_YM";
            this.MMIQ_YM.OptionsColumn.AllowEdit = false;
            this.MMIQ_YM.Visible = true;
            this.MMIQ_YM.VisibleIndex = 0;
            this.MMIQ_YM.Width = 112;
            // 
            // MMIQ_FCM_NO
            // 
            this.MMIQ_FCM_NO.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.MMIQ_FCM_NO.AppearanceCell.Options.UseBackColor = true;
            this.MMIQ_FCM_NO.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMIQ_FCM_NO.AppearanceHeader.Options.UseBackColor = true;
            this.MMIQ_FCM_NO.Caption = "期貨商代號";
            this.MMIQ_FCM_NO.ColumnEdit = this.repositoryItemTextEdit2;
            this.MMIQ_FCM_NO.FieldName = "MMIQ_FCM_NO";
            this.MMIQ_FCM_NO.MinWidth = 30;
            this.MMIQ_FCM_NO.Name = "MMIQ_FCM_NO";
            this.MMIQ_FCM_NO.Visible = true;
            this.MMIQ_FCM_NO.VisibleIndex = 1;
            this.MMIQ_FCM_NO.Width = 112;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.MaxLength = 3;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // MMIQ_ACC_NO
            // 
            this.MMIQ_ACC_NO.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.MMIQ_ACC_NO.AppearanceCell.Options.UseBackColor = true;
            this.MMIQ_ACC_NO.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMIQ_ACC_NO.AppearanceHeader.Options.UseBackColor = true;
            this.MMIQ_ACC_NO.Caption = "交易人代號";
            this.MMIQ_ACC_NO.ColumnEdit = this.repositoryItemTextEdit2;
            this.MMIQ_ACC_NO.FieldName = "MMIQ_ACC_NO";
            this.MMIQ_ACC_NO.MinWidth = 30;
            this.MMIQ_ACC_NO.Name = "MMIQ_ACC_NO";
            this.MMIQ_ACC_NO.Visible = true;
            this.MMIQ_ACC_NO.VisibleIndex = 2;
            this.MMIQ_ACC_NO.Width = 112;
            // 
            // MMIQ_KIND_ID
            // 
            this.MMIQ_KIND_ID.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.MMIQ_KIND_ID.AppearanceCell.Options.UseBackColor = true;
            this.MMIQ_KIND_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMIQ_KIND_ID.AppearanceHeader.Options.UseBackColor = true;
            this.MMIQ_KIND_ID.Caption = "契約代碼(3碼)";
            this.MMIQ_KIND_ID.ColumnEdit = this.repositoryItemTextEdit2;
            this.MMIQ_KIND_ID.FieldName = "MMIQ_KIND_ID";
            this.MMIQ_KIND_ID.MinWidth = 30;
            this.MMIQ_KIND_ID.Name = "MMIQ_KIND_ID";
            this.MMIQ_KIND_ID.Visible = true;
            this.MMIQ_KIND_ID.VisibleIndex = 3;
            this.MMIQ_KIND_ID.Width = 112;
            // 
            // MMIQ_INVALID_QNTY
            // 
            this.MMIQ_INVALID_QNTY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMIQ_INVALID_QNTY.AppearanceHeader.Options.UseBackColor = true;
            this.MMIQ_INVALID_QNTY.Caption = "不合理成交量";
            this.MMIQ_INVALID_QNTY.ColumnEdit = this.repositoryItemTextEdit1;
            this.MMIQ_INVALID_QNTY.FieldName = "MMIQ_INVALID_QNTY";
            this.MMIQ_INVALID_QNTY.MinWidth = 30;
            this.MMIQ_INVALID_QNTY.Name = "MMIQ_INVALID_QNTY";
            this.MMIQ_INVALID_QNTY.Visible = true;
            this.MMIQ_INVALID_QNTY.VisibleIndex = 4;
            this.MMIQ_INVALID_QNTY.Width = 112;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // IS_NEWROW
            // 
            this.IS_NEWROW.Caption = "IS_NEWROW";
            this.IS_NEWROW.FieldName = "IS_NEWROW";
            this.IS_NEWROW.MinWidth = 30;
            this.IS_NEWROW.Name = "IS_NEWROW";
            this.IS_NEWROW.Width = 112;
            // 
            // MMIQ_W_TIME
            // 
            this.MMIQ_W_TIME.Caption = "MMIQ_W_TIME";
            this.MMIQ_W_TIME.FieldName = "MMIQ_W_TIME";
            this.MMIQ_W_TIME.MinWidth = 30;
            this.MMIQ_W_TIME.Name = "MMIQ_W_TIME";
            this.MMIQ_W_TIME.Width = 112;
            // 
            // MMIQ_W_USER_ID
            // 
            this.MMIQ_W_USER_ID.Caption = "MMIQ_W_USER_ID";
            this.MMIQ_W_USER_ID.FieldName = "MMIQ_W_USER_ID";
            this.MMIQ_W_USER_ID.MinWidth = 30;
            this.MMIQ_W_USER_ID.Name = "MMIQ_W_USER_ID";
            this.MMIQ_W_USER_ID.Width = 112;
            // 
            // W51060
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1055, 589);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W51060";
            this.Text = "W51060";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtYM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn MMIQ_YM;
        private DevExpress.XtraGrid.Columns.GridColumn MMIQ_FCM_NO;
        private DevExpress.XtraGrid.Columns.GridColumn MMIQ_ACC_NO;
        private DevExpress.XtraGrid.Columns.GridColumn MMIQ_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn MMIQ_INVALID_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn IS_NEWROW;
        private DevExpress.XtraGrid.Columns.GridColumn MMIQ_W_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn MMIQ_W_USER_ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private Widget.TextDateEdit txtYM;
    }
}