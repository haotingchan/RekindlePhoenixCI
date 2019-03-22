using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2018/2/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D20220: DataGate {
        
        public DataTable ListAll() {

            string sql =
        @"
 SELECT PLT1_PROD_TYPE, 
 		PLT1_PROD_SUBTYPE, 
 		PLT1_QNTY_MIN, 
 		PLT1_QNTY_MAX, 
 		PLT1_MULTIPLE, 
 		PLT1_MIN_NATURE, 
 		PLT1_MIN_LEGAL, 
 		' ' as OP_TYPE
 FROM CI.PLT1
 ORDER BY PLT1_PROD_TYPE, PLT1_PROD_SUBTYPE, PLT1_QNTY_MIN
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public ResultData updatePLT1(DataTable inputData) {
            string sql = @"
 SELECT PLT1_PROD_TYPE, 
 		  PLT1_PROD_SUBTYPE, 
 		  PLT1_QNTY_MIN, 
 		  PLT1_QNTY_MAX, 
 		  PLT1_MULTIPLE, 
 		  PLT1_MIN_NATURE, 
 		  PLT1_MIN_LEGAL
 FROM CI.PLT1";

            return db.UpdateOracleDB(inputData, sql);
        }

    }
}
