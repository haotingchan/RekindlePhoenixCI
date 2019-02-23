using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together
{
    public enum LogspQueryType
    {
        All,
        Runned
    }

    public class LOGSP
    {
        private Db db;

        public LOGSP()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData(DateTime LOGSP_DATE, string LOGSP_TXN_ID, LogspQueryType logspQueryType)
        {
            object[] parms =
            {
                "@LOGSP_DATE", LOGSP_DATE,
                "@LOGSP_TXN_ID", LOGSP_TXN_ID
            };

            #region sql

            string sql =
                @"
                    SELECT  * 
                    FROM    ci.LOGSP 
                    WHERE LOGSP_DATE = @LOGSP_DATE 
                    AND LOGSP_TXN_ID = @LOGSP_TXN_ID 
                ";

            #endregion sql

            if(logspQueryType == LogspQueryType.All)
            {

            }
            else if(logspQueryType == LogspQueryType.Runned)
            {
                sql += " AND NOT LOGSP_BEGIN_TIME IS NULL";
            }

            sql += " ORDER BY LOGSP_SEQ_NO ASC";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public bool Save(DateTime LOGSP_DATE, string LOGSP_TXN_ID, int LOGSP_SEQ_NO, string LOGSP_TID, string LOGSP_TID_NAME, DateTime LOGSP_BEGIN_TIME, DateTime LOGSP_END_TIME, string LOGSP_MSG)
        {
            object[] parms =
            {
                "@LOGSP_DATE", LOGSP_DATE,
                "@LOGSP_TXN_ID", LOGSP_TXN_ID,
                "@LOGSP_SEQ_NO", LOGSP_SEQ_NO,
                "@LOGSP_TID", LOGSP_TID,
                "@LOGSP_TID_NAME", LOGSP_TID_NAME,
                "@LOGSP_BEGIN_TIME", LOGSP_BEGIN_TIME,
                "@LOGSP_END_TIME", LOGSP_END_TIME,
                "@LOGSP_MSG", LOGSP_MSG,
            };

            #region sql

            string sqlQuery =
                @"
                    SELECT * FROM CI.LOGSP WHERE LOGSP_DATE=@LOGSP_DATE AND LOGSP_TXN_ID=@LOGSP_TXN_ID AND LOGSP_TID=@LOGSP_TID
                 ";

            DataTable dtResult = db.GetDataTable(sqlQuery, parms);

            string sqlInsert =
                @"
                    INSERT INTO CI.LOGSP(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG) 
	                VALUES
                    (@LOGSP_DATE, @LOGSP_TXN_ID, @LOGSP_SEQ_NO, @LOGSP_TID, @LOGSP_TID_NAME, @LOGSP_BEGIN_TIME, @LOGSP_END_TIME, @LOGSP_MSG) 
                ";

            string sqlUpdate =
                @"
                    UPDATE CI.LOGSP 
	                SET LOGSP_BEGIN_TIME=@LOGSP_BEGIN_TIME, LOGSP_END_TIME=@LOGSP_END_TIME, LOGSP_MSG=@LOGSP_MSG
	                WHERE LOGSP_DATE=@LOGSP_DATE AND LOGSP_TXN_ID=@LOGSP_TXN_ID AND LOGSP_TID=@LOGSP_TID
                 ";

            string sql = "";

            if (dtResult.Rows.Count > 0)
            {
                sql = sqlUpdate;
            }
            else
            {
                sql = sqlInsert;
            }

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