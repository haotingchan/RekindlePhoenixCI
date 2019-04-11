using System;
using System.Data;
/// <summary>
/// Winni, 2019/4/10
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49071 : DataGate {

      /// <summary>
      /// get ci.spnt2 data return spnt2_kind_id/spnt2_delta_xxx/spnt2_w_time/spnt2_w_user_id/is_newrow 5 feild (D49071)
      /// </summary>
      /// <returns></returns>
      public DataTable ListData() {

         string sql = @"
select 
    spnt2_kind_id,
    spnt2_delta_xxx,
    spnt2_w_time,
    spnt2_w_user_id,
    ' ' as is_newrow 
from ci.spnt2
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// get ci.spt1 data return spt1_kind_id1 1 feild (dddw_spnt2_kind_id)
      /// </summary>
      /// <returns></returns>
      public DataTable GetDdlSpt1() {

         string sql = @"
select spt1_kind_id1
from
   (select 
		spt1_kind_id1 
	from ci.spt1
	where spt1_kind_id2 <> '-'
	group by spt1_kind_id1
	union all
	select 
		spt1_kind_id2 
	from ci.spt1
	where spt1_kind_id2 <> '-'
	group by spt1_kind_id2)
group by spt1_kind_id1
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }
   }
}
