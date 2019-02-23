using OnePiece;
using System.Data;

/// <summary>
/// Winni 2019/01/24
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30650 {
      private Db db;

      public D30650() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// Get data by CI.ABRK CI.AM10 (已經固定一些過濾條件) (ABRK_NO/ABRK_ABBR_NAME/ABRK_NO4/AM10_YM/AM10_QNTY/AM10_DT_QNTY/AM10_RATE)
      /// </summary>
      /// <param name="as_symd">起始年月</param>
      /// <param name="as_eymd">結束年月</param>
      /// <returns></returns>
      public DataTable GetData(string as_symd , string as_eymd) {
         object[] parms =
         {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

         string sql = @"
SELECT ABRK_NO,
       ABRK_ABBR_NAME,
       ABRK_NO4,
       AM10_YM,
       AM10_QNTY,
       AM10_DT_QNTY,
       CASE WHEN AM10_QNTY = 0 THEN 0 ELSE ROUND(AM10_DT_QNTY/AM10_QNTY,4)*100 END AS AM10_RATE
FROM 
    (SELECT SUBSTR(ABRK_NO,1,4) AS ABRK_NO4,SUBSTR(ABRK_NO,1,4)||NVL(MAX(CASE WHEN SUBSTR(ABRK_NO,5,3) = '000' THEN '000' ELSE NULL END),'999') AS ABRK_NO7
    FROM CI.ABRK 
    GROUP BY SUBSTR(ABRK_NO,1,4)),
    CI.ABRK,
    (SELECT SUBSTR(AM10_YMD,1,6) AS AM10_YM,
    SUBSTR(AM10_BRK_NO,1,4) AS AM10_BRK_NO4,
    SUM(AM10_QNTY) AS AM10_QNTY,
    SUM(AM10_DT_QNTY) AS AM10_DT_QNTY
      FROM CI.AM10
    WHERE AM10_YMD >= :as_symd
    AND AM10_YMD <= :as_eymd
    AND SUBSTR(AM10_BRK_NO,5,3) <> '999'
     GROUP BY SUBSTR(AM10_YMD,1,6),SUBSTR(AM10_BRK_NO,1,4)) M
WHERE ABRK_NO4 = AM10_BRK_NO4
AND ABRK_NO7 = ABRK_NO
ORDER BY AM10_YM , ABRK_NO
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Get data by CI.ABRK CI.AM10 (已經固定一些過濾條件) (ABRK_NO/ABRK_ABBR_NAME/ABRK_NO4/AM10_QNTY/AM10_DT_QNTY/AM10_RATE)
      /// </summary>
      /// <param name="as_symd">起始年月</param>
      /// <param name="as_eymd">結束年月</param>
      /// <returns></returns>
      public DataTable GetTmpData(string as_symd , string as_eymd) {
         object[] parms =
         {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

         string sql = @"
SELECT ABRK_NO,
       ABRK_ABBR_NAME,
       ABRK_NO4,
       AM10_QNTY,
       AM10_DT_QNTY,
       CASE WHEN AM10_QNTY = 0 THEN 0 ELSE ROUND(AM10_DT_QNTY/AM10_QNTY,4)*100 END AS AM10_RATE
FROM 
    (SELECT SUBSTR(ABRK_NO,1,4) AS ABRK_NO4,
            SUBSTR(ABRK_NO,1,4)||NVL(MAX(CASE WHEN SUBSTR(ABRK_NO,5,3) = '000' THEN '000' ELSE NULL END),'999') AS ABRK_NO7
    FROM CI.ABRK 
    GROUP BY SUBSTR(ABRK_NO,1,4)),
    CI.ABRK,
    (SELECT SUBSTR(AM10_BRK_NO,1,4) AS AM10_BRK_NO4,
            SUM(AM10_QNTY) AS AM10_QNTY,SUM(AM10_DT_QNTY) AS AM10_DT_QNTY
    FROM CI.AM10
    WHERE AM10_YMD >= :as_symd
    AND AM10_YMD <= :as_eymd
    AND SUBSTR(AM10_BRK_NO,5,3) <> '999'
    GROUP BY SUBSTR(AM10_BRK_NO,1,4)) M
WHERE ABRK_NO4 = AM10_BRK_NO4
AND ABRK_NO7 = ABRK_NO
ORDER BY AM10_RATE DESC ,ABRK_NO ASC
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
