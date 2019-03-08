using System.Data;

/// <summary>
/// Winni, 2019/3/5
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class VIXS : DataGate {

      /// <summary>
      /// for W30680 (d_30680_1)
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
SELECT   
VIXS_YMD,   
VIXS_OPEN,   
VIXS_HIGH,   
VIXS_LOW,   
VIXS_CLOSE
--,VIXS_AVG
FROM CI.VIXS  
WHERE VIXS_TYPE = :type
AND  VIXS_YMD >= :startDate
AND  VIXS_YMD <= :endDate     
ORDER BY VIXS_YMD 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isTitle == "Y") {
            string[] title = new string[] { "日期" , "開盤(8:46)" , "最高" , "最低" , "收盤(13:45)" };
            int w = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[w++];
            }
         }

         return dtResult;
      }

   }
}
