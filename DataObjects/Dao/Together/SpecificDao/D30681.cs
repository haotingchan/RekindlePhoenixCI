using System;
using System.Data;

/// <summary>
/// ken,2019/4/9
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 成交價偏離幅度統計表
   /// </summary>
   public class D30681 : DataGate {

      /// <summary>
      /// get ftprices data, return 16 fields
      /// </summary>
      /// <param name="as_fm_date"></param>
      /// <param name="as_to_date"></param>
      /// <param name="as_sc_code"></param>
      /// <param name="as_kind_id1"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_mth_seq1"></param>
      /// <param name="as_mth_seq2"></param>
      /// <returns></returns>
      public DataTable d_30681_s_new(DateTime as_fm_date,
                                      DateTime as_to_date,
                                      string as_sc_code,
                                      string as_kind_id1,
                                      string as_kind_id2,
                                      int as_mth_seq1,
                                      int as_mth_seq2) {

         object[] parms = {
                ":as_fm_date", as_fm_date,
                ":as_to_date", as_to_date,
                ":as_sc_code", as_sc_code,
                ":as_kind_id1", as_kind_id1+"%",
                ":as_kind_id2", as_kind_id2+"%",
                ":as_mth_seq1", as_mth_seq1,
                ":as_mth_seq2", as_mth_seq2
            };

         string sql = @"
select ftprices_market_code as ""交易時段: 0一般 / 1夜盤"",   
   to_char(ftprices_trade_date,'mm/dd/yy hh24:mi:ss') as ""交易日期"",   
   ftprices_prod_type as ""F期貨 / O選擇權"",   
   ftprices_sc_code as ""S單式 / C複式"",   
   ftprices_prod_id as ""序列"",

   ftprices_kind_id1 as ""第一支腳契約代碼"",   
   ftprices_mth_seq1 as ""第一支腳月份序號"",   
   ftprices_kind_id2 as ""第二支腳契約代碼"",   
   ftprices_mth_seq2 as ""第二支腳月份序號"",  
   ftprices_level1 as ""級距1"",   

   ftprices_level2 as ""級距2"",   
   ftprices_level3 as ""級距3"",   
   ftprices_level4 as ""級距4"",   
   ftprices_level5 as ""級距5"",   
   ftprices_level6 as ""級距6"",   

   ftprices_level7 as ""級距7""  
from ci.ftprices  
where ftprices_trade_date >= :as_fm_date
and ftprices_trade_date <= :as_to_date
and ftprices_prod_type = 'F'
and ftprices_sc_code like :as_sc_code
and ftprices_kind_id1 like :as_kind_id1
and ftprices_kind_id2 like :as_kind_id2
and (:as_mth_seq1 = 99 or ftprices_mth_seq1 = :as_mth_seq1)
and (:as_mth_seq2 = 99 or ftprices_mth_seq2 = :as_mth_seq2)
order by ftprices_market_code , ftprices_trade_date , ftprices_prod_type , ftprices_prod_id     
";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// get tprices data, return 16 fields
      /// </summary>
      /// <param name="as_fm_date"></param>
      /// <param name="as_to_date"></param>
      /// <param name="as_sc_code"></param>
      /// <param name="as_kind_id1"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_mth_seq1"></param>
      /// <param name="as_mth_seq2"></param>
      /// <returns></returns>
      public DataTable d_30681_s(DateTime as_fm_date,
                                      DateTime as_to_date,
                                      string as_sc_code,
                                      string as_kind_id1,
                                      string as_kind_id2,
                                      int as_mth_seq1,
                                      int as_mth_seq2) {

         object[] parms = {
                ":as_fm_date", as_fm_date,
                ":as_to_date", as_to_date,
                ":as_sc_code", as_sc_code,
                ":as_kind_id1", as_kind_id1+"%",
                ":as_kind_id2", as_kind_id2+"%",
                ":as_mth_seq1", as_mth_seq1,
                ":as_mth_seq2", as_mth_seq2
            };

         string sql = @"
select tprices_market_code as ""交易時段: 0一般 / 1夜盤"",   
   to_char(tprices_trade_date,'mm/dd/yy hh24:mi:ss') as ""交易日期"",   
   tprices_prod_type as ""F期貨 / O選擇權"",   
   tprices_sc_code as ""S單式 / C複式"",   
   tprices_prod_id as ""序列"",

   tprices_kind_id1 as ""第一支腳契約代碼"",   
   tprices_mth_seq1 as ""第一支腳月份序號"",   
   tprices_kind_id2 as ""第二支腳契約代碼"",   
   tprices_mth_seq2 as ""第二支腳月份序號"",  
   tprices_level1 as ""級距1"",   

   tprices_level2 as ""級距2"",   
   tprices_level3 as ""級距3"",   
   tprices_level4 as ""級距4"",   
   tprices_level5 as ""級距5"",   
   tprices_level6 as ""級距6"",   

   tprices_level7 as ""級距7""  
from ci.tprices  
where tprices_trade_date >= :as_fm_date
and tprices_trade_date <= :as_to_date
and tprices_prod_type = 'F'
and tprices_sc_code like :as_sc_code
and tprices_kind_id1 like :as_kind_id1
and tprices_kind_id2 like :as_kind_id2
and (:as_mth_seq1 = 99 or tprices_mth_seq1 = :as_mth_seq1)
and (:as_mth_seq2 = 99 or tprices_mth_seq2 = :as_mth_seq2)
order by tprices_market_code , tprices_trade_date , tprices_prod_type , tprices_prod_id     
";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }





      /// <summary>
      /// get ftpriced data, return 22 fields
      /// </summary>
      /// <param name="as_fm_date"></param>
      /// <param name="as_to_date"></param>
      /// <param name="as_sc_code"></param>
      /// <param name="as_kind_id1"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_mth_seq1"></param>
      /// <param name="as_mth_seq2"></param>
      /// <param name="as_order_type"></param>
      /// <param name="as_order_cond"></param>
      /// <param name="as_level_list">陣列轉字串,直接傳入字串,用逗號分開</param>
      /// <param name="as_null"></param>
      /// <returns></returns>
      public DataTable d_30681_d_new(DateTime as_fm_date,
                                      DateTime as_to_date,
                                      string as_sc_code,
                                      string as_kind_id1,
                                      string as_kind_id2,
                                      int as_mth_seq1,
                                      int as_mth_seq2,
                                      string as_order_type,
                                      string as_order_cond,
                                      string as_level_list,
                                      string as_null) {

         object[] parms = {
                ":as_fm_date", as_fm_date,
                ":as_to_date", as_to_date,
                ":as_sc_code", as_sc_code,
                ":as_kind_id1", as_kind_id1+"%",
                ":as_kind_id2", as_kind_id2+"%",
                ":as_mth_seq1", as_mth_seq1,
                ":as_mth_seq2", as_mth_seq2,
                ":as_order_type", as_order_type,
                ":as_order_cond", as_order_cond,
                ":as_null", as_null
            };

         //ken,如果陣列字串為空,sql會出錯,所以有值才輸出where條件
         string temp = string.IsNullOrEmpty(as_level_list) ? "" : "ftpriced_level in (" + as_level_list + ") or ";

         string sql = string.Format(@"
select to_char(ftpriced_trade_date,'mm/dd/yy hh24:mi:ss') as ""交易日期"",   
    ftpriced_market_code as ""交易時段: 0一般 / 1夜盤"",  
    ftpriced_prod_type as ""F期貨 / O選擇權"",   
    ftpriced_sc_code as ""S單式 / C複式"",   
    ftpriced_prod_id as ""序列"", 
      
    ftpriced_kind_id1 as ""第一支腳契約代碼"", 
    ftpriced_mth_seq1 as ""第一支腳月份序號"",
    ftpriced_kind_id2 as ""第二支腳契約代碼"",
    ftpriced_mth_seq2 as ""第二支腳月份序號"",
    ftpriced_order_seq as ""委託序號"",
      
    to_char(ftpriced_order_time,'mm/dd/yy hh24:mi:ss') as ""委託時間"",
    ftpriced_bs_code as ""買賣別"",
    ftpriced_order_type as ""委託方式"",
    ftpriced_order_cond as ""委託條件"",
    ftpriced_order_price as ""委託價"",
     
    ftpriced_order_qnty as ""委託口數"",
    ftpriced_m_inst as ""撮合序號"",
    to_char(ftpriced_m_time,'mm/dd/yy hh24:mi:ss') as ""成交時間"",
    ftpriced_m_price as ""成交價"",
    ftpriced_m_qnty as ""成交口數"",
     
    ftpriced_price as ""基準價格"",
    ftpriced_level as ""級距""
from ci.ftpriced  
where ftpriced_trade_date >= :as_fm_date
and ftpriced_trade_date <= :as_to_date
and ftpriced_prod_type = 'F'
and ftpriced_sc_code like :as_sc_code
and nvl(ftpriced_kind_id1,' ') like :as_kind_id1
and nvl(ftpriced_kind_id2,' ') like :as_kind_id2
and (:as_mth_seq1 = 99 or ftpriced_mth_seq1 = :as_mth_seq1)
and (:as_mth_seq2 = 99 or ftpriced_mth_seq2 = :as_mth_seq2)
and nvl(ftpriced_order_type,' ') like :as_order_type
and nvl(ftpriced_order_cond,' ') like :as_order_cond
and ( {0} (:as_null = 'Y' and ftpriced_level is null) )
order by ftpriced_trade_date , ftpriced_market_code , ftpriced_prod_type , ftpriced_m_time
", temp);

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// get tpriced data, return 22 fields
      /// </summary>
      /// <param name="as_fm_date"></param>
      /// <param name="as_to_date"></param>
      /// <param name="as_sc_code"></param>
      /// <param name="as_kind_id1"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_mth_seq1"></param>
      /// <param name="as_mth_seq2"></param>
      /// <param name="as_order_type"></param>
      /// <param name="as_order_cond"></param>
      /// <param name="as_level_list">陣列轉字串,直接傳入字串,用逗號分開</param>
      /// <param name="as_null"></param>
      /// <returns></returns>
      public DataTable d_30681_d(DateTime as_fm_date,
                                 DateTime as_to_date,
                                 string as_sc_code,
                                 string as_kind_id1,
                                 string as_kind_id2,
                                 int as_mth_seq1,
                                 int as_mth_seq2,
                                 string as_order_type,
                                 string as_order_cond,
                                 string as_level_list,
                                 string as_null) {

         object[] parms = {
                ":as_fm_date", as_fm_date,
                ":as_to_date", as_to_date,
                ":as_sc_code", as_sc_code,
                ":as_kind_id1", as_kind_id1+"%",
                ":as_kind_id2", as_kind_id2+"%",
                ":as_mth_seq1", as_mth_seq1,
                ":as_mth_seq2", as_mth_seq2,
                ":as_order_type", as_order_type,
                ":as_order_cond", as_order_cond,
                ":as_null", as_null
            };

         //ken,如果陣列字串為空,sql會出錯,所以有值才輸出where條件
         string temp = string.IsNullOrEmpty(as_level_list) ? "" : "tpriced_level in (" + as_level_list + ") or ";

         string sql = string.Format(@"
select to_char(tpriced_trade_date,'mm/dd/yy hh24:mi:ss') as ""交易日期"",   
    tpriced_market_code as ""交易時段: 0一般 / 1夜盤"",  
    tpriced_prod_type as ""F期貨 / O選擇權"",   
    tpriced_sc_code as ""S單式 / C複式"",   
    tpriced_prod_id as ""序列"", 
      
    tpriced_kind_id1 as ""第一支腳契約代碼"", 
    tpriced_mth_seq1 as ""第一支腳月份序號"",
    tpriced_kind_id2 as ""第二支腳契約代碼"",
    tpriced_mth_seq2 as ""第二支腳月份序號"",
    tpriced_order_seq as ""委託序號"",
      
    to_char(tpriced_order_time,'mm/dd/yy hh24:mi:ss') as ""委託時間"",
    tpriced_bs_code as ""買賣別"",
    tpriced_order_type as ""委託方式"",
    tpriced_order_cond as ""委託條件"",
    tpriced_order_price as ""委託價"",
     
    tpriced_order_qnty as ""委託口數"",
    tpriced_m_inst as ""撮合序號"",
    to_char(tpriced_m_time,'mm/dd/yy hh24:mi:ss') as ""成交時間"",
    tpriced_m_price as ""成交價"",
    tpriced_m_qnty as ""成交口數"",
     
    tpriced_price as ""基準價格"",
    tpriced_level as ""級距"" 
from ci.tpriced  
where tpriced_trade_date >= :as_fm_date
and tpriced_trade_date <= :as_to_date
and tpriced_prod_type = 'F'
and tpriced_sc_code like :as_sc_code
and tpriced_kind_id1 like :as_kind_id1
and tpriced_kind_id2 like :as_kind_id2
and (:as_mth_seq1 = 99 or tpriced_mth_seq1 = :as_mth_seq1)
and (:as_mth_seq2 = 99 or tpriced_mth_seq2 = :as_mth_seq2)
and tpriced_order_type like :as_order_type
and tpriced_order_cond like :as_order_cond
and ( {0} (:as_null = 'Y' and tpriced_level is null) )
order by tpriced_trade_date , tpriced_market_code , tpriced_prod_type , tpriced_m_time
", temp);

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// get tpricemtf+tpriced data, return 30 fields
      /// </summary>
      /// <param name="as_fm_date"></param>
      /// <param name="as_to_date"></param>
      /// <param name="as_sc_code"></param>
      /// <param name="as_kind_id1"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_mth_seq1"></param>
      /// <param name="as_mth_seq2"></param>
      /// <param name="as_order_type"></param>
      /// <param name="as_order_cond"></param>
      /// <param name="as_level_list">陣列轉字串,直接傳入字串,用逗號分開</param>
      /// <param name="as_null"></param>
      /// <returns></returns>
      public DataTable d_30681_d_mtf_new(DateTime as_fm_date,
                                      DateTime as_to_date,
                                      string as_sc_code,
                                      string as_kind_id1,
                                      string as_kind_id2,
                                      int as_mth_seq1,
                                      int as_mth_seq2,
                                      string as_order_type,
                                      string as_order_cond,
                                      string as_level_list,
                                      string as_null) {

         object[] parms = {
                ":as_fm_date", as_fm_date,
                ":as_to_date", as_to_date,
                ":as_sc_code", as_sc_code,
                ":as_kind_id1", as_kind_id1+"%",
                ":as_kind_id2", as_kind_id2+"%",
                ":as_mth_seq1", as_mth_seq1,
                ":as_mth_seq2", as_mth_seq2,
                ":as_order_type", as_order_type,
                ":as_order_cond", as_order_cond,
                ":as_null", as_null
            };

         //ken,如果陣列字串為空,sql會出錯,所以有值才輸出where條件
         string temp = string.IsNullOrEmpty(as_level_list) ? "" : "ftpriced_level in (" + as_level_list + ") or ";

         string sql = string.Format(@"
select to_char(ftpricemtf_trade_date,'mm/dd/yy hh24:mi:ss') as ""交易日期"",
   ftpricemtf_market_code as ""交易時段: 0一般 / 1夜盤"",
   ftpricemtf_prod_type as ""F期貨 / O選擇權"",
   ftpricemtf_sc_code as ""S單式 / C複式"",
   ftpricemtf_prod_id as ""序列"",
          
   ftpricemtf_kind_id1 as ""第一支腳契約代碼"",
   ftpricemtf_mth_seq1 as ""第一支腳月份序號"",
   ftpricemtf_m_price1 as ""成交價格1"",
   ftpricemtf_kind_id2 as ""第二支腳契約代碼"",
   ftpricemtf_mth_seq2 as ""第二支腳月份序號"",
          
   ftpricemtf_m_price2 as ""成交價格2"",
   ftpricemtf_bs_code as ""買賣別"",
   ftpricemtf_osf_seq_no as ""委託序號"",
   to_char(ftpricemtf_osf_orig_time,'mm/dd/yy hh24:mi:ss') as ""委託時間"",
   ftpricemtf_oq_code as ""委託單類別(Order / Quote)"",
          
   ftpricemtf_osf_order_type as ""委託方式"",
   ftpricemtf_osf_order_cond as ""委託條件"",
   ftpricemtf_osf_order_price as ""委託價"",
   ftpricemtf_osf_order_qnty as ""委託口數"",
   ftpricemtf_m_inst as ""搓合標記"",
          
   ftpricemtf_seq_no as ""撮合序號"",
   to_char(ftpricemtf_orig_time,'mm/dd/yy hh24:mi:ss') as ""成交時間"",
   ftpricemtf_price as ""成交價"",
   ftpricemtf_qnty as ""成交口數"",
   ftpricemtf_m_cm_code as ""價差對價差成交flag"",
          
   ftpricemtf_fcm_no as ""期貨商代號"",
   ftpricemtf_order_no as ""委託書 / 報價單編號"",
   ftpricemtf_acc_no as ""投資人帳號"",
   ftpricemtf_tprice as ""基準價格"",
   ftpricemtf_level as ""級距""
from ci.ftpricemtf,
   (select ftpriced_trade_date,   
            ftpriced_market_code,   
            ftpriced_prod_type,   
            ftpriced_prod_id,   
            ftpriced_m_inst
      from ci.ftpriced  
      where ftpriced_trade_date >= :as_fm_date
         and ftpriced_trade_date <= :as_to_date
         and ftpriced_prod_type = 'F'
         and ftpriced_sc_code like :as_sc_code
         and ftpriced_kind_id1 like :as_kind_id1
         and ftpriced_kind_id2 like :as_kind_id2
         and (:as_mth_seq1 = 99 or ftpriced_mth_seq1 = :as_mth_seq1)
         and (:as_mth_seq2 = 99 or ftpriced_mth_seq2 = :as_mth_seq2)
         and ftpriced_order_type like :as_order_type
         and ftpriced_order_cond like :as_order_cond
         and ( {0} (:as_null = 'Y' and  ftpriced_level is null))
   group by ftpriced_trade_date,   
            ftpriced_market_code,   
            ftpriced_prod_type,   
            ftpriced_prod_id,   
                 ftpriced_m_inst)  
   where ftpricemtf_trade_date = ftpriced_trade_date
     and ftpricemtf_market_code = ftpriced_market_code
     and ftpricemtf_prod_type = ftpriced_prod_type
     and ftpricemtf_m_inst = ftpriced_m_inst
order by ftpricemtf_trade_date , ftpricemtf_market_code , ftpricemtf_prod_type , ftpricemtf_orig_time , ftpricemtf_seq_no  
", temp);

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// get tpricemtf+tpriced data, return 30 fields
      /// </summary>
      /// <param name="as_fm_date"></param>
      /// <param name="as_to_date"></param>
      /// <param name="as_sc_code"></param>
      /// <param name="as_kind_id1"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_mth_seq1"></param>
      /// <param name="as_mth_seq2"></param>
      /// <param name="as_order_type"></param>
      /// <param name="as_order_cond"></param>
      /// <param name="as_level_list">陣列轉字串,直接傳入字串,用逗號分開</param>
      /// <param name="as_null"></param>
      /// <returns></returns>
      public DataTable d_30681_d_mtf(DateTime as_fm_date,
                                      DateTime as_to_date,
                                      string as_sc_code,
                                      string as_kind_id1,
                                      string as_kind_id2,
                                      int as_mth_seq1,
                                      int as_mth_seq2,
                                      string as_order_type,
                                      string as_order_cond,
                                      string as_level_list,
                                      string as_null) {

         object[] parms = {
                ":as_fm_date", as_fm_date,
                ":as_to_date", as_to_date,
                ":as_sc_code", as_sc_code,
                ":as_kind_id1", as_kind_id1+"%",
                ":as_kind_id2", as_kind_id2+"%",
                ":as_mth_seq1", as_mth_seq1,
                ":as_mth_seq2", as_mth_seq2,
                ":as_order_type", as_order_type,
                ":as_order_cond", as_order_cond,
                ":as_null", as_null
            };
         
         //ken,如果陣列字串為空,sql會出錯,所以有值才輸出where條件
         string temp = string.IsNullOrEmpty(as_level_list) ? "" : "tpriced_level in (" + as_level_list + ") or ";

         string sql = string.Format(@"
select to_char(tpricemtf_trade_date,'mm/dd/yy hh24:mi:ss') as ""交易日期"",
   tpricemtf_market_code as ""交易時段: 0一般 / 1夜盤"",
   tpricemtf_prod_type as ""F期貨 / O選擇權"",
   tpricemtf_sc_code as ""S單式 / C複式"",
   tpricemtf_prod_id as ""序列"",
          
   tpricemtf_kind_id1 as ""第一支腳契約代碼"",
   tpricemtf_mth_seq1 as ""第一支腳月份序號"",
   tpricemtf_m_price1 as ""成交價格1"",
   tpricemtf_kind_id2 as ""第二支腳契約代碼"",
   tpricemtf_mth_seq2 as ""第二支腳月份序號"",
          
   tpricemtf_m_price2 as ""成交價格2"",
   tpricemtf_bs_code as ""買賣別"",
   tpricemtf_osf_seq_no as ""委託序號"",
   to_char(tpricemtf_osf_orig_time,'mm/dd/yy hh24:mi:ss') as ""委託時間"",
   tpricemtf_oq_code as ""委託單類別(Order / Quote)"",
          
   tpricemtf_osf_order_type as ""委託方式"",
   tpricemtf_osf_order_cond as ""委託條件"",
   tpricemtf_osf_order_price as ""委託價"",
   tpricemtf_osf_order_qnty as ""委託口數"",
   tpricemtf_m_inst as ""搓合標記"",
          
   tpricemtf_seq_no as ""撮合序號"",
   to_char(tpricemtf_orig_time,'mm/dd/yy hh24:mi:ss') as ""成交時間"",
   tpricemtf_price as ""成交價"",
   tpricemtf_qnty as ""成交口數"",
   tpricemtf_m_cm_code as ""價差對價差成交flag"",
          
   tpricemtf_fcm_no as ""期貨商代號"",
   tpricemtf_order_no as ""委託書 / 報價單編號"",
   tpricemtf_acc_no as ""投資人帳號"",
   tpricemtf_tprice as ""基準價格"",
   tpricemtf_level as ""級距""
from ci.tpricemtf,
   (select tpriced_trade_date,   
            tpriced_market_code,   
            tpriced_prod_type,   
            tpriced_prod_id,   
            tpriced_m_inst
      from ci.tpriced  
      where tpriced_trade_date >= :as_fm_date
         and tpriced_trade_date <= :as_to_date
         and tpriced_prod_type = 'F'
         and tpriced_sc_code like :as_sc_code
         and tpriced_kind_id1 like :as_kind_id1
         and tpriced_kind_id2 like :as_kind_id2
         and (:as_mth_seq1 = 99 or tpriced_mth_seq1 = :as_mth_seq1)
         and (:as_mth_seq2 = 99 or tpriced_mth_seq2 = :as_mth_seq2)
         and tpriced_order_type like :as_order_type
         and tpriced_order_cond like :as_order_cond
         and ( {0} (:as_null = 'Y' and  tpriced_level is null))
   group by tpriced_trade_date,   
            tpriced_market_code,   
            tpriced_prod_type,   
            tpriced_prod_id,   
            tpriced_m_inst)  
   where tpricemtf_trade_date = tpriced_trade_date
     and tpricemtf_market_code = tpriced_market_code
     and tpricemtf_prod_type = tpriced_prod_type
     and tpricemtf_m_inst = tpriced_m_inst
order by tpricemtf_trade_date , tpricemtf_market_code , tpricemtf_prod_type , tpricemtf_orig_time , tpricemtf_seq_no     
", temp);

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


   }


}
