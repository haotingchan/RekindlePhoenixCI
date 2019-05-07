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
            this.TXN_INS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_DEL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_QUERY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_IMPORT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_EXPORT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_PRINT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_UPDATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_DEFAULT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_AUDIT = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.TXN_INS,
            this.TXN_DEL,
            this.TXN_QUERY,
            this.TXN_IMPORT,
            this.TXN_EXPORT,
            this.TXN_PRINT,
            this.TXN_UPDATE,
            this.TXN_DEFAULT,
            this.TXN_AUDIT,
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
            this.TXN_ID.Caption = "作業\n代號";
            this.TXN_ID.FieldName = "TXN_ID";
            this.TXN_ID.Name = "TXN_ID";
            this.TXN_ID.Visible = true;
            this.TXN_ID.VisibleIndex = 0;
            // 
            // TXN_NAME
            // 
            this.TXN_NAME.Caption = "作業名稱";
            this.TXN_NAME.FieldName = "TXN_NAME";
            this.TXN_NAME.Name = "TXN_NAME";
            this.TXN_NAME.Visible = true;
            this.TXN_NAME.VisibleIndex = 1;
            this.TXN_NAME.Width = 410;
            // 
            // TXN_INS
            // 
            this.TXN_INS.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_INS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_INS.Caption = "新\n增";
            this.TXN_INS.FieldName = "TXN_INS";
            this.TXN_INS.Name = "TXN_INS";
            this.TXN_INS.Visible = true;
            this.TXN_INS.VisibleIndex = 2;
            this.TXN_INS.Width = 30;
            // 
            // TXN_DEL
            // 
            this.TXN_DEL.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_DEL.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_DEL.Caption = "刪\n除";
            this.TXN_DEL.FieldName = "TXN_DEL";
            this.TXN_DEL.Name = "TXN_DEL";
            this.TXN_DEL.Visible = true;
            this.TXN_DEL.VisibleIndex = 3;
            this.TXN_DEL.Width = 30;
            // 
            // TXN_QUERY
            // 
            this.TXN_QUERY.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_QUERY.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_QUERY.Caption = "查\n詢";
            this.TXN_QUERY.FieldName = "TXN_QUERY";
            this.TXN_QUERY.Name = "TXN_QUERY";
            this.TXN_QUERY.Visible = true;
            this.TXN_QUERY.VisibleIndex = 4;
            this.TXN_QUERY.Width = 30;
            // 
            // TXN_IMPORT
            // 
            this.TXN_IMPORT.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_IMPORT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_IMPORT.Caption = "轉\n入";
            this.TXN_IMPORT.FieldName = "TXN_IMPORT";
            this.TXN_IMPORT.Name = "TXN_IMPORT";
            this.TXN_IMPORT.Visible = true;
            this.TXN_IMPORT.VisibleIndex = 5;
            this.TXN_IMPORT.Width = 30;
            // 
            // TXN_EXPORT
            // 
            this.TXN_EXPORT.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_EXPORT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_EXPORT.Caption = "轉\n出";
            this.TXN_EXPORT.FieldName = "TXN_EXPORT";
            this.TXN_EXPORT.Name = "TXN_EXPORT";
            this.TXN_EXPORT.Visible = true;
            this.TXN_EXPORT.VisibleIndex = 6;
            this.TXN_EXPORT.Width = 30;
            // 
            // TXN_PRINT
            // 
            this.TXN_PRINT.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_PRINT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_PRINT.Caption = "列\n印";
            this.TXN_PRINT.FieldName = "TXN_PRINT";
            this.TXN_PRINT.Name = "TXN_PRINT";
            this.TXN_PRINT.Visible = true;
            this.TXN_PRINT.VisibleIndex = 7;
            this.TXN_PRINT.Width = 30;
            // 
            // TXN_UPDATE
            // 
            this.TXN_UPDATE.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_UPDATE.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_UPDATE.Caption = "變\n更";
            this.TXN_UPDATE.FieldName = "TXN_UPDATE";
            this.TXN_UPDATE.Name = "TXN_UPDATE";
            this.TXN_UPDATE.Visible = true;
            this.TXN_UPDATE.VisibleIndex = 8;
            this.TXN_UPDATE.Width = 30;
            // 
            // TXN_DEFAULT
            // 
            this.TXN_DEFAULT.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_DEFAULT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_DEFAULT.Caption = "預\n設";
            this.TXN_DEFAULT.FieldName = "TXN_DEFAULT";
            this.TXN_DEFAULT.Name = "TXN_DEFAULT";
            this.TXN_DEFAULT.Visible = true;
            this.TXN_DEFAULT.VisibleIndex = 9;
            this.TXN_DEFAULT.Width = 30;
            // 
            // TXN_AUDIT
            // 
            this.TXN_AUDIT.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_AUDIT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_AUDIT.Caption = "檢\n核";
            this.TXN_AUDIT.FieldName = "TXN_AUDIT";
            this.TXN_AUDIT.Name = "TXN_AUDIT";
            this.TXN_AUDIT.Visible = true;
            this.TXN_AUDIT.VisibleIndex = 10;
            this.TXN_AUDIT.Width = 30;
            // 
            // TXN_RMARK
            // 
            this.TXN_RMARK.Caption = "備註";
            this.TXN_RMARK.FieldName = "TXN_RMARK";
            this.TXN_RMARK.Name = "TXN_RMARK";
            this.TXN_RMARK.Visible = true;
            this.TXN_RMARK.VisibleIndex = 11;
            this.TXN_RMARK.Width = 355;
            // 
            // TXN_W_TIME
            // 
            this.TXN_W_TIME.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXN_W_TIME.AppearanceCell.Options.UseFont = true;
            this.TXN_W_TIME.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_W_TIME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_W_TIME.Caption = "設定時間";
            this.TXN_W_TIME.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.TXN_W_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.TXN_W_TIME.FieldName = "TXN_W_TIME";
            this.TXN_W_TIME.Name = "TXN_W_TIME";
            this.TXN_W_TIME.OptionsColumn.AllowEdit = false;
            this.TXN_W_TIME.Visible = true;
            this.TXN_W_TIME.VisibleIndex = 12;
            this.TXN_W_TIME.Width = 143;
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
        private DevExpress.XtraGrid.Columns.GridColumn TXN_INS;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_DEL;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_QUERY;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_IMPORT;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_EXPORT;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_PRINT;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_UPDATE;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_DEFAULT;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_AUDIT;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_RMARK;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_W_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_ID_ORG;
    }
}