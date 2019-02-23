using System.Data;

namespace ActionService.Interfaces.PrefixZ
{
    public interface IServiceZ0990 : IService
    {
        DataTable CheckUPFByPassword(string UPF_USER_ID, string UPF_PASSWORD);

        bool UpdatePasswordByUserId(string UPF_USER_ID, string UPF_PASSWORD, string UPF_CHANGE_FLAG);
    }
}