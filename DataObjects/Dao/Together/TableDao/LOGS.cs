using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together
{
    public enum LOGSQueryType
    {
        All,
        Runned
    }

    public class LOGS
    {
        private Db db;

        public LOGS()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData(DateTime LOGS_DATE, string LOGS_TXN_ID, LOGSQueryType LOGSQueryType)
        {
            object[] parms =
            {
                "@LOGS_DATE", LOGS_DATE,
                "@LOGS_TXN_ID", LOGS_TXN_ID
            };

            #region sql

            string sql =
                @"
                    SELECT  * 
                    FROM    ci.LOGS 
                    WHERE LOGS_DATE = @LOGS_DATE 
                    AND LOGS_TXN_ID = @LOGS_TXN_ID 
                ";

            #endregion sql

            if(LOGSQueryType == LOGSQueryType.All)
            {

            }
            else if(LOGSQueryType == LOGSQueryType.Runned)
            {
                sql += " AND NOT LOGS_BEGIN_TIME IS NULL";
            }

            sql += " ORDER BY LOGS_SEQ_NO ASC";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public bool Save(DateTime LOGS_DATE, string LOGS_TXD_ID,  DateTime LOGS_W_TIME,string LOGS_W_USER_ID, string LOGS_ERR_TXT)
        {
            object[] parms =
            {
                "@LOGS_DATE", LOGS_DATE,
                "@LOGS_TXD_ID", LOGS_TXD_ID,
                "@LOGS_W_TIME", LOGS_W_TIME,
                "@LOGS_W_USER_ID", LOGS_W_USER_ID,
                "@LOGS_ERR_TXT", LOGS_ERR_TXT,
            };

            #region sql
            string sql =
                @"
                    INSERT INTO CI.LOGS(LOGS_DATE, LOGS_TXD_ID, LOGS_W_TIME, LOGS_W_USER_ID, LOGS_ERR_TXT) 
	                VALUES
                    (@LOGS_DATE, @LOGS_TXD_ID, @LOGS_W_TIME, @LOGS_W_USER_ID, @LOGS_ERR_TXT) 
                ";

            #endregion sql

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}