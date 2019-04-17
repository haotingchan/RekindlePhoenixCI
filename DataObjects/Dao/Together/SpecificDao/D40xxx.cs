using BusinessObjects;
using OnePiece;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D40xxx : DataGate {

      public DataTable GetData(string AS_ISSUE_YMD, string AS_ADJ_TYPE, string AS_ADJ_SUBTYPE) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_ISSUE_YMD",AS_ISSUE_YMD),
            new DbParameterEx("AS_ADJ_TYPE",AS_ADJ_TYPE),
            new DbParameterEx("AS_ADJ_SUBTYPE",AS_ADJ_SUBTYPE),
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_40090_DETL";

         DataTable dt = db.ExecuteStoredProcedureEx(sql, parms, true);

         DataView dv = dt.AsDataView();
         dv.Sort = "SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC";
         dt = dv.ToTable();

         //dt = dt.AsEnumerable().OrderBy(d => d.Field<int>("SEQ_NO"))
         //      .ThenBy(d => d.Field<string>("PROD_TYPE"))
         //      .ThenBy(d => d.Field<string>("KIND_GRP2"))
         //      //.ThenBy(d => {
         //      //   if (d.Field<string>("KIND_ID").Substring(0, 2) == d.Field<string>("KIND_GRP2"))
         //      //      return 0;
         //      //   else
         //      //      return 1;
         //      //})
         //      .ThenBy(d => d.Field<string>("KIND_ID"))
         //      .ThenBy(d => d.Field<string>("AB_TYPE"))
         //      .CopyToDataTable();

         return dt;
      }
   }
}
