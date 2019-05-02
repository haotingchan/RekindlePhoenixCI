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

      /// <summary>
      /// 40210專用
      /// </summary>
      /// <returns></returns>
      public DataTable ListData() {
         
         string sql = @"
select max(case when spnt1_type = 'CHI' and spnt1_days = 150 then spnt1_val else 0 end) as chi_150,
       max(case when spnt1_type = 'CHI' and spnt1_days = 180 then spnt1_val else 0 end) as chi_180,
       max(case when spnt1_type = '^' and spnt1_days = 365 then spnt1_val else 0 end) as v365
from ci.spnt1
where spnt1_type in ('CHI','^')
";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// 40210專用, return tinv_150/tinv_180
      /// </summary>
      /// <returns></returns>
      public DataTable ListData2(string spnt1_type = "TINV") {
         object[] parms = {
            ":spnt1_type", spnt1_type
         };

         string sql = @"
select max(case when spnt1_days = 150 then spnt1_val else 0 end) as tinv_150,
    max(case when spnt1_days = 180 then spnt1_val else 0 end) as tinv_180                  
from ci.spnt1
where spnt1_type = :spnt1_type
";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
