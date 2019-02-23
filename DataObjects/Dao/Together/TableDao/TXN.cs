using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class TXN
    {
        private Db db;

        public TXN()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData()
        {
            #region sql

            string sql =
                @"
                    SELECT  CI.TXN.*,TXN_ID AS TXN_ID_ORG
                    FROM    CI.TXN
                    ORDER BY TXN_ID ASC
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable ListDataForTxnIdAndName()
        {
            #region sql

            string sql =
                @"
                    SELECT  TXN_ID,TXN_NAME,TRIM(TXN_ID) || ' ─ ' || TXN_NAME AS TxnIdAndName
                    FROM    CI.TXN
                    WHERE TXN_DEFAULT <> 'Y'
                    ORDER BY TXN_ID ASC
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable ListTxnByUser(string UTP_USER_ID)
        {
            object[] parms =
            {
                "@UTP_USER_ID", UTP_USER_ID
            };

            #region sql

            string sql =
                @"
                    SELECT TXN_ID, TXN_NAME
                    FROM CI.TXN, CI.UTP
                    WHERE TXN_ID = UTP_TXN_ID
                    AND UTP_USER_ID = @UTP_USER_ID
                    AND TXN_DEFAULT <> 'Y'

                    UNION

                    SELECT TXN_ID, TXN_NAME FROM CI.TXN
                    WHERE  COALESCE(TXN_DEFAULT,'') = 'Y'
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}