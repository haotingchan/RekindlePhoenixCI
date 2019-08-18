using BusinessObjects;
using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class D40014:DataGate
    {

        public DataTable ListLevelData(string MGCT1_CP_KIND)
        {
            object[] parms = {
                "@MGCT1_CP_KIND",MGCT1_CP_KIND
            };
            #region sql

            string sql =
            @"   
                    SELECT MGCT1_LEVEL,
                                CASE MGCT1_MAX WHEN 999 THEN MGCT1_MIN * 100 || '%以上' 
                                ELSE  MGCT1_MIN * 100 || '%(含)~' || MGCT1_MAX * 100 || '%(不含)' END AS MGCT1_RANGE,
                                'B值×' || MGCT1_VAL_COMBI_RATE * 100 || '%' AS MGCT1_VAL_COMBI_RATE,
                                'b%×' || MGCT1_PCT_COMBI_RATE * 100 || '%' AS MGCT1_PCT_COMBI_RATE
                    FROM ci.MGCT1 
                    WHERE MGCT1_CP_KIND =  @MGCT1_CP_KIND
            ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListData(string MGC1_CP_KIND, string MGC1_YMD,string MGC1_OSW_GRP)
        {
            object[] parms = {
                "@MGC1_CP_KIND",MGC1_CP_KIND,
                "@MGC1_YMD",MGC1_YMD,
                "@MGC1_OSW_GRP",MGC1_OSW_GRP
            };
            #region sql

            string sql =
            @"   
                      SELECT M.MGC1_KIND_ID,
                                   APDK_NAME,
                                   MGC1_CP_ACC_CNT,
                                   MGC1_CP_COMBI_CNT,
                                   MGC1_NATURE_COMBI_CNT,
                                   MGC1_NATURE_OI,
                                   MGC1_CP_RATE,
                                   MGC1_5DAY_AVG_RATE,
                                   MGCD1_R_DAY_CNT,
                                   MGC1_CUR_RATE,
                                   MGC1_RATE,
                                   MGC1_CHANGE_FLAG,
                                   DAYS_CP_ACC_CNT,
                                   DAYS_CP_COMBI_CNT,
                                   DAYS_CP_RATE
                                   FROM CI.MGC1 M,CI.APDK ,CI.MGCD1,
                                   (
                                           SELECT MGC1_KIND_ID,
                                            WMSYS.WM_CONCAT(MGC1_CP_ACC_CNT) DAYS_CP_ACC_CNT ,
                                            WMSYS.WM_CONCAT(MGC1_CP_COMBI_CNT) DAYS_CP_COMBI_CNT,
                                            WMSYS.WM_CONCAT(MGC1_CP_RATE*100 || '%' || '(' || to_char(to_date(MGC1_YMD,'yyyymmdd'),'mm/dd') || ')') DAYS_CP_RATE
                                            FROM
                                            (
                                               SELECT row_number() over (partition BY MGC1_KIND_ID order by MGC1_YMD) rownumber ,
                                               MGC1_YMD,MGC1_KIND_ID, MGC1_CP_ACC_CNT,MGC1_CP_COMBI_CNT,MGC1_CP_RATE
                                               FROM ci.MGC1 
                                               WHERE MGC1_CP_KIND = @MGC1_CP_KIND
                                               AND MGC1_OSW_GRP like @MGC1_OSW_GRP
                                               AND MGC1_YMD >= MGC1_5DAY_YMD
                                               AND MGC1_YMD <= @MGC1_YMD
                                           )
                                           GROUP BY MGC1_KIND_ID                                   
                                   )A
                                   WHERE MGC1_CP_KIND = @MGC1_CP_KIND
                                   AND M.MGC1_YMD = @MGC1_YMD
                                   AND M.MGC1_OSW_GRP LIKE @MGC1_OSW_GRP
                                   AND M.MGC1_KIND_ID = APDK_KIND_ID
                                   AND M.MGC1_YMD = MGCD1_YMD(+)
                                   AND M.MGC1_KIND_ID = MGCD1_KIND_ID(+)
                                   AND M.MGC1_CP_KIND = MGCD1_CP_KIND(+)
                                   AND M.MGC1_KIND_ID = A.MGC1_KIND_ID(+)
                                   ORDER BY M.MGC1_KIND_ID
            ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

    }
}
