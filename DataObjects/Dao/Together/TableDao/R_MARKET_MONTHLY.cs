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
SELECT MC_MONTH, 
       FUT_ID, 
       FUT_NAME, 
       REWARD_TYPE, 
       REWARD, 
       DETAIL, 
       ACCTNO, 
       PROD_TYPE
FROM CI.R_MARKET_MONTHLY
WHERE MC_MONTH = :AS_YM
--ORDER BY MC_MONTH, 
         --FUT_ID, 
         --FUT_NAME, 
         --REWARD_TYPE, 
         --REWARD, 
         --DETAIL, 
         --ACCTNO, 
         --PROD_TYPE
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
