namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0020
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
            this.TXN_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_LEVEL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_PARENT_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_SEQ_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_EXTEND = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_DEFAULT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_RMARK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_ID_ORG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(1306, 542);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1306, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(12, 12);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1282, 518);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.ColumnPanelRowHeight = 50;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.TXN_ID,
            this.TXN_NAME,
            this.TXN_TYPE,
            this.TXN_LEVEL,
            this.TXN_PARENT_ID,
            this.TXN_SEQ_NO,
            this.TXN_EXTEND,
            this.TXN_DEFAULT,
            this.TXN_RMARK,
            this.TXN_W_TIME,
            this.TXN_ID_ORG});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            // 
            // TXN_ID
            // 
            this.TXN_ID.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.TXN_ID.AppearanceHeader.ForeColor = System.Drawing.Color.Maroon;
            this.TXN_ID.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_ID.AppearanceHeader.Options.UseForeColor = true;
            this.TXN_ID.Caption = "作業\n代號";
            this.TXN_ID.FieldName = "TXN_ID";
            this.TXN_ID.Name = "TXN_ID";
            this.TXN_ID.Visible = true;
            this.TXN_ID.VisibleIndex = 0;
            this.TXN_ID.Width = 68;
            // 
            // TXN_NAME
            // 
            this.TXN_NAME.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_NAME.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_NAME.Caption = "作業名稱";
            this.TXN_NAME.FieldName = "TXN_NAME";
            this.TXN_NAME.Name = "TXN_NAME";
            this.TXN_NAME.Visible = true;
            this.TXN_NAME.VisibleIndex = 1;
            this.TXN_NAME.Width = 372;
            // 
            // TXN_TYPE
            // 
            this.TXN_TYPE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_TYPE.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_TYPE.Caption = "作業類型";
            this.TXN_TYPE.FieldName = "TXN_TYPE";
            this.TXN_TYPE.Name = "TXN_TYPE";
            this.TXN_TYPE.Visible = true;
            this.TXN_TYPE.VisibleIndex = 2;
            this.TXN_TYPE.Width = 72;
            // 
            // TXN_LEVEL
            // 
            this.TXN_LEVEL.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_LEVEL.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_LEVEL.Caption = "層級";
            this.TXN_LEVEL.FieldName = "TXN_LEVEL";
            this.TXN_LEVEL.Name = "TXN_LEVEL";
            this.TXN_LEVEL.Visible = true;
            this.TXN_LEVEL.VisibleIndex = 3;
            this.TXN_LEVEL.Width = 52;
            // 
            // TXN_PARENT_ID
            // 
            this.TXN_PARENT_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_PARENT_ID.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_PARENT_ID.Caption = "上層\n作業代號";
            this.TXN_PARENT_ID.FieldName = "TXN_PARENT_ID";
            this.TXN_PARENT_ID.Name = "TXN_PARENT_ID";
            this.TXN_PARENT_ID.Visible = true;
            this.TXN_PARENT_ID.VisibleIndex = 4;
            this.TXN_PARENT_ID.Width = 68;
            // 
            // TXN_SEQ_NO
            // 
            this.TXN_SEQ_NO.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_SEQ_NO.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_SEQ_NO.Caption = "排序";
            this.TXN_SEQ_NO.FieldName = "TXN_SEQ_NO";
            this.TXN_SEQ_NO.Name = "TXN_SEQ_NO";
            this.TXN_SEQ_NO.Visible = true;
            this.TXN_SEQ_NO.VisibleIndex = 5;
            this.TXN_SEQ_NO.Width = 53;
            // 
            // TXN_EXTEND
            // 
            this.TXN_EXTEND.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_EXTEND.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_EXTEND.Caption = "自動\n展開";
            this.TXN_EXTEND.FieldName = "TXN_EXTEND";
            this.TXN_EXTEND.Name = "TXN_EXTEND";
            this.TXN_EXTEND.Visible = true;
            this.TXN_EXTEND.VisibleIndex = 6;
            this.TXN_EXTEND.Width = 38;
            // 
            // TXN_DEFAULT
            // 
            this.TXN_DEFAULT.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_DEFAULT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_DEFAULT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_DEFAULT.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_DEFAULT.Caption = "預\n設";
            this.TXN_DEFAULT.FieldName = "TXN_DEFAULT";
            this.TXN_DEFAULT.Name = "TXN_DEFAULT";
            this.TXN_DEFAULT.Visible = true;
            this.TXN_DEFAULT.VisibleIndex = 7;
            this.TXN_DEFAULT.Width = 27;
            // 
            // TXN_RMARK
            // 
            this.TXN_RMARK.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_RMARK.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_RMARK.Caption = "備註";
            this.TXN_RMARK.FieldName = "TXN_RMARK";
            this.TXN_RMARK.Name = "TXN_RMARK";
            this.TXN_RMARK.Visible = true;
            this.TXN_RMARK.VisibleIndex = 8;
            this.TXN_RMARK.Width = 357;
            // 
            // TXN_W_TIME
            // 
            this.TXN_W_TIME.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_W_TIME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_W_TIME.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TXN_W_TIME.AppearanceHeader.Options.UseBackColor = true;
            this.TXN_W_TIME.Caption = "設定時間";
            this.TXN_W_TIME.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.TXN_W_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.TXN_W_TIME.FieldName = "TXN_W_TIME";
            this.TXN_W_TIME.Name = "TXN_W_TIME";
            this.TXN_W_TIME.OptionsColumn.AllowEdit = false;
            this.TXN_W_TIME.Visible = true;
            this.TXN_W_TIME.VisibleIndex = 9;
            this.TXN_W_TIME.Width = 154;
            // 
            // TXN_ID_ORG
            // 
            this.TXN_ID_ORG.FieldName = "TXN_ID";
            this.TXN_ID_ORG.Name = "TXN_ID_ORG";
            // 
            // WZ0020
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1306, 572);
            this.Name = "WZ0020";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_ID;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_DEFAULT;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_RMARK;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_W_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_ID_ORG;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_LEVEL;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_PARENT_ID;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_SEQ_NO;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_EXTEND;
    }
}