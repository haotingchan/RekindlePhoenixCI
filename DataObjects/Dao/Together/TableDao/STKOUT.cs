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
    public class STKOUT: DataGate {

        /// <summary>
        /// for d_20233
        /// stkout_name有多餘空白故trim掉
        /// </summary>
        /// <returns></returns>
        public DataTable ListAllByDate(string as_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd
            };

            string sql =
@"
 SELECT 
 	stkout_id, 
 	trim(stkout_name) as stkout_name, 
 	stkout_b, 
 	' ' AS OP_TYPE, 
 	stkout_date, 
 	stkout_time
 FROM CI.STKOUT
 WHERE STKOUT_DATE = :as_ymd
 ORDER BY stkout_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
