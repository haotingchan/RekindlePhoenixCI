using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.TableDao {
    public class MOCF : DataGate {


        public string GetMaxOcfDate(string beginDate, string endDate) {

            object[] parms = {
                ":begin_ymd", beginDate,
                ":end_ymd", endDate
            };

            string sql = @"SELECT MAX(MOCF_YMD) as ls_mocf_ymd
                           FROM ci.MOCF 
                           WHERE MOCF_YMD >= :begin_ymd
                           AND MOCF_YMD < :end_ymd
                           AND MOCF_OPEN_CODE = 'Y'";

            return db.ExecuteScalar(sql, CommandType.Text, parms);
        }

        public DataTable GetStartEndDate(string beginDate, string endDate) {
            object[] parms = {
                ":ls_impl_begin_ymd", beginDate,
                ":ls_end_ymd", endDate
            };

            string sql = @"SELECT MIN(MOCF_YMD) as ls_start_ymd,MAX(MOCF_YMD) as ls_end_ymd
                           FROM ci.MOCF 
                           WHERE MOCF_YMD > :ls_impl_begin_ymd
                           AND MOCF_YMD <= :ls_end_ymd";

            return db.GetDataTable(sql, parms);

        }

        public string GetValidDatePrev(string validDate) {

            object[] parms = {
                ":validDate", validDate
            };

            string sql = @"SELECT MAX(MOCF_YMD) as validDatePrev  
FROM ci.MOCF 
WHERE MOCF_YMD < :validDate 
AND MOCF_OPEN_CODE = 'Y' ";

            return db.ExecuteScalar(sql, CommandType.Text, parms);
        }

        /// <summary>
        /// for f_get_ocf_next_n_day
        /// </summary>
        /// <param name="ls_symd"></param>
        /// <param name="ls_eymd"></param>
        /// <param name="day_cnt"></param>
        /// <returns></returns>
        public DateTime GetSpecOcfDay(string ls_symd, string ls_eymd, int day_cnt) {

            object[] parms = {
                ":ls_symd", ls_symd,
                ":ls_eymd", ls_eymd,
                ":day_cnt", day_cnt
            };

            string sql = @"
SELECT to_date(MOCF_YMD,'yyyy/mm/dd') as LDT_NEXT_DATE 
FROM (
	 SELECT ROW_NUMBER() OVER (ORDER BY MOCF_YMD) AS NUM,CI.MOCF.* 
	 FROM CI.MOCF 
	 WHERE MOCF_YMD > :ls_symd
	 AND MOCF_YMD <= :ls_eymd
	 AND MOCF_OPEN_CODE = 'Y'
)M
WHERE NUM = :day_cnt";

            DataTable dtResult = db.GetDataTable(sql, parms);

            if (dtResult.Rows.Count == 0) {
                return DateTime.MinValue;
            }
            else {
                return dtResult.Rows[0]["LDT_NEXT_DATE"].AsDateTime();
            }
        }

        /// <summary>
        /// for f_get_ocf_next_n_day
        /// </summary>
        /// <param name="ls_symd"></param>
        /// <param name="ls_eymd"></param>
        /// <param name="day_cnt"></param>
        /// <returns></returns>
        public DateTime GetSpecOcfDay2(string ls_symd, string ls_eymd, int day_cnt) {

            object[] parms = {
                ":ls_symd", ls_symd,
                ":ls_eymd", ls_eymd,
                ":day_cnt", day_cnt
            };

            string sql = @"
SELECT to_date(MOCF_YMD,'yyyy/mm/dd') as LDT_NEXT_DATE 
FROM (
	 SELECT ROW_NUMBER() OVER (ORDER BY MOCF_YMD DESC) AS NUM,CI.MOCF.* 
	 FROM CI.MOCF 
	 WHERE MOCF_YMD >= :ls_symd
	 AND MOCF_YMD < :ls_eymd
	 AND MOCF_OPEN_CODE = 'Y'
)M
WHERE NUM = :day_cnt";

            DataTable dtResult = db.GetDataTable(sql, parms);

            if (dtResult.Rows.Count == 0) {
                return DateTime.MinValue;
            }
            else {
                return dtResult.Rows[0]["LDT_NEXT_DATE"].AsDateTime();
            }
        }

        /// <summary>
        /// for W40072
        /// </summary>
        /// <param name="ls_impl_begin_ymd"></param>
        /// <param name="ls_mocf_ymd"></param>
        /// <returns></returns>
        public string GetNextTradeDay(string ls_impl_begin_ymd, string ls_mocf_ymd) {

            object[] parms = {
                ":ls_impl_begin_ymd", ls_impl_begin_ymd,
                ":ls_mocf_ymd", ls_mocf_ymd
            };

            string sql = @"
SELECT min(MOCF_YMD) as ls_next_ymd
				FROM ci.MOCF
				 WHERE MOCF_YMD > :ls_impl_begin_ymd
					AND MOCF_YMD  <= :ls_mocf_ymd
					AND MOCF_OPEN_CODE = 'Y'";

            DataTable dtResult = db.GetDataTable(sql, parms);

            if (dtResult.Rows.Count == 0) {
                return null;
            }
            else {
                return dtResult.Rows[0]["LS_NEXT_YMD"].ToString();
            }
        }

        /// <summary>
        /// for W40074
        /// </summary>
        /// <param name="ls_impl_begin_ymd"></param>
        /// <param name="ls_mocf_ymd"></param>
        /// <returns></returns>
        public string GetPrevTradeDay(string ls_ymd, string ls_mocf_ymd) {

            object[] parms = {
                ":ls_ymd", ls_ymd,
                ":ls_mocf_ymd", ls_mocf_ymd
            };

            string sql = @"
SELECT max(MOCF_YMD) as is_pre_ymd
FROM ci.MOCF
 WHERE MOCF_YMD < :ls_ymd
	 AND MOCF_YMD  >= :ls_mocf_ymd
	 AND MOCF_OPEN_CODE = 'Y'
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            if (dtResult.Rows.Count == 0) {
                return null;
            }
            else {
                return dtResult.Rows[0]["IS_PRE_YMD"].ToString();
            }
        }
    }
}
