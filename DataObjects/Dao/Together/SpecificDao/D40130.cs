using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/4/3
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 40130 保證金計算流程表
   /// </summary>
   public class D40130 : DataGate {

      /// <summary>
      /// get ci.mgt2/ci.apdk/ci.hcpr (dddw_mgt2_kind_idname_edit + d_40130_mg1 +  + d_40132)
      /// return mgt2_kind_id_out/mgt2_kind_id/apdk_name/cpr_price_risk_rate/cp_display 5 fields 
      /// </summary>
      /// <param name="startDate">yyyy/MM/dd</param>
      /// <param name="kindId">xxx%</param>
      /// <returns></returns>
      public DataTable GetDataList(DateTime startDate, string kindId) {

         object[] parms = {
                ":startDate", startDate,
                ":kindId",kindId
            };

         string sql = @"
select
    mgt2_seq_no,
    mgt2_kind_id_out, 
    mgt2.mgt2_kind_id,
    --契約名稱
    apdk.apdk_name,
    --min風險價格係數
    h.cpr_price_risk_rate,
    trim(mgt2.mgt2_kind_id)||' - '||trim(mgt2.mgt2_abbr_name) as cp_display
from ci.mgt2
left join 
    ci.apdk on apdk.apdk_kind_id = mgt2.mgt2_kind_id 
left join 
    (select * from 
        (select 
            cpr_kind_id as max_kind_id,
         max(cpr_effective_date) as max_effective_date 
       from ci.hcpr  
       where CPR_EFFECTIVE_DATE <= :startDate  group by cpr_kind_id) tmp 
    left join ci.hcpr
        on hcpr.cpr_kind_id = tmp.max_kind_id
        and hcpr.cpr_effective_date = tmp.max_effective_date
) h on h.cpr_kind_id = mgt2.mgt2_kind_id
where nvl(mgt2.mgt2_data_type,' ') = ' '
and mgt2.mgt2_kind_id like :kindId
union 
select 0,' ','% ',' 全部', 0 ,'% - 全部 ' from dual
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.ai5 (d_40131)
      /// return ai5_date/ai5_settle_price/ai5_kind_id/ai5_open_ref 4 fields 
      /// </summary>
      /// <param name="startDate">yyyy/MM/dd</param>
      /// <param name="endDate">yyyy/MM/dd</param>
      /// <param name="kindId">xxx%</param>
      /// <returns></returns>
      public DataTable GetAi5Data(DateTime startDate , DateTime endDate , string kindId) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate,
                ":kindId",kindId
            };

         string sql = @"
select 
    ai5_date,   
    ai5_settle_price,   
    ai5_kind_id,   
    ai5_open_ref  
from ci.ai5 
where ai5_date >= :startDate
and ai5_date <= :endDate
and ai5_kind_id like :kindId
order by ai5_date desc
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.idxf (d_40132)
      /// return idxf_date/idxf_idx/idxf_kind_id 3 fields 
      /// </summary>
      /// <param name="startDate">yyyy/MM/dd</param>
      /// <param name="endDate">yyyy/MM/dd</param>
      /// <param name="kindId">xxx%</param>
      /// <returns></returns>
      public DataTable GetIdxfData(DateTime startDate , DateTime endDate , string kindId) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate,
                ":kindId",kindId
            };

         string sql = @"
select
	idxf_date, 
	idxf_idx, 
	idxf_kind_id
from ci.idxf
where idxf_date >= :startDate 
and idxf_date <= :endDate 
and idxf_kind_id like :kindId
order by idxf_date desc
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.mg1 (d_40130_mg1)
      /// return mgt2_kind_id_out/mgt2_kind_id/apdk_name/cpr_price_risk_rate/cp_display 5 fields 
      /// </summary>
      /// <param name="startDate">yyyy/MM/dd</param>
      /// <param name="kindId">xxx%</param>
      /// <returns></returns>
      public DataTable GetMg1Data(DateTime startDate  , string kindId) {

         object[] parms = {
                ":startDate", startDate,
                ":kindId",kindId
            };

         string sql = @"
select
mg1_date, 
mg1_prod_type, 
mg1_kind_id, 
mg1_type, 
mg1_cur_cm, 
mg1_cur_mm, 
mg1_cur_im, 
mg1_cp_cm, 
mg1_cm, 
mg1_mm, 
mg1_im, 
mg1_risk, 
mg1_cp_risk, 
mg1_change_range, 
mg1_price, 
mg1_currency_type, 
mg1_m_multi, 
mg1_i_multi, 
mg1_xxx
from　ci.mg1
where mg1_date = :startDate
and mg1_kind_id like :kindId
order by mg1_type
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
