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
    public class D42033: DataGate {

        public DataTable d_42033(string as_ymd_fm, string as_ymd_to) {

            object[] parms = {
                ":as_ymd_fm", as_ymd_fm,
                ":as_ymd_to", as_ymd_to
            };

            string sql =
@"

";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
