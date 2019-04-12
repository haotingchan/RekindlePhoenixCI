using BusinessObjects;
using Common;
using System;
using System.Data;

/// <summary>
/// Winni, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class HCPR : DataGate {

      /// <summary>
      /// save ci.hcpr data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
select
cpr_kind_id, 
cpr_price_risk_rate, 
cpr_w_time, 
cpr_w_user_id, 
cpr_prod_subtype, 

cpr_effective_date, 
cpr_approval_date, 
cpr_approval_number, 
cpr_remark, 
cpr_data_num
from ci.hcpr
";
         return db.UpdateOracleDB(inputData , sql);
      }

   }
}
