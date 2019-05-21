﻿using System;
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
using System.Linq;

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

         int colwidthSum = caption.Select(x => x.CellWidth).Sum();

         int colCount = caption.Count;
         int pagewidth = (PageWidth - (Margins.Left + Margins.Right));
         int colWidth = pagewidth / colCount;
         PageHeadertable = new XRTable();
         XRTableRow PageHeaderRow = new XRTableRow();
         Detailtable = new XRTable();
         XRTableRow DetailRow = new XRTableRow();
         foreach (var col in caption) {
            XRTableCell PageHeaderCel = new XRTableCell();
            int cellwidth = col?.CellWidth == null ? colWidth.AsInt() : col.CellWidth;
            PageHeaderCel.Width = cellwidth;

            PageHeaderCel.TextAlignment = TextAlignment.BottomCenter;
            PageHeaderCel.WordWrap = true;
            //PageHeaderCel.RowSpan = 2;
            PageHeaderCel.Text = col.Caption;
            PageHeaderCel.Font = new Font(Font.FontFamily, col.HeaderFontSize, Font.Style, Font.Unit);
            PageHeaderRow.Cells.Add(PageHeaderCel);
            XRTableCell DetailCel = new XRTableCell();
            DetailCel.Width = cellwidth;
            DetailCel.Font = new Font(Font.FontFamily, col.DetailRowFontSize, Font.Style, Font.Unit);
            DetailCel.TextAlignment = col.textAlignment == 0 ? TextAlignment.MiddleJustify : col.textAlignment;
            DetailCel.TextFormatString = col.TextFormatString;
            DetailCel.WordWrap = false;
            DetailCel.DataBindings.Add("Text", null, col.DataColumn.ToString());
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
         PageHeadertable.Width = colwidthSum;
         PageHeadertable.Borders = BorderSide.Bottom;
         Detailtable.Rows.Add(DetailRow);

         Detailtable.Width = colwidthSum;

         Bands[BandKind.PageHeader].Controls.Add(PageHeadertable);
         Bands[BandKind.Detail].Controls.Add(Detailtable);
      }

      public void SetMemoInPageFooter(string memo)
      {
         XRTable xrTableFooter = new XRTable();
         XRTableRow xrTableRowMemo = new XRTableRow();
         XRTableCell xrTableCellMemo = new XRTableCell();
         xrTableCellMemo.Text = memo;
         xrTableRowMemo.Cells.Add(xrTableCellMemo);
         xrTableFooter.Rows.Add(xrTableRowMemo);
         Bands[BandKind.PageFooter].Controls.Add(xrTableFooter);
      }
   }
}
