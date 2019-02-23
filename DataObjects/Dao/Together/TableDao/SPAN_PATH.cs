using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class SPAN_PATH
    {
        private Db db;

        public SPAN_PATH()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetPathByModule(string as_module)
        {
            object[] parms =
            {
                ":as_module",as_module
            };

            #region sql

            string sql =
                @"
                    select SPAN_PATH_BAT,SPAN_PATH_VALUE
                    from cfo.SPAN_PATH
                    where SPAN_PATH_MODULE = :as_module
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}