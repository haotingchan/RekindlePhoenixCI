using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace PhoenixCI.Report
{
   public partial class RW50030 : DevExpress.XtraReports.UI.XtraReport
   {
      private int detailRowCount = 0;
      /// <summary>
      /// 50030 report
      /// </summary>
      /// <param name="hasSum">是否加總</param>
      public RW50030(bool hasSum = true)
      {
         InitializeComponent();
         if (!hasSum) {
            this.xrTableRow4.Cells.Remove(this.cp_prod2);
            this.xrTableRow1.Cells.Remove(this.cp_prod_id2);
         }
         else {
            this.xrTableRow4.Cells.Remove(this.cp_prod1);
            this.xrTableRow1.Cells.Remove(this.cp_prod_id);
         }
      }

      private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
      {
         //if (++detailRowCount > 5)
         //   e.Cancel = true;
         detailRowCount += 1;
         if (detailRowCount == 50) {
            Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            detailRowCount = 0;
         }
         else {
            Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.None;
         }
      }

      private void groupHeaderBand1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
      {
         //detailRowCount = 0;
      }
   }
}
