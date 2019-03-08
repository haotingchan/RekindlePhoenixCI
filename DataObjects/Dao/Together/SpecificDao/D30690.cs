using OnePiece;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// ken,2019/1/24
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 本週交易量分析
    /// </summary>
    public class D30690 {

        private Db db;

        public D30690() {

            db = GlobalDaoSetting.DB;

        }

        /// <summary>
        /// 一般成交量--一般 return 17 fields
        /// </summary>
        /// <param name="as_last_fm">yyyyMMdd</param>
        /// <param name="as_last_to">yyyyMMdd</param>
        /// <param name="as_this_fm">yyyyMMdd</param>
        /// <param name="as_this_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_day_avg_qnty1(string as_last_fm,
                                                string as_last_to,
                                                string as_this_fm,
                                                string as_this_to) {

            object[] parms = {
                ":as_last_fm", as_last_fm,
                ":as_last_to", as_last_to,
                ":as_this_fm", as_this_fm,
                ":as_this_to", as_this_to
            };


            string sql = @"
SELECT AI2_PROD_TYPE as prod_type,
	trim(AI2_PARAM_KEY) as param_key,
	trim(AI2_PARAM_KEY) AS kind_id2,
	sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_M_QNTY END) AS week_qnty,
	sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_AH_M_QNTY END) AS week_ah_qnty,
	sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_OI END) AS week_oi,
	count(DISTINCT CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_YMD END) AS week_days,
	sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_M_QNTY END) AS last_qnty,
	sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_AH_M_QNTY END) AS last_ah_qnty,
	sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_OI END) AS last_oi,
	count(DISTINCT CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_YMD END) AS last_days,
	sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_M_QNTY END) AS year_qnty,
	sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_AH_M_QNTY END) AS year_ah_qnty,
	sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_OI END) AS year_oi,
	count(DISTINCT CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_YMD END) AS year_days,
	AI2_PROD_TYPE,
	0 as kind_seq
FROM CI.AI2
WHERE AI2_SUM_TYPE = 'D'
AND AI2_SUM_SUBTYPE = '3'
AND AI2_PROD_TYPE IN ('F','O')
AND AI2_YMD >= substr( :as_last_fm,1,4)||'0101'
AND AI2_YMD <=  :as_this_to
GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY

UNION ALL 
SELECT ' ' as prod_type,
	trim(AI2_PARAM_KEY) as param_key,
	'└─'||trim(AI2_KIND_ID2) as kind_id2,
	sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_M_QNTY END) AS week_qnty,
	sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_AH_M_QNTY END) AS week_ah_qnty,
	sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_OI END) AS week_oi,
	count(DISTINCT CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_YMD END) AS week_day,
	sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_M_QNTY END) AS last_qnty,
	sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_AH_M_QNTY END) AS last_ah_qnty,
	sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_OI END) AS last_oi,
	count(DISTINCT CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_YMD END) AS last_day,
	sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_M_QNTY END) AS year_qnty,
	sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_AH_M_QNTY END) AS year_ah_qnty,
	sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_OI END) AS year_oi,
	count(DISTINCT CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_YMD END) AS year_day,
	AI2_PROD_TYPE,
	1 as kind_seq
FROM CI.AI2
WHERE AI2_SUM_TYPE = 'D'
AND AI2_SUM_SUBTYPE = '4'
AND AI2_PROD_TYPE IN ('F','O')
AND AI2_YMD >= substr( :as_last_fm,1,4)||'0101'
AND AI2_YMD <=  :as_this_to
AND AI2_PARAM_KEY IN ('TXO')
GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY,AI2_KIND_ID2
order by ai2_prod_type , param_key , kind_seq , kind_id2
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            dtResult.Columns.Remove("AI2_PROD_TYPE");
            dtResult.Columns.Remove("kind_seq");

            return dtResult;
        }


        /// <summary>
        /// 一般成交量--夜盤上線迄今 return 8 fields
        /// </summary>
        /// <param name="as_fm">yyyyMMdd</param>
        /// <param name="as_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_day_avg_qnty2(string as_fm,
                                                string as_to) {

            object[] parms = {
                ":as_fm", as_fm,
                ":as_to", as_to
            };

            string sql = @"
SELECT AI2_PROD_TYPE as prod_type,
	trim(AI2_PARAM_KEY) as param_key,
	trim(AI2_PARAM_KEY) AS kind_id2,
	sum(AI2_M_QNTY) AS week_qnty,
	sum(AI2_AH_M_QNTY) AS week_ah_qnty,
	sum(AI2_OI) AS week_oi,
	count(DISTINCT AI2_YMD) AS week_days,
    ai2_prod_type,
	0 as kind_seq
FROM CI.AI2
WHERE AI2_SUM_TYPE = 'D'
AND AI2_SUM_SUBTYPE = '3'
AND AI2_PROD_TYPE IN ('F','O')
AND AI2_YMD >= :as_fm
AND AI2_YMD <= :as_to
GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY

UNION ALL 
SELECT ' ' as prod_type,
    trim(AI2_PARAM_KEY) as param_key,
    '└─'||trim(AI2_KIND_ID2) as kind_id2,
    sum(AI2_M_QNTY) AS week_qnty,
    sum(AI2_AH_M_QNTY) AS week_ah_qnty,
    sum(AI2_OI) AS week_oi,
    count(DISTINCT AI2_YMD) AS week_days,
    ai2_prod_type,
    1 as kind_seq
FROM CI.AI2
WHERE AI2_SUM_TYPE = 'D'
AND AI2_SUM_SUBTYPE = '4'
AND AI2_PROD_TYPE IN ('F','O')
AND AI2_YMD >= :as_fm
AND AI2_YMD <= :as_to
AND AI2_PARAM_KEY IN ('TXO')
GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY,AI2_KIND_ID2
order by ai2_prod_type , param_key , kind_seq , kind_id2
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            dtResult.Columns.Remove("ai2_prod_type");
            dtResult.Columns.Remove("kind_seq");

            return dtResult;
        }

        /// <summary>
        /// 盤後成交量 return 14 fields
        /// </summary>
        /// <param name="as_last_fm">yyyyMMdd</param>
        /// <param name="as_last_to">yyyyMMdd</param>
        /// <param name="as_this_fm">yyyyMMdd</param>
        /// <param name="as_this_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_night_avg_qnty1(string as_last_fm,
                                                string as_last_to,
                                                string as_this_fm,
                                                string as_this_to) {

            object[] parms = {
                ":as_last_fm", as_last_fm,
                ":as_last_to", as_last_to,
                ":as_this_fm", as_this_fm,
                ":as_this_to", as_this_to
            };


            string sql = @"
SELECT AI2_PROD_TYPE as prod_type,
    trim(AI2_PARAM_KEY) as param_key,
    trim(AI2_PARAM_KEY) AS kind_id2,
    0 as kind_seq,
    sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_AH_M_QNTY END) AS week_qnty,
    sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_OI END) AS week_oi,
    count(DISTINCT CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_YMD END) AS week_days,
    sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_AH_M_QNTY END) AS last_qnty,
    sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_OI END) AS last_oi,
    count(DISTINCT CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_YMD END) AS last_days,
    sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to THEN AI2_AH_M_QNTY END) AS year_qnty,
    sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to THEN AI2_OI END) AS year_oi,
    count(DISTINCT CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_YMD END) AS year_days,
    AI2_PROD_TYPE
FROM CI.AI2
WHERE AI2_SUM_TYPE = 'D'
AND AI2_SUM_SUBTYPE = '3'
AND AI2_PROD_TYPE IN ('F','O')
AND AI2_YMD >= substr( :as_last_fm,1,4)||'0101'
AND AI2_YMD <=  :as_this_to
AND NVL(AI2_AH_DAY_COUNT,0) > 0
GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY

