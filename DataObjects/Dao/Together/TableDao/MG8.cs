using BusinessObjects;
using System.Data;
/// <summary>
/// Winni, 2019/3/19
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class MG8 : DataGate {

      /// <summary>
      /// get CI.MG8 data return 8 feild (d94060)
      /// MG8的dropdownlist (for 49060)
      /// </summary>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <returns></returns>
      public DataTable ListData(string startDate , string endDate) {
         object[] parms =
            {
                ":startDate", startDate,
                ":endDate", endDate
            };

         string sql = @"
SELECT     
    MG8_EFFECT_YMD, 
    MG8_F_ID, 
    MG8_CM, 
    MG8_MM, 
    MG8_IM, 

    MG8_ISSUE_YMD, 
    MG8_W_TIME, 
    MG8_W_USER_ID,
    '' AS IS_NEWROW
FROM CI.MG8
WHERE MG8_EFFECT_YMD >= :startDate
AND MG8_EFFECT_YMD <= :endDate
ORDER BY MG8_EFFECT_YMD , MG8_F_ID
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.MG8 data return 8 feild
      /// </summary>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT 
    MG8_EFFECT_YMD, 
    MG8_F_ID, 
    MG8_CM, 
    MG8_MM, 
    MG8_IM, 

    MG8_ISSUE_YMD, 
    MG8_W_TIME, 
    MG8_W_USER_ID
FROM CI.MG8
";

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
