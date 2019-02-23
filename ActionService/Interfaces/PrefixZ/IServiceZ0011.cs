using System.Data;

namespace ActionService.Interfaces.PrefixZ
{
    public interface IServiceZ0011 : IService
    {
        bool UpdatePasswordByUserId(string UPF_USER_ID, string UPF_PASSWORD);

        DataTable ListUPFByUserId(string UPF_USER_ID);
    }
}