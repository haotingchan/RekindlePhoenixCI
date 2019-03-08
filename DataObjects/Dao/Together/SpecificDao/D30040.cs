using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/6
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    public class D30040: DataGate {

        public DataTable d_30040_1(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
SELECT NVL(AI2_YMD,STW_YMD) AS AI2_YMD,    
         AI2_M_QNTY,
         AI2_OI,
         A_QNTY,A_OI,N_QNTY,N_OI
    FROM 
         --臺指期貨
        (SELECT AI2_YMD,    
                sum(AI2_M_QNTY) as AI2_M_QNTY,
                sum(AI2_OI) as AI2_OI
           FROM ci.AI2,
             (select COD_ID AS PARAM_KEY from CI.COD where COD_TXN_ID = '30040' AND COD_COL_ID = 'AI2_PARAM_KEY')
          WHERE AI2_SUM_TYPE = 'D'  
            and AI2_YMD >= :as_symd      
            and AI2_YMD <= :as_eymd  
            and AI2_PROD_TYPE = 'F'   
            and AI2_PROD_SUBTYPE = 'I' 
            and AI2_SUM_SUBTYPE = '3'
            and AI2_PARAM_KEY = PARAM_KEY 
            --and AI2_M_QNTY > 0
           group by AI2_YMD) AI
         FULL OUTER JOIN 
         --摩根臺指
        (SELECT A.STW_YMD,
                A.STW_VOLUMN AS A_QNTY,
                A.STW_OINT AS A_OI, 
                nvl(N.STW_VOLUMN,0) AS N_QNTY,
                nvl(N.STW_OINT,0) AS N_OI
           FROM
              --電子盤 
             (select STW_YMD,SUM(STW_VOLUMN) AS STW_VOLUMN,SUM(STW_OINT) AS STW_OINT
                from ci.STW
               WHERE STW_YMD >= :as_symd
                 and STW_YMD <= :as_eymd
                 AND STW_RECTYP = 'A'
               GROUP BY STW_YMD,STW_RECTYP) A,
              --人工盤
             (select STW_YMD,SUM(STW_VOLUMN) AS STW_VOLUMN,SUM(STW_OINT) AS STW_OINT
                from ci.STW
               WHERE STW_YMD >= :as_symd
                 and STW_YMD <= :as_eymd
                 AND STW_RECTYP = 'E'
               GROUP BY STW_YMD,STW_RECTYP) N
         WHERE A.STW_YMD = N.STW_YMD (+))
         ON AI2_YMD = STW_YMD
ORDER BY AI2_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_30040_2(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
SELECT AI2_YMD,    
         case when AI2_DAY_COUNT > 0 then round(AI2_M_QNTY / AI2_DAY_COUNT,0) else 0 end as  AVG_QNTY,
         case when AI2_DAY_COUNT > 0 then round(OI.AI2_OI/ AI2_DAY_COUNT,0) else 0 end as  AVG_OI,
         case when A_DAY_COUNT > 0 then round(N_QNTY/ A_DAY_COUNT,0) else 0 end as  AVG_N_QNTY,
         case when A_DAY_COUNT > 0 then round(A_QNTY/ A_DAY_COUNT,0) else 0 end as  AVG_A_QNTY,
         case when A_DAY_COUNT > 0 then round(STW_OI/ A_DAY_COUNT,0) else 0 end as  AVG_STW_OI,
         case when J_DAY_COUNT > 0 then round(J_QNTY / J_DAY_COUNT,0) else 0 end as  AVG_J_QNTY,
         case when J_DAY_COUNT > 0 then round(J_OI/ J_DAY_COUNT,0) else 0 end as  AVG_J_OI,
         AI2_DAY_COUNT,
         AI2_M_QNTY,
         OI.AI2_OI,
         A_QNTY,STW_OI,N_QNTY,A_DAY_COUNT,
         J_QNTY,J_OI,J_DAY_COUNT
    FROM ci.AI2,
         --OI加總
        (SELECT SUBSTR(AI2_YMD,1,6)||'  ' as OI_YM,   
                SUM(AI2_OI) AS AI2_OI
           FROM ci.AI2 ,
                 (select COD_ID AS PARAM_KEY from CI.COD where COD_TXN_ID = '30040' AND COD_COL_ID = 'AI2_PARAM_KEY') 
          WHERE AI2_SUM_TYPE = 'D'  AND  
                AI2_YMD >= :as_symd||'01'  AND    
                AI2_YMD <= :as_eymd||'31'  AND
                AI2_PROD_TYPE = 'F'   AND
                AI2_PROD_SUBTYPE = 'I'   AND
                AI2_SUM_SUBTYPE = '3' AND
                AI2_PARAM_KEY = PARAM_KEY 
                --AI2_M_QNTY > 0
          GROUP BY SUBSTR(AI2_YMD,1,6)) OI,
         --摩根臺指
        (SELECT A.STW_YM||'  ' as STW_YM,
                A.STW_VOLUMN AS A_QNTY,
                A.STW_OINT + nvl(N.STW_OINT,0) AS STW_OI, 
                nvl(N.STW_VOLUMN,0) AS N_QNTY,
                GREATEST(A.STW_DAY_COUNT,nvl(N.STW_DAY_COUNT,0)) AS A_DAY_COUNT
           FROM
              --電子盤
             (select SUBSTR(STW_YMD,1,6) AS STW_YM,SUM(STW_VOLUMN) AS STW_VOLUMN,SUM(STW_OINT) AS STW_OINT,count(DISTINCT STW_YMD) as STW_DAY_COUNT
                from ci.STW
               WHERE STW_YMD >= :as_symd||'01'
                 and STW_YMD <= :as_eymd||'31'
                 AND STW_RECTYP = 'A'
               GROUP BY SUBSTR(STW_YMD,1,6),STW_RECTYP) A,
              --人工盤 
             (select SUBSTR(STW_YMD,1,6) AS STW_YM,SUM(STW_VOLUMN) AS STW_VOLUMN,SUM(STW_OINT) AS STW_OINT,count(DISTINCT STW_YMD) as STW_DAY_COUNT
                from ci.STW
               WHERE STW_YMD >= :as_symd||'01'
                 and STW_YMD <= :as_eymd||'31'
                 AND STW_RECTYP = 'E'
               GROUP BY SUBSTR(STW_YMD,1,6),STW_RECTYP) N
         WHERE A.STW_YM = N.STW_YM (+)),
         --東證臺指
        (SELECT AI2_YMD as J_YM,
                AI2_M_QNTY as J_QNTY,
                AI2_OI as J_OI,
                AI2_DAY_COUNT as J_DAY_COUNT
           from ci.AI2
          where AI2_SUM_TYPE = 'M'   
            and AI2_SUM_SUBTYPE = '3'
            and AI2_YMD >= :as_symd    
            and AI2_YMD <= :as_eymd
            and AI2_PROD_TYPE = 'J'   
            and AI2_PARAM_KEY = 'JTW') J         
   WHERE AI2_SUM_TYPE = 'M'  
     and AI2_YMD >= :as_symd      
     and AI2_YMD <= :as_eymd  
     and AI2_PROD_TYPE = 'F'   
     and AI2_PROD_SUBTYPE = 'I' 
     and AI2_SUM_SUBTYPE = '2' 
     and AI2_M_QNTY > 0
     and AI2_YMD = OI_YM(+)
     and AI2_YMD = STW_YM(+)
     and AI2_YMD = J_YM(+)
ORDER BY AI2_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_30040_3(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
select AI2_YMD,
        TX_QNTY,TX_OI,
        J_QNTY,J_OI
   from
        (select AI2_YMD,
                sum(case when AI2_PARAM_KEY = 'TXF' then AI2_M_QNTY else 0 end) + round(sum(case when AI2_PARAM_KEY = 'MXF' then AI2_M_QNTY else 0 end) /4 ,0) as TX_QNTY,
                sum(case when AI2_PARAM_KEY = 'TXF' then AI2_OI else 0 end) + round(sum(case when AI2_PARAM_KEY = 'MXF' then AI2_OI else 0 end) /4 ,0) as TX_OI 
           from ci.AI2      
          WHERE AI2_SUM_TYPE = 'D' 
            and AI2_SUM_SUBTYPE = '3'  
            and AI2_YMD >= :as_symd      
            and AI2_YMD <= :as_eymd  
            and AI2_PROD_TYPE = 'F'   
            and AI2_PARAM_KEY in ('TXF','MXF')
            and AI2_M_QNTY > 0
          group by AI2_YMD),    
         --東證臺指
        (SELECT AI2_YMD as J_YMD,
                AI2_M_QNTY as J_QNTY,
                AI2_OI as J_OI
           from ci.AI2
          where AI2_SUM_TYPE = 'D'  
            and AI2_SUM_SUBTYPE = '3' 
            and AI2_YMD >= :as_symd      
            and AI2_YMD <= :as_eymd  
            and AI2_PROD_TYPE = 'J'   
            and AI2_PARAM_KEY = 'JTW') J   
  where AI2_YMD = J_YMD(+)
  order by AI2_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_30040_4(string as_symd, string as_eymd) {

            object[] parms = {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

            string sql =
@"
select AI2_YMD,
         case when AI2_DAY_COUNT > 0 then round(TX_QNTY / AI2_DAY_COUNT,0) else 0 end as  AVG_QNTY,
         case when AI2_DAY_COUNT > 0 then round(TX_OI/ AI2_DAY_COUNT,0) else 0 end as  AVG_OI,
         case when J_DAY_COUNT > 0 then round(J_QNTY / J_DAY_COUNT,0) else 0 end as  AVG_J_QNTY,
         case when J_DAY_COUNT > 0 then round(J_OI/ J_DAY_COUNT,0) else 0 end as  AVG_J_OI,
         AI2_DAY_COUNT,
         (TX_QNTY) as TX_QNTY,
         (TX_OI) as TX_OI,
         J_DAY_COUNT,
         nvl((J_QNTY),0) as J_QNTY,
         nvl((J_OI),0) as J_OI
    from     
         --OI加總
        (SELECT SUBSTR(AI2_YMD,1,6)||'  ' as AI2_YMD,   
                sum(case when AI2_PARAM_KEY = 'TXF' then AI2_M_QNTY else 0 end) + round(sum(case when AI2_PARAM_KEY = 'MXF' then AI2_M_QNTY else 0 end) /4 ,0) as TX_QNTY,
                sum(case when AI2_PARAM_KEY = 'TXF' then AI2_OI else 0 end) + round(sum(case when AI2_PARAM_KEY = 'MXF' then AI2_OI else 0 end) /4 ,0) as TX_OI,
                count(DISTINCT AI2_YMD) as AI2_DAY_COUNT
           FROM ci.AI2  
          WHERE AI2_SUM_TYPE = 'D'  AND  
                AI2_SUM_SUBTYPE = '3' and
                AI2_YMD >= :as_symd||'01'  AND    
                AI2_YMD <= :as_eymd||'31'  AND
                AI2_PROD_TYPE = 'F'   AND
                AI2_PROD_SUBTYPE = 'I'   AND
                AI2_PARAM_KEY in ('TXF','MXF') and
                AI2_M_QNTY > 0
          GROUP BY SUBSTR(AI2_YMD,1,6)) OI,
         --東證臺指
        (SELECT AI2_YMD as J_YMD,
                AI2_M_QNTY as J_QNTY,
                AI2_OI as J_OI,
                AI2_DAY_COUNT as J_DAY_COUNT
           from ci.AI2
          where AI2_SUM_TYPE = 'M'   
            and AI2_SUM_SUBTYPE = '3'
            and AI2_YMD >= :as_symd    
            and AI2_YMD <= :as_eymd
            and AI2_PROD_TYPE = 'J'   
            and AI2_PARAM_KEY = 'JTW') J           
   WHERE AI2_YMD = J_YMD(+)
   ORDER BY AI2_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
