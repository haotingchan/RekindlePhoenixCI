using System;
using System.Data;

/// <summary>
/// Winni 2019/04/30
/// </summary>
namespace DataObjects.Dao.Together {
   public class D40021 : DataGate {
      /// <summary>
      /// get data by ci.rpt,ci.sp1 (d_40020_9)
      /// </summary>
      /// <param name="as_date">yyyy/MM/dd</param>
      /// <param name="as_osw_grp"></param>
      /// <param name="as_txd_id"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_date , string as_osw_grp , string as_txd_id) {
         object[] parms =
         {
                ":as_date", as_date,
                ":as_osw_grp", as_osw_grp,
                ":as_txd_id", as_txd_id
            };

         string sql = @"
select  
    s.*,
    rpt_txn_id,   
    rpt_txd_id,   
    rpt_seq_no,   
    rpt_value,   
    rpt_level_1,   
    rpt_level_2,   
    rpt_level_3,    
    rpt_value_2,   
    rpt_value_3,   
    rpt_value_4
from ci.rpt ,
    (select * from ci.sp1
    where sp1_date = :as_date 
    and sp1_osw_grp like :as_osw_grp)s
where rpt_txd_id = :as_txd_id
and trim(rpt_value) = trim(sp1_kind_id1)
and  trim(rpt_value_3) = trim(sp1_kind_id2)
and trim(rpt_value_4) = trim(sp1_type)
order by rpt_level_1, sp1_type, rpt_level_3, rpt_level_2
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// (d_40020_7)
      /// </summary>
      /// <param name="as_date">yyyy/MM/dd</param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public DataTable GetRiskData(DateTime as_date , string as_osw_grp) {
         object[] parms =
         {
                ":as_date", as_date,
                ":as_osw_grp", as_osw_grp
            };

         string sql = @"
select 
    sp1_date,   
    sp1_type,
    sp1_kind_id1,  
    sp1_kind_id2,    
    sp1_change_range,  
    sp1_change_cond,
    sp1_rate as rate,
    sp1_cur_rate as cur_rate,
    sp1_seq_no,
    case when sp1_change_flag = 'Y' then 'Y' else 'N' end as sp1_flag,
    m1.mgt2_kind_id_out as sp1_kind_id1_out,
    m2.mgt2_kind_id_out as sp1_kind_id2_out
from 
    ci.sp1,
    ci.mgt2 m1,
    ci.mgt2 m2 
where sp1_date = :as_date   
and sp1_osw_grp like :as_osw_grp
and sp1_kind_id1 = m1.mgt2_kind_id(+)
and sp1_kind_id2 = m2.mgt2_kind_id(+)
order by sp1_date , decode( sp1_type ,'SV',1,'SD',2,'SS',3 ) , sp1_seq_no , sp1_kind_id1 , sp1_kind_id2
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      //Span參數日狀況表(一)
      public DataTable GetRowColNum1() {

         string sql = @"
select 
    rpt_level_1 ii_ole_row,
    rpt_level_2 li_row,
    rpt_value_2 li_col
from ci.rpt
where rpt_txn_id = '40020'
and rpt_txd_id = '40020_5e'
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      //Span參數日狀況表(二)
      public DataTable GetRowColNum2() {

         string sql = @"
select
    rpt_level_1 ii_ole_row,
    rpt_value_2 li_col 
from ci.rpt
where rpt_txn_id = '40020'
and rpt_txd_id = '40020_6e'
and rpt_value = 'SD'
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      //Span參數日狀況表(三)
      public DataTable GetRowColNum3() {

         string sql = @"
select
    rpt_level_1 ii_ole_row,
    rpt_value_2 li_col 
from ci.rpt
where rpt_txn_id = '40020'
  and rpt_txd_id = '40020_6e'
  and rpt_value = 'SS'
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }
   }
}
