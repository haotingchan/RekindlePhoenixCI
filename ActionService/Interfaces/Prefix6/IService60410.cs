using System;
using System.Data;

namespace ActionService.Interfaces.Prefix6
{
    public interface IService60410 : IService
    {
        DataTable List60410a(DateTime symd, DateTime eymd);

        DataTable List60410b(DateTime symd, DateTime eymd, int ai_tot_cnt, Decimal ad_max_weight, Decimal ad_top5_weight, int ai_25_cnt, Decimal ad_avg_amt1, Decimal ad_avg_amt2);

        DataTable List60410_2(DateTime symd, DateTime eymd, decimal rate);

        DataTable List60410_3(DateTime symd, DateTime eymd, decimal rate);
    }
}