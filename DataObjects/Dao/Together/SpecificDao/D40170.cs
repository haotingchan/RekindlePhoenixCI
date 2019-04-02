using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/4/1
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 40170 選擇權原始資料下載
   /// </summary>
   public class D40170 : DataGate {

      /// <summary>
      /// get CI.RPT/CI.APDK (dddw_40170_kind_id) 第一行加入空白
      /// return SORT_SEQ_NO/RPT_KEY/RPT_NAME/CP_DISPLAY 4 fields 
      /// </summary>
      /// <returns></returns>
      public DataTable GetDwList() {

         string sql = @"
SELECT 
    T.SORT_SEQ_NO,
    T.RPT_KEY,
    T.RPT_NAME,
    (TRIM(T.RPT_KEY)||' - '||TRIM(T.RPT_NAME)) AS CP_DISPLAY
FROM
    (
        SELECT 
            0 AS SORT_SEQ_NO,
            '%' AS RPT_KEY,
            '全部' AS RPT_NAME
        FROM DUAL
        UNION
          SELECT 
                RPT_SEQ_NO,   
                RPT_VALUE,    
                APDK_NAME  
          FROM CI.RPT ,CI.APDK
          WHERE RPT_TXD_ID = '40171'    
          AND TRIM(RPT_VALUE) = TRIM(APDK_KIND_ID)
    ) T      
UNION 
    SELECT 
         -1,' ',' ',' ' FROM DUAL
    ORDER BY SORT_SEQ_NO
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// get CI.AOCF data 取得資料起始日
      /// return START_YMD (yyyyMMdd)
      /// </summary>
      /// <param name="endYmd">yyyyMMdd</param>
      /// <param name="aocfYmd">yyyyMMdd</param>
      /// <param name="days"></param>
      /// <returns></returns>
      public string GetStartDate(string endYmd , string aocfYmd , decimal days) {

         object[] parms =
        {
                ":endYmd", endYmd,
                ":aocfYmd", aocfYmd,
                ":days", days
            };


         string sql = @"
SELECT OCF_YMD AS START_YMD
FROM(
        SELECT OCF_YMD,DAY_NUM 
        FROM
                (SELECT OCF_YMD,
                                 ROW_NUMBER() OVER (ORDER BY OCF_YMD DESC) AS DAY_NUM
                  FROM CI.AOCF
                  WHERE OCF_YMD <= :endYmd
                  AND OCF_YMD >= :aocfYmd)
       WHERE DAY_NUM = :days
  )
";
         return db.ExecuteScalar(sql , CommandType.Text , parms);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="modelType"></param>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <param name="kindId"></param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure(string modelType , string startDate , string endDate , string kindId) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("modelType",modelType),
            new DbParameterEx("startDate",startDate),
            new DbParameterEx("endDate",endDate),
            new DbParameterEx("kindId",kindId)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_40170_DETL";

         DataTable bb = db.ExecuteStoredProcedureEx(sql , parms , true);

         return db.ExecuteStoredProcedureEx(sql , parms , true);
      }
   }
}
