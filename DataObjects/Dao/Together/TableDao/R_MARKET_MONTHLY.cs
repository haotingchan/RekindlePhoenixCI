using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Lukas, 2019/1/3
/// </summary>
namespace DataObjects.Dao.Together.TableDao {

    public class R_MARKET_MONTHLY {

        private Db db;

        public R_MARKET_MONTHLY() {
            db = GlobalDaoSetting.DB;
        }
        /// <summary>
        /// for D50070
        /// </summary>
        /// <param name="as_ym"></param>
        /// <returns></returns>
        public DataTable ListAllByDate(string as_ym) {

            object[] parms =
            {
                ":as_ym", as_ym
            };

            string sql = @"
SELECT mc_month, 
       fut_id, 
       fut_name, 
       reward_type, 
       reward, 
       detail, 
       acctno, 
       prod_type
FROM CI.r_market_monthly
where mc_month = :as_ym
order by mc_month, 
         fut_id, 
         fut_name, 
         reward_type, 
         reward, 
         detail, 
         acctno, 
         prod_type
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
