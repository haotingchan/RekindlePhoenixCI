using OnePiece;
using System.Data;

/// <summary>
/// ken 2018/12/24
/// </summary>
namespace DataObjects.Dao.Together {
    /// <summary>
    /// APDK_PARAM商品資訊?
    /// </summary>
    public class APDK_PARAM {
        private Db db;

        public APDK_PARAM() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// CI.APDK_PARAM (已經固定一些過濾條件)
        /// </summary>
        /// <returns>前面[全部]+PARAM_KEY/PARAM_NAME/PARAM_PROD_TYPE/PARAM_PROD_SUBTYPE</returns>
        public DataTable ListAll() {

            string sql = @"
SELECT PARAM_KEY,   
PARAM_NAME,   
PARAM_PROD_TYPE,   
PARAM_PROD_SUBTYPE  
FROM CI.APDK_PARAM  
where PARAM_EXPIRY_CODE <> 'E'
union all
select '%','全部',' ',' ' from dual 
order by param_prod_type , param_key";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// CI.APDK_PARAM (已經固定一些過濾條件)
        /// </summary>
        /// <returns>前面[% – 全部]+PARAM_KEY/PARAM_NAME/PARAM_PROD_TYPE/cp_display</returns>
        public DataTable ListAll2() {

            string sql = @"
SELECT A.*,
(CASE WHEN TRIM(PARAM_KEY) IS NULL THEN TRIM(PARAM_NAME)
     ELSE TRIM(PARAM_KEY)||' – '||TRIM(PARAM_NAME) END) AS CP_DISPLAY
FROM (SELECT PARAM_KEY,   
       PARAM_NAME,   
       PARAM_PROD_TYPE,   
       PARAM_PROD_SUBTYPE  
      FROM CI.APDK_PARAM  
      WHERE PARAM_EXPIRY_CODE <> 'E'
      UNION ALL
      SELECT '%','全部',' ',' ' FROM DUAL) A
ORDER BY PARAM_PROD_TYPE , PARAM_KEY
";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// for W20430
        /// </summary>
        /// <returns></returns>
        public DataTable ListAllForInsert() {

            string sql = @"
SELECT B.*
FROM
    (SELECT A.*,
    (CASE WHEN TRIM(PARAM_KEY) IS NULL THEN TRIM(PARAM_NAME)
         ELSE TRIM(PARAM_KEY)||' – '||TRIM(PARAM_NAME) END) AS CP_DISPLAY
    FROM (SELECT PARAM_KEY,   
           PARAM_NAME,   
           PARAM_PROD_TYPE,   
           PARAM_PROD_SUBTYPE  
          FROM CI.APDK_PARAM  
          WHERE PARAM_EXPIRY_CODE <> 'E'
          UNION ALL
          SELECT '%','全部',' ',' ' FROM DUAL) A
    ORDER BY PARAM_PROD_TYPE , PARAM_KEY) B
WHERE trim(PARAM_KEY) not in ('%','ETF','ETC','MXF')
ORDER BY PARAM_PROD_TYPE, PARAM_KEY
";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

    }
}
