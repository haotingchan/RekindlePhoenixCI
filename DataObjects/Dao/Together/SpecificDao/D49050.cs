using System;
using System.Data;
/// <summary>
/// Winni, 2019/4/12
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49050 : DataGate {

      /// <summary>
      /// get ci.mgt3 data return mgt3_date_fm/mgt3_date_to/mgt3_memo/mgt3_w_time/mgt3_w_user_id/is_newrow 6 feild (d_49050)
      /// </summary>
      /// <returns></returns>
      public DataTable GetDataList() {

         string sql = @"
select
    mgt3_date_fm,
    mgt3_date_to, 
    mgt3_memo, 
    mgt3_w_time, 
    mgt3_w_user_id, 

    ' '  as is_newrow
from ci.mgt3
order by mgt3_date_to , mgt3_date_fm
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// get ci.mgt2 data return mgt2_seq_no/mgt2_kind_id/mgt2_prod_subtype 3 feild (dddw_mgt2_kind_id_edit) KindId下拉選單
      /// </summary>
      /// <returns></returns>
      public DataTable GetDdlKindId() {

         string sql = @"
select 
    mgt2_seq_no,
    mgt2_kind_id,
    mgt2_prod_subtype
from ci.mgt2    
where (nvl(mgt2_data_type,' ') = ' ' or mgt2_kind_id in ('ETF','ETC'))
order by mgt2_seq_no
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }
   }
}
