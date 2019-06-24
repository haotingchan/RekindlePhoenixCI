using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/6/24
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D20310: DataGate {

        /// <summary>
        /// Taifex指數期貨總成交值
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="eym"></param>
        /// <returns></returns>
        public DataTable TotalAMT(string eym) {

            object[] parms = {
                ":eym", eym
            };

            string sql =
@"
SELECT ROUND(SUM(AA2_AMT)/2/100000000,0) as TotalAMT
  FROM ci.AA2,ci.APDK
 WHERE AA2_YMD >= :eym||'01'
  AND AA2_YMD <= :eym||'31'
  AND AA2_PROD_TYPE = 'F'
  AND AA2_PROD_SUBTYPE = 'I'
  AND AA2_PARAM_KEY = APDK_KIND_ID
  AND APDK_UNDERLYING_MARKET IN ('1','2')
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 月交易天數
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="eym"></param>
        /// <returns></returns>
        public DataTable OCFDaysCnt(string eym) {

            object[] parms = {
                ":eym", eym
            };

            string sql =
@"
SELECT COUNT(*) as dayscnt,
       MAX(OCF_YMD) as maxday
  FROM ci.AOCF
 WHERE OCF_YMD >= :eym||'01'
   AND OCF_YMD <= :eym||'31'
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 美元匯率 (當月有交易之日期)
        /// </summary>
        /// <param name="ymd"></param>
        /// <returns></returns>
        public DataTable ExchangeRate(string ymd) {

            object[] parms = {
                ":ymd", ymd
            };

            string sql =
@"
SELECT HEXRT_EXCHANGE_RATE
  FROM ci.HEXRT
 WHERE HEXRT_DATE = TO_DATE(:ymd,'YYYYMMDD')
   AND HEXRT_CURRENCY_TYPE = '2' AND HEXRT_COUNT_CURRENCY = '1'
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
