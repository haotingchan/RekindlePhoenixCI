using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class TXEMAIL
    {
        private Db db;

        public TXEMAIL()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData(string TXEMAIL_TXN_ID, int TXEMAIL_SEQ_NO)
        {
            object[] parms =
            {
                "@TXEMAIL_TXN_ID", TXEMAIL_TXN_ID,
                "@TXEMAIL_SEQ_NO", TXEMAIL_SEQ_NO
            };

            #region sql

            string sql =
                @"
                    SELECT  *
                    FROM    CI.TXEMAIL
                    WHERE   TXEMAIL_TXN_ID = @TXEMAIL_TXN_ID  
                    AND     TXEMAIL_SEQ_NO = @TXEMAIL_SEQ_NO
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}