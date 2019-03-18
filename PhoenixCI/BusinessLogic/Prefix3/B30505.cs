using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 20190313,john,最佳1檔加權平均委託買賣價差統計表(週) 
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 最佳1檔加權平均委託買賣價差統計表(週)
   /// </summary>
   public class B30505
   {
      private string lsFile;
      private string startDateText;
      private string endDateText;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath"></param>
      /// <param name="StartDate"></param>
      /// <param name="EndDate"></param>
      public B30505(string FilePath,string StartDate,string EndDate)
      {
         lsFile = FilePath;
         startDateText = StartDate;
         endDateText = EndDate;
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
         try {
            Workbook wb = new Workbook();
            wb.Options.Export.Csv.WritePreamble = true;
            wb.Worksheets[0].Import(dataTable, true, 1, 0);
            wb.Worksheets[0].Name = SheetName(lsFile);
            //string lsStr = lsTab + lsRptId + lsRptName;
            wb.Worksheets[0].Cells["B1"].Value="30505股票期貨最近月份契約買賣價差週資料統計表(單位：tick)";
            //存檔
            wb.SaveDocument(lsFile, DocumentFormat.Csv);
         }
         catch (Exception ex) {
            throw ex;
         }
      }

      private string SheetName(string filePath)
      {
         string filename = Path.GetFileNameWithoutExtension(filePath);
         int nameLen = filename.Length > 31 ? 31 : filename.Length;//sheetName不能超過31字
         return filename.Substring(0, nameLen);
      }
      /// <summary>
      /// wf_30505()
      /// </summary>
      /// <returns></returns>
      public string Wf30505()
      {
         string flowStepDesc = "開始轉出資料";
         //產生檔案
         CreateCsvFile(lsFile);
         try {
            string lsRptName = "股票期貨最近月份契約買賣價差週資料統計表(單位：tick)";
            string lsRptId = "30505";
            DateTime startDate = startDateText.AsDateTime();
            DateTime endDate = endDateText.AsDateTime();
            //讀取資料
            flowStepDesc = "讀取資料";
            DataTable AI2dt = PbFunc.f_week(startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
            if (AI2dt.Rows.Count <= 0) {
               return $"{DateTime.Now.ToShortDateString()},30505－年月,無任何資料!";
            }
            DataTable dt = new D30505().GetData(startDate, endDate);
            if (dt.Rows.Count <= 0) {
               return $"{DateTime.Now.ToShortDateString()},{lsRptId}－{lsRptName}無任何資料!";
            }
            /*統計表*/
            DataTable workTable = new DataTable();
            //表頭
            flowStepDesc = "表頭";

            
            //lsStr = "排序" + lsTab + "商品代碼" + lsTab + "商品名稱";
            workTable.Columns.Add("排序", typeof(string));
            workTable.Columns.Add("商品代碼", typeof(string));
            workTable.Columns.Add("商品名稱", typeof(string));
            foreach (DataRow row in AI2dt.Rows) {
               string dateColStr=$"{row["startDate"].AsDateTime().ToString("yyyy/MM/dd")}~{row["endDate"].AsDateTime().ToString("yyyy/MM/dd")}";
               workTable.Columns.Add(dateColStr, typeof(string));
            }

            //內容
            flowStepDesc = "寫入內容";

            int seqNO = 0;
            for (int k=0; k<dt.Rows.Count;k++) {
               seqNO = seqNO + 1;
               string lskindID = dt.Rows[k]["SPRD1_KIND_ID"].AsString();
               DataRow workRow = workTable.NewRow();
               workRow["排序"] = seqNO.AsString();
               workRow["商品代碼"] = lskindID;
               workRow["商品名稱"] = dt.Rows[k]["PDK_NAME"].AsString();
               
               int indexCount = 3;
               foreach (DataRow AI2row in AI2dt.Rows) {
                  DataTable newdt = dt.Filter($"SPRD1_KIND_ID = '{lskindID}' and SPRD1_YMD >= '{AI2row["startDate"].AsDateTime().ToString("yyyyMMdd")}' and SPRD1_YMD <= '{AI2row["endDate"].AsDateTime().ToString("yyyyMMdd")}'");
                  int newdtCount = newdt.Rows.Count;
                  if (newdtCount > 0) {
                     
                     decimal diffSUM = newdt.Compute("Sum(diff)", "").AsDecimal(); //.AsEnumerable().Select(q => q.Field<decimal>("diff")).Sum();
                     workRow[indexCount++] = Math.Round(diffSUM / newdtCount, 2, MidpointRounding.AwayFromZero);
                     k = k + newdt.AsEnumerable().Select(q => q.Field<string>("SPRD1_KIND_ID")).Count();
                  }
                  else {
                     workRow[indexCount++] = "0";
                  }
               }//foreach (DataRow AI2row in AI2dt.Rows)
               k=k-1;
               workTable.Rows.Add(workRow);
            }//for (int k=0; k<dt.Rows.Count;k++)

            //轉出Csv
            flowStepDesc = "轉出Csv";
            SaveExcel(workTable);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"wf_30505-{flowStepDesc}:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

   }
}
