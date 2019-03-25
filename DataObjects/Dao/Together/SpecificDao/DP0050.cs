using BusinessObjects;
using OnePiece;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class DP0050 : DataGate {

        public DataTable ExecuteStoredProcedure(string IN_START_DATE, string IN_END_DATE, string iniKey, DataTable dtTXFP) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_START_DATE",IN_START_DATE),
            new DbParameterEx("IN_END_DATE",IN_END_DATE)
            //new DbParameterEx("RETURNPARAMETER",0)
         };
            //切換DB Connection
            Db posDB = ChangeDB(dtTXFP, iniKey);

            string sql = "pos_owner.PKG_UTILITY.SP_QUERY_LOCK_TIMES";

            return posDB.ExecuteStoredProcedureEx(sql, parms, true);
        }

    } 
}
