using OnePiece;
using System.Data;

/// <summary>
/// Lukas, 2018/12/26
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    /// <summary>
    /// for W55040
    /// </summary>
    public class D55040 {

        private Db db;

        public D55040() {
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
SELECT a.*
FROM (SELECT CI.FEETXO.FEETXO_YM,   
         CI.FEETXO.FEETXO_FCM_NO, FEETXO_ACC_NO,  
         CI.FEETXO.FEETXO_KIND_ID,   
         CI.FEETXO.FEETXO_RATE  ,  
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = FEETXO_FCM_NO ) as BRK_ABBR_NAME,  
         (select NVL(min(RPT.RPT_SEQ_NO),0)
            from ci.RPT
           where RPT.RPT_TXD_ID = '55040'  
             and trim(RPT.RPT_VALUE) = trim(FEETXO_KIND_ID)) as RPT_SEQ_NO
    FROM CI.FEETXO  
    WHERE CI.FEETXO.FEETXO_YM = :as_ym
    Order By feetxo_fcm_no,
             feetxo_acc_no,
             rpt_seq_no) a
WHERE a.rpt_seq_no <> 0
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        /// <summary>
        /// d55040_AMM0
        /// </summary>
        /// <returns></returns>
        public DataTable ListByDate_AMM0(string as_ym) {

            object[] parms = {
                "@as_ym",as_ym
            };

            string sql =
                @"
SELECT AMM0_YMD,   
         AMM0_BRK_NO,AMM0_ACC_NO,
         AMM0_PARAM_KEY as AMM0_KIND_ID,
         AMM0_RQ_RATE / 100 as MMK_RATE         
    FROM ci.AMM0
    WHERE AMM0_YMD = :as_ym AND
         AMM0_SUM_TYPE = 'M'  AND  
         AMM0_SUM_SUBTYPE = '3'   AND  
         AMM0_DATA_TYPE = 'Q'AND
         AMM0_PARAM_KEY = 'TXO'
    Order By amm0_brk_no,
             amm0_acc_no
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
