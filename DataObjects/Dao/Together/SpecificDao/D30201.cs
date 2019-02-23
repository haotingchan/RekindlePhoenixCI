using OnePiece;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   //Winni, 2018/12/26
   public class D30201 {
      private Db db;

      public D30201() {
         db = GlobalDaoSetting.DB;
      }

      public DataTable ListData(string as_symd , string as_eymd) {
         object[] parms = {
               "@as_symd",as_symd,
               "@as_eymd",as_eymd
            };

         string sql = @"SELECT a.*
FROM (select DT_SYMD,DT_EYMD,month_seq_no,
       AI2_KIND_ID,
       case when DAY_CNT= 0 then 0 else round(M_QNTY/DAY_CNT,0) end as AVG_QNTY,
       case when DAY_CNT= 0 then 0 else round(OI/DAY_CNT,0) end as AVG_OI,
       RPT_SEQ_NO
  from 
      (select DT_SYMD,DT_EYMD,
             ROW_NUMBER( ) OVER (PARTITION BY DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY) ORDER BY DT_SYMD desc NULLS LAST) as month_seq_no,
             DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY) as AI2_KIND_ID,
             sum(case when AI2_PARAM_KEY='MXF' then round(AI2_M_QNTY/4,0) else AI2_M_QNTY end) as M_QNTY,
             sum(case when AI2_PARAM_KEY='MXF' then round(AI2_OI/4,0) else AI2_OI end) as OI,
             sum(case when AI2_PARAM_KEY='MXF' then 0 else AI2_DAY_COUNT end) as DAY_CNT  
        from ci.AI2,
            (select AI2_YMD as DT_SYMD,
                    :as_eymd as DT_EYMD
               from CI.AI2
              where AI2_YMD >= :as_symd
                and AI2_YMD <= :as_eymd
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
       group by DT_SYMD,DT_EYMD,DECODE(AI2_PARAM_KEY,'MXF    ','TXF    ',AI2_PARAM_KEY)),
     (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30201' and RPT_TXD_ID = '30201') 
 where trim(AI2_KIND_ID) = trim(RPT_VALUE(+)) order by rpt_seq_no , month_seq_no) a
WHERE a.rpt_seq_no <> 0 ";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}