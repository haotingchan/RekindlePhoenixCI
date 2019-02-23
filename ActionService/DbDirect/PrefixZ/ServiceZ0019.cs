
using DataObjects.Dao.Together.SpecificDao;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ0019
    {
        private DZ0019 daoZ0019;

        public ServiceZ0019()
        {
            daoZ0019 = new DZ0019();
        }

        public DataTable ListUTPByDept(string UPF_DPT_ID)
        {
            return daoZ0019.ListUTPByDept(UPF_DPT_ID);
        }
    }
}