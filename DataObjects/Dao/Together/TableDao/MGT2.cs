using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/4/3
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class MGT2 : DataGate {

      /// <summary>
      /// save data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT  
	MGT2_KIND_ID,
	MGT2_KIND_ID_OUT,
	MGT2_SEQ_NO,
	MGT2_PROD_TYPE,
	MGT2_PROD_SUBTYPE,

	MGT2_ABBR_NAME,
	MGT2_NAME,
	MGT2_GROUP_KIND_ID,
	MGT2_STOCK_ID,
	MGT2_END_YMD,

	MGT2_DATA_TYPE,
	MGT2_ADJUST_RATE,
	MGT2_CP_KIND,
	MGT2_ABROAD,
	MGT2_W_TIME,

	MGT2_W_USER_ID
FROM CI.MGT2
ORDER BY MGT2_SEQ_NO , MGT2_KIND_ID
";

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
