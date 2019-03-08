using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D30670 : DataGate {

      /// <summary>
      /// get data by CI.AA2,CI.HEXRT,CI.APDK,CI.RPT 
      /// (AA2_KIND_ID/SUM_AMT/SUM_AMT_ORG/SUM_AMT_USD/RPT_COL/RPT_ROW)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="kindName"></param>
      /// <returns></returns>
      public DataTable d30670_AA2_AMT(string startDate , string endDate , string kindName) {

         object[] parms =
         {
                ":startDate", startDate,
                ":endDate", endDate,
                ":kindName", kindName
            };

         string sql = @"
SELECT AA2_KIND_ID,
SUM_AMT,
SUM_AMT_ORG,
SUM_AMT_USD,
RPT_COL,
RPT_ROW
FROM 
    (SELECT AA2_KIND_ID,
            --台幣金額
            SUM(AA2_AMT)/2 AS SUM_AMT,
            --原始幣別金額
            SUM(AA2_AMT_ORG_CURRENCY)/2 AS SUM_AMT_ORG,
            --美元金額
            SUM(CASE WHEN NVL(HEXRT_EXCHANGE_RATE,0) = 0 THEN NULL ELSE ROUND(AA2_AMT_ORG_CURRENCY * HEXRT_EXCHANGE_RATE,4) END)/2 AS SUM_AMT_USD
    FROM CI.AA2,
         --每日外幣對美元匯率資料
         (SELECT 
          APDK_KIND_ID,
          TO_CHAR(HEXRT_DATE,'YYYYMMDD') AS HEXRT_YMD,HEXRT_CURRENCY_TYPE,HEXRT_EXCHANGE_RATE
          FROM CI.HEXRT,CI.APDK
          WHERE HEXRT_DATE >= TO_DATE(:startDate,'YYYYMMDD')
          AND HEXRT_DATE <= TO_DATE(:endDate,'YYYYMMDD')
          AND HEXRT_COUNT_CURRENCY = '2'
          AND APDK_CURRENCY_TYPE = HEXRT_CURRENCY_TYPE)
    WHERE AA2_YMD >= :startDate
    AND AA2_YMD <= :endDate
    AND AA2_KIND_ID IN (SELECT RPT_VALUE
                        FROM CI.RPT
                        WHERE RPT_TXN_ID = '30670'
                        AND RPT_TXD_ID = '30670_AMT'
                        AND RPT_VALUE_2 LIKE :kindName) 
    AND AA2_YMD = HEXRT_YMD(+)
    AND AA2_KIND_ID = APDK_KIND_ID(+)
    GROUP BY AA2_KIND_ID),
         --報表檔
         (SELECT RPT_VALUE AS RPT_KIND_ID,RPT_LEVEL_1 AS RPT_COL,RPT_LEVEL_3 AS RPT_ROW
          FROM CI.RPT
          WHERE RPT_TXN_ID = '30670'
          AND RPT_TXD_ID = '30670_AMT'
          AND RPT_VALUE_2 LIKE :kindName)
WHERE AA2_KIND_ID = RPT_KIND_ID
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get data by CI.AI9,CI.RPT 
      /// (AI9_KIND_ID/MTH_SEQ_NO/SUM_QNTY/SUM_OI/RPT_M_COL/RPT_OI_COL/RPT_ROW)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="kindName"></param>
      /// <returns></returns>
      public DataTable d30670_AI2_SEQ(string startDate , string endDate , string kindName) {

         object[] parms =
         {
                ":startDate", startDate,
                ":endDate", endDate,
                ":kindName", kindName
            };

         string sql = @"
SELECT AI9_KIND_ID,
MTH_SEQ_NO,
SUM_QNTY,
SUM_OI,
RPT_M_COL,
RPT_OI_COL,
RPT_ROW
FROM 
    (SELECT 
     AI9_KIND_ID,
     MTH_SEQ_NO,
     SUM(AI9_M_QNTY) AS SUM_QNTY,
     SUM(AI9_OI) AS SUM_OI
     FROM
     	(SELECT AI9_KIND_ID,
                AI9_MTH_SEQ_NO AS MTH_SEQ_NO,
                AI9_M_QNTY,
                --AI9_OI, --只取最大日期之OI
                CASE WHEN AI9_YMD = FIRST_VALUE(AI9_YMD) OVER (PARTITION BY AI9_KIND_ID ORDER BY AI9_YMD DESC  ROWS UNBOUNDED PRECEDING) THEN AI9_OI ELSE 0 END AS AI9_OI
         FROM CI.AI9
         WHERE AI9_YMD >= :startDate
         AND AI9_YMD <= :endDate
         AND AI9_KIND_ID IN(SELECT RPT_VALUE
                            FROM CI.RPT
                            WHERE RPT_TXN_ID = '30670'
                            AND RPT_TXD_ID = '30670_MTH'
                            AND RPT_VALUE_2 LIKE :kindName))
     GROUP BY AI9_KIND_ID,MTH_SEQ_NO),
              (SELECT RPT_VALUE AS RPT_KIND_ID,RPT_LEVEL_1 AS RPT_M_COL,RPT_LEVEL_2 AS RPT_OI_COL,RPT_LEVEL_3 AS RPT_ROW
               FROM CI.RPT
       		   WHERE RPT_TXN_ID = '30670'
               AND RPT_TXD_ID ='30670_MTH'
               AND RPT_VALUE_2 LIKE :kindName)
WHERE AI9_KIND_ID = RPT_KIND_ID
ORDER BY RPT_ROW , MTH_SEQ_NO , RPT_M_COL
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get data by CI.AI2,CI.RPT 
      /// (AI2_KIND_ID/AI2_YMD/SUM_QNTY/SUM_OI/RPT_M_COL/RPT_OI_COL/RPT_ROW/DAY_SEQ_NO)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="kindName"></param>
      /// <returns></returns>
      public DataTable d30670_AI2_YMD(string startDate , string endDate , string kindName) {

         object[] parms =
         {
                ":startDate", startDate,
                ":endDate", endDate,
                ":kindName", kindName
            };

         string sql = @"
SELECT AI2_KIND_ID,
AI2_YMD,
SUM_QNTY,
SUM_OI,
RPT_M_COL,
RPT_OI_COL,
RPT_ROW,
ROW_NUMBER( ) OVER (PARTITION BY AI2_KIND_ID ORDER BY AI2_KIND_ID,AI2_YMD NULLS LAST)  AS DAY_SEQ_NO
FROM 
    (SELECT AI2_KIND_ID,
            AI2_YMD,
            SUM(AI2_M_QNTY) AS SUM_QNTY,SUM(AI2_OI) AS SUM_OI
     FROM CI.AI2
     WHERE AI2_YMD >= :startDate
     AND AI2_YMD <= :endDate
     AND AI2_SUM_TYPE = 'D'
     AND AI2_SUM_SUBTYPE = '7' 
     AND AI2_KIND_ID IN (SELECT RPT_VALUE
                         FROM CI.RPT
                         WHERE RPT_TXN_ID = '30670'
                         AND RPT_TXD_ID = '30670_DAY'
						 AND RPT_VALUE_2 LIKE :kindName)
     GROUP BY AI2_KIND_ID,AI2_YMD),
    (SELECT RPT_VALUE AS RPT_KIND_ID,RPT_LEVEL_1 AS RPT_M_COL,RPT_LEVEL_2 AS RPT_OI_COL,RPT_LEVEL_3 AS RPT_ROW
     FROM CI.RPT
     WHERE RPT_TXN_ID = '30670'
     AND RPT_TXD_ID = '30670_DAY'
	 AND RPT_VALUE_2 LIKE :kindName)
WHERE AI2_KIND_ID = RPT_KIND_ID
ORDER BY AI2_YMD , RPT_M_COL 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get data by CI.AI3,CI.AI5,CI.RPT,CI.AOCF  (目前缺CI.RELATIVEDATE的Function)
      /// (AI3_DATE/AI3_KIND_ID/AI3_INDEX/AI3_CLOSE_PRICE/RPT_PRICE_COL/RPT_INDEX_COL/AI5_SETTLE_PRICE)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="kindName"></param>
      /// <returns></returns>
      public DataTable d30670_AI3(string endDate , string kindName) {

         object[] parms =
         {
                ":endDate", endDate,
                ":kindName", kindName
            };

         string sql = @"
SELECT AI3_DATE,
AI3_KIND_ID,
AI3_INDEX,
AI3_CLOSE_PRICE,
RPT_PRICE_COL,
RPT_INDEX_COL,
AI5_SETTLE_PRICE            
FROM CI.AI3,                                                                                     
     --報表檔                                                                                    
     (SELECT RPT_VALUE AS RPT_KIND_ID,RPT_LEVEL_1 AS RPT_PRICE_COL,RPT_LEVEL_2 AS RPT_INDEX_COL   
      FROM CI.RPT                                                                               
      WHERE RPT_TXN_ID = '30670'                                                                 
      AND RPT_TXD_ID = '30670_AI3'                                                             
	  AND RPT_VALUE_2 LIKE :kindName),                                                              
     (SELECT AI5_DATE,AI5_KIND_ID,AI5_SETTLE_PRICE                                                       
      FROM CI.AI5,
           (SELECT RPT_VALUE AS RPT_KIND_ID,RPT_LEVEL_1 AS RPT_PRICE_COL,RPT_LEVEL_2 AS RPT_INDEX_COL   
            FROM CI.RPT                                                                               
            WHERE RPT_TXN_ID = '30670'                                                                 
            AND RPT_TXD_ID = '30670_AI3'                                                             
		    AND RPT_VALUE_2 LIKE :kindName)                                                                                       
      WHERE AI5_KIND_ID =  RPT_KIND_ID                                                           
      AND AI5_DATE >= CI.RELATIVEDATE(TO_DATE(:endDate,'YYYYMMDD'), 2, 'MONTH')
      AND AI5_DATE <= TO_DATE(:endDate,'YYYYMMDD'))                                           
WHERE AI3_DATE >= (SELECT TO_DATE(MIN(OCF_YMD),'YYYYMMDD')                                                          
	  			   FROM                                                                                           
		  			   (SELECT OCF_YMD                                                                                
						FROM CI.AOCF                                                                                   
						WHERE OCF_YMD <= :endDate                                                                      
				 		AND OCF_YMD >= TO_CHAR(CI.RELATIVEDATE(TO_DATE(:endDate,'YYYYMMDD'), 2, 'MONTH'),'YYYYMMDD')
			 			ORDER BY OCF_YMD DESC)                                                                        
				   WHERE ROWNUM <= 30)                                                                                                
AND AI3_DATE <= TO_DATE(:endDate,'YYYYMMDD')                                                    
AND AI3_KIND_ID = RPT_KIND_ID          
AND AI3_DATE = AI5_DATE(+)
AND AI3_KIND_ID = AI5_KIND_ID(+)
ORDER BY AI3_DATE , AI3_KIND_ID
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
