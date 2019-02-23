using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Lukas, 2019/1/4
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class MMWK : DataGate {
        /// <summary>
        /// for D51040
        /// </summary>
        /// <param name="as_ym"></param>
        /// <returns></returns>
        public DataTable ListAllByDate(string as_ym) {

            object[] parms =
            {
                ":as_ym", as_ym
            };

            string sql = @"
SELECT mmwk_prod_type,
	   mmwk_ym,
	   mmwk_kind_id,
	   mmwk_weight,
	   mmwk_w_user_id,
	   mmwk_w_time,
	   ' ' as OP_TYPE
FROM ci.mmwk
where MMWK_YM = :as_ym
order by mmwk_prod_type, 
		 mmwk_ym, 
		 mmwk_kind_id";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
