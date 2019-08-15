using Common;
using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken 2018/12/20
/// </summary>
namespace DataObjects.Dao.Together {
   /// <summary>
   /// 設定4種營業時段(日盤/夜盤)
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
SELECT TRIM(OCFG_OSW_GRP) as osw_grp,
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
        /// CI.OCFG (OCFG_MARKET_CODE = '0')
        /// </summary>
        /// <returns>osw_grp/osw_grp_name/sort_key</returns>
        public DataTable ListAllTime()
        {

            string sql = @"
SELECT TRIM(OCFG_OSW_GRP) as osw_grp,
'Group'|| (case OCFG_OSW_GRP when '1' then '1' when '5' then '2' when '7' then '3' else ' ' end) || to_char(OCFG_CLOSE_TIME,' (hh24:mi)') as osw_grp_name , 
to_char(OCFG_CLOSE_TIME,'hh24mi') as sort_key,
OCFG_CLOSE_TIME
from ci.OCFG
WHERE OCFG_MARKET_CODE = '0'
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
      /// list by OCFG_OSW_GRP , return all max(OCFG_CLOSE_TIME) by Ken
      /// </summary>
      /// <param name="OCFG_OSW_GRP"></param>
      /// <returns>OCFG_OSW_GRP/OCFG_CLOSE_TIME</returns>
      public DataTable ListCloseTime(string OCFG_OSW_GRP) {
         object[] parms =
         {
                ":as_osw_grp", OCFG_OSW_GRP
            };

         string sql = @"
select trim(ocfg_osw_grp) as ocfg_osw_grp,
max(ocfg_close_time) as ocfg_close_time
from ci.ocfg
group by ocfg_osw_grp
having trim(ocfg_osw_grp)= :as_osw_grp
order by to_number(ocfg_osw_grp)";

         DataTable dtResult = db.GetDataTable(sql , parms);

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
        /// 只有四個作業會用到(30010, 30053, 30055, 20110)
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

            //16:15 / 全部
            if (DateTime.Now <= dtResult.Rows[0]["LDT_GRP7"].AsDateTime()) {
                ls_rtn = "5";
            }
            else {
                ls_rtn = "%";
            }

            //switch (as_txn_id) {
            //    case ("30010"):
            //    case ("30053"):
            //    case ("30055"):
            //    case ("20110"):
            //        //16:15 / 全部
            //        if (DateTime.Now <= dtResult.Rows[0]["LDT_GRP7"].AsDateTime()) {
            //            ls_rtn = "5";
            //        }
            //        else {
            //            ls_rtn = "%";
            //        }
            //        break;
            //    default:
            //        ls_rtn = "%";
            //        break;
            //}

            return ls_rtn;
        }

        /// <summary>
        /// 取得商品交易時段(決定畫面中下拉選單的值)
        /// 用到的程式: 40030, 40040, 40041, 40050, 40060, 43010, 43020
        /// </summary>
        /// <returns></returns>
        public string f_gen_osw_grp() {


            string sql = @"
select case when to_char(OCFG_CLOSE_TIME,'yyyymmdd') <> to_char(sysdate,'yyyymmdd') then '1' 
                   else  case when OCFG_CLOSE_TIME = max_all  then '%' else OCFG_OSW_GRP end end as ls_osw_grp
  from ci.OCFG,
      (select max(OCFG_CLOSE_TIME) as max_all,min(OCFG_CLOSE_TIME) as min_all from ci.OCFG where OCFG_MARKET_CODE = '0'),
      (select max(OCFG_CLOSE_TIME) as max_now
         from ci.OCFG,(select sysdate as dtime from dual)
        where OCFG_CLOSE_TIME <=  dtime
         and OCFG_MARKET_CODE = '0')
 where OCFG_CLOSE_TIME = case when max_now is null then min_all else max_now end
";
            DataTable dtResult = db.GetDataTable(sql, null);
            if (dtResult.Rows.Count > 0)
                return dtResult.Rows[0]["LS_OSW_GRP"].AsString();
            else
                return "";
        }
    }
}
