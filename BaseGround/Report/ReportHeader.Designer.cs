namespace BaseGround.Report
{
    partial class ReportHeader
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
         this.lblLeftMemo = new DevExpress.XtraReports.UI.XRLabel();
         this.lblOperateTime = new DevExpress.XtraReports.UI.XRPageInfo();
         this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
         this.lblOperateUser = new DevExpress.XtraReports.UI.XRLabel();
         this.lblOperateUserDescription = new DevExpress.XtraReports.UI.XRLabel();
         this.lblReportID = new DevExpress.XtraReports.UI.XRLabel();
         this.lblOperateTimeDescription = new DevExpress.XtraReports.UI.XRLabel();
         this.lblReportIDDescription = new DevExpress.XtraReports.UI.XRLabel();
         this.lblCompany = new DevExpress.XtraReports.UI.XRLabel();
         this.lblTitle = new DevExpress.XtraReports.UI.XRLabel();
         this.OcfDate = new DevExpress.XtraReports.Parameters.Parameter();
         this.UserName = new DevExpress.XtraReports.Parameters.Parameter();
         this.ReportID = new DevExpress.XtraReports.Parameters.Parameter();
         this.ReportTitle = new DevExpress.XtraReports.Parameters.Parameter();
         this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
         this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
         this.LeftMemo = new DevExpress.XtraReports.Parameters.Parameter();
         ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
         // 
         // Detail
         // 
         this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblLeftMemo,
            this.lblOperateTime,
            this.xrPageInfo,
            this.lblOperateUser,
            this.lblOperateUserDescription,
            this.lblReportID,
            this.lblOperateTimeDescription,
            this.lblReportIDDescription,
            this.lblCompany,
            this.lblTitle});
         this.Detail.HeightF = 154.8335F;
         this.Detail.Name = "Detail";
         this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
         // 
         // lblLeftMemo
         // 
         this.lblLeftMemo.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Parameters].[LeftMemo]")});
         this.lblLeftMemo.Font = new System.Drawing.Font("標楷體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblLeftMemo.LocationFloat = new DevExpress.Utils.PointFloat(0.000335552F, 121.8335F);
         this.lblLeftMemo.Name = "lblLeftMemo";
         this.lblLeftMemo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblLeftMemo.SizeF = new System.Drawing.SizeF(786.9997F, 23.00001F);
         this.lblLeftMemo.StylePriority.UseFont = false;
         this.lblLeftMemo.StylePriority.UseTextAlignment = false;
         this.lblLeftMemo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
         // 
         // lblOperateTime
         // 
         this.lblOperateTime.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblOperateTime.LocationFloat = new DevExpress.Utils.PointFloat(82.29163F, 98.8334F);
         this.lblOperateTime.Name = "lblOperateTime";
         this.lblOperateTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblOperateTime.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
         this.lblOperateTime.SizeF = new System.Drawing.SizeF(202.4999F, 23F);
         this.lblOperateTime.StylePriority.UseFont = false;
         this.lblOperateTime.TextFormatString = "{0:yyyy/MM/dd HH:mm:ss}";
         // 
         // xrPageInfo
         // 
         this.xrPageInfo.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(674.5F, 98.8334F);
         this.xrPageInfo.Name = "xrPageInfo";
         this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.xrPageInfo.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
         this.xrPageInfo.SizeF = new System.Drawing.SizeF(112.4998F, 23F);
         this.xrPageInfo.StylePriority.UseFont = false;
         this.xrPageInfo.StylePriority.UseTextAlignment = false;
         this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
         this.xrPageInfo.TextFormatString = "第{0}頁,共{1}頁";
         // 
         // lblOperateUser
         // 
         this.lblOperateUser.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Parameters.UserName]")});
         this.lblOperateUser.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblOperateUser.LocationFloat = new DevExpress.Utils.PointFloat(80.20829F, 75.83344F);
         this.lblOperateUser.Name = "lblOperateUser";
         this.lblOperateUser.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblOperateUser.SizeF = new System.Drawing.SizeF(68.75F, 23F);
         this.lblOperateUser.StylePriority.UseFont = false;
         // 
         // lblOperateUserDescription
         // 
         this.lblOperateUserDescription.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblOperateUserDescription.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75.83344F);
         this.lblOperateUserDescription.Name = "lblOperateUserDescription";
         this.lblOperateUserDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblOperateUserDescription.SizeF = new System.Drawing.SizeF(80.20831F, 23F);
         this.lblOperateUserDescription.StylePriority.UseFont = false;
         this.lblOperateUserDescription.Text = "作業人員:";
         // 
         // lblReportID
         // 
         this.lblReportID.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Parameters.ReportID]")});
         this.lblReportID.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblReportID.LocationFloat = new DevExpress.Utils.PointFloat(711.9998F, 75.8335F);
         this.lblReportID.Name = "lblReportID";
         this.lblReportID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblReportID.SizeF = new System.Drawing.SizeF(75F, 22.99998F);
         this.lblReportID.StylePriority.UseFont = false;
         this.lblReportID.StylePriority.UseTextAlignment = false;
         this.lblReportID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
         // 
         // lblOperateTimeDescription
         // 
         this.lblOperateTimeDescription.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblOperateTimeDescription.LocationFloat = new DevExpress.Utils.PointFloat(0F, 98.83345F);
         this.lblOperateTimeDescription.Name = "lblOperateTimeDescription";
         this.lblOperateTimeDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblOperateTimeDescription.SizeF = new System.Drawing.SizeF(82.29167F, 23.00002F);
         this.lblOperateTimeDescription.StylePriority.UseFont = false;
         this.lblOperateTimeDescription.StylePriority.UseTextAlignment = false;
         this.lblOperateTimeDescription.Text = "作業時間:";
         // 
         // lblReportIDDescription
         // 
         this.lblReportIDDescription.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblReportIDDescription.LocationFloat = new DevExpress.Utils.PointFloat(629.7081F, 75.83336F);
         this.lblReportIDDescription.Name = "lblReportIDDescription";
         this.lblReportIDDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblReportIDDescription.SizeF = new System.Drawing.SizeF(82.29167F, 23F);
         this.lblReportIDDescription.StylePriority.UseFont = false;
         this.lblReportIDDescription.StylePriority.UseTextAlignment = false;
         this.lblReportIDDescription.Text = "報表代號:";
         this.lblReportIDDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
         // 
         // lblCompany
         // 
         this.lblCompany.Font = new System.Drawing.Font("標楷體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblCompany.LocationFloat = new DevExpress.Utils.PointFloat(298.6667F, 10.00001F);
         this.lblCompany.Name = "lblCompany";
         this.lblCompany.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblCompany.SizeF = new System.Drawing.SizeF(193.7501F, 23F);
         this.lblCompany.StylePriority.UseFont = false;
         this.lblCompany.StylePriority.UseTextAlignment = false;
         this.lblCompany.Text = "臺灣期貨交易所";
         this.lblCompany.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
         // 
         // lblTitle
         // 
         this.lblTitle.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Parameters.ReportTitle]")});
         this.lblTitle.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.lblTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 39.00003F);
         this.lblTitle.Name = "lblTitle";
         this.lblTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
         this.lblTitle.SizeF = new System.Drawing.SizeF(786.9998F, 23F);
         this.lblTitle.StylePriority.UseFont = false;
         this.lblTitle.StylePriority.UseTextAlignment = false;
         this.lblTitle.Text = "標題";
         this.lblTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
         // 
         // OcfDate
         // 
         this.OcfDate.Description = "OcfDate";
         this.OcfDate.Name = "OcfDate";
         // 
         // UserName
         // 
         this.UserName.Description = "UserName";
         this.UserName.Name = "UserName";
         // 
         // ReportID
         // 
         this.ReportID.Description = "ReportID";
         this.ReportID.Name = "ReportID";
         // 
         // ReportTitle
         // 
         this.ReportTitle.Description = "ReportTitle";
         this.ReportTitle.Name = "ReportTitle";
         // 
         // TopMargin
         // 
         this.TopMargin.HeightF = 6F;
         this.TopMargin.Name = "TopMargin";
         this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
         // 
         // BottomMargin
         // 
         this.BottomMargin.HeightF = 22.84703F;
         this.BottomMargin.Name = "BottomMargin";
         this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
         this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
         // 
         // LeftMemo
         // 
         this.LeftMemo.Name = "LeftMemo";
         // 
         // ReportHeader
         // 
         this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
         this.Margins = new System.Drawing.Printing.Margins(20, 20, 6, 23);
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
         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPageInfo lblOperateTime;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo;
        private DevExpress.XtraReports.UI.XRLabel lblOperateUser;
        private DevExpress.XtraReports.UI.XRLabel lblOperateUserDescription;
        private DevExpress.XtraReports.UI.XRLabel lblReportID;
        private DevExpress.XtraReports.UI.XRLabel lblOperateTimeDescription;
        private DevExpress.XtraReports.UI.XRLabel lblReportIDDescription;
        private DevExpress.XtraReports.UI.XRLabel lblCompany;
        private DevExpress.XtraReports.UI.XRLabel lblTitle;
        private DevExpress.XtraReports.Parameters.Parameter ReportTitle;
        private DevExpress.XtraReports.Parameters.Parameter UserName;
        private DevExpress.XtraReports.Parameters.Parameter OcfDate;
        private DevExpress.XtraReports.Parameters.Parameter ReportID;
        private DevExpress.XtraReports.UI.XRLabel lblLeftMemo;
        private DevExpress.XtraReports.Parameters.Parameter LeftMemo;
    }
}
