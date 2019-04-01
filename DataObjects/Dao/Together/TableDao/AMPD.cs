using OnePiece;
using System.Data;

/// <summary>
/// ken 2018/12/20
/// </summary>
namespace DataObjects.Dao.Together {
    /// <summary>
    /// AMPD
    /// </summary>
    public class AMPD {
        private Db db;

        public AMPD() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// merge AMPD+ABRK
        /// </summary>
        /// <returns>第一行空白+ampd_fcm_no/abrk_name/cp_display</returns>
        public DataTable ListByFcmAccNo() {
           
            string sql = @"
select distinct AMPD_FCM_NO AS ampd_fcm_no,
abrk_name,
(case when trim(abrk_name) is null then trim(ampd_fcm_no)
     else trim(ampd_fcm_no)||'('||trim(abrk_name)||')' end) as cp_display
from
(select AMPD_FCM_NO,AMPD_ACC_NO 
   from ci.AMPD
  group by AMPD_FCM_NO,AMPD_ACC_NO),
ci.ABRK
where AMPD_FCM_NO = ABRK_NO
union all 
select ' ',' ',' ' from dual
order by ampd_fcm_no";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

       
    }
}
