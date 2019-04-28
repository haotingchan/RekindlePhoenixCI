using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Linq;
/// <summary>
/// 20190402,john,最佳1檔加權平均委託買賣價差統計表(月) 
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 最佳1檔加權平均委託買賣價差統計表(月) 
   /// </summary>
   public class B30503
   {
      private readonly string _lsFile;
      private string _startYmDateText;
      private string _endYmDateText;
      private readonly string _GlobalDefaultPath;
      private D30503 dao30503;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="DEFAULT_REPORT_DIRECTORY_PATH"></param>
      /// <param name="FilePath"></param>
      /// <param name="StartDate"></param>
      /// <param name="EndDate"></param>
      public B30503(string DEFAULT_REPORT_DIRECTORY_PATH, string FilePath,string StartDate,string EndDate)
      {
         _GlobalDefaultPath = DEFAULT_REPORT_DIRECTORY_PATH;
         _lsFile = FilePath;
         _startYmDateText = StartDate;
         _endYmDateText = EndDate;
         dao30503 =new D30503();
      }

      private string CreateCsvFile(string saveFilePath)
      {
         //避免重複寫入
         try {
            if (File.Exists(saveFilePath)) {
               File.Delete(saveFilePath);
            }
            File.Create(saveFilePath).Close();
         }
         catch (Exception ex) {
            throw ex;
         }
         return saveFilePath;
      }
      /// <summary>
      /// 存Csv
      /// </summary>
      /// <param name="dataTable">要輸出的資料</param>
      private void SaveExcel(DataTable dataTable)
      {
         //產生檔案
         string saveFilePath = CreateCsvFile(Path.Combine(_GlobalDefaultPath, $@"30504_{DateTime.Now.ToString("yyyy.MM.dd")}-{DateTime.Now.ToString("HH.mm.ss")}.csv"));
         try {
            Workbook wb = new Workbook();
            wb.Options.Export.Csv.WritePreamble = true;
            wb.Worksheets[0].Import(dataTable, true, 1, 0);
            wb.Worksheets[0].Name = SheetName(saveFilePath);
            //string lsStr = lsTab + lsRptId + lsRptName;
            wb.Worksheets[0].Cells["B1"].Value= $"30504股票期貨最近月份契約買賣價差日資料";
            //存檔
            wb.SaveDocument(saveFilePath, DocumentFormat.Csv);
         }
         catch (Exception ex) {
            File.Delete(saveFilePath);
            throw ex;
         }
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="AI2dt"></param>
      /// <param name="dt"></param>
      /// <param name="val">要寫入的欄位</param>
      /// <returns></returns>
      private static DataTable DtContextData(DataTable AI2dt, DataTable dt,string val)
      {
         /*統計表*/
         DataTable workTable = new DataTable();
         //lsStr = "排序" + lsTab + "商品代碼" + lsTab + "商品名稱";
         workTable.Columns.Add("排序", typeof(string));
         workTable.Columns.Add("商品代碼", typeof(string));
         workTable.Columns.Add("商品名稱", typeof(string));
         foreach (DataRow row in AI2dt.Rows) {
            string dateColStr = row["AI2_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
            workTable.Columns.Add(dateColStr, typeof(string));
         }

         int seqNO = 0;
         for (int k = 0; k < dt.Rows.Count; k++) {
            seqNO = seqNO + 1;
            DataRow dr = dt.Rows[k];
            string lskindID = dr["SPRD1_KIND_ID"].AsString();
            DataRow workRow = workTable.NewRow();
            workRow["排序"] = seqNO.AsString();
            workRow["商品代碼"] = lskindID;
            workRow["商品名稱"] = dr["PDK_NAME"].AsString();

            int indexCount = 3;
            foreach (DataRow AI2row in AI2dt.Rows) {
               int foundIndex = dt.Rows.IndexOf(dt.Select($"SPRD1_KIND_ID = '{lskindID}' and SPRD1_YMD = '{AI2row["AI2_YMD"].AsString()}'").FirstOrDefault());
               if (foundIndex > -1) {
                  workRow[indexCount++] = dt.Rows[foundIndex][val];
               }
               else {
                  workRow[indexCount++] = "0";
               }
            }//foreach (DataRow AI2row in AI2dt.Rows)
            workTable.Rows.Add(workRow);
            k = k + dt.Select($"SPRD1_KIND_ID='{lskindID}'").Count()-1;
         }//for (int k=0; k<dt.Rows.Count;k++)

         return workTable;
      }//private static DataTable DtData(DataTable AI2dt, DataTable dt)

      private string SheetName(string filePath)
      {
         string filename = Path.GetFileNameWithoutExtension(filePath);
         int nameLen = filename.Length > 31 ? 31 : filename.Length;//sheetName不能超過31字
         return filename.Substring(0, nameLen);
      }

      /// <summary>
      /// wf_30503()
      /// </summary>
      /// <returns></returns>
      public string WF30503()
      {
         Workbook workbook = new Workbook();
         try {
            string lsRptName = "股票期貨最近月份契約買賣價差月資料統計表";
            string lsRptId = "30503";
            DateTime startDate = _startYmDateText.AsDateTime();
            DateTime endDate = _endYmDateText.AsDateTime();

            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[lsRptId];

            //讀取資料
            DataTable AI2dt = dao30503.ListYMD(startDate.ToString("yyyyMM"), endDate.ToString("yyyyMM"), "M", "F"); ;
            if (AI2dt.Rows.Count <= 0) {
               return $"{DateTime.Now.ToShortDateString()},{lsRptName}－年月,無任何資料!";
            }
            DataTable dt = dao30503.GetData(startDate.ToString("yyyyMM01"), endDate.ToString("yyyyMM31"));
            if (dt.Rows.Count <= 0) {
               return $"{startDate.ToShortDateString()}~{endDate.ToShortDateString()},{lsRptId}－{lsRptName}無任何資料!";
            }

            //表頭
            for (int k=0;k<AI2dt.Rows.Count;k++) {
               DataRow dr = AI2dt.Rows[k];
               DateTime ai2YMD = dr["AI2_YMD"].AsDateTime("yyyyMM");
               worksheet.Rows[2 - 1][k + 3 ].Value = $"{ai2YMD.Year}年{ai2YMD.Month}月";
            }

            int rowIndex = 2-1;
            int seqNO = 0;
            string lskindID="";
            foreach (DataRow dr in dt.Rows) {
               if (lskindID!= dr["SPRD1_KIND_ID"].AsString()) {
                  lskindID = dr["SPRD1_KIND_ID"].AsString();
                  rowIndex = rowIndex + 1;
                  seqNO = seqNO + 1;
                  worksheet.Rows[rowIndex][1 - 1].SetValue(seqNO);
                  worksheet.Rows[rowIndex][2 - 1].SetValue(lskindID);
                  worksheet.Rows[rowIndex][3 - 1].SetValue(dr["PDK_NAME"].AsString());
               }
               int foundIndex = AI2dt.Rows.IndexOf(AI2dt.Select($"AI2_YMD = '{dr["SPRD1_YMD"].AsString()}'").FirstOrDefault());
               if (foundIndex > -1) {
                  foundIndex = foundIndex + 3;
                  worksheet.Rows[rowIndex][foundIndex].SetValue(dr["DIFF"]);
               }
            }//foreach (DataRow dr in dt.Rows)

            
            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("WF30503:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);//存檔
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30504()
      /// </summary>
      /// <returns></returns>
      public string WF30504()
      {
         try {
            string lsRptName = "股票期貨最近月份契約買賣價差日資料";
            string lsRptId = "30504";
            DateTime startDate = _startYmDateText.AsDateTime();
            DateTime endDate = _endYmDateText.AsDateTime();
            //讀取資料
            DataTable AI2dt = dao30503.ListYMD(startDate.ToString("yyyyMM01"), endDate.ToString("yyyyMM31"),"D","F"); ;
            if (AI2dt.Rows.Count <= 0) {
               return $"{DateTime.Now.ToShortDateString()},{lsRptName}－年月,無任何資料!";
            }
            DataTable dt = dao30503.List30504(startDate.ToString("yyyyMM01"), endDate.ToString("yyyyMM31"));
            if (dt.Rows.Count <= 0) {
               return $"{startDate.ToShortDateString()}~{endDate.ToShortDateString()},{lsRptId}－{lsRptName}無任何資料!";
            }
            //內容
            DataTable diffTable = DtContextData(AI2dt, dt, "DIFF");
            //轉出Csv
            SaveExcel(diffTable);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"wf_30504:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }


   }
}
