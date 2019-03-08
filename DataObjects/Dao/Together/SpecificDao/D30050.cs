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
    public class D30050: DataGate {

        public DataTable d_30051(string as_ymd, string as_data_type) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_data_type", as_data_type
            };

            string sql =
@"
SELECT AI4_YMD,         
         AI4_PARAM_KEY,   
         AI4_SORT_SEQ,  
         AI4_QNTY,   
         AI4_MAX_YMD  ,  
         (select min(RPT.RPT_SEQ_NO)
            from ci.RPT
           where RPT.RPT_TXD_ID = '30051'  
             and RPT.RPT_VALUE = AI4_PARAM_KEY) as RPT_SEQ_NO
    FROM CI.AI4   
   WHERE AI4_YMD = :as_ymd
     AND AI4_SUM_TYPE = 'D'
     AND AI4_SUM_SUBTYPE = '3'
     --AND AI4_PROD_SUBTYPE <> 'S'
     AND AI4_DATA_TYPE = :as_data_type
     AND AI4_SORT_SEQ = 1
   ORDER BY RPT_SEQ_NO, AI4_SORT_SEQ
/*
--20150720
UNION ALL
SELECT AI4_YMD,         
         AI4_PROD_SUBTYPE,   
         AI4_SORT_SEQ,  
         AI4_QNTY,   
         AI4_MAX_YMD  ,  
         (select min(RPT.RPT_SEQ_NO)
            from ci.RPT
           where RPT.RPT_TXD_ID = '30051'  
             and RPT.RPT_VALUE = AI4_PROD_SUBTYPE
             and RPT.RPT_VALUE_2 = AI4_PROD_TYPE) as RPT_SEQ_NO
    FROM CI.AI4   
   WHERE AI4_YMD = :as_ymd
     AND AI4_SUM_TYPE = 'D'
     AND AI4_SUM_SUBTYPE = '2'
     AND AI4_PROD_SUBTYPE = 'S'
     AND AI4_DATA_TYPE = :as_data_type
     AND AI4_SORT_SEQ = 1
*/
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_30052(string as_ymd, string as_sum_type, string as_sum_subtype, string as_prod_type, string as_param_key, string as_data_type) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_sum_type", as_sum_type,
                ":as_sum_subtype", as_sum_subtype,
                ":as_prod_type", as_prod_type,
                ":as_param_key", as_param_key,
                ":as_data_type", as_data_type
            };

            string sql =
@"
SELECT AI4_YMD,         
         AI4_PARAM_KEY,   
         AI4_SORT_SEQ,  
         AI4_QNTY,   
         AI4_MAX_YMD  
    FROM CI.AI4   
   WHERE AI4_YMD = :as_ymd
     AND AI4_SUM_TYPE = :as_sum_type
     AND AI4_SUM_SUBTYPE = :as_sum_subtype
     AND AI4_PROD_TYPE LIKE :as_prod_type
     AND AI4_PARAM_KEY LIKE :as_param_key
     AND AI4_DATA_TYPE = :as_data_type
  ORDER BY AI4_PARAM_KEY, AI4_SORT_SEQ
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
