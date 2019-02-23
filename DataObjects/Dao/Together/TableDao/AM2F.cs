using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Winni, 2019/1/21
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class AM2F : DataGate {

      /// <summary>
      /// get CI.AM2F data (for D28510)
      /// </summary>
      /// <param name="as_ym">查詢年月</param>
      /// <returns></returns>
      public DataTable ListData(string as_ym) {
         object[] parms =
         {
                ":as_ym", as_ym
            };

         string sql = @"
SELECT  AM2F_KIND_ID,   
      AM2F_PC_CODE,   
      AM2F_ACC_CODE,   
      AM2F_SETTLE_MONTH1,   
      AM2F_SETTLE_MONTH2,   
      AM2F_SETTLE_MONTH3,   
      AM2F_SETTLE_MONTH4,   
      AM2F_SETTLE_MONTH5,   
      AM2F_SETTLE_MONTH6,   
      AM2F_SETTLE_MONTH7,   
      AM2F_SETTLE_MONTH8,   
      AM2F_SETTLE_MONTH9,   
      AM2F_SETTLE_MONTH10,   
      AM2F_SETTLE_MONTH11,   
      AM2F_SETTLE_MONTH12,   
      AM2F_SETTLE_MONTH13,   
      AM2F_BPOS1,   
      AM2F_SPOS1,   
      AM2F_L_BPOS1,   
      AM2F_L_SPOS1,   
      AM2F_BPOS2,   
      AM2F_SPOS2,   
      AM2F_L_BPOS2,   
      AM2F_L_SPOS2,   
      AM2F_BPOS3,   
      AM2F_SPOS3,   
      AM2F_L_BPOS3,   
      AM2F_L_SPOS3,   
      AM2F_BPOS4,   
      AM2F_SPOS4,   
      AM2F_L_BPOS4,   
      AM2F_L_SPOS4,   
      AM2F_BPOS5,   
      AM2F_SPOS5,   
      AM2F_L_BPOS5,   
      AM2F_L_SPOS5,   
      AM2F_BPOS6,   
      AM2F_SPOS6,   
      AM2F_L_BPOS6,   
      AM2F_L_SPOS6,   
      AM2F_BPOS7,   
      AM2F_SPOS7,   
      AM2F_L_BPOS7,   
      AM2F_L_SPOS7,   
      AM2F_BPOS8,   
      AM2F_SPOS8,   
      AM2F_L_BPOS8,   
      AM2F_L_SPOS8,   
      AM2F_BPOS9,   
      AM2F_SPOS9,   
      AM2F_L_BPOS9,   
      AM2F_L_SPOS9,   
      AM2F_BPOS10,   
      AM2F_SPOS10,   
      AM2F_L_BPOS10,   
      AM2F_L_SPOS10,   
      AM2F_BPOS11,   
      AM2F_SPOS11,   
      AM2F_L_BPOS11,   
      AM2F_L_SPOS11,   
      AM2F_BPOS12,   
      AM2F_SPOS12,   
      AM2F_L_BPOS12,   
      AM2F_L_SPOS12,   
      AM2F_BPOS13,   
      AM2F_SPOS13,   
      AM2F_L_BPOS13,   
      AM2F_L_SPOS13,
      AM2F_YM,
		AM2F_MARKET_CODE
    FROM ci.AM2F  
   WHERE AM2F_YM = :as_ym 
--自己加上ORDER BY 不然排序會跟PB不一樣
order by AM2F_YM,AM2F_KIND_ID,AM2F_ACC_CODE
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}

