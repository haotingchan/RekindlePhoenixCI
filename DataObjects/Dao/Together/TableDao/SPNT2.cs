using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/4/10
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class SPNT2 : DataGate {

      /// <summary>
      /// save CI.SPNT2 data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
select 
    spnt2_kind_id,
    spnt2_delta_xxx,
    spnt2_w_time,
    spnt2_w_user_id
from ci.spnt2
";
         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
