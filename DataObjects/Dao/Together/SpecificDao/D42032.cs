using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/21
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D42032: DataGate {

        /// <summary>
        /// CI.SP_H_TXN_42032_SCRN
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_42032_scrn(string as_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd
            };

            string sql =
@"

";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// CI.SP_H_TXN_42032_DETL 
        /// </summary>
        /// <param name="as_ymd_fm">yyyyMMdd</param>
        /// <param name="as_ymd_to">yyyyMMdd</param>
        /// <param name="as_sid"></param>
        /// <returns></returns>
        public DataTable d_42032(string as_ymd_fm, string as_ymd_to, string as_sid) {

            object[] parms = {
                ":as_ymd_fm", as_ymd_fm,
                ":as_ymd_to", as_ymd_to,
                ":as_sid", as_sid
            };

            string sql =
@"

";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
