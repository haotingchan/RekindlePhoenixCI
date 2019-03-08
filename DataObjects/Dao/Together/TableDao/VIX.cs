using System.Data;

/// <summary>
/// Winni, 2019/3/5
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class VIX : DataGate {

      /// <summary>
      /// for W30680 (d_30680_2)
      /// </summary>
      /// <param name="type">N:新/O:舊</param>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="isTitle">是否有表頭</param>
      /// <returns></returns>
      public DataTable GetDataByDate(string type , string startDate , string endDate , string isTitle = "N") {

         object[] parms =
         {
                ":type", type,
                ":startDate", startDate,
                ":endDate", endDate
            };

         string sql = @"
SELECT VIX_YMD,
VIX_TIME,
VIX_VALUE
FROM CI.VIX
WHERE VIX_TYPE = :type
AND VIX_YMD >= :startDate
AND VIX_YMD <= :endDate
ORDER BY VIX_YMD,VIX_TIME
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isTitle == "Y") {
            string[] title = new string[] { "日期" , "時間" , "VIX指數" };
            int w = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[w++];
            }
         }

         return dtResult;
      }

   }
}
