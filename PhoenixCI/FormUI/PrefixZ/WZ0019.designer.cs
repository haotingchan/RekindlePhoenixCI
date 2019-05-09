namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0019
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
            this.DPT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_USER_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UTP_TXN_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TXN_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblDpt = new System.Windows.Forms.Label();
            this.ddlDept = new DevExpress.XtraEditors.LookUpEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDept.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.ddlDept);
            this.panParent.Controls.Add(this.lblDpt);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(838, 542);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(838, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 49);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(811, 478);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DPT_NAME,
            this.UPF_USER_ID,
            this.UPF_USER_NAME,
            this.UTP_TXN_ID,
            this.TXN_NAME});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsBehavior.Editable = false;
            this.gvMain.OptionsView.AllowCellMerge = true;
            // 
            // DPT_NAME
            // 
            this.DPT_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.DPT_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DPT_NAME.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.DPT_NAME.Caption = "部門";
            this.DPT_NAME.FieldName = "DPT_NAME";
            this.DPT_NAME.Name = "DPT_NAME";
            this.DPT_NAME.Visible = true;
            this.DPT_NAME.VisibleIndex = 0;
            this.DPT_NAME.Width = 110;
            // 
            // UPF_USER_ID
            // 
            this.UPF_USER_ID.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_USER_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_USER_ID.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.UPF_USER_ID.Caption = "使用者代號";
            this.UPF_USER_ID.FieldName = "UPF_USER_ID";
            this.UPF_USER_ID.Name = "UPF_USER_ID";
            this.UPF_USER_ID.Visible = true;
            this.UPF_USER_ID.VisibleIndex = 1;
            this.UPF_USER_ID.Width = 100;
            // 
            // UPF_USER_NAME
            // 
            this.UPF_USER_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_USER_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_USER_NAME.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.UPF_USER_NAME.Caption = "使用者名稱";
            this.UPF_USER_NAME.FieldName = "UPF_USER_NAME";
            this.UPF_USER_NAME.Name = "UPF_USER_NAME";
            this.UPF_USER_NAME.Visible = true;
            this.UPF_USER_NAME.VisibleIndex = 2;
            this.UPF_USER_NAME.Width = 100;
            // 
            // UTP_TXN_ID
            // 
            this.UTP_TXN_ID.AppearanceCell.Options.UseTextOptions = true;
            this.UTP_TXN_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UTP_TXN_ID.Caption = "作業代號";
            this.UTP_TXN_ID.FieldName = "UTP_TXN_ID";
            this.UTP_TXN_ID.Name = "UTP_TXN_ID";
            this.UTP_TXN_ID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.UTP_TXN_ID.Visible = true;
            this.UTP_TXN_ID.VisibleIndex = 3;
            this.UTP_TXN_ID.Width = 85;
            // 
            // TXN_NAME
            // 
            this.TXN_NAME.Caption = "作業名稱";
            this.TXN_NAME.FieldName = "TXN_NAME";
            this.TXN_NAME.Name = "TXN_NAME";
            this.TXN_NAME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.TXN_NAME.Visible = true;
            this.TXN_NAME.VisibleIndex = 4;
            this.TXN_NAME.Width = 386;
            // 
            // lblDpt
            // 
            this.lblDpt.AutoSize = true;
            this.lblDpt.Location = new System.Drawing.Point(15, 18);
            this.lblDpt.Name = "lblDpt";
            this.lblDpt.Size = new System.Drawing.Size(89, 20);
            this.lblDpt.TabIndex = 2;
            this.lblDpt.Text = "部門代號：";
            // 
            // ddlDept
            // 
            this.ddlDept.Location = new System.Drawing.Point(110, 15);
            this.ddlDept.MenuManager = this.ribbonControl;
            this.ddlDept.Name = "ddlDept";
            this.ddlDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddlDept.Size = new System.Drawing.Size(160, 26);
            this.ddlDept.TabIndex = 14;
            // 
            // WZ0019
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 572);
            this.Name = "WZ0019";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDept.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label lblDpt;
        private DevExpress.XtraGrid.Columns.GridColumn DPT_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_USER_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn UTP_TXN_ID;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_NAME;
        private DevExpress.XtraEditors.LookUpEdit ddlDept;
    }
}