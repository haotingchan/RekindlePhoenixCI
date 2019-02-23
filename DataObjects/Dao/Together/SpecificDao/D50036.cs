using OnePiece;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D50036
   {
      private Db db;

      public D50036()
      {
         db = GlobalDaoSetting.DB;
      }

      public DbDataAdapter ListAMMOAH(string is_sdate, string is_edate, string is_sum_type, string is_sum_subtype)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype
            };
         string sql = @"
 SELECT AMM0_YMD,   
         AMM0_BRK_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_ACC_NO,
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         LEAST(nvl(amm0_mm_qnty,0),nvl(amm0_max_mm_qnty,0)) as AMM0_MMK_QNTY,
         amm0_om_qnty + amm0_qm_qnty + amm0_iqm_qnty as AMM0_TOT_QNTY,
        AMM0_RQ_RATE as AMM0_MMK_RATE,         
         case when amm0_sum_type = 'D' 
              then ceil(trunc(amm0_keep_time,0) / 60/ amm0_day_count) 
              else ceil(amm0_keep_time / 60 / amm0_day_count) end as AMM0_KEEP_TIME,
         round(AMM0_VALID_RESULT,1) AS AMM0_RESULT,
         AMM0_CONTRACT_CNT
    FROM ci.AMM0AH,ci.AMMF
   WHERE AMM0_DATA_TYPE = 'Q'
     AND AMM0_YMD >= :as_symd
     AND AMM0_YMD <= :as_eymd
     AND AMM0_SUM_TYPE = :as_sum_type 
     AND AMM0_SUM_SUBTYPE = :as_sum_subtype  
     and AMM0_PARAM_KEY = AMMF_PARAM_KEY (+)
     and substr(AMM0_YMD,1,6) = AMMF_YM (+)
     and '0' = AMMF_MARKET_CODE(+)
ORDER BY amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id 
";
         DbDataAdapter adapter = db.GetDataAdapter(sql, parms);
         return adapter;
      }//public DbDataAdapter ListAMMOAH
      public DbDataAdapter ListAMMO(string is_sdate, string is_edate, string is_sum_type, string is_sum_subtype)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype
            };
         string sql = @" 
SELECT AMM0_YMD,   
         AMM0_BRK_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_ACC_NO,
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         LEAST(nvl(amm0_mm_qnty,0),nvl(amm0_max_mm_qnty,0)) as AMM0_MMK_QNTY,
         amm0_om_qnty + amm0_qm_qnty + amm0_iqm_qnty as AMM0_TOT_QNTY,
        AMM0_RQ_RATE as AMM0_MMK_RATE,         
         case when amm0_sum_type = 'D' 
              then ceil(trunc(amm0_keep_time,0) / 60/ amm0_day_count) 
              else ceil(amm0_keep_time / 60 / amm0_day_count) end as AMM0_KEEP_TIME,
         round(AMM0_VALID_RESULT,1) AS AMM0_RESULT,
         AMM0_CONTRACT_CNT
    FROM ci.AMM0,ci.AMMF
   WHERE AMM0_DATA_TYPE = 'Q'
     AND AMM0_YMD >= :as_symd
     AND AMM0_YMD <= :as_eymd
     AND AMM0_SUM_TYPE = :as_sum_type 
     AND AMM0_SUM_SUBTYPE = :as_sum_subtype  
     and AMM0_PARAM_KEY = AMMF_PARAM_KEY (+)
     and substr(AMM0_YMD,1,6) = AMMF_YM (+)
     and '0' = AMMF_MARKET_CODE(+)
   ORDER BY amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id 
";
         DbDataAdapter adapter = db.GetDataAdapter(sql, parms);
         return adapter;
      }//public DbDataAdapter ListAMMO
   }
}
