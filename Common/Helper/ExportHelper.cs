using System.Data;
using System.IO;
using System.Text;

namespace Common.Helper {
   public static class ExportHelper {
      public static void ToText(DataTable dt , Stream stream , ExportOptions exportOptions) {
         StreamWriter str = new StreamWriter(stream , exportOptions.Encoding);

         if (exportOptions.HasHeader) {
            string Columns = string.Empty;

            foreach (DataColumn column in dt.Columns) {
               Columns += (column.Caption == null ? column.ColumnName : column.Caption) + exportOptions.Separator;
            }

            str.WriteLine(Columns.Remove(Columns.Length - 1 , 1));
         }

         foreach (DataRow datarow in dt.Rows) {
            string row = string.Empty;

            foreach (object items in datarow.ItemArray) {
               row += items.ToString().Trim() + exportOptions.Separator;
            }

            str.WriteLine(row.Remove(row.Length - 1 , 1));
         }

         str.Flush();
         str.Close();
      }

      public static void ToText(DataTable dt , Stream stream) {
         ExportOptions exportOptions = new ExportOptions();

         ToText(dt , stream , exportOptions);
      }

      public static void ToText(DataTable dt , string fileName) {
         FileStream fs = new FileStream(fileName , FileMode.Create);

         ExportOptions exportOptions = new ExportOptions();

         ToText(dt , fs , exportOptions);
      }

      public static void ToText(DataTable dt , string fileName , ExportOptions exportOptions) {
         FileStream fs = new FileStream(fileName , FileMode.Create);

         ToText(dt , fs , exportOptions);
      }

      public static void ToCsv(DataTable dt , string fileName , ExportOptions exportOptions) {
         exportOptions.Separator = ",";

         ToText(dt , fileName , exportOptions);
      }

      public static void ToCsv(DataTable dt , Stream stream , ExportOptions exportOptions) {
         exportOptions.Separator = ",";

         ToText(dt , stream , exportOptions);
      }
   }
}

public class ExportOptions {
   private bool _HasHeader = false;

   private string _Separator = "\t";

   private Encoding _Encoding = Encoding.Default;


   public bool HasHeader {
      get { return _HasHeader; }
      set { _HasHeader = value; }
   }

   public string Separator {
      get { return _Separator; }
      set { _Separator = value; }
   }

   public Encoding Encoding {
      get { return _Encoding; }
      set { _Encoding = value; }
   }

}