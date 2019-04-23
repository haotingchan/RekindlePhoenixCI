/// <summary>
/// John, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40011 : D4001x
   {
      protected override string FutDataCountSql()
      {
         return @"SELECT count(*)
                  from(
                    SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                         MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                         MG1_CHANGE_FLAG,
                         MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                    from ci.MG1,ci.MGT2,
                         (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                            FROM ci.RPT
                           WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID IN ('40011_1','40011_2')) R
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
                  )
                  where sheet=1";
      }

      protected override string OptDataCountSql()
      {
         return @"SELECT count(*)
                  from(
                    SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                         MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                         MG1_CHANGE_FLAG,
                         MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                    from ci.MG1,ci.MGT2,
                         (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                            FROM ci.RPT
                           WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID IN ('40011_1','40011_2')) R
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
                  )
                  where sheet=2";
      }

      protected override string FutDataSql(int sheet)
      {
         return string.Format(@"select rpt.{0},MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                     MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM
                     from
                           (select RPT_LEVEL_2 as R1,RPT_LEVEL_3 as R2 from ci.RPT
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
                           ORDER BY SHEET,R1,R2,MG1_KIND_ID,MG1_TYPE) 
                    main on main.{0} = rpt.{0}
                     order by {0}", $"R{sheet}");
      }

      protected override string OptDataSql(int sheet)
      {
         return string.Format(@"select rpt.{0},rpt.MG1_TYPE,MG1_CUR_CM,'',MG1_CUR_MM,'',MG1_CUR_IM,
                        MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_CP_CM 
                        from

                             ( select {0},MG1_TYPE from
                                 (SELECT DISTINCT MG1_TYPE ,{0},R_KIND_ID
                                  from ci.MG1,ci.MGT2,
                                       (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                          FROM ci.RPT
                                         WHERE RPT_TXN_ID = '40011' AND RPT_TXD_ID ='40011_2' AND RPT_VALUE <> 'E') R
                                 where MG1_KIND_ID = R_KIND_ID
                                 order by {0})
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
                        on main.{0} = rpt.{0}
                        and main.MG1_TYPE=rpt.MG1_TYPE
                        order by rpt.{0},rpt.MG1_TYPE", $"R{sheet}");
      }

      protected override string WorkItemSql(int Num)
      {
         return $@"select N10P,R10P 
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
      }

   }
}