UNION ALL 
SELECT ' ' as prod_type,
    trim(AI2_PARAM_KEY) as param_key,
    '└─'||trim(AI2_KIND_ID2) as kind_id2,
    1 as kind_seq,
    sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_AH_M_QNTY END) AS week_qnty,
    sum(CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_OI END) AS week_oi,
    count(DISTINCT CASE WHEN AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_YMD END) AS week_days,
    sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_AH_M_QNTY END) AS last_qnty,
    sum(CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_OI END) AS last_oi,
    count(DISTINCT CASE WHEN AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_YMD END) AS last_days,
    sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to THEN AI2_AH_M_QNTY END) AS year_qnty,
    sum(CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to THEN AI2_OI END) AS year_oi,
    count(DISTINCT CASE WHEN AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_YMD END) AS year_days,
    AI2_PROD_TYPE
FROM CI.AI2
WHERE AI2_SUM_TYPE = 'D'
AND AI2_SUM_SUBTYPE = '4'
AND AI2_PROD_TYPE IN ('F','O')
AND AI2_YMD >= substr( :as_last_fm,1,4)||'0101'
AND AI2_YMD <=  :as_this_to
AND AI2_PARAM_KEY IN ('TXO')
AND NOT AI2_AH_M_QNTY IS NULL
GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY,AI2_KIND_ID2
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            dtResult.Columns.Remove("AI2_PROD_TYPE");
            dtResult.Columns.Remove("kind_seq");

            return dtResult;
        }


        /// <summary>
        /// 盤後成交量 return 9 fields
        /// </summary>
        /// <param name="as_fm">yyyyMMdd</param>
        /// <param name="as_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_night_avg_qnty2(string as_fm,
                                                string as_to) {

            object[] parms = {
                ":as_fm", as_fm,
                ":as_to", as_to
            };

            string sql = @"
SELECT CASE WHEN kind_seq = 1 THEN ' ' else AI2_PROD_TYPE end as prod_type,
      trim(AI2_PARAM_KEY) as param_key,
      kind_seq,
      CASE WHEN kind_seq = 1 THEN '└─'||trim(AI2_KIND_ID2) ELSE trim(AI2_PARAM_KEY) END as kind_id2,
      week_qnty,
      week_oi,
      week_days,
      AI2_PROD_TYPE,
      week_dt_qnty/2 as week_dt_qnty
 FROM         
     (SELECT AI2_PARAM_KEY,AI2_PARAM_KEY AS AI2_KIND_ID2,0 as kind_seq,
             sum(AI2_AH_M_QNTY) AS week_qnty,
             sum(AI2_OI) AS week_oi,
             count(DISTINCT AI2_YMD) AS week_days,
             AI2_PROD_TYPE
        FROM ci.AI2
       WHERE AI2_SUM_TYPE = 'D'
         AND AI2_SUM_SUBTYPE = '3'
         AND AI2_PROD_TYPE IN ('F','O')
         AND AI2_YMD >= :as_fm
         AND AI2_YMD <= :as_to
         AND NOT AI2_AH_M_QNTY IS NULL
       GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY
       UNION ALL
      SELECT AI2_PARAM_KEY,AI2_KIND_ID2,1 as kind_seq,
             sum(AI2_AH_M_QNTY) AS week_qnty,
             sum(AI2_OI) AS week_oi,
             count(DISTINCT AI2_YMD) AS week_days,
             AI2_PROD_TYPE
        FROM ci.AI2
       WHERE AI2_SUM_TYPE = 'D'
         AND AI2_SUM_SUBTYPE = '4'
         AND AI2_PROD_TYPE IN ('F','O')
         AND AI2_YMD >= :as_fm
         AND AI2_YMD <= :as_to
         AND AI2_PARAM_KEY IN ('TXO')
         AND NOT AI2_AH_M_QNTY IS NULL
       GROUP BY AI2_PROD_TYPE,AI2_PARAM_KEY,AI2_KIND_ID2) A, 
     
     --當日沖銷口數  
     (select AM10_PARAM_KEY,AM10_PARAM_KEY as M_KIND_ID2,sum(AM10_DT_QNTY) as week_dt_qnty,0 as m_kind_seq
        from ci.AM10
       where AM10_YMD >= :as_fm  
         and AM10_YMD <= :as_to
         and AM10_MARKET_CODE = '1' 
       group by AM10_PARAM_KEY
       UNION ALL
      select AM10_PARAM_KEY,AM10_KIND_ID2,sum(AM10_DT_QNTY) as week_dt_qnty,1 as m_kind_seq
        from ci.AM10
       where AM10_YMD >= :as_fm  
         and AM10_YMD <= :as_to
         and AM10_MARKET_CODE = '1' 
         and AM10_PARAM_KEY = 'TXO'
       group by AM10_PARAM_KEY,AM10_KIND_ID2) M
       
WHERE AI2_PARAM_KEY = AM10_PARAM_KEY(+) 
 AND  AI2_KIND_ID2 = M_KIND_ID2(+) 
 AND  kind_seq = m_kind_seq(+) 
order by ai2_prod_type , param_key , kind_seq , kind_id2
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 盤後成交量 return prod_type / param_key / ah_oi / ah_days / d_oi / d_days
        /// </summary>
        /// <param name="as_d_symd">日盤平均OI_StartDate yyyyMMdd</param>
        /// <param name="as_d_eymd">日盤平均OI_EndDate yyyyMMdd</param>
        /// <param name="as_ah_symd">夜盤平均OI_StartDate yyyyMMdd</param>
        /// <param name="as_ah_eymd">夜盤平均OI_EndDate yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_ah_oi(string as_d_symd,
                                        string as_d_eymd,
                                        string as_ah_symd,
                                        string as_ah_eymd) {

            object[] parms = {
                ":as_d_symd", as_d_symd,
                ":as_d_eymd", as_d_eymd,
                ":as_ah_symd", as_ah_symd,
                ":as_ah_eymd", as_ah_eymd
            };


            string sql = @"
SELECT d_prod_type as prod_type,
     d_param_key as param_key,        
     ah_oi,
     ah_days,
     d_oi,
     d_days
FROM
     --日盤OI
    (SELECT APDK_PROD_TYPE as d_prod_type,APDK_PARAM_KEY as d_param_key,round(SUM(AOI2_OI)/2,0) AS d_oi,count(distinct AOI2_YMD) AS d_days
       FROM ci.AOI2 ,
               (SELECT DISTINCT APDK_PROD_TYPE,APDK_PARAM_KEY FROM ci.APDK WHERE APDK_MARKET_CODE = '1') P
      WHERE AOI2_ymd >= :as_d_symd
        AND AOI2_ymd <= :as_d_eymd
        AND AOI2_PARAM_KEY = APDK_PARAM_KEY
      GROUP BY APDK_PROD_TYPE,APDK_PARAM_KEY) D,
     --夜盤OI
    (SELECT AOI2_PARAM_KEY as ah_param_key,round(SUM(AOI2_OI_AH)/2,0) AS ah_oi,count(distinct AOI2_YMD) AS ah_days
       FROM ci.AOI2
      WHERE AOI2_ymd >= :as_ah_symd
        AND AOI2_ymd <= :as_ah_eymd
        AND AOI2_AH_TRADE_FLAG = 'Y'
      GROUP BY AOI2_PARAM_KEY) A
WHERE d_param_key = ah_param_key(+)
order by prod_type , param_key
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// ETF成交量 return 29 fields
        /// </summary>
        /// <param name="as_last_fm">yyyyMMdd</param>
        /// <param name="as_last_to">yyyyMMdd</param>
        /// <param name="as_this_fm">yyyyMMdd</param>
        /// <param name="as_this_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_etf(string as_last_fm,
                                     string as_last_to,
                                     string as_this_fm,
                                     string as_this_to) {

            object[] parms = {
                ":as_last_fm", as_last_fm,
                ":as_last_to", as_last_to,
                ":as_this_fm", as_this_fm,
                ":as_this_to", as_this_to
            };


            string sql = @"
SELECT apdk_name,APDK_MARKET_CLOSE as market_close,AI2_KIND_ID2 as kind_id2,
    f_week_qnty,f_week_oi,f_week_days,am11_etf_week_qnty,
    f_last_qnty,f_last_oi,f_last_days,am11_etf_last_qnty,
    f_year_qnty,f_year_oi,f_year_days,am11_etf_year_qnty,
    o_week_qnty,o_week_oi,o_week_days,am11_etc_week_qnty,
    o_last_qnty,o_last_oi,o_last_days,am11_etc_last_qnty,
    o_year_qnty,o_year_oi,o_year_days,am11_etc_year_qnty
FROM
    (SELECT trim(APDK_NAME) as apdk_name,AI2_KIND_ID2,APDK_MARKET_CLOSE,
        sum(CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_M_QNTY END) AS f_week_qnty,
        sum(CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_OI END) AS f_week_oi,
        count(DISTINCT CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_YMD END) AS f_week_days,
        sum(CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_M_QNTY END) AS f_last_qnty,
        sum(CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_OI END) AS f_last_oi,
        count(DISTINCT CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_YMD END) AS f_last_days,
        sum(CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_M_QNTY END) AS f_year_qnty,
        sum(CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_OI END) AS f_year_oi,
        count(DISTINCT CASE WHEN AI2_PROD_TYPE = 'F' and AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_YMD END) AS f_year_days,
        sum(CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_M_QNTY END) AS o_week_qnty,
        sum(CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_OI END) AS o_week_oi,
        count(DISTINCT CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= :as_this_fm AND AI2_YMD <= :as_this_to THEN AI2_YMD END) AS o_week_days,
        sum(CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_M_QNTY END) AS o_last_qnty,
        sum(CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_OI END) AS o_last_oi,
        count(DISTINCT CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= :as_last_fm AND AI2_YMD <= :as_last_to THEN AI2_YMD END) AS o_last_days,
        sum(CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_M_QNTY END) AS o_year_qnty,
        sum(CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_OI END) AS o_year_oi,
        count(DISTINCT CASE WHEN AI2_PROD_TYPE = 'O' and AI2_YMD >= substr( :as_this_to,1,4)||'0101' AND AI2_YMD <=:as_this_to  THEN AI2_YMD END) AS o_year_days
        
        FROM ci.AI2,
            (select APDK_KIND_ID2,
            substr(max(APDK_NAME),1,GREATEST(INSTR(max(APDK_NAME),'期貨', 1, 1),INSTR(max(APDK_NAME),'選擇權', 1, 1)) - 1) as APDK_NAME,
            APDK_MARKET_CLOSE 
            from ci.APDK
            where APDK_PARAM_KEY IN ('ETF','ETC')
            group by APDK_KIND_ID2,APDK_MARKET_CLOSE) P
        WHERE AI2_SUM_TYPE = 'D'
        AND AI2_SUM_SUBTYPE = '4'
        AND AI2_PROD_TYPE IN ('F','O')
        AND AI2_YMD >= substr( :as_last_fm,1,4)||'0101'
        AND AI2_YMD <=  :as_this_to
        AND AI2_PARAM_KEY IN ('ETF','ETC') 
        AND AI2_KIND_ID2 = APDK_KIND_ID2(+)   
        GROUP BY AI2_KIND_ID2,APDK_NAME,APDK_MARKET_CLOSE),

    (select AM11_KIND_ID2,
        sum(CASE WHEN AM11_PARAM_KEY = 'ETF' and AM11_YMD >= :as_this_fm AND AM11_YMD <= :as_this_to THEN AM11_M_QNTY END) / 2 AS am11_etf_week_qnty,
        sum(CASE WHEN AM11_PARAM_KEY = 'ETF' and AM11_YMD >= :as_last_fm AND AM11_YMD <= :as_last_to THEN AM11_M_QNTY END) / 2 AS am11_etf_last_qnty,
        sum(CASE WHEN AM11_PARAM_KEY = 'ETF' and AM11_YMD >= substr( :as_this_to,1,4)||'0101' AND AM11_YMD <=:as_this_to  THEN AM11_M_QNTY END) AS am11_etf_year_qnty,
        sum(CASE WHEN AM11_PARAM_KEY = 'ETC' and AM11_YMD >= :as_this_fm AND AM11_YMD <= :as_this_to THEN AM11_M_QNTY END) / 2 AS am11_etc_week_qnty,
        sum(CASE WHEN AM11_PARAM_KEY = 'ETC' and AM11_YMD >= :as_last_fm AND AM11_YMD <= :as_last_to THEN AM11_M_QNTY END) / 2 AS am11_etc_last_qnty,
        sum(CASE WHEN AM11_PARAM_KEY = 'ETC' and AM11_YMD >= substr( :as_this_to,1,4)||'0101' AND AM11_YMD <=:as_this_to  THEN AM11_M_QNTY END) AS am11_etc_year_qnty
        from ci.AM11  
        where AM11_SUM_TYPE = 'D'
        and AM11_YMD >= substr( :as_last_fm,1,4)||'0101'
        and AM11_YMD <= :as_this_to
        and AM11_PARAM_KEY in ('ETF','ETC')
        and AM11_OSW_GRP = '5'
        group by AM11_KIND_ID2) 

where AI2_KIND_ID2 = AM11_KIND_ID2(+)
order by kind_id2
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 每月日均量
        /// </summary>
        /// <param name="as_fm">yyyyMMdd</param>
        /// <param name="as_to">yyyyMMdd</param>
        /// <param name="as_day_night">A=all, D=day, N=night</param>
        /// <returns></returns>
        public DataTable d_30690_mth_qnty_day_night(string as_fm,
                                                    string as_to,
                                                    string as_day_night = "A") {

            object[] parms = {
                ":as_fm", as_fm,
                ":as_to", as_to,
                ":as_day_night",as_day_night
            };

            //TODO 整個SQL要請期交所優化才行
            //1.日盤會當掉(as_day_night = "D"),請優化
            //  ken 原本的:as_day_night = 'D'非常非常慢,我特別優化,將子查詢先撈出來組SQL語法,避開自身同樣TABLE當查詢條件而造成的短暫lock現象 
            //2.條件不對,as_day_night = "A"不等同於D + N,請確認邏輯
            //3.日盤為什麼要從20170515開始,夜盤不用 ?
            //4.日盤條件AI2_AH_DAY_COUNT > 0應該指的是夜盤,為什麼這邊AI2_PARAM_KEY 是要用夜盤商品來當日盤範圍?
            string pKey = "";
            if (as_day_night == "D") {
                string sqlFirst = @"
                                    select AI2_PARAM_KEY
                                    from ci.AI2
                                    where AI2_SUM_TYPE = 'D'
                                    and AI2_SUM_SUBTYPE = '3'
                                    and AI2_PROD_TYPE IN('F', 'O')
                                    and AI2_YMD >= substr( :as_fm, 1, 4) || '0101'
                                    and AI2_YMD <=  :as_to
                                    and AI2_AH_DAY_COUNT > 0
                                    group by AI2_PARAM_KEY";

                DataTable dtTemp = db.GetDataTable(sqlFirst, parms);

                pKey = " and AI2_PARAM_KEY in ('";
                foreach (DataRow dr in dtTemp.Rows) {
                    pKey += dr[0].ToString() + "','";
                }
                pKey += "')";
            }//if (as_day_night == "D") {


            string sql = string.Format(@"
SELECT rpt_ym as data_ym,
             rpt_prod_type as prod_type,
             rpt_param_key as param_key,
             nvl(data_qnty,0) as data_qnty,
             nvl(data_day_cnt,0) as data_day_cnt,
             rpt_col
        FROM
     (--每月資料
      SELECT AI2_PROD_TYPE as prod_type,TRIM(AI2_PARAM_KEY) AS param_key,SUBSTR(AI2_YMD,1,6) AS data_ym,
             SUM((case :as_day_night when  'D' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0) when 'N' then NVL(AI2_AH_M_QNTY,0) else AI2_M_QNTY end)) AS data_qnty,
             COUNT(case when :as_day_night in ('A','D') then AI2_YMD else (case when AI2_AH_DAY_COUNT > 0 then AI2_YMD else null end) end) AS data_day_cnt
        FROM ci.AI2
        WHERE AI2_SUM_TYPE = 'D'
          AND AI2_SUM_SUBTYPE = '3'
          AND AI2_PROD_TYPE IN ('F','O')
          AND AI2_YMD >= substr( :as_fm,1,4)||'0101'
          AND AI2_YMD <=  :as_to 
          AND ( 
               (:as_day_night = 'A') or 
               (:as_day_night = 'N' and AI2_AH_DAY_COUNT > 0) or 
               (:as_day_night = 'D' and AI2_YMD >= '20170515' {0})
              )
        GROUP BY  AI2_PROD_TYPE,AI2_PARAM_KEY,SUBSTR(AI2_YMD,1,6)
        UNION ALL
       --年度資料
       SELECT AI2_PROD_TYPE as prod_type,TRIM(AI2_PARAM_KEY) AS param_key,SUBSTR(AI2_YMD,1,4) AS data_ym,
             SUM((case :as_day_night when  'D' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0) when 'N' then NVL(AI2_AH_M_QNTY,0) else AI2_M_QNTY end)) AS data_qnty,
             COUNT(case when :as_day_night in ('A','D') then AI2_YMD else (case when AI2_AH_DAY_COUNT > 0 then AI2_YMD else null end) end) AS data_day
        FROM ci.AI2
        WHERE AI2_SUM_TYPE = 'D'
          AND AI2_SUM_SUBTYPE = '3'
          AND AI2_PROD_TYPE IN ('F','O')
          AND AI2_YMD >= substr( :as_to,1,4)||'0101'
          AND AI2_YMD <=  :as_to 
          AND ( 
               (:as_day_night = 'A') or 
               (:as_day_night = 'N' and AI2_AH_DAY_COUNT > 0) or 
               (:as_day_night = 'D' and AI2_YMD >= '20170515' {0})
              )
        GROUP BY  AI2_PROD_TYPE,AI2_PARAM_KEY,SUBSTR(AI2_YMD,1,4)),

       (--每月資料
        SELECT rpt_ym,rpt_col,PDK_PROD_TYPE as rpt_prod_type,trim(PDK_PARAM_KEY) as rpt_param_key
          FROM 
              (SELECT rpt_ym,rpt_col
                 FROM
                     (SELECT SUBSTR(OCF_YMD,1,6) as rpt_ym,
                                 12 - ROW_NUMBER( ) OVER ( ORDER BY SUBSTR(OCF_YMD,1,6) desc NULLS LAST) + 1 as rpt_col
                      FROM CI.AOCF 
                     WHERE OCF_YMD >= substr(:as_fm,1,4)||'0101' 
                       AND OCF_YMD <= :as_to
                     GROUP BY SUBSTR(OCF_YMD,1,6)
                     ORDER BY  2)
               WHERE rpt_col > 0
               UNION ALL
              SELECT substr(:as_to,1,4),13 
                FROM DUAL) R,
             (SELECT PDK_PROD_TYPE,PDK_PARAM_KEY from ci.HPDK
               WHERE PDK_DATE >= TO_DATE(:as_fm,'YYYYMMDD')
                 AND PDK_DATE <= TO_DATE(:as_to,'YYYYMMDD')
                 AND (:as_day_night <> 'N' or NOT PDK_AH_MARKET_OPEN IS NULL)
               GROUP BY PDK_PROD_TYPE,PDK_PARAM_KEY) P )

WHERE rpt_ym = data_ym(+)
AND rpt_prod_type = prod_type(+)
AND rpt_param_key = param_key(+)
order by prod_type , param_key , rpt_col
", pKey);

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 平均預估量
        /// </summary>
        /// <param name="as_fm">yyyyMMdd</param>
        /// <param name="as_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_mth_qnty_am7t(string as_fm,
                                                string as_to) {

            object[] parms = {
                ":as_fm", as_fm,
                ":as_to", as_to
            };


            string sql = @"
SELECT prod_type as prod_type,param_key as param_key,nvl(AM7T_AVG_QNTY,0) as avg_qnty,nvl(seq_no,9999) as seq_no
  FROM ci.AM7T,
       --商品檔  
      (SELECT prod_type,param_key,rownum as seq_no
         FROM
             (SELECT AI2_PROD_TYPE as prod_type,AI2_PARAM_KEY AS param_key
                FROM ci.AI2
               WHERE AI2_SUM_TYPE = 'D'
                 AND AI2_SUM_SUBTYPE = '3'
                 AND AI2_PROD_TYPE IN ('F','O')
                 AND AI2_YMD >= substr( :as_fm,1,4)||'0101'
                 AND AI2_YMD <= :as_to
               GROUP BY  AI2_PROD_TYPE,AI2_PARAM_KEY
               ORDER BY AI2_PROD_TYPE,AI2_PARAM_KEY)) 
WHERE AM7T_Y(+) = substr(:as_to,1,4)
  AND AM7T_PROD_TYPE(+) = prod_type
  and AM7T_PARAM_KEY(+) = param_key
order by seq_no
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 每月平均預估量
        /// </summary>
        /// <param name="as_fm">yyyyMMdd</param>
        /// <param name="as_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_mth_qnty_am7m(string as_fm,
                                                string as_to) {

            object[] parms = {
                ":as_fm", as_fm,
                ":as_to", as_to
            };


            string sql = @"
SELECT AM7M_YM as rpt_ym,AM7M_PROD_TYPE as prod_type,nvl(sum(AM7M_AVG_QNTY),0) as avg_qnty,rpt_col
  FROM ci.AM7M,
      (--每月資料
       SELECT rpt_ym,rpt_col
         FROM
             (SELECT SUBSTR(OCF_YMD,1,6) as rpt_ym,
                         12 - ROW_NUMBER( ) OVER ( ORDER BY SUBSTR(OCF_YMD,1,6) desc NULLS LAST) + 1 as rpt_col
              FROM CI.AOCF 
             WHERE OCF_YMD >= substr( :as_fm,1,4)||'0101' 
               AND OCF_YMD <= :as_to
             GROUP BY SUBSTR(OCF_YMD,1,6)
             ORDER BY  2)
       WHERE rpt_col > 0
       UNION ALL
      SELECT substr(:as_to,1,4),13 
        FROM DUAL) R
 WHERE rpt_ym = AM7m_ym
 GROUP BY AM7M_YM,AM7M_PROD_TYPE,rpt_col
order by prod_type , rpt_ym
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 每月波動及振幅(現貨/期貨) return data_ym / prod_type / param_key / TFXM_RATE / AVG_TFXM / rpt_col
        /// </summary>
        /// <param name="as_fm_ymd"></param>
        /// <param name="as_to_ymd"></param>
        /// <param name="as_type"></param>
        /// <returns></returns>
        public DataTable d_30690_high_low(string as_fm_ymd,
                                            string as_to_ymd,
                                            string as_type) {

            object[] parms = {
                ":as_fm_ymd", as_fm_ymd,
                ":as_to_ymd", as_to_ymd,
                ":as_type", as_type
            };


            string sql = @"
SELECT rpt_ym as data_ym,
    rpt_prod_type as prod_type,
    rpt_param_key as param_key,
    nvl(TFXM_RATE,0) as TFXM_RATE,
    nvl(AVG_TFXM,0) as AVG_TFXM,
    rpt_col
FROM
   (--每月資料
    SELECT AI6_PROD_TYPE as prod_type,TRIM(AI6_KIND_ID) as param_key,TO_CHAR(AI6_DATE,'YYYYMM') as data_ym,
           NVL(round(STDDEV(case when :as_type = 'F' then AI6_LN_RETURN else AI6_TFXM_LN_RETURN end) * SQRT(256),4),0)* 100 as TFXM_RATE,
           round(AVG(case when :as_type = 'F' then AI6_HIGH_LOW else AI6_TFXM_HIGH_LOW end),6) as AVG_TFXM
      FROM ci.AI6 
     WHERE AI6_DATE >= to_date(substr(:as_fm_ymd,1,4)||'0101','yyyymmdd')
       AND AI6_DATE <= to_date(:as_to_ymd,'yyyymmdd')
       AND AI6_KIND_ID = AI6_PARAM_KEY 
       AND AI6_PROD_TYPE IN ('F','O') 
     GROUP BY AI6_PROD_TYPE,AI6_KIND_ID,TO_CHAR(AI6_DATE,'YYYYMM')
     UNION ALL
   --年度資料
    SELECT AI6_PROD_TYPE as prod_type,TRIM(AI6_KIND_ID) as kind_id,TO_CHAR(AI6_DATE,'YYYY') as data_ym,
           NVL(round(STDDEV(case when :as_type = 'F' then AI6_LN_RETURN else AI6_TFXM_LN_RETURN end) * SQRT(256),4),0)* 100 as TFXM_RATE,
           round(AVG(case when :as_type = 'F' then AI6_HIGH_LOW else AI6_TFXM_HIGH_LOW end),6) as AVG_TFXM
      FROM ci.AI6 
     WHERE AI6_DATE >= to_date(substr(:as_to_ymd,1,4)||'0101','yyyymmdd')
       AND AI6_DATE <= to_date(:as_to_ymd,'yyyymmdd')
       AND AI6_KIND_ID = AI6_PARAM_KEY 
       AND AI6_PROD_TYPE IN ('F','O') 
     GROUP BY AI6_PROD_TYPE,AI6_KIND_ID,TO_CHAR(AI6_DATE,'YYYY')),

   (--每月資料
    SELECT rpt_ym,rpt_col,PDK_PROD_TYPE as rpt_prod_type,trim(PDK_PARAM_KEY) as rpt_param_key
      FROM 
          (SELECT rpt_ym,rpt_col
             FROM
                 (SELECT SUBSTR(OCF_YMD,1,6) as rpt_ym,
                             12 - ROW_NUMBER( ) OVER ( ORDER BY SUBSTR(OCF_YMD,1,6) desc NULLS LAST) + 1 as rpt_col
                  FROM CI.AOCF 
                 WHERE OCF_YMD >= substr(:as_fm_ymd,1,4)||'0101' 
                   AND OCF_YMD <= :as_to_ymd
                 GROUP BY SUBSTR(OCF_YMD,1,6)
                 ORDER BY  2)
           WHERE rpt_col > 0
           UNION ALL
          SELECT substr(:as_to_ymd,1,4),13 
            FROM DUAL) R,
         (SELECT PDK_PROD_TYPE,PDK_PARAM_KEY from ci.HPDK
           WHERE PDK_DATE >= TO_DATE(:as_fm_ymd,'YYYYMMDD')
             AND PDK_DATE <= TO_DATE( :as_to_ymd,'YYYYMMDD')
             AND PDK_PROD_TYPE = 'F' AND PDK_SUBTYPE IN ('I','E') 
             AND PDK_PARAM_KEY NOT IN ('MXF')
           GROUP BY PDK_PROD_TYPE,PDK_PARAM_KEY
           UNION ALL
          SELECT 'F','MSF' FROM DUAL) P )

WHERE rpt_ym = data_ym(+)
and rpt_prod_type = prod_type(+)
and rpt_param_key = param_key(+)
order by prod_type , param_key , data_ym
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// 交易人結構 return 7 fields
        /// </summary>
        /// <param name="as_fm">yyyyMMdd</param>
        /// <param name="as_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_day_am21(string as_fm,
                                          string as_to) {

            object[] parms = {
                ":as_fm", as_fm,
                ":as_to", as_to
            };

            string sql = @"
SELECT PDK_PROD_TYPE as prod_type,
  trim(PDK_PARAM_KEY) as param_key,
  nvl(tot,0) as tot,
  nvl(m1,0) as m1, 
  nvl(m2,0) as m2,
  nvl(m3,0) as m3,
  nvl(m4,0) as m4
FROM
  (select PDK_PROD_TYPE,PDK_PARAM_KEY from ci.HPDK
     WHERE PDK_DATE >= TO_DATE(:as_fm,'YYYYMMDD')
       AND PDK_DATE >= TO_DATE(:as_to,'YYYYMMDD')
     GROUP BY PDK_PROD_TYPE,PDK_PARAM_KEY) P,
  (select AM21_PROD_TYPE as prod_type,AM21_PARAM_KEY AS param_key,
        sum(AM21_M_QNTY) as tot,
        sum(case when AM21_IDFG_TYPE = '7' then AM21_M_QNTY  else 0 end) as m1,
        sum(case when AM21_IDFG_TYPE = '8' then AM21_M_QNTY  else 0 end) as m2,
        sum(case when AM21_IDFG_TYPE = '3' then AM21_M_QNTY  else 0 end) as m3,
        sum(case when AM21_IDFG_TYPE = '6' then AM21_M_QNTY  else 0 end) as m4
   from ci.AM21
  where AM21_YMD >=  :as_fm
    and AM21_YMD <=  :as_to
    and AM21_SUM_SUBTYPE = '3'
    and AM21_SUM_TYPE = 'D'
  group by AM21_PROD_TYPE,AM21_PARAM_KEY) A
WHERE PDK_PROD_TYPE = prod_type(+)
AND PDK_PARAM_KEY = param_key(+)
order by prod_type , param_key
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// 盤後交易人結構 return 7 fields
        /// </summary>
        /// <param name="as_fm">yyyyMMdd</param>
        /// <param name="as_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_night_am21(string as_fm,
                                            string as_to) {

            object[] parms = {
                ":as_fm", as_fm,
                ":as_to", as_to
            };

            string sql = @"
SELECT PDK_PROD_TYPE as prod_type,
  trim(PDK_PARAM_KEY) as param_key,
  nvl(tot,0) as tot,
  nvl(m1,0) as m1, 
  nvl(m2,0) as m2,
  nvl(m3,0) as m3,
  nvl(m4,0) as m4
FROM
  (select PDK_PROD_TYPE,PDK_PARAM_KEY from ci.HPDK
    WHERE PDK_DATE >= TO_DATE(:as_fm,'YYYYMMDD')
    AND PDK_DATE >= TO_DATE(:as_to,'YYYYMMDD')
    AND NOT PDK_AH_MARKET_OPEN IS NULL
    GROUP BY PDK_PROD_TYPE,PDK_PARAM_KEY) P,
  (select AM21_PROD_TYPE as prod_type,AM21_PARAM_KEY AS param_key,
    sum(AM21_AH_M_QNTY) as tot,
    sum(case when AM21_IDFG_TYPE = '7' then AM21_AH_M_QNTY  else 0 end) as m1,
    sum(case when AM21_IDFG_TYPE = '8' then AM21_AH_M_QNTY  else 0 end) as m2,
    sum(case when AM21_IDFG_TYPE = '3' then AM21_AH_M_QNTY  else 0 end) as m3,
    sum(case when AM21_IDFG_TYPE = '6' then AM21_AH_M_QNTY  else 0 end) as m4
    from ci.AM21,
    (select APDK_PARAM_KEY from ci.APDK where APDK_MARKET_CODE = '1' group by APDK_PARAM_KEY)
    where AM21_YMD >= :as_fm
    and AM21_YMD <=  :as_to
    and AM21_SUM_SUBTYPE = '3'
    and AM21_SUM_TYPE = 'D'
    and AM21_PARAM_KEY = APDK_PARAM_KEY
    group by AM21_PROD_TYPE,AM21_PARAM_KEY) A
WHERE PDK_PROD_TYPE = prod_type(+)
AND PDK_PARAM_KEY = param_key(+)
order by prod_type , param_key
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// TX波動及振福 return OCF_YMD,AI6_HIGH_LOW,VIX_AVG_VALUE,AMIF_SUM_AMT
        /// </summary>
        /// <param name="as_fm_ymd">yyyyMMdd</param>
        /// <param name="as_to_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_first(string as_fm_ymd,
                                       string as_to_ymd) {

            object[] parms = {
                ":as_fm_ymd", as_fm_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql = @"
SELECT OCF_YMD,AI6_HIGH_LOW,VIX_AVG_VALUE,AMIF_SUM_AMT
  FROM 
       --交易天數
      (select OCF_YMD
         from ci.AOCF
        where OCF_YMD >= substr( :as_fm_ymd,1,4)||'0101'
          and OCF_YMD <=  :as_to_ymd) D,
       --2.振幅 
      (SELECT to_char(AI6_DATE,'YYYYMMDD') as AI6_YMD,AI6_HIGH_LOW
         FROM ci.AI6
        WHERE AI6_DATE >= TO_DATE(substr( :as_fm_ymd,1,4)||'0101','YYYYMMDD')
          AND AI6_DATE <= TO_DATE( :as_to_ymd,'YYYYMMDD')
          AND AI6_KIND_ID = 'TXF') I,
       --3.波動率指數
      (SELECT VIX_YMD,round(AVG(VIX_VALUE),4) as VIX_AVG_VALUE
         FROM ci.VIX
        WHERE VIX_YMD >= substr( :as_fm_ymd,1,4)||'0101'
          AND VIX_YMD <=  :as_to_ymd
          AND VIX_TYPE = 'N'
        GROUP BY VIX_YMD) V,
       --4.成交值
      (SELECT to_char(AMIF_DATE,'YYYYMMDD') as AMIF_YMD,AMIF_SUM_AMT
         FROM ci.AMIF
        WHERE AMIF_DATE >= TO_DATE(substr( :as_fm_ymd,1,4)||'0101','YYYYMMDD')
          AND AMIF_DATE <= TO_DATE( :as_to_ymd,'YYYYMMDD')
          AND AMIF_PROD_ID = 'TXF00') T
 where OCF_YMD = AI6_YMD(+)
   and OCF_YMD = VIX_YMD(+)
   and OCF_YMD = AMIF_YMD(+)
order by ocf_ymd
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// 加掛小型股票期貨交易概況(日均量) return 7 fields
        /// </summary>
        /// <param name="as_last_fm">yyyyMMdd</param>
        /// <param name="as_last_to">yyyyMMdd</param>
        /// <param name="as_this_fm">yyyyMMdd</param>
        /// <param name="as_this_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_6(string as_last_fm,
                                   string as_last_to,
                                   string as_this_fm,
                                   string as_this_to) {

            object[] parms = {
                ":as_1_fm_ymd", as_last_fm,
                ":as_1_to_ymd", as_last_to,
                ":as_2_fm_ymd", as_this_fm,
                ":as_2_to_ymd", as_this_to,
            };

            string sql = @"
select grp_id2,kind_id2,p_name,
   --本週2000股
   case when day_cnt_week = 0 then 0 else round((qnty_week_2000 / day_cnt_week),0) end as avg_qnty_week_2000,
   --本週100股
   case when day_cnt_week = 0 then 0 else round((qnty_week_100 / day_cnt_week),0) end as avg_qnty_week_100,
   --上週2000股
   case when day_cnt_last = 0 then 0 else round((qnty_last_2000 / day_cnt_last),0) end as avg_qnty_last_2000,
   --上週100股
   case when day_cnt_last = 0 then 0 else round((qnty_last_100 / day_cnt_last),0) end as avg_qnty_last_100
from 
   --成交合計
  (select m_kind_grp2 as grp_id2,m_kind_id2 as kind_id2,
          --本週
          sum(case when AI2_YMD >=  :as_2_fm_ymd AND AI2_ymd <=  :as_2_to_ymd and AI2_KIND_ID2 = m_kind_grp2 then AI2_M_QNTY else 0 end) as qnty_week_2000,
          sum(case when AI2_YMD >=  :as_2_fm_ymd AND AI2_ymd <=  :as_2_to_ymd and AI2_KIND_ID2 = m_kind_id2  then AI2_M_QNTY else 0 end) as qnty_week_100,
          --上週
          sum(case when AI2_YMD >=  :as_1_fm_ymd AND AI2_ymd <=  :as_1_to_ymd and AI2_KIND_ID2 = m_kind_grp2 then AI2_M_QNTY else 0 end) as qnty_last_2000,
          sum(case when AI2_YMD >=  :as_1_fm_ymd AND AI2_ymd <=  :as_1_to_ymd and AI2_KIND_ID2 = m_kind_id2  then AI2_M_QNTY else 0 end) as qnty_last_100
     from ci.AI2,
         (select APDK_KIND_GRP2 as m_kind_grp2,APDK_KIND_ID2 as m_kind_id2
            from ci.APDK
           where APDK_REMARK = 'M'
           group by APDK_KIND_ID2,APDK_KIND_GRP2) p 
    where AI2_SUM_TYPE = 'D'
      and AI2_SUM_SUBTYPE = '4'
      and AI2_PROD_TYPE = 'F'
      and AI2_PROD_SUBTYPE = 'S'
      and AI2_YMD >=  :as_1_fm_ymd
      and AI2_YMD <=  :as_2_to_ymd
      and (AI2_KIND_ID2 = m_kind_grp2 or AI2_KIND_ID2 = m_kind_id2)
    group by m_kind_grp2,m_kind_id2) T,
    --商品名稱
   (select APDK_KIND_GRP2 as p_kind_grp2,APDK_KIND_ID2 as p_kind_id2,
           substr(substr(max(APDK_NAME),1,GREATEST(INSTR(max(APDK_NAME),'期貨', 1, 1),INSTR(max(APDK_NAME),'選擇權', 1, 1)) - 1),INSTR(max(APDK_NAME),'小型', 1, 1)+ 2, 99) as p_name
      from ci.APDK
     where APDK_REMARK = 'M'
     group by APDK_KIND_ID2,APDK_KIND_GRP2) p,
   --本週交易日期  
  (select sum(case when OCF_YMD >=  :as_1_fm_ymd AND OCF_ymd <=  :as_1_to_ymd then 1 else 0 end) as day_cnt_last,
          sum(case when OCF_YMD >=  :as_2_fm_ymd AND OCF_ymd <=  :as_2_to_ymd then 1 else 0 end) as day_cnt_week
     from ci.AOCF
    where OCF_YMD >=  :as_1_fm_ymd
      and OCF_YMD <=  :as_2_to_ymd) D
where trim(grp_id2) = trim(p_kind_grp2(+))
order by 1
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// 新上市個股選擇權交易概況(日均量) return p_name,avg_qnty_week,avg_qnty_last
        /// </summary>
        /// <param name="as_last_fm">yyyyMMdd</param>
        /// <param name="as_last_to">yyyyMMdd</param>
        /// <param name="as_this_fm">yyyyMMdd</param>
        /// <param name="as_this_to">yyyyMMdd</param>
        /// <param name="prodList">KIND_ID2 in 必須要有值</param>
        /// <returns></returns>
        public DataTable d_30690_7(string as_last_fm,
                                   string as_last_to,
                                   string as_this_fm,
                                   string as_this_to,
                                   List<string> prodList) {

            object[] parms = {
                ":as_1_fm_ymd", as_last_fm,
                ":as_1_to_ymd", as_last_to,
                ":as_2_fm_ymd", as_this_fm,
                ":as_2_to_ymd", as_this_to,
            };

            if (prodList.Count <= 0) {
                throw new System.Exception("d_30690_7傳入的參數prodList必須要有值");
            }

            string as_kind = "'" + string.Join("','", prodList) + "'";


            string sql = string.Format(@"
select p_name,
  --本週
  case when day_cnt_week = 0 then 0 else round((qnty_week / day_cnt_week),1) end as avg_qnty_week,
  --上週
  case when day_cnt_last = 0 then 0 else round((qnty_last / day_cnt_last),1) end as avg_qnty_last
from 
  --成交合計
  (select AI2_KIND_ID2,
    --本週
    sum(case when AI2_YMD >=  :as_2_fm_ymd AND AI2_ymd <=  :as_2_to_ymd then AI2_M_QNTY else 0 end) as qnty_week,
    --上週
    sum(case when AI2_YMD >=  :as_1_fm_ymd AND AI2_ymd <=  :as_1_to_ymd then AI2_M_QNTY else 0 end) as qnty_last
    from ci.AI2
    where AI2_SUM_TYPE = 'D'
    and AI2_SUM_SUBTYPE = '4'
    and AI2_PROD_TYPE = 'O'
    and AI2_PROD_SUBTYPE = 'S'
    and AI2_YMD >=  :as_1_fm_ymd
    and AI2_YMD <=  :as_2_to_ymd
    and AI2_KIND_ID2 in ({0})
    group by AI2_KIND_ID2) T,
  --商品名稱
  (select APDK_KIND_ID2 as p_kind_id2,
    substr(max(APDK_NAME),1,INSTR(max(APDK_NAME),'選擇權', 1,1) - 1) as p_name
    from ci.APDK
    where APDK_PROD_TYPE = 'O'
    and APDK_PROD_SUBTYPE = 'S'
    and APDK_KIND_ID2  in ({0})
    group by APDK_KIND_ID2) p,
  --本週交易日期  
  (select sum(case when OCF_YMD >=  :as_2_fm_ymd AND OCF_ymd <=  :as_2_to_ymd then 1 else 0 end) as day_cnt_week,
    sum(case when OCF_YMD >=  :as_1_fm_ymd AND OCF_ymd <=  :as_1_to_ymd then 1 else 0 end) as day_cnt_last
    from ci.AOCF
    where OCF_YMD >=  :as_1_fm_ymd
    and OCF_YMD <=  :as_2_to_ymd) D
    where AI2_KIND_ID2 = p_kind_id2
order by AI2_KIND_ID2
", as_kind);

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 歐臺期及歐臺選日均量 return 6 fields
        /// </summary>
        /// <param name="as_fm_ymd">yyyyMMdd</param>
        /// <param name="as_to_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_second(string as_fm_ymd,
                                        string as_to_ymd) {

            object[] parms = {
                ":as_fm_ymd", as_fm_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql = @"
select case when day_cnt_ae2 = 0 then 0 else round(qnty_f /day_cnt_ae2 ,0) end as eurex_f ,
       case when day_cnt_ae2 = 0 then 0 else round(qnty_o /day_cnt_ae2 ,0) end as eurex_o ,
       case when day_cnt_ae2 = 0 then 0 else round(qnty_tot /day_cnt_ae2,0) end as eurex_tot ,
       case when qnty_tot = 0 then 0 else round(oi_tot / qnty_tot * 100,2) end as oi_rate,
       case when day_cnt_ae6 = 0 then 0 else round(TOT_HIGH_LOW / day_cnt_ae6,0) end as high_low,
       AE6_RATE * 100 as rate
  from
       --交易量
      (select sum(case when AE2_PROD_TYPE = 'F' then AE2_M_QNTY else 0 end) as qnty_f,
              sum(case when AE2_PROD_TYPE = 'O' then AE2_M_QNTY else 0 end) as qnty_o,
              sum(AE2_OI) as oi_tot,
              sum(AE2_M_QNTY) as qnty_tot,
              count(distinct AE2_YMD) as day_cnt_ae2 
         from ci.AE2
        where AE2_YMD >=  :as_fm_ymd
          and AE2_YMD <=  :as_to_ymd
          and AE2_SUM_TYPE = 'D'
          and AE2_SUM_SUBTYPE = '1') E2,
       --振幅及波動度
      (select round(STDDEV(AE6_LN_RETURN) * 16,6) as AE6_RATE,
              SUM(AE6_HIGH_LOW) as TOT_HIGH_LOW,
              COUNT(DISTINCT AE6_YMD) as day_cnt_ae6
         from CI.AE6
        where AE6_YMD >=  :as_fm_ymd
          and AE6_YMD <=  :as_to_ymd
          and  not AE6_LN_RETURN is null
          and AE6_KIND_ID = 'TXF') E6
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// 人民幣匯率期貨及選擇權交易量 return 11 fields
        /// </summary>
        /// <param name="as_this_to">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_third(string as_this_to) {

            object[] parms = {
                ":as_2_to_ymd", as_this_to
            };

            string sql = @"
select 
    (case when data_ymd = '999999' then '上市至今日均量交易' 
          when data_type = 'M' and data_ymd <> '999999' then to_char(to_date(data_ymd,'yyyymm'),'yyyy/mm')||' 日均量' 
          else to_char(to_date(data_ymd,'yyyymmdd'),'yyyy/mm/dd') end) as data_ymd,
    avg_CME,avg_HKEX,avg_SGX,avg_RHF,avg_RTF,avg_R_F,avg_RHO,avg_RTO,avg_R_O
from (    
    --月統計資料
    select 'M' as data_type,
       ai2_ym as data_ymd,
       case when day_cnt_CME = 0 then 0 else round(vol_CME / day_cnt_CME,0) end as avg_CME,
       case when day_cnt_HKEX = 0 then 0 else round(vol_HKEX / day_cnt_HKEX,0) end as avg_HKEX,
       case when day_cnt_SGX = 0 then 0 else round(vol_SGX / day_cnt_SGX,0) end as avg_SGX,
       case when day_cnt_R_F = 0 then 0 else round(qnty_RHF / day_cnt_R_F,0) end as avg_RHF,
       case when day_cnt_R_F = 0 then 0 else round(qnty_RTF / day_cnt_R_F,0) end as avg_RTF,
       case when day_cnt_R_F = 0 then 0 else round(qnty_R_F / day_cnt_R_F,0) end as avg_R_F,
       case when day_cnt_R_O = 0 then 0 else round(qnty_RHO / day_cnt_R_O,0) end as avg_RHO,
       case when day_cnt_R_O = 0 then 0 else round(qnty_RTO / day_cnt_R_O,0) end as avg_RTO,
       case when day_cnt_R_O = 0 then 0 else round(qnty_R_O / day_cnt_R_O,0) end as avg_R_O
    from       
      --期貨及選擇權成交量
      (
       --前年度每月
       select substr(AI2_YMD,1,6) as ai2_ym,
              sum(case when AI2_PARAM_KEY = 'RHF' then AI2_M_QNTY else 0 end) as qnty_RHF,
              sum(case when AI2_PARAM_KEY = 'RTF' then AI2_M_QNTY else 0 end) as qnty_RTF,
              sum(case when AI2_PARAM_KEY in ('RTF','RHF') then AI2_M_QNTY else 0 end) as qnty_R_F,
              count(distinct (case when AI2_PARAM_KEY in ('RTF','RHF') then AI2_YMD else null end)) as day_cnt_R_F,
              sum(case when AI2_PARAM_KEY = 'RHO' then AI2_M_QNTY else 0 end) as qnty_RHO,
              sum(case when AI2_PARAM_KEY = 'RTO' then AI2_M_QNTY else 0 end) as qnty_RTO,
              sum(case when AI2_PARAM_KEY in ('RTO','RHO') then AI2_M_QNTY else 0 end) as qnty_R_O,
              count(distinct (case when AI2_PARAM_KEY in ('RTO','RHO') then AI2_YMD else null end)) as day_cnt_R_O
         from ci.AI2
        where AI2_YMD >= to_char(to_number(substr( :as_2_to_ymd,1,4)) - 1)||'0101'
          and AI2_YMD <=  :as_2_to_ymd
          and AI2_SUM_TYPE = 'D'
          and AI2_SUM_SUBTYPE = '3'
          and AI2_PARAM_KEY in ('RHF','RTF','RHO','RTO')
        group by substr(AI2_YMD,1,6)
        UNION ALL
       --上市至今 
       select '999999' as ai2_ym,
              sum(case when AI2_PARAM_KEY = 'RHF' then AI2_M_QNTY else 0 end) as qnty_RHF,
              sum(case when AI2_PARAM_KEY = 'RTF' then AI2_M_QNTY else 0 end) as qnty_RTF,
              sum(case when AI2_PARAM_KEY in ('RTF','RHF') then AI2_M_QNTY else 0 end) as qnty_R_F,
              count(distinct (case when AI2_PARAM_KEY in ('RTF','RHF') then AI2_YMD else null end)) as day_cnt_R_F,
              sum(case when AI2_PARAM_KEY = 'RHO' then AI2_M_QNTY else 0 end) as qnty_RHO,
              sum(case when AI2_PARAM_KEY = 'RTO' then AI2_M_QNTY else 0 end) as qnty_RTO,
              sum(case when AI2_PARAM_KEY in ('RTO','RHO') then AI2_M_QNTY else 0 end) as qnty_R_O,
              count(distinct (case when AI2_PARAM_KEY in ('RTO','RHO') then AI2_YMD else null end)) as day_cnt_R_O
         from ci.AI2
        where AI2_YMD >= '20150720' --RHF,RTF-20150720上市,RHO,RTO-20160627上市
          and AI2_YMD <=  :as_2_to_ymd
          and AI2_SUM_TYPE = 'D'
          and AI2_SUM_SUBTYPE = '3'
          and AI2_PARAM_KEY in ('RHF','RTF','RHO','RTO')) A,
      --現貨成交量        
      (
       --前年度每月
       select substr(AM12_YMD,1,6) as vol_ym,
              sum(case when AM12_F_ID = 'CME' then AM12_VOL else 0 end) as vol_CME,               
              sum(case when AM12_F_ID = 'HKEX' then AM12_VOL else 0 end) as vol_HKEX,
              sum(case when AM12_F_ID = 'SGX' then AM12_VOL else 0 end) as vol_SGX,
              sum((case when AM12_F_ID = 'CME' then 1 else 0 end)) as day_cnt_CME,
              sum( (case when AM12_F_ID = 'HKEX' then 1 else 0 end)) as day_cnt_HKEX,
              sum( (case when AM12_F_ID = 'SGX' then 1 else 0 end)) as day_cnt_SGX  
         from ci.AM12 
        where AM12_YMD >= to_char(to_number(substr( :as_2_to_ymd,1,4)) - 1)||'0101'
          and AM12_YMD <=  :as_2_to_ymd
        group by substr(AM12_YMD,1,6)
        UNION ALL
       --上市至今 
       select '999999' as vol_ym,
              sum(case when AM12_F_ID = 'CME' then AM12_VOL else 0 end) as vol_CME,               
              sum(case when AM12_F_ID = 'HKEX' then AM12_VOL else 0 end) as vol_HKEX,
              sum(case when AM12_F_ID = 'SGX' then AM12_VOL else 0 end) as vol_SGX,
              sum((case when AM12_F_ID = 'CME' then 1 else 0 end)) as day_cnt_CME,
              sum( (case when AM12_F_ID = 'HKEX' then 1 else 0 end)) as day_cnt_HKEX,
              sum( (case when AM12_F_ID = 'SGX' then 1 else 0 end)) as day_cnt_SGX  
         from ci.AM12 
        where AM12_YMD >= '20150720'
          and AM12_YMD <=  :as_2_to_ymd) S
    where ai2_ym = vol_ym(+)
    
    union all
    --每日資料 
    select 'D' as data_type,
       AI2_YMD as data_ymd,
       CME_vol,
       HKEX_vol,
       SGX_vol,
       qnty_RHF,
       qnty_RTF,
       qnty_R_F,
       qnty_RHO,
       qnty_RTO,
       qnty_R_O
    from       
      --期貨及選擇權成交量
      (select AI2_YMD, 
              sum(case when AI2_PARAM_KEY = 'RHF' then AI2_M_QNTY else 0 end) as qnty_RHF,
              sum(case when AI2_PARAM_KEY = 'RTF' then AI2_M_QNTY else 0 end) as qnty_RTF,
              sum(case when AI2_PARAM_KEY in ('RTF','RHF') then AI2_M_QNTY else 0 end) as qnty_R_F,
              sum(case when AI2_PARAM_KEY = 'RHO' then AI2_M_QNTY else 0 end) as qnty_RHO,
              sum(case when AI2_PARAM_KEY = 'RTO' then AI2_M_QNTY else 0 end) as qnty_RTO,
              sum(case when AI2_PARAM_KEY in ('RTO','RHO') then AI2_M_QNTY else 0 end) as qnty_R_O
         from ci.AI2
        where AI2_YMD >= substr( :as_2_to_ymd,1,6)||'01'
          and AI2_YMD <=  :as_2_to_ymd
          and AI2_SUM_TYPE = 'D'
          and AI2_SUM_SUBTYPE = '3'
          and AI2_PARAM_KEY in ('RHF','RTF','RHO','RTO')
        group by AI2_YMD) A,
      (select AM12_YMD ,
              sum(case when AM12_F_ID = 'CME' then AM12_VOL else 0 end) as CME_vol,               
              sum(case when AM12_F_ID = 'HKEX' then AM12_VOL else 0 end) as HKEX_vol,
              sum(case when AM12_F_ID = 'SGX' then AM12_VOL else 0 end) as SGX_vol 
         from ci.AM12 
        where AM12_YMD >= substr( :as_2_to_ymd,1,6)||'01'
          and AM12_YMD <=  :as_2_to_ymd
        group by AM12_YMD) S
    where AI2_YMD = AM12_YMD(+)
    order by data_type desc ,data_ymd
) t 
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 每月波動及振幅(明細) for admin test
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30690_high_low_detial(string as_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd
            };

            string sql = @"
select to_char(ai6_date,'yyyymmdd hh24:mi:ss') as ""日期"",
ai6_prod_type as ""F期貨/O選擇權"",
trim(ai6_kind_id) as ""商品"",
ai6_tfxm_close_price as ""現貨收盤價"",
ai6_tfxm_high_low as ""現貨振幅"",
ai6_tfxm_ln_return as ""現貨Ln"",
ai6_close_price as ""期貨收盤價"",
ai6_high_low as ""期貨振幅"",
ai6_ln_return as ""期貨Ln""
from ci.ai6 
where ai6_date >= to_date(substr(:as_ymd,1,4)||'0101','yyyymmdd')
and ai6_date <= to_date(:as_ymd,'yyyymmdd')
and ai6_kind_id = ai6_param_key 
and ai6_prod_type in ('F','O')   
order by ai6_date , ai6_prod_type , ai6_kind_id
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
