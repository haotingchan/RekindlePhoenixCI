using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class TXF1
    {
        private Db db;

        public TXF1()
        {
            db = GlobalDaoSetting.DB;
        }

        public bool UpdateTid(string TXF_ID,string TXF_TXN_ID)
        {
            object[] parms =
            {
                "@AS_TID",TXF_ID,
                "@AS_TXN_ID",TXF_TXN_ID
            };

            #region sql

            string sql =
                @"
                    UPDATE CI.TXF1 SET TXF1_TID = @AS_TID,TXF1_W_TIME = SYSDATE
                    WHERE TXF1_TXN_ID = @AS_TXN_ID
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