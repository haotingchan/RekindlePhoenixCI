
using BusinessObjects;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Collections.Generic;
using System.Data;

namespace ActionServiceW.DbDirect.Prefix
{
    public class ServicePrefix1
    {
        private TXF     daoTXF;
        private TXF1 daoTXF1;
        private TXF2 daoTXF2;
        private TXEMAIL daoTXEMAIL;
        private LOGSP   daoLOGSP;
        private LOGS daoLOGS;
        private JSW daoJSW;
        private JRF daoJRF;
        private OCF daoOCF;

        public ServicePrefix1()
        {
            daoTXF      = new TXF();
            daoLOGSP    = new LOGSP();
            daoLOGS = new LOGS();
            daoTXEMAIL  = new TXEMAIL();
            daoTXF1 = new TXF1();
            daoTXF2 = new TXF2();
            daoJRF = new JRF();
            daoOCF = new OCF();
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

        public DataTable ListJrf(string JRF_TXN_TID, string JRF_TXF_TID)
        {
            return daoJRF.ListData(JRF_TXN_TID, JRF_TXF_TID);
        }

        public List<BO_OCF> ListOCFList() {
            return daoOCF.GetOCFList();
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

        public bool SaveLogs(DateTime LOGSP_DATE, string LOGS_TXD_ID,DateTime LOGS_W_TIME, string LOGS_W_USER_ID, string LOGS_ERR_TXT)
        {
            return daoLOGS.Save(LOGSP_DATE, LOGS_TXD_ID, LOGS_W_TIME, LOGS_W_USER_ID, LOGS_ERR_TXT);
        }

        public bool UpdateJsw(string JSW_TXN_ID, string JSW_JOB_TYPE, string JSW_SEQ_NO, string JSW_SW_CODE, DateTime JSW_DATE, DateTime JSW_W_TIME, string JSW_W_USER_ID) {
            return daoJSW.UpdateJswByTxnId(JSW_TXN_ID, JSW_JOB_TYPE, JSW_SEQ_NO, JSW_SW_CODE, JSW_DATE, JSW_W_TIME, JSW_W_USER_ID);
        }

        public bool SetTXF1(string TXF_TID,string TXF_TXN_ID) {
            return daoTXF1.UpdateTid(TXF_TID,TXF_TXN_ID);
        }
        public bool setOCF()
        {
            return daoOCF.UpdateCI();
        }

        public DataTable CheckTXF2(string TXF2_TXN_ID,string TXF2_TID) {
            return daoTXF2.ListDataByTXN(TXF2_TXN_ID, TXF2_TID);
        }

    }
}