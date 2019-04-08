using System;
using System.Data;
/// <summary>
/// john,201900408,D40060
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40060 : DataGate
   {
      /// <summary>
      /// return CNT/SP3_DATE/SP3_TYPE/SP3_KIND_ID1/SP3_KIND_ID2/SP3_RATE/CP_CNT
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="as_year"></param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_date, string as_year, string as_osw_grp)
      {
         object[] parms = {
            ":as_date",as_date,
            ":as_year",as_year,
            ":as_osw_grp",as_osw_grp
            };

         string sql =
             @"SELECT rank() OVER (partition by SP3_TYPE,SP3_KIND_ID1,SP3_KIND_ID2 order by SP3_DATE )-1 as CNT,
                     SP3_DATE,   
                     SP3_TYPE,   
                     SP3_KIND_ID1,   
                     SP3_KIND_ID2,   
                     SP3_RATE  ,
                     count(SP3_KIND_ID1) Over( partition by SP3_TYPE,SP3_KIND_ID1,SP3_KIND_ID2 ) as CP_CNT
                FROM CI.SP3  
               WHERE ( SP3_DATE <= :as_date ) AND  
                     ( to_char(SP3_DATE,'yyyy') >= :as_year ) AND  
                     ( SP3_OSW_GRP like :as_osw_grp )
               ORDER BY SP3_TYPE,SP3_KIND_ID1,SP3_KIND_ID2,SP3_DATE
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
