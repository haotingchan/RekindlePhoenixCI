namespace BaseGround.Report
{
    partial class RZ0011
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
            this.lblUserPwdTxt = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUserIdTxt = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUserNameTxt = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUserPwd = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUserId = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUserName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTittle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrTableFooter = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRowMemo = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCellMemo = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblMemo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRowSeal = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCellSeal = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblDoubleConfirmDescription = new DevExpress.XtraReports.UI.XRLabel();
            this.lblHandlingDescription = new DevExpress.XtraReports.UI.XRLabel();
            this.lblManagerDescription = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooterMain = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblWdate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblWdateTxt = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Detail.HeightF = 34.375F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            this.Detail.StylePriority.UseFont = false;
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblWdateTxt,
            this.lblWdate,
            this.lblUserPwdTxt,
            this.lblUserIdTxt,
            this.lblUserNameTxt,
            this.lblUserPwd,
            this.lblUserId,
            this.lblUserName,
            this.lblTittle,
            this.xrLine1});
            this.PageFooter.HeightF = 244.2497F;
            this.PageFooter.Name = "PageFooter";
            // 
            // lblUserPwdTxt
            // 
            this.lblUserPwdTxt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblUserPwdTxt.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblUserPwdTxt.LocationFloat = new DevExpress.Utils.PointFloat(126.0417F, 156.6664F);
            this.lblUserPwdTxt.Name = "lblUserPwdTxt";
            this.lblUserPwdTxt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUserPwdTxt.SizeF = new System.Drawing.SizeF(169.9167F, 22.99999F);
            this.lblUserPwdTxt.StylePriority.UseBorders = false;
            this.lblUserPwdTxt.StylePriority.UseFont = false;
            this.lblUserPwdTxt.StylePriority.UseTextAlignment = false;
            this.lblUserPwdTxt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblUserIdTxt
            // 
            this.lblUserIdTxt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblUserIdTxt.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblUserIdTxt.LocationFloat = new DevExpress.Utils.PointFloat(126.0416F, 123.2498F);
            this.lblUserIdTxt.Name = "lblUserIdTxt";
            this.lblUserIdTxt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUserIdTxt.SizeF = new System.Drawing.SizeF(169.9167F, 22.99999F);
            this.lblUserIdTxt.StylePriority.UseBorders = false;
            this.lblUserIdTxt.StylePriority.UseFont = false;
            this.lblUserIdTxt.StylePriority.UseTextAlignment = false;
            this.lblUserIdTxt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblUserNameTxt
            // 
            this.lblUserNameTxt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblUserNameTxt.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblUserNameTxt.LocationFloat = new DevExpress.Utils.PointFloat(126.0417F, 87.74973F);
            this.lblUserNameTxt.Name = "lblUserNameTxt";
            this.lblUserNameTxt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUserNameTxt.SizeF = new System.Drawing.SizeF(169.9167F, 22.99999F);
            this.lblUserNameTxt.StylePriority.UseBorders = false;
            this.lblUserNameTxt.StylePriority.UseFont = false;
            this.lblUserNameTxt.StylePriority.UseTextAlignment = false;
            this.lblUserNameTxt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblUserPwd
            // 
            this.lblUserPwd.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserPwd.LocationFloat = new DevExpress.Utils.PointFloat(0F, 156.6664F);
            this.lblUserPwd.Name = "lblUserPwd";
            this.lblUserPwd.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUserPwd.SizeF = new System.Drawing.SizeF(115.625F, 23F);
            this.lblUserPwd.StylePriority.UseFont = false;
            this.lblUserPwd.StylePriority.UseTextAlignment = false;
            this.lblUserPwd.Text = "密碼：";
            this.lblUserPwd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblUserId
            // 
            this.lblUserId.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserId.LocationFloat = new DevExpress.Utils.PointFloat(0F, 123.2498F);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUserId.SizeF = new System.Drawing.SizeF(115.625F, 23F);
            this.lblUserId.StylePriority.UseFont = false;
            this.lblUserId.StylePriority.UseTextAlignment = false;
            this.lblUserId.Text = "使用者代號：";
            this.lblUserId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.LocationFloat = new DevExpress.Utils.PointFloat(0F, 87.7498F);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUserName.SizeF = new System.Drawing.SizeF(115.625F, 23F);
            this.lblUserName.StylePriority.UseFont = false;
            this.lblUserName.StylePriority.UseTextAlignment = false;
            this.lblUserName.Text = "姓名：";
            this.lblUserName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblTittle
            // 
            this.lblTittle.Font = new System.Drawing.Font("標楷體", 12F);
            this.lblTittle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 50.08332F);
            this.lblTittle.Name = "lblTittle";
            this.lblTittle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTittle.SizeF = new System.Drawing.SizeF(170.8333F, 23F);
            this.lblTittle.StylePriority.UseFont = false;
            this.lblTittle.Text = "交易資訊統計管理系統";
            // 
            // xrLine1
            // 
            this.xrLine1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLine1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLine1.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(786.9999F, 23F);
            this.xrLine1.StylePriority.UseBorderDashStyle = false;
            this.xrLine1.StylePriority.UseBorders = false;
            // 
            // xrTableFooter
            // 
            this.xrTableFooter.LocationFloat = new DevExpress.Utils.PointFloat(2.384186E-05F, 7F);
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
            this.lblDoubleConfirmDescription.LocationFloat = new DevExpress.Utils.PointFloat(356.3752F, 10.00001F);
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
            this.lblHandlingDescription.LocationFloat = new DevExpress.Utils.PointFloat(227.625F, 10.00001F);
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
            this.lblManagerDescription.LocationFloat = new DevExpress.Utils.PointFloat(489.0833F, 10.00001F);
            this.lblManagerDescription.Name = "lblManagerDescription";
            this.lblManagerDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblManagerDescription.SizeF = new System.Drawing.SizeF(54.16663F, 23F);
            this.lblManagerDescription.StylePriority.UseFont = false;
            this.lblManagerDescription.StylePriority.UseTextAlignment = false;
            this.lblManagerDescription.Text = "主管：";
            this.lblManagerDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // GroupFooterMain
            // 
            this.GroupFooterMain.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableFooter});
            this.GroupFooterMain.HeightF = 75.12496F;
            this.GroupFooterMain.Name = "GroupFooterMain";
            this.GroupFooterMain.RepeatEveryPage = true;
            // 
            // lblWdate
            // 
            this.lblWdate.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWdate.LocationFloat = new DevExpress.Utils.PointFloat(398.1875F, 87.74973F);
            this.lblWdate.Name = "lblWdate";
            this.lblWdate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblWdate.SizeF = new System.Drawing.SizeF(115.625F, 23F);
            this.lblWdate.StylePriority.UseFont = false;
            this.lblWdate.StylePriority.UseTextAlignment = false;
            this.lblWdate.Text = "設定日期：";
            this.lblWdate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblWdateTxt
            // 
            this.lblWdateTxt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblWdateTxt.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblWdateTxt.LocationFloat = new DevExpress.Utils.PointFloat(525.2084F, 87.74973F);
            this.lblWdateTxt.Name = "lblWdateTxt";
            this.lblWdateTxt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblWdateTxt.SizeF = new System.Drawing.SizeF(169.9167F, 22.99999F);
            this.lblWdateTxt.StylePriority.UseBorders = false;
            this.lblWdateTxt.StylePriority.UseFont = false;
            this.lblWdateTxt.StylePriority.UseTextAlignment = false;
            this.lblWdateTxt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblWdateTxt.TextFormatString = "{0:yyyy/MM/dd}";
            // 
            // RZ0011
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
            this.Version = "17.2";
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
        public DevExpress.XtraReports.UI.XRSubreport xrSubreportMain;
        private DevExpress.XtraReports.Parameters.Parameter LeftMemo;
        private DevExpress.XtraReports.UI.XRTable xrTableFooter;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowMemo;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellMemo;
        public DevExpress.XtraReports.UI.XRLabel lblMemo;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRowSeal;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellSeal;
        private DevExpress.XtraReports.UI.XRLabel lblDoubleConfirmDescription;
        private DevExpress.XtraReports.UI.XRLabel lblHandlingDescription;
        private DevExpress.XtraReports.UI.XRLabel lblManagerDescription;
        private DevExpress.XtraReports.UI.XRLabel lblUserPwd;
        private DevExpress.XtraReports.UI.XRLabel lblUserId;
        private DevExpress.XtraReports.UI.XRLabel lblUserName;
        private DevExpress.XtraReports.UI.XRLabel lblTittle;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        public DevExpress.XtraReports.UI.XRLabel lblUserPwdTxt;
        public DevExpress.XtraReports.UI.XRLabel lblUserIdTxt;
        public DevExpress.XtraReports.UI.XRLabel lblUserNameTxt;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooterMain;
        public DevExpress.XtraReports.UI.XRLabel lblWdateTxt;
        private DevExpress.XtraReports.UI.XRLabel lblWdate;
    }
}
