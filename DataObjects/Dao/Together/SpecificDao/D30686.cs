using OnePiece;
using System;
using System.Data;

/// <summary>
/// Winni 2019/02/21
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30686 : DataGate {

      /// <summary>
      /// Get 加權指數日內值 by CI.VPR, CI.COD (VPR_YMD/VPR_SYMBOL/SYMBOL_NAME/VPR_DATA_TIME/VPR_VALUE/CLOSE_FLAG)
      /// </summary>
      /// <param name="as_symd">查詢起始日(yyyyMM01)</param>
      /// <param name="as_eymd">查詢結束日(yyyyMM31)</param>
      /// <returns></returns>
      public DataTable GetData(string as_symd , string as_eymd , char isPrint = 'N') {
         object[] parms =
         {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

         string sql = @"
SELECT VPR_YMD,VPR_SYMBOL,SYMBOL_NAME,VPR_DATA_TIME,VPR_VALUE,
       --是否為指數收盤價
       CASE WHEN VPR_SEQ = 99999999 THEN 'Y' ELSE '' END AS CLOSE_FLAG
  FROM ci.VPR,
      (SELECT COD_ID,TRIM(COD_DESC) AS SYMBOL_NAME FROM CI.COD 
        WHERE COD_TXN_ID = 'VPR' AND COD_COL_ID = 'VPR_SYMBOL')
WHERE VPR_YMD BETWEEN :as_symd AND :as_eymd
   AND VPR_UNDERLYING_MARKET IN ('1','2','F')
   AND VPR_SYMBOL = COD_ID(+)
ORDER BY VPR_YMD,VPR_SYMBOL,CLOSE_FLAG,VPR_DATA_TIME
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isPrint == 'Y') {
            string[] title = new string[] { "日期" , "指數" , "指數名稱" , "時間" , "值" , "是否為指數收盤價" };
            int k = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[k++];
            }
         }

         return dtResult;
      }

   }
}
