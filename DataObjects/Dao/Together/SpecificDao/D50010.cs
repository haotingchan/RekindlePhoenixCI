using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D50010 : DataGate
    {
      public DataTable GetData(string sortType, DateTime adtdate, string paramKey, string kindId2, string kindId, string prodId, string fcmRange) {
         object[] parms = {
                ":as_sort_type", sortType,
                ":adt_date", adtdate,
                ":as_ammd_param_key",paramKey,
                ":as_ammd_kind_id2",kindId2,
                ":as_ammd_kind_id",kindId,
                ":as_ammd_prod_id",prodId
            };

         string sql = string.Format(@"  SELECT AMMD_BRK_NO, 
            AMMD_ACC_NO, BRK_ABBR_NAME, 
        TRIM(AMMD_PROD_ID) as AMMD_PROD_ID,   
         AMMD_W_TIME,   
         AMMD_R_TIME,
         r_second,
         m_second, 
         cp_valid,
         AMMD_KEEP_TIME,
         AMMD_WEIGHT_P,
         AMMD_WEIGHT_Q,
         AMMD_WEIGHT_KIND,
         AMMD_RESULT,
         AMMD_OQ_CODE from (
 SELECT  case when :as_sort_type ='F' then ammd_brk_no || ammd_acc_no else ammd_prod_type || ammd_prod_id end as  cp_group1,  
         case when :as_sort_type ='F' then  ammd_prod_type || ammd_prod_id else ammd_brk_no|| ammd_acc_no end as cp_group2 ,
         ammd_brk_no,
         AMMD_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMMD_BRK_NO ) as BRK_ABBR_NAME,  
         AMMD_PROD_ID,   
         AMMD_W_TIME,   
         AMMD_R_TIME, 
         AMMD_M_TIME, 
         AMMD_PROD_TYPE,
         case when AMMD_R_TIME >= AMMD_W_TIME then 0 else round(fut.TIME_DIFF(AMMD_R_TIME,AMMD_W_TIME),0) end   as r_second,
         round(fut.TIME_DIFF(AMMD_W_TIME,AMMD_M_TIME),0)   as m_second,
         case when AMMD_VALID_CNT > 0 then 'Y' else '' end as cp_valid,
         AMMD_KEEP_TIME,
         AMMD_WEIGHT_P,
         AMMD_WEIGHT_Q,
         AMMD_WEIGHT_KIND,
         AMMD_RESULT,
         AMMD_OQ_CODE,
         AMMD_Q_NO  
    FROM ci.AMMD  
   WHERE AMMD_DATE = :adt_date  AND  
         AMMD_DATA_TYPE  IN ('Q'  ,'q')
         and ammd_PARAM_KEY like :as_ammd_PARAM_KEY
         and ammd_kind_id2 like  :as_ammd_kind_id2
         and ammd_KIND_ID like :as_ammd_KIND_ID
         --and ammd_PROD_ID like :as_ammd_PROD_ID
         order by cp_group1 , cp_group2 , ammd_w_time)", fcmRange);

         return db.GetDataTable(sql, parms);
      }

      public DataTable GetExportData(string sortType, DateTime adtdate, string paramKey, string kindId2, string kindId, string prodId, string fcmRange) {
         object[] parms = {
                ":as_sort_type", sortType,
                ":adt_date", adtdate,
                ":as_ammd_param_key",paramKey,
                ":as_ammd_kind_id2",kindId2,
                ":as_ammd_kind_id",kindId,
                ":as_ammd_prod_id",prodId
            };

         string sql = string.Format(@"  SELECT AMMD_BRK_NO, 
            AMMD_ACC_NO, BRK_ABBR_NAME, 
          AMMD_PROD_TYPE,
        TRIM(AMMD_PROD_ID) as AMMD_PROD_ID,   
         AMMD_W_TIME,   
         AMMD_R_TIME, 
         AMMD_M_TIME, 
         r_second,
         m_second, 
         cp_valid,
         AMMD_KEEP_TIME,
         AMMD_WEIGHT_P,
         AMMD_WEIGHT_Q,
         AMMD_WEIGHT_KIND,
         AMMD_RESULT,
         AMMD_OQ_CODE,
         AMMD_Q_NO from (
 SELECT  case when :as_sort_type ='F' then ammd_brk_no || ammd_acc_no else ammd_prod_type || ammd_prod_id end as  cp_group1,  
         case when :as_sort_type ='F' then  ammd_prod_type || ammd_prod_id else ammd_brk_no|| ammd_acc_no end as cp_group2 ,
         ammd_brk_no,
         AMMD_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMMD_BRK_NO ) as BRK_ABBR_NAME,  
         AMMD_PROD_ID,   
         AMMD_W_TIME,   
         AMMD_R_TIME, 
         AMMD_M_TIME, 
         AMMD_PROD_TYPE,
         case when AMMD_R_TIME >= AMMD_W_TIME then 0 else round(fut.TIME_DIFF(AMMD_R_TIME,AMMD_W_TIME),0) end   as r_second,
         round(fut.TIME_DIFF(AMMD_W_TIME,AMMD_M_TIME),0)   as m_second,
         case when AMMD_VALID_CNT > 0 then 'Y' else '' end as cp_valid,
         AMMD_KEEP_TIME,
         AMMD_WEIGHT_P,
         AMMD_WEIGHT_Q,
         AMMD_WEIGHT_KIND,
         AMMD_RESULT,
         AMMD_OQ_CODE,
         AMMD_Q_NO  
    FROM ci.AMMD  
   WHERE AMMD_DATE = :adt_date  AND  
         AMMD_DATA_TYPE  IN ('Q'  ,'q')
         and ammd_PARAM_KEY like :as_ammd_PARAM_KEY
         and ammd_kind_id2 like  :as_ammd_kind_id2
         and ammd_KIND_ID like :as_ammd_KIND_ID
         and ammd_PROD_ID like :as_ammd_PROD_ID
         order by cp_group1 , cp_group2 , ammd_w_time)", fcmRange);

         return db.GetDataTable(sql, parms);
      }

   }
}
