using System;
using System.Data;

namespace ActionService.Interfaces.PrefixZ
{
    public interface IServiceZ0111 : IService
    {
        DataTable ListLOGUTPByUser(DateTime LOGUTP_START_DATE, DateTime LOGUTP_END_DATE, string LOGUTP_USER_ID);
    }
}