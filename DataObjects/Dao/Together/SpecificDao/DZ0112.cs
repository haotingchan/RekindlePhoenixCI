using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class DZ0112
    {
        private Db db;

        public DZ0112()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListUTPByTxnAndDpt(string UTP_TXN_ID, string DPT_ID)
        {
            object[] parms = {
                "@UTP_TXN_ID",UTP_TXN_ID,
                "@DPT_ID",DPT_ID
            };

            string sql =
                @"
                                SELECT 	UPF_USER_ID,
		                                UPF_USER_NAME,
		                                CASE NVL(UTP_USER_ID,' ') WHEN ' ' THEN ' ' ELSE 'Y' END AS UTP_FLAG,
		                                CASE NVL(UTP_USER_ID,' ') WHEN ' ' THEN ' ' ELSE 'Y' END AS UTP_FLAG_ORG,
		                                UPF_DPT_ID,DPT_NAME,
		                                ' ' AS OP_TYPE
                                FROM CI.UPF ,CI.DPT,
	                                ( SELECT UTP_USER_ID FROM CI.UTP  WHERE UTP_TXN_ID = @UTP_TXN_ID)
                                WHERE UPF_USER_ID = UTP_USER_ID(+)
                                AND UPF_DPT_ID = DPT_ID
                                AND DPT_ID LIKE @DPT_ID
                                ORDER BY UPF_DPT_ID,UPF_USER_ID
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}