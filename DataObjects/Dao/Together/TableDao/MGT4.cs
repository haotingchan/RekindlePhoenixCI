using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/3/19
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class MGT4 : DataGate {
      /// <summary>
      /// get CI.MGT4 data return 12 feild (d49040)
      /// </summary>
      /// <returns></returns>

      public DataTable ListDataByMGT4() {

         string sql = @"
SELECT 
    MGT4_KIND_ID, 
    MGT4_TYPE, 
    MGT4_CUR_M_MULTI,
    MGT4_CUR_I_MULTI, 
    MGT4_CUR_DIGITAL, 

    MGT4_M_MULTI, 
    MGT4_I_MULTI, 
    MGT4_DIGITAL, 
    MGT4_W_TIME, 
    MGT4_W_USER_ID,

    MGT4_M_DIGITAL,
    ' ' as IS_NEWROW
FROM CI.MGT4
ORDER BY MGT4_KIND_ID , MGT4_TYPE
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT 
    MGT4_KIND_ID, 
    MGT4_TYPE, 
    MGT4_CUR_M_MULTI,
    MGT4_CUR_I_MULTI, 
    MGT4_CUR_DIGITAL, 
    MGT4_M_MULTI, 
    MGT4_I_MULTI, 
    MGT4_DIGITAL, 
    MGT4_W_TIME, 
    MGT4_W_USER_ID,
    MGT4_M_DIGITAL
FROM CI.MGT4
";

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
