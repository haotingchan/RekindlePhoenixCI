using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

/// <summary>
/// ken,2019/4/29
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// SPAN參數計算原始資料下載
   /// </summary>
   public class D40210 : DataGate {

      /// <summary>
      /// 3.7 STC VSR計算
      /// 3.8 ETC VSR計算
      /// return spnv1_sid/spnv1_150_std/spnv1_180_std/spnv2_150_rate/spnv2_180_rate/spnv2_cp_day_rate/rpt_col_num
      /// </summary>
      /// <param name="as_ymd">yyyyMMdd</param>
      /// <param name="as_key">ETC/STC</param>
      /// <returns></returns>
      public DataTable d_40210_1(string as_ymd, string as_key = "STC") {

         object[] parms = {
            ":as_ymd", as_ymd,
            ":as_key", as_key.PadRight(7)
         };

         string sql = @"
select spnv1_sid,
    --欄位(row 5,6)
    spnv1_150_std,
    spnv1_180_std,
    --欄位(row 7,8,9)
    spnv2_150_rate,
    spnv2_180_rate,
    spnv2_cp_day_rate,
    --col位置
    rpt_col_num
from ci.spnv1,
    ci.spnv2,
    (select max(substr(pdk_kind_id,1,2))||'O' as data_kind_id,pdk_stock_id as data_sid,
        row_number() over(order by max(substr(pdk_kind_id,1,2))) as rpt_col_num
    from ci.hpdk 
    where pdk_date = to_date(:as_ymd,'YYYYMMDD')
    and pdk_prod_type = 'O' and pdk_param_key = :as_key
    and pdk_status_code = 'N'
    group by pdk_stock_id) p
where spnv1_ymd = spnv2_ymd
and spnv1_sid = spnv2_sid
and data_sid = spnv1_sid(+)
and data_sid = spnv2_sid(+)
and spnv1_ymd = :as_ymd
";


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 3.7 STC VSR計算
      /// 3.8 ETC VSR計算
      /// return spnv1_sid/spnv1_150_std/spnv1_180_std/spnv2_150_rate/spnv2_180_rate/spnv2_cp_day_rate/rpt_col_num
      /// </summary>
      /// <param name="as_ymd">yyyyMMdd</param>
      /// <param name="as_key">ETC/STC</param>
      /// <returns></returns>
      public DataTable d_40210_1_detail(string as_symd, string as_eymd, string as_key = "STC") {

         object[] parms = {
            ":as_symd", as_symd,
            ":as_eymd", as_eymd,
            ":as_key", as_key.PadRight(7)
         };

         string sql = @"
select rpt_row_num,
   rpt_col_num,
   data_ymd,
   data_kind_id,
   data_sid,
   nvl(spnv1_return_rate,tfxm1_return_rate) as return_rate,
   data_pdk_name,
   data_pid_name
from ci.tfxm1,
     ci.spnv1,
     (select ocf_ymd as data_ymd,
            dense_rank()  over( order by ocf_ymd desc )  as rpt_row_num,
            data_kind_id,pdk_stock_id as data_sid,rpt_col_num,
            data_pdk_name,data_pid_name
      from ci.aocf,
            (select max(substr(pdk_kind_id,1,2))||'O' as data_kind_id,pdk_stock_id,
                  row_number() over(order by max(substr(pdk_kind_id,1,2))) as rpt_col_num,
                  max(pdk_name) as data_pdk_name,max(pid_name) as data_pid_name
               from ci.hpdk ,
                  --上市/上櫃中文名稱
                  (select trim(cod_id) as cod_id,trim(cod_desc) as pid_name from ci.cod where cod_txn_id = 'TFXM')   
            where pdk_date = to_date(:as_eymd,'YYYYMMDD')
               and pdk_prod_type = 'O' and pdk_param_key = :as_key
               and pdk_status_code = 'N'
               and pdk_underlying_market = cod_id 
            group by pdk_stock_id) p
      where ocf_ymd <= :as_eymd
         and ocf_ymd >= :as_symd
      ) r
where to_date(data_ymd,'YYYYMMDD') = tfxm1_date(+)
and data_sid = tfxm1_sid(+)
and data_ymd = spnv1_ymd(+)
and data_sid = spnv1_sid(+)
order by rpt_row_num , rpt_col_num , data_ymd desc
";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.1 現貨data(廢棄)
      public DataTable d_40210_3(string as_symd, string as_eymd, List<string> oswGrps) {

         object[] parms = {
            ":as_symd", as_symd,
            ":as_eymd", as_eymd
         };

         string as_code = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select * from (
    select substr(ocf_ymd,1,4)||'/'||substr(ocf_ymd,5,2)||'/'||substr(ocf_ymd,7,2) as ocf_ymd,
        --ocf_rownum,
        mgt2_kind_id,
        mgr1_close_price
        --rpt_seq_no as col_num
    from
        --日期和商品基本資料,其中OCF_ROWNUM為日期列數
        (select ocf_ymd,
            dense_rank()  over( order by ocf_ymd desc )  as ocf_rownum,
            mgt2_kind_id
            from ci.aocf,ci.mgt2,ci.apdk
            where ocf_ymd >= :as_symd
            and ocf_ymd <= :as_eymd
            and mgt2_prod_type = 'O' and mgt2_prod_subtype <> 'S'
            and not nvl(mgt2_data_type ,' ') in ('E','N')
            and mgt2_kind_id = apdk_kind_id
            and apdk_market_close in ({0}) ),
        --收盤價
        (select mgp1_ymd,mgp1_m_kind_id,mgp1_close_price as mgr1_close_price
            from ci.mgp1_sma,ci.apdk
            where mgp1_ymd >= :as_symd
            and mgp1_ymd <= :as_eymd
            and mgp1_m_kind_id = apdk_kind_id 
            and apdk_prod_type = 'O' and apdk_prod_subtype  <> 'S') ,
        ci.rpt
    where ocf_ymd = mgp1_ymd(+)
    and mgt2_kind_id = mgp1_m_kind_id(+)
    and mgt2_kind_id = rpt_value
    and rpt.rpt_txn_id = '40210' 
    and rpt.rpt_txd_id = '40210_OPT'
    order by ocf_ymd desc, ocf_rownum ,rpt_seq_no
)
pivot(sum(mgr1_close_price) for (mgt2_kind_id) in (
                                                    'TXO    ' as TXO,
                                                    'TEO    ' as TEO,
                                                    'TFO    ' as TFO,
                                                    'XIO    ' as XIO,
                                                    'GTO    ' as GTO)
)
order by ocf_ymd desc
", as_code);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.1 現貨data(廢棄)
      public DataTable d_40210_3_old(string as_symd, string as_eymd, List<string> oswGrps) {

         object[] parms = {
            ":as_symd", as_symd,
            ":as_eymd", as_eymd
         };

         string as_code = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select ocf_ymd,
    ocf_rownum,
    mgt2_kind_id,
    mgr1_close_price,
    rpt_seq_no as col_num
from
    --日期和商品基本資料,其中OCF_ROWNUM為日期列數
    (select ocf_ymd,
        dense_rank()  over( order by ocf_ymd desc )  as ocf_rownum,
        mgt2_kind_id
        from ci.aocf,ci.mgt2,ci.apdk
        where ocf_ymd >= :as_symd
        and ocf_ymd <= :as_eymd
        and mgt2_prod_type = 'O' and mgt2_prod_subtype <> 'S'
        and not nvl(mgt2_data_type ,' ') in ('E','N')
        and mgt2_kind_id = apdk_kind_id
        and apdk_market_close in ({0}) ),
    --收盤價
    (select mgp1_ymd,mgp1_m_kind_id,mgp1_close_price as mgr1_close_price
        from ci.mgp1_sma,ci.apdk
        where mgp1_ymd >= :as_symd
        and mgp1_ymd <= :as_eymd
        and mgp1_m_kind_id = apdk_kind_id 
        and apdk_prod_type = 'O' and apdk_prod_subtype  <> 'S') ,
    ci.rpt
where ocf_ymd = mgp1_ymd(+)
and mgt2_kind_id = mgp1_m_kind_id(+)
and mgt2_kind_id = rpt_value
and rpt.rpt_txn_id = '40210' 
and rpt.rpt_txd_id = '40210_OPT'
order by ocf_ymd desc, ocf_rownum , col_num   
", as_code);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.2 指數選擇權VSR
      public DataTable d_40210_4(string as_ymd, List<string> oswGrps) {

         object[] parms = {
            ":as_ymd", as_ymd
         };

         string as_code = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select spnv1_sid,
    --欄位(row 5,6)
    spnv1_150_std,
    spnv1_180_std,
    --欄位(row 7,8,9)
    spnv2_150_rate,
    spnv2_180_rate,
    spnv2_cp_day_rate,
    rpt_seq_no as rpt_col_num
from ci.spnv1,
    ci.spnv2,
    (select mgt2_kind_id as data_sid
        from ci.mgt2,ci.apdk
        where mgt2_prod_type = 'O' and mgt2_prod_subtype <> 'S'
        and not nvl(mgt2_data_type ,' ') in ('E','N')
        and mgt2_kind_id = apdk_kind_id
        and apdk_market_close in({0})
        ) r,
    (select * from ci.rpt
        where rpt_txn_id = '40210' 
        and rpt_txd_id = '40210_OPT')
where spnv1_ymd = spnv2_ymd
and spnv1_sid = spnv2_sid
and data_sid = spnv1_sid(+)
and data_sid = spnv2_sid(+)
and data_sid = rpt_value
and spnv1_ymd = :as_ymd
", as_code);

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.2 指數選擇權VSR detail
      public DataTable d_40210_4_detail(string as_symd, string as_eymd, List<string> oswGrps) {

         object[] parms = {
            ":as_symd", as_symd,
            ":as_eymd", as_eymd
         };

         string as_code = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select rpt_row_num,
    rpt_seq_no as rpt_col_num,
    data_ymd,
    data_sid,
    nvl(spnv1_return_rate,tfxm1_return_rate) as return_rate
from ci.tfxm1,
    ci.spnv1,
    (select ocf_ymd as data_ymd,mgt2_kind_id as data_sid,
        dense_rank() over( order by ocf_ymd desc ) as rpt_row_num
        from ci.aocf,ci.mgt2,ci.apdk
        where ocf_ymd <= :as_eymd
        and ocf_ymd >= :as_symd
        and mgt2_prod_type = 'O' and mgt2_prod_subtype <> 'S'
        and not nvl(mgt2_data_type ,' ') in ('E','N')
        and mgt2_kind_id = apdk_kind_id
        and apdk_market_close in ({0} )
        ) r,
    (select * from ci.rpt
        where rpt_txn_id = '40210' 
        and rpt_txd_id = '40210_OPT')
where to_date(data_ymd,'YYYYMMDD') = tfxm1_date(+)
and data_sid = tfxm1_sid(+)
and data_ymd = spnv1_ymd(+)
and data_sid = spnv1_sid(+)
and data_sid = rpt_value 
order by rpt_row_num , rpt_col_num , data_ymd 
", as_code);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.3 期貨data
      public DataTable d_40210_5(string as_symd, string as_eymd, List<string> oswGrps) {

         object[] parms = {
            ":as_symd", as_symd,
            ":as_eymd", as_eymd
         };

         string as_code = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select ocf_ymd,
    ocf_rownum,
    mgt2_kind_id,
    mgr1_close_price,
    mgr1_open_ref,
    mgr1_close_price - mgr1_open_ref as up_down,
    rpt_seq_no as col_1,
    rpt_level_1 as col_2,
    rpt_level_2 as col_3
from
    --日期和商品基本資料,其中OCF_ROWNUM為日期列數
    (select ocf_ymd,
          dense_rank()  over( order by ocf_ymd desc )  as ocf_rownum,
          mgt2_kind_id
     from ci.aocf,ci.mgt2,ci.apdk
     where ocf_ymd >= :as_symd
      and ocf_ymd <=:as_eymd
      and mgt2_prod_type = 'F' and mgt2_prod_subtype <> 'S'
      and not nvl(mgt2_data_type ,' ') in ('E','N')
      and mgt2_kind_id = apdk_kind_id
      and apdk_market_close in({0})),
    --收盤價,開盤參考價,漲跌
    (select mgp1_ymd,mgp1_m_kind_id,
          mgp1_settle_price as mgr1_close_price,
          mgp1_open_ref as mgr1_open_ref
     from ci.mgp1_sma,ci.apdk
     where mgp1_ymd >= :as_symd
      and mgp1_ymd <= :as_eymd
      and mgp1_m_kind_id = apdk_kind_id 
      and apdk_prod_type = 'F' and apdk_prod_subtype <> 'S')   ,
    (select * from ci.rpt 
        where rpt_txn_id = '40210' 
        and rpt_txd_id = '40210_FUT')     
where ocf_ymd = mgp1_ymd(+)
and mgt2_kind_id = mgp1_m_kind_id(+)
and mgt2_kind_id= rpt_value
", as_code);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.4 期貨契約PSR
      public DataTable d_40210_6(string as_ymd, List<string> oswGrps) {

         object[] parms = {
            ":as_ymd", as_ymd
         };

         string as_osw_grp = string.Join(",", oswGrps.ToArray()).Replace("\"" , "");        

         string sql = string.Format(@"
select mgr4_ymd,
    mgr4_kind_id,
    mgr4_cm,
    rpt_seq_no as rpt_col_num
from ci.mgr4,
    ci.rpt
where mgr4_kind_id = rpt_value  
and mgr4_prod_type = 'F'
and rpt_txn_id = '40210'     
and rpt_txd_id = '40210_PSR' 
and mgr4_ymd = :as_ymd
and mgr4_osw_grp in({0})
", as_osw_grp);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.5 Delta折耗比率
      public DataTable d_40210_7(string as_ymd,  List<string> oswGrps) {

         object[] parms = {
            ":as_ymd", as_ymd
         };

         string as_code = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select sp1_kind_id1,
    sp1_kind_id2,
    sp1_rate,
    rpt_level_2 as rpt_col_num,
    rpt_level_3 as rpt_row_num
from ci.sp1,
    (select * from ci.rpt
        where rpt_txn_id = '40210'
        and rpt_txd_id = '40210_D')
where sp1_kind_id1 = rpt_value
and sp1_kind_id2 = rpt_value_2
and sp1_type = 'SD'
and sp1_date = to_date(:as_ymd,'YYYYMMDD')
and sp1_osw_grp in ({0})
", as_code);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.5 Delta折耗比率 detail
      public DataTable d_40210_7_detail(string as_symd, string as_eymd, List<string> oswGrps) {

         object[] parms = {
            ":as_symd", as_symd,
            ":as_eymd", as_eymd
         };

         string as_osw_grp = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select spnd_ymd,
    spnd_kind_id1,
    spnd_kind_id2,
    spnd_t_val,
    rpt_level_1 as rpt_col_num
from ci.spnd,
    (select * from ci.rpt
        where rpt_txn_id = '40210'
        and rpt_txd_id = '40210_D')
where spnd_kind_id1 = rpt_value
and spnd_kind_id2 = rpt_value_2
and spnd_ymd >= :as_symd
and spnd_ymd <= :as_eymd
and spnd_osw_grp in ({0}) 
order by spnd_ymd desc, rpt_col_num
", as_osw_grp);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


      //3.6 跨商品折抵比率
      public DataTable d_40210_8(string as_ymd, List<string> oswGrps) {

         object[] parms = {
            ":as_ymd", as_ymd
         };

         string as_code = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select spns1_kind_id1,
    spns1_kind_id2,
    spns1_150_avg_val,
    spns1_150_std,
    spns2_150_rate,
    
    spns1_180_avg_val,
    spns1_180_std,
    spns2_180_rate,
    spns2_max_rate,
    spns2_cp_day_rate,
    
    spns2_day_rate,
    rpt_level_1 as rpt_col_num,
    rpt_level_2 as col,
    rpt_level_3 as row_1,
    rpt_level_4 as row_2
from ci.spns1,
    ci.spns2,
    (select * from ci.rpt
        where rpt_txn_id = '40210'
        and rpt_txd_id = '40210_P')
 where spns1_ymd = :as_ymd
   and spns1_ymd = spns2_ymd
   and spns1_kind_id1 = spns2_kind_id1
   and spns1_kind_id2 = spns2_kind_id2
   and spns2_osw_grp in ({0})
   and spns1_kind_id1 = rpt_value
   and spns1_kind_id2 = rpt_value_2
order by rpt_col_num ,  col , row_1 , row_2
", as_code);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      //3.6 跨商品折抵比率 detail
      public DataTable d_40210_8_detail(string as_ymd,  List<string> oswGrps) {

         object[] parms = {
            ":as_ymd", as_ymd
         };

         string as_osw_grp = string.Join(",", oswGrps.ToArray());

         string sql = string.Format(@"
select spns1d_detial_ymd,
    spns1d_kind_id1,
    spns1d_kind_id2,
    spns1d_t_val,
    rpt_level_1 as rpt_col_num 
from ci.spns1d,
    (select * from ci.rpt
        where rpt_txn_id = '40210'
        and rpt_txd_id = '40210_D')
where spns1d_kind_id1 = rpt_value
and spns1d_kind_id2 = rpt_value_2
and spns1d_ymd = :as_ymd
and spns1d_osw_grp in ({0})
order by spns1d_detial_ymd desc, rpt_col_num
", as_osw_grp);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }































      // 專案的namespace
      //private static readonly string AssemblyName = "DataObjects";//其實就是最後compile出來的dll名稱

      /// <summary>
      /// 針對不同的grid data source,合併相同的輸入與輸出
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public IGridData40210 CreateGridData(Type type, string name) {

         //string className = string.Format("{0}.Dao.Together.SpecificDao.{1}",AssemblyName, name);//完整的class路徑

         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + name;//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (IGridData40210)Assembly.Load(AssemblyName).CreateInstance(className);
      }

   }

   public class Q40210 {
      public DateTime ad_date { get; set; }
      public string as_prod_subtype { get; set; }
      public string as_kind_id { get; set; }
      /// <summary>
      /// 只為了排序,value=DATE/KIND
      /// </summary>
      public string as_sort_type { get; set; }

      public Q40210(DateTime ad_date, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE") {
         this.ad_date = ad_date;
         this.as_prod_subtype = as_prod_subtype;
         this.as_kind_id = as_kind_id;
         this.as_sort_type = as_sort_type;
      }

      /// <summary>
      /// convert all Properties to object[]
      /// </summary>
      /// <returns></returns>
      public object[] ToParam() {
         object[] aryParam = new object[GetType().GetProperties().Length * 2];
         int pos = 0;

         foreach (var prop in GetType().GetProperties()) {
            aryParam[pos++] = ":" + prop.Name;//":"其實可不用
            aryParam[pos++] = prop.GetValue(this);
         }

         return aryParam;
      }
   }

   public interface IGridData40210 {
      //第一次改寫(廢除)
      DataTable ListAll(DateTime ad_date, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE");

      DataTable ListAll(Q40210 query);
   }

   /// <summary>
   /// 重點資料
   /// </summary>
   public class D40210KeyInfo : DataGate, IGridData40210 {

      /// <summary>
      /// 第一次改寫(廢除) 重點資料, return 8 fields
      /// </summary>
      /// <param name="ad_date"></param>
      /// <param name="as_prod_subtype"></param>
      /// <param name="as_kind_id"></param>
      /// <param name="as_sort_type">只為了排序,value=DATE/KIND</param>
      /// <returns></returns>
      public DataTable ListAll(DateTime ad_date, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE") {

         object[] parms = {
                ":ad_date", ad_date,
                ":as_prod_subtype", as_prod_subtype,
                ":as_kind_id", as_kind_id
            };

         string sort = (as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

         string sql = string.Format(@"
select cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    --cpr_approval_number,
    --cpr_remark,
    --to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,
    --trim(cpr_w_user_id) as cpr_w_user_id,
    
    nvl(mgt2_seq_no,999) as seq_no,
    mgt2_prod_type as prod_type
  from ci.hcpr,
       ci.mgt2,
        (select cpr_kind_id as max_kind_id,max(cpr_effective_date) as max_effective_date
         from ci.hcpr
         where cpr_effective_date <= :ad_date
         group by cpr_kind_id) test,
       (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
        from ci.cod where cod_txn_id = '49020') cod
 where cpr_kind_id = mgt2_kind_id(+)
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_prod_subtype=COD.COD_ID
   and (nvl(mgt2_data_type,' ') = ' '  or mgt2_kind_id in ('ETF','ETC'))
   and cpr_prod_subtype like :as_prod_subtype
   and cpr_kind_id like :as_kind_id
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date
", sort);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 重點資料, return 8 fields
      /// </summary>
      /// <param name="query">ref Q40210</param>
      /// <returns></returns>
      public DataTable ListAll(Q40210 query) {

         object[] parms = query.ToParam();

         string sort = (query.as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

         string sql = string.Format(@"
select cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    --cpr_approval_number,
    --cpr_remark,
    --to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,
    --trim(cpr_w_user_id) as cpr_w_user_id,
    
    nvl(mgt2_seq_no,999) as seq_no,
    mgt2_prod_type as prod_type
  from ci.hcpr,
       ci.mgt2,
        (select cpr_kind_id as max_kind_id,max(cpr_effective_date) as max_effective_date
         from ci.hcpr
         where cpr_effective_date <= :ad_date
         group by cpr_kind_id) test,
       (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
        from ci.cod where cod_txn_id = '49020') cod
 where cpr_kind_id = mgt2_kind_id(+)
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_prod_subtype=COD.COD_ID
   and (nvl(mgt2_data_type,' ') = ' '  or mgt2_kind_id in ('ETF','ETC'))
   and cpr_prod_subtype like :as_prod_subtype
   and cpr_kind_id like :as_kind_id
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date
", sort);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }

   /// <summary>
   /// 明細資料
   /// </summary>
   public class D40210Detail : DataGate, IGridData40210 {

      /// <summary>
      /// 第一次改寫(廢除) 明細資料, return 12 fields
      /// </summary>
      /// <param name="ad_date"></param>
      /// <param name="as_prod_subtype"></param>
      /// <param name="as_kind_id"></param>
      /// <param name="as_sort_type">只為了排序,value=DATE/KIND</param>
      /// <returns></returns>
      public DataTable ListAll(DateTime ad_date, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE") {

         object[] parms = {
                ":ad_date", ad_date,
                ":as_prod_subtype", as_prod_subtype,
                ":as_kind_id", as_kind_id
            };

         string sort = (as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

         string sql = string.Format(@"
select cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    cpr_approval_number,
    cpr_remark,
    to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,
    trim(cpr_w_user_id) as cpr_w_user_id,
    
    nvl(mgt2_seq_no,999) as seq_no,
    mgt2_prod_type as prod_type
  from ci.hcpr,
       ci.mgt2,
        (select cpr_kind_id as max_kind_id,max(cpr_effective_date) as max_effective_date
         from ci.hcpr
         where cpr_effective_date <= :ad_date
         group by cpr_kind_id) test,
       (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
        from ci.cod where cod_txn_id = '49020') cod
 where cpr_kind_id = mgt2_kind_id(+)
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_prod_subtype=COD.COD_ID
   and (nvl(mgt2_data_type,' ') = ' '  or mgt2_kind_id in ('ETF','ETC'))
   and cpr_prod_subtype like :as_prod_subtype
   and cpr_kind_id like :as_kind_id
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date
", sort);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 明細資料, return 12 fields
      /// </summary>
      /// <param name="query">ref Q40210</param>
      /// <returns></returns>
      public DataTable ListAll(Q40210 query) {

         object[] parms = query.ToParam();

         string sort = (query.as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

         string sql = string.Format(@"
select cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    cpr_approval_number,
    cpr_remark,
    to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,
    trim(cpr_w_user_id) as cpr_w_user_id,
    
    nvl(mgt2_seq_no,999) as seq_no,
    mgt2_prod_type as prod_type
  from ci.hcpr,
       ci.mgt2,
        (select cpr_kind_id as max_kind_id,max(cpr_effective_date) as max_effective_date
         from ci.hcpr
         where cpr_effective_date <= :ad_date
         group by cpr_kind_id) test,
       (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
        from ci.cod where cod_txn_id = '49020') cod
 where cpr_kind_id = mgt2_kind_id(+)
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_prod_subtype=COD.COD_ID
   and (nvl(mgt2_data_type,' ') = ' '  or mgt2_kind_id in ('ETF','ETC'))
   and cpr_prod_subtype like :as_prod_subtype
   and cpr_kind_id like :as_kind_id
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date
", sort);


         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
