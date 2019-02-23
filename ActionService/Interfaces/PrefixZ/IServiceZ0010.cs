using System.Data;

namespace ActionService.Interfaces.PrefixZ
{
    public interface IServiceZ0010 : IService
    {
        DataTable ListUPFByDept(string UPF_DPT_ID);

        bool DeleteUTPByUserId(string UTP_USER_ID);
    }
}