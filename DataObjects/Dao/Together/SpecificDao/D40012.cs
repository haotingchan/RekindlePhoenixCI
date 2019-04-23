/// <summary>
/// John, 2019/4/15
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40012 : D4001x
   {

      protected override string FutDataCountSql()
      {
         return @"SELECT count(*)
                    from ci.MG1,ci.MGT2,
                         (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                            FROM ci.RPT
                           WHERE RPT_TXN_ID = '40012' AND RPT_TXD_ID = '40012_1') R
                   where MG1_DATE = :as_date
                     and MG1_KIND_ID = R_KIND_ID 
                     and MG1_KIND_ID = MGT2_KIND_ID";
      }

      protected override string OptDataCountSql()
      {
         return @"SELECT count(*)
                    from ci.MG1,ci.MGT2,
                         (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                            FROM ci.RPT
                           WHERE RPT_TXN_ID = '40012' AND RPT_TXD_ID = '40012_2') R
                   where MG1_DATE = :as_date
                     and MG1_OSW_GRP = '5'
                     and MG1_KIND_ID = R_KIND_ID
                     and MG1_KIND_ID = MGT2_KIND_ID";
      }

      protected override string FutDataSql(int sheet)
      {
         return string.Format(@"select rpt.{0},MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                                        MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM
                                                      from
                                                            (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                           FROM ci.RPT
                                          WHERE RPT_TXN_ID = '40012' AND RPT_TXD_ID = '40012_1' AND RPT_VALUE <> 'E'
                                                            ) rpt
                                                      left join
                                 (SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                                        MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                                        MG1_CHANGE_FLAG,
                                        MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                                   from ci.MG1,ci.MGT2,
                                        (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                           FROM ci.RPT
                                          WHERE RPT_TXN_ID = '40012' AND RPT_TXD_ID = '40012_1') R
                                  where MG1_DATE = :as_date
                                    and MG1_KIND_ID = R_KIND_ID 
                                    and MG1_KIND_ID = MGT2_KIND_ID
                                    order by r1,r2,mg1_kind_id,mg1_type
                                    ) main
                                  on main.{0} = rpt.{0}
                                  order by {0}
                                 ", $"R{sheet}");
      }

      protected override string OptDataSql(int sheet)
      {
         return string.Format(@"SELECT rpt.{0},rpt.MG1_TYPE,MG1_CUR_CM,'',MG1_CUR_MM,'',MG1_CUR_IM,
                                        MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_CP_CM
                                 from
                                     ( select {0},MG1_TYPE from
                                         (SELECT DISTINCT MG1_TYPE ,{0},R_KIND_ID
                                         from ci.MG1,ci.MGT2,
                                         (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                                   FROM ci.RPT
                                                  WHERE RPT_TXN_ID = '40012' AND RPT_TXD_ID = '40012_2' AND RPT_VALUE <> 'E') R
                                         where MG1_KIND_ID = R_KIND_ID
                                         order by {0})
                                      ) rpt
                                 left join
                                 (SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,
                                        MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_CP_CM,
                                        MG1_CHANGE_FLAG,
                                        MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                                   from ci.MG1,ci.MGT2,
                                        (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                                           FROM ci.RPT
                                          WHERE RPT_TXN_ID = '40012' AND RPT_TXD_ID = '40012_2') R
                                  where MG1_DATE = :as_date
                                    and MG1_OSW_GRP = '5'
                                    and MG1_KIND_ID = R_KIND_ID
                                    and MG1_KIND_ID = MGT2_KIND_ID
                                 order by sheet,r1,mg1_kind_id,mg1_type) main
                                 on main.{0} = rpt.{0}
                                 and main.MG1_TYPE=rpt.MG1_TYPE
                                 order by rpt.{0},rpt.MG1_TYPE", $"R{sheet}");
      }

      protected override string WorkItemSql(int Num)
      {
         return $@"select N10P,R10P 
                  from
                  (select DISTINCT R2,decode(MG1_CHANGE_FLAG,'N','■'||MGT2_KIND_ID_OUT,'□'||MGT2_KIND_ID_OUT) as N10P,decode(MG1_CHANGE_FLAG,'Y','■'||MGT2_KIND_ID_OUT,'□'||MGT2_KIND_ID_OUT) as R10P 
                  from
                  (SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,
                         MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_CP_CM,
                         MG1_CHANGE_FLAG,
                         MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                    from ci.MG1,ci.MGT2,
                         (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                            FROM ci.RPT
                           WHERE RPT_TXN_ID = '40012' AND RPT_TXD_ID = '40012_{ Num.ToString().Substring(0, 1)}') R
                   where MG1_DATE = :as_date
                     and MG1_OSW_GRP = '5'
                     and MG1_KIND_ID = R_KIND_ID
                     and MG1_KIND_ID = MGT2_KIND_ID
                  order by sheet,r1,mg1_kind_id,mg1_type)
                  where MG1_CHANGE_FLAG IS NOT NULL
                  order by R2
                  )";
      }

   }
}
