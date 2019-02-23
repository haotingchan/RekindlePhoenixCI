using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
using System.Collections.Generic;
using Common;

namespace BaseGround.Report
{
   public partial class defReport : DevExpress.XtraReports.UI.XtraReport
   {
      public defReport()
      {
         InitializeComponent();
      }
      public XRTable PageHeadertable { get; set; }
      public XRTable Detailtable { get; set; }
      public defReport(DataTable gridData, List<ReportHelper.ReportProp> caption)
      {
         InitializeComponent();
         this.DataSource = gridData;
         this.DataMember = gridData.TableName;
         
         int colCount = caption.Count;
         int pagewidth = (PageWidth - (Margins.Left + Margins.Right));
         int colWidth = pagewidth / colCount;
         PageHeadertable = new XRTable();
         XRTableRow PageHeaderRow = new XRTableRow();
         Detailtable = new XRTable();
         XRTableRow DetailRow = new XRTableRow();
         foreach (var col in caption) {
            XRTableCell PageHeaderCel = new XRTableCell();
            int colLen = ExtensionCommon.GetMaxColLen(gridData, col.dataColumn.ToString());
            PageHeaderCel.Width = string.IsNullOrEmpty(col.cellWidth.AsString())? colWidth.AsInt(): col.cellWidth;
            //PageHeaderCel.Width = colLen < 7 && col.dataColumn.Length > 7 ? 7 : colLen;
            PageHeaderCel.Text = col.caption.ToString();
            PageHeaderCel.TextAlignment = TextAlignment.BottomLeft;
            PageHeaderCel.WordWrap = true;
            PageHeaderRow.Cells.Add(PageHeaderCel);
            XRTableCell DetailCel = new XRTableCell();
            DetailCel.Width = string.IsNullOrEmpty(col.cellWidth.AsString()) ? colWidth.AsInt() : col.cellWidth;
            //DetailCel.Width = colLen < 7&& col.dataColumn.Length>7? 7 : colLen;
            DetailCel.DataBindings.Add("Text", null, col.dataColumn.ToString());
            //////// Create a new rule and add it to a report. 
            //////FormattingRule rule = new FormattingRule();
            //////this.FormattingRuleSheet.Add(rule);

            //////// Specify the rule's properties. 
            //////rule.DataSource = this.DataSource;
            //////rule.DataMember = this.DataMember;
            //////rule.Condition = "[UnitPrice] >= 30";
            //////rule.Formatting.BackColor = Color.WhiteSmoke;
            //////rule.Formatting.ForeColor = Color.IndianRed;

            //////// Apply this rule to the detail band. 
            //////DetailCel.FormattingRules.Add(rule);
            DetailRow.Cells.Add(DetailCel);
         }
         PageHeadertable.Rows.Add(PageHeaderRow);
         PageHeadertable.Width = pagewidth;
         PageHeadertable.Borders = BorderSide.Bottom;
         Detailtable.Rows.Add(DetailRow);


         Detailtable.Width = pagewidth;
         Bands[BandKind.PageHeader].Controls.Add(PageHeadertable);
         Bands[BandKind.Detail].Controls.Add(Detailtable);
      }
   }
}
