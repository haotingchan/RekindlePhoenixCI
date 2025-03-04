﻿using System;
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
        /// '%'全部 /'0'日盤 /'1'夜盤
        /// </summary>
        /// <param name="as_symd"></param>
        /// <param name="as_eymd"></param>
        /// <returns></returns>
        public DataTable d_30060(string as_symd, string as_eymd, string as_rpt_type) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_rpt_type",as_rpt_type
            };

            string sql = @"
select AI2_YMD,
       AI2_PARAM_KEY,
       --:as_rpt_type = '%'全部 /'0'日盤 /'1'夜盤
       case :as_rpt_type when '%' then AI2_M_QNTY
                         when '0' then (AI2_M_QNTY - nvl(AI2_AH_M_QNTY,0))
                         when '1' then AI2_AH_M_QNTY end as AI2_M_QNTY,
       case when :as_rpt_type = '1' then null else AI2_OI end as AI2_OI,
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

        public DataTable d_30060_prod(string as_symd, string as_eymd, string as_rpt_type) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_rpt_type",as_rpt_type
            };

            string sql = @"
select AI2_YMD,
      AI2_PARAM_KEY,
      case :as_rpt_type when '%' then sum(AI2_M_QNTY)
                        when '0' then sum (AI2_M_QNTY - nvl(AI2_AH_M_QNTY,0))
                        when '1' then sum(AI2_AH_M_QNTY) end as AI2_M_QNTY,
      sum(AI2_OI) as AI2_OI,
      RPT_LEVEL_1 as M_COL_SEQ,RPT_LEVEL_2 as OI_COL_SEQ
from ci.AI2 ,ci.RPT,ci.APDK
where AI2_SUM_TYPE = 'D'
 and AI2_SUM_SUBTYPE = '4'
 and AI2_YMD >= :as_symd
 and AI2_YMD <= :as_eymd
 and AI2_PROD_TYPE IN ('F','O')
 and RPT_TXN_ID = '30060' and RPT_TXD_ID = '30060'
 and AI2_PROD_TYPE = RPT_VALUE_2(+)
 and AI2_PARAM_KEY = RPT_VALUE(+)
 and AI2_KIND_ID = APDK_KIND_ID
 and APDK_EXPIRY_TYPE = 'W'
 group by AI2_YMD,AI2_PARAM_KEY,RPT_LEVEL_1,RPT_LEVEL_2
order by AI2_YMD, OI_COL_SEQ
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
