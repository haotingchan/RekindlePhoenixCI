using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class DZ0110
    {
        private Db db;

        public DZ0110()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListUTPByUser(string UTP_USER_ID)
        {
            object[] parms = {
                "@UTP_USER_ID",UTP_USER_ID
            };

            string sql =
                @"
                          SELECT	TXN_ID,
		                                TXN_INS,
		                                TXN_DEL,
		                                TXN_QUERY,
		                                TXN_IMPORT,
		                                TXN_EXPORT,
		                                TXN_PRINT,
		                                TXN_DEFAULT,
		                                TXN_NAME,
		                                CASE  WHEN NVL(UTP_USER_ID,' ') =' ' OR TXN_DEFAULT = 'Y' THEN ' ' ELSE 'Y' END AS UTP_FLAG,
		                                CASE  WHEN NVL(UTP_USER_ID,' ') =' ' OR TXN_DEFAULT = 'Y'  THEN ' ' ELSE 'Y' END AS UTP_FLAG_ORG,
		                                UTP_USER_ID
                            FROM ci.TXN LEFT JOIN ci.UTP ON
                            TXN_ID = UTP_TXN_ID
                            AND UTP_USER_ID = @UTP_USER_ID
                            ORDER BY TXN_ID ASC
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}