using BusinessObjects;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Winni, 2019/5/3
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D40080 : DataGate {

      /// <summary>
      /// d40080
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_date) {
         object[] parms = {
                ":as_date", as_date
            };

         string sql = @"
select 
	sp1_date,   
	sp1_type,
	decode( sp1_type ,  'SV',1,'SD',2,'SS',3 ) cp_type_sort,
	sp1_kind_id1,  
	sp1_kind_id2,    
	sp1_change_range,   
	case when sp2_adj_code is null then ' ' else sp2_adj_code end as  sp2_adj_code,   
	case when sp2_span_code is null then ' ' else sp2_span_code end as sp2_span_code, 
	sp2_value_date as sp2_value_date, 
	case when sp2_adj_code is null then ' ' else sp2_adj_code end as  sp2_adj_code_org,   
	case when sp2_span_code is null then ' ' else sp2_span_code end as sp2_span_code_org, 
	sp2_value_date as sp2_value_date_org,
	case when sp2_date is null then 'I' else ' ' end as op_type,
	sp1_change_cond,
	sp1_rate as rate,
	sp1_cur_rate as cur_rate,
	sp1_seq_no,
	nvl(apdk_prod_type,param_prod_type) as apdk_prod_type,
	nvl(apdk_prod_subtype,param_prod_subtype) as apdk_prod_subtype,
	sp1_osw_grp,
   decode(sp1_osw_grp,1,'Group1 (13:45)',5,'Group2 (16:15)') as osw_grp
from ci.sp1, ci.sp2,ci.apdk,ci.apdk_param
where sp1_date = :as_date 
and sp1_change_flag = 'Y'
and sp1_kind_id1 = apdk_kind_id(+)
and sp1_kind_id1 = param_key(+)
and sp1_date = sp2_date(+)
and sp1_type = sp2_type(+)
and sp1_kind_id1 = sp2_kind_id1(+)
and sp1_kind_id2 = sp2_kind_id2(+)
order by sp1_date , sp1_osw_grp, cp_type_sort , sp1_seq_no , sp1_kind_id1 , sp1_kind_id2
";
         return db.GetDataTable(sql , parms);
      }

      /// <summary>
      /// d40080_sp2
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public DataTable GetSP2Data(DateTime as_date) {
         object[] parms = {
                ":as_date", as_date
            };

         string sql = @"
select 
	sp2_date, 
	sp2_type, 
	sp2_kind_id1, 
	sp2_kind_id2, 
	sp2_value_date, 
	sp2_w_time, 
	sp2_w_user_id, 
	sp2_osw_grp, 
	sp2_span_code, 
	sp2_adj_code
from ci.sp2
where sp2_date = :as_date
";
         return db.GetDataTable(sql , parms);
      }
   }
}
