using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/5/9
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D40072: DataGate {

        /// <summary>
        /// 參數為空字串時只取table schema
        /// </summary>
        /// <param name="as_ymd"></param>
        /// <param name="adj_type"></param>
        /// <param name="as_stock_id"></param>
        /// <returns></returns>
        public DataTable d_40072(string as_ymd="", string adj_type="", string as_stock_id="") {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":adj_type", adj_type,
                ":as_stock_id", as_stock_id
            };

            string sql =
@"
SELECT A.*, CASE WHEN A.mgd2_prod_subtype <>'S' THEN 0 ELSE 1 END as cpSort
FROM
    (SELECT *
    FROM ci.MGD2
    WHERE MGD2_YMD = :as_ymd
    AND MGD2_ADJ_TYPE = :adj_type
    AND MGD2_STOCK_ID  = :as_stock_id) A
ORDER BY mgd2_ymd, mgd2_prod_type, cpSort, mgd2_prod_subtype, mgd2_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
