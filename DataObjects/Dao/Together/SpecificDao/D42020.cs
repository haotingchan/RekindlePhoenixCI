using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/15
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D42020 : DataGate {

        /// <summary>
        /// 保證金適用比例級距
        /// </summary>
        /// <param name="as_prod_type">"O"</param>
        /// <returns></returns>
        public DataTable d_42020_mgrt1(string as_prod_type) {

            object[] parms = {
                ":as_prod_type", as_prod_type
            };

            string sql =
@"
SELECT
    MGRT1_LEVEL_NAME,
    MGRT1_CM_RATE,
    MGRT1_CM_RATE_B, 
    MGRT1_MM_RATE,
    MGRT1_MM_RATE_B, 
    MGRT1_IM_RATE,
    MGRT1_IM_RATE_B
FROM CI.MGRT1
WHERE MGRT1_PROD_TYPE = :as_prod_type
  and MGRT1_REPORT = 'Y'
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 當日保證金適用比例
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <param name="as_prod_type">"O"</param>
        /// <returns></returns>
        public DataTable d_42020(string as_ymd, string as_prod_type) {

            object[] parms = {
                ":as_ymd",as_ymd,
                ":as_prod_type", as_prod_type
            };

            string sql =
@"
SELECT   ROW_NUMBER() over (order by apdk_kind_grp2, apdk_kind_level, mgr3_kind_id) as NO,
         TRIM(MGR3_KIND_ID), 
         TRIM(APDK_NAME), 
         TRIM(MGR3_SID),
         PID_NAME,
         MGR3_RATE, 
         MGR3_CM,   
         MGR3_CUR_CM
         --MGR3_YMD,   
         --MGR3_PROD_TYPE,   
         --MGR3_PARAM_KEY,
         --MGR3_LEVEL,   
         --MGR3_STATUS,   
         --MGRT1.MGRT1_LEVEL_NAME,
         --MGRT1L.MGRT1_LEVEL_NAME as MGRT1_LAST_LEVEL_NAME,
         --NVL(MGRT1L.MGRT1_LEVEL,'Z') as MGR3_CUR_LEVEL,
         --MGR3_LAST_LEVEL,
         --MGR3_PID,
         --APDK_KIND_GRP2,APDK_KIND_LEVEL
    FROM CI.MGR3,   
         CI.APDK,   
         CI.MGRT1 MGRT1,CI.MGRT1 MGRT1L,
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
   WHERE MGR3_KIND_ID = APDK_KIND_ID   
     and MGRT1.MGRT1_PROD_TYPE = 'O'   
     and MGR3_LEVEL = MGRT1.MGRT1_LEVEL   
     and MGRT1L.MGRT1_PROD_TYPE(+) = 'O'   
     and MGR3_CUR_CM = MGRT1L.MGRT1_CM_RATE(+)   
     and MGR3_YMD = :as_ymd    
     AND MGR3_PROD_TYPE = :as_prod_type 
     and MGR3_PID = COD_ID
   ORDER BY apdk_kind_grp2, apdk_kind_level, mgr3_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 當日保證金適用比例(用來計算級距調整過的期貨)
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <param name="as_prod_type">"O"</param>
        /// <returns></returns>
        public DataTable d_42020Compute(string as_ymd, string as_prod_type) {

            object[] parms = {
                ":as_ymd",as_ymd,
                ":as_prod_type", as_prod_type
            };

            string sql =
@"
SELECT
         MGR3_KIND_ID, 
         APDK_NAME, 
         MGR3_SID,
         PID_NAME,
         MGR3_RATE, 
         MGR3_CM,   
         MGR3_CUR_CM,
         --MGR3_YMD,   
         --MGR3_PROD_TYPE,   
         --MGR3_PARAM_KEY,
         MGR3_LEVEL,   
         MGR3_STATUS,   
         MGRT1.MGRT1_LEVEL_NAME,
         MGRT1L.MGRT1_LEVEL_NAME as MGRT1_LAST_LEVEL_NAME,
         NVL(MGRT1L.MGRT1_LEVEL,'Z') as MGR3_CUR_LEVEL
         --MGR3_LAST_LEVEL,
         --MGR3_PID,
         --APDK_KIND_GRP2,APDK_KIND_LEVEL
    FROM CI.MGR3,   
         CI.APDK,   
         CI.MGRT1 MGRT1,CI.MGRT1 MGRT1L,
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
   WHERE MGR3_KIND_ID = APDK_KIND_ID   
     and MGRT1.MGRT1_PROD_TYPE = 'O'   
     and MGR3_LEVEL = MGRT1.MGRT1_LEVEL   
     and MGRT1L.MGRT1_PROD_TYPE(+) = 'O'   
     and MGR3_CUR_CM = MGRT1L.MGRT1_CM_RATE(+)   
     and MGR3_YMD = :as_ymd    
     AND MGR3_PROD_TYPE = :as_prod_type 
     and MGR3_PID = COD_ID
   ORDER BY apdk_kind_grp2, apdk_kind_level, mgr3_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 確認當日保證金適用比例資料是否已轉入
        /// </summary>
        /// <param name="ls_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public int mgr3Count(string ls_ymd) {

            object[] parms = {
                ":ls_ymd",ls_ymd
            };

            string sql =
@"
SELECT COUNT(*) as MGR3COUNT 
  FROM CI.MGR3
 WHERE MGR3_YMD = :LS_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0)
                return dtResult.Rows[0]["MGR3COUNT"].AsInt();
            else
                return 0;
        }
    }
}
