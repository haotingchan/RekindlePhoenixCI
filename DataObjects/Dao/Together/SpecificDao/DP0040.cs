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
      /// <param name="iniKey"></param>
      /// <returns></returns>
      public DataTable SP_QUERY_USER_STATUS(string IN_FCM_NO , string IN_ACC_NO , string iniKey , DataTable dtTXFP) {

         //切換DB Connection
         Db posDB = ChangeDB(dtTXFP , iniKey);

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_FCM_NO",IN_FCM_NO),
            new DbParameterEx("IN_ACC_NO",IN_ACC_NO)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

         string sql = "pos_owner.PKG_UTILITY.SP_QUERY_USER_STATUS";

         return posDB.ExecuteStoredProcedureEx(sql , parms , true);
      }

   }
}
