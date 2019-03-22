namespace PhoenixCI.FormUI.Prefix4 {
    partial class W49060 {
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
         this.txtEndDate = new PhoenixCI.Widget.TextDateEdit();
         this.txtStartDate = new PhoenixCI.Widget.TextDateEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.lblDate = new System.Windows.Forms.Label();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.MG8_EFFECT_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_F_ID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_CM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_MM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_IM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MG8_ISSUE_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Is_NewRow = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).BeginInit();
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
         this.panelControl1.Controls.Add(this.txtEndDate);
         this.panelControl1.Controls.Add(this.txtStartDate);
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Controls.Add(this.lblDate);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(778, 70);
         this.panelControl1.TabIndex = 0;
         // 
         // txtEndDate
         // 
         this.txtEndDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtEndDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtEndDate.EditValue = "2018/12";
         this.txtEndDate.EnterMoveNextControl = true;
         this.txtEndDate.Location = new System.Drawing.Point(248, 23);
         this.txtEndDate.MenuManager = this.ribbonControl;
         this.txtEndDate.Name = "txtEndDate";
         this.txtEndDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtEndDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtEndDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtEndDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtEndDate.Size = new System.Drawing.Size(100, 26);
         this.txtEndDate.TabIndex = 22;
         this.txtEndDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // txtStartDate
         // 
         this.txtStartDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtStartDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtStartDate.EditValue = "2018/12";
         this.txtStartDate.EnterMoveNextControl = true;
         this.txtStartDate.Location = new System.Drawing.Point(115, 23);
         this.txtStartDate.MenuManager = this.ribbonControl;
         this.txtStartDate.Name = "txtStartDate";
         this.txtStartDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtStartDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtStartDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtStartDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtStartDate.Size = new System.Drawing.Size(100, 26);
         this.txtStartDate.TabIndex = 21;
         this.txtStartDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.BackColor = System.Drawing.Color.Transparent;
         this.label1.Location = new System.Drawing.Point(221, 26);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(21, 20);
         this.label1.TabIndex = 20;
         this.label1.Text = "~";
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.BackColor = System.Drawing.Color.Transparent;
         this.lblDate.Location = new System.Drawing.Point(29, 26);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(89, 20);
         this.lblDate.TabIndex = 18;
         this.lblDate.Text = "生效日期：";
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
         this.gcMain.Visible = false;
         // 
         // gvMain
         // 
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.MG8_EFFECT_YMD,
            this.MG8_F_ID,
            this.MG8_CM,
            this.MG8_MM,
            this.MG8_IM,
            this.MG8_ISSUE_YMD,
            this.Is_NewRow});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         this.gvMain.OptionsView.ShowGroupPanel = false;
         this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
         this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
         // 
         // MG8_EFFECT_YMD
         // 
         this.MG8_EFFECT_YMD.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.MG8_EFFECT_YMD.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_EFFECT_YMD.AppearanceHeader.Options.UseTextOptions = true;
         this.MG8_EFFECT_YMD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MG8_EFFECT_YMD.Caption = "生效日期";
         this.MG8_EFFECT_YMD.FieldName = "MG8_EFFECT_YMD";
         this.MG8_EFFECT_YMD.Name = "MG8_EFFECT_YMD";
         this.MG8_EFFECT_YMD.Visible = true;
         this.MG8_EFFECT_YMD.VisibleIndex = 0;
         // 
         // MG8_F_ID
         // 
         this.MG8_F_ID.AppearanceHeader.BackColor = System.Drawing.Color.Yellow;
         this.MG8_F_ID.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_F_ID.AppearanceHeader.Options.UseTextOptions = true;
         this.MG8_F_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MG8_F_ID.Caption = "交易所+商品";
         this.MG8_F_ID.FieldName = "MG8_F_ID";
         this.MG8_F_ID.Name = "MG8_F_ID";
         this.MG8_F_ID.Visible = true;
         this.MG8_F_ID.VisibleIndex = 1;
         this.MG8_F_ID.Width = 200;
         // 
         // MG8_CM
         // 
         this.MG8_CM.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MG8_CM.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_CM.AppearanceHeader.Options.UseTextOptions = true;
         this.MG8_CM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MG8_CM.Caption = "結算保證金";
         this.MG8_CM.FieldName = "MG8_CM";
         this.MG8_CM.Name = "MG8_CM";
         this.MG8_CM.Visible = true;
         this.MG8_CM.VisibleIndex = 2;
         this.MG8_CM.Width = 145;
         // 
         // MG8_MM
         // 
         this.MG8_MM.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MG8_MM.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_MM.AppearanceHeader.Options.UseTextOptions = true;
         this.MG8_MM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MG8_MM.Caption = "維持保證金";
         this.MG8_MM.FieldName = "MG8_MM";
         this.MG8_MM.Name = "MG8_MM";
         this.MG8_MM.Visible = true;
         this.MG8_MM.VisibleIndex = 3;
         this.MG8_MM.Width = 120;
         // 
         // MG8_IM
         // 
         this.MG8_IM.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MG8_IM.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_IM.AppearanceHeader.Options.UseTextOptions = true;
         this.MG8_IM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.MG8_IM.Caption = "原始保證金";
         this.MG8_IM.FieldName = "MG8_IM";
         this.MG8_IM.Name = "MG8_IM";
         this.MG8_IM.Visible = true;
         this.MG8_IM.VisibleIndex = 4;
         this.MG8_IM.Width = 145;
         // 
         // MG8_ISSUE_YMD
         // 
         this.MG8_ISSUE_YMD.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         this.MG8_ISSUE_YMD.AppearanceHeader.Options.UseBackColor = true;
         this.MG8_ISSUE_YMD.Caption = "公告日期";
         this.MG8_ISSUE_YMD.FieldName = "MG8_ISSUE_YMD";
         this.MG8_ISSUE_YMD.Name = "MG8_ISSUE_YMD";
         this.MG8_ISSUE_YMD.Visible = true;
         this.MG8_ISSUE_YMD.VisibleIndex = 5;
         // 
         // Is_NewRow
         // 
         this.Is_NewRow.Caption = "Is_NewRow";
         this.Is_NewRow.FieldName = "Is_NewRow";
         this.Is_NewRow.Name = "Is_NewRow";
         // 
         // W49060
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(778, 598);
         this.Controls.Add(this.panelControl2);
         this.Controls.Add(this.panelControl1);
         this.Name = "W49060";
         this.Text = "W49060";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtEndDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtStartDate.Properties)).EndInit();
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
        private DevExpress.XtraGrid.Columns.GridColumn MG8_EFFECT_YMD;
        private DevExpress.XtraGrid.Columns.GridColumn MG8_F_ID;
        private DevExpress.XtraGrid.Columns.GridColumn MG8_CM;
        private DevExpress.XtraGrid.Columns.GridColumn MG8_MM;
        private DevExpress.XtraGrid.Columns.GridColumn MG8_IM;
        private DevExpress.XtraGrid.Columns.GridColumn MG8_ISSUE_YMD;
        private DevExpress.XtraGrid.Columns.GridColumn Is_NewRow;
      private System.Windows.Forms.Label label1;
      private Widget.TextDateEdit txtEndDate;
      private Widget.TextDateEdit txtStartDate;
   }
}