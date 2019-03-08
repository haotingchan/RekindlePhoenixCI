using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/4
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    public class D30030 : DataGate {

        public DataTable d_30031(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
SELECT AI2_YMD,   
         AI2_PARAM_KEY,   
         AI2_DAY_COUNT,   
         AI2_M_QNTY as AI2_M_QNTY,
         AI2_OI as AI2_OI ,  
         RPT_SEQ_NO
    FROM ci.AI2  ,
         (select RPT_VALUE,nvl(min(RPT.RPT_SEQ_NO),0) AS RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30031'  
           group by RPT_VALUE) 
   WHERE AI2_SUM_TYPE = 'D'  AND  
         AI2_YMD >= :as_symd  AND  
         AI2_YMD <= :as_eymd    AND
         AI2_SUM_SUBTYPE = '3' and
         AI2_PROD_TYPE IN ('F','O')  AND
         AI2_PARAM_KEY = RPT_VALUE(+)
         --and AI2_PROD_SUBTYPE <> 'S' 
   ORDER BY AI2_YMD, RPT_SEQ_NO, AI2_PARAM_KEY      
/*
--20150720
union all 
  SELECT AI2_YMD,   
         case when AI2_PROD_TYPE = 'F' then 'STF' else 'STC' end as AI2_PARAM_KEY,   
         AI2_DAY_COUNT,   
         AI2_M_QNTY as AI2_M_QNTY,
         AI2_OI as AI2_OI ,  
         (select nvl(min(RPT.RPT_SEQ_NO),0)
            from ci.RPT
           where RPT.RPT_TXD_ID = '30031'  
             and RPT.RPT_VALUE = case when AI2_PROD_TYPE = 'F' then 'STF' else 'STC' end) as RPT_SEQ_NO
    FROM ci.AI2  
   WHERE AI2_SUM_TYPE = 'D'  AND  
         AI2_YMD >= :as_symd  AND  
         AI2_YMD <= :as_eymd    AND
         AI2_SUM_SUBTYPE = '2' and
         AI2_PROD_TYPE IN ('F','O') and
         AI2_PROD_SUBTYPE = 'S'
*/
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_30032(string as_sum_type, string as_symd, string as_eymd) {

            object[] parms = {
                ":as_sum_type", as_sum_type,
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
SELECT A.*
FROM
 (SELECT AI2_YMD,   
         AI2_PROD_SUBTYPE,
         AI2_PARAM_KEY,   
         AI2_DAY_COUNT,   
         AI2_M_QNTY as AI2_M_QNTY,
         RPT_SEQ_NO 
    FROM ci.AI2 A,
         (select RPT_VALUE,nvl(min(RPT.RPT_SEQ_NO),0) AS RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30031'  
           group by RPT_VALUE) 
   WHERE AI2_SUM_TYPE = :as_sum_type  AND  
         AI2_YMD >= :as_symd    AND
         AI2_YMD <= :as_eymd    AND
         AI2_SUM_SUBTYPE = '3' AND
         AI2_PROD_TYPE IN ('F','O')  AND
         AI2_PARAM_KEY = RPT_VALUE(+)
 union all
 SELECT '999999',   
         AI2_PROD_SUBTYPE,
         AI2_PARAM_KEY,   
         sum(AI2_DAY_COUNT) as AI2_DAY_COUNT,   
         sum(AI2_M_QNTY) as AI2_M_QNTY,
         RPT_SEQ_NO 
    FROM ci.AI2 A,
         (select RPT_VALUE,nvl(min(RPT.RPT_SEQ_NO),0) AS RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30031'  
           group by RPT_VALUE) 
   WHERE AI2_SUM_TYPE = 'M'  AND  
         AI2_YMD >= :as_symd    AND
         AI2_YMD <= :as_eymd    AND
         AI2_SUM_SUBTYPE = '3'  AND
         AI2_PROD_TYPE IN ('F','O')  AND
         AI2_PARAM_KEY = RPT_VALUE(+)
   GROUP BY AI2_PROD_SUBTYPE,AI2_PARAM_KEY,RPT_SEQ_NO
 union all
  SELECT AI2_YMD, 
         AI2_PROD_SUBTYPE,
         'SUM_F_I',   
         (AI2_DAY_COUNT) as AI2_DAY_COUNT,   
         (AI2_M_QNTY) as AI2_M_QNTY,
         RPT_SEQ_NO 
    FROM ci.AI2 A,
         (select nvl(min(RPT.RPT_SEQ_NO),0) AS RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30031'   
             and RPT_VALUE = 'SUM_F_I'
           group by RPT_VALUE) 
   WHERE AI2_SUM_TYPE = 'M'  AND  
         AI2_YMD >= :as_symd    AND
         AI2_YMD <= :as_eymd    AND
         AI2_SUM_SUBTYPE = '2'AND
         AI2_PROD_TYPE = 'F' AND
         AI2_PROD_SUBTYPE = 'I' 
 union
 SELECT '999999',   
         AI2_PROD_SUBTYPE,
         'SUM_F_I',   
         sum(AI2_DAY_COUNT) as AI2_DAY_COUNT,   
         sum(AI2_M_QNTY) as AI2_M_QNTY,
        RPT_SEQ_NO 
    FROM ci.AI2 A,
         (select nvl(min(RPT.RPT_SEQ_NO),0) AS RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30031'   
             and RPT_VALUE = 'SUM_F_I'
           group by RPT_VALUE) 
   WHERE AI2_SUM_TYPE = 'M'  AND  
         AI2_YMD >= :as_symd    AND
         AI2_YMD <= :as_eymd    AND
         AI2_SUM_SUBTYPE = '2'AND
         AI2_PROD_TYPE = 'F' AND
         AI2_PROD_SUBTYPE = 'I' 
   GROUP BY AI2_PROD_SUBTYPE, AI2_PARAM_KEY, RPT_SEQ_NO) A
ORDER BY AI2_YMD, RPT_SEQ_NO, AI2_PROD_SUBTYPE, AI2_PARAM_KEY
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_30032_OI(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
select OI.AI2_YMD,OI.AI2_OI,D.AI2_DAY_COUNT,  
         (select nvl(min(RPT.RPT_SEQ_NO),0)
            from ci.RPT
           where RPT.RPT_TXD_ID = '30031'
             and RPT_VALUE = 'OI') as RPT_SEQ_NO 
  from
     (select substr(O.AI2_YMD,1,6)||'  ' as AI2_YMD,
             sum(O.AI2_OI) as AI2_OI
        from ci.AI2 O
       where O.AI2_SUM_TYPE = 'D'
         and O.AI2_SUM_SUBTYPE = '1'
         and substr(O.AI2_YMD,1,6) >= :as_symd
         and substr(O.AI2_YMD,1,6) <= :as_eymd
         and O.AI2_PROD_TYPE IN ('F','O')
       group by substr(O.AI2_YMD,1,6) ) OI,     
     (select D.AI2_YMD as AI2_YMD,
             max(AI2_DAY_COUNT) as AI2_DAY_COUNT
        from ci.AI2 D
       where D.AI2_SUM_TYPE = 'M'
         and D.AI2_SUM_SUBTYPE = '1'
         and D.AI2_YMD >= :as_symd
         and D.AI2_YMD <= :as_eymd
         and D.AI2_PROD_TYPE IN ('F','O')
       group by D.AI2_YMD) D
 WHERE OI.AI2_YMD = D.AI2_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public int colTol() {

            string sql =
@"
select nvl(rpt_seq_no,0) as colTol
 from ci.RPT
 where RPT_TXN_ID = '30030'
   and RPT_TXD_ID = '30031'
   and RPT_VALUE= 'TOT'
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult.Rows[0]["colTol"].AsInt();
        }

        public DataTable d_30033(string as_sym, string as_eym) {

            object[] parms = {
                ":as_sym", as_sym,
                ":as_eym", as_eym
            };

            string sql =
@"
SELECT AA1_YM,   
         AA1_TAIFEX,   
         (AA1_OTC+AA1_TSE) AS AA1_TSE,   
         AA1_SGX_DT,   
         AA1_DAY_COUNT  
    FROM ci.AA1  
   WHERE AA1_YM >= :as_sym  AND  
         AA1_YM <= :as_eym
   ORDER BY AA1_YM

";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_30034(string as_symd, string as_eymd, string as_sym, string as_eym) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_sym", as_sym,
                ":as_eym", as_eym
            };

            string sql =
@"
SELECT STW.STW_YMD,
       STW.STW_AMT,
       STW.STW_DAYS,
       AA1.AA1_US_RATE,
       STW.STW_AMT*AA1.AA1_US_RATE as cp_c
FROM
(SELECT substr(STWD_YMD,1,6) as STW_YMD,
       sum(((STWD_HIGH_PRICE + STWD_LOW_PRICE)/2)*STWD_QNTY * 100) AS STW_AMT,
       COUNT(DISTINCT STWD_YMD) AS STW_DAYS
FROM ci.STWD
WHERE STWD_YMD >= :as_symd
  AND STWD_YMD <= :as_eymd
  AND STWD_QNTY > 0
group by substr(STWD_YMD,1,6)) STW,
(SELECT substr(AA1_YM,1,6) as AA1_YM,AA1_US_RATE
  FROM ci.AA1
 WHERE AA1_YM >= :as_sym
   AND AA1_YM <= :as_eym) AA1
WHERE STW.STW_YMD = AA1.AA1_YM
ORDER BY STW_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }
    }
}
