using System;
using System.Data;
using Common;
/// <summary>
/// john,20190305,D30370
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30370 : DataGate
   {
      /// <summary>
      /// return AM2_IDFG_TYPE/AM2_BS_CODE/AM2_M_QNTY/AM2_YMD/AM2_SUM_TYPE
      /// </summary>
      /// <param name="as_syear"></param>
      /// <param name="as_eyear"></param>
      /// <param name="as_sym"></param>
      /// <param name="as_eym"></param>
      /// <returns></returns>
      //public DataTable GetData(string as_syear, string as_eyear, string as_sym,string as_eym)
      //{
      //   object[] parms = {
      //      ":as_syear",as_syear,
      //      ":as_eyear",as_eyear,
      //      ":as_sym",as_sym,
      //      ":as_eym",as_eym
      //      };

      //   string sql =
      //       @"SELECT AM2_IDFG_TYPE,   
      //               AM2_BS_CODE,   
      //               AM2_M_QNTY,   
      //               AM2_YMD,   
      //               AM2_SUM_TYPE  
      //          FROM ci.AM2  
      //         WHERE AM2_SUM_TYPE = 'Y'  AND  
      //               AM2_YMD >= :as_syear  AND  
      //               AM2_YMD <= :as_eyear  AND  
      //               AM2_PROD_TYPE = 'F'  AND  
      //               AM2_SUM_SUBTYPE = '1'  AND  
      //               AM2_PROD_TYPE in ('1','2','3','5','6','7','8')  
      //      union 
      //        SELECT AM2_IDFG_TYPE,   
      //               AM2_BS_CODE,   
      //               AM2_M_QNTY,   
      //               AM2_YMD,   
      //               AM2_SUM_TYPE  
      //          FROM ci.AM2  
      //         WHERE AM2_SUM_TYPE = 'M'  AND  
      //               AM2_YMD >= :as_sym  AND  
      //               AM2_YMD <= :as_eym  AND  
      //               AM2_PROD_TYPE = 'F'  AND  
      //               AM2_SUM_SUBTYPE = '1'  AND   
      //               AM2_PROD_TYPE in ('1','2','3','5','6','7','8')  
      //      union 
      //        SELECT AM2_IDFG_TYPE,   
      //               AM2_BS_CODE,   
      //               sum(AM2_M_QNTY) as AM2_M_QNTY ,   
      //               substr(AM2_YMD,1,4) as AM2_YMD,   
      //               AM2_SUM_TYPE  
      //          FROM ci.AM2  
      //         WHERE AM2_SUM_TYPE = 'M'  AND  
      //               AM2_YMD >= :as_sym  AND  
      //               AM2_YMD <= :as_eym  AND  
      //               AM2_PROD_TYPE = 'F'  AND  
      //               AM2_SUM_SUBTYPE = '1'  AND  
      //               AM2_PROD_TYPE in ('1','2','3','5','6','7','8')   
      //       GROUP BY  AM2_IDFG_TYPE,   
      //               AM2_BS_CODE,   
      //               AM2_SUM_TYPE,   
      //               substr(AM2_YMD,1,4)    
      //       ORDER BY am2_ymd,am2_idfg_type,am2_bs_code";
      //   DataTable dtResult = db.GetDataTable(sql, parms);
      //   return dtResult;
      //}


      /// <summary>
      /// return AM2_IDFG_TYPE/AM2_BS_CODE/AM2_M_QNTY/AM2_YMD/AM2_SUM_TYPE/SORT_TYPE
      /// </summary>
      /// <param name="as_syear"></param>
      /// <param name="as_eyear"></param>
      /// <param name="as_sym"></param>
      /// <param name="as_eym"></param>
      /// <returns></returns>
      public DataTable Get30371Data(string as_syear, string as_eyear, string as_sym, string as_eym)
      {
         object[] parms = {
            ":as_syear",as_syear,
            ":as_eyear",as_eyear,
            ":as_sym",as_sym,
            ":as_eym",as_eym
            };

         string sql =
             @" SELECT AM2_IDFG_TYPE,   
                        AM2_BS_CODE,   
                        SUM(AM2_M_QNTY) AS AM2_M_QNTY,   
                        AM2_YMD,   
                        AM2_SUM_TYPE,
                        '1' as SORT_TYPE 
                   FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'Y'  AND  
                        trim(AM2_YMD) >= :as_syear  AND  
                        trim(AM2_YMD) < :as_eyear  AND  
                        AM2_SUM_SUBTYPE = '1'  AND  
                        AM2_IDFG_TYPE in ('1','2','3','4','5','6','7','8') 
                  GROUP BY AM2_IDFG_TYPE,   
                        AM2_BS_CODE,  
                        AM2_YMD,   
                        AM2_SUM_TYPE
               union 
                 SELECT AM2_IDFG_TYPE,   
                        AM2_BS_CODE,   
                        sum(AM2_M_QNTY) as AM2_M_QNTY ,  
                        AM2_YMD,   
                        AM2_SUM_TYPE ,
                        '2' as SORT_TYPE  
                   FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'M'  AND  
                        trim(AM2_YMD) >= :as_sym  AND  
                        trim(AM2_YMD) <= :as_eym  AND   
                        AM2_SUM_SUBTYPE = '1'  AND  
                        AM2_IDFG_TYPE in ('1','2','3','4','5','6','7','8') 
                GROUP BY  AM2_IDFG_TYPE,   
                        AM2_BS_CODE,   
                        AM2_SUM_TYPE,   
                        AM2_YMD
               union 
                 SELECT AM2_IDFG_TYPE,   
                        AM2_BS_CODE,   
                        sum(AM2_M_QNTY) as AM2_M_QNTY ,   
                        substr(AM2_YMD,1,4) as AM2_YMD,   
                        AM2_SUM_TYPE  ,
                        '3' as SORT_TYPE  
                   FROM ci.AM2  
                  WHERE AM2_SUM_TYPE = 'M'  AND  
                        trim(AM2_YMD) >= :as_sym  AND  
                        trim(AM2_YMD) <= :as_eym  AND   
                        AM2_SUM_SUBTYPE = '1'  AND  
                        AM2_IDFG_TYPE in ('1','2','3','4','5','6','7','8') 
                GROUP BY  AM2_IDFG_TYPE,   
                        AM2_BS_CODE,   
                        AM2_SUM_TYPE,   
                        substr(AM2_YMD,1,4)  
                ORDER BY sort_type,am2_ymd,am2_idfg_type,am2_bs_code ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return KPR_DATE/KPR_PROD_TYPE/KPR_RATE
      /// </summary>
      /// <param name="as_sym"></param>
      /// <param name="as_eym"></param>
      /// <returns></returns>
      public DataTable Get30375Data(string as_sym, string as_eym)
      {
         object[] parms = {
            ":as_sym",as_sym,
            ":as_eym",as_eym
            };

         string sql =
             @"SELECT CI.KPR.KPR_DATE,   
                     CI.KPR.KPR_PROD_TYPE,   
                     CI.KPR.KPR_RATE  
                  FROM CI.KPR
               where TO_CHAR(KPR_DATE,'YYYYmm') >= :as_sym
                  and TO_CHAR(KPR_DATE,'YYYYmm') <= :as_eym
                  ORDER by KPR_DATE";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
   }
}
