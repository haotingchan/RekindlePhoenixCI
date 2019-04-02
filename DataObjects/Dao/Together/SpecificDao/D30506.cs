using System;
using System.Data;
/// <summary>
/// john,20190402,D30506
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30506 : DataGate
   {
      /// <summary>
      /// return BST1_YMD/PDK_NAME/B_QNTY/S_QNTY
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
               select substr(BST1_YMD,1,6) AS BST1_YMD,
                     BST1_KIND_ID,APDK_NAME as PDK_NAME,
                     case when SUM(BST1_B_TOT_SEC) = 0 then 0 else round(SUM(BST1_B_QNTY_WEIGHT)/SUM(BST1_B_TOT_SEC),2) end as B_QNTY,
                     case when SUM(BST1_S_TOT_SEC) = 0 then 0 else round(SUM(BST1_S_QNTY_WEIGHT)/SUM(BST1_S_TOT_SEC),2) end as S_QNTY
                  from ci.BST1,ci.APDK
               where BST1_YMD >=:as_symd 
                 and BST1_YMD <=:as_eymd
                 and BST1_KIND_ID = APDK_KIND_ID
               group by substr(BST1_YMD,1,6),BST1_KIND_ID,APDK_NAME
               order by BST1_KIND_ID,BST1_YMD
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
      /// return BST1_YMD/BST1_KIND_ID/PDK_NAME/B_QNTY/S_QNTY
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable List30507(string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"
               select BST1_YMD AS BST1_YMD,
                     BST1_KIND_ID,APDK_NAME as PDK_NAME,
                     case when BST1_B_TOT_SEC = 0 then 0 else round(BST1_B_QNTY_WEIGHT/BST1_B_TOT_SEC,2) end as B_QNTY,
                     case when BST1_S_TOT_SEC = 0 then 0 else round(BST1_S_QNTY_WEIGHT/BST1_S_TOT_SEC,2) end as S_QNTY
                  from ci.BST1,ci.APDK
               where BST1_YMD >=:as_symd 
                 and BST1_YMD <=:as_eymd
                 and BST1_KIND_ID = APDK_KIND_ID
               order by BST1_KIND_ID,BST1_YMD
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

   }
}
