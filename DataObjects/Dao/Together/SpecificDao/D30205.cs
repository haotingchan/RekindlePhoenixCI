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
    public class D30205:DataGate {

        public DataTable d_30205(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
select PL2_YMD,PL2_EFFECTIVE_YMD, 
       PL2_NATURE_ADJ,
       PL2_KIND_ID,
	    PL2_LEGAL_ADJ,
	    PL2_999_ADJ
  from ci.PL2
 where PL2_EFFECTIVE_YMD >= :as_symd
   and PL2_EFFECTIVE_YMD <= :as_eymd
   and (PL2_NATURE_ADJ in ('-','+') or PL2_LEGAL_ADJ in ('-','+') or PL2_999_ADJ in ('-','+'))
 order by pl2_ymd, pl2_effective_ymd, pl2_nature_adj
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
