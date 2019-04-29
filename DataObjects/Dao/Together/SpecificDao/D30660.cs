using OnePiece;
using System.Data;

/// <summary>
/// Winni 2019/02/18
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30660 : DataGate {

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
select to_char(to_date(fm_ymd,'yyyymmdd'),'yyyy/mm/dd') || ' - '||to_char(to_date(to_ymd,'yyyymmdd'),'yyyy/mm/dd') as DATE_RANGE,
        case when T.CNT > 0 then round(T.TOT_HIGH_LOW / T.CNT) else 0 end as T_HIGH_LOW,
        case when AH.CNT > 0 then round(AH.TOT_HIGH_LOW / AH.CNT) else 0 end as AH_HIGH_LOW,
        case when E.CNT > 0 then round(E.TOT_HIGH_LOW / E.CNT) else 0 end as E_HIGH_LOW,
        AI6_RATE,AE6_RATE,
        case when AI.CNT > 0 then AI.F_M_QNTY / AI.CNT else 0 end as T_F_QNTY,
        case when AI.CNT > 0 then AI.O_M_QNTY / AI.CNT else 0 end as T_O_QNTY,
        case when AI.CNT > 0 then (AI.F_M_QNTY + AI.O_M_QNTY) / AI.CNT else 0 end as T_ALL_QNTY,
        case when AE.CNT > 0 then AE.F_M_QNTY / AE.CNT else 0 end as E_F_QNTY,
        case when AE.CNT > 0 then AE.O_M_QNTY / AE.CNT else 0 end as E_O_QNTY,
        case when AE.CNT > 0 then (AE.F_M_QNTY + AE.O_M_QNTY) / AE.CNT else 0 end as E_ALL_QNTY,
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
select 
	to_char(to_date(fm_ymd,'yyyymmdd'),'yyyy/mm/dd') || ' - '||to_char(to_date(to_ymd,'yyyymmdd'),'yyyy/mm/dd') as date_range,
	case when t.cnt > 0 then round(t.tot_high_low / t.cnt) else 0 end as t_high_low,
	case when ah.cnt > 0 then round(ah.tot_high_low / ah.cnt) else 0 end as ah_high_low,
	case when e.cnt > 0 then round(e.tot_high_low / e.cnt) else 0 end as e_high_low,
	ai6_rate,ae6_rate,
	case when ai.cnt > 0 then ai.f_m_qnty / ai.cnt else 0 end as t_f_qnty,
	case when ai.cnt > 0 then ai.o_m_qnty / ai.cnt else 0 end as t_o_qnty,
	case when ai.cnt > 0 then (ai.f_m_qnty + ai.o_m_qnty) / ai.cnt else 0 end as t_all_qnty,
	case when ae.cnt > 0 then ae.f_m_qnty / ae.cnt else 0 end as e_f_qnty,
	case when ae.cnt > 0 then ae.o_m_qnty / ae.cnt else 0 end as e_o_qnty,
	case when ae.cnt > 0 then (ae.f_m_qnty + ae.o_m_qnty) / ae.cnt else 0 end as e_all_qnty,
	case when ae.f_m_qnty > 0 then round((ae.f_oi + ae.o_oi) / (ae.f_m_qnty + ae.o_m_qnty),4) else 0 end rtn_rate,
	ai.data_type
from
	--
	(select '1' as data_type, :as_1_fm_ymd as fm_ymd,:as_1_to_ymd  as to_ymd from dual 
	union all
	select '2' as data_type, :as_2_fm_ymd as fm_ymd,:as_2_to_ymd  as to_ymd from dual 
	union all
	select '3' as data_type, :as_3_fm_ymd as fm_ymd,:as_3_to_ymd  as to_ymd from dual ) dt,
	--TXF,TXO交易量
	(select 
		'1' as data_type,
		sum(case when ai2_param_key = 'TXF' then ai2_m_qnty else 0 end) as f_m_qnty,
		sum(case when ai2_param_key = 'TXO' then ai2_m_qnty else 0 end) as o_m_qnty,
		count(distinct ai2_ymd) as cnt
	from ci.ai2
	where ai2_sum_type = 'D'
	and ai2_sum_subtype = '3'
	and ai2_param_key in ('TXF','TXO')
	and ai2_ymd >= :as_1_fm_ymd
	and ai2_ymd <= :as_1_to_ymd
	union all      
	select 
		'2',
		sum(case when ai2_param_key = 'TXF' then ai2_m_qnty else 0 end) as f_m_qnty,
		sum(case when ai2_param_key = 'TXO' then ai2_m_qnty else 0 end) as o_m_qnty,
		count(distinct ai2_ymd) as cnt
	from ci.ai2
	where ai2_sum_type = 'D'
	and ai2_sum_subtype = '3'
	and ai2_param_key in ('TXF','TXO')
	and ai2_ymd >= :as_2_fm_ymd
	and ai2_ymd <= :as_2_to_ymd
	union all      
	select 
		'3',
		sum(case when ai2_param_key = 'TXF' then ai2_m_qnty else 0 end) as f_m_qnty,
		sum(case when ai2_param_key = 'TXO' then ai2_m_qnty else 0 end) as o_m_qnty,
		count(distinct ai2_ymd) as cnt
	from ci.ai2
	where ai2_sum_type = 'D'
	and ai2_sum_subtype = '3'
	and ai2_param_key in ('TXF','TXO')
	and ai2_ymd >= :as_3_fm_ymd
	and ai2_ymd <= :as_3_to_ymd) ai,
	--EUREX交易量及OI    
	(select 
		'1' as data_type,
		sum(case when ae2_param_key = 'TXF' then ae2_m_qnty else 0 end) as f_m_qnty,
		sum(case when ae2_param_key = 'TXF' then ae2_oi else 0 end) as f_oi,
		sum(case when ae2_param_key = 'TXO' then ae2_m_qnty else 0 end) as o_m_qnty,
		sum(case when ae2_param_key = 'TXO' then ae2_oi else 0 end) as o_oi,
		count(distinct ae2_ymd) as cnt
	from ci.ae2
	where ae2_sum_type = 'D'
	and ae2_sum_subtype = '3'
	and ae2_param_key in ('TXF','TXO')
	and ae2_ymd >= :as_1_fm_ymd
	and ae2_ymd <= :as_1_to_ymd
	union all      
	select 
		'2',
		sum(case when ae2_param_key = 'TXF' then ae2_m_qnty else 0 end) as f_m_qnty,
		sum(case when ae2_param_key = 'TXF' then ae2_oi else 0 end) as f_oi,
		sum(case when ae2_param_key = 'TXO' then ae2_m_qnty else 0 end) as o_m_qnty,
		sum(case when ae2_param_key = 'TXO' then ae2_oi else 0 end) as o_oi,
		count(distinct ae2_ymd) as cnt
	from ci.ae2
	where ae2_sum_type = 'D'
	and ae2_sum_subtype = '3'
	and ae2_param_key in ('TXF','TXO')
	and ae2_ymd >= :as_2_fm_ymd
	and ae2_ymd <= :as_2_to_ymd
	union all      
	select 
		'3',
		sum(case when ae2_param_key = 'TXF' then ae2_m_qnty else 0 end) as f_m_qnty,
		sum(case when ae2_param_key = 'TXF' then ae2_oi else 0 end) as f_oi,
		sum(case when ae2_param_key = 'TXO' then ae2_m_qnty else 0 end) as o_m_qnty,
		sum(case when ae2_param_key = 'TXO' then ae2_oi else 0 end) as o_oi,
		count(distinct ae2_ymd) as cnt
	from ci.ae2
	where ae2_sum_type = 'D'
	and ae2_sum_subtype = '3'
	and ae2_param_key in ('TXF','TXO')
	and ae2_ymd >= :as_3_fm_ymd
	and ae2_ymd <= :as_3_to_ymd) ae,
	--TX波動度
	(select 
		'1' as data_type,round(stddev(ai6_ln_return) * 16,6) as ai6_rate,
		sum(ai6_high_low) as tot_high_low,count(distinct ai6_date) as cnt
	from ci.ai6
	where ai6_date >= to_date(:as_1_fm_ymd,'YYYYMMDD')
	and ai6_date <= to_date(:as_1_to_ymd,'YYYYMMDD')
	and ai6_kind_id = 'TXF'
	union all   
	select 
		'2' as data_type,round(stddev(ai6_ln_return) * 16,6) as ai6_rate,
		sum(ai6_high_low) as tot_high_low,count(distinct ai6_date) as cnt
	from ci.ai6
	where ai6_date >= to_date(:as_2_fm_ymd,'YYYYMMDD')
	and ai6_date <= to_date(:as_2_to_ymd,'YYYYMMDD')
	and ai6_kind_id = 'TXF'
	union all   
	select 
		'3' as data_type,round(stddev(ai6_ln_return) * 16,6) as ai6_rate,
		sum(ai6_high_low) as tot_high_low,count(distinct ai6_date) as cnt
	from ci.ai6
	where ai6_date >= to_date(:as_3_fm_ymd,'YYYYMMDD')
	and ai6_date <= to_date(:as_3_to_ymd,'YYYYMMDD')
	and ai6_kind_id = 'TXF') t,
	--TX夜盤波動度
	(select 
		'1' as data_type,sum(ai6_high_low) as tot_high_low,count(distinct ai6_date) as cnt
	from ci.ai6ah
	where ai6_date >= to_date(:as_1_fm_ymd,'YYYYMMDD')
	and ai6_date <= to_date(:as_1_to_ymd,'YYYYMMDD')
	and ai6_kind_id = 'TXF'
	union all   
	select 
		'2' as data_type,sum(ai6_high_low) as tot_high_low,count(distinct ai6_date) as cnt
	from ci.ai6ah
	where ai6_date >= to_date(:as_2_fm_ymd,'YYYYMMDD')
	and ai6_date <= to_date(:as_2_to_ymd,'YYYYMMDD')
	and ai6_kind_id = 'TXF'
	union all   
	select 
		'3' as data_type,sum(ai6_high_low) as tot_high_low,count(distinct ai6_date) as cnt
	from ci.ai6ah
	where ai6_date >= to_date(:as_3_fm_ymd,'YYYYMMDD')
	and ai6_date <= to_date(:as_3_to_ymd,'YYYYMMDD')
	and ai6_kind_id = 'TXF') ah,
	--EUREX-FTX波動度
	(select 
		'1' as data_type,round(stddev(ae6_ln_return) * 16,6) as ae6_rate,
		sum(ae6_high_low) as tot_high_low,count(distinct ae6_ymd) as cnt
	from ci.ae6
	where ae6_ymd >= :as_1_fm_ymd
	and ae6_ymd <= :as_1_to_ymd
	and  not ae6_ln_return is null
	and ae6_kind_id = 'TXF'
	union all   
	select 
		'2' as data_type,round(stddev(ae6_ln_return) * 16,6) as ae6_rate,
		sum(ae6_high_low) as tot_high_low,count(distinct ae6_ymd) as cnt
	from ci.ae6
	where ae6_ymd >= :as_2_fm_ymd
	and ae6_ymd <= :as_2_to_ymd
	and  not ae6_ln_return is null
	and ae6_kind_id = 'TXF'
	union all   
	select 
		'3' as data_type,round(stddev(ae6_ln_return) * 16,6) as ae6_rate,
		sum(ae6_high_low) as tot_high_low,count(distinct ae6_ymd) as cnt
	from ci.ae6
	where ae6_ymd >= :as_3_fm_ymd
	and ae6_ymd <= :as_3_to_ymd
	and  not ae6_ln_return is null
	and ae6_kind_id = 'TXF') e
where ai.data_type = ae.data_type
and ai.data_type = t.data_type
and ai.data_type = ah.data_type(+)
and ai.data_type = e.data_type
and ai.data_type = dt.data_type
order by data_type
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
