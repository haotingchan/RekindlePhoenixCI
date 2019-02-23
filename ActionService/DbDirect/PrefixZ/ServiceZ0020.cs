
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ0020 
    {
        private TXN daoTXN;
        private UTP daoUTP;
        private LOGUTP daoLOGUTP;

        public ServiceZ0020()
        {
            daoTXN = new TXN();
            daoUTP = new UTP();
            daoLOGUTP = new LOGUTP();
        }

        public bool DeleteUTPByTxnId(string TXN_ID)
        {
            return daoUTP.DeleteUTPByTxnId(TXN_ID);
        }

        public bool InsertLOGUTPByUTPAndUPF(string TXN_ID, string LOGUTP_W_DPT, string LOGUTP_W_USER_ID, string LOGUTP_W_USER_NAME, string LOGUTP_TYPE)
        {
            return daoLOGUTP.InsertByUTPAndUPF(TXN_ID, LOGUTP_W_DPT, LOGUTP_W_USER_ID, LOGUTP_W_USER_NAME, LOGUTP_TYPE);
        }

        public DataTable ListTXN()
        {
            return daoTXN.ListData();
        }
    }
}