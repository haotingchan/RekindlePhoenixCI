using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D50031:DataGate
   {
      public DbDataAdapter ListD50031(int ii_market_code,string is_sum_subtype, string is_detail_type, string is_sdate, 
         string is_edate, string is_sbrkno, string is_ebrkno, string is_key, string is_prod_subtype, 
         string is_kind_id2, string is_condition)
      {
         object[] parms = {
                ":as_market_code",ii_market_code,
                ":as_sum_subtype",is_sum_subtype,
                ":as_detail_type",is_detail_type,
                ":as_symd",is_sdate,
                ":as_eymd",is_edate,
                ":as_sbrk_no",is_sbrkno,
                ":as_ebrk_no",is_ebrkno,
                ":as_key",is_key,
                ":as_prod_subtype",is_prod_subtype,
                ":as_kind_id2",is_kind_id2,
                ":as_condition",is_condition
            };
         string sql = @"
         select B.*
         from(         
         SELECT 
                     :as_condition AS AMM4_YMD_AREA,'' AS AMM4_KIND_ID,''AMM4_BRK_NO,'' AS AMM4_BRK_NAME,'' AS AMM4_ACC_NO,
                     '' AS AMM4_OM_QNTY_1,'' AS AMM4_QM_QNTY_1,'' AS AMM4_MM_TOT_QNTY_1,'' AS AMM4_MARKET_M_QNTY_1,
                     '' AS AMM4_OM_QNTY_2,'' AS AMM4_QM_QNTY_2,'' AS AMM4_MM_TOT_QNTY_2,'' AS AMM4_MARKET_M_QNTY_2,            
                     '' AS AMM4_OM_QNTY_3,'' AS AMM4_QM_QNTY_3,'' AS AMM4_MM_TOT_QNTY_3,'' AS AMM4_MARKET_M_QNTY_3,         
                     '' AS AMM4_OM_QNTY_4,'' AS AMM4_QM_QNTY_4,'' AS AMM4_MM_TOT_QNTY_4,'' AS AMM4_MARKET_M_QNTY_4,      
                     '' AS AMM4_OM_QNTY_5,'' AS AMM4_QM_QNTY_5,'' AS AMM4_MM_TOT_QNTY_5,'' AS AMM4_MARKET_M_QNTY_5,      
                     '' AS AMM4_OM_QNTY_6,'' AS AMM4_QM_QNTY_6,'' AS AMM4_MM_TOT_QNTY_6,'' AS AMM4_MARKET_M_QNTY_6   
            FROM DUAL
            UNION ALL
            SELECT 
                     ' ' AS AMM4_YMD_AREA,'' AS AMM4_KIND_ID,''AMM4_BRK_NO,'' AS AMM4_BRK_NAME,'' AS AMM4_ACC_NO,
                     '1' AS AMM4_OM_QNTY_1,'' AS AMM4_QM_QNTY_1,'' AS AMM4_MM_TOT_QNTY_1,'' AS AMM4_MARKET_M_QNTY_1,
                     '2' AS AMM4_OM_QNTY_2,'' AS AMM4_QM_QNTY_2,'' AS AMM4_MM_TOT_QNTY_2,'' AS AMM4_MARKET_M_QNTY_2,            
                     '3' AS AMM4_OM_QNTY_3,'' AS AMM4_QM_QNTY_3,'' AS AMM4_MM_TOT_QNTY_3,'' AS AMM4_MARKET_M_QNTY_3,         
                     '4' AS AMM4_OM_QNTY_4,'' AS AMM4_QM_QNTY_4,'' AS AMM4_MM_TOT_QNTY_4,'' AS AMM4_MARKET_M_QNTY_4,      
                     '5' AS AMM4_OM_QNTY_5,'' AS AMM4_QM_QNTY_5,'' AS AMM4_MM_TOT_QNTY_5,'' AS AMM4_MARKET_M_QNTY_5,      
                     '6' AS AMM4_OM_QNTY_6,'' AS AMM4_QM_QNTY_6,'' AS AMM4_MM_TOT_QNTY_6,'' AS AMM4_MARKET_M_QNTY_6   
            FROM DUAL
            UNION ALL
            SELECT   
                  '日期'AS AMM4_YMD_AREA,'商品代稱' AS AMM4_KIND_ID,'期貨商代號' AS AMM4_BRK_NO,'期貨商名稱'AS AMM4_BRK_NAME,'投資人帳號' AS AMM4_ACC_NO,
                  '一般委託成交量' AS AMM4_OM_QNTY_1,'報價成交量' AS AMM4_QM_QNTY_1,'造市者總成交量' AS AMM4_MM_TOT_QNTY_1,'全市場總成交量' AS AMM4_MARKET_M_QNTY_1,
                  '一般委託成交量' AS AMM4_OM_QNTY_2,'報價成交量' AS AMM4_QM_QNTY_2,'造市者總成交量' AS AMM4_MM_TOT_QNTY_2,'全市場總成交量' AS AMM4_MARKET_M_QNTY_2,            
                  '一般委託成交量' AS AMM4_OM_QNTY_3,'報價成交量' AS AMM4_QM_QNTY_3,'造市者總成交量' AS AMM4_MM_TOT_QNTY_3,'全市場總成交量' AS AMM4_MARKET_M_QNTY_3,         
                  '一般委託成交量' AS AMM4_OM_QNTY_4,'報價成交量' AS AMM4_QM_QNTY_4,'造市者總成交量' AS AMM4_MM_TOT_QNTY_4,'全市場總成交量' AS AMM4_MARKET_M_QNTY_4,      
                  '一般委託成交量' AS AMM4_OM_QNTY_5,'報價成交量' AS AMM4_QM_QNTY_5,'造市者總成交量' AS AMM4_MM_TOT_QNTY_5,'全市場總成交量' AS AMM4_MARKET_M_QNTY_5,      
                  '一般委託成交量' AS AMM4_OM_QNTY_6,'報價成交量' AS AMM4_QM_QNTY_6,'造市者總成交量' AS AMM4_MM_TOT_QNTY_6,'全市場總成交量' AS AMM4_MARKET_M_QNTY_6   
            FROM DUAL
            UNION ALL
            SELECT  
                  CASE :as_detail_type WHEN 'D' THEN AMM4_YMD_AREA ELSE MIN(AMM4_YMD) || '-' || MAX(AMM4_YMD) END AS AMM4_YMD_AREA,
                  AMM4_KIND_ID,
                  AMM4_BRK_NO,
            NVL(ABRK_NAME,'') AS AMM4_BRK_NAME,                
                  AMM4_ACC_NO,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 1  THEN    AMM4_OM_QNTY    ELSE 0  END) )AS AMM4_OM_QNTY_1,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 1  THEN    AMM4_QM_QNTY    ELSE 0  END) )AS AMM4_QM_QNTY_1,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 1  THEN    AMM4_MM_TOT_QNTY    ELSE 0  END) )AS AMM4_MM_TOT_QNTY_1,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 1  THEN    AMM4_MARKET_M_QNTY    ELSE 0  END) )AS AMM4_MARKET_M_QNTY_1,
                
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 2  THEN    AMM4_OM_QNTY    ELSE 0  END) )AS AMM4_OM_QNTY_2,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 2  THEN    AMM4_QM_QNTY    ELSE 0  END) )AS AMM4_QM_QNTY_2,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 2  THEN    AMM4_MM_TOT_QNTY    ELSE 0  END) )AS AMM4_MM_TOT_QNTY_2,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 2  THEN    AMM4_MARKET_M_QNTY    ELSE 0  END) )AS AMM4_MARKET_M_QNTY_2,            
                    
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 3  THEN    AMM4_OM_QNTY    ELSE 0  END) )AS AMM4_OM_QNTY_3,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 3  THEN    AMM4_QM_QNTY    ELSE 0  END) )AS AMM4_QM_QNTY_3,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 3  THEN    AMM4_MM_TOT_QNTY    ELSE 0  END) )AS AMM4_MM_TOT_QNTY_3,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 3  THEN    AMM4_MARKET_M_QNTY    ELSE 0  END) )AS AMM4_MARKET_M_QNTY_3,         
                       
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 4  THEN    AMM4_OM_QNTY    ELSE 0  END) )AS AMM4_OM_QNTY_4,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 4  THEN    AMM4_QM_QNTY    ELSE 0  END) )AS AMM4_QM_QNTY_4,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 4  THEN    AMM4_MM_TOT_QNTY    ELSE 0  END) )AS AMM4_MM_TOT_QNTY_4,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 4  THEN    AMM4_MARKET_M_QNTY    ELSE 0  END) )AS AMM4_MARKET_M_QNTY_4,      
                          
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 5  THEN    AMM4_OM_QNTY    ELSE 0  END) )AS AMM4_OM_QNTY_5,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 5  THEN    AMM4_QM_QNTY    ELSE 0  END) )AS AMM4_QM_QNTY_5,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 5  THEN    AMM4_MM_TOT_QNTY    ELSE 0  END) )AS AMM4_MM_TOT_QNTY_5,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 5  THEN    AMM4_MARKET_M_QNTY    ELSE 0  END) )AS AMM4_MARKET_M_QNTY_5,      
                          
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 6  THEN    AMM4_OM_QNTY    ELSE 0  END) )AS AMM4_OM_QNTY_6,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 6  THEN    AMM4_QM_QNTY    ELSE 0  END) )AS AMM4_QM_QNTY_6,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 6  THEN    AMM4_MM_TOT_QNTY    ELSE 0  END) )AS AMM4_MM_TOT_QNTY_6,
                  TO_CHAR(SUM(CASE  WHEN AMM4_MTH_SEQ_NO = 6  THEN    AMM4_MARKET_M_QNTY    ELSE 0  END) )AS AMM4_MARKET_M_QNTY_6                      
            FROM (

                     SELECT 	AMM4_YMD,
                        CASE :as_detail_type WHEN 'D' THEN AMM4_YMD ELSE '' END AS AMM4_YMD_AREA,
                        CASE :as_sum_subtype
                        WHEN '3' THEN CASE WHEN AMM4_PROD_SUBTYPE = 'S' AND AMM4_PROD_TYPE = 'F' THEN 'STF*' 
                                          WHEN AMM4_PROD_SUBTYPE = 'S' AND AMM4_PROD_TYPE = 'O' THEN 'STC*' 
                                          ELSE AMM4_PARAM_KEY END
                        WHEN 'S' THEN  AMM4_PARAM_KEY
                        WHEN '4' THEN CASE WHEN LENGTH(TRIM(AMM4_KIND_ID2)) = 2 THEN TRIM(AMM4_KIND_ID2)||AMM4_PROD_TYPE 
                                          ELSE TRIM(AMM4_KIND_ID2) END
                        ELSE AMM4_KIND_ID END AS AMM4_KIND_ID,
                        AMM4_KIND_ID2,
                        AMM4_PARAM_KEY,	
                        AMM4_BRK_NO,
                        AMM4_ACC_NO,
                        AMM4_MTH_SEQ_NO,
                        NVL(AMM4_OM_QNTY,0) AS AMM4_OM_QNTY,
                        NVL(AMM4_QM_QNTY,0) AS AMM4_QM_QNTY,
                        (NVL(AMM4_OM_QNTY,0) + NVL(AMM4_QM_QNTY,0) + NVL(AMM4_IQM_QNTY,0) + NVL(AMM4_BTRADE_M_QNTY,0)) AS AMM4_MM_TOT_QNTY,
                        NVL(AMM4_MARKET_M_QNTY,0) AS AMM4_MARKET_M_QNTY                                
               FROM CI.AMM4
  	            WHERE AMM4_BRK_NO <> ' '  --固定條件
               AND AMM4_MARKET_CODE = :as_market_code
	            AND AMM4_YMD >= :as_symd
	            AND AMM4_YMD <= :as_eymd
	            AND((AMM4_BRK_NO >= :as_sbrk_no AND AMM4_BRK_NO <= :as_ebrk_no))
	            AND AMM4_PARAM_KEY LIKE :as_key
	            AND AMM4_PROD_SUBTYPE LIKE :as_prod_subtype
	            AND AMM4_KIND_ID2 LIKE :as_kind_id2
            )A,ci.ABRK
            WHERE A.AMM4_BRK_NO = ABRK_NO(+)
            GROUP BY  A.AMM4_YMD_AREA,A.AMM4_KIND_ID,A.AMM4_BRK_NO,ABRK_NAME,A.AMM4_ACC_NO
         )B
         ORDER BY (case when substr(amm4_ymd_area,0,4)='報表條件'  then 0 
                       when substr(amm4_ymd_area,0,4)=' ' then 1 
                       when substr(amm4_ymd_area,0,4)='日期' then 2 else 3 end),
         amm4_ymd_area ,amm4_kind_id,amm4_brk_no,amm4_acc_no
";
         DbDataAdapter adapter = db.GetDataAdapter(sql, parms);
         return adapter;
      }//public DbDataAdapter ListD50030

   }
}
