using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/5/8
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class MGRT1: DataGate {

        public DataTable dddw_mgrt1(string as_prod_type) {

            object[] parms = {
                ":as_prod_type", as_prod_type
            };

            string sql =
@"
SELECT  MGRT1_LEVEL,
                MGRT1_LEVEL_NAME,
                MGRT1_CM_RATE, 
                MGRT1_CM_RATE_B,
                MGRT1_MM_RATE,
                MGRT1_MM_RATE_B,
                MGRT1_IM_RATE,
                MGRT1_IM_RATE_B
FROM ci.MGRT1
WHERE MGRT1_PROD_TYPE like :as_prod_type
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
