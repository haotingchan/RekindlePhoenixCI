using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D51040: DataGate {

        public bool DeleteByDate(string ym) {

            object[] parms = {
                ":ym", ym
            };

            string sql = @"DELETE FROM CI.MMWK
                           WHERE MMWK_YM = :ym";
            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        public ResultData UpdateMMWK(DataTable inputData) {
            string sql = @"
SELECT 
    MMWK_PROD_TYPE,
    MMWK_YM,       
    MMWK_KIND_ID,  
    MMWK_WEIGHT,   
    MMWK_W_USER_ID,
    MMWK_W_TIME   
FROM CI.MMWK";

            return db.UpdateOracleDB(inputData, sql);
        }
    }

}
