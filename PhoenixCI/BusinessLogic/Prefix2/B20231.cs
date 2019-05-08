using System.Data;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix2
{
   /// <summary>
   /// 部位限制個股類標的轉入
   /// </summary>
   public class B20231
   {
      public DataTable TxtWriteToDataTable(Stream openFile, DataTable dtReadTxt)
      {
         using (TextReader tr = new StreamReader(openFile)) {
            string line;
            int rowIndex = 0;
            while ((line = tr.ReadLine()) != null) {
               dtReadTxt.Rows.Add(dtReadTxt.NewRow());
               string[] strCols= line.Split('\t');
               for (int colIndex = 0; colIndex < strCols.Length; colIndex++) {
                  dtReadTxt.Rows[rowIndex][colIndex] = strCols[colIndex];
               }
               rowIndex++;
            }//while ((line = tr.ReadLine()) != null)
         }//using (TextReader tr = new StreamReader(openFile))
         dtReadTxt.AcceptChanges();
         return dtReadTxt;
      }
   }
}
