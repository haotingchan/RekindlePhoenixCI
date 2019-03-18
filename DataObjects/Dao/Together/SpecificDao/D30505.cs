using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190313,D30505
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30505 : DataGate
   {
      /// <summary>
      /// return SPRD1_YMD/SPRD1_KIND_ID/SPRD1_TOT_KEEP_TIME/SPRD1_TOT_WEIGHT/diff
      /// </summary>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_symd, DateTime as_eymd)
      {
         object[] parms = {
            ":as_symd",as_symd.ToString("yyyyMMdd"),
            ":as_eymd",as_eymd.ToString("yyyyMMdd")
            };

         string sql =
             @"
               SELECT SPRD1_YMD as SPRD1_YMD,
                     SPRD1_KIND_ID,   
                     APDK_NAME as PDK_NAME,
                     SPRD1_TOT_KEEP_TIME,SPRD1_TOT_WEIGHT
                     ,CASE WHEN sprd1_tot_keep_time = 0 then 0 when sprd1_put_unit=0 then 0 else round(sprd1_tot_weight/sprd1_tot_keep_time/sprd1_put_unit,2) end as diff
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
