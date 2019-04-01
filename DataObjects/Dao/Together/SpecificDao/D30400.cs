using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/3/28
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 30400 股票期貨價量統計表
   /// </summary>
   public class D30400 : DataGate {

      /// <summary>
      /// get CI.AI2 data (d_30401) 
      /// return AI2_YMD/AI2_PROD_SUBTYPE/AI2_PC_CODE/AI2_M_QNTY/AI2_OI/AI2_MMK_QNTY/CP_SUM_AI2_OI/CP_SUM_AI2_MMK_QNTY 6 field
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="prodType">F</param>
      /// <returns></returns>
      public DataTable Get30401Data(string startDate , string endDate , string prodType) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate,
                ":prodType", prodType
            };

         string sql = @"
SELECT 
	AI2_YMD,
	AI2_PROD_SUBTYPE,
	AI2_PC_CODE,
	AI2_M_QNTY,
	AI2_OI,

	AI2_MMK_QNTY,
	SUM( AI2_OI ) OVER( PARTITION BY T.AI2_YMD ) AS CP_SUM_AI2_OI,
	SUM( AI2_MMK_QNTY ) OVER( PARTITION BY T.AI2_YMD ) AS CP_SUM_AI2_MMK_QNTY
FROM(
	SELECT 
		AI2_YMD,   
		AI2_PROD_SUBTYPE,   
		AI2_PC_CODE,   
		SUM(AI2_M_QNTY) AS AI2_M_QNTY,   
		SUM(AI2_OI) AS AI2_OI,   
		SUM(AI2_MMK_QNTY) AS AI2_MMK_QNTY  
	FROM CI.AI2 
	WHERE AI2_SUM_TYPE = 'D'  
	AND AI2_YMD >= :startdate  
	AND AI2_YMD <= :enddate  
	AND AI2_SUM_SUBTYPE = '5'  
	AND AI2_PROD_TYPE = :prodtype 
	AND AI2_PROD_SUBTYPE = 'S' 
	AND AI2_PARAM_KEY <> 'STO'
	GROUP BY AI2_YMD, AI2_PROD_SUBTYPE, AI2_PC_CODE   
	ORDER BY AI2_YMD  )T
";

         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }

      /// <summary>
      /// get CI.AI2/CI.APDK data (d_30402)
      /// return KIND_ID_2/M_QNTY/APDK_NAME 3 field
      /// </summary>
      /// <param name="startMon">yyyyMM</param>
      /// <param name="prodType">F</param>
      /// <returns></returns>
      public DataTable Get30402Data(string startMon , string prodType) {

         object[] parms = {
                ":startMon", startMon,
                ":prodType", prodType
            };

         string sql = @"
SELECT 
      I.AI2_KIND_ID_2 AS KIND_ID_2,                      
      I.AI2_M_QNTY AS M_QNTY ,                           
      P.APDK_NAME  AS PDK_NAME                           
FROM                                                    
      (SELECT 
         SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID_2,     
         SUM(AI2_M_QNTY) AS AI2_M_QNTY                
      FROM CI.AI2                                      
      WHERE AI2_SUM_TYPE = 'M'  
      AND TRIM(AI2_YMD) = :startMon
      AND AI2_SUM_SUBTYPE = '4' 
      AND AI2_PROD_SUBTYPE = 'S' 
      AND AI2_PROD_TYPE = :prodType               
      GROUP BY SUBSTR(AI2_KIND_ID,1,2)) I,                  
      (SELECT 
         SUBSTR(APDK_KIND_ID,1,2) AS APDK_KIND_ID_2,  
         MIN(APDK_NAME) AS APDK_NAME                 
      FROM CI.APDK                                     
      WHERE APDK_PROD_TYPE = 'F'                             
      AND APDK_PROD_SUBTYPE = 'S'                      
      GROUP BY SUBSTR(APDK_KIND_ID,1,2)) P                 
WHERE I.AI2_KIND_ID_2 = P.APDK_KIND_ID_2
ORDER BY KIND_ID_2
";
         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }

      /// <summary>
      /// get CI.AI2 data (wf_30361 抓當月最後交易日) return LAST_DATE
      /// PB備註(wf_30361取idt_end_month) 且30351,30361,30366相同 之後看是否放共用?
      /// </summary>
      /// <param name="thisMon">yyyyMM</param>
      /// <returns></returns>
      public string GetThisMonLastTradeData(string thisMon) {

         object[] parms = {
                ":thisMon", thisMon
            };

         string sql = @"
SELECT MAX(AI2_YMD) AS LAST_DATE
FROM CI.AI2
WHERE (AI2_PARAM_KEY LIKE 'STC%' OR AI2_PARAM_KEY LIKE 'STO%')
AND AI2_SUM_TYPE = 'D'
AND SUBSTR(AI2_YMD,1,6) = :thisMon
";
         return db.ExecuteScalar(sql , CommandType.Text , parms);
      }

      /// <summary>
      /// get CI.AI2 data (d_30403)
      /// return ai2_ymd/ai2_prod_subtype/ai2_pc_code/ai2_m_qnty/ai2_oi/ai2_mmk_qnty 6 field
      /// </summary>
      /// <param name="startDate">當月第一天(yyyyMM01)</param>
      /// <param name="endDate">當月最後一天交易日(yyyyMMdd)</param>
      /// <returns></returns>
      public DataTable Get30403Data(string startDate , string endDate) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate
            };

         string sql = @"
