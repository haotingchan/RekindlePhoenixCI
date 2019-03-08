namespace PhoenixCI.FormUI.Prefix2 {
   partial class W28110 {
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
         this.grpxDescription = new System.Windows.Forms.GroupBox();
         this.txtDate = new PhoenixCI.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.labMsg = new System.Windows.Forms.Label();
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.btnSp = new System.Windows.Forms.Button();
         this.btnStwd = new System.Windows.Forms.Button();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.STW_YMD = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_COM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_SETTLE_M = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_SETTLE_Y = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_OPEN_1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_HIGH = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_LOW = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_CLSE_1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_SETTLE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_VOLUMN = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_OINT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_RECTYP = new DevExpress.XtraGrid.Columns.GridColumn();
         this.gcMain2 = new DevExpress.XtraGrid.GridControl();
         this.gvMain2 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.STW_DEL = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_OPEN_I1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_OPEN_2 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_OPEN_I2 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_HIGH_I = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_LOW_I = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_CLSE_I1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_CLSE_2 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.STW_CLSE_I2 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain2)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain2);
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Location = new System.Drawing.Point(0, 167);
         this.panParent.Size = new System.Drawing.Size(1249, 536);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(1249, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.Controls.Add(this.txtDate);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Location = new System.Drawing.Point(15, 8);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(324, 97);
         this.grpxDescription.TabIndex = 8;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // txtDate
         // 
         this.txtDate.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtDate.DateType = PhoenixCI.Widget.TextDateEdit.DateTypeItem.Month;
         this.txtDate.EditValue = "2018/12";
         this.txtDate.EnterMoveNextControl = true;
         this.txtDate.Location = new System.Drawing.Point(100, 43);
         this.txtDate.MenuManager = this.ribbonControl;
         this.txtDate.Name = "txtDate";
         this.txtDate.Properties.Appearance.Options.UseTextOptions = true;
         this.txtDate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtDate.Properties.Mask.EditMask = "yyyy/MM/dd";
         this.txtDate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtDate.Size = new System.Drawing.Size(120, 26);
         this.txtDate.TabIndex = 6;
         this.txtDate.TextMaskFormat = PhoenixCI.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
         // 
         // lblDate
         // 
         this.lblDate.AutoSize = true;
         this.lblDate.Location = new System.Drawing.Point(37, 46);
         this.lblDate.Name = "lblDate";
         this.lblDate.Size = new System.Drawing.Size(57, 20);
         this.lblDate.TabIndex = 2;
         this.lblDate.Text = "日期：";
         // 
         // labMsg
         // 
         this.labMsg.AutoSize = true;
         this.labMsg.ForeColor = System.Drawing.Color.Blue;
         this.labMsg.Location = new System.Drawing.Point(12, 108);
         this.labMsg.Name = "labMsg";
         this.labMsg.Size = new System.Drawing.Size(85, 20);
         this.labMsg.TabIndex = 10;
         this.labMsg.Text = "開始轉檔...";
         this.labMsg.Visible = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.btnSp);
         this.panelControl1.Controls.Add(this.btnStwd);
         this.panelControl1.Controls.Add(this.grpxDescription);
         this.panelControl1.Controls.Add(this.labMsg);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(1249, 137);
         this.panelControl1.TabIndex = 11;
         // 
         // btnSp
         // 
         this.btnSp.Location = new System.Drawing.Point(510, 33);
         this.btnSp.Name = "btnSp";
         this.btnSp.Size = new System.Drawing.Size(86, 41);
         this.btnSp.TabIndex = 12;
         this.btnSp.Text = "SP";
         this.btnSp.UseVisualStyleBackColor = true;
         this.btnSp.Visible = false;
         this.btnSp.Click += new System.EventHandler(this.btnSp_Click);
         // 
         // btnStwd
         // 
         this.btnStwd.Location = new System.Drawing.Point(402, 33);
         this.btnStwd.Name = "btnStwd";
         this.btnStwd.Size = new System.Drawing.Size(86, 41);
         this.btnStwd.TabIndex = 11;
         this.btnStwd.Text = "STWD";
         this.btnStwd.UseVisualStyleBackColor = true;
         this.btnStwd.Visible = false;
         this.btnStwd.Click += new System.EventHandler(this.btnStwd_Click);
         // 
         // panelControl2
         // 
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 30);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(1249, 673);
         this.panelControl2.TabIndex = 12;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Left;
         this.gcMain.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.gcMain.Location = new System.Drawing.Point(12, 12);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(930, 512);
         this.gcMain.TabIndex = 0;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         this.gcMain.Visible = false;
         // 
         // gvMain
         // 
         this.gvMain.Appearance.CustomizationFormHint.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.gvMain.Appearance.CustomizationFormHint.Options.UseFont = true;
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.STW_YMD,
            this.STW_COM,
            this.STW_SETTLE_M,
            this.STW_SETTLE_Y,
            this.STW_OPEN_1,
            this.STW_HIGH,
            this.STW_LOW,
            this.STW_CLSE_1,
            this.STW_SETTLE,
            this.STW_VOLUMN,
            this.STW_OINT,
            this.STW_RECTYP});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         // 
         // STW_YMD
         // 
         this.STW_YMD.Caption = "STW_YMD";
         this.STW_YMD.FieldName = "STW_YMD";
         this.STW_YMD.Name = "STW_YMD";
         this.STW_YMD.Visible = true;
         this.STW_YMD.VisibleIndex = 0;
         // 
         // STW_COM
         // 
         this.STW_COM.Caption = "STW_COM";
         this.STW_COM.FieldName = "STW_COM";
         this.STW_COM.Name = "STW_COM";
         this.STW_COM.Visible = true;
         this.STW_COM.VisibleIndex = 1;
         // 
         // STW_SETTLE_M
         // 
         this.STW_SETTLE_M.Caption = "STW_SETTLE_M";
         this.STW_SETTLE_M.FieldName = "STW_SETTLE_M";
         this.STW_SETTLE_M.Name = "STW_SETTLE_M";
         this.STW_SETTLE_M.Visible = true;
         this.STW_SETTLE_M.VisibleIndex = 2;
         // 
         // STW_SETTLE_Y
         // 
         this.STW_SETTLE_Y.Caption = "STW_SETTLE_Y";
         this.STW_SETTLE_Y.FieldName = "STW_SETTLE_Y";
         this.STW_SETTLE_Y.Name = "STW_SETTLE_Y";
         this.STW_SETTLE_Y.Visible = true;
         this.STW_SETTLE_Y.VisibleIndex = 3;
         // 
         // STW_OPEN_1
         // 
         this.STW_OPEN_1.Caption = "STW_OPEN_1";
         this.STW_OPEN_1.FieldName = "STW_OPEN_1";
         this.STW_OPEN_1.Name = "STW_OPEN_1";
         this.STW_OPEN_1.Visible = true;
         this.STW_OPEN_1.VisibleIndex = 4;
         // 
         // STW_HIGH
         // 
         this.STW_HIGH.Caption = "STW_HIGH";
         this.STW_HIGH.FieldName = "STW_HIGH";
         this.STW_HIGH.Name = "STW_HIGH";
         this.STW_HIGH.Visible = true;
         this.STW_HIGH.VisibleIndex = 5;
         // 
         // STW_LOW
         // 
         this.STW_LOW.Caption = "STW_LOW";
         this.STW_LOW.FieldName = "STW_LOW";
         this.STW_LOW.Name = "STW_LOW";
         this.STW_LOW.Visible = true;
         this.STW_LOW.VisibleIndex = 6;
         // 
         // STW_CLSE_1
         // 
         this.STW_CLSE_1.Caption = "STW_CLSE_1";
         this.STW_CLSE_1.FieldName = "STW_CLSE_1";
         this.STW_CLSE_1.Name = "STW_CLSE_1";
         this.STW_CLSE_1.Visible = true;
         this.STW_CLSE_1.VisibleIndex = 7;
         // 
         // STW_SETTLE
         // 
         this.STW_SETTLE.Caption = "STW_SETTLE";
         this.STW_SETTLE.FieldName = "STW_SETTLE";
         this.STW_SETTLE.Name = "STW_SETTLE";
         this.STW_SETTLE.Visible = true;
         this.STW_SETTLE.VisibleIndex = 8;
         // 
         // STW_VOLUMN
         // 
         this.STW_VOLUMN.Caption = "STW_VOLUMN";
         this.STW_VOLUMN.FieldName = "STW_VOLUMN";
         this.STW_VOLUMN.Name = "STW_VOLUMN";
         this.STW_VOLUMN.Visible = true;
         this.STW_VOLUMN.VisibleIndex = 9;
         // 
         // STW_OINT
         // 
         this.STW_OINT.Caption = "STW_OINT";
         this.STW_OINT.FieldName = "STW_OINT";
         this.STW_OINT.Name = "STW_OINT";
         this.STW_OINT.Visible = true;
         this.STW_OINT.VisibleIndex = 10;
         // 
         // STW_RECTYP
         // 
         this.STW_RECTYP.Caption = "STW_RECTYP";
         this.STW_RECTYP.FieldName = "STW_RECTYP";
         this.STW_RECTYP.Name = "STW_RECTYP";
         this.STW_RECTYP.Visible = true;
         this.STW_RECTYP.VisibleIndex = 11;
         // 
         // gcMain2
         // 
         this.gcMain2.Dock = System.Windows.Forms.DockStyle.Right;
         this.gcMain2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.gcMain2.Location = new System.Drawing.Point(989, 12);
         this.gcMain2.MainView = this.gvMain2;
         this.gcMain2.MenuManager = this.ribbonControl;
         this.gcMain2.Name = "gcMain2";
         this.gcMain2.Size = new System.Drawing.Size(248, 512);
         this.gcMain2.TabIndex = 1;
         this.gcMain2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain2});
         this.gcMain2.Visible = false;
         // 
         // gvMain2
         // 
         this.gvMain2.Appearance.CustomizationFormHint.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.gvMain2.Appearance.CustomizationFormHint.Options.UseFont = true;
         this.gvMain2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.STW_DEL,
            this.STW_OPEN_I1,
            this.STW_OPEN_2,
            this.STW_OPEN_I2,
            this.STW_HIGH_I,
            this.STW_LOW_I,
            this.STW_CLSE_I1,
            this.STW_CLSE_2,
            this.STW_CLSE_I2});
         this.gvMain2.GridControl = this.gcMain2;
         this.gvMain2.Name = "gvMain2";
         this.gvMain2.OptionsView.ColumnAutoWidth = false;
         // 
         // STW_DEL
         // 
         this.STW_DEL.Caption = "STW_DEL";
         this.STW_DEL.FieldName = "STW_DEL";
         this.STW_DEL.Name = "STW_DEL";
         this.STW_DEL.Visible = true;
         this.STW_DEL.VisibleIndex = 0;
         // 
         // STW_OPEN_I1
         // 
         this.STW_OPEN_I1.Caption = "STW_OPEN_I1";
         this.STW_OPEN_I1.FieldName = "STW_OPEN_I1";
         this.STW_OPEN_I1.Name = "STW_OPEN_I1";
         this.STW_OPEN_I1.Visible = true;
         this.STW_OPEN_I1.VisibleIndex = 1;
         // 
         // STW_OPEN_2
         // 
         this.STW_OPEN_2.Caption = "STW_OPEN_2";
         this.STW_OPEN_2.FieldName = "STW_OPEN_2";
         this.STW_OPEN_2.Name = "STW_OPEN_2";
         this.STW_OPEN_2.Visible = true;
         this.STW_OPEN_2.VisibleIndex = 2;
         // 
         // STW_OPEN_I2
         // 
         this.STW_OPEN_I2.Caption = "STW_OPEN_I2";
         this.STW_OPEN_I2.FieldName = "STW_OPEN_I2";
         this.STW_OPEN_I2.Name = "STW_OPEN_I2";
         this.STW_OPEN_I2.Visible = true;
         this.STW_OPEN_I2.VisibleIndex = 3;
         // 
         // STW_HIGH_I
         // 
         this.STW_HIGH_I.Caption = "STW_HIGH_I";
         this.STW_HIGH_I.FieldName = "STW_HIGH_I";
         this.STW_HIGH_I.Name = "STW_HIGH_I";
         this.STW_HIGH_I.Visible = true;
         this.STW_HIGH_I.VisibleIndex = 4;
         // 
         // STW_LOW_I
         // 
         this.STW_LOW_I.Caption = "STW_LOW_I";
         this.STW_LOW_I.FieldName = "STW_LOW_I";
         this.STW_LOW_I.Name = "STW_LOW_I";
         this.STW_LOW_I.Visible = true;
         this.STW_LOW_I.VisibleIndex = 5;
         // 
         // STW_CLSE_I1
         // 
         this.STW_CLSE_I1.Caption = "STW_CLSE_I1";
         this.STW_CLSE_I1.FieldName = "STW_CLSE_I1";
         this.STW_CLSE_I1.Name = "STW_CLSE_I1";
         this.STW_CLSE_I1.Visible = true;
         this.STW_CLSE_I1.VisibleIndex = 6;
         // 
         // STW_CLSE_2
         // 
         this.STW_CLSE_2.Caption = "STW_CLSE_2";
         this.STW_CLSE_2.FieldName = "STW_CLSE_2";
         this.STW_CLSE_2.Name = "STW_CLSE_2";
         this.STW_CLSE_2.Visible = true;
         this.STW_CLSE_2.VisibleIndex = 7;
         // 
         // STW_CLSE_I2
         // 
         this.STW_CLSE_I2.Caption = "STW_CLSE_I2";
         this.STW_CLSE_I2.FieldName = "STW_CLSE_I2";
         this.STW_CLSE_I2.Name = "STW_CLSE_I2";
         this.STW_CLSE_I2.Visible = true;
         this.STW_CLSE_I2.VisibleIndex = 8;
         // 
         // W28110
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1249, 703);
         this.Controls.Add(this.panelControl1);
         this.Controls.Add(this.panelControl2);
         this.Name = "W28110";
         this.Text = "W28110";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox grpxDescription;
      private Widget.TextDateEdit txtDate;
      private System.Windows.Forms.Label lblDate;
      private System.Windows.Forms.Label labMsg;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private DevExpress.XtraGrid.Columns.GridColumn STW_YMD;
      private DevExpress.XtraGrid.Columns.GridColumn STW_COM;
      private DevExpress.XtraGrid.Columns.GridColumn STW_SETTLE_M;
      private DevExpress.XtraGrid.Columns.GridColumn STW_SETTLE_Y;
      private DevExpress.XtraGrid.Columns.GridColumn STW_OPEN_1;
      private DevExpress.XtraGrid.GridControl gcMain2;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain2;
      private DevExpress.XtraGrid.Columns.GridColumn STW_DEL;
      private DevExpress.XtraGrid.Columns.GridColumn STW_OPEN_I1;
      private DevExpress.XtraGrid.Columns.GridColumn STW_OPEN_2;
      private DevExpress.XtraGrid.Columns.GridColumn STW_OPEN_I2;
      private DevExpress.XtraGrid.Columns.GridColumn STW_HIGH_I;
      private DevExpress.XtraGrid.Columns.GridColumn STW_LOW_I;
      private DevExpress.XtraGrid.Columns.GridColumn STW_CLSE_I1;
      private DevExpress.XtraGrid.Columns.GridColumn STW_CLSE_2;
      private DevExpress.XtraGrid.Columns.GridColumn STW_CLSE_I2;
      private DevExpress.XtraGrid.Columns.GridColumn STW_HIGH;
      private DevExpress.XtraGrid.Columns.GridColumn STW_LOW;
      private DevExpress.XtraGrid.Columns.GridColumn STW_CLSE_1;
      private DevExpress.XtraGrid.Columns.GridColumn STW_SETTLE;
      private DevExpress.XtraGrid.Columns.GridColumn STW_VOLUMN;
      private DevExpress.XtraGrid.Columns.GridColumn STW_OINT;
      private DevExpress.XtraGrid.Columns.GridColumn STW_RECTYP;
      private System.Windows.Forms.Button btnSp;
      private System.Windows.Forms.Button btnStwd;
   }
}