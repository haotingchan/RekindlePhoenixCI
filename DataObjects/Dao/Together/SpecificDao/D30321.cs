using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190220,D30321
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30321 : DataGate
   {
      /// <summary>
      /// return AI2_YMD/AI2_PARAM_KEY/AI2_DAY_COUNT/AI2_M_QNTY/AI2_OI
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable GetData(string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"SELECT AI2_YMD,   
                        AI2_PARAM_KEY,   
                        AI2_DAY_COUNT,   
                        AI2_M_QNTY as AI2_M_QNTY,
                        AI2_OI as AI2_OI ,  
                        (select nvl(min(RPT.RPT_SEQ_NO),0)
                           from ci.RPT
                          where RPT.RPT_TXD_ID = '30321'  
                            and RPT.RPT_VALUE = A.AI2_PARAM_KEY) as RPT_SEQ_NO
                   FROM ci.AI2 A
                  WHERE AI2_SUM_TYPE = 'D'  AND  
                        AI2_YMD >= :as_symd  AND  
                        AI2_YMD <= :as_eymd  AND
                        AI2_PROD_TYPE = 'F' AND
                        AI2_SUM_SUBTYPE = '3'
                  ORDER BY AI2_YMD,RPT_SEQ_NO,AI2_PARAM_KEY";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
