using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/24
/// </summary>
namespace DataObjects.Dao.Together.TableDao {

    /// <summary>
    /// for d_20430
    /// </summary>
    public class AM7M: DataGate {

        public DataTable ListAllByDate(string as_ym) {

            object[] parms = {
                ":as_ym", as_ym
            };

            string sql =
@"
  SELECT AM7M_YM,   
        AM7M_PROD_TYPE,
        AM7M_PROD_SUBTYPE,
        AM7M_PARAM_KEY,
        AM7M_DAY_COUNT,
        AM7M_AVG_QNTY,
        AM7M_W_USER_ID,
        AM7M_W_TIME,
         ' ' as OP_TYPE ,
        AM7M_DAY_COUNT AS ORG_DAY_COUNT,
        AM7M_AVG_QNTY AS ORG_AVG_QNTY,
        AM7M_TFXM_YEAR_AVG_QNTY
    FROM ci.AM7M
   WHERE AM7M.AM7M_YM = :as_ym 
ORDER BY AM7M_PROD_TYPE, AM7M_PARAM_KEY
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public ResultData UpdateAM7M(DataTable inputData) {
            string sql =
@"
SELECT AM7M_YM,   
       AM7M_PROD_TYPE,
       AM7M_PROD_SUBTYPE,
       AM7M_PARAM_KEY,
       AM7M_DAY_COUNT,
       AM7M_AVG_QNTY,
       AM7M_W_USER_ID,
       AM7M_W_TIME,
       AM7M_TFXM_YEAR_AVG_QNTY
FROM ci.AM7M
";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
