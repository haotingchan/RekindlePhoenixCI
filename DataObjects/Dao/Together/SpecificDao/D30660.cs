using OnePiece;
using System;
using System.Data;

/// <summary>
/// Winni 2019/02/18
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30660 {


      private Db db;

      public D30660() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// Get data by CI.AI2 CI.AE2 CI.AI6 CI.AE6 CI.AE6AH (已經固定一些過濾條件) 
      /// (DATE_RANGE/T_HIGH_LOW/AH_HIGH_LOW/E_HIGH_LOW/AI6_RATE/AE6_RATE/T_F_QNTY/T_O_QNTY/T_ALL_QNTY/E_F_QNTY/E_O_QNTY/E_ALL_QNTY/RTN_RATE/DATA_TYPE)
      /// </summary>
      /// <param name="as_1_fm_ymd">上週：起日(yyyyMMdd)</param>
      /// <param name="as_1_to_ymd">上週：訖日(yyyyMMdd)</param>
      /// <param name="as_2_fm_ymd">本週：起日(yyyyMMdd)</param>
      /// <param name="as_2_to_ymd">本週：訖日(yyyyMMdd)</param>
      /// <param name="as_3_fm_ymd">全期：起日(yyyyMMdd)</param>
      /// <param name="as_3_to_ymd">全期：訖日(yyyyMMdd)</param>
      /// <returns></returns>
      public DataTable GetData(string as_1_fm_ymd , string as_1_to_ymd , string as_2_fm_ymd ,
                                  string as_2_to_ymd , string as_3_fm_ymd , string as_3_to_ymd) {
         object[] parms =
         {
               ":as_1_fm_ymd", as_1_fm_ymd,
               ":as_1_to_ymd", as_1_to_ymd,
               ":as_2_fm_ymd", as_2_fm_ymd,
               ":as_2_to_ymd", as_2_to_ymd,
               ":as_3_fm_ymd", as_3_fm_ymd,
               ":as_3_to_ymd", as_3_to_ymd
            };

         string sql = @"
select 
to_char(to_date(fm_ymd,'yyyymmdd'),'yyyy/mm/dd') || ' - '||to_char(to_date(to_ymd,'yyyymmdd'),'yyyy/mm/dd') as DATE_RANGE,
        case when T.CNT > 0 then round(T.TOT_HIGH_LOW / T.CNT,14) else 0 end as T_HIGH_LOW,
        case when AH.CNT > 0 then round(AH.TOT_HIGH_LOW / AH.CNT,14) else 0 end as AH_HIGH_LOW,
        case when E.CNT > 0 then round(E.TOT_HIGH_LOW / E.CNT,14) else 0 end as E_HIGH_LOW,
        AI6_RATE,AE6_RATE,
        case when AI.CNT > 0 then round(AI.F_M_QNTY / AI.CNT,14) else 0 end as T_F_QNTY,
        case when AI.CNT > 0 then round(AI.O_M_QNTY / AI.CNT,14) else 0 end as T_O_QNTY,
        case when AI.CNT > 0 then round((AI.F_M_QNTY + AI.O_M_QNTY) / AI.CNT,14) else 0 end as T_ALL_QNTY,
        case when AE.CNT > 0 then round(AE.F_M_QNTY / AE.CNT,14) else 0 end as E_F_QNTY,
        case when AE.CNT > 0 then round(AE.O_M_QNTY / AE.CNT,14) else 0 end as E_O_QNTY,
        case when AE.CNT > 0 then round((AE.F_M_QNTY + AE.O_M_QNTY) / AE.CNT,14) else 0 end as E_ALL_QNTY,
        case when AE.F_M_QNTY > 0 then round((AE.F_OI + AE.O_OI) / (AE.F_M_QNTY + AE.O_M_QNTY),4) else 0 end RTN_RATE,
       AI.DATA_TYPE
   from
--
(select '1' as DATA_TYPE, :as_1_fm_ymd as fm_ymd,:as_1_to_ymd  as to_ymd from dual 
  union all
 select '2' as DATA_TYPE, :as_2_fm_ymd as fm_ymd,:as_2_to_ymd  as to_ymd from dual 
  union all
 select '3' as DATA_TYPE, :as_3_fm_ymd as fm_ymd,:as_3_to_ymd  as to_ymd from dual ) DT,
--TXF,TXO交易量
(select '1' as DATA_TYPE,
        sum(case when AI2_PARAM_KEY = 'TXF' then AI2_M_QNTY else 0 end) as F_M_QNTY,
        sum(case when AI2_PARAM_KEY = 'TXO' then AI2_M_QNTY else 0 end) as O_M_QNTY,
        count(distinct AI2_YMD) as CNT
   from ci.AI2
  where AI2_SUM_TYPE = 'D'
    and AI2_SUM_SUBTYPE = '3'
    and AI2_PARAM_KEY IN ('TXF','TXO')
    and AI2_YMD >= :as_1_fm_ymd
    and AI2_YMD <= :as_1_to_ymd
  union all      
 select '2',
        sum(case when AI2_PARAM_KEY = 'TXF' then AI2_M_QNTY else 0 end) as F_M_QNTY,
        sum(case when AI2_PARAM_KEY = 'TXO' then AI2_M_QNTY else 0 end) as O_M_QNTY,
        count(distinct AI2_YMD) as CNT
   from ci.AI2
  where AI2_SUM_TYPE = 'D'
    and AI2_SUM_SUBTYPE = '3'
    and AI2_PARAM_KEY IN ('TXF','TXO')
    and AI2_YMD >= :as_2_fm_ymd
    and AI2_YMD <= :as_2_to_ymd
  union all      
 select '3',
        sum(case when AI2_PARAM_KEY = 'TXF' then AI2_M_QNTY else 0 end) as F_M_QNTY,
        sum(case when AI2_PARAM_KEY = 'TXO' then AI2_M_QNTY else 0 end) as O_M_QNTY,
        count(distinct AI2_YMD) as CNT
   from ci.AI2
  where AI2_SUM_TYPE = 'D'
    and AI2_SUM_SUBTYPE = '3'
    and AI2_PARAM_KEY IN ('TXF','TXO')
    and AI2_YMD >= :as_3_fm_ymd
    and AI2_YMD <= :as_3_to_ymd) AI,
--EUREX交易量及OI    
(select '1' as DATA_TYPE,
        sum(case when AE2_PARAM_KEY = 'TXF' then AE2_M_QNTY else 0 end) as F_M_QNTY,
        sum(case when AE2_PARAM_KEY = 'TXF' then AE2_OI else 0 end) as F_OI,
        sum(case when AE2_PARAM_KEY = 'TXO' then AE2_M_QNTY else 0 end) as O_M_QNTY,
        sum(case when AE2_PARAM_KEY = 'TXO' then AE2_OI else 0 end) as O_OI,
        count(distinct AE2_YMD) as CNT
   from ci.AE2
  where AE2_SUM_TYPE = 'D'
    and AE2_SUM_SUBTYPE = '3'
    and AE2_PARAM_KEY IN ('TXF','TXO')
    and AE2_YMD >= :as_1_fm_ymd
    and AE2_YMD <= :as_1_to_ymd
  union all      
 select '2',
        sum(case when AE2_PARAM_KEY = 'TXF' then AE2_M_QNTY else 0 end) as F_M_QNTY,
        sum(case when AE2_PARAM_KEY = 'TXF' then AE2_OI else 0 end) as F_OI,
        sum(case when AE2_PARAM_KEY = 'TXO' then AE2_M_QNTY else 0 end) as O_M_QNTY,
        sum(case when AE2_PARAM_KEY = 'TXO' then AE2_OI else 0 end) as O_OI,
        count(distinct AE2_YMD) as CNT
   from ci.AE2
  where AE2_SUM_TYPE = 'D'
    and AE2_SUM_SUBTYPE = '3'
    and AE2_PARAM_KEY IN ('TXF','TXO')
    and AE2_YMD >= :as_2_fm_ymd
    and AE2_YMD <= :as_2_to_ymd
  union all      
 select '3',
        sum(case when AE2_PARAM_KEY = 'TXF' then AE2_M_QNTY else 0 end) as F_M_QNTY,
        sum(case when AE2_PARAM_KEY = 'TXF' then AE2_OI else 0 end) as F_OI,
        sum(case when AE2_PARAM_KEY = 'TXO' then AE2_M_QNTY else 0 end) as O_M_QNTY,
        sum(case when AE2_PARAM_KEY = 'TXO' then AE2_OI else 0 end) as O_OI,
        count(distinct AE2_YMD) as CNT
   from ci.AE2
  where AE2_SUM_TYPE = 'D'
    and AE2_SUM_SUBTYPE = '3'
    and AE2_PARAM_KEY IN ('TXF','TXO')
    and AE2_YMD >= :as_3_fm_ymd
    and AE2_YMD <= :as_3_to_ymd) AE,
--TX波動度
(select '1' as DATA_TYPE,round(STDDEV(AI6_LN_RETURN) * 16,6) as AI6_RATE,
        SUM(AI6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AI6_DATE) as CNT
   from CI.AI6
  where AI6_DATE >= TO_DATE(:as_1_fm_ymd,'YYYYMMDD')
    and AI6_DATE <= TO_DATE(:as_1_to_ymd,'YYYYMMDD')
    and AI6_KIND_ID = 'TXF'
  union all   
 select '2' as DATA_TYPE,round(STDDEV(AI6_LN_RETURN) * 16,6) as AI6_RATE,
        SUM(AI6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AI6_DATE) as CNT
   from CI.AI6
  where AI6_DATE >= TO_DATE(:as_2_fm_ymd,'YYYYMMDD')
    and AI6_DATE <= TO_DATE(:as_2_to_ymd,'YYYYMMDD')
    and AI6_KIND_ID = 'TXF'
  union all   
 select '3' as DATA_TYPE,round(STDDEV(AI6_LN_RETURN) * 16,6) as AI6_RATE,
        SUM(AI6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AI6_DATE) as CNT
   from CI.AI6
  where AI6_DATE >= TO_DATE(:as_3_fm_ymd,'YYYYMMDD')
    and AI6_DATE <= TO_DATE(:as_3_to_ymd,'YYYYMMDD')
    and AI6_KIND_ID = 'TXF') T,
--TX夜盤波動度
(select '1' as DATA_TYPE,SUM(AI6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AI6_DATE) as CNT
   from CI.AI6AH
  where AI6_DATE >= TO_DATE(:as_1_fm_ymd,'YYYYMMDD')
    and AI6_DATE <= TO_DATE(:as_1_to_ymd,'YYYYMMDD')
    and AI6_KIND_ID = 'TXF'
  union all   
 select '2' as DATA_TYPE,SUM(AI6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AI6_DATE) as CNT
   from CI.AI6AH
  where AI6_DATE >= TO_DATE(:as_2_fm_ymd,'YYYYMMDD')
    and AI6_DATE <= TO_DATE(:as_2_to_ymd,'YYYYMMDD')
    and AI6_KIND_ID = 'TXF'
  union all   
 select '3' as DATA_TYPE,SUM(AI6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AI6_DATE) as CNT
   from CI.AI6AH
  where AI6_DATE >= TO_DATE(:as_3_fm_ymd,'YYYYMMDD')
    and AI6_DATE <= TO_DATE(:as_3_to_ymd,'YYYYMMDD')
    and AI6_KIND_ID = 'TXF') AH,
--EUREX-FTX波動度
(select '1' as DATA_TYPE,round(STDDEV(AE6_LN_RETURN) * 16,6) as AE6_RATE,
        SUM(AE6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AE6_YMD) as CNT
   from CI.AE6
  where AE6_YMD >= :as_1_fm_ymd
    and AE6_YMD <= :as_1_to_ymd
    and  not AE6_LN_RETURN is null
    and AE6_KIND_ID = 'TXF'
  union all   
 select '2' as DATA_TYPE,round(STDDEV(AE6_LN_RETURN) * 16,6) as AE6_RATE,
        SUM(AE6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AE6_YMD) as CNT
   from CI.AE6
  where AE6_YMD >= :as_2_fm_ymd
    and AE6_YMD <= :as_2_to_ymd
    and  not AE6_LN_RETURN is null
    and AE6_KIND_ID = 'TXF'
  union all   
 select '3' as DATA_TYPE,round(STDDEV(AE6_LN_RETURN) * 16,6) as AE6_RATE,
        SUM(AE6_HIGH_LOW) as TOT_HIGH_LOW,COUNT(DISTINCT AE6_YMD) as CNT
   from CI.AE6
  where AE6_YMD >= :as_3_fm_ymd
    and AE6_YMD <= :as_3_to_ymd
    and  not AE6_LN_RETURN is null
    and AE6_KIND_ID = 'TXF') E
  where AI.DATA_TYPE = AE.DATA_TYPE
    and AI.DATA_TYPE = T.DATA_TYPE
    and AI.DATA_TYPE = AH.DATA_TYPE(+)
    and AI.DATA_TYPE = E.DATA_TYPE
    and AI.DATA_TYPE = DT.DATA_TYPE
order by data_type
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Query data by CI.AI2 CI.AE2 CI.AI6 CI.AE6 CI.AE6AH (已經固定一些過濾條件) get 19個欄位
      /// </summary>
      /// <param name="as_fm_ymd">全期：起日(yyyyMMdd)</param>
      /// <param name="as_to_ymd">全期：訖日(yyyyMMDd)</param>
      /// <returns></returns>
      public DataTable GetDetailData(string as_fm_ymd , string as_to_ymd) {
         object[] parms =
         {
                ":as_fm_ymd", as_fm_ymd,
                ":as_to_ymd", as_to_ymd
            };

         string sql = @"
select to_char(to_date(AI2_YMD,'yyyymmdd'),'yyyy/mm/dd') as AI2_YMD,
        T.AI6_HIGH_LOW,AH.AI6_HIGH_LOW,AE6_HIGH_LOW,
        T.AI6_LAST_CLOSE_PRICE,T.AI6_CLOSE_PRICE,T.AI6_LN_RETURN,
        AE6_LAST_CLOSE_PRICE,AE6_CLOSE_PRICE,AE6_LN_RETURN,
        AI.F_M_QNTY,AI.O_M_QNTY,AI.F_M_QNTY + AI.O_M_QNTY AS AI_M_QNTY,
        AE.F_M_QNTY,AE.O_M_QNTY,AE.F_M_QNTY + AE.O_M_QNTY AS AE_M_QNTY,
        F_OI ,O_OI ,F_OI +O_OI as TOT_OI
   from
--TXF,TXO交易量
(select AI2_YMD,
        sum(case when AI2_PARAM_KEY = 'TXF' then AI2_M_QNTY else 0 end) as F_M_QNTY,
        sum(case when AI2_PARAM_KEY = 'TXO' then AI2_M_QNTY else 0 end) as O_M_QNTY
   from ci.AI2
  where AI2_SUM_TYPE = 'D'
    and AI2_SUM_SUBTYPE = '3'
    and AI2_PARAM_KEY IN ('TXF','TXO')
    and AI2_YMD >= :as_fm_ymd
    and AI2_YMD <= :as_to_ymd
  group by AI2_YMD) AI,
--EUREX交易量及OI    
(select AE2_YMD,
        sum(case when AE2_PARAM_KEY = 'TXF' then AE2_M_QNTY else 0 end) as F_M_QNTY,
        sum(case when AE2_PARAM_KEY = 'TXO' then AE2_M_QNTY else 0 end) as O_M_QNTY,
        sum(case when AE2_PARAM_KEY = 'TXF' then AE2_OI else 0 end) as F_OI,
        sum(case when AE2_PARAM_KEY = 'TXO' then AE2_OI else 0 end) as O_OI
   from ci.AE2
  where AE2_SUM_TYPE = 'D'
    and AE2_SUM_SUBTYPE = '3'
    and AE2_PARAM_KEY IN ('TXF','TXO')
    and AE2_YMD >= :as_fm_ymd
    and AE2_YMD <= :as_to_ymd
   group by AE2_YMD) AE,
--TX振幅
(select to_char(AI6_DATE,'YYYYMMDD') as AI6_YMD,AI6_HIGH_LOW,AI6_CLOSE_PRICE,AI6_LN_RETURN,AI6_LAST_CLOSE_PRICE
   from CI.AI6
  where AI6_DATE >= TO_DATE(:as_fm_ymd,'YYYYMMDD')
    and AI6_DATE <= TO_DATE(:as_to_ymd,'YYYYMMDD')
    and AI6_KIND_ID = 'TXF'
    and AI6_SETTLE_DATE <> '000000') T,
--TX夜盤波動度
(select to_char(AI6_DATE,'YYYYMMDD') as AI6_YMD,AI6_HIGH_LOW
   from CI.AI6AH
  where AI6_DATE >= TO_DATE(:as_fm_ymd,'YYYYMMDD')
    and AI6_DATE <= TO_DATE(:as_to_ymd,'YYYYMMDD')
    and AI6_KIND_ID = 'TXF'
    and AI6_SETTLE_DATE <> '000000') AH,
--EUREX-FTX振幅
(select AE6_YMD,AE6_HIGH_LOW,AE6_CLOSE_PRICE,AE6_LN_RETURN,AE6_LAST_CLOSE_PRICE
   from CI.AE6
  where AE6_YMD >= :as_fm_ymd
    and AE6_YMD <= :as_to_ymd
    and AE6_KIND_ID = 'TXF')
  where AI.AI2_YMD = AE2_YMD(+)
    and AI.AI2_YMD = T.AI6_YMD
    and AI.AI2_YMD = AH.AI6_YMD(+)
    and AI.AI2_YMD = AE6_YMD(+)
order by ai2_ymd
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