SELECT 
	SEQ_NO,
	PDK_NAME,
	A.AI2_KIND_ID_2,
	A.AI2_YMD,
	A.AI2_PC_CODE,
	A.AI2_M_QNTY
FROM
   --日期
   (SELECT 
	   	AI2_YMD, 
	   	ROWNUM AS SEQ_NO
    FROM
      (SELECT AI2_YMD
       FROM CI.AI2  
       WHERE AI2_SUM_TYPE = 'D'   
       AND AI2_YMD >= :startDate 
       AND AI2_YMD <= :endDate  
       AND AI2_SUM_SUBTYPE = '5' 
       AND AI2.AI2_PROD_TYPE = 'F' 
       AND AI2.AI2_PROD_SUBTYPE = 'S' 
       GROUP BY AI2_YMD
       ORDER BY AI2_YMD)
      ) D,                
   --成交量
   (SELECT 
   		AI2_YMD,
        SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID_2,
        AI2_PC_CODE,
    	SUM(AI2_M_QNTY) AS AI2_M_QNTY 
    FROM CI.AI2
    WHERE AI2_SUM_TYPE = 'D'   
    AND AI2_YMD >= :startDate  
    AND AI2_YMD <= :endDate  
    AND AI2_SUM_SUBTYPE = '5' 
    AND AI2.AI2_PROD_TYPE = 'F' 
    AND AI2.AI2_PROD_SUBTYPE = 'S' 
    GROUP BY 
	    AI2_YMD,
	    SUBSTR(AI2_KIND_ID,1,2),
	    AI2_PC_CODE
   ) A,
   --商品檔
   (SELECT 
	   	SUBSTR(APDK.APDK_KIND_ID,1,2) AS APDK_KIND_ID_2,
	    MIN(APDK.APDK_NAME) AS PDK_NAME 
    FROM CI.APDK APDK
    WHERE APDK_PROD_TYPE = 'F'
    GROUP BY SUBSTR(APDK.APDK_KIND_ID,1,2)
    ORDER BY SUBSTR(APDK.APDK_KIND_ID,1,2)
   ) P
WHERE A.AI2_KIND_ID_2 = P.APDK_KIND_ID_2 
AND A.AI2_YMD = D.AI2_YMD  
ORDER BY AI2_KIND_ID_2 , AI2_YMD , AI2_PC_CODE
";

         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }

      /// <summary>
      /// get CI.AI3 data (d_30404) return AI3_DATE/AI3_CLOSE_PRICE/AI3_INDEX/AI3_AMOUNT/AI3_M_QNTY/AI3_OI 6 field
      /// </summary>
      /// <param name="kindId">%=全部</param>
      /// <param name="startDate">yyyy/MM/dd</param>
      /// <param name="endDate">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable Get30404Data(string kindId , string startDate , string endDate) {

         object[] parms = {
                ":kindId", kindId,
                ":startDate", startDate,
                ":endDate", endDate
            };

         string sql = @"
SELECT 
    AI3_DATE,   
    AI3_CLOSE_PRICE,  
    AI3_INDEX,   
    AI3_AMOUNT,
    AI3_M_QNTY,
    AI3_OI  
FROM CI.AI3  
WHERE AI3_KIND_ID LIKE :kindId
AND AI3_DATE >= TO_DATE(:startDate,'yyyy/mm/dd')  
AND AI3_DATE <= TO_DATE(:endDate ,'yyyy/mm/dd')  
ORDER BY AI3_DATE
";
         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }

      /// <summary>
      /// get CI.APDK data (wf_30611 抓取商品名稱)
      /// return APDK_NAME/APDK_STOCK_ID 2 field
      /// </summary>
      /// <param name="strKindId">%=全部</param>
      /// <returns></returns>
      public DataTable GetAdpkData(string strKindId) {

         object[] parms = {
                ":strKindId", strKindId
            };

         string sql = @"
select 
    APDK_NAME,
    APDK_STOCK_ID 
from ci.APDK
where TRIM(APDK_KIND_ID) like :strKindId
";

         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }

      /// <summary>
      /// get CI.AI2/CI.AM9/CI.AM10/CI.AB4 data (d_30405) 
      /// return AI2_YMD/AI2_M_QNTY/AI2_OI/AM10_CNT/AM9_ACC_CNT/AB4_ID_CNT 6 field
      /// </summary>
      /// <param name="startDate">當月第1天(yyyyMMdd)</param>
      /// <param name="endDate">當月最後1天(yyyyMMdd)</param>
      /// <returns></returns>
      public DataTable Get30405Data(string startDate , string endDate) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate
            };

         string sql = @"
