using System;
using System.Data;

/// <summary>
/// Winni, 2019/2/12
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class TPPINTD : DataGate {

      /// <summary>
      /// (for W30683)
      /// </summary>
      /// <param name="as_fm_date">yyyy/MM/dd</param>
      /// <param name="as_to_date">yyyy/MM/dd</param>
      /// <param name="as_mth_seq1"></param>
      /// <param name="as_mth_seq2"></param>
      /// <returns></returns>
      public DataTable d30683(DateTime as_fm_date , DateTime as_to_date , int as_mth_seq1 , int as_mth_seq2 , string isPrint = "N") {

         object[] parms =
         {
                ":as_fm_date", as_fm_date,
                ":as_to_date", as_to_date,
                ":as_mth_seq1", as_mth_seq1,
                ":as_mth_seq2", as_mth_seq2
            };

         string sql = @"
SELECT TPPINTD_MARKET_CODE,   
         TPPINTD_DATE,   
         TPPINTD_FIRST_KIND_ID,   
         TPPINTD_FIRST_MONTH,   
         TPPINTD_M_PRICE_LIMIT,   
         TPPINTD_M_INTERVAL,   
         TPPINTD_ACCU_QNTY,   
         TPPINTD_M_PRICE_FILTER,   
         TPPINTD_BS_PRICE_FILTER,   
         TPPINTD_DIVIDEND_POINTS  
         --,TPPINTD_SECOND_KIND_ID  
         --,TPPINTD_SECOND_MONTH  
FROM CI.TPPINTD  
WHERE  TPPINTD_DATE >= :as_fm_date   
AND TPPINTD_DATE <= :as_to_date   
AND (:as_mth_seq1 = 99 or TPPINTD_FIRST_MONTH = :as_mth_seq1)
AND (:as_mth_seq2 = 99 or TPPINTD_SECOND_MONTH = :as_mth_seq2)
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isPrint == "Y") {
            string[] title = new string[]{ "交易時段:0一般/1夜盤","交易日期","第一支腳契約代碼","第一支腳月份序號","成交價偏離幅度","成交間隔秒數",
                                                "最小累計口數","成交價濾網","買賣中價濾網","除息影響點數" };
            int k = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[k++];
            }
         }

         return dtResult;
      }

   }
}
