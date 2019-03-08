using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190305,D30380
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30380 : DataGate
   {
      /// <summary>
      /// return AI3_DATE/AI3_CLOSE_PRICE/AI3_M_QNTY/AI3_OI/AI3_INDEX/AI3_AMOUNT/AI3_M_QNTY_FITX
      /// </summary>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_sdate, DateTime as_edate)
      {
         object[] parms = {
            ":as_sdate",as_sdate,
            ":as_edate",as_edate
            };

         string sql =
             @"
               SELECT M.AI3_DATE AS AI3_DATE,   
                     M.AI3_CLOSE_PRICE AS AI3_CLOSE_PRICE,   
                     M.AI3_M_QNTY AS AI3_M_QNTY,   
                     M.AI3_OI AS AI3_OI,   
                     M.AI3_INDEX AS AI3_INDEX,   
                     M.AI3_AMOUNT AS AI3_AMOUNT,
                     I.AI3_M_QNTY AS AI3_M_QNTY_FITX
            FROM
            (  SELECT AI3_DATE,   
                     AI3_CLOSE_PRICE,   
                     AI3_M_QNTY,   
                     AI3_OI,   
                     AI3_INDEX,   
                     AI3_AMOUNT  
                FROM ci.AI3  
               WHERE AI3_KIND_ID = 'STW'  AND  
                     AI3_DATE >= :as_sdate  AND  
                     AI3_DATE <= :as_edate ) M,  
            ( SELECT AI3_DATE,    
                     SUM(AI3_M_QNTY) as AI3_M_QNTY
                FROM ci.AI3  
               WHERE AI3_KIND_ID in ('TXF')  AND  
                     AI3_DATE >= :as_sdate  AND  
                     AI3_DATE <= :as_edate 
               GROUP BY AI3_DATE) I
            WHERE M.AI3_DATE = I.AI3_DATE
            ORDER BY AI3_DATE
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
