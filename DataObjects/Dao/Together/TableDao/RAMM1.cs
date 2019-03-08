using BusinessObjects;
using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together {
    public class RAMM1 {
        private Db db;

        public RAMM1() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// 3系列取得造市者交易量
        /// </summary>
        /// <param name="symd"> Start Date</param>
        /// <param name="eymd"> End Date</param>
        /// <param name="paramKey"> apdk_param_key</param>
        /// <returns></returns>
        public DataTable ListRamm1Ymd(string symd, string eymd, string paramKey) {
            object[] parms = {
                ":as_sym", symd,
                ":as_eym", eymd,
                ":as_param_key", paramKey
            };

            string sql = @" SELECT APDK_KIND_ID2 as PARAM_KEY,  
         SUM(RAMM1_BO_QNTY) as BO,   
         SUM(RAMM1_BQ_QNTY) as BQ,   
         SUM(RAMM1_SO_QNTY) as SO,   
         SUM(RAMM1_SQ_QNTY) as SQ  
    FROM ci.RAMM1,ci.APDK  
   WHERE trim(RAMM1_YMD) >= :as_sym    
     and trim(RAMM1_YMD) <= :as_eym
     AND RAMM1_SOURCE = 'O'    
     AND RAMM1_SUM_TYPE = 'D'
     and RAMM1_KIND_ID = APDK_KIND_ID
     and TRIM(APDK_PARAM_KEY) = :as_param_key
GROUP BY APDK_KIND_ID2";

            return db.GetDataTable(sql, parms);
        }
    }
}
