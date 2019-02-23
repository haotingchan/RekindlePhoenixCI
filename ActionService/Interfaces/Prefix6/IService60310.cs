using System.Data;

namespace ActionService.Interfaces.Prefix6
{
    public interface IService60310 : IService
    {
        DataTable ListAM7(string yyyy);

        DataTable ListData(string symd, string eymd);

        DataTable ListRPTF(string RPTF_TXN_ID, string RPTF_TXD_ID, string RPTF_KEY);
    }
}