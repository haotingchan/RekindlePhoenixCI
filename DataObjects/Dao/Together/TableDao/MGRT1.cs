using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/5/8
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class MGRT1 : DataGate {

      public DataTable dddw_mgrt1(string as_prod_type) {

         object[] parms = {
                ":as_prod_type", as_prod_type
            };

         string sql =
@"
SELECT  MGRT1_LEVEL,
                MGRT1_LEVEL_NAME,
                MGRT1_CM_RATE, 
                MGRT1_CM_RATE_B,
                MGRT1_MM_RATE,
                MGRT1_MM_RATE_B,
                MGRT1_IM_RATE,
                MGRT1_IM_RATE_B
FROM ci.MGRT1
WHERE MGRT1_PROD_TYPE like :as_prod_type
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      public string GetCmMax(string prod_type) {

         object[] parms = {
                ":prod_type", prod_type
            };


         string sql = @"SELECT MGRT1_CM_RATE AS LD_CM_MAX
		                  FROM CI.MGRT1
		                  WHERE MGRT1_PROD_TYPE = :prod_type
			               AND MGRT1_LEVEL = 
                        (SELECT MAX(MGRT1_LEVEL) FROM CI.MGRT1
									WHERE MGRT1_PROD_TYPE = :prod_type AND MGRT1_REPORT = 'Y')";

         return db.ExecuteScalar(sql, CommandType.Text, parms);
      }

      public DataTable GetCmRate(string prod_type) {

         object[] parms = {
                ":prod_type", prod_type
            };


         string sql = @"SELECT MAX(CASE WHEN MGRT1_LEVEL = '1' THEN MGRT1_CM_RATE ELSE 0 END) AS LD_CM_RATE1,
                               MAX(CASE WHEN MGRT1_LEVEL = '2' THEN MGRT1_CM_RATE ELSE 0 END) AS LD_CM_RATE2,
                               MAX(CASE WHEN MGRT1_LEVEL = '3' THEN MGRT1_CM_RATE ELSE 0 END) AS LD_CM_RATE3
                         FROM CI.MGRT1
                         WHERE MGRT1_PROD_TYPE = :prod_type
                         GROUP BY MGRT1_PROD_TYPE";

         return db.GetDataTable(sql, parms);
      }

      public DataTable GetDistinctMGRT1Level() {

         string sql = @"SELECT DISTINCT MGRT1_LEVEL 
                         FROM CI.MGRT1 
                         ORDER  BY MGRT1_LEVEL";

         return db.GetDataTable(sql, null);
      }
   }
}
