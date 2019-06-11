using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken,2019/3/5
/// GetMaxDate/ListDateArea/ListKind 這三個函數與48030完全相同,之後可考慮合併
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 個別契約風險價格係數狀況表
   /// </summary>
   public class D48040 {

      private Db db;

      public D48040() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// get MAX(PDK_DATE) in HPDK
      /// </summary>
      /// <param name="as_ymd">yyyy/MM/dd</param>
      /// <returns></returns>
      public string GetMaxDate(string as_ymd) {
         object[] parms =
         {
                ":as_ymd", as_ymd
            };

         string sql = @"
select to_char(MAX(PDK_DATE),'yyyy/mm/dd') as ldt_date
from ci.HPDK
where PDK_DATE <= TO_DATE(:as_ymd, 'YYYY/MM/DD')
and PDK_DATE >= TO_DATE(:as_ymd, 'YYYY/MM/DD') - INTERVAL '90' DAY
";

         return db.ExecuteScalar(sql , CommandType.Text , parms);
      }

      /// <summary>
      /// list data from ai2 , return AI2_SELECT/mon_diff/sdate/edate/day_cnt
      /// </summary>
      /// <param name="ad_edate"></param>
      /// <param name="ad_next_edate"></param>
      /// <returns></returns>
      public DataTable ListDateArea(DateTime ad_edate , DateTime ad_next_edate) {
         object[] parms ={
                ":ad_edate", ad_edate,
                ":ad_next_edate", ad_next_edate
            };

         string sql = @"
select 'Y' as AI2_SELECT,
    mon_diff,
    case when to_number(to_char(sdate,'DD')) > to_number(to_char(:ad_next_edate,'DD')) then sdate - (to_number(to_char(sdate,'DD')) - to_number(to_char(:ad_next_edate,'DD'))) else sdate end as sdate,
    :ad_edate as edate,
    count(*) as day_cnt 
from ci.ai2,
(
    select mth_cnt,mon_diff,case when ((to_number(to_char(:ad_next_edate,'YYYY')) - to_number(to_char(sdate,'YYYY')))*12) + to_number(to_char(:ad_next_edate,'mm')) - to_number(to_char(sdate,'mm')) >= abs(mth_cnt)  and
                              to_char(sdate,'dd') < to_char(:ad_next_edate,'dd') then sdate + 1 else sdate end as sdate
    from
    (
        select 3 as mth_cnt,'3個月' as mon_diff,add_months(:ad_next_edate,-3) as sdate  from dual
         union all
        select 6,'6個月',add_months(:ad_next_edate,-6)   from dual
         union all
        select 12,'1年',add_months(:ad_next_edate,-12)   from dual
         union all
        select 24,'2年',add_months(:ad_next_edate,-24)   from dual
         union all
        select 36,'3年',add_months(:ad_next_edate,-36)   from dual)
)
where ai2_ymd < to_char(:ad_next_edate,'yyyymmdd')
  and ai2_sum_type = 'D'
  and ai2_sum_subtype = '1' 
  and ai2_prod_type = 'F'
  and ai2_ymd >= to_char(sdate,'yyyymmdd')
group by mth_cnt,sdate,:ad_next_edate,mon_diff
order by mth_cnt";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// list data from hcpr , return 11 fields
      /// </summary>
      /// <param name="ad_date"></param>
      /// <returns></returns>
      public DataTable ListKind(DateTime ad_date) {

         object[] parms = { ":ad_date" , ad_date };

         string sql = @"
select 'Y' as cpr_select,
    pdk_kind_id as cpr_kind_id,
    cast(max(case when row_num = 1 then cpr_price_risk_rate else 0 end) as number(12,6)) as cpr_price_risk_rate,
    cast(max(case when row_num = 1 then cpr_price_risk_rate else 0 end)  * 0.2 as number(12,6))  as risk_interval,
    max(case when row_num = 1 then cpr_effective_date else null end) as cpr_effective_date,
    cast(max(case when row_num = 2 then cpr_price_risk_rate else null end) as number(12,6))  as last_risk_rate,

    cpr_prod_subtype,
    pdk_prod_type as prod_type,
    cpr_kind_id as cpr_param_key,
    'Y' as cpr_modify_flag,
    cast(max(case when row_num = 1 then cpr_price_risk_rate else 0 end)  as number(12,6)) as cpr_price_risk_rate_org
    
from ci.hcpr,
      (select pdk_prod_type,pdk_subtype,pdk_param_key,pdk_kind_id 
        from ci.hpdk 
        where pdk_date = :ad_date and (pdk_subtype = 'S' or pdk_param_key = pdk_kind_id)) p,
      (select cpr_kind_id as max_kind_id,cpr_effective_date as max_effective_date,
              row_number( ) over (partition by cpr_kind_id order by cpr_effective_date desc nulls last) as row_num
        from ci.hcpr
        where cpr_effective_date <= :ad_date) c
 where row_num <= 2
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_kind_id = pdk_param_key 
  group by cpr_prod_subtype,cpr_kind_id,pdk_kind_id,pdk_prod_type
order by prod_type , cpr_kind_id";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// list data from MG1 , return mg1_ymd/mg1_risk/mg1_min_risk
      /// </summary>
      /// <param name="as_kind_id"></param>
      /// <param name="ad_sdate"></param>
      /// <param name="ad_edate"></param>
      /// <param name="as_model_type"></param>
      /// <returns></returns>
      public DataTable ListKindByKindId(string as_kind_id , string ad_sdate , string ad_edate , string as_model_type) {

         object[] parms = {
                ":as_kind_id", as_kind_id,
                ":ad_sdate", ad_sdate,
                ":ad_edate", ad_edate,
                ":as_model_type", as_model_type
            };

         string sql = @"
select mg1_ymd, 
    round(mg1_cp_risk, 4) as mg1_risk,
    mg1_min_risk
from ci.mg1_3m
where trim(mg1_kind_id) = :as_kind_id
and mg1_ymd >= :ad_sdate
and mg1_ymd <= :ad_edate
and mg1_ab_type in ('-','A')
and mg1_model_type = :as_model_type
order by mg1_ymd";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
