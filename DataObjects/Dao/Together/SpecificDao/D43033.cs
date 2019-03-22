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
   public class D43033 : DataGate {
      /// <summary>
      /// SP_H_TXN_43033_DETL
      /// </summary>
      /// <param name="AS_DATE_FM"></param>
      /// <param name="AS_DATE_TO"></param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure(DateTime AS_DATE_FM,DateTime AS_DATE_TO) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_DATE_FM",AS_DATE_FM),
            new DbParameterEx("AS_DATE_TO",AS_DATE_TO)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_43033_DETL";

         return db.ExecuteStoredProcedureEx(sql , parms , true);
      }

   }
}
