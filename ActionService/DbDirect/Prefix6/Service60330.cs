
using DataObjects.Dao.Together.SpecificDao;
using System.Data;

namespace ActionServiceW.DbDirect.Prefix6
{
    public class Service60330 
    {
        private D60330 dao60330;

        public Service60330()
        {
            dao60330 = new D60330();
        }

        public DataTable ListData(string symd, string eymd)
        {
            return dao60330.ListData(symd, eymd);
        }
    }
}