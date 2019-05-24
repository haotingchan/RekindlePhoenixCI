using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/3/19
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class MGT8 : DataGate {

      /// <summary>
      /// get CI.MGT8 data MGT8_F_ID/MGT8_F_NAME/MGT8_F_EXCHANGE/CP_DISPLAY return 4 feild (dddw_mgt8_f_id)
      /// MGT8的dropdownlist (for 49060)
      /// </summary>
      /// <returns></returns>
      public DataTable ListDataByMGT8() {

         string sql = @"
SELECT 
A.MGT8_F_ID,
A.MGT8_F_NAME,
A.MGT8_F_EXCHANGE,
(CASE WHEN TRIM(MGT8_F_ID) IS NULL THEN TRIM(MGT8_F_NAME)
     ELSE '【'||TRIM(MGT8_F_ID)||'】'||TRIM(MGT8_F_EXCHANGE)||' '|| TRIM(MGT8_F_NAME)END) AS CP_DISPLAY
FROM (
		SELECT 
		MGT8_F_ID,
		MGT8_F_NAME,
		MGT8_F_EXCHANGE
		FROM CI.MGT8
	 ) A
ORDER BY MGT8_F_ID
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// return 12 feild (for d49061) 
      /// </summary>
      /// <returns></returns>
      public DataTable ListData() {

         string sql = @"
SELECT
   MGT8_F_ID,  
   MGT8_F_EXCHANGE, 
   MGT8_F_NAME,      
   MGT8_RT_ID, 
   MGT8_KIND_TYPE,  

   MGT8_FOREIGN,
   MGT8_CURRENCY_TYPE,           
   MGT8_STRUTURE,           
   MGT8_AMT_TYPE,  
   MGT8_XXX,           	     
	      
   MGT8_W_USER_ID,           
   MGT8_W_TIME,
   ' ' as IS_NEWROW
FROM CI.MGT8
ORDER BY UPPER(MGT8_F_ID)
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// save data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT 
   MGT8_F_ID,  
   MGT8_F_EXCHANGE, 
   MGT8_F_NAME,      
   MGT8_RT_ID, 
   MGT8_KIND_TYPE, 

   MGT8_FOREIGN, 
   MGT8_CURRENCY_TYPE,           
   MGT8_STRUTURE,           
   MGT8_AMT_TYPE,  
   MGT8_XXX,      

   MGT8_W_USER_ID,           
   MGT8_W_TIME
FROM CI.MGT8
";

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
