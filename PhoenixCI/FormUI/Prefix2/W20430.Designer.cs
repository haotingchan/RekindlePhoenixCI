namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20430 {
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
            this.txtDate = new PhoenixCI.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.AM7T_Y = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7T_PARAM_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7T_AVG_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7T_DAY_COUNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7T_TFXM_YEAR_AVG_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7T_PROD_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7T_PROD_SUBTYPE = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.panParent.Location = new System.Drawing.Point(0, 100);
            this.panParent.Size = new System.Drawing.Size(778, 498);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(778, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtDate);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(778, 70);
            this.panelControl1.TabIndex = 0;
            // 
            // txtDate
            // 
            this.txtDate.DateTimeValue = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.txtDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Year;
            this.txtDate.EditValue = "2018/12";
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(92, 23);
            this.txtDate.MenuManager = this.ribbonControl;
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtDate.Properties.Mask.EditMask = "0000";
            this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtDate.Properties.Mask.PlaceHolder = '0';
            this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDate.Properties.MaxLength = 4;
            this.txtDate.Size = new System.Drawing.Size(71, 26);
            this.txtDate.TabIndex = 17;
            this.txtDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(29, 26);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 18;
            this.lblDate.Text = "年度：";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 100);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(778, 498);
            this.panelControl2.TabIndex = 1;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(12, 12);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(754, 474);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.AM7T_Y,
            this.AM7T_PARAM_KEY,
            this.AM7T_AVG_QNTY,
            this.AM7T_DAY_COUNT,
            this.AM7T_TFXM_YEAR_AVG_QNTY,
            this.AM7T_PROD_TYPE,
            this.AM7T_PROD_SUBTYPE,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            // 
            // AM7T_Y
            // 
            this.AM7T_Y.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.AM7T_Y.AppearanceHeader.Options.UseBackColor = true;
            this.AM7T_Y.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7T_Y.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7T_Y.Caption = "年月";
            this.AM7T_Y.FieldName = "AM7T_Y";
            this.AM7T_Y.Name = "AM7T_Y";
            this.AM7T_Y.Visible = true;
            this.AM7T_Y.VisibleIndex = 0;
            // 
            // AM7T_PARAM_KEY
            // 
            this.AM7T_PARAM_KEY.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
            this.AM7T_PARAM_KEY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7T_PARAM_KEY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7T_PARAM_KEY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7T_PARAM_KEY.Caption = "商品";
            this.AM7T_PARAM_KEY.FieldName = "AM7T_PARAM_KEY";
            this.AM7T_PARAM_KEY.Name = "AM7T_PARAM_KEY";
            this.AM7T_PARAM_KEY.Visible = true;
            this.AM7T_PARAM_KEY.VisibleIndex = 1;
            this.AM7T_PARAM_KEY.Width = 200;
            // 
            // AM7T_AVG_QNTY
            // 
            this.AM7T_AVG_QNTY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7T_AVG_QNTY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7T_AVG_QNTY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7T_AVG_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7T_AVG_QNTY.Caption = "預估日均量";
            this.AM7T_AVG_QNTY.FieldName = "AM7T_AVG_QNTY";
            this.AM7T_AVG_QNTY.Name = "AM7T_AVG_QNTY";
            this.AM7T_AVG_QNTY.Visible = true;
            this.AM7T_AVG_QNTY.VisibleIndex = 2;
            this.AM7T_AVG_QNTY.Width = 145;
            // 
            // AM7T_DAY_COUNT
            // 
            this.AM7T_DAY_COUNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7T_DAY_COUNT.AppearanceHeader.Options.UseBackColor = true;
            this.AM7T_DAY_COUNT.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7T_DAY_COUNT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7T_DAY_COUNT.Caption = "總交易天數";
            this.AM7T_DAY_COUNT.FieldName = "AM7T_DAY_COUNT";
            this.AM7T_DAY_COUNT.Name = "AM7T_DAY_COUNT";
            this.AM7T_DAY_COUNT.Visible = true;
            this.AM7T_DAY_COUNT.VisibleIndex = 3;
            this.AM7T_DAY_COUNT.Width = 120;
            // 
            // AM7T_TFXM_YEAR_AVG_QNTY
            // 
            this.AM7T_TFXM_YEAR_AVG_QNTY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7T_TFXM_YEAR_AVG_QNTY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7T_TFXM_YEAR_AVG_QNTY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7T_TFXM_YEAR_AVG_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7T_TFXM_YEAR_AVG_QNTY.Caption = "年度現貨平均成交值";
            this.AM7T_TFXM_YEAR_AVG_QNTY.FieldName = "AM7T_TFXM_YEAR_AVG_QNTY";
            this.AM7T_TFXM_YEAR_AVG_QNTY.Name = "AM7T_TFXM_YEAR_AVG_QNTY";
            this.AM7T_TFXM_YEAR_AVG_QNTY.Visible = true;
            this.AM7T_TFXM_YEAR_AVG_QNTY.VisibleIndex = 4;
            this.AM7T_TFXM_YEAR_AVG_QNTY.Width = 145;
            // 
            // AM7T_PROD_TYPE
            // 
            this.AM7T_PROD_TYPE.Caption = "gridColumn6";
            this.AM7T_PROD_TYPE.FieldName = "AM7T_PROD_TYPE";
            this.AM7T_PROD_TYPE.Name = "AM7T_PROD_TYPE";
            // 
            // AM7T_PROD_SUBTYPE
            // 
            this.AM7T_PROD_SUBTYPE.Caption = "gridColumn7";
            this.AM7T_PROD_SUBTYPE.FieldName = "AM7T_PROD_SUBTYPE";
            this.AM7T_PROD_SUBTYPE.Name = "AM7T_PROD_SUBTYPE";
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // W20430
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 598);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20430";
            this.Text = "W20430";
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
        private Widget.TextDateEdit txtDate;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn AM7T_Y;
        private DevExpress.XtraGrid.Columns.GridColumn AM7T_PARAM_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn AM7T_AVG_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn AM7T_DAY_COUNT;
        private DevExpress.XtraGrid.Columns.GridColumn AM7T_TFXM_YEAR_AVG_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn AM7T_PROD_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn AM7T_PROD_SUBTYPE;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
    }
}