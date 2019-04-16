using OnePiece;
using System;
using System.Data;

/// <summary>
/// Winni 2019/01/23
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30630 {
      private Db db;

      public D30630() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// Get data by CI.RPT CI.AI2, CI.AM21 (已經固定一些過濾條件)
      /// </summary>
      /// <param name="as_market_code">全部/一般/盤後</param>
      /// <param name="as_prev_sym">前期起始日</param>
      /// <param name="as_prev_eym">前期結束日</param>
      /// <param name="as_sum_subtype">選全部"0"/非全部"3"</param>
      /// <param name="as_param_key">商品名</param>
      /// <param name="as_aft_sym">後期起始日</param>
      /// <param name="as_aft_eym">後期結束日</param>
      /// <returns></returns>
      public DataTable GetData(string as_market_code , string as_prev_sym , string as_prev_eym , string as_sum_subtype ,
                               string as_param_key , string as_aft_sym , string as_aft_eym) {
         object[] parms =
         {
                ":as_market_code", as_market_code,
                ":as_prev_sym", as_prev_sym,
                ":as_prev_eym", as_prev_eym,
                ":as_sum_subtype", as_sum_subtype,
                ":as_param_key", as_param_key,
                ":as_aft_sym", as_aft_sym,
                ":as_aft_eym", as_aft_eym
            };

         string sql = @"
SELECT A.*
FROM
(SELECT NVL(P.AM21_IDFG_TYPE,A.AM21_IDFG_TYPE) AS AM21_IDFG_TYPE,   
       NVL(P.AM21_M_QNTY,0) AS AM21_M_QNTY_PREV,   
       NVL(P.AM21_OI_QNTY,0) AS AM21_OI_QNTY_PREV,  
       P_DAYS.TRADE_DAYS AS TRADE_DAYS_PREV,   
       NVL(A.AM21_M_QNTY,0) AS AM21_M_QNTY_AFT,   
       NVL(A.AM21_OI_QNTY,0) AS AM21_OI_QNTY_AFT,   
       A_DAYS.TRADE_DAYS AS TRADE_DAYS_AFT,
       RPT_SEQ_NO
FROM   CI.RPT,
        --前期
        (SELECT AM21_IDFG_TYPE,   
                SUM(CASE :AS_MARKET_CODE WHEN '0%' THEN AM21_M_QNTY - NVL(AM21_AH_M_QNTY,0) 
                                         WHEN '1%' THEN NVL(AM21_AH_M_QNTY,0) 
                                         ELSE AM21_M_QNTY END) AS AM21_M_QNTY,
                SUM(AM21_OI_QNTY) AS AM21_OI_QNTY
        FROM CI.AM21
        WHERE AM21_SUM_TYPE = 'M'
        AND TRIM(AM21_YMD) >= :AS_PREV_SYM
        AND TRIM(AM21_YMD) <= :AS_PREV_EYM 
        AND AM21_SUM_SUBTYPE = :AS_SUM_SUBTYPE
        AND AM21_PARAM_KEY    LIKE :AS_PARAM_KEY
        GROUP BY AM21_IDFG_TYPE) P,
        --前期交易天數
        (SELECT COUNT(AI2_YMD) AS TRADE_DAYS
        FROM CI.AI2
        WHERE AI2_YMD >= :AS_PREV_SYM||'01'
        AND AI2_YMD <= :AS_PREV_EYM||'31'
        AND AI2_SUM_TYPE = 'D'
        AND AI2_SUM_SUBTYPE = '1'
        AND AI2_PROD_TYPE = 'F'
        AND (:AS_MARKET_CODE <> '1%' OR NVL(AI2_AH_DAY_COUNT,0) > 0)) P_DAYS,
        --後期  
        (SELECT AM21_IDFG_TYPE,   
                SUM(CASE :AS_MARKET_CODE WHEN '0%' THEN AM21_M_QNTY - NVL(AM21_AH_M_QNTY,0) 
                                         WHEN '1%' THEN NVL(AM21_AH_M_QNTY,0) 
                                         ELSE  AM21_M_QNTY END) AS AM21_M_QNTY, 
                SUM(AM21_OI_QNTY) AS AM21_OI_QNTY
         FROM CI.AM21
         WHERE AM21_SUM_TYPE = 'M'
         AND TRIM(AM21_YMD) >= :AS_AFT_SYM
         AND TRIM(AM21_YMD) <= :AS_AFT_EYM 
         AND AM21_SUM_SUBTYPE = :AS_SUM_SUBTYPE
         AND AM21_PARAM_KEY    LIKE :AS_PARAM_KEY
         GROUP BY AM21_IDFG_TYPE) A,
        --前期交易天數
        (SELECT COUNT(AI2_YMD) AS TRADE_DAYS
         FROM CI.AI2
         WHERE AI2_YMD >= :AS_AFT_SYM||'01'
         AND AI2_YMD <= :AS_AFT_EYM||'31'
         AND AI2_SUM_TYPE = 'D'
         AND AI2_SUM_SUBTYPE = '1'
         AND AI2_PROD_TYPE = 'F'
         AND (:AS_MARKET_CODE <> '1%' OR NVL(AI2_AH_DAY_COUNT,0) > 0)) A_DAYS
WHERE RPT_TXD_ID = '30631'
AND RPT_VALUE = P.AM21_IDFG_TYPE(+)
AND RPT_VALUE = A.AM21_IDFG_TYPE(+) ) A
WHERE AM21_M_QNTY_PREV + AM21_OI_QNTY_PREV + AM21_M_QNTY_AFT + AM21_OI_QNTY_AFT > 0
ORDER BY RPT_SEQ_NO
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
