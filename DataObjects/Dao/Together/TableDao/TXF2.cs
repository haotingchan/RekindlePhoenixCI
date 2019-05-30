using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class TXF2
    {
        private Db db;

        public TXF2()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListDataByTXN(string TXF2_TXN_ID,string TXF2_TID)
        {
            object[] parms =
            {
                "@AS_TXF2_TXN_ID",TXF2_TXN_ID,
                "@AS_TXF2_TID",TXF2_TID
            };

            #region sql

            string sql =
                @"
                        SELECT NVL(TXF2_TXN_ID2,'')AS TXF2_TXN_ID,TXF2_TID2 
                        FROM CI.TXF2 
                        WHERE TXF2_TXN_ID = @AS_TXF2_TXN_ID AND TXF2_TID = @AS_TXF2_TID
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}