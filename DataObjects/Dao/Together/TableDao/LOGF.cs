using OnePiece;
using System;

namespace DataObjects.Dao.Together
{
    public class LOGF
    {
        private Db db;

        public LOGF()
        {
            db = GlobalDaoSetting.DB;
        }

        public bool Insert(string LOGF_USER_ID, string LOGF_TXN_ID, string LOGF_KEY_DATA, string LOGF_JOB_TYPE)
        {
            object[] parms =
            {
                "@LOGF_TIME", DateTime.Now,
                "@LOGF_USER_ID",LOGF_USER_ID,
                "@LOGF_TXN_ID",LOGF_TXN_ID,
                "@LOGF_KEY_DATA","[C#]"+LOGF_KEY_DATA,
                "@LOGF_JOB_TYPE",LOGF_JOB_TYPE
            };

            #region sql

            string sql =
                @"
                    INSERT INTO ci.LOGF(LOGF_TIME, LOGF_USER_ID, LOGF_TXN_ID, LOGF_KEY_DATA, LOGF_JOB_TYPE)
	                VALUES(@LOGF_TIME, @LOGF_USER_ID, @LOGF_TXN_ID, @LOGF_KEY_DATA, @LOGF_JOB_TYPE)
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