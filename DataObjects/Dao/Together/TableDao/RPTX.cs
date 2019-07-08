using OnePiece;
using System.Data;

/// <summary>
/// ken 2019/2/19
/// </summary>
namespace DataObjects.Dao.Together {
    /// <summary>
    /// 找出該功能對應的報表template檔名,實際沒什麼用的設計,之後可以廢除
    /// </summary>
    public class RPTX {
        private Db db;

        public RPTX() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// ci.RPTX return ls_sub_path/ls_excel_ext/ls_rename
        /// </summary>
        /// <param name="is_txn_id">txn id</param>
        /// <param name="as_filename">該功能裡面,各分類所對應不同的報表template檔名</param>
        /// <returns>ls_sub_path/ls_excel_ext/ls_rename</returns>
        public DataTable ListByTxn(string is_txn_id, string as_filename) {
            object[] parms =
            {
                ":is_txn_id", is_txn_id,
                ":as_filename", as_filename
            };

            string sql = @"
select trim(RPTX_SUBPATH) as ls_sub_path,
trim(RPTX_FILENAME_EXT) as ls_excel_ext,
trim(RPTX_RENAME) as ls_rename
from ci.RPTX2
where TRIM(RPTX_TXN_ID) = :is_txn_id
and TRIM(RPTX_FILENAME) = :as_filename
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

    }
}
