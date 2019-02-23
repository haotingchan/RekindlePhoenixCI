
using DataObjects.Dao.Together;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ0011
    {
        private UPF daoUPF;

        public ServiceZ0011()
        {
            daoUPF = new UPF();
        }

        public DataTable ListUPFByUserId(string UPF_USER_ID)
        {
            return daoUPF.ListDataByUserId(UPF_USER_ID);
        }

        public bool UpdatePasswordByUserId(string UPF_USER_ID, string UPF_PASSWORD)
        {
            return daoUPF.UpdatePasswordByUserId(UPF_USER_ID, UPF_PASSWORD);
        }
    }
}