namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20220 {
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
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PLT1_PROD_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLT1_PROD_SUBTYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLT1_QNTY_MIN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLT1_QNTY_MAX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLT1_MULTIPLE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLT1_MIN_NATURE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PLT1_MIN_LEGAL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(836, 531);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(836, 43);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(18, 18);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(800, 495);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PLT1_PROD_TYPE,
            this.PLT1_PROD_SUBTYPE,
            this.PLT1_QNTY_MIN,
            this.PLT1_QNTY_MAX,
            this.PLT1_MULTIPLE,
            this.PLT1_MIN_NATURE,
            this.PLT1_MIN_LEGAL,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // PLT1_PROD_TYPE
            // 
            this.PLT1_PROD_TYPE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.PLT1_PROD_TYPE.AppearanceHeader.Options.UseBackColor = true;
            this.PLT1_PROD_TYPE.Caption = "商品別";
            this.PLT1_PROD_TYPE.FieldName = "PLT1_PROD_TYPE";
            this.PLT1_PROD_TYPE.MaxWidth = 100;
            this.PLT1_PROD_TYPE.Name = "PLT1_PROD_TYPE";
            this.PLT1_PROD_TYPE.OptionsColumn.FixedWidth = true;
            this.PLT1_PROD_TYPE.Visible = true;
            this.PLT1_PROD_TYPE.VisibleIndex = 0;
            this.PLT1_PROD_TYPE.Width = 100;
            // 
            // PLT1_PROD_SUBTYPE
            // 
            this.PLT1_PROD_SUBTYPE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.PLT1_PROD_SUBTYPE.AppearanceHeader.Options.UseBackColor = true;
            this.PLT1_PROD_SUBTYPE.Caption = "商品子類別";
            this.PLT1_PROD_SUBTYPE.FieldName = "PLT1_PROD_SUBTYPE";
            this.PLT1_PROD_SUBTYPE.MaxWidth = 100;
            this.PLT1_PROD_SUBTYPE.Name = "PLT1_PROD_SUBTYPE";
            this.PLT1_PROD_SUBTYPE.OptionsColumn.FixedWidth = true;
            this.PLT1_PROD_SUBTYPE.Visible = true;
            this.PLT1_PROD_SUBTYPE.VisibleIndex = 1;
            this.PLT1_PROD_SUBTYPE.Width = 100;
            // 
            // PLT1_QNTY_MIN
            // 
            this.PLT1_QNTY_MIN.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PLT1_QNTY_MIN.AppearanceHeader.Options.UseBackColor = true;
            this.PLT1_QNTY_MIN.Caption = "最小契約數";
            this.PLT1_QNTY_MIN.FieldName = "PLT1_QNTY_MIN";
            this.PLT1_QNTY_MIN.MaxWidth = 100;
            this.PLT1_QNTY_MIN.Name = "PLT1_QNTY_MIN";
            this.PLT1_QNTY_MIN.OptionsColumn.FixedWidth = true;
            this.PLT1_QNTY_MIN.Visible = true;
            this.PLT1_QNTY_MIN.VisibleIndex = 2;
            this.PLT1_QNTY_MIN.Width = 100;
            // 
            // PLT1_QNTY_MAX
            // 
            this.PLT1_QNTY_MAX.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PLT1_QNTY_MAX.AppearanceHeader.Options.UseBackColor = true;
            this.PLT1_QNTY_MAX.Caption = "最大契約數";
            this.PLT1_QNTY_MAX.FieldName = "PLT1_QNTY_MAX";
            this.PLT1_QNTY_MAX.MaxWidth = 100;
            this.PLT1_QNTY_MAX.Name = "PLT1_QNTY_MAX";
            this.PLT1_QNTY_MAX.OptionsColumn.FixedWidth = true;
            this.PLT1_QNTY_MAX.Visible = true;
            this.PLT1_QNTY_MAX.VisibleIndex = 3;
            this.PLT1_QNTY_MAX.Width = 100;
            // 
            // PLT1_MULTIPLE
            // 
            this.PLT1_MULTIPLE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PLT1_MULTIPLE.AppearanceHeader.Options.UseBackColor = true;
            this.PLT1_MULTIPLE.Caption = "倍數";
            this.PLT1_MULTIPLE.FieldName = "PLT1_MULTIPLE";
            this.PLT1_MULTIPLE.MaxWidth = 100;
            this.PLT1_MULTIPLE.Name = "PLT1_MULTIPLE";
            this.PLT1_MULTIPLE.OptionsColumn.FixedWidth = true;
            this.PLT1_MULTIPLE.Visible = true;
            this.PLT1_MULTIPLE.VisibleIndex = 4;
            this.PLT1_MULTIPLE.Width = 100;
            // 
            // PLT1_MIN_NATURE
            // 
            this.PLT1_MIN_NATURE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PLT1_MIN_NATURE.AppearanceHeader.Options.UseBackColor = true;
            this.PLT1_MIN_NATURE.Caption = "自然人最低數";
            this.PLT1_MIN_NATURE.FieldName = "PLT1_MIN_NATURE";
            this.PLT1_MIN_NATURE.MaxWidth = 110;
            this.PLT1_MIN_NATURE.Name = "PLT1_MIN_NATURE";
            this.PLT1_MIN_NATURE.OptionsColumn.FixedWidth = true;
            this.PLT1_MIN_NATURE.Visible = true;
            this.PLT1_MIN_NATURE.VisibleIndex = 5;
            this.PLT1_MIN_NATURE.Width = 110;
            // 
            // PLT1_MIN_LEGAL
            // 
            this.PLT1_MIN_LEGAL.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PLT1_MIN_LEGAL.AppearanceHeader.Options.UseBackColor = true;
            this.PLT1_MIN_LEGAL.Caption = "法人最低數";
            this.PLT1_MIN_LEGAL.FieldName = "PLT1_MIN_LEGAL";
            this.PLT1_MIN_LEGAL.MaxWidth = 100;
            this.PLT1_MIN_LEGAL.Name = "PLT1_MIN_LEGAL";
            this.PLT1_MIN_LEGAL.OptionsColumn.FixedWidth = true;
            this.PLT1_MIN_LEGAL.Visible = true;
            this.PLT1_MIN_LEGAL.VisibleIndex = 6;
            this.PLT1_MIN_LEGAL.Width = 100;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // W20220
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 574);
            this.Name = "W20220";
            this.Text = "W20220";
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
        private DevExpress.XtraGrid.Columns.GridColumn PLT1_PROD_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn PLT1_PROD_SUBTYPE;
        private DevExpress.XtraGrid.Columns.GridColumn PLT1_QNTY_MIN;
        private DevExpress.XtraGrid.Columns.GridColumn PLT1_QNTY_MAX;
        private DevExpress.XtraGrid.Columns.GridColumn PLT1_MULTIPLE;
        private DevExpress.XtraGrid.Columns.GridColumn PLT1_MIN_NATURE;
        private DevExpress.XtraGrid.Columns.GridColumn PLT1_MIN_LEGAL;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
    }
}