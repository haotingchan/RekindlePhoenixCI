using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class DZ9999
    {
        private Db db;

        public DZ9999()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListLOGF(DateTime START_DATE, DateTime END_DATE, string START_TIME, string END_TIME, string TXN_AUDIT)
        {
            object[] parms = {
                "@START_DATE",START_DATE,
                "@END_DATE",END_DATE,
                "@START_TIME",START_TIME,
                "@END_TIME",END_TIME,
                "@TXN_AUDIT",TXN_AUDIT+"%"
            };

            string sql =
                @"
                          SELECT	LOGF.LOGF_TIME,
		                                LOGF.LOGF_USER_ID,
		                                UPF.UPF_USER_NAME,
		                                LOGF.LOGF_TXN_ID,
		                                TXN.TXN_NAME,
		                                LOGF.LOGF_KEY_DATA
                            FROM CI.LOGF LOGF,CI.TXN TXN,CI.UPF UPF
                            WHERE LOGF_TIME >= @START_DATE
                            AND TO_DATE(TO_CHAR(LOGF_TIME,'YYYY/MM/DD'),'YYYY/MM/DD') <= @END_DATE
                            AND ((@END_TIME = '' OR TO_CHAR(LOGF_TIME,'HH24:MI:SS') > @END_TIME)
                            OR  (@START_TIME = '' OR TO_CHAR(LOGF_TIME,'HH24:MI:SS') < @START_TIME))
                            AND NVL(TXN_AUDIT,' ') LIKE @TXN_AUDIT
                            AND LOGF_TXN_ID = TXN_ID(+)
                            AND LOGF_USER_ID = UPF_USER_ID(+)
                            ORDER BY LOGF_TIME
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}