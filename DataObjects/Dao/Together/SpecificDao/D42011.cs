using BusinessObjects;
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

        /// <summary>
        /// 股票期貨風險價格係數機動評估指標
        /// </summary>
        /// <param name="as_today"></param>
        /// <param name="as_last_date"></param>
        /// <param name="as_30_rate_level1"></param>
        /// <param name="as_30_rate_level2"></param>
        /// <param name="as_30_rate_level3"></param>
        /// <param name="as_30_rate_levelz"></param>
        /// <param name="as_day_rate_level1"></param>
        /// <param name="as_day_rate_level2"></param>
        /// <param name="as_day_rate_level3"></param>
        /// <param name="as_day_rate_levelz"></param>
        /// <returns></returns>
        public DataTable d_42011_detl(DateTime as_today, DateTime as_last_date, decimal as_30_rate_level1,
                                      decimal as_30_rate_level2, decimal as_30_rate_level3, decimal as_30_rate_levelz,
                                      decimal as_day_rate_level1, decimal as_day_rate_level2, decimal as_day_rate_level3, decimal as_day_rate_levelz) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("as_today",as_today),
            new DbParameterEx("as_last_date",as_last_date),
            new DbParameterEx("as_30_rate_level1",as_30_rate_level1),
            new DbParameterEx("as_30_rate_level2",as_30_rate_level2),
            new DbParameterEx("as_30_rate_level3",as_30_rate_level3),
            new DbParameterEx("as_30_rate_levelz",as_30_rate_levelz),
            new DbParameterEx("as_day_rate_level1",as_day_rate_level1),
            new DbParameterEx("as_day_rate_level2",as_day_rate_level2),
            new DbParameterEx("as_day_rate_level3",as_day_rate_level3),
            new DbParameterEx("as_day_rate_levelz",as_day_rate_levelz)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

            string sql = "CI.SP_H_TXN_42011_DETL";

            return db.ExecuteStoredProcedureEx(sql, parms, true);
        }

        /// <summary>
        /// 取前一交易日
        /// </summary>
        /// <param name="ld_last_date"></param>
        /// <returns></returns>
        public DateTime GetLastDate(DateTime ld_last_date) {

            object[] parms = {
                ":ld_last_date", ld_last_date
            };

            string sql =
@"
SELECT TO_DATE(MAX(AI2_YMD),'YYYYMMDD') AS LD_LAST_DATE FROM CI.AI2
 WHERE AI2_YMD < TO_CHAR(:LD_LAST_DATE,'YYYYMMDD')
   AND AI2_PROD_TYPE = 'F'
    AND AI2_SUM_TYPE = 'D'
   AND AI2_SUM_SUBTYPE = '1'
";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count == 0) {

                return DateTime.MinValue;
            }
            else {
                return dtResult.Rows[0]["LD_LAST_DATE"].AsDateTime();
            }
        }
    }
}
