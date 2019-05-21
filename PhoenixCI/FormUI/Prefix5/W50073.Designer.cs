namespace PhoenixCI.FormUI.Prefix5 {
    partial class W50073 {
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
            this.RWD_REF_OMNI_ACTIVITY_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RWD_REF_OMNI_FCM_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RWD_REF_OMNI_ACC_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RWD_REF_OMNI_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(826, 691);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(826, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(12, 12);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemTextEdit3});
            this.gcMain.Size = new System.Drawing.Size(802, 667);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.RWD_REF_OMNI_ACTIVITY_ID,
            this.RWD_REF_OMNI_FCM_NO,
            this.RWD_REF_OMNI_ACC_NO,
            this.RWD_REF_OMNI_NAME});
            this.gvMain.CustomizationFormBounds = new System.Drawing.Rectangle(476, 341, 322, 375);
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            // 
            // RWD_REF_OMNI_ACTIVITY_ID
            // 
            this.RWD_REF_OMNI_ACTIVITY_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RWD_REF_OMNI_ACTIVITY_ID.AppearanceHeader.Options.UseBackColor = true;
            this.RWD_REF_OMNI_ACTIVITY_ID.Caption = "活動名稱";
            this.RWD_REF_OMNI_ACTIVITY_ID.FieldName = "RWD_REF_OMNI_ACTIVITY_ID";
            this.RWD_REF_OMNI_ACTIVITY_ID.MinWidth = 25;
            this.RWD_REF_OMNI_ACTIVITY_ID.Name = "RWD_REF_OMNI_ACTIVITY_ID";
            this.RWD_REF_OMNI_ACTIVITY_ID.Visible = true;
            this.RWD_REF_OMNI_ACTIVITY_ID.VisibleIndex = 0;
            this.RWD_REF_OMNI_ACTIVITY_ID.Width = 148;
            // 
            // RWD_REF_OMNI_FCM_NO
            // 
            this.RWD_REF_OMNI_FCM_NO.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RWD_REF_OMNI_FCM_NO.AppearanceHeader.Options.UseBackColor = true;
            this.RWD_REF_OMNI_FCM_NO.Caption = "期貨商代號";
            this.RWD_REF_OMNI_FCM_NO.ColumnEdit = this.repositoryItemTextEdit3;
            this.RWD_REF_OMNI_FCM_NO.FieldName = "RWD_REF_OMNI_FCM_NO";
            this.RWD_REF_OMNI_FCM_NO.MinWidth = 25;
            this.RWD_REF_OMNI_FCM_NO.Name = "RWD_REF_OMNI_FCM_NO";
            this.RWD_REF_OMNI_FCM_NO.Visible = true;
            this.RWD_REF_OMNI_FCM_NO.VisibleIndex = 1;
            this.RWD_REF_OMNI_FCM_NO.Width = 136;
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.MaxLength = 7;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // RWD_REF_OMNI_ACC_NO
            // 
            this.RWD_REF_OMNI_ACC_NO.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RWD_REF_OMNI_ACC_NO.AppearanceHeader.Options.UseBackColor = true;
            this.RWD_REF_OMNI_ACC_NO.Caption = "交易人帳號";
            this.RWD_REF_OMNI_ACC_NO.ColumnEdit = this.repositoryItemTextEdit2;
            this.RWD_REF_OMNI_ACC_NO.FieldName = "RWD_REF_OMNI_ACC_NO";
            this.RWD_REF_OMNI_ACC_NO.MinWidth = 25;
            this.RWD_REF_OMNI_ACC_NO.Name = "RWD_REF_OMNI_ACC_NO";
            this.RWD_REF_OMNI_ACC_NO.Visible = true;
            this.RWD_REF_OMNI_ACC_NO.VisibleIndex = 2;
            this.RWD_REF_OMNI_ACC_NO.Width = 136;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.MaxLength = 7;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // RWD_REF_OMNI_NAME
            // 
            this.RWD_REF_OMNI_NAME.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RWD_REF_OMNI_NAME.AppearanceHeader.Options.UseBackColor = true;
            this.RWD_REF_OMNI_NAME.Caption = "法人機構名稱";
            this.RWD_REF_OMNI_NAME.ColumnEdit = this.repositoryItemTextEdit1;
            this.RWD_REF_OMNI_NAME.FieldName = "RWD_REF_OMNI_NAME";
            this.RWD_REF_OMNI_NAME.Name = "RWD_REF_OMNI_NAME";
            this.RWD_REF_OMNI_NAME.OptionsColumn.FixedWidth = true;
            this.RWD_REF_OMNI_NAME.Visible = true;
            this.RWD_REF_OMNI_NAME.VisibleIndex = 3;
            this.RWD_REF_OMNI_NAME.Width = 250;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.MaxLength = 120;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "系統別";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 98;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "系統別";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 98;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "系統別";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 98;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "交易人帳號";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 151;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "交易人帳號";
            this.gridColumn5.MinWidth = 25;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 151;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "交易人帳號";
            this.gridColumn6.MinWidth = 25;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            this.gridColumn6.Width = 151;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "盤別";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            this.gridColumn7.Width = 156;
            // 
            // W50073
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 721);
            this.Name = "W50073";
            this.Text = "W50073";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn RWD_REF_OMNI_ACTIVITY_ID;
        private DevExpress.XtraGrid.Columns.GridColumn RWD_REF_OMNI_FCM_NO;
        private DevExpress.XtraGrid.Columns.GridColumn RWD_REF_OMNI_ACC_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn RWD_REF_OMNI_NAME;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
    }
}