using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together.TableDao
{
    public class UTP
    {
        private Db db;

        public UTP()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListUTPByTxn(string UTP_TXN_ID)
        {
            object[] parms = {
                "@UTP_TXN_ID",UTP_TXN_ID
            };

            string sql =
                @"
                                SELECT 	UPF_USER_ID,
		                                UPF_USER_NAME,
		                                CASE NVL(UTP_USER_ID,' ') WHEN ' ' THEN ' ' ELSE 'Y' END AS UTP_FLAG,
		                                CASE NVL(UTP_USER_ID,' ') WHEN ' ' THEN ' ' ELSE 'Y' END AS UTP_FLAG_ORG,
		                                UPF_DPT_ID,DPT_NAME,
		                                ' ' AS OP_TYPE
                                FROM CI.UPF ,CI.DPT,
	                                ( SELECT UTP_USER_ID FROM CI.UTP  WHERE UTP_TXN_ID = @UTP_TXN_ID)
                                WHERE UPF_USER_ID = UTP_USER_ID(+)
                                AND UPF_DPT_ID = DPT_ID
                                AND DPT_ID LIKE @DPT_ID
                                ORDER BY UPF_DPT_ID,UPF_USER_ID
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
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
                                    SELECT @UTP_USER_ID,TXN_ID,@UTP_W_TIME,@UTP_W_USER_ID
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

        public bool DeleteUTPByTxnId(List<string> UTP_TXN_ID)
        {
            string txnList = "'" + string.Join(",", UTP_TXN_ID) + "'"; 

            string sql = String.Format(@"
                                     DELETE   FROM    ci.UTP
                                     WHERE   UTP_TXN_ID  IN ({0})
                                   ", txnList);

            int executeResult = db.ExecuteSQL(sql);

            if (executeResult >= 0)
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