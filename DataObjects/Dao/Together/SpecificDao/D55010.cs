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
SELECT FEETRD_FCM_NO,
   FEETRD_ACC_NO,  
   NVL(ABRK_NAME,'') as BRK_ABBR_NAME,
   FEETRD_KIND_ID as sort_kind_id,
   CASE WHEN FEETRD_KIND_ID = 'TXW' THEN ' └─'||trim(FEETRD_KIND_ID) ELSE FEETRD_KIND_ID END AS FEETRD_KIND_ID,   
   (FEETRD_DISC_QNTY) AS FEETRD_DISC_QNTY,   
   (1 - FEETRD_REC_RATE) as DISC_RATE,   
   (FEETRD_MK_DISC_AMT + FEETRD_OTH_DISC_AMT) as DISC_AMT  
FROM CI.FEETRD,ci.ABRK 
WHERE FEETRD_YM >= @as_sym 
AND FEETRD_YM <= @as_eym 
AND FEETRD_FCM_KIND = '3'
AND (FEETRD_PARAM_KEY = FEETRD_KIND_ID OR FEETRD_KIND_ID = 'TXW')
AND FEETRD_FCM_NO = ABRK_NO(+)
ORDER BY feetrd_fcm_no , feetrd_acc_no , FEETRD_KIND_ID";

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
   sum(feetrd_disc_qnty) as feetrd_disc_qnty,   
   max(1 - feetrd_rec_rate) as disc_rate,   
   sum(feetrd_mk_disc_amt + feetrd_oth_disc_amt) as disc_amt  --,FEETRD_SESSION 
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