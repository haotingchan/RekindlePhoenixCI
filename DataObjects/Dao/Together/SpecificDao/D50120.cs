using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D50120 : DataGate {

      public DataTable GetData(string as_ym) {

         object[] parms = {
                "@as_ym", as_ym
            };
         //MPDF_KIND_ID資料有多餘空白
         string sql = @"SELECT  MPDF_YM ,
                           MPDF_FCM_NO ,
                           MPDF_ACC_NO,
                           MPDF_STATUS ,
                           trim(MPDF_KIND_ID) as MPDF_KIND_ID ,
                           to_char(MPDF_EFF_DATE,'yyyy/mm/dd') as MPDF_EFF_DATE,
                           ' ' as OP_TYPE     
                           FROM CI.MPDF      
                           WHERE ( MPDF_YM = :as_ym )
                           Order By mpdf_ym Asc,
                                    mpdf_fcm_no Asc,
                                    mpdf_status Asc,
                                    mpdf_kind_id Asc";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      //會並行違規
      //        public ResultData UpdateMPDF(DataTable inputData) {
      //            string sql = @"
      //SELECT 
      //    MPDF_YM,      
      //    MPDF_FCM_NO,  
      //    MPDF_STATUS,  
      //    MPDF_KIND_ID, 
      //    MPDF_EFF_DATE,
      //    MPDF_ACC_NO,  
      //    MPDF_ACC_CODE
      //FROM CI.MPDF";

      //            return db.UpdateOracleDB(inputData, sql);
      //        }

      public ResultData UpdateMPDF(DataTable inputData) {

         string tableName = "CI.MPDF";
         string keysColumnList = @"MPDF_YM,      
                                      MPDF_FCM_NO,  
                                      MPDF_STATUS,  
                                      MPDF_KIND_ID, 
                                      MPDF_ACC_NO";
         string insertColumnList = @"MPDF_YM,      
                                        MPDF_FCM_NO,  
                                        MPDF_STATUS,  
                                        MPDF_KIND_ID, 
                                        MPDF_EFF_DATE,
                                        MPDF_ACC_NO";
         string updateColumnList = insertColumnList;
         try {
            //update to DB
            return SaveForChanged(inputData , tableName , insertColumnList , updateColumnList , keysColumnList);
         } catch (Exception ex) {
            throw ex;
         }
      }

      public DataTable GetMergeData(string as_ym) {

         object[] parms = {
                "@as_ym", as_ym
            };

         string sql = @"select A.MPDF_YM ,
            A.MPDF_FCM_NO ,
            A.MPDF_ACC_NO,
            A.MPDF_STATUS ,
            A.MPDF_KIND_ID ,
            A.MPDF_EFF_DATE  , 
            B.APDK_KIND_ID , 
            B.APDK_NAME,
            (case when B.APDK_NAME is not null then trim(A.MPDF_KIND_ID) || '(' || trim(B.APDK_NAME) || ')' else A.MPDF_KIND_ID end) as CP_DISPLAY
from ci.mpdf A
left  join  (SELECT APDK_PROD_TYPE,case APDK_PROD_SUBTYPE when 'S' then substr(APDK_KIND_ID,1,2) else substr(APDK_KIND_ID,1,3) end as APDK_KIND_ID,
       max(trim(APDK_NAME)) as APDK_NAME
FROM CI.APDK
WHERE (APDK_END_DATE IS NULL or APDK_END_DATE = '')
  and APDK_PROD_TYPE in ('F','O')
  and APDK_QUOTE_CODE = 'Y'
group by APDK_PROD_TYPE,case APDK_PROD_SUBTYPE when 'S' then substr(APDK_KIND_ID,1,2) else substr(APDK_KIND_ID,1,3) end
union 
SELECT ' ','-',' '
  FROM DUAL
order by apdk_prod_type , apdk_kind_id) B
on( trim(A.MPDF_KIND_ID) = trim(B.APDK_KIND_ID))
where A.MPDF_YM = :as_ym";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
