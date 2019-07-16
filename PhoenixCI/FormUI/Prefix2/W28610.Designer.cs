namespace PhoenixCI.FormUI.Prefix2 {
   partial class W28610 {
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
         this.txtMonth = new BaseGround.Widget.TextDateEdit();
         this.lblDate = new System.Windows.Forms.Label();
         this.lblProcessing = new System.Windows.Forms.Label();
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.AB1_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AB1_ACC_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AB1_ACCU_COUNT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AB1_TRADE_COUNT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AB1_COUNT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panParent.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         this.grpxDescription.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtMonth.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.Controls.Add(this.gcMain);
         this.panParent.Location = new System.Drawing.Point(0, 167);
         this.panParent.Size = new System.Drawing.Size(836, 407);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(836, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // grpxDescription
         // 
         this.grpxDescription.AutoSize = true;
         this.grpxDescription.Controls.Add(this.txtMonth);
         this.grpxDescription.Controls.Add(this.lblDate);
         this.grpxDescription.Location = new System.Drawing.Point(13, 6);
         this.grpxDescription.Name = "grpxDescription";
         this.grpxDescription.Size = new System.Drawing.Size(324, 97);
         this.grpxDescription.TabIndex = 8;
         this.grpxDescription.TabStop = false;
         this.grpxDescription.Text = "請輸入交易日期";
         // 
         // txtMonth
         // 
         this.txtMonth.DateTimeValue = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
         this.txtMonth.DateType = BaseGround.Widget.TextDateEdit.DateTypeItem.Date;
         this.txtMonth.EditValue = "2018/12/01";
         this.txtMonth.EnterMoveNextControl = true;
         this.txtMonth.Location = new System.Drawing.Point(100, 43);
         this.txtMonth.MenuManager = this.ribbonControl;
         this.txtMonth.Name = "txtMonth";
         this.txtMonth.Properties.Appearance.Options.UseTextOptions = true;
         this.txtMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.txtMonth.Properties.EditFormat.FormatString = "yyyyMMdd";
         this.txtMonth.Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         this.txtMonth.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         this.txtMonth.Properties.Mask.ShowPlaceHolders = false;
         this.txtMonth.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.txtMonth.Size = new System.Drawing.Size(100, 26);
         this.txtMonth.TabIndex = 84;
         this.txtMonth.TextMaskFormat = BaseGround.Widget.TextDateEdit.TextMaskFormatItem.IncludePrompt;
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
         // lblProcessing
         // 
         this.lblProcessing.AutoSize = true;
         this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
         this.lblProcessing.Location = new System.Drawing.Point(10, 106);
         this.lblProcessing.Name = "lblProcessing";
         this.lblProcessing.Size = new System.Drawing.Size(85, 20);
         this.lblProcessing.TabIndex = 10;
         this.lblProcessing.Text = "開始轉檔...";
         this.lblProcessing.Visible = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panelControl1.Appearance.Options.UseBackColor = true;
         this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelControl1.Controls.Add(this.grpxDescription);
         this.panelControl1.Controls.Add(this.lblProcessing);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(836, 137);
         this.panelControl1.TabIndex = 11;
         // 
         // panelControl2
         // 
         this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl2.Location = new System.Drawing.Point(0, 30);
         this.panelControl2.Name = "panelControl2";
         this.panelControl2.Size = new System.Drawing.Size(836, 544);
         this.panelControl2.TabIndex = 12;
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.gcMain.Location = new System.Drawing.Point(12, 12);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.MenuManager = this.ribbonControl;
         this.gcMain.Name = "gcMain";
         this.gcMain.Size = new System.Drawing.Size(812, 383);
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
            this.AB1_DATE,
            this.AB1_ACC_TYPE,
            this.AB1_ACCU_COUNT,
            this.AB1_TRADE_COUNT,
            this.AB1_COUNT});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsView.ColumnAutoWidth = false;
         // 
         // AB1_DATE
         // 
         this.AB1_DATE.Caption = "資料日期";
         this.AB1_DATE.FieldName = "AB1_DATE";
         this.AB1_DATE.Name = "AB1_DATE";
         this.AB1_DATE.Visible = true;
         this.AB1_DATE.VisibleIndex = 0;
         // 
         // AB1_ACC_TYPE
         // 
         this.AB1_ACC_TYPE.Caption = "身分碼";
         this.AB1_ACC_TYPE.FieldName = "AB1_ACC_TYPE";
         this.AB1_ACC_TYPE.Name = "AB1_ACC_TYPE";
         this.AB1_ACC_TYPE.Visible = true;
         this.AB1_ACC_TYPE.VisibleIndex = 1;
         // 
         // AB1_ACCU_COUNT
         // 
         this.AB1_ACCU_COUNT.Caption = "累計開戶數";
         this.AB1_ACCU_COUNT.FieldName = "AB1_ACCU_COUNT";
         this.AB1_ACCU_COUNT.Name = "AB1_ACCU_COUNT";
         this.AB1_ACCU_COUNT.Visible = true;
         this.AB1_ACCU_COUNT.VisibleIndex = 2;
         // 
         // AB1_TRADE_COUNT
         // 
         this.AB1_TRADE_COUNT.Caption = "交易戶數";
         this.AB1_TRADE_COUNT.FieldName = "AB1_TRADE_COUNT";
         this.AB1_TRADE_COUNT.Name = "AB1_TRADE_COUNT";
         this.AB1_TRADE_COUNT.Visible = true;
         this.AB1_TRADE_COUNT.VisibleIndex = 3;
         // 
         // AB1_COUNT
         // 
         this.AB1_COUNT.Caption = "資料日開戶數";
         this.AB1_COUNT.FieldName = "AB1_COUNT";
         this.AB1_COUNT.Name = "AB1_COUNT";
         this.AB1_COUNT.Visible = true;
         this.AB1_COUNT.VisibleIndex = 4;
         // 
         // W28610
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(836, 574);
         this.Controls.Add(this.panelControl1);
         this.Controls.Add(this.panelControl2);
         this.Name = "W28610";
         this.Text = "W28610";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panelControl2, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.panParent.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         this.grpxDescription.ResumeLayout(false);
         this.grpxDescription.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtMonth.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox grpxDescription;
      private System.Windows.Forms.Label lblDate;
      private System.Windows.Forms.Label lblProcessing;
      private DevExpress.XtraGrid.GridControl gcMain;
      private DevExpress.XtraGrid.Views.Grid.GridView gvMain;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private DevExpress.XtraGrid.Columns.GridColumn AB1_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn AB1_ACC_TYPE;
      private DevExpress.XtraGrid.Columns.GridColumn AB1_ACCU_COUNT;
      private DevExpress.XtraGrid.Columns.GridColumn AB1_TRADE_COUNT;
      private DevExpress.XtraGrid.Columns.GridColumn AB1_COUNT;
      private BaseGround.Widget.TextDateEdit txtMonth;
   }
}