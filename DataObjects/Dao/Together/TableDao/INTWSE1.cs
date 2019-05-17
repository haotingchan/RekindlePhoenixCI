using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/28
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class INTWSE1: DataGate {

        public DataTable ListAll() {

            string sql =
@"
SELECT * FROM CI.INTWSE1
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
delete ci.INTWSE1 
where INTWSE1_YMD >= :ls_start_ymd
  and INTWSE1_YMD <=  :ls_end_ymd
";
            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0) {
                return true;
            }
            else {
                throw new Exception("INTWSE1刪除失敗");
            }
        }

        public ResultData UpdateINTWSE1(DataTable inputData) {
            string sql =
@"
SELECT  INTWSE1_YMD,         
        INTWSE1_TRADE_VOLUMN,
        INTWSE1_TRADE_AMT,   
        INTWSE1_TRADE_CNT,   
        INTWSE1_INDEX,       
        INTWSE1_UP_DOWN,     
        INTWSE1_W_USER_ID,   
        INTWSE1_W_TIME      
    FROM ci.IDFG
";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
