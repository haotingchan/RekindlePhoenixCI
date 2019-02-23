using OnePiece;
using System.Data;

/// <summary>
/// ken 2019/2/21
/// </summary>
namespace DataObjects.Dao.Together {
   /// <summary>
   /// for 30684 , 30685
   /// </summary>
   public class VPR {
      private Db db;

      public VPR() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// ci.VPR return VPR_YMD/VPR_DATA_TIME/VPR_VALUE (for 30684 , 30685)
      /// </summary>
      /// <param name="as_symd">yyyyMMdd</param>
      /// <param name="as_eymd">yyyyMMdd</param>
      /// <param name="marketCode">S:(30684) / C:(30685)</param>
      /// <returns></returns>
      public DataTable ListByMarket(string as_symd , string as_eymd , char marketCode = 'S' , char printType = 'S') {
         object[] parms =
         {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":marketCode",marketCode
         };

         string sql = @"
SELECT VPR_YMD,VPR_DATA_TIME,VPR_VALUE
FROM CI.VPR
WHERE VPR_YMD BETWEEN :as_symd AND :as_eymd
AND VPR_UNDERLYING_MARKET = :marketCode
ORDER BY VPR_YMD , VPR_DATA_TIME 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (printType == 'S') {
            string[] title = new string[]{ "日期","時間","新加坡摩台期基準價" }; //30684(txt)
            int k = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[k++];
            }
         } else if(printType == 'C') {
            string[] title = new string[] { "日期" , "時間" , "CBOE VIX指數" }; //30685(csv)
            int k = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[k++];
            }
         }

         return dtResult;
      }

   }
}
