using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/4/10
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class SPNT1 : DataGate {

      /// <summary>
      /// save ci.spnt1 data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
select 
    spnt1_type,
    spnt1_days,
    spnt1_val,
    spnt1_w_time,
    spnt1_w_user_id
from ci.spnt1
";
         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
