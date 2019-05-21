using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/5/20
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class SP2 : DataGate {
      /// <summary>
      /// save CI.SP2 data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
select 
	sp2_date, 
	sp2_type, 
	sp2_kind_id1, 
	sp2_kind_id2, 
	sp2_value_date, 
	sp2_w_time, 
	sp2_w_user_id, 
	sp2_osw_grp, 
	sp2_span_code, 
	sp2_adj_code
from ci.sp2
";

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
