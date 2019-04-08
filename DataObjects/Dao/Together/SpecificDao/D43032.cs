using BusinessObjects;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/4/2
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 上市、上櫃受益憑證原始資料查詢
   /// </summary>
   public class D43032 : DataGate {

      /// <summary>
      /// 呼CI.SP_H_TXN_43032_SCRN (d_403032_scrn)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure(string startDate) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("startDate",startDate)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_43032_SCRN";

         return db.ExecuteStoredProcedureEx(sql , parms , true);
      }

      /// <summary>
      /// 呼CI.SP_H_TXN_43032_DETL (d_403032)
      /// </summary>
      /// <param name="modelType">yyyyMMdd</param>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="prodType">yyyyMMdd</param>
      /// <param name="kindId">yyyyMMdd</param>
      /// <param name="sid">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure2(string modelType , string startDate , string endDate ,
                                                string prodType , string kindId , string sid) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("modelType",modelType),
            new DbParameterEx("startDate",startDate),
            new DbParameterEx("endDate",endDate),
            new DbParameterEx("prodType",prodType),
            new DbParameterEx("kindId",kindId),
            new DbParameterEx("sid",sid)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_43032_DETL";

         return db.ExecuteStoredProcedureEx(sql , parms , true);
      }

   }
}
