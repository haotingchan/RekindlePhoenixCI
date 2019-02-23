using Common;
using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken 2018/12/20
/// </summary>
namespace DataObjects.Dao.Together {
    /// <summary>
    /// OCFG券商資訊?
    /// </summary>
    public class OCFG {
        private Db db;

        public OCFG() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// CI.OCFG (OCFG_MARKET_CODE = '0')
        /// </summary>
        /// <returns>osw_grp/osw_grp_name/sort_key</returns>
        public DataTable ListAll() {

            string sql = @"
SELECT OCFG_OSW_GRP as osw_grp,
'Group'|| (case OCFG_OSW_GRP when '1' then '1' when '5' then '2' when '7' then '3' else ' ' end) || to_char(OCFG_CLOSE_TIME,' (hh24:mi)') as osw_grp_name , 
to_char(OCFG_CLOSE_TIME,'hh24mi') as sort_key
from ci.OCFG
WHERE OCFG_MARKET_CODE = '0'
UNION ALL
SELECT '%','ALL','9999' from dual
order by sort_key";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// CI.OCFG (WHERE OCFG_OSW_GRP IN ('1','5'))
        /// </summary>
        /// <returns>osw_grp/osw_grp_name/sort_key</returns>
        public DataTable ListAll2() {

            string sql = @"
SELECT OCFG_OSW_GRP as osw_grp,
'Group'|| (case OCFG_OSW_GRP when '1' then '1' when '5' then '2' when '7' then '3' else ' ' end) || to_char(OCFG_CLOSE_TIME,' (hh24:mi)') as osw_grp_name , 
to_char(OCFG_CLOSE_TIME,'hh24mi') as sort_key
from ci.OCFG
WHERE OCFG_OSW_GRP IN ('1','5')
UNION ALL
SELECT '%','ALL','9999' from dual
order by sort_key";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// return to_char(OCFG_CLOSE_TIME,'hh24:mi') as ls_rtn
        /// </summary>
        /// <param name="OCFG_OSW_GRP"></param>
        /// <param name="OCFG_MARKET_CODE"></param>
        /// <returns></returns>
        public string GetCloseTime(string OCFG_OSW_GRP, string OCFG_MARKET_CODE="0") {
            object[] parms =
{
                ":as_osw_grp", OCFG_OSW_GRP,
                ":as_ocfg_market_code", OCFG_MARKET_CODE
            };

            string sql =
                @"
SELECT to_char(OCFG_CLOSE_TIME,'hh24:mi') as ls_rtn
from ci.OCFG
WHERE OCFG_MARKET_CODE = :as_ocfg_market_code 
AND OCFG_OSW_GRP = :as_osw_grp;
";
            return db.ExecuteScalar(sql, CommandType.Text, parms);
        }

        /// <summary>
        /// Lukas, 2019/1/30
        /// f_get_txn_osw_grp 依作業代號，決定畫面中盤別的預設值
        /// </summary>
        /// <param name="as_txn_id">_ProgramID</param>
        /// <returns>ls_rtn</returns>
        public string f_get_txn_osw_grp(string as_txn_id) {

            string ls_rtn = "";
            string sql = @"
select max(case when OCFG_OSW_GRP = '1' then OCFG_CLOSE_TIME else  null end) as ldt_grp1,
       max(case when OCFG_OSW_GRP = '5' then OCFG_CLOSE_TIME else  null end) as ldt_grp5,
       max(case when OCFG_OSW_GRP = '7' then OCFG_CLOSE_TIME else  null end) as ldt_grp7
from ci.OCFG
";

            DataTable dtResult = db.GetDataTable(sql, null);

            switch (as_txn_id) {
                case ("30010"):
                case ("30053"):
                case ("30055"):
                case ("20110"):
                    //16:15 / 全部
                    if (DateTime.Now <= dtResult.Rows[0]["LDT_GRP7"].AsDateTime()) {
                        ls_rtn = "5";
                    }
                    else {
                        ls_rtn = "%";
                    }
                    break;
                default:
                    ls_rtn = "%";
                    break;
            }

            return ls_rtn;
        }
    }
}
