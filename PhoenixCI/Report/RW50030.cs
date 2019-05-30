using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace PhoenixCI.Report
{
   public partial class RW50030 : DevExpress.XtraReports.UI.XtraReport
   {
      /// <summary>
      /// 50030 report
      /// </summary>
      public RW50030()
      {
         InitializeComponent();
      }

      /// <summary>
      /// 列印按"商品"才有小計 值為P時
      /// </summary>
      /// <param name="sorttype">F or P</param>
      public void SetSortType(string sorttype)
      {
         switch (sorttype) {
            case "F":
               this.xrTableRow4.Cells.Remove(this.cp_prod2);
               this.xrTableRow1.Cells.Remove(this.cp_prod_id2);
               this.cp_prod1.WidthF = 68f;
               this.cp_prod_id.WidthF = 68f;
               //列印按"商品"才有小計 其他時候隱藏
               xrTableRow2.HeightF = 0;
               xrTableRow2.Visible = false;
               xrTableRow3.HeightF = 0;
               xrTableRow3.Visible = false;
               groupFooterBand1.HeightF = 0;
               groupFooterBand1.Visible = false;

               break;
            case "P":
               this.xrTableRow4.Cells.Remove(this.cp_prod1);
               this.xrTableRow1.Cells.Remove(this.cp_prod_id);
               this.cp_prod2.WidthF = 68f;
               this.cp_prod_id2.WidthF = 68f;
               break;
            default:
               break;
         }
      }

   }
}
