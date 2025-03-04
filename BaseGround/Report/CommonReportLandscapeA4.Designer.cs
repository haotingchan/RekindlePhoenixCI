﻿namespace BaseGround.Report {
   partial class CommonReportLandscapeA4 {
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

      #region Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.Detail = new DevExpress.XtraReports.UI.DetailBand();
         this.printableComponentContainerMain = new DevExpress.XtraReports.UI.PrintableComponentContainer();
         this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
         this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
         this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
         this.ReportTitle = new DevExpress.XtraReports.Parameters.Parameter();
         this.UserName = new DevExpress.XtraReports.Parameters.Parameter();
         this.OcfDate = new DevExpress.XtraReports.Parameters.Parameter();
         this.ReportID = new DevExpress.XtraReports.Parameters.Parameter();
         this.LeftMemo = new DevExpress.XtraReports.Parameters.Parameter();
         this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
         this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
         this.xrTableFooter = new DevExpress.XtraReports.UI.XRTable();
         this.xrTableRowMemo = new DevExpress.XtraReports.UI.XRTableRow();
         this.xrTableCellMemo = new DevExpress.XtraReports.UI.XRTableCell();
         this.lblMemo = new DevExpress.XtraReports.UI.XRLabel();
         this.xrTableRowSeal = new DevExpress.XtraReports.UI.XRTableRow();
         this.xrTableCellSeal = new DevExpress.XtraReports.UI.XRTableCell();
         this.lblDoubleConfirmDescription = new DevExpress.XtraReports.UI.XRLabel();
         this.lblHandlingDescription = new DevExpress.XtraReports.UI.XRLabel();
         this.lblManagerDescription = new DevExpress.XtraReports.UI.XRLabel();
         this.FooterMemo = new DevExpress.XtraReports.Parameters.Parameter();
         this.xrSubreportMain = new DevExpress.XtraReports.UI.XRSubreport();
         this.xrSubreportFooter = new DevExpress.XtraReports.UI.XRSubreport();
         ((System.ComponentModel.ISupportInitialize)(this.xrTableFooter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
         // 
         // Detail
         // 
         this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.printableComponentContainerMain});
         this.Detail.HeightF = 43.37495F;
         this.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown;
         this.Detail.Name = "Detail";
         this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBandExceptLastEntry;
         this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
         // 
         // printableComponentContainerMain
         // 
         this.printableComponentContainerMain.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
         this.printableComponentContainerMain.Name = "printableComponentContainerMain";
         this.printableComponentContainerMain.SizeF = new System.Drawing.SizeF(1129F, 33.3333F);
         // 
         // TopMargin
         // 
         this.TopMargin.HeightF = 20F;
         this.TopMargin.Name = "TopMargin";
         this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
         // 
         // BottomMargin
         // 
         this.BottomMargin.HeightF = 20F;
         this.BottomMargin.Name = "BottomMargin";
         this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
         // 
         // PageHeader
         // 
         this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreportMain});
         this.PageHeader.HeightF = 44.7917F;
         this.PageHeader.Name = "PageHeader";
         // 
         // ReportTitle
         // 
         this.ReportTitle.Description = "ReportTitle";
         this.ReportTitle.Name = "ReportTitle";
         // 
         // UserName
         // 
         this.UserName.Description = "UserName";
         this.UserName.Name = "UserName";
         // 
         // OcfDate
         // 
         this.OcfDate.Description = "OCF_DATE";
         this.OcfDate.Name = "OcfDate";
         // 
         // ReportID
         // 
         this.ReportID.Description = "ReportID";
         this.ReportID.Name = "ReportID";
         // 
         // LeftMemo
         // 
         this.LeftMemo.Description = "LeftMemo";
         this.LeftMemo.Name = "LeftMemo";
         // 
         // PageFooter
         // 
         this.PageFooter.HeightF = 75.16673F;
         this.PageFooter.Name = "PageFooter";
         this.PageFooter.PrintOn = ((DevExpress.XtraReports.UI.PrintOnPages)((DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader | DevExpress.XtraReports.UI.PrintOnPages.NotWithReportFooter)));
         // 
         // ReportFooter
         // 
         this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableFooter,
            this.xrSubreportFooter});
         this.ReportFooter.HeightF = 180.167F;
         this.ReportFooter.KeepTogether = true;
         this.ReportFooter.Name = "ReportFooter";
         this.ReportFooter.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
         // 
         // xrTableFooter
         // 
         this.xrTableFooter.LocationFloat = new DevExpress.Utils.PointFloat(150F, 94.54168F);
         this.xrTableFooter.Name = "xrTableFooter";
         this.xrTableFooter.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRowMemo,
            this.xrTableRowSeal});
         this.xrTableFooter.SizeF = new System.Drawing.SizeF(822.75F, 68.12496F);
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
         // FooterMemo
         // 
         this.FooterMemo.Description = "FooterMemo";
         this.FooterMemo.Name = "FooterMemo";
         // 
         // xrSubreportMain
         // 
         this.xrSubreportMain.LocationFloat = new DevExpress.Utils.PointFloat(150F, 0F);
         this.xrSubreportMain.Name = "xrSubreportMain";
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("ReportTitle", this.ReportTitle));
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("UserName", this.UserName));
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("OcfDate", this.OcfDate));
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("ReportID", this.ReportID));
         this.xrSubreportMain.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("LeftMemo", this.LeftMemo));
         this.xrSubreportMain.ReportSource = new BaseGround.Report.ReportHeader();
         this.xrSubreportMain.SizeF = new System.Drawing.SizeF(822.75F, 44.7917F);
         // 
         // xrSubreportFooter
         // 
         this.xrSubreportFooter.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00004F);
         this.xrSubreportFooter.Name = "xrSubreportFooter";
         this.xrSubreportFooter.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("FooterMemo", this.FooterMemo));
         this.xrSubreportFooter.ReportSource = new BaseGround.Report.ReportFooter();
         this.xrSubreportFooter.SizeF = new System.Drawing.SizeF(1109F, 67.95832F);
         // 
         // CommonReportLandscapeA4
         // 
         this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter});
         this.Landscape = true;
         this.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20);
         this.PageHeight = 827;
         this.PageWidth = 1169;
         this.PaperKind = System.Drawing.Printing.PaperKind.A4;
         this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.OcfDate,
            this.ReportID,
            this.ReportTitle,
            this.UserName,
            this.LeftMemo,
            this.FooterMemo});
         this.Version = "18.2";
         ((System.ComponentModel.ISupportInitialize)(this.xrTableFooter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

      }

      #endregion

      private DevExpress.XtraReports.UI.DetailBand Detail;
      private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
      private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
      public DevExpress.XtraReports.UI.PrintableComponentContainer printableComponentContainerMain;
      private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
      public DevExpress.XtraReports.UI.XRSubreport xrSubreportMain;
      private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
      private DevExpress.XtraReports.Parameters.Parameter OcfDate;
      private DevExpress.XtraReports.Parameters.Parameter ReportID;
      private DevExpress.XtraReports.Parameters.Parameter ReportTitle;
      private DevExpress.XtraReports.Parameters.Parameter UserName;
      private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
      private DevExpress.XtraReports.Parameters.Parameter LeftMemo;
      private DevExpress.XtraReports.Parameters.Parameter FooterMemo;
      private DevExpress.XtraReports.UI.XRSubreport xrSubreportFooter;
      private DevExpress.XtraReports.UI.XRTable xrTableFooter;
      private DevExpress.XtraReports.UI.XRTableRow xrTableRowMemo;
      private DevExpress.XtraReports.UI.XRTableCell xrTableCellMemo;
      public DevExpress.XtraReports.UI.XRLabel lblMemo;
      private DevExpress.XtraReports.UI.XRTableRow xrTableRowSeal;
      private DevExpress.XtraReports.UI.XRTableCell xrTableCellSeal;
      private DevExpress.XtraReports.UI.XRLabel lblDoubleConfirmDescription;
      private DevExpress.XtraReports.UI.XRLabel lblHandlingDescription;
      private DevExpress.XtraReports.UI.XRLabel lblManagerDescription;
   }
}
