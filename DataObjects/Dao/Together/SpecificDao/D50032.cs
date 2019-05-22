using OnePiece;
using System.Data;
using System.Data.Common;


namespace DataObjects.Dao.Together.SpecificDao
{
   public class D50032: DataGate
   {

      public DataTable List50032(D500xx d500Xx)
      {
         object[] parms = {
                ":as_fcm_no_s",d500Xx.Sbrkno,
                ":as_fcm_no_e",d500Xx.Ebrkno,
                ":as_param_key",d500Xx.ProdCategory,
                ":as_kind_id2",d500Xx.ProdKindIdSto,
                ":as_symd",d500Xx.Sdate,
                ":as_eymd",d500Xx.Edate
            };
         string sql = @"
SELECT ROWNUM as CP_ROW,main.*
FROM
(SELECT AMM0_YMD,   
         AMM0_BRK_NO,AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME, 
         AMM0_PROD_TYPE,
         case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end as AMM0_PROD_ID ,
         AMM0_CNT,   
         AMM0_VALID_CNT,   
         AMM0_OM_QNTY,   
         AMM0_QM_QNTY,  
         AMM0_MARKET_R_CNT, 
         AMM0_MARKET_M_QNTY,
         AMM0_KEEP_FLAG,
         NVL(AMMF_RESP_RATIO,99999) as MMF_RESP_RATIO,
         NVL(AMMF_QNTY_LOW,0) as MMF_QNTY_LOW,
         LEAST(nvl(AMM0_MM_QNTY,0),nvl(AMM0_MAX_MM_QNTY,0)) AS QNTY,
         (AMM0_OM_QNTY + AMM0_QM_QNTY ) as CP_M_QNTY,
         AMM0_RQ_RATE as VALID_RATE,
         decode(AMM0_MARKET_M_QNTY,0,0,round((AMM0_OM_QNTY +  AMM0_QM_QNTY )/AMM0_MARKET_M_QNTY,2) * 100) as CP_RATE_M,
         case when AMMF_AVG_TIME = 0 then 'Y' else AMM0_KEEP_FLAG end as KEEP_FLAG,
         AMMF_RFC_MIN_CNT as MMF_RFC_MIN_CNT
    FROM ci.AMM0,ci.AMMF  
   WHERE AMM0_SUM_TYPE = 'M'  AND  
         AMM0_SUM_SUBTYPE = '4'   AND  
         AMM0_DATA_TYPE = 'Q' AND
         AMM0_YMD >= :as_symd AND
         AMM0_YMD <= :as_eymd AND
         AMM0_BRK_NO >= :as_fcm_no_s AND
         AMM0_BRK_NO <= :as_fcm_no_e AND
         AMM0_PARAM_KEY LIKE :as_param_key AND
         AMM0_KIND_ID2 LIKE :as_kind_id2
     and AMM0_PARAM_KEY = AMMF_PARAM_KEY (+)
     and substr(AMM0_YMD,1,6) = AMMF_YM
     and AMMF_MARKET_CODE = '0'
ORDER BY AMM0_BRK_NO,BRK_ABBR_NAME,AMM0_ACC_NO,AMM0_PROD_TYPE,AMM0_PROD_ID,AMM0_YMD) main
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable List50032

      public DataTable ListChk(string is_sdate, string is_edate)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate
            };
         string sql = @"
         SELECT AMM0_YMD,AMM0_BRK_NO,AMM0_PROD_TYPE,AMM0_PROD_ID,
       MAX(CHK_FLAG) AS CHK_FLAG
  FROM
 (SELECT AMM0_YMD,AMM0_BRK_NO,AMM0_ACC_NO,
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID ,
         ammf_qnty_low  as mmf_qnty_low,
         ammf_resp_ratio as mmf_resp_ratio,
         case when LEAST(nvl(AMM0_MM_QNTY,0),nvl(AMM0_MAX_MM_QNTY,0)) < ammf_qnty_low  
                   or
                  AMM0_RQ_RATE < ammf_resp_ratio 
                   or
                   case when ammf_avg_time = 0 then 'Y' else amm0_keep_flag end  <> 'Y' 
              then 1 else 0 end CHK_FLAG
    FROM ci.AMM0,ci.AMMF  
   WHERE AMM0_SUM_TYPE = 'M'  AND  
         AMM0_SUM_SUBTYPE = '4'   AND  
         AMM0_DATA_TYPE = 'Q' AND
         AMM0_YMD >= :as_symd AND
         AMM0_YMD <= :as_eymd 
     and AMM0_PARAM_KEY = AMMF_PARAM_KEY (+)
     and AMM0_YMD = AMMF_YM(+)||'  '
     and '0' = AMMF_MARKET_CODE(+))  
 GROUP BY AMM0_YMD,AMM0_BRK_NO,AMM0_PROD_TYPE,AMM0_PROD_ID
 ORDER BY AMM0_PROD_TYPE,AMM0_PROD_ID,AMM0_BRK_NO,AMM0_YMD
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListChk

   }
}