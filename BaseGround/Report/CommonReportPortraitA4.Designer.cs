namespace BaseGround.Report
{
    partial class CommonReportPortraitA4
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.Detail = new DevExpress.XtraReports.UI.DetailBand();
         this.printableComponentContainerMain = new DevExpress.XtraReports.UI.PrintableComponentContainer();
         this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
         this.OcfDate = new DevExpress.XtraReports.Parameters.Parameter();
         this.UserName = new DevExpress.XtraReports.Parameters.Parameter();
         this.ReportID = new DevExpress.XtraReports.Parameters.Parameter();
         this.ReportTitle = new DevExpress.XtraReports.Parameters.Parameter();
         this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
         this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
         this.xrSubreportMain = new DevExpress.XtraReports.UI.XRSubreport();
         this.LeftMemo = new DevExpress.XtraReports.Parameters.Parameter();
         this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
         this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
         this.xrSubreportFooter = new DevExpress.XtraReports.UI.XRSubreport();
         this.xrTableFooter = new DevExpress.XtraReports.UI.XRTable();
         this.xrTableRowMemo = new DevExpress.XtraReports.UI.XRTableRow();
         this.xrTableCellMemo = new DevExpress.XtraReports.UI.XRTableCell();
         this.lblMemo = new DevExpress.XtraReports.UI.XRLabel();
         this.xrTableRowSeal = new DevExpress.XtraReports.UI.XRTableRow();
         this.xrTableCellSeal = new DevExpress.XtraReports.UI.XRTableCell();
         this.lblDoubleConfirmDescription = new DevExpress.XtraReports.UI.XRLabel();
         this.lblHandlingDescription = new DevExpress.XtraReports.UI.XRLabel();
         this.lblManagerDescription = new DevExpress.XtraReports.UI.XRLabel();
         ((System.ComponentModel.ISupportInitialize)(this.xrTableFooter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
         // 
         // Detail
         // 
         this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.printableComponentContainerMain});
         this.Detail.HeightF = 67.95832F;
         this.Detail.KeepTogether = true;
         this.Detail.Name = "Detail";
         this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBandExceptLastEntry;
         this.Detail.StylePriority.UseFont = false;
         this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
         // 
         // printableComponentContainerMain
         // 
         this.printableComponentContainerMain.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
         this.printableComponentContainerMain.Name = "printableComponentContainerMain";
         this.printableComponentContainerMain.SizeF = new System.Drawing.SizeF(786.9999F, 23.95833F);
         // 
         // TopMargin
         // 
         this.TopMargin.HeightF = 20F;
         this.TopMargin.Name = "TopMargin";
         this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
         // 
         // OcfDate
         // 
         this.OcfDate.Description = "OCF_DATE";
         this.OcfDate.Name = "OcfDate";
         // 
         // UserName
         // 
         this.UserName.Description = "作業人員";
         this.UserName.Name = "UserName";
         // 
         // ReportID
         // 
         this.ReportID.Description = "報表代號";
         this.ReportID.Name = "ReportID";
         // 
         // ReportTitle
         // 
         this.ReportTitle.Description = "報表標題";
         this.ReportTitle.Name = "ReportTitle";
         // 
         // BottomMargin
         // 
         this.BottomMargin.HeightF = 20F;
         this.BottomMargin.Name = "BottomMargin";
         this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         // 
         // PageHeader
         // 
         this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreportMain});
         this.PageHeader.HeightF = 44.7917F;
         this.PageHeader.Name = "PageHeader";
         // 
         // xrSubreportMain
         // 
         this.xrSubreportMain.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
         this.xrSubreportMain.Name = "xrSubreportMain";
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("ReportTitle", this.ReportTitle));
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("UserName", this.UserName));
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("OcfDate", this.OcfDate));
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("ReportID", this.ReportID));
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("LeftMemo", this.LeftMemo));
         this.xrSubreportMain.ReportSource = new BaseGround.Report.ReportHeader();
         this.xrSubreportMain.SizeF = new System.Drawing.SizeF(787F, 44.7917F);
         // 
         // LeftMemo
         // 
         this.LeftMemo.Name = "LeftMemo";
         // 
         // PageFooter
         // 
         this.PageFooter.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
         this.PageFooter.BorderWidth = 3F;
         this.PageFooter.HeightF = 37.5F;
         this.PageFooter.Name = "PageFooter";
         this.PageFooter.PrintOn = ((DevExpress.XtraReports.UI.PrintOnPages)((DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader | DevExpress.XtraReports.UI.PrintOnPages.NotWithReportFooter)));
         this.PageFooter.StylePriority.UseBorders = false;
         this.PageFooter.StylePriority.UseBorderWidth = false;
         // 
         // ReportFooter
         // 
         this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreportFooter,
            this.xrTableFooter});
         this.ReportFooter.HeightF = 185.9584F;
         this.ReportFooter.KeepTogether = true;
         this.ReportFooter.Name = "ReportFooter";
         this.ReportFooter.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
         // 
         // xrSubreportFooter
         // 
         this.xrSubreportFooter.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 22.66668F);
         this.xrSubreportFooter.Name = "xrSubreportFooter";
         this.xrSubreportFooter.ReportSource = new BaseGround.Report.ReportFooter();
         this.xrSubreportFooter.SizeF = new System.Drawing.SizeF(767.0001F, 67.95832F);
         // 
         // xrTableFooter
         // 
         this.xrTableFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 100.7917F);
         this.xrTableFooter.Name = "xrTableFooter";
         this.xrTableFooter.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRowMemo,
            this.xrTableRowSeal});
         this.xrTableFooter.SizeF = new System.Drawing.SizeF(787F, 68.12496F);
         // 
         // xrTableRowMemo
         // 
         this.xrTableRowMemo.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellMemo});
         this.xrTableRowMemo.Name = "xrTableRowMemo";
         this.xrTableRowMemo.Visible = false;
         this.xrTableRowMemo.Weight = 1D;
         // 
         // xrTableCellMemo
         // 
         this.xrTableCellMemo.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblMemo});
         this.xrTableCellMemo.Name = "xrTableCellMemo";
         this.xrTableCellMemo.Weight = 3D;
         // 
         // lblMemo
         // 
         this.lblMemo.AutoWidth = true;
         this.lblMemo.Font = new System.Drawing.Font("標楷體", 12F);
         this.lblMemo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
         this.lblMemo.Multiline = true;
         this.lblMemo.Name = "lblMemo";
         this.lblMemo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblMemo.SizeF = new System.Drawing.SizeF(786.9999F, 34.06248F);
         this.lblMemo.StylePriority.UseFont = false;
         this.lblMemo.Visible = false;
         // 
         // xrTableRowSeal
         // 
         this.xrTableRowSeal.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellSeal});
         this.xrTableRowSeal.Name = "xrTableRowSeal";
         this.xrTableRowSeal.Weight = 1D;
         // 
         // xrTableCellSeal
         // 
         this.xrTableCellSeal.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblDoubleConfirmDescription,
            this.lblHandlingDescription,
            this.lblManagerDescription});
         this.xrTableCellSeal.Name = "xrTableCellSeal";
         this.xrTableCellSeal.Weight = 3D;
         // 
         // lblDoubleConfirmDescription
         // 
         this.lblDoubleConfirmDescription.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblDoubleConfirmDescription.LocationFloat = new DevExpress.Utils.PointFloat(366.3752F, 10.00001F);
         this.lblDoubleConfirmDescription.Name = "lblDoubleConfirmDescription";
         this.lblDoubleConfirmDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblDoubleConfirmDescription.SizeF = new System.Drawing.SizeF(58.33334F, 23F);
         this.lblDoubleConfirmDescription.StylePriority.UseFont = false;
         this.lblDoubleConfirmDescription.StylePriority.UseTextAlignment = false;
         this.lblDoubleConfirmDescription.Text = "覆核：";
         this.lblDoubleConfirmDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
         this.lblDoubleConfirmDescription.Visible = false;
         // 
         // lblHandlingDescription
         // 
         this.lblHandlingDescription.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblHandlingDescription.LocationFloat = new DevExpress.Utils.PointFloat(237.625F, 10.00001F);
         this.lblHandlingDescription.Name = "lblHandlingDescription";
         this.lblHandlingDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblHandlingDescription.SizeF = new System.Drawing.SizeF(58.33334F, 23F);
         this.lblHandlingDescription.StylePriority.UseFont = false;
         this.lblHandlingDescription.StylePriority.UseTextAlignment = false;
         this.lblHandlingDescription.Text = "經辦：";
         this.lblHandlingDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
         // 
         // lblManagerDescription
         // 
         this.lblManagerDescription.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblManagerDescription.LocationFloat = new DevExpress.Utils.PointFloat(499.0833F, 10.00001F);
         this.lblManagerDescription.Name = "lblManagerDescription";
         this.lblManagerDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblManagerDescription.SizeF = new System.Drawing.SizeF(54.16663F, 23F);
         this.lblManagerDescription.StylePriority.UseFont = false;
         this.lblManagerDescription.StylePriority.UseTextAlignment = false;
         this.lblManagerDescription.Text = "主管：";
         this.lblManagerDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
         // 
         // CommonReportPortraitA4
         // 
         this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter});
         this.Bookmark = "myBookmark";
         this.BorderColor = System.Drawing.Color.Maroon;
         this.DisplayName = "myDisplayName";
         this.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20);
         this.PageHeight = 1169;
         this.PageWidth = 827;
         this.PaperKind = System.Drawing.Printing.PaperKind.A4;
         this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.ReportTitle,
            this.UserName,
            this.OcfDate,
            this.ReportID,
            this.LeftMemo});
         this.Version = "18.2";
         ((System.ComponentModel.ISupportInitialize)(this.xrTableFooter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.Parameters.Parameter ReportTitle;
        private DevExpress.XtraReports.Parameters.Parameter UserName;
        private DevExpress.XtraReports.Parameters.Parameter OcfDate;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.Parameters.Parameter ReportID;
        public DevExpress.XtraReports.UI.XRSubreport xrSubreportMain;
        public DevExpress.XtraReports.UI.PrintableComponentContainer printableComponentContainerMain;
        private DevExpress.XtraReports.Parameters.Parameter LeftMemo;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRTable xrTableFooter;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowMemo;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellMemo;
        public DevExpress.XtraReports.UI.XRLabel lblMemo;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowSeal;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellSeal;
        private DevExpress.XtraReports.UI.XRLabel lblDoubleConfirmDescription;
        private DevExpress.XtraReports.UI.XRLabel lblHandlingDescription;
        private DevExpress.XtraReports.UI.XRLabel lblManagerDescription;
      private DevExpress.XtraReports.UI.XRSubreport xrSubreportFooter;
   }
}
