using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D60330
    {
        private Db db;

        public D60330()
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
                                    SELECT AI2_PROD_TYPE,AI2_PARAM_KEY,AI2_M_QNTY,AI2_DAY_COUNT,
	                                    AM2_QNTY1,AM2_QNTY2,AM2_QNTY3,AM2_QNTY4,AM2_QNTY5,
	                                    TAX,PARAM_NAME
                                    FROM
                                        (SELECT AI2_PROD_TYPE,AI2_PARAM_KEY AS AI2_PARAM_KEY,
		                                        SUM(AI2_M_QNTY) AS AI2_M_QNTY,
		                                        SUM(AI2_DAY_COUNT) AS AI2_DAY_COUNT
	                                        FROM CI.AI2
	                                        WHERE AI2_SUM_TYPE = 'M'  AND
		                                        AI2_YMD >= @as_symd  AND
		                                        AI2_YMD <= @as_eymd  AND
		                                        AI2_PROD_TYPE IN ('F','O') AND
		                                        AI2_SUM_SUBTYPE = '3'
	                                        GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY) I,
                                        (SELECT AM2_PARAM_KEY AS AM2_PARAM_KEY,
		                                        SUM(CASE WHEN AM2_IDFG_TYPE = '7' THEN AM2_M_QNTY ELSE 0 END) AS AM2_QNTY1,
		                                        SUM(CASE WHEN AM2_IDFG_TYPE = '3' THEN AM2_M_QNTY ELSE 0 END) AS AM2_QNTY2,
		                                        SUM(CASE WHEN AM2_IDFG_TYPE = '6' THEN AM2_M_QNTY ELSE 0 END) AS AM2_QNTY3,
		                                        SUM(CASE WHEN AM2_IDFG_TYPE IN ('1','2','4','5') THEN AM2_M_QNTY ELSE 0 END) AS AM2_QNTY4,
		                                        SUM(CASE WHEN AM2_IDFG_TYPE = '8' THEN AM2_M_QNTY ELSE 0 END) AS AM2_QNTY5
	                                        FROM CI.AM2
	                                        WHERE AM2_SUM_TYPE = 'M'  AND
		                                        AM2_YMD >= @as_symd  AND
		                                        AM2_YMD <= @as_eymd  AND
		                                        AM2_SUM_SUBTYPE = '3' AND
		                                        AM2_IDFG_TYPE IN ('1','2','3','4','5','6','7','8','9')
	                                        GROUP BY AM2_PARAM_KEY
	                                    HAVING SUM(AM2_M_QNTY) > 0) M,
                                        (SELECT TAX2_PARAM_KEY AS TAX2_PARAM_KEY,
			                                        SUM(TAX2_B_TRADE_TAX + TAX2_S_TRADE_TAX + TAX2_B_SETTLE_TAX + TAX2_S_SETTLE_TAX) AS TAX
	                                        FROM CI.TAX2
	                                        WHERE TAX2_YMD >= TRIM(@as_symd)||'01'
	                                        AND TAX2_YMD <= TRIM(@as_eymd)||'31'
	                                        GROUP BY TAX2_PARAM_KEY) T,
                                        (SELECT PARAM_KEY,PARAM_NAME FROM CI.APDK_PARAM) R
                                    WHERE AI2_PARAM_KEY = AM2_PARAM_KEY (+)
	                                    AND AI2_PARAM_KEY = TAX2_PARAM_KEY (+)
	                                    AND AI2_PARAM_KEY = PARAM_KEY (+)
                                    ORDER BY AI2_PROD_TYPE,AI2_PARAM_KEY
                            ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}