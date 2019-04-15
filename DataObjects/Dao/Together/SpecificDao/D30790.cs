using System;
using System.Data;
using Common;
/// <summary>
/// john,20190305,D30370
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30790 : DataGate
   {
      /// <summary>
      /// return BEGIN_TIME/BEGIN_TYPE/END_TIME/END_TYPE/KIND_ID/SEQ_NO/SEQ_T_DAY
      /// </summary>
      /// <returns></returns>
      public DataTable Get30790Data(DateTime as_fm_ymd, DateTime as_to_ymd)
      {
         object[] parms = {
            ":as_fm_ymd",as_fm_ymd.ToString("yyyyMMdd"),
            ":as_to_ymd",as_to_ymd.ToString("yyyyMMdd")
            };
         string sql = @"SELECT AMT13_BEGIN_TIME AS BEGIN_TIME,
                         CASE WHEN AMT13_BEGIN_TYPE = 'Y' THEN '含' ELSE ' ' END AS BEGIN_TYPE,
                         AMT13_END_TIME AS END_TIME,
                         CASE WHEN AMT13_END_TYPE = 'Y' THEN '含' ELSE ' ' END AS END_TYPE,
                         RPT_KIND_ID AS KIND_ID,
                         RPT_SEQ_NO AS SEQ_NO,
                         AMT13_T_DAY AS SEQ_T_DAY,
                         --交易總量
                         M_QNTY,
                         --日均總量
                         case when nvl(DAY_CNT,0) = 0 then 0 else round(M_QNTY / DAY_CNT,0) end as AVG_QNTY,
                         --交易量比重
                         case when nvl(TOT_QNTY,0) = 0 then 0 else round(M_QNTY / TOT_QNTY * 100,16) end as M_RATE,
                         --
                         case when RPT_KIND_ID <> 'TXF' or nvl(DAY_CNT,0) = 0 then 0 else round(M_HIGH_LOW / DAY_CNT,0) end as AVG_TX_HIGH_LOW
                    FROM 
                         --各交易時段成交量
                         (SELECT APDK_PARAM_KEY AS AM13_KIND_ID,
                                 AM13_BEGIN_TIME,
                                 AM13_BEGIN_TYPE,
                                 AM13_END_TIME,
                                 AM13_END_TYPE,
                                 SUM(AM13_M_QNTY)/2 AS M_QNTY,
                                 SUM(NVL(AM13_M_HIGH_PRICE,0) - NVL(AM13_M_LOW_PRICE,0)) AS M_HIGH_LOW
                            FROM ci.AM13,ci.APDK
                           WHERE AM13_YMD >= :as_fm_ymd
                             AND AM13_YMD <= :as_to_ymd
                             AND AM13_KIND_ID = APDK_KIND_ID
                           GROUP BY APDK_PARAM_KEY,AM13_BEGIN_TIME,AM13_BEGIN_TYPE,AM13_END_TIME,AM13_END_TYPE) A,
                         --各商品總成交量
                         (SELECT APDK_PARAM_KEY as TOT_KIND_ID,
                                 SUM(AM13_M_QNTY)/2 AS TOT_QNTY
                            FROM ci.AM13,ci.APDK
                           WHERE AM13_YMD >= :as_fm_ymd
                             AND AM13_YMD <= :as_to_ymd
                             AND AM13_KIND_ID = APDK_KIND_ID
                           GROUP BY APDK_PARAM_KEY) T,
                         --各商品盤後交易天數
                        (SELECT PDK_PARAM_KEY as PDK_KIND_ID,count(distinct PDK_DATE) as DAY_CNT
                           FROM CI.HPDK
                          WHERE PDK_MARKET_CODE = '1' 
                            AND PDK_DATE >= TO_DATE(:as_fm_ymd,'YYYYMMDD')
                            AND PDK_DATE <= TO_DATE(:as_to_ymd,'YYYYMMDD')
                          GROUP BY PDK_PARAM_KEY) D,
                         --報表欄位位置 
                        (SELECT RPT_SEQ_NO,RPT_VALUE as RPT_KIND_ID,AMT13_BEGIN_TIME,AMT13_BEGIN_TYPE,AMT13_END_TIME,AMT13_END_TYPE,AMT13_T_DAY
                           FROM CI.RPT,ci.AMT13 
                          WHERE RPT_TXN_ID = '30790' AND RPT_TXD_ID = '30790') R
                   WHERE RPT_KIND_ID = PDK_KIND_ID(+)
                     AND RPT_KIND_ID = TOT_KIND_ID(+)
                     AND RPT_KIND_ID = AM13_KIND_ID(+)
                     AND AMT13_BEGIN_TIME = AM13_BEGIN_TIME(+)
                     AND AMT13_BEGIN_TYPE = AM13_BEGIN_TYPE(+)
                     AND AMT13_END_TIME = AM13_END_TIME(+)
                     AND AMT13_END_TYPE = AM13_END_TYPE(+)
                  UNION ALL
                  SELECT  '9999','','','',RPT_KIND_ID,RPT_SEQ_NO,0,M_QNTY,
                          CASE WHEN DAY_CNT = 0 THEN 0 ELSE round(M_QNTY/DAY_CNT,16) END AS AVG_QNTY ,0,null
                    FROM     
                        (SELECT AI2_PROD_TYPE,AI2_SUM_SUBTYPE,AI2_PARAM_KEY,AI2_KIND_ID2,
                                sum(CASE WHEN AI2_YMD >=  :as_fm_ymd AND AI2_ymd <=  :as_to_ymd THEN AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0) ELSE 0 END) AS M_QNTY
                           FROM ci.AI2
                             WHERE AI2_SUM_TYPE = 'D'
                              and AI2_SUM_SUBTYPE ='3'
                              and AI2_PROD_TYPE in ('F','O')
                              and AI2_YMD >=  :as_fm_ymd
                              and AI2_YMD <= :as_to_ymd
                              --AND AI2_PARAM_KEY IN (select APDK_KIND_ID from ci.APDK where APDK_MARKET_CODE = '1')
                              AND NOT AI2_AH_M_QNTY IS NULL
                          group by AI2_PROD_TYPE,AI2_SUM_SUBTYPE,AI2_PARAM_KEY,AI2_KIND_ID2 )T ,
                        (SELECT SUM(CASE WHEN  OCF_YMD >=  :as_fm_ymd AND OCF_YMD <= :as_to_ymd THEN 1 ELSE 0 END)AS DAY_CNT 
                           FROM CI.AOCF
                          WHERE OCF_YMD >=  :as_fm_ymd
                            AND OCF_YMD <= :as_to_ymd)D,
                        (SELECT DISTINCT RPT_SEQ_NO,RPT_VALUE as RPT_KIND_ID
                           FROM CI.RPT,ci.AMT13 
                          WHERE RPT_TXN_ID = '30790' AND RPT_TXD_ID = '30790')R
                   WHERE R.RPT_KIND_ID = T.AI2_PARAM_KEY(+)
                   ORDER BY seq_t_day,begin_time,begin_type,end_time,end_type,kind_id";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return DATA_DATE/NIGHT_HIGH_LOW/DAY_HIGH_LOW/AMIF_OPEN_PRICE/AMIF_HIGH_PRICE/AMIF_LOW_PRICE/NIGHT_CLOSE_PRICE/DAY_CLOSE_PRICE
      /// </summary>
      /// <param name="as_date_fm"></param>
      /// <param name="as_date_to"></param>
      /// <returns></returns>
      public DataTable Get30790_4Data(DateTime as_date_fm, DateTime as_date_to)
      {
         object[] parms = {
            ":as_date_fm",as_date_fm,
            ":as_date_to",as_date_to
            };

         string sql =
             @"SELECT AI6_DATE as DATA_DATE,
                      AMIF_HIGH_LOW as NIGHT_HIGH_LOW,
                      AI6_HIGH_LOW as DAY_HIGH_LOW,
                      AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,
                      AMIF_CLOSE_PRICE as NIGHT_CLOSE_PRICE,
                      AI6_CLOSE_PRICE as DAY_CLOSE_PRICE
                 FROM
                    (SELECT AI6_DATE,AI6_HIGH_LOW,AI6_CLOSE_PRICE
                       FROM CI.AI6 
                      WHERE AI6_DATE >= :as_date_fm
                        AND AI6_DATE <= :as_date_to
                        AND AI6_PARAM_KEY = 'TXF') D,
                    (SELECT AMIF_DATE,AMIF_HIGH_PRICE - AMIF_LOW_PRICE AS AMIF_HIGH_LOW,AMIF_CLOSE_PRICE,
                            AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE        
                       FROM CI.AMIFAH
                      WHERE AMIF_DATE >= :as_date_fm
                        AND AMIF_DATE <= :as_date_to
                        AND AMIF_PARAM_KEY = 'TXF'
                        AND AMIF_MTH_SEQ_NO = 1) N
                WHERE AI6_DATE = AMIF_DATE(+)
                ORDER BY DATA_DATE";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// 取得資料庫內最大日期
      /// </summary>
      /// <returns></returns>
      public string MaxDate()
      {
         string sql =@"SELECT NVL(MAX(AI2_YMD),'') FROM CI.AI2";
         DataTable dtResult = db.GetDataTable(sql, null);
         return dtResult.Rows[0][0].AsInt().ToString("0000/00/00");
      }
   }
}
