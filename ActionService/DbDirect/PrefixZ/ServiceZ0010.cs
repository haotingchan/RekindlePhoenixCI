
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using System.Data;

namespace ActionServiceW.DbDirect.PrefixZ
{
    public class ServiceZ0010
    {
        private UPF daoUPF;
        private UTP daoUTP;

        public ServiceZ0010()
        {
            daoUPF = new UPF();
            daoUTP = new UTP();
        }

        public bool DeleteUTPByUserId(string UTP_USER_ID)
        {
            return daoUTP.DeleteUTPByUserId(UTP_USER_ID);
        }

        public DataTable ListUPFByDept(string UPF_DPT_ID)
        {
            return daoUPF.ListDataByDept(UPF_DPT_ID);
        }
    }
}