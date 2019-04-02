using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// John, 2019/4/1,d_30290
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30290:DataGate {
      /// <summary>
      /// d_30290
      /// </summary>
      /// <param name="as_symd"></param>
      /// <returns></returns>
        public DataTable GetData(string as_symd) {

            object[] parms = {
                ":as_symd", as_symd
            };

            string sql =
                     @"
                     SELECT CI.PLP13.*,' ' AS OP_TYPE 
                     FROM CI.PLP13   
                     WHERE PLP13_ISSUE_YMD = :as_ymd
                         AND PLP13_PROD_SUBTYPE <> 'S'
                         AND PLP13_FUT = 'F'
                     UNION ALL
                     SELECT CI.PLP13.*,' ' AS OP_TYPE 
                     FROM CI.PLP13   
                     WHERE PLP13_ISSUE_YMD = :as_ymd
                         AND PLP13_PROD_SUBTYPE <> 'S'
                         AND PLP13_OPT = 'O'
                     UNION ALL
                     SELECT CI.PLP13.* ,' ' AS OP_TYPE
                     FROM CI.PLP13   
                     WHERE PLP13_ISSUE_YMD = :as_ymd
                         AND PLP13_PROD_SUBTYPE = 'S'
                     ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
