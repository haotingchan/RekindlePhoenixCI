using System;
using System.Data;
/// <summary>
/// john,20190402,D30503
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30503 : DataGate
   {
      /// <summary>
      /// return SPRD1_YMD/SPRD1_KIND_ID/PDK_NAME/DIFF
      /// </summary>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable GetData(string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"
               SELECT SUBSTR(SPRD1_YMD,1,6) as SPRD1_YMD,
                     SPRD1_KIND_ID,   
                     APDK_NAME as PDK_NAME,
                     ROUND(sum(CASE WHEN SPRD1_TOT_KEEP_TIME = 0 OR SPRD1_PUT_UNIT = 0 THEN 0 ELSE round(SPRD1_TOT_WEIGHT / SPRD1_TOT_KEEP_TIME / SPRD1_PUT_UNIT,2) end) / count(*),2) as DIFF 
                FROM CI.SPRD1 ,ci.APDK
               WHERE SPRD1_YMD >= :as_symd
                 AND SPRD1_YMD <= :as_eymd
                 AND SPRD1_KIND_ID = APDK_KIND_ID  
            GROUP BY SUBSTR(SPRD1_YMD,1,6),SPRD1_KIND_ID,APDK_NAME
            ORDER BY SPRD1_KIND_ID,SPRD1_YMD
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return AI2_YMD/AI2_SUM_TYPE
      /// </summary>
      /// <param name="as_sym"></param>
      /// <param name="as_eym"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListYMD(string as_sym, string as_eym, string as_sum_type,string as_prod_type)
      {
         object[] parms = {
            ":as_sym",as_sym,
            ":as_eym",as_eym,
            ":as_sum_type",as_sum_type,
            ":as_prod_type",as_prod_type
            };

         string sql =
             @"
               SELECT AI2_YMD,AI2_SUM_TYPE
                  FROM CI.AI2
                 WHERE AI2_YMD >= :as_sym
                   AND AI2_YMD <= :as_eym
                   AND AI2_SUM_TYPE = :as_sum_type
                   AND AI2_SUM_SUBTYPE = '1'
                   AND AI2_PROD_TYPE = :as_prod_type
               ORDER BY AI2_SUM_TYPE,AI2_YMD
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return SPRD1_YMD/SPRD1_KIND_ID/PDK_NAME/DIFF
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable List30504(string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"
               SELECT SPRD1_YMD as SPRD1_YMD,
                     SPRD1_KIND_ID,   
                     APDK_NAME as PDK_NAME,
                     CASE WHEN SPRD1_TOT_KEEP_TIME = 0 or SPRD1_PUT_UNIT = 0 THEN 0 ELSE round(SPRD1_TOT_WEIGHT/SPRD1_TOT_KEEP_TIME/SPRD1_PUT_UNIT,2) end as DIFF 
                FROM CI.SPRD1 ,ci.APDK
               WHERE SPRD1_YMD >= :as_symd
                 AND SPRD1_YMD <= :as_eymd
                 AND SPRD1_KIND_ID = APDK_KIND_ID  
               ORDER BY SPRD1_KIND_ID,SPRD1_YMD 
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

   }
}
