using System;
using System.Data;

/// <summary>
/// Lukas, 2019/3/11
/// ken,2019/3/12 改寫
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 特製行情表(經濟工商)
   /// </summary>
   public class D30055 : DataGate {


      /// <summary>
      /// 主要指數期貨商品行情表
      /// return 商品,開盤,最高價,最低價,收盤指數/每日結算價,漲跌,成交量(口),未平倉量(口),較前一日增減(口),RPT_SEQ_NO
      /// </summary>
      /// <param name="as_ocf_ymd"></param>
      /// <param name="as_prev_ymd"></param>
      /// <returns></returns>
      public DataTable wf_30055_b(string as_ocf_ymd, string as_prev_ymd) {

         object[] parms = {
                ":as_ocf_ymd", as_ocf_ymd,
                ":as_prev_ymd", as_prev_ymd
            };

         string sql = @"
SELECT MIF.AMIF_KIND_ID,
    MIF.AMIF_OPEN_PRICE,
    MIF.AMIF_HIGH_PRICE,
    MIF.AMIF_LOW_PRICE,
    MIF.AMIF_SETTLE_PRICE,

    MIF.AMIF_UP_DOWN_VAL,
    MIF.AMIF_M_QNTY_TAL,
    MIF.AMIF_OPEN_INTEREST,
    (MIF.AMIF_OPEN_INTEREST -Y.AMIF_OPEN_INTEREST) AS AI2_OI_UP_DOWN,
    RPT_SEQ_NO
FROM
(SELECT AMIF_KIND_ID2 AS AMIF_KIND_ID,AMIF_SETTLE_DATE,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_UP_DOWN_VAL,AMIF_M_QNTY_TAL,AMIF_OPEN_INTEREST
  FROM ci.AMIF
 WHERE AMIF_DATE = TO_DATE(:as_ocf_ymd,'yyyymmdd')
   AND AMIF_PROD_TYPE = 'F'
   AND AMIF_KIND_ID IN ('TXF','EXF','FXF','MXF','MX1','MX2','MX4','MX5','TJF','BTF','I5F','SPF','UDF','UNF')
   AND AMIF_DELIVERY_MTH_SEQ_NO = 1) MIF,
(SELECT AMIF_KIND_ID,AMIF_SETTLE_DATE,AMIF_M_QNTY_TAL,AMIF_OPEN_INTEREST 
  FROM CI.AMIF
 WHERE AMIF_DATE = TO_DATE(:as_prev_ymd,'yyyymmdd')
   AND AMIF_PROD_TYPE = 'F'
   AND AMIF_KIND_ID IN ('TXF','EXF','FXF','MXF','MX1','MX2','MX4','MX5','TJF','BTF','I5F','SPF','UDF','UNF')
   AND AMIF_DELIVERY_MTH_SEQ_NO in (1,2)) Y,
(SELECT RPT_VALUE as RPT_KIND_ID,RPT_SEQ_NO 
   FROM ci.RPT
  where RPT_TXN_ID = '30055' and RPT_TXD_ID = '30055b') R
WHERE MIF.AMIF_KIND_ID = Y.AMIF_KIND_ID(+)
  and MIF.AMIF_SETTLE_DATE = Y.AMIF_SETTLE_DATE(+)
  and MIF.AMIF_KIND_ID = RPT_KIND_ID(+)
union all
--現貨指數
select KIND_ID,OPEN_PRICE,HIGH_PRICE,LOW_PRICE,CLOSE_PRICE,UP_DOWN_VAL,NULL,NULL,NULL,RPT_SEQ_NO
  from 
      (SELECT RPT_VALUE as RPT_KIND_ID,RPT_LEVEL_1 as RPT_SEQ_NO 
         FROM ci.RPT
        where RPT_TXN_ID = '30055' and RPT_TXD_ID = '30055b') R,
      (select case when AMIF_KIND_ID is null then AMIFU_KIND_ID else AMIF_KIND_ID end as KIND_ID,
             case when AMIF_OPEN_PRICE is null then AMIFU_OPEN_PRICE else AMIF_OPEN_PRICE end as OPEN_PRICE,
             case when AMIF_HIGH_PRICE is null then AMIFU_HIGH_PRICE else AMIF_HIGH_PRICE end as HIGH_PRICE,
             case when AMIF_LOW_PRICE is null then AMIFU_LOW_PRICE else AMIF_LOW_PRICE end as LOW_PRICE,
             case when AMIF_CLOSE_PRICE is null then AMIFU_CLOSE_PRICE else AMIF_CLOSE_PRICE end as CLOSE_PRICE,
             case when AMIF_UP_DOWN_VAL is null then AMIFU_UP_DOWN_VAL else AMIF_UP_DOWN_VAL end as UP_DOWN_VAL
        from (select AMIF_KIND_ID,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_CLOSE_PRICE,AMIF_UP_DOWN_VAL
                from ci.AMIF
               where AMIF_DATE = TO_DATE(:as_ocf_ymd,'yyyymmdd')
                 and AMIF_DATA_SOURCE = 'U'
                 and AMIF_SETTLE_DATE = '000000'                      
                 and AMIF_KIND_ID in ('TXF','EXF','FXF')) FULL OUTER JOIN 
             (select AMIFU_KIND_ID,max(AMIFU_OPEN_PRICE) as AMIFU_OPEN_PRICE,
                     max(AMIFU_HIGH_PRICE) as AMIFU_HIGH_PRICE,
                     max(AMIFU_LOW_PRICE) as AMIFU_LOW_PRICE,
                     max(AMIFU_CLOSE_PRICE) as AMIFU_CLOSE_PRICE,
                     max(AMIFU_UP_DOWN_VAL) as AMIFU_UP_DOWN_VAL
                from ci.AMIFU
               where AMIFU_DATE = TO_DATE(:as_ocf_ymd,'yyyymmdd')
                 and AMIFU_DATA_SOURCE = 'R'
                 and AMIFU_SETTLE_DATE = '000000' 
                 and AMIFU_KIND_ID in ('TXF','EXF','FXF')
               group by AMIFU_KIND_ID) 
          ON ( AMIF_KIND_ID = AMIFU_KIND_ID))        
 where kind_id = RPT_KIND_ID
 order by RPT_SEQ_NO
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 台指選擇權(近月及一週到期)主要序列行情表
      /// </summary>
      /// <param name="adt_date"></param>
      /// <param name="an_strike_price"></param>
      /// <param name="ai_txo_row_cnt"></param>
      /// <param name="ai_txw_row_cnt"></param>
      /// <returns></returns>
      public DataTable wf_30055_tx(DateTime adt_date, decimal an_strike_price, int ai_txo_row_cnt, int ai_txw_row_cnt) {

         object[] parms = {
                ":adt_date", adt_date,
                ":an_strike_price", an_strike_price,
                ":ai_txo_row_cnt", ai_txo_row_cnt,
                ":ai_txw_row_cnt", ai_txw_row_cnt
            };

         string sql = @"
select AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE,
       AMIF_KIND_ID,RPT_SEQ_NO
  from
    (select AMIF_KIND_ID,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE
      from
          (select AMIF_KIND_ID,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE 
             from ci.amif
            where amif_date = :adt_date
              and amif_kind_id = 'TXO'
              and amif_delivery_mth_seq_no = 1
              and amif_strike_price >= :an_strike_price
            order by amif_strike_price,amif_pc_code)
     where rownum <= :ai_txo_row_cnt + 2
    union all
    select AMIF_KIND_ID,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE 
      from
          (select AMIF_KIND_ID,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE 
             from ci.amif
            where amif_date = :adt_date
              and amif_kind_id = 'TXO'
              and amif_delivery_mth_seq_no = 1
              and amif_strike_price < :an_strike_price
            order by amif_strike_price desc,amif_pc_code)
    where rownum <= :ai_txo_row_cnt 
    union all
    select AMIF_KIND_ID2,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE 
      from
          (select AMIF_KIND_ID2,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE 
            from ci.amif
           where amif_date = :adt_date
             and amif_kind_id2 = 'TXW'
             and amif_delivery_mth_seq_no = 1
             and amif_strike_price >= :an_strike_price
           order by amif_strike_price,amif_pc_code)
    where rownum <= :ai_txw_row_cnt + 2
    union all
    select AMIF_KIND_ID2,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE 
     from
          (select AMIF_KIND_ID2,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_SETTLE_PRICE,AMIF_M_QNTY_TAL,AMIF_STRIKE_PRICE,AMIF_PC_CODE 
             from ci.amif
            where amif_date = :adt_date
              and amif_kind_id2 = 'TXW'
              and amif_delivery_mth_seq_no = 1
              and amif_strike_price < :an_strike_price
            order by amif_strike_price desc,amif_pc_code)
     where rownum <= :ai_txw_row_cnt),
    (SELECT RPT_VALUE as RPT_KIND_ID,RPT_SEQ_NO 
       FROM ci.RPT
      where RPT_TXN_ID = '30055' and RPT_TXD_ID = '30055tx') R
 where AMIF_KIND_ID = RPT_KIND_ID(+)
 order by rpt_seq_no, amif_strike_price, amif_pc_code
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 大額交易人－指數期貨, return 14 fields
      /// </summary>
      /// <param name="adt_date"></param>
      /// <param name="adt_last_date"></param>
      /// <returns></returns>
      public DataTable wf_30055_big_keep(DateTime adt_date, DateTime adt_last_date) {

         object[] parms = {
                ":adt_date", adt_date,
                ":adt_last_date", adt_last_date
            };

         //ken,調整不少,主要是filter和sort
         string sql = @"
select m.*, 
(case when instr(m.month_type,'W') > 0 then 0 when m.month_type='000000' then 1 else 2 end) as cp_seq_no 

from (
    select t.toi1_date,   
         t.toi1_prod_type,   
         t.toi1_prod_subtype,   
         t.toi1_b_10,   
         t.toi1_s_10, 
           
         t.toi1_net_10,
         t.toi1_net_10 - y.toi1_net_10 as net_up_down, 
         --y.toi1_net_10,
         t.toi1_kind_id,   
         t.toi1_com_name,   
         case when t.month_type = '999999' or instr(t.month_type,'W') > 0 then t.month_type 
              else  '000000' end as month_type,   
         t.toi1_pc_code,  
         t.toi1_seq_no,
         rpt_seq_no  
    from 
        (select toi1_date,   
                toi1_prod_type,   
                toi1_prod_subtype,   
                toi1_kind_id,  
                toi1_com_name,   

                toi1_month_type as month_type,   
                toi1_pc_code,    
                toi1_b_10,   
                toi1_s_10,    
                toi1_net_10,
                toi1_seq_no 
           from ci.toi1 t
          where toi1_date = :adt_date
            and toi1_seq_no <> 0 and toi1_seq_no <= 10) t,
        (select toi1_date,   
                toi1_prod_type,   
                toi1_prod_subtype,   
                toi1_kind_id,  
                toi1_month_type as month_type,   
                toi1_pc_code,    
                toi1_net_10 
           from ci.toi1 y
          where toi1_date = :adt_last_date) y,
        (select rpt_value_2 as rpt_kind_id,rpt_level_1 as rpt_seq_no 
           from ci.rpt
          where rpt_txn_id = '30055' and rpt_txd_id = '30055t') r
      
    where t.toi1_prod_type = y.toi1_prod_type(+) 
    and t.toi1_prod_subtype = y.toi1_prod_subtype(+)
    and t.toi1_kind_id = y.toi1_kind_id(+)
    and t.month_type = y.month_type(+) 
    and t.toi1_pc_code = y.toi1_pc_code(+)
    and t.toi1_kind_id = rpt_kind_id(+)
) m
where nvl(m.rpt_seq_no,'0')<>'0'
order by rpt_seq_no , cp_seq_no , toi1_prod_type , toi1_prod_subtype , toi1_seq_no
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 三大法人－指數期貨, return 10 fields
      /// </summary>
      /// <param name="adt_date"></param>
      /// <param name="adt_last_date"></param>
      /// <returns></returns>
      public DataTable wf_30055_three_keep(DateTime adt_date, DateTime adt_last_date) {

         object[] parms = {
                ":adt_date", adt_date,
                ":adt_last_date", adt_last_date
            };

         //ken,調整不少,主要是filter和sort
         string sql = @"
select m.*, 
(case btinoivl3f_acc_type when 'C' then 1 when 'B' then 2 else 3 end) as cp_seq_no 

from (
    --期貨
    select t.btinoivl3f_prodid,
            ' ' as pc_code,
            trim(t.btinoivl3f_acc_type) as btinoivl3f_acc_type,
            t.btinoivl3f_oib_qnty,
            t.btinoivl3f_ois_qnty,
            
           (t.btinoivl3f_oib_qnty - t.btinoivl3f_ois_qnty) as oi_net,
           (t.btinoivl3f_oib_qnty - t.btinoivl3f_ois_qnty) - (y.btinoivl3f_oib_qnty - y.btinoivl3f_ois_qnty) as oi_up_down,
           (y.btinoivl3f_oib_qnty - y.btinoivl3f_ois_qnty) as oi_last_net,
           rpt_seq_no                
      from ci.m_btinoivl3f t,
           ci.m_btinoivl3f y, 
          (select rpt_value as rpt_kind_id,rpt_seq_no 
             from ci.rpt
            where rpt_txn_id = '30055' and rpt_txd_id = '30055t') r
     where t.btinoivl3f_ocfdate = :adt_date
       and t.btinoivl3f_prodid in ('TXF','EXF','FXF')
       and y.btinoivl3f_ocfdate = :adt_last_date
       and y.btinoivl3f_prodid in ('TXF','EXF','FXF')
       and t.btinoivl3f_prodid = y.btinoivl3f_prodid
       and t.btinoivl3f_acc_type = y.btinoivl3f_acc_type
       and t.btinoivl3f_prodid = rpt_kind_id(+)
       
    union all
    --TXO台指選擇權
    select t.btinoivl4_prodid,
            t.btinoivl4_pc_code as pc_code,
            trim(t.btinoivl4_acc_type) as btinoivl4_acc_type,
            t.btinoivl4_oib_qnty,
            t.btinoivl4_ois_qnty,
            (t.btinoivl4_oib_qnty - t.btinoivl4_ois_qnty) as oi_net,
            (t.btinoivl4_oib_qnty - t.btinoivl4_ois_qnty) - (y.btinoivl4_oib_qnty - y.btinoivl4_ois_qnty) as oi_up_down,
            (y.btinoivl4_oib_qnty - y.btinoivl4_ois_qnty) as oi_last_net,
            rpt_seq_no  
    from ci.m_btinoivl4 t,ci.m_btinoivl4 y, 
          (select rpt_value as rpt_kind_id,rpt_seq_no 
             from ci.rpt
            where rpt_txn_id = '30055' and rpt_txd_id = '30055t') r
    where t.btinoivl4_ocfdate = :adt_date
      and t.btinoivl4_prodid = 'TXO'
      and y.btinoivl4_ocfdate = :adt_last_date
      and y.btinoivl4_prodid = 'TXO'
      and t.btinoivl4_prodid = y.btinoivl4_prodid
      and t.btinoivl4_pc_code = y.btinoivl4_pc_code
      and t.btinoivl4_acc_type = y.btinoivl4_acc_type
      and t.btinoivl4_prodid = rpt_kind_id(+)
) m
where nvl(m.rpt_seq_no,'0')<>'0'
order by rpt_seq_no , cp_seq_no , btinoivl3f_acc_type desc , pc_code
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 2.6主要股票期貨商品行情表（依未平倉量）
      /// return 11 fields
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <param name="as_last_ymd"></param>
      /// <param name="as_param_key"></param>
      /// <returns></returns>
      public DataTable wf_30055_stf(string as_ymd, string as_last_ymd, string as_param_key = "STF") {

         object[] parms = {
                ":as_ymd", as_ymd,
                ":as_last_ymd", as_last_ymd,
                ":as_param_key", as_param_key.PadRight(7,' ')
            };

         string sql = @"
select apdk_name,
    t.kind_id2,

    amif_open_price,
    amif_high_price,
    amif_low_price,
    amif_settle_price,

    amif_up_down_val,
    t.m_qnty,
    t.oi,
    (t.oi - y.oi) as oi_up_down,

    t.seq_no  
from ci.amif,
    ci.apdk,
    (select kind_id2,m_qnty,oi,rownum as seq_no
     from
         (select substr(ai2_kind_id,1,2) as kind_id2,sum(ai2_m_qnty) as m_qnty,sum(ai2_oi) as oi
          from ci.ai2
         where ai2_ymd = :as_ymd
           and ai2_sum_type = 'D'
           and ai2_sum_subtype = '4'
           and ai2_prod_type = 'F'
           and ai2_param_key = :as_param_key
         group by substr(ai2_kind_id,1,2)
         order by sum(ai2_oi) desc)
    where rownum <= (case when :as_param_key = 'ETF    ' then 9 else 5 end)) t,
    (select substr(ai2_kind_id,1,2) as kind_id2,sum(ai2_oi) as oi
        from ci.ai2
        where ai2_ymd = :as_last_ymd
         and ai2_sum_type = 'D'
         and ai2_sum_subtype = '4'
         and ai2_prod_type = 'F'
         and ai2_param_key = :as_param_key
        group by substr(ai2_kind_id,1,2)) y,
    (select amif_kind_id as m_kind_id,min(amif_settle_date) as min_settle_date 
        from ci.amif 
        where amif_date = to_date(:as_ymd,'yyyymmdd') 
         and amif_prod_type = 'F' 
         and amif_param_key = :as_param_key
         and amif_delivery_mth_seq_no >= 1 
        group by amif_kind_id) m
 where t.kind_id2 = y.kind_id2
   and amif_kind_id = m_kind_id
   and amif_settle_date = min_settle_date
   and amif_kind_id = apdk_kind_id
   and trim(amif_kind_id)= t.kind_id2||'F'
   and amif_date = to_date(:as_ymd,'yyyymmdd')
   and amif_prod_type = 'F'
   and amif_param_key = :as_param_key
order by seq_no
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 2.7主要ETF選擇權(近月價平)序列行情表
      /// </summary>
      /// <param name="adt_date"></param>
      /// <returns></returns>
      public DataTable wf_30055_etc(DateTime adt_date,string pdk_param_key="ETC") {

         object[] parms = {
                ":adt_date", adt_date,
                ":pdk_param_key",pdk_param_key.PadRight(7,' ')
            };

         string sql = @"
select * from (
   select max(case when amif_pc_code = 'C' then amif_open_price else 0 end) as c_open_price,
          max(case when amif_pc_code = 'C' then amif_high_price else 0 end) as c_high_price,
          max(case when amif_pc_code = 'C' then amif_low_price else 0 end) as c_low_price,
          max(case when amif_pc_code = 'C' then amif_settle_price else 0 end) as c_settle_price,
          max(case when amif_pc_code = 'C' then amif_m_qnty_tal else 0 end) as c_m_qnty_tal,
          
          pdk_name,
          amif_strike_price,

          max(case when amif_pc_code = 'P' then amif_open_price else 0 end) as p_open_price,
          max(case when amif_pc_code = 'P' then amif_high_price else 0 end) as p_high_price,
          max(case when amif_pc_code = 'P' then amif_low_price else 0 end) as p_low_price,
          max(case when amif_pc_code = 'P' then amif_settle_price else 0 end) as p_settle_price,
          max(case when amif_pc_code = 'P' then amif_m_qnty_tal else 0 end) as p_m_qnty_tal,
          
          --amif_kind_id,
          --amif_settle_date,
          sum(amif_m_qnty_tal) as m_qnty
     from
         --近月價平     
         (select amif_kind_id2,amif_kind_id,amif_settle_date,amif_open_price,amif_high_price,amif_low_price,amif_settle_price,amif_m_qnty_tal,amif_strike_price,amif_pc_code,pdk_name,
                 row_number( ) over (partition by amif_kind_id,amif_pc_code order by amif_kind_id,amif_pc_code,amif_strike_price desc nulls last) as kind_seq_no
            from ci.amif,
              (select pdk_date as strike_date,pdk_kind_id as strike_kind_id ,pdk_prod_idx as an_strike_price ,pdk_name
                 from ci.hpdk 
                 where pdk_date = :adt_date
                   and pdk_param_key = :pdk_param_key)
           where amif_date = strike_date
             and amif_kind_id = strike_kind_id
             and amif_delivery_mth_seq_no = 1
             and amif_strike_price <= an_strike_price)
   where kind_seq_no = 1
   group by pdk_name,amif_strike_price, amif_kind_id,amif_settle_date
   order by m_qnty desc
) where rownum < 9
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 2.8人民幣匯率期貨行情表
      /// </summary>
      /// <param name="adt_date"></param>
      /// <param name="as_last_ymd"></param>
      /// <param name="as_type">C/E</param>
      /// <returns></returns>
      public DataTable wf_30055_prod_subtype(string as_ymd, string as_last_ymd, string as_type = "E") {

         string as_txd_id = "30055" + as_type;//char(10)

         object[] parms = {
                ":as_ymd", as_ymd,
                ":as_last_ymd",as_last_ymd,
                ":as_type",as_type,
                ":as_txd_id",as_txd_id.PadRight(10,' ')
            };

         string sql = @"
select apdk_name,
    amif_settle_date,
    amif_open_price,
    amif_high_price,
    amif_low_price,

    amif_settle_price,
    amif_up_down_val,
    t.m_qnty,
    t.oi,
    (t.oi - y.oi) as oi_up_down,

    t.kind_id2,
    rpt_row + row_number( ) over (partition by apdk_name order by apdk_name ,amif_settle_date nulls last)  - 1 as seq_no
from ci.amif,
    ci.apdk,
    (select substr(ai2_kind_id,1,2) as kind_id2,sum(ai2_m_qnty) as m_qnty,sum(ai2_oi) as oi, 0 as seq_no,ai2_settle_date
          from ci.ai2
         where ai2_ymd = :as_ymd
           and ai2_sum_type = 'D'
           and ai2_sum_subtype = '7'
           and ai2_prod_type = 'F'
           and ai2_prod_subtype = :as_type
         group by substr(ai2_kind_id,1,2) ,ai2_settle_date
         order by substr(ai2_kind_id,1,2) desc) t,
    (select substr(ai2_kind_id,1,2) as kind_id2,sum(ai2_oi) as oi,ai2_settle_date
        from ci.ai2
        where ai2_ymd = :as_last_ymd
         and ai2_sum_type = 'D'
         and ai2_sum_subtype = '7'
         and ai2_prod_type = 'F'
         and ai2_prod_subtype = :as_type
        group by substr(ai2_kind_id,1,2),ai2_settle_date) y,
    (select rpt_value as rpt_kind_id,rpt_level_1 as rpt_row
        from ci.rpt
        where rpt_txn_id = '30055' and rpt_txd_id = :as_txd_id)
 where amif_prod_type = 'F'
   and amif_prod_subtype = :as_type
   and amif_delivery_mth_seq_no >= 1  
   and amif_delivery_mth_seq_no <= 2
   and trim(amif_kind_id)= t.kind_id2||'F'
   and trim(amif_kind_id)= y.kind_id2(+)||'F'
   and amif_kind_id = apdk_kind_id
   and amif_settle_date = t.ai2_settle_date
   and amif_settle_date = y.ai2_settle_date(+)
   and amif_kind_id = rpt_kind_id
   and amif_date = to_date(:as_ymd,'yyyymmdd')
order by seq_no , kind_id2 desc, amif_settle_date
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


      /// <summary>
      /// 2.9 人民幣匯率選擇權主要序列行情表(依成交量)
      /// </summary>
      /// <param name="adt_date"></param>
      /// <returns></returns>
      public DataTable wf_30055_rho(DateTime adt_date) {
         object[] parms = {
                ":adt_date", adt_date
            };

         string sql = @"
select * from (
   select max(case when amif_pc_code = 'C' then amif_open_price else 0 end) as c_open_price,
          max(case when amif_pc_code = 'C' then amif_high_price else 0 end) as c_high_price,
          max(case when amif_pc_code = 'C' then amif_low_price else 0 end) as c_low_price,
          max(case when amif_pc_code = 'C' then amif_settle_price else 0 end) as c_settle_price,
          max(case when amif_pc_code = 'C' then amif_m_qnty_tal else 0 end) as c_m_qnty_tal,

          apdk_name as pdk_name,
          amif_strike_price,
          max(case when amif_pc_code = 'P' then amif_open_price else 0 end) as p_open_price,
          max(case when amif_pc_code = 'P' then amif_high_price else 0 end) as p_high_price,
          max(case when amif_pc_code = 'P' then amif_low_price else 0 end) as p_low_price,
          max(case when amif_pc_code = 'P' then amif_settle_price else 0 end) as p_settle_price,
          max(case when amif_pc_code = 'P' then amif_m_qnty_tal else 0 end) as p_m_qnty_tal,

          amif_kind_id,
          amif_settle_date,
          sum(amif_m_qnty_tal) as m_qnty,
          sum(amif_open_interest) as oi
     from ci.apdk, ci.amif 
   where amif_date = :adt_date
     and amif_kind_id = apdk_kind_id
     and amif_prod_type = 'O'
     and amif_prod_subtype = 'E'  
     and amif_mth_seq_no = 1
   group by apdk_name,amif_strike_price, amif_kind_id,amif_settle_date
   order by m_qnty desc, oi desc, amif_strike_price
) 
where rownum < 4
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 2.12 股票期貨週
      /// </summary>
      /// <param name="adt_sdate"></param>
      /// <param name="adt_edate"></param>
      /// <returns></returns>
      public DataTable wf_30055_weekly(DateTime adt_sdate, DateTime adt_edate) {
         object[] parms = {
                ":adt_sdate", adt_sdate,
                ":adt_edate", adt_edate
            };

         string sql = @"
select * from (
   select row_number() over(order by ai2_m_qnty desc) row_num,
      --AI2_KIND_ID, 
      trim(apdk_name) as apdk_name, 
      ai2_m_qnty
   from ci.apdk,
      (select trim(ai2_kind_id2)||'F' as ai2_kind_id, sum(ai2_m_qnty) as ai2_m_qnty
      from ci.ai2
      where ai2_ymd >= to_char(:adt_sdate,'yyyymmdd')
        and ai2_ymd <= to_char(:adt_edate,'yyyymmdd')
        and ai2_prod_type = 'F'
        and ai2_param_key = 'STF'
        and ai2_sum_type = 'D'
        and ai2_sum_subtype = '4'
      group by ai2_kind_id2)
   where trim(apdk_kind_id(+)) = ai2_kind_id
   order by ai2_m_qnty desc
) where rownum<=10
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 統一刪除多的ROW
      /// </summary>
      /// <returns></returns>
      public DataTable wf_del_row(){
         

         string sql = @"
select nvl(max(case when rpt_txd_id = '30055b'  and rpt_value = 'MXW' then rpt_seq_no else 0 end),0) as mxw_seq_no,
       nvl(max(case when rpt_txd_id = '30055tx' and rpt_value = 'TXO' then rpt_level_1 else 0 end),0) as txo_seq_no_end,
       nvl(max(case when rpt_txd_id = '30055tx' and rpt_value = 'TXO' then rpt_level_2 else 0 end),0) as txo_seq_no_start,
       nvl(max(case when rpt_txd_id = '30055tx' and rpt_value = 'TXW' then rpt_level_1 else 0 end),0) as txw_seq_no_end,
       nvl(max(case when rpt_txd_id = '30055tx' and rpt_value = 'TXW' then rpt_level_2 else 0 end),0) as txw_seq_no_start,
       nvl(max(case when rpt_txd_id = '30055t'  and rpt_value = 'TXO' then rpt_level_1 else 0 end),0) as toi_seq_no
  from ci.rpt
where rpt_txn_id = '30055'
  and (    (rpt_txd_id = '30055b'  and rpt_value = 'MXW')
        or (rpt_txd_id = '30055tx' and rpt_value in ('TXO','TXW'))
        or (rpt_txd_id = '30055t'  and rpt_value = 'TXO'))
";
         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

   }
}
