using BusinessObjects;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// John, 2019/4/1,d_30290
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30290 : DataGate
   {

      public int PLP13ListCount(string is_ymd)
      {
         object[] parms = {
                ":is_ymd", is_ymd
            };

         string sql = @"select count(*) 
                        from ci.PLP13
                        where PLP13_ISSUE_YMD = :is_ymd";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult.Rows[0][0].AsInt();
      }

      /// <summary>
      /// dw_ymd
      /// </summary>
      /// <param name="as_symd"></param>
      /// <returns>YMD</returns>
      public DataTable ListEffectiveYmd(string as_ymd)
      {
         object[] parms = {
                ":as_ymd", as_ymd
            };

         string sql =
                  @"
                     SELECT DISTINCT YMD FROM 
                     (
                         SELECT PL2_EFFECTIVE_YMD AS YMD
                         FROM ci.PL2
                         WHERE PL2_YMD >= :as_ymd
                         GROUP BY PL2_EFFECTIVE_YMD
                         UNION ALL
                         SELECT PLS2_EFFECTIVE_YMD
                         FROM ci.PLS2
                         WHERE PLS2_YMD >= :as_ymd
                         GROUP BY PLS2_EFFECTIVE_YMD
                         UNION ALL
                         SELECT '' FROM DUAL
                     )
                     ORDER BY YMD
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_30290
      /// </summary>
      /// <param name="as_symd"></param>
      /// <returns></returns>
      public DataTable List30290Data(string as_ymd)
      {

         object[] parms = {
                ":as_ymd", as_ymd
            };

         string sql =
                  @"
                     SELECT CI.PLP13.*,' ' AS OP_TYPE 
                     FROM CI.PLP13   
                     WHERE PLP13_ISSUE_YMD = :as_ymd
                         AND PLP13_PROD_SUBTYPE <> 'S'
                         AND PLP13_FUT = 'F'
                     UNION ALL
                     SELECT CI.PLP13.*,' ' AS OP_TYPE 
                     FROM CI.PLP13   
                     WHERE PLP13_ISSUE_YMD = :as_ymd
                         AND PLP13_PROD_SUBTYPE <> 'S'
                         AND PLP13_OPT = 'O'
                     UNION ALL
                     SELECT CI.PLP13.* ,' ' AS OP_TYPE
                     FROM CI.PLP13   
                     WHERE PLP13_ISSUE_YMD = :as_ymd
                         AND PLP13_PROD_SUBTYPE = 'S'
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_30290_insert
      /// </summary>
      /// <param name="as_cal_ymd"></param>
      /// <param name="as_eff_ymd"></param>
      /// <returns></returns>
      public DataTable ListInsertData(string as_cal_ymd, string as_eff_ymd)
      {
         object[] parms = {
                ":as_cal_ymd", as_cal_ymd,
                ":as_eff_ymd",as_eff_ymd
            };

         string sql =
                  @" select * from
 (--非個股
 select arg_eff_ymd as PLP13_ISSUE_YMD,
        case when APDK_PROD_TYPE = 'F' then APDK_PROD_TYPE else ' '  end as PLP13_FUT,
        case when APDK_PROD_TYPE = 'O' then APDK_PROD_TYPE else ' '  end as PLP13_OPT,
        APDK_PROD_SUBTYPE as PLP13_PROD_SUBTYPE,
        APDK_KIND_ID2 as PLP13_KIND_GRP2,
        APDK_KIND_ID2 as PLP13_KIND_ID2,
        PL2_EFFECTIVE_YMD as PLP13_EFFECTIVE_YMD,
        PL2_NATURE as PLP13_NATURE_TOT,
        PL2_LEGAL as PLP13_LEGAL_TOT,
        PL2_999 as PLP13_999_TOT,
        PL2_999 as PLP13_MMK_TOT,
        null as PLP13_NATURE_MTH,
        null as PLP13_LEGAL_MTH,
        null as PLP13_999_MTH,
        null as PLP13_MMK_MTH,
        null as PLP13_NATURE_LAST,
        null as PLP13_LEGAL_LAST,
        null as PLP13_999_LAST,
        null as PLP13_MMK_LAST,         
        SYSDATE as PLP13_W_TIME,
        ' ' as PLP13_W_USER_ID,
        ' ' as OP_TYPE   
   from ci.PL2 p,ci.APDK,
       (select PL2_KIND_ID as cp_kind_id,
               nvl(max(case when PL2_EFFECTIVE_YMD <= arg_eff_ymd then PL2_EFFECTIVE_YMD else null end),
                   max(case when prev_ymd < arg_eff_ymd then prev_ymd else null end)) as eff_ymd,
               max(arg_eff_ymd) as arg_eff_ymd
          from ci.PL2,
              (select :as_cal_ymd as arg_cp_ymd,:as_eff_ymd as arg_eff_ymd from dual) d,  
              (select PL2_KIND_ID as prev_kind_id,PL2_EFFECTIVE_YMD as prev_ymd 
                 from ci.PL2
                where PL2_YMD <  :as_eff_ymd
                  and PL2_YMD >= to_char(to_number(substr(:as_eff_ymd,1,4)) - 1) ||substr(:as_eff_ymd,5,4)) pr
         where PL2_YMD >= arg_cp_ymd and PL2_YMD <= arg_eff_ymd
           and PL2_KIND_ID = prev_kind_id(+)
         group by PL2_KIND_ID) K
  where PL2_KIND_ID = cp_kind_id
    and PL2_EFFECTIVE_YMD = eff_ymd
    and PL2_KIND_ID = APDK_KIND_ID
union all
 --個股
 select arg_eff_ymd,
        PLS2_FUT,
        PLS2_OPT,
        'S',PLS2_KIND_GRP2,PLS2_KIND_ID2,PLS2_EFFECTIVE_YMD,
        PLS2_NATURE,PLS2_LEGAL,PLS2_LEGAL,PLS2_999,
        null,null,null,null,null,null,null,null,        
        SYSDATE,' ',' '
   from ci.PLS2 p,
       (select PLS2_KIND_ID2 as cp_kind_id2,
               nvl(max(case when PLS2_EFFECTIVE_YMD <= arg_eff_ymd then PLS2_EFFECTIVE_YMD else null end),
                   max(case when prev_ymd < arg_eff_ymd then prev_ymd else null end)) as eff_ymd,
               max(arg_eff_ymd) as arg_eff_ymd
          from ci.PLS2,
              (select :as_cal_ymd as arg_cp_ymd,:as_eff_ymd as arg_eff_ymd from dual),  
              (select PLS2_KIND_ID2 as prev_kind_id2,PLS2_EFFECTIVE_YMD as prev_ymd 
                 from ci.PLS2
                where PLS2_YMD <  :as_eff_ymd
                  and PLS2_YMD >= to_char(to_number(substr(:as_eff_ymd,1,4)) - 1) ||substr(:as_eff_ymd,5,4)) pr
         where PLS2_YMD >= arg_cp_ymd and PLS2_YMD <= arg_eff_ymd
           and PLS2_KIND_ID2 = prev_kind_id2(+)
         group by PLS2_KIND_ID2) K
  where PLS2_KIND_ID2 = cp_kind_id2
    and PLS2_EFFECTIVE_YMD = eff_ymd
union all
 --公債
 select arg_eff_ymd,
        'F',' ','B',PL2B_KIND_ID,PL2B_KIND_ID,PL2B_EFFECTIVE_YMD,
        PL2B_NATURE_LEGAL_TOT,PL2B_NATURE_LEGAL_TOT,PL2B_999_TOT,PL2B_999_TOT,
        PL2B_NATURE_LEGAL_MTH,PL2B_NATURE_LEGAL_MTH,PL2B_999_MTH,PL2B_999_MTH,
        null,null,PL2B_999_NEARBY_MTH,PL2B_999_NEARBY_MTH,        
        SYSDATE,' ',' '
   from ci.PL2B p,
       (select PL2B_KIND_ID as cp_kind_id,
               nvl(max(case when PL2B_EFFECTIVE_YMD <= arg_eff_ymd then PL2B_EFFECTIVE_YMD else null end),
                   max(case when prev_ymd < arg_eff_ymd then prev_ymd else null end)) as eff_ymd,
               max(arg_eff_ymd) as arg_eff_ymd
          from ci.PL2B,
              (select :as_cal_ymd as arg_cp_ymd,:as_eff_ymd as arg_eff_ymd from dual),  
              (select PL2B_KIND_ID as prev_kind_id,PL2B_EFFECTIVE_YMD as prev_ymd 
                 from ci.PL2B
                where PL2B_YMD <  :as_eff_ymd
                  and PL2B_YMD >= to_char(to_number(substr(:as_eff_ymd,1,4)) - 1) ||substr(:as_eff_ymd,5,4)) pr
         where PL2B_YMD >= arg_cp_ymd and PL2B_YMD <= arg_eff_ymd
           and PL2B_KIND_ID = prev_kind_id(+)
         group by PL2B_KIND_ID) K         
  where PL2B_KIND_ID = cp_kind_id
    and PL2B_EFFECTIVE_YMD = eff_ymd)
order by decode(PLP13_PROD_SUBTYPE,'S',3,decode(PLP13_FUT,'F',0,1)), PLP13_KIND_ID2
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      /// <summary>
      /// d_30290_gbf
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public DataTable ListGBF(string as_ymd)
      {
         object[] parms = {
                ":as_ymd", as_ymd
            };
         string sql = @"SELECT PL2B_YMD,   
                        PL2B_EFFECTIVE_YMD,   
                        PL2B_KIND_ID,
                        PL2B_NATURE_LEGAL_MTH,
                        PL2B_NATURE_LEGAL_TOT,
                        PL2B_999_MTH,
                        PL2B_999_NEARBY_MTH,
                        PL2B_999_TOT,
                        PL2B_PREV_NATURE_LEGAL_MTH,
                        PL2B_PREV_NATURE_LEGAL_TOT,
                        PL2B_PREV_999_MTH,
                        PL2B_PREV_999_NEARBY_MTH,
                        PL2B_PREV_999_TOT,
                        PL2B_ADJ,
                        C.RPT_SEQ_NO AS C_SEQ_NO,
                        E.RPT_SEQ_NO AS E_SEQ_NO
                   FROM ci.PL2B,ci.PLP13,
                       (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204c') C,
                       (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204e') E 
               where PLP13_ISSUE_YMD =:as_ymd
                 and PL2B_EFFECTIVE_YMD = PLP13_EFFECTIVE_YMD
                 and PL2B_KIND_ID = PLP13_KIND_ID2
                 and trim(PL2B_KIND_ID) = trim(C.RPT_VALUE)
                 and trim(PL2B_KIND_ID) = trim(E.RPT_VALUE)
               order by pl2b_effective_ymd";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


      /// <summary>
      /// d_30290_stock
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public DataTable ListStock(string as_ymd)
      {
         object[] parms = {
                ":as_ymd", as_ymd
            };
         string sql = @"SELECT PLS2_YMD,   
                        PLS2_EFFECTIVE_YMD,   
                        PLS2_KIND_ID2 as PLS2_KIND_ID2,   
                        PLS2_FUT as  PLS2_FUT,   
                        PLS2_OPT as PLS2_OPT,   
                        PLS2_SID,   
                        PLS2_LEVEL,   
                        PLS2_NATURE,   
                        PLS2_LEGAL,   
                        PLS2_999,   
                        PLS2_PREV_LEVEL,
                        PLS2_PREV_NATURE,
                        PLS2_PREV_LEGAL,
                        PLS2_PREV_999,
                        PLS2_LEVEL_ADJ,   
                        PLS2_W_TIME,   
                        PLS2_W_USER_ID,  
                        NVL(APDK_NAME,TFXMS_SNAME) AS APDK_NAME,
                        PLS2_KIND_GRP2 as APDK_KIND_GRP2,
                       PLP13_ISSUE_YMD
                  from   CI.PLS2,
                        --現貨名稱
                        ci.TFXMS,ci.PLP13,
                       (select APDK_KIND_ID2,nvl(max(case when APDK_PROD_TYPE = 'F' then APDK_NAME else '' end),max(case when APDK_PROD_TYPE = 'O' then APDK_NAME else '' end)) AS APDK_NAME 
                          from ci.APDK GROUP BY APDK_KIND_ID2) P 
                  WHERE PLP13_ISSUE_YMD = :as_ymd
                     and PLS2_EFFECTIVE_YMD = PLP13_EFFECTIVE_YMD
                     and PLS2_KIND_ID2 = PLP13_KIND_ID2
                     and PLS2_KIND_ID2 = APDK_KIND_ID2
                     and PLS2_SID = TFXMS_SID(+)
               order by apdk_kind_grp2,pls2_kind_id2,pls2_effective_ymd";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_30290_n_stock
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public DataTable ListNStock(string as_ymd)
      {
         object[] parms = {
                ":as_ymd", as_ymd
            };
         string sql = @"SELECT PL2_YMD,   
                        PL2_EFFECTIVE_YMD,   
                        PL2_KIND_ID,   
                        PL2_NATURE,   
                        PL2_LEGAL,   
                        PL2_999,   
                        PL2_PREV_NATURE,PL2_PREV_LEGAL,PL2_PREV_999,
                        PL2_W_TIME,   
                        PL2_W_USER_ID,
                        C.RPT_SEQ_NO AS C_SEQ_NO,
                        E.RPT_SEQ_NO AS E_SEQ_NO,
                        PL2_EFFECTIVE_YMD,
                        PL2_NATURE_ADJ,
                       PLP13_ISSUE_YMD
                   FROM ci.PL2,ci.PLP13,
                       (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204c') C,
                       (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204e') E 
               where PLP13_ISSUE_YMD =:as_ymd
                 and PL2_EFFECTIVE_YMD = PLP13_EFFECTIVE_YMD
                 and PL2_KIND_ID = PLP13_KIND_ID2
                 and trim(PL2_KIND_ID) = trim(C.RPT_VALUE)
                 and trim(PL2_KIND_ID) = trim(E.RPT_VALUE)";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_30290_txt_f
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public DataTable List30290TxtF(string as_ymd)
      {
         object[] parms = {
                ":as_ymd", as_ymd
            };
         string sql = @"select * from
                        (select case when PLP13_PROD_SUBTYPE = 'S' then trim(PLP13_KIND_ID2) || 'F' else  trim(PLP13_KIND_ID2) end as PLP13_KIND_ID2,
                                '0' as compute_0002,NVL(PLP13_NATURE_TOT,99999999) as TOT,NVL(PLP13_NATURE_MTH,99999999) as MTH,NVL(PLP13_NATURE_LAST,99999999) as PLAST
                           from ci.PLP13
                          WHERE PLP13_ISSUE_YMD = :as_ymd
                          and PLP13_FUT = 'F'
                          union all
                           select case when PLP13_PROD_SUBTYPE = 'S' then trim(PLP13_KIND_ID2) || 'F' else  trim(PLP13_KIND_ID2) end as PLP13_KIND_ID2,
                                '1' as compute_0002,NVL(PLP13_LEGAL_TOT,99999999),NVL(PLP13_LEGAL_MTH,99999999),NVL(PLP13_LEGAL_LAST,99999999)
                           from ci.PLP13
                          WHERE PLP13_ISSUE_YMD = :as_ymd
                          and PLP13_FUT = 'F'
                          union all
                           select case when PLP13_PROD_SUBTYPE = 'S' then trim(PLP13_KIND_ID2) || 'F' else  trim(PLP13_KIND_ID2) end as PLP13_KIND_ID2,
                                '2' as compute_0002,NVL(PLP13_999_TOT,99999999),NVL(PLP13_999_MTH,99999999),NVL(PLP13_999_LAST,99999999)
                           from ci.PLP13
                          WHERE PLP13_ISSUE_YMD = :as_ymd
                          and PLP13_FUT = 'F'
                          union all
                           select case when PLP13_PROD_SUBTYPE = 'S' then trim(PLP13_KIND_ID2) || 'F' else  trim(PLP13_KIND_ID2) end as PLP13_KIND_ID2,
                                '3' as compute_0002,NVL(PLP13_MMK_TOT,99999999),NVL(PLP13_MMK_MTH,99999999),NVL(PLP13_MMK_LAST,99999999)
                           from ci.PLP13
                          WHERE PLP13_ISSUE_YMD = :as_ymd
                          and PLP13_FUT = 'F')
                         order by compute_0002,plp13_kind_id2";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_30290_txt_o
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public DataTable List30290TxtO(string as_ymd)
      {
         object[] parms = {
                ":as_ymd", as_ymd
            };
         string sql = @" select * from
                         (select case when PLP13_PROD_SUBTYPE = 'S' then trim(PLP13_KIND_ID2) || 'O' else  trim(PLP13_KIND_ID2) end as PLP13_KIND_ID2,
                                '0' as compute_0002,NVL(PLP13_NATURE_TOT,99999999)as TOT,NVL(PLP13_NATURE_MTH,99999999) as MTH,NVL(PLP13_NATURE_LAST,99999999) as PLAST
                           from ci.PLP13
                          WHERE PLP13_ISSUE_YMD = :as_ymd
                          and PLP13_OPT = 'O'
                          union all
                           select case when PLP13_PROD_SUBTYPE = 'S' then trim(PLP13_KIND_ID2) || 'O' else  trim(PLP13_KIND_ID2) end as PLP13_KIND_ID2,
                                '1' as compute_0002,NVL(PLP13_LEGAL_TOT,99999999),NVL(PLP13_LEGAL_MTH,99999999),NVL(PLP13_LEGAL_LAST,99999999)
                           from ci.PLP13
                          WHERE PLP13_ISSUE_YMD = :as_ymd
                          and PLP13_OPT = 'O'
                          union all
                           select case when PLP13_PROD_SUBTYPE = 'S' then trim(PLP13_KIND_ID2) || 'O' else  trim(PLP13_KIND_ID2) end as PLP13_KIND_ID2,
                                '2' as compute_0002,NVL(PLP13_999_TOT,99999999),NVL(PLP13_999_MTH,99999999),NVL(PLP13_999_LAST,99999999)
                           from ci.PLP13
                          WHERE PLP13_ISSUE_YMD = :as_ymd
                          and PLP13_OPT = 'O'
                          union all
                           select case when PLP13_PROD_SUBTYPE = 'S' then trim(PLP13_KIND_ID2) || 'O' else  trim(PLP13_KIND_ID2) end as PLP13_KIND_ID2,
                                '3' as compute_0002,NVL(PLP13_MMK_TOT,99999999),NVL(PLP13_MMK_MTH,99999999),NVL(PLP13_MMK_LAST,99999999)
                           from ci.PLP13
                          WHERE PLP13_ISSUE_YMD = :as_ymd
                          and PLP13_OPT = 'O')
                        order by compute_0002,plp13_kind_id2";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      public ResultData UpdatePLP13(DataTable inputData)
      {
         string sql = @"
                     SELECT  PLP13_ISSUE_YMD,
                              PLP13_FUT,
                              PLP13_OPT,
                              PLP13_PROD_SUBTYPE,
                              PLP13_KIND_GRP2,
                              PLP13_KIND_ID2,
                              PLP13_EFFECTIVE_YMD,
                              PLP13_NATURE_TOT,
                              PLP13_LEGAL_TOT,
                              PLP13_999_TOT,
                              PLP13_MMK_TOT,
                              PLP13_NATURE_MTH,
                              PLP13_LEGAL_MTH,
                              PLP13_999_MTH,
                              PLP13_MMK_MTH,
                              PLP13_NATURE_LAST,
                              PLP13_LEGAL_LAST,
                              PLP13_999_LAST,
                              PLP13_MMK_LAST,
                              PLP13_W_TIME,
                              PLP13_W_USER_ID
                     FROM CI.PLP13";

         return db.UpdateOracleDB(inputData, sql);
      }

      public bool DeletePLP13(string is_ymd)
      {
         object[] parms = {
               ":is_ymd",is_ymd
            };
         string sql = @"delete ci.PLP13 where PLP13_ISSUE_YMD = :is_ymd";
         int executeResult = db.ExecuteSQL(sql, parms);

         if (executeResult > 0) {
            return true;
         }
         else {
            throw new Exception("PLP13刪除失敗");
         }
      }

   }
}
