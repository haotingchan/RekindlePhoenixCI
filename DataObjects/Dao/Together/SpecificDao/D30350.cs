using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190222,D30350
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30350 : DataGate
   {
      /// <summary>
      /// return AI2_YMD/AI2_PARAM_KEY/AI2_PC_CODE/sum(AI2_M_QNTY)/sum(AI2_OI)/sum(AI2_MMK_QNTY)/CP_SUM_AI2_OI/CP_SUM_AI2_MMK_QNTY
      /// </summary>
      /// <param name="as_kind_id"></param>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable Get30351Data(string as_kind_id, string as_symd,string as_eymd)
      {
         object[] parms = {
            ":as_kind_id",as_kind_id,
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"SELECT main.*,
               SUM(AI2_OI) OVER (partition by AI2_YMD ORDER BY AI2_YMD) as CP_SUM_AI2_OI,
               SUM(AI2_MMK_QNTY) OVER (partition by AI2_YMD ORDER BY AI2_YMD) as CP_SUM_AI2_MMK_QNTY
               FROM
               (SELECT AI2_YMD,   
                        AI2_PARAM_KEY,   
                        AI2_PC_CODE,   
                        sum(AI2_M_QNTY) as AI2_M_QNTY,   
                        sum(AI2_OI) as AI2_OI,   
                        sum(AI2_MMK_QNTY) as AI2_MMK_QNTY  
                   FROM ci.AI2  ,ci.APDK
                  WHERE AI2_SUM_TYPE = 'D'  AND  
                        AI2_YMD >= :as_symd  AND  
                        AI2_YMD <= :as_eymd  AND  
                        AI2_SUM_SUBTYPE = '5'  AND
                        AI2_KIND_ID = APDK_KIND_ID AND
                        APDK_PARAM_KEY like :as_kind_id||'%'   
               GROUP BY AI2_YMD,   
                        AI2_PARAM_KEY,   
                        AI2_PC_CODE 
               ORDER BY AI2_YMD) main";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return AI2_YMD/AI2_KIND_ID2/AI2_PC_CODE/sum(AI2_M_QNTY)/sum(AI2_OI)/sum(AI2_MMK_QNTY)/CP_SUM_AI2_OI/CP_SUM_AI2_MMK_QNTY
      /// </summary>
      /// <param name="as_kind_id"></param>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable Get30358Data(string as_kind_id, string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_kind_id",as_kind_id,
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"SELECT main.*,
               SUM(AI2_OI) OVER (partition by AI2_YMD ORDER BY AI2_YMD) as CP_SUM_AI2_OI,
               SUM(AI2_MMK_QNTY) OVER (partition by AI2_YMD ORDER BY AI2_YMD) as CP_SUM_AI2_MMK_QNTY
               FROM
               (SELECT AI2_YMD,   
                        AI2_KIND_ID2 as AI2_PARAM_KEY,   
                        AI2_PC_CODE,   
                        sum(AI2_M_QNTY) as AI2_M_QNTY,   
                        sum(AI2_OI) as AI2_OI,   
                        sum(AI2_MMK_QNTY) as AI2_MMK_QNTY  
                   FROM ci.AI2  ,ci.APDK
                  WHERE AI2_SUM_TYPE = 'D'  AND  
                        AI2_YMD >= :as_symd  AND  
                        AI2_YMD <= :as_eymd  AND  
                        AI2_SUM_SUBTYPE = '5'  AND
                        AI2_KIND_ID = APDK_KIND_ID AND
                        AI2_KIND_ID2 like :as_kind_id||'%'   
               GROUP BY AI2_YMD,   
                         AI2_KIND_ID2,   
                        AI2_PC_CODE 
               ORDER BY AI2_YMD) main";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
