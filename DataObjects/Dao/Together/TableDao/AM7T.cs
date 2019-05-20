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
    public class AM7T: DataGate {

        public DataTable ListAllByDate(string as_ym) {

            object[] parms = {
                ":as_ym", as_ym
            };

            string sql =
@"
SELECT AM7T_Y,   
       AM7T_PROD_TYPE,
       AM7T_PROD_SUBTYPE,
       AM7T_PARAM_KEY,
       AM7T_DAY_COUNT,
       AM7T_AVG_QNTY,
       AM7T_W_USER_ID,
       AM7T_W_TIME,
           ' ' as OP_TYPE ,
       AM7T_DAY_COUNT AS ORG_DAY_COUNT,
       AM7T_AVG_QNTY AS ORG_AVG_QNTY,
       AM7T_TFXM_YEAR_AVG_QNTY
FROM ci.AM7T
WHERE AM7T.AM7T_Y = :as_ym
ORDER BY AM7T_PROD_TYPE, AM7T_PARAM_KEY
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public ResultData UpdateAM7T(DataTable inputData) {
            string sql =
@"
SELECT AM7T_Y,   
       AM7T_PROD_TYPE,
       AM7T_PROD_SUBTYPE,
       AM7T_PARAM_KEY,
       AM7T_DAY_COUNT,
       AM7T_AVG_QNTY,
       AM7T_W_USER_ID,
       AM7T_W_TIME,
       AM7T_TFXM_YEAR_AVG_QNTY
FROM ci.AM7T
";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
