using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D60320
    {
        private Db db;

        public D60320()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData(string symd, string eymd)
        {
            object[] parms = {
                "@as_symd",symd,
                "@as_eymd",eymd
            };

            #region sql

            string sql =
                        @"
                                                 SELECT DT_EYMD AS YMD,
                                                       M_QNTY_I,M_QNTY_S,M_ACCU_QNTY + NVL(S_ACCU_QNTY,0) AS ACCU_QNTY,DAY_COUNT,
                                                       NVL(T3_QNTY,0) AS T3_QNTY,NVL(T3_QNTY,0)+NVL(T5_QNTY,0) AS T5_QNTY,
                                                       NVL(S_QNTY_I,0) AS S_QNTY_I,NVL(S_QNTY_S,0) AS S_QNTY_S,
                                                       NVL(M_AMT_T_I,0) AS M_AMT_T_I,NVL(M_AMT_I,0) AS M_AMT_I,NVL(M_AMT_T_S,0) AS M_AMT_T_S,NVL(M_AMT_S,0) AS M_AMT_S,
                                                       STWD_QNTY,STWD_AMT,AMIF_SUM_AMT
                                                  FROM
                                                       --交易量
                                                      (SELECT DT_EYMD,
                                                              M_QNTY_I,
                                                              M_QNTY_S,
                                                              SUM(AI2_M_QNTY)*2 AS M_ACCU_QNTY,
                                                              COUNT(DISTINCT AI2_YMD) AS DAY_COUNT
                                                         FROM CI.AI2,
                                                             (SELECT SUBSTR(AI2_YMD,1,6)||'01' AS DT_SYMD,
                                                                     AI2_YMD AS DT_EYMD,
                                                                     SUM(CASE WHEN AI2_PROD_SUBTYPE = 'I' THEN AI2_M_QNTY ELSE 0 END) *2 AS M_QNTY_I,
                                                                     SUM(CASE WHEN AI2_PROD_SUBTYPE = 'S' THEN AI2_M_QNTY ELSE 0 END) *2 AS M_QNTY_S
                                                                FROM CI.AI2
                                                               WHERE AI2_YMD >= @as_symd
                                                                 AND AI2_YMD <= @as_eymd
                                                                 AND AI2_SUM_TYPE = 'D'
                                                                 AND AI2_SUM_SUBTYPE = '2'
                                                                 AND AI2_PROD_TYPE = 'F'
                                                                 AND AI2_PROD_SUBTYPE IN ('I','S')
                                                               GROUP BY AI2_YMD)
                                                        WHERE AI2_YMD >= DT_SYMD
                                                          AND AI2_YMD <= DT_EYMD
                                                          AND AI2_SUM_TYPE = 'D'
                                                          AND AI2_SUM_SUBTYPE = '2'
                                                          AND AI2_PROD_TYPE = 'F'
                                                          AND AI2_PROD_SUBTYPE IN ('I','S')
                                                        GROUP BY DT_SYMD,DT_EYMD,M_QNTY_I,M_QNTY_S) Q,
                                                      (SELECT DT_EYMD AS S_EYMD,
                                                              SUM(AOI1_QNTY) AS  S_ACCU_QNTY
                                                         FROM CI.AOI1,
                                                             (SELECT SUBSTR(AI2_YMD,1,6)||'01' AS DT_SYMD,
                                                                     AI2_YMD AS DT_EYMD
                                                                FROM CI.AI2
                                                               WHERE AI2_YMD >= @as_symd
                                                                 AND AI2_YMD <= @as_eymd
                                                                 AND AI2_SUM_TYPE = 'D'
                                                                 AND AI2_SUM_SUBTYPE = '2'
                                                                 AND AI2_PROD_TYPE = 'F'
                                                                 AND AI2_PROD_SUBTYPE IN ('I','S')
                                                               GROUP BY AI2_YMD)
                                                        WHERE AOI1_YMD >= DT_SYMD
                                                          AND AOI1_YMD <= DT_EYMD
                                                          AND AOI1_DATA_TYPE = 'OI'
                                                          AND AOI1_PROD_TYPE = 'F'
                                                          AND AOI1_PROD_SUBTYPE IN ('I','S')
                                                        GROUP BY DT_SYMD,DT_EYMD) S,
                                                       --契約金額,到期交割量
                                                      (SELECT AOI1_YMD,
                                                              SUM(CASE WHEN AOI1_DATA_TYPE = 'T' AND AOI1_PROD_SUBTYPE = 'I' THEN AOI1_AMT ELSE 0 END) AS M_AMT_T_I,
                                                              SUM(CASE WHEN AOI1_DATA_TYPE = 'T' AND AOI1_PROD_SUBTYPE = 'S' THEN AOI1_AMT ELSE 0 END) AS M_AMT_T_S,
                                                              SUM(CASE WHEN AOI1_PROD_SUBTYPE = 'I' THEN AOI1_AMT ELSE 0 END) AS M_AMT_I,
                                                              SUM(CASE WHEN AOI1_PROD_SUBTYPE = 'S' THEN AOI1_AMT ELSE 0 END) AS M_AMT_S,
                                                              SUM(CASE WHEN AOI1_DATA_TYPE = 'OI' AND AOI1_PROD_SUBTYPE = 'I' THEN AOI1_QNTY ELSE 0 END) AS S_QNTY_I,
                                                              SUM(CASE WHEN AOI1_DATA_TYPE = 'OI' AND AOI1_PROD_SUBTYPE = 'S' THEN AOI1_QNTY ELSE 0 END) AS S_QNTY_S,
                                                              SUM(CASE WHEN AOI1_DATA_TYPE = 'T' AND AOI1_PROD_SUBTYPE = 'I' AND AOI1_ACC_IDFG = '3' THEN AOI1_QNTY ELSE 0 END) AS T3_QNTY,
                                                              SUM(CASE WHEN AOI1_DATA_TYPE = 'T' AND AOI1_PROD_SUBTYPE = 'I' AND AOI1_ACC_IDFG = '5' THEN AOI1_QNTY ELSE 0 END) AS T5_QNTY
                                                         FROM CI.AOI1
                                                        WHERE AOI1_YMD >= @as_symd
                                                          AND AOI1_YMD <= @as_eymd
                                                          AND AOI1_PROD_TYPE = 'F'
                                                          AND AOI1_PROD_SUBTYPE IN ('I','S')
                                                        GROUP BY AOI1_YMD) AM,
                                                       --摩臺
                                                      (SELECT STWD_YMD,SUM(STWD_QNTY) AS STWD_QNTY,
                                                              ROUND(SUM(STWD_QNTY) * SUM(CASE WHEN STWD_SETTLE_DATE = '000000' THEN STWD_CLOSE_PRICE ELSE 0 END) * HEXRT_EXCHANGE_RATE * 100 ,0) AS STWD_AMT
                                                         FROM CI.STWD,CI.HEXRT
                                                       WHERE STWD_YMD >= @as_symd
                                                         AND STWD_YMD <= @as_eymd
                                                         AND HEXRT_DATE = TO_DATE(STWD_YMD,'YYYYMMDD')
                                                         AND HEXRT_CURRENCY_TYPE = '2'
                                                       GROUP BY STWD_YMD,HEXRT_EXCHANGE_RATE) M,
                                                       --成交值
                                                     (SELECT AMIF_DATE,AMIF_SUM_AMT
                                                        FROM CI.AMIF
                                                       WHERE AMIF_DATE >= TO_DATE(@as_symd,'YYYYMMDD')
                                                         AND AMIF_DATE <= TO_DATE(@as_eymd,'YYYYMMDD')
                                                         AND AMIF_KIND_ID = 'TXF'
                                                         AND AMIF_SETTLE_DATE = '000000') V
                                                 WHERE DT_EYMD = AOI1_YMD(+)
                                                   AND DT_EYMD = STWD_YMD(+)
                                                   AND DT_EYMD = S_EYMD(+)
                                                   AND DT_EYMD = TO_CHAR(AMIF_DATE(+),'YYYYMMDD')
                                                ORDER BY YMD
                            ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}