using OnePiece;
using System.Data;
/// <summary>
/// Winni, 2019/1/25
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class AM2:DataGate {

      /// <summary>
      /// get CI.AM2 data (for W28510) 好像跟28510跟28511的Stored Procedue有關係，但不確定
      /// </summary>
      /// <returns></returns>
      public DataTable ListData() {

         string sql = @"
SELECT AM2_YMD,   
       AM2_SUM_TYPE,   
       AM2_IDFG_TYPE,   
       AM2_PROD_TYPE,   
       AM2_PROD_SUBTYPE,   
       AM2_PARAM_KEY,   
       AM2_PC_CODE,   
       AM2_BS_CODE,   
       AM2_M_QNTY  
FROM ci.AM2
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }
   }
}

