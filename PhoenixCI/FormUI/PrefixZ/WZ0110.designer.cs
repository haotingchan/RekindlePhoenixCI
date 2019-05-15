namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0110
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
            this.UTP_FLAG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_DEFAULT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MODIFY_MARK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblUserId = new System.Windows.Forms.Label();
            this.ddlUserId = new DevExpress.XtraEditors.LookUpEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUserId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.ddlUserId);
            this.panParent.Controls.Add(this.lblUserId);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(698, 538);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(698, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 43);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(674, 481);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.ColumnPanelRowHeight = 50;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.UTP_FLAG,
            this.TXN_ID,
            this.TXN_NAME,
            this.TXN_DEFAULT,
            this.MODIFY_MARK});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridViewMain_ShowingEditor);
            // 
            // UTP_FLAG
            // 
            this.UTP_FLAG.Caption = "勾選\n權限";
            this.UTP_FLAG.FieldName = "UTP_FLAG";
            this.UTP_FLAG.Name = "UTP_FLAG";
            this.UTP_FLAG.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.UTP_FLAG.Visible = true;
            this.UTP_FLAG.VisibleIndex = 0;
            this.UTP_FLAG.Width = 85;
            // 
            // TXN_ID
            // 
            this.TXN_ID.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_ID.Caption = "作業代號";
            this.TXN_ID.FieldName = "TXN_ID";
            this.TXN_ID.Name = "TXN_ID";
            this.TXN_ID.OptionsColumn.AllowEdit = false;
            this.TXN_ID.Visible = true;
            this.TXN_ID.VisibleIndex = 1;
            this.TXN_ID.Width = 79;
            // 
            // TXN_NAME
            // 
            this.TXN_NAME.Caption = "作業名稱";
            this.TXN_NAME.FieldName = "TXN_NAME";
            this.TXN_NAME.Name = "TXN_NAME";
            this.TXN_NAME.OptionsColumn.AllowEdit = false;
            this.TXN_NAME.Visible = true;
            this.TXN_NAME.VisibleIndex = 2;
            this.TXN_NAME.Width = 407;
            // 
            // TXN_DEFAULT
            // 
            this.TXN_DEFAULT.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_DEFAULT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_DEFAULT.Caption = "預設";
            this.TXN_DEFAULT.FieldName = "TXN_DEFAULT";
            this.TXN_DEFAULT.Name = "TXN_DEFAULT";
            this.TXN_DEFAULT.OptionsColumn.AllowEdit = false;
            this.TXN_DEFAULT.Visible = true;
            this.TXN_DEFAULT.VisibleIndex = 3;
            this.TXN_DEFAULT.Width = 42;
            // 
            // MODIFY_MARK
            // 
            this.MODIFY_MARK.AppearanceCell.Options.UseTextOptions = true;
            this.MODIFY_MARK.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MODIFY_MARK.FieldName = "MODIFY_MARK";
            this.MODIFY_MARK.Name = "MODIFY_MARK";
            this.MODIFY_MARK.OptionsColumn.AllowEdit = false;
            this.MODIFY_MARK.OptionsColumn.ShowCaption = false;
            this.MODIFY_MARK.Visible = true;
            this.MODIFY_MARK.VisibleIndex = 4;
            this.MODIFY_MARK.Width = 41;
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(8, 12);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(105, 20);
            this.lblUserId.TabIndex = 6;
            this.lblUserId.Text = "使用者代號：";
            // 
            // ddlUserId
            // 
            this.ddlUserId.Location = new System.Drawing.Point(119, 9);
            this.ddlUserId.MenuManager = this.ribbonControl;
            this.ddlUserId.Name = "ddlUserId";
            this.ddlUserId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlUserId.Size = new System.Drawing.Size(205, 26);
            this.ddlUserId.TabIndex = 15;
            this.ddlUserId.EditValueChanged += new System.EventHandler(this.ddlUserId_EditValueChanged);
            // 
            // WZ0110
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 568);
            this.Name = "WZ0110";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlUserId.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_ID;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_DEFAULT;
        private DevExpress.XtraGrid.Columns.GridColumn MODIFY_MARK;
        private DevExpress.XtraGrid.Columns.GridColumn UTP_FLAG;
        private System.Windows.Forms.Label lblUserId;
        private DevExpress.XtraEditors.LookUpEdit ddlUserId;
    }
}