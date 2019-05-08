namespace PhoenixCI.FormUI.Prefix6
{
    partial class W60311
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
            this.RPTF_TXN_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPTF_TXD_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPTF_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPTF_SEQ_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RPTF_TEXT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblYear = new System.Windows.Forms.Label();
            this.txtYear = new DevExpress.XtraEditors.TextEdit();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.txtYear);
            this.panParent.Controls.Add(this.lblYear);
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(1027, 538);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1027, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcMain.Location = new System.Drawing.Point(12, 44);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1000, 479);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.RPTF_TXN_ID,
            this.RPTF_TXD_ID,
            this.RPTF_KEY,
            this.RPTF_SEQ_NO,
            this.RPTF_TEXT});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            // 
            // RPTF_TXN_ID
            // 
            this.RPTF_TXN_ID.FieldName = "RPTF_TXN_ID";
            this.RPTF_TXN_ID.Name = "RPTF_TXN_ID";
            // 
            // RPTF_TXD_ID
            // 
            this.RPTF_TXD_ID.FieldName = "RPTF_TXD_ID";
            this.RPTF_TXD_ID.Name = "RPTF_TXD_ID";
            // 
            // RPTF_KEY
            // 
            this.RPTF_KEY.AppearanceCell.Options.UseTextOptions = true;
            this.RPTF_KEY.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.RPTF_KEY.Caption = "年度";
            this.RPTF_KEY.FieldName = "RPTF_KEY";
            this.RPTF_KEY.Name = "RPTF_KEY";
            this.RPTF_KEY.Visible = true;
            this.RPTF_KEY.VisibleIndex = 0;
            this.RPTF_KEY.Width = 55;
            // 
            // RPTF_SEQ_NO
            // 
            this.RPTF_SEQ_NO.AppearanceCell.Options.UseTextOptions = true;
            this.RPTF_SEQ_NO.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.RPTF_SEQ_NO.Caption = "順序";
            this.RPTF_SEQ_NO.FieldName = "RPTF_SEQ_NO";
            this.RPTF_SEQ_NO.Name = "RPTF_SEQ_NO";
            this.RPTF_SEQ_NO.Visible = true;
            this.RPTF_SEQ_NO.VisibleIndex = 1;
            this.RPTF_SEQ_NO.Width = 56;
            // 
            // RPTF_TEXT
            // 
            this.RPTF_TEXT.Caption = "文字說明";
            this.RPTF_TEXT.FieldName = "RPTF_TEXT";
            this.RPTF_TEXT.Name = "RPTF_TEXT";
            this.RPTF_TEXT.Visible = true;
            this.RPTF_TEXT.VisibleIndex = 2;
            this.RPTF_TEXT.Width = 871;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(15, 13);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(57, 20);
            this.lblYear.TabIndex = 2;
            this.lblYear.Text = "年度：";
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(78, 10);
            this.txtYear.Name = "txtYear";
            this.txtYear.Properties.Appearance.Options.UseTextOptions = true;
            this.txtYear.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtYear.Properties.MaxLength = 4;
            this.txtYear.Size = new System.Drawing.Size(63, 26);
            this.txtYear.TabIndex = 3;
            // 
            // W60311
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 568);
            this.Name = "W60311";
            this.Text = "FormChild";
            this.panParent.ResumeLayout(false);
            this.panParent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYear.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private System.Windows.Forms.Label lblYear;
        private DevExpress.XtraEditors.TextEdit txtYear;
        private DevExpress.XtraGrid.Columns.GridColumn RPTF_TXN_ID;
        private DevExpress.XtraGrid.Columns.GridColumn RPTF_TXD_ID;
        private DevExpress.XtraGrid.Columns.GridColumn RPTF_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn RPTF_SEQ_NO;
        private DevExpress.XtraGrid.Columns.GridColumn RPTF_TEXT;
    }
}