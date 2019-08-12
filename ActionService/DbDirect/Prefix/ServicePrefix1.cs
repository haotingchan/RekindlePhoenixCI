
using ActionService.DbDirect;
using BusinessObjects;
using Common;
using Common.Config;
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
        private ServiceCommon serviceCommon;
        private OCFUPD daoFutAHOCFUPD;
        private OCFUPD daoOptAHOCFUPD;

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
            serviceCommon = new ServiceCommon();
            daoFutAHOCFUPD = new OCFUPD("futAH");
            daoOptAHOCFUPD = new OCFUPD("optAH");


        }

        

        public DataTable ListTxfByTxn(string TXF_TXN_ID)
        {
            return daoTXF.ListDataByTxn(TXF_TXN_ID);
        }

        public DataTable ListTxemail(string TXEMAIL_TXN_ID, int TXEMAIL_SEQ_NO)
        {
            return daoTXEMAIL.ListData(TXEMAIL_TXN_ID, TXEMAIL_SEQ_NO);
        }

        public DataTable ListLogsp(DateTime LOGSP_DATE, string TXN_ID,string OCF_TYPE)
        {
            if (OCF_TYPE == "D")
            {
                return daoLOGSP.ListData(LOGSP_DATE, TXN_ID, LogspQueryType.All);
            }
            else
            {
                return daoLOGSP.ListDataByMonth(LOGSP_DATE, TXN_ID, LogspQueryType.All);
            }
            
        }

        public DataTable ListLogspForRunned(DateTime LOGSP_DATE, string TXN_ID,string OCF_TYPE)
        {
            if (OCF_TYPE == "D")
            {
                return daoLOGSP.ListData(LOGSP_DATE, TXN_ID, LogspQueryType.Runned);
            }
            else
            {
                return daoLOGSP.ListDataByMonth(LOGSP_DATE, TXN_ID, LogspQueryType.Runned);
            }
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

        public bool SaveLogsp(DateTime LOGSP_DATE, string LOGSP_TXN_ID, int LOGSP_SEQ_NO, string LOGSP_TID, string LOGSP_TID_NAME, DateTime LOGSP_BEGIN_TIME, DateTime LOGSP_END_TIME, string LOGSP_MSG,string OCF_TYPE)
        {
            if (OCF_TYPE == "D")
            {
                return daoLOGSP.Save(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG);
            }
            else
            {
                return daoLOGSP.SaveByMonth(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG);
            }
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
        public bool setCIOCF()
        {
            return daoOCF.UpdateCI();
        }

        public bool setOCF(DateTime OCF_DATE,string DB_TYPE,string USER_ID)
        {
            OCF ocf = new OCF(DB_TYPE);
            BO_OCF boOCF = ocf.GetOCF();
            if (boOCF == null)
            {
                MessageDisplay.Error("交易日期檔(OCF)讀取錯誤!");
                return false;
            }

            if (DB_TYPE == "futAH" || DB_TYPE == "optAH")
            {
                if (MessageDisplay.Choose($"請確定交易日期({boOCF.OCF_DATE.AsString("yyyy/MM/dd")}) 沒有例外而遞延日期？\r\n（eg.颱風,災害...）") == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }
            }

            //清除異動紀錄檔
            daoFutAHOCFUPD.Delete();
            daoOptAHOCFUPD.Delete();

            if (DB_TYPE == "futAH")
            {
                daoFutAHOCFUPD.Insert(boOCF.OCF_PREV_DATE, boOCF.OCF_DATE, OCF_DATE, USER_ID);
                ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(DB_TYPE);
                ResultData resultData = serviceCommon.ExecuteStoredProcedure(connectionInfo, "futAH.sp_FAH_chg_OCF_Hdata", null, false);
            }
            else if (DB_TYPE == "optAH") {
                daoOptAHOCFUPD.Insert(boOCF.OCF_PREV_DATE, boOCF.OCF_DATE, OCF_DATE, USER_ID);
                ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(DB_TYPE);
                ResultData resultData = serviceCommon.ExecuteStoredProcedure(connectionInfo, "optAH.sp_OAH_chg_OCF_Hdata", null, false);
            }

            //更新OCF_DATE
            return ocf.UpdateDate(OCF_DATE);
        }

        public bool setPrevOCF(DateTime OCF_PREV_DATE, string DB_TYPE, string USER_ID)
        {
            OCF ocf = new OCF(DB_TYPE);

            //更新OCF_DATE
            return ocf.UpdatePrevDate(OCF_PREV_DATE);
        }

        public DataTable CheckTXF2(string TXF2_TXN_ID,string TXF2_TID) {
            return daoTXF2.ListDataByTXN(TXF2_TXN_ID, TXF2_TID);
        }

    }
}