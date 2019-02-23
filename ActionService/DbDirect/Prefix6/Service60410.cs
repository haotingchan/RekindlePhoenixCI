
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;

namespace ActionServiceW.DbDirect.Prefix6
{
    public class Service60410 
    {
        private D60410 dao60410;

        public Service60410()
        {
            dao60410 = new D60410();
        }

        public DataTable List60410a(DateTime symd, DateTime eymd)
        {
            return dao60410.List60410a(symd, eymd);
        }

        public DataTable List60410b(DateTime symd, DateTime eymd, int ai_tot_cnt, Decimal ad_max_weight, Decimal ad_top5_weight, int ai_25_cnt, Decimal ad_avg_amt1, Decimal ad_avg_amt2)
        {
            return dao60410.List60410b(symd, eymd, ai_tot_cnt, ad_max_weight, ad_top5_weight, ai_25_cnt, ad_avg_amt1, ad_avg_amt2);
        }

        public DataTable List60410_2(DateTime symd, DateTime eymd, decimal rate)
        {
            return dao60410.List60410_2(symd, eymd, rate);
        }

        public DataTable List60410_3(DateTime symd, DateTime eymd, decimal rate)
        {
            return dao60410.List60410_3(symd, eymd, rate);
        }
    }
}