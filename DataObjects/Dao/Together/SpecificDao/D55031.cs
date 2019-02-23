using OnePiece;
using System.Data;

/// <summary>
/// Lukas, 2018/12/25
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    /// <summary>
    /// for W55030
    /// </summary>
    public class D55031 {

        private Db db;

        public D55031() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// join tables
        /// </summary>
        /// <param name="as_ym">年月</param>
        /// <returns></returns>
        public DataTable ListByDate(string as_ym) {

            object[] parms = {
                "@as_ym",as_ym
            };

            string sql =
                @"
SELECT FEETRD_FCM_NO, FEETRD_ACC_NO,  
    (SELECT NVL(ABRK_NAME,'') 
    FROM ci.ABRK
    WHERE ABRK_NO = FEETRD_FCM_NO ) as BRK_ABBR_NAME,  
    FEETRD_KIND_ID,   
    (1 - FEETRD_REC_RATE) as FEETRD_RATE 
FROM CI.FEETRD
WHERE FEETRD_YM = :as_ym    
AND FEETRD_FCM_KIND = '3'
AND FEETRD_PARAM_KEY IN (SELECT PARAM_KEY FROM CI.APDK_PARAM
                                WHERE PARAM_PROD_TYPE = 'O'
                                AND PARAM_PROD_SUBTYPE = 'S')
--AND FEETRD_SESSION = :as_session
Order By feetrd_fcm_no, 
         feetrd_acc_no,
         feetrd_kind_id
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

    }
}
