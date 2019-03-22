using BusinessObjects;
using OnePiece;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D51060
    {
        private Db db;

        public D51060()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetData(string as_ym)
        {
            object[] parms ={
                "@as_ym",as_ym
            };

            string sql = @"
                        SELECT 
                            CI.MMIQ.MMIQ_YM,
                            CI.MMIQ.MMIQ_FCM_NO,   
                            CI.MMIQ.MMIQ_ACC_NO,   
                            CI.MMIQ.MMIQ_KIND_ID,   
                            CI.MMIQ.MMIQ_INVALID_QNTY,   
                            CI.MMIQ.MMIQ_W_TIME,   
                            CI.MMIQ.MMIQ_W_USER_ID,
                            '0' as IS_NEWROW
                        FROM CI.MMIQ  
                        WHERE CI.MMIQ.MMIQ_YM = :as_ym
                        order by MMIQ_FCM_NO";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public string ExecuteStoredProcedure(string Is_ym) {
            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("ls_ym",Is_ym),
            //new DbParameterEx("RETURNPARAMETER",0)
         };

            string sql = "CI.SP_H_UPD_AMM0_MONTH";

            return db.ExecuteStoredProcedureReturnString(sql, parms, true, OracleDbType.Int32);            
        }

        public ResultData updateData(DataTable inputData) {
            string sql = @"
                        SELECT 
                            CI.MMIQ.MMIQ_YM,
                            CI.MMIQ.MMIQ_FCM_NO,   
                            CI.MMIQ.MMIQ_ACC_NO,   
                            CI.MMIQ.MMIQ_KIND_ID,   
                            CI.MMIQ.MMIQ_INVALID_QNTY,   
                            CI.MMIQ.MMIQ_W_TIME,   
                            CI.MMIQ.MMIQ_W_USER_ID
                        FROM CI.MMIQ";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
