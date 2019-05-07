using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D500xx
   {
      public bool IsOpen { get; set; }
      public string IsCheck { get; set; }
      public string Sbrkno { get; set; }
      public string Ebrkno { get; set; }
      public string ProdKindIdSto { get; set; }
      public string ProdKindId { get; set; }
      public string ProdCategory { get; set; }
      public string Sdate { get; set; }
      public string Edate { get; set; }
      public string SumType { get; set; }
      public string SumSubType { get; set; }
      public string DataType { get; set; }
      public string SortType { get; set; }
      public string TableName { get; set; }
      public string LogText { get; set; }
      public string TxnID { get; set; }
      public string TimeNow { get; set; }
      public string Filename { get; set; }
      public DataTable Data { get; set; }
   }
}
