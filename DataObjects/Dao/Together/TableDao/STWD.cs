using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/30
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class STWD: DataGate {

      /// <summary>
      /// 刪除被選取的日期資料 (for 28110 /winni)
      /// </summary>
      /// <param name="dateYmd">yyyyMMdd</param>
      /// <returns></returns>
      public bool DeleteByDate(string dateYmd) {

         object[] parms = {
                "@dateYmd", dateYmd
            };

         string sql = @"DELETE FROM CI.STWD WHERE STWD_YMD = :dateYmd";
         int executeResult = db.ExecuteSQL(sql , parms);

         if (executeResult >= 0) {
            return true;
         } else {
            throw new Exception("刪除失敗");
         }
      }

   }
}
