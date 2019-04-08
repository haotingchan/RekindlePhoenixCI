using System;
using System.Data;

/// <summary>
/// ken,2019/4/8
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 黃金期貨契約價量資料
   /// </summary>
   public class D30395 : DataGate {


      /// <summary>
      /// get ai3 data,return 8 fields
      /// </summary>
      /// <param name="as_kind_id"></param>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable d_ai3(string as_kind_id, DateTime as_sdate, DateTime as_edate) {

         object[] parms = {
                ":as_kind_id", as_kind_id,
                ":as_sdate", as_sdate,
                ":as_edate", as_edate
            };

         string sql = @"
select ai3_date,   
    ai3_close_price,   
    ai3_m_qnty,   
    ai3_oi,   
    ai3_index,  
    
    ai3_amount,   
    ai3_last_close_price,
    tfxmmd_px
from ci.ai3  ,
    (select tfxmmd_ymd,tfxmmd_px
    from ci.tfxmmd
    where tfxmmd_ymd >= to_char(:as_sdate,'YYYYMMDD')
    and tfxmmd_ymd <= to_char(:as_edate,'YYYYMMDD')
    and tfxmmd_mth_seq_no = 1
    and tfxmmd_kind_id = :as_kind_id  )
where ai3_date = to_date(tfxmmd_ymd(+),'YYYYMMDD')
and ai3_date >= :as_sdate   
and ai3_date <= :as_edate     
and ai3_kind_id = :as_kind_id  
order by ai3_date
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// get ai2 data,return 6 fields
      /// </summary>
      /// <param name="as_param_key">char(7)</param>
      /// <param name="as_sym">yyyyMM</param>
      /// <param name="as_eym">yyyyMM</param>
      /// <returns></returns>
      public DataTable d_ai2(string as_param_key, string as_sym, string as_eym) {

         object[] parms = {
                ":as_param_key", as_param_key.PadRight(7,' '),
                ":as_sym", as_sym,
                ":as_eym", as_eym
            };

         string sql = @"
select am2_idfg_type,   
    am2_bs_code,   
    sum(am2_m_qnty) as am2_m_qnty ,   
    substr(am2_ymd,1,6) as am2_ymd,   
    am2_sum_type,
    
    '3' as sort_type
from ci.am2  
where am2_sum_type = 'M'   
and am2_sum_subtype = '3'  
and am2_idfg_type in ('1','2','3','5','6','7','8','9')
and am2_ymd >= :as_sym  
and am2_ymd <= :as_eym  
and am2_param_key = :as_param_key  
group by am2_idfg_type,am2_bs_code,am2_sum_type,substr(am2_ymd,1,6)
order by sort_type , am2_ymd , am2_idfg_type , am2_bs_code
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
