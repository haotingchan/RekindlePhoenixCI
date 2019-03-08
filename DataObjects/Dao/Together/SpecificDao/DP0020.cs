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
   public class DP0020 : DataGate {

      /// <summary>
      /// 執行SP SP_QUERY_USER_STATUS , return 1 dataTable
      /// </summary>
      /// <param name="IN_FCM_NO"></param>
      /// <param name="IN_ACC_NO"></param>
      /// <returns></returns>
      //public DataTable SP_QUERY_USER_STATUS(string IN_FCM_NO , string IN_ACC_NO) {
      //   //object[] parms ={
      //   //       "@IN_FCM_NO",IN_FCM_NO,
      //   //       "@IN_ACC_NO",IN_ACC_NO,
      //   //       "@RETURNPARAMETER",null
      //   //   };

      //   List<DbParameterEx> parms = new List<DbParameterEx>() {
      //      new DbParameterEx("IN_FCM_NO",IN_FCM_NO),
      //      new DbParameterEx("IN_ACC_NO",IN_ACC_NO)
      //      //new DbParameterEx("RETURNPARAMETER",null)
      //   };


      //   string sql = "pos_owner.PKG_UTILITY.SP_QUERY_USER_STATUS";

      //   DataTable res = db.ExecuteStoredProcedureEx(sql ,parms,true);

      //   return res;
      //}

      public DataTable ExecuteStoredProcedure(string IN_START_DATE , string IN_END_DATE , 
         string IN_SYSTEM , string IN_KIND , string IN_GROUP_TYPE) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_START_DATE",IN_START_DATE),
            new DbParameterEx("IN_END_DATE",IN_END_DATE),
            new DbParameterEx("IN_SYSTEM",IN_SYSTEM),
            new DbParameterEx("IN_KIND",IN_KIND),
            new DbParameterEx("IN_GROUP_TYPE",IN_GROUP_TYPE),
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "pos_owner.PKG_UTILITY.SP_QUERY_APPLY_CASE";

         return db.ExecuteStoredProcedureEx(sql , parms , true);
      }

   }
}
