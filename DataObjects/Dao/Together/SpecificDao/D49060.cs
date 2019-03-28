using BusinessObjects;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// Winni, 2019/3/19
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49060 : DataGate {
      /// <summary>
      /// get CI.MGT8,CI.MG8 data return 12 feild (d49060_txt)
      /// </summary>
      /// <param name="dateYmd"></param>
      /// <param name="fId">MG8_F_ID</param>
      /// <returns></returns>
      public DataTable GetTxtDataById(string dateYmd , string fId) {
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
           MG8_F_ID,
           MG8_EFFECT_YMD,
          ROW_NUMBER( ) OVER (PARTITION BY MG8_F_ID ORDER BY MG8_F_ID,MG8_EFFECT_YMD DESC NULLS LAST) AS MG8_SEQ_NO,
          LEAD(MG8_CM) OVER (PARTITION BY MG8_F_ID ORDER BY MG8_F_ID,MG8_EFFECT_YMD DESC NULLS LAST) AS MG8_PREV_CM,
          LEAD(MG8_MM) OVER (PARTITION BY MG8_F_ID ORDER BY MG8_F_ID,MG8_EFFECT_YMD DESC NULLS LAST) AS MG8_PREV_MM,
          LEAD(MG8_IM) OVER (PARTITION BY MG8_F_ID ORDER BY MG8_F_ID,MG8_EFFECT_YMD DESC NULLS LAST) AS MG8_PREV_IM,
          MG8_CM,
          MG8_MM,
          MG8_IM 
       FROM CI.MG8 
       WHERE TRIM(MG8_EFFECT_YMD) <= :dateYmd
      AND TRIM(MG8_F_ID) = :fId
       ORDER BY MG8_F_ID,MG8_EFFECT_YMD),
      (SELECT 
            COD_ID AS COD_CURRENCY_TYPE,
            TRIM(COD_DESC) AS COD_CURRENCY_NAME
       FROM CI.COD
       WHERE COD_TXN_ID = 'MGT8' 
       AND COD_COL_ID = 'MGT8_CURRENCY_TYPE')
WHERE TRIM(MG8_F_ID) = TRIM(MGT8_F_ID)      
AND MG8_SEQ_NO <= 2
AND TRIM(MGT8_CURRENCY_TYPE) = TRIM(COD_CURRENCY_TYPE)
ORDER BY MG8_EFFECT_YMD DESC                   
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="DATA_EFFECT_YMD">yyyyMMdd</param>
      /// <param name="DATA_F_ID"></param>
      /// <param name="DATA_TRADE_TYPE">I:insert</param>
      /// <returns></returns>
      public string ExecuteStoredProcedure(string DATA_EFFECT_YMD , string DATA_F_ID , string DATA_TRADE_TYPE) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("DATA_EFFECT_YMD",DATA_EFFECT_YMD),
            new DbParameterEx("DATA_F_ID",DATA_F_ID),
            new DbParameterEx("DATA_TRADE_TYPE",DATA_TRADE_TYPE)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_STT_MG8D";

         return db.ExecuteStoredProcedureReturnString(sql , parms , true , OracleDbType.Int32);
      }
   }
}
