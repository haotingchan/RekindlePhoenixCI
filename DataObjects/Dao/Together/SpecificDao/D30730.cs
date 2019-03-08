using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D30730 {
        private Db db;

        public D30730() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetAM6Data(string ymd) {

            object[] parms = {
                ":as_ym", ymd
            };

            string sql = @"SELECT AM6_YM,   
         AM6_TRADE_AUX,   
         AM6_F000,   
         AM6_F999,   
         AM6_000,   
         AM6_999  
    FROM ci.AM6  
   WHERE AM6_YM = :as_ym ";

            return db.GetDataTable(sql, parms);
        }

        public DataTable GetAM0Data(string ymd) {

            object[] parms = {
                ":as_ym", ymd
            };

            string sql = @"select case when SUBSTR(BRK_NO,1,1) = 'F' then 'F' else 'S' end as am0_brk_no,
       AM0_BRK_TYPE,
       SUM(AM0_M_QNTY) AS AM0_M_QNTY
  from
(select SUBSTR(AM0_BRK_NO4,1,1)  AS BRK_NO,
       AM0_BRK_TYPE,
       SUM(AM0_M_QNTY) AS AM0_M_QNTY
  FROM ci.AM0
 where AM0_SUM_TYPE = 'M'
   and substr(AM0_YMD,1,6) = :as_ym 
 GROUP BY SUBSTR(AM0_BRK_NO4,1,1),
       AM0_BRK_TYPE)
group by case when SUBSTR(BRK_NO,1,1) = 'F' then 'F' else 'S' end  ,
       AM0_BRK_TYPE";

            return db.GetDataTable(sql, parms);
        }
    }
}
