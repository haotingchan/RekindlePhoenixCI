namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0112
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
            this.DPT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_USER_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MODIFY_MARK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblTXN = new System.Windows.Forms.Label();
            this.lblDpt = new System.Windows.Forms.Label();
            this.ddlDept = new DevExpress.XtraEditors.LookUpEdit();
            this.ddlTxnId = new DevExpress.XtraEditors.LookUpEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTxnId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.ddlTxnId);
            this.panParent.Controls.Add(this.ddlDept);
            this.panParent.Controls.Add(this.lblDpt);
            this.panParent.Controls.Add(this.lblTXN);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(815, 538);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(815, 30);
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
            this.gcMain.Size = new System.Drawing.Size(791, 481);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.ColumnPanelRowHeight = 50;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.UTP_FLAG,
            this.DPT_NAME,
            this.UPF_USER_ID,
            this.UPF_USER_NAME,
            this.MODIFY_MARK});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            // 
            // UTP_FLAG
            // 
            this.UTP_FLAG.Caption = "勾選\n權限";
            this.UTP_FLAG.FieldName = "UTP_FLAG";
            this.UTP_FLAG.Name = "UTP_FLAG";
            this.UTP_FLAG.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.UTP_FLAG.Visible = true;
            this.UTP_FLAG.VisibleIndex = 0;
            this.UTP_FLAG.Width = 45;
            // 
            // DPT_NAME
            // 
            this.DPT_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.DPT_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DPT_NAME.Caption = "部門";
            this.DPT_NAME.FieldName = "DPT_NAME";
            this.DPT_NAME.Name = "DPT_NAME";
            this.DPT_NAME.OptionsColumn.AllowEdit = false;
            this.DPT_NAME.Visible = true;
            this.DPT_NAME.VisibleIndex = 1;
            this.DPT_NAME.Width = 257;
            // 
            // UPF_USER_ID
            // 
            this.UPF_USER_ID.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_USER_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_USER_ID.Caption = "使用者代號";
            this.UPF_USER_ID.FieldName = "UPF_USER_ID";
            this.UPF_USER_ID.Name = "UPF_USER_ID";
            this.UPF_USER_ID.OptionsColumn.AllowEdit = false;
            this.UPF_USER_ID.Visible = true;
            this.UPF_USER_ID.VisibleIndex = 2;
            this.UPF_USER_ID.Width = 161;
            // 
            // UPF_USER_NAME
            // 
            this.UPF_USER_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_USER_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_USER_NAME.Caption = "使用者名稱";
            this.UPF_USER_NAME.FieldName = "UPF_USER_NAME";
            this.UPF_USER_NAME.Name = "UPF_USER_NAME";
            this.UPF_USER_NAME.OptionsColumn.AllowEdit = false;
            this.UPF_USER_NAME.Visible = true;
            this.UPF_USER_NAME.VisibleIndex = 3;
            this.UPF_USER_NAME.Width = 250;
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
            this.MODIFY_MARK.Width = 57;
            // 
            // lblTXN
            // 
            this.lblTXN.AutoSize = true;
            this.lblTXN.Location = new System.Drawing.Point(8, 12);
            this.lblTXN.Name = "lblTXN";
            this.lblTXN.Size = new System.Drawing.Size(89, 20);
            this.lblTXN.TabIndex = 6;
            this.lblTXN.Text = "作業代號：";
            // 
            // lblDpt
            // 
            this.lblDpt.AutoSize = true;
            this.lblDpt.Location = new System.Drawing.Point(566, 12);
            this.lblDpt.Name = "lblDpt";
            this.lblDpt.Size = new System.Drawing.Size(57, 20);
            this.lblDpt.TabIndex = 9;
            this.lblDpt.Text = "部門：";
            // 
            // ddlDept
            // 
            this.ddlDept.Location = new System.Drawing.Point(629, 9);
            this.ddlDept.MenuManager = this.ribbonControl;
            this.ddlDept.Name = "ddlDept";
            this.ddlDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlDept.Size = new System.Drawing.Size(160, 26);
            this.ddlDept.TabIndex = 14;
            this.ddlDept.EditValueChanged += new System.EventHandler(this.ddlDept_EditValueChanged);
            // 
            // ddlTxnId
            // 
            this.ddlTxnId.Location = new System.Drawing.Point(94, 9);
            this.ddlTxnId.MenuManager = this.ribbonControl;
            this.ddlTxnId.Name = "ddlTxnId";
            this.ddlTxnId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlTxnId.Size = new System.Drawing.Size(466, 26);
            this.ddlTxnId.TabIndex = 15;
            this.ddlTxnId.EditValueChanged += new System.EventHandler(this.ddlTxnId_EditValueChanged);
            // 
            // WZ0112
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 568);
            this.Name = "WZ0112";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlTxnId.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn MODIFY_MARK;
        private DevExpress.XtraGrid.Columns.GridColumn UTP_FLAG;
        private System.Windows.Forms.Label lblTXN;
        private DevExpress.XtraGrid.Columns.GridColumn DPT_NAME;
        private System.Windows.Forms.Label lblDpt;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_USER_NAME;
        private DevExpress.XtraEditors.LookUpEdit ddlDept;
        private DevExpress.XtraEditors.LookUpEdit ddlTxnId;
    }
}