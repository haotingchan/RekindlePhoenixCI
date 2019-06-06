namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20233 {
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.STKOUT_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STKOUT_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STKOUT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STKOUT_B = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STKOUT_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Location = new System.Drawing.Point(0, 113);
            this.panParent.Size = new System.Drawing.Size(814, 437);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(814, 43);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.txtDate);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 43);
            this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(814, 70);
            this.panelControl1.TabIndex = 0;
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.txtDate.EditValue = "2018/12/01";
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(118, 22);
            this.txtDate.MenuManager = this.ribbonControl;
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtDate.Properties.EditFormat.FormatString = "yyyyMMdd";
            this.txtDate.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
            this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDate.Size = new System.Drawing.Size(118, 40);
            this.txtDate.TabIndex = 13;
            this.txtDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(32, 25);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(133, 30);
            this.lblDate.TabIndex = 14;
            this.lblDate.Text = "資料日期：";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 113);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(814, 437);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(18, 18);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(778, 401);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.STKOUT_DATE,
            this.STKOUT_ID,
            this.STKOUT_NAME,
            this.STKOUT_B,
            this.STKOUT_TIME,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // STKOUT_DATE
            // 
            this.STKOUT_DATE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.STKOUT_DATE.AppearanceHeader.Options.UseBackColor = true;
            this.STKOUT_DATE.AppearanceHeader.Options.UseTextOptions = true;
            this.STKOUT_DATE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STKOUT_DATE.Caption = "資料日期";
            this.STKOUT_DATE.FieldName = "STKOUT_DATE";
            this.STKOUT_DATE.Name = "STKOUT_DATE";
            this.STKOUT_DATE.OptionsColumn.FixedWidth = true;
            this.STKOUT_DATE.Visible = true;
            this.STKOUT_DATE.VisibleIndex = 0;
            this.STKOUT_DATE.Width = 100;
            // 
            // STKOUT_ID
            // 
            this.STKOUT_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.STKOUT_ID.AppearanceHeader.Options.UseBackColor = true;
            this.STKOUT_ID.AppearanceHeader.Options.UseTextOptions = true;
            this.STKOUT_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STKOUT_ID.Caption = "股票代號";
            this.STKOUT_ID.FieldName = "STKOUT_ID";
            this.STKOUT_ID.Name = "STKOUT_ID";
            this.STKOUT_ID.OptionsColumn.FixedWidth = true;
            this.STKOUT_ID.Visible = true;
            this.STKOUT_ID.VisibleIndex = 1;
            this.STKOUT_ID.Width = 100;
            // 
            // STKOUT_NAME
            // 
            this.STKOUT_NAME.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.STKOUT_NAME.AppearanceHeader.Options.UseBackColor = true;
            this.STKOUT_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.STKOUT_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STKOUT_NAME.Caption = "股票名稱";
            this.STKOUT_NAME.FieldName = "STKOUT_NAME";
            this.STKOUT_NAME.Name = "STKOUT_NAME";
            this.STKOUT_NAME.OptionsColumn.FixedWidth = true;
            this.STKOUT_NAME.Visible = true;
            this.STKOUT_NAME.VisibleIndex = 2;
            this.STKOUT_NAME.Width = 200;
            // 
            // STKOUT_B
            // 
            this.STKOUT_B.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.STKOUT_B.AppearanceHeader.Options.UseBackColor = true;
            this.STKOUT_B.AppearanceHeader.Options.UseTextOptions = true;
            this.STKOUT_B.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STKOUT_B.Caption = "在外流通股數(B)";
            this.STKOUT_B.DisplayFormat.FormatString = "0,00";
            this.STKOUT_B.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.STKOUT_B.FieldName = "STKOUT_B";
            this.STKOUT_B.Name = "STKOUT_B";
            this.STKOUT_B.OptionsColumn.FixedWidth = true;
            this.STKOUT_B.Visible = true;
            this.STKOUT_B.VisibleIndex = 3;
            this.STKOUT_B.Width = 160;
            // 
            // STKOUT_TIME
            // 
            this.STKOUT_TIME.Caption = "STKOUT_TIME";
            this.STKOUT_TIME.FieldName = "STKOUT_TIME";
            this.STKOUT_TIME.Name = "STKOUT_TIME";
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // W20233
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 550);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20233";
            this.Text = "W20233";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private BaseGround.Widget.TextDateEdit txtDate;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn STKOUT_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn STKOUT_ID;
        private DevExpress.XtraGrid.Columns.GridColumn STKOUT_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn STKOUT_B;
        private DevExpress.XtraGrid.Columns.GridColumn STKOUT_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
    }
}