using OnePiece;
using System;
using System.Data;

/// <summary>
/// Winni 2019/01/22
/// </summary>
namespace DataObjects.Dao.Together {
   /// <summary>
   /// AI6券商資訊?
   /// </summary>
   public class D30620 {
      private Db db;

      public D30620() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// Get 各商品每月平均振幅明細表 by CI.AI6, CI.RPT  (AVG_AI6/AVG_TFXM)
      /// </summary>
      /// <param name="as_sdate">查詢起始月</param>
      /// <param name="as_edate">查詢結束月</param>
      /// <returns></returns>
      public DataTable GetListAavg(DateTime as_sdate , DateTime as_edate) {
         object[] parms =
         {
                ":as_sdate", as_sdate,
                ":as_edate", as_edate
            };

         string sql = @"
SELECT AI6_YM, AI6_KIND_ID, AVG_AI6, AVG_TFXM, RPT_LEVEL_1, RPT_LEVEL_2
FROM
   (SELECT TO_CHAR(AI6_DATE,'YYYYMM') AS AI6_YM,
   AI6_KIND_ID,
   ROUND(AVG(AI6_HIGH_LOW),2) AS AVG_AI6,
   ROUND(AVG(AI6_TFXM_HIGH_LOW),2) AS AVG_TFXM
FROM CI.AI6
WHERE AI6_DATE >= :as_sdate
AND AI6_DATE <= :as_edate
GROUP BY TO_CHAR(AI6_DATE,'YYYYMM'),AI6_KIND_ID),CI.RPT
WHERE RPT_TXD_ID = '30621'
AND AI6_KIND_ID = RPT_VALUE
ORDER BY AI6_YM , RPT_LEVEL_1 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Get 各商品每月年化波動度明細表 by CI.AI6, CI.RPT  (AI6_RATE/TFXM_RATE)
      /// </summary>
      /// <param name="as_sdate">查詢起始月</param>
      /// <param name="as_edate">查詢結束月</param>
      /// <returns></returns>
      public DataTable GetListVol(DateTime as_sdate , DateTime as_edate) {
         object[] parms =
         {
                ":as_sdate", as_sdate,
                ":as_edate", as_edate
            };

         string sql = @"
SELECT AI6_YM,AI6_KIND_ID,AI6_RATE,TFXM_RATE,RPT_LEVEL_1,RPT_LEVEL_2
FROM
	(SELECT TO_CHAR(AI6_DATE,'YYYYMM') AS AI6_YM,AI6_KIND_ID,
    NVL(ROUND(STDDEV(AI6_LN_RETURN) * SQRT(256),3),0) AS AI6_RATE,
    NVL(ROUND(STDDEV(AI6_TFXM_LN_RETURN) * SQRT(256),3),0) AS TFXM_RATE
FROM CI.AI6
WHERE AI6_DATE >= :as_sdate
AND AI6_DATE <= :as_edate
GROUP BY TO_CHAR(AI6_DATE,'YYYYMM'),AI6_KIND_ID),CI.RPT
WHERE RPT_TXD_ID = '30621'
AND AI6_KIND_ID = RPT_VALUE
ORDER BY AI6_YM , RPT_LEVEL_1 , RPT_LEVEL_2 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
