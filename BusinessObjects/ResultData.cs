using BusinessObjects.Enums;
using System.Data;
using System.Data.Common;

namespace BusinessObjects {
   public class ResultData {
      public DbTransaction DbTransaction;

      public ResultStatus Status { get; set; }

      public string returnString { get; set; }

      public object ReturnObject { get; set; }

      public DataTable ReturnData { get; set; }

      public DataTable ChangedDataTable { get; set; } = new DataTable();

      public DataView ChangedDataView { get; set; } = new DataView();


      public DataView ChangedDataViewForAdded { get; set; } = new DataView();

      public DataView ChangedDataViewForModified { get; set; } = new DataView();

      public DataTable ChangedDataViewForDeleted { get; set; } = new DataTable();

   }
}
