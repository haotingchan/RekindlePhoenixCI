using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190417,D40040
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40040 : DataGate
   {
      /// <summary>
      /// 前一交易日
      /// </summary>
      /// <param name="ld_date_last">當前交易日期</param>
      /// <param name="sheet">第N個工作表</param>
      /// <returns></returns>
      public DateTime GetDateLast(DateTime ld_date_last, int sheet)
      {
         object[] parms = {
            ":ld_date_last",ld_date_last
            };

         DateTime dateTime=DateTime.MinValue;

         switch (sheet)
         {
            case 1:
               string sql = @"select nvl(max(mg1_date),to_date('1901/1/1','yyyy/mm/dd'))
                          from ci.mg1
                         where mg1_date < :ld_date_last";
               DataTable dtResult = db.GetDataTable(sql, parms);
               dateTime= dtResult.Rows[0][0].AsDateTime();
               break;
            case 2:
               string dateLast = new AI2().GetLastSumTypeDate("D","2","S", ld_date_last);
               dateTime = dateLast.AsDateTime();
               break;
            default:
               break;
         }

         return dateTime;
      }
      
      /// <summary>
      /// Get d_40040 17 欄位
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="as_osw_grp">T.MG1_OSW_GRP</param>
      /// <returns></returns>
      public DataTable ListData(DateTime as_date, string as_osw_grp)
      {
         object[] parms = {
            ":as_date",as_date,
            ":as_osw_grp",as_osw_grp
            };

         string sql =
             @"
               SELECT T.MG1_DATE AS MG1_DATE,   
                        T.MG1_KIND_ID AS MG1_KIND_ID,  
                        T.MG1_IM AS MG1_IM,
                        T.MG1_CHANGE_RANGE as MG1_CHANGE_RANGE,
                        T.MG1_PRICE AS MG1_PRICE,
                        T.MG1_CM AS MG1_CM,
                        L.MG1_CHANGE_RANGE AS MG1_CHANGE_RANGE_LAST,
                        L.MG1_PRICE AS MG1_PRICE_LAST,
                        T.MG1_CUR_CM AS MG1_CM_LAST,
                        T.MG1_SEQ_NO AS MG1_SEQ_NO,
                        MGT2_KIND_ID_OUT ,
                        RPT_SEQ_NO,
                        T.MG1_PROD_TYPE,
                        T.MG1_PROD_SUBTYPE,
                        T.MG1_CHANGE_FLAG,
                        oi_rate,ai2_oi,aprod_7date,aprod_delivery_date,tot_oi
                   FROM ci.MGT2,
                        ci.MG1 T,
                       (select MG1_KIND_ID,MG1_PRICE,MG1_CHANGE_RANGE,MG1_DATE
                          from ci.MG1,
                              (select MG1_KIND_ID as Y_KIND_ID,max(MG1_DATE) as Y_DATE
                                 from ci.MG1
                                where MG1_DATE < :as_date
                                group by MG1_KIND_ID)
                         where MG1_DATE = Y_DATE
                           and MG1_KIND_ID = Y_KIND_ID
                           and MG1_TYPE(+) in ('-','A')
                           and MG1_PROD_SUBTYPE(+) <> 'S') L,   
                       (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '40040' and RPT_TXD_ID = '40040') R,
                       --OI
                       (select ai2_param_key as ai2_kind_id,ai2_oi,tot_oi,
                               case when tot_oi = 0 then 0 else round(ai2_oi/tot_oi,4) end as oi_rate,
                               aprod_7date,aprod_delivery_date,aprod_settle_date,aprod_end_date
                          from ci.ai2,
                              (select aprod_param_key 
                                     ,fut.DATE_DIFF_OCF_DAYS(max(aprod_delivery_date),-7) as aprod_7date,aprod_settle_date,max(aprod_end_date) as aprod_end_date,max(aprod_delivery_date) as aprod_delivery_date
                                from ci.aprod,
                                    (select amif_param_key,min(amif_settle_date) as amif_settle_date from ci.amif
                                     where amif_date = :as_date
                                       and amif_data_source = 'T'
                                       and amif_prod_subtype <> 'S'
                                       and amif_expiry_type = 'S'
                                     group by amif_param_key)
                               where aprod_param_key = amif_param_key
                                 and aprod_settle_date = amif_settle_date
                                 and aprod_expiry_type = 'S'
                               group by  aprod_param_key ,aprod_settle_date),
                             (select sum(ai2_oi) as tot_oi 
                                from ci.ai2
                               where ai2_ymd = to_char(:as_date,'YYYYMMDD')
                                 and ai2_sum_type = 'D'
                                 and ai2_sum_subtype = '1'
                                 and ai2_prod_type in ('F','O')) T
                         where ai2_ymd = to_char(:as_date,'YYYYMMDD')
                           and ai2_sum_type = 'D'
                           and ai2_sum_subtype = '3'
                           and ai2_prod_type in ('F','O')
                           and ai2_prod_subtype <> 'S'
                           and ai2_param_key = aprod_param_key(+))
                  WHERE T.MG1_DATE = :as_date
                    and T.MG1_TYPE in ('-','A')
                    and T.MG1_PROD_SUBTYPE <> 'S'
                    and T.MG1_OSW_GRP LIKE :as_osw_grp
                    and T.MG1_KIND_ID = L.MG1_KIND_ID(+)
                    and T.MG1_KIND_ID = MGT2_KIND_ID(+)
                    and trim(T.MG1_KIND_ID) = trim(RPT_VALUE(+))
                    and T.MG1_KIND_ID = AI2_KIND_ID(+)
               order by rpt_seq_no,substr(mg1_kind_id,0,2),decode(substr(mg1_kind_id,2,1),mg1_prod_type ,'',substr(mg1_kind_id,2,1)) 
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40030_mg6
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="as_last_date">前一交易日</param>
      /// <param name="as_osw_grp">PDK_MARKET_CLOSE</param>
      /// <returns></returns>
      public DataTable ListMg6Data(DateTime as_date,DateTime as_last_date, string as_osw_grp)
      {
         object[] parms = {
            ":as_date",as_date,
            ":as_last_date",as_last_date,
            ":as_osw_grp",as_osw_grp
            };

         string sql =
             @"SELECT F.MG6_KIND_ID AS F_KIND_ID,F.MG6_UP_DOWN AS F_UP_DOWN,ABS(F.MG6_RETURN_RATE) AS F_RETURN_RATE, 
                      O.MG6_KIND_ID AS O_KIND_ID,O.MG6_UP_DOWN AS O_UP_DOWN,ABS(O.MG6_RETURN_RATE) AS O_RETURN_RATE, 
                      F.APDK_STOCK_ID,PDK_UP_DOWN,PDK_RETURN_RATE,NVL(F.APDK_PROD_SUBTYPE,O.APDK_PROD_SUBTYPE) AS APDK_PROD_SUBTYPE,
                      PDK_DATE as data_date
                FROM 
                     (SELECT MG6_KIND_ID,MG6_UP_DOWN,MG6_RETURN_RATE,APDK_PROD_SUBTYPE,case when MG6_KIND_ID = 'TGF' then 'FIXNTP' else APDK_STOCK_ID end as APDK_STOCK_ID FROM CI.MG6,ci.APDK WHERE MG6_DATE = :as_date AND MG6_PROD_TYPE= 'F' AND MG6_KIND_ID = APDK_KIND_ID) F, 
                     (SELECT MG6_KIND_ID,MG6_UP_DOWN,MG6_RETURN_RATE,APDK_PROD_SUBTYPE,APDK_STOCK_ID FROM CI.MG6,ci.APDK WHERE MG6_DATE = :as_date AND MG6_PROD_TYPE= 'O' AND MG6_KIND_ID = APDK_KIND_ID ) O,
                     (SELECT T.PDK_KIND_ID, 
                             case when T.SCP_CLOSE_PRICE is null then case when T.PDK_PROD_IDX = 0 then null else (T.PDK_PROD_IDX - Y.PDK_PROD_IDX) end
                                                               else case when T.SCP_CLOSE_PRICE = 0 then null else (T.SCP_CLOSE_PRICE - Y.SCP_CLOSE_PRICE) end end AS PDK_UP_DOWN,      
                             case when T.SCP_CLOSE_PRICE is null then case when Y.PDK_PROD_IDX = 0 then null else CEIL((T.PDK_PROD_IDX - Y.PDK_PROD_IDX)/Y.PDK_PROD_IDX*10000)/10000 end
                                                               else case when Y.SCP_CLOSE_PRICE = 0 then null else CEIL((T.SCP_CLOSE_PRICE - Y.SCP_CLOSE_PRICE)/Y.SCP_CLOSE_PRICE*10000)/10000 end end as PDK_RETURN_RATE,
                             T.PDK_MARKET_CLOSE,T.PDK_DATE
                        FROM ci.HPDK T,ci.HPDK Y
                       WHERE T.PDK_DATE = :as_date
                         and T.PDK_PROD_TYPE = 'F'
                         and T.PDK_SUBTYPE <> 'S'
                         --and (T.PDK_SUBTYPE <> 'S' or T.PDK_PARAM_KEY = 'ETF')
                         and T.PDK_KIND_ID not in ('TGF','GDF','RHF','RTF')
                         AND Y.PDK_DATE = :as_last_date
                         AND T.PDK_KIND_ID = Y.PDK_KIND_ID
                      UNION ALL
                     SELECT T.AMIFU_KIND_ID,(T.AMIFU_CLOSE_PRICE - Y.AMIFU_CLOSE_PRICE) as PDK_UP_DOWN,
                            CEIL((T.AMIFU_CLOSE_PRICE - Y.AMIFU_CLOSE_PRICE)/Y.AMIFU_CLOSE_PRICE*  10000)/10000 as PDK_RETURN_RATE ,
                            APDK_MARKET_CLOSE,T.AMIFU_DATE
                       FROM CI.AMIFU T,CI.AMIFU Y,ci.APDK
                      WHERE T.AMIFU_DATE = :as_date
                        AND T.AMIFU_KIND_ID in ('RHF','RTF','GDF','TGF')
                        AND T.AMIFU_SETTLE_DATE = '000000'
                        AND T.AMIFU_KIND_ID = APDK_KIND_ID
                        AND Y.AMIFU_DATE =  :as_last_date
                        AND Y.AMIFU_KIND_ID = T.AMIFU_KIND_ID
                        AND Y.AMIFU_SETTLE_DATE = T.AMIFU_SETTLE_DATE) 
                WHERE trim(F.APDK_STOCK_ID) = trim(O.APDK_STOCK_ID(+))
                  and PDK_KIND_ID = F.MG6_KIND_ID
                  AND PDK_MARKET_CLOSE LIKE :as_osw_grp
                ORDER BY apdk_stock_id";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40030_mg8
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public DataTable ListMg8Data(DateTime ad_date, string as_osw_grp)
      {
         object[] parms = {
            ":ad_date",ad_date,
            ":as_osw_grp",as_osw_grp
            };

         string sql =
             @"select '1' as data_type,'TAIFEX'as COM ,MG1_KIND_ID,MG1_CM,MG1_PRICE ,0 as EXCHANGE_RATE,
                      MG1_CM AS OUT_M,'' AS F_NAME,
                      MG1_CUR_CM as CUR_CM,
                      MG1_XXX as P_XXX,MG1_DATE  as data_date
                 from ci.MG1,
                     (SELECT COD_ID as KIND_ID FROM CI.COD 
                       WHERE COD_TXN_ID  ='MGT8' 
                         AND COD_COL_ID = 'MGT8_PDK_KIND_ID')
                WHERE MG1_DATE = :ad_date
                  AND MG1_KIND_ID = KIND_ID
                  and MG1_OSW_GRP like :as_osw_grp
                UNION
               select '2',MGT8_F_ID,MGT8_PDK_KIND_ID,MG8_CM,
                      MG9_PRICE,0,
                      --MG5_LAST_CLOSE_PRICE,NVL(MG5_EXCHANGE_RATE,0),
                      MG8_CM AS OUT_CM,
                      MGT8_F_EXCHANGE,null,MGT8_XXX,TO_DATE(MG8_EFFECT_YMD,'YYYYMMDD')
                 from ci.MG8,ci.MGT8,ci.APDK,
                     (select MG8_F_ID as MAX_F_ID,max(MG8_EFFECT_YMD) as MAX_YMD 
                        from ci.MG8 
                       where MG8_EFFECT_YMD <= TO_CHAR(:ad_date,'YYYYMMDD') 
                       group by MG8_F_ID) M8,
                     (select MG9_YMD,MG9_F_ID,MG9_PRICE
                        from ci.MG9,
                            (select MG9_F_ID as MAX_F_ID,max(MG9_YMD) as MAX_YMD 
                               from ci.MG9
                              where MG9_YMD <= TO_CHAR(:ad_date,'YYYYMMDD') 
                              group by MG9_F_ID)
                       where MG9_YMD = MAX_YMD
                         and MG9_F_ID = MAX_F_ID) M9
                WHERE MG8_EFFECT_YMD = MAX_YMD
                  and MG8_F_ID = MAX_F_ID
                  AND NVL(MGT8_PDK_KIND_ID,' ') <> ' ' 
                  and MAX_F_ID = MGT8_F_ID 
                  and MAX_F_ID = MG9_F_ID(+) 
                  AND MGT8_PDK_KIND_ID = APDK_KIND_ID
                  and APDK_MARKET_CLOSE like :as_osw_grp
               ORDER BY data_type,com,mg1_kind_id";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40030_day 沒有as_osw_grp這個參數
      /// </summary>
      /// <param name="as_trade_date"></param>
      /// <returns></returns>
      public DataTable ListDayData(DateTime as_trade_date)
      {
         object[] parms = {
            ":as_trade_date",as_trade_date
            };

         string sql =
             @"select  D.MG1_PROD_TYPE as MG1_PROD_TYPE,
                D.MG1_KIND_ID as MG1_KIND_ID,
                count(M.MG1_DATE) as DAY_CNT
           from
               (SELECT MIN_KIND_ID as MG1_KIND_ID,MIN_PROD_TYPE as MG1_PROD_TYPE,
                    case when MG4_DATE is null and MG1_DATE is null then MIN_DATE - 1
                            --生效同第1天,則減1天
                            when MG4_DATE = MIN_DATE  and  (MG1_DATE is null or MG4_DATE > MG1_DATE )then MG4_DATE - 1
                            when NVL((MG4_DATE),TO_DATE('19010101','YYYYMMDD')) > NVL((MG1_DATE),TO_DATE('19010101','YYYYMMDD')) then MG4_PREV_DATE 
                            else MG1_DATE end  as M_DATE 
                  FROM   
                       --商品最小日期     
                     (SELECT MG1_PROD_TYPE as MIN_PROD_TYPE,
                             M.MG1_KIND_ID AS MIN_KIND_ID,  
                             MIN(MG1_DATE) as MIN_DATE 
                        FROM CI.MG1 M
                       WHERE MG1_DATE <= :as_trade_date
                         and MG1_TYPE in ( '-','A' )
                       GROUP BY MG1_PROD_TYPE,M.MG1_KIND_ID),                         
                      --MG1最後不需調整日期                   
                     (SELECT M.MG1_KIND_ID,  
                             MAX(MG1_DATE) as MG1_DATE 
                        FROM CI.MG1 M, 
                                 (SELECT MG1_KIND_ID
                               FROM CI.MG1
                              WHERE MG1_DATE = :as_trade_date AND  
                                    MG1_TYPE in ( '-','A' )  AND
                                    MG1_CHANGE_FLAG = 'Y') K
                       WHERE MG1_DATE <  :as_trade_date
                         and M.MG1_KIND_ID = K.MG1_KIND_ID
                         and MG1_TYPE in ( '-','A' )
                         and MG1_CHANGE_FLAG <> 'Y'
                       GROUP BY MG1_PROD_TYPE,M.MG1_KIND_ID),
                      --調整日期 
                            (SELECT MG4_KIND_ID,   
                                    MAX(MG4_DATE) as MG4_DATE ,
                                    MAX(MG4_PREV_DATE) as MG4_PREV_DATE         
                                FROM CI.MG4 
                            WHERE MG4_DATE <= :as_trade_date
                            GROUP BY MG4_KIND_ID)                      
                 WHERE MIN_KIND_ID = MG1_KIND_ID(+) 
                   AND MIN_KIND_ID = MG4_KIND_ID(+)) D,
                 ci.MG1 M
         where M.MG1_KIND_ID = D.MG1_KIND_ID
           and MG1_TYPE in ( '-','A' )
           and MG1_DATE >  M_DATE
           and MG1_DATE <= :as_trade_date
           and MG1_CHANGE_FLAG = 'Y'
         group by D.MG1_PROD_TYPE,D.MG1_KIND_ID,D.M_DATE
         ORDER BY mg1_kind_id";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40040_mgt3
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public DataTable ListMgt3Data(DateTime as_date)
      {
         object[] parms = {
            ":as_date",as_date
            };

         string sql =
             @"SELECT MGT3_DATE_FM,   
                      MGT3_DATE_TO,   
                      MGT3_MEMO,   
                      MGT3_W_TIME,   
                      MGT3_W_USER_ID  
                  FROM CI.MGT3 
                  WHERE ( MGT3_DATE_FM <= :as_date ) AND  
                       ( MGT3_DATE_TO >= :as_date )";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40040_etf_mg6
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public DataTable ListEtfMg6Data(DateTime as_date)
      {
         object[] parms = {
            ":as_date",as_date
            };

         string sql =
             @"SELECT MG6_PROD_TYPE,MG6_KIND_ID,APDK_STOCK_ID,
                     ROW_NUMBER( ) OVER (PARTITION BY APDK_PROD_TYPE,APDK_STOCK_ID ORDER BY case when substr(MG6_KIND_ID,3,1) = APDK_PROD_TYPE then ' ' else substr(MG6_KIND_ID,3,1) end NULLS LAST) as KIND_SEQ_NO,
                     MG6_UP_DOWN,ABS(NVL(MG6_RETURN_RATE,0)) as MG6_RETURN_RATE,MG6_XXX,
                     AI5_UP_DOWN,AI5_RETURN_RATE
                  FROM CI.MG6,ci.APDK,
                      (select ai5_kind_id, (ai5_settle_price - ai5_open_ref) as ai5_up_down, 
                              case when ai5_open_ref = 0 then 0 else trunc(((ai5_settle_price / ai5_open_ref) - 1)  * 1000) / 1000 end as ai5_return_rate
                         from ci.ai5
                        where ai5_date = :as_date
                          and ai5_prod_type = 'F')                
                 WHERE MG6_DATE = :as_date
                    and MG6_PROD_SUBTYPE = 'S'
                   AND MG6_KIND_ID = APDK_KIND_ID
                   and MG6_KIND_ID = AI5_KIND_ID(+)";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40040_etf
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="as_last_date">前一交易日</param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public DataTable ListEtfData(DateTime as_date, DateTime as_last_date, string as_osw_grp)
      {
         object[] parms = {
            ":as_date",as_date,
            ":as_last_date",as_last_date,
            ":as_osw_grp",as_osw_grp
            };

         string sql =
             @"SELECT T.MG1_DATE AS MG1_DATE,   
                     T.MG1_KIND_ID AS MG1_KIND_ID,  
                     T.MG1_IM AS MG1_IM,
                     T.MG1_CHANGE_RANGE as MG1_CHANGE_RANGE,
                     T.MG1_PRICE AS MG1_PRICE,
                     T.MG1_CM AS MG1_CM,
                     L.MG1_CHANGE_RANGE AS MG1_CHANGE_RANGE_LAST,
                     L.MG1_PRICE AS MG1_PRICE_LAST,
                     T.MG1_CUR_CM AS MG1_CM_LAST,
                     T.MG1_SEQ_NO,
                     APDK_KIND_ID as MGT2_KIND_ID_OUT,
                     APDK_NAME,APDK_STOCK_ID,
                     PID_NAME,
                     APDK_PROD_TYPE,
                     T.MG1_CHANGE_FLAG,
                     oi_rate,ai2_oi,aprod_7date,aprod_delivery_date,tot_oi
                  FROM ci.MG1 T,ci.APDK,
                     (select MG1_KIND_ID,MG1_CHANGE_RANGE,MG1_PRICE 
                        from ci.MG1 
                        where MG1_DATE(+) = :as_last_date
                        and MG1_TYPE(+) in ('-','A')) L,
                     --上市/上櫃中文名稱
                     (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM'),
                     --OI
                     (select ai2_kind_id,ai2_oi,tot_oi,
                              case when tot_oi = 0 then 0 else round(ai2_oi/tot_oi,4) end as oi_rate,
                              aprod_7date,aprod_delivery_date,aprod_settle_date,aprod_end_date
                        from ci.ai2,
                           (select aprod_param_key 
                                    ,fut.DATE_DIFF_OCF_DAYS(max(aprod_delivery_date),-7) as aprod_7date,aprod_settle_date,max(aprod_end_date) as aprod_end_date,max(aprod_delivery_date) as aprod_delivery_date
                              from ci.aprod,
                                 (select amif_param_key ,min(amif_settle_date) as amif_settle_date from ci.amif
                                    where amif_date = to_date(to_char(:as_date,'YYYYMMDD'),'YYYYMMDD')
                                    and amif_data_source = 'T'
                                    and amif_prod_subtype = 'S'
                                    group by amif_param_key )
                              where aprod_param_key  = amif_param_key 
                              and aprod_settle_date = amif_settle_date
                              group by aprod_param_key ,aprod_settle_date),
                           (select sum(ai2_oi) as tot_oi 
                              from ci.ai2
                              where ai2_ymd = to_char(:as_date,'YYYYMMDD')
                              and ai2_sum_type = 'D'
                              and ai2_sum_subtype = '1'
                              and ai2_prod_type in ('F','O')) T
                        where ai2_ymd = to_char(:as_date,'YYYYMMDD')
                        and ai2_sum_type = 'D'
                        and ai2_sum_subtype = '4'
                        and ai2_prod_type in ('F','O')
                        and ai2_prod_subtype = 'S'
                        and ai2_param_key in ('ETF','ETC')
                        and ai2_param_key  = aprod_param_key(+))
               WHERE T.MG1_DATE = :as_date
                  and T.MG1_TYPE in ('-','A')
                  and T.MG1_OSW_GRP LIKE :as_osw_grp
                  and T.MG1_KIND_ID = L.MG1_KIND_ID(+)
                  and T.MG1_KIND_ID = APDK_KIND_ID
                  and APDK_UNDERLYING_MARKET = COD_ID 
                  and T.MG1_KIND_ID = AI2_KIND_ID(+)
                  AND T.MG1_PARAM_KEY IN ('ETF','ETC')
               ORDER BY apdk_prod_type,mg1_kind_id";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40040_SPAN_SV
      /// </summary>
      /// <param name="ad_trade_date">當日交易日期</param>
      /// <param name="ad_start_date">Excel Template預設日期 ex:2014/12/1</param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public DataTable ListSpanSvData(DateTime ad_trade_date, DateTime ad_start_date, string as_osw_grp)
      {
         object[] parms = {
            ":ad_trade_date",ad_trade_date,
            ":ad_start_date",ad_start_date,
            ":as_osw_grp",as_osw_grp
            };

         string sql =
             @"select SP1_KIND_ID1,SP1_CHANGE_RANGE,nvl(DAY_CNT,0) as DAY_CNT,RPT_SEQ_NO ,SP1_CHANGE_FLAG
         from ci.SP1,
             (select SP1_KIND_ID1 as D_KIND_ID1,SP1_KIND_ID2 as D_KIND_ID2,count(*) as DAY_CNT,MIN_DATE
                from ci.SP1,
                   (SELECT MIN_KIND_ID1,MIN_KIND_ID2,
                    case when SP3_DATE is null and SP1_DATE is null then MIN_DATE  -  1
                            --生效同第1天,則減1天
                            when SP3_DATE = MIN_DATE  and  (SP1_DATE is null or SP3_DATE = SP1_DATE ) then SP3_DATE - 1
                            when NVL((SP3_DATE),TO_DATE('19010101','YYYYMMDD')) > NVL((SP1_DATE),TO_DATE('19010101','YYYYMMDD')) then SP3_PREV_DATE 
                            else SP1_DATE end  as MIN_DATE 
                  FROM   
                       --商品最小日期     
                     (SELECT SP1_KIND_ID1 AS MIN_KIND_ID1,
                             SP1_KIND_ID2 AS MIN_KIND_ID2,  
                             MIN(SP1_DATE) as MIN_DATE 
                        FROM CI.SP1
                       WHERE SP1_DATE <= :ad_trade_date
                         and SP1_TYPE = 'SV'
                         and SP1_CHANGE_FLAG = 'Y'
                       GROUP BY SP1_KIND_ID1,SP1_KIND_ID2),                         
                      --MG1最後不需調整日期                   
                     (SELECT M.SP1_KIND_ID1,M.SP1_KIND_ID2,  
                             MAX(SP1_DATE) as SP1_DATE 
                        FROM CI.SP1 M, 
                                 (SELECT SP1_KIND_ID1,SP1_KIND_ID2
                               FROM CI.SP1
                              WHERE SP1_DATE = :ad_trade_date 
                                AND SP1_TYPE = 'SV'
                                AND SP1_CHANGE_FLAG = 'Y') K
                       WHERE SP1_DATE <  :ad_trade_date
                         and M.SP1_KIND_ID1 = K.SP1_KIND_ID1
                         and M.SP1_KIND_ID2 = K.SP1_KIND_ID2
                         AND SP1_TYPE = 'SV'
                         AND SP1_CHANGE_FLAG <> 'Y'
                       GROUP BY M.SP1_KIND_ID1,M.SP1_KIND_ID2),
                      --調整日期 
                            (SELECT SP3_KIND_ID1,SP3_KIND_ID2,   
                                    MAX(SP3_DATE) as SP3_DATE ,
                                    MAX(SP3_PREV_DATE) as SP3_PREV_DATE         
                                FROM CI.SP3 
                            WHERE SP3_DATE <= :ad_trade_date
                            GROUP BY SP3_KIND_ID1,SP3_KIND_ID2)                      
                 WHERE MIN_KIND_ID1 = SP1_KIND_ID1(+) 
                   AND MIN_KIND_ID2 = SP1_KIND_ID2(+) 
                   AND MIN_KIND_ID1 = SP3_KIND_ID1(+) 
                   AND MIN_KIND_ID2 = SP3_KIND_ID2(+))
              where SP1_DATE <= :ad_trade_date 
                and SP1_DATE >= :ad_start_date 
                and SP1_TYPE = 'SV'
                and SP1_KIND_ID1 = MIN_KIND_ID1
                and SP1_KIND_ID2 = MIN_KIND_ID2
                and SP1_DATE > MIN_DATE
              group by SP1_KIND_ID1,SP1_KIND_ID2,MIN_DATE),
          (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT where RPT_TXN_ID = '40040' and RPT_TXD_ID = '40043' and RPT_VALUE_3 = 'SV')
        where SP1_DATE = :ad_trade_date
          and SP1_TYPE = 'SV'
          and SP1_OSW_GRP LIKE :as_osw_grp
          and SP1_KIND_ID1 = D_KIND_ID1(+)
          and SP1_KIND_ID1 = RPT_VALUE";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40040_SPAN_SD
      /// </summary>
      /// <param name="ad_trade_date">當日交易日期</param>
      /// <param name="ad_start_date">Excel Template預設日期 ex:2014/12/1</param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public DataTable ListSpanSdData(DateTime ad_trade_date, DateTime ad_start_date, string as_osw_grp)
      {
         object[] parms = {
            ":ad_trade_date",ad_trade_date,
            ":ad_start_date",ad_start_date,
            ":as_osw_grp",as_osw_grp
            };

         string sql =
             @"select SP1_KIND_ID1,SP1_KIND_ID2,SP1_CHANGE_RANGE,nvl(DAY_CNT,0) as DAY_CNT,
             RPT_COL,RPT_ROW ,SP1_CHANGE_FLAG
        from ci.SP1,
             (select SP1_KIND_ID1 as D_KIND_ID1,SP1_KIND_ID2 as D_KIND_ID2,count(*) as DAY_CNT,MIN_DATE
                from ci.SP1,
                   (SELECT MIN_KIND_ID1,MIN_KIND_ID2,
                    case when SP3_DATE is null and SP1_DATE is null then MIN_DATE  -  1
                            --生效同第1天,則減1天
                            when SP3_DATE = MIN_DATE  and  (SP1_DATE is null or SP3_DATE = SP1_DATE ) then SP3_DATE - 1
                            when NVL((SP3_DATE),TO_DATE('19010101','YYYYMMDD')) > NVL((SP1_DATE),TO_DATE('19010101','YYYYMMDD')) then SP3_PREV_DATE 
                            else SP1_DATE end  as MIN_DATE 
                  FROM   
                       --商品最小日期     
                     (SELECT SP1_KIND_ID1 AS MIN_KIND_ID1,
                             SP1_KIND_ID2 AS MIN_KIND_ID2,  
                             MIN(SP1_DATE) as MIN_DATE 
                        FROM CI.SP1
                       WHERE SP1_DATE <= :ad_trade_date
                         and SP1_TYPE = 'SD'
                         and SP1_CHANGE_FLAG = 'Y'
                       GROUP BY SP1_KIND_ID1,SP1_KIND_ID2),                         
                      --MG1最後不需調整日期                   
                     (SELECT M.SP1_KIND_ID1,M.SP1_KIND_ID2,  
                             MAX(SP1_DATE) as SP1_DATE 
                        FROM CI.SP1 M, 
                                 (SELECT SP1_KIND_ID1,SP1_KIND_ID2
                               FROM CI.SP1
                              WHERE SP1_DATE = :ad_trade_date 
                                AND SP1_TYPE = 'SD'
                                AND SP1_CHANGE_FLAG = 'Y') K
                       WHERE SP1_DATE <  :ad_trade_date
                         and M.SP1_KIND_ID1 = K.SP1_KIND_ID1
                         and M.SP1_KIND_ID2 = K.SP1_KIND_ID2
                         AND SP1_TYPE = 'SD'
                         AND SP1_CHANGE_FLAG <> 'Y'
                       GROUP BY M.SP1_KIND_ID1,M.SP1_KIND_ID2),
                      --調整日期 
                            (SELECT SP3_KIND_ID1,SP3_KIND_ID2,   
                                    MAX(SP3_DATE) as SP3_DATE ,
                                    MAX(SP3_PREV_DATE) as SP3_PREV_DATE         
                                FROM CI.SP3 
                            WHERE SP3_DATE <= :ad_trade_date
                            GROUP BY SP3_KIND_ID1,SP3_KIND_ID2)                      
                 WHERE MIN_KIND_ID1 = SP1_KIND_ID1(+) 
                   AND MIN_KIND_ID2 = SP1_KIND_ID2(+) 
                   AND MIN_KIND_ID1 = SP3_KIND_ID1(+) 
                   AND MIN_KIND_ID2 = SP3_KIND_ID2(+))
              where SP1_DATE <= :ad_trade_date 
                and SP1_DATE >= :ad_start_date 
                and SP1_TYPE = 'SD'
                and SP1_KIND_ID1 = MIN_KIND_ID1
                and SP1_KIND_ID2 = MIN_KIND_ID2
                and SP1_DATE > MIN_DATE
              group by SP1_KIND_ID1,SP1_KIND_ID2,MIN_DATE),
          (SELECT RPT_VALUE as RPT_KIND_ID1,RPT_VALUE_2 as RPT_KIND_ID2,RPT_LEVEL_1 as RPT_COL,RPT_LEVEL_2  as RPT_ROW
             FROM CI.RPT where RPT_TXN_ID = '40040' and RPT_TXD_ID = '40043' and RPT_VALUE_3 = 'SD')
        where SP1_DATE = :ad_trade_date
          and SP1_CHANGE_FLAG = 'Y'
          and SP1_TYPE = 'SD'
          and SP1_OSW_GRP LIKE :as_osw_grp
          and SP1_KIND_ID1 = D_KIND_ID1(+)
          and SP1_KIND_ID2 = D_KIND_ID2(+)
          and SP1_KIND_ID1 = RPT_KIND_ID1(+)
          and SP1_KIND_ID2 = RPT_KIND_ID2(+)
       ORDER BY sp1_kind_id1,sp1_kind_id2";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


   }
}
