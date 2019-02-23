namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0010
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
            this.UPF_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_USER_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_EMPLOYEE_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_DPT_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UPF_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MODIFY_MARK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cbxDpt = new System.Windows.Forms.ComboBox();
            this.lblDpt = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panelParent
            // 
            this.panParent.Controls.Add(this.lblDpt);
            this.panParent.Controls.Add(this.cbxDpt);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(754, 536);
            // 
            // gridControlMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 44);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gridControlMain";
            this.gcMain.Size = new System.Drawing.Size(727, 477);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gridViewMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.UPF_USER_ID,
            this.UPF_USER_NAME,
            this.UPF_EMPLOYEE_ID,
            this.UPF_DPT_ID,
            this.UPF_W_TIME,
            this.MODIFY_MARK});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gridViewMain";
            // 
            // colUPF_USER_ID
            // 
            this.UPF_USER_ID.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_USER_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_USER_ID.Caption = "使用者代號";
            this.UPF_USER_ID.FieldName = "UPF_USER_ID";
            this.UPF_USER_ID.Name = "colUPF_USER_ID";
            this.UPF_USER_ID.Visible = true;
            this.UPF_USER_ID.VisibleIndex = 0;
            this.UPF_USER_ID.Width = 87;
            // 
            // colUPF_USER_NAME
            // 
            this.UPF_USER_NAME.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_USER_NAME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_USER_NAME.Caption = "使用者名稱";
            this.UPF_USER_NAME.FieldName = "UPF_USER_NAME";
            this.UPF_USER_NAME.Name = "colUPF_USER_NAME";
            this.UPF_USER_NAME.Visible = true;
            this.UPF_USER_NAME.VisibleIndex = 1;
            this.UPF_USER_NAME.Width = 85;
            // 
            // colUPF_EMPLOYEE_ID
            // 
            this.UPF_EMPLOYEE_ID.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_EMPLOYEE_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_EMPLOYEE_ID.Caption = "員工編號";
            this.UPF_EMPLOYEE_ID.FieldName = "UPF_EMPLOYEE_ID";
            this.UPF_EMPLOYEE_ID.Name = "colUPF_EMPLOYEE_ID";
            this.UPF_EMPLOYEE_ID.Visible = true;
            this.UPF_EMPLOYEE_ID.VisibleIndex = 2;
            // 
            // colUPF_DPT_ID
            // 
            this.UPF_DPT_ID.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_DPT_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_DPT_ID.Caption = "部門";
            this.UPF_DPT_ID.FieldName = "UPF_DPT_ID";
            this.UPF_DPT_ID.Name = "colUPF_DPT_ID";
            this.UPF_DPT_ID.Visible = true;
            this.UPF_DPT_ID.VisibleIndex = 3;
            this.UPF_DPT_ID.Width = 114;
            // 
            // colUPF_W_TIME
            // 
            this.UPF_W_TIME.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.UPF_W_TIME.AppearanceCell.Options.UseBackColor = true;
            this.UPF_W_TIME.AppearanceCell.Options.UseTextOptions = true;
            this.UPF_W_TIME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UPF_W_TIME.Caption = "設定時間";
            this.UPF_W_TIME.DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.UPF_W_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.UPF_W_TIME.FieldName = "UPF_W_TIME";
            this.UPF_W_TIME.Name = "colUPF_W_TIME";
            this.UPF_W_TIME.OptionsColumn.AllowEdit = false;
            this.UPF_W_TIME.OptionsColumn.ReadOnly = true;
            this.UPF_W_TIME.Visible = true;
            this.UPF_W_TIME.VisibleIndex = 4;
            this.UPF_W_TIME.Width = 192;
            // 
            // MODIFY_MARK
            // 
            this.MODIFY_MARK.FieldName = "MODIFY_MARK";
            this.MODIFY_MARK.Name = "MODIFY_MARK";
            this.MODIFY_MARK.OptionsColumn.ReadOnly = true;
            this.MODIFY_MARK.OptionsColumn.ShowCaption = false;
            this.MODIFY_MARK.Visible = true;
            this.MODIFY_MARK.VisibleIndex = 5;
            this.MODIFY_MARK.Width = 54;
            // 
            // cbxDpt
            // 
            this.cbxDpt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDpt.FormattingEnabled = true;
            this.cbxDpt.Location = new System.Drawing.Point(119, 10);
            this.cbxDpt.Name = "cbxDpt";
            this.cbxDpt.Size = new System.Drawing.Size(174, 28);
            this.cbxDpt.TabIndex = 1;
            // 
            // lblDpt
            // 
            this.lblDpt.AutoSize = true;
            this.lblDpt.Location = new System.Drawing.Point(15, 13);
            this.lblDpt.Name = "lblDpt";
            this.lblDpt.Size = new System.Drawing.Size(89, 20);
            this.lblDpt.TabIndex = 2;
            this.lblDpt.Text = "部門代號：";
            // 
            // WZ0010
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 568);
            this.Name = "WZ0010";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        public DevExpress.XtraGrid.Columns.GridColumn UPF_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_USER_NAME;
        public DevExpress.XtraGrid.Columns.GridColumn UPF_EMPLOYEE_ID;
        public DevExpress.XtraGrid.Columns.GridColumn MODIFY_MARK;
        private System.Windows.Forms.ComboBox cbxDpt;
        private System.Windows.Forms.Label lblDpt;
        public DevExpress.XtraGrid.Columns.GridColumn UPF_DPT_ID;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_W_TIME;
    }
}