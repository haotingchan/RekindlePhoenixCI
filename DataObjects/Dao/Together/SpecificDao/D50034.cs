using OnePiece;
using System.Data;
using System.Data.Common;


namespace DataObjects.Dao.Together.SpecificDao
{
   public class D50034: DataGate
   {
      public D50034()
      {
      }

      public DataTable ListAMMOAH(string is_sdate, string is_edate, string is_sum_type, string is_sum_subtype)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype
            };
         string sql = @"
SELECT AMM0_YMD,   
      AMM0_BRK_NO,AMM0_ACC_NO,
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
      AMM0_O_SUBTRACT_QNTY,
      AMM0_Q_SUBTRACT_QNTY,
      AMM0_IQM_SUBTRACT_QNTY
   FROM ci.AMM0AH
WHERE AMM0_DATA_TYPE = 'Q'
   AND AMM0_YMD >= :as_symd
   AND AMM0_YMD <= :as_eymd
   AND AMM0_SUM_TYPE = :as_sum_type 
   AND AMM0_SUM_SUBTYPE = :as_sum_subtype  
   AND AMM0_O_SUBTRACT_QNTY + AMM0_Q_SUBTRACT_QNTY + AMM0_IQM_SUBTRACT_QNTY > 0
ORDER BY amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id 
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListAMMOAH

      public DataTable ListAMMO(string is_sdate, string is_edate, string is_sum_type, string is_sum_subtype)
      {
         object[] parms = {
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sum_type",is_sum_type,
                ":as_sum_subtype",is_sum_subtype
            };
         string sql = @"
SELECT AMM0_YMD,   
         AMM0_BRK_NO,AMM0_ACC_NO,
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
         AMM0_O_SUBTRACT_QNTY,
         AMM0_Q_SUBTRACT_QNTY,
         AMM0_IQM_SUBTRACT_QNTY
    FROM ci.AMM0
   WHERE AMM0_DATA_TYPE = 'Q'
     AND AMM0_YMD >= :as_symd
     AND AMM0_YMD <= :as_eymd
     AND AMM0_SUM_TYPE = :as_sum_type 
     AND AMM0_SUM_SUBTYPE = :as_sum_subtype  
     AND AMM0_O_SUBTRACT_QNTY + AMM0_Q_SUBTRACT_QNTY + AMM0_IQM_SUBTRACT_QNTY > 0
   ORDER BY amm0_ymd , amm0_brk_no , amm0_acc_no , amm0_prod_type , amm0_prod_id 
";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListAMMO

   }
}