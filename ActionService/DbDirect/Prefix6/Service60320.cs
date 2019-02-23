
using DataObjects.Dao.Together.SpecificDao;
using System.Data;

namespace ActionServiceW.DbDirect.Prefix6
{
    public class Service60320 
    {
        private D60320 dao60320;

        public Service60320()
        {
            dao60320 = new D60320();
        }

        public DataTable ListData(string symd, string eymd)
        {
            return dao60320.ListData(symd, eymd);
        }
    }
}