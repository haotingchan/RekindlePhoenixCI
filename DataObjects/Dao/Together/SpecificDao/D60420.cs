using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Lukas, 2019/1/15
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D60420: DataGate {
        /// <summary>
        /// d_60410_1a
        /// </summary>
        /// <param name="as_sdate"></param>
        /// <param name="as_edate"></param>
        /// <returns></returns>
        public DataTable d_60410_1a(DateTime as_sdate, DateTime as_edate) {

            object[] parms = {
                ":as_sdate",as_sdate,
                ":as_edate",as_edate
            };

            string sql =
                @"
select TSE5_YMD as ymd,
       TSE5_IDSTK_INDEX AS idstk_index,
       TSE5_TOT_CNT as tot_cnt,
       TSE5_MAX_WEIGHT as max_weight ,
       TSE5_TOP5_WEIGHT as top5,
       TSE5_25_CNT as cnt25,
       TSE5_25_WEIGHT as weight25,
       TSE5_25_AVG6M_CLS_TW as avg_amt_cls_tw,
       TSE5_25_AVG6M_MTH_TW as avg_amt_mth_tw,                        
       TSE5_25_AVG6M_CLS_USD as avg_amt_cls_usd,
       TSE5_25_AVG6M_MTH_USD as avg_amt_mth_usd,
       TSE5_25_DAY_CLS_TW as day_amt_cls_tw,
       TSE5_25_DAY_MTH_TW as day_amt_mth_tw,                        
       TSE5_25_DAY_CLS_USD as day_amt_cls_usd,
       TSE5_25_DAY_MTH_USD as day_amt_mth_usd,
       TSE5_PID as PID,
       TSE5_USD_EXCHANGE_RATE as day_exchange_rate,
       RPT_SEQ_NO,
       COD_NAME,
       TSE5_AVG6M_CNT as m_day_cnt
from ci.TSE5,   
    --順序及指數名稱
    (SELECT TRIM(RPT_VALUE_2) AS RPT_PID,SUBSTR(RPT_VALUE,1,4) AS RPT_INDEX,RPT_SEQ_NO ,COD_NAME 
        FROM CI.RPT ,
            (SELECT SUBSTR(COD_ID,1,1) AS COD_PID,TRIM(SUBSTR(COD_ID,3,4)) AS COD_INDEX,COD_DESC AS COD_NAME 
                FROM ci.COD
                WHERE COD_TXN_ID = '60410') C
        WHERE RPT_TXN_ID = '60410' and RPT_TXD_ID = '60410_1a'
        AND TRIM(RPT_VALUE_2) = COD_PID(+)
        AND TRIM(RPT_VALUE) = COD_INDEX(+)) R                       
where TSE5_YMD >= to_char(:as_sdate,'YYYYMMDD')
  and TSE5_YMD <=  to_char(:as_edate,'YYYYMMDD')
  and TSE5_PID = RPT_PID(+)
  and TSE5_IDSTK_INDEX = RPT_INDEX(+)
order by rpt_seq_no, ymd
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// d_60410_1achk
        /// </summary>
        /// <param name="as_sdate"></param>
        /// <param name="as_edate"></param>
        /// <returns></returns>
        public DataTable d_60410_1achk(DateTime as_sdate, DateTime as_edate) {

            object[] parms = {
                ":as_sdate",as_sdate,
                ":as_edate",as_edate
            };

            string sql =
                @"
Select a.*
from
    (select to_char(TSE3_DATE,'YYYYMMDD') as ymd,
        TSE3_IDSTK_INDEX,
        tot_cnt,
        ROUND(max_weight / 100 ,4) as max_weight ,
        ROUND(top5 / 100 ,4) as top5,
        cnt25,
        ROUND(weight25  / 100,4) as weight25,
        avg_amt_cls_tw,
        avg_amt_mth_tw,                        
        avg_amt_cls_usd,
        avg_amt_mth_usd,
        day_amt_cls_tw,
        day_amt_mth_tw,                        
        day_amt_cls_usd,
        day_amt_mth_usd,
        TSE3_PID,
        day_exchange_rate,
        RPT_SEQ_NO,COD_NAME,
        m_day_cnt
    from 
        --每日成份股
        (select TSE3_DATE,
                TSE3_PID,TSE3_IDSTK_INDEX,
                max(TSE3_DESC_SEQ) as tot_cnt,
                max(TSE3_INDEX_WEIGHT) as max_weight,
                sum(case when TSE3_DESC_SEQ <= 5 then TSE3_INDEX_WEIGHT else 0 end) as top5,
                max(case when TSE3_ACCU_WEIGHT - TSE3_INDEX_WEIGHT < 25 then TSE3_ASC_SEQ else 0 end) as cnt25,
                sum(case when TSE3_ACCU_WEIGHT - TSE3_INDEX_WEIGHT < 25 then TSE3_INDEX_WEIGHT else 0 end) as weight25
            from ci.TSE3                      
            where TSE3_DATE >= :as_sdate        
              and TSE3_DATE <= :as_edate   
              AND ((TSE3_PID = 1 AND TSE3_IDSTK_INDEX IN ('0000','13','17','A07'))  OR
                  (TSE3_PID = 2 AND TSE3_IDSTK_INDEX IN ('4000','22')))
            group by TSE3_DATE,TSE3_PID,TSE3_IDSTK_INDEX) T,
        --最低百分之25成份股  
        (select t_date,start_date,m_day_cnt,m_pid,m_index,
                sum(case when m_date = t_date then m_amt_cls_tw else 0 end) as day_amt_cls_tw,
                sum(case when m_date = t_date then m_amt_mth_tw else 0 end) as day_amt_mth_tw,
                sum(case when m_date = t_date then m_amt_cls_usd else 0 end) as day_amt_cls_usd,
                sum(case when m_date = t_date then m_amt_mth_usd else 0 end) as day_amt_mth_usd,
                sum(case when m_date = t_date then HEXRT_EXCHANGE_RATE else 0 end) as day_exchange_rate,
                round(case when m_day_cnt > 0 then sum(m_amt_cls_tw) / m_day_cnt else 0 end,0) as avg_amt_cls_tw,
                round(case when m_day_cnt > 0 then sum(m_amt_mth_tw) / m_day_cnt else 0 end,0) as avg_amt_mth_tw,
                round(case when m_day_cnt > 0 then sum(m_amt_cls_usd) / m_day_cnt else 0 end,0) as avg_amt_cls_usd,
                round(case when m_day_cnt > 0 then sum(m_amt_mth_usd) / m_day_cnt else 0 end,0) as avg_amt_mth_usd 
            from      
                (SELECT TO_DATE(T_YMD,'YYYYMMDD') AS t_date,
                        TO_DATE(START_YMD,'YYYYMMDD') AS start_date,
                        COUNT(*) AS m_day_cnt
                FROM
                    (SELECT OCF_YMD AS T_YMD,
                            TO_CHAR(ci.RelativeDate(To_Date(OCF_YMD,'yyyymmdd'),6,'MONTH'),'YYYYMMDD') as START_YMD
                        FROM ci.AOCF T
                        WHERE T.OCF_YMD >= TO_CHAR(:as_sdate,'YYYYMMDD') 
                          AND T.OCF_YMD <= TO_CHAR(:as_edate,'YYYYMMDD')) O,
                        ci.AOCF 
                WHERE OCF_YMD <= T_YMD
                  AND OCF_YMD >= START_YMD
                GROUP BY T_YMD,START_YMD) O, 
                (select TSE3_DATE as m_date,TSE3_PID as m_pid,TSE3_IDSTK_INDEX as m_index,
                        sum(NVL(TSE4_CLS_AMT,0)) as m_amt_cls_tw,
                        sum(NVL(TSE4_MTH_AMT,0)) as m_amt_mth_tw,
                        HEXRT_EXCHANGE_RATE,
                        round(sum(NVL(TSE4_CLS_AMT,0))/ HEXRT_EXCHANGE_RATE ,2) as m_amt_cls_usd,
                        round(sum(NVL(TSE4_MTH_AMT,0))/ HEXRT_EXCHANGE_RATE ,2) as m_amt_mth_usd
                from ci.TSE3,ci.TSE4,CI.HEXRT
                where TSE3_DATE >= ci.RelativeDate(:as_sdate,6,'MONTH')
                  AND TSE3_DATE <= :as_edate
                  AND TSE3_ACCU_WEIGHT - TSE3_INDEX_WEIGHT < 25
                  AND ((TSE3_PID = 1 AND TSE3_IDSTK_INDEX IN ('0000','13','17','A07'))  OR
                      (TSE3_PID = 2 AND TSE3_IDSTK_INDEX IN ('4000','22')))
                  AND TSE3_DATE = TSE4_DATE(+) 
                  AND TSE3_PID = TSE4_PID(+)
                  AND TSE3_SID = TSE4_SID(+)
                  AND TSE3_DATE = HEXRT_DATE 
                  AND HEXRT_CURRENCY_TYPE = '2' 
                  AND HEXRT_COUNT_CURRENCY = '1'  
                group by TSE3_DATE,TSE3_PID,TSE3_IDSTK_INDEX,HEXRT_EXCHANGE_RATE) AVG6M 
        where m_date <= t_date
            and m_date >= start_date
        group by t_date,start_date,m_day_cnt,m_pid,m_index),                
        --順序及指數名稱
        (SELECT TRIM(RPT_VALUE_2) AS RPT_PID,SUBSTR(RPT_VALUE,1,4) AS RPT_INDEX,RPT_SEQ_NO ,COD_NAME 
            FROM CI.RPT ,
                (SELECT SUBSTR(COD_ID,1,1) AS COD_PID,TRIM(SUBSTR(COD_ID,3,4)) AS COD_INDEX,COD_DESC AS COD_NAME 
                    FROM ci.COD
                    WHERE COD_TXN_ID = '60410') C
            WHERE RPT_TXN_ID = '60410' and RPT_TXD_ID = '60410_1a'
            AND TRIM(RPT_VALUE_2) = COD_PID(+)
            AND TRIM(RPT_VALUE) = COD_INDEX(+)) R                       
    where TSE3_DATE = t_date  
      and TSE3_PID = m_pid
      and TSE3_IDSTK_INDEX = m_index 
      and TSE3_PID = RPT_PID(+)
      and TSE3_IDSTK_INDEX = RPT_INDEX(+)
    order by 1,2,3) a
order by rpt_seq_no, ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// d_60410_1b
        /// </summary>
        /// <param name="as_sdate"></param>
        /// <param name="as_edate"></param>
        /// <param name="ai_tot_cnt"></param>
        /// <param name="ad_max_weight"></param>
        /// <param name="ad_top5_weight"></param>
        /// <param name="ai_25_cnt"></param>
        /// <param name="ad_avg_amt1"></param>
        /// <param name="ad_avg_amt2"></param>
        /// <returns></returns>
        public DataTable d_60410_1b(DateTime as_sdate, 
                                    DateTime as_edate, 
                                    int ai_tot_cnt, 
                                    decimal ad_max_weight, 
                                    decimal ad_top5_weight, 
                                    int ai_25_cnt, 
                                    decimal ad_avg_amt1, 
                                    decimal ad_avg_amt2) {

            object[] parms = {
                ":as_sdate",as_sdate,
                ":as_edate",as_edate,
                ":ai_tot_cnt",ai_tot_cnt,
                ":ad_max_weight",ad_max_weight,
                ":ad_top5_weight",ad_top5_weight,
                ":ai_25_cnt",ai_25_cnt,
                ":ad_avg_amt1",ad_avg_amt1,
                ":ad_avg_amt2",ad_avg_amt2,
            };

            string sql =
                @"
select TSE5_IDSTK_INDEX as idstk_index,RPT_SEQ_NO,
        sum(case when TSE5_TOT_CNT < :ai_tot_cnt then 1 else 0 end) as tot_cnt,
        sum(case when TSE5_MAX_WEIGHT > :ad_max_weight then 1 else 0 end) as max_weight,
        sum(case when TSE5_TOP5_WEIGHT > :ad_top5_weight then 1 else 0 end) as top5_weight,
        sum(case when TSE5_25_CNT >= :ai_25_cnt and TSE5_25_AVG6M_MTH_USD <= :ad_avg_amt1 then 1 else 0 end) as day_avg_amt1,
        sum(case when TSE5_25_CNT <  :ai_25_cnt and TSE5_25_AVG6M_MTH_USD <= :ad_avg_amt2 then 1 else 0 end) as day_avg_amt2 
    from ci.TSE5,
        (SELECT TRIM(RPT_VALUE_2) AS RPT_PID,SUBSTR(RPT_VALUE,1,4) AS RPT_INDEX,RPT_SEQ_NO  FROM CI.RPT  WHERE RPT_TXN_ID = '60410' and RPT_TXD_ID = '60410_1b')
where TSE5_YMD >= to_char(:as_sdate,'YYYYMMDD')
  and TSE5_YMD <= to_char(:as_edate,'YYYYMMDD')
  and TSE5_PID = RPT_PID(+)
  and TSE5_IDSTK_INDEX = RPT_INDEX(+)
group by TSE5_IDSTK_INDEX,RPT_SEQ_NO
order by rpt_seq_no
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// d_60410_1bchk
        /// </summary>
        /// <param name="as_sdate"></param>
        /// <param name="as_edate"></param>
        /// <returns></returns>
        public DataTable d_60410_1bchk(DateTime as_sdate, DateTime as_edate) {

            object[] parms = {
                ":as_sdate",as_sdate,
                ":as_edate",as_edate
            };

            string sql =
                @"
select TSE3_IDSTK_INDEX,RPT_SEQ_NO,
    sum(case when tot_cnt < 10 then 1 else 0 end) as tot_cnt,
    sum(case when max_weight < 0.3 then 1 else 0 end) as max_weight,
    sum(case when top5 < 0.6 then 1 else 0 end) as max_weight,
    sum(case when cnt25 >= 15 and day_avg_amt < 3000 then 1 else 0 end) as day_avg_amt1,
    sum(case when cnt25 <  15 and day_avg_amt < 5000 then 1 else 0 end) as day_avg_amt2
from 
    (         
    select ymd,TSE3_IDSTK_INDEX,tot_cnt,max_weight,top5,cnt25,
            round(amt25_day_mth / HEXRT_EXCHANGE_RATE / 10000,0) as day_amt,
            round(amt25_6m_mth / amt25_6m_day_cnt / HEXRT_EXCHANGE_RATE / 10000,0) as day_avg_amt,
            RPT_SEQ_NO
    from
            (
            select to_char(TSE3_DATE,'YYYYMMDD') as ymd,
                TSE3_PID,TSE3_IDSTK_INDEX,max(TSE3_DESC_SEQ) as tot_cnt,max(TSE3_INDEX_WEIGHT) as max_weight,
                sum(case when TSE3_DESC_SEQ <= 5 then TSE3_INDEX_WEIGHT else 0 end) as top5,
                max(case when TSE3_ACCU_WEIGHT - TSE3_INDEX_WEIGHT < 25 then TSE3_ASC_SEQ else 0 end) as cnt25,
                sum(case when TSE3_ACCU_WEIGHT - TSE3_INDEX_WEIGHT < 25 then NVL(TSE4_CLS_AMT,0) else 0 end) as amt25_day_cls,
                sum(case when TSE3_ACCU_WEIGHT - TSE3_INDEX_WEIGHT < 25 then NVL(TSE4_MTH_AMT,0) else 0 end) as amt25_day_mth,
                sum(case when TSE3_ACCU_WEIGHT - TSE3_INDEX_WEIGHT < 25 then NVL(TSE4_6M_CLS_AMT,0) else 0 end) as amt25_6m_cls,
                sum(case when TSE3_ACCU_WEIGHT - TSE3_INDEX_WEIGHT < 25 then NVL(TSE4_6M_MTH_AMT,0) else 0 end) as amt25_6m_mth,
                max(TSE4_6M_CNT) as amt25_6m_day_cnt,
                min(TSE4_6M_YMD) as amt25_6m_date,
                HEXRT_EXCHANGE_RATE
            from ci.TSE3,ci.TSE4,CI.HEXRT
            where TSE3_DATE >= :as_sdate
              AND TSE3_DATE <=  :as_edate
              AND ((TSE3_PID = 1 AND TSE3_IDSTK_INDEX IN ('0000','13','17','A07'))  OR
                      (TSE3_PID = 2 AND TSE3_IDSTK_INDEX IN ('4000','22')))
              AND TSE3_DATE = TSE4_DATE 
              AND TSE3_PID = TSE4_PID(+)
              AND TSE3_SID = TSE4_SID(+)
              AND TSE4_DATE = HEXRT_DATE 
              AND HEXRT_CURRENCY_TYPE = '2' 
              AND HEXRT_COUNT_CURRENCY = '1'
            group by TSE3_DATE,TSE3_PID,TSE3_IDSTK_INDEX,HEXRT_EXCHANGE_RATE
            )
            ,(SELECT TRIM(RPT_VALUE_2) AS RPT_PID,SUBSTR(RPT_VALUE,1,4) AS RPT_INDEX,RPT_SEQ_NO  FROM CI.RPT  WHERE RPT_TXN_ID = '60410' and RPT_TXD_ID = '60410_1b')
    WHERE TSE3_PID = RPT_PID(+)
      AND TSE3_IDSTK_INDEX = RPT_INDEX(+)
                 
    )
group by TSE3_IDSTK_INDEX,RPT_SEQ_NO
order by rpt_seq_no
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// d_60410_2
        /// </summary>
        /// <param name="as_sdate"></param>
        /// <param name="as_edate"></param>
        /// <param name="ad_rate"></param>
        /// <returns></returns>
        public DataTable d_60410_2(DateTime as_sdate, DateTime as_edate, decimal ad_rate) {

            object[] parms = {
                ":as_sdate", as_sdate,
                ":as_edate", as_edate,
                ":ad_rate", ad_rate
            };

            string sql =
                @"
select COD_NAME,TO_CHAR(TSE3_DATE,'YYYY/MM/DD') AS TSE3_YMD,
    TSE3_SID,TFXMS_SNAME,
    ROUND(TSE3_INDEX_WEIGHT / 100,4) as INDEX_WEIGHT,
    RPT_SEQ_NO 
from ci.TSE3,ci.TFXMS,
    (SELECT TRIM(RPT_VALUE_2) AS RPT_PID,SUBSTR(RPT_VALUE,1,4) AS RPT_INDEX,RPT_SEQ_NO ,COD_NAME 
        FROM CI.RPT ,
            (SELECT SUBSTR(COD_ID,1,1) AS COD_PID,TRIM(SUBSTR(COD_ID,3,4)) AS COD_INDEX,COD_DESC AS COD_NAME 
                FROM ci.COD
                WHERE COD_TXN_ID = '60410') C
        WHERE RPT_TXN_ID = '60410' and RPT_TXD_ID = '60410_1a'
          AND TRIM(RPT_VALUE_2) = COD_PID(+)
          AND TRIM(RPT_VALUE) = COD_INDEX(+)) R                        
where TSE3_DATE >= :as_sdate
  AND TSE3_DATE <= :as_edate
  AND ((TSE3_PID = 1 AND TSE3_IDSTK_INDEX IN ('0000','13','17','A07'))  OR
          (TSE3_PID = 2 AND TSE3_IDSTK_INDEX IN ('4000','22')))
  AND TSE3_DESC_SEQ = 1
  AND TSE3_PID = RPT_PID(+)
  AND TSE3_IDSTK_INDEX = RPT_INDEX(+)
  AND TSE3_SID = TFXMS_SID
  AND ROUND(TSE3_INDEX_WEIGHT / 100,4) > :ad_rate
Order by rpt_seq_no, tse3_ymd, tse3_sid
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// d_60410_3
        /// </summary>
        /// <param name="as_sdate"></param>
        /// <param name="as_edate"></param>
        /// <param name="ad_rate"></param>
        /// <returns></returns>
        public DataTable d_60410_3(DateTime as_sdate, DateTime as_edate, decimal ad_rate) {

            object[] parms = {
                ":as_sdate", as_sdate,
                ":as_edate", as_edate,
                ":ad_rate", ad_rate
            };

            string sql =
                @"
select COD_NAME,TSE3_DATE,
    TSE3_DESC_SEQ,TSE3_SID,TFXMS_SNAME,
    ROUND(TSE3_INDEX_WEIGHT / 100,4) AS INDEX_WEIGHT ,
    RPT_SEQ_NO                       
from 
    --篩選條件
    (SELECT TSE3_DATE as cond_date,TSE3_PID as cond_pid,TSE3_IDSTK_INDEX as cond_index,SUM(TSE3_INDEX_WEIGHT) as sum_weight
        FROM ci.TSE3
        WHERE TSE3_DATE >= :as_sdate
          AND TSE3_DATE <= :as_edate
          AND ((TSE3_PID = 1 AND TSE3_IDSTK_INDEX IN ('0000','13','17','A07'))  OR
                (TSE3_PID = 2 AND TSE3_IDSTK_INDEX IN ('4000','22')))
          AND TSE3_DESC_SEQ <= 5
        GROUP BY TSE3_DATE,TSE3_PID,TSE3_IDSTK_INDEX
        HAVING round(SUM(TSE3_INDEX_WEIGHT)/ 100,4) > :ad_rate),                      
    --順序及指數名稱
    (SELECT TRIM(RPT_VALUE_2) AS RPT_PID,SUBSTR(RPT_VALUE,1,4) AS RPT_INDEX,RPT_SEQ_NO ,COD_NAME 
        FROM CI.RPT ,
            (SELECT SUBSTR(COD_ID,1,1) AS COD_PID,TRIM(SUBSTR(COD_ID,3,4)) AS COD_INDEX,COD_DESC AS COD_NAME 
                FROM ci.COD
                WHERE COD_TXN_ID = '60410') C
        WHERE RPT_TXN_ID = '60410' and RPT_TXD_ID = '60410_1a'
          AND TRIM(RPT_VALUE_2) = COD_PID(+)
          AND TRIM(RPT_VALUE) = COD_INDEX(+)) R,
        --主要資料來源
        ci.TSE3,
        --證券名稱
        ci.TFXMS
where cond_date = TSE3_DATE 
  AND cond_pid = TSE3_PID 
  AND cond_index = TSE3_IDSTK_INDEX 
  AND TSE3_DESC_SEQ <= 5
  AND TSE3_PID = RPT_PID(+)
  AND TSE3_IDSTK_INDEX = RPT_INDEX(+)
  AND TSE3_SID = TFXMS_SID
Order By rpt_seq_no, tse3_date, tse3_desc_seq
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// d_60412_3
        /// </summary>
        /// <param name="as_sdate"></param>
        /// <param name="as_edate"></param>
        /// <param name="ad_rate"></param>
        /// <returns></returns>
        public DataTable d_60412_3(DateTime as_sdate, DateTime as_edate, decimal ad_rate) {

            object[] parms = {
                ":as_sdate", as_sdate,
                ":as_edate", as_edate,
                ":ad_rate", ad_rate
            };

            string sql =
                @"
select COD_NAME,TSE3_DATE,TSE5_25_WEIGHT,
    TSE3_DESC_SEQ,TSE3_SID,TFXMS_SNAME,
    ROUND(TSE3_INDEX_WEIGHT / 100,4) AS INDEX_WEIGHT ,
    RPT_SEQ_NO                       
from ci.TSE5,
    --篩選條件
    (SELECT TSE3_DATE as cond_date,TSE3_PID as cond_pid,TSE3_IDSTK_INDEX as cond_index,SUM(TSE3_INDEX_WEIGHT) as sum_weight
        FROM ci.TSE3
        WHERE TSE3_DATE >= :as_sdate
          AND TSE3_DATE <= :as_edate
          AND ((TSE3_PID = 1 AND TSE3_IDSTK_INDEX IN ('0000','13','17','A07'))  OR
                  (TSE3_PID = 2 AND TSE3_IDSTK_INDEX IN ('4000','22')))
          AND TSE3_DESC_SEQ <= 5
        GROUP BY TSE3_DATE,TSE3_PID,TSE3_IDSTK_INDEX
        HAVING round(SUM(TSE3_INDEX_WEIGHT)/ 100,4) > :ad_rate),                      
    --順序及指數名稱
    (SELECT TRIM(RPT_VALUE_2) AS RPT_PID,SUBSTR(RPT_VALUE,1,4) AS RPT_INDEX,RPT_SEQ_NO ,COD_NAME 
        FROM CI.RPT ,
            (SELECT SUBSTR(COD_ID,1,1) AS COD_PID,TRIM(SUBSTR(COD_ID,3,4)) AS COD_INDEX,COD_DESC AS COD_NAME 
                FROM ci.COD
                WHERE COD_TXN_ID = '60410') C
        WHERE RPT_TXN_ID = '60410' and RPT_TXD_ID = '60410_1a'
          AND TRIM(RPT_VALUE_2) = COD_PID(+)
          AND TRIM(RPT_VALUE) = COD_INDEX(+)) R,
        --主要資料來源
        ci.TSE3,
        --證券名稱
        ci.TFXMS
where cond_date = TSE3_DATE 
  AND cond_pid = TSE3_PID 
  AND cond_index = TSE3_IDSTK_INDEX 
  AND TSE3_DESC_SEQ <= 5
  AND TSE3_PID = RPT_PID(+)
  AND TSE3_IDSTK_INDEX = RPT_INDEX(+)
  AND TSE3_SID = TFXMS_SID
  AND TO_CHAR(TSE3_DATE,'YYYYMMDD') = TSE5_YMD
  AND TSE3_IDSTK_INDEX = TSE5_IDSTK_INDEX
Order By rpt_seq_no, tse3_date, tse3_desc_seq
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// d_60420_Index 好像沒用到
        /// </summary>
        /// <returns></returns>
        public DataTable d_60420_Index() {

            string sql =
    @"
SELECT trim(COD_ID) as COD_ID,   
        COD_DESC  
FROM CI.COD  
WHERE COD_TXN_ID = '60420'    
  AND COD_COL_ID = 'PID-IDSTK'
                    ";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }
    }
}
