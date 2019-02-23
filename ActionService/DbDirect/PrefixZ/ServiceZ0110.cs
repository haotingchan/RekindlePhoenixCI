
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ0110
    {
        private DZ0110 daoDZ0110;
        private UTP daoUTP;
        private LOGUTP daoLOGUTP;

        public ServiceZ0110()
        {
            daoDZ0110 = new DZ0110();
            daoUTP = new UTP();
            daoLOGUTP = new LOGUTP();
        }

        public bool DeleteUTPByUserIdAndTxnId(string UTP_USER_ID, string TXN_ID)
        {
            return daoUTP.DeleteUTPByUserIdAndTxnId(UTP_USER_ID, TXN_ID);
        }

        public bool InsertLOGUTPByUTPAndUPF(string TXN_ID, string LOGUTP_W_DPT, string LOGUTP_W_USER_ID, string LOGUTP_W_USER_NAME, string LOGUTP_TYPE, string UTP_USER_ID)
        {
            return daoLOGUTP.InsertByUTPAndUPF(TXN_ID, LOGUTP_W_DPT, LOGUTP_W_USER_ID, LOGUTP_W_USER_NAME, LOGUTP_TYPE, UTP_USER_ID);
        }

        public bool InsertUTPByTXN(string UTP_USER_ID, string UTP_W_USER_ID, string TXN_ID)
        {
            return daoUTP.InsertUTPByTXN(UTP_USER_ID, UTP_W_USER_ID, TXN_ID);
        }

        public DataTable ListUTPByUser(string UTP_USER_ID)
        {
            return daoDZ0110.ListUTPByUser(UTP_USER_ID);
        }
    }
}