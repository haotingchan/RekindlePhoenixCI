using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    public class D30070: DataGate {

        public DataTable d_30070(string as_ymd_fm, string as_ymd_to) {

            object[] parms = {
                ":as_ymd_fm", as_ymd_fm,
                ":as_ymd_to", as_ymd_to
            };

            string sql =
@"
select AA2_YMD,AA2_PARAM_KEY,SUM(AA2_AMT) as aa2_amt
  from ci.AA2
 where AA2_YMD >= :as_ymd_fm
   and AA2_YMD <= :as_ymd_to
   and AA2_PROD_SUBTYPE <> 'S'
 group by AA2_YMD,AA2_PROD_TYPE,AA2_PARAM_KEY
 union all
select AA2_YMD,
       trim(AA2_PARAM_KEY) ||'-'|| case when APDK_UNDERLYING_MARKET='1' then 'TWSE' else 'OTC' end,  
       SUM(AA2_AMT) as aa2_amt
  from ci.AA2,ci.APDK
 where AA2_YMD >= :as_ymd_fm
   and AA2_YMD <= :as_ymd_to
   and AA2_PROD_SUBTYPE = 'S'
   and AA2_KIND_ID = APDK_KIND_ID
 group by AA2_YMD,AA2_PROD_TYPE,AA2_PROD_SUBTYPE,AA2_PARAM_KEY,APDK_UNDERLYING_MARKET
 order by aa2_ymd, aa2_param_key
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_30070_stk(string as_ymd_fm, string as_ymd_to) {

            object[] parms = {
                ":as_ymd_fm", as_ymd_fm,
                ":as_ymd_to", as_ymd_to
            };

            string sql =
@"
Select A.*
from
(select AA2_YMD,AA2_PARAM_KEY,SUM(AA2_AMT_STK) as aa2_amt
  from ci.AA2
 where AA2_YMD >= :as_ymd_fm
   and AA2_YMD <= :as_ymd_to
   and AA2_PROD_SUBTYPE <> 'S'
 group by AA2_YMD,AA2_PROD_TYPE,AA2_PARAM_KEY
union all
select AA2_YMD,
          TRIM(AA2_PARAM_KEY)||'-'|| case when APDK_UNDERLYING_MARKET='1' then 'TWSE' else 'OTC' end,  
          SUM(AA2_AMT_STK) as aa2_amt
  from ci.AA2,ci.APDK
 where AA2_YMD >= :as_ymd_fm
   and AA2_YMD <= :as_ymd_to
   and AA2_PROD_SUBTYPE = 'S'
   and AA2_KIND_ID = APDK_KIND_ID
 group by AA2_YMD,AA2_PROD_TYPE,AA2_PROD_SUBTYPE,AA2_PARAM_KEY,APDK_UNDERLYING_MARKET
 order by aa2_ymd, aa2_param_key) A
 where aa2_amt is not null
   and aa2_amt > 0
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
