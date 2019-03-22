using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D30770 {
        private Db db;

        public D30770() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetProd(string sumType, string symd, string eymd, string prodType) {

            object[] parms = {
                ":as_sum_type", sumType,
                ":as_symd", symd,
                ":as_eymd", eymd,
                ":as_prod_type",prodType
            };

            string sql = @"        
 SELECT AM11_KIND_ID2
                  FROM ci.AM11
                 WHERE AM11_SUM_TYPE = :as_sum_type
                   AND AM11_YMD >= :as_symd
                   AND AM11_YMD <= :as_eymd 
                   AND AM11_PROD_TYPE LIKE :as_prod_type
                 GROUP BY AM11_PROD_TYPE,AM11_KIND_ID2
                 order by AM11_PROD_TYPE,AM11_KIND_ID2";

            return db.GetDataTable(sql, parms);

        }

        public DataTable ListNightTransactionData(string sumType, string symd, string eymd, string prodType,string strProd) {

            object[] parms = {
                ":as_sum_type", sumType,
                ":as_symd", symd,
                ":as_eymd", eymd,
                ":as_prod_type",prodType
            };

            string sql = string.Format(@"SELECT * from (
        --1345-1615交易量  
        SELECT AM11_YMD,AM11_KIND_ID2 as AM11_KIND_ID ,sum(AM11_M_QNTY) / 2 as M_QNTY,
         sum(sum(AM11_M_QNTY) / 2) over(partition by AM11_YMD) as sumbydate
           FROM ci.AM11
          WHERE TRIM(AM11_SUM_TYPE) = :as_sum_type
            AND AM11_OSW_GRP like '%'
            AND AM11_OSW_GRP <> '1'
            AND TRIM(AM11_YMD) >= :as_symd
            AND TRIM(AM11_YMD) <= :as_eymd
            AND AM11_PROD_TYPE LIKE :as_prod_type
          GROUP BY AM11_YMD,AM11_PROD_TYPE,AM11_KIND_ID2
     )  S
     pivot 
     (
        sum (S.M_QNTY)
        for AM11_KIND_ID in ({0})
     )
     order by Am11_YMD", strProd);

            return db.GetDataTable(sql, parms);
        }
    }
}
