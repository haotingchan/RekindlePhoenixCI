using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/28
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30221 : DataGate {

        /// <summary>
        /// 個股部位限制 報表輸出用
        /// </summary>
        /// <param name="as_cp_ymd">yyyyMMdd</param>
        /// <param name="as_q_sym">yyyyMM</param>
        /// <param name="as_q_eym">yyyyMM</param>
        /// <param name="as_stkout_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30221(string as_cp_ymd, string as_q_sym, string as_q_eym, string as_stkout_ymd) {
            object[] parms = {
                ":as_cp_ymd", as_cp_ymd,
                ":as_q_sym", as_q_sym,
                ":as_q_eym", as_q_eym,
                ":as_stkout_ymd", as_stkout_ymd
            };

            string sql =
@"
select ROW_NUMBER() over (order by pls4_kind_id2) as NO,
       PLS4_KIND_ID2,
       case when pls4_opt = 'O' and pls4_fut = 'F' then trim(APDK_NAME)||'及選擇權' else trim(APDK_NAME) end as APDK_NAME,
       PLS4_SID as APDK_STOCK_ID,
       nvl(PLS3_TOT_QNTY,0) as PLS3_TOT_QNTY,
       STKOUT_B,
       NVL(PLS2_LEVEL,' ') AS PLS2_LEVEL,
       CP_LEVEL,    
       PLST1_NATURE,
       PLST1_LEGAL,
       PLST1_999,
       case when PLS4_STATUS_CODE = 'I' then '新增'
            when nvl(PLS2_LEVEL,' ') = ' ' then '不變'
            when nvl(PLS2_LEVEL,' ') < CP_LEVEL then '降低'
            when nvl(PLS2_LEVEL,' ') > CP_LEVEL then '提高'
            else '不變' end as LEVEL_ADJ,
       M_KIND_ID2
  from ci.PLST1,
      (select PLS4_YMD,PLS4_KIND_ID2,PLS4_STATUS_CODE,APDK_NAME,M_KIND_ID2,PLS4_FUT,PLS4_OPT,PLS4_SID,PLS3_TOT_QNTY,STKOUT_B,
             LEAST(C1.PLST1_LEVEL,GREATEST(C2.PLST1_LEVEL,C3.PLST1_LEVEL)) AS CP_LEVEL
        from ci.PLST1 C1, ci.PLST1 C2, ci.PLST1 C3,
             --總交易量
            (select PLS4_YMD,APDK_NAME,M_KIND_ID2,PLS4_KIND_ID2,PLS4_STATUS_CODE,PLS4_FUT,PLS4_OPT,PLS4_SID as PLS4_SID,PLS3_TOT_QNTY,nvl(STKOUT_B,0) as STKOUT_B
               from ci.PLS4,
                   (select PLS3_SID,sum(PLS3_QNTY)/100000000 as PLS3_TOT_QNTY from ci.PLS3
                    where PLS3_YM >= :as_q_sym
                      and PLS3_YM <= :as_q_eym
                    group by PLS3_SID) Q,
                    --在外流流通股
                   (select STKOUT_ID,MAX(STKOUT_B)/100000000 AS STKOUT_B
                     from CI.STKOUT
                    where STKOUT_DATE = :as_stkout_ymd
                    group by STKOUT_ID) S,
                   (select A.APDK_KIND_ID,A.APDK_NAME,
                           case when APDK_PROD_TYPE = 'F' then M.APDK_KIND_ID2 else ' ' end as M_KIND_ID2
                      from ci.APDK A,
                          (select APDK_STOCK_ID,APDK_KIND_ID2,APDK_KIND_GRP2
                            from ci.PLS4,ci.APDK
                           where PLS4_YMD = :as_cp_ymd 
                             and PLS4_STATUS_CODE = 'M'
                             and PLS4_SID = APDK_STOCK_ID
                             and APDK_REMARK = 'M'
                           group by APDK_STOCK_ID,APDK_KIND_ID2,APDK_KIND_GRP2) M
                     where substr(A.APDK_KIND_ID,3,1) in ('F','O')
                       and A.APDK_KIND_ID2 = M.APDK_KIND_GRP2(+)) M
              where PLS4_YMD = :as_cp_ymd
                and PLS4_STATUS_CODE in ('N','I')  --正常,新增
                and trim(PLS4_KIND_ID2)||case when PLS4_FUT = 'F' then 'F' else PLS4_OPT end = trim(APDK_KIND_ID(+))
                and PLS4_SID = PLS3_SID(+)
                and PLS4_SID = STKOUT_ID(+)) Q
       where --條件1
             (NVL(PLS3_TOT_QNTY,0) > C1.PLST1_C1_QNTY_MIN and NVL(PLS3_TOT_QNTY,0) <= C1.PLST1_C1_QNTY_MAX) 
             --條件2
         and (NVL(PLS3_TOT_QNTY,0) > C2.PLST1_C2_QNTY_MIN(+) and NVL(PLS3_TOT_QNTY,0) <= C2.PLST1_C2_QNTY_MAX(+))
         and (NVL(STKOUT_B,0) > C3.PLST1_STKOUT_MIN(+) and  NVL(STKOUT_B,0) <= C3.PLST1_STKOUT_MAX(+))),
       --上季部位限制
      (select PLS2_SID,PLS2_KIND_ID2,PLS2_LEVEL,PLS2_NATURE,PLS2_LEGAL,PLS2_999 
         from CI.PLS2,
             (select PLS2_KIND_ID2 AS MAX_KIND_ID,max(PLS2_YMD) as MAX_YMD
                from ci.PLS2
               where PLS2_YMD < :as_cp_ymd
               group by PLS2_KIND_ID2)
        where PLS2_KIND_ID2 = MAX_KIND_ID
          and PLS2_YMD = MAX_YMD) P
 where PLS4_SID = PLS2_SID(+)
   and PLS4_KIND_ID2 = PLS2_KIND_ID2(+)
   and CP_LEVEL = PLST1_LEVEL
 order by pls4_kind_id2
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 個股部位限制 維護用
        /// </summary>
        /// <param name="as_cp_ymd">yyyyMMdd</param>
        /// <param name="as_q_sym">yyyyMM</param>
        /// <param name="as_q_eym">yyyyMM</param>
        /// <param name="as_stkout_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30221_all(string as_cp_ymd, string as_q_sym, string as_q_eym, string as_stkout_ymd) {
            object[] parms = {
                ":as_cp_ymd", as_cp_ymd,
                ":as_q_sym", as_q_sym,
                ":as_q_eym", as_q_eym,
                ":as_stkout_ymd", as_stkout_ymd
            };

            string sql =
@"
select PLS4_YMD,PLS4_KIND_ID2,PLS4_FUT,PLS4_OPT,PLS4_SID as APDK_STOCK_ID,nvl(PLS3_TOT_QNTY,0) as PLS3_TOT_QNTY,STKOUT_B,NVL(PLS2_LEVEL,' ') AS PLS2_LEVEL,
       PLS2_NATURE,PLS2_LEGAL,PLS2_999,       
       CP_LEVEL,PLST1_NATURE,PLST1_LEGAL,PLST1_999,
       case when PLS4_STATUS_CODE = 'I' then '*'
            when nvl(PLS2_LEVEL,' ') = ' ' then ' '
            when nvl(PLS2_LEVEL,' ') < CP_LEVEL then '-'
            when nvl(PLS2_LEVEL,' ') > CP_LEVEL then '+'
            else ' ' end as LEVEL_ADJ,
       APDK_NAME,PLS4_STATUS_CODE,M_KIND_ID2
  from ci.PLST1,
      (select PLS4_YMD,PLS4_KIND_ID2,PLS4_STATUS_CODE,APDK_NAME,M_KIND_ID2,PLS4_FUT,PLS4_OPT,PLS4_SID,PLS3_TOT_QNTY,STKOUT_B,
             LEAST(C1.PLST1_LEVEL,GREATEST(C2.PLST1_LEVEL,C3.PLST1_LEVEL)) AS CP_LEVEL
        from ci.PLST1 C1, ci.PLST1 C2, ci.PLST1 C3,
             --總交易量
            (select PLS4_YMD,APDK_NAME,M_KIND_ID2,PLS4_KIND_ID2,PLS4_STATUS_CODE,PLS4_FUT,PLS4_OPT,PLS4_SID as PLS4_SID,PLS3_TOT_QNTY,nvl(STKOUT_B,0) as STKOUT_B
               from ci.PLS4,
                   (select PLS3_SID,sum(PLS3_QNTY)/100000000 as PLS3_TOT_QNTY from ci.PLS3
                    where PLS3_YM >= :as_q_sym
                      and PLS3_YM <= :as_q_eym
                    group by PLS3_SID) Q,
                    --在外流流通股
                   (select STKOUT_ID,MAX(STKOUT_B)/100000000 AS STKOUT_B
                     from CI.STKOUT
                    where STKOUT_DATE = :as_stkout_ymd
                    group by STKOUT_ID) S,
                   (select A.APDK_KIND_ID,A.APDK_NAME,
                           case when APDK_PROD_TYPE = 'F' then M.APDK_KIND_ID2 else ' ' end as M_KIND_ID2
                      from ci.APDK A,
                          (select APDK_STOCK_ID,APDK_KIND_ID2,APDK_KIND_GRP2
                            from ci.PLS4,ci.APDK
                           where PLS4_YMD = :as_cp_ymd 
                             and PLS4_STATUS_CODE = 'M'
                             and PLS4_SID = APDK_STOCK_ID
                             and APDK_REMARK = 'M'
                           group by APDK_STOCK_ID,APDK_KIND_ID2,APDK_KIND_GRP2) M
                     where substr(A.APDK_KIND_ID,3,1) in ('F','O')
                       and A.APDK_KIND_ID2 = M.APDK_KIND_GRP2(+)) M
              where PLS4_YMD = :as_cp_ymd
                and PLS4_STATUS_CODE in ('N','I')  --正常,新增
                and trim(PLS4_KIND_ID2)||case when PLS4_FUT = 'F' then 'F' else PLS4_OPT end = trim(APDK_KIND_ID(+))
                and PLS4_SID = PLS3_SID(+)
                and PLS4_SID = STKOUT_ID(+)) Q
       where --條件1
             (NVL(PLS3_TOT_QNTY,0) > C1.PLST1_C1_QNTY_MIN and NVL(PLS3_TOT_QNTY,0) <= C1.PLST1_C1_QNTY_MAX) 
             --條件2
         and (NVL(PLS3_TOT_QNTY,0) > C2.PLST1_C2_QNTY_MIN(+) and NVL(PLS3_TOT_QNTY,0) <= C2.PLST1_C2_QNTY_MAX(+))
         and (NVL(STKOUT_B,0) > C3.PLST1_STKOUT_MIN(+) and  NVL(STKOUT_B,0) <= C3.PLST1_STKOUT_MAX(+))),
       --上季部位限制
      (select PLS2_SID,PLS2_KIND_ID2,PLS2_LEVEL,PLS2_NATURE,PLS2_LEGAL,PLS2_999 
         from CI.PLS2,
             (select PLS2_KIND_ID2 AS MAX_KIND_ID,max(PLS2_YMD) as MAX_YMD
                from ci.PLS2
               where PLS2_YMD < :as_cp_ymd
               group by PLS2_KIND_ID2)
        where PLS2_KIND_ID2 = MAX_KIND_ID
          and PLS2_YMD = MAX_YMD) P
 where PLS4_SID = PLS2_SID(+)
   and PLS4_KIND_ID2 = PLS2_KIND_ID2(+)
   and CP_LEVEL = PLST1_LEVEL
 order by pls4_kind_id2
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 既有計算資料
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30221_pls1(string as_ymd) {
            object[] parms = {
                ":as_ymd", as_ymd
            };

            string sql =
@"
SELECT
    PLS1_YMD, 
    PLS1_KIND_ID2, 
    PLS1_FUT, 
    PLS1_OPT, 
    PLS1_SID, 
    PLS1_QNTY, 
    PLS1_STKOUT, 
    PLS1_CUR_LEVEL, 
    PLS1_CUR_NATURE, 
    PLS1_CUR_LEGAL, 
    PLS1_CUR_999, 
    PLS1_CP_LEVEL, 
    PLS1_CP_NATURE, 
    PLS1_CP_LEGAL, 
    PLS1_CP_999, 
    PLS1_LEVEL_ADJ, 
    PLS1_W_TIME, 
    PLS1_W_USER_ID
FROM CI.PLS1
WHERE PLS1_YMD = :as_ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 刪除PL0的資料
        /// </summary>
        /// <param name="ls_ymd"></param>
        /// <returns></returns>
        public bool DeletePL0ByDate(string ls_ymd) {
            object[] parms =
            {
                ":ls_ymd", ls_ymd
            };

            #region sql

            string sql =
@"
delete ci.PL0 
where PL0_TYPE = 'S' and PL0_YMD = :ls_ymd
";

            #endregion sql
            try {
                int executeResult = db.ExecuteSQLForTransaction(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
                    //throw new Exception("PLS2刪除失敗");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除PLS2的資料
        /// </summary>
        /// <param name="ls_ymd"></param>
        /// <returns></returns>
        public bool DeletePLS2ByDate(string ls_ymd) {
            object[] parms =
            {
                ":ls_ymd", ls_ymd
            };

            #region sql

            string sql =
@"
delete ci.PLS2 
where PLS2_YMD = :ls_ymd
";

            #endregion sql
            try {
                int executeResult = db.ExecuteSQLForTransaction(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
                    //throw new Exception("PLS2刪除失敗");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除PLS1的資料
        /// </summary>
        /// <param name="ls_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public bool DeletePLS1ByDate(string as_ymd) {

            object[] parms =
{
                ":as_ymd", as_ymd
            };

            #region sql

            string sql =
@"
delete ci.PLS1
WHERE PLS1_YMD = :as_ymd
";

            #endregion sql
            try {
                int executeResult = db.ExecuteSQLForTransaction(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
                    //throw new Exception("PLS2刪除失敗");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 新增PL0的資料
        /// </summary>
        /// <param name="ls_ymd"></param>
        /// <returns></returns>
        public bool InsertPL0(string ls_ymd, string ls_sym, string ls_eym, string ls_stk_ymd, string gs_user_id) {
            object[] parms =
            {
                ":ls_ymd", ls_ymd,
                ":ls_sym", ls_sym,
                ":ls_eym", ls_eym,
                ":ls_stk_ymd", ls_stk_ymd,
                ":gs_user_id", gs_user_id
            };

            #region sql

            string sql =
@"
insert into ci.PL0 
values('S', 
       :ls_ymd, 
       null, 
       null, 
       :ls_sym, 
       :ls_eym, 
       :ls_stk_ymd, 
       sysdate, 
       :gs_user_id)
";

            #endregion sql
            try {
                int executeResult = db.ExecuteSQLForTransaction(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
                    //throw new Exception("PLS2刪除失敗");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// update PLS1
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public ResultData updatePLS1(DataTable inputData) {
            string sql = @"
SELECT 
    PLS1_YMD,       
    PLS1_KIND_ID2,  
    PLS1_FUT,       
    PLS1_OPT,       
    PLS1_SID,

    PLS1_QNTY,      
    PLS1_STKOUT,    
    PLS1_CUR_LEVEL, 
    PLS1_CUR_NATURE,
    PLS1_CUR_LEGAL, 

    PLS1_CUR_999,   
    PLS1_CP_LEVEL,  
    PLS1_CP_NATURE, 
    PLS1_CP_LEGAL,  
    PLS1_CP_999,   

    PLS1_LEVEL_ADJ, 
    PLS1_W_TIME,    
    PLS1_W_USER_ID 
FROM CI.PLS1
";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
