using Common;
using System;
using System.Data;

/// <summary>
/// ken,2019/3/12
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    /// <summary>
    /// AMIFU
    /// </summary>
    public class AMIFU : DataGate {

        /// <summary>
        /// get close price
        /// </summary>
        /// <param name="amifu_date"></param>
        /// <param name="amifu_data_source"></param>
        /// <param name="amifu_kind_id"></param>
        /// <returns></returns>
        public decimal GetClosePrice(DateTime amifu_date, string amifu_data_source = "R", string amifu_kind_id = "TXF") {
            object[] parms = {
                ":amifu_date", amifu_date,
                ":amifu_data_source", amifu_data_source,
                ":amifu_kind_id", amifu_kind_id
            };

            string sql = @"
select amifu_close_price as id_close_price
from ci.amifu
where amifu_date = :amifu_date
and amifu_data_source = :amifu_data_source
and amifu_kind_id = :amifu_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0)
                return dtResult.Rows[0]["id_close_price"].AsDecimal();
            else
                return 0;
        }
    }
}
