using System.Data;

namespace ActionService.Interfaces.PrefixZ
{
    public interface IServiceZ0112 : IService
    {
        DataTable ListUTPByTxnAndDpt(string UTP_TXN_ID, string DPT_ID);
    }
}