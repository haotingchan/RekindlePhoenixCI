using System;
using System.Data;

namespace ActionService.Interfaces.PrefixZ
{
    public interface IServiceZ9999 : IService
    {
        DataTable ListLOGF(DateTime START_DATE, DateTime END_DATE, string START_TIME, string END_TIME, string TXN_AUDIT);
    }
}