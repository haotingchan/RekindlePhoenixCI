using Common;
using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class RPT
    {
        private Db db;

        public RPT()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData(string RPT_TXD_ID)
        {
            object[] parms =
            {
                "@RPT_TXD_ID", RPT_TXD_ID
            };

            #region sql

            string sql =
                @"
                    SELECT  *
                    FROM    CI.RPT
                    WHERE   RPT_TXD_ID = @RPT_TXD_ID
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// Lukas, 2019/1/30
        /// for W20110
        /// </summary>
        /// <returns></returns>
        public int RowCount() {

            string sql =
@"
SELECT count(*) as ll_found
FROM CI.RPT   
WHERE RPT_TXD_ID = '20110'
 AND  RPT_VALUE_2 = '000000'
";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult.Rows[0]["LL_FOUND"].AsInt();
        }

        /// <summary>
        /// Lukas, 2019/1/30
        /// from PB: ci_fun.d_rpt
        /// </summary>
        /// <param name="as_txd_id"></param>
        /// <returns></returns>
        public DataTable ListAllByTXD_ID(string as_txd_id) {

            object[] parms =
            {
                ":as_txd_id", as_txd_id
            };

            string sql =
@"
SELECT RPT_TXN_ID,   
       RPT_TXD_ID,   
       RPT_SEQ_NO,   
       RPT_VALUE,   
       RPT_LEVEL_1,   
       RPT_LEVEL_2,   
       RPT_LEVEL_3,   
       RPT_LEVEL_4,   
       RPT_TYPE,   
       RPT_LEVEL_CNT,   
       RPT_VALUE_2,   
       RPT_VALUE_3,   
       RPT_VALUE_4,   
       RPT_VALUE_5  
FROM CI.RPT   
WHERE RPT_TXD_ID = :as_txd_id
ORDER BY RPT_SEQ_NO
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}