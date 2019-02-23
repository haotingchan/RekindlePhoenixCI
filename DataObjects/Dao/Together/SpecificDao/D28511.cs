using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D28511 : DataGate {

      /// <summary>
      /// 執行SP
      /// </summary>
      /// <param name="Is_ym"></param>
      /// <param name="RETURNPARAMETER">一開始傳null，成功回傳0</param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure(string Is_ym) {
         object[] parms ={
                "@ls_ym",Is_ym,
                "@RETURNPARAMETER",null
            };

         string sql = "sp_H_stt_AM21";

         DataTable reResult = db.ExecuteStoredProcedure_Override(sql ,parms);

         return reResult;
      }

      /// <summary>
      /// 刪除整個被選取的月份資料
      /// </summary>
      /// <param name="as_date">選取月份</param>
      /// <returns></returns>
      public bool DeleteByDate(string as_date) {

         object[] parms = {
                "@as_date", as_date
            };

         string sql = @"DELETE FROM CI.AM21F WHERE AM2F_YM = :as_date";
         int executeResult = db.ExecuteSQL(sql , parms);

         if (executeResult > 0) {
            return true;
         } else {
            throw new Exception("刪除失敗");
         }
      }
   }
}
