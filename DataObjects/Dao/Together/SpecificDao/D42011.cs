using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D42011: DataGate {

        /// <summary>
        /// 取得標的證券風險價格係數
        /// </summary>
        /// <returns></returns>
        public decimal GetCmRate() {

            string sql =
@"
SELECT MGRT1_CM_RATE AS LD_CM_RATE1
  FROM CI.MGRT1
 WHERE MGRT1_PROD_TYPE = 'F'
    AND MGRT1_LEVEL = (SELECT MAX(MGRT1_LEVEL) FROM CI.MGRT1
                                         WHERE MGRT1_PROD_TYPE = 'F' AND MGRT1_REPORT = 'Y') 
";
            DataTable dtResult = db.GetDataTable(sql, null);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LD_CM_RATE1"].AsDecimal();
            }
        }

        /// <summary>
        /// 取得三個級距的本日風險價格係數
        /// </summary>
        /// <returns></returns>
        public DataTable Get3CmRate() {

            string sql =
@"
SELECT MAX(CASE WHEN MGRT1_LEVEL = '1' THEN MGRT1_CM_RATE  ELSE 0 END) AS LD_CM_RATE1,
           MAX(CASE WHEN MGRT1_LEVEL = '2' THEN MGRT1_CM_RATE  ELSE 0 END) AS LD_CM_RATE2,
           MAX(CASE WHEN MGRT1_LEVEL = '3' THEN MGRT1_CM_RATE  ELSE 0 END) AS LD_CM_RATE3
  FROM CI.MGRT1
 WHERE MGRT1_PROD_TYPE = 'F'
 GROUP BY MGRT1_PROD_TYPE
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// 取得表一達標準判斷
        /// </summary>
        /// <returns></returns>
        public decimal GetRange() {

            string sql =
@"
SELECT MGT2_ADJUST_RATE AS LD_CM_RATE1
 FROM CI.MGT2
 WHERE MGT2_PROD_TYPE = 'F' AND MGT2_KIND_ID = 'STF'
";
            DataTable dtResult = db.GetDataTable(sql, null);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LD_CM_RATE1"].AsDecimal();
            }
        }
    }
}
