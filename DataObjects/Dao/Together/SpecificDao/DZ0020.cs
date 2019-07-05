using OnePiece;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class DZ0020
    {
        private Db db;

        public DZ0020()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListUTPByTxn(List<string> UTP_TXN_ID)
        {
            string txnList = "'" + string.Join("','", UTP_TXN_ID) + "'";
            string sql =
                string.Format(@"
                                SELECT TXN_ID as 作業代號,
                                            TXN_NAME as 作業名稱,
                                            UTP_USER_ID as 使用者代號,
                                            UPF_USER_NAME as 使用者名稱 
                                FROM ci.UTP,ci.TXN,ci.UPF
                                WHERE UTP_TXN_ID = TXN_ID
                                AND UTP_USER_ID = UPF_USER_ID
                                AND UTP_TXN_ID in( {0})
                                ORDER BY UTP_TXN_ID
                    ", txnList);
            DataTable dtResult = db.GetDataTable(sql);

            return dtResult;
        }
    }
}