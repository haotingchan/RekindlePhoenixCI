
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;

namespace ActionServiceW.DbDirect.Prefix
{
    public class ServicePrefix1
    {
        private TXF     daoTXF;
        private TXF1 daoTXF1;
        private TXEMAIL daoTXEMAIL;
        private LOGSP   daoLOGSP;

        public ServicePrefix1()
        {
            daoTXF      = new TXF();
            daoLOGSP    = new LOGSP();
            daoTXEMAIL  = new TXEMAIL();
            daoTXF1 = new TXF1();
        }

        public DataTable ListTxfByTxn(string TXF_TXN_ID)
        {
            return daoTXF.ListDataByTxn(TXF_TXN_ID);
        }

        public DataTable ListTxemail(string TXEMAIL_TXN_ID, int TXEMAIL_SEQ_NO)
        {
            return daoTXEMAIL.ListData(TXEMAIL_TXN_ID, TXEMAIL_SEQ_NO);
        }

        public DataTable ListLogsp(DateTime LOGSP_DATE, string TXN_ID)
        {
            return daoLOGSP.ListData(LOGSP_DATE, TXN_ID, LogspQueryType.All);
        }

        public DataTable ListLogspForRunned(DateTime LOGSP_DATE, string TXN_ID)
        {
            return daoLOGSP.ListData(LOGSP_DATE, TXN_ID, LogspQueryType.Runned);
        }

        public bool HasLogspDone(DateTime LOGSP_DATE, string LOGSP_TXN_ID)
        {
            if(daoLOGSP.ListData(LOGSP_DATE, LOGSP_TXN_ID, LogspQueryType.Runned).Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveLogsp(DateTime LOGSP_DATE, string LOGSP_TXN_ID, int LOGSP_SEQ_NO, string LOGSP_TID, string LOGSP_TID_NAME, DateTime LOGSP_BEGIN_TIME, DateTime LOGSP_END_TIME, string LOGSP_MSG)
        {
            return daoLOGSP.Save(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG);
        }

        public bool SetTXF1(string TXF_TID,string TXF_TXN_ID) {
            return daoTXF1.UpdateTid(TXF_TID,TXF_TXN_ID);
        }
    }
}