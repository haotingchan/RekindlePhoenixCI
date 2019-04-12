using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// John, 2019/4/8
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40190 : DataGate
   {

      /// <summary>
      /// 保證金適用比例級距import F data
      /// </summary>
      /// <returns></returns>
      public DataTable Get42010Mgrt1FData()
      {

         string sql =
                  @"
                     SELECT
                         MGRT1_LEVEL_NAME,
                         MGRT1_CM_RATE,
                         '', 
                         MGRT1_MM_RATE,
                         '', 
                         MGRT1_IM_RATE
                     FROM CI.MGRT1
                     WHERE MGRT1_PROD_TYPE = 'F'
                       and MGRT1_REPORT = 'Y'
                     ";
         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// 保證金適用比例級距import O data
      /// </summary>
      /// <returns></returns>
      public DataTable Get42010Mgrt1OData()
      {

         string sql =
                  @"SELECT MGRT1_LEVEL_NAME, 
                           MGRT1_CM_RATE,
                           MGRT1_CM_RATE_B,
                           MGRT1_MM_RATE,
                           MGRT1_MM_RATE_B,
                           MGRT1_IM_RATE,
                           MGRT1_IM_RATE_B
                      FROM CI.MGRT1  
                     WHERE ( MGRT1_PROD_TYPE = 'O' ) AND  
                           ( MGRT1_REPORT = 'Y' )
                     ";
         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// return MG1_CUR_CM/MG1_CUR_MM/MG1_CUR_IM/MG1_CM_RATE/MG1_MM_RATE/MG1_IM_RATE/CURRENCY_NAME/APRF_MAX_RAISE_FALL
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public DataTable Get49191FUT(DateTime as_date)
      {

         object[] parms = {
                ":as_date",as_date,
            };

         string sql =
@"select rpt.RPT_SEQ_NO,MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                        CURRENCY_NAME,
                        decode(APRF_PRICE_FLUC,'P',APRF_MAX_RAISE_FALL||'%','新臺幣'||APRF_MAX_RAISE_FALL||'元') as APRF_MAX_RAISE_FALL
                        from
    (select RPT_SEQ_NO from CI.RPT  where RPT_TXD_ID = '40191' order by RPT_SEQ_NO) rpt
left join
                        (select * from
                        (SELECT MG1_KIND_ID,   
                                 MG1_TYPE,   
                                 MG1_CUR_CM,   
                                 MG1_CUR_MM,   
                                 MG1_CUR_IM,   
                                 CASE MG1_KIND_ID WHEN 'CPF' THEN MG1_PRICE ELSE MG1_PRICE * MG1_XXX END as M_PRICE,   
                                 RPT_SEQ_NO,
                                 MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE ,
                                 MG1_PROD_SUBTYPE,
                                 APDK_STOCK_ID,APDK_NAME,
                                 APRF_MAX_RAISE_FALL,
                                 APRF_PRICE_FLUC,
                                 CURRENCY_NAME
                            FROM CI.MG1,  ci.APDK,
                                 (select RPT_VALUE,RPT_SEQ_NO from CI.RPT  where RPT_TXD_ID = '40191'),
                                 --漲跌幅
                                 (SELECT APRF_KIND_ID,APRF_PRICE_FLUC,
                                         --case when APRF_RAISE_OPEN2 = 'Y' then APRF_RAISE_LIMIT2 
                                         --     when APRF_RAISE_OPEN1 = 'Y' then APRF_RAISE_LIMIT1
                                         --     else APRF_RAISE_LIMIT end AS APRF_MAX_RAISE_FALL             
                                         GREATEST(GREATEST(APRF_RAISE_LIMIT,NVL(APRF_RAISE_LIMIT1,0)),NVL(APRF_RAISE_LIMIT2,0)) AS APRF_MAX_RAISE_FALL
                                    FROM CI.APRF 
                                   WHERE APRF_YMD = TO_CHAR(:as_date,'YYYYMMDD')
                                      AND APRF_SETTLE_DATE = ' '),
                                 --幣別
                                 (SELECT trim(COD_ID) as CURRENCY_TYPE,trim(COD_DESC) as CURRENCY_NAME
                                    FROM CI.COD 
                                   WHERE COD_TXN_ID = 'EXRT' AND COD_COL_ID = 'EXRT_CURRENCY_TYPE')
                             WHERE MG1_DATE = :as_date
                               and MG1_PROD_TYPE = 'F'
                               and MG1_KIND_ID = RPT_VALUE(+) 
                               and MG1_KIND_ID = APDK_KIND_ID
                               and MG1_KIND_ID = APRF_KIND_ID(+)
                               and APDK_CURRENCY_TYPE = CURRENCY_TYPE(+)
                        UNION ALL
                        select 'SGX','-',
                               MG8_CM , MG8_MM ,MG8_IM , 
                               MG9_PRICE,
                               (SELECT RPT_SEQ_NO from ci.RPT  WHERE RPT_TXD_ID = '40191' AND RPT_VALUE = 'SGX'),
                               case when MG9_PRICE is null then 0 else  TRUNC((MG8_CM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                               case when MG9_PRICE is null then 0 else  TRUNC((MG8_MM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                               case when MG9_PRICE is null then 0 else  TRUNC((MG8_IM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                               'I',NULL,NULL,decode(NULL,null,15) as APRF_MAX_RAISE_FALL,decode(NULL,null,'P'),decode(NULL,null,'美元') as CURRENCY_NAME
                          from ci.MG8D,ci.MG8,ci.MG9,ci.MGT8
                         where MG8D_YMD = to_char(:as_date,'YYYYMMDD')
                           AND MG8D_F_ID = 'SGX02'
                           AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
                           AND MG8D_F_ID = MG8_F_ID
                           AND MG8D_YMD = MG9_YMD
                           AND MG8D_F_ID = MG9_F_ID   
                           AND MG8D_F_ID = MGT8_F_ID)
                        where mg1_prod_subtype <> 'S' and  rpt_seq_no IS NOT NULL
                        order by rpt_seq_no,mg1_type,mg1_kind_id 
                        )main 
on main.RPT_SEQ_NO = rpt.RPT_SEQ_NO
order by RPT_SEQ_NO";
         DataTable dtResult = db.GetDataTable(sql, parms);
         //import 商品資料行合併
         //string sql2 = @"select RPT_SEQ_NO,
         //               NULL as MG1_CUR_CM,NULL as MG1_CUR_MM,NULL as MG1_CUR_IM,NULL as MG1_CM_RATE,NULL as MG1_MM_RATE,NULL as MG1_IM_RATE,
         //               NULL as CURRENCY_NAME,
         //               NULL as APRF_MAX_RAISE_FALL from CI.RPT  where RPT_TXD_ID = '40191' order by RPT_SEQ_NO";
         //DataTable mergeDT = db.GetDataTable(sql2, null);
         //DataTable dt = MergeData(mergeDT, dtResult, "RPT_SEQ_NO");
         dtResult.Columns.Remove(dtResult.Columns["RPT_SEQ_NO"]);
         return dtResult;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public DataTable Get49191ETF(DateTime as_date)
      {

         object[] parms = {
                ":as_date",as_date
            };

         string sql =
@"
               select trim(MG1_KIND_ID) as MG1_KIND_ID,trim(APDK_NAME) as APDK_NAME,MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE
               from
               (select * from
               (SELECT MG1_KIND_ID,   
                        MG1_TYPE,   
                        MG1_CUR_CM,   
                        MG1_CUR_MM,   
                        MG1_CUR_IM,   
                        CASE MG1_KIND_ID WHEN 'CPF' THEN MG1_PRICE ELSE MG1_PRICE * MG1_XXX END as M_PRICE,   
                        RPT_SEQ_NO,
                        MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE ,
                        MG1_PROD_SUBTYPE,
                        APDK_STOCK_ID,APDK_NAME,
                        APRF_MAX_RAISE_FALL,
                        APRF_PRICE_FLUC,
                        CURRENCY_NAME
                   FROM CI.MG1,  ci.APDK,
                        (select RPT_VALUE,RPT_SEQ_NO from CI.RPT  where RPT_TXD_ID = '40191'),
                        --漲跌幅
                        (SELECT APRF_KIND_ID,APRF_PRICE_FLUC,
                                --case when APRF_RAISE_OPEN2 = 'Y' then APRF_RAISE_LIMIT2 
                                --     when APRF_RAISE_OPEN1 = 'Y' then APRF_RAISE_LIMIT1
                                --     else APRF_RAISE_LIMIT end AS APRF_MAX_RAISE_FALL             
                                GREATEST(GREATEST(APRF_RAISE_LIMIT,NVL(APRF_RAISE_LIMIT1,0)),NVL(APRF_RAISE_LIMIT2,0)) AS APRF_MAX_RAISE_FALL
                           FROM CI.APRF 
                          WHERE APRF_YMD = TO_CHAR(:as_date,'YYYYMMDD')
                             AND APRF_SETTLE_DATE = ' '),
                        --幣別
                        (SELECT trim(COD_ID) as CURRENCY_TYPE,trim(COD_DESC) as CURRENCY_NAME
                           FROM CI.COD 
                          WHERE COD_TXN_ID = 'EXRT' AND COD_COL_ID = 'EXRT_CURRENCY_TYPE')
                    WHERE MG1_DATE = :as_date
                      and MG1_PROD_TYPE = 'F'
                      and MG1_KIND_ID = RPT_VALUE(+) 
                      and MG1_KIND_ID = APDK_KIND_ID
                      and MG1_KIND_ID = APRF_KIND_ID(+)
                      and APDK_CURRENCY_TYPE = CURRENCY_TYPE(+)
               UNION ALL
               select 'SGX','-',
                      MG8_CM , MG8_MM ,MG8_IM , 
                      MG9_PRICE,
                      (SELECT RPT_SEQ_NO from ci.RPT  WHERE RPT_TXD_ID = '40191' AND RPT_VALUE = 'SGX'),
                      case when MG9_PRICE is null then 0 else  TRUNC((MG8_CM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                      case when MG9_PRICE is null then 0 else  TRUNC((MG8_MM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                      case when MG9_PRICE is null then 0 else  TRUNC((MG8_IM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                      'I',NULL,NULL,NULL,NULL,NULL
                 from ci.MG8D,ci.MG8,ci.MG9,ci.MGT8
                where MG8D_YMD = to_char(:as_date,'YYYYMMDD')
                  AND MG8D_F_ID = 'SGX02'
                  AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
                  AND MG8D_F_ID = MG8_F_ID
                  AND MG8D_YMD = MG9_YMD
                  AND MG8D_F_ID = MG9_F_ID   
                  AND MG8D_F_ID = MGT8_F_ID)
               where mg1_prod_subtype = 'S'
               order by rpt_seq_no,mg1_type,mg1_kind_id 
               )

";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


      /// <summary>
      /// return MG1_CUR_CM/MG1_CUR_MM/MG1_CUR_IM/MG1_CM_RATE/MG1_MM_RATE/MG1_IM_RATE/CURRENCY_NAME/APRF_MAX_RAISE_FALL
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public DataTable Get49192FUT(DateTime as_date)
      {

         object[] parms = {
                ":as_date",as_date,
            };

         string sql =
            @"select MG1_CUR_CM,'',MG1_CUR_MM,'',MG1_CUR_IM
            from
            (SELECT MG1_KIND_ID,   
                     MG1_TYPE,   
                     MG1_CUR_CM,   
                     MG1_CUR_MM,   
                     MG1_CUR_IM,   
                     MG1_PRICE,   
                     RPT_SEQ_NO,
                     MG1_PROD_SUBTYPE,
                    APDK_NAME
                FROM CI.MG1, ci.APDK,
                     (select RPT_VALUE,RPT_SEQ_NO from CI.RPT  where RPT_TXD_ID = '40192')
               WHERE MG1_DATE = :as_date 
                 and MG1_KIND_ID = RPT_VALUE(+)
                 and MG1_PROD_TYPE = 'O'
                 and MG1_KIND_ID = APDK_KIND_ID
                 and mg1_prod_subtype <> 'S' and  rpt_seq_no IS NOT NULL
               ORDER BY rpt_seq_no,mg1_kind_id,mg1_type)                   
            ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return MG1_KIND_ID/APDK_NAME/MG1_TYPE/MG1_CUR_CM/''/MG1_CUR_MM/''/MG1_CUR_IM
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public DataTable Get49192ETF(DateTime as_date)
      {

         object[] parms = {
                ":as_date",as_date
            };

         string sql =
                     @"
                     select decode(MG1_TYPE,'A',trim(MG1_KIND_ID),null) as MG1_KIND_ID,
                     decode(MG1_TYPE,'A',trim(APDK_NAME),null) as APDK_NAME,
                     '風險保證金（'||MG1_TYPE||'值）' as MG1_TYPE,MG1_CUR_CM,'',MG1_CUR_MM,'',MG1_CUR_IM
                     from
                     (SELECT MG1_KIND_ID,   
                              MG1_TYPE,   
                              MG1_CUR_CM,   
                              MG1_CUR_MM,   
                              MG1_CUR_IM,   
                              MG1_PRICE,   
                              RPT_SEQ_NO,
                              MG1_PROD_SUBTYPE,
                             APDK_NAME
                         FROM CI.MG1, ci.APDK,
                              (select RPT_VALUE,RPT_SEQ_NO from CI.RPT  where RPT_TXD_ID = '40192')
                        WHERE MG1_DATE = :as_date 
                          and MG1_KIND_ID = RPT_VALUE(+)
                          and MG1_PROD_TYPE = 'O'
                          and MG1_KIND_ID = APDK_KIND_ID
                          and mg1_prod_subtype = 'S' 
                        ORDER BY rpt_seq_no,mg1_kind_id,mg1_type)
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return MG4_PREV_DATE
      /// </summary>
      /// <param name="as_begin_date"></param>
      /// <param name="as_end_date"></param>
      /// <returns></returns>
      public DataTable Get40193Data(DateTime as_begin_date,DateTime as_end_date)
      {
         object[] parms = {
                ":as_begin_date",as_begin_date,
                ":as_end_date",as_end_date
            };
         string sql = @"
                        select '公告日期：' ||(SUBSTR(TO_CHAR(mg4_prev_date,'YYYYMMDD'),0,4)-1911) || to_char(mg4_prev_date,'/MM/DD')  as MG4_PREV_DATE 
                        from ci.mg4
                        where MG4_PREV_DATE between :as_begin_date and :as_end_date
                            and MG4_PREV_DATE <> MG4_DATE
                        group by MG4_PREV_DATE
                        ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return MGR4_PROD_TYPE/MGR4_KIND_ID/APDK_NAME/MGR4_CM
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public DataTable GetStfData(DateTime as_ymd)
      {

         object[] parms = {
                ":as_ymd",as_ymd.ToString("yyyyMMdd"),
            };

         string sql =
            @"select * from
            (SELECT MGR4_PROD_TYPE,MGR4_KIND_ID,APDK_NAME,
                        nvl((select MGRT1_LEVEL from ci.MGRT1 where MGR4_PROD_TYPE = MGRT1_PROD_TYPE and MGRT1_CM_RATE = MGR4_CM),'Z') as MGRT1_LEVEL,
                         MGR4_CM
            FROM CI.MGR4,ci.APDK
            where MGR4_YMD = :as_ymd   
               and MGR4_KIND_ID = APDK_KIND_ID
               and APDK_PROD_SUBTYPE = 'S'
               and APDK_PARAM_KEY IN ('STF','STC')
            order by MGR4_PROD_TYPE,substr(MGR4_KIND_ID,1,2),case when substr(MGR4_KIND_ID,3,1) = MGR4_PROD_TYPE then ' ' else substr(MGR4_KIND_ID,3,1)  end)
            where MGRT1_LEVEL <> '1'                   
            ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// 取得起始列
      /// </summary>
      public int Get40193SeqNo
      {
         get {
            string sql = @"select rpt_seq_no from ci.RPT WHERE RPT_TXD_ID = '40193'";
            DataTable dtResult = db.GetDataTable(sql, null);
            return dtResult.Rows[0][0].AsInt();
         }
      }

      /// <summary>
      /// 合併資料 方便import
      /// </summary>
      /// <param name="dt1">主資料表</param>
      /// <param name="dt2">要合併進來的data</param>
      /// <param name="key">共同索引鍵</param>
      /// <returns></returns>
      private DataTable MergeData(DataTable dt1,DataTable dt2,string key) {

         DataTable newDataTable = dt2.Clone();
         foreach (DataRow dr in dt1.Rows)
         {
            DataRow newRow = dt2.Select($"{key}='{dr[key]}'").FirstOrDefault();
            if (newRow!=null)
            {
               newDataTable.ImportRow(newRow);
            }
            else
            {
               newDataTable.ImportRow(dr);
            }
         }
         newDataTable.Columns.Remove(newDataTable.Columns[key]);
         newDataTable.AcceptChanges();
         return newDataTable;
      }

   }
}
