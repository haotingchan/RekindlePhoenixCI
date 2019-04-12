namespace PhoenixCI.FormUI.Prefix4 {
    partial class W49050 {
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
         this.label1 = new System.Windows.Forms.Label();
         this.lblDate = new System.Windows.Forms.Label();
         this.txtStartYear = new DevExpress.XtraEditors.TextEdit();
         this.txtEndYear = new DevExpress.XtraEditors.TextEdit();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.MGT3_DATE_FM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT3_DATE_TO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT3_MEMO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_CMTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.MGT3_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT3_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.IS_NEWROW = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_MMTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.MG8_IMTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.MG8_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_EFFECT_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_F_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_CM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYear.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYear.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.MG8_CMTextEdit)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.MG8_MMTextEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.MG8_IMTextEdit1)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Location = new System.Drawing.Point(0, 100);
         this.panParent.Size = new System.Drawing.Size(899, 545);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(899, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Controls.Add(this.lblDate);
         this.panelControl1.Controls.Add(this.txtStartYear);
         this.panelControl1.Controls.Add(this.txtEndYear);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(899, 70);
         this.panelControl1.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.BackColor = System.Drawing.Color.Transparent;
         this.label1.Location = new System.Drawing.Point(221, 26);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(21, 20);
         this.label1.TabIndex = 20;
         this.label1.Text = "~";
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.BackColor = System.Drawing.Color.Transparent;
         this.lblDate.Location = new System.Drawing.Point(29, 26);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(57, 20);
         this.lblDate.TabIndex = 18;
         this.lblDate.Text = "年度：";
         // 
         // txtStartYear
         // 
         this.txtStartYear.EditValue = "2018";
         this.txtStartYear.Location = new System.Drawing.Point(115, 23);
         this.txtStartYear.MenuManager = this.ribbonControl;
         this.txtStartYear.Name = "txtStartYear";
         this.txtStartYear.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartYear.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartYear.Size = new System.Drawing.Size(100, 26);
         this.txtStartYear.TabIndex = 21;
         // 
         // txtEndYear
         // 
         this.txtEndYear.EditValue = "2018";
         this.txtEndYear.Location = new System.Drawing.Point(248, 23);
         this.txtEndYear.MenuManager = this.ribbonControl;
         this.txtEndYear.Name = "txtEndYear";
         this.txtEndYear.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndYear.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndYear.Size = new System.Drawing.Size(100, 26);
         this.txtEndYear.TabIndex = 22;
         // 
         // panelControl2
         // 
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 100);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(899, 545);
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
            this.MG8_CMTextEdit,
            this.MG8_MMTextEdit1,
            this.MG8_IMTextEdit1});
         this.gcMain.Size = new System.Drawing.Size(875, 521);
         this.gcMain.TabIndex = 0;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         this.gcMain.Visible = false;
         // 
         // gvMain
         // 
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.MGT3_DATE_FM,
            this.MGT3_DATE_TO,
            this.MGT3_MEMO,
            this.MGT3_W_TIME,
            this.MGT3_W_USER_ID,
            this.IS_NEWROW});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         // 
         // MGT3_DATE_FM
         // 
         this.MGT3_DATE_FM.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.MGT3_DATE_FM.AppearanceHeader.Options.UseBackColor = true;
         this.MGT3_DATE_FM.AppearanceHeader.Options.UseTextOptions = true;
         this.MGT3_DATE_FM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MGT3_DATE_FM.Caption = "起始日期";
         this.MGT3_DATE_FM.FieldName = "MGT3_DATE_FM";
         this.MGT3_DATE_FM.Name = "MGT3_DATE_FM";
         this.MGT3_DATE_FM.Visible = true;
         this.MGT3_DATE_FM.VisibleIndex = 0;
         // 
         // MGT3_DATE_TO
         // 
         this.MGT3_DATE_TO.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.MGT3_DATE_TO.AppearanceHeader.Options.UseBackColor = true;
         this.MGT3_DATE_TO.AppearanceHeader.Options.UseTextOptions = true;
         this.MGT3_DATE_TO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MGT3_DATE_TO.Caption = "迄止日期";
         this.MGT3_DATE_TO.FieldName = "MGT3_DATE_TO";
         this.MGT3_DATE_TO.Name = "MGT3_DATE_TO";
         this.MGT3_DATE_TO.Visible = true;
         this.MGT3_DATE_TO.VisibleIndex = 1;
         this.MGT3_DATE_TO.Width = 200;
         // 
         // MGT3_MEMO
         // 
         this.MGT3_MEMO.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MGT3_MEMO.AppearanceHeader.Options.UseBackColor = true;
         this.MGT3_MEMO.AppearanceHeader.Options.UseTextOptions = true;
         this.MGT3_MEMO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MGT3_MEMO.Caption = "說明";
         this.MGT3_MEMO.ColumnEdit = this.MG8_CMTextEdit;
         this.MGT3_MEMO.FieldName = "MGT3_MEMO";
         this.MGT3_MEMO.Name = "MGT3_MEMO";
         this.MGT3_MEMO.Visible = true;
         this.MGT3_MEMO.VisibleIndex = 2;
         this.MGT3_MEMO.Width = 145;
         // 
         // MG8_CMTextEdit
         // 
         this.MG8_CMTextEdit.AutoHeight = false;
         this.MG8_CMTextEdit.DisplayFormat.FormatString = "###0.####";
         this.MG8_CMTextEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.MG8_CMTextEdit.MaxLength = 10;
         this.MG8_CMTextEdit.Name = "MG8_CMTextEdit";
         // 
         // MGT3_W_TIME
         // 
         this.MGT3_W_TIME.Caption = "MGT3_W_TIME";
         this.MGT3_W_TIME.FieldName = "MGT3_W_TIME";
         this.MGT3_W_TIME.Name = "MGT3_W_TIME";
         // 
         // MGT3_W_USER_ID
         // 
         this.MGT3_W_USER_ID.Caption = "MGT3_W_USER_ID";
         this.MGT3_W_USER_ID.FieldName = "MGT3_W_USER_ID";
         this.MGT3_W_USER_ID.Name = "MGT3_W_USER_ID";
         // 
         // IS_NEWROW
         // 
         this.IS_NEWROW.Caption = "Is_NewRow";
         this.IS_NEWROW.FieldName = "IS_NEWROW";
         this.IS_NEWROW.Name = "IS_NEWROW";
         // 
         // MG8_MMTextEdit1
         // 
         this.MG8_MMTextEdit1.AutoHeight = false;
         this.MG8_MMTextEdit1.DisplayFormat.FormatString = "###0.####";
         this.MG8_MMTextEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.MG8_MMTextEdit1.Mask.EditMask = "####0.0000";
         this.MG8_MMTextEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.MG8_MMTextEdit1.MaxLength = 10;
         this.MG8_MMTextEdit1.Name = "MG8_MMTextEdit1";
         // 
         // MG8_IMTextEdit1
         // 
         this.MG8_IMTextEdit1.AutoHeight = false;
         this.MG8_IMTextEdit1.DisplayFormat.FormatString = "###0.####";
         this.MG8_IMTextEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.MG8_IMTextEdit1.Mask.EditMask = "####0.0000";
         this.MG8_IMTextEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.MG8_IMTextEdit1.MaxLength = 10;
         this.MG8_IMTextEdit1.Name = "MG8_IMTextEdit1";
         // 
         // MG8_W_TIME
         // 
         this.MG8_W_TIME.Caption = "MG8_W_TIME";
         this.MG8_W_TIME.FieldName = "MG8_W_TIME";
         this.MG8_W_TIME.Name = "MG8_W_TIME";
         // 
         // MG8_W_USER_ID
         // 
         this.MG8_W_USER_ID.Caption = "MGT3_W_USER_ID";
         this.MG8_W_USER_ID.FieldName = "MG8_W_USER_ID";
         this.MG8_W_USER_ID.Name = "MG8_W_USER_ID";
         // 
         // MG8_EFFECT_YMD
         // 
         this.MG8_EFFECT_YMD.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.MG8_EFFECT_YMD.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_EFFECT_YMD.AppearanceHeader.Options.UseTextOptions = true;
         this.MG8_EFFECT_YMD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MG8_EFFECT_YMD.Caption = "生效日期";
         this.MG8_EFFECT_YMD.FieldName = "MG8_EFFECT_YMD";
         this.MG8_EFFECT_YMD.Name = "MG8_EFFECT_YMD";
         this.MG8_EFFECT_YMD.Visible = true;
         this.MG8_EFFECT_YMD.VisibleIndex = 0;
         // 
         // MG8_F_ID
         // 
         this.MG8_F_ID.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.MG8_F_ID.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_F_ID.AppearanceHeader.Options.UseTextOptions = true;
         this.MG8_F_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MG8_F_ID.Caption = "交易所+商品";
         this.MG8_F_ID.FieldName = "MG8_F_ID";
         this.MG8_F_ID.Name = "MG8_F_ID";
         this.MG8_F_ID.Visible = true;
         this.MG8_F_ID.VisibleIndex = 1;
         this.MG8_F_ID.Width = 200;
         // 
         // MG8_CM
         // 
         this.MG8_CM.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MG8_CM.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_CM.AppearanceHeader.Options.UseTextOptions = true;
         this.MG8_CM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MG8_CM.Caption = "結算保證金";
         this.MG8_CM.ColumnEdit = this.MG8_CMTextEdit;
         this.MG8_CM.FieldName = "MG8_CM";
         this.MG8_CM.Name = "MG8_CM";
         this.MG8_CM.Visible = true;
         this.MG8_CM.VisibleIndex = 2;
         this.MG8_CM.Width = 145;
         // 
         // W49050
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(899, 645);
         this.Controls.Add(this.panelControl2);
         this.Controls.Add(this.panelControl1);
         this.Name = "W49050";
         this.Text = "W49050";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartYear.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndYear.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.MG8_CMTextEdit)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.MG8_MMTextEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.MG8_IMTextEdit1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn MGT3_DATE_FM;
        private DevExpress.XtraGrid.Columns.GridColumn MGT3_DATE_TO;
        private DevExpress.XtraGrid.Columns.GridColumn MGT3_MEMO;
        private DevExpress.XtraGrid.Columns.GridColumn IS_NEWROW;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraGrid.Columns.GridColumn MGT3_W_TIME;
      private DevExpress.XtraGrid.Columns.GridColumn MGT3_W_USER_ID;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit MG8_CMTextEdit;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit MG8_MMTextEdit1;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit MG8_IMTextEdit1;
      private DevExpress.XtraGrid.Columns.GridColumn MG8_W_TIME;
      private DevExpress.XtraGrid.Columns.GridColumn MG8_W_USER_ID;
      private DevExpress.XtraGrid.Columns.GridColumn MG8_EFFECT_YMD;
      private DevExpress.XtraGrid.Columns.GridColumn MG8_F_ID;
      private DevExpress.XtraGrid.Columns.GridColumn MG8_CM;
      private DevExpress.XtraEditors.TextEdit txtStartYear;
      private DevExpress.XtraEditors.TextEdit txtEndYear;
   }
}