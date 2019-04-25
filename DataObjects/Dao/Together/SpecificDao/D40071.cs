using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/24
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D40071: DataGate {

        public DataTable d_40071(string as_ymd, string adj_type) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":adj_type", adj_type
            };

            string sql =
@"
SELECT A.*, CASE WHEN A.mgd2_prod_subtype <>'S' THEN 0 ELSE 1 END as cpSort
FROM
    (SELECT *
    FROM ci.MGD2
    WHERE MGD2_YMD = :as_ymd
    AND MGD2_ADJ_TYPE = :adj_type) A
ORDER BY mgd2_ymd, mgd2_prod_type, cpSort, mgd2_prod_subtype, mgd2_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_40071_log() {

            string sql =
@"
SELECT * FROM ci.MGD2L
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }
    }
}
