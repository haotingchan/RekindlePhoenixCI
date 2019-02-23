using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class TXFPARM
    {
        private Db db;

        public TXFPARM()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable List(string TXFPARM_SERVER, string TXFPARM_DB, string TXFPARM_TXN_ID, string TXFPARM_TID)
        {
            object[] parms =
            {
                "@TXFPARM_SERVER", TXFPARM_SERVER,
                "@TXFPARM_DB", TXFPARM_DB,
                "@TXFPARM_TXN_ID", TXFPARM_TXN_ID,
                "@TXFPARM_TID", TXFPARM_TID,
            };

            #region sql

            string sql =
                @"
                    SELECT  *
                    FROM    ci.TXFPARM
                    WHERE   TXFPARM_SERVER = @TXFPARM_SERVER
                    AND     TXFPARM_DB = @TXFPARM_DB
                    AND     TXFPARM_TXN_ID = @TXFPARM_TXN_ID
                    AND     TXFPARM_TID = @TXFPARM_TID
                    ORDER BY TXFPARM_ARG_SEQ_NO ASC 
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}