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

         string sql = @"SELECT FEETRD_YM,FEETRD_FCM_NO,
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
                        AND FEETRD_FCM_NO = ABRK_NO(+)";

         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }
   }
}