﻿namespace PhoenixCI.FormUI.Prefix5 {
    partial class W51040 {
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
            this.MMWK_PROD_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtMonth = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.MMWK_YM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMWK_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMWK_WEIGHT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.MMWK_W_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MMWK_W_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Location = new System.Drawing.Point(0, 102);
            this.panParent.Size = new System.Drawing.Size(801, 408);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(801, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // MMWK_PROD_TYPE
            // 
            this.MMWK_PROD_TYPE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMWK_PROD_TYPE.AppearanceHeader.Options.UseBackColor = true;
            this.MMWK_PROD_TYPE.Caption = "商品別";
            this.MMWK_PROD_TYPE.FieldName = "MMWK_PROD_TYPE";
            this.MMWK_PROD_TYPE.Name = "MMWK_PROD_TYPE";
            this.MMWK_PROD_TYPE.Visible = true;
            this.MMWK_PROD_TYPE.VisibleIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.txtMonth);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(801, 72);
            this.panelControl1.TabIndex = 0;
            // 
            // txtMonth
            // 
            this.txtMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtMonth.EditValue = "2018/12";
            this.txtMonth.EnterMoveNextControl = true;
            this.txtMonth.Location = new System.Drawing.Point(88, 21);
            this.txtMonth.MenuManager = this.ribbonControl;
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtMonth.Properties.EditFormat.FormatString = "yyyyMM";
            this.txtMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
            this.txtMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtMonth.Properties.Mask.ShowPlaceHolders = false;
            this.txtMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtMonth.Size = new System.Drawing.Size(100, 28);
            this.txtMonth.TabIndex = 8;
            this.txtMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(25, 24);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 7;
            this.lblDate.Text = "月份：";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 102);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(801, 408);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Left;
            this.gcMain.Location = new System.Drawing.Point(12, 12);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.gcMain.Size = new System.Drawing.Size(777, 384);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            this.gcMain.Visible = false;
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.MMWK_PROD_TYPE,
            this.MMWK_YM,
            this.MMWK_KIND_ID,
            this.MMWK_WEIGHT,
            this.MMWK_W_USER_ID,
            this.MMWK_W_TIME,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsPrint.AutoWidth = false;
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            this.gvMain.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gvMain_FocusedColumnChanged);
            // 
            // MMWK_YM
            // 
            this.MMWK_YM.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMWK_YM.AppearanceHeader.Options.UseBackColor = true;
            this.MMWK_YM.Caption = "年月";
            this.MMWK_YM.FieldName = "MMWK_YM";
            this.MMWK_YM.Name = "MMWK_YM";
            this.MMWK_YM.Visible = true;
            this.MMWK_YM.VisibleIndex = 1;
            this.MMWK_YM.Width = 80;
            // 
            // MMWK_KIND_ID
            // 
            this.MMWK_KIND_ID.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MMWK_KIND_ID.AppearanceHeader.Options.UseBackColor = true;
            this.MMWK_KIND_ID.Caption = "契約";
            this.MMWK_KIND_ID.FieldName = "MMWK_KIND_ID";
            this.MMWK_KIND_ID.Name = "MMWK_KIND_ID";
            this.MMWK_KIND_ID.Visible = true;
            this.MMWK_KIND_ID.VisibleIndex = 2;
            // 
            // MMWK_WEIGHT
            // 
            this.MMWK_WEIGHT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MMWK_WEIGHT.AppearanceHeader.Options.UseBackColor = true;
            this.MMWK_WEIGHT.Caption = "權重";
            this.MMWK_WEIGHT.ColumnEdit = this.repositoryItemTextEdit1;
            this.MMWK_WEIGHT.DisplayFormat.FormatString = "0.####";
            this.MMWK_WEIGHT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.MMWK_WEIGHT.FieldName = "MMWK_WEIGHT";
            this.MMWK_WEIGHT.Name = "MMWK_WEIGHT";
            this.MMWK_WEIGHT.Visible = true;
            this.MMWK_WEIGHT.VisibleIndex = 3;
            this.MMWK_WEIGHT.Width = 69;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.EditFormat.FormatString = "0.####";
            this.repositoryItemTextEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // MMWK_W_USER_ID
            // 
            this.MMWK_W_USER_ID.Caption = "MMWK_W_USER_ID";
            this.MMWK_W_USER_ID.FieldName = "MMWK_W_USER_ID";
            this.MMWK_W_USER_ID.Name = "MMWK_W_USER_ID";
            // 
            // MMWK_W_TIME
            // 
            this.MMWK_W_TIME.Caption = "MMWK_W_TIME";
            this.MMWK_W_TIME.FieldName = "MMWK_W_TIME";
            this.MMWK_W_TIME.Name = "MMWK_W_TIME";
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // W51040
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 510);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W51040";
            this.Text = "W51040";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private BaseGround.Widget.TextDateEdit txtMonth;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn MMWK_YM;
        private DevExpress.XtraGrid.Columns.GridColumn MMWK_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn MMWK_WEIGHT;
        private DevExpress.XtraGrid.Columns.GridColumn MMWK_PROD_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
        private DevExpress.XtraGrid.Columns.GridColumn MMWK_W_USER_ID;
        private DevExpress.XtraGrid.Columns.GridColumn MMWK_W_TIME;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
    }
}