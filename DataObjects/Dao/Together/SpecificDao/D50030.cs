using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D50030:DataGate
   {
      public DataTable ListD50030(string is_sum_type, string is_sum_subtype,string is_data_type)
      {
         object[] parms = {
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype,
                ":as_data_type",is_data_type
            };
         string sql = @"
 SELECT AMM0_YMD,   
         AMM0_BRK_NO,
         AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         AMM0_CNT,   
         AMM0_VALID_CNT,   
         AMM0_OM_QNTY,   
         AMM0_QM_QNTY,  
         AMM0_MARKET_R_CNT, 
         AMM0_MARKET_M_QNTY,
         NVL(AMMF_QUOTE_VALID_RATE,0) as MMF_QUOTE_VALID_RATE,
         NVL(AMMF_RESP_RATIO,99999) as MMF_RESP_RATIO,
         NVL(AMMF_QNTY_LOW,0) as MMF_QNTY_LOW,
         0 as TOT_R,
         0 as TOT_M,
         AMM0_KEEP_TIME,
         AMM0_KEEP_FLAG,
         AMM0_SUM_TYPE,
         AMM0_SUM_SUBTYPE, NVL(AMMF_AVG_TIME,0) as MMF_AVG_TIME,
         AMM0_O_SUBTRACT_QNTY,
         AMM0_Q_SUBTRACT_QNTY,
         nvl(AMM0_IQM_QNTY,0) as AMM0_IQM_QNTY,
         nvl(AMM0_IQM_SUBTRACT_QNTY,0) as AMM0_IQM_SUBTRACT_QNTY,
         AMM0_BASIC_PROD,
         AMM0_PROD_TYPE,
         AMM0_DAY_COUNT,
         LEAST(nvl(AMM0_MM_QNTY,0),nvl(AMM0_MAX_MM_QNTY,0)) as mmk_qnty,
         AMM0_TRD_INVALID_QNTY,
         AMM0_BTRADE_M_QNTY,
         AMM0_RQ_RATE,
         AMMF_RFC_MIN_CNT as MMF_RFC_MIN_CNT,
         AMMF_CP_KIND,
         AMM0_DAY_COUNT
    FROM ci.AMM0,ci.AMMF
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         AMM0_DATA_TYPE = :as_data_type 
     and AMM0_PARAM_KEY = AMMF_PARAM_KEY (+)
     and substr(AMM0_YMD,1,6) = AMMF_YM (+)
     and '0' = AMMF_MARKET_CODE(+)
ORDER BY AMM0_BRK_NO,AMM0_ACC_NO,AMM0_PROD_ID
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListD50030

      public DataTable ListACCU(string is_symd,string is_eymd, string is_sum_type, string is_sum_subtype, string is_data_type)
      {
         object[] parms = {
            ":as_symd",is_symd,
            ":as_eymd",is_eymd,
            ":as_sum_type",is_sum_type,
            ":as_sum_subtype",is_sum_subtype,
            ":as_data_type",is_data_type
            };
         string sql = @"
 SELECT min(AMM0_YMD) ||'-'|| max(AMM0_YMD) as AMM0_YMD,   
         AMM0_BRK_NO,
         AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         sum(AMM0_CNT) as AMM0_CNT,   
         sum(AMM0_VALID_CNT) as AMM0_VALID_CNT,   
         sum(AMM0_OM_QNTY) as AMM0_OM_QNTY,   
         sum(AMM0_QM_QNTY) as AMM0_QM_QNTY,  
			(select nvl(sum(R.AMM0_CNT),0)
            from ci.AMM0 R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = 'R'
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd) as AMM0_MARKET_R_CNT,   
			(select nvl(sum(R.AMM0_MARKET_M_QNTY),0)
            from ci.AMM0 R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = 'M'
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd) as AMM0_MARKET_M_QNTY, 
         NVL(sum(AMM0_KEEP_TIME),0) as AMM0_KEEP_TIME,
         NVL(sum(AMM0_O_SUBTRACT_QNTY),0) as AMM0_O_SUBTRACT_QNTY,
         NVL(sum(AMM0_Q_SUBTRACT_QNTY),0) as AMM0_Q_SUBTRACT_QNTY,
         NVL(sum(AMM0_IQM_QNTY),0) as AMM0_IQM_QNTY,
         NVL(sum(AMM0_IQM_SUBTRACT_QNTY),0) as AMM0_IQM_SUBTRACT_QNTY,
         NVL(sum(AMM0_BTRADE_M_QNTY),0) as AMM0_BTRADE_M_QNTY,
         CAST('' AS NUMBER(8,0)) AS AMM0_TRD_INVALID_QNTY,
         COUNT(DISTINCT (AMM0_YMD)) AS AMM0_DAY_COUNT
    FROM ci.AMM0  A
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         AMM0_DATA_TYPE = :as_data_type 
 GROUP BY A.AMM0_BRK_NO,
          A.AMM0_ACC_NO,
          A.AMM0_SUM_TYPE,
          A.AMM0_SUM_SUBTYPE,
          A.AMM0_DATA_TYPE,
          A.AMM0_PROD_TYPE,
          A.AMM0_PROD_SUBTYPE,
          AMM0_PARAM_KEY,
          A.AMM0_KIND_ID2,
          A.AMM0_KIND_ID,
          A.AMM0_PROD_ID
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListD50030Accu

      public DataTable ListACCUAH(string is_symd, string is_eymd, string is_sum_type, string is_sum_subtype, string is_data_type)
      {
         object[] parms = {
            ":as_symd",is_symd,
            ":as_eymd",is_eymd,
            ":as_sum_type",is_sum_type,
             ":as_sum_subtype",is_sum_subtype,
            ":as_data_type",is_data_type
            };
         string sql = @"
SELECT min(AMM0_YMD) ||'-'|| max(AMM0_YMD) as AMM0_YMD,   
         AMM0_BRK_NO,
         AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         sum(AMM0_CNT) as AMM0_CNT,   
         sum(AMM0_VALID_CNT) as AMM0_VALID_CNT,   
         sum(AMM0_OM_QNTY) as AMM0_OM_QNTY,   
         sum(AMM0_QM_QNTY) as AMM0_QM_QNTY,  
			(select nvl(sum(R.AMM0_CNT),0)
            from ci.AMM0AH R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = 'R'
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd) as AMM0_MARKET_R_CNT,   
			(select nvl(sum(R.AMM0_MARKET_M_QNTY),0)
            from ci.AMM0AH R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = 'M'
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd) as AMM0_MARKET_M_QNTY, 
         NVL(sum(AMM0_KEEP_TIME),0) as AMM0_KEEP_TIME,
         NVL(sum(AMM0_O_SUBTRACT_QNTY),0) as AMM0_O_SUBTRACT_QNTY,
         NVL(sum(AMM0_Q_SUBTRACT_QNTY),0) as AMM0_Q_SUBTRACT_QNTY,
         NVL(sum(AMM0_IQM_QNTY),0) as AMM0_IQM_QNTY,
         NVL(sum(AMM0_IQM_SUBTRACT_QNTY),0) as AMM0_IQM_SUBTRACT_QNTY,
         NVL(sum(AMM0_BTRADE_M_QNTY),0) as AMM0_BTRADE_M_QNTY,
         CAST('' AS NUMBER(8,0)) AS AMM0_TRD_INVALID_QNTY,
         COUNT(DISTINCT (AMM0_YMD)) AS AMM0_DAY_COUNT
    FROM ci.AMM0AH  A
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         AMM0_DATA_TYPE = :as_data_type 
 GROUP BY A.AMM0_BRK_NO,
          A.AMM0_ACC_NO,
          A.AMM0_SUM_TYPE,
          A.AMM0_SUM_SUBTYPE,
          A.AMM0_DATA_TYPE,
          A.AMM0_PROD_TYPE,
          A.AMM0_PROD_SUBTYPE,
          AMM0_PARAM_KEY,
          A.AMM0_KIND_ID2,
          A.AMM0_KIND_ID,
          A.AMM0_PROD_ID
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListD50030AccuAh

      public DataTable ListAH(string is_sum_type, string is_sum_subtype, string is_data_type)
      {
         object[] parms = {
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype,
                ":as_data_type",is_data_type
            };
         string sql = @"
 SELECT AMM0_YMD,   
         AMM0_BRK_NO,
         AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         AMM0_CNT,   
         AMM0_VALID_CNT,   
         AMM0_OM_QNTY,   
         AMM0_QM_QNTY,  
         AMM0_MARKET_R_CNT, 
         AMM0_MARKET_M_QNTY,
         NVL(AMMF_QUOTE_VALID_RATE,0) as MMF_QUOTE_VALID_RATE,
         NVL(AMMF_RESP_RATIO,99999) as MMF_RESP_RATIO,
         NVL(AMMF_QNTY_LOW,0) as MMF_QNTY_LOW,
         0 as TOT_R,
         0 as TOT_M,
         AMM0_KEEP_TIME,
         AMM0_KEEP_FLAG,
         AMM0_SUM_TYPE,
         AMM0_SUM_SUBTYPE, NVL(AMMF_AVG_TIME,0) as MMF_AVG_TIME,
         AMM0_O_SUBTRACT_QNTY,
         AMM0_Q_SUBTRACT_QNTY,
         nvl(AMM0_IQM_QNTY,0) as AMM0_IQM_QNTY,
         nvl(AMM0_IQM_SUBTRACT_QNTY,0) as AMM0_IQM_SUBTRACT_QNTY,
         AMM0_BASIC_PROD,
         AMM0_PROD_TYPE,
         AMM0_DAY_COUNT,
         LEAST(nvl(AMM0_MM_QNTY,0),nvl(AMM0_MAX_MM_QNTY,0)) as mmk_qnty,
         AMM0_TRD_INVALID_QNTY,
         AMM0_BTRADE_M_QNTY,
         AMM0_RQ_RATE,
         AMMF_RFC_MIN_CNT as MMF_RFC_MIN_CNT,
         AMMF_CP_KIND,
         AMM0_DAY_COUNT
    FROM ci.AMM0AH,ci.AMMF
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         AMM0_DATA_TYPE = :as_data_type 
     and AMM0_PARAM_KEY = AMMF_PARAM_KEY (+)
     and substr(AMM0_YMD,1,6) = AMMF_YM (+)
     and '1' = AMMF_MARKET_CODE(+)
ORDER BY AMM0_BRK_NO,AMM0_ACC_NO,AMM0_PROD_ID
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListD50030Ah

   }
}
