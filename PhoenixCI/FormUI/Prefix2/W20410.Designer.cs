namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20410 {
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
            this.txtStartDate = new BaseGround.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEndDate = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.AM7_Y = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7_DAY_COUNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7_FUT_AVG_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7_OPT_AVG_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7_FC_TAX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7_FC_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Location = new System.Drawing.Point(0, 118);
            this.panParent.Size = new System.Drawing.Size(683, 408);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(683, 43);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.txtStartDate);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.txtEndDate);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 43);
            this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(683, 75);
            this.panelControl1.TabIndex = 0;
            // 
            // txtStartDate
            // 
            this.txtStartDate.DateTimeValue = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.txtStartDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Year;
            this.txtStartDate.EditValue = "2018/12";
            this.txtStartDate.EnterMoveNextControl = true;
            this.txtStartDate.Location = new System.Drawing.Point(88, 24);
            this.txtStartDate.MenuManager = this.ribbonControl;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtStartDate.Properties.EditFormat.FormatString = "yyyy";
            this.txtStartDate.Properties.Mask.EditMask = "0000";
            this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtStartDate.Properties.Mask.PlaceHolder = '0';
            this.txtStartDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtStartDate.Properties.MaxLength = 4;
            this.txtStartDate.Size = new System.Drawing.Size(71, 40);
            this.txtStartDate.TabIndex = 14;
            this.txtStartDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(165, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 30);
            this.label1.TabIndex = 17;
            this.label1.Text = "～";
            // 
            // txtEndDate
            // 
            this.txtEndDate.DateTimeValue = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.txtEndDate.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Year;
            this.txtEndDate.EditValue = "2018/12";
            this.txtEndDate.EnterMoveNextControl = true;
            this.txtEndDate.Location = new System.Drawing.Point(196, 24);
            this.txtEndDate.MenuManager = this.ribbonControl;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEndDate.Properties.EditFormat.FormatString = "yyyy";
            this.txtEndDate.Properties.Mask.EditMask = "0000";
            this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtEndDate.Properties.Mask.PlaceHolder = '0';
            this.txtEndDate.Properties.Mask.ShowPlaceHolders = false;
            this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEndDate.Properties.MaxLength = 4;
            this.txtEndDate.Size = new System.Drawing.Size(71, 40);
            this.txtEndDate.TabIndex = 15;
            this.txtEndDate.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(25, 27);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(85, 30);
            this.lblDate.TabIndex = 16;
            this.lblDate.Text = "年度：";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 118);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(683, 408);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(18, 18);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(647, 372);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvMain.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AM7_Y,
            this.AM7_DAY_COUNT,
            this.AM7_FUT_AVG_QNTY,
            this.AM7_OPT_AVG_QNTY,
            this.AM7_FC_TAX,
            this.AM7_FC_QNTY,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // AM7_Y
            // 
            this.AM7_Y.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.AM7_Y.AppearanceHeader.Options.UseBackColor = true;
            this.AM7_Y.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7_Y.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7_Y.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.AM7_Y.Caption = "年月";
            this.AM7_Y.FieldName = "AM7_Y";
            this.AM7_Y.Name = "AM7_Y";
            this.AM7_Y.Visible = true;
            this.AM7_Y.VisibleIndex = 0;
            // 
            // AM7_DAY_COUNT
            // 
            this.AM7_DAY_COUNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7_DAY_COUNT.AppearanceHeader.Options.UseBackColor = true;
            this.AM7_DAY_COUNT.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7_DAY_COUNT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7_DAY_COUNT.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.AM7_DAY_COUNT.Caption = "總交易天數";
            this.AM7_DAY_COUNT.FieldName = "AM7_DAY_COUNT";
            this.AM7_DAY_COUNT.Name = "AM7_DAY_COUNT";
            this.AM7_DAY_COUNT.Visible = true;
            this.AM7_DAY_COUNT.VisibleIndex = 1;
            this.AM7_DAY_COUNT.Width = 100;
            // 
            // AM7_FUT_AVG_QNTY
            // 
            this.AM7_FUT_AVG_QNTY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7_FUT_AVG_QNTY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7_FUT_AVG_QNTY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7_FUT_AVG_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7_FUT_AVG_QNTY.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.AM7_FUT_AVG_QNTY.Caption = "預估期貨目標日均量";
            this.AM7_FUT_AVG_QNTY.FieldName = "AM7_FUT_AVG_QNTY";
            this.AM7_FUT_AVG_QNTY.Name = "AM7_FUT_AVG_QNTY";
            this.AM7_FUT_AVG_QNTY.Visible = true;
            this.AM7_FUT_AVG_QNTY.VisibleIndex = 2;
            this.AM7_FUT_AVG_QNTY.Width = 120;
            // 
            // AM7_OPT_AVG_QNTY
            // 
            this.AM7_OPT_AVG_QNTY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7_OPT_AVG_QNTY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7_OPT_AVG_QNTY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7_OPT_AVG_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7_OPT_AVG_QNTY.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.AM7_OPT_AVG_QNTY.Caption = "預估選擇權目標日均量";
            this.AM7_OPT_AVG_QNTY.FieldName = "AM7_OPT_AVG_QNTY";
            this.AM7_OPT_AVG_QNTY.Name = "AM7_OPT_AVG_QNTY";
            this.AM7_OPT_AVG_QNTY.Visible = true;
            this.AM7_OPT_AVG_QNTY.VisibleIndex = 3;
            this.AM7_OPT_AVG_QNTY.Width = 120;
            // 
            // AM7_FC_TAX
            // 
            this.AM7_FC_TAX.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7_FC_TAX.AppearanceHeader.Options.UseBackColor = true;
            this.AM7_FC_TAX.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7_FC_TAX.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7_FC_TAX.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.AM7_FC_TAX.Caption = "預估目標稅收";
            this.AM7_FC_TAX.FieldName = "AM7_FC_TAX";
            this.AM7_FC_TAX.Name = "AM7_FC_TAX";
            this.AM7_FC_TAX.Visible = true;
            this.AM7_FC_TAX.VisibleIndex = 4;
            this.AM7_FC_TAX.Width = 120;
            // 
            // AM7_FC_QNTY
            // 
            this.AM7_FC_QNTY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7_FC_QNTY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7_FC_QNTY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7_FC_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7_FC_QNTY.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.AM7_FC_QNTY.Caption = "預估日均量";
            this.AM7_FC_QNTY.FieldName = "AM7_FC_QNTY";
            this.AM7_FC_QNTY.Name = "AM7_FC_QNTY";
            this.AM7_FC_QNTY.Width = 100;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // W20410
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 526);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20410";
            this.Text = "W20410";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private BaseGround.Widget.TextDateEdit txtStartDate;
        private System.Windows.Forms.Label label1;
        private BaseGround.Widget.TextDateEdit txtEndDate;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn AM7_Y;
        private DevExpress.XtraGrid.Columns.GridColumn AM7_DAY_COUNT;
        private DevExpress.XtraGrid.Columns.GridColumn AM7_FUT_AVG_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn AM7_OPT_AVG_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn AM7_FC_TAX;
        private DevExpress.XtraGrid.Columns.GridColumn AM7_FC_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
    }
}