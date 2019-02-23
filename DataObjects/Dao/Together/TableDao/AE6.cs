using System.Data;
/// <summary>
/// Winni, 2019/2/18
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class AE6:DataGate {

      /// <summary>
      /// get CI.AE6 data 取得資料庫內最大日期 (for 30660)
      /// </summary>
      /// <returns>yyyyMMdd</returns>
      public DataTable MaxDate() {

         string sql = @"
SELECT NVL(MAX(AE6_YMD),'') as AE6_MAX_DATE FROM CI.AE6
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }
   }
}

