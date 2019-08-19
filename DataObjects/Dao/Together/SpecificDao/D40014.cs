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
                                   MGC1_1DAY_RATE,
                                   MGC1_CP_RATE,
                                   MGCD1_R_DAY_CNT,
                                   MGC1_CUR_RATE,
                                   MGC1_RATE,
                                   MGC1_CHANGE_FLAG,
                                   DAYS_CP_ACC_CNT,
                                   DAYS_CP_COMBI_CNT,
                                   DAYS_RATE,
                                   TOP5_ACC_CNT,
                                   TOP5_COMBI_CNT,
                                   TOP5_RATE
                                   FROM CI.MGC1 M,CI.APDK ,CI.MGCD1,
                                   (
                                            SELECT  MGC1_KIND_ID,
                                                            LISTAGG(MGC1_CP_ACC_CNT, '、') WITHIN GROUP (ORDER BY MGC1_YMD) DAYS_CP_ACC_CNT ,
                                                            LISTAGG(MGC1_CP_COMBI_CNT,'、')  WITHIN GROUP (ORDER BY MGC1_YMD) DAYS_CP_COMBI_CNT,
                                                            LISTAGG(MGC1_1DAY_RATE*100 || '%' || '(' || to_char(to_date(MGC1_YMD,'yyyymmdd'),'mm/dd') || ')','、')  WITHIN GROUP (ORDER BY MGC1_YMD) DAYS_RATE
                                            FROM CI.MGC1
                                            WHERE MGC1_CP_KIND = @MGC1_CP_KIND
                                            AND MGC1_OSW_GRP like @MGC1_OSW_GRP
                                            AND MGC1_YMD >= MGC1_5DAY_YMD
                                            AND MGC1_YMD <= @MGC1_YMD
                                            GROUP BY MGC1_KIND_ID                                  
                                   )A,
                                   (
                                            SELECT            MGC5_KIND_ID2,
                                                                    LISTAGG(MGC5_ACC_CNT, '、') WITHIN GROUP (ORDER BY MGC5_SEQ_NO) AS TOP5_ACC_CNT,
                                                                    LISTAGG(MGC5_COMBI_CNT, '、') WITHIN GROUP (ORDER BY MGC5_SEQ_NO) AS TOP5_COMBI_CNT,
                                                                    LISTAGG(MGC5_COMBI_RATE*100|| '%'|| '(' || to_char(to_date(MGC5_COMBI_RATE_YMD,'yyyymmdd'),'mm/dd') || ')', '、') WITHIN GROUP (ORDER BY MGC5_SEQ_NO) AS TOP5_RATE
                                            FROM            CI.MGC5
                                            WHERE           MGC5_YMD = @MGC1_YMD
                                            GROUP BY    MGC5_KIND_ID2
                                    )B
                                   WHERE MGC1_CP_KIND = @MGC1_CP_KIND
                                   AND M.MGC1_YMD = @MGC1_YMD
                                   AND M.MGC1_OSW_GRP LIKE @MGC1_OSW_GRP
                                   AND M.MGC1_KIND_ID = TRIM(APDK_KIND_ID)
                                   AND M.MGC1_YMD = MGCD1_YMD(+)
                                   AND M.MGC1_KIND_ID = MGCD1_KIND_ID(+)
                                   AND M.MGC1_CP_KIND = MGCD1_CP_KIND(+)
                                   AND M.MGC1_KIND_ID = A.MGC1_KIND_ID(+)
                                   AND M.MGC1_KIND_ID2 = B.MGC5_KIND_ID2(+)
                                   ORDER BY M.MGC1_KIND_ID
            ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

    }
}
