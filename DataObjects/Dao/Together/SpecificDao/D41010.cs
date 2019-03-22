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
    public class D41010: DataGate {

        /// <summary>
        /// table: CI.PCP
        /// </summary>
        /// <param name="as_prod_type">F or O</param>
        /// <param name="as_sdate">yyyy/MM/dd</param>
        /// <param name="as_edate">yyyy/MM/dd</param>
        /// <param name="as_kind_id">商品</param>
        /// <returns></returns>
        public DataTable d_41010(string as_prod_type, DateTime as_sdate, DateTime as_edate, string as_kind_id) {

            object[] parms = {
                ":as_prod_type", as_prod_type,
                ":as_sdate", as_sdate,
                ":as_edate", as_edate,
                ":as_kind_id", as_kind_id
            };

            string sql =
@"
select PDK_DATE ,PDK_KIND_ID,PDK_NAME ,PDK_STATUS_CODE  
 from ci.HPDK
where PDK_DATE >= :as_sdate
  and PDK_DATE <= :as_edate
  and PDK_PROD_TYPE = :as_prod_type
  and PDK_SUBTYPE='S' 
  and PDK_STATUS_CODE <> 'E'
  and PDK_KIND_ID LIKE :as_kind_id
order by pdk_date, pdk_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
