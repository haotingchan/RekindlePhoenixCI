
using DataObjects.Dao.Together;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ0990 
    {
        private UPF daoUPF;

        public ServiceZ0990()
        {
            daoUPF = new UPF();
        }

        public DataTable CheckUPFByPassword(string UPF_USER_ID, string UPF_PASSWORD)
        {
            return daoUPF.ListDataByUserIdAndPassword(UPF_USER_ID, UPF_PASSWORD);
        }

        public bool UpdatePasswordByUserId(string UPF_USER_ID, string UPF_PASSWORD, string UPF_CHANGE_FLAG)
        {
            return daoUPF.UpdatePasswordByUserId(UPF_USER_ID, UPF_PASSWORD, UPF_CHANGE_FLAG);
        }
    }
}