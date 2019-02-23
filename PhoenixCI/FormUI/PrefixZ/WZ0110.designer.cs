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
            this.cbxUserId = new System.Windows.Forms.ComboBox();
            this.lblUserId = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.cbxUserId);
            this.panParent.Controls.Add(this.lblUserId);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(698, 530);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(698, 38);
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
            this.gcMain.Size = new System.Drawing.Size(674, 473);
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
            // cbxUserId
            // 
            this.cbxUserId.BackColor = System.Drawing.Color.White;
            this.cbxUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUserId.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxUserId.ForeColor = System.Drawing.Color.Black;
            this.cbxUserId.FormattingEnabled = true;
            this.cbxUserId.Location = new System.Drawing.Point(119, 9);
            this.cbxUserId.Name = "cbxUserId";
            this.cbxUserId.Size = new System.Drawing.Size(169, 33);
            this.cbxUserId.TabIndex = 7;
            this.cbxUserId.SelectedIndexChanged += new System.EventHandler(this.cbxUserId_SelectedIndexChanged);
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(8, 12);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(132, 25);
            this.lblUserId.TabIndex = 6;
            this.lblUserId.Text = "使用者代號：";
            // 
            // WZ0110
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 568);
            this.Name = "WZ0110";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
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
        private DevExpress.XtraGrid.Columns.GridColumn MODIFY_MARK;
        private DevExpress.XtraGrid.Columns.GridColumn UTP_FLAG;
        private System.Windows.Forms.ComboBox cbxUserId;
        private System.Windows.Forms.Label lblUserId;
    }
}