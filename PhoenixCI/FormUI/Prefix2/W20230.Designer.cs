namespace PhoenixCI.FormUI.Prefix2 {
    partial class W20230 {
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
            this.gcMain = new DevExpress.XtraGrid.GridControl();
            this.gvMain = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_LEVEL = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand5 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand15 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_C1_QNTY_MIN = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_C1_QNTY_MAX = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand6 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.Is_NewRow = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand7 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand8 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand9 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_C2_QNTY_MIN = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand10 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_C2_QNTY_MAX = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand11 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand12 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand13 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_STKOUT_MIN = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand14 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_STKOUT_MAX = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand16 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand17 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_NATURE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand18 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_LEGAL = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand19 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.PLST1_999 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.panParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // panParent
            // 
            this.panParent.Controls.Add(this.gcMain);
            this.panParent.Size = new System.Drawing.Size(1231, 610);
            // 
            // ribbonControl
            // 
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Size = new System.Drawing.Size(1231, 43);
            this.ribbonControl.Toolbar.ShowCustomizeItem = false;
            // 
            // gcMain
            // 
            this.gcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMain.Location = new System.Drawing.Point(18, 18);
            this.gcMain.MainView = this.gvMain;
            this.gcMain.MenuManager = this.ribbonControl;
            this.gcMain.Name = "gcMain";
            this.gcMain.Size = new System.Drawing.Size(1195, 574);
            this.gcMain.TabIndex = 0;
            this.gcMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMain});
            // 
            // gvMain
            // 
            this.gvMain.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand16});
            this.gvMain.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.PLST1_LEVEL,
            this.PLST1_C1_QNTY_MIN,
            this.PLST1_C1_QNTY_MAX,
            this.PLST1_C2_QNTY_MIN,
            this.PLST1_C2_QNTY_MAX,
            this.PLST1_STKOUT_MIN,
            this.PLST1_STKOUT_MAX,
            this.PLST1_NATURE,
            this.PLST1_LEGAL,
            this.PLST1_999,
            this.Is_NewRow});
            this.gvMain.GridControl = this.gcMain;
            this.gvMain.Name = "gvMain";
            this.gvMain.OptionsPrint.PrintHeader = false;
            this.gvMain.OptionsView.ShowGroupPanel = false;
            this.gvMain.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMain_RowCellStyle);
            this.gvMain.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMain_ShowingEditor);
            this.gvMain.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvMain_InitNewRow);
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridBand1.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand1.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand1.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.gridBand1.Caption = "級距";
            this.gridBand1.Columns.Add(this.PLST1_LEVEL);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 75;
            // 
            // PLST1_LEVEL
            // 
            this.PLST1_LEVEL.Caption = "PLST1_LEVEL";
            this.PLST1_LEVEL.FieldName = "PLST1_LEVEL";
            this.PLST1_LEVEL.Name = "PLST1_LEVEL";
            this.PLST1_LEVEL.Visible = true;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.BackColor = System.Drawing.Color.Blue;
            this.gridBand2.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand2.AppearanceHeader.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.gridBand2.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand2.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand2.AppearanceHeader.Options.UseFont = true;
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "最終條件 : min(條件1 , 條件2)";
            this.gridBand2.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand5,
            this.gridBand6,
            this.gridBand7});
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 1;
            this.gridBand2.Width = 605;
            // 
            // gridBand5
            // 
            this.gridBand5.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand5.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand5.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand5.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand5.Caption = "條件1";
            this.gridBand5.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand15});
            this.gridBand5.Name = "gridBand5";
            this.gridBand5.VisibleIndex = 0;
            this.gridBand5.Width = 150;
            // 
            // gridBand15
            // 
            this.gridBand15.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand15.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand15.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand15.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand15.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand15.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand15.Caption = "最近三個月份總交易量(億)";
            this.gridBand15.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand3,
            this.gridBand4});
            this.gridBand15.Name = "gridBand15";
            this.gridBand15.VisibleIndex = 0;
            this.gridBand15.Width = 150;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand3.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand3.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand3.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "最小量<";
            this.gridBand3.Columns.Add(this.PLST1_C1_QNTY_MIN);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 0;
            this.gridBand3.Width = 75;
            // 
            // PLST1_C1_QNTY_MIN
            // 
            this.PLST1_C1_QNTY_MIN.Caption = "PLST1_C1_QNTY_MIN";
            this.PLST1_C1_QNTY_MIN.FieldName = "PLST1_C1_QNTY_MIN";
            this.PLST1_C1_QNTY_MIN.Name = "PLST1_C1_QNTY_MIN";
            this.PLST1_C1_QNTY_MIN.Visible = true;
            // 
            // gridBand4
            // 
            this.gridBand4.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand4.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand4.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand4.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand4.Caption = "≦最大量";
            this.gridBand4.Columns.Add(this.PLST1_C1_QNTY_MAX);
            this.gridBand4.Name = "gridBand4";
            this.gridBand4.VisibleIndex = 1;
            this.gridBand4.Width = 75;
            // 
            // PLST1_C1_QNTY_MAX
            // 
            this.PLST1_C1_QNTY_MAX.Caption = "PLST1_C1_QNTY_MAX";
            this.PLST1_C1_QNTY_MAX.FieldName = "PLST1_C1_QNTY_MAX";
            this.PLST1_C1_QNTY_MAX.Name = "PLST1_C1_QNTY_MAX";
            this.PLST1_C1_QNTY_MAX.Visible = true;
            // 
            // gridBand6
            // 
            this.gridBand6.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand6.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand6.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand6.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand6.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.gridBand6.Caption = "或";
            this.gridBand6.Columns.Add(this.Is_NewRow);
            this.gridBand6.Name = "gridBand6";
            this.gridBand6.VisibleIndex = 1;
            this.gridBand6.Width = 75;
            // 
            // Is_NewRow
            // 
            this.Is_NewRow.Caption = "Is_NewRow";
            this.Is_NewRow.FieldName = "Is_NewRow";
            this.Is_NewRow.Name = "Is_NewRow";
            // 
            // gridBand7
            // 
            this.gridBand7.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand7.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand7.AppearanceHeader.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.gridBand7.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand7.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand7.AppearanceHeader.Options.UseFont = true;
            this.gridBand7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand7.Caption = "條件2  : max(2-1 , 2-2)";
            this.gridBand7.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand8,
            this.gridBand11,
            this.gridBand12});
            this.gridBand7.Name = "gridBand7";
            this.gridBand7.VisibleIndex = 2;
            this.gridBand7.Width = 380;
            // 
            // gridBand8
            // 
            this.gridBand8.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand8.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand8.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand8.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand8.Caption = "2-1 最近三個月份總交易量(億)";
            this.gridBand8.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand9,
            this.gridBand10});
            this.gridBand8.Name = "gridBand8";
            this.gridBand8.VisibleIndex = 0;
            this.gridBand8.Width = 150;
            // 
            // gridBand9
            // 
            this.gridBand9.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand9.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand9.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand9.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand9.Caption = "最小值<";
            this.gridBand9.Columns.Add(this.PLST1_C2_QNTY_MIN);
            this.gridBand9.Name = "gridBand9";
            this.gridBand9.VisibleIndex = 0;
            this.gridBand9.Width = 75;
            // 
            // PLST1_C2_QNTY_MIN
            // 
            this.PLST1_C2_QNTY_MIN.Caption = "PLST1_C2_QNTY_MIN";
            this.PLST1_C2_QNTY_MIN.FieldName = "PLST1_C2_QNTY_MIN";
            this.PLST1_C2_QNTY_MIN.Name = "PLST1_C2_QNTY_MIN";
            this.PLST1_C2_QNTY_MIN.Visible = true;
            // 
            // gridBand10
            // 
            this.gridBand10.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand10.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand10.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand10.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand10.Caption = "≦最大值";
            this.gridBand10.Columns.Add(this.PLST1_C2_QNTY_MAX);
            this.gridBand10.Name = "gridBand10";
            this.gridBand10.VisibleIndex = 1;
            this.gridBand10.Width = 75;
            // 
            // PLST1_C2_QNTY_MAX
            // 
            this.PLST1_C2_QNTY_MAX.Caption = "PLST1_C2_QNTY_MAX";
            this.PLST1_C2_QNTY_MAX.FieldName = "PLST1_C2_QNTY_MAX";
            this.PLST1_C2_QNTY_MAX.Name = "PLST1_C2_QNTY_MAX";
            this.PLST1_C2_QNTY_MAX.Visible = true;
            // 
            // gridBand11
            // 
            this.gridBand11.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand11.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand11.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand11.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand11.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.gridBand11.Caption = "且";
            this.gridBand11.Name = "gridBand11";
            this.gridBand11.VisibleIndex = 1;
            this.gridBand11.Width = 80;
            // 
            // gridBand12
            // 
            this.gridBand12.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand12.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand12.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand12.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand12.Caption = "2-2 在外流通股數(億)";
            this.gridBand12.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand13,
            this.gridBand14});
            this.gridBand12.Name = "gridBand12";
            this.gridBand12.VisibleIndex = 2;
            this.gridBand12.Width = 150;
            // 
            // gridBand13
            // 
            this.gridBand13.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand13.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand13.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand13.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand13.Caption = "最小值<";
            this.gridBand13.Columns.Add(this.PLST1_STKOUT_MIN);
            this.gridBand13.Name = "gridBand13";
            this.gridBand13.VisibleIndex = 0;
            this.gridBand13.Width = 75;
            // 
            // PLST1_STKOUT_MIN
            // 
            this.PLST1_STKOUT_MIN.Caption = "PLST1_STKOUT_MIN";
            this.PLST1_STKOUT_MIN.FieldName = "PLST1_STKOUT_MIN";
            this.PLST1_STKOUT_MIN.Name = "PLST1_STKOUT_MIN";
            this.PLST1_STKOUT_MIN.Visible = true;
            // 
            // gridBand14
            // 
            this.gridBand14.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBand14.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand14.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand14.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand14.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand14.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand14.Caption = "≦最大值";
            this.gridBand14.Columns.Add(this.PLST1_STKOUT_MAX);
            this.gridBand14.Name = "gridBand14";
            this.gridBand14.VisibleIndex = 1;
            this.gridBand14.Width = 75;
            // 
            // PLST1_STKOUT_MAX
            // 
            this.PLST1_STKOUT_MAX.Caption = "PLST1_STKOUT_MAX";
            this.PLST1_STKOUT_MAX.FieldName = "PLST1_STKOUT_MAX";
            this.PLST1_STKOUT_MAX.Name = "PLST1_STKOUT_MAX";
            this.PLST1_STKOUT_MAX.Visible = true;
            // 
            // gridBand16
            // 
            this.gridBand16.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridBand16.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand16.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand16.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand16.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand16.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.gridBand16.Caption = "部位限制數";
            this.gridBand16.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand17,
            this.gridBand18,
            this.gridBand19});
            this.gridBand16.Name = "gridBand16";
            this.gridBand16.VisibleIndex = 2;
            this.gridBand16.Width = 225;
            // 
            // gridBand17
            // 
            this.gridBand17.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridBand17.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand17.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand17.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand17.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand17.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand17.Caption = "自然人";
            this.gridBand17.Columns.Add(this.PLST1_NATURE);
            this.gridBand17.Name = "gridBand17";
            this.gridBand17.VisibleIndex = 0;
            this.gridBand17.Width = 75;
            // 
            // PLST1_NATURE
            // 
            this.PLST1_NATURE.Caption = "PLST1_NATURE";
            this.PLST1_NATURE.FieldName = "PLST1_NATURE";
            this.PLST1_NATURE.Name = "PLST1_NATURE";
            this.PLST1_NATURE.Visible = true;
            // 
            // gridBand18
            // 
            this.gridBand18.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridBand18.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand18.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand18.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand18.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand18.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand18.Caption = "法人";
            this.gridBand18.Columns.Add(this.PLST1_LEGAL);
            this.gridBand18.Name = "gridBand18";
            this.gridBand18.VisibleIndex = 1;
            this.gridBand18.Width = 75;
            // 
            // PLST1_LEGAL
            // 
            this.PLST1_LEGAL.Caption = "PLST1_LEGAL";
            this.PLST1_LEGAL.FieldName = "PLST1_LEGAL";
            this.PLST1_LEGAL.Name = "PLST1_LEGAL";
            this.PLST1_LEGAL.Visible = true;
            // 
            // gridBand19
            // 
            this.gridBand19.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridBand19.AppearanceHeader.BorderColor = System.Drawing.Color.Black;
            this.gridBand19.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand19.AppearanceHeader.Options.UseBorderColor = true;
            this.gridBand19.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand19.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand19.Caption = "造市者";
            this.gridBand19.Columns.Add(this.PLST1_999);
            this.gridBand19.Name = "gridBand19";
            this.gridBand19.VisibleIndex = 2;
            this.gridBand19.Width = 75;
            // 
            // PLST1_999
            // 
            this.PLST1_999.Caption = "PLST1_999";
            this.PLST1_999.FieldName = "PLST1_999";
            this.PLST1_999.Name = "PLST1_999";
            this.PLST1_999.Visible = true;
            // 
            // W20230
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1231, 653);
            this.Name = "W20230";
            this.Text = "W20230";
            this.panParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMain;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvMain;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_LEVEL;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_C1_QNTY_MIN;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_C1_QNTY_MAX;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_C2_QNTY_MIN;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_C2_QNTY_MAX;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_STKOUT_MIN;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_STKOUT_MAX;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_NATURE;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_LEGAL;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn PLST1_999;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn Is_NewRow;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand5;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand15;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand6;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand7;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand8;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand9;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand10;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand11;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand12;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand13;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand14;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand16;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand17;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand18;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand19;
    }
}