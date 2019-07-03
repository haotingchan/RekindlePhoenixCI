using System.Data;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix2
{
   /// <summary>
   /// 部位限制個股類標的轉入
   /// </summary>
   public class B20231
   {
      /// <summary>
      /// 20231 Import匯入資料時 讀取txt轉為DataTable
      /// </summary>
      /// <param name="openFile">檔案Stream</param>
      /// <param name="dtReadTxt">要轉出的DataTable結構</param>
      /// <returns></returns>
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
