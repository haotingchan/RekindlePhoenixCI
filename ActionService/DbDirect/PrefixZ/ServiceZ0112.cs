
using DataObjects.Dao.Together.SpecificDao;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ0112 
    {
        private DZ0112 daoDZ0112;

        public ServiceZ0112()
        {
            daoDZ0112 = new DZ0112();
        }

        public DataTable ListUTPByTxnAndDpt(string UTP_TXN_ID, string DPT_ID)
        {
            return daoDZ0112.ListUTPByTxnAndDpt(UTP_TXN_ID, DPT_ID);
        }
    }
}