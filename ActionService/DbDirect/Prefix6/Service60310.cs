
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System.Data;

namespace ActionServiceW.DbDirect.Prefix6
{
    public class Service60310 
    {
        private D60310 dao60310;
        private RPTF daoRPTF;

        public Service60310()
        {
            dao60310 = new D60310();
            daoRPTF = new RPTF();
        }

        public DataTable ListAM7(string yyyy)
        {
            return dao60310.ListAM7(yyyy);
        }

        public DataTable ListData(string symd, string eymd)
        {
            return dao60310.ListData(symd, eymd);
        }

        public DataTable ListRPTF(string RPTF_TXN_ID, string RPTF_TXD_ID, string RPTF_KEY)
        {
            return daoRPTF.ListData(RPTF_TXN_ID, RPTF_TXD_ID, RPTF_KEY);
        }
    }
}