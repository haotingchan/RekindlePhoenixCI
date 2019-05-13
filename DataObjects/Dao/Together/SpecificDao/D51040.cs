using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D51040: DataGate {

        public bool DeleteByDate(string date) {

            object[] parms = {
                "@date", date
            };

            string sql = @"DELETE FROM CI.MMWK
                           WHERE MMWK_YM = :date";
            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0) {
                return true;
            }
            else {
                throw new Exception("刪除失敗");
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
