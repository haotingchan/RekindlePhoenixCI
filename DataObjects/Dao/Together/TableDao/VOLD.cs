using System.Data;

namespace DataObjects.Dao.Together.TableDao {
   public class VOLD : DataGate {

      /// <summary>
      /// for W30680 (for d_30680_4, 30685)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="type">V/H/I/J (30680預設帶入V)</param>
      /// <param name="isTitle">是否有表頭</param>
      /// <returns></returns>
      public DataTable GetDataByDate(string startDate , string endDate , string type , string isTitle = "N") {

         object[] parms =
         {
                ":startDate", startDate,
                ":endDate", endDate,
                ":type", type
            };

         string sql = @"
SELECT 
VOLD_YMD,
VOLD_MARKET_CODE,
to_char(VOLD_DATA_TIME,'yyyy/mm/dd hh24:mi:ss') as VOLD_DATA_TIME,   
VOLD_VALUE,   
VOLD_EXCLUDE_FLAG  
FROM CI.VOLD   
WHERE VOLD_YMD >= :startDate
AND VOLD_YMD <= :endDate
AND VOLD_DATA_TYPE = :type
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isTitle == "Y") {
            string[] title = new string[] { "日期" , "交易時段:0一般/1夜盤" , "時間" , "值" , "Y: 極端值/N:非極端值" };
            int w = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[w++];
            }
         }

         return dtResult;
      }

   }
}
