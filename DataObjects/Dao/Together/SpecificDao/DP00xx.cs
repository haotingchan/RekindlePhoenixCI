using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DataObjects.Dao.Together.SpecificDao {
   public class DP00xx : DataGate
    {
      /// <summary>
      /// 針對不同的grid data source,合併相同的輸入與輸出
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public IGridDataP00xx CreateGridData(Type daotype, Type classType, string name) {

         string AssemblyName = daotype.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = daotype.Namespace + "." + classType.Name + name;//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (IGridDataP00xx)Assembly.Load(AssemblyName).CreateInstance(className);
      }
   }

   public class QP00xx : DataGate {
      public string IN_START_DATE { get; set; }
      public string IN_END_DATE { get; set; }
      public string IN_SYSTEM { get; set; }
      public string IN_KIND { get; set; }
      public string IN_GROUP_TYPE { get; set; }

      public string IN_FCM_NO { get; set; }
      public string IN_ACC_NO { get; set; }
      public DataTable dtTXFP { get; set; }
      public Db posDB { get; }

      public QP00xx(string in_start_date, string in_end_date , string in_system , string in_kind , string in_group_type, DataTable dttxfp) {

         IN_START_DATE = in_start_date;
         IN_END_DATE = in_end_date;
         IN_SYSTEM = in_system;
         IN_KIND = in_kind;
         IN_GROUP_TYPE = in_group_type;

         dtTXFP = dttxfp;

         //切換DB Connection
         posDB = ChangeDB(dttxfp);
      }

      public QP00xx(string in_fcm_no , string in_acc_no , DataTable dttxfp) {
         IN_FCM_NO = in_fcm_no;
         IN_ACC_NO = in_acc_no;
         dtTXFP = dttxfp;

         //切換DB Connection
         posDB = ChangeDB(dttxfp);
      }
   }

   public interface IGridDataP00xx {
      DataTable GetData(QP00xx queryArgs);
   }

   public class WP0020Retrieve :  IGridDataP00xx {
      public DataTable GetData(QP00xx queryArgs) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_START_DATE",queryArgs.IN_START_DATE),
            new DbParameterEx("IN_END_DATE",queryArgs.IN_END_DATE),
            new DbParameterEx("IN_SYSTEM",queryArgs.IN_SYSTEM),
            new DbParameterEx("IN_KIND",queryArgs.IN_KIND),
            new DbParameterEx("IN_GROUP_TYPE",queryArgs.IN_GROUP_TYPE),
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "pos_owner.PKG_UTILITY.SP_QUERY_APPLY_CASE";

         return queryArgs.posDB.ExecuteStoredProcedureEx(sql, parms, true);
      }
   }

   public class WP0030Retrieve :  IGridDataP00xx {

      public DataTable GetData(QP00xx queryArgs) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_START_DATE",queryArgs.IN_START_DATE),
            new DbParameterEx("IN_END_DATE",queryArgs.IN_END_DATE),
            new DbParameterEx("IN_SYSTEM",queryArgs.IN_SYSTEM),
            new DbParameterEx("IN_GROUP_TYPE",queryArgs.IN_GROUP_TYPE)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "pos_owner.PKG_UTILITY.SP_QUERY_SEARCH_TIMES";

         return queryArgs.posDB.ExecuteStoredProcedureEx(sql, parms, true);
      }

   }

   public class WP0040Retrieve :  IGridDataP00xx {
      public DataTable GetData(QP00xx queryArgs) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_FCM_NO",queryArgs.IN_FCM_NO),
            new DbParameterEx("IN_ACC_NO",queryArgs.IN_ACC_NO)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

         string sql = "pos_owner.PKG_UTILITY.SP_QUERY_USER_STATUS";

         return queryArgs.posDB.ExecuteStoredProcedureEx(sql, parms, true);
      }
   }

   public class WP0050Retrieve :  IGridDataP00xx {
      public DataTable GetData(QP00xx queryArgs) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("IN_START_DATE",queryArgs.IN_START_DATE),
            new DbParameterEx("IN_END_DATE",queryArgs.IN_END_DATE)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "pos_owner.PKG_UTILITY.SP_QUERY_LOCK_TIMES";

         return queryArgs.posDB.ExecuteStoredProcedureEx(sql, parms, true);
      }

   }

}
