using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/6
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D30017 : DataGate {

      public DataTable check20110(string ls_ymd) {

         object[] parms = {
                ":ls_ymd", ls_ymd
            };

         string sql =
@"
select nvl(sum(case when AI8_DATA_SOURCE = 'U' then 1 else 0 end),0) as li_tfxm_cnt,
           nvl(sum(case when AI8_DATA_SOURCE = 'T' and AI8_PROD_TYPE = 'F' then 1 else 0 end),0) as li_fut_cnt,
           nvl(sum(case when AI8_DATA_SOURCE = 'T' and AI8_PROD_TYPE = 'O' then 1 else 0 end),0) as li_opt_cnt 
  --into :li_tfxm_cnt,:li_fut_cnt,:li_opt_cnt
 from ci.AI8
where AI8_YMD = :ls_ymd
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      public DataTable d_30017(string as_ymd) {

         object[] parms = {
                ":as_ymd", as_ymd
            };

         string sql =
@"
select AI8_DATA_SOURCE,AI8_PROD_TYPE,AI8_PROD_SUBTYPE,AI8_PARAM_KEY,AI8_KIND_ID2,
        AI8_CLOSE_PRICE,AI8_UP_DOWN_VAL, 
        case when nvl(AI8_OPEN_REF,0) = 0 then null else ROUND(AI8_UP_DOWN_VAL/AI8_OPEN_REF,4)*100 end as UP_DOWN_RATE,AI8_QNTY,
        case when nvl(AI8_YEAR_DAYS,0) = 0 then null else ROUND(AI8_YEAR_QNTY / AI8_YEAR_DAYS,0) end as AVG_YEAR,                                                             
        case when nvl(AI8_LAST_YEAR_DAYS,0) = 0 then null else ROUND(AI8_LAST_YEAR_QNTY / AI8_LAST_YEAR_DAYS,0) end as AVG_LAST_YEAR ,                            
        case when nvl(AI8_LAST_ALL_YEAR_DAYS,0) = 0 then null else ROUND(AI8_LAST_ALL_YEAR_QNTY / AI8_LAST_ALL_YEAR_DAYS,0) end as AVG_ALL_LAST_YEAR  ,
        case when AI8_KIND_ID2 not in ('MXF','TXO') and (AI8_PARAM_KEY = AI8_KIND_ID2 or nvl(trim(AI8_KIND_ID2),' ') = ' ') then AM7T_AVG_QNTY else null end as AM7T_AVG_QNTY,
        rpt_seq_no,
        AI2_AH_M_QNTY as AI8_AH_QNTY,
        AI2_AH_DAY_COUNT as AI8_AH_MTH_DAYS
   from ci.AI8,
       --夜盤
       (SELECT AI2_PARAM_KEY,AI2_AH_M_QNTY,AI2_AH_DAY_COUNT
        FROM ci.ai2
        where AI2_YMD = :as_ymd
          AND AI2_AH_DAY_COUNT > 0
          AND AI2_SUM_TYPE = 'D'
          AND AI2_SUM_SUBTYPE = '3') A,
       (select AM7T_PROD_TYPE,AM7T_PROD_SUBTYPE,AM7T_PARAM_KEY,AM7T_AVG_QNTY
          from ci.AM7T where AM7T_Y = substr(:as_ymd,1,4)
         union all
        select AM7T_PROD_TYPE,' ','       ',sum(AM7T_AVG_QNTY)
          from ci.AM7T where AM7T_Y = substr(:as_ymd,1,4)
        group by AM7T_PROD_TYPE
         union all
        select ' ',' ','       ',sum(AM7T_AVG_QNTY)
          from ci.AM7T where AM7T_Y = substr(:as_ymd,1,4)) T ,
       (select rpt_value_4 as rpt_prod_type,rpt_value_3 as rpt_prod_subtype,rpt_value_2 as rpt_param_key,rpt_value as rpt_kind_id2,rpt_seq_no 
          from ci.rpt  where rpt_txn_id = '30017' and rpt_txd_id = '30017') R
 where AI8_YMD = :as_ymd
   and AI8_DATA_SOURCE = 'T'
   and AI8_PROD_TYPE = rpt_prod_type(+)
   and AI8_PROD_SUBTYPE = rpt_prod_subtype(+)
   and AI8_PARAM_KEY = rpt_param_key(+)
   and AI8_KIND_ID2 = rpt_kind_id2(+)
   and AI8_PROD_TYPE = AM7T_PROD_TYPE(+)
   and AI8_PROD_SUBTYPE = AM7T_PROD_SUBTYPE(+)
   and AI8_PARAM_KEY = AM7T_PARAM_KEY(+)
   and AI8_PARAM_KEY = AI2_PARAM_KEY(+)
 order by rpt_seq_no
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      public DataTable d_30017_all(string as_ymd) {

         object[] parms = {
                ":as_ymd", as_ymd
            };

         string sql =
@"
select AI8_DATA_SOURCE,AI8_PROD_TYPE,AI8_PROD_SUBTYPE,AI8_PARAM_KEY,AI8_KIND_ID2,
          AI8_CLOSE_PRICE,AI8_UP_DOWN_VAL, 
          case when nvl(AI8_OPEN_REF,0) = 0 then null else ROUND(AI8_UP_DOWN_VAL/AI8_OPEN_REF,4)*100 end as UP_DOWN_RATE,AI8_QNTY,
          case when nvl(AI8_YEAR_DAYS,0) = 0 then null else ROUND(AI8_YEAR_QNTY / AI8_YEAR_DAYS,0) end as AVG_YEAR,                                                             
          case when nvl(AI8_LAST_YEAR_DAYS,0) = 0 then null else ROUND(AI8_LAST_YEAR_QNTY / AI8_LAST_YEAR_DAYS,0) end as AVG_LAST_YEAR ,                            
          case when nvl(AI8_LAST_ALL_YEAR_DAYS,0) = 0 then null else ROUND(AI8_LAST_ALL_YEAR_QNTY / AI8_LAST_ALL_YEAR_DAYS,0) end as AVG_ALL_LAST_YEAR  ,
          case when AI8_KIND_ID2 not in ('MXF','TXO') and (AI8_PARAM_KEY = AI8_KIND_ID2 or nvl(trim(AI8_KIND_ID2),' ') = ' ') then AM7T_AVG_QNTY else null end as AM7T_AVG_QNTY,
          rpt_seq_no ,
          AI8_OI,
          AI8_OI_COMPARE,
          AI8_OI_MAX_YMD AS YMD_OI,
          AI8_MAX_YMD   AS YMD_QNTY,
          AI8_AH_QNTY,AI8_AH_MTH_DAYS
from ci.AI8A,
       (select AM7T_PROD_TYPE,AM7T_PROD_SUBTYPE,AM7T_PARAM_KEY,AM7T_AVG_QNTY
          from ci.AM7T where AM7T_Y = substr(:as_ymd,1,4)
         union all
        select AM7T_PROD_TYPE,' ','       ',sum(AM7T_AVG_QNTY)
          from ci.AM7T where AM7T_Y = substr(:as_ymd,1,4)
        group by AM7T_PROD_TYPE
         union all
        select ' ',' ','       ',sum(AM7T_AVG_QNTY)
          from ci.AM7T where AM7T_Y = substr(:as_ymd,1,4)) ,
       (select rpt_value_4 as rpt_prod_type,rpt_value_3 as rpt_prod_subtype,rpt_value_2 as rpt_param_key,rpt_value as rpt_kind_id2,rpt_seq_no 
          from ci.rpt  where rpt_txn_id = '30017' and rpt_txd_id = '30017')
where AI8_YMD = :as_ymd
   and AI8_DATA_SOURCE = 'T'
   and AI8_PROD_TYPE = rpt_prod_type(+)
   and AI8_PROD_SUBTYPE = rpt_prod_subtype(+)
   and AI8_PARAM_KEY = rpt_param_key(+)
   and AI8_KIND_ID2 = rpt_kind_id2(+)
   and AI8_PROD_TYPE = AM7T_PROD_TYPE(+)
   and AI8_PROD_SUBTYPE = AM7T_PROD_SUBTYPE(+)
   and AI8_PARAM_KEY = AM7T_PARAM_KEY(+)
order by rpt_seq_no
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      public DataTable d_30017_up(string as_ymd) {

         object[] parms = {
                ":as_ymd", as_ymd
            };

         string sql =
@"
select AI8_DATA_SOURCE,AI8_PARAM_KEY,AI8_KIND_ID2,
          AI8_CLOSE_PRICE,AI8_UP_DOWN_VAL, 
          case when nvl(AI8_OPEN_REF,0) = 0 then null else ROUND(AI8_UP_DOWN_VAL/AI8_OPEN_REF,4)*100 end as UP_DOWN_RATE,AI8_QNTY,
          case when nvl(AI8_MTH_DAYS,0) = 0 then null else ROUND(AI8_MTH_QNTY / AI8_MTH_DAYS,(case when AI8_DATA_SOURCE = 'U' then 2 else 0 end)) end as AVG_MTH,                                                      
          case when nvl(AI8_YEAR_DAYS,0) = 0 then null else ROUND(AI8_YEAR_QNTY / AI8_YEAR_DAYS,(case when AI8_DATA_SOURCE= 'U' then 2 else 0 end)) end as AVG_YEAR,                                                             
          case when nvl(AI8_LAST_YEAR_DAYS,0) = 0 then null else ROUND(AI8_LAST_YEAR_QNTY / AI8_LAST_YEAR_DAYS,(case when AI8_DATA_SOURCE = 'U' then 2 else 0 end)) end as AVG_LAST_YEAR ,                               
          case when nvl(AI8_LAST_ALL_YEAR_DAYS,0) = 0 then null else ROUND(AI8_LAST_ALL_YEAR_QNTY / AI8_LAST_ALL_YEAR_DAYS,(case when AI8_DATA_SOURCE = 'U' then 2 else 0 end)) end as AVG_LAST_ALL_YEAR ,   
          rpt_seq_no    
from ci.AI8,
       (select rpt_value_4 as rpt_prod_type,rpt_value_3 as rpt_prod_subtype,rpt_value_2 as rpt_param_key,rpt_value as rpt_kind_id2,rpt_seq_no 
          from ci.rpt  where rpt_txn_id = '30017' and rpt_txd_id = '30017up' and rpt_value_4 = '- ')
where AI8_YMD = :as_ymd
   and (AI8_DATA_SOURCE = 'U' or (AI8_DATA_SOURCE = 'T' and AI8_PROD_TYPE= ' '))
   --and AI8_PROD_TYPE = rpt_prod_type(+)
   --and AI8_PROD_SUBTYPE = rpt_prod_subtype(+)
   and AI8_PARAM_KEY = rpt_param_key(+)
   --and AI8_KIND_ID2 = rpt_kind_id2(+)
order by rpt_seq_no
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      public decimal? txfAvgQnty(string ls_ymd) {

         object[] parms = {
                ":ls_ymd", ls_ymd
            };

         string sql =
@"
select AM7T_TFXM_YEAR_AVG_QNTY --into :ld_val 
  from ci.AM7T
 where AM7T_Y = :ls_ymd
  and AM7T_PARAM_KEY = 'TXF'
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         decimal? rtn = string.IsNullOrEmpty(dtResult.Rows[0]["AM7T_TFXM_YEAR_AVG_QNTY"].AsString()) ? null : (decimal?)dtResult.Rows[0]["AM7T_TFXM_YEAR_AVG_QNTY"].AsDecimal();

         return rtn;
      }

      public decimal? gtfAvgQnty(string ls_ymd) {

         object[] parms = {
                ":ls_ymd", ls_ymd
            };

         string sql =
@"
select AM7T_TFXM_YEAR_AVG_QNTY --into :ld_val 
  from ci.AM7T
 where AM7T_Y = :ls_ymd
  and AM7T_PARAM_KEY = 'GTF'
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         decimal? rtn = string.IsNullOrEmpty(dtResult.Rows[0]["AM7T_TFXM_YEAR_AVG_QNTY"].AsString()) ? null : (decimal?)dtResult.Rows[0]["AM7T_TFXM_YEAR_AVG_QNTY"].AsDecimal();

         return rtn;
      }

      public DataTable d_30017down_all(string as_ymd) {

         object[] parms = {
                ":as_ymd", as_ymd
            };

         string sql =
@"
select AI8_PROD_TYPE,
         ROUND(AI8_MTH_QNTY / AI8_MTH_DAYS,0) as AVG_MTH,
         AI8_MTH_QNTY,AI8_MTH_DAYS,
         ROUND(AI8_LAST_ALL_MTH_QNTY / AI8_LAST_ALL_MTH_DAYS,0) as AVG_LAST_ALL_MTH,
         AI8_LAST_ALL_MTH_QNTY,AI8_LAST_ALL_MTH_DAYS,
         ROUND(AI8_YEAR_QNTY / AI8_YEAR_DAYS,0) as AVG_YEAR,                 
         AI8_YEAR_QNTY,AI8_YEAR_DAYS,                                            
         ROUND(AI8_LAST_YEAR_QNTY / AI8_LAST_YEAR_DAYS,0) as AVG_LAST_YEAR ,                      
         AI8_LAST_YEAR_QNTY,AI8_LAST_YEAR_DAYS,      
         ROUND(AI8_LAST_ALL_YEAR_QNTY / AI8_LAST_ALL_YEAR_DAYS,0) as AVG_LAST_ALL_YEAR,
         AI8_LAST_ALL_YEAR_QNTY,AI8_LAST_ALL_YEAR_DAYS,
         ROUND(AI8_YEAR_OI / AI8_YEAR_DAYS,0) as AVG_YEAR_OI,   
         ai8_ah_year_qnty,
         ROUND(ai8_ah_year_qnty / AI8_AH_YEAR_DAYS,0) as AVG_YEAR_AH,        
         ai8_ah_mth_qnty,
         ROUND(ai8_ah_mth_qnty / AI8_AH_MTH_DAYS,0) as AVG_MTH_AH
from ci.AI8A                                                                                                              
where AI8_YMD = :as_ymd
    and AI8_DATA_SOURCE = 'T'  
    and AI8_PROD_SUBTYPE = ' '
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      public int am7tDayCount(string ls_str) {

         object[] parms = {
                ":ls_str", ls_str
            };

         string sql =
@"
SELECT AM7T_DAY_COUNT --into :li_days 
 FROM CI.AM7T
WHERE AM7T_Y = :ls_str
  AND AM7T_PARAM_KEY = 'TXO'
";
         DataTable dtResult = db.GetDataTable(sql , parms);
         if (dtResult.Rows.Count == 0) {
            return 0;
         } else {
            return dtResult.Rows[0]["AM7T_DAY_COUNT"].AsInt();
         }
      }

      public DataTable d_30017down(string as_ymd) {

         object[] parms = {
                ":as_ymd", as_ymd
            };

         string sql =
@"
select AI8_PROD_TYPE,
         ROUND(AI8_MTH_QNTY / AI8_MTH_DAYS,0) as AVG_MTH,
         AI8_MTH_QNTY,AI8_MTH_DAYS,
         ROUND(AI8_LAST_ALL_MTH_QNTY / AI8_LAST_ALL_MTH_DAYS,0) as AVG_LAST_ALL_MTH,
         AI8_LAST_ALL_MTH_QNTY,AI8_LAST_ALL_MTH_DAYS,
         ROUND(AI8_YEAR_QNTY / AI8_YEAR_DAYS,0) as AVG_YEAR,                 
         AI8_YEAR_QNTY,AI8_YEAR_DAYS,                                            
         ROUND(AI8_LAST_YEAR_QNTY / AI8_LAST_YEAR_DAYS,0) as AVG_LAST_YEAR ,                      
         AI8_LAST_YEAR_QNTY,AI8_LAST_YEAR_DAYS,      
         ROUND(AI8_LAST_ALL_YEAR_QNTY / AI8_LAST_ALL_YEAR_DAYS,0) as AVG_LAST_ALL_YEAR,
         AI8_LAST_ALL_YEAR_QNTY,AI8_LAST_ALL_YEAR_DAYS
from ci.AI8                                                                                                              
where AI8_YMD = :as_ymd
    and AI8_DATA_SOURCE = 'T'  
    and AI8_PROD_SUBTYPE = ' '
order by AI8_PROD_TYPE
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
