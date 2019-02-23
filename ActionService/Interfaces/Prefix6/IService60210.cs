using System;
using System.Data;

namespace ActionService.Interfaces.Prefix6
{
    public interface IService60210 : IService
    {
        DataTable ListDataFor602111(DateTime queryDate);

        DataTable ListDataFor602112(DateTime queryDate);

        DataTable ListDataFor602113(DateTime queryDate, DateTime lastDate);

        DateTime GetStwLastDate(DateTime queryDate);
    }
}