using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/3/28
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 30400 股票期貨價量統計表
   /// </summary>
   public class D30400 : DataGate {

      /// <summary>
      /// get CI.AI2 data (d_30401)
      /// return ai2_ymd/ai2_prod_subtype/ai2_pc_code/ai2_m_qnty/ai2_oi/ai2_mmk_qnty 6 field
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="prodType">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable Get30401Data(string startDate , string endDate , string prodType) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate,
                ":prodType", prodType
            };


         string sql = @"
select 
   ai2_ymd,   
   ai2_prod_subtype,   
   ai2_pc_code,   
   sum(ai2_m_qnty) as ai2_m_qnty,   
   sum(ai2_oi) as ai2_oi,   
   sum(ai2_mmk_qnty) as ai2_mmk_qnty  
from ci.ai2  
where ai2_sum_type = 'D'  
and ai2_ymd >= :startDate  
and ai2_ymd <= :endDate  
and ai2_sum_subtype = '5'  
and ai2_prod_type = :prodType 
and ai2_prod_subtype = 'S' 
and ai2_param_key <> 'STO'
group by ai2_ymd, ai2_prod_subtype, ai2_pc_code   
order by ai2_ymd 
";

         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }
   }
}
