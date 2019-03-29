using BusinessObjects;
using Common;
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
    public class D30202 : DataGate {

        /// <summary>
        /// 股價指數暨黃金類商品部位限制數
        /// </summary>
        /// <param name="as_data_ymd"></param>
        /// <param name="as_prev_sym"></param>
        /// <param name="as_prev_eym"></param>
        /// <param name="as_cur_sym"></param>
        /// <param name="as_cur_eym"></param>
        /// <param name="ai_nature"></param>
        /// <param name="ai_legal"></param>
        /// <returns></returns>
        public DataTable d_30202(string as_data_ymd,
                                 string as_prev_sym,
                                 string as_prev_eym,
                                 string as_cur_sym,
                                 string as_cur_eym,
                                 decimal ai_nature,
                                 decimal ai_legal) {
            object[] parms = {
                ":as_data_ymd", as_data_ymd,
                ":as_prev_sym", as_prev_sym,
                ":as_prev_eym", as_prev_eym,
                ":as_cur_sym", as_cur_sym,
                ":as_cur_eym", as_cur_eym,
                ":ai_nature", ai_nature,
                ":ai_legal", ai_legal
            };

            string sql =
@"
select :as_data_ymd||'' as ymd,
       V.AI2_PROD_TYPE,V.AI2_PROD_SUBTYPE,V.AI2_KIND_ID,
       nvl(P_AVG_QNTY,0) as P_AVG_QNTY,nvl(P_AVG_OI,0) as P_AVG_OI,
       nvl(C_AVG_QNTY,0) as C_AVG_QNTY,nvl(C_AVG_OI,0) as C_AVG_OI,
       0.0 as CHANGE_RANGE,
       PL2_NATURE,PL2_LEGAL,PL2_999,
       0 as cp_nature, 0 as cp_legal,0 AS cp_999,
       MAX_MONTH_SEQ_NO,MAX_TYPE,MAX_VALUE,
       0 as nature, 0 as legal,0 as p999,
       ' ' as nature_adj, ' ' as legal_adj,' ' as p999_adj,
       greatest(nvl(C_AVG_QNTY,0),nvl(C_AVG_OI,0)) as C_MAX_VALUE,       
       RPT_SEQ_NO,
       T1.PLT1_MULTIPLE as plt1_t1_multiple,T1.PLT1_MIN_NATURE as plt1_t1_min_nature,T1.PLT1_MIN_LEGAL as plt1_t1_min_legal,
       T2.PLT1_MULTIPLE as plt1_t2_multiple,T2.PLT1_MIN_NATURE as plt1_t2_min_nature,T1.PLT1_MIN_LEGAL as plt1_t2_min_legal,
       R1.PLT1_MULTIPLE as plt1_r1_multiple,R1.PLT1_MIN_NATURE plt1_r1_min_nature,R1.PLT1_MIN_LEGAL plt1_r1_min_legal,
       R2.PLT1_MULTIPLE as plt1_r2_multiple,R2.PLT1_MIN_NATURE plt1_r2_min_nature,R2.PLT1_MIN_LEGAL plt1_r2_min_legal,
       MIN_MONTH_SEQ_NO,MIN_TYPE,MIN_VALUE
  from
      (select AI2_PROD_TYPE,AI2_PROD_SUBTYPE,C.AI2_KIND_ID,
              case when NVL(P.DAY_CNT,0) = 0 then 0 else round(P.M_QNTY/P.DAY_CNT,0) end as P_AVG_QNTY,
              case when NVL(P.DAY_CNT,0) = 0 then 0 else round(P.OI/P.DAY_CNT,0) end as P_AVG_OI,
              case when C.DAY_CNT= 0 then 0 else round(C.M_QNTY/C.DAY_CNT,0) end as C_AVG_QNTY,
              case when C.DAY_CNT= 0 then 0 else round(C.OI/C.DAY_CNT,0) end as C_AVG_OI
         from
             --本次
             (select AI2_PROD_TYPE,AI2_PROD_SUBTYPE,DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY) as AI2_KIND_ID,
                    sum(case when AI2_PARAM_KEY='MXF' then round(AI2_M_QNTY/4,0) else AI2_M_QNTY end) as M_QNTY,
                    sum(case when AI2_PARAM_KEY='MXF' then round(AI2_OI/4,0) else AI2_OI end) as OI,
                    sum(case when AI2_PARAM_KEY='MXF' then 0 else AI2_DAY_COUNT end) as DAY_CNT  
               from ci.AI2
              where AI2_SUM_TYPE = 'D'
                and AI2_SUM_SUBTYPE = '3'
                and AI2_PROD_SUBTYPE in ('I','C','E')
                and AI2_PROD_TYPE in ('F','O')
                and AI2_YMD >= :as_cur_sym||'01'
                and AI2_YMD <= :as_cur_eym||'31'
              group by AI2_PROD_TYPE,AI2_PROD_SUBTYPE,DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY)) C,
              --前次
             (select DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY) as AI2_KIND_ID,
                    sum(case when AI2_PARAM_KEY='MXF' then round(AI2_M_QNTY/4,0) else AI2_M_QNTY end) as M_QNTY,
                    sum(case when AI2_PARAM_KEY='MXF' then round(AI2_OI/4,0) else AI2_OI end) as OI,
                    sum(case when AI2_PARAM_KEY='MXF' then 0 else AI2_DAY_COUNT end) as DAY_CNT  
               from ci.AI2
              where AI2_SUM_TYPE = 'D'
                and AI2_SUM_SUBTYPE = '3'
                and AI2_PROD_SUBTYPE in ('I','C','E')
                and AI2_PROD_TYPE in ('F','O')
                and AI2_YMD >= :as_prev_sym||'01'
                and AI2_YMD <= :as_prev_eym||'31'
              group by DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY)) P
        where C.AI2_KIND_ID = P.AI2_KIND_ID(+)) V,
       --現行
       (select PL2_KIND_ID,
             PL2_NATURE,PL2_LEGAL,PL2_999
        from ci.PL2,
            (select PL2_KIND_ID as MAX_KIND_ID,Max(PL2_YMD) as MAX_YMD
              from ci.PL2
             where PL2_YMD  < :as_data_ymd
             group by PL2_KIND_ID)
       where PL2_YMD = MAX_YMD
           and PL2_KIND_ID = MAX_KIND_ID) PL,
       --最大
      (select AI2_KIND_ID,
              max(case when MAX_SEQ_NO = 1 then case when AVG_M_QNTY > AVG_OI then 'M' else 'OI' end else ' ' end) AS MAX_TYPE,
              max(case when MAX_SEQ_NO = 1 then greatest(AVG_M_QNTY,AVG_OI) else 0 end) AS MAX_VALUE,
              max(case when MAX_SEQ_NO = 1 then MONTH_SEQ_NO else 0 end) as MAX_MONTH_SEQ_NO,
              max(case when MIN_SEQ_NO = 1 then case when AVG_M_QNTY > AVG_OI then 'M' else 'OI' end else ' ' end) AS MIN_TYPE,
              max(case when MIN_SEQ_NO = 1 then greatest(AVG_M_QNTY,AVG_OI) else 0 end) AS MIN_VALUE,
              max(case when MIN_SEQ_NO = 1 then MONTH_SEQ_NO else 0 end) as MIN_MONTH_SEQ_NO
         from
             (select AI2_KIND_ID,round(M_QNTY/DAY_CNT,0) as AVG_M_QNTY,round(OI/DAY_CNT,0) as AVG_OI,
                     ROW_NUMBER( ) OVER (PARTITION BY AI2_KIND_ID ORDER BY greatest(round(M_QNTY/DAY_CNT,0),round(OI/DAY_CNT,0)) desc NULLS LAST) as MAX_SEQ_NO,
                     ROW_NUMBER( ) OVER (PARTITION BY AI2_KIND_ID ORDER BY greatest(round(M_QNTY/DAY_CNT,0),round(OI/DAY_CNT,0)) NULLS LAST) as MIN_SEQ_NO,
                     DT_SYMD,DT_EYMD,month_seq_no
               from
                    (select DT_SYMD,DT_EYMD,
                           ROW_NUMBER( ) OVER (PARTITION BY DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY) ORDER BY DT_SYMD desc NULLS LAST) as month_seq_no,
                           DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY) as AI2_KIND_ID,
                           sum(case when AI2_PARAM_KEY='MXF' then round(AI2_M_QNTY/4,0) else AI2_M_QNTY end) as M_QNTY,
                           sum(case when AI2_PARAM_KEY='MXF' then round(AI2_OI/4,0) else AI2_OI end) as OI,
                           sum(case when AI2_PARAM_KEY='MXF' then 0 else AI2_DAY_COUNT end) as DAY_CNT  
                      from ci.AI2,
                          (select AI2_YMD as DT_SYMD,
                                  :as_cur_eym as DT_EYMD
                             from CI.AI2
                            where AI2_YMD >= :as_prev_sym
                              and AI2_YMD <= :as_cur_eym
                              and AI2_SUM_TYPE = 'M'
                              and AI2_SUM_SUBTYPE = '1'
                              and AI2_PROD_TYPE ='F'
                           group by AI2_YMD)     
              where AI2_SUM_TYPE = 'D'
                and AI2_SUM_SUBTYPE = '3'
                and AI2_PROD_SUBTYPE in ('I','C','E')
                and AI2_PROD_TYPE in ('F','O')
                and AI2_YMD >= trim(DT_SYMD)||'01'
                and AI2_YMD <= trim(DT_EYMD)||'31'
              group by DT_SYMD,DT_EYMD,DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY)))
        group by AI2_KIND_ID) M,
       (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30202' and RPT_TXD_ID = '30202') R,
        ci.PLT1 T1,
        ci.PLT1 T2,
        ci.PLT1 R1 ,
        ci.PLT1 R2   
  where V.AI2_KIND_ID = PL2_KIND_ID(+)
    and V.AI2_KIND_ID = M.AI2_KIND_ID(+)
    and trim(V.AI2_KIND_ID) = trim(RPT_VALUE(+))
    --自然人
    and AI2_PROD_TYPE = T1.PLT1_PROD_TYPE
    and AI2_PROD_SUBTYPE = T1.PLT1_PROD_SUBTYPE
    and greatest(C_AVG_QNTY,C_AVG_OI) * :ai_nature >= T1.PLT1_QNTY_MIN
    and greatest(C_AVG_QNTY,C_AVG_OI) * :ai_nature <= T1.PLT1_QNTY_MAX
    --法人
    and AI2_PROD_TYPE = T2.PLT1_PROD_TYPE
    and AI2_PROD_SUBTYPE = T2.PLT1_PROD_SUBTYPE
    and greatest(C_AVG_QNTY,C_AVG_OI) * :ai_legal >= T2.PLT1_QNTY_MIN
    and greatest(C_AVG_QNTY,C_AVG_OI) * :ai_legal <= T2.PLT1_QNTY_MAX
    --最大值自然人
    and AI2_PROD_TYPE = R1.PLT1_PROD_TYPE
    and AI2_PROD_SUBTYPE = R1.PLT1_PROD_SUBTYPE
    and MAX_VALUE * :ai_nature >= R1.PLT1_QNTY_MIN
    and MAX_VALUE * :ai_nature <= R1.PLT1_QNTY_MAX
    --最大值法人
    and AI2_PROD_TYPE = R2.PLT1_PROD_TYPE
    and AI2_PROD_SUBTYPE = R2.PLT1_PROD_SUBTYPE
    and MAX_VALUE * :ai_legal >= R2.PLT1_QNTY_MIN
    and MAX_VALUE * :ai_legal <= R2.PLT1_QNTY_MAX
    
    and rpt_seq_no > 0
  order by rpt_seq_no
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 既有計算資料
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30202_pl1(string as_ymd) {
            object[] parms = {
                ":as_ymd", as_ymd
            };

            string sql =
@"
SELECT 
       PL1_YMD,   
       PL1_PROD_TYPE,   
       PL1_PROD_SUBTYPE,   
       PL1_KIND_ID,   
       PL1_PREV_AVG_QNTY,   
       PL1_PREV_AVG_OI,   
       PL1_AVG_QNTY,   
       PL1_AVG_OI,   
       PL1_CHANGE_RANGE,   
       PL1_CUR_NATURE,   
       PL1_CUR_LEGAL,   
       PL1_CUR_999,   
       PL1_CP_NATURE,   
       PL1_CP_LEGAL,   
       PL1_CP_999,   
       PL1_MAX_MONTH_CNT,   
       PL1_MAX_TYPE,   
       PL1_MAX_QNTY,   
       PL1_NATURE,   
       PL1_LEGAL,    
       PL1_999,   
       PL1_NATURE_ADJ,   
       PL1_LEGAL_ADJ,   
       PL1_999_ADJ,   
       PL1_UPD_TIME,   
       PL1_UPD_USER_ID  
 FROM CI.PL1   
where PL1_YMD = :as_ymd";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 查出表尾應該在哪一列插入
        /// </summary>
        /// <returns></returns>
        public int row_index() {


            string sql =
@"
SELECT MAX(RPT_SEQ_NO) + 1 as row_index 
 FROM CI.RPT
WHERE RPT_TXN_ID = '30202'
  and RPT_TXD_ID = '30202'
";
            DataTable dtResult = db.GetDataTable(sql, null);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["ROW_INDEX"].AsInt();
            }
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
where PL0_TYPE = 'I' and PL0_YMD = :ls_ymd
";

            #endregion sql
            try {
                int executeResult = db.ExecuteSQLForTransaction(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
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
        public bool InsertPL0(string ls_ymd, string ls_prev_sym, string ls_prev_eym, string ls_cur_sym, string ls_cur_eym, string gs_user_id) {
            object[] parms =
            {
                ":ls_ymd", ls_ymd,
                ":ls_prev_sym", ls_prev_sym,
                ":ls_prev_eym", ls_prev_eym,
                ":ls_cur_sym", ls_cur_sym,
                ":ls_cur_eym", ls_cur_eym,
                ":gs_user_id", gs_user_id
            };

            #region sql

            string sql =
@"
insert into ci.PL0 
values('I',
       :ls_ymd,
       :ls_prev_sym,
       :ls_prev_eym,
       :ls_cur_sym,

       :ls_cur_eym,
       null,
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
        /// 刪除PL2的資料
        /// </summary>
        /// <param name="ls_ymd"></param>
        /// <returns></returns>
        public bool DeletePL2ByDate(string ls_ymd) {
            object[] parms =
            {
                ":ls_ymd", ls_ymd
            };

            #region sql

            string sql =
@"
delete ci.PL2 
where PL2_YMD = :ls_ymd
";

            #endregion sql
            try {
                int executeResult = db.ExecuteSQLForTransaction(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除PL1的資料
        /// </summary>
        /// <param name="ls_ymd"></param>
        /// <returns></returns>
        public bool DeletePL1ByDate(string ls_ymd) {
            object[] parms =
            {
                ":ls_ymd", ls_ymd
            };

            #region sql

            string sql =
@"
delete ci.PL1 
where PL1_YMD = :ls_ymd
";

            #endregion sql
            try {
                int executeResult = db.ExecuteSQLForTransaction(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// update PL1
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public ResultData updatePL1(DataTable inputData) {
            string sql = @"
SELECT 
    PL1_YMD,          
    PL1_PROD_TYPE,    
    PL1_PROD_SUBTYPE, 
    PL1_KIND_ID,      
    PL1_PREV_AVG_QNTY,

    PL1_PREV_AVG_OI,  
    PL1_AVG_QNTY,     
    PL1_AVG_OI,       
    PL1_CHANGE_RANGE, 
    PL1_CUR_NATURE,  

    PL1_CUR_LEGAL,    
    PL1_CUR_999,      
    PL1_CP_NATURE,    
    PL1_CP_LEGAL,     
    PL1_CP_999,  

    PL1_MAX_MONTH_CNT,
    PL1_MAX_TYPE,     
    PL1_MAX_QNTY,    
    PL1_NATURE,       
    PL1_LEGAL, 

    PL1_999,          
    PL1_NATURE_ADJ,   
    PL1_LEGAL_ADJ,    
    PL1_999_ADJ,      
    PL1_UPD_TIME,    

    PL1_UPD_USER_ID  
FROM CI.PL1
";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
