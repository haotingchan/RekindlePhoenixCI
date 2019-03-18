using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/18
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D42030 : DataGate {

        /// <summary>
        /// 當日保證金適用比例
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <param name="as_prod_type">"O"</param>
        /// <returns></returns>
        public DataTable d_42030(string as_ymd) {

            object[] parms = {
                ":as_ymd",as_ymd
            };

            string sql =
@"
SELECT CASE WHEN MGR5_CP_STATUS_CODE <> 'N' THEN '*'|| to_char(Row_Number() over(ORDER BY MGR5_SID)) 
                                            ELSE to_char(Row_Number() over(ORDER BY MGR5_SID)) END as No,
         --MGR5_YMD,   
         TRIM(MGR5_SID),
         TFXMS_SNAME,
         PID_NAME,
         MGR5_DAY_RATE  ,   
         MGRT1_LEVEL_NAME,   
         MGR5_CM,   
         MGR5_MM,   
         MGR5_IM,
         DECODE(MGR5_FUT_KIND_ID,NULL,' ','Y') AS MGR5_FUT_KIND_ID,   
         DECODE(MGR5_OPT_KIND_ID,NULL,' ','Y') AS MGR5_OPT_KIND_ID  
         --MGR5_LEVEL,
         --MGR5_CP_STATUS_CODE
    FROM CI.MGR5,   
         CI.MGRT1,
         CI.TFXMS,
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) AS COD_ID,TRIM(COD_DESC) AS PID_NAME FROM CI.COD WHERE COD_TXN_ID = 'TFXM')
   WHERE MGR5_YMD = :AS_YMD
     AND MGR5_LEVEL = MGRT1_LEVEL AND MGRT1_PROD_TYPE ='F'
     AND MGR5_SID = TFXMS_SID
     AND MGR5_PID = COD_ID
     --AND MGR5_PID = 1
   ORDER BY MGR5_SID
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 確認當日上市證券保證金適用比例資料是否已轉入
        /// </summary>
        /// <param name="ls_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public int mgr5Count(string ls_ymd) {

            object[] parms = {
                ":ls_ymd",ls_ymd
            };

            string sql =
@"
SELECT COUNT(*) AS MGR5COUNT 
  FROM CI.MGR5
 WHERE MGR5_YMD = :LS_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0)
                return dtResult.Rows[0]["MGR5COUNT"].AsInt();
            else
                return 0;
        }
    }
}
