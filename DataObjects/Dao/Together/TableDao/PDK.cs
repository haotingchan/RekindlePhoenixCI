using BusinessObjects.Enums;
using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class PDK
    {
        private Db db;

        public PDK()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListForNonStockByID(string PDK_KIND_ID)
        {
            object[] parms =
            {
                "@PDK_KIND_ID", PDK_KIND_ID
            };

            #region sql

            string sql =
                @"   
                    SELECT  * 
                    FROM    PDK
                    WHERE   PDK_SUBTYPE <>'S' 
                    AND     PDK_KIND_ID LIKE @PDK_KIND_ID + '%'
                ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListForStockByID(string PDK_KIND_ID)
        {
            object[] parms =
            {
                "@PDK_KIND_ID", PDK_KIND_ID
            };

            #region sql

            string sql =
                @"   
                    SELECT  * 
                    FROM    PDK
                    WHERE   PDK_SUBTYPE ='S' 
                    AND     PDK_KIND_ID LIKE @PDK_KIND_ID + '%'
                ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
