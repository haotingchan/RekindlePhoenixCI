using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
   public static class TestHepler
   {
      /// <summary>
      /// 將CSV文件的數據讀取到DataTable中
      /// </summary>
      /// <param name="fileName">CSV文件路徑</param>
      /// <returns>返回讀取了CSV數據的DataTable</returns>
      public static DataTable OpenCSV(string filePath)
      {
         DataTable dt = new DataTable();
         FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
         StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(950));
         //StreamReader sr = new StreamReader(fs, encoding);
         //string fileContent = sr.ReadToEnd();
         //記錄每次讀取的一行記錄
         string strLine = "";
         //記錄每行記錄中的各字段內容
         string[] aryLine = null;
         string[] tableHead = null;
         //存放所有行的集合
         List<string> lines = new List<string>();
         //標示列數
         int columnCount = 0;
         //逐行讀取CSV中的數據
         while ((strLine = sr.ReadLine()) != null) {
            lines.Add(strLine);
         }
         
         //創建DT欄位列
         if (lines.Count > 0) {
            tableHead = lines[0].Split(',');
            columnCount = tableHead.Length;
            //創建列
            for (int i = 0; i < columnCount; i++) {
               //tableHead[i] = tableHead[i].Replace("\"", "");
               tableHead[i] = "Column" + i;
               DataColumn dc = new DataColumn(tableHead[i]);
               dt.Columns.Add(dc);
            }
         }
         else {
            return new DataTable();
         }
         //內容
         foreach (var str in lines) {
            aryLine = str.Split(',');
            int aryLinecolumnCount = aryLine.Length;
            DataRow dr = dt.NewRow();
            for (int j = 0; j < aryLinecolumnCount; j++) {
               dr[j] = aryLine[j].Replace("\"", "");
            }
            dt.Rows.Add(dr);
         }
         //if (aryLine != null && aryLine.Length > 0) {
         //   dt.DefaultView.Sort = tableHead[2] + " " + "DESC";
         //}
         sr.Close();
         fs.Close();
         return dt;
      }
      /// <summary>
      /// 比對CI的csv是否和PB的csv一致
      /// </summary>
      /// <param name="PbFile">PB csv路徑</param>
      /// <param name="CiFile">CI csv路徑</param>
      /// <returns></returns>
      public static bool CsvEqual(string PbFile,string CiFile)
      {
         DataTable DataByPB=OpenCSV(PbFile);
         DataTable DataByCI= OpenCSV(CiFile);
         //bool isEqual=DataByPB.Equals(DataByCI);
         bool isEqual = DataTableTheSame(DataByPB, DataByCI);
         return isEqual;
      }
      /// <summary>
      /// 清除原有的檔案設定檔案路徑
      /// </summary>
      /// <param name="ciFilename">原檔案路徑</param>
      public static void checkFile(string ciFilename)
      {
         if (File.Exists(ciFilename)) {
            File.Delete(ciFilename);
         }
         else {
            File.Create(ciFilename).Close();
         }
      }

      public static bool DataTableTheSame(DataTable Table1, DataTable Table2)
      {
         if (Table1 == null || Table2 == null) {
            return false;
         }
         if (Table1.Rows.Count != Table2.Rows.Count) {
            return false;
         }
         if (Table1.Columns.Count != Table2.Columns.Count) {
            return false;
         }
         for (int i = 0; i < Table1.Rows.Count; i++) {
            for (int j = 0; j < Table1.Columns.Count; j++) {
               if (Table1.Rows[i][j].ToString().Trim() != Table2.Rows[i][j].ToString().Trim()) {
                  return false;
               }
            }
         }
         return true;
      }
   }
}
