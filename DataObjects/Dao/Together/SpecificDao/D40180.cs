using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/5/8
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 交易系統使用文字檔
   /// </summary>
   public class D40180 : DataGate {

      /// <summary>
      /// get ci.mgd2 data return 10 fields (d40180_7122)
      /// </summary>
      /// <param name="isDate">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable GetAllTxtData(string isDate) {

         object[] parms = {
                ":isDate", isDate
            };


         string sql = @"
select  
   mgd2_issue_begin_ymd,
   mgd2_prod_type,
   mgd2_kind_id,
   sum(case when mgd2_ab_type ='A' or mgd2_ab_type ='-' then  mgd2_cm else 0 end) as mgd2_cm_1,
   sum(case when mgd2_ab_type ='B' then  mgd2_cm else 0  end) as  mgd2_cm_2,
   sum(case when mgd2_ab_type ='A' or mgd2_ab_type ='-' then  mgd2_im else 0 end) as mgd2_im_1,
   sum(case when mgd2_ab_type ='B' then  mgd2_im else 0  end) as  mgd2_im_2,
   sum(case when mgd2_ab_type ='A' or mgd2_ab_type ='-' then  mgd2_mm else 0 end) as mgd2_mm_1,
   sum(case when mgd2_ab_type ='B' then  mgd2_mm else 0  end) as mgd2_mm_2,                                
   nvl(mgd2_issue_end_ymd,'')as mgd2_issue_end_ymd
from(
   select  
      mgd2_issue_begin_ymd,
      mgd2_prod_type,
      case when mgd2_param_key like 'ST%' then mgd2_cm  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_cm *100 else mgd2_cm end as mgd2_cm,
      case when mgd2_param_key like 'ST%' then mgd2_im  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_im *100 else mgd2_im end as mgd2_im,
      case when mgd2_param_key like 'ST%' then mgd2_mm  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_mm *100 else mgd2_mm end as mgd2_mm,
      case when mgd2_param_key like 'ST%' then substr(mgd2_kind_id,1,2) else substr(mgd2_kind_id,1,3) end as  mgd2_kind_id,
      mgd2_issue_end_ymd,
      mgd2_ab_type,
      mgd2_prod_subtype 
   from ci.mgd2
   where mgd2_issue_begin_ymd = :isDate
   union all
   select  
      mgd2_issue_end_ymd,
      mgd2_prod_type,
      case when mgd2_param_key like 'ST%' then mgd2_cur_cm  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_cur_cm *100 else mgd2_cur_cm end as mgd2_cm,
      case when mgd2_param_key like 'ST%' then mgd2_cur_im  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_cur_im *100 else mgd2_cur_im end as mgd2_im,
      case when mgd2_param_key like 'ST%' then mgd2_cur_mm  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_cur_mm *100 else mgd2_cur_mm end as mgd2_mm,
      case when mgd2_param_key like 'ST%' then substr(mgd2_kind_id,1,2) else substr(mgd2_kind_id,1,3) end as  mgd2_kind_id,
      '',
      mgd2_ab_type,
      mgd2_prod_subtype 
   from ci.mgd2
   where mgd2_issue_end_ymd = :isDate                
)a
group by mgd2_issue_begin_ymd,mgd2_prod_type,mgd2_kind_id,mgd2_issue_end_ymd,mgd2_prod_subtype
order by mgd2_issue_end_ymd,mgd2_prod_type,mgd2_prod_subtype,mgd2_kind_id
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.mgd2 data return 10 fields (d40180_7122_rtn)
      /// </summary>
      /// <param name="beginDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable GetAllTxtTmp(string beginDate , string endDate) {

         object[] parms = {
                ":beginDate", beginDate,
                ":endDate", endDate
            };


         string sql = @"
select  
   mgd2_issue_begin_ymd,
   mgd2_prod_type,
   mgd2_kind_id,
   sum(case when mgd2_ab_type ='A' or mgd2_ab_type ='-' then  mgd2_cm else 0 end) as mgd2_cm_1,
   sum(case when mgd2_ab_type ='B' then  mgd2_cm else 0  end) as  mgd2_cm_2,
   sum(case when mgd2_ab_type ='A' or mgd2_ab_type ='-' then  mgd2_im else 0 end) as mgd2_im_1,
   sum(case when mgd2_ab_type ='B' then  mgd2_im else 0  end) as  mgd2_im_2,
   sum(case when mgd2_ab_type ='A' or mgd2_ab_type ='-' then  mgd2_mm else 0 end) as mgd2_mm_1,
   sum(case when mgd2_ab_type ='B' then  mgd2_mm else 0  end) as mgd2_mm_2,                                
   nvl(mgd2_issue_end_ymd,'')as mgd2_issue_end_ymd
from(
   select  
      mgd2_issue_begin_ymd,
      mgd2_prod_type,
      case when mgd2_param_key like 'ST%' then mgd2_cm  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_cm *100 else mgd2_cm end as mgd2_cm,
      case when mgd2_param_key like 'ST%' then mgd2_im  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_im *100 else mgd2_im end as mgd2_im,
      case when mgd2_param_key like 'ST%' then mgd2_mm  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_mm *100 else mgd2_mm end as mgd2_mm,
      case when mgd2_param_key like 'ST%' then substr(mgd2_kind_id,1,2) else substr(mgd2_kind_id,1,3) end as  mgd2_kind_id,
      mgd2_issue_end_ymd,
      mgd2_ab_type,
      mgd2_prod_subtype 
   from ci.mgd2
   where mgd2_issue_begin_ymd = :beginDate
   union all
   select  
      mgd2_issue_end_ymd,
      mgd2_prod_type,
      case when mgd2_param_key like 'ST%' then mgd2_cur_cm  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_cur_cm *100 else mgd2_cur_cm end as mgd2_cm,
      case when mgd2_param_key like 'ST%' then mgd2_cur_im  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_cur_im *100 else mgd2_cur_im end as mgd2_im,
      case when mgd2_param_key like 'ST%' then mgd2_cur_mm  when mgd2_param_key like 'ET%' and mgd2_amt_type = 'P' then mgd2_cur_mm *100 else mgd2_cur_mm end as mgd2_mm,
      case when mgd2_param_key like 'ST%' then substr(mgd2_kind_id,1,2) else substr(mgd2_kind_id,1,3) end as  mgd2_kind_id,
      '',
      mgd2_ab_type,
      mgd2_prod_subtype 
   from ci.mgd2
   where mgd2_issue_end_ymd = :endDate                
)a
group by mgd2_issue_begin_ymd,mgd2_prod_type,mgd2_kind_id,mgd2_issue_end_ymd,mgd2_prod_subtype
order by mgd2_issue_end_ymd,mgd2_prod_type,mgd2_prod_subtype,mgd2_kind_id
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.mgd2 ,ci.mgt2 ,ci.apdk data return 18 fields (d40180_a0001)
      /// 期貨/選擇權A0001公告文字檔
      /// </summary>
      /// <param name="txtDate">yyyyMMdd</param>
      /// <param name="prodType">F=期貨 / O=選擇權</param>
      /// <param name="as_osw_grp">%%</param>
      /// <param name="as_adj_type"></param>
      /// <param name="as_adj_type_rtn"></param>
      /// <returns></returns>
      public DataTable GetA0001TextDate(string txtDate , string prodType , string as_osw_grp , List<string> as_adj_type , List<string> as_adj_type_rtn) {

         object[] parms = {
                ":txtDate", txtDate,
                ":prodType", prodType,
                ":as_osw_grp", as_osw_grp
            };

         string adjType, selectAdjType;
         if (as_adj_type.Count > 0) {
            adjType = string.Join("," , as_adj_type.ToArray()).Replace("\"" , "");
            selectAdjType = string.Format("  and mgd2_adj_type in ({0})   " , adjType);
         } else {
            selectAdjType = " ";
         }

         string adjTypeRtn, selectAdjTypeRtn;
         if (as_adj_type_rtn.Count > 0) {
            adjTypeRtn = string.Join("," , as_adj_type_rtn.ToArray()).Replace("\"" , "");
            selectAdjTypeRtn = string.Format("  and mgd2_adj_type in ({0})   " , adjTypeRtn);
         } else {
            selectAdjTypeRtn = " ";
         }


         string sql = string.Format(@"
select * 
from(
   select 
   a.* ,
   dense_rank() over(partition by mgd2_kind_id order by num) as row_num
   from(
            select  
               mgd2_ymd,   
               mgd2_kind_id,   
               mgd2_issue_begin_ymd,   
               nvl(mgt2_abbr_name,trim(apdk_name)||'契約('||trim(apdk_kind_id)||')') as mgt2_abbr_name,   
               nvl(mgt2_prod_type,apdk_prod_type) as mgt2_prod_type,   
               nvl(mgt2_kind_id,apdk_kind_id) as mgt2_kind_id,   
               mgd2_ab_type,   
               mgd2_cm,   
               mgd2_mm,   
               mgd2_im,   
               mgd2_prod_type,  
               mgd2_currency_type,   
               mgd2_seq_no,
               mgd2_param_key,
               mgd2_adj_type,
               apdk_remark,
               0 as num
            from    
               ci.mgd2,   
               ci.mgt2,  
               ci.apdk  
            where  mgd2_kind_id = mgt2_kind_id(+)    
            and mgd2_prod_type = apdk_prod_type    
            and mgd2_kind_id = apdk_kind_id    
            and mgd2_issue_begin_ymd = :txtDate    
            and mgd2_prod_type = :prodType 
            and mgd2_osw_grp like :as_osw_grp   
            and mgd2_adj_code = 'Y'
            {0}   
            union all 
            --回調
            select  
               mgd2_ymd,   
               mgd2_kind_id,   
               mgd2_issue_end_ymd,   
               nvl(mgt2_abbr_name,trim(apdk_name)||'契約('||trim(apdk_kind_id)||')') as mgt2_abbr_name,   
               nvl(mgt2_prod_type,apdk_prod_type) as mgt2_prod_type,   
               nvl(mgt2_kind_id,apdk_kind_id) as mgt2_kind_id,   
               mgd2_ab_type,   
               mgd2_cur_cm,   
               mgd2_cur_mm,   
               mgd2_cur_im,   
               mgd2_prod_type,  
               mgd2_currency_type,   
               mgd2_seq_no,
               mgd2_param_key,
               mgd2_adj_type,
               apdk_remark,
               1   
            from    
               ci.mgd2,   
               ci.mgt2,  
               ci.apdk  
            where mgd2_kind_id = mgt2_kind_id(+)    
            and mgd2_prod_type = apdk_prod_type    
            and mgd2_kind_id = apdk_kind_id    
            and mgd2_issue_end_ymd = :txtDate    
            and mgd2_prod_type = :prodType 
            and mgd2_osw_grp like :as_osw_grp   
            and mgd2_adj_code = 'Y'
            {1}
      )a
   )
where row_num = 1
order by decode(mgd2_seq_no,'',0,mgd2_seq_no) , decode(apdk_remark,'M',1,0) , substr(mgd2_kind_id,1,2) ,
decode(substr(mgd2_kind_id,3,1) , mgd2_prod_type ,' ',substr(mgd2_kind_id,3,1)) , mgd2_ab_type
" , selectAdjType , selectAdjTypeRtn);

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.mgd2 ,ci.apdk data return 16 fields (d40180_30004)
      /// 期貨/選擇權30004保證金文字檔
      /// </summary>
      /// <param name="txtDate">yyyyMMdd</param>
      /// <param name="prodType">F=期貨 / O=選擇權</param>
      /// <param name="as_osw_grp">1% / 5% / 7%</param>
      /// <param name="as_adj_type"></param>
      /// <param name="as_adj_type_rtn"></param>
      /// <returns></returns>
      public DataTable Get30004TextDate(string txtDate , string prodType , string as_osw_grp , List<string> as_adj_type , List<string> as_adj_type_rtn) {

         object[] parms = {
                ":txtDate", txtDate,
                ":prodType", prodType,
                ":as_osw_grp", as_osw_grp
            };

         string adjType, selectAdjType;
         if (as_adj_type.Count > 0) {
            adjType = string.Join("," , as_adj_type.ToArray()).Replace("\"" , "");
            selectAdjType = string.Format("  and mgd2_adj_type in ({0})   " , adjType);
         } else {
            selectAdjType = " ";
         }

         string adjTypeRtn, selectAdjTypeRtn;
         if (as_adj_type_rtn.Count > 0) {
            adjTypeRtn = string.Join("," , as_adj_type_rtn.ToArray()).Replace("\"" , "");
            selectAdjTypeRtn = string.Format("  and mgd2_adj_type in ({0})   " , adjTypeRtn);
         } else {
            selectAdjTypeRtn = " ";
         }


         string sql = string.Format(@"
select 
   m.* , 
   (case when mgd2_currency_type = '1' then ceil(mgd2_cm * 2 / 3 / 1000) * 1000 else ceil(mgd2_cm * 2 / 3 / 100) * 100 end) as cp_cm 
from 
   (select 
        a.*,
        dense_rank() over(partition by mgd2_kind_id order by num) as row_num
    from
       (select 
            mgd2_ymd,
            mgd2_kind_id,
            mgd2_issue_begin_ymd,
            mgd2_cm,
            mgd2_mm,
            mgd2_im,
            mgd2_prod_type,
            mgd2_currency_type,
            mgd2_seq_no,
            mgd2_osw_grp,
            '       ' as comb_prod,
            apdk_remark,
            mgd2_adj_type,
            0 as num,
            mgd2_param_key
        from ci.mgd2 ,ci.apdk
        where mgd2_issue_begin_ymd = :txtDate 
        and mgd2_prod_type = :prodType
        and mgd2_ab_type  in ('-','A')
        and mgd2_osw_grp like :as_osw_grp
        and mgd2_adj_code = 'Y'
        {0}
        and mgd2_kind_id = apdk_kind_id
        union all
        --回調
        select 
            mgd2_ymd,
            mgd2_kind_id,
            mgd2_issue_end_ymd,
            mgd2_cur_cm,
            mgd2_cur_mm,
            mgd2_cur_im,
            mgd2_prod_type,
            mgd2_currency_type,
            mgd2_seq_no,
            mgd2_osw_grp,
            '       ' as comb_prod,
            apdk_remark,
            mgd2_adj_type,
            1,
            mgd2_param_key
        from ci.mgd2 ,ci.apdk
        where mgd2_issue_end_ymd = :txtDate 
        and mgd2_prod_type = :prodType
        and mgd2_ab_type  in ('-','A')
        and mgd2_osw_grp like :as_osw_grp
        and mgd2_adj_code = 'Y'
        {1}
        and mgd2_kind_id = apdk_kind_id
        ) a
    ) m 
where row_num = 1
order by decode(mgd2_seq_no,'',0,mgd2_seq_no) , decode(apdk_remark,'M',1,0) , mgd2_kind_id
" , selectAdjType , selectAdjTypeRtn);

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.sp1, ci.sp2, ci.spt1 data return 12 fields (d40180_span)
      /// SPAN VSR文字檔 40180_fut_S1010(VSR)
      /// </summary>
      /// <param name="as_date">yyyyMMdd</param>
      /// <param name="as_type">yyyyMMdd</param>
      /// <param name="as_osw_grp">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable GetVsrTxtData(DateTime as_date , string as_type , string as_osw_grp) {

         object[] parms = {
                ":as_date", as_date,
                ":as_type", as_type,
                ":as_osw_grp", as_osw_grp
            };


         string sql = @"
select 
    sp2_date, 
    sp2_type, 
    sp2_kind_id1, 
    sp2_kind_id2, 
    sp2_value_date, 
    spt1_kind_id1_out, 
    spt1_kind_id2_out, 
    spt1_abbr_name, 
    spt1_seq_no, 
    spt1_com_id, 
    sp1_rate, 
    sp1_seq_no, 
    sp2_osw_grp 
from 
    ci.sp2,  
    ci.sp1,  
    ci.spt1
where sp2_date = sp1_date
and sp2_type = sp1_type
and sp2_kind_id1 = sp1_kind_id1
and sp2_kind_id2 = sp1_kind_id2   
and sp2_kind_id1 = spt1_kind_id1   
and sp2_kind_id2 = spt1_kind_id2
and sp2_value_date = :as_date 
and sp2_type = :as_type 
and sp2_osw_grp like :as_osw_grp 
and sp2_adj_code = 'Y'
order by sp2_type , sp1_seq_no
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get ci.hzparm data return zparm_prod_id/zparm_comb_prod 2 fields (d40180_hparm)
      /// </summary>
      /// <param name="as_date">yyyyMMdd</param>
      /// <param name="as_type">yyyyMMdd</param>
      /// <param name="as_osw_grp">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable GetHzparmData(DateTime as_date) {

         object[] parms = {
                ":as_date", as_date
            };

         string sql = @"
select
    zparm_prod_id, 
    zparm_comb_prod
from ci.hzparm
where zparm_date = :as_date
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
