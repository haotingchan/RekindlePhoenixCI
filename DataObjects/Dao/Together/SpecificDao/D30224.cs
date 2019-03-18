using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/12
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30224:DataGate {

        public DataTable d_30224(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
select PLS2_YMD,PLS2_EFFECTIVE_YMD, 
       PLS2_RAISE_CNT,
       PLS2_LOWER_CNT,
       PLS2_NEW_CNT,
       PLS2_FUT_CNT,
       PLS2_OPT_CNT
  from 
      (select PLS2_YMD as P_YMD,
              sum(case when PLS2_FUT = 'F' then 1 else 0 end) as PLS2_FUT_CNT,
              sum(case when PLS2_OPT = 'O' then 1 else 0 end) as PLS2_OPT_CNT
         from ci.PLS2
        where PLS2_EFFECTIVE_YMD >= :as_symd
          and PLS2_EFFECTIVE_YMD <= :as_eymD 
        group by PLS2_YMD),
      (select PLS2_YMD,PLS2_EFFECTIVE_YMD,
              sum(case when PLS2_LEVEL_ADJ = '+' then 1 else 0 end) as PLS2_RAISE_CNT,
              sum(case when PLS2_LEVEL_ADJ = '-' then 1 else 0 end) as PLS2_LOWER_CNT,
              sum(case when PLS2_LEVEL_ADJ = '*' then 1 else 0 end) as PLS2_NEW_CNT
         from ci.PLS2
        where PLS2_EFFECTIVE_YMD >= :as_symd
          and PLS2_EFFECTIVE_YMD <= :as_eymD 
        group by PLS2_YMD,PLS2_EFFECTIVE_YMD)    
 where PLS2_YMD = P_YMD
 order by pls2_ymd, pls2_effective_ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
