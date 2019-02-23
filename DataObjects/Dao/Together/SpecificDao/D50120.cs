using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D50120 {

        private Db db;

        public D50120() {

            db = GlobalDaoSetting.DB;

        }

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
                           MPDF_EFF_DATE ,
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
    }
}
