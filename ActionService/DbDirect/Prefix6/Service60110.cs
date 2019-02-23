
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System.Data;

namespace ActionServiceW.DbDirect.Prefix6
{
    public class Service60110 
    {
        private D60110 dao60110;

        public Service60110()
        {
            dao60110 = new D60110();
        }

        public DataTable ListW_AM_And_APDK_PARAM(string yyyyMM)
        {
            return dao60110.ListW_AM_And_APDK_PARAM(yyyyMM);
        }
    }
}