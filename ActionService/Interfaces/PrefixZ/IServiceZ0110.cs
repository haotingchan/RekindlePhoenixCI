using System.Data;

namespace ActionService.Interfaces.PrefixZ
{
    public interface IServiceZ0110 : IService
    {
        DataTable ListUTPByUser(string UTP_USER_ID);

        bool InsertUTPByTXN(string UTP_USER_ID, string UTP_W_USER_ID, string TXN_ID);

        bool InsertLOGUTPByUTPAndUPF(string TXN_ID, string LOGUTP_W_DPT, string LOGUTP_W_USER_ID, string LOGUTP_W_USER_NAME, string LOGUTP_TYPE, string UTP_USER_ID);

        bool DeleteUTPByUserIdAndTxnId(string UTP_USER_ID, string TXN_ID);
    }
}