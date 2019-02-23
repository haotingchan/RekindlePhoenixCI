
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;

namespace ActionServiceW.DbDirect.Prefix6
{
    public class Service60210 
    {
        private D60210 dao60210;

        public Service60210()
        {
            dao60210 = new D60210();
        }

        public DataTable ListDataFor602111(DateTime queryDate)
        {
            return dao60210.ListDataFor602111(queryDate);
        }

        public DataTable ListDataFor602112(DateTime queryDate)
        {
            return dao60210.ListDataFor602112(queryDate);
        }

        public DataTable ListDataFor602113(DateTime queryDate, DateTime lastDate)
        {
            return dao60210.ListDataFor602113(queryDate, lastDate);
        }

        public DateTime GetStwLastDate(DateTime queryDate)
        {
            return dao60210.GetStwLastDate(queryDate);
        }
    }
}