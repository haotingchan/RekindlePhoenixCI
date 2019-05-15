using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D50120:DataGate {

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

            DataTable dtResult = db.GetDataTable(sql, parms);

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
                return SaveForChanged(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
