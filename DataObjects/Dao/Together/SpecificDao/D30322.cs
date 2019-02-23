using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190220,D30322
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30322 : DataGate
   {
      /// <summary>
      /// return AI3_DATE/AI3_KIND_ID/AI3_CLOSE_PRICE/RPT_SEQ_NO
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_sdate, DateTime as_edate)
      {
         object[] parms = {
            ":as_sdate",as_sdate,
            ":as_edate",as_edate
            };

         string sql =
             @"SELECT AI3_DATE,   
                        AI3_KIND_ID,
                        AI3_CLOSE_PRICE,  
                        (select nvl(min(RPT.RPT_SEQ_NO),0)
                           from ci.RPT
                          where RPT.RPT_TXD_ID = '30321'  
                            and RPT.RPT_VALUE =  AI3.AI3_KIND_ID) as RPT_SEQ_NO
                   FROM ci.AI3  AI3
                  WHERE AI3_KIND_ID in (SELECT APDK_KIND_ID FROM ci.APDK WHERE APDK_PROD_TYPE = 'F' and APDK_PROD_SUBTYPE = 'I') 
                    AND AI3_DATE >= :as_sdate  
                    AND AI3_DATE <= :as_edate 
                  ORDER BY AI3_DATE";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
