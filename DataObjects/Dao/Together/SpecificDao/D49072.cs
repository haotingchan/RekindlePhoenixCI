using System;
using System.Data;
/// <summary>
/// Winni, 2019/4/10
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49072 : DataGate {

      /// <summary>
      /// get ci.spnt1 data return spnt1_type/spnt1_days/spnt1_val/spnt1_w_time/spnt1_w_user_id/is_newrow 6 feild (D49072)
      /// PB sort 順序特殊符號會在前面，oracle用decode設定改寫即可
      /// </summary>
      /// <returns></returns>
      public DataTable ListData() {

         string sql = @"
select 
    spnt1_type,
    spnt1_days,
    spnt1_val,
    spnt1_w_time,
    spnt1_w_user_id,
    ' ' as is_newrow
from ci.spnt1
--讓'^'開根號會在最前面
order by decode(spnt1_type,'^',1) ,upper(spnt1_type), spnt1_days

";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      //直接用COD的ListByTxn
      /// <summary>
      /// get ci.cod data return spnt1_type/spnt1_type_name 2 feild (dddw_spnt1_type)
      /// </summary>
      /// <returns></returns>
      //      public DataTable GetDdlSpt1() {

      //         string sql = @"
      //select 
      //    trim(cod_id) as spnt1_type,
      //    trim(cod_desc) as spnt1_type_name
      //from ci.cod 
      //where cod_txn_id = 'SPNT1' 
      //and cod_col_id = 'SPNT1_TYPE'
      //";
      //         DataTable dtResult = db.GetDataTable(sql , null);

      //         return dtResult;
      //      }
   }
}
