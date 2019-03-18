using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190312,D30550
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30550 : DataGate
   {
      /// <summary>
      /// return AM2_IDFG_TYPE/AM2_BS_CODE/AM2_M_QNTY/AM2_YMD/AM2_SUM_TYPE
      /// </summary>
      /// <param name="as_syear"></param>
      /// <param name="as_eyear"></param>
      /// <param name="as_sym"></param>
      /// <param name="as_eym"></param>
      /// <returns></returns>
      public DataTable GetData(string as_syear, string as_eyear, string as_sym, string as_eym)
      {
         object[] parms = {
            ":as_syear",as_syear,
            ":as_eyear",as_eyear,
            ":as_sym",as_sym,
            ":as_eym",as_eym
            };

         string sql =
             @"
               SELECT AM2_IDFG_TYPE,   
                        AM2_PC_CODE,  
                        SUM(AM2_M_QNTY) AS AM2_M_QNTY,  
                        AM2_YMD,   
                        AM2_SUM_TYPE  
                   FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'Y'  AND  
                        AM2_YMD >= :as_syear  AND  
                        AM2_YMD < :as_eyear  AND  
                        AM2_PROD_TYPE = 'O'  AND  
                        AM2_PROD_SUBTYPE = 'I'  AND
                        AM2_SUM_SUBTYPE = '2'  AND
                        AM2_IDFG_TYPE in ('1','2','3','4','5','7') 
                 GROUP BY AM2_IDFG_TYPE,   
                        AM2_PC_CODE,  
                        AM2_YMD,   
                        AM2_SUM_TYPE  
               union 
                 SELECT AM2_IDFG_TYPE,   
                        AM2_PC_CODE,   
                        sum(AM2_M_QNTY) as AM2_M_QNTY ,   
                        substr(AM2_YMD,1,4) as AM2_YMD,   
                        AM2_SUM_TYPE 
                   FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'M'  AND  
                        AM2_YMD >= :as_sym  AND  
                        AM2_YMD <= :as_eym  AND  
                        AM2_PROD_TYPE = 'O'  AND  
                        AM2_PROD_SUBTYPE = 'I'  AND
                        AM2_SUM_SUBTYPE = '2'  AND
                        AM2_IDFG_TYPE in ('1','2','3','4','5','7') 
                GROUP BY  AM2_IDFG_TYPE,   
                        AM2_PC_CODE,   
                        AM2_SUM_TYPE,   
                        substr(AM2_YMD,1,4)   

               union 
                 SELECT AM2_IDFG_TYPE,   
                        AM2_PC_CODE,  
                        SUM(AM2_M_QNTY) AS AM2_M_QNTY,  
                        AM2_YMD,   
                        AM2_SUM_TYPE  
                   FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'M'  AND  
                        AM2_YMD >= :as_sym  AND  
                        AM2_YMD <= :as_eym  AND  
                        AM2_PROD_TYPE = 'O'  AND  
                        AM2_PROD_SUBTYPE = 'I'  AND
                        AM2_SUM_SUBTYPE = '2'  AND
                        AM2_IDFG_TYPE in ('1','2','3','4','5','7') 
                 GROUP BY AM2_IDFG_TYPE,   
                        AM2_PC_CODE,  
                        AM2_YMD,   
                        AM2_SUM_TYPE  
                 ORDER BY am2_ymd,am2_idfg_type
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
