using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D60310
    {
        private Db db;

        public D60310()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListAM7(string yyyy)
        {
            object[] parms = {
                ":as_y",yyyy
            };

            string sql =
                @"
                            SELECT AM7_FUT_AVG_QNTY,AM7_OPT_AVG_QNTY,
                            AM7_FC_TAX,AM7_DAY_COUNT
                            FROM CI.AM7
                            WHERE AM7_Y =  :as_y
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListData(string symd, string eymd)
        {
            object[] parms = {
                ":as_symd",symd,
                ":as_eymd",eymd
            };

            #region sql

            string sql =
                        @"
                                        SELECT Q.AI2_PROD_TYPE AS PROD_TYPE,
                                               Q.YMD,DAY_QNTY,YEAR_QNTY,DAY_COUNT,DAY_TAX,YEAR_TAX
                                          FROM
                                        (SELECT AI2_PROD_TYPE,DT_SYMD,DT_EYMD AS YMD,
                                               M_QNTY AS DAY_QNTY,
                                               SUM(AI2_M_QNTY) AS YEAR_QNTY,
                                               COUNT(AI2_YMD) AS DAY_COUNT
                                          FROM CI.AI2,
                                            (SELECT AI2_PROD_TYPE AS DT_PROD_TYPE,SUBSTR(AI2_YMD,1,4)||'0101' AS DT_SYMD,
                                                    AI2_YMD AS DT_EYMD,
                                                    SUM(AI2_M_QNTY) AS M_QNTY
                                               FROM CI.AI2
                                              WHERE AI2_YMD >= :as_symd
                                                AND AI2_YMD <= :as_eymd
                                                AND AI2_SUM_TYPE = 'D'
                                                AND AI2_SUM_SUBTYPE = '1'
                                                AND AI2_PROD_TYPE IN ('F','O')
                                             GROUP BY AI2_PROD_TYPE,AI2_YMD)
                                         WHERE AI2_YMD >= DT_SYMD
                                           AND AI2_YMD <= DT_EYMD
                                           AND AI2_SUM_TYPE = 'D'
                                           AND AI2_SUM_SUBTYPE = '1'
                                           AND AI2_PROD_TYPE = DT_PROD_TYPE
                                         GROUP BY AI2_PROD_TYPE,DT_SYMD,DT_EYMD,M_QNTY) Q,
                                        (SELECT TAX2_PROD_TYPE,DT_SYMD,DT_EYMD AS YMD,
                                               TAX AS DAY_TAX,
                                               SUM(TAX2_B_TRADE_TAX + TAX2_S_TRADE_TAX + TAX2_B_SETTLE_TAX + TAX2_S_SETTLE_TAX) AS YEAR_TAX
                                          FROM CI.TAX2,
                                            (SELECT TAX2_PROD_TYPE AS DT_PROD_TYPE,
                                                    SUBSTR(TAX2_YMD,1,4)||'0101' AS DT_SYMD,
                                                    TAX2_YMD AS DT_EYMD,
                                                    SUM(TAX2_B_TRADE_TAX + TAX2_S_TRADE_TAX + TAX2_B_SETTLE_TAX + TAX2_S_SETTLE_TAX) AS TAX
                                               FROM CI.TAX2
                                              WHERE TAX2_YMD >= :as_symd
                                                AND TAX2_YMD <= :as_eymd
                                              GROUP BY TAX2_PROD_TYPE,TAX2_YMD)
                                         WHERE TAX2_YMD >= DT_SYMD
                                           AND TAX2_YMD <= DT_EYMD
                                           AND TAX2_PROD_TYPE = DT_PROD_TYPE
                                         GROUP BY TAX2_PROD_TYPE,DT_SYMD,DT_EYMD,TAX) T
                                        WHERE Q.AI2_PROD_TYPE = T.TAX2_PROD_TYPE(+)
                                          AND Q.YMD = T.YMD(+)
                                        ORDER BY PROD_TYPE,YMD
                            ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}