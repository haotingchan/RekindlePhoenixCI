using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/23
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class AM7: DataGate {
        /// <summary>
        /// for d_20410
        /// </summary>
        /// <param name="as_sym"></param>
        /// <param name="as_eym"></param>
        /// <returns></returns>
        public DataTable ListAllByDate(string as_sym, string as_eym) {

            object[] parms = {
                ":as_sym", as_sym,
                ":as_eym", as_eym
            };

            string sql =
@"
SELECT AM7.AM7_Y,   
       AM7.AM7_FC_QNTY,   
       AM7.AM7_DAY_COUNT,
       AM7_FUT_AVG_QNTY ,
       AM7_OPT_AVG_QNTY,
       AM7_FC_TAX,
       ' ' as OP_TYPE  
FROM ci.AM7
WHERE AM7.AM7_Y >= :as_sym  
  AND AM7.AM7_Y <= :as_eym
ORDER BY AM7_Y
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public ResultData UpdateAM7(DataTable inputData) {
            string sql =
@"
SELECT AM7.AM7_Y,   
       AM7.AM7_FC_QNTY,   
       AM7.AM7_DAY_COUNT,
       AM7_FUT_AVG_QNTY ,
       AM7_OPT_AVG_QNTY,
       AM7_FC_TAX
FROM ci.AM7
";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
