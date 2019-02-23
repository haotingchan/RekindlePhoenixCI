namespace BaseGround.Report
{
   partial class defReport
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

      #region Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
         this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
         this.Detail = new DevExpress.XtraReports.UI.DetailBand();
         this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
         ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
         // 
         // TopMargin
         // 
         this.TopMargin.HeightF = 10F;
         this.TopMargin.Name = "TopMargin";
         // 
         // BottomMargin
         // 
         this.BottomMargin.HeightF = 10F;
         this.BottomMargin.Name = "BottomMargin";
         // 
         // Detail
         // 
         this.Detail.HeightF = 0F;
         this.Detail.Name = "Detail";
         // 
         // PageHeader
         // 
         this.PageHeader.HeightF = 1.458327F;
         this.PageHeader.Name = "PageHeader";
         // 
         // defReport
         // 
         this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.PageHeader});
         this.ExportOptions.Xls.ShowGridLines = true;
         this.ExportOptions.Xls.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
         this.ExportOptions.Xls.WorkbookColorPaletteCompliance = DevExpress.XtraPrinting.WorkbookColorPaletteCompliance.AdjustColorsToDefaultPalette;
         this.Font = new System.Drawing.Font("Arial", 9.75F);
         this.Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);
         this.PageHeight = 827;
         this.PageWidth = 1169;
         this.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
         this.Version = "18.2";
         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

      }

      #endregion

      private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
      private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
      private DevExpress.XtraReports.UI.DetailBand Detail;
      private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
   }
}
