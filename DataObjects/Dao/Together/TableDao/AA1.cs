using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/22
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class AA1: DataGate {

        /// <summary>
        /// for d_20310
        /// </summary>
        /// <param name="as_sym"></param>
        /// <param name="as_eym"></param>
        /// <returns></returns>
        public DataTable ListAllByDate(string as_sym, string as_eym) {

            object[] parms = {
                "@as_sym", as_sym,
                "@as_eym", as_eym
            };

            string sql =
        @"
SELECT AA1.AA1_YM,   
         AA1.AA1_TAIFEX,   
         AA1.AA1_TSE,   
         AA1.AA1_SGX_DT,   
         AA1.AA1_DAY_COUNT,   
         ' ' as OP_TYPE,
         AA1.AA1_US_RATE,
         AA1.AA1_OTC
    FROM ci.AA1  
   WHERE AA1.AA1_YM >= :as_sym  
     AND AA1.AA1_YM <= :as_eym
   Order By AA1_YM
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
