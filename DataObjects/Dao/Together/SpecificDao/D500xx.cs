using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D500xx
   {
      public bool IsOpen { get; set; }
      public string IsCheck { get; set; }
      public string Sbrkno { get; set; } = "";
      public string Ebrkno { get; set; } = "";
      public string ProdKindIdSto { get; set; } = "";
      public string ProdKindId { get; set; } = "";
      public string ProdCategory { get; set; } = "";
      public string Sdate { get; set; } = "";
      public string Edate { get; set; } = "";
      public string SumType { get; set; } = "";
      public string SumSubType { get; set; } = "";
      public string DataType { get; set; } = "";
      public string SortType { get; set; } = "";
      public string TableName { get; set; } = "AMM0";
      public string LogText { get; set; } = "";
      public string TxnID { get; set; } = "";
      public string TimeNow { get; set; } = "";
      public string Filename { get; set; } = "";
      public string GbGroup { get; set; } 

      public string ConditionWhereSyntax()
      {
         StringBuilder iswhere = new StringBuilder("");
         /* 日期起迄 */
         if (!string.IsNullOrEmpty(Sdate)) {
            iswhere.Append($@" and {TableName}_YMD >= :Sdate ");
         }
         if (!string.IsNullOrEmpty(Edate)) {
            iswhere.Append($@" and {TableName}_YMD <= :Edate ");
         }
         /* 期貨商代號起迄 */
         if (!string.IsNullOrEmpty(Sbrkno)) {
            iswhere.Append($@" and {TableName}_BRK_NO >= :Sbrkno ");
         }
         if (!string.IsNullOrEmpty(Ebrkno)) {
            iswhere.Append($@" and {TableName}_BRK_NO <= :Ebrkno ");
         }

         /*******************
         Where條件
         *******************/
         if (string.IsNullOrEmpty(GbGroup)) {
            return iswhere.ToString();
         }
         /* 商品群組 */
         if (!GbGroup.Equals("rb_gall")) {
            if (!string.IsNullOrEmpty(ProdCategory)) {
               iswhere.Append($@" and {TableName}_PARAM_KEY = :ProdCategory ");
            }
            /* 個股商品 */
            if (!GbGroup.Equals("rb_gparam")) {
               if (!string.IsNullOrEmpty(ProdKindIdSto)) {
                  iswhere.Append($@" and {TableName}_KIND_ID2 = :ProdKindIdSto ");
               }
               /* 商品 */
               if (!GbGroup.Equals("rb_gkind2")) {
                  if (!string.IsNullOrEmpty(ProdKindId)) {
                     iswhere.Append($@" and {TableName}_KIND_ID = :ProdKindId ");
                  }
               }//rb_gkind2.checked = False
            }//rb_gparam.checked = False
         }//rb_gall.checked = False
         return iswhere.ToString();
      }

   }
}
