using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D55010 {
      private Db db;

      public D55010() {
         db = GlobalDaoSetting.DB;
      }

      //單月報表
      public DataTable ListDataSingleMonth(string as_sym , string as_eym) {
         object[] parms = {
               "@as_sym",as_sym,
               "@as_eym",as_eym
            };

            string sql = @"
                                select 
                                   feetrd_fcm_no,
                                   feetrd_acc_no,  
                                   nvl(abrk_name,'') as brk_abbr_name,
                                   feetrd_kind_id as sort_kind_id,
                                   case when feetrd_kind_id = 'TXW' then ' └─'||trim(feetrd_kind_id) else feetrd_kind_id end as feetrd_kind_id,   
                                   sum(case when (feetrd_kind_id in ('TXO','TXW') and feetrd_session =0 ) or  feetrd_session = 1 then feetrd_disc_qnty else 0 end) as feetrd_disc_qnty,   
                                   sum(case when (feetrd_kind_id in ('TXO','TXW') and feetrd_session =0 ) or feetrd_session = 1 then 1 - feetrd_rec_rate else 0 end) as disc_rate,   
                                   sum(case when (feetrd_kind_id in ('TXO','TXW') and feetrd_session =0 ) or feetrd_session = 1 then feetrd_mk_disc_amt + feetrd_oth_disc_amt else 0 end) as disc_amt,
                                   sum(case when feetrd_session = 2 then feetrd_disc_qnty else 0 end) as feetrd_disc_qnty_ah,   
                                   sum(case when feetrd_session = 2 then 1 - feetrd_rec_rate else 0 end) as disc_rate_ah,   
                                   sum(case when feetrd_session = 2 then feetrd_mk_disc_amt + feetrd_oth_disc_amt else 0 end) as disc_amt_ah,
                                   sum(feetrd_disc_qnty) as feetrd_disc_qnty_sum,    
                                   sum(feetrd_mk_disc_amt + feetrd_oth_disc_amt) as disc_amt_sum
                                from ci.feetrd,ci.abrk 
                                where feetrd_ym >= :as_sym 
                                and feetrd_ym <=  :as_eym 
                                and feetrd_fcm_kind = '3'
                                and (feetrd_param_key = feetrd_kind_id or feetrd_kind_id = 'TXW')
                                and feetrd_fcm_no = abrk_no(+)
                                group by 
                                    feetrd_fcm_no,
                                    feetrd_acc_no,  
                                    abrk_name,
                                    feetrd_kind_id
                                order by feetrd_fcm_no ,feetrd_acc_no , sort_kind_id            
                                ";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      //多月明細報表
      public DataTable ListDataMultiMonth(string as_sym , string as_eym) {
         object[] parms = {
               "@as_sym",as_sym,
               "@as_eym",as_eym
            };

         string sql = @"
select 
   feetrd_ym,
   feetrd_fcm_no,
   feetrd_acc_no,  
   nvl(abrk_name,'') as brk_abbr_name,
   feetrd_kind_id as sort_kind_id,
   case when feetrd_kind_id = 'TXW' then ' └─'||trim(feetrd_kind_id) else feetrd_kind_id end as feetrd_kind_id,   
   sum(case when (feetrd_kind_id in ('TXO','TXW') and feetrd_session =0 ) or  feetrd_session = 1 then feetrd_disc_qnty else 0 end) as feetrd_disc_qnty,   
   sum(case when (feetrd_kind_id in ('TXO','TXW') and feetrd_session =0 ) or feetrd_session = 1 then 1 - feetrd_rec_rate else 0 end) as disc_rate,   
   sum(case when (feetrd_kind_id in ('TXO','TXW') and feetrd_session =0 ) or feetrd_session = 1 then feetrd_mk_disc_amt + feetrd_oth_disc_amt else 0 end) as disc_amt,
   sum(case when feetrd_session = 2 then feetrd_disc_qnty else 0 end) as feetrd_disc_qnty_ah,   
   sum(case when feetrd_session = 2 then 1 - feetrd_rec_rate else 0 end) as disc_rate_ah,   
   sum(case when feetrd_session = 2 then feetrd_mk_disc_amt + feetrd_oth_disc_amt else 0 end) as disc_amt_ah,
   sum(feetrd_disc_qnty) as feetrd_disc_qnty_sum,    
   sum(feetrd_mk_disc_amt + feetrd_oth_disc_amt) as disc_amt_sum
from 
   ci.feetrd,
   ci.abrk 
where feetrd_ym >= @as_sym 
and feetrd_ym <= @as_eym 
and feetrd_fcm_kind = '3'
and (feetrd_param_key = feetrd_kind_id or feetrd_kind_id = 'TXW')
and feetrd_fcm_no = abrk_no(+)
group by 
   feetrd_ym,feetrd_fcm_no,
   feetrd_acc_no,  
   abrk_name,
   feetrd_kind_id
order by feetrd_ym ,feetrd_fcm_no ,feetrd_acc_no ,sort_kind_id
";

         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }
   }
}