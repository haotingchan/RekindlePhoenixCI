﻿using System.Drawing;
using DevExpress.XtraReports.UI;
using System.Data;
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
         //計算整體報表寬度
         int colwidthSum = caption.Select(x => x.CellWidth).Sum();
         //欄位總數
         int colCount = caption.Count;
         int pagewidth = PageWidth - (Margins.Left + Margins.Right);
         //無設置寬度每個欄位寬度平均
         int colWidth = pagewidth / colCount;
         PageHeadertable = new XRTable();
         XRTableRow PageHeaderRow = new XRTableRow();
         Detailtable = new XRTable();
         XRTableRow DetailRow = new XRTableRow();
         foreach (var col in caption) {
            //報表標頭
            XRTableCell PageHeaderCel = new XRTableCell();
            int cellwidth = col?.CellWidth == null ? colWidth.AsInt() : col.CellWidth;
            PageHeaderCel.Width = cellwidth;

            PageHeaderCel.TextAlignment = TextAlignment.BottomCenter;
            PageHeaderCel.WordWrap = true;
            //PageHeaderCel.RowSpan = 2;
            PageHeaderCel.Text = col.Caption;
            PageHeaderCel.Font = new Font(Font.FontFamily, col.HeaderFontSize, Font.Style, Font.Unit);
            PageHeaderRow.Cells.Add(PageHeaderCel);

            //報表內容
            XRTableCell DetailCel = new XRTableCell();
            DetailCel.Width = cellwidth;
            DetailCel.Font = new Font(Font.FontFamily, col.DetailRowFontSize, Font.Style, Font.Unit);
            DetailCel.TextAlignment = col.textAlignment == 0 ? TextAlignment.MiddleJustify : col.textAlignment;
            DetailCel.TextFormatString = col.TextFormatString;
            DetailCel.TextTrimming = StringTrimming.Word;
            DetailCel.WordWrap = false;
            DetailCel.DataBindings.Add("Text", null, col.DataColumn.ToString());
            if (col.DataRowMerge) {
               DetailCel.ProcessDuplicatesMode = ProcessDuplicatesMode.SuppressAndShrink;
               DetailCel.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;
            }

            if (col.Expression != null) {
               DetailCel.ExpressionBindings.AddRange(col.Expression);
            }

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
      /// <summary>
      /// 設置報表底下說明
      /// </summary>
      /// <param name="memo"></param>
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
