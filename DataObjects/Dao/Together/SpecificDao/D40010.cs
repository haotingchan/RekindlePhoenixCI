using BusinessObjects;
using OnePiece;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D40010 : DataGate {

      public DataTable GetData(string AS_ISSUE_YMD, string AS_ADJ_TYPE, string AS_ADJ_SUBTYPE) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_ISSUE_YMD",AS_ISSUE_YMD),
            new DbParameterEx("AS_ADJ_TYPE",AS_ADJ_TYPE),
            new DbParameterEx("AS_ADJ_SUBTYPE",AS_ADJ_SUBTYPE),
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_40090_DETL";

         return db.ExecuteStoredProcedureEx(sql, parms, true);
      }

      public string GetMaxOcfDate(string implBeginDate, string implEndDate) {

         object[] parms = {
                ":ls_impl_begin_ymd", implBeginDate,
                ":ls_impl_end_ymd", implEndDate
            };

         string sql = @"SELECT MAX(MOCF_YMD) as ls_mocf_ymd
FROM ci.MOCF 
WHERE MOCF_YMD >= :ls_impl_begin_ymd
AND MOCF_YMD < :ls_impl_end_ymd
AND MOCF_OPEN_CODE = 'Y'";

         return db.ExecuteScalar(sql, CommandType.Text, parms);
      }
   }
}
