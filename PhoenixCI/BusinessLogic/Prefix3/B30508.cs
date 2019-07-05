using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
/// <summary>
/// 20190313,john,最佳1檔加權平均委託買賣數量統計表(週) 
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 最佳1檔加權平均委託買賣數量統計表(週) 
   /// </summary>
   public class B30508
   {
      /// <summary>
      /// 檔案輸出路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 日期 起始日期
      /// </summary>
      private string _startDateText;
      /// <summary>
      /// 日期 迄止日期
      /// </summary>
      private string _endDateText;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath"></param>
      /// <param name="StartDate"></param>
      /// <param name="EndDate"></param>
      public B30508(string FilePath, string StartDate, string EndDate)
      {
         _lsFile = FilePath;
         _startDateText = StartDate;
         _endDateText = EndDate;
      }
      /// <summary>
      /// 建立Csv檔案
      /// </summary>
      /// <param name="saveFilePath">輸出路徑</param>
      /// <returns></returns>
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
      /// 重複寫入文字並換行
      /// </summary>
      /// <param name="openData">檔案路徑</param>
      /// <param name="textToAdd">文字內容</param>
      private void WriteFile(string openData, string textToAdd)
      {
         using (FileStream fs = new FileStream(openData, FileMode.Append)) {
            using (StreamWriter writer = new StreamWriter(fs, Encoding.GetEncoding(950))) {
               writer.WriteLine(textToAdd);
            }
         }
      }

      /// <summary>
      /// 30508(賣)
      /// </summary>
      /// <param name="lsTab"></param>
      /// <param name="lsSellStr"></param>
      /// <param name="newdt"></param>
      /// <returns></returns>
      private static string WriteSellData(string lsTab, string lsSellStr, DataTable newdt)
      {
         //IF(sum(bst1_s_tot_sec for all)=0,0,round(sum(bst1_s_qnty_weight for all)/sum(bst1_s_tot_sec for all),2))
         //decimal sTotSecSUM = newdt.AsEnumerable().Select(q => q.Field<decimal>("BST1_S_TOT_SEC")).Sum();
         decimal sTotSecSUM = newdt.Compute("Sum(BST1_S_TOT_SEC)", "").AsDecimal();
         //decimal sQntyWeightSUM = newdt.AsEnumerable().Select(q => q.Field<decimal>("BST1_S_QNTY_WEIGHT")).Sum();
         decimal sQntyWeightSUM = newdt.Compute("Sum(BST1_S_QNTY_WEIGHT)", "").AsDecimal();
         lsSellStr = $"{lsSellStr}{lsTab}{(sTotSecSUM == 0 ? 0 : (sQntyWeightSUM / sTotSecSUM)).ToString("F2")}";//ls_str2 = ls_str2 + ls_tab + string(ids_1.getitemdecimal(1, "cp_s_qnty"));
         //ToString("F2")比Math.Round(sQntyWeightSUM / sTotSecSUM,2)還符合PB取的小數位
         return lsSellStr;
      }
      /// <summary>
      /// 30508(買)
      /// </summary>
      /// <param name="lsTab"></param>
      /// <param name="lsStr"></param>
      /// <param name="newdt"></param>
      /// <returns></returns>
      private static string WriteBuyData(string lsTab, string lsStr, DataTable newdt)
      {
         //IF(sum(bst1_b_tot_sec for all)=0,0,round(sum(bst1_b_qnty_weight for all)/sum(bst1_b_tot_sec for all),2))
         //decimal bTotSecSUM = newdt.Rows.Count<2? newdt.AsEnumerable().FirstOrDefault()["BST1_B_TOT_SEC"].AsDecimal():newdt.AsEnumerable().Select(q => q.Field<decimal>("BST1_B_TOT_SEC")).Sum();
         decimal bTotSecSUM = newdt.Compute("Sum(BST1_B_TOT_SEC)", "").AsDecimal();
         //decimal bQntyWeightSUM = newdt.Rows.Count < 2 ? newdt.AsEnumerable().FirstOrDefault()["BST1_B_QNTY_WEIGHT"].AsDecimal() : newdt.AsEnumerable().Select(q => q.Field<decimal>("BST1_B_QNTY_WEIGHT")).Sum();
         decimal bQntyWeightSUM = newdt.Compute("Sum(BST1_B_QNTY_WEIGHT)", "").AsDecimal();
         lsStr = $"{lsStr}{lsTab}{(bTotSecSUM == 0 ? 0 : (bQntyWeightSUM / bTotSecSUM)).ToString("F2")}";//ls_str = ls_str + ls_tab + string(ids_1.getitemdecimal(1, "cp_b_qnty"));
         //ToString("F2")比Math.Round(bQntyWeightSUM / bTotSecSUM,2)還符合PB取的小數位
         return lsStr;
      }

      /// <summary>
      /// wf_30508() 產出兩個Csv檔案
      /// </summary>
      /// <returns></returns>
      public string Wf30508()
      {
         try {
            string lsRptName = "股票期貨最近月份契約最佳1檔加權平均委託買進數量週資料統計表";
            string lsRptId = "30508";
            DateTime startDate = _startDateText.AsDateTime();
            DateTime endDate = _endDateText.AsDateTime();
            //讀取資料
            DataTable AI2dt = PbFunc.f_week(startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
            if (AI2dt.Rows.Count <= 0) {
               return $"{startDate.ToShortDateString()}～{endDate.ToShortDateString()},30508－年月,無任何資料!";
            }
            DataTable dt = new D30508().GetData(startDate, endDate);
            if (dt.Rows.Count <= 0) {
               return $"{startDate.ToShortDateString()}～{endDate.ToShortDateString()},{lsRptId}－{lsRptName}無任何資料!";
            }
            //產生30508(買)檔案
            CreateCsvFile(_lsFile);
            string lsSellFile = _lsFile.Replace("30508(買)", "30508(賣)");
            //產生30508(賣)檔案
            CreateCsvFile(lsSellFile);

            /*統計表*/
            string lsTab = ",";
            //表頭
            string lsStr = lsTab + lsRptId + lsRptName;
            WriteFile(_lsFile, lsStr);//30508(買)

            lsStr = lsTab + lsRptId + "股票期貨最近月份契約最佳1檔加權平均委託賣出數量週資料統計表";
            WriteFile(lsSellFile, lsStr);//30508(賣)
            lsStr = "排序" + lsTab + "商品代碼" + lsTab + "商品名稱";
            foreach (DataRow row in AI2dt.Rows) {
               lsStr = lsStr + lsTab + $"{row["startDate"].AsDateTime().ToString("yyyy/MM/dd")}～{row["endDate"].AsDateTime().ToString("yyyy/MM/dd")}";
            }
            //30508(買)
            WriteFile(_lsFile, lsStr);//FileWrite(li_FileNum, ls_str)
            //30508(賣)
            WriteFile(lsSellFile, lsStr);//FileWrite(li_FileNum2, ls_str)
            lsStr = string.Empty;

            int seqNO = 0;
            for (int k = 0; k < dt.Rows.Count; k++) {
               seqNO = seqNO + 1;
               string lskindID = dt.Rows[k]["BST1_KIND_ID"].AsString();
               lsStr = seqNO.AsString() + lsTab + lskindID + lsTab + dt.Rows[k]["PDK_NAME"].AsString();
               string lsSellStr = lsStr;
               foreach (DataRow AI2row in AI2dt.Rows) {
                  DataTable newdt = dt.Filter($"BST1_KIND_ID = '{lskindID}' and BST1_YMD >= '{AI2row["startDate"].AsDateTime().ToString("yyyyMMdd")}' and BST1_YMD <= '{AI2row["endDate"].AsDateTime().ToString("yyyyMMdd")}'");
                  if (newdt.Rows.Count > 0) {
                     //30508(買)
                     lsStr = WriteBuyData(lsTab, lsStr, newdt);
                     //30508(賣)
                     lsSellStr = WriteSellData(lsTab, lsSellStr, newdt);

                     k = k + newdt.AsEnumerable().Select(q => q.Field<string>("BST1_KIND_ID")).Count();
                  }
                  else {
                     //30508(買)
                     lsStr = lsStr + lsTab + "0";
                     //30508(賣)
                     lsSellStr = lsSellStr + lsTab + "0";
                  }
               }//foreach (DataRow AI2row in AI2dt.Rows)
               k = k - 1;
               //30508(買)
               WriteFile(_lsFile, lsStr);//FileWrite(li_FileNum, ls_str)
               //30508(賣)
               WriteFile(lsSellFile, lsSellStr);//FileWrite(li_FileNum2, ls_str2)
            }//for (int k = 0; k < dt.Rows.Count; k++)
         }
         catch (Exception ex) {
            throw ex;
         }
         return MessageDisplay.MSG_OK;
      }
      
   }
}
