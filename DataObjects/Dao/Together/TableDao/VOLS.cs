using System.Data;

namespace DataObjects.Dao.Together.TableDao {
   public class VOLS : DataGate {

      /// <summary>
      /// for W30680 (for d_30680_3, 30685)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="type">V/H/I/J (30680預設帶入V)</param>
      /// <param name="isTitle">是否有表頭</param>
      /// <returns></returns>
      public DataTable GetDataByDate(string startDate , string endDate , string type = "V" , string isTitle = "N") {

         object[] parms =
         {
                ":startDate", startDate,
                ":endDate", endDate,
                ":type", type
            };

         string sql = @"
SELECT 
VOLS_YMD,   
VOLS_MARKET_CODE,
VOLS_EXCLUDE_CNT,   
VOLS_VALUE  
FROM ci.VOLS   
WHERE VOLS_YMD >= :startDate
AND VOLS_YMD <= :endDate
AND VOLS_DATA_TYPE = :type
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isTitle == "Y") {
            string[] title = new string[] { "日期" , "交易時段:0一般/1夜盤" , "極端筆數" , "平均值" };
            int w = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[w++];
            }
         }

         return dtResult;
      }

   }
}
