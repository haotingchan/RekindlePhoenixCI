namespace PhoenixCI.FormUI.Prefix5
{
    partial class W56090
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.grpxDescription = new System.Windows.Forms.GroupBox();
            this.txtYM = new System.Windows.Forms.MaskedTextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.ImportShow = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFEETDCC_YM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_FCM_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_DISC_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_DISC_RATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_ORG_AR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_DISC_AMT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_ACC_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFEETDCC_SESSION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.grpxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Size = new System.Drawing.Size(813, 536);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(813, 43);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.grpxDescription);
            this.panelControl1.Controls.Add(this.ImportShow);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 43);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(813, 209);
            this.panelControl1.TabIndex = 0;
            // 
            // grpxDescription
            // 
            this.grpxDescription.AutoSize = true;
            this.grpxDescription.Controls.Add(this.txtYM);
            this.grpxDescription.Controls.Add(this.lblDate);
            this.grpxDescription.Location = new System.Drawing.Point(12, 12);
            this.grpxDescription.Name = "grpxDescription";
            this.grpxDescription.Size = new System.Drawing.Size(404, 120);
            this.grpxDescription.TabIndex = 11;
            this.grpxDescription.TabStop = false;
            this.grpxDescription.Text = "請輸入交易日期";
            // 
            // txtYM
            // 
            this.txtYM.Location = new System.Drawing.Point(117, 43);
            this.txtYM.Mask = "0000/00";
            this.txtYM.Name = "txtYM";
            this.txtYM.Size = new System.Drawing.Size(152, 39);
            this.txtYM.TabIndex = 11;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(37, 46);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(85, 30);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "月份：";
            // 
            // ImportShow
            // 
            this.ImportShow.AutoSize = true;
            this.ImportShow.Location = new System.Drawing.Point(7, 135);
            this.ImportShow.Name = "ImportShow";
            this.ImportShow.Size = new System.Drawing.Size(81, 30);
            this.ImportShow.TabIndex = 12;
            this.ImportShow.Text = "label1";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcMain);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 252);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(813, 327);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(2, 2);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(809, 323);
            this.gcMain.TabIndex = 10;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain,
            this.gridView1});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFEETDCC_YM,
            this.colFEETDCC_FCM_NO,
            this.colFEETDCC_KIND_ID,
            this.colFEETDCC_DISC_QNTY,
            this.colFEETDCC_DISC_RATE,
            this.colFEETDCC_ORG_AR,
            this.colFEETDCC_DISC_AMT,
            this.colFEETDCC_W_USER_ID,
            this.colFEETDCC_W_TIME,
            this.colFEETDCC_ACC_NO,
            this.colFEETDCC_SESSION});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsBehavior.Editable = false;
            this.gvMain.OptionsCustomization.AllowSort = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            // 
            // colFEETDCC_YM
            // 
            this.colFEETDCC_YM.FieldName = "FEETDCC_YM";
            this.colFEETDCC_YM.MinWidth = 30;
            this.colFEETDCC_YM.Name = "colFEETDCC_YM";
            this.colFEETDCC_YM.Visible = true;
            this.colFEETDCC_YM.VisibleIndex = 0;
            this.colFEETDCC_YM.Width = 112;
            // 
            // colFEETDCC_FCM_NO
            // 
            this.colFEETDCC_FCM_NO.FieldName = "FEETDCC_FCM_NO";
            this.colFEETDCC_FCM_NO.MinWidth = 30;
            this.colFEETDCC_FCM_NO.Name = "colFEETDCC_FCM_NO";
            this.colFEETDCC_FCM_NO.Visible = true;
            this.colFEETDCC_FCM_NO.VisibleIndex = 1;
            this.colFEETDCC_FCM_NO.Width = 112;
            // 
            // colFEETDCC_KIND_ID
            // 
            this.colFEETDCC_KIND_ID.FieldName = "FEETDCC_KIND_ID";
            this.colFEETDCC_KIND_ID.MinWidth = 30;
            this.colFEETDCC_KIND_ID.Name = "colFEETDCC_KIND_ID";
            this.colFEETDCC_KIND_ID.Visible = true;
            this.colFEETDCC_KIND_ID.VisibleIndex = 2;
            this.colFEETDCC_KIND_ID.Width = 112;
            // 
            // colFEETDCC_DISC_QNTY
            // 
            this.colFEETDCC_DISC_QNTY.FieldName = "FEETDCC_DISC_QNTY";
            this.colFEETDCC_DISC_QNTY.MinWidth = 30;
            this.colFEETDCC_DISC_QNTY.Name = "colFEETDCC_DISC_QNTY";
            this.colFEETDCC_DISC_QNTY.Visible = true;
            this.colFEETDCC_DISC_QNTY.VisibleIndex = 3;
            this.colFEETDCC_DISC_QNTY.Width = 112;
            // 
            // colFEETDCC_DISC_RATE
            // 
            this.colFEETDCC_DISC_RATE.FieldName = "FEETDCC_DISC_RATE";
            this.colFEETDCC_DISC_RATE.MinWidth = 30;
            this.colFEETDCC_DISC_RATE.Name = "colFEETDCC_DISC_RATE";
            this.colFEETDCC_DISC_RATE.Visible = true;
            this.colFEETDCC_DISC_RATE.VisibleIndex = 4;
            this.colFEETDCC_DISC_RATE.Width = 112;
            // 
            // colFEETDCC_ORG_AR
            // 
            this.colFEETDCC_ORG_AR.FieldName = "FEETDCC_ORG_AR";
            this.colFEETDCC_ORG_AR.MinWidth = 30;
            this.colFEETDCC_ORG_AR.Name = "colFEETDCC_ORG_AR";
            this.colFEETDCC_ORG_AR.Visible = true;
            this.colFEETDCC_ORG_AR.VisibleIndex = 5;
            this.colFEETDCC_ORG_AR.Width = 112;
            // 
            // colFEETDCC_DISC_AMT
            // 
            this.colFEETDCC_DISC_AMT.FieldName = "FEETDCC_DISC_AMT";
            this.colFEETDCC_DISC_AMT.MinWidth = 30;
            this.colFEETDCC_DISC_AMT.Name = "colFEETDCC_DISC_AMT";
            this.colFEETDCC_DISC_AMT.Visible = true;
            this.colFEETDCC_DISC_AMT.VisibleIndex = 6;
            this.colFEETDCC_DISC_AMT.Width = 112;
            // 
            // colFEETDCC_W_USER_ID
            // 
            this.colFEETDCC_W_USER_ID.FieldName = "FEETDCC_W_USER_ID";
            this.colFEETDCC_W_USER_ID.MinWidth = 30;
            this.colFEETDCC_W_USER_ID.Name = "colFEETDCC_W_USER_ID";
            this.colFEETDCC_W_USER_ID.Visible = true;
            this.colFEETDCC_W_USER_ID.VisibleIndex = 7;
            this.colFEETDCC_W_USER_ID.Width = 112;
            // 
            // colFEETDCC_W_TIME
            // 
            this.colFEETDCC_W_TIME.FieldName = "FEETDCC_W_TIME";
            this.colFEETDCC_W_TIME.MinWidth = 30;
            this.colFEETDCC_W_TIME.Name = "colFEETDCC_W_TIME";
            this.colFEETDCC_W_TIME.Visible = true;
            this.colFEETDCC_W_TIME.VisibleIndex = 8;
            this.colFEETDCC_W_TIME.Width = 112;
            // 
            // colFEETDCC_ACC_NO
            // 
            this.colFEETDCC_ACC_NO.FieldName = "FEETDCC_ACC_NO";
            this.colFEETDCC_ACC_NO.MinWidth = 30;
            this.colFEETDCC_ACC_NO.Name = "colFEETDCC_ACC_NO";
            this.colFEETDCC_ACC_NO.Visible = true;
            this.colFEETDCC_ACC_NO.VisibleIndex = 9;
            this.colFEETDCC_ACC_NO.Width = 112;
            // 
            // colFEETDCC_SESSION
            // 
            this.colFEETDCC_SESSION.FieldName = "FEETDCC_SESSION";
            this.colFEETDCC_SESSION.MinWidth = 30;
            this.colFEETDCC_SESSION.Name = "colFEETDCC_SESSION";
            this.colFEETDCC_SESSION.Visible = true;
            this.colFEETDCC_SESSION.VisibleIndex = 10;
            this.colFEETDCC_SESSION.Width = 112;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcMain;
            this.gridView1.Name = "gridView1";
            // 
            // W56090
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 579);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W56090";
            this.Text = "W56090";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.grpxDescription.ResumeLayout(false);
            this.grpxDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_YM;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_FCM_NO;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_DISC_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_DISC_RATE;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_ORG_AR;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_DISC_AMT;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_W_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_W_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_ACC_NO;
        private DevExpress.XtraGrid.Columns.GridColumn colFEETDCC_SESSION;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.GroupBox grpxDescription;
        private System.Windows.Forms.MaskedTextBox txtYM;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label ImportShow;
    }
}