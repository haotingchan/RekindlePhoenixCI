using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D30500 {
        private Db db;

        public D30500() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData(string symd, string eymd) {

            object[] parms = {
                ":adt_sdate", symd,
                ":adt_edate", eymd
            };

            string sql = @"select D.SPRD1_KIND_ID as PROD_ID,APDK_NAME,
       D.weight_diff as WEIGHT_DIFF,
       D.simple_diff as SIMPLE_DIFF,
       D.SPRD1_DIFF_MAX as MAX_DIFF,SPRD1_MAX_KEEP_TIME as MAX_DIFF_TIME,
       D.SPRD1_DIFF_MIN as MIN_DIFF,SPRD1_MIN_KEEP_TIME as MIN_DIFF_TIME,
       no_time as NO_TWO_SIDE_TIME
 from  ci.APDK,
 (select SPRD1_KIND_ID,
         CASE WHEN sum(SPRD1_TOT_KEEP_TIME) = 0 THEN 0 ELSE round(sum(SPRD1_TOT_WEIGHT)/sum(SPRD1_TOT_KEEP_TIME),2) end as weight_diff,
         case when sum(SPRD1_TOT_CNT) = 0 then 0 else round(sum(SPRD1_TOT_SIMPLE)/sum(SPRD1_TOT_CNT),2) end as simple_diff,
         max(SPRD1_DIFF_MAX) as SPRD1_DIFF_MAX,
         case when min(case when SPRD1_DIFF_MIN > 0 then SPRD1_DIFF_MIN else 999999 end) = 999999 then 0
              else min(case when SPRD1_DIFF_MIN > 0 then SPRD1_DIFF_MIN else 999999 end) end as SPRD1_DIFF_MIN,
         sum(SPRD1_NO_TIME) as no_time
   from ci.SPRD1   
  where SPRD1_YMD >= :adt_sdate
   and SPRD1_YMD <= :adt_edate
  group by SPRD1_KIND_ID) D,
 (select SPRD1_KIND_ID,SPRD1_DIFF_MAX,sum(SPRD1_MAX_KEEP_TIME) as SPRD1_MAX_KEEP_TIME
   from ci.SPRD1
   where SPRD1_YMD >= :adt_sdate
     and SPRD1_YMD <= :adt_edate
     group by SPRD1_KIND_ID,SPRD1_DIFF_MAX) MA,     
 (select SPRD1_KIND_ID,SPRD1_DIFF_MIN,sum(SPRD1_MIN_KEEP_TIME) as SPRD1_MIN_KEEP_TIME
    from ci.SPRD1
   where SPRD1_YMD >= :adt_sdate
     and SPRD1_YMD <= :adt_edate
     group by SPRD1_KIND_ID,SPRD1_DIFF_MIN) MI
 where D.SPRD1_KIND_ID = APDK_KIND_ID
   and D.SPRD1_KIND_ID = MA.SPRD1_KIND_ID
   and D.SPRD1_DIFF_MAX = MA.SPRD1_DIFF_MAX
   and D.SPRD1_KIND_ID = MI.SPRD1_KIND_ID
   and D.SPRD1_DIFF_MIN = MI.SPRD1_DIFF_MIN";

            return db.GetDataTable(sql, parms);
        }
    }
}
