using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/3/14
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 股票期貨各標的統計表
   /// </summary>
   public class D30410 {

      private Db db;

      public D30410() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// get CI.AI2/CI.AM10/CI.AM9/CI.AB4/CI.APDK data (d30410)
      /// return AI2_KIND_ID/AI2_M_QNTY/AI2_OI/AM10_CNT/AM9_ACC_CNT/AB4_ID_CNT/APDK_NAME
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable ListData(string startDate , string endDate) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate
            };


         string sql = @"
SELECT 
  AI2.AI2_KIND_ID||'F' AS AI2_KIND_ID,
  AI2_M_QNTY,
  AI2_OI,
  NVL(AM10_CNT,0) AM10_CNT,
  NVL(AM9_ACC_CNT,0) AM9_ACC_CNT,
  NVL(AB4_ID_CNT,0) AB4_ID_CNT,
  APDK_NAME
FROM 
  (SELECT 
    SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID,
    SUM(AI2_M_QNTY) AS AI2_M_QNTY,
    SUM(CASE WHEN AI2_YMD = :endDate THEN AI2_OI ELSE 0 END) AS AI2_OI
   FROM CI.AI2
   WHERE AI2_YMD >= :startDate
   AND AI2_YMD <= :endDate
   AND AI2_SUM_TYPE = 'D'
   AND AI2_SUM_SUBTYPE = '4'
   AND AI2_PROD_TYPE = 'F'
   AND AI2_PROD_SUBTYPE = 'S'
   GROUP BY SUBSTR(AI2_KIND_ID,1,2) ) AI2,
  (SELECT 
    AM10_KIND_ID2 AS AM10_KIND_ID,
    SUM(AM10_CNT) AM10_CNT 
   FROM CI.AM10 
   WHERE AM10_YMD >= :startDate 
   AND AM10_YMD <= :endDate
   AND AM10_PROD_TYPE = 'F' 
   AND AM10_PROD_SUBTYPE = 'S'
   GROUP BY AM10_KIND_ID2) AM10,
  (SELECT 
    AM9_KIND_ID2 AS AM9_KIND_ID,
    AM9_ACC_CNT_ACCU AS AM9_ACC_CNT 
   FROM CI.AM9 
   WHERE AM9_YMD = :endDate
   AND AM9_PROD_TYPE = 'F' 
   AND AM9_PROD_SUBTYPE = 'S' 
   AND AM9_KIND_ID2 <> '999') ACC,
  (SELECT 
    AB4_KIND_ID,
    AB4_ID_CNT_ACCU AS AB4_ID_CNT 
   FROM CI.AB4
   WHERE AB4_PROD_TYPE = 'F' 
   AND AB4_PROD_SUBTYPE = 'S'  
   AND AB4_KIND_ID <> '999' 
   AND AB4_DATE = TO_DATE(:endDate,'YYYYMMDD')) AB4,
  CI.APDK
WHERE AI2.AI2_KIND_ID = TRIM(AM10.AM10_KIND_ID(+))
AND AI2.AI2_KIND_ID = TRIM(ACC.AM9_KIND_ID(+))
AND AI2.AI2_KIND_ID = TRIM(AB4_KIND_ID(+))
AND AI2.AI2_KIND_ID||'F' = TRIM(APDK_KIND_ID(+))
ORDER BY AI2_KIND_ID
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.APDK/CI.AMIF data (d30411)
      /// return AMIF_KIND_ID/AMIF_SETTLE_DATE/AMIF_M_QNTY_TAL/APDK_NAME/AMIF_OPEN_INTEREST
      /// </summary>
      /// <param name="startDate">yyyy/MM/dd</param>
      /// <param name="endDate">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable ListData2(DateTime startDate , DateTime endDate) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate
            };


         string sql = @"
SELECT 
  AMIF_KIND_ID,
  AMIF_SETTLE_DATE,
  AMIF_M_QNTY_TAL,
  APDK_NAME,SEQ_NO,
  AMIF_OPEN_INTEREST
FROM CI.APDK,
      --交割月份
     (SELECT SETTLE_DATE,ROWNUM AS SEQ_NO
      FROM
        (SELECT AMIF_SETTLE_DATE AS SETTLE_DATE
         FROM CI.AMIF
         WHERE AMIF_DATE >= :startDate
         AND AMIF_DATE <= :endDate
         AND AMIF_DATA_SOURCE  IN ('T','P')
         AND AMIF_PROD_TYPE = 'F'
         AND AMIF_PROD_SUBTYPE = 'S'
         GROUP BY AMIF_SETTLE_DATE
         ORDER BY AMIF_SETTLE_DATE)) S,
      --值  
     (SELECT 
        AMIF_KIND_ID,
        AMIF_SETTLE_DATE,
        SUM(AMIF_M_QNTY_TAL) AMIF_M_QNTY_TAL ,
        SUM(CASE WHEN AMIF_DATE = :endDate THEN AMIF_OPEN_INTEREST ELSE 0 END) AMIF_OPEN_INTEREST
      FROM CI.AMIF
      WHERE AMIF_DATE >= :startDate
      AND AMIF_DATE <= :endDate
      AND AMIF_DATA_SOURCE  IN ('T','P')
      AND AMIF_PROD_TYPE = 'F'
      AND AMIF_PROD_SUBTYPE = 'S'
      GROUP BY AMIF_KIND_ID,AMIF_SETTLE_DATE)
WHERE AMIF_KIND_ID = APDK_KIND_ID
AND AMIF_SETTLE_DATE = SETTLE_DATE
ORDER BY AMIF_KIND_ID , AMIF_SETTLE_DATE
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Get max(AI2_YMD) to string [yyyyMMdd]
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <returns>yyyyMMdd</returns>
      public string GetMaxDate(string startDate , string endDate) {
         object[] parms =
         {
                ":startDate", startDate,
                ":endDate", endDate
            };

         //AI2_YMD format=yyyyMMdd
         string sql = @"
SELECT MAX(AI2_YMD) as maxDate
FROM CI.AI2
WHERE AI2_YMD >= :startDate
AND AI2_YMD <= :endDate
AND AI2_PROD_TYPE = 'F'
AND AI2_PROD_SUBTYPE = 'S'
AND AI2_SUM_TYPE = 'D' 
AND AI2_SUM_SUBTYPE = '2'
";

         string res = db.ExecuteScalar(sql , CommandType.Text , parms);
         return res;
      }
   }
}
