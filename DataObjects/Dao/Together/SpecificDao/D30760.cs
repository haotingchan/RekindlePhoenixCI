using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D30760 {
        private Db db;

        public D30760() {
            db = GlobalDaoSetting.DB;
        }

        public string GetMaxDate(string eym) {

            object[] parms = {
                ":is_end_ymd", eym
            };

            string sql = @"SELECT max(AI2_YMD) as endymd
FROM ci.AI2
where TRIM(AI2_YMD) >= :is_end_ymd||'01'
  and TRIM(AI2_YMD) <= :is_end_ymd||'31'
  and AI2_SUM_TYPE = 'D'
  and AI2_SUM_SUBTYPE = '1'
  and AI2_PROD_TYPE in ('F','O')";

            DataTable re = db.GetDataTable(sql, parms);

            return re.Rows[0]["endymd"].ToString();
        }

        public DataTable GetProdData(string sym, string eym, string eymd) {

            object[] parms = {
                ":as_eym", sym,
                ":as_sym", eym,
                ":as_eymd", eymd
            };

            string sql = @"SELECT AI2_PARAM_KEY, ' ' as ASSET_CLASS,' ' as A_COMMENT , AI2_M_QNTY, ' ' as Size_Trading,
       (notional/2/1000000) as notional, AM10_CNT,
       (case when AI2_PROD_TYPE = 'F' then null else  (premium/2/1000000) end) as premium,
        AI2_OI,
       (D_notional/1000000) as D_notional,
       AI2_PROD_TYPE
FROM
(SELECT AI2_PROD_TYPE,AI2_PARAM_KEY,
         SUM(AI2_M_QNTY) AS AI2_M_QNTY,
         SUM(case when TRIM(AI2_YMD) = :as_eym then AI2_OI else 0 end) AS AI2_OI
    FROM ci.AI2  
   WHERE AI2_SUM_TYPE = 'M'  AND  
         TRIM(AI2_YMD) >= :as_sym  AND
         TRIM(AI2_YMD) <= :as_eym  AND
         AI2_PROD_TYPE IN ('F','O') AND
         AI2_SUM_SUBTYPE = '3' 
GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY) A,
(SELECT TRIM(AA2_PARAM_KEY) as AA2_PARAM_KEY,
       sum(case when TRIM(AA2_PROD_TYPE) = 'F' then AA2_AMT else AA2_AMT_STK end) notional,
       sum(case when TRIM(AA2_PROD_TYPE) = 'F' then 0 else AA2_AMT end) premium
  FROM ci.AA2
 where TRIM(AA2_YMD) >= :as_sym||'01'
   and TRIM(AA2_YMD) <= :as_eym||'31'
GROUP BY AA2_PARAM_KEY) I,
(SELECT TRIM(AM10_PARAM_KEY) as AM10_PARAM_KEY,SUM(AM10_CNT) AS AM10_CNT
   FROM ci.AM10
 where TRIM(AM10_YMD) >= :as_sym||'01'
   and TRIM(AM10_YMD) <= :as_eym||'31'
 GROUP BY AM10_PARAM_KEY) C ,
(SELECT TRIM(AA3_PARAM_KEY) as D_AA2_PARAM_KEY,
       sum(AA3_OI_AMT) D_notional
  FROM ci.AA3
 where TRIM(AA3_YMD) = :as_eymd
GROUP BY AA3_PARAM_KEY) D
where TRIM(AI2_PARAM_KEY) = AA2_PARAM_KEY(+)
  AND TRIM(AI2_PARAM_KEY) = AM10_PARAM_KEY(+)
  AND TRIM(AI2_PARAM_KEY) = D_AA2_PARAM_KEY(+)
  ORDER BY AI2_PROD_TYPE, AI2_PARAM_KEY";

            return db.GetDataTable(sql, parms);
        }

        public DataTable GetTradedData(string sym, string eym, string eymd, string prodType) {

            object[] parms = {
                ":as_eym", sym,
                ":as_sym", eym,
                ":as_eymd", eymd,
                ":as_prod_type",prodType
            };

            string sql = @"SELECT AI2_KIND_ID, APDK_NAME, ' ' as ASSET_CLASS,' ' as A_COMMENT ,AI2_M_QNTY,' ' as Side_Trading,    
      NVL(notional,0)/2/1000000 as notional,
      NVL(AM10_CNT,0) AS AM10_CNT,
      NVL(premium,0)/2/1000000 as premium,
      AI2_OI,
      NVL(D_notional,0)/1000000 as D_notional
FROM
(SELECT AI2_PROD_TYPE,SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID,
         SUM(AI2_M_QNTY) AS AI2_M_QNTY,
         SUM(case when TRIM(AI2_YMD) = :as_eym then AI2_OI else 0 end) AS AI2_OI
    FROM ci.AI2  
   WHERE AI2_SUM_TYPE = 'M'  AND  
         TRIM(AI2_YMD) >= :as_sym  AND
         TRIM(AI2_YMD) <= :as_eym  AND
         AI2_PROD_TYPE = :as_prod_type AND
         AI2_SUM_SUBTYPE = '4' AND
         AI2_PROD_SUBTYPE = 'S' 
GROUP BY AI2_PROD_TYPE,SUBSTR(AI2_KIND_ID,1,2)) A,
(SELECT TRIM(AA2_KIND_ID2) AS AA2_KIND_ID,
       sum(case when AA2_PROD_TYPE = 'F' then AA2_AMT else AA2_AMT_STK end) notional,
       sum(case when AA2_PROD_TYPE = 'F' then 0 else AA2_AMT end) premium
  FROM ci.AA2
 where  TRIM(AA2_YMD) >= :as_sym||'01'
   and TRIM(AA2_YMD) <= :as_eym||'31'
   and AA2_PROD_TYPE = :as_prod_type
   and AA2_PROD_SUBTYPE = 'S'
GROUP BY AA2_KIND_ID2) I,
(SELECT SUBSTR(AM10_KIND_ID,1,2) AS AM10_KIND_ID,SUM(AM10_CNT) AS AM10_CNT
   FROM ci.AM10
 where AM10_YMD >= :as_sym||'01'
   and AM10_YMD <= :as_eym||'31'
   and AM10_PROD_TYPE = :as_prod_type
 GROUP BY SUBSTR(AM10_KIND_ID,1,2)) C ,
(SELECT SUBSTR(APDK_KIND_ID,1,2) AS APDK_KIND_ID,MIN(APDK_NAME) AS APDK_NAME
   FROM CI.APDK
  WHERE APDK_PROD_TYPE = :as_prod_type
    AND APDK_PROD_SUBTYPE = 'S'
  GROUP BY SUBSTR(APDK_KIND_ID,1,2)) P,
(SELECT TRIM(AA3_KIND_ID2) AS D_AA2_KIND_ID,
       sum(AA3_OI_AMT) D_notional
  FROM ci.AA3
 where TRIM(AA3_YMD) = :as_eymd
   and AA3_PROD_TYPE = :as_prod_type
   and AA3_PROD_SUBTYPE = 'S'
 GROUP BY AA3_KIND_ID2) D
where AI2_KIND_ID = AA2_KIND_ID(+)
  AND AI2_KIND_ID = AM10_KIND_ID(+)
  AND AI2_KIND_ID = APDK_KIND_ID(+)
  AND AI2_KIND_ID = D_AA2_KIND_ID(+)
  ORDER BY AI2_M_QNTY DESC , AI2_PROD_TYPE, AI2_KIND_ID";

            return db.GetDataTable(sql, parms);
        }

        public DataTable GetFuturesData(string sym, string eym, string eymd, string prodType) {

            object[] parms = {
                ":as_eym", sym,
                ":as_sym", eym,
                ":as_eymd", eymd,
                ":as_prod_type",prodType
            };

            string sql = @"SELECT AI2_KIND_ID, APDK_NAME, ' ' as ASSET_CLASS,' ' as A_COMMENT ,AI2_M_QNTY,' ' as Side_Trading,
      NVL(notional,0)/2/1000000 as notional,
      NVL(AM10_CNT,0) AS AM10_CNT,
      ' ' as TOTAL_OPTIONS,
      AI2_OI,
      NVL(D_notional,0)/1000000 as D_notional
FROM
(SELECT AI2_PROD_TYPE,SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID,
         SUM(AI2_M_QNTY) AS AI2_M_QNTY,
         SUM(case when TRIM(AI2_YMD) = :as_eym then AI2_OI else 0 end) AS AI2_OI
    FROM ci.AI2  
   WHERE AI2_SUM_TYPE = 'M'  AND  
         TRIM(AI2_YMD) >= :as_sym  AND
         TRIM(AI2_YMD) <= :as_eym  AND
         AI2_PROD_TYPE = :as_prod_type AND
         AI2_SUM_SUBTYPE = '4' AND
         AI2_PROD_SUBTYPE = 'S' 
GROUP BY AI2_PROD_TYPE,SUBSTR(AI2_KIND_ID,1,2)) A,
(SELECT TRIM(AA2_KIND_ID2) AS AA2_KIND_ID,
       sum(case when AA2_PROD_TYPE = 'F' then AA2_AMT else AA2_AMT_STK end) notional,
       sum(case when AA2_PROD_TYPE = 'F' then 0 else AA2_AMT end) premium
  FROM ci.AA2
 where  TRIM(AA2_YMD) >= :as_sym||'01'
   and TRIM(AA2_YMD) <= :as_eym||'31'
   and AA2_PROD_TYPE = :as_prod_type
   and AA2_PROD_SUBTYPE = 'S'
GROUP BY AA2_KIND_ID2) I,
(SELECT SUBSTR(AM10_KIND_ID,1,2) AS AM10_KIND_ID,SUM(AM10_CNT) AS AM10_CNT
   FROM ci.AM10
 where AM10_YMD >= :as_sym||'01'
   and AM10_YMD <= :as_eym||'31'
   and AM10_PROD_TYPE = :as_prod_type
 GROUP BY SUBSTR(AM10_KIND_ID,1,2)) C ,
(SELECT SUBSTR(APDK_KIND_ID,1,2) AS APDK_KIND_ID,MIN(APDK_NAME) AS APDK_NAME
   FROM CI.APDK
  WHERE APDK_PROD_TYPE = :as_prod_type
    AND APDK_PROD_SUBTYPE = 'S'
  GROUP BY SUBSTR(APDK_KIND_ID,1,2)) P,
(SELECT TRIM(AA3_KIND_ID2) AS D_AA2_KIND_ID,
       sum(AA3_OI_AMT) D_notional
  FROM ci.AA3
 where TRIM(AA3_YMD) = :as_eymd
   and AA3_PROD_TYPE = :as_prod_type
   and AA3_PROD_SUBTYPE = 'S'
 GROUP BY AA3_KIND_ID2) D
where AI2_KIND_ID = AA2_KIND_ID(+)
  AND AI2_KIND_ID = AM10_KIND_ID(+)
  AND AI2_KIND_ID = APDK_KIND_ID(+)
  AND AI2_KIND_ID = D_AA2_KIND_ID(+)
  ORDER BY AI2_M_QNTY DESC , AI2_PROD_TYPE, AI2_KIND_ID";

            return db.GetDataTable(sql, parms);
        }

        public DataTable GetSumData(string sym, string eym, string eymd) {

            object[] parms = {
                ":as_eym", sym,
                ":as_sym", eym,
                ":as_eymd", eymd
            };

            string sql = @"select sum(AI2_M_QNTY) as sum_M_qnty, ' ' as Size_Trading,
 sum(notional) as Sum_notional, nvl(sum(AM10_CNT),0) as sum_CNT, sum(premium) as sum_premium, 
 sum(AI2_OI) as sum_AI2_OI, nvl(sum(D_notional),0)as sum_D_notional  from (
SELECT AI2_PARAM_KEY, ' ' as ASSET_CLASS,' ' as A_COMMENT , AI2_M_QNTY, ' ' as Size_Trading,
       (notional/2/1000000) as notional, AM10_CNT,
       (case when AI2_PROD_TYPE = 'F' then null else  (premium/2/1000000) end) as premium,
        AI2_OI,
       (D_notional/1000000) as D_notional,
       AI2_PROD_TYPE
FROM
(SELECT AI2_PROD_TYPE,AI2_PARAM_KEY,
         SUM(AI2_M_QNTY) AS AI2_M_QNTY,
         SUM(case when TRIM(AI2_YMD) = :as_eym then AI2_OI else 0 end) AS AI2_OI
    FROM ci.AI2  
   WHERE AI2_SUM_TYPE = 'M'  AND  
         TRIM(AI2_YMD) >= :as_sym  AND
         TRIM(AI2_YMD) <= :as_eym  AND
         AI2_PROD_TYPE IN ('F','O') AND
         AI2_SUM_SUBTYPE = '3' 
GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY) A,
(SELECT TRIM(AA2_PARAM_KEY) as AA2_PARAM_KEY,
       sum(case when TRIM(AA2_PROD_TYPE) = 'F' then AA2_AMT else AA2_AMT_STK end) notional,
       sum(case when TRIM(AA2_PROD_TYPE) = 'F' then 0 else AA2_AMT end) premium
  FROM ci.AA2
 where TRIM(AA2_YMD) >= :as_sym||'01'
   and TRIM(AA2_YMD) <= :as_eym||'31'
GROUP BY AA2_PARAM_KEY) I,
(SELECT TRIM(AM10_PARAM_KEY) as AM10_PARAM_KEY,SUM(AM10_CNT) AS AM10_CNT
   FROM ci.AM10
 where TRIM(AM10_YMD) >= :as_sym||'01'
   and TRIM(AM10_YMD) <= :as_eym||'31'
 GROUP BY AM10_PARAM_KEY) C ,
(SELECT TRIM(AA3_PARAM_KEY) as D_AA2_PARAM_KEY,
       sum(AA3_OI_AMT) D_notional
  FROM ci.AA3
 where TRIM(AA3_YMD) = :as_eymd
GROUP BY AA3_PARAM_KEY) D
where TRIM(AI2_PARAM_KEY) = AA2_PARAM_KEY(+)
  AND TRIM(AI2_PARAM_KEY) = AM10_PARAM_KEY(+)
  AND TRIM(AI2_PARAM_KEY) = D_AA2_PARAM_KEY(+)
  ORDER BY AI2_PROD_TYPE, AI2_PARAM_KEY
  )
  group by AI2_PROD_TYPE";

            return db.GetDataTable(sql, parms);
        }
    }
}
