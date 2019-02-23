using OnePiece;
using System.Data;
using System;

namespace DataObjects.Dao.Together {
   public class AOCF {
      private Db db;

      public AOCF() {
         db = GlobalDaoSetting.DB;
      }

      public DataTable GetData(string symd , string eymd) {
         object[] parms = {
                "@as_symd",symd,
                "@as_eymd",eymd,
            };

         #region sql

         string sql =
             @"
                    SELECT  *
                    FROM    CI.AOCF
                    WHERE OCF_YMD >= @as_symd
                    AND OCF_YMD <= @as_eymd
                ";

         #endregion sql

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      public int GetAOCFDates(string symd , string eymd) {
         int count = 0;
         DataTable dt = GetData(symd , eymd);

         count = dt.Rows.Count;

         return count;
      }

        /// <summary>
        /// Get Max Date (如果不存在則回傳DateTime.MinValue)
        /// </summary>
        /// <param name="is_fm_ymd">yyyyMMdd,假如為空字串則不當成過濾條件</param>
        /// <param name="is_to_ymd">yyyyMMdd,假如為空字串則不當成過濾條件</param>
        /// <returns></returns>
        public DateTime GetMaxDate(string is_fm_ymd = "", string is_to_ymd = "") {
            object[] parms = {
                ":is_fm_ymd",is_fm_ymd,
                ":is_to_ymd",is_to_ymd,
            };

            string filter = "";
            if (is_fm_ymd != "")
                filter += " and OCF_YMD >= :is_fm_ymd ";
            if (is_to_ymd != "")
                filter += " and OCF_YMD < :is_to_ymd ";
            if (filter.Length > 0)
                filter = " where " + filter.Substring(5);

            string sql = string.Format(@"
SELECT NVL(MAX(OCF_YMD),'') as is_max_ymd
FROM CI.AOCF
{0}
", filter);

            string tmp = db.ExecuteScalar(sql, CommandType.Text, parms);
            DateTime.TryParseExact(tmp, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out DateTime result);
            return result;

        }

        /// <summary>
        /// Get Min Date (如果不存在則回傳DateTime.MinValue)
        /// </summary>
        /// <param name="ls_ymd">yyyyMMdd</param>
        /// <param name="compareStr">只能是>= 或 = 或 <=</param>
        /// <returns></returns>
        public DateTime GetMinDate(string ls_ymd, string compareStr = ">=") {
            object[] parms = {
                ":ls_ymd",ls_ymd
            };

            if (compareStr.Length > 2)
                compareStr = ">=";

            string sql = string.Format(@"
SELECT NVL(MIN(OCF_YMD),'') as ls_min_ymd
FROM ci.AOCF
WHERE OCF_YMD {0} :ls_ymd 
", compareStr);

            string tmp = db.ExecuteScalar(sql, CommandType.Text, parms);
            DateTime.TryParseExact(tmp, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out DateTime result);
            return result;

        }

        /// <summary>
        /// get min(ocf_ymd) + max(ocf_ymd)
        /// </summary>
        /// <param name="ls_fm_ymd">yyyyMMdd</param>
        /// <param name="ls_to_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable ListOcfYmd(string ls_fm_ymd, string ls_to_ymd) {

            object[] parms = {
                ":ls_fm_ymd",ls_fm_ymd,
                ":ls_to_ymd",ls_to_ymd
            };

            string sql = @"
select min(ocf_ymd) as min_ymd ,
max(ocf_ymd) as max_ymd 
from ci.aocf
where ocf_ymd >= :ls_fm_ymd
and ocf_ymd <= :ls_to_ymd
";

            return db.GetDataTable(sql, parms);

        }

        /// <summary>
        /// get min(ocf_ymd) + max(ocf_ymd)
        /// </summary>
        /// <param name="ls_fm_ymd">yyyyMMdd</param>
        /// <param name="ls_to_ymd">yyyyMMdd</param>
        /// <param name="ls_t_day">1 or 2</param>
        /// <returns></returns>
        public DataTable ListOcfYmdByT(string ls_fm_ymd, string ls_to_ymd, int ls_t_day = 1) {

            object[] parms = {
                ":ls_fm_ymd",ls_fm_ymd,
                ":ls_to_ymd",ls_to_ymd,
                ":ls_t_day",ls_t_day
            };

            string sql = @"
select min(ocf_ymd) as min_ymd ,
max(ocf_ymd) as max_ymd
from ci.aocf
where ocf_ymd >= ci.relativemonth(substr(:ls_fm_ymd,1,6),:ls_t_day,'MONTH')||'01'
and ocf_ymd <= ci.relativemonth(substr(:ls_to_ymd,1,6),:ls_t_day,'MONTH')||'31'
";

            return db.GetDataTable(sql, parms);

        }

    }
}