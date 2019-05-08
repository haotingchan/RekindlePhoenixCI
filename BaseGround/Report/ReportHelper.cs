using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DevExpress.Utils;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using System;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;

namespace BaseGround.Report
{
   public class ReportHelper
   {
      #region 變數區

      // 解決DevExpress會隨機遇到的錯誤
      // 此錯誤是因為current value of the floating point control word (fpcw)的原因
      // 會造成呼叫ExportToPdf出現DivideByZeroException的嘗試以零除的錯誤
      [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
      private static extern int _controlfp(int newControl, int mask);

      private XtraReport _MainReport;

      private bool _Landscape;

      private PaperKind _PaperKind;

      private object _PrintableComponent;

      private string _ReportID;

      private string _ReportTitle;

      private string _LeftMemo;

      private string _FooterMemo;

      private string _FilePath;

      private FileType _FileType;

      private bool _IsPrintedFromPrintButton = false;

      #endregion 變數區

      private ReportHelper()
      {
         PaperKind = PaperKind.A4;
         Landscape = false;
      }

      public ReportHelper(object pComponent, string reportID, string reportTitle) : this()
      {
         PrintableComponent = pComponent;
         ReportID = reportID;
         ReportTitle = reportTitle;
         SettingReport(new CommonReportPortraitA4());
      }

      #region 報表屬性

      public XtraReport MainReport {
         get { return _MainReport; }
         set { _MainReport = value; }
      }

      public bool Landscape {
         get {
            return _Landscape;
         }

         set {
            _Landscape = value;
         }
      }

      public PaperKind PaperKind {
         get {
            return _PaperKind;
         }

         set {
            _PaperKind = value;
         }
      }

      public object PrintableComponent {
         get {
            return _PrintableComponent;
         }

         set {
            _PrintableComponent = value;

            if (_MainReport != null) {
               PrintableComponentContainer pcc = (PrintableComponentContainer)_MainReport.FindControl("printableComponentContainerMain", true);
               if (pcc != null) {
                  pcc.PrintableComponent = value;
               }
            }
         }
      }

      public string ReportID {
         get {
            return _ReportID;
         }

         set {
            _ReportID = value;

            if (_MainReport != null) {
               Parameter pReportID = _MainReport.Parameters["ReportID"];
               if (pReportID != null) {
                  pReportID.Value = value;
                  pReportID.Visible = false;
               }
            }
         }
      }

      public string ReportTitle {
         get {
            return _ReportTitle;
         }

         set {
            _ReportTitle = value;

            if (_MainReport != null) {
               Parameter pReportTitle = _MainReport.Parameters["ReportTitle"];
               if (pReportTitle != null) {
                  pReportTitle.Value = value;
                  pReportTitle.Visible = false;
               }
            }

            // 改ReportTitle預設就改一下匯出檔案的路徑
            FilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, "CI_" + value + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
         }
      }

      public string LeftMemo {
         get {
            return _LeftMemo;
         }

         set {
            _LeftMemo = value;

            if (_MainReport != null) {
               Parameter pLeftMemo = _MainReport.Parameters["LeftMemo"];
               if (pLeftMemo != null) {
                  pLeftMemo.Value = value;
                  pLeftMemo.Visible = false;
               }
            }
         }
      }

      public string FooterMemo {
         get {
            return _FooterMemo;
         }

         set {
            _FooterMemo = value;

            if (_MainReport != null) {
               Parameter pFooterMemo = _MainReport.Parameters["FooterMemo"];
               if (pFooterMemo != null) {
                  pFooterMemo.Value = value;
                  pFooterMemo.Visible = false;
               }
            }
         }
      }

      public string FilePath {
         get {
            return _FilePath;
         }

         set {
            _FilePath = value;
         }
      }

      public FileType FileType {
         get {
            return _FileType;
         }

         set {
            _FileType = value;
         }
      }

      public bool IsPrintedFromPrintButton {
         get {
            return _IsPrintedFromPrintButton;
         }

         set {
            _IsPrintedFromPrintButton = value;
         }
      }

      public bool IsHandlePersonVisible { get; set; }
      public bool IsManagerVisible { get; set; }

      #endregion 報表屬性

      public void Preview()
      {
         _MainReport.CreateDocument();

         using (ReportPrintTool printTool = new ReportPrintTool(_MainReport)) {
            printTool.ShowPreviewDialog();
         }
      }

      public void Print()
      {
         AddReportTitleForNight();

         _MainReport.CreateDocument();

         using (ReportPrintTool printTool = new ReportPrintTool(_MainReport)) {
            printTool.Print();
         }
      }

      public void Export(FileType fileType, string path)
      {
         AddReportTitleForNight();

         path += FileTypeToExtension(fileType);

         _controlfp(0x0008001F, 0xfffff);

         if (fileType == FileType.PDF) {
            _MainReport.CreateDocument();
            _MainReport.ExportToPdf(path);
         }
      }

      public void Create(XtraReport report)
      {
         SettingReport(report);
      }

      public void SetParams(string paramName, object paramValue)
      {
         if (_MainReport != null) {
            _MainReport.Parameters[paramName].Value = paramValue;
            _MainReport.Parameters[paramName].Visible = false;
         }
      }

      public void AddHeaderBottomInfo(string info)
      {
         ReportHeader header = (ReportHeader)((XRSubreport)_MainReport.FindControl("xrSubreportMain", true)).ReportSource;

         XRLabel lblInfo = new XRLabel();
         lblInfo.WidthF = ((XRSubreport)_MainReport.FindControl("xrSubreportMain", true)).WidthF;
         lblInfo.LocationF = new PointFloat(0, header.FindControl("Detail", true).HeightF);
         lblInfo.Font = new System.Drawing.Font("標楷體", 12F);
         lblInfo.Text = info;

         header.FindControl("Detail", true).Controls.Add(lblInfo);
      }

      /// <summary>
      /// 如果是夜盤的報表，標題後面都加(夜盤)兩個字
      /// </summary>
      private void AddReportTitleForNight()
      {
         if (SystemStatus.SystemType == SystemType.FutureNight || SystemStatus.SystemType == SystemType.OptionNight) {
            ReportTitle += "(夜盤)";
         }
      }

      private void SettingReport(XtraReport inputReport)
      {
         _MainReport = inputReport;

         if (PrintableComponent != null) {
            PrintableComponentContainer pcc = (PrintableComponentContainer)_MainReport.FindControl("printableComponentContainerMain", true);
            if (pcc != null) {
               pcc.PrintableComponent = PrintableComponent;
            }
         }

         XRLabel lblHandlingDescription = (XRLabel)_MainReport.FindControl("lblHandlingDescription", true);
        if (lblHandlingDescription!= null)
        {
            lblHandlingDescription.Visible = IsHandlePersonVisible;
        }

         XRLabel lblManagerDescription = (XRLabel)_MainReport.FindControl("lblManagerDescription", true);
        if (lblManagerDescription != null)
        {
            lblManagerDescription.Visible = IsManagerVisible;
        }

            Parameter pReportTitle = _MainReport.Parameters["ReportTitle"];
         if (pReportTitle != null) {
            pReportTitle.Value = ReportTitle;
            pReportTitle.Visible = false;
         }

         Parameter pUserName = _MainReport.Parameters["UserName"];
         if (pUserName != null) {
            pUserName.Value = GlobalInfo.USER_NAME;
            pUserName.Visible = false;
         }

         Parameter pOcfDate = _MainReport.Parameters["OcfDate"];
         if (pOcfDate != null) {
            pOcfDate.Value = "中華民國 " + (GlobalInfo.OCF_DATE.Year - 1911) + " 年 " + GlobalInfo.OCF_DATE.ToString("MM 月 dd 日 ");
            pOcfDate.Visible = false;
         }

         Parameter pReportID = _MainReport.Parameters["ReportID"];
         if (pReportID != null) {
            pReportID.Value = ReportID;
            pReportID.Visible = false;
         }

         Parameter pLeftMemo = _MainReport.Parameters["LeftMemo"];
         if (pLeftMemo != null) {
            pLeftMemo.Value = LeftMemo;
            pLeftMemo.Visible = false;
         }

         Parameter pFooterMemo = _MainReport.Parameters["FooterMemo"];
         if (pFooterMemo != null) {
            pFooterMemo.Value = FooterMemo;
            pFooterMemo.Visible = false;
         }

         _MainReport.DisplayName = ReportTitle;
         _MainReport.RequestParameters = false;
         _MainReport.Visible = true;

         _MainReport.ShowPrintStatusDialog = false;
      }

      private string FileTypeToExtension(FileType fileType)
      {
         string result = "";

         switch (fileType) {
            case FileType.PDF:
               result = ".pdf";
               break;

            case FileType.XLS:
               result = ".xls";
               break;

            case FileType.XLSX:
               result = ".xlsx";
               break;

            case FileType.TXT:
               result = ".txt";
               break;

            default:
               break;
         }

         return result;
      }
      /// <summary>
      /// 報表套用模板
      /// </summary>
      /// <param name="detailReport">報表</param>
      /// <param name="mainReport">模板報表</param>
      /// <returns></returns>
      public XtraReport CreateCompositeReport(XtraReport detailReport, XtraReport mainReport)
      {
         // Create a subreport. 
         XRSubreport subreport = new XRSubreport();
        mainReport.Margins= detailReport.Margins;
         // Create a detail band and add it to the main report. 

         DetailBand detailBand = (DetailBand)mainReport.Bands[BandKind.Detail];
         mainReport.Bands.Add(detailBand);

         // Set the subreport's location. 
         subreport.Location = new System.Drawing.Point(mainReport.FindControl("PageHeader",true).Width, mainReport.FindControl("PageHeader", true).Height);

         // 設定報表
         subreport.ReportSource = detailReport;

         // Add the subreport to the detail band. 
         detailBand.Controls.Add(subreport);
         return mainReport;
      }
      /// <summary>
      /// 報表細部屬性設定
      /// </summary>
      public class ReportProp
      {
         public string dataColumn { get; set; }
         public string caption { get; set; }
         public ReportExprssion expression { get; set; }
         /// <summary>
         /// 按照pb datawindow設定的width即可
         /// </summary>
         public int cellWidth { get; set; }
      }
      /// <summary>
      /// 報表過濾條件設定
      /// </summary>
      public class ReportExprssion
      {
         /// <summary>
         /// ex:"[UnitPrice] >= 30"
         /// </summary>
         public string Condition { get; set; }
         public System.Drawing.Color BackColor { get; set; }
         public System.Drawing.Color ForeColor { get; set; }
      }
   }
}