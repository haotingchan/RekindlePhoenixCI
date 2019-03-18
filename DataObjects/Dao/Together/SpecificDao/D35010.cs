using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D35010 {
        private Db db;

        public D35010() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetHDPKData(DateTime symd, DateTime eymd) {

            object[] parms = {
                ":as_sdate", symd,
                ":as_edate", eymd
            };

            string sql = @"select substr(PDK_KIND_ID,1,2) as PDK_KIND_ID,
      case when substr(trim(MIN(PDK_NAME)),-3,1) = '選' and substr(trim(MIN(PDK_NAME)),-2,1) = '擇' and substr(trim(MIN(PDK_NAME)),-1,1) = '權' then 
             substr(trim(MIN(PDK_NAME)),1,length(trim(MIN(PDK_NAME))) - 3) else
             substr(trim(MIN(PDK_NAME)),1,length(trim(MIN(PDK_NAME))) - 2) end as PDK_NAME,PDK_STOCK_ID,
        max(case when PDK_PROD_TYPE = 'F' then 'F' else '' end) as FUT,
        max(case when PDK_PROD_TYPE = 'O' then 'O' else '' end) as OPT
  from ci.HPDK
 where PDK_DATE >= :as_sdate
   and PDK_DATE <= :as_edate
   and PDK_PARAM_KEY in ('STF','STC')
 group by substr(PDK_KIND_ID,1,2),PDK_STOCK_ID
ORDER BY PDK_KIND_ID";

            return db.GetDataTable(sql, parms);
        }
    }
}
