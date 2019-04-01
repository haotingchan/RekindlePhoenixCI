using Common;
using System;
using System.Data;
/// <summary>
/// john,20190329,D30780
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30780 : DataGate
   {
      public DateTime MaxDate(DateTime ld_end_date)
      {
         object[] parms = {
            ":ld_end_date",ld_end_date
            };
         string sql = @"select MAX(AI1_DATE) 
                        from ci.AI1 where AI1_DATE <= :ld_end_date
                                  and AI1_PARAM_KEY <> ' ' and AI1_KIND_ID2 = ' ' 			 
                             AND AI1_MARKET_CODE IN (' ','%')";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult.Rows[0][0].AsDateTime();
      }

      public DataTable List30780_1(string as_ym, string as_last_ym, DateTime ad_end_date, string as_market_code)
      {
         object[] parms = {
            ":as_ym",as_ym,
            ":as_last_ym",as_last_ym,
            ":ad_end_date",ad_end_date,
            ":as_market_code",as_market_code
            };

         string sql =
             @"select RPT_SEQ_NO,
                         AI2_PARAM_KEY,Y_QNTY,M_QNTY,
                         case when Y_DAY_CNT = 0  then 0 else ROUND(Y_QNTY/Y_DAY_CNT,0) end as Y_AVG_QNTY,
                         case when M_DAY_CNT = 0  then 0 else ROUND(M_QNTY/M_DAY_CNT,0) end as M_AVG_QNTY, 
                         MAX_YMD,MAX_M_QNTY,
                         AI1_DATE,AI1_HIGH_DATE,AI1_HIGH_QNTY,AI2_PROD_TYPE,MARKET_CODE
                    from 
                        (select AI2_PROD_TYPE,AI2_PARAM_KEY as AI2_PARAM_KEY,
                                sum(case when AI2_YMD = :as_last_ym then case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                                                              when '1' then NVL(AI2_AH_M_QNTY,0)
                                                                                              else AI2_M_QNTY end else 0 end) as Y_QNTY,
                                sum(case when AI2_YMD = :as_last_ym then case :as_market_code when '0' then AI2_DAY_COUNT
                                                                                              when '1' then AI2_AH_DAY_COUNT
                                                                                              else AI2_DAY_COUNT end else 0 end) as Y_DAY_CNT,
                                sum(case when AI2_YMD = :as_ym then case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                                                              when '1' then NVL(AI2_AH_M_QNTY,0)
                                                                                              else AI2_M_QNTY end else 0 end) as M_QNTY,
                                sum(case when AI2_YMD = :as_ym then case :as_market_code when '0' then AI2_DAY_COUNT
                                                                                              when '1' then AI2_AH_DAY_COUNT
                                                                                              else AI2_DAY_COUNT end else 0 end) as M_DAY_CNT
                           from ci.AI2
                          where AI2_YMD >= :as_last_ym
                            and AI2_YMD <= :as_ym
                            and AI2_SUM_TYPE = 'M' and AI2_SUM_SUBTYPE = '3' and AI2_PROD_TYPE in ('F','O')
                          group by AI2_PROD_TYPE,AI2_PARAM_KEY
                          union all
                         select AI2_PROD_TYPE,'       ' as AI2_PARAM_KEY,
                                sum(case when AI2_YMD = :as_last_ym then case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                                                              when '1' then NVL(AI2_AH_M_QNTY,0)
                                                                                              else AI2_M_QNTY end else 0 end) as Y_QNTY,
                                sum(case when AI2_YMD = :as_last_ym then case :as_market_code when '0' then AI2_DAY_COUNT
                                                                                              when '1' then AI2_AH_DAY_COUNT
                                                                                              else AI2_DAY_COUNT end else 0 end) as Y_DAY_CNT,
                                sum(case when AI2_YMD = :as_ym then case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                                                              when '1' then NVL(AI2_AH_M_QNTY,0)
                                                                                              else AI2_M_QNTY end else 0 end) as M_QNTY,
                                sum(case when AI2_YMD = :as_ym then case :as_market_code when '0' then AI2_DAY_COUNT
                                                                                              when '1' then AI2_AH_DAY_COUNT
                                                                                              else AI2_DAY_COUNT end else 0 end) as M_DAY_CNT
                           from ci.AI2
                          where AI2_YMD >= :as_last_ym
                            and AI2_YMD <= :as_ym
                            and AI2_SUM_TYPE = 'M' and AI2_SUM_SUBTYPE = '1' and AI2_PROD_TYPE in ('F','O')
                          group by AI2_PROD_TYPE
                          union all
                         select ' ','       ' ,
                                sum(case when AI2_YMD = :as_last_ym then case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                                                              when '1' then NVL(AI2_AH_M_QNTY,0)
                                                                                              else AI2_M_QNTY end else 0 end) as Y_QNTY,
                                max(case when AI2_YMD = :as_last_ym then case :as_market_code when '0' then AI2_DAY_COUNT
                                                                                              when '1' then AI2_AH_DAY_COUNT
                                                                                              else AI2_DAY_COUNT end else 0 end) as Y_DAY_CNT,
                                sum(case when AI2_YMD = :as_ym then case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                                                              when '1' then NVL(AI2_AH_M_QNTY,0)
                                                                                              else AI2_M_QNTY end else 0 end) as M_QNTY,
                                max(case when AI2_YMD = :as_ym then case :as_market_code when '0' then AI2_DAY_COUNT
                                                                                              when '1' then AI2_AH_DAY_COUNT
                                                                                              else AI2_DAY_COUNT end else 0 end) as M_DAY_CNT
                           from ci.AI2
                          where AI2_YMD >= :as_last_ym
                            and AI2_YMD <= :as_ym
                            and AI2_SUM_TYPE = 'M' and AI2_SUM_SUBTYPE = '1' and AI2_PROD_TYPE in ('F','O')),
                        --當月最大量  
                        (select AI2_PARAM_KEY as MAX_PARAM_KEY,MIN(AI2_YMD) AS MAX_YMD,MAX_QNTY as MAX_M_QNTY
                           from ci.AI2,
                               (select AI2_PARAM_KEY as MAX_PARAM_KEY,MAX(case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                                                              when '1' then NVL(AI2_AH_M_QNTY,0)
                                                                                              else AI2_M_QNTY end) as MAX_QNTY
                                 from ci.AI2
                                where AI2_YMD >= :as_ym||'01' and AI2_YMD <= :as_ym||'31'
                                  and AI2_SUM_TYPE = 'D' and AI2_SUM_SUBTYPE = '3'
                                group by AI2_PARAM_KEY)
                          where AI2_YMD >= :as_ym||'01' and AI2_YMD <= :as_ym||'31'
                            and AI2_SUM_TYPE = 'D' and AI2_SUM_SUBTYPE = '3'
                            and AI2_PARAM_KEY = MAX_PARAM_KEY
                            and ((:as_market_code = '0' and AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0) = MAX_QNTY) or 
                                 (:as_market_code = '1' and NVL(AI2_AH_M_QNTY,0) = MAX_QNTY) or 
                                 (:as_market_code not in ('0','1') and AI2_M_QNTY = MAX_QNTY))
                          group by AI2_PARAM_KEY,MAX_QNTY),  
                        --下月10日最大量  
                        (select AI1_DATE,AI1_PROD_TYPE,AI1_PARAM_KEY,AI1_HIGH_DATE,AI1_HIGH_QNTY from CI.AI1
                          where AI1_DATE = :ad_end_date     
                            and AI1_MARKET_CODE in (:as_market_code,' ')
                            and AI1_KIND_ID2 = ' '
                            and AI1_PARAM_KEY <> ' ' 
                            --and (AI1_PARAM_KEY <> ' ' or AI1_PROD_SUBTYPE = ' ')
                         ) ,
                        --報表檔     
                        (select RPT_SEQ_NO,RPT_VALUE as RPT_PARAM_KEY,RPT_VALUE_2 as RPT_PROD_TYPE , case when MARKET_CODE is null then '%' else MARKET_CODE end as MARKET_CODE
                           from ci.RPT,
                               (select APDK_PARAM_KEY,MAX(APDK_MARKET_CODE) AS MARKET_CODE from ci.APDK group by APDK_PARAM_KEY) 
                          where RPT_TXN_ID = '30780' and RPT_TXD_ID = '30781'
                            and RPT_VALUE = APDK_PARAM_KEY(+))
                   where AI2_PARAM_KEY = MAX_PARAM_KEY(+)
                     and AI2_PROD_TYPE = AI1_PROD_TYPE(+)
                     and AI2_PARAM_KEY = AI1_PARAM_KEY(+)
                     and AI2_PROD_TYPE = RPT_PROD_TYPE(+)
                     and AI2_PARAM_KEY = RPT_PARAM_KEY(+)
                  ORDER BY rpt_seq_no
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      public DataTable List30780_2(string as_fm_ym, string as_to_ym, string as_market_code)
      {
         object[] parms = {
            ":as_fm_ym",as_fm_ym,
            ":as_to_ym",as_to_ym,
            ":as_market_code",as_market_code
            };

         string sql =
             @"select DATA_YMD,DATA_SEQ_NO as col_num,RPT_SEQ_NO AS row_num,
                      DATA_PARAM_KEY,M_QNTY,
                      round(M_QNTY / DAY_COUNT,0) as AVG_QNTY,DATA_PROD_TYPE,MARKET_CODE
                 from
                     (select AI2_YMD as DATA_YMD,
                             --ROW_NUMBER( ) OVER (PARTITION BY  DATA_PROD_TYPE,DATA_PARAM_KEY ORDER BY AI2_YMD NULLS LAST) as DATA_SEQ_NO,
                             mth_seq_no as DATA_SEQ_NO, 
                             DATA_PROD_TYPE,DATA_PARAM_KEY
                        from ci.AI2,
                             --商品  
                            (select AI2_PROD_TYPE as DATA_PROD_TYPE,
                                    AI2_PARAM_KEY as DATA_PARAM_KEY
                               from ci.AI2
                              where AI2_YMD >= :as_fm_ym
                                and AI2_YMD <= :as_to_ym
                                and AI2_SUM_TYPE = 'M' and AI2_SUM_SUBTYPE = '3' and AI2_PROD_TYPE in ('F','O')
                              group by AI2_PROD_TYPE,AI2_PARAM_KEY
                              union all
                             select 'F','       ' from dual
                              union all
                             select 'O','       ' from dual
                              union all
                             select ' ','       ' from dual),
                            --年月順序
                            (SELECT OCF_YM,7 - rownum as mth_seq_no
                               FROM
                                   (select CAST(SUBSTR(OCF_YMD,1,6) as CHAR(6)) as OCF_YM from ci.AOCF 
                                     WHERE OCF_YMD >= :as_fm_ym || '01'
                                       AND OCF_YMD <= :as_to_ym || '31'
                                     GROUP BY SUBSTR(OCF_YMD,1,6)
                                     ORDER BY SUBSTR(OCF_YMD,1,6) DESC)
                              WHERE rownum <= 6)
                      where AI2_YMD >= :as_fm_ym
                        and AI2_YMD <= :as_to_ym
                        and AI2_SUM_TYPE = 'M' and AI2_SUM_SUBTYPE = '1' and AI2_PROD_TYPE = 'F'
                        and AI2_YMD = OCF_YM(+)),
                     --明細資料   
                    (select AI2_YMD,
                            AI2_PROD_TYPE,AI2_PARAM_KEY,
                            case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                 when '1' then NVL(AI2_AH_M_QNTY,0)
                                                 else AI2_M_QNTY end as M_QNTY,
                            case :as_market_code when '0' then AI2_DAY_COUNT
                                                 when '1' then AI2_AH_DAY_COUNT
                                                 else AI2_DAY_COUNT end as DAY_COUNT            
                       from ci.AI2
                      where AI2_YMD >= :as_fm_ym
                        and AI2_YMD <= :as_to_ym
                        and AI2_SUM_TYPE = 'M' and AI2_SUM_SUBTYPE = '3' and AI2_PROD_TYPE in ('F','O')
                      union all
                     select AI2_YMD,
                            AI2_PROD_TYPE,AI2_PARAM_KEY,
                            case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                 when '1' then NVL(AI2_AH_M_QNTY,0)
                                                 else AI2_M_QNTY end as M_QNTY,
                            case :as_market_code when '0' then AI2_DAY_COUNT
                                                 when '1' then AI2_AH_DAY_COUNT
                                                 else AI2_DAY_COUNT end as DAY_COUNT         
                       from ci.AI2
                      where AI2_YMD >= :as_fm_ym
                        and AI2_YMD <= :as_to_ym
                        and AI2_SUM_TYPE = 'M' and AI2_SUM_SUBTYPE = '1' and AI2_PROD_TYPE in ('F','O')
                      union all
                     select AI2_YMD,
                            ' ','       ',
                            sum(case :as_market_code when '0' then AI2_M_QNTY - NVL(AI2_AH_M_QNTY,0)
                                                 when '1' then NVL(AI2_AH_M_QNTY,0)
                                                 else AI2_M_QNTY end) as M_QNTY,
                            max(case :as_market_code when '0' then AI2_DAY_COUNT
                                                 when '1' then AI2_AH_DAY_COUNT
                                                 else AI2_DAY_COUNT end) as DAY_COUNT            
                       from ci.AI2
                      where AI2_YMD >= :as_fm_ym
                        and AI2_YMD <= :as_to_ym
                        and AI2_SUM_TYPE = 'M' and AI2_SUM_SUBTYPE = '1' and AI2_PROD_TYPE in ('F','O')
                      group by AI2_YMD,'       '
                     ) ,
                     --報表檔     
                     (select RPT_SEQ_NO,RPT_VALUE as RPT_PARAM_KEY,RPT_VALUE_2 as RPT_PROD_TYPE , case when MARKET_CODE is null then '%' else MARKET_CODE end as MARKET_CODE
                        from ci.RPT,
                            (select APDK_PARAM_KEY,MAX(APDK_MARKET_CODE) AS MARKET_CODE from ci.APDK group by APDK_PARAM_KEY) 
                       where RPT_TXN_ID = '30780' and RPT_TXD_ID = '30782'
                         and RPT_VALUE = APDK_PARAM_KEY(+))
                 where DATA_YMD = AI2_YMD(+)
                   and DATA_PROD_TYPE = AI2_PROD_TYPE(+) 
                   and DATA_PARAM_KEY = AI2_PARAM_KEY(+) 
                   and DATA_PROD_TYPE = RPT_PROD_TYPE
                   and DATA_PARAM_KEY = RPT_PARAM_KEY
                  ORDER BY row_num,col_num
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return AM0_BRK_NAME/M_QNTY/M_RATE
      /// </summary>
      /// <param name="as_ym"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable List30780_4(string as_ym,string as_market_code)
      {
         object[] parms = {
            ":as_ym",as_ym,
            ":as_market_code",as_market_code
            };

         string sql =
             @"select AM0_BRK_NO|| '　'||trim(ABRK_NAME) as AM0_BRK_NAME,
                      M_QNTY,ROUND(M_QNTY / TOT_QNTY,4) * 100 as M_RATE
                 from ci.ABRK,
                     (select SUM(case :as_market_code when '0' then AM0_M_QNTY - NVL(AM0_AH_M_QNTY,0)
                                                      when '1' then NVL(AM0_AH_M_QNTY,0)
                                                      else AM0_M_QNTY end
                                 ) AS TOT_QNTY 
                        from CI.AM0
                       where AM0_YMD =:as_ym
                         and AM0_SUM_TYPE = 'M') T,      
                     (select trim(AM0_BRK_NO4) || CASE WHEN AM0_BRK_TYPE = '9' then '999' else '000' end as AM0_BRK_NO,
                             SUM(case :as_market_code when '0' then AM0_M_QNTY - NVL(AM0_AH_M_QNTY,0)
                                                      when '1' then NVL(AM0_AH_M_QNTY,0)
                                                      else AM0_M_QNTY end
                                 ) as M_QNTY
                        from ci.AM0
                       where AM0_YMD = :as_ym
                         and AM0_SUM_TYPE = 'M'
                       group by AM0_BRK_NO4,AM0_BRK_TYPE
                       order by 2 DESC) M
                where AM0_BRK_NO = ABRK_NO  
                order by M_QNTY desc
 ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return AM0_BRK_NAME/Y_QNTY/M_QNTY/DIFF_QNTY/DIFF_RATE
      /// </summary>
      /// <param name="as_ym"></param>
      /// <param name="as_last_ym"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable List30780_5(string as_ym, string as_last_ym, string as_market_code)
      {
         object[] parms = {
            ":as_ym",as_ym,
            ":as_last_ym",as_last_ym,
            ":as_market_code",as_market_code
            };

         string sql =
             @"select ABRK_NO|| '　'||trim(ABRK_NAME) as AM0_BRK_NAME,
                      Y.M_QNTY as Y_QNTY,T.M_QNTY as M_QNTY,  
                      (nvl(T.M_QNTY,0) - nvl(Y.M_QNTY,0)) as DIFF_QNTY,
                      case when (Y.M_QNTY = 0 or Y.M_QNTY is null) and (T.M_QNTY = 0 or T.M_QNTY is null) then 0
                               when  nvl(Y.M_QNTY,0) = 0 then 99999999 
                               when  nvl(T.M_QNTY,0) = 0 then -99999999 
                               else round((T.M_QNTY - Y.M_QNTY)/Y.M_QNTY,4) end as DIFF_RATE
                 from ci.ABRK,
                     (select trim(AM0_BRK_NO4) || CASE WHEN AM0_BRK_TYPE = '9' then '999' else '000' end AS AM0_BRK_NO,
                             SUM(case :as_market_code when '0' then AM0_M_QNTY - NVL(AM0_AH_M_QNTY,0)
                                                      when '1' then NVL(AM0_AH_M_QNTY,0)
                                                      else AM0_M_QNTY end) AS M_QNTY 
                        from CI.AM0
                       where AM0_YMD = :as_ym
                         and AM0_SUM_TYPE = 'M'
                       group by AM0_BRK_NO4,AM0_BRK_TYPE) T, 
                     (select trim(AM0_BRK_NO4) || CASE WHEN AM0_BRK_TYPE = '9' then '999' else '000' end AS AM0_BRK_NO,
                             SUM(case :as_market_code when '0' then AM0_M_QNTY - NVL(AM0_AH_M_QNTY,0)
                                                      when '1' then NVL(AM0_AH_M_QNTY,0)
                                                      else AM0_M_QNTY end) AS M_QNTY 
                        from CI.AM0
                       where AM0_YMD = :as_last_ym
                         and AM0_SUM_TYPE = 'M'
                       group by AM0_BRK_NO4,AM0_BRK_TYPE) Y
                where substr(ABRK_NO,5,3) in ('000','999')
                  and ABRK_NO = T.AM0_BRK_NO(+)  
                  and ABRK_NO = Y.AM0_BRK_NO(+)
                order by diff_rate";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


   }
}
