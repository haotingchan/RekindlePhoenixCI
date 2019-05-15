namespace BaseGround.Report
{
    partial class RZ0910_TXN
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
            this.xrTableContent = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRowContent = new DevExpress.XtraReports.UI.XRTableRow();
            this.cellApply = new DevExpress.XtraReports.UI.XRTableCell();
            this.cellApproved = new DevExpress.XtraReports.UI.XRTableCell();
            this.cellDefault = new DevExpress.XtraReports.UI.XRTableCell();
            this.cellTxnId = new DevExpress.XtraReports.UI.XRTableCell();
            this.cellTxnName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRowHeader = new DevExpress.XtraReports.UI.XRTableRow();
            this.colApply = new DevExpress.XtraReports.UI.XRTableCell();
            this.colApproved = new DevExpress.XtraReports.UI.XRTableCell();
            this.colDefault = new DevExpress.XtraReports.UI.XRTableCell();
            this.colTxnId = new DevExpress.XtraReports.UI.XRTableCell();
            this.colTxnName = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.OcfDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.UserName = new DevExpress.XtraReports.Parameters.Parameter();
            this.ReportID = new DevExpress.XtraReports.Parameters.Parameter();
            this.ReportTitle = new DevExpress.XtraReports.Parameters.Parameter();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lblReportIDDescription = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportID = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUser = new DevExpress.XtraReports.UI.XRLabel();
            this.lblOperateDateDescription = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCompany = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrTableHeader = new DevExpress.XtraReports.UI.XRTable();
            this.LeftMemo = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrTableFooter = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRowSeal = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCellSeal = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblInstall = new DevExpress.XtraReports.UI.XRLabel();
            this.lblApprovedCount = new DevExpress.XtraReports.UI.XRLabel();
            this.lblApplyCount = new DevExpress.XtraReports.UI.XRLabel();
            this.lblManagerSign = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooterMain = new DevExpress.XtraReports.UI.GroupFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableContent});
            this.Detail.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Detail.HeightF = 25F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableContent
            // 
            this.xrTableContent.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableContent.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTableContent.Name = "xrTableContent";
            this.xrTableContent.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRowContent});
            this.xrTableContent.SizeF = new System.Drawing.SizeF(626F, 25F);
            this.xrTableContent.StylePriority.UseBorders = false;
            // 
            // xrTableRowContent
            // 
            this.xrTableRowContent.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cellApply,
            this.cellApproved,
            this.cellDefault,
            this.cellTxnId,
            this.cellTxnName});
            this.xrTableRowContent.Name = "xrTableRowContent";
            this.xrTableRowContent.Weight = 0.18099547511312214D;
            // 
            // cellApply
            // 
            this.cellApply.Name = "cellApply";
            this.cellApply.Weight = 0.36594670072887958D;
            // 
            // cellApproved
            // 
            this.cellApproved.Name = "cellApproved";
            this.cellApproved.Weight = 0.36594670072887964D;
            // 
            // cellDefault
            // 
            this.cellDefault.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([TXN_DEFAULT] = \'Y\',\'Y\' ,\' \' )")});
            this.cellDefault.Name = "cellDefault";
            this.cellDefault.Weight = 0.36594670072887964D;
            // 
            // cellTxnId
            // 
            this.cellTxnId.Name = "cellTxnId";
            this.cellTxnId.Text = "cellTxnId";
            this.cellTxnId.Weight = 0.6912325030666524D;
            // 
            // cellTxnName
            // 
            this.cellTxnName.Name = "cellTxnName";
            this.cellTxnName.StylePriority.UseTextAlignment = false;
            this.cellTxnName.Text = "cellTxnName";
            this.cellTxnName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.cellTxnName.Weight = 4.5743335501913824D;
            // 
            // xrTableRowHeader
            // 
            this.xrTableRowHeader.BackColor = System.Drawing.Color.LightGray;
            this.xrTableRowHeader.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colApply,
            this.colApproved,
            this.colDefault,
            this.colTxnId,
            this.colTxnName});
            this.xrTableRowHeader.Name = "xrTableRowHeader";
            this.xrTableRowHeader.StylePriority.UseBackColor = false;
            this.xrTableRowHeader.StylePriority.UseTextAlignment = false;
            this.xrTableRowHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableRowHeader.Weight = 1.355294117647059D;
            // 
            // colApply
            // 
            this.colApply.Name = "colApply";
            this.colApply.Text = "申請權限";
            this.colApply.Weight = 0.36594670151723568D;
            // 
            // colApproved
            // 
            this.colApproved.Name = "colApproved";
            this.colApproved.Text = "核准權限";
            this.colApproved.Weight = 0.36594672090579039D;
            // 
            // colDefault
            // 
            this.colDefault.Name = "colDefault";
            this.colDefault.Text = "預設權限";
            this.colDefault.Weight = 0.36594672090579039D;
            // 
            // colTxnId
            // 
            this.colTxnId.Name = "colTxnId";
            this.colTxnId.Text = "交易代號";
            this.colTxnId.Weight = 0.69123248210138544D;
            // 
            // colTxnName
            // 
            this.colTxnName.Name = "colTxnName";
            this.colTxnName.Text = "交易名稱";
            this.colTxnName.Weight = 4.5743339419681D;
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
            this.BottomMargin.HeightF = 41F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblReportIDDescription,
            this.lblReportID,
            this.lblUser,
            this.lblOperateDateDescription,
            this.lblCompany,
            this.lblTitle,
            this.xrPageInfo,
            this.xrTableHeader});
            this.PageHeader.HeightF = 155.4167F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.PageHeader_BeforePrint);
            // 
            // lblReportIDDescription
            // 
            this.lblReportIDDescription.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblReportIDDescription.LocationFloat = new DevExpress.Utils.PointFloat(619.7083F, 64.4166F);
            this.lblReportIDDescription.Name = "lblReportIDDescription";
            this.lblReportIDDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportIDDescription.SizeF = new System.Drawing.SizeF(82.29167F, 23F);
            this.lblReportIDDescription.StylePriority.UseFont = false;
            this.lblReportIDDescription.StylePriority.UseTextAlignment = false;
            this.lblReportIDDescription.Text = "報表代號:";
            this.lblReportIDDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // lblReportID
            // 
            this.lblReportID.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Parameters.ReportID]")});
            this.lblReportID.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblReportID.LocationFloat = new DevExpress.Utils.PointFloat(702F, 64.41673F);
            this.lblReportID.Name = "lblReportID";
            this.lblReportID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportID.SizeF = new System.Drawing.SizeF(75F, 22.99998F);
            this.lblReportID.StylePriority.UseFont = false;
            this.lblReportID.StylePriority.UseTextAlignment = false;
            this.lblReportID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // lblUser
            // 
            this.lblUser.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.LocationFloat = new DevExpress.Utils.PointFloat(294.6248F, 87.41671F);
            this.lblUser.Name = "lblUser";
            this.lblUser.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUser.SizeF = new System.Drawing.SizeF(228.875F, 23F);
            this.lblUser.StylePriority.UseFont = false;
            this.lblUser.Text = "申請人：__________________";
            // 
            // lblOperateDateDescription
            // 
            this.lblOperateDateDescription.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblOperateDateDescription.LocationFloat = new DevExpress.Utils.PointFloat(0F, 87.41668F);
            this.lblOperateDateDescription.Name = "lblOperateDateDescription";
            this.lblOperateDateDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblOperateDateDescription.SizeF = new System.Drawing.SizeF(81.25008F, 23.00002F);
            this.lblOperateDateDescription.StylePriority.UseFont = false;
            this.lblOperateDateDescription.Text = "作業日期:";
            // 
            // lblCompany
            // 
            this.lblCompany.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCompany.LocationFloat = new DevExpress.Utils.PointFloat(335.9583F, 7.395832F);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCompany.SizeF = new System.Drawing.SizeF(132.2917F, 23F);
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
            this.lblTitle.LocationFloat = new DevExpress.Utils.PointFloat(237.0416F, 36.39588F);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTitle.SizeF = new System.Drawing.SizeF(330.2083F, 23F);
            this.lblTitle.StylePriority.UseFont = false;
            this.lblTitle.StylePriority.UseTextAlignment = false;
            this.lblTitle.Text = "標題";
            this.lblTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(655.5834F, 87.41671F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(121.4166F, 23F);
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrPageInfo.TextFormatString = "第{0}頁,共{1}頁";
            // 
            // xrTableHeader
            // 
            this.xrTableHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableHeader.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableHeader.LocationFloat = new DevExpress.Utils.PointFloat(3.178914E-05F, 110.4167F);
            this.xrTableHeader.Name = "xrTableHeader";
            this.xrTableHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRowHeader});
            this.xrTableHeader.SizeF = new System.Drawing.SizeF(626F, 45F);
            this.xrTableHeader.StylePriority.UseBorders = false;
            this.xrTableHeader.StylePriority.UseFont = false;
            // 
            // LeftMemo
            // 
            this.LeftMemo.Name = "LeftMemo";
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 35.91636F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrTableFooter
            // 
            this.xrTableFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 7.000001F);
            this.xrTableFooter.Name = "xrTableFooter";
            this.xrTableFooter.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRowSeal});
            this.xrTableFooter.SizeF = new System.Drawing.SizeF(786.9999F, 68.54166F);
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
            this.lblInstall,
            this.lblApprovedCount,
            this.lblApplyCount,
            this.lblManagerSign});
            this.xrTableCellSeal.Name = "xrTableCellSeal";
            this.xrTableCellSeal.Weight = 3D;
            // 
            // lblInstall
            // 
            this.lblInstall.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstall.LocationFloat = new DevExpress.Utils.PointFloat(3.178914E-05F, 45.54167F);
            this.lblInstall.Name = "lblInstall";
            this.lblInstall.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblInstall.SizeF = new System.Drawing.SizeF(272.9167F, 22.99999F);
            this.lblInstall.StylePriority.UseFont = false;
            this.lblInstall.StylePriority.UseTextAlignment = false;
            this.lblInstall.Text = "申請安裝系統：□是   □否";
            this.lblInstall.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblApprovedCount
            // 
            this.lblApprovedCount.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApprovedCount.LocationFloat = new DevExpress.Utils.PointFloat(218.8752F, 10.00001F);
            this.lblApprovedCount.Name = "lblApprovedCount";
            this.lblApprovedCount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblApprovedCount.SizeF = new System.Drawing.SizeF(195.8333F, 23F);
            this.lblApprovedCount.StylePriority.UseFont = false;
            this.lblApprovedCount.StylePriority.UseTextAlignment = false;
            this.lblApprovedCount.Text = "核准權限計____項";
            this.lblApprovedCount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblApplyCount
            // 
            this.lblApplyCount.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplyCount.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.lblApplyCount.Name = "lblApplyCount";
            this.lblApplyCount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblApplyCount.SizeF = new System.Drawing.SizeF(193.75F, 22.99999F);
            this.lblApplyCount.StylePriority.UseFont = false;
            this.lblApplyCount.StylePriority.UseTextAlignment = false;
            this.lblApplyCount.Text = "申請權限計_____項";
            this.lblApplyCount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblManagerSign
            // 
            this.lblManagerSign.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblManagerSign.LocationFloat = new DevExpress.Utils.PointFloat(465.4999F, 10.00001F);
            this.lblManagerSign.Name = "lblManagerSign";
            this.lblManagerSign.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblManagerSign.SizeF = new System.Drawing.SizeF(321.5F, 22.99999F);
            this.lblManagerSign.StylePriority.UseFont = false;
            this.lblManagerSign.StylePriority.UseTextAlignment = false;
            this.lblManagerSign.Text = "最高權限人簽章：_____________________";
            this.lblManagerSign.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // GroupFooterMain
            // 
            this.GroupFooterMain.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableFooter});
            this.GroupFooterMain.HeightF = 85.54166F;
            this.GroupFooterMain.Name = "GroupFooterMain";
            // 
            // RZ0910_TXN
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter,
            this.GroupFooterMain});
            this.Bookmark = "myBookmark";
            this.DisplayName = "myDisplayName";
            this.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 41);
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
            ((System.ComponentModel.ISupportInitialize)(this.xrTableContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableFooter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.Parameters.Parameter ReportTitle;
        private DevExpress.XtraReports.Parameters.Parameter UserName;
        private DevExpress.XtraReports.Parameters.Parameter OcfDate;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.Parameters.Parameter ReportID;
        private DevExpress.XtraReports.Parameters.Parameter LeftMemo;
        private DevExpress.XtraReports.UI.XRTable xrTableFooter;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowSeal;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellSeal;
        private DevExpress.XtraReports.UI.XRLabel lblApprovedCount;
        private DevExpress.XtraReports.UI.XRLabel lblApplyCount;
        private DevExpress.XtraReports.UI.XRLabel lblManagerSign;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooterMain;
        private DevExpress.XtraReports.UI.XRTable xrTableContent;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowHeader;
        private DevExpress.XtraReports.UI.XRTableCell colApply;
        private DevExpress.XtraReports.UI.XRTableCell colApproved;
        private DevExpress.XtraReports.UI.XRTableCell colTxnId;
        private DevExpress.XtraReports.UI.XRTableCell colTxnName;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowContent;
        private DevExpress.XtraReports.UI.XRTableCell cellApply;
        private DevExpress.XtraReports.UI.XRTableCell cellApproved;
        private DevExpress.XtraReports.UI.XRTableCell cellTxnId;
        private DevExpress.XtraReports.UI.XRTableCell cellTxnName;
        private DevExpress.XtraReports.UI.XRTable xrTableHeader;
        private DevExpress.XtraReports.UI.XRLabel lblCompany;
        private DevExpress.XtraReports.UI.XRLabel lblTitle;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo;
        private DevExpress.XtraReports.UI.XRLabel lblUser;
        private DevExpress.XtraReports.UI.XRLabel lblOperateDateDescription;
        private DevExpress.XtraReports.UI.XRLabel lblInstall;
        private DevExpress.XtraReports.UI.XRTableCell cellDefault;
        private DevExpress.XtraReports.UI.XRTableCell colDefault;
        private DevExpress.XtraReports.UI.XRLabel lblReportIDDescription;
        private DevExpress.XtraReports.UI.XRLabel lblReportID;
    }
}
