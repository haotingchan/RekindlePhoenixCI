using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D51060
    {
        private Db db;

        public D51060()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetData(string as_ym)
        {
            object[] parms ={
                "@as_ym",as_ym
            };

            string sql = @"
                        SELECT * 
                        FROM CI.MMIQ  
                        WHERE CI.MMIQ.MMIQ_YM = :as_ym
                        order by MMIQ_FCM_NO";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ExecuteStoredProcedure(string Is_ym)
        {
            object[] parms ={
                "@ls_ym",Is_ym,
                "@RETURNPARAMETER",0
            };

            string sql = "SP_H_UPD_AMM0_MONTH";

            DataTable dtResult = db.ExecuteStoredProcedure_Override(sql, parms);

            return dtResult;
        }
    }
}
