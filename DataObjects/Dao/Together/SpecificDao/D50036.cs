using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
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

      public DataTable List50036(D500xx d500Xx)
      {
         object[] parms = {
                ":as_symd",d500Xx.Sdate,
                ":as_eymd",d500Xx.Edate,
                ":as_sum_type",d500Xx.SumType,
                ":as_sum_subtype",d500Xx.SumSubType,
                ":Sdate",d500Xx.Sdate,
                ":Edate",d500Xx.Edate,
                ":Sbrkno",d500Xx.Sbrkno,
                ":Ebrkno",d500Xx.Ebrkno,
                ":ProdCategory",d500Xx.ProdCategory,
                ":ProdKindIdSto",d500Xx.ProdKindIdSto,
                ":ProdKindId",d500Xx.ProdKindId
            };
         string iswhere = d500Xx.ConditionWhereSyntax();
         string sql = string.Format(@" 
SELECT ROWNUM as CP_ROW,main.*
FROM
(SELECT AMM0_YMD,   
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
              then ceil(nvl(trunc(amm0_keep_time,0) / 60/ NULLIF(amm0_day_count,0),0)) 
              else ceil(nvl(amm0_keep_time / 60 / NULLIF(amm0_day_count,0),0)) end as AMM0_KEEP_TIME,
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
     {0}
   ORDER BY AMM0_YMD , AMM0_BRK_NO , AMM0_ACC_NO , AMM0_PROD_TYPE , AMM0_PROD_ID  ) main
", iswhere);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable List50036

      public DataTable ListAH(D500xx d500Xx)
      {
         object[] parms = {
                ":as_symd",d500Xx.Sdate,
                ":as_eymd",d500Xx.Edate,
                ":as_sum_type",d500Xx.SumType,
                ":as_sum_subtype",d500Xx.SumSubType,
                ":Sdate",d500Xx.Sdate,
                ":Edate",d500Xx.Edate,
                ":Sbrkno",d500Xx.Sbrkno,
                ":Ebrkno",d500Xx.Ebrkno,
                ":ProdCategory",d500Xx.ProdCategory,
                ":ProdKindIdSto",d500Xx.ProdKindIdSto,
                ":ProdKindId",d500Xx.ProdKindId
            };
         string iswhere = d500Xx.ConditionWhereSyntax();
         string sql = string.Format(@"
 SELECT ROWNUM as CP_ROW,main.*
FROM
(SELECT AMM0_YMD,   
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
              then ceil(nvl(trunc(amm0_keep_time,0) / 60/ NULLIF(amm0_day_count,0),0))
              else ceil(nvl(amm0_keep_time / 60 / NULLIF(amm0_day_count,0),0)) end as AMM0_KEEP_TIME,
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
     {0}
ORDER BY AMM0_YMD , AMM0_BRK_NO , AMM0_ACC_NO , AMM0_PROD_TYPE , AMM0_PROD_ID) main
", iswhere);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable List50036AH

   }
}
