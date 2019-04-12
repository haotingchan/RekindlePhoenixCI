using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/4/12
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class MGT3 : DataGate {

      /// <summary>
      /// save ci.mgt3
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
select
    mgt3_date_fm,
    mgt3_date_to, 
    mgt3_memo, 
    mgt3_w_time, 
    mgt3_w_user_id
from ci.mgt3
";

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
