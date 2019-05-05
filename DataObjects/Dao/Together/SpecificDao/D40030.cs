using BusinessObjects;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D40030 : DataGate {

      public DataTable GetAborad(string AS_ISSUE_YMD, string AS_OSW_GRP) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_DATE",AS_ISSUE_YMD),
            new DbParameterEx("AS_OSW_GRP",AS_OSW_GRP)
         };

         string sql = "CI.SP_H_TXN_40030_ABROAD";

         DataTable dt = db.ExecuteStoredProcedureEx(sql, parms, true);

         DataView dv = dt.AsDataView();
         dv.Sort = "SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC";
         dt = dv.ToTable();

         return dt;
      }

   }

}
