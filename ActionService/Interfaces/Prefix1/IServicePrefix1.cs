using System;
using System.Data;

namespace ActionService.Interfaces.PrefixC
{
    public interface IServicePrefix1: IService
    {
        DataTable ListTxfByTxn(string TXF_TXN_ID);

        DataTable ListTxemail(string TXEMAIL_TXN_ID, int TXEMAIL_SEQ_NO);

        Boolean HasLogspDone(DateTime LOGSP_DATE, string TXN_ID);

        DataTable ListLogspForRunned(DateTime LOGSP_DATE, string TXN_ID);

        DataTable ListLogsp(DateTime LOGSP_DATE, string TXN_ID);

        Boolean SaveLogsp(DateTime LOGSP_DATE, string LOGSP_TXN_ID, int LOGSP_SEQ_NO, string LOGSP_TID, string LOGSP_TID_NAME, DateTime LOGSP_BEGIN_TIME, DateTime LOGSP_END_TIME, string LOGSP_MSG);
    }
}