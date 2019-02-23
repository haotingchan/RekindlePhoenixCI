using System;
using System.Data;
/// <summary>
/// Winni, 2019/1/28
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class AB1:DataGate {

      /// <summary>
      /// get CI.AB1 data (for D28510)
      /// </summary>
      /// <returns></returns>
      public DataTable ListData(DateTime adt_date) {

         object[] parms =
            {
                ":adt_date", adt_date
            };

         string sql = @"
SELECT AB1_ACC_TYPE,   
      '     ' as ACC_NAME, 
      AB1_COUNT,    
      AB1_ACCU_COUNT,    
      AB1_TRADE_COUNT,   
      AB1_DATE  
FROM CI.AB1   
WHERE AB1_DATE = :adt_date
ORDER BY AB1_DATE , AB1_ACC_TYPE 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}

