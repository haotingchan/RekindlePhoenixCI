using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class TXF
    {
        private Db db;

        public TXF()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListDataByTxn(string TXF_TXN_ID)
        {
            object[] parms =
            {
                "@TXF_TXN_ID",TXF_TXN_ID
            };

            #region sql

            string sql =
                @"
                    SELECT  t.*, ' ' AS ERR_MSG
                    FROM    CI.TXF t 
                    WHERE   TXF_TXN_ID = @TXF_TXN_ID 
                    ORDER BY TXF_SEQ_NO ASC 
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}