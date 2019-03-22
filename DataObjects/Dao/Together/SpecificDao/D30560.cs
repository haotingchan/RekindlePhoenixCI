using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190318,D30560
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30560 : DataGate
   {
      /// <summary>
      /// return AM2_IDFG_TYPE/AM2_PC_CODE/AM2_M_QNTY/AM2_YMD/AM2_SUM_TYPE
      /// </summary>
      /// <param name="as_syear"></param>
      /// <param name="as_eyear"></param>
      /// <param name="as_sym"></param>
      /// <param name="as_eym"></param>
      /// <returns></returns>
      public DataTable List30561(string as_syear, string as_eyear, string as_sym, string as_eym)
      {
         object[] parms = {
            ":as_syear",as_syear,
            ":as_eyear",as_eyear,
            ":as_sym",as_sym,
            ":as_eym",as_eym
            };

         string sql =
             @"
               SELECT AM2_YMD,
                      AM2_IDFG_TYPE,   
                        AM2_PC_CODE,   
                        AM2_M_QNTY,   
                        AM2_SUM_TYPE  
                     FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'Y'  AND  
                        AM2_YMD >= :as_syear  AND  
                        AM2_YMD < :as_eyear  AND  
                        AM2_PROD_TYPE = 'O'  AND  
                        ((AM2_SUM_SUBTYPE = '2'  AND AM2_PROD_SUBTYPE = 'S') ) AND
                        AM2_IDFG_TYPE in ('A','B') 
               union 
                  SELECT substr(AM2_YMD,1,4) as AM2_YMD,
                        AM2_IDFG_TYPE,   
                        AM2_PC_CODE,   
                        sum(AM2_M_QNTY) as AM2_M_QNTY ,   
                        AM2_SUM_TYPE  
                     FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'M'  AND  
                        trim(AM2_YMD)  >= :as_sym  AND  
                        trim(AM2_YMD)  <= :as_eym  AND  
                        AM2_PROD_TYPE = 'O'  AND  
                        ((AM2_SUM_SUBTYPE = '2'  AND AM2_PROD_SUBTYPE = 'S')) AND
                        AM2_IDFG_TYPE in ('A','B') 
                  GROUP BY  AM2_IDFG_TYPE,   
                        AM2_PC_CODE,   
                        AM2_SUM_TYPE,   
                        substr(AM2_YMD,1,4)   

               union 
                  SELECT AM2_YMD, 
                        AM2_IDFG_TYPE,   
                        AM2_PC_CODE, 
                        sum(AM2_M_QNTY) as AM2_M_QNTY ,
                        AM2_SUM_TYPE 
                     FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'M'  AND  
                        trim(AM2_YMD) >= :as_sym  AND  
                        trim(AM2_YMD)  <= :as_eym  AND  
                        AM2_PROD_TYPE = 'O'  AND  
                        ((AM2_SUM_SUBTYPE = '2'  AND AM2_PROD_SUBTYPE = 'S') ) AND
                        AM2_IDFG_TYPE in ('A','B') 
                  GROUP BY  AM2_IDFG_TYPE,   
                        AM2_PC_CODE,   
                        AM2_SUM_TYPE,     
                        AM2_YMD 
                 ORDER BY AM2_YMD,AM2_IDFG_TYPE
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// return AI2_SUM_TYPE/AI2_YMD/AI2_PC_CODE/AI2_M_QNTY/AI2_OI
      /// </summary>
      /// <param name="as_syear"></param>
      /// <param name="as_eyear"></param>
      /// <param name="as_sym"></param>
      /// <param name="as_eym"></param>
      /// <returns></returns>
      public DataTable List30561AI2(string as_syear, string as_eyear, string as_sym, string as_eym)
      {
         object[] parms = {
            ":as_syear",as_syear,
            ":as_eyear",as_eyear,
            ":as_sym",as_sym,
            ":as_eym",as_eym
            };

         string sql =
             @"
               SELECT AI2_SUM_TYPE,
                     AI2_YMD,   
                     AI2_PC_CODE,
                     AI2_M_QNTY,
                     AI2_OI  
                FROM ci.AI2  
               WHERE AI2_SUM_TYPE = 'Y' AND  
                     trim(AI2_YMD) >= :as_syear AND  
                     trim(AI2_YMD) < :as_eyear AND  
                     AI2_PROD_TYPE = 'O' AND  
                    ((AI2_SUM_SUBTYPE = '2'  AND AI2_PROD_SUBTYPE = 'S') ) 
            union 
              SELECT AI2_SUM_TYPE,
                     substr(AI2_YMD,1,4) as AI2_YMD,   
                     AI2_PC_CODE,
                     sum(AI2_M_QNTY) as AI2_M_QNTY,
                     sum(AI2_OI) as AI2_OI  
                FROM ci.AI2  
               WHERE AI2_SUM_TYPE = 'M' AND  
                     trim(AI2_YMD) >= :as_sym AND  
                     trim(AI2_YMD) <= :as_eym AND  
                     AI2_PROD_TYPE = 'O' AND  
                    (AI2_PROD_SUBTYPE = 'S' ) AND
                     AI2_SUM_SUBTYPE = '5' 
            GROUP BY AI2_SUM_TYPE,
                     substr(AI2_YMD,1,4),
                     AI2_PC_CODE

            union 
              SELECT AI2_SUM_TYPE,
                     AI2_YMD,   
                     AI2_PC_CODE,
                     sum(AI2_M_QNTY) as AI2_M_QNTY,
                     sum(AI2_OI) as AI2_OI  
                FROM ci.AI2  
               WHERE AI2_SUM_TYPE = 'M' AND  
                     trim(AI2_YMD) >= :as_sym AND  
                     trim(AI2_YMD) <= :as_eym AND 
                     AI2_PROD_TYPE = 'O' AND  
                    (AI2_PROD_SUBTYPE = 'S' ) AND
                     AI2_SUM_SUBTYPE = '5' 
               GROUP BY AI2_SUM_TYPE,
                     AI2_YMD,   
                     AI2_PC_CODE 
            ORDER BY AI2_YMD,AI2_PC_CODE
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

   }
}
