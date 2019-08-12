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
                    SELECT  t.*, ' ' AS ERR_MSG,
                                case TXF_TYPE WHEN 'I' THEN TRIM(TXF_TID) || '(' ||NVL(TXF_SERVICE,TXFP_PARAM5 ) || ',' || TRIM(TXF_FOLDER) || ')' ELSE TXF_TID  END AS TXF_DESC
                    FROM    CI.TXF t ,
                    (SELECT * FROM CI.TXFP WHERE TXFP_TXN_ID = 'infa' AND TXFP_SEQ_NO = 1)
                    WHERE   TXF_TXN_ID = @TXF_TXN_ID 
                    ORDER BY TXF_SEQ_NO ASC 
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}