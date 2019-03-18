using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D35030 {
        private Db db;

        public D35030() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetData(string yq,DateTime symd, DateTime eymd) {

            object[] parms = {
                ":as_yq", yq,
                ":as_sdate", symd,
                ":as_edate", eymd
            };

            string sql = @"select to_number(case when FUT is null and OPT is null then '1' else '0' end) as DATA_TYPE,
       PDK_KIND_ID ,TSE1_SNAME,TSE1_SID,FUT,OPT      
from
(select TSE1_SID,TSE1_SNAME from ci.TSE1 WHERE TSE1_YQ = :as_yq) TSE,
(select substr(PDK_KIND_ID,1,2) as PDK_KIND_ID,PDK_STOCK_ID,
      case when substr(trim(MIN(PDK_NAME)),-3,1) = '選' and substr(trim(MIN(PDK_NAME)),-2,1) = '擇' and substr(trim(MIN(PDK_NAME)),-1,1) = '權' then 
             substr(trim(MIN(PDK_NAME)),1,length(trim(MIN(PDK_NAME))) - 3) else
             substr(trim(MIN(PDK_NAME)),1,length(trim(MIN(PDK_NAME))) - 2) end as PDK_NAME,
        max(case when PDK_PROD_TYPE = 'F' then 'F' else '' end) as FUT,
        max(case when PDK_PROD_TYPE = 'O' then 'O' else '' end) as OPT
  from ci.HPDK
 where PDK_DATE >= :as_sdate
   and PDK_DATE <= :as_edate
   and PDK_PARAM_KEY in ('STF','STC')
 group by substr(PDK_KIND_ID,1,2),PDK_STOCK_ID) TAI
WHERE TSE1_SID = PDK_STOCK_ID(+) 
union
select to_number('2'),substr(PDK_KIND_ID,1,2) as PDK_KIND_ID,
      case when substr(trim(MIN(PDK_NAME)),-3,1) = '選' and substr(trim(MIN(PDK_NAME)),-2,1) = '擇' and substr(trim(MIN(PDK_NAME)),-1,1) = '權' then 
             substr(trim(MIN(PDK_NAME)),1,length(trim(MIN(PDK_NAME))) - 3) else
             substr(trim(MIN(PDK_NAME)),1,length(trim(MIN(PDK_NAME))) - 2) end as PDK_NAME,
        PDK_STOCK_ID,
        max(case when PDK_PROD_TYPE = 'F' then 'F' else '' end) as FUT,
        max(case when PDK_PROD_TYPE = 'O' then 'O' else '' end) as OPT
  from ci.HPDK
 where PDK_DATE >= :as_sdate
   and PDK_DATE <= :as_edate
   and PDK_PARAM_KEY in ('STF','STC')
   and PDK_STOCK_ID NOT IN (SELECT TSE1_SID FROM ci.TSE1 where TSE1_YQ = :as_yq)
 group by substr(PDK_KIND_ID,1,2),PDK_STOCK_ID
 order by tse1_sid";

            return db.GetDataTable(sql, parms);
        }
    }
}
