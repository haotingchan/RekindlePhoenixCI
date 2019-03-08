using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D30740 {
        private Db db;

        public D30740() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetAM1Data(DateTime ymd) {

            object[] parms = {
                ":as_syear", ymd.Year.ToString(),
                ":as_eyear", ymd.Year.ToString(),
                ":as_sym", ymd.Year.ToString() + "01",
                ":as_eym",ymd.ToString("yyyyMM")
            };

            string sql = @"select substr(AM1_YMD,1,4) as AM1_YMD,
       AM1_PROD_TYPE,
       SUM(AM1_M_QNTY) AS AM1_M_QNTY
  FROM ci.AM1
 where AM1_SUM_TYPE = 'M'
   and substr(AM1_YMD,1,4) >= :as_syear 
   and substr(AM1_YMD,1,4) <  :as_eyear
 GROUP BY substr(AM1_YMD,1,4),
       AM1_PROD_TYPE
union 
select AM1_YMD,
       AM1_PROD_TYPE,
       SUM(AM1_M_QNTY) AS AM1_M_QNTY
  FROM ci.AM1
 where AM1_SUM_TYPE = 'M'
   and substr(AM1_YMD,1,6) >= :as_sym 
   and substr(AM1_YMD,1,6) <= :as_eym 
 GROUP BY AM1_YMD,
       AM1_PROD_TYPE";

            return db.GetDataTable(sql, parms);
        }

        public DataTable GetAM4Data(DateTime ymd) {

            object[] parms = {
                ":as_syear", ymd.Year.ToString(),
                ":as_eyear", ymd.Year.ToString(),
                ":as_sym", ymd.Year.ToString() + "01",
                ":as_eym",ymd.ToString("yyyyMM")
            };

            string sql = @"SELECT AM4_SUM_TYPE,
         substr(AM4_YM,1,4) as AM4_YM,   
         nvl(sum(AM4_F_QNTY),0) as AM4_F_QNTY, 
         nvl(sum(AM4_O_QNTY),0) as AM4_O_QNTY, 
         nvl(sum(AM4_TRADE_COUNT),0) as AM4_TRADE_COUNT, 
         '1' as SORT_TYPE 
    FROM ci.AM4 
   WHERE AM4_SUM_TYPE = 'M'  AND  
         TRIM(AM4_YM) >= :as_syear  AND  
         TRIM(AM4_YM) < :as_eyear 
group by AM4_SUM_TYPE,
         substr(AM4_YM,1,4)   
union 
  SELECT AM4_SUM_TYPE,
         AM4_YM,   
         nvl(AM4_F_QNTY,0) as AM4_F_QNTY, 
         nvl(AM4_O_QNTY,0) as AM4_O_QNTY,  
         nvl(AM4_TRADE_COUNT,0) as AM4_TRADE_COUNT, 
         '3' as SORT_TYPE  
    FROM ci.AM4  
   WHERE AM4_SUM_TYPE = 'M'  AND  
         TRIM(AM4_YM) >= :as_sym  AND  
         TRIM(AM4_YM) <= :as_eym ";

            return db.GetDataTable(sql, parms);
        }
    }
}
