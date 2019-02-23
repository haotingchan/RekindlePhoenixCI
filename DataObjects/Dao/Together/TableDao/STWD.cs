using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/30
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class STWD: DataGate {

        /// <summary>
        /// for W20110
        /// </summary>
        /// <param name="ls_date"></param>
        /// <param name="ls_m"></param>
        /// <returns></returns>
        public decimal GetSettlePrice(string ls_date, string ls_m) {

            decimal ld_value = 0;
            object[] parms =
            {
                ":ls_date", ls_date,
                ":ls_m", ls_m
            };

            string sql =
@"
select STWD_SETTLE_PRICE
from ci.STWD
where STWD_YMD = :ls_date
  and STWD_SETTLE_DATE = :ls_m
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count == 0) {
                return ld_value;
            }
            else {
                ld_value = dtResult.Rows[0]["STWD_SETTLE_PRICE"].AsDecimal();
                return ld_value;
            }

        }

      /// <summary>
      /// 刪除被選取的日期資料
      /// </summary>
      /// <param name="as_date">yyyyMMdd</param>
      /// <returns></returns>
      public bool DeleteByDate(string as_date) {

         object[] parms = {
                "@as_date", as_date
            };

         string sql = @"DELETE FROM CI.STWD WHERE STWD_YMD = :as_date";
         int executeResult = db.ExecuteSQL(sql , parms);

         if (executeResult > 0) {
            return true;
         } else {
            throw new Exception("刪除失敗");
         }
      }

   }
}
