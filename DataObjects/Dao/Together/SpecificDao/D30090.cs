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
    public class D30090: DataGate {

        public DataTable d_30090(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
SELECT AE1_PARAM_KEY,   
         AE1_IDFG_TYPE,   
         sum(AE1_ACCEPTED_OI) as AE1_ACCEPTED_OI  
    FROM CI.AE1  
   WHERE AE1_YMD >= :as_symd  AND  
         AE1_YMD <= :as_eymd  AND  
         AE1_SUM_TYPE = 'D'  AND  
         AE1_SUM_SUBTYPE = '5'  
   GROUP BY  
         AE1_PARAM_KEY,   
         AE1_IDFG_TYPE   
 union all
   SELECT  
         'ZZZ',   
         AE1_IDFG_TYPE,   
         sum(AE1_ACCEPTED_OI) as AE1_ACCEPTED_OI  
    FROM CI.AE1  
   WHERE AE1_YMD >= :as_symd  AND  
         AE1_YMD <= :as_eymd  AND  
         AE1_SUM_TYPE = 'D'  AND  
         AE1_SUM_SUBTYPE = '5'    
   GROUP BY AE1_IDFG_TYPE
   ORDER BY AE1_PARAM_KEY, AE1_IDFG_TYPE
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