SELECT 
	AI2_YMD,
	AI2_M_QNTY,
	AI2_OI,
	NVL(AM10_CNT,0) AM10_CNT,
	NVL(AM9_ACC_CNT,0) AM9_ACC_CNT,
	NVL(AB4_ID_CNT,0) AB4_ID_CNT
FROM CI.AI2,
     (SELECT 
     	AM9_YMD,
     	AM9_ACC_CNT 
      FROM CI.AM9 
      WHERE AM9_PROD_TYPE = 'F' 
      AND AM9_PROD_SUBTYPE = 'S' 
      AND AM9_PARAM_KEY = '999' 
      AND AM9_KIND_ID2 = '999' 
      AND AM9_YMD >= :startDate 
      AND AM9_YMD <= :endDate
     ) AM9,
     (SELECT 
     	AM10_YMD,
     	SUM(AM10_CNT) AS AM10_CNT 
      FROM CI.AM10
      WHERE AM10_PROD_TYPE = 'F' 
      AND AM10_PROD_SUBTYPE = 'S' 
      AND AM10_YMD >= :startDate 
      AND AM10_YMD <= :endDate 
      GROUP BY AM10_YMD
     ) AM10,
     (SELECT 
     	TO_CHAR(AB4_DATE,'YYYYMMDD') AS AB4_YMD,
     	AB4_ID_CNT 
      FROM CI.AB4
      WHERE AB4_PROD_TYPE = 'F' 
      AND AB4_PARAM_KEY = 'STF' 
      AND AB4_KIND_ID = '999' 
      AND AB4_DATE >= TO_DATE(:startDate,'YYYYMMDD') 
      AND AB4_DATE <= TO_DATE(:endDate,'YYYYMMDD')
     ) AB4
WHERE AI2_YMD >= :startDate
  AND AI2_YMD <= :endDate
  AND AI2_SUM_TYPE = 'D'
  AND AI2_SUM_SUBTYPE = '2'
  AND AI2_PROD_TYPE = 'F'
  AND AI2_PROD_SUBTYPE = 'S'
  AND AI2_YMD = AM9_YMD(+)
  AND AI2_YMD = AM10_YMD(+)
  AND AI2_YMD = AB4_YMD(+)
ORDER BY AI2_YMD
";
         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }

      /// <summary>
      /// get CI.AMIF data (d_30406) return AMIF_DATE/SEQ_NO/AMIF_SETTLE_DATE/AMIF_M_QNTY_TAL/AMIF_OPEN_INTEREST 5 field
      /// </summary>
      /// <param name="startDate">yyyy/MM/dd</param>
      /// <param name="endDate">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable Get30406Data(string startDate , string endDate) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate
            };

         string sql = @"
SELECT 
    AMIF_DATE,
    D.SEQ_NO,
    A.AMIF_SETTLE_DATE,
    AMIF_M_QNTY_TAL,
    AMIF_OPEN_INTEREST 
FROM
   (SELECT 
        AMIF_DATE,
        AMIF_SETTLE_DATE,
        SUM(AMIF_M_QNTY_TAL) AMIF_M_QNTY_TAL,
        SUM(AMIF_OPEN_INTEREST) AMIF_OPEN_INTEREST 
    FROM CI.AMIF A
    WHERE AMIF_DATE >= TO_DATE(:startDate,'yyyy/mm/dd')
    AND AMIF_DATE <= TO_DATE(:endDate,'yyyy/mm/dd')
    AND AMIF_DATA_SOURCE  IN ('T','P')
    AND AMIF_PROD_TYPE = 'F'
    AND AMIF_PROD_SUBTYPE = 'S'
    GROUP BY AMIF_DATE,AMIF_SETTLE_DATE
   ) A,
   (SELECT 
           AMIF_SETTLE_DATE,
           ROWNUM AS SEQ_NO
    FROM
       (SELECT AMIF_SETTLE_DATE 
        FROM CI.AMIF
        WHERE AMIF_DATE >= TO_DATE(:startDate,'yyyy/mm/dd')
        AND AMIF_DATE <= TO_DATE(:endDate,'yyyy/mm/dd')
        AND AMIF_DATA_SOURCE  IN ('T','P')
        AND AMIF_PROD_TYPE = 'F'
        AND AMIF_PROD_SUBTYPE = 'S'
        GROUP BY AMIF_SETTLE_DATE
        ORDER BY AMIF_SETTLE_DATE)
       ) D
WHERE A.AMIF_SETTLE_DATE = D.AMIF_SETTLE_DATE
ORDER BY AMIF_DATE , AMIF_SETTLE_DATE
";
         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }
   }
}
