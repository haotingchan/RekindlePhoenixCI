using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.TableDao {
    public class INOTC1: DataGate {

        public DataTable ListAll() {

            string sql =
@"
SELECT * FROM CI.INOTC1
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public bool DeleteByDate(string ls_start_ymd, string ls_end_ymd) {

            object[] parms =
           {
                ":ls_start_ymd",ls_start_ymd,
                ":ls_end_ymd",ls_end_ymd
            };

            string sql = @"
delete ci.INOTC1 
where INOTC1_YMD >= :ls_start_ymd
  and INOTC1_YMD <=  :ls_end_ymd
";
            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0) {
                return true;
            }
            else {
                throw new Exception("INOTC1刪除失敗");
            }
        }

        public ResultData UpdateINOTC1(DataTable inputData) {
            string sql =
@"
SELECT  INOTC1_YMD,         
        INOTC1_TRADE_VOLUMN,
        INOTC1_TRADE_AMT,   
        INOTC1_TRADE_CNT,   
        INOTC1_INDEX,       
        INOTC1_UP_DOWN,     
        INOTC1_W_USER_ID,   
        INOTC1_W_TIME      
    FROM ci.IDFG
";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
