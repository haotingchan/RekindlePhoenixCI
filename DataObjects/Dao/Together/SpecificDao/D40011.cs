using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// John, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40011 : DataGate
   {
      /// <summary>
      /// 合併資料 方便import
      /// </summary>
      /// <param name="dt1">主資料表</param>
      /// <param name="dt2">要合併進來的data</param>
      /// <param name="key">共同索引鍵</param>
      /// <returns></returns>
      private DataTable MergeData(DataTable dt1, DataTable dt2, string key)
      {

         DataTable newDataTable = dt2.Clone();
         foreach (DataRow dr in dt1.Rows)
         {
            DataRow newRow = dt2.Select($"{key}='{dr[key]}'").FirstOrDefault();
            if (newRow != null)
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
      /// <summary>
      /// sheet=1 現行收取保證金金額
      /// </summary>
      /// <returns>MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE</returns>
      public DataTable GetFutR1Data(DateTime as_date)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql =
                  @"select rpt.R1,MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE
                     from
                           (select RPT_LEVEL_2 as R1 from ci.RPT
                                 WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_1' AND RPT_VALUE<>'E'
                           ) rpt
                     left join
                           (SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                                 MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                                 MG1_CHANGE_FLAG,
                                 MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                              from ci.MG1,ci.MGT2,
                                 (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                    FROM ci.RPT
                                    WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_1') R
                           where MG1_DATE = :as_date
                              and MG1_KIND_ID = R_KIND_ID
                              and MG1_KIND_ID = MGT2_KIND_ID
                           union all
                           select MG8_CM , MG8_MM ,MG8_IM , 
                                       case when MG9_PRICE is null then 0 else  TRUNC((MG8_CM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                       case when MG9_PRICE is null then 0 else  TRUNC((MG8_MM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                       case when MG9_PRICE is null then 0 else  TRUNC((MG8_IM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                 MG9_PRICE,MGT8_XXX,NULL,NULL,NULL,NULL,NULL,
                                 NULL,MG8D_F_ID,NULL,R1,R2,SHEET,MG8D_F_ID
                              from ci.MG8D,ci.MG8,ci.MG9,ci.MGT8,
                                 (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                    FROM ci.RPT
                                    WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID = '40011_1') R
                           where MG8D_YMD = to_char(:as_date,'YYYYMMDD')
                              AND MG8D_F_ID = 'SGX02'
                              AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
                              AND MG8D_F_ID = MG8_F_ID
                              AND MG8D_YMD = MG9_YMD
                              AND MG8D_F_ID = MG9_F_ID   
                              AND MG8D_F_ID = MGT8_F_ID
                              and MG8D_F_ID = R_KIND_ID 
                           ORDER BY SHEET,R1,R2,MG1_KIND_ID,MG1_TYPE) main on main.R1 = rpt.R1
                     order by R1";
         DataTable dtResult = db.GetDataTable(sql, parms);
         //import 商品資料行合併
         //string sql2 = @"SELECT RPT_LEVEL_2 AS R1,NULL as MG1_CUR_CM,NULL as MG1_CUR_MM,NULL as MG1_CUR_IM,NULL as MG1_CM_RATE,NULL as MG1_MM_RATE,NULL as MG1_IM_RATE
         //                      FROM ci.RPT
         //                     WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_1' AND RPT_VALUE<>'E'
         //                     order by R1";
         //DataTable mergeDT = db.GetDataTable(sql2, null);
         //DataTable dt = MergeData(mergeDT, dtResult, "R1");
         dtResult.Columns.Remove(dtResult.Columns["R1"]);
         return dtResult;
      }

      /// <summary>
      /// sheet=1 本日結算保證金計算
      /// </summary>
      /// <returns>MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM</returns>
      public DataTable GetFutR2Data(DateTime as_date)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql =
                  @"select rpt.R2,MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM
                     from
                         (select RPT_LEVEL_3 as R2 from ci.RPT
                               WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_1' AND RPT_VALUE<>'E'
                         ) rpt
    
                     left join 
                         (SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                                MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                                MG1_CHANGE_FLAG,
                                MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                           from ci.MG1,ci.MGT2,
                                (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                   FROM ci.RPT
                                  WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_1') R
                          where MG1_DATE = :as_date
                            and MG1_KIND_ID = R_KIND_ID
                            and MG1_KIND_ID = MGT2_KIND_ID
                         union all
                         select MG8_CM , MG8_MM ,MG8_IM , 
                                    case when MG9_PRICE is null then 0 else  TRUNC((MG8_CM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                    case when MG9_PRICE is null then 0 else  TRUNC((MG8_MM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                    case when MG9_PRICE is null then 0 else  TRUNC((MG8_IM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                MG9_PRICE,MGT8_XXX,NULL,NULL,NULL,NULL,NULL,
                                NULL,MG8D_F_ID,NULL,R1,R2,SHEET,MG8D_F_ID
                           from ci.MG8D,ci.MG8,ci.MG9,ci.MGT8,
                                (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                   FROM ci.RPT
                                  WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID = '40011_1') R
                          where MG8D_YMD = to_char(:as_date,'YYYYMMDD')
                            AND MG8D_F_ID = 'SGX02'
                            AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
                            AND MG8D_F_ID = MG8_F_ID
                            AND MG8D_YMD = MG9_YMD
                            AND MG8D_F_ID = MG9_F_ID   
                            AND MG8D_F_ID = MGT8_F_ID
                            and MG8D_F_ID = R_KIND_ID
                         ORDER BY SHEET,R1,R2,MG1_KIND_ID,MG1_TYPE
                         ) main on main.R2 = rpt.R2
                     order by R2";
         DataTable dtResult = db.GetDataTable(sql, parms);
         dtResult.Columns.Remove(dtResult.Columns["R2"]);
         //import 商品資料行合併
         //string sql2 = @"SELECT RPT_LEVEL_3 AS R2,NULL as MG1_PRICE,NULL as MG1_XXX,NULL as MG1_RISK,NULL as MG1_CP_RISK,NULL as MG1_MIN_RISK,NULL as MG1_CP_CM
         //                      FROM ci.RPT
         //                     WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_1' AND RPT_VALUE<>'E'
         //                     order by R2";
         //DataTable mergeDT = db.GetDataTable(sql2, null);
         //DataTable dt = MergeData(mergeDT, dtResult, "R1");
         return dtResult;
      }

      /// <summary>
      /// sheet=2 現行收取保證金金額
      /// </summary>
      /// <returns>MG1_CUR_CM,'',MG1_CUR_MM,'',MG1_CUR_IM</returns>
      public DataTable GetOptR1Data(DateTime as_date)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql =
                  @"select rpt.R1,rpt.MG1_TYPE,MG1_CUR_CM,'',MG1_CUR_MM,'',MG1_CUR_IM from
                       ( select R1,MG1_TYPE from
                           (SELECT DISTINCT MG1_TYPE ,R1,R_KIND_ID
                            from ci.MG1,ci.MGT2,
                                 (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                    FROM ci.RPT
                                   WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_2' AND RPT_VALUE <> 'E') R
                           where MG1_KIND_ID = R_KIND_ID
                           order by R1)
                       ) rpt
                   left join
                  (SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                         MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                         MG1_CHANGE_FLAG,
                         MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                    from ci.MG1,ci.MGT2,
                         (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                            FROM ci.RPT
                           WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_2') R
                   where MG1_DATE = :as_date
                     and MG1_KIND_ID = R_KIND_ID
                     and MG1_KIND_ID = MGT2_KIND_ID
                  union all
                  select MG8_CM , MG8_MM ,MG8_IM , 
                             case when MG9_PRICE is null then 0 else  TRUNC((MG8_CM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                             case when MG9_PRICE is null then 0 else  TRUNC((MG8_MM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                             case when MG9_PRICE is null then 0 else  TRUNC((MG8_IM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                         MG9_PRICE,MGT8_XXX,NULL,NULL,NULL,NULL,NULL,
                         NULL,MG8D_F_ID,NULL,R1,R2,SHEET,MG8D_F_ID
                    from ci.MG8D,ci.MG8,ci.MG9,ci.MGT8,
                         (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                            FROM ci.RPT
                           WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID = '40011_2') R
                   where MG8D_YMD = to_char(:as_date,'YYYYMMDD')
                     AND MG8D_F_ID = 'SGX02'
                     AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
                     AND MG8D_F_ID = MG8_F_ID
                     AND MG8D_YMD = MG9_YMD
                     AND MG8D_F_ID = MG9_F_ID   
                     AND MG8D_F_ID = MGT8_F_ID
                     and MG8D_F_ID = R_KIND_ID
                  ORDER BY SHEET,R1,R2,MG1_KIND_ID,MG1_TYPE) main 
                  on main.R1 = rpt.R1
                  and main.MG1_TYPE=rpt.MG1_TYPE
                  order by R1,MG1_TYPE";
         DataTable dtResult = db.GetDataTable(sql, parms);

         dtResult.Columns.Remove(dtResult.Columns["R1"]);
         dtResult.Columns.Remove(dtResult.Columns["MG1_TYPE"]);

         return dtResult;
      }

      /// <summary>
      /// sheet=2 本日結算保證金計算
      /// </summary>
      /// <returns>MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_CP_CM</returns>
      public DataTable GetOptR2Data(DateTime as_date)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql =
                  @"select rpt.R2,rpt.MG1_TYPE,MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_CP_CM from
                             ( select R2,MG1_TYPE from
                                 (SELECT DISTINCT MG1_TYPE ,R2,R_KIND_ID
                                  from ci.MG1,ci.MGT2,
                                       (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                          FROM ci.RPT
                                         WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_2' AND RPT_VALUE <> 'E') R
                                 where MG1_KIND_ID = R_KIND_ID
                                 order by R2)
                             ) rpt
                         left join
                        (SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                               MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                               MG1_CHANGE_FLAG,
                               MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                          from ci.MG1,ci.MGT2,
                               (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                  FROM ci.RPT
                                 WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_2') R
                         where MG1_DATE = :as_date
                           and MG1_KIND_ID = R_KIND_ID
                           and MG1_KIND_ID = MGT2_KIND_ID
                        union all
                        select MG8_CM , MG8_MM ,MG8_IM , 
                                   case when MG9_PRICE is null then 0 else  TRUNC((MG8_CM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                   case when MG9_PRICE is null then 0 else  TRUNC((MG8_MM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                   case when MG9_PRICE is null then 0 else  TRUNC((MG8_IM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                               MG9_PRICE,MGT8_XXX,NULL,NULL,NULL,NULL,NULL,
                               NULL,MG8D_F_ID,NULL,R1,R2,SHEET,MG8D_F_ID
                          from ci.MG8D,ci.MG8,ci.MG9,ci.MGT8,
                               (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                  FROM ci.RPT
                                 WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID = '40011_2') R
                         where MG8D_YMD = to_char(:as_date,'YYYYMMDD')
                           AND MG8D_F_ID = 'SGX02'
                           AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
                           AND MG8D_F_ID = MG8_F_ID
                           AND MG8D_YMD = MG9_YMD
                           AND MG8D_F_ID = MG9_F_ID   
                           AND MG8D_F_ID = MGT8_F_ID
                           and MG8D_F_ID = R_KIND_ID
                        ORDER BY SHEET,R1,R2,MG1_KIND_ID,MG1_TYPE) main 
                        on main.R2 = rpt.R2
                        and main.MG1_TYPE=rpt.MG1_TYPE
                        order by R2,MG1_TYPE";
         DataTable dtResult = db.GetDataTable(sql, parms);

         dtResult.Columns.Remove(dtResult.Columns["R2"]);
         dtResult.Columns.Remove(dtResult.Columns["MG1_TYPE"]);

         return dtResult;
      }

      /// <summary>
      /// 四、	作業事項 已達或未達10%
      /// </summary>
      /// <param name="as_date">datetime</param>
      /// <param name="Num">sheet 1 or 2</param>
      /// <returns></returns>
      public DataTable WorkItem(DateTime as_date,int Num)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql = $@"select N10P,R10P 
                           from
                           (select DISTINCT R2,decode(MG1_CHANGE_FLAG,'N','■'||MGT2_KIND_ID_OUT,'□'||MGT2_KIND_ID_OUT) as N10P,decode(MG1_CHANGE_FLAG,'Y','■'||MGT2_KIND_ID_OUT,'□'||MGT2_KIND_ID_OUT) as R10P from
                           (SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                                  MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                                  MG1_CHANGE_FLAG,
                                  MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                             from ci.MG1,ci.MGT2,
                                  (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                     FROM ci.RPT
                                    WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID = '40011_{Num.ToString().Substring(0, 1)}') R
                            where MG1_DATE = :as_date
                              and MG1_KIND_ID = R_KIND_ID
                              and MG1_KIND_ID = MGT2_KIND_ID
                           union all
                           select MG8_CM , MG8_MM ,MG8_IM , 
                                      case when MG9_PRICE is null then 0 else  TRUNC((MG8_CM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                      case when MG9_PRICE is null then 0 else  TRUNC((MG8_MM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                      case when MG9_PRICE is null then 0 else  TRUNC((MG8_IM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                  MG9_PRICE,MGT8_XXX,NULL,NULL,NULL,NULL,NULL,
                                  NULL,MG8D_F_ID,NULL,R1,R2,SHEET,MG8D_F_ID
                             from ci.MG8D,ci.MG8,ci.MG9,ci.MGT8,
                                  (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                     FROM ci.RPT
                                    WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID = '40011_1') R
                            where MG8D_YMD = to_char(:as_date,'YYYYMMDD')
                              AND MG8D_F_ID = 'SGX02'
                              AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
                              AND MG8D_F_ID = MG8_F_ID
                              AND MG8D_YMD = MG9_YMD
                              AND MG8D_F_ID = MG9_F_ID   
                              AND MG8D_F_ID = MGT8_F_ID
                              and MG8D_F_ID = R_KIND_ID
                           ORDER BY SHEET,R1,R2,MG1_KIND_ID,MG1_TYPE)
                           where MG1_CHANGE_FLAG IS NOT NULL
                           order by R2)";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// sheet 作業事項
      /// </summary>
      /// <param name="Num">1 or 2</param>
      /// <returns></returns>
      public int Get40011RptLV(int Num)
      {
         string sql = $@"select rpt_level_2
                           from ci.rpt
                           where rpt_txn_id = '40011'
                             and rpt_txd_id = '40011_{Num.ToString().Substring(0,1)}'
                             and rpt_value = 'E'";
         DataTable dtResult = db.GetDataTable(sql, null);
         return dtResult.Rows[0][0].AsInt();
      }
      /// <summary>
      /// 判斷FMIF資料已轉入
      /// </summary>
      /// <param name="ld_date">輸入日期</param>
      /// <returns></returns>
      public int CheckFMIF(DateTime ld_date) {
         object[] parms = {
                ":ld_date",ld_date,
            };
         string sql = $@"select count(*) as CNT
                          from ci.AI5,ci.APDK
                         where AI5_DATE = :ld_date
                            and AI5_KIND_ID = APDK_KIND_ID
                            and APDK_MARKET_CLOSE = '1'";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult.Rows[0][0].AsInt();
      }

   }
}
