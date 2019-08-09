using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/3/13
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 黃金類契約保證金調整補充說明
   /// </summary>
   public class D40140 {

      private Db db;

      public D40140() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// get ci.mgd2/ci.mgt2/ci.mgt6 data (d40140_1_mg1)
      /// return mgd2_cur_cm/mgd2_cm/mgd2_adj_rate/mgd2_im/mgd2_kind_id/mgt2_abbr_name 6 fields 
      /// </summary>
      /// <param name="as_date">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable GetGoldData(DateTime as_date) {

         object[] parms = {
                ":as_date", as_date
            };


         string sql = @"
select 
   --原本結算保證金改為原始保證金
   mgd2_cur_im,   
   mgd2_im as mgd2_im_c,   
   mgd2_adj_rate,   
   mgd2_im,   
   mgd2_kind_id,   
   mgt2_abbr_name  
from 
   ci.mgd2,   
   ci.mgt2,
   ci.mgt6
where mgd2_ymd = to_char(:as_date,'YYYYMMDD')
and mgd2_prod_type = mgt2_prod_type
and mgd2_kind_id = mgt2_kind_id
and mgd2_kind_id = mgt6_kind_id
and mgt6_grp_id = 'GOLD'
and mgd2_adj_code = 'Y' 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.mg1_3m/ci.rpt/ci.mg8d/ci.mg8/ci.mg9/ci.mgt8/ci.hexrt data return 12 fields (d40140_2) 
      /// </summary>
      /// <param name="ad_date">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable ListMoneyData(DateTime ad_date) {

         object[] parms = {
                ":ad_date", ad_date
            };


         string sql = @"
select 
   '1' as data_type,
   'TAIFEX'as com,
   mg1_kind_id,
   mg1_cm,
   mg1_mm,
   mg1_im,
   mg1_price,
   0 as exchange_rate,
   seq_no as rpt_seq_no,
   mg1_im as out_im,
   '' as f_name,
   mg1_xxx
from 
   ci.mg1_3m,
   (select rpt_value,min(rpt_seq_no) as seq_no 
    from ci.rpt
    where rpt_txd_id = '40140_2'
    group by rpt_value) r
where mg1_ymd = to_char(:ad_date,'YYYYMMDD')
and mg1_model_type = 'S'
and mg1_kind_id in ('GDF','TGF')
and mg1_kind_id = rpt_value

union all

select 
   '2',
   mgt8_f_id,
   mgt8_pdk_kind_id,
   mg8_cm,
   mg8_mm,
   mg8_im,
   mg9_price,
   nvl(case when hexrt_count_currency <> '1' then hexrt_market_exchange_rate else hexrt_exchange_rate end,0),
   seq_no,
   mg8_im as out_im,       
   mgt8_f_exchange,
   mgt8_xxx
from 
   ci.mg8d,
   ci.mg8,
   ci.mg9,
   ci.mgt8,
   (select rpt_value,min(rpt_seq_no) as seq_no 
    from ci.rpt
    where rpt_txd_id = '40140_2'
    group by rpt_value) r,
   (select hexrt_currency_type,hexrt_count_currency,hexrt_exchange_rate,hexrt_market_exchange_rate
    from ci.hexrt
    where hexrt_date = :ad_date) e
where mg8d_ymd = to_char(:ad_date,'YYYYMMDD')
--保證金
and mg8d_effect_ymd = mg8_effect_ymd
and mg8d_f_id = mg8_f_id
--基本資料
and mg8d_f_id = mgt8_f_id
and mgt8_pdk_kind_id = 'GDF' 
--報表位置
and mg8d_f_id = rpt_value(+)
--價格
and mg8d_ymd = mg9_ymd
and mg8d_f_id = mg9_f_id
--匯率
and mgt8_currency_type = hexrt_currency_type(+)   
and case when mgt8_f_id = 'TOC01' then '2' else '1' end = hexrt_count_currency(+)
order by rpt_seq_no
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
