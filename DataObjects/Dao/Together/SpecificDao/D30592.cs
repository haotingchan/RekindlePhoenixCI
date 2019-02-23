using OnePiece;
using System;
using System.Data;

/// <summary>
/// Winni 2019/02/11
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30592:DataGate {

      /// <summary>
      /// Get data by CI.APDK, CI.AI2, CI.AA2, CI.AB4, CI.AM9, CI.AM10 (已經固定一些過濾條件)
      /// </summary>
      /// <param name="as_symd">查詢起始日期(yyyyMMdd)</param>
      /// <param name="as_eymd">查詢結束日期(yyyyMMdd)</param>
      /// <param name="as_market_code">交易盤別(0 = 一般 / 1 = 盤後 / % = 全部)</param>
      /// <returns></returns>
      public DataTable GetData(string as_symd , string as_eymd , string as_market_code) {
         object[] parms =
         {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_market_code", as_market_code
            };

         string sql = @"
SELECT  APDK_YMD,
        APDK_PARAM_KEY,
        AI2_M_QNTY,
        AI2_OI,
        AM10_CNT,
        AA2_AMT,
        AA2_AMT_STK,
        AM9_ACC_CNT,
        AB4_ID_CNT,
        AA2_AMT_ORG_CURRENCY
  FROM
       (
       SELECT APDK_PARAM_KEY,AI2_YMD AS APDK_YMD
         FROM
             (SELECT APDK_PARAM_KEY
                  from CI.APDK
              WHERE  APDK_PROD_TYPE = 'F'
                    and APDK_PROD_SUBTYPE = 'E'
               group by APDK_PARAM_KEY),
             (SELECT AI2_YMD
                FROM CI.AI2
               WHERE AI2_YMD >= :as_symd
                 AND AI2_YMD <= :as_eymd
                 AND AI2_SUM_TYPE = 'D'
                 AND AI2_PROD_TYPE = 'F'
                 AND AI2_PROD_SUBTYPE = 'E'
                 AND AI2_SUM_SUBTYPE = '2' 
               GROUP BY AI2_YMD)),
       (SELECT AA2_YMD,AA2_PARAM_KEY,
               SUM(AA2_AMT)/2 AS AA2_AMT,SUM(AA2_AMT_STK)/2 AS AA2_AMT_STK,
               sum(AA2_AMT_ORG_CURRENCY) / 2 as AA2_AMT_ORG_CURRENCY
          FROM CI.AA2
         WHERE AA2_YMD >= :as_symd
           and AA2_YMD <= :as_eymd
           and AA2_PROD_tYPE = 'F'
           AND AA2_PROD_SUBTYPE = 'E'
           AND AA2_MARKET_CODE LIKE trim(:as_market_code)||'%'
         GROUP BY AA2_YMD,AA2_PARAM_KEY),
       --ID數AB4(全部期別999
       (SELECT TO_CHAR(AB4_DATE,'YYYYMMDD') AS AB4_YMD,AB4_PARAM_KEY,
               AB4_ID_CNT
          FROM CI.AB4
         WHERE AB4_DATE >= TO_DATE(:as_symd,'YYYYMMDD')
           and AB4_DATE <= TO_DATE(:as_eymd,'YYYYMMDD')
           and AB4_PROD_TYPE = 'F'
           and AB4_PROD_SUBTYPE = 'E'
           and AB4_KIND_ID = '999'
           AND (AB4_MARKET_CODE = :as_market_code  or AB4_MARKET_CODE = ' ')),
       --戶數AM9(全部期別999)
       (SELECT AM9_YMD,AM9_PARAM_KEY,
               AM9_ACC_CNT
          FROM CI.AM9
         WHERE AM9_YMD >= :as_symd
           and AM9_YMD <= :as_eymd
           and AM9_PROD_TYPE = 'F'
           and AM9_PROD_SUBTYPE = 'E'
           and AM9_PARAM_KEY = AM9_KIND_ID2 
           AND (AM9_MARKET_CODE = :as_market_code or AM9_MARKET_CODE = ' ')),
       --成交筆數AM10
       (SELECT AM10_YMD,AM10_PARAM_KEY,
               SUM(AM10_QNTY) as AM10_QNTY,SUM(AM10_CNT) as AM10_CNT 
          FROM CI.AM10
         WHERE AM10_YMD >= :as_symd
           and AM10_YMD <= :as_eymd
           AND AM10_PROD_TYPE = 'F'
           AND AM10_PROD_SUBTYPE = 'E'
           AND AM10_MARKET_CODE LIKE trim(:as_market_code)||'%'
         GROUP BY AM10_YMD,AM10_PARAM_KEY),
       --未平倉量及成交量AI2
       (SELECT AI2_YMD,AI2_PARAM_KEY,
               SUM(case :as_market_code when '0' then AI2_M_QNTY - nvl(AI2_AH_M_QNTY,0)
                                        when '1' then nvl(AI2_AH_M_QNTY,0)
                                        else AI2_M_QNTY end) AS AI2_M_QNTY,
               SUM(AI2_OI) AS AI2_OI
          FROM CI.AI2
         WHERE AI2_YMD >= :as_symd
           AND AI2_YMD <= :as_eymd
           AND AI2_SUM_TYPE = 'D'
           AND AI2_PROD_TYPE = 'F'
           AND AI2_PROD_SUBTYPE = 'E'
           AND AI2_SUM_SUBTYPE = '3' 
         GROUP BY AI2_YMD,AI2_PARAM_KEY)
      --交易人類別AM2
where APDK_YMD = AA2_YMD(+)
  and APDK_PARAM_KEY = AA2_PARAM_KEY(+)
  and APDK_YMD = AB4_YMD(+)
  and APDK_PARAM_KEY = AB4_PARAM_KEY(+)
  and APDK_YMD = AM9_YMD(+)
  and APDK_PARAM_KEY = AM9_PARAM_KEY(+)
  and APDK_YMD = AM10_YMD(+)
  and APDK_PARAM_KEY = AM10_PARAM_KEY(+)
  and APDK_YMD = AI2_YMD(+)
  and APDK_PARAM_KEY = AI2_PARAM_KEY(+)
order by apdk_ymd , apdk_param_key
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Get data by CI.AM1, CI.AM12 (RMB期貨交易量)
      /// </summary>
      /// <param name="as_fm_ymd">查詢起始日期(yyyyMMdd)</param>
      /// <param name="as_to_ymd">查詢結束日期(yyyyMMdd)</param>
      /// <returns></returns>
      public DataTable GetRmbData(string as_fm_ymd , string as_to_ymd) {
         object[] parms =
         {
                ":as_fm_ymd", as_fm_ymd,
                ":as_to_ymd", as_to_ymd
            };

         string sql = @"
select to_date(DT_YMD,'yyyymmdd') as DT_DATE,
       CME_VOL,
       HKEX_VOL,
       SGX_VOL,
       RHF_VOL,
       RTF_VOL,
       TRADE_FLAG
  from       
      (select DT_YMD,max(RHF_VOL) as RHF_VOL,max(RTF_VOL) as RTF_VOL, 
                 case when max(RHF_VOL) is null then 'N' else 'Y' end as TRADE_FLAG
         from 
             (select AM1_YMD as DT_YMD,
                           sum(case when AM1_KIND_ID = 'RHF' then AM1_M_QNTY else 0 end)/2 as RHF_VOL,
                           sum(case when AM1_KIND_ID = 'RTF' then AM1_M_QNTY else 0 end)/2 as RTF_VOL
                      from ci.AM1
                     where AM1_YMD >= :as_fm_ymd  
                       and AM1_YMD <= :as_to_ymd
                       and AM1_SUM_TYPE = 'D'
                       and AM1_KIND_ID in ('RHF','RTF')
                      group by AM1_YMD 
             union all
             select AM12_YMD,null,null from ci.AM12
                     where AM12_YMD >= :as_fm_ymd  
                       and AM12_YMD <= :as_to_ymd
             group by AM12_YMD)
         group by DT_YMD) DT,
      (select AM12_YMD as CME_YMD,AM12_VOL as CME_VOL 
         from ci.AM12 
        where AM12_YMD >= :as_fm_ymd
          and AM12_YMD <= :as_to_ymd
          and AM12_F_ID = 'CME'),
      (select AM12_YMD as HKEX_YMD,AM12_VOL as HKEX_VOL 
         from ci.AM12 
        where AM12_YMD >= :as_fm_ymd
          and AM12_YMD <= :as_to_ymd
          and AM12_F_ID = 'HKEX'),
      (select AM12_YMD as SGX_YMD,AM12_VOL as SGX_VOL 
         from ci.AM12 
        where AM12_YMD >= :as_fm_ymd
          and AM12_YMD <= :as_to_ymd
          and AM12_F_ID = 'SGX')
 where DT_YMD = CME_YMD(+)
   and DT_YMD = HKEX_YMD(+)
   and DT_YMD = SGX_YMD(+)
order by dt_date 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
