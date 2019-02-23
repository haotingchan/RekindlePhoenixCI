
using DataObjects.Dao.Together;
using System;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ0111
    {
        private LOGUTP daoLOGUTP;

        public ServiceZ0111()
        {
            daoLOGUTP = new LOGUTP();
        }

        public DataTable ListLOGUTPByUser(DateTime LOGUTP_START_DATE, DateTime LOGUTP_END_DATE, string LOGUTP_USER_ID)
        {
            return daoLOGUTP.ListDataByUser(LOGUTP_START_DATE, LOGUTP_END_DATE, LOGUTP_USER_ID);
        }
    }
}