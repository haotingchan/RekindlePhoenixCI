using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/3/25
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class SPT1 : DataGate {

      /// <summary>
      /// save CI.SPT1 data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT 
   SPT1_KIND_ID1, 
   SPT1_KIND_ID2, 
   SPT1_ABBR_NAME, 
   SPT1_NAME, 
   SPT1_SEQ_NO, 

   SPT1_W_TIME, 
   SPT1_W_USER_ID, 
   SPT1_KIND_ID1_OUT, 
   SPT1_KIND_ID2_OUT, 
   SPT1_COM_ID, 

   SPT1_OSW_GRP, 
   SPT1_ADJUST_RATE, 
   SPT1_DATA_TYPE, 
   SPT1_MAX_SPNS_RATE
FROM CI.SPT1
";

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
