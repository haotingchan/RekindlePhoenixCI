using BusinessObjects;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/17
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D40070: DataGate {

        public DataTable d_40070_scrn(string as_ymd, string as_cond) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("as_ymd",as_ymd),
            new DbParameterEx("as_cond",as_cond)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

            string sql = "CI.SP_H_TXN_40070_SCRN";

            return db.ExecuteStoredProcedureEx(sql, parms, true);
        }

        /// <summary>
        /// CM_B維持保證金 (kind_id,CM金額A值,0,'CM_B')
        /// MM_B維持保證金(kind_id, MM金額A值, CM金額B值,'MM_B')
        /// IM_B原始保證金(kind_id, IM金額A值, CM金額B值,'IM_B')
        /// MM維持保證金(kind_id, CM金額A值,0,'MM')
        /// IM原始保證金(kind_id, CM金額A值,0,'IM')
        /// ADJ變動幅度(kind_id, 調整後CM, 現行CM,'ADJ')
        /// </summary>
        /// <param name="as_kind_id"></param>
        /// <param name="adc_m1"></param>
        /// <param name="adc_m2"></param>
        /// <param name="as_tyep"></param>
        /// <returns></returns>
        public decimal GetMarginVal(string as_kind_id, decimal adc_m1, decimal adc_m2, string as_type) {

            decimal ldc_m;
            object[] parms = {
                ":as_kind_id", as_kind_id,
                ":adc_m1", adc_m1,
                ":adc_m2", adc_m2,
                ":as_type", as_type
            };

            string sql =
@"
SELECT ci.GetMarginVal(:as_kind_id,:adc_m1,:adc_m2,:as_type) as ldc_m
from dual
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                ldc_m = dtResult.Rows[0]["LDC_M"].AsDecimal();
                return ldc_m;
            }

        }
    }
}
