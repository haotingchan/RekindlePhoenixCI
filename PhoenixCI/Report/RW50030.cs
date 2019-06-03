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
               this.cp_prod2.WidthF = 0f;
               this.cp_prod_id2.WidthF = 0f;

               //期貨商代號
               this.xrTableCell17.WidthF = 62.86f;
               this.amm0_brk_no.WidthF = 62.86f;
               //期貨商名稱
               this.xrTableCell18.WidthF = 115f;
               this.brk_abbr_name.WidthF = 115f;
               //投資人帳號
               this.xrTableCell19.WidthF = 53f;
               this.amm0_acc_no.WidthF = 53f;
               //商品名稱
               this.cp_prod1.WidthF = 80f;
               this.cp_prod_id.WidthF = 80f;
               //一般委託成交量
               this.xrTableCell21.WidthF = 59.48f;
               this.amm0_om_qnty.WidthF = 59.48f;
               //列印按"商品"才有小計 其他時候隱藏
               xrTableRow2.HeightF = 0;
               xrTableRow2.Visible = false;
               xrTableRow3.HeightF = 0;
               xrTableRow3.Visible = false;
               groupFooterBand1.HeightF = 0;
               groupFooterBand1.Visible = false;

               break;
            case "P":
               this.cp_prod1.WidthF = 0f;
               this.cp_prod_id.WidthF = 0f;
               
               //期貨商代號
               this.xrTableCell17.WidthF = 62.86f;
               this.amm0_brk_no.WidthF = 62.86f;
               //期貨商名稱
               this.xrTableCell18.WidthF = 115f;
               this.brk_abbr_name.WidthF = 115f;
               //投資人帳號
               this.xrTableCell19.WidthF = 53f;
               this.amm0_acc_no.WidthF = 53f;
               //一般委託成交量
               this.xrTableCell21.WidthF = 59.48f;
               this.amm0_om_qnty.WidthF = 59.48f;
               //商品名稱
               this.cp_prod2.WidthF = 80f;
               this.cp_prod_id2.WidthF = 80f;
               break;
            default:
               break;
         }
      }

   }
}
