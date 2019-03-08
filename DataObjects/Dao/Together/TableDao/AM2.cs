using OnePiece;
using System.Data;
/// <summary>
/// Winni, 2019/1/25
/// </summary>
namespace DataObjects.Dao.Together {
    public class AM2 : DataGate
    {

        /// <summary>
        /// get CI.AM2 data (for W28510) 好像跟28510跟28511的Stored Procedue有關係，但不確定
        /// </summary>
        /// <returns></returns>
        public DataTable ListData() {

            string sql = @"
SELECT AM2_YMD,   
       AM2_SUM_TYPE,   
       AM2_IDFG_TYPE,   
       AM2_PROD_TYPE,   
       AM2_PROD_SUBTYPE,   
       AM2_PARAM_KEY,   
       AM2_PC_CODE,   
       AM2_BS_CODE,   
       AM2_M_QNTY  
FROM ci.AM2
";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// Get Am2 Data For 3 series
        /// </summary>
        /// <param name="ymd"></param>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        public DataTable ListAm2DataByYmd(string ymd, string paramKey) {

            object[] parms = {
                ":as_ymd", ymd,
                ":as_param_key", paramKey
            };

            string sql = @"SELECT AM2_KIND_ID2 AS AM2_PARAM_KEY,
         AM2_IDFG_TYPE,   
         AM2_BS_CODE,   
         SUM(AM2_M_QNTY) AS AM2_M_QNTY,   
         AM2_YMD,   
         AM2_SUM_TYPE  
    FROM ci.AM2  
   WHERE AM2_SUM_TYPE = 'M'  AND  
         TRIM(AM2_YMD) = :as_ymd  AND 
         AM2_SUM_SUBTYPE = '4' and
         AM2_IDFG_TYPE in ('1','2','3','4','5','6','7','8','9') AND
         AM2_PARAM_KEY = :as_param_key
  GROUP BY AM2_PARAM_KEY,AM2_KIND_ID2,
         AM2_IDFG_TYPE,   
         AM2_BS_CODE,  
         AM2_YMD,   
         AM2_SUM_TYPE  
  HAVING SUM(AM2_M_QNTY) > 0";

            return db.GetDataTable(sql, parms);
        }

      /// <summary>
      /// d_am2_1
      /// </summary>
      /// <param name="as_param_key">ex: TXF</param>
      /// <param name="as_sym">起始日期</param>
      /// <param name="as_eym">終止日期</param>
      /// <returns></returns>
      public DataTable ListAM2(string as_param_key, string as_sym, string as_eym)
      {
         object[] parms =
         {
                ":as_param_key", as_param_key,
                ":as_sym", as_sym,
                ":as_eym", as_eym
            };

         string sql = @"
                     SELECT AM2_IDFG_TYPE,   
                           AM2_BS_CODE,   
                           sum(AM2_M_QNTY) as AM2_M_QNTY ,   
                           substr(AM2_YMD,1,6) as AM2_YMD,   
                           AM2_SUM_TYPE  ,
                           '3' as SORT_TYPE  
                        FROM ci.AM2  
                     WHERE AM2_SUM_TYPE = 'M'  AND  
                           trim(AM2_YMD) >= :as_sym  AND  
                           trim(AM2_YMD) <= :as_eym  AND  
                           trim(AM2_PARAM_KEY) = :as_param_key  AND  
                           AM2_SUM_SUBTYPE = '3'  AND  
                           AM2_IDFG_TYPE in ('1','2','3','5','6','7','8','9')
                     GROUP BY  AM2_IDFG_TYPE,   
                           AM2_BS_CODE,   
                           AM2_SUM_TYPE,   
                           substr(AM2_YMD,1,6)
                     ORDER BY SORT_TYPE,AM2_YMD,AM2_IDFG_TYPE,AM2_BS_CODE
";

         return db.GetDataTable(sql, parms);

      }

   }
}

