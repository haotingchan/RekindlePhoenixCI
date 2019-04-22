namespace PhoenixCI.FormUI.Prefix5
{
    partial class W51010
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
            this.label1 = new System.Windows.Forms.Label();
            this.TXTEndDate = new BaseGround.Widget.TextDateEdit();
            this.TXTStartDate = new BaseGround.Widget.TextDateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DTS_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextDateEdit1 = new BaseGround.Widget.RepositoryItemTextDateEdit();
            this.DTS_DATE_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DTS_WORK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemFontEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemFontEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TXTEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TXTStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemFontEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.BackColor = System.Drawing.Color.Transparent;
            this.panParent.Size = new System.Drawing.Size(739, 557);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(739, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.TXTEndDate);
            this.panelControl1.Controls.Add(this.TXTStartDate);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(739, 61);
            this.panelControl1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "交易日期";
            // 
            // TXTEndDate
            // 
            this.TXTEndDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.TXTEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.TXTEndDate.EditValue = "0001/1/1 上午 12:00:00";
            this.TXTEndDate.Location = new System.Drawing.Point(320, 5);
            this.TXTEndDate.Name = "TXTEndDate";
            this.TXTEndDate.Properties.Appearance.Options.UseTextOptions = true;
            this.TXTEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXTEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.TXTEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.TXTEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.TXTEndDate.Size = new System.Drawing.Size(150, 26);
            this.TXTEndDate.TabIndex = 12;
            this.TXTEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // TXTStartDate
            // 
            this.TXTStartDate.DateTimeValue = new System.DateTime(((long)(0)));
            this.TXTStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
            this.TXTStartDate.EditValue = "0001/1/1 上午 12:00:00";
            this.TXTStartDate.Location = new System.Drawing.Point(127, 5);
            this.TXTStartDate.Name = "TXTStartDate";
            this.TXTStartDate.Properties.Appearance.Options.UseTextOptions = true;
            this.TXTStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TXTStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.TXTStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.TXTStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.TXTStartDate.Size = new System.Drawing.Size(150, 26);
            this.TXTStartDate.TabIndex = 11;
            this.TXTStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePromptAndLiterals;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "~";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcMain);
            this.panelControl2.Controls.Add(this.label4);
            this.panelControl2.Controls.Add(this.label3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 91);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(739, 496);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcMain.Location = new System.Drawing.Point(2, 2);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.Name = "gcMain";
            this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemFontEdit1,
            this.repositoryItemTextDateEdit1});
            this.gcMain.Size = new System.Drawing.Size(735, 399);
            this.gcMain.TabIndex = 7;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DTS_DATE,
            this.DTS_DATE_TYPE,
            this.DTS_WORK,
            this.Is_NewRow});
            this.gvMain.CustomizationFormBounds = new System.Drawing.Rectangle(440, 407, 385, 454);
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsCustomization.AllowSort = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // DTS_DATE
            // 
            this.DTS_DATE.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.DTS_DATE.AppearanceCell.Options.UseBackColor = true;
            this.DTS_DATE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.DTS_DATE.AppearanceHeader.Options.UseBackColor = true;
            this.DTS_DATE.Caption = "日期";
            this.DTS_DATE.ColumnEdit = this.repositoryItemTextDateEdit1;
            this.DTS_DATE.FieldName = "DTS_DATE";
            this.DTS_DATE.MinWidth = 30;
            this.DTS_DATE.Name = "DTS_DATE";
            this.DTS_DATE.OptionsFilter.AllowFilter = false;
            this.DTS_DATE.Visible = true;
            this.DTS_DATE.VisibleIndex = 0;
            this.DTS_DATE.Width = 112;
            // 
            // repositoryItemTextDateEdit1
            // 
            this.repositoryItemTextDateEdit1.Appearance.Options.UseTextOptions = true;
            this.repositoryItemTextDateEdit1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.repositoryItemTextDateEdit1.AutoHeight = false;
            this.repositoryItemTextDateEdit1.Mask.EditMask = "yyyy/MM/dd";
            this.repositoryItemTextDateEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.repositoryItemTextDateEdit1.Mask.UseMaskAsDisplayFormat = true;
            this.repositoryItemTextDateEdit1.Name = "repositoryItemTextDateEdit1";
            // 
            // DTS_DATE_TYPE
            // 
            this.DTS_DATE_TYPE.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DTS_DATE_TYPE.AppearanceHeader.Options.UseBackColor = true;
            this.DTS_DATE_TYPE.Caption = "日期類型";
            this.DTS_DATE_TYPE.FieldName = "DTS_DATE_TYPE";
            this.DTS_DATE_TYPE.MinWidth = 30;
            this.DTS_DATE_TYPE.Name = "DTS_DATE_TYPE";
            this.DTS_DATE_TYPE.Visible = true;
            this.DTS_DATE_TYPE.VisibleIndex = 1;
            this.DTS_DATE_TYPE.Width = 112;
            // 
            // DTS_WORK
            // 
            this.DTS_WORK.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DTS_WORK.AppearanceHeader.Options.UseBackColor = true;
            this.DTS_WORK.Caption = "是否交易";
            this.DTS_WORK.FieldName = "DTS_WORK";
            this.DTS_WORK.MinWidth = 30;
            this.DTS_WORK.Name = "DTS_WORK";
            this.DTS_WORK.Visible = true;
            this.DTS_WORK.VisibleIndex = 2;
            this.DTS_WORK.Width = 112;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "IS_NEWROW";
            this.Is_NewRow.MinWidth = 30;
            this.Is_NewRow.Name = "Is_NewRow";
            this.Is_NewRow.Width = 112;
            // 
            // repositoryItemFontEdit1
            // 
            this.repositoryItemFontEdit1.AutoHeight = false;
            this.repositoryItemFontEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemFontEdit1.Name = "repositoryItemFontEdit1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 440);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "上班日 : 週一至週五";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 410);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "假日 : 週六週日";
            // 
            // W51010
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 587);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W51010";
            this.Text = "W51010";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TXTEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TXTStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemFontEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn DTS_DATE;
        private BaseGround.Widget.RepositoryItemTextDateEdit repositoryItemTextDateEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn DTS_DATE_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn DTS_WORK;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
        private DevExpress.XtraEditors.Repository.RepositoryItemFontEdit repositoryItemFontEdit1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private BaseGround.Widget.TextDateEdit TXTEndDate;
        private BaseGround.Widget.TextDateEdit TXTStartDate;
        private System.Windows.Forms.Label label2;
    }
}