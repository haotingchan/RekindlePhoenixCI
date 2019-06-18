using BusinessObjects;
using System;
using System.Data;
/// <summary>
/// Winni, 2019/3/29
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class MMFO : DataGate {

        /// <summary>
        /// update ci.mmfo data
        /// </summary>
        /// <returns></returns>
        public ResultData UpdateData(DataTable inputData) {
            try {
                string sql = @"
select 
	mmfo_param_key, 
	mmfo_min_price, 
	mmfo_w_user_id, 
	mmfo_w_time, 
	mmfo_market_code
from ci.mmfo
";

                return db.UpdateOracleDB(inputData, sql);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        }
   }
