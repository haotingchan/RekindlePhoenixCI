using OnePiece;
using System;
using System.Data;

/// <summary>
/// Winni 2019/01/29
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30590:DataGate {

      /// <summary>
      /// Get data by CI.APDK, CI.AI2, CI.AA2, CI.AB4, CI.AM9, CI.AM10 (已經固定一些過濾條件)
      /// </summary>
      /// <param name="as_expiry_type"></param>
      /// <param name="as_pc_code"></param>
      /// <param name="as_kind_id2">商品名</param>
      /// <param name="as_symd">起始日期</param>
      /// <param name="as_eymd">結束日期</param>
      /// <param name="as_market_code">全部/一般/盤後</param>
      /// <returns></returns>
      public DataTable GetData(string as_expiry_type , string as_pc_code , string as_kind_id2 , string as_symd ,
                               string as_eymd , string as_market_code) {
         object[] parms =
         {
                ":as_expiry_type", as_expiry_type,
                ":as_pc_code", as_pc_code,
                ":as_kind_id2", as_kind_id2,
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_market_code", as_market_code
            };

         string sql = @"
SELECT APDK_YMD,
        APDK_PARAM_KEY,
        APDK_KIND_ID2,
        APDK_EXPIRY_TYPE,
        APDK_PC_CODE,
        AI2_M_QNTY,
        AI2_OI,AM10_CNT,
        AA2_AMT,
        AA2_AMT_STK,
        AM9_ACC_CNT,
        AB4_ID_CNT
FROM
       (SELECT APDK_PARAM_KEY,
        APDK_KIND_ID2,
        APDK_EXPIRY_TYPE,
        APDK_PC_CODE,
        AI2_YMD AS APDK_YMD
        FROM
             (SELECT APDK_PARAM_KEY,
              APDK_KIND_ID2,
              APDK_EXPIRY_TYPE,
              APDK_PC_CODE
              FROM
                     (SELECT APDK_PARAM_KEY,
                            case when :as_expiry_type='9' then ' ' else APDK_KIND_ID2 end as APDK_KIND_ID2 ,
                            case when :as_expiry_type='9' then ' ' else APDK_EXPIRY_TYPE end as APDK_EXPIRY_TYPE,
                            case when :as_pc_code='Y' then 'C' else ' ' end as APDK_PC_CODE
                      FROM CI.APDK
                      WHERE APDK_PARAM_KEY = 'TXO'
                      AND APDK_KIND_ID2 LIKE :as_kind_id2
                      GROUP BY APDK_PARAM_KEY,
                            case when :as_expiry_type='9' then ' ' else APDK_KIND_ID2 end,
                            case when :as_expiry_type='9' then ' ' else APDK_EXPIRY_TYPE end
                      UNION
                      SELECT APDK_PARAM_KEY,
                            case when :as_expiry_type='9' then ' ' else APDK_KIND_ID2 end as APDK_KIND_ID2 ,
                            case when :as_expiry_type='9' then ' ' else APDK_EXPIRY_TYPE end as APDK_EXPIRY_TYPE,
                            case when :as_pc_code='Y' then 'P' else ' ' end as APDK_PC_CODE
                      FROM CI.APDK
                      WHERE APDK_PARAM_KEY = 'TXO'
                      AND APDK_KIND_ID2 LIKE :as_kind_id2
                      GROUP BY APDK_PARAM_KEY,
                               case when :as_expiry_type='9' then ' ' else APDK_KIND_ID2 end,
                               case when :as_expiry_type='9' then ' ' else APDK_EXPIRY_TYPE end)
              GROUP BY APDK_PARAM_KEY,APDK_KIND_ID2,APDK_EXPIRY_TYPE,APDK_PC_CODE),
                       (SELECT AI2_YMD
                        FROM CI.AI2
                        WHERE AI2_YMD >= :as_symd
                        AND AI2_YMD <= :as_eymd
                        AND AI2_SUM_TYPE = 'D'
                        AND AI2_PROD_TYPE = 'O'
                        AND AI2_SUM_SUBTYPE = '1' 
                        GROUP BY AI2_YMD)),
                       (SELECT AA2_YMD,AA2_PARAM_KEY,
                               case when :as_expiry_type='9' then ' ' else AA2_KIND_ID2 end as AA2_KIND_ID2,
                               ' ' as AA2_PC_CODE,
                               SUM(AA2_AMT) AS AA2_AMT,
                               SUM(AA2_AMT_STK) AS AA2_AMT_STK
                        FROM CI.AA2
                        WHERE AA2_YMD >= :as_symd
                        AND AA2_YMD <= :as_eymd
                        AND AA2_PARAM_KEY = 'TXO'
                        AND AA2_KIND_ID2 LIKE :as_kind_id2
                        AND AA2_MARKET_CODE LIKE trim(:as_market_code)||'%'
                        GROUP BY AA2_YMD,AA2_PARAM_KEY,
                                 case when :as_expiry_type='9' then ' ' else AA2_KIND_ID2 end),
                        --ID數AB4(全部期別999
                       (SELECT TO_CHAR(AB4_DATE,'YYYYMMDD') AS AB4_YMD,AB4_PARAM_KEY,
                               case when :as_expiry_type='9' then ' ' else AB4_KIND_ID end AS AB4_KIND_ID2,
                               ' ' as AB4_PC_CODE,AB4_ID_CNT
                        FROM CI.AB4
                        WHERE AB4_DATE >= TO_DATE(:as_symd,'YYYYMMDD')
                        AND AB4_DATE <= TO_DATE(:as_eymd,'YYYYMMDD')
                        AND AB4_PARAM_KEY = 'TXO'
                        AND AB4_KIND_ID LIKE case when :as_expiry_type='9' then '999%' else :as_kind_id2 end           
                        AND (AB4_MARKET_CODE = :as_market_code  or AB4_MARKET_CODE = ' ')),
                        --戶數AM9(全部期別999)
                       (SELECT AM9_YMD,AM9_PARAM_KEY,
                               case when :as_expiry_type='9' then ' ' else AM9_KIND_ID2 end as AM9_KIND_ID2,
                               ' ' as AM9_PC_CODE,AM9_ACC_CNT
                        FROM CI.AM9
                        WHERE AM9_YMD >= :as_symd
                        AND AM9_YMD <= :as_eymd
                        AND AM9_PARAM_KEY = 'TXO'
                        AND AM9_KIND_ID2 LIKE case when :as_expiry_type='9' then '999%' else :as_kind_id2 end
                        AND (AM9_MARKET_CODE = :as_market_code or AM9_MARKET_CODE = ' ')),
                        --成交筆數AM10
                       (SELECT AM10_YMD,
                               AM10_PARAM_KEY,
                               case when :as_expiry_type='9' then ' ' else AM10_KIND_ID2 end AS AM10_KIND_ID2,
                               case when :as_pc_code='Y' then AM10_PC_CODE else ' ' end as AM10_PC_CODE,
                               SUM(AM10_QNTY),SUM(AM10_CNT) AS AM10_CNT 
                               FROM CI.AM10
                        WHERE AM10_YMD >= :as_symd
                        AND AM10_YMD <= :as_eymd
                        AND AM10_PROD_TYPE = 'O'
                        AND AM10_PARAM_KEY = 'TXO'
                        AND AM10_KIND_ID2 LIKE :as_kind_id2
                        AND AM10_MARKET_CODE LIKE trim(:as_market_code)||'%'
                        GROUP BY AM10_YMD,
                                 AM10_PARAM_KEY,
                                 case when :as_expiry_type='9' then ' ' else AM10_KIND_ID2 end,
                                 case when :as_pc_code='Y' then AM10_PC_CODE else ' ' end),
                        --未平倉量及成交量AI2
                       (SELECT AI2_YMD,AI2_PARAM_KEY,
                               case when :as_expiry_type='9' then ' ' else AI2_KIND_ID2 end as AI2_KIND_ID2,
                               case when :as_pc_code='Y' then AI2_PC_CODE else ' ' end AS AI2_PC_CODE,
                               SUM(case :as_market_code when '0' then AI2_M_QNTY - nvl(AI2_AH_M_QNTY,0)
                                                        when '1' then nvl(AI2_AH_M_QNTY,0)
                                                        else AI2_M_QNTY end) AS AI2_M_QNTY,
                               SUM(AI2_OI) AS AI2_OI
                        FROM CI.AI2
                        WHERE AI2_YMD >= :as_symd
                        AND AI2_YMD <= :as_eymd
                        AND AI2_SUM_TYPE = 'D'
                        AND AI2_PARAM_KEY = 'TXO'
                        AND AI2_KIND_ID2 LIKE :as_kind_id2
                        AND AI2_SUM_SUBTYPE = '5' 
                        GROUP BY AI2_YMD,AI2_PARAM_KEY,
                                 case when :as_expiry_type='9' then ' ' else AI2_KIND_ID2 end,
                                 case when :as_pc_code='Y' then AI2_PC_CODE else ' ' end)
where APDK_YMD = AA2_YMD(+)
  and APDK_PARAM_KEY = AA2_PARAM_KEY(+)
  and APDK_KIND_ID2 = AA2_KIND_ID2(+)
  and APDK_PC_CODE = AA2_PC_CODE(+)
  and APDK_YMD = AB4_YMD(+)
  and APDK_PARAM_KEY = AB4_PARAM_KEY(+)
  and APDK_KIND_ID2 = AB4_KIND_ID2(+)
  and APDK_PC_CODE = AB4_PC_CODE(+)
  and APDK_YMD = AM9_YMD(+)
  and APDK_PARAM_KEY = AM9_PARAM_KEY(+)
  and APDK_KIND_ID2 = AM9_KIND_ID2(+)
  and APDK_PC_CODE = AM9_PC_CODE(+)
  and APDK_YMD = AM10_YMD(+)
  and APDK_PARAM_KEY = AM10_PARAM_KEY(+)
  and APDK_KIND_ID2 = AM10_KIND_ID2(+)
  and APDK_PC_CODE = AM10_PC_CODE(+)
  and APDK_YMD = AI2_YMD(+)
  and APDK_PARAM_KEY = AI2_PARAM_KEY(+)
  and APDK_KIND_ID2 = AI2_KIND_ID2(+)
  and APDK_PC_CODE = AI2_PC_CODE(+)
order by apdk_ymd , apdk_param_key , apdk_kind_id2 , apdk_pc_code 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
