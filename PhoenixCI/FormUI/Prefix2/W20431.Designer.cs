namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20431 {
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
            this.txtMonth = new BaseGround.Widget.TextDateEdit();
            this.lblDate = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.AM7M_YM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7M_PARAM_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7M_AVG_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7M_DAY_COUNT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7M_TFXM_YEAR_AVG_QNTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7M_PROD_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.AM7M_PROD_SUBTYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblNote = new System.Windows.Forms.Label();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMonth.Properties)).BeginInit();
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
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.lblNote);
            this.panelControl1.Controls.Add(this.txtMonth);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(778, 70);
            this.panelControl1.TabIndex = 0;
            // 
            // txtMonth
            // 
            this.txtMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtMonth.EditValue = "2018/12";
            this.txtMonth.EnterMoveNextControl = true;
            this.txtMonth.Location = new System.Drawing.Point(78, 22);
            this.txtMonth.MenuManager = this.ribbonControl;
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.txtMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtMonth.Properties.EditFormat.FormatString = "yyyyMM";
            this.txtMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
            this.txtMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtMonth.Properties.Mask.ShowPlaceHolders = false;
            this.txtMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtMonth.Size = new System.Drawing.Size(87, 28);
            this.txtMonth.TabIndex = 19;
            this.txtMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(29, 26);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 18;
            this.lblDate.Text = "年月：";
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
            this.AM7M_YM,
            this.AM7M_PARAM_KEY,
            this.AM7M_AVG_QNTY,
            this.AM7M_DAY_COUNT,
            this.AM7M_TFXM_YEAR_AVG_QNTY,
            this.AM7M_PROD_TYPE,
            this.AM7M_PROD_SUBTYPE,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            // 
            // AM7M_YM
            // 
            this.AM7M_YM.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.AM7M_YM.AppearanceHeader.Options.UseBackColor = true;
            this.AM7M_YM.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7M_YM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7M_YM.Caption = "年月";
            this.AM7M_YM.FieldName = "AM7M_YM";
            this.AM7M_YM.Name = "AM7M_YM";
            this.AM7M_YM.Visible = true;
            this.AM7M_YM.VisibleIndex = 0;
            // 
            // AM7M_PARAM_KEY
            // 
            this.AM7M_PARAM_KEY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.AM7M_PARAM_KEY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7M_PARAM_KEY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7M_PARAM_KEY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7M_PARAM_KEY.Caption = "商品";
            this.AM7M_PARAM_KEY.FieldName = "AM7M_PARAM_KEY";
            this.AM7M_PARAM_KEY.Name = "AM7M_PARAM_KEY";
            this.AM7M_PARAM_KEY.Visible = true;
            this.AM7M_PARAM_KEY.VisibleIndex = 1;
            this.AM7M_PARAM_KEY.Width = 200;
            // 
            // AM7M_AVG_QNTY
            // 
            this.AM7M_AVG_QNTY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7M_AVG_QNTY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7M_AVG_QNTY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7M_AVG_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7M_AVG_QNTY.Caption = "預估日均量";
            this.AM7M_AVG_QNTY.FieldName = "AM7M_AVG_QNTY";
            this.AM7M_AVG_QNTY.Name = "AM7M_AVG_QNTY";
            this.AM7M_AVG_QNTY.Visible = true;
            this.AM7M_AVG_QNTY.VisibleIndex = 2;
            this.AM7M_AVG_QNTY.Width = 145;
            // 
            // AM7M_DAY_COUNT
            // 
            this.AM7M_DAY_COUNT.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7M_DAY_COUNT.AppearanceHeader.Options.UseBackColor = true;
            this.AM7M_DAY_COUNT.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7M_DAY_COUNT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7M_DAY_COUNT.Caption = "總交易天數";
            this.AM7M_DAY_COUNT.FieldName = "AM7M_DAY_COUNT";
            this.AM7M_DAY_COUNT.Name = "AM7M_DAY_COUNT";
            this.AM7M_DAY_COUNT.Visible = true;
            this.AM7M_DAY_COUNT.VisibleIndex = 3;
            this.AM7M_DAY_COUNT.Width = 120;
            // 
            // AM7M_TFXM_YEAR_AVG_QNTY
            // 
            this.AM7M_TFXM_YEAR_AVG_QNTY.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AM7M_TFXM_YEAR_AVG_QNTY.AppearanceHeader.Options.UseBackColor = true;
            this.AM7M_TFXM_YEAR_AVG_QNTY.AppearanceHeader.Options.UseTextOptions = true;
            this.AM7M_TFXM_YEAR_AVG_QNTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AM7M_TFXM_YEAR_AVG_QNTY.Caption = "年度現貨平均成交值";
            this.AM7M_TFXM_YEAR_AVG_QNTY.FieldName = "AM7M_TFXM_YEAR_AVG_QNTY";
            this.AM7M_TFXM_YEAR_AVG_QNTY.Name = "AM7M_TFXM_YEAR_AVG_QNTY";
            this.AM7M_TFXM_YEAR_AVG_QNTY.Visible = true;
            this.AM7M_TFXM_YEAR_AVG_QNTY.VisibleIndex = 4;
            this.AM7M_TFXM_YEAR_AVG_QNTY.Width = 145;
            // 
            // AM7M_PROD_TYPE
            // 
            this.AM7M_PROD_TYPE.Caption = "gridColumn6";
            this.AM7M_PROD_TYPE.FieldName = "AM7M_PROD_TYPE";
            this.AM7M_PROD_TYPE.Name = "AM7M_PROD_TYPE";
            // 
            // AM7M_PROD_SUBTYPE
            // 
            this.AM7M_PROD_SUBTYPE.Caption = "gridColumn7";
            this.AM7M_PROD_SUBTYPE.FieldName = "AM7M_PROD_SUBTYPE";
            this.AM7M_PROD_SUBTYPE.Name = "AM7M_PROD_SUBTYPE";
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.BackColor = System.Drawing.Color.Transparent;
            this.lblNote.Location = new System.Drawing.Point(171, 26);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(182, 20);
            this.lblNote.TabIndex = 20;
            this.lblNote.Text = "備註：資料從20430轉入";
            // 
            // W20431
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 598);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "W20431";
            this.Text = "W20431";
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.Label lblDate;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn AM7M_YM;
        private DevExpress.XtraGrid.Columns.GridColumn AM7M_PARAM_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn AM7M_AVG_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn AM7M_DAY_COUNT;
        private DevExpress.XtraGrid.Columns.GridColumn AM7M_TFXM_YEAR_AVG_QNTY;
        private DevExpress.XtraGrid.Columns.GridColumn AM7M_PROD_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn AM7M_PROD_SUBTYPE;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
        private BaseGround.Widget.TextDateEdit txtMonth;
        private System.Windows.Forms.Label lblNote;
    }
}