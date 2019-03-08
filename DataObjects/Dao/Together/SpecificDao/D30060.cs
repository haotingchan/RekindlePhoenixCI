using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/6
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30060: DataGate {

        public DataTable d_30060(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
select AI2_YMD,
       AI2_PARAM_KEY,
       AI2_M_QNTY,
       AI2_OI,
       RPT_LEVEL_1 as M_COL_SEQ,RPT_LEVEL_2 as OI_COL_SEQ
from ci.AI2 ,ci.RPT
where AI2_SUM_TYPE = 'D'
  and AI2_SUM_SUBTYPE = '3'
  --and AI2_PROD_SUBTYPE <> 'S'
  and AI2_YMD >= :as_symd
  and AI2_YMD <= :as_eymd
  and AI2_PROD_TYPE IN ('F','O') 
  and RPT_TXN_ID = '30060' and RPT_TXD_ID = '30060'
  and AI2_PROD_TYPE = RPT_VALUE_2(+)
  and AI2_PARAM_KEY = RPT_VALUE(+)
order by AI2_YMD, OI_COL_SEQ
/*
20150720
union all
select AI2_YMD,
       AI2_PROD_SUBTYPE,
       AI2_M_QNTY,
       AI2_OI,
       RPT_LEVEL_1 as M_COL_SEQ,RPT_LEVEL_2 as OI_COL_SEQ
from ci.AI2 ,ci.RPT
where AI2_SUM_TYPE = 'D'
  and AI2_SUM_SUBTYPE = '2'
  and AI2_PROD_SUBTYPE = 'S'
  and AI2_YMD >= :as_symd
  and AI2_YMD <= :as_eymd
  and AI2_PROD_TYPE IN ('F','O') 
  and RPT_TXN_ID = '30060' and RPT_TXD_ID = '30060'
  and AI2_PROD_SUBTYPE = RPT_VALUE(+)
  and AI2_PROD_TYPE = RPT_VALUE_2(+)
*/
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
