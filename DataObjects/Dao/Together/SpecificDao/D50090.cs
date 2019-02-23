using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D50090 {

        private Db db;

        public D50090() {

            db = GlobalDaoSetting.DB;

        }

        public DataTable GetData(string as_ym) {

            object[] parms = {
                "@as_ym", as_ym,
            };

            string sql = @"SELECT AMM0_YMD,
                           AMM0_BRK_NO,
                           AMM0_ACC_NO,
                           AMM0_KIND_ID2,
                           AMM0_RQ_RATE / 100 as AMM0_RATE,
                           AMM0_VALID_CNT,
                           AMM0_MARKET_R_CNT, 
                           AMM0_KEEP_FLAG,
                           AMM0_KEEP_TIME,
                           AMM0_DAY_COUNT,
                           AMMF_AVG_TIME as MMF_AVG_TIME,
                           AMM0_PROD_TYPE,
                           AMMF_QNTY_LOW as MMF_QNTY_LOW,
                           nvl(AMM0_MM_QNTY,0) + nvl(o_mm_qnty,0) as AMM0_QNTY1 ,
                           nvl(AMM0_MAX_MM_QNTY,0) + nvl(o_max_mm_qnty,0) as AMM0_QNTY2,
                           AMM0_RESULT,AMM0_VALID_RESULT,
                           AMM0_TRD_INVALID_QNTY,
                           AMMF_MARKET_CODE AS MARKET_CODE
                           FROM ci.AMM0 ,ci.AMMF,
                           (select AMM0_BRK_NO as o_brk_no,AMM0_ACC_NO as o_acc_no,AMM0_KIND_ID2 as o_kind_id2,AMM0_MM_QNTY as o_mm_qnty,AMM0_MAX_MM_QNTY as o_max_mm_qnty 
                            FROM ci.AMM0AH 
                            WHERE AMM0_DATA_TYPE  = 'Q'
                              AND AMM0_SUM_TYPE   = 'M'
                              AND AMM0_SUM_SUBTYPE = '4'
                              AND AMM0_YMD = :as_ym
                              AND AMM0_PARAM_KEY = 'TXO')
                            WHERE AMM0_DATA_TYPE  = 'Q'
                              AND AMM0_SUM_TYPE   = 'M'
                              AND AMM0_SUM_SUBTYPE = '4'
                              AND AMM0_YMD = :as_ym
                              AND AMM0_PARAM_KEY = AMMF_PARAM_KEY
	                          AND AMM0_YMD = TRIM(AMMF_YM)||'  '
                              AND AMMF_MARKET_CODE = '0'
                              AND AMM0_BRK_NO = o_brk_no(+)
                              AND AMM0_ACC_NO = o_acc_no(+)
                              AND AMM0_KIND_ID2 = o_kind_id2(+)
                            UNION ALL 
                            SELECT AMM0_YMD,
                            AMM0_BRK_NO,
                            AMM0_ACC_NO,
                            TRIM(AMM0_PARAM_KEY)||'*' AS AMM0_KIND_ID2,
                            AMM0_RQ_RATE / 100 as AMM0_RATE,
                            AMM0_VALID_CNT,
                            AMM0_MARKET_R_CNT, 
                            AMM0_KEEP_FLAG,
                            AMM0_KEEP_TIME,
                            AMM0_DAY_COUNT,
                            AMMF_AVG_TIME as MMF_AVG_TIME,
                            AMM0_PROD_TYPE,
                            AMMF_QNTY_LOW as MMF_QNTY_LOW,
                            nvl(AMM0_MM_QNTY,0) + nvl(o_mm_qnty,0) as AMM0_QNTY1 ,
                            nvl(AMM0_MAX_MM_QNTY,0) + nvl(o_max_mm_qnty,0) as AMM0_QNTY2,
                            AMM0_RESULT,AMM0_VALID_RESULT,
                            AMM0_TRD_INVALID_QNTY,
                            AMMF_MARKET_CODE AS MARKET_CODE
                            FROM ci.AMM0 ,ci.AMMF ,
                            (select AMM0_BRK_NO as o_brk_no,AMM0_ACC_NO as o_acc_no,AMM0_MM_QNTY as o_mm_qnty,AMM0_MAX_MM_QNTY as o_max_mm_qnty 
                             FROM ci.AMM0AH
                             WHERE AMM0_DATA_TYPE  = 'Q'
                               AND AMM0_SUM_TYPE   = 'M'
                               AND AMM0_SUM_SUBTYPE = '3'
                               AND AMM0_YMD =  :as_ym
                               AND AMM0_PARAM_KEY = 'TXO')
                            WHERE AMM0_DATA_TYPE  = 'Q'
                              AND AMM0_SUM_TYPE   = 'M'
                              AND AMM0_SUM_SUBTYPE = '3'
                              AND AMM0_YMD = :as_ym
                              AND AMM0_PARAM_KEY = AMMF_PARAM_KEY
                              AND AMM0_PARAM_KEY = 'TXO'
	                          AND AMM0_YMD = TRIM(AMMF_YM)||'  '
                              AND AMMF_MARKET_CODE = '0'
                              AND AMM0_BRK_NO = o_brk_no(+)
                              AND AMM0_ACC_NO = o_acc_no(+)
                            UNION ALL
                            SELECT AMM0_YMD,
                            AMM0_BRK_NO,
                            AMM0_ACC_NO,
                            AMM0_KIND_ID2,
                            AMM0_RQ_RATE / 100 as AMM0_RATE,
                            AMM0_VALID_CNT,
                            AMM0_MARKET_R_CNT, 
                            AMM0_KEEP_FLAG,
                            AMM0_KEEP_TIME,
                            AMM0_DAY_COUNT,
                            AMMF_AVG_TIME as MMF_AVG_TIME,
                            AMM0_PROD_TYPE,
                            AMMF_QNTY_LOW as MMF_QNTY_LOW,
                            nvl(AMM0_MM_QNTY,0) as AMM0_QNTY1 ,
                            nvl(AMM0_MAX_MM_QNTY,0) as AMM0_QNTY2,
                            AMM0_RESULT,AMM0_VALID_RESULT,
                            AMM0_TRD_INVALID_QNTY,
                            AMMF_MARKET_CODE AS MARKET_CODE
                            FROM ci.AMM0AH ,ci.AMMF
                            WHERE AMM0_DATA_TYPE  = 'Q'
                            AND AMM0_SUM_TYPE   = 'M'
                            AND AMM0_SUM_SUBTYPE = '4'
                            AND AMM0_YMD = :as_ym
                            AND AMM0_PARAM_KEY = AMMF_PARAM_KEY
                            AND AMM0_YMD = TRIM(AMMF_YM)||'  '
                            AND AMMF_MARKET_CODE = '1'
                            UNION ALL
                            SELECT AMM0_YMD,
                            AMM0_BRK_NO,
                            AMM0_ACC_NO,
                            TRIM(AMM0_PARAM_KEY)||'*' AS AMM0_KIND_ID2,
                            AMM0_RQ_RATE / 100 as AMM0_RATE,
                            AMM0_VALID_CNT,
                            AMM0_MARKET_R_CNT, 
                            AMM0_KEEP_FLAG,
                            AMM0_KEEP_TIME,
                            AMM0_DAY_COUNT,
                            AMMF_AVG_TIME as MMF_AVG_TIME,
                            AMM0_PROD_TYPE,
                            AMMF_QNTY_LOW as MMF_QNTY_LOW,
                            nvl(AMM0_MM_QNTY,0) as AMM0_QNTY1 ,
                            nvl(AMM0_MAX_MM_QNTY,0) as AMM0_QNTY2,
                            AMM0_RESULT,AMM0_VALID_RESULT,
                            AMM0_TRD_INVALID_QNTY,
                            AMMF_MARKET_CODE AS MARKET_CODE
                            FROM ci.AMM0AH ,ci.AMMF
                            WHERE AMM0_DATA_TYPE  = 'Q'
                            AND AMM0_SUM_TYPE   = 'M'
                            AND AMM0_SUM_SUBTYPE = '3'
                            AND AMM0_YMD = :as_ym
                            AND AMM0_PARAM_KEY = AMMF_PARAM_KEY
                            AND AMM0_PARAM_KEY = 'TXO'
	                        AND AMM0_YMD = TRIM(AMMF_YM)||'  '
                            AND AMMF_MARKET_CODE = '1'
                            Order By amm0_brk_no Asc, 
                            amm0_acc_no Asc,
                            amm0_kind_id2 Asc,
                            amm0_prod_type Asc,
                            market_code Asc";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
