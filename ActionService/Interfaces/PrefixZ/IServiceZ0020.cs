using System.Data;

namespace ActionService.Interfaces.PrefixZ
{
    public interface IServiceZ0020 : IService
    {
        DataTable ListTXN();

        bool DeleteUTPByTxnId(string TXN_ID);

        bool InsertLOGUTPByUTPAndUPF(string TXN_ID, string LOGUTP_W_DPT, string LOGUTP_W_USER_ID, string LOGUTP_W_USER_NAME, string LOGUTP_TYPE);
    }
}