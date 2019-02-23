using OnePiece;
using System;

namespace DataObjects.Dao.Together.TableDao
{
    public class UTP
    {
        private Db db;

        public UTP()
        {
            db = GlobalDaoSetting.DB;
        }

        public bool InsertUTPByTXN(string UTP_USER_ID, string UTP_W_USER_ID, String TXN_ID)
        {
            object[] parms =
            {
                "@UTP_USER_ID",UTP_USER_ID,
                "@UTP_W_TIME",DateTime.Now,
                "@UTP_W_USER_ID",UTP_W_USER_ID,
                "@TXN_ID",TXN_ID
            };

            string sql = @"
                                    INSERT INTO ci.UTP
                                    SELECT @UTP_USER_ID,TXN_ID,TXN_INS,TXN_DEL,TXN_QUERY,TXN_IMPORT,TXN_EXPORT,TXN_PRINT,@UTP_W_TIME,@UTP_W_USER_ID
                                    FROM ci.TXN
                                    WHERE TXN_ID = @TXN_ID
                                ";

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0)
            {
                return true;
            }
            else
            {
                throw new Exception("UTP更新失敗");
            }
        }

        public bool DeleteUTPByTxnId(string UTP_TXN_ID)
        {
            object[] parms =
            {
                "@UTP_TXN_ID", UTP_TXN_ID
            };

            string sql = @"
                                     DELETE   FROM    ci.UTP
                                     WHERE   UTP_TXN_ID  = @UTP_TXN_ID
                                   ";

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0)
            {
                return true;
            }
            else
            {
                throw new Exception("UTP更新失敗");
            }
        }

        public bool DeleteUTPByUserIdAndTxnId(string UTP_USER_ID, string TXN_ID)
        {
            object[] parms =
            {
                "@UTP_USER_ID", UTP_USER_ID,
                "@TXN_ID",TXN_ID
            };

            string sql = @"
                                     DELETE   FROM    ci.UTP
                                     WHERE   UTP_USER_ID  = @UTP_USER_ID
                                     AND UTP_TXN_ID  = @TXN_ID
                                   ";

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult >= 0)
            {
                return true;
            }
            else
            {
                throw new Exception("UTP更新失敗");
            }
        }

        public bool DeleteUTPByUserId(string UTP_USER_ID)
        {
            object[] parms =
            {
                "@UTP_USER_ID", UTP_USER_ID
            };

            string sql = @"
                                     DELETE   FROM    ci.UTP
                                     WHERE   UTP_USER_ID  = @UTP_USER_ID
                                   ";

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult >= 0)
            {
                return true;
            }
            else
            {
                throw new Exception("UTP更新失敗");
            }
        }
    }
}