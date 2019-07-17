using BusinessObjects;
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
                    SELECT  CI.TXN.*,TXN_ID AS TXN_ID_ORG,'' AS OP_TYPE
                    FROM    CI.TXN
                    ORDER BY TXN_SEQ_NO,TXN_ID
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
                    SELECT  TXN_ID,TXN_NAME,TRIM(TXN_ID) || ' ─ ' || TXN_NAME AS TXN_ID_NAME
                    FROM    CI.TXN
                    WHERE TXN_DEFAULT <> 'Y'
                    AND TXN_TYPE = 'F'
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
                       SELECT TXN_ID, TXN_NAME,TXN_SEQ_NO,TXN_PARENT_ID,TXN_LEVEL, TXN_EXTEND
                        FROM CI.TXN, CI.UTP
                        WHERE TXN_ID = UTP_TXN_ID
                        AND UTP_USER_ID = @UTP_USER_ID
                        AND TXN_DEFAULT <> 'Y'
        
                        UNION
        
                        SELECT TXN_ID, TXN_NAME,TXN_SEQ_NO,TXN_PARENT_ID,TXN_LEVEL, TXN_EXTEND
                        FROM CI.TXN
                        WHERE  COALESCE(TXN_DEFAULT,'') = 'Y'
            
                         UNION 
            
                        SELECT TXN_ID, TXN_NAME,TXN_SEQ_NO,TXN_PARENT_ID,TXN_LEVEL, TXN_EXTEND
                        FROM CI.TXN
                        WHERE TXN_TYPE = 'N'
                        AND TRIM(TXN_ID) IN (
                                SELECT DISTINCT TXN_PARENT_ID AS TXN_ID
                                FROM CI.TXN, CI.UTP
                                WHERE TXN_ID = UTP_TXN_ID
                                AND UTP_USER_ID = @UTP_USER_ID
                                AND TXN_DEFAULT <> 'Y'
                        )
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public ResultData UpdateData(DataTable inputData)
        {

            string sql = @"
                    SELECT  *
                    FROM    CI.TXN                    
            ";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}