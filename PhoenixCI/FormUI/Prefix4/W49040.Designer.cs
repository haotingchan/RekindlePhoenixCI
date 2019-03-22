namespace PhoenixCI.FormUI.Prefix4 {
    partial class W49040 {
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
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.MGT4_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT4_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT4_CUR_M_MULTI = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT4_CUR_I_MULTI = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT4_CUR_DIGITAL = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT4_M_MULTI = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT4_I_MULTI = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT4_DIGITAL = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MGT4_M_DIGITAL = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Size = new System.Drawing.Size(1035, 617);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(1035, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl2
         // 
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 30);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(1035, 617);
         this.panelControl2.TabIndex = 1;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(12, 12);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(1011, 593);
         this.gcMain.TabIndex = 0;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.MGT4_KIND_ID,
            this.MGT4_TYPE,
            this.MGT4_CUR_M_MULTI,
            this.MGT4_CUR_I_MULTI,
            this.MGT4_CUR_DIGITAL,
            this.MGT4_M_MULTI,
            this.MGT4_I_MULTI,
            this.MGT4_DIGITAL,
            this.MGT4_M_DIGITAL,
            this.Is_NewRow});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         // 
         // MGT4_KIND_ID
         // 
         this.MGT4_KIND_ID.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.MGT4_KIND_ID.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_KIND_ID.AppearanceHeader.Options.UseTextOptions = true;
         this.MGT4_KIND_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MGT4_KIND_ID.Caption = "商品";
         this.MGT4_KIND_ID.FieldName = "MGT4_KIND_ID";
         this.MGT4_KIND_ID.Name = "MGT4_KIND_ID";
         this.MGT4_KIND_ID.Visible = true;
         this.MGT4_KIND_ID.VisibleIndex = 0;
         // 
         // MGT4_TYPE
         // 
         this.MGT4_TYPE.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.MGT4_TYPE.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_TYPE.AppearanceHeader.Options.UseTextOptions = true;
         this.MGT4_TYPE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MGT4_TYPE.Caption = "類別";
         this.MGT4_TYPE.FieldName = "MGT4_TYPE";
         this.MGT4_TYPE.Name = "MGT4_TYPE";
         this.MGT4_TYPE.Visible = true;
         this.MGT4_TYPE.VisibleIndex = 1;
         this.MGT4_TYPE.Width = 200;
         // 
         // MGT4_CUR_M_MULTI
         // 
         this.MGT4_CUR_M_MULTI.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MGT4_CUR_M_MULTI.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_CUR_M_MULTI.AppearanceHeader.Options.UseTextOptions = true;
         this.MGT4_CUR_M_MULTI.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MGT4_CUR_M_MULTI.Caption = "現行維持比率";
         this.MGT4_CUR_M_MULTI.FieldName = "MGT4_CUR_M_MULTI";
         this.MGT4_CUR_M_MULTI.Name = "MGT4_CUR_M_MULTI";
         this.MGT4_CUR_M_MULTI.Visible = true;
         this.MGT4_CUR_M_MULTI.VisibleIndex = 2;
         this.MGT4_CUR_M_MULTI.Width = 145;
         // 
         // MGT4_CUR_I_MULTI
         // 
         this.MGT4_CUR_I_MULTI.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MGT4_CUR_I_MULTI.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_CUR_I_MULTI.AppearanceHeader.Options.UseTextOptions = true;
         this.MGT4_CUR_I_MULTI.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MGT4_CUR_I_MULTI.Caption = "現行原始比率";
         this.MGT4_CUR_I_MULTI.FieldName = "MGT4_CUR_I_MULTI";
         this.MGT4_CUR_I_MULTI.Name = "MGT4_CUR_I_MULTI";
         this.MGT4_CUR_I_MULTI.Visible = true;
         this.MGT4_CUR_I_MULTI.VisibleIndex = 3;
         this.MGT4_CUR_I_MULTI.Width = 120;
         // 
         // MGT4_CUR_DIGITAL
         // 
         this.MGT4_CUR_DIGITAL.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MGT4_CUR_DIGITAL.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_CUR_DIGITAL.AppearanceHeader.Options.UseTextOptions = true;
         this.MGT4_CUR_DIGITAL.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MGT4_CUR_DIGITAL.Caption = "現行進位數";
         this.MGT4_CUR_DIGITAL.FieldName = "MGT4_CUR_DIGITAL";
         this.MGT4_CUR_DIGITAL.Name = "MGT4_CUR_DIGITAL";
         this.MGT4_CUR_DIGITAL.Visible = true;
         this.MGT4_CUR_DIGITAL.VisibleIndex = 4;
         this.MGT4_CUR_DIGITAL.Width = 145;
         // 
         // MGT4_M_MULTI
         // 
         this.MGT4_M_MULTI.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MGT4_M_MULTI.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_M_MULTI.Caption = "本日維持比率";
         this.MGT4_M_MULTI.FieldName = "MGT4_M_MULTI";
         this.MGT4_M_MULTI.Name = "MGT4_M_MULTI";
         this.MGT4_M_MULTI.Visible = true;
         this.MGT4_M_MULTI.VisibleIndex = 5;
         // 
         // MGT4_I_MULTI
         // 
         this.MGT4_I_MULTI.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MGT4_I_MULTI.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_I_MULTI.Caption = "本日原始比率";
         this.MGT4_I_MULTI.FieldName = "MGT4_I_MULTI";
         this.MGT4_I_MULTI.Name = "MGT4_I_MULTI";
         this.MGT4_I_MULTI.Visible = true;
         this.MGT4_I_MULTI.VisibleIndex = 6;
         // 
         // MGT4_DIGITAL
         // 
         this.MGT4_DIGITAL.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MGT4_DIGITAL.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_DIGITAL.Caption = "本日進位數";
         this.MGT4_DIGITAL.FieldName = "MGT4_DIGITAL";
         this.MGT4_DIGITAL.Name = "MGT4_DIGITAL";
         this.MGT4_DIGITAL.Visible = true;
         this.MGT4_DIGITAL.VisibleIndex = 7;
         // 
         // MGT4_M_DIGITAL
         // 
         this.MGT4_M_DIGITAL.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MGT4_M_DIGITAL.AppearanceHeader.Options.UseBackColor = true;
         this.MGT4_M_DIGITAL.Caption = "本日結算進位數";
         this.MGT4_M_DIGITAL.FieldName = "MGT4_M_DIGITAL";
         this.MGT4_M_DIGITAL.Name = "MGT4_M_DIGITAL";
         this.MGT4_M_DIGITAL.Visible = true;
         this.MGT4_M_DIGITAL.VisibleIndex = 8;
         // 
         // Is_NewRow
         // 
         this.Is_NewRow.Caption = "Is_NewRow";
         this.Is_NewRow.FieldName = "Is_NewRow";
         this.Is_NewRow.Name = "Is_NewRow";
         // 
         // W49040
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1035, 647);
         this.Controls.Add(this.panelControl2);
         this.Name = "W49040";
         this.Text = "W49040";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn MGT4_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn MGT4_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn MGT4_CUR_M_MULTI;
        private DevExpress.XtraGrid.Columns.GridColumn MGT4_CUR_I_MULTI;
        private DevExpress.XtraGrid.Columns.GridColumn MGT4_CUR_DIGITAL;
        private DevExpress.XtraGrid.Columns.GridColumn MGT4_M_MULTI;
        private DevExpress.XtraGrid.Columns.GridColumn MGT4_I_MULTI;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
      private DevExpress.XtraGrid.Columns.GridColumn MGT4_DIGITAL;
      private DevExpress.XtraGrid.Columns.GridColumn MGT4_M_DIGITAL;
   }
}