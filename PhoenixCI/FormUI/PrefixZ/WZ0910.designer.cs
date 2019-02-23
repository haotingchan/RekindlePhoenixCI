namespace PhoenixCI.FormUI.PrefixZ
{
    partial class WZ0910
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
            this.btnPrintEmpty = new System.Windows.Forms.Button();
            this.lblUserIdDesc = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.lblUserIdDesc);
            this.panParent.Controls.Add(this.lblUserId);
            this.panParent.Controls.Add(this.btnPrintEmpty);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(698, 536);
            // 
            // gcMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 51);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(674, 471);
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
            this.TXN_DEFAULT});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsBehavior.Editable = false;
            // 
            // UTP_FLAG
            // 
            this.UTP_FLAG.Caption = "勾選\n權限";
            this.UTP_FLAG.FieldName = "FLAG";
            this.UTP_FLAG.Name = "UTP_FLAG";
            this.UTP_FLAG.UnboundExpression = "Iif([TXN_DEFAULT] = \'Y\', \' \', [UTP_FLAG])";
            this.UTP_FLAG.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.UTP_FLAG.Visible = true;
            this.UTP_FLAG.VisibleIndex = 0;
            this.UTP_FLAG.Width = 45;
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
            this.TXN_ID.Width = 85;
            // 
            // TXN_NAME
            // 
            this.TXN_NAME.Caption = "作業名稱";
            this.TXN_NAME.FieldName = "TXN_NAME";
            this.TXN_NAME.Name = "TXN_NAME";
            this.TXN_NAME.OptionsColumn.AllowEdit = false;
            this.TXN_NAME.Visible = true;
            this.TXN_NAME.VisibleIndex = 2;
            this.TXN_NAME.Width = 436;
            // 
            // TXN_DEFAULT
            // 
            this.TXN_DEFAULT.AppearanceCell.Options.UseTextOptions = true;
            this.TXN_DEFAULT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXN_DEFAULT.Caption = "預設";
            this.TXN_DEFAULT.FieldName = "TXN_DEFAULT_FLAG";
            this.TXN_DEFAULT.Name = "TXN_DEFAULT";
            this.TXN_DEFAULT.OptionsColumn.AllowEdit = false;
            this.TXN_DEFAULT.UnboundExpression = "Iif([TXN_DEFAULT] = \'Y\', \'Y\', \' \')";
            this.TXN_DEFAULT.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.TXN_DEFAULT.Visible = true;
            this.TXN_DEFAULT.VisibleIndex = 3;
            this.TXN_DEFAULT.Width = 45;
            // 
            // btnPrintEmpty
            // 
            this.btnPrintEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintEmpty.Location = new System.Drawing.Point(533, 15);
            this.btnPrintEmpty.Name = "btnPrintEmpty";
            this.btnPrintEmpty.Size = new System.Drawing.Size(150, 30);
            this.btnPrintEmpty.TabIndex = 1;
            this.btnPrintEmpty.Text = "列印空白權限表";
            this.btnPrintEmpty.UseVisualStyleBackColor = true;
            this.btnPrintEmpty.Click += new System.EventHandler(this.btnPrintEmpty_Click);
            // 
            // lblUserIdDesc
            // 
            this.lblUserIdDesc.AutoSize = true;
            this.lblUserIdDesc.Location = new System.Drawing.Point(12, 20);
            this.lblUserIdDesc.Name = "lblUserIdDesc";
            this.lblUserIdDesc.Size = new System.Drawing.Size(105, 20);
            this.lblUserIdDesc.TabIndex = 10;
            this.lblUserIdDesc.Text = "使用者代號：";
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(123, 20);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(0, 20);
            this.lblUserId.TabIndex = 11;
            // 
            // WZ0910
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 568);
            this.Name = "WZ0910";
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
        private DevExpress.XtraGrid.Columns.GridColumn TXN_ID;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn TXN_DEFAULT;
        private DevExpress.XtraGrid.Columns.GridColumn UTP_FLAG;
        private System.Windows.Forms.Button btnPrintEmpty;
        private System.Windows.Forms.Label lblUserIdDesc;
        private System.Windows.Forms.Label lblUserId;
    }
}