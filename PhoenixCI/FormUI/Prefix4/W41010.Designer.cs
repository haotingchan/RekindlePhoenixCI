namespace PhoenixCI.FormUI.Prefix4 {
    partial class W41010 {
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
            this.label4 = new System.Windows.Forms.Label();
            this.rdgProdType = new DevExpress.XtraEditors.RadioGroup();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEDate = new PhoenixCI.Widget.TextDateEdit();
            this.txtSDate = new PhoenixCI.Widget.TextDateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PDK_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PDK_KIND_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PDK_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PDK_STATUS_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gcMainE = new DevExpress.XtraGrid.GridControl();
            this.gvMainE = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PDK_DATE_E = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PDK_KIND_ID_E = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PDK_NAME_E = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PDK_STATUS_CODE_E = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdgProdType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMainE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMainE)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Size = new System.Drawing.Size(891, 619);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(891, 30);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.rdgProdType);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.txtProd);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.txtEDate);
            this.panelControl1.Controls.Add(this.txtSDate);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.lblDate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 30);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(891, 102);
            this.panelControl1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(368, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "系統：";
            // 
            // rdgProdType
            // 
            this.rdgProdType.EditValue = "F";
            this.rdgProdType.Location = new System.Drawing.Point(447, 9);
            this.rdgProdType.MenuManager = this.ribbonControl;
            this.rdgProdType.Name = "rdgProdType";
            this.rdgProdType.Properties.Columns = 2;
            this.rdgProdType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("F", "期貨"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("O", "選擇權")});
            this.rdgProdType.Size = new System.Drawing.Size(237, 43);
            this.rdgProdType.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(212, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(453, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "註：(1)商品名稱%代表查全部 (2)下市商品資料起始為2010開始";
            // 
            // txtProd
            // 
            this.txtProd.Location = new System.Drawing.Point(106, 58);
            this.txtProd.Name = "txtProd";
            this.txtProd.Size = new System.Drawing.Size(100, 29);
            this.txtProd.TabIndex = 13;
            this.txtProd.Text = "%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "商品：";
            // 
            // txtEDate
            // 
            this.txtEDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtEDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtEDate.EditValue = "2018/12";
            this.txtEDate.EnterMoveNextControl = true;
            this.txtEDate.Location = new System.Drawing.Point(243, 18);
            this.txtEDate.MenuManager = this.ribbonControl;
            this.txtEDate.Name = "txtEDate";
            this.txtEDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtEDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtEDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtEDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtEDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtEDate.Size = new System.Drawing.Size(100, 26);
            this.txtEDate.TabIndex = 12;
            this.txtEDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // txtSDate
            // 
            this.txtSDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.txtSDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
            this.txtSDate.EditValue = "2018/12";
            this.txtSDate.EnterMoveNextControl = true;
            this.txtSDate.Location = new System.Drawing.Point(106, 18);
            this.txtSDate.MenuManager = this.ribbonControl;
            this.txtSDate.Name = "txtSDate";
            this.txtSDate.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtSDate.Properties.Mask.EditMask = "yyyy/MM/dd";
            this.txtSDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtSDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSDate.Size = new System.Drawing.Size(100, 26);
            this.txtSDate.TabIndex = 11;
            this.txtSDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "～";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(43, 21);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 20);
            this.lblDate.TabIndex = 9;
            this.lblDate.Text = "日期：";
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(0, 0);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(891, 282);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PDK_DATE,
            this.PDK_KIND_ID,
            this.PDK_NAME,
            this.PDK_STATUS_CODE});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsView.ColumnAutoWidth = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            // 
            // PDK_DATE
            // 
            this.PDK_DATE.AppearanceHeader.BackColor = System.Drawing.Color.Cyan;
            this.PDK_DATE.AppearanceHeader.Options.UseBackColor = true;
            this.PDK_DATE.Caption = "交易日期";
            this.PDK_DATE.FieldName = "PDK_DATE";
            this.PDK_DATE.Name = "PDK_DATE";
            this.PDK_DATE.OptionsColumn.AllowEdit = false;
            this.PDK_DATE.OptionsColumn.ReadOnly = true;
            this.PDK_DATE.Visible = true;
            this.PDK_DATE.VisibleIndex = 0;
            this.PDK_DATE.Width = 101;
            // 
            // PDK_KIND_ID
            // 
            this.PDK_KIND_ID.AppearanceHeader.BackColor = System.Drawing.Color.Cyan;
            this.PDK_KIND_ID.AppearanceHeader.Options.UseBackColor = true;
            this.PDK_KIND_ID.Caption = "契約代號";
            this.PDK_KIND_ID.FieldName = "PDK_KIND_ID";
            this.PDK_KIND_ID.Name = "PDK_KIND_ID";
            this.PDK_KIND_ID.OptionsColumn.AllowEdit = false;
            this.PDK_KIND_ID.OptionsColumn.ReadOnly = true;
            this.PDK_KIND_ID.Visible = true;
            this.PDK_KIND_ID.VisibleIndex = 1;
            this.PDK_KIND_ID.Width = 88;
            // 
            // PDK_NAME
            // 
            this.PDK_NAME.AppearanceHeader.BackColor = System.Drawing.Color.Cyan;
            this.PDK_NAME.AppearanceHeader.Options.UseBackColor = true;
            this.PDK_NAME.Caption = "契約名稱";
            this.PDK_NAME.FieldName = "PDK_NAME";
            this.PDK_NAME.Name = "PDK_NAME";
            this.PDK_NAME.OptionsColumn.AllowEdit = false;
            this.PDK_NAME.OptionsColumn.ReadOnly = true;
            this.PDK_NAME.Visible = true;
            this.PDK_NAME.VisibleIndex = 2;
            this.PDK_NAME.Width = 128;
            // 
            // PDK_STATUS_CODE
            // 
            this.PDK_STATUS_CODE.AppearanceHeader.BackColor = System.Drawing.Color.Cyan;
            this.PDK_STATUS_CODE.AppearanceHeader.Options.UseBackColor = true;
            this.PDK_STATUS_CODE.Caption = "狀態";
            this.PDK_STATUS_CODE.FieldName = "PDK_STATUS_CODE";
            this.PDK_STATUS_CODE.Name = "PDK_STATUS_CODE";
            this.PDK_STATUS_CODE.OptionsColumn.AllowEdit = false;
            this.PDK_STATUS_CODE.OptionsColumn.ReadOnly = true;
            this.PDK_STATUS_CODE.Visible = true;
            this.PDK_STATUS_CODE.VisibleIndex = 3;
            this.PDK_STATUS_CODE.Width = 99;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gcMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 367);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(891, 282);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gcMainE);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 132);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(891, 235);
            this.panel2.TabIndex = 0;
            // 
            // gcMainE
            // 
            this.gcMainE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMainE.Location = new System.Drawing.Point(0, 0);
            this.gcMainE.MainView = this.gvMainE;
            this.gcMainE.MenuManager = this.ribbonControl;
            this.gcMainE.Name = "gcMainE";
            this.gcMainE.Size = new System.Drawing.Size(891, 235);
            this.gcMainE.TabIndex = 1;
            this.gcMainE.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMainE});
            // 
            // gvMainE
            // 
            this.gvMainE.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PDK_DATE_E,
            this.PDK_KIND_ID_E,
            this.PDK_NAME_E,
            this.PDK_STATUS_CODE_E});
            this.gvMainE.GridControl = this.gcMainE;
            this.gvMainE.Name = "gvMainE";
            this.gvMainE.OptionsView.ColumnAutoWidth = false;
            this.gvMainE.OptionsView.ShowGroupPanel = false;
            // 
            // PDK_DATE_E
            // 
            this.PDK_DATE_E.AppearanceHeader.BackColor = System.Drawing.Color.Cyan;
            this.PDK_DATE_E.AppearanceHeader.Options.UseBackColor = true;
            this.PDK_DATE_E.Caption = "交易日期";
            this.PDK_DATE_E.FieldName = "PDK_DATE";
            this.PDK_DATE_E.Name = "PDK_DATE_E";
            this.PDK_DATE_E.OptionsColumn.AllowEdit = false;
            this.PDK_DATE_E.OptionsColumn.ReadOnly = true;
            this.PDK_DATE_E.Visible = true;
            this.PDK_DATE_E.VisibleIndex = 0;
            this.PDK_DATE_E.Width = 101;
            // 
            // PDK_KIND_ID_E
            // 
            this.PDK_KIND_ID_E.AppearanceHeader.BackColor = System.Drawing.Color.Cyan;
            this.PDK_KIND_ID_E.AppearanceHeader.Options.UseBackColor = true;
            this.PDK_KIND_ID_E.Caption = "契約代號";
            this.PDK_KIND_ID_E.FieldName = "PDK_KIND_ID";
            this.PDK_KIND_ID_E.Name = "PDK_KIND_ID_E";
            this.PDK_KIND_ID_E.OptionsColumn.AllowEdit = false;
            this.PDK_KIND_ID_E.OptionsColumn.ReadOnly = true;
            this.PDK_KIND_ID_E.Visible = true;
            this.PDK_KIND_ID_E.VisibleIndex = 1;
            this.PDK_KIND_ID_E.Width = 88;
            // 
            // PDK_NAME_E
            // 
            this.PDK_NAME_E.AppearanceHeader.BackColor = System.Drawing.Color.Cyan;
            this.PDK_NAME_E.AppearanceHeader.Options.UseBackColor = true;
            this.PDK_NAME_E.Caption = "契約名稱";
            this.PDK_NAME_E.FieldName = "PDK_NAME";
            this.PDK_NAME_E.Name = "PDK_NAME_E";
            this.PDK_NAME_E.OptionsColumn.AllowEdit = false;
            this.PDK_NAME_E.OptionsColumn.ReadOnly = true;
            this.PDK_NAME_E.Visible = true;
            this.PDK_NAME_E.VisibleIndex = 2;
            this.PDK_NAME_E.Width = 128;
            // 
            // PDK_STATUS_CODE_E
            // 
            this.PDK_STATUS_CODE_E.AppearanceHeader.BackColor = System.Drawing.Color.Cyan;
            this.PDK_STATUS_CODE_E.AppearanceHeader.Options.UseBackColor = true;
            this.PDK_STATUS_CODE_E.Caption = "狀態";
            this.PDK_STATUS_CODE_E.FieldName = "PDK_STATUS_CODE";
            this.PDK_STATUS_CODE_E.Name = "PDK_STATUS_CODE_E";
            this.PDK_STATUS_CODE_E.OptionsColumn.AllowEdit = false;
            this.PDK_STATUS_CODE_E.OptionsColumn.ReadOnly = true;
            this.PDK_STATUS_CODE_E.Visible = true;
            this.PDK_STATUS_CODE_E.VisibleIndex = 3;
            this.PDK_STATUS_CODE_E.Width = 99;
            // 
            // W41010
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 649);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelControl1);
            this.Name = "W41010";
            this.Text = "W41010";
            this.Controls.SetChildIndex(this.ribbonControl, 0);
            this.Controls.SetChildIndex(this.panParent, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdgProdType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMainE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMainE)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private Widget.TextDateEdit txtEDate;
        private Widget.TextDateEdit txtSDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProd;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
        private DevExpress.XtraGrid.Columns.GridColumn PDK_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn PDK_KIND_ID;
        private DevExpress.XtraGrid.Columns.GridColumn PDK_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn PDK_STATUS_CODE;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.RadioGroup rdgProdType;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gcMainE;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMainE;
        private DevExpress.XtraGrid.Columns.GridColumn PDK_DATE_E;
        private DevExpress.XtraGrid.Columns.GridColumn PDK_KIND_ID_E;
        private DevExpress.XtraGrid.Columns.GridColumn PDK_NAME_E;
        private DevExpress.XtraGrid.Columns.GridColumn PDK_STATUS_CODE_E;
    }
}