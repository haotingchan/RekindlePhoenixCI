namespace PhoenixCI.FormUI.Prefix4
{
   partial class W49010
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
      private void InitializeComponent()
      {
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.label1 = new System.Windows.Forms.Label();
         this.gcMain = new DevExpress.XtraGrid.GridControl();
         this.gvMain = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
         this.CPR_PROD_SUBTYPE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_KIND_ID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_EFFECTIVE_DATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_PRICE_RISK_RATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_APPROVAL_DATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_APPROVAL_NUMBER = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_REMARK = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_W_TIME = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_W_USER_ID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.CPR_DATA_NUM = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.IS_NEWROW = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
         this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
         this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand9 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand5 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand6 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand7 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.gridBand8 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
         this.SuspendLayout();
         // 
         // panParent
         // 
         this.panParent.BackColor = System.Drawing.Color.MintCream;
         this.panParent.Size = new System.Drawing.Size(905, 527);
         // 
         // ribbonControl
         // 
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Size = new System.Drawing.Size(905, 30);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         // 
         // panelControl1
         // 
         this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(202)))), ((int)(((byte)(240)))));
         this.panelControl1.Appearance.Options.UseBackColor = true;
         this.panelControl1.Controls.Add(this.label1);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 30);
         this.panelControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(905, 65);
         this.panelControl1.TabIndex = 0;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.ForeColor = System.Drawing.Color.Red;
         this.label1.Location = new System.Drawing.Point(25, 25);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(697, 20);
         this.label1.TabIndex = 0;
         this.label1.Text = "備    註：已下市契約之最小風險價格係數一律為空白；有效契約之最小風險價格係數不可為空白。";
         // 
         // gcMain
         // 
         this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gcMain.Location = new System.Drawing.Point(0, 95);
         this.gcMain.MainView = this.gvMain;
         this.gcMain.Name = "gcMain";
         this.gcMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemMemoEdit1,
            this.repositoryItemTextEdit2});
         this.gcMain.Size = new System.Drawing.Size(905, 462);
         this.gcMain.TabIndex = 1;
         this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
         // 
         // gvMain
         // 
         this.gvMain.Appearance.BandPanel.Options.UseTextOptions = true;
         this.gvMain.Appearance.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gvMain.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3,
            this.gridBand4,
            this.gridBand5,
            this.gridBand6,
            this.gridBand7,
            this.gridBand8});
         this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.CPR_PROD_SUBTYPE,
            this.CPR_KIND_ID,
            this.CPR_EFFECTIVE_DATE,
            this.CPR_PRICE_RISK_RATE,
            this.CPR_APPROVAL_DATE,
            this.CPR_APPROVAL_NUMBER,
            this.CPR_REMARK,
            this.CPR_W_TIME,
            this.CPR_W_USER_ID,
            this.CPR_DATA_NUM,
            this.IS_NEWROW});
         this.gvMain.GridControl = this.gcMain;
         this.gvMain.Name = "gvMain";
         this.gvMain.OptionsPrint.PrintHeader = false;
         // 
         // CPR_PROD_SUBTYPE
         // 
         this.CPR_PROD_SUBTYPE.Caption = "CPR_PROD_SUBTYPE";
         this.CPR_PROD_SUBTYPE.FieldName = "CPR_PROD_SUBTYPE";
         this.CPR_PROD_SUBTYPE.Name = "CPR_PROD_SUBTYPE";
         this.CPR_PROD_SUBTYPE.Visible = true;
         this.CPR_PROD_SUBTYPE.Width = 108;
         // 
         // CPR_KIND_ID
         // 
         this.CPR_KIND_ID.Caption = "CPR_KIND_ID";
         this.CPR_KIND_ID.FieldName = "CPR_KIND_ID";
         this.CPR_KIND_ID.Name = "CPR_KIND_ID";
         this.CPR_KIND_ID.Visible = true;
         this.CPR_KIND_ID.Width = 71;
         // 
         // CPR_EFFECTIVE_DATE
         // 
         this.CPR_EFFECTIVE_DATE.Caption = "CPR_EFFECTIVE_DATE";
         this.CPR_EFFECTIVE_DATE.FieldName = "CPR_EFFECTIVE_DATE";
         this.CPR_EFFECTIVE_DATE.Name = "CPR_EFFECTIVE_DATE";
         this.CPR_EFFECTIVE_DATE.Visible = true;
         this.CPR_EFFECTIVE_DATE.Width = 112;
         // 
         // CPR_PRICE_RISK_RATE
         // 
         this.CPR_PRICE_RISK_RATE.Caption = "CPR_PRICE_RISK_RATE";
         this.CPR_PRICE_RISK_RATE.FieldName = "CPR_PRICE_RISK_RATE";
         this.CPR_PRICE_RISK_RATE.Name = "CPR_PRICE_RISK_RATE";
         this.CPR_PRICE_RISK_RATE.Visible = true;
         this.CPR_PRICE_RISK_RATE.Width = 121;
         // 
         // CPR_APPROVAL_DATE
         // 
         this.CPR_APPROVAL_DATE.Caption = "CPR_APPROVAL_DATE";
         this.CPR_APPROVAL_DATE.FieldName = "CPR_APPROVAL_DATE";
         this.CPR_APPROVAL_DATE.Name = "CPR_APPROVAL_DATE";
         this.CPR_APPROVAL_DATE.Visible = true;
         this.CPR_APPROVAL_DATE.Width = 111;
         // 
         // CPR_APPROVAL_NUMBER
         // 
         this.CPR_APPROVAL_NUMBER.Caption = "CPR_APPROVAL_NUMBER";
         this.CPR_APPROVAL_NUMBER.FieldName = "CPR_APPROVAL_NUMBER";
         this.CPR_APPROVAL_NUMBER.Name = "CPR_APPROVAL_NUMBER";
         this.CPR_APPROVAL_NUMBER.Visible = true;
         this.CPR_APPROVAL_NUMBER.Width = 128;
         // 
         // CPR_REMARK
         // 
         this.CPR_REMARK.Caption = "CPR_REMARK";
         this.CPR_REMARK.FieldName = "CPR_REMARK";
         this.CPR_REMARK.Name = "CPR_REMARK";
         this.CPR_REMARK.Visible = true;
         this.CPR_REMARK.Width = 68;
         // 
         // CPR_W_TIME
         // 
         this.CPR_W_TIME.Caption = "CPR_W_TIME";
         this.CPR_W_TIME.FieldName = "CPR_W_TIME";
         this.CPR_W_TIME.Name = "CPR_W_TIME";
         this.CPR_W_TIME.Visible = true;
         this.CPR_W_TIME.Width = 71;
         // 
         // CPR_W_USER_ID
         // 
         this.CPR_W_USER_ID.Caption = "CPR_W_USER_ID";
         this.CPR_W_USER_ID.FieldName = "CPR_W_USER_ID";
         this.CPR_W_USER_ID.Name = "CPR_W_USER_ID";
         this.CPR_W_USER_ID.Visible = true;
         this.CPR_W_USER_ID.Width = 97;
         // 
         // CPR_DATA_NUM
         // 
         this.CPR_DATA_NUM.Caption = "CPR_DATA_NUM";
         this.CPR_DATA_NUM.FieldName = "CPR_DATA_NUM";
         this.CPR_DATA_NUM.Name = "CPR_DATA_NUM";
         // 
         // IS_NEWROW
         // 
         this.IS_NEWROW.Caption = "IS_NEWROW";
         this.IS_NEWROW.FieldName = "IS_NEWROW";
         this.IS_NEWROW.Name = "IS_NEWROW";
         // 
         // repositoryItemTextEdit1
         // 
         this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
         // 
         // repositoryItemMemoEdit1
         // 
         this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
         // 
         // repositoryItemTextEdit2
         // 
         this.repositoryItemTextEdit2.AutoHeight = false;
         this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
         // 
         // gridBand1
         // 
         this.gridBand1.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand1.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand1.Caption = "契約類別";
         this.gridBand1.Columns.Add(this.CPR_PROD_SUBTYPE);
         this.gridBand1.Name = "gridBand1";
         this.gridBand1.VisibleIndex = 0;
         this.gridBand1.Width = 108;
         // 
         // gridBand2
         // 
         this.gridBand2.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand2.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand2.Caption = "契約代號";
         this.gridBand2.Columns.Add(this.CPR_KIND_ID);
         this.gridBand2.Name = "gridBand2";
         this.gridBand2.VisibleIndex = 1;
         this.gridBand2.Width = 71;
         // 
         // gridBand3
         // 
         this.gridBand3.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand3.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand3.Caption = "系統生效日";
         this.gridBand3.Columns.Add(this.CPR_EFFECTIVE_DATE);
         this.gridBand3.Name = "gridBand3";
         this.gridBand3.VisibleIndex = 2;
         this.gridBand3.Width = 112;
         // 
         // gridBand4
         // 
         this.gridBand4.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand4.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand4.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand4.Caption = "最小風險價格係數";
         this.gridBand4.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand9});
         this.gridBand4.Name = "gridBand4";
         this.gridBand4.VisibleIndex = 3;
         this.gridBand4.Width = 121;
         // 
         // gridBand9
         // 
         this.gridBand9.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand9.AppearanceHeader.Font = new System.Drawing.Font("微軟正黑體", 8F);
         this.gridBand9.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.gridBand9.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand9.AppearanceHeader.Options.UseFont = true;
         this.gridBand9.AppearanceHeader.Options.UseForeColor = true;
         this.gridBand9.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand9.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.gridBand9.Caption = "(輸入方式：如3.5%，則輸入0.035)";
         this.gridBand9.Columns.Add(this.CPR_PRICE_RISK_RATE);
         this.gridBand9.Name = "gridBand9";
         this.gridBand9.OptionsBand.FixedWidth = true;
         this.gridBand9.VisibleIndex = 0;
         this.gridBand9.Width = 121;
         // 
         // gridBand5
         // 
         this.gridBand5.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand5.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand5.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand5.Caption = "核定日期";
         this.gridBand5.Columns.Add(this.CPR_APPROVAL_DATE);
         this.gridBand5.Name = "gridBand5";
         this.gridBand5.VisibleIndex = 4;
         this.gridBand5.Width = 111;
         // 
         // gridBand6
         // 
         this.gridBand6.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand6.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand6.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand6.Caption = "核定文號及日期";
         this.gridBand6.Columns.Add(this.CPR_APPROVAL_NUMBER);
         this.gridBand6.Name = "gridBand6";
         this.gridBand6.VisibleIndex = 5;
         this.gridBand6.Width = 128;
         // 
         // gridBand7
         // 
         this.gridBand7.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand7.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand7.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand7.Caption = "備註";
         this.gridBand7.Columns.Add(this.CPR_REMARK);
         this.gridBand7.Name = "gridBand7";
         this.gridBand7.VisibleIndex = 6;
         this.gridBand7.Width = 68;
         // 
         // gridBand8
         // 
         this.gridBand8.AppearanceHeader.BackColor = System.Drawing.Color.PaleTurquoise;
         this.gridBand8.AppearanceHeader.Options.UseBackColor = true;
         this.gridBand8.AppearanceHeader.Options.UseTextOptions = true;
         this.gridBand8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridBand8.Caption = "異動時點紀錄";
         this.gridBand8.Columns.Add(this.CPR_W_TIME);
         this.gridBand8.Columns.Add(this.CPR_W_USER_ID);
         this.gridBand8.Name = "gridBand8";
         this.gridBand8.VisibleIndex = 7;
         this.gridBand8.Width = 168;
         // 
         // W49010
         // 
         this.Appearance.Options.UseFont = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(905, 557);
         this.Controls.Add(this.gcMain);
         this.Controls.Add(this.panelControl1);
         this.Name = "W49010";
         this.Text = "W49010";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.panParent, 0);
         this.Controls.SetChildIndex(this.panelControl1, 0);
         this.Controls.SetChildIndex(this.gcMain, 0);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.panelControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraGrid.GridControl gcMain;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
      private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
      private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvMain;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_PROD_SUBTYPE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_KIND_ID;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_EFFECTIVE_DATE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_PRICE_RISK_RATE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_APPROVAL_DATE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_APPROVAL_NUMBER;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_REMARK;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_W_TIME;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_W_USER_ID;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn IS_NEWROW;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn CPR_DATA_NUM;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand9;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand5;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand6;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand7;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand8;
   }
}