using System;
using System.Data;
/// <summary>
/// Winni, 2019/3/25
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49061 : DataGate {
      /// <summary>
      /// get CI.MGT8,CI.MG8 data return 12 feild (d49060_txt)
      /// </summary>
      /// <param name="dateYmd"></param>
      /// <param name="fId">MG8_F_ID</param>
      /// <returns></returns>
      public DataTable GetDataById(string dateYmd , string fId) {
         object[] parms =
            {
                ":dateYmd", dateYmd,
                ":fId", fId
            };

         string sql = @"
SELECT 
    MGT8_F_EXCHANGE,
    MGT8_F_NAME,
    MG8_EFFECT_YMD,
    CASE WHEN NVL(MG8_CM,0) = 0 THEN NULL ELSE MG8_CM END AS MG8_CM,
    CASE WHEN NVL(MG8_PREV_CM,0) = 0 THEN NULL ELSE ROUND((MG8_CM - MG8_PREV_CM) / MG8_PREV_CM ,4) END AS CM_RATE,

    CASE WHEN NVL(MG8_MM,0) = 0 THEN NULL ELSE MG8_MM END AS MG8_MM,
    CASE WHEN NVL(MG8_PREV_MM,0) = 0  THEN NULL ELSE ROUND((MG8_MM - MG8_PREV_MM) / MG8_PREV_MM ,4) END AS MM_RATE,
    CASE WHEN NVL(MG8_IM,0) = 0 THEN NULL ELSE MG8_IM END AS MG8_IM,
    CASE WHEN NVL(MG8_PREV_IM,0) = 0  THEN NULL ELSE ROUND((MG8_IM - MG8_PREV_IM) / MG8_PREV_IM  ,4) END AS IM_RATE,
    MGT8_AMT_TYPE,

    MGT8_CURRENCY_TYPE,
    COD_CURRENCY_NAME
FROM CI.MGT8,
       --保證金
      (SELECT 
            MG8_F_ID,MG8_EFFECT_YMD,
          ROW_NUMBER( ) OVER (PARTITION BY MG8_F_ID ORDER BY MG8_F_ID,MG8_EFFECT_YMD DESC NULLS LAST) AS MG8_SEQ_NO,
          LEAD(MG8_CM) OVER (PARTITION BY MG8_F_ID ORDER BY MG8_F_ID,MG8_EFFECT_YMD DESC NULLS LAST) AS MG8_PREV_CM,
          LEAD(MG8_MM) OVER (PARTITION BY MG8_F_ID ORDER BY MG8_F_ID,MG8_EFFECT_YMD DESC NULLS LAST) AS MG8_PREV_MM,
          LEAD(MG8_IM) OVER (PARTITION BY MG8_F_ID ORDER BY MG8_F_ID,MG8_EFFECT_YMD DESC NULLS LAST) AS MG8_PREV_IM,
          MG8_CM,
          MG8_MM,
          MG8_IM 
       FROM CI.MG8 
       WHERE MG8_EFFECT_YMD <= :dateYmd
      AND MG8_F_ID = :fId
       ORDER BY MG8_F_ID,MG8_EFFECT_YMD),
      (SELECT 
            COD_ID AS COD_CURRENCY_TYPE,
            TRIM(COD_DESC) AS COD_CURRENCY_NAME
       FROM CI.COD
       WHERE COD_TXN_ID = 'MGT8' 
       AND COD_COL_ID = 'MGT8_CURRENCY_TYPE')
WHERE MG8_F_ID = MGT8_F_ID      
AND MG8_SEQ_NO <= 2
AND MGT8_CURRENCY_TYPE = COD_CURRENCY_TYPE
ORDER BY MG8_EFFECT_YMD DESC               
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
