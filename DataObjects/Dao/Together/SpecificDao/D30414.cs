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
   public class D30414 {

      private Db db;

      public D30414() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// get CI.AI2/CI.AM9/CI.AM10 data (D30414)
      /// return I.AI2_YMD/AI2_M_QNTY/AI2_OI/AI2_DAY_COUNT/AM9_ACC_CNT/AM10_CNT 6 feilds
      /// </summary>
      /// <param name="startMon">yyyyMM</param>
      /// <param name="endMon">yyyyMM</param>
      /// <returns></returns>
      public DataTable ListData(string startMon , string endMon) {

         object[] parms = {
                ":startMon", startMon,
                ":endMon", endMon
            };

         string sql = @"
SELECT 
    I.AI2_YMD,
    AI2_M_QNTY,
    AI2_OI,
    AI2_DAY_COUNT,
    AM9_ACC_CNT,
    AM10_CNT 
FROM     
    (SELECT 
        AI2_YMD,
        SUM(AI2_M_QNTY) AS AI2_M_QNTY,
        MAX(AI2_DAY_COUNT) AS AI2_DAY_COUNT 
     FROM CI.AI2
     WHERE AI2_YMD >= :startMon
       AND AI2_YMD <= :endMon
       AND AI2_SUM_TYPE = 'M'
       AND AI2_SUM_SUBTYPE = '2'
       AND AI2_PROD_TYPE = 'F'
       AND AI2_PROD_SUBTYPE = 'S'
     GROUP BY AI2_YMD) I,
    (SELECT 
        SUBSTR(AI2_YMD,1,6)||'  ' AS AI2_YMD,
        SUM(AI2_OI) AS AI2_OI 
     FROM CI.AI2
     WHERE AI2_YMD >= :startMon||'01'
       AND AI2_YMD <= :endMon||'31'
       AND AI2_SUM_TYPE = 'D'
       AND AI2_SUM_SUBTYPE = '2'
       AND AI2_PROD_TYPE = 'F'
       AND AI2_PROD_SUBTYPE = 'S'
     GROUP BY SUBSTR(AI2_YMD,1,6)) OI,
    (SELECT 
        SUBSTR(AM9_YMD,1,6)||'  ' AS AM9_YMD ,
        SUM(AM9_ACC_CNT) AS AM9_ACC_CNT
     FROM CI.AM9
     WHERE AM9_YMD >= :startMon||'01'
     AND AM9_YMD <= :endMon||'31'
     AND AM9_PROD_TYPE = 'F'
     AND AM9_PROD_SUBTYPE = 'S'
     GROUP BY SUBSTR(AM9_YMD,1,6) ) M,
    (SELECT 
        SUBSTR(AM10_YMD,1,6)||'  ' AS AM10_YMD ,
        SUM(AM10_CNT) AS AM10_CNT
     FROM CI.AM10
     WHERE AM10_YMD >= :startMon||'01'
     AND AM10_YMD <= :endMon||'31'
     AND AM10_PROD_TYPE = 'F'
     AND AM10_PARAM_KEY = 'STF'
     GROUP BY SUBSTR(AM10_YMD,1,6) ) C
WHERE I.AI2_YMD = AM9_YMD(+)
AND I.AI2_YMD = OI.AI2_YMD(+)
AND I.AI2_YMD = C.AM10_YMD(+)
ORDER BY AI2_YMD 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.AI2 data return AI2_SUM_TYPE/AI2_YMD/SEQ_NO 3 feilds (d30414_ym)
      /// </summary>
      /// <param name="startMon">yyyyMM</param>
      /// <param name="endMon">yyyyMM</param>
      /// <returns></returns>
      public DataTable ListTitleByMon(string startMon , string endMon) {

         object[] parms = {
                ":startMon", startMon,
                ":endMon", endMon
            };

         string sql = @"
SELECT 
  AI2_SUM_TYPE,
  AI2_YMD,
  ROWNUM AS SEQ_NO
FROM
  (SELECT 
    CASE WHEN AI2_M = '99' THEN 'Y' ELSE 'M' END AS AI2_SUM_TYPE,
    CASE WHEN AI2_M = '99' THEN AI2_Y || ' ' ELSE AI2_Y || AI2_M END AS AI2_YMD
   FROM 
      (SELECT SUBSTR(AI2_YMD,1,4) AS AI2_Y
         FROM CI.AI2
        WHERE AI2_YMD >= :startMon
          AND AI2_YMD <= :endMon
          AND AI2_SUM_TYPE = 'M'
          AND AI2_SUM_SUBTYPE = '1'
          AND AI2_PROD_TYPE = 'F'
        GROUP BY SUBSTR(AI2_YMD,1,4)),
      (SELECT '01' AS AI2_M FROM DUAL UNION ALL
       SELECT '02' FROM DUAL UNION ALL
       SELECT '03' FROM DUAL UNION ALL
       SELECT '04' FROM DUAL UNION ALL
       SELECT '05' FROM DUAL UNION ALL
       SELECT '06' FROM DUAL UNION ALL
       SELECT '07' FROM DUAL UNION ALL
       SELECT '08' FROM DUAL UNION ALL
       SELECT '09' FROM DUAL UNION ALL
       SELECT '10' FROM DUAL UNION ALL
       SELECT '11' FROM DUAL UNION ALL
       SELECT '12' FROM DUAL UNION ALL
       SELECT '99' FROM DUAL)
   WHERE AI2_Y || AI2_M >= :startMon  
   ORDER BY AI2_Y,1,2)   
ORDER BY SEQ_NO
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.AI2,CI.APDK data return AI2_KIND_ID/APDK_NAME 2 feilds (d30415_kind_id)
      /// </summary>
      /// <param name="startMon">yyyyMM</param>
      /// <param name="endMon">yyyyMM</param>
      /// <returns></returns>
      public DataTable ListProdByMon(string startMon , string endMon) {

         object[] parms = {
                ":startMon", startMon,
                ":endMon", endMon
            };

         string sql = @"
SELECT 
	AI2_KIND_ID,
	APDK_NAME
FROM
	(SELECT 
		SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID,
		SUBSTR(AI2_KIND_ID,1,2)||'F' AS AI2_KIND_ID_F
     FROM CI.AI2
     WHERE TRIM(AI2_YMD) >= :startMon
     AND TRIM(AI2_YMD) <= :endMon
     AND AI2_SUM_TYPE = 'M'
     AND AI2_SUM_SUBTYPE = '4'
     AND AI2_PROD_TYPE = 'F'
     AND AI2_PROD_SUBTYPE = 'S'
     GROUP BY SUBSTR(AI2_KIND_ID,1,2)),
 	CI.APDK
WHERE  AI2_KIND_ID_F = TRIM(APDK_KIND_ID(+))
ORDER BY AI2_KIND_ID 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.AI2 data return AI2_YMD/AI2_KIND_ID/AI2_M_QNTY/AI2_DAY_COUNT 4 feilds (d30415)
      /// </summary>
      /// <param name="startMon">yyyyMM</param>
      /// <param name="endMon">yyyyMM</param>
      /// <returns></returns>
      public DataTable ListDataByMon(string startMon , string endMon) {

         object[] parms = {
                ":startMon", startMon,
                ":endMon", endMon
            };

         string sql = @"
SELECT 
  AI2_YMD,
  AI2_KIND_ID,
  AI2_M_QNTY,
  AI2_OI,
  AI2_DAY_COUNT,
  CASE AI2_DAY_COUNT WHEN 0 THEN 0 ELSE ROUND(AI2_M_QNTY / AI2_DAY_COUNT , 0) END AS AVG_M_QNTY,
  CASE AI2_DAY_COUNT  WHEN 0 THEN 0 ELSE ROUND(AI2_OI / AI2_DAY_COUNT,0) END AS AVG_OI
FROM(

  SELECT 
    M.AI2_YMD,
    M.AI2_KIND_ID,
    AI2_M_QNTY,
    AI2_OI,
    DT.AI2_DAY_COUNT AS AI2_DAY_COUNT
  FROM
    (SELECT 
      AI2_YMD AS AI2_YMD,
      SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID,
      SUM(AI2_M_QNTY) AS AI2_M_QNTY,
      MAX(AI2_DAY_COUNT) AS AI2_DAY_COUNT 
     FROM CI.AI2
     WHERE AI2_YMD >= :startMon
     AND AI2_YMD <= :endMon
     AND AI2_SUM_TYPE = 'M'
     AND AI2_SUM_SUBTYPE = '4'
     AND AI2_PROD_TYPE = 'F'
     AND AI2_PROD_SUBTYPE = 'S'
     GROUP BY AI2_YMD,SUBSTR(AI2_KIND_ID,1,2)) M,
    (SELECT 
      SUBSTR(AI2_YMD,1,6)||'  ' AS AI2_YMD,
      SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID,
      SUM(AI2_OI) AS AI2_OI
     FROM CI.AI2 
     WHERE AI2_YMD >= :startMon||'01'
     AND AI2_YMD <= :endMon||'31'
     AND AI2_SUM_TYPE = 'D'
     AND AI2_SUM_SUBTYPE = '4'
     AND AI2_PROD_TYPE = 'F'
     AND AI2_PROD_SUBTYPE = 'S'
     GROUP BY SUBSTR(AI2_YMD,1,6),SUBSTR(AI2_KIND_ID,1,2)) OI,
    (SELECT 
      AI2_YMD,
      MAX(AI2_DAY_COUNT) AS AI2_DAY_COUNT 
     FROM CI.AI2
     WHERE AI2_YMD >= :startMon
     AND AI2_YMD <= :endMon
     AND AI2_SUM_TYPE = 'M'
     AND AI2_SUM_SUBTYPE = '2'
     AND AI2_PROD_TYPE = 'F'
     AND AI2_PROD_SUBTYPE = 'S'
     GROUP BY AI2_YMD) DT
  WHERE M.AI2_YMD = OI.AI2_YMD
  AND M.AI2_KIND_ID = OI.AI2_KIND_ID
  AND M.AI2_YMD = DT.AI2_YMD
  )
ORDER BY AI2_KIND_ID, AI2_YMD
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
