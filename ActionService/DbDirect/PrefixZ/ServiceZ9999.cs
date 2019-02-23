
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ9999 
    {
        private DZ9999 daoDZ9999;

        public ServiceZ9999()
        {
            daoDZ9999 = new DZ9999();
        }

        public DataTable ListLOGF(DateTime START_DATE, DateTime END_DATE, string START_TIME, string END_TIME, string TXN_AUDIT)
        {
            return daoDZ9999.ListLOGF(START_DATE, END_DATE, START_TIME, END_TIME, TXN_AUDIT);
        }
    }
}