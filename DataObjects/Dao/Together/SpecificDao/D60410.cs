using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D60410
    {
        private Db db;

        public D60410()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable List60410b(DateTime symd, DateTime eymd, int ai_tot_cnt, Decimal ad_max_weight, Decimal ad_top5_weight, int ai_25_cnt, Decimal ad_avg_amt1, Decimal ad_avg_amt2)
        {
            object[] parms = {
                "@as_sdate",symd,
                "@as_edate",eymd,
                "@ai_tot_cnt",ai_tot_cnt,
                "@ad_max_weight",ad_max_weight,
                "@ad_top5_weight",ad_top5_weight,
                "@ai_25_cnt",ai_25_cnt,
                "@ad_avg_amt1",ad_avg_amt1,
                "@ad_avg_amt2",ad_avg_amt2
            };

            string sql =
                @"
                               select TSE5_IDSTK_INDEX as idstk_index,RPT_SEQ_NO,
                                          sum(case when TSE5_TOT_CNT < @ai_tot_cnt then 1 else 0 end) as tot_cnt,
                                          sum(case when TSE5_MAX_WEIGHT > @ad_max_weight then 1 else 0 end) as max_weight,
                                          sum(case when TSE5_TOP5_WEIGHT > @ad_top5_weight then 1 else 0 end) as top5_weight,
                                          sum(case when TSE5_25_CNT >= @ai_25_cnt and TSE5_25_AVG6M_MTH_USD <= @ad_avg_amt1 then 1 else 0 end) as day_avg_amt1,
                                          sum(case when TSE5_25_CNT <  @ai_25_cnt and TSE5_25_AVG6M_MTH_USD <= @ad_avg_amt2 then 1 else 0 end) as day_avg_amt2
                                 from ci.TSE5,
                                        (SELECT TRIM(RPT_VALUE_2) AS RPT_PID,SUBSTR(RPT_VALUE,1,4) AS RPT_INDEX,RPT_SEQ_NO
                                         FROM CI.RPT  WHERE RPT_TXN_ID = '60410' and RPT_TXD_ID = '60410_1b')
                                where TSE5_YMD >= to_char(@as_sdate,'YYYYMMDD')
                                  and TSE5_YMD <= to_char(@as_edate,'YYYYMMDD')
                                  and TSE5_PID = RPT_PID(+)
                                  and TSE5_IDSTK_INDEX = RPT_INDEX(+)
                                group by TSE5_IDSTK_INDEX,RPT_SEQ_NO

                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable List60410a(DateTime symd, DateTime eymd)
        {
            object[] parms = {
                "@as_sdate",symd,
                "@as_edate",eymd
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
                                        RPT_SEQ_NO,COD_NAME,
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
                              where TSE5_YMD >= to_char(@as_sdate,'YYYYMMDD')
                                and TSE5_YMD <=  to_char(@as_edate,'YYYYMMDD')
                                and TSE5_PID = RPT_PID(+)
                                and TSE5_IDSTK_INDEX = RPT_INDEX(+)
                              order by rpt_seq_no,ymd
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable List60410_2(DateTime symd, DateTime eymd, decimal rate)
        {
            object[] parms = {
                "@as_sdate",symd,
                "@as_edate",eymd,
                "@ad_rate",rate
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
                          where TSE3_DATE >= @as_sdate
                            AND TSE3_DATE <= @as_edate
                            AND ((TSE3_PID = 1 AND TSE3_IDSTK_INDEX IN ('0000','13','17','A07'))  OR
                                 (TSE3_PID = 2 AND TSE3_IDSTK_INDEX IN ('4000','22')))
                            AND TSE3_DESC_SEQ = 1
                            AND TSE3_PID = RPT_PID(+)
                            AND TSE3_IDSTK_INDEX = RPT_INDEX(+)
                            AND TSE3_SID = TFXMS_SID
                            AND ROUND(TSE3_INDEX_WEIGHT / 100,4) > @ad_rate
                          order by rpt_seq_no,TSE3_YMD,TSE3_SID
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable List60410_3(DateTime symd, DateTime eymd, decimal rate)
        {
            object[] parms = {
                "@as_sdate",symd,
                "@as_edate",eymd,
                "@ad_rate",rate
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
                                  order by rpt_seq_no,TSE3_DATE,TSE3_DESC_SEQ
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}