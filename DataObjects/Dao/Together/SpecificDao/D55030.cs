using OnePiece;
using System.Data;

/// <summary>
/// Lukas, 2018/12/25
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    /// <summary>
    /// for W55030
    /// </summary>
    public class D55030 {

        private Db db;

        public D55030() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// join tables
        /// </summary>
        /// <param name="as_ym">年月</param>
        /// <returns></returns>
        public DataTable ListByDate(string as_ym, string as_session="0") {

            object[] parms = {
                ":as_ym",as_ym,
                ":as_session", as_session
            };

            string sql =
                @"
SELECT a.*
FROM (SELECT FEETRD_FCM_NO,FEETRD_ACC_NO,   
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = FEETRD_FCM_NO ) as BRK_ABBR_NAME,  
         FEETRD_KIND_ID,   
         (1 - FEETRD_REC_RATE) as FEETRD_RATE ,
         (select nvl(min(RPT.RPT_SEQ_NO),0)
            from ci.RPT
           where RPT.RPT_TXD_ID = '55040'  
             and RPT.RPT_VALUE = FEETRD_KIND_ID) as RPT_SEQ_NO
        FROM CI.FEETRD
        WHERE FEETRD_YM = :as_ym    
         AND FEETRD_FCM_KIND = '3'
         AND (FEETRD_PARAM_KEY = FEETRD_KIND_ID or FEETRD_KIND_ID = 'TXW')
         AND FEETRD_SESSION = :as_session
        ) a
WHERE a.rpt_seq_no <> 0
Order By feetrd_fcm_no,
             feetrd_acc_no,
             rpt_seq_no
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }
    }
}
