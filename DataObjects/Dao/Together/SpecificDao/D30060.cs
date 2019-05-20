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

        /// <summary>
        /// 日盤
        /// </summary>
        /// <param name="as_symd"></param>
        /// <param name="as_eymd"></param>
        /// <returns></returns>
        public DataTable d_30060(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
  select AI2_YMD,
       AI2_PARAM_KEY,
       --:as_rpt_type = '%'全部 /'0'日盤 /'1'夜盤
       (AI2_M_QNTY - nvl(AI2_AH_M_QNTY,0)) as AI2_M_QNTY,
       AI2_OI,
       RPT_LEVEL_1 as M_COL_SEQ,
       RPT_LEVEL_2 as OI_COL_SEQ
from ci.AI2 ,ci.RPT
where AI2_SUM_TYPE = 'D'
  and AI2_SUM_SUBTYPE = '3'
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

        /// <summary>
        /// 夜盤
        /// </summary>
        /// <param name="as_symd"></param>
        /// <param name="as_eymd"></param>
        /// <returns></returns>
        public DataTable d_30060_night(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
select AI2_YMD,
       AI2_PARAM_KEY,
       --:as_rpt_type = '%'全部 /'0'日盤 /'1'夜盤
       AI2_AH_M_QNTY as AI2_M_QNTY,
       null as AI2_OI,
       RPT_LEVEL_1 as M_COL_SEQ,RPT_LEVEL_2 as OI_COL_SEQ
from ci.AI2 ,ci.RPT
where AI2_SUM_TYPE = 'D'
  and AI2_SUM_SUBTYPE = '3'
  and AI2_YMD >= :as_symd
  and AI2_YMD <= :as_eymd
  and AI2_PROD_TYPE IN ('F','O') 
  and RPT_TXN_ID = '30060' and RPT_TXD_ID = '30060'
  and AI2_PROD_TYPE = RPT_VALUE_2(+)
  and AI2_PARAM_KEY = RPT_VALUE(+)
order by AI2_YMD, OI_COL_SEQ
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
