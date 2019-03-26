using BusinessObjects;
using OnePiece;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class DP0030 : DataGate {

        public DataTable ExecuteStoredProcedure(string IN_START_DATE, string IN_END_DATE, string IN_SYSTEM,
           string IN_GROUP_TYPE, string iniKey, DataTable dtTXFP) {

            //切換DB Connection
            Db posDB = ChangeDB(dtTXFP, iniKey);

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_START_DATE",IN_START_DATE),
            new DbParameterEx("IN_END_DATE",IN_END_DATE),
            new DbParameterEx("IN_SYSTEM",IN_SYSTEM),
            new DbParameterEx("IN_GROUP_TYPE",IN_GROUP_TYPE)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

            string sql = "pos_owner.PKG_UTILITY.SP_QUERY_SEARCH_TIMES";

            return posDB.ExecuteStoredProcedureEx(sql, parms, true);
        }

    }
}
