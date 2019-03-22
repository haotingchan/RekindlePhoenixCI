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

      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT 
    MGT8_KIND_ID, 
    MGT8_TYPE, 
    MGT8_CUR_M_MULTI,
    MGT8_CUR_I_MULTI, 
    MGT8_CUR_DIGITAL, 
    MGT8_M_MULTI, 
    MGT8_I_MULTI, 
    MGT8_DIGITAL, 
    MGT8_W_TIME, 
    MGT8_W_USER_ID,
    MGT8_M_DIGITAL
FROM CI.MGT8
";

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
