using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/15
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D42012: DataGate {

        /// <summary>
        /// 股票期貨風險價格係數分析表
        /// </summary>
        /// <param name="as_ymd_fm"></param>
        /// <param name="as_ymd_to"></param>
        /// <param name="as_sid"></param>
        /// <param name="as_30_rate_level1"></param>
        /// <param name="as_30_rate_level2"></param>
        /// <param name="as_30_rate_level3"></param>
        /// <param name="as_30_rate_levelz"></param>
        /// <param name="as_day_rate_level1"></param>
        /// <param name="as_day_rate_level2"></param>
        /// <param name="as_day_rate_level3"></param>
        /// <param name="as_day_rate_levelz"></param>
        /// <returns></returns>
        public DataTable d_42012_detl(string as_ymd_fm, string as_ymd_to, string as_sid, decimal as_30_rate_level1,
                                      decimal as_30_rate_level2, decimal as_30_rate_level3, decimal as_30_rate_levelz,
                                      decimal as_day_rate_level1, decimal as_day_rate_level2, decimal as_day_rate_level3, decimal as_day_rate_levelz) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("as_today",as_ymd_fm),
            new DbParameterEx("as_last_date",as_ymd_to),
            new DbParameterEx("as_sid",as_sid),
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

            string sql = "CI.SP_H_TXN_42012_DETL";

            return db.ExecuteStoredProcedureEx(sql, parms, true);
        }
    }
}
