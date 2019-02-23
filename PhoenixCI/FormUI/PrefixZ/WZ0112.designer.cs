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
            this.cbxTxnId = new System.Windows.Forms.ComboBox();
            this.lblTXN = new System.Windows.Forms.Label();
            this.lblDpt = new System.Windows.Forms.Label();
            this.cbxDpt = new System.Windows.Forms.ComboBox();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.lblDpt);
            this.panParent.Controls.Add(this.cbxDpt);
            this.panParent.Controls.Add(this.cbxTxnId);
            this.panParent.Controls.Add(this.lblTXN);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(815, 536);
            // 
            // gcMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 43);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(791, 479);
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
            // cbxTxnId
            // 
            this.cbxTxnId.BackColor = System.Drawing.Color.White;
            this.cbxTxnId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTxnId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxTxnId.ForeColor = System.Drawing.Color.Black;
            this.cbxTxnId.FormattingEnabled = true;
            this.cbxTxnId.Location = new System.Drawing.Point(94, 9);
            this.cbxTxnId.Name = "cbxTxnId";
            this.cbxTxnId.Size = new System.Drawing.Size(466, 28);
            this.cbxTxnId.TabIndex = 7;
            this.cbxTxnId.SelectedIndexChanged += new System.EventHandler(this.cbxTxnId_SelectedIndexChanged);
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
            // cbxDpt
            // 
            this.cbxDpt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDpt.FormattingEnabled = true;
            this.cbxDpt.Location = new System.Drawing.Point(629, 9);
            this.cbxDpt.Name = "cbxDpt";
            this.cbxDpt.Size = new System.Drawing.Size(174, 28);
            this.cbxDpt.TabIndex = 8;
            this.cbxDpt.SelectedIndexChanged += new System.EventHandler(this.cbxDpt_SelectedIndexChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn MODIFY_MARK;
        private DevExpress.XtraGrid.Columns.GridColumn UTP_FLAG;
        private System.Windows.Forms.ComboBox cbxTxnId;
        private System.Windows.Forms.Label lblTXN;
        private DevExpress.XtraGrid.Columns.GridColumn DPT_NAME;
        private System.Windows.Forms.Label lblDpt;
        private System.Windows.Forms.ComboBox cbxDpt;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn UPF_USER_NAME;
    }
}