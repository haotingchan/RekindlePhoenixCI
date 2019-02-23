using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Winni, 2019/2/12
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class STW : DataGate {

      /// <summary>
      /// for W28110
      /// </summary>
      /// <param name="ls_date"></param>
      /// <param name="ls_m"></param>
      /// <returns></returns>
      public DataTable d_28110(string as_ymd) {

         object[] parms =
         {
                ":as_ymd", as_ymd
            };

         string sql = @"
SELECT STW_YMD,   
         STW_COM,   
         STW_SETTLE_M,   
         STW_SETTLE_Y,   
         STW_OPEN_1,  
         STW_HIGH,     
         STW_LOW,    
         STW_CLSE_1,    
         STW_SETTLE,   
         STW_VOLUMN,   
         STW_OINT, 
         --多  
         STW_DEL,   
         STW_RECTYP,   
         STW_OPEN_I1,   
         STW_OPEN_2,   
         STW_OPEN_I2, 
         STW_HIGH_I,  
         STW_LOW_I,   
         STW_CLSE_I1,   
         STW_CLSE_2,   
         STW_CLSE_I2  
FROM ci.STW   
WHERE STW_YMD = :as_ymd
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// 取得履約年月排序 (for W28110)
      /// </summary>
      /// <param name="as_ymd">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable getSettleYM(string as_ymd) {

         object[] parms =
         {
                ":as_ymd", as_ymd
            };

         string sql = @"
SELECT  TRIM(STW_SETTLE_Y || STW_SETTLE_M)  A   
FROM CI.STW
WHERE STW_YMD = :ls_ymd 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// 當日若出現一筆結算價為0,有可能是最後結算日,將履約年月最小的一筆資料OI清為0
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public int updateOI(string ls_ymd , string ls_year , string ls_month) {

         object[] parms =
         {
                ":ls_ymd", ls_ymd,
                ":ls_year", ls_year,
                ":ls_month", ls_month
            };

         string sql = @"
UPDATE CI.STW
SET STW_OINT = '0'
WHERE STW_YMD = :ls_ymd
AND STW_COM = 'TW'
AND STW_SETTLE_Y = :ls_year
AND STW_SETTLE_M = :ls_month
";
         return db.ExecuteSQL(sql , null);
      }

   }
}
