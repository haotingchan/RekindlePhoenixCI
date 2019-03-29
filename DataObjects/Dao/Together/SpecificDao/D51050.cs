using OnePiece;
using System.Data;

/// <summary>
/// ken,2019/1/2
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 造市商品單邊回應詢價價格限制設定    
   /// </summary>
   public class D51050 : DataGate {

      /// <summary>
      /// get ci.mmfo data 
      /// return mmfo_param_key/mmfo_min_price/mmfo_w_user_id/mmfo_w_time/mmfo_market_code 5 field
      /// </summary>
      /// <returns>' ' as OP_TYPE + all fields</returns>
      public DataTable GetMmfoData() {

         string sql = @"
select 
	mmfo_param_key, 
	mmfo_min_price, 
	mmfo_w_user_id, 
	mmfo_w_time, 
	mmfo_market_code,
	' ' as is_newrow
from ci.mmfo
order by mmfo_market_code,mmfo_param_key
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

   }
}