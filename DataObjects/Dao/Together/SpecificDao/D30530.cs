using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D30530 {
        private Db db;

        public D30530() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListYearData(string sy, string ey, string sym, string eym, string idfgType, string B_index, string S_index) {

            object[] parms = {
                ":as_syear", sy,
                ":as_eyear", ey,
                ":as_sym",sym,
                ":as_eym",eym,
                ":as_idfg_type",idfgType
            };

            string sql = string.Format(@"SELECT AM2_IDFG_TYPE,   
         case when AM2_BS_CODE='B' then {0} else {1} end as BS_Index,   
         AM2_M_QNTY,   
         AM2_YMD,   
         AM2_SUM_TYPE 
    FROM ci.AM2  
   WHERE AM2_SUM_TYPE = 'Y'  AND  
         TRIM(AM2_YMD) >= :as_syear  AND  
         TRIM(AM2_YMD) < :as_eyear  AND  
         AM2_SUM_SUBTYPE = '1'  AND
         AM2_PROD_TYPE = 'F' AND
         AM2_IDFG_TYPE =:as_idfg_type
union 
  SELECT AM2_IDFG_TYPE,   
          case when AM2_BS_CODE='B' then {0} else {1}  end as BS_Index,   
         sum(AM2_M_QNTY) as AM2_M_QNTY ,   
         substr(AM2_YMD,1,4) as AM2_YMD,   
         AM2_SUM_TYPE  
    FROM ci.AM2  
   WHERE AM2_SUM_TYPE = 'M'  AND  
         TRIM(AM2_YMD) >= :as_sym  AND  
         TRIM(AM2_YMD) <= :as_eym  AND  
         AM2_SUM_SUBTYPE = '1'  AND
         AM2_PROD_TYPE = 'F' AND
         AM2_IDFG_TYPE =:as_idfg_type
 GROUP BY  AM2_IDFG_TYPE,   
         AM2_BS_CODE,   
         AM2_SUM_TYPE,   
         substr(AM2_YMD,1,4)
ORDER BY  AM2_YMD, AM2_IDFG_TYPE,BS_INDEX", B_index, S_index);

            return db.GetDataTable(sql, parms);
        }

        public DataTable ListMonthData(string sym, string eym, string idfgType, string B_index, string S_index) {
            object[] parms = {
                ":as_sym",sym,
                ":as_eym",eym,
                ":as_idfg_type",idfgType
            };

            string sql = string.Format(@"SELECT AM2_IDFG_TYPE,   
          case when AM2_BS_CODE='B' then {0} else {1}  end as BS_Index,   
         sum(AM2_M_QNTY) as AM2_M_QNTY , 
         AM2_YMD,   
         AM2_SUM_TYPE 
    FROM ci.AM2  
   WHERE AM2_SUM_TYPE = 'M'  AND  
         TRIM(AM2_YMD) >= :as_sym  AND  
         TRIM(AM2_YMD) <= :as_eym  AND 
         AM2_SUM_SUBTYPE = '1'  AND
         AM2_PROD_TYPE = 'F' AND
         AM2_IDFG_TYPE =:as_idfg_type
GROUP BY AM2_IDFG_TYPE,   
         AM2_BS_CODE, 
         AM2_YMD,   
         AM2_SUM_TYPE", B_index, S_index);

            return db.GetDataTable(sql, parms);
        }
    }
}
