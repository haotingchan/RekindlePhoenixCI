using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class DP0040 : DataGate {

      /// <summary>
      /// 執行SP SP_QUERY_USER_STATUS , return 1 dataTable
      /// </summary>
      /// <param name="IN_FCM_NO"></param>
      /// <param name="IN_ACC_NO"></param>
      /// <returns></returns>
      public DataTable SP_QUERY_USER_STATUS(string IN_FCM_NO , string IN_ACC_NO) {
         //object[] parms ={
         //       "@IN_FCM_NO",IN_FCM_NO,
         //       "@IN_ACC_NO",IN_ACC_NO,
         //       "@RETURNPARAMETER",null
         //   };

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_FCM_NO",IN_FCM_NO),
            new DbParameterEx("IN_ACC_NO",IN_ACC_NO)
            //new DbParameterEx("RETURNPARAMETER",null)
         };


         string sql = "pos_owner.PKG_UTILITY.SP_QUERY_USER_STATUS";

         DataTable res = db.ExecuteStoredProcedureEx(sql ,parms,true);

         return res;
      }

   }
}
