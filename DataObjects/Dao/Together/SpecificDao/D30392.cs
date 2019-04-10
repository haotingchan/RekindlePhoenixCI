using System;
using System.Data;

/// <summary>
/// Winni,2019/4/9
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 國外指數類期貨契約價量資料
   /// </summary>
   public class D30392 : DataGate {


      /// <summary>
      /// get ci.ai3 data,return 8 fields
      /// </summary>
      /// <param name="as_kind_id"></param>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable d_ai3(string as_kind_id , DateTime as_sdate , DateTime as_edate) {

         object[] parms = {
                ":as_kind_id", as_kind_id.PadRight(7,' '),
                ":as_sdate", as_sdate,
                ":as_edate", as_edate
            };

         string sql = @"
select 
    ai3_date,   
    ai3_close_price,
    ai3_m_qnty,   
    ai3_oi,   
    ai3_index,   
    ai3_amount  ,   
    ai3_last_close_price ,
    tfxmmd_px
from ci.ai3  ,
    (select 
        tfxmmd_ymd,
        tfxmmd_px
    from ci.tfxmmd
    where tfxmmd_ymd >= to_char(:as_sdate,'YYYYMMDD')
    and tfxmmd_ymd <= to_char(:as_edate,'YYYYMMDD')
    and tfxmmd_mth_seq_no = 1
    and tfxmmd_kind_id = :as_kind_id  )
where ai3_kind_id = :as_kind_id  
and ai3_date >= :as_sdate   
and ai3_date <= :as_edate     
and ai3_date = to_date(tfxmmd_ymd(+),'YYYYMMDD')
order by ai3_date 
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.ai2 data,return 10 fields
      /// </summary>
      /// <param name="as_kind_id"></param>
      /// <param name="as_ym">yyyyMM</param>
      /// <param name="as_last_ym">yyyyMM</param>
      /// <returns></returns>
      public DataTable d_ai2_ym(string as_kind_id , string as_last_ym , string as_ym) {

         object[] parms = {
                ":as_kind_id", as_kind_id.PadRight(7,' '),
                ":as_ym", as_ym,
                ":as_last_ym", as_last_ym
            };

         string sql = @"
select 
    ai2_param_key, 
    sum(case when ai2_ymd >= :as_last_ym||'01' and ai2_ymd <= :as_last_ym||'31' then ai2_m_qnty else 0 end) as last_m_qnty,
    sum(case when ai2_ymd >= :as_last_ym||'01' and ai2_ymd <= :as_last_ym||'31' then ai2_oi else 0 end) as last_m_oi,
    sum(case when ai2_ymd >= :as_last_ym||'01' and ai2_ymd <= :as_last_ym||'31' then 1 else 0 end) as last_m_day_cnt,
    sum(case when ai2_ymd >= :as_ym||'01' and ai2_ymd <= :as_ym||'31' then ai2_m_qnty else 0 end) as cur_m_qnty,
    sum(case when ai2_ymd >= :as_ym||'01' and ai2_ymd <= :as_ym||'31' then ai2_oi else 0 end) as cur_m_oi,
    sum(case when ai2_ymd >= :as_ym||'01' and ai2_ymd <= :as_ym||'31' then 1 else 0 end) as cur_m_day_cnt,
    sum(case when ai2_ymd >= substr(:as_ym,1,4)||'0101'  and ai2_ymd <= :as_ym||'31' then ai2_m_qnty else 0 end) as y_qnty,
    sum(case when ai2_ymd >= substr(:as_ym,1,4)||'0101'  and ai2_ymd <= :as_ym||'31' then ai2_oi else 0 end) as y_oi,
    sum(case when ai2_ymd >= substr(:as_ym,1,4)||'0101'  and ai2_ymd <= :as_ym||'31' then 1 else 0 end) as y_day_cnt 
from ci.ai2 
where ai2_ymd >= case when substr(:as_last_ym,1,4) < substr(:as_ym,1,4) then :as_last_ym||'01' else substr(:as_ym,1,4)||'0101'  end 
and ai2_ymd <= :as_ym||'31'
and ai2_sum_type = 'D'
and ai2_sum_subtype = '3'
and ai2_prod_type = 'F'
--and AI2_PROD_SUBTYPE like '%'
and ai2_param_key = :as_kind_id
group by ai2_param_key
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.am2 data,return 6 fields
      /// </summary>
      /// <param name="as_param_key">char(7)</param>
      /// <param name="as_sym">yyyyMM</param>
      /// <param name="as_eym">yyyyMM</param>
      /// <returns></returns>
      public DataTable d_am2(string as_param_key , string as_sym , string as_eym) {

         object[] parms = {
                ":as_param_key", as_param_key.PadRight(7,' '),
                ":as_sym", as_sym,
                ":as_eym", as_eym
            };

         string sql = @"
select 
    am2_idfg_type,   
    am2_bs_code,   
    sum(am2_m_qnty) as am2_m_qnty ,   
    substr(am2_ymd,1,6) as am2_ymd,   
    am2_sum_type  ,
    '3' as sort_type  
from ci.am2  
where am2_sum_type = 'M'  
and  am2_ymd >= :as_sym 
and  am2_ymd <= :as_eym  
and  am2_param_key = :as_param_key  
and  am2_sum_subtype = '3'  
and  am2_idfg_type in ('1','2','3','5','6','7','8','9')
group by  
    am2_idfg_type,   
    am2_bs_code,   
    am2_sum_type,   
    substr(am2_ymd,1,6)  
order by sort_type , am2_ymd , am2_idfg_type , am2_bs_code
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.aprf data,return 6 fields
      /// </summary>
      /// <param name="as_sdate">yyyyMM</param>
      /// <param name="as_edate">yyyyMM</param>
      /// <returns></returns>
      public DataTable d_30392_aprf(DateTime as_sdate , DateTime as_edate) {

         object[] parms = {
                ":as_sdate", as_sdate,
                ":as_edate", as_edate
            };

         string sql = @"
select 
    to_date(aprf_ymd,'YYYYMMDD') as data_date,
    amif_high_price as high_price,
    amif_low_price as low_price,
    aprf_raise_price as raise_price,
    aprf_fall_price as fall_price,
    aprf_raise_price1 as raise_price1,
    aprf_fall_price1 as fall_price1,
    case when aprf_raise_open1 = 'N' then ' ' else aprf_raise_open1 end as open1,
    aprf_raise_price2 as raise_price2,
    aprf_fall_price2 as fall_price2,
    case when aprf_raise_open2 = 'N' then ' ' else aprf_raise_open2 end as open2
from ci.aprf,
    (select 
        amif_date,amif_kind_id,amif_settle_date,amif_high_price,amif_low_price
    from ci.amif
    where amif_date >= :as_sdate
    and  amif_date <= :as_edate
    and amif_kind_id = 'TJF'
    and amif_mth_seq_no = 1) t
where aprf_ymd >= to_char(:as_sdate,'YYYYMMDD')
and aprf_ymd<= to_char(:as_edate,'YYYYMMDD')
and aprf_ymd = to_char(amif_date,'YYYYMMDD')
and aprf_kind_id = amif_kind_id
and aprf_settle_date = amif_settle_date
order by data_date
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
