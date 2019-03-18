using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D35020 {
        private Db db;

        public D35020() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GenAddReport(string yearq, DateTime symd, DateTime eymd) {

            object[] parms = {
                ":as_yq", yearq,
                ":as_sdate", symd,
                ":as_edate", eymd
            };

            string sql = @"select to_number(TSE1_SID) as TSE1_SID,TSE1_SNAME 
  from ci.TSE1 
 WHERE TSE1_YQ = :as_yq
   and TSE1_SID NOT IN 
(select PDK_STOCK_ID
  from ci.HPDK
 where PDK_DATE >= :as_sdate
   and PDK_DATE <= :as_edate
   and PDK_PARAM_KEY in ('STF','STC')
 group by PDK_STOCK_ID) 
ORDER BY TSE1_SID";

            return db.GetDataTable(sql, parms);
        }

        public DataTable GenSubReport(string yearq, DateTime symd, DateTime eymd) {

            object[] parms = {
                ":as_yq", yearq,
                ":as_sdate", symd,
                ":as_edate", eymd
            };

            string sql = @"select to_number(PDK_STOCK_ID) as PDK_STOCK_ID,
      case when substr(trim(MIN(PDK_NAME)),-3,1) = '選' and substr(trim(MIN(PDK_NAME)),-2,1) = '擇' and substr(trim(MIN(PDK_NAME)),-1,1) = '權' then 
             substr(trim(MIN(PDK_NAME)),1,length(trim(MIN(PDK_NAME))) - 3) else
             substr(trim(MIN(PDK_NAME)),1,length(trim(MIN(PDK_NAME))) - 2) end as PDK_NAME,
        substr(PDK_KIND_ID,1,2) as PDK_KIND_ID,
        case when max(PDK_PROD_TYPE) != 'O' then max(case when PDK_PROD_TYPE = 'F' then 'F' else '' end)  else 
        max(case when PDK_PROD_TYPE = 'F' then 'F' else '' end) ||  '/' ||max(case when PDK_PROD_TYPE = 'O' then 'O' else '' end) end as FUT_OPT
  from ci.HPDK
 where PDK_DATE >= :as_sdate
   and PDK_DATE <= :as_edate
   and PDK_PARAM_KEY in ('STF','STC')
   and PDK_STOCK_ID NOT IN (SELECT TSE1_SID FROM ci.TSE1 where TSE1_YQ = :as_yq)
 group by substr(PDK_KIND_ID,1,2),PDK_STOCK_ID";

            return db.GetDataTable(sql, parms);
        }

    }
}
