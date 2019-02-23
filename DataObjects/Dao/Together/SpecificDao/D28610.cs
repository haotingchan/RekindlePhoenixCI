using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D28610 : DataGate {

      /// <summary>
      /// 執行SP
      /// </summary>
      /// <param name="Is_ym"></param>
      /// <param name="RETURNPARAMETER">一開始傳null，成功回傳0</param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure(DateTime Is_ym) {
         object[] parms ={
                "@ls_ym",Is_ym,
                "@RETURNPARAMETER",null
            };

         string sql = "sp_H_stt_AB3";

         DataTable reResult = db.ExecuteStoredProcedure_Override(sql , parms);

         return reResult;
      }

      /// <summary>
      /// 刪除被選取的日期資料
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public bool DeleteByDate(string as_date) {

         object[] parms = {
                "@as_date", as_date
            };

         string sql = @"DELETE FROM CI.AB1 WHERE AB1_DATE = TO_DATE(:as_date , 'yyyy/mm/dd')";
         int executeResult = db.ExecuteSQL(sql , parms);

         if (executeResult > 0) {
            return true;
         } else {
            throw new Exception("刪除失敗");
         }
      }

      /// <summary>
      /// 新增的Row都Inser進DB
      /// </summary>
      /// <param name="ab1_acc_type">身分碼</param>
      /// <param name="ab1_count">資料日開戶數</param>
      /// <param name="ab1_accu_count">累積開戶數</param>
      /// <param name="ab1_trade_count">交易戶數</param>
      /// <param name="ab1_date">資料日期</param>
      /// <returns></returns>
      public bool InsertAB1(string ab1_acc_type, string ab1_count, string ab1_accu_count, string ab1_trade_count, DateTime ab1_date) {

         object[] parms =
         {
                ":ab1_acc_type",ab1_acc_type,
                ":ab1_count",ab1_count,
                ":ab1_accu_count",ab1_accu_count,
                ":ab1_trade_count",ab1_trade_count,
                ":ab1_date",ab1_date
            };

         string sql = @"
INSERT INTO ci.AB1
(AB1_ACC_TYPE,AB1_COUNT,AB1_ACCU_COUNT,AB1_TRADE_COUNT,AB1_DATE)
VALUES (:ab1_acc_type, :ab1_count, :ab1_accu_count, :ab1_trade_count, :ab1_date)
                                ";

         int executeResult = db.ExecuteSQL(sql , parms);

         if (executeResult > 0) {
            return true;
         } else {
            throw new Exception("AB1更新失敗");
         }
      }
   }
}
