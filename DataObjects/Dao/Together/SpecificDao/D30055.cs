using Common;
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
      public DataTable wf_30055_b(string as_ocf_ymd , string as_prev_ymd) {

         object[] parms = {
                ":as_ocf_ymd", as_ocf_ymd,
                ":as_prev_ymd", as_prev_ymd
            };

         string sql =
@"
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
         DataTable dtResult = db.GetDataTable(sql , parms);

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
      public DataTable wf_30055_tx(DateTime adt_date , decimal an_strike_price , int ai_txo_row_cnt , int ai_txw_row_cnt) {

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
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// 大額交易人－指數期貨, return 14 fields
      /// </summary>
      /// <param name="adt_date"></param>
      /// <param name="adt_last_date"></param>
      /// <returns></returns>
      public DataTable wf_30055_toi1(DateTime adt_date , DateTime adt_last_date) {

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
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// 三大法人－指數期貨, return 10 fields
      /// </summary>
      /// <param name="adt_date"></param>
      /// <param name="adt_last_date"></param>
      /// <returns></returns>
      public DataTable wf_30055_3(DateTime adt_date , DateTime adt_last_date) {

         object[] parms = {
                ":adt_date", adt_date,
                ":adt_last_date", adt_last_date
            };

         //ken,調整不少,主要是filter和sort
         string sql = @"
select m.*, 
(case btinoivl3f_acc_type when 'C' then 1 when 'B' then 2 else 3 end) as cp_seq_no 

from (
    select t.btinoivl3f_prodid,
            ' ' as pc_code,
            trim(t.btinoivl3f_acc_type) as btinoivl3f_acc_type,
            t.btinoivl3f_oib_qnty,
            t.btinoivl3f_ois_qnty,
            
           (t.btinoivl3f_oib_qnty - t.btinoivl3f_ois_qnty) as oi_net,
           (t.btinoivl3f_oib_qnty - t.btinoivl3f_ois_qnty) - (y.btinoivl3f_oib_qnty - y.btinoivl3f_ois_qnty) as oi_up_down,
           (y.btinoivl3f_oib_qnty - y.btinoivl3f_ois_qnty) as oi_last_net,
           rpt_seq_no                
      from ci.m_btinoivl3f t,ci.m_btinoivl3f y, 
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
    select t.btinoivl4_prodid,t.btinoivl4_pc_code as pc_code,t.btinoivl4_acc_type,t.btinoivl4_oib_qnty ,t.btinoivl4_ois_qnty,
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
order by rpt_seq_no , cp_seq_no , btinoivl3f_acc_type
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }



   }
}
