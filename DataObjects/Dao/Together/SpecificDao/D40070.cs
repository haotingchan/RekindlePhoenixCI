using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/17
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D40070: DataGate {

        public DataTable d_40070_scrn(string as_ymd, string as_cond) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("as_ymd",as_ymd),
            new DbParameterEx("as_cond",as_cond)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

            string sql = "CI.SP_H_TXN_40070_SCRN";

            return db.ExecuteStoredProcedureEx(sql, parms, true);
        }

    }
}
